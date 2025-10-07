﻿using Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Build;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

namespace ThunderRoad
{
    public class AssetBundleBuilder
    {
        public static string exportFolderName;

        public delegate void BuildEvent(EventTime eventTime);
        public static event BuildEvent OnBuildEvent;

        public static string androidAssetFolderName = "obb";

        public static string assetsLocalPath
        {
            get
            {
                string buildtarget = "Windows";
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
                {
                    buildtarget = "Android";
                }
                return (Path.Combine(ThunderRoadSettings.current.addressableEditorPath, buildtarget, exportFolderName));
            }
        }

        [InitializeOnLoad]
        public class Startup
        {
            static Startup()
            {
                EditorCoroutineUtility.StartCoroutineOwnerless(ClearMissingAAGroupsCoroutine());
            }
        }

        static IEnumerator ClearMissingAAGroupsCoroutine()
        {
            // Cleanup addressable groups
            while (AddressableAssetSettingsDefaultObject.Settings == null || AddressableAssetSettingsDefaultObject.Settings.groups == null)
            {
                yield return null;
            }
            for (int i = AddressableAssetSettingsDefaultObject.Settings.groups.Count - 1; i >= 0; i--)
            {
                if (AddressableAssetSettingsDefaultObject.Settings.groups[i] == null)
                {
                    Debug.Log($"Removing missing AA group at index {i}");
                    AddressableAssetSettingsDefaultObject.Settings.groups.RemoveAt(i);
                }
            }
            foreach (AddressableAssetGroup aaGroup in EditorCommon.GetAllProjectAssets<AddressableAssetGroup>())
            {
                if (AddressableAssetSettingsDefaultObject.Settings.groups.Contains(aaGroup))
                    continue;
                Debug.Log($"Adding missing AA group {aaGroup.name}");
                AddressableAssetSettingsDefaultObject.Settings.groups.Add(aaGroup);
            }
            //save the settings
            AddressableAssetSettingsDefaultObject.Settings.SetDirty(AddressableAssetSettings.ModificationEvent.BatchModification, null, true, true);
            AssetDatabase.SaveAssets();
        }


