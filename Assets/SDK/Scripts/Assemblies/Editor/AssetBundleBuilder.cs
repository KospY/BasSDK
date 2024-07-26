using Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public static string androidAssetFolderName = "assets";

        public static string assetsLocalPath
        {
            get
            {
                string buildtarget = "Windows";
                if (Common.GetQualityLevel() == QualityLevel.Android)
                    buildtarget = "Android";
                if (Common.GetQualityLevel() == QualityLevel.PS5)
                    buildtarget = "PS5";
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
                    
                    case "-assetbundlegroup":
                        {
                            Debug.Log($"AssetBundleGroup: {args[i + 1]}");
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
                                Debug.LogError($"assetBundleGroup: {args[i + 1]} , cannot be found");
                            }
                            break;
                        }
                    case "-purgecache":
                        {
                            Debug.Log("Purge Cache");
                            purgeCache = true;
                            break;
                        }
                    case "-dryrun":
                        {
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

        private static void VFXReimport()
        {
            // Get all loaded assemblies in the current AppDomain
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Find the assembly by name
            Assembly loadedAssembly = loadedAssemblies.FirstOrDefault(a => a.GetName().Name == "Unity.VisualEffectGraph.Editor");
            if (loadedAssembly == null)
            {
                Debug.LogError("Failed to find VisualEffectGraph.Editor assembly");
                return;
            }
            // Get all non-public types in the assembly
            Type[] allTypes = loadedAssembly.GetTypes();

            // Find the specific non-public class by name
            Type vfxAssetManagerType = allTypes.FirstOrDefault(t => t.Name == "VFXAssetManager");
            if (vfxAssetManagerType != null)
            {
                System.Reflection.MethodInfo buildAndSaveMethod = vfxAssetManagerType.GetMethod("BuildAndSave", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                if (buildAndSaveMethod != null)
                {
                    buildAndSaveMethod.Invoke(null, null);
                }
                else
                {
                    Debug.LogError("Failed to find VFXAssetManager.BuildAndSave method");
                }
            }
            else
            {
                Debug.LogError("Failed to find VFXAssetManager type");
            }
        }

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
                            Debug.LogWarning($"Group {group.name} is missing BundledAssetGroupSchema, skipping, bundle will not be built");
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
                            AssetDatabase.ImportAsset(groupEntry.AssetPath);
                        }
                    }
                }

                settings.profileSettings.SetValue(settings.activeProfileId, "ModLoadPath", $"{{ThunderRoad.FileManager.aaModPath}}/{assetBundleGroup.folderName}");
                settings.OverridePlayerVersion = assetBundleGroup.folderName;

                // Set builtin shader to export folder name to avoid duplicates
                settings.ShaderBundleNaming = ShaderBundleNaming.Custom;
                settings.ShaderBundleCustomNaming = assetBundleGroup.folderName;
                settings.BuildRemoteCatalog = true;

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
                    if (!Application.isBatchMode)
                        Debug.Log($"Stripped shader variant of shader [{shader.name}]");
                    shaderCompilerData.Clear();
                }
            }
        }
    }
}
