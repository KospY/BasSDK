using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering;
using Ionic.Zip;
using UnityEditor.Build;
using UnityEditor.Rendering;
using Unity.EditorCoroutines.Editor;
using System.Collections;

namespace ThunderRoad
{
    public class AssetBundleBuilder
    {
        public static string exportFolderName;

        public delegate void BuildEvent(EventTime eventTime);
        public static event BuildEvent OnBuildEvent;

        public static string androidAssetFolderName = "assets";

        public static string assetsLocalPath
        {
            get
            {
                string buildtarget = "Windows";
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) buildtarget = "Android";
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.PS5) buildtarget = "PS5";
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
            while  (AddressableAssetSettingsDefaultObject.Settings == null || AddressableAssetSettingsDefaultObject.Settings.groups == null)
            {
                yield return null;
            }
            for (int i = AddressableAssetSettingsDefaultObject.Settings.groups.Count - 1; i >= 0; i--)
            {
                if (AddressableAssetSettingsDefaultObject.Settings.groups[i] == null)
                {
                    Debug.Log("Removing missing AA group at index " + i);
                    AddressableAssetSettingsDefaultObject.Settings.groups.RemoveAt(i);
                }
            }
            foreach (AddressableAssetGroup aaGroup in EditorCommon.GetAllProjectAssets<AddressableAssetGroup>())
            {
                if (AddressableAssetSettingsDefaultObject.Settings.groups.Contains(aaGroup)) continue;
                Debug.Log("Adding missing AA group " + aaGroup.name);
                AddressableAssetSettingsDefaultObject.Settings.groups.Add(aaGroup);
            }
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

