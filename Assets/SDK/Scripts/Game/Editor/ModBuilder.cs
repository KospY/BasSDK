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

namespace ThunderRoad
{
    public class ModBuilder : EditorWindow
    {
        public static string BuildPath
        {
            get
            {
                string buildtarget = "Windows";
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) buildtarget = "Android";
                return (Path.Combine("BuildStaging", "AddressableAssets", buildtarget, exportFolderName));
            }
        }

        public static string CatalogPath
        {
            get
            {
                return (Path.Combine("BuildStaging", "Catalog", exportFolderName));
            }
        }

        public static string projectPath;
        public static string gamePath;

        public static bool toDefault;
        public static string exportFolderName;
        public static ExportTo exportTo = ExportTo.Game;
        public static bool runGameAfterBuild;

        public static Action action = Action.BuildOnly;
        public static SupportedGame gameName = SupportedGame.BladeAndSorcery;

        public delegate void BuildEvent(EventTime eventTime);
        public static event BuildEvent OnBuildEvent;

        public enum SupportedGame
        {
            BladeAndSorcery,
#if PrivateSDK
            FutureProjectNameHere,
#endif
        }

        public enum ExportTo
        {
            Game,
            Project,
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
            EditorWindow.GetWindow<ModBuilder>("Mod Builder & Exporter");
        }

