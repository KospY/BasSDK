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

namespace ThunderRoad
{
    public class ModBuilder : EditorWindow
    {
        public static string assetsLocalPath
        {
            get
            {
                string buildtarget = "Windows";
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) buildtarget = "Android";
                return (Path.Combine("BuildStaging", "AddressableAssets", buildtarget, exportFolderName));
            }
        }

        public static string catalogLocalPath
        {
            get
            {
                return (Path.Combine("BuildStaging", "Catalog", exportFolderName));
            }
        }

        [InitializeOnLoad]
        public class Startup
        {
            static Startup()
            {
                string catalogFullPath = Path.Combine(Directory.GetCurrentDirectory(), "BuildStaging/Catalog");
                if (!Directory.Exists(catalogFullPath))
                {
                    Directory.CreateDirectory(catalogFullPath);
                    Debug.Log("Created folder " + catalogFullPath);
                }
                string buildFullPath = Path.Combine(Directory.GetCurrentDirectory(), "BuildStaging/AddressableAssets");
                if (!Directory.Exists(buildFullPath))
                {
                    Directory.CreateDirectory(buildFullPath);
                    Debug.Log("Created folder " + buildFullPath);
                }
            }
        }

        public static string gamePath;

        public static bool toDefault;
        public static bool clearCache;
        public static string exportFolderName;
        public static ExportTo exportTo = ExportTo.Windows;
        public static bool runGameAfterBuild;
        public static string runGameArguments;
        public static bool cleanDestination = true;

        public static Action action = Action.BuildOnly;
        public static SupportedGame gameName = SupportedGame.BladeAndSorcery;

        public delegate void BuildEvent(EventTime eventTime);
        public static event BuildEvent OnBuildEvent;

        private static bool currentCheck = true;
        private static Vector2 scrollPos;

        private static int profileIndex = 0;
        private static int previousProfileIndex = 0;
        private static string profileName = "ProfileName";

        private static List<ModBuilderProfile> profiles = new List<ModBuilderProfile>();

        public static string androidAssetFolderName = "assets";

        public enum SupportedGame
        {
            BladeAndSorcery,
        }

        public enum ExportTo
        {
            Windows,
#if PrivateSDK
            Android,
#endif
        }

        public enum Action
        {
            BuildOnly,
            ExportOnly,
            BuildAndExport,
        }

        [MenuItem("ThunderRoad (SDK)/Mod Builder")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ModBuilder>("Mod Builder");
        }

