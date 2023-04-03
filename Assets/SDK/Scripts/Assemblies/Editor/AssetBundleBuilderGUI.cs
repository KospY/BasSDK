using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEditor.Android;
using Newtonsoft.Json;
using System.Linq;
using Ionic.Zip;
using UnityEngine.Rendering;
using System.Reflection;
using UnityEditor.Build.Content;

namespace ThunderRoad
{
    public class AssetBundleBuilderGUI : EditorWindow
    {
        public static List<AssetBundleGroup> assetBundleGroups = new List<AssetBundleGroup>();

        public static string gameExePath;

        public static bool clearCache;
        public static bool uncompressed;
        public static bool runGameAfterBuild;
        public static string runGameArguments;
        public static bool cleanDestination = true;

        [MenuItem("ThunderRoad (SDK)/Asset Bundle Builder")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<AssetBundleBuilderGUI>("Asset Bundle Builder");
        }

        private void OnFocus()
        {
            gameExePath = EditorPrefs.GetString("TRAB.GameExePath");
            clearCache = EditorPrefs.GetBool("TRAB.ClearCache");
            uncompressed = EditorPrefs.GetBool("TRAB.Uncompressed");
            runGameAfterBuild = EditorPrefs.GetBool("TRAB.RunGameAfterBuild");
            cleanDestination = EditorPrefs.GetBool("TRAB.CleanDestination");
            runGameArguments = EditorPrefs.GetString("TRAB.RunGameArguments");

            assetBundleGroups = new List<AssetBundleGroup>();
            foreach (AssetBundleGroup assetBundleGroup in EditorCommon.GetAllProjectAssets<AssetBundleGroup>())
            {
                assetBundleGroups.Add(assetBundleGroup);
            }
        }

        public void RefreshAvailableAAGroups()
        {
            foreach (AddressableAssetGroup aaGroup in EditorCommon.GetAllProjectAssets<AddressableAssetGroup>())
            {
                if (AddressableAssetSettingsDefaultObject.Settings.groups.Contains(aaGroup)) continue;
                AddressableAssetSettingsDefaultObject.Settings.groups.Add(aaGroup);
            }
        }


        private void OnGUI()
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label(new GUIContent("AssetBundle builder (" + EditorUserBuildSettings.activeBuildTarget + ")"), new GUIStyle("BoldLabel"));
            GUILayout.Space(5);

