using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using System.IO;

namespace ThunderRoad
{
    public class AssetBuilder : EditorWindow
    {
        public static string BuildPath
        {
            get
            {
                return Addressables.BuildPath;
            }
        }

        public static string projectPath;
        public static string gamePath;

        public static bool toDefault;
        public static string modeName;
        public static ExportTo exportTo = ExportTo.Game;
        public static bool runGameAfterBuild;

        public delegate void BuildEvent(EventTime eventTime);
        public static event BuildEvent OnBuildEvent;

        public enum ExportTo
        {
            Game,
            Project,
            Android,
        }

        [MenuItem("ThunderRoad (SDK)/Asset Builder")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<AssetBuilder>("Asset Builder");
        }

        private void OnFocus()
        {
            projectPath = EditorPrefs.GetString("AB.ProjectPath");
            gamePath = EditorPrefs.GetString("AB.GamePath");
            modeName = EditorPrefs.GetString("AB.ModeName");
            exportTo = (ExportTo)EditorPrefs.GetInt("AB.ExportTo");
            toDefault = EditorPrefs.GetBool("AB.ToDefault");
            runGameAfterBuild = EditorPrefs.GetBool("AB.RunGameAfterBuild");
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.Label(new GUIContent("Game folder Directory"), new GUIStyle("BoldLabel"));
            GUILayout.Space(5);
            if (GUILayout.Button(gamePath, new GUIStyle("textField")))
            {
                gamePath = EditorUtility.OpenFolderPanel("Select game folder", "", "");
                EditorPrefs.SetString("AB.GamePath", gamePath);
            }
            GUILayout.Label(new GUIContent("Project folder Directory"), new GUIStyle("BoldLabel"));
            GUILayout.Space(5);
            if (GUILayout.Button(projectPath, new GUIStyle("textField")))
            {
                projectPath = EditorUtility.OpenFolderPanel("Select project folder", "", "");
                EditorPrefs.SetString("AB.ProjectPath", projectPath);
            }

            GUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
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

            GUILayout.Space(10);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            ExportTo newExportTo = (ExportTo)EditorGUILayout.EnumPopup("Export to", exportTo);
            if (newExportTo != exportTo)
            {
                EditorPrefs.SetInt("AB.ExportTo", (int)newExportTo);
                exportTo = newExportTo;
            }

            if (exportTo == ExportTo.Android && EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                GUIStyle style = new GUIStyle(EditorStyles.label);
                style.normal.textColor = Color.red;
                GUILayout.Label(new GUIContent("Current platefrom is not Android, please switch to Android in the build settings"), style);
            }
            else if (exportTo == ExportTo.Game && EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                GUIStyle style = new GUIStyle(EditorStyles.label);
                style.normal.textColor = Color.red;
                GUILayout.Label(new GUIContent("Current platefrom is not Windows, please switch to Windows in the build settings"), style);
            }

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

            GUILayout.Label(new GUIContent("Mod name"), new GUIStyle("BoldLabel"), GUILayout.Width(100));
            if (!toDefault)
            {
                string newModeName = GUILayout.TextField(modeName, 25);
                if (newModeName != modeName)
                {
                    EditorPrefs.SetString("AB.ModeName", newModeName);
                    modeName = newModeName;
                }
            }

            bool newToDefault = GUILayout.Toggle(toDefault, "Default", GUILayout.Width(100));
            if (newToDefault != toDefault)
            {
                EditorPrefs.SetBool("AB.ToDefault", newToDefault);
                toDefault = newToDefault;
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            EditorGUI.BeginDisabledGroup(exportTo == ExportTo.Project ? true : false);
            bool newRunGameAfterBuild = GUILayout.Toggle(runGameAfterBuild, "Run game after build");
            if (newRunGameAfterBuild != runGameAfterBuild)
            {
                EditorPrefs.SetBool("AB.RunGameAfterBuild", newRunGameAfterBuild);
                runGameAfterBuild = newRunGameAfterBuild;
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(5);

            if (GUILayout.Button("Build package")) BuildPackage();
        }

        static void BuildPackage()
        {
            if (exportTo == ExportTo.Android && EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                Debug.LogError("Cannot deploy to android device as the plateform selected is not android");
                return;
            }
            else if (exportTo == ExportTo.Game && EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                Debug.LogError("Cannot deploy to Windows as the plateform selected is Android");
                return;
            }

            if (exportTo == ExportTo.Project && !Directory.Exists(Path.Combine(projectPath, "Assets/StreamingAssets")))
            {
                Debug.LogError("Cannot deploy to project dir as the folder doesn't seem to be an Unity project");
                return;
            }
            if (exportTo == ExportTo.Game && !Directory.Exists(Path.Combine(gamePath, "BladeAndSorcery_Data/StreamingAssets")))
            {
                Debug.LogError("Cannot deploy to game dir as the folder doesn't seem to be a B&S game folder");
                return;
            }

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                PlayerSettings.stereoRenderingPath = StereoRenderingPath.SinglePass;
            }
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
            {
                PlayerSettings.stereoRenderingPath = StereoRenderingPath.Instancing;
            }

            if (AddressableAssetSettingsDefaultObject.Settings != null)
            {
                foreach (AddressableAssetGroup group in AddressableAssetSettingsDefaultObject.Settings.groups)
                {
                    BundledAssetGroupSchema bundledAssetGroupSchema = group.GetSchema<BundledAssetGroupSchema>();
                    if (bundledAssetGroupSchema != null)
                    {
                        bundledAssetGroupSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.NoHash;
                        bundledAssetGroupSchema.BundleNaming = BundledAssetGroupSchema.BundleNamingStyle.NoHash;
                        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(group.Settings.activeProfileId, "LocalBuildPath", "[ThunderRoad.AssetBuilder.BuildPath]");
                        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(group.Settings.activeProfileId, "LocalLoadPath", toDefault ? ("{ThunderRoad.Catalog.AaPath}") : ("{ThunderRoad.Catalog.ModPath}/" + modeName));
                    }
                }
            }

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

            if (OnBuildEvent != null) OnBuildEvent.Invoke(EventTime.OnStart);

            BuildCache.PurgeCache(true);
            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
            AddressableAssetSettings.BuildPlayerContent();
            
            string buildPath = Path.Combine(Directory.GetCurrentDirectory(), Addressables.BuildPath);

            string catalogPath = Path.Combine(buildPath, "catalog.json");
            if (toDefault && File.Exists(catalogPath)) File.Move(catalogPath, Path.Combine(buildPath, "catalog_assets.json"));

            string linkPath = Path.Combine(buildPath, "link.xml");
            if (File.Exists(linkPath)) File.Delete(linkPath);

            string settingsPath = Path.Combine(buildPath, "settings.json");
            if (File.Exists(settingsPath)) File.Delete(settingsPath);

            string destinationPath = "";
            string plateform = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ? "Android" : "Windows";

            if (exportTo == ExportTo.Project)
            {
                if (toDefault) destinationPath = Path.Combine(projectPath, "Library/com.unity.addressables/StreamingAssetsCopy/aa/" + plateform);
                else destinationPath = Path.Combine(Path.Combine(projectPath, "Assets/StreamingAssets/Mods"), modeName);
            }
            else if (exportTo == ExportTo.Game)
            {
                if (toDefault) destinationPath = Path.Combine(gamePath, "BladeAndSorcery_Data/StreamingAssets/aa/" + plateform);
                else destinationPath = Path.Combine(Path.Combine(gamePath, "BladeAndSorcery_Data/StreamingAssets/Mods"), modeName);
            }

            if (!File.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);
            foreach (string filePath in Directory.GetFiles(buildPath))
            {
                Debug.Log("Copy file [" + filePath + "] to " + Path.Combine(destinationPath, Path.GetFileName(filePath)));
                File.Copy(filePath, Path.Combine(destinationPath, Path.GetFileName(filePath)), true);
            }
            if (OnBuildEvent != null) OnBuildEvent.Invoke(EventTime.OnEnd);
            System.Media.SystemSounds.Asterisk.Play();
        }
    }
}