        private void OnFocus()
        {
            gamePath = EditorPrefs.GetString("TRMB.GamePath");
            exportFolderName = EditorPrefs.HasKey("TRMB.ExportFolderName") ? EditorPrefs.GetString("TRMB.ExportFolderName") : "MyMod";
            exportTo = (ExportTo)EditorPrefs.GetInt("TRMB.ExportTo");
            toDefault = EditorPrefs.GetBool("TRMB.ToDefault");
            clearCache = EditorPrefs.GetBool("TRMB.ClearCache");
            runGameAfterBuild = EditorPrefs.GetBool("TRMB.RunGameAfterBuild");
            cleanDestination = EditorPrefs.GetBool("TRMB.CleanDestination");
            runGameArguments = EditorPrefs.GetString("TRMB.RunGameArguments");
            gameName = (SupportedGame)EditorPrefs.GetInt("TRMB.GameName");
            action = (Action)EditorPrefs.GetInt("TRMB.Action");

            LoadProfiles();
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            BuildProfileGUI();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            ExportFolderGUI();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (action == Action.BuildAndExport || action == Action.BuildOnly)
            {
                GUILayout.Label(new GUIContent("Included addressable group(s)"), new GUIStyle("BoldLabel"));
                GUILayout.Space(5);

                if (GUILayout.Button("Refresh available groups") && AddressableAssetSettingsDefaultObject.Settings != null)
                {
                    foreach (AddressableAssetGroup aaGroup in GetAllInstances<AddressableAssetGroup>())
                    {
                        if (AddressableAssetSettingsDefaultObject.Settings.groups.Contains(aaGroup)) continue;
                        AddressableAssetSettingsDefaultObject.Settings.groups.Add(aaGroup);
                    }
                }

                GUILayout.Space(5);
                if (GUILayout.Button("Check/uncheck all"))
                {
                    CheckAll(currentCheck);
                    currentCheck = !currentCheck;
                }

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(400));
                if (AddressableAssetSettingsDefaultObject.Settings != null)
                {
                    foreach (AddressableAssetGroup group in AddressableAssetSettingsDefaultObject.Settings.groups)
                    {
                        if (group == null) continue;
                        BundledAssetGroupSchema bundledAssetGroupSchema = group.GetSchema<BundledAssetGroupSchema>();
                        if (bundledAssetGroupSchema != null)
                        {
                            EditorGUILayout.BeginHorizontal();
                            bool newInclude = EditorGUILayout.Toggle(bundledAssetGroupSchema.IncludeInBuild, GUILayout.MaxWidth(20));
                            if (newInclude != bundledAssetGroupSchema.IncludeInBuild)
                            {
                                bundledAssetGroupSchema.IncludeInBuild = newInclude;
                                EditorUtility.SetDirty(group);
                            }
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUILayout.ObjectField(group, typeof(AddressableAssetGroup), false, GUILayout.MaxWidth(500));
                            EditorGUI.EndDisabledGroup();
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
                EditorGUILayout.EndScrollView();
            }

            if (action == Action.BuildAndExport || action == Action.ExportOnly)
            {
                if (action == Action.BuildAndExport)
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                }
                ExportToGUI();
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();
#if PrivateSDK
            if (action == Action.BuildOnly || exportTo == ExportTo.Android)
#else
            if (action == Action.BuildOnly)
#endif
            {
                if (GUILayout.Button("Copy assets to BuildStaging"))
                {
                    CopyBuildFiles(toDefault, exportFolderName, false);
                }
            }
            else
            {
                if (GUILayout.Button("Copy assets to game folder"))
                {
                    CopyBuildFiles(toDefault, exportFolderName, true);
                }
            }
            if (GUILayout.Button(action == Action.BuildOnly ? "Build" : (action == Action.ExportOnly ? "Export" : "Build and export"))) Build(action);
            
            bool newClearCache = GUILayout.Toggle(clearCache, new GUIContent("Clear build cache", ""));
            if (newClearCache != clearCache)
            {
                EditorPrefs.SetBool("TRMB.ClearCache", newClearCache);
                clearCache = newClearCache;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private static void CheckAll(bool b)
        {
            foreach (AddressableAssetGroup group in AddressableAssetSettingsDefaultObject.Settings.groups)
            {
                BundledAssetGroupSchema bundledAssetGroupSchema = group.GetSchema<BundledAssetGroupSchema>();
                if (bundledAssetGroupSchema != null)
                {
                    group.GetSchema<BundledAssetGroupSchema>().IncludeInBuild = b;
                }
            }
        }

        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return a;

        }

        static void ExportFolderGUI()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(toDefault ? "Default folder name" : "Mod folder name"), new GUIStyle("BoldLabel"), GUILayout.Width(150));
            string newModeName = GUILayout.TextField(exportFolderName, 25);

            string invalidChars = new string(Path.GetInvalidFileNameChars());
            foreach (char c in invalidChars)
            {
                newModeName = newModeName.Replace(c.ToString(), "");
            }

            if (newModeName != exportFolderName)
            {
                EditorPrefs.SetString("TRMB.ExportFolderName", newModeName);
                exportFolderName = newModeName;
            }

            Action newAction = (Action)EditorGUILayout.EnumPopup("", action, GUILayout.Width(150));
            if (newAction != action)
            {
                EditorPrefs.SetInt("TRMB.Action", (int)newAction);
                action = newAction;
            }

#if PrivateSDK
            bool newToDefault = GUILayout.Toggle(toDefault, new GUIContent("Default", "Export files and set catalog bundle paths to default folder (Warpfrog devs only!)"), GUILayout.Width(80));
            if (newToDefault != toDefault)
            {
                EditorPrefs.SetBool("TRMB.ToDefault", newToDefault);
                toDefault = newToDefault;
            }
#endif

            GUILayout.EndHorizontal();
        }

        static void BuildProfileGUI()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent("Profile"), new GUIStyle("BoldLabel"), GUILayout.Width(150));

            previousProfileIndex = profileIndex;
            profileIndex = EditorGUILayout.Popup(profileIndex, profiles.Select(n => n.profileName).ToArray());

