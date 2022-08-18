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

        public static string gamePath;

        public static bool clearCache;
        public static bool runGameAfterBuild;
        public static string runGameArguments;
        public static bool cleanDestination = true;
        public static bool forceLinearFog = true;

        [MenuItem("ThunderRoad (SDK)/Asset Bundle Builder")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<AssetBundleBuilderGUI>("Asset Bundle Builder");
        }

        private void OnFocus()
        {
            gamePath = EditorPrefs.GetString("TRAB.GamePath");
            clearCache = EditorPrefs.GetBool("TRAB.ClearCache");
            runGameAfterBuild = EditorPrefs.GetBool("TRAB.RunGameAfterBuild");
            cleanDestination = EditorPrefs.GetBool("TRAB.CleanDestination");
            runGameArguments = EditorPrefs.GetString("TRAB.RunGameArguments");
            forceLinearFog = EditorPrefs.GetBool("TRAB.ForceLinearFog");

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
                        string assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), GameSettings.instance.addressableEditorPath);
                        AssetBundleBuilder.CopyAssetsToBuild(assetsFolderPath, AssetBundleBuilder.exportFolderName, assetBundleGroup.isDefault, gamePath);
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
                EditorGUI.EndDisabledGroup();
                clearCache = GUILayout.Toggle(clearCache, new GUIContent("Clear build cache", ""));
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

            if (runGameAfterBuild)
            {
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Arguments"), new GUIStyle("BoldLabel"), GUILayout.Width(150));
                string newRunGameArguments = GUILayout.TextField(runGameArguments, 25);
                if (newRunGameArguments != runGameArguments)
                {
                    EditorPrefs.SetString("TRAB.RunGameArguments", newRunGameArguments);
                    runGameArguments = newRunGameArguments;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Game folder Directory"), new GUIStyle("BoldLabel"), GUILayout.Width(150));
            if (GUILayout.Button(gamePath, new GUIStyle("textField")))
            {
                gamePath = EditorUtility.OpenFolderPanel("Select game folder", "", "");
                EditorPrefs.SetString("TRAB.GamePath", gamePath);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        protected static void BuildSelected()
        {
            foreach (AssetBundleGroup assetBundleGroup in assetBundleGroups)
            {
                if (assetBundleGroup.selected) Build(assetBundleGroup);
            }

            if ((EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows) && runGameAfterBuild)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Path.Combine(gamePath, PlayerSettings.productName + ".exe");
                process.StartInfo.Arguments = runGameArguments;
                process.Start();
                Debug.Log("Start game: " + process.StartInfo.FileName + " " + process.StartInfo.Arguments);
            }

            // The end
            System.Media.SystemSounds.Asterisk.Play();
        }

        protected static void Build(AssetBundleGroup assetBundleGroup)
        {
            string gameName = PlayerSettings.productName;

            AssetBundleBuilder.Build(assetBundleGroup, clearCache);

            // Export
            if (assetBundleGroup.exportAfterBuild)
            {
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
                {
                    if (!File.Exists(Path.Combine(gamePath, gameName + ".exe")))
                    {
                        Debug.LogError("Target game not supported!");
                        return;
                    }

                    // Get paths
                    string assetsFullPath = Path.Combine(Directory.GetCurrentDirectory(), AssetBundleBuilder.assetsLocalPath);
                    string catalogFullPath = Path.Combine(Directory.GetCurrentDirectory(), AssetBundleBuilder.catalogLocalPath);
                    string destinationAssetsPath = "";
                    string destinationCatalogPath = "";
                    if (assetBundleGroup.isDefault) destinationAssetsPath = destinationCatalogPath = Path.Combine(gamePath, gameName + "_Data/StreamingAssets/Default");
                    else destinationAssetsPath = destinationCatalogPath = Path.Combine(gamePath, gameName + "_Data/StreamingAssets/Mods", AssetBundleBuilder.exportFolderName);

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

                    // Copy json catalog to destination path
                    AssetBundleBuilder.CopyDirectory(catalogFullPath, destinationCatalogPath);
                    Debug.Log("Copied catalog folder " + catalogFullPath + " to " + destinationCatalogPath);
                    // Copy plugin dll if any
                    string dllPath = Path.Combine("BuildStaging", "Plugins", AssetBundleBuilder.exportFolderName) + "/bin/Release/netstandard2.0/" + AssetBundleBuilder.exportFolderName + ".dll";
                    if (File.Exists(dllPath))
                    {
                        File.Copy(dllPath, destinationCatalogPath + "/" + AssetBundleBuilder.exportFolderName + ".dll", true);
                        Debug.Log("Copied dll " + dllPath + " to " + destinationCatalogPath);
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
                    AndroidPushAssets(assetBundleGroup.isDefault, AssetBundleBuilder.exportFolderName);
                }
            }

            Debug.Log("Export done");
        }

        public static void AndroidPushAssets(bool toDefault, string folderName)
        {
            string buildPlatformPath = Path.Combine(Directory.GetCurrentDirectory(), "BuildStaging/Builds", EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ? "Android" : "Windows");
            string buildPlatformFolderPath = Path.Combine(buildPlatformPath, AssetBundleBuilder.androidAssetFolderName);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = GetAdbPath();
            if (toDefault)
            {
                process.StartInfo.Arguments = "push " + buildPlatformFolderPath + "/. /sdcard/Android/obb/" + PlayerSettings.applicationIdentifier;
            }
            else
            {
                process.StartInfo.Arguments = "push " + buildPlatformFolderPath + "/. /sdcard/Android/data/" + PlayerSettings.applicationIdentifier + "/files/Mods/" + folderName;
            }
            process.Start();
            process.WaitForExit();
            Debug.Log("Assets pushed to Android device");
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