            //scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            int selectedCount = 0;
            foreach (AssetBundleGroup assetBundleGroup in assetBundleGroups)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    assetBundleGroup.selected = EditorGUILayout.Toggle(assetBundleGroup.selected, GUILayout.MaxWidth(20));
                    if (assetBundleGroup.selected) selectedCount++;
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField(assetBundleGroup, typeof(AssetBundleGroup), false);
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.LabelField("Export", GUILayout.Width(60));
                    assetBundleGroup.exportAfterBuild = EditorGUILayout.Toggle(assetBundleGroup.exportAfterBuild, GUILayout.MaxWidth(20));
                    if (GUILayout.Button("Export now", GUILayout.Width(80)))
                    {
                        AssetBundleBuilder.exportFolderName = assetBundleGroup.folderName;
                        Export(assetBundleGroup);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            //EditorGUILayout.EndScrollView();

            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();
            {
                if (selectedCount == 0) EditorGUI.BeginDisabledGroup(true);
                if (GUILayout.Button("Build asset bundle group(s) selected"))
                {
                    BuildSelected();
                }

                string buttonName = uncompressed ? "Set Compressed" : "Set Uncompressed";
                if (GUILayout.Button(buttonName))
                {
                    uncompressed = !uncompressed;
                    EditorPrefs.SetBool("TRAB.Uncompressed", uncompressed);
                    SetCompression(uncompressed ? BundledAssetGroupSchema.BundleCompressionMode.Uncompressed : BundledAssetGroupSchema.BundleCompressionMode.LZ4);
                }
                EditorGUI.EndDisabledGroup();
                clearCache = GUILayout.Toggle(clearCache, new GUIContent("Clear build cache", ""));
                EditorPrefs.SetBool("TRAB.ClearCache", clearCache);
                //uncompressed = GUILayout.Toggle(uncompressed, new GUIContent("Uncompressed for internal use", ""));
                //EditorPrefs.SetBool("TRAB.Uncompressed", uncompressed);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label(new GUIContent("Export parameters"), new GUIStyle("BoldLabel"));
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            if (selectedCount != 1) EditorGUI.BeginDisabledGroup(true);
            bool newRunGameAfterBuild = GUILayout.Toggle(runGameAfterBuild, "Run game after build", GUILayout.Width(150));
            if (newRunGameAfterBuild != runGameAfterBuild)
            {
                EditorPrefs.SetBool("TRAB.RunGameAfterBuild", newRunGameAfterBuild);
                runGameAfterBuild = newRunGameAfterBuild;
            }
            EditorGUI.EndDisabledGroup();

            bool newCleanDestination = GUILayout.Toggle(cleanDestination, "Clean destination", GUILayout.Width(150));
            if (newCleanDestination != cleanDestination)
            {
                EditorPrefs.SetBool("TRAB.CleanDestination", newCleanDestination);
                cleanDestination = newCleanDestination;
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Arguments"), new GUIStyle("BoldLabel"), GUILayout.Width(80));
            string newRunGameArguments = GUILayout.TextField(runGameArguments, 25);
            if (newRunGameArguments != runGameArguments)
            {
                EditorPrefs.SetString("TRAB.RunGameArguments", newRunGameArguments);
                runGameArguments = newRunGameArguments;
            }

            if (GUILayout.Button("Run game", GUILayout.Width(90)))
            {
                RunGame();
            }

            if (GUILayout.Button("Stop game", GUILayout.Width(100)))
            {
                StopGame();
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
            {
                GUILayout.Label(new GUIContent("Game executable path"), new GUIStyle("BoldLabel"), GUILayout.Width(150));
                if (GUILayout.Button(gameExePath, new GUIStyle("textField")))
                {
                    gameExePath = EditorUtility.OpenFilePanel("Select game executable", "", "exe");
                    if (!CheckGameExe(gameExePath))
                    {
                        gameExePath = null;
                    }
                    EditorPrefs.SetString("TRAB.GameExePath", gameExePath);
                }
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                if (GUILayout.Button("Check android device connection"))
                {
                    AndroidListDevices();
                }
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }


        protected static void CloseAddressablesGroupsWindow()
        {
            var window = EditorWindow.GetWindow(typeof(EditorWindow), false, "Addressables Groups");
            if (window.titleContent.text == "Addressables Groups") window.Close();
        }

        protected static void BuildSelected()
        {
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Single);

            EditorUtility.UnloadUnusedAssetsImmediate(); // https://issuetracker.unity3d.com/issues/addressables-very-slow-build-when-editor-heap-memory-is-full
            GC.Collect();

            CloseAddressablesGroupsWindow(); // https://forum.unity.com/threads/buildplayercontent-calculate-asset-dependency-data-takes-forever.1015951/

            foreach (AssetBundleGroup assetBundleGroup in assetBundleGroups)
            {
                if (assetBundleGroup.selected) Build(assetBundleGroup);
            }

            if (runGameAfterBuild)
            {
                RunGame();
            }

            // The end
            System.Media.SystemSounds.Asterisk.Play();
        }

        /// <summary>
        /// LZ4 recommended generally
        /// https://thegamedev.guru/unity-addressables/compression-benchmark/
        /// </summary>
        protected static void SetCompression(BundledAssetGroupSchema.BundleCompressionMode compressionMode)
        {
            foreach (var addressableAssetGroup in UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.groups)
            {
                if (addressableAssetGroup == null) continue;
                var groupSchema = addressableAssetGroup.GetSchema<BundledAssetGroupSchema>();

                if (groupSchema != null)
                {
                    groupSchema.Compression = compressionMode;
                }
            }
        }

        protected static void RunGame()
        {
            if ((EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows) && runGameAfterBuild)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = gameExePath;
                process.StartInfo.Arguments = runGameArguments;
                process.Start();
                Debug.Log("Start game: " + process.StartInfo.FileName + " " + process.StartInfo.Arguments);
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = GetAdbPath();
                process.StartInfo.Arguments = $"shell am start -n {ThunderRoadSettings.current.game.appIdentifier}/com.unity3d.player.UnityPlayerActivity --es args '{runGameArguments}'";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();
                Debug.Log("Start process: " + process.StandardOutput.ReadToEnd());
            }
        }

        protected static void StopGame()
        {
            if ((EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows) && runGameAfterBuild)
            {
                foreach (var process in System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(gameExePath)))
                {
                    process.Kill();
                }
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = GetAdbPath();
                process.StartInfo.Arguments = $"shell am force-stop {ThunderRoadSettings.current.game.appIdentifier}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();
                Debug.Log("Stop process: " + process.StandardOutput.ReadToEnd());
            }
        }

        protected static bool CheckGameExe(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("Game executable can't be found");
                return false;
            }

            var versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(path);


            if (versionInfo.LegalCopyright != "Warpfrog" || versionInfo.ProductName != ThunderRoadSettings.current.game.productName)
            {
                Debug.LogError("Target game not supported!");
                return false;
            }

            if (versionInfo.ProductVersion != ThunderRoadSettings.current.game.minModVersion)
            {
                Debug.LogError($"Game executable is not compatible (Game version is: {versionInfo.ProductVersion}, while SDK version is {ThunderRoadSettings.current.game.minModVersion}");
                return false;
            }
            return true;
        }