            if (profileIndex != previousProfileIndex)
            {
                OnProfileIndexChange();
            }

            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Save profile"))
            {
                SaveProfile();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add profile..."))
            {
                AddProfile();
            }

            profileName = GUILayout.TextField(profileName);

            GUILayout.EndHorizontal();
        }

        static void OnProfileIndexChange()
        {
            ModBuilderProfile currentProfile = profiles[profileIndex];

            foreach (AddressableAssetGroup group in AddressableAssetSettingsDefaultObject.Settings.groups)
            {
                BundledAssetGroupSchema bundledAssetGroupSchema = group.GetSchema<BundledAssetGroupSchema>();
                if (bundledAssetGroupSchema != null)
                {

                    if (currentProfile.groups != null)
                    {
                        if (currentProfile.groups.ContainsKey(group.Name))
                        {
                            group.GetSchema<BundledAssetGroupSchema>().IncludeInBuild = currentProfile.groups[group.Name];
                        }
                        else
                        {
                            Debug.LogWarning("Group " + group.Name + " not found in profile " + currentProfile.profileName + ".");
                        }
                    }
                    else
                        CheckAll(false);
                }
            }

            exportFolderName = currentProfile.exportFolder;
            EditorPrefs.SetString("TRMB.ExportFolderName", exportFolderName);
        }

        static void SaveProfile()
        {
            ModBuilderProfile currentProfile = profiles[profileIndex];

            ModBuilderProfile mbp = new ModBuilderProfile();
            mbp.groups = new Dictionary<string, bool>();

            foreach (AddressableAssetGroup group in AddressableAssetSettingsDefaultObject.Settings.groups)
            {
                BundledAssetGroupSchema bundledAssetGroupSchema = group.GetSchema<BundledAssetGroupSchema>();
                if (bundledAssetGroupSchema != null)
                {
                    mbp.groups.Add(group.Name, group.GetSchema<BundledAssetGroupSchema>().IncludeInBuild);
                }
            }

            mbp.profileName = currentProfile.profileName;
            mbp.exportFolder = exportFolderName;

            profiles[profileIndex] = mbp;

            string json = JsonConvert.SerializeObject(profiles, Formatting.Indented);

            using (StreamWriter sw = new StreamWriter(Application.dataPath + "/SDK/modprofiles.json"))
            {
                sw.Write(json);
                sw.Close();
            }
        }

        static void LoadProfiles()
        {
            string json;

            using (StreamReader sr = new StreamReader(Application.dataPath + "/SDK/modprofiles.json"))
            {
                json = sr.ReadToEnd();
                sr.Close();
            }

            profiles = JsonConvert.DeserializeObject<List<ModBuilderProfile>>(json);
        }

        static void AddProfile()
        {
            if (profiles.FindIndex(x => x.profileName == profileName) != -1)
            {
                Debug.LogError("Profile " + profileName + " already exists.");
            }
            else
            {
                profiles.Add(new ModBuilderProfile(profileName, exportFolderName, null));
                profileIndex = profiles.Count - 1;
            }
        }