        public static void BatchBuild()
        {
            AssetBundleGroup assetBundleGroup = null;
            bool purgeCache = false;
            bool dryRun = false;
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-assetbundlegroup": {
                        Debug.Log("AssetBundleGroup: " + args[i + 1]);
                        foreach (AssetBundleGroup abg in EditorCommon.GetAllProjectAssets<AssetBundleGroup>())
                        {
                            if (abg.name == args[i + 1])
                            {
                                assetBundleGroup = abg;
                                break;
                            }
                        }
                        if (!assetBundleGroup)
                        {
                            Debug.LogError("assetBundleGroup: " + args[i + 1] + " , cannot be found");
                        }
                        break;
                    }
                    case "-purgecache": {
                        Debug.Log("Purge Cache");
                        purgeCache = true;
                        break;
                    }
                    case "-dryrun": {
                        Debug.Log("dryrun mode");
                        dryRun = true;
                        break;
                    }
                }
            }


            if (dryRun)
            {
                return;
            }

            if (assetBundleGroup)
            {
                if (Build(assetBundleGroup, purgeCache))
                {
                    EditorApplication.Exit(0);
                }
                else
                {
                    EditorApplication.Exit(1);
                }
            }
            else
            {
                Debug.LogError("-assetBundleGroup parameter is not set");
            }
        }

        public static bool Build(AssetBundleGroup assetBundleGroup, bool purgeCache)
        {
            DateTime timeStart = DateTime.Now;

            exportFolderName = assetBundleGroup.folderName;

            Debug.Log("Building asset bundles to destination: " + assetsLocalPath);

            // Workaround to fix VFX breaking randomly
            string[] visualEffectAssetGuids = AssetDatabase.FindAssets("t:visualeffectasset");
            foreach (string visualEffectAssetGuid in visualEffectAssetGuids)
            {
                AssetDatabase.ImportAsset(AssetDatabase.GUIDToAssetPath(visualEffectAssetGuid));
            }

            // Configure addressable groups
            List<Shader> shadersToStrip = new List<Shader>();
            if (AddressableAssetSettingsDefaultObject.Settings != null)
            {
                foreach (AddressableAssetGroup group in AddressableAssetSettingsDefaultObject.Settings.groups)
                {
                    if (group == null) continue;
                    BundledAssetGroupSchema bundledAssetGroupSchema = group.GetSchema<BundledAssetGroupSchema>();
                    if (bundledAssetGroupSchema != null)
                    {
                        bundledAssetGroupSchema.IncludeInBuild = false;
                        bundledAssetGroupSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.NoHash;

                        if (assetBundleGroup.isDefault)
                        {
                            AddressableAssetSettingsDefaultObject.Settings.DefaultGroup = group;
                        }

                        if (assetBundleGroup.addressableAssetGroups.Contains(group))
                        {
                            if (!assetBundleGroup.isDefault)
                            {
                                BundledAssetGroupSchema defaultAssetGroupSchema = AddressableAssetSettingsDefaultObject.Settings.DefaultGroup.GetSchema<BundledAssetGroupSchema>();
                                if (defaultAssetGroupSchema.BuildPath.GetName(AddressableAssetSettingsDefaultObject.Settings) == "DefaultBuildPath")
                                {
                                    AddressableAssetSettingsDefaultObject.Settings.DefaultGroup = group;
                                }
                            }
                            bundledAssetGroupSchema.IncludeInBuild = true;
                        }

                        if (bundledAssetGroupSchema.BuildPath.GetName(AddressableAssetSettingsDefaultObject.Settings) == "DefaultBuildPath")
                        {
                            bool haveAndroidLabel = false;
                            bool haveWindowsLabel = false;
                            foreach (var entry in group.entries)
                            {
                                Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(entry.AssetPath);
                                if (shader)
                                {
                                    shadersToStrip.Add(shader);
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
              
                            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android && !haveAndroidLabel)
                            {
                                bundledAssetGroupSchema.IncludeInBuild = false;
                            }
                            else if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android && Common.GetPlatform() == Platform.Windows && !haveWindowsLabel)
                            {
                                bundledAssetGroupSchema.IncludeInBuild = false;
                            }
                            else if (assetBundleGroup.isDefault && group.name == "Default")
                            {
                                AddressableAssetSettingsDefaultObject.Settings.DefaultGroup = group;
                            }
                            else
                            {
                                bundledAssetGroupSchema.IncludeInBuild = true;     
                            }
                        }

                        // Workaround to fix an issue when room is not correctly imported after a change
                        if (bundledAssetGroupSchema.IncludeInBuild)
                        {
                            foreach (AddressableAssetEntry groupEntry in group.entries)
                            {
                                if (groupEntry.labels.Contains("Area"))
                                {
                                    AssetDatabase.ImportAsset(groupEntry.AssetPath);
                                }
                            }
                        }
      
                        AddressableAssetSettingsDefaultObject.Settings.OverridePlayerVersion = assetBundleGroup.folderName;
                        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(group.Settings.activeProfileId, "LocalBuildPath", "[ThunderRoad.AssetBundleBuilder.assetsLocalPath]");
                        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(group.Settings.activeProfileId, "LocalLoadPath", assetBundleGroup.isDefault ? "{ThunderRoad.FileManager.aaDefaultPath}" : ("{ThunderRoad.FileManager.aaModPath}/" + assetBundleGroup.folderName));
                        // Set builtin shader to export folder name to avoid duplicates
                        AddressableAssetSettingsDefaultObject.Settings.ShaderBundleNaming = UnityEditor.AddressableAssets.Build.ShaderBundleNaming.Custom;
                        AddressableAssetSettingsDefaultObject.Settings.ShaderBundleCustomNaming = assetBundleGroup.folderName;
                        AddressableAssetSettingsDefaultObject.Settings.BuildRemoteCatalog = true;
                    }
                }
            }

            SetGraphicSettings(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ? true : false);
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

            if (OnBuildEvent != null) OnBuildEvent.Invoke(EventTime.OnStart);

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
                AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
            }

            AssetBundleShaderStripPreprocess.SetShadersToStripDuringBuild(shadersToStrip);
            AddressableAssetSettings.BuildPlayerContent(out var buildResult);
            //The PostProcessBuild callback for our shader stripper doesnt get called by Addressables BuildPlayerContent. Only in the PlayerBuilder
            
            AssetBundleShaderStripPreprocess.SetShadersToStripDuringBuild(null);

            if (!string.IsNullOrEmpty(buildResult.Error))
            {
                Debug.LogError("BuildPlayerContent failed");
                return false;
            }

            foreach (AddressableAssetGroup addressableAssetGroup in assetBundleGroup.addressableAssetGroups)
            {
                if (addressableAssetGroup == null) continue;
                BundledAssetGroupSchema bundledAssetGroupSchema = addressableAssetGroup.GetSchema<BundledAssetGroupSchema>();
                if (bundledAssetGroupSchema) bundledAssetGroupSchema.IncludeInBuild = false;
            }

            TimeSpan span = DateTime.Now.Subtract(timeStart);
            Debug.Log($"Build completed in {span.ToString(@"hh\:mm\:ss\:fff")}");

            if (OnBuildEvent != null) OnBuildEvent.Invoke(EventTime.OnEnd);

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

            public int callbackOrder
            {
                get { return 0; }
            }

            public static void SetShadersToStripDuringBuild(List<Shader> shaders)
            {
                if (shaders != null)
                {
                    foreach (Shader shader in shaders)
                    {
                        Debug.Log($"This shader will be stripped from build [{shader.name}]");
                    }
                }
                stripShaders = shaders;
                enabled = (stripShaders != null && stripShaders.Count > 0) ? true : false;
            }

            public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderCompilerData)
            {
                if (enabled && stripShaders.Contains(shader))
                {
                    if (!Application.isBatchMode) Debug.Log($"Stripped shader variant of shader [{shader.name}]");
                    shaderCompilerData.Clear();
                }
            }
        }
    }
}