        protected static void Build(AssetBundleGroup assetBundleGroup)
        {
            if (assetBundleGroup.exportAfterBuild && !CheckGameExe(gameExePath)) return;

            SetCompression(uncompressed ? BundledAssetGroupSchema.BundleCompressionMode.Uncompressed : BundledAssetGroupSchema.BundleCompressionMode.LZ4);

            AssetBundleBuilder.Build(assetBundleGroup, clearCache);

            if (assetBundleGroup.exportAfterBuild)
            {
                Export(assetBundleGroup);
            }
        }

        public static void Export(AssetBundleGroup assetBundleGroup)
        {
            string assetsFullPath = Path.Combine(Directory.GetCurrentDirectory(), AssetBundleBuilder.assetsLocalPath);
            string catalogFullPath = Path.Combine(Directory.GetCurrentDirectory(), ThunderRoadSettings.current.catalogsEditorPath, FileManager.defaultFolderName, AssetBundleBuilder.exportFolderName);

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
            {
                string destinationAssetsPath = "";
                string destinationCatalogPath = "";
                if (assetBundleGroup.isDefault) destinationAssetsPath = destinationCatalogPath = Path.Combine(Path.GetDirectoryName(gameExePath), Path.GetFileNameWithoutExtension(gameExePath) + "_Data/StreamingAssets/Default");
                else destinationAssetsPath = destinationCatalogPath = Path.Combine(Path.GetDirectoryName(gameExePath), Path.GetFileNameWithoutExtension(gameExePath) + "_Data/StreamingAssets/Mods", AssetBundleBuilder.exportFolderName);

                // Create folders if needed
                if (!File.Exists(destinationAssetsPath)) Directory.CreateDirectory(destinationAssetsPath);
                if (!File.Exists(destinationCatalogPath)) Directory.CreateDirectory(destinationCatalogPath);

                // Clean destination path
                if (cleanDestination)
                {
                    foreach (string filePath in Directory.GetFiles(destinationAssetsPath, "*.*", SearchOption.AllDirectories)) File.Delete(filePath);
                    foreach (string filePath in Directory.GetFiles(destinationCatalogPath, "*.*", SearchOption.AllDirectories)) File.Delete(filePath);
                }
                else
                {
                    foreach (string filePath in Directory.GetFiles(destinationAssetsPath, "catalog_*.json", SearchOption.AllDirectories)) File.Delete(filePath);
                    foreach (string filePath in Directory.GetFiles(destinationAssetsPath, "catalog_*.hash", SearchOption.AllDirectories)) File.Delete(filePath);

                    foreach (string filePath in Directory.GetFiles(destinationCatalogPath, "catalog_*.json", SearchOption.AllDirectories)) File.Delete(filePath);
                    foreach (string filePath in Directory.GetFiles(destinationCatalogPath, "catalog_*.hash", SearchOption.AllDirectories)) File.Delete(filePath);
                }

                // Copy addressable assets to destination path
                AssetBundleBuilder.CopyDirectory(assetsFullPath, destinationAssetsPath);
                Debug.Log("Copied addressable asset folder " + assetsFullPath + " to " + destinationAssetsPath);

                if (Directory.Exists(catalogFullPath))
                {
                    AssetBundleBuilder.CopyDirectory(catalogFullPath, destinationAssetsPath);
                    Debug.Log("Copied json folder " + catalogFullPath + " to " + destinationAssetsPath);
                }

                if (assetBundleGroup.exportModManifest)
                {
                    string manifestTempFolderPath = GenerateManifest(assetBundleGroup);
                    AssetBundleBuilder.CopyDirectory(manifestTempFolderPath, destinationAssetsPath);
                }
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                string adbPath = Path.Combine(EditorPrefs.GetString("AndroidSdkRoot"), "platform-tools", "adb.exe");
                if (!EditorPrefs.HasKey("AndroidSdkRoot") || !File.Exists(adbPath))
                {
                    Debug.LogError("Android SDK is not installed!");
                    Debug.LogError("Path not found " + adbPath);
                    return;
                }

                if (assetBundleGroup.isDefault)
                {
                    AndroidPushAssets(assetsFullPath);
                    if (Directory.Exists(catalogFullPath))
                    {
                        AndroidPushAssets(catalogFullPath);
                    }
                }
                else
                {
                    AndroidPushAssets(assetsFullPath, assetBundleGroup.folderName);
                    if (Directory.Exists(catalogFullPath))
                    {
                        AndroidPushAssets(catalogFullPath, assetBundleGroup.folderName);
                    }
                    if (assetBundleGroup.exportModManifest)
                    {
                        string manifestTempFolderPath = GenerateManifest(assetBundleGroup);
                        AndroidPushAssets(manifestTempFolderPath, assetBundleGroup.folderName);
                    }
                }
            }
            Debug.Log("Export done");
        }