        static void ExportToGUI()
        {
            GUILayout.BeginHorizontal();
            ExportTo newExportTo = (ExportTo)EditorGUILayout.EnumPopup("Export to", exportTo);
            if (newExportTo != exportTo)
            {
                EditorPrefs.SetInt("TRMB.ExportTo", (int)newExportTo);
                exportTo = newExportTo;
            }

            bool newRunGameAfterBuild = GUILayout.Toggle(runGameAfterBuild, "Run game after build", GUILayout.Width(150));
            if (newRunGameAfterBuild != runGameAfterBuild)
            {
                EditorPrefs.SetBool("TRMB.RunGameAfterBuild", newRunGameAfterBuild);
                runGameAfterBuild = newRunGameAfterBuild;
            }

            bool newCleanDestination = GUILayout.Toggle(cleanDestination, "Clean destination", GUILayout.Width(150));
            if (newCleanDestination != cleanDestination)
            {
                EditorPrefs.SetBool("TRMB.CleanDestination", newCleanDestination);
                cleanDestination = newCleanDestination;
            }


            GUILayout.EndHorizontal();

            if (runGameAfterBuild && exportTo == ExportTo.Windows)
            {
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Arguments"), new GUIStyle("BoldLabel"), GUILayout.Width(150));
                string newRunGameArguments = GUILayout.TextField(runGameArguments, 25);
                if (newRunGameArguments != runGameArguments)
                {
                    EditorPrefs.SetString("TRMB.RunGameArguments", newRunGameArguments);
                    runGameArguments = newRunGameArguments;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(5);
#if PrivateSDK
            if (exportTo == ExportTo.Android)
            {
                GUILayout.Space(5);
                SupportedGame newGameName = (SupportedGame)EditorGUILayout.EnumPopup("Game name", gameName);
                if (newGameName != gameName)
                {
                    EditorPrefs.SetInt("TRMB.GameName", (int)newGameName);
                    gameName = newGameName;
                }
            }
#endif
            if (exportTo == ExportTo.Windows)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Game folder Directory"), new GUIStyle("BoldLabel"), GUILayout.Width(150));
                if (GUILayout.Button(gamePath, new GUIStyle("textField")))
                {
                    gamePath = EditorUtility.OpenFolderPanel("Select game folder", "", "");
                    EditorPrefs.SetString("TRMB.GamePath", gamePath);
                }
                GUILayout.EndHorizontal();
            }
        }

        static void Build(Action behaviour)
        {
            // Check error
            if (exportTo == ExportTo.Windows)
            {
                bool gameSupported = false;
                foreach (string supportedGame in Enum.GetNames(typeof(SupportedGame)))
                {
                    if (File.Exists(Path.Combine(gamePath, supportedGame + ".exe")))
                    {
                        gameSupported = true;
                        gameName = (SupportedGame)Enum.Parse(typeof(SupportedGame), supportedGame);
                    }
                }
                if (!gameSupported)
                {
                    Debug.LogError("Target game not supported!");
                    return;
                }
            }
#if PrivateSDK
            else if (exportTo == ExportTo.Android)
            {
                string adbPath = Path.Combine(EditorPrefs.GetString("AndroidSdkRoot"), "platform-tools", "adb.exe");
                if (!EditorPrefs.HasKey("AndroidSdkRoot") || !File.Exists(adbPath))
                {
                    Debug.LogError("Android SDK is not installed!");
                    Debug.LogError("Path not found " + adbPath);
                    return;
                }
            }
#endif
            // Configure stereo rendering
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                PlayerSettings.stereoRenderingPath = StereoRenderingPath.SinglePass;
            }
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
            {
                PlayerSettings.stereoRenderingPath = StereoRenderingPath.Instancing;
            }

            // Configure addressable groups
            if (AddressableAssetSettingsDefaultObject.Settings != null)
            {
                foreach (AddressableAssetGroup group in AddressableAssetSettingsDefaultObject.Settings.groups)
                {
                    BundledAssetGroupSchema bundledAssetGroupSchema = group.GetSchema<BundledAssetGroupSchema>();
                    if (bundledAssetGroupSchema != null)
                    {
                        if (group.Default)
                        {
                            bundledAssetGroupSchema.BuildPath.SetVariableByName(AddressableAssetSettingsDefaultObject.Settings, "LocalBuildPath");
                            bundledAssetGroupSchema.LoadPath.SetVariableByName(AddressableAssetSettingsDefaultObject.Settings, "LocalLoadPath");
                        }
                        bundledAssetGroupSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.NoHash;

                        AddressableAssetSettingsDefaultObject.Settings.OverridePlayerVersion = exportFolderName;
                        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(group.Settings.activeProfileId, "LocalBuildPath", "[ThunderRoad.ModBuilder.assetsLocalPath]");
                        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(group.Settings.activeProfileId, "LocalLoadPath", toDefault ? "{ThunderRoad.FileManager.aaDefaultPath}" : ("{ThunderRoad.FileManager.aaModPath}/" + exportFolderName));
                        // Set builtin shader to export folder name to avoid duplicates
                        AddressableAssetSettingsDefaultObject.Settings.ShaderBundleNaming = UnityEditor.AddressableAssets.Build.ShaderBundleNaming.Custom;
                        AddressableAssetSettingsDefaultObject.Settings.ShaderBundleCustomNaming = exportFolderName;
                        AddressableAssetSettingsDefaultObject.Settings.BuildRemoteCatalog = true;
                    }
                }
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            // Build
            if (behaviour == Action.BuildAndExport || behaviour == Action.BuildOnly)
            {
                Debug.Log("Assets path is: " + assetsLocalPath);
                if (OnBuildEvent != null) OnBuildEvent.Invoke(EventTime.OnStart);

                // Clean build path
                if (Directory.Exists(assetsLocalPath))
                {
                    foreach (string filePath in Directory.GetFiles(assetsLocalPath, "*.*", SearchOption.AllDirectories)) File.Delete(filePath);
                }
                else
                {

                }

#if PrivateSDK
                ShaderReport shaderReport = ShaderReport.Get();
                if (shaderReport) shaderReport.buildShaders.Clear();
                shaderReport.lastBuildUpdateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
#endif

                BuildCache.PurgeCache(clearCache);
                AddressableAssetSettings.CleanPlayerContent();
                AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
                AddressableAssetSettings.BuildPlayerContent();
                Debug.Log("Build done");

#if PrivateSDK
                if (shaderReport) shaderReport.RefreshShadersFromBuild();
#endif
                if (OnBuildEvent != null) OnBuildEvent.Invoke(EventTime.OnEnd);
            }

            // Export
            if (behaviour == Action.BuildAndExport || behaviour == Action.ExportOnly)
            {
                if (exportTo == ExportTo.Windows)
                {
                    // Get paths
                    string assetsFullPath = Path.Combine(Directory.GetCurrentDirectory(), assetsLocalPath);
                    string catalogFullPath = Path.Combine(Directory.GetCurrentDirectory(), catalogLocalPath);
                    string destinationAssetsPath = "";
                    string destinationCatalogPath = "";
                    if (exportTo == ExportTo.Windows)
                    {
                        if (toDefault) destinationAssetsPath = destinationCatalogPath = Path.Combine(gamePath, gameName + "_Data/StreamingAssets/Default");
                        else destinationAssetsPath = destinationCatalogPath = Path.Combine(gamePath, gameName + "_Data/StreamingAssets/Mods", exportFolderName);
                    }

                    // Create folders if needed
                    if (!File.Exists(destinationAssetsPath)) Directory.CreateDirectory(destinationAssetsPath);
                    if (!File.Exists(destinationCatalogPath)) Directory.CreateDirectory(destinationCatalogPath);

                    // Clean destination path
                    if (cleanDestination)
                    {
                        foreach (string filePath in Directory.GetFiles(destinationAssetsPath, "*.*", SearchOption.AllDirectories)) File.Delete(filePath);
                        if (exportTo == ExportTo.Windows)
                        {
                            foreach (string filePath in Directory.GetFiles(destinationCatalogPath, "*.*", SearchOption.AllDirectories)) File.Delete(filePath);
                        }
                    }
                    else
                    {
                        foreach (string filePath in Directory.GetFiles(destinationAssetsPath, "catalog_*.json", SearchOption.AllDirectories)) File.Delete(filePath);
                        foreach (string filePath in Directory.GetFiles(destinationAssetsPath, "catalog_*.hash", SearchOption.AllDirectories)) File.Delete(filePath);
                        if (exportTo == ExportTo.Windows)
                        {
                            foreach (string filePath in Directory.GetFiles(destinationCatalogPath, "catalog_*.json", SearchOption.AllDirectories)) File.Delete(filePath);
                            foreach (string filePath in Directory.GetFiles(destinationCatalogPath, "catalog_*.hash", SearchOption.AllDirectories)) File.Delete(filePath);
                        }
                    }

                    // Copy addressable assets to destination path
                    CopyDirectory(assetsFullPath, destinationAssetsPath);
                    Debug.Log("Copied addressable asset folder " + assetsFullPath + " to " + destinationAssetsPath);

                    if (exportTo == ExportTo.Windows)
                    {
                        // Copy json catalog to destination path
                        CopyDirectory(catalogFullPath, destinationCatalogPath);
                        Debug.Log("Copied catalog folder " + catalogFullPath + " to " + destinationCatalogPath);
                        // Copy plugin dll if any
                        string dllPath = Path.Combine("BuildStaging", "Plugins", exportFolderName) + "/bin/Release/netstandard2.0/" + exportFolderName + ".dll";
                        if (File.Exists(dllPath))
                        {
                            File.Copy(dllPath, destinationCatalogPath + "/" + exportFolderName + ".dll", true);
                            Debug.Log("Copied dll " + dllPath + " to " + destinationCatalogPath);
                        }
                    }
                }

                if ((exportTo == ExportTo.Windows) && runGameAfterBuild)
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = Path.Combine(gamePath, gameName + ".exe");
                    process.StartInfo.Arguments = runGameArguments;
                    process.Start();
                    Debug.Log("Start game: " + process.StartInfo.FileName + " " + process.StartInfo.Arguments);
                }
#if PrivateSDK
                if (exportTo == ExportTo.Android)
                {
                    AndroidPushAssets(toDefault, exportFolderName);
                }
#endif
                Debug.Log("Export done");
            }
            // The end
            System.Media.SystemSounds.Asterisk.Play();
        }