        public static void CopyDirectory(string strSource, string strDestination, string searchPattern = "*.*")
        {
            if (!Directory.Exists(strDestination))
            {
                Directory.CreateDirectory(strDestination);
            }

            if (!Directory.Exists(strSource))
            {
                Directory.CreateDirectory(strSource);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(strSource);
            FileInfo[] files = dirInfo.GetFiles(searchPattern);
            foreach (FileInfo tempfile in files)
            {
                tempfile.CopyTo(Path.Combine(strDestination, tempfile.Name), true);
            }

            DirectoryInfo[] directories = dirInfo.GetDirectories();
            foreach (DirectoryInfo tempdir in directories)
            {
                CopyDirectory(Path.Combine(strSource, tempdir.Name), Path.Combine(strDestination, tempdir.Name), searchPattern);
            }
        }

 //ProjectCore
        public static bool Build(AssetBundleGroup assetBundleGroup, bool purgeCache)
        {
            DateTime timeStart = DateTime.Now;

            exportFolderName = assetBundleGroup.folderName;

            Debug.Log($"Building asset bundles to destination: {assetsLocalPath}");


            //ensure the refresh import mode is parallel
            AssetDatabase.ActiveRefreshImportMode = AssetDatabase.RefreshImportMode.OutOfProcessPerQueue;

            Stopwatch stopwatch = new Stopwatch();
            
            stopwatch.Start();
            

            // Configure addressable groups
            List<Shader> shadersToStrip = new List<Shader>();
            HashSet<string> reimportPaths = new HashSet<string>();
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings != null)
            {
                //Cleanup addressable groups
                settings.RemoveMissingAddressableReferences();
                foreach (AddressableAssetGroup group in settings.groups)
                {
                    if (group == null) continue;
                    bool isInBundleGroup = assetBundleGroup.addressableAssetGroups.Contains(group);
                    
                    BundledAssetGroupSchema bundledAssetGroupSchema = group.GetSchema<BundledAssetGroupSchema>();
                    if (bundledAssetGroupSchema == null)
                    {
                        //if its in the bundle group
                        if(isInBundleGroup && assetBundleGroup.isMod)
                        {
                            //we can only safely add them to mod groups because if its a base game one thats missing, we might set the wrong build/load paths
                            bundledAssetGroupSchema = group.AddSchema<BundledAssetGroupSchema>();
                            Debug.LogWarning($"Group {group.name} is missing BundledAssetGroupSchema, added one");
                        }
                        else
                        {
                            if(group.name != "Built In Data") Debug.LogError($"Group {group.name} is missing BundledAssetGroupSchema, skipping, bundle will not be built");
                            continue;
                        }
                    }

                    ThunderRoadAAGroupSchema thunderRoadAAGroupSchema = group.GetSchema<ThunderRoadAAGroupSchema>();
                    if (thunderRoadAAGroupSchema == null)
                    {
                        //add one
                        thunderRoadAAGroupSchema = group.AddSchema<ThunderRoadAAGroupSchema>();
                        thunderRoadAAGroupSchema.sharedBundle = false;
                        Debug.LogWarning($"Group {group.name} is missing ThunderRoadAAGroupSchema, added one");
                    }
                    

                    bundledAssetGroupSchema.IncludeInBuild = false;
                    bundledAssetGroupSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.NoHash;
                    bundledAssetGroupSchema.Compression = BundledAssetGroupSchema.BundleCompressionMode.LZ4;
                    bundledAssetGroupSchema.UseAssetBundleCrc = false;
                    bundledAssetGroupSchema.UseAssetBundleCrcForCachedBundles = false;
                    bundledAssetGroupSchema.IncludeAddressInCatalog = true;
                    bundledAssetGroupSchema.IncludeGUIDInCatalog = true;
                    bundledAssetGroupSchema.IncludeLabelsInCatalog = true;
                    
                    //check if the bundle group is a mod, so we can set the correct build and load paths
                    if(assetBundleGroup.isMod && isInBundleGroup)
                    {
                        bundledAssetGroupSchema.BuildPath.SetVariableByName(settings, "DefaultBuildPath");
                        bundledAssetGroupSchema.LoadPath.SetVariableByName(settings, "ModLoadPath");
                    }
                    
                    if (isInBundleGroup || thunderRoadAAGroupSchema.sharedBundle)
                    {
                        // Only take group that is compatible with target platform
                        bool haveAndroidLabel = false;
                        bool haveWindowsLabel = false;
                        foreach (var entry in group.entries)
                        {
                            if (thunderRoadAAGroupSchema.sharedBundle)
                            {
                                Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(entry.AssetPath);
                                if (shader)
                                {
                                    shadersToStrip.Add(shader);
                                }
                            }
                            if (entry.labels.Contains("Android"))
                            {
                                haveAndroidLabel = true;
                            }
                            if (entry.labels.Contains("Windows"))
                            {
                                haveWindowsLabel = true;
                            }
                        }
         
                        if ((Common.GetQualityLevel() == QualityLevel.Android && haveAndroidLabel)
                            || (Common.GetQualityLevel() == QualityLevel.Windows && haveWindowsLabel))
                        {
                            bundledAssetGroupSchema.IncludeInBuild = true;
                            
                            //if were on android, enable using unitywebrequest for local asset bundles. its faster on android
                            bundledAssetGroupSchema.UseUnityWebRequestForLocalBundles = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
                            //dont use a timeout, as we dont want to timeout on local asset bundles
                            bundledAssetGroupSchema.Timeout = 0;
                            // unitybuiltinshaders use same path as default group, so it's important to set this correctly
                            if (assetBundleGroup.isDefault && bundledAssetGroupSchema.LoadPath.GetName(settings) == "DefaultLoadPath" && !thunderRoadAAGroupSchema.sharedBundle)
                            {
                                Debug.Log($"Set default group to {group.name}");
                                settings.DefaultGroup = group;
                            }
                            else if (!assetBundleGroup.isDefault && bundledAssetGroupSchema.LoadPath.GetName(settings) == "ModLoadPath")
                            {
                                Debug.Log($"Set default group to {group.name}");
                                settings.DefaultGroup = group;
                            }
                        }
                    }

                    if (Application.isBatchMode && bundledAssetGroupSchema.IncludeInBuild)
                    {
                        foreach (AddressableAssetEntry groupEntry in group.entries)
                        {
                            // Workaround to fix an issue when room is not correctly imported after a change
                            if (!groupEntry.labels.Contains("Area")) continue;
                            reimportPaths.Add(groupEntry.AssetPath);
                        }
                    }
                }

                settings.profileSettings.SetValue(settings.activeProfileId, "ModLoadPath", $"{{ThunderRoad.FileManager.aaModPath}}/{assetBundleGroup.folderName}");
                settings.OverridePlayerVersion = assetBundleGroup.folderName;

                // Set builtin shader to export folder name to avoid duplicates
                settings.ShaderBundleNaming = ShaderBundleNaming.Custom;
                settings.ShaderBundleCustomNaming = assetBundleGroup.folderName;
                settings.BuildRemoteCatalog = true;

                try
                {
                    AssetDatabase.StartAssetEditing();
                    // Reimport all assets that are in the addressable groups
                    if (reimportPaths.Count > 0)
                    {
                        stopwatch.Reset();
                        stopwatch.Start();
                        foreach (string path in reimportPaths)
                        {
                            AssetDatabase.ImportAsset(path);
                        }
                        stopwatch.Stop();
                        Debug.Log($"Reimported {reimportPaths.Count} vfx and area assets in: {stopwatch.ElapsedMilliseconds}ms");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error during scripted asset import: {e.Message}");
                }
                finally
                {
                    AssetDatabase.StopAssetEditing();
                }
            }

            SetGraphicSettings(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            // Create build path if doesn't exist
            if (!Directory.Exists(assetsLocalPath))
            {
                Directory.CreateDirectory(assetsLocalPath);
            }

            // Clean build path
            foreach (string filePath in Directory.GetFiles(assetsLocalPath, "*.*", SearchOption.AllDirectories))
            {
                File.Delete(filePath);
            }

            OnBuildEvent?.Invoke(EventTime.OnStart);

            if (purgeCache)
            {
                /*
                 * https://docs.unity3d.com/Packages/com.unity.addressables@1.20/manual/ContinuousIntegration.html
                 * The Library/BuildCache folder contains .info files created by SBP during the build
                 * which speeds up subsequent builds by reading data from these .info files instead of re-generating data that hasn't changed.
                 */

                BuildCache.PurgeCache(false);

                /*
                 *
                 * https://forum.unity.com/threads/automate-build-w-custom-addressables-build-script-ie-variation-in-example-repo.1073480/#post-6951992
                 * If you delete the Library folder, then you remove everything.
                 * If you CleanPlayerContent, then you remove any cached build data.
                 *
                 * Since our addressables are split out better now into a bundle per item, creature, apparel etc, bundles which haven't changed get rebuilt from the cache
                 * Ones which have changed only rebuild smaller bundles now, rather than large 
                 */
                AddressableAssetSettings.CleanPlayerContent();
                AddressableAssetSettings.CleanPlayerContent(settings.ActivePlayerDataBuilder);
            }

            AssetBundleShaderStripPreprocess.SetShadersToStripDuringBuild(shadersToStrip);
            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult buildResult);
            //The PostProcessBuild callback for our shader stripper doesnt get called by Addressables BuildPlayerContent. Only in the PlayerBuilder
            
            AssetBundleShaderStripPreprocess.SetShadersToStripDuringBuild(null);

            if (!string.IsNullOrEmpty(buildResult.Error))
            {
                Debug.LogError("BuildPlayerContent failed");
                return false;
            }

            foreach (AddressableAssetGroup addressableAssetGroup in settings.groups)
            {
                if (addressableAssetGroup == null) continue;
                BundledAssetGroupSchema bundledAssetGroupSchema = addressableAssetGroup.GetSchema<BundledAssetGroupSchema>();
                if (bundledAssetGroupSchema == null) continue;
                ThunderRoadAAGroupSchema thunderRoadAAGroupSchema = addressableAssetGroup.GetSchema<ThunderRoadAAGroupSchema>();
                if (thunderRoadAAGroupSchema == null) continue;

                if (!assetBundleGroup.isDefault && thunderRoadAAGroupSchema.sharedBundle)
                {
                    foreach (var br in buildResult.AssetBundleBuildResults)
                    {
                        if (br.SourceAssetGroup == addressableAssetGroup)
                        {
                            string noHashFilePath = br.FilePath.Replace($"_{br.Hash}", "");
                            File.Delete(br.FilePath);
                            File.Delete(noHashFilePath);
                            Debug.Log($"Deleted shared bundle: {noHashFilePath}");
                        }
                    }
                }
                bundledAssetGroupSchema.IncludeInBuild = false;
                if (addressableAssetGroup.name == "Default")
                {
                    settings.DefaultGroup = addressableAssetGroup;
                }
            }

            TimeSpan span = DateTime.Now.Subtract(timeStart);
            Debug.Log($"Build completed in {span.ToString(@"hh\:mm\:ss\:fff")}");

            OnBuildEvent?.Invoke(EventTime.OnEnd);

            return true;
        }

        public static void SetGraphicSettings(bool android)
        {
            // Configure stereo rendering
            if (android)
            {
                PlayerSettings.stereoRenderingPath = StereoRenderingPath.SinglePass;
            }
            else
            {
                PlayerSettings.stereoRenderingPath = StereoRenderingPath.Instancing;
            }

            var graphicsSettings = GraphicsSettings.GetGraphicsSettings();
            SerializedObject serializedObject = new SerializedObject(graphicsSettings);
            SerializedProperty lightmapKeepDirCombined = serializedObject.FindProperty("m_LightmapKeepDirCombined");
            SerializedProperty lightmapKeepShadowMask = serializedObject.FindProperty("m_LightmapKeepShadowMask");
            SerializedProperty lightmapKeepSubtractive = serializedObject.FindProperty("m_LightmapKeepSubtractive");

            lightmapKeepDirCombined.boolValue = !android;
            lightmapKeepShadowMask.boolValue = !android;
            lightmapKeepSubtractive.boolValue = android;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(graphicsSettings);
            AssetDatabase.SaveAssets();
        }

        public class AssetBundleShaderStripPreprocess : IPreprocessShaders
        {
            private static bool enabled;
            private static List<Shader> stripShaders = new List<Shader>();

            public int callbackOrder => 0;

            public static void SetShadersToStripDuringBuild(List<Shader> shaders)
            {
                StringBuilder sb = new();
                if (shaders != null)
                {
                    foreach (Shader shader in shaders)
                    {
                        sb.AppendLine($"This shader will be stripped from build [{shader.name}]");
                    }
                    Debug.Log(sb.ToString());
                }
                stripShaders = shaders;
                enabled = stripShaders is { Count: > 0 };
            }

            public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderCompilerData)
            {
                if (enabled && stripShaders.Contains(shader))
                {
                    shaderCompilerData.Clear();
                }
            }
        }
    }
}