        public static string GenerateManifest(AssetBundleGroup assetBundleGroup)
        {
            ModManager.ModData modData = new ModManager.ModData();
            modData.Name = assetBundleGroup.modName;
            modData.Description = assetBundleGroup.modDescription;
            modData.Author = assetBundleGroup.modAuthor;
            modData.ModVersion = assetBundleGroup.modVersion;
            modData.GameVersion = ThunderRoadSettings.current.game.minModVersion;
            modData.Thumbnail = assetBundleGroup.modThumbnail;

            string json = JsonConvert.SerializeObject(modData, Formatting.Indented);

            string tempFolder = $"{Application.temporaryCachePath}/tmpManifest";
            if (!Directory.Exists(tempFolder)) Directory.CreateDirectory(tempFolder);

            File.WriteAllText($"{tempFolder}/manifest.json", json);
            return tempFolder;
        }

        public static void AndroidListDevices()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = GetAdbPath();
            process.StartInfo.Arguments = "devices";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();
            Debug.Log("Result: " + process.StandardOutput.ReadToEnd());
        }

        public static void AndroidPushAssets(string sourcePath, string modFolderName = null)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = GetAdbPath();
            if (string.IsNullOrEmpty(modFolderName))
            {
                process.StartInfo.Arguments = $"push {sourcePath}/. /sdcard/Android/obb/{ThunderRoadSettings.current.game.appIdentifier}";
            }
            else
            {
                process.StartInfo.Arguments = $"push {sourcePath}/. /sdcard/Android/data/{ThunderRoadSettings.current.game.appIdentifier}/files/Mods/" + modFolderName;
            }
            Debug.Log(process.StartInfo.Arguments);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();
            Debug.Log("Push Assets output: " + process.StandardOutput.ReadToEnd());
        }

        public static string GetAdbPath()
        {
            return Path.Combine(EditorPrefs.GetString("AndroidSdkRoot"), "platform-tools", "adb.exe");
        }
    }

    [JsonObject]
    [Serializable]
    public class AssetBundleBuilderProfile
    {
        public AssetBundleBuilderProfile(string n = "", string f = "", Dictionary<string, bool> g = null)
        {
            profileName = n;
            exportFolder = f;
            groups = g;
        }

        public string profileName = "";
        public string exportFolder = "";
        public Dictionary<string, bool> groups = new Dictionary<string, bool>();
    }
}