        private void OnFocus()
        {
            projectPath = EditorPrefs.GetString("TR.ProjectPath");
            gamePath = EditorPrefs.GetString("TR.GamePath");
            exportFolderName = EditorPrefs.HasKey("TR.ExportFolderName") ? EditorPrefs.GetString("TR.ExportFolderName") : "MyMod";
            exportTo = (ExportTo)EditorPrefs.GetInt("TR.ExportTo");
            toDefault = EditorPrefs.GetBool("TR.ToDefault");
            runGameAfterBuild = EditorPrefs.GetBool("TR.RunGameAfterBuild");
            gameName = (SupportedGame)EditorPrefs.GetInt("TR.GameName");
            action = (Action)EditorPrefs.GetInt("TR.Action");
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            ExportFolderGUI();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (action == Action.BuildAndExport || action == Action.BuildOnly)
            {
                GUILayout.Label(new GUIContent("Included addressable group(s)"), new GUIStyle("BoldLabel"));
                GUILayout.Space(5);

                if (AddressableAssetSettingsDefaultObject.Settings != null)
                {
                    foreach (AddressableAssetGroup group in AddressableAssetSettingsDefaultObject.Settings.groups)
                    {
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

            if (GUILayout.Button(action == Action.BuildOnly ? "Build" : (action == Action.ExportOnly ? "Export" : "Build and export"))) Build(action);
        }

        static void ExportFolderGUI()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent(toDefault ? "Default folder name" : "Mod folder name"), new GUIStyle("BoldLabel"), GUILayout.Width(150));
            string newModeName = GUILayout.TextField(exportFolderName, 25);
            if (newModeName != exportFolderName)
            {
                EditorPrefs.SetString("TR.ExportFolderName", newModeName);
                exportFolderName = newModeName;
            }

            Action newAction = (Action)EditorGUILayout.EnumPopup("", action, GUILayout.Width(150));
            if (newAction != action)
            {
                EditorPrefs.SetInt("TR.Action", (int)newAction);
                action = newAction;
            }

#if PrivateSDK
            bool newToDefault = GUILayout.Toggle(toDefault, new GUIContent("Default", "Export files and set catalog bundle paths to default folder (Warpfrog devs only!)"), GUILayout.Width(80));
            if (newToDefault != toDefault)
            {
                EditorPrefs.SetBool("TR.ToDefault", newToDefault);
                toDefault = newToDefault;
            }
#endif

            GUILayout.EndHorizontal();
        }

        static void ExportToGUI()
        {
            GUILayout.BeginHorizontal();
            ExportTo newExportTo = (ExportTo)EditorGUILayout.EnumPopup("Export to", exportTo);
            if (newExportTo != exportTo)
            {
                EditorPrefs.SetInt("TR.ExportTo", (int)newExportTo);
                exportTo = newExportTo;
            }

            EditorGUI.BeginDisabledGroup((exportTo == ExportTo.Project) ? true : false);
            bool newRunGameAfterBuild = GUILayout.Toggle(runGameAfterBuild, "Run game after build", GUILayout.Width(150));
            if (newRunGameAfterBuild != runGameAfterBuild)
            {
                EditorPrefs.SetBool("TR.RunGameAfterBuild", newRunGameAfterBuild);
                runGameAfterBuild = newRunGameAfterBuild;
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            if (exportTo == ExportTo.Android)
            {
                GUILayout.Space(5);
                SupportedGame newGameName = (SupportedGame)EditorGUILayout.EnumPopup("Game name", gameName);
                if (newGameName != gameName)
                {
                    EditorPrefs.SetInt("TR.GameName", (int)newGameName);
                    gameName = newGameName;
                }
            }

            if (exportTo == ExportTo.Game)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Game folder Directory"), new GUIStyle("BoldLabel"), GUILayout.Width(150));
                if (GUILayout.Button(gamePath, new GUIStyle("textField")))
                {
                    gamePath = EditorUtility.OpenFolderPanel("Select game folder", "", "");
                    EditorPrefs.SetString("TR.GamePath", gamePath);
                }
                GUILayout.EndHorizontal();
            }

            if (exportTo == ExportTo.Project)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent("Project folder Directory"), new GUIStyle("BoldLabel"), GUILayout.Width(150));
                if (GUILayout.Button(projectPath, new GUIStyle("textField")))
                {
                    projectPath = EditorUtility.OpenFolderPanel("Select project folder", "", "");
                    EditorPrefs.SetString("TR.ProjectPath", projectPath);
                }
                GUILayout.EndHorizontal();
            }
        }

        static void Build(Action behaviour)
        {
            // Check error
            if (exportTo == ExportTo.Project && !Directory.Exists(Path.Combine(projectPath, "Assets/StreamingAssets")))
            {
                Debug.LogError("Cannot deploy to project dir as the folder doesn't seem to be an Unity project");
                return;
            }
            if (exportTo == ExportTo.Game)
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
            if (exportTo == ExportTo.Android)
            {
                string adbPath = Path.Combine(EditorPrefs.GetString("AndroidSdkRoot"), "platform-tools", "adb.exe");
                if (!EditorPrefs.HasKey("AndroidSdkRoot") || !File.Exists(adbPath))
                {
                    Debug.LogError("Android SDK is not installed!");
                    Debug.LogError("Path not found " + adbPath);
                    return;
                }
            }

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
                        bundledAssetGroupSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.NoHash;
                        bundledAssetGroupSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.NoHash;
                        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(group.Settings.activeProfileId, "LocalBuildPath", "[ThunderRoad.ModBuilder.BuildPath]");
                        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(group.Settings.activeProfileId, "LocalLoadPath", (toDefault ? "{ThunderRoad.FileManager.aaDefaultPath}/" : "{ThunderRoad.FileManager.aaModPath}/") + exportFolderName);
                        /* TODO: OBB support (zip file uncompressed and adb push to obb folder)
                            AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(group.Settings.activeProfileId, "LocalLoadPath", "{ThunderRoad.FileManager.obbPath}/" + exportFolderName + "{ThunderRoad.FileManager.obbPathEnd}");
                        */
                    }
                }
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            // Build
            if (behaviour == Action.BuildAndExport || behaviour == Action.BuildOnly)
            {
                Debug.Log("Build path is: " + BuildPath);
                if (OnBuildEvent != null) OnBuildEvent.Invoke(EventTime.OnStart);

                // Clean build path
                if (Directory.Exists(BuildPath))
                {
                    foreach (string filePath in Directory.GetFiles(BuildPath, "*.*", SearchOption.AllDirectories)) File.Delete(filePath);
                }

                BuildCache.PurgeCache(true);
                AddressableAssetSettings.CleanPlayerContent();
                AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
                AddressableAssetSettings.BuildPlayerContent();
                Debug.Log("Build done");

                if (OnBuildEvent != null) OnBuildEvent.Invoke(EventTime.OnEnd);
            }

            // Export
            if (behaviour == Action.BuildAndExport || behaviour == Action.ExportOnly)
            {
                if (exportTo == ExportTo.Game || exportTo == ExportTo.Project)
                {
                    // Get paths
                    string buildFullPath = Path.Combine(Directory.GetCurrentDirectory(), BuildPath);
                    string catalogFullPath = Path.Combine(Directory.GetCurrentDirectory(), CatalogPath);
                    string destinationAssetsPath = "";
                    string destinationCatalogPath = "";
                    if (exportTo == ExportTo.Project)
                    {
                        destinationAssetsPath = Path.Combine(projectPath, BuildPath);
                        destinationCatalogPath = Path.Combine(projectPath, CatalogPath);
                    }
                    else if (exportTo == ExportTo.Game)
                    {
                        if (toDefault) destinationAssetsPath = destinationCatalogPath = Path.Combine(gamePath, gameName + "_Data/StreamingAssets/Default", exportFolderName);
                        else destinationAssetsPath = destinationCatalogPath = Path.Combine(gamePath, gameName + "_Data/StreamingAssets/Mods", exportFolderName);
                    }

                    // Create folders if needed
                    if (!File.Exists(destinationAssetsPath)) Directory.CreateDirectory(destinationAssetsPath);
                    if (!File.Exists(destinationCatalogPath)) Directory.CreateDirectory(destinationCatalogPath);

                    // Clean destination path
                    foreach (string filePath in Directory.GetFiles(destinationAssetsPath, "*.*", SearchOption.AllDirectories)) File.Delete(filePath);
                    foreach (string filePath in Directory.GetFiles(destinationCatalogPath, "*.*", SearchOption.AllDirectories)) File.Delete(filePath);

                    // Copy addressable assets to destination path
                    CopyDirectory(buildFullPath, destinationAssetsPath);
                    Debug.Log("Copied addressable asset folder " + buildFullPath + " to " + destinationAssetsPath);

                    // Copy json catalog to destination path
                    //CopyDirectory(catalogFullPath, destinationCatalogPath);
                    //Debug.Log("Copied catalog folder " + catalogFullPath + " to " + destinationCatalogPath);
                }

                if ((exportTo == ExportTo.Game || exportTo == ExportTo.Android) && runGameAfterBuild)
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = Path.Combine(gamePath, gameName + ".exe");
                    process.Start();
                    Debug.Log("Start game: " + process.StartInfo.FileName + " " + process.StartInfo.Arguments);
                }

                if (exportTo == ExportTo.Android)
                {
                    string buildFullPath = Path.Combine(Directory.GetCurrentDirectory(), "BuildStaging", "AddressableAssets", "Android");
                    string adbPath = Path.Combine(EditorPrefs.GetString("AndroidSdkRoot"), "platform-tools", "adb.exe");
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = adbPath;
                    string destinationPath = "/sdcard/Android/data/com.Warpfrog." + gameName + "/files/mods/" + exportFolderName;
                    process.StartInfo.Arguments = "push " + buildFullPath + "/. " + destinationPath;
                    // for default: obb : /sdcard/Android/obb/" + PlayerSettings.applicationIdentifier + "/main.1.com.Warpfrog.BladeAndSorcery.obb");
                    process.Start();
                    process.WaitForExit();
                    Debug.Log(adbPath + " " + process.StartInfo.Arguments);
                }
                Debug.Log("Export done");
            }
            // The end
            System.Media.SystemSounds.Asterisk.Play();
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
}