        public static void CopyBuildFiles(bool toDefault, string exportFolderName, bool useGamePath)
        {
            string buildtarget = "Windows";
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) buildtarget = "Android";

            string buildPlateformPath = Path.Combine(Directory.GetCurrentDirectory(), "BuildStaging/Builds", EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ? "Android" : "Windows");
            string assetsFullPath = Path.Combine(Directory.GetCurrentDirectory(), "BuildStaging/AddressableAssets", buildtarget, exportFolderName);
            string catalogFullPath = Path.Combine(Directory.GetCurrentDirectory(), "BuildStaging/Catalog", exportFolderName);

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                string buildPlateformFolderPath = Path.Combine(buildPlateformPath, androidAssetFolderName);
                if (Directory.Exists(buildPlateformFolderPath)) Directory.Delete(buildPlateformFolderPath, true);
                CopyDirectory(assetsFullPath, buildPlateformFolderPath);
                Debug.Log("Copied folder " + exportFolderName + " to " + buildPlateformFolderPath);

                string jsondbPath = Path.Combine(buildPlateformFolderPath, exportFolderName + ".jsondb");
                ZipFile zip = new ZipFile();
                zip.AddDirectory(catalogFullPath);
                zip.Save(jsondbPath);
                Debug.Log("Zipped json " + exportFolderName + " to " + jsondbPath);
            }
            else
            {
                string streamingAssetDefaultPath = Path.Combine(useGamePath ? gamePath : buildPlateformPath, PlayerSettings.productName + "_Data/StreamingAssets/Default");
                string streamingAssetModPath = Path.Combine(useGamePath ? gamePath : buildPlateformPath, PlayerSettings.productName + "_Data/StreamingAssets/Mods");
                string destFolder = toDefault ? streamingAssetDefaultPath : streamingAssetModPath;

                if (toDefault)
                {
                    CopyDirectory(assetsFullPath, destFolder);
                    Debug.Log("Copied folder " + exportFolderName + " to " + destFolder);
                }
                else
                {
                    CopyDirectory(assetsFullPath, Path.Combine(destFolder, exportFolderName));
                    Debug.Log("Copied folder " + exportFolderName + " to " + Path.Combine(destFolder, exportFolderName));
                }

                string jsondbPath = Path.Combine(destFolder, exportFolderName + ".jsondb");
                ZipFile zip = new ZipFile();
                zip.AddDirectory(catalogFullPath);
                zip.Save(jsondbPath);
                Debug.Log("Zipped json " + exportFolderName + " to " + jsondbPath);
            }
        }

        public static void AndroidPushAssets(bool toDefault, string folderName)
        {
            string buildPlateformPath = Path.Combine(Directory.GetCurrentDirectory(), "BuildStaging/Builds", EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ? "Android" : "Windows");
            string buildPlateformFolderPath = Path.Combine(buildPlateformPath, androidAssetFolderName);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = GetAdbPath();
            if (toDefault)
            {
                process.StartInfo.Arguments = "push " + buildPlateformFolderPath + "/. /sdcard/Android/obb/" + PlayerSettings.applicationIdentifier;
            }
            else
            {
                process.StartInfo.Arguments = "push " + buildPlateformFolderPath + "/. /sdcard/Android/data/" + PlayerSettings.applicationIdentifier + "/files/Mods/" + folderName;
            }
            process.Start();
            process.WaitForExit();
            Debug.Log("Assets pushed to Android device");
        }

        public static string GetAdbPath()
        {
            return Path.Combine(EditorPrefs.GetString("AndroidSdkRoot"), "platform-tools", "adb.exe");
        }

        private static void CopyDirectory(string strSource, string strDestination, string searchPattern = "*.*")
        {
            if (!Directory.Exists(strDestination))
            {
                Directory.CreateDirectory(strDestination);
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
    }

    [JsonObject]
    [Serializable]
    public class ModBuilderProfile
    {
        public ModBuilderProfile(string n = "", string f = "", Dictionary<string, bool> g = null)
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