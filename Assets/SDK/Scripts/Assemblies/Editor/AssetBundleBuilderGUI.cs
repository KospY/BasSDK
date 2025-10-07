using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using System.IO;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace ThunderRoad
{
    public class AssetBundleBuilderGUI : EditorWindow
    {
        public enum ExportMode { ToGame, ToFolder }
        public static List<AssetBundleGroup> assetBundleGroups = new List<AssetBundleGroup>();

        public static string gameExePath;

        public static bool clearCache;
        public static bool runGameAfterBuild;
        public static string runGameArguments;
        public static bool cleanDestination = true;
        private Vector2 scrollPos;
        
        [MenuItem("ThunderRoad (SDK)/Asset Bundle Builder")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<AssetBundleBuilderGUI>("Asset Bundle Builder");
        }

        private void OnFocus()
        {
            gameExePath = EditorPrefs.GetString("TRAB.GameExePath");
            clearCache = EditorPrefs.GetBool("TRAB.ClearCache");
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
            GUILayout.Label(new GUIContent($"AssetBundle builder ({EditorUserBuildSettings.activeBuildTarget})"), new GUIStyle("BoldLabel"));
            GUILayout.Space(5);

            using (new EditorGUILayout.HorizontalScope(GUILayout.Width(500)))
            { 
                if (GUILayout.Button("Create New Asset Group"))
                    {
                        //show a dialog to ask the name of the new group
                        if (!EditorInputDialog.Show("Create new Asset Bundle Group", "Please type a name for the new Asset Bundle Group", "NewMod", "OK", "Cancel", out string assetName))
                        {
                            return;
                        }

//ProjectCore
                    string bundleGroupPath = "Assets/Personal/AssetBundleGroups";
 //ProjectCore
                        //remove spaces from the name and trim it
                        assetName = assetName.Trim();
                        assetName = assetName.Replace(" ", "");
                        string bundleGroupFile = $"{bundleGroupPath}/{assetName}.asset";
                        if (File.Exists(bundleGroupFile))
                        {
                            EditorUtility.DisplayDialog($"Bundle already exists", $"An Asset Bundle Group with the name {assetName} already exists. Please choose a different name ", "Ok");
                            Debug.LogWarning("You already have a new asset group");
                            return;
                        }
                        //ensure the path exists
                        if (!Directory.Exists(bundleGroupPath)) Directory.CreateDirectory(bundleGroupPath);
                        AssetBundleGroup newGroup = CreateInstance<AssetBundleGroup>();
                        newGroup.folderName = assetName;
                        newGroup.addressableAssetGroups = new List<AddressableAssetGroup>();

                        AssetDatabase.CreateAsset(newGroup, bundleGroupFile);
                        assetBundleGroups.Add(newGroup);
                    }
                if (GUILayout.Button("Open Addressables Group Window"))
                {
                    OpenAddressablesGroupsWindow();
                }
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(5);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            int selectedCount = 0;
            foreach (AssetBundleGroup assetBundleGroup in assetBundleGroups)
            {
                bool dirty = false;
                EditorGUILayout.BeginHorizontal();
                {
                    var prevSelected = assetBundleGroup.selected;
                    assetBundleGroup.selected = EditorGUILayout.Toggle(assetBundleGroup.selected, GUILayout.MaxWidth(20));
                    if (prevSelected != assetBundleGroup.selected) EditorUtility.SetDirty(assetBundleGroup);
                    
                    if (assetBundleGroup.selected) selectedCount++;
                    if (GUILayout.Button($"{assetBundleGroup.name}", GUILayout.Width(120)))
                    {
                        //open the asset bundle group file in a new inspector window
                        EditorUtility.OpenPropertyEditor(assetBundleGroup);
                    }
                    var prevexportAfterBuild = assetBundleGroup.exportAfterBuild;
                    var prevExportMode = assetBundleGroup.exportMode;
                    EditorGUILayout.LabelField("Export After Build", GUILayout.Width(100));
                    assetBundleGroup.exportAfterBuild = EditorGUILayout.Toggle(assetBundleGroup.exportAfterBuild, GUILayout.MaxWidth(20));
                    assetBundleGroup.exportMode = (ExportMode)EditorGUILayout.Popup((int)assetBundleGroup.exportMode, Enum.GetNames(typeof(ExportMode)), GUILayout.Width(100));
                    if (prevexportAfterBuild != assetBundleGroup.exportAfterBuild || prevExportMode != assetBundleGroup.exportMode) EditorUtility.SetDirty(assetBundleGroup);
                    if (GUILayout.Button("Export now", GUILayout.Width(80)))
                    {
                        AssetBundleBuilder.exportFolderName = assetBundleGroup.folderName;
                        Export(assetBundleGroup, assetBundleGroup.exportMode == ExportMode.ToFolder);
                    }
                    string buildTarget = "Windows";
                    if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) buildTarget = "Android";
                    if(GUILayout.Button($"Open {buildTarget} Bundle Folder", GUILayout.Width(180)))
                    {
                        //temp set the export folder name
                        AssetBundleBuilder.exportFolderName = assetBundleGroup.folderName;
                        var path = AssetBundleBuilder.assetsLocalPath;
                        //make sure teh folder exists
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        EditorUtility.RevealInFinder(path);
                    }
                    if(assetBundleGroup.isMod && GUILayout.Button($"Open Catalog Folder", GUILayout.Width(160)))
                    {
                        assetBundleGroup.OpenCatalogFolder();
                    }
                }
               
                AssetDatabase.SaveAssetIfDirty(assetBundleGroup);
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();
            {
                using (new EditorGUILayout.VerticalScope( GUILayout.Width(200)))
                {
                    //label that says to set the build mode
                    GUILayout.Label($"[{EditorUserBuildSettings.activeBuildTarget}] Set build mode");
                    if (GUILayout.Button("Windows"))
                    {
                        SDKTools.SetWindowsQualityAndPlatform();
                    }
                    if (GUILayout.Button("Android"))
                    {
                        SDKTools.SetAndroidQualityAndPlatform();
                    }
                }
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(200)))
                {
                    if (selectedCount == 0) EditorGUI.BeginDisabledGroup(true);
                    GUILayout.Label("Build asset bundle group(s) selected");
                    if (GUILayout.Button("Build"))
                    {
                        EditorApplication.delayCall += BuildSelected;
                    }
                    EditorGUI.EndDisabledGroup();
                    clearCache = GUILayout.Toggle(clearCache, new GUIContent("Clear build cache", ""));
                    EditorPrefs.SetBool("TRAB.ClearCache", clearCache);

                }
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

#if UNITY_EDITOR
        [MenuItem("ThunderRoad (SDK)/Addressables Groups")]
#endif
        protected static void OpenAddressablesGroupsWindow()
        {
            //open the menu, but unfortunatly doesnt redock it on the last place it was
            EditorApplication.ExecuteMenuItem("Window/Asset Management/Addressables/Groups");
        }
        
        protected static void CloseAddressablesGroupsWindow()
        {
            var window = EditorWindow.GetWindow(typeof(EditorWindow), false, "Addressables Groups");
            if (window.titleContent.text == "Addressables Groups") window.Close();
        }

        protected static void BuildSelected()
        {
            EditorApplication.delayCall -= BuildSelected;
            bool reopenLastScene = false;
            
            //save reference to current scene path
            string scenePath = null;
            
            if (!Application.isBatchMode)
            {
                Scene activeScene = EditorSceneManager.GetActiveScene();
                //prompt user to save the current scene if modified
                if (activeScene.isDirty)
                {
                    // Prompt the user with a warning
                    bool userResponse = EditorUtility.DisplayDialog("Warning - Unsaved scene", "Do you want to save the scene and continue build?", "Save and continue", "Abort build");

                    if (!userResponse)
                    {
                        //user clicked cancel
                        Debug.Log("Aborting build, please save your changes before building as the scene needs to be closed");
                        return;
                    }
                    
                    //save the scene
                    if (!EditorSceneManager.SaveScene(activeScene))
                    {
                        Debug.LogWarning("Failed to save current modified scene, aborting build. Please save your changes.");
                        return;
                    }
                }
                //update the active scene since its a struct, the path could have changed on save
                scenePath = EditorSceneManager.GetActiveScene().path;
                reopenLastScene = true;
            }
   
            
            // Open a new scene
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Single);

            EditorUtility.UnloadUnusedAssetsImmediate(); // https://issuetracker.unity3d.com/issues/addressables-very-slow-build-when-editor-heap-memory-is-full
            GC.Collect();
            
            if (Application.isBatchMode)
            {
                CloseAddressablesGroupsWindow(); // https://forum.unity.com/threads/buildplayercontent-calculate-asset-dependency-data-takes-forever.1015951/
            }

            foreach (AssetBundleGroup assetBundleGroup in assetBundleGroups)
            {
                if (assetBundleGroup.selected)
                {
                    assetBundleGroup.OnValidate();
//warnings for the SDK
                    //check if the group selected is a mod, as modders should be checking isMod
                    if (Application.isEditor && !Application.isBatchMode)
                    {
                        //Check its marked as a mod
                        if (!assetBundleGroup.isMod)
                        {
                            //popup warning
                            bool userResponse = EditorUtility.DisplayDialog("Warning", $"You are trying to build a bundle is not a mod bundle.\nIf this sounds incorrect please check 'Is Mod' on the Asset Bundle Group: {assetBundleGroup.name}\nDo you want to continue building?", "Yes", "No");
                            if (!userResponse)
                            {
                                Debug.Log($"Aborting build, please check the 'Is Mod' on the Asset Bundle Group: {assetBundleGroup.name}");
                                return;
                            }
                        }
                        //Check the bundles entries have the correct labels
                        if (!assetBundleGroup.CheckAddressableLabels(out string message))
                        {
                            bool userResponse = EditorUtility.DisplayDialog("Warning", $"Entries are missing Windows/Android labels.\nThey will not build correctly\n\n{message}\n\nDo you want to continue building?", "Yes", "No");
                            if (!userResponse)
                            {
                                Debug.Log($"Aborting build, please check the labels on the Asset Bundle Group: {assetBundleGroup.name}");
                                return;
                            }
                        }
                    }
                    
                    Build(assetBundleGroup);
                }
            }
            
            if (!Application.isBatchMode && reopenLastScene)
            {
                if (string.IsNullOrEmpty(scenePath))
                {
                    Debug.LogWarning($"Last scene path is empty, but we were expected to reopen the last scene. Unable to open last scene.");
                }
                else
                {
                    // Reopen the original scene
                    EditorSceneManager.OpenScene(scenePath);
                }
            }
            
            if (runGameAfterBuild)
            {
                RunGame();
            }

            // The end
            System.Media.SystemSounds.Asterisk.Play();
        }

        protected static void RunGame()
        {
            if ((EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows) && runGameAfterBuild)
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = gameExePath;
                process.StartInfo.Arguments = runGameArguments;
                process.Start();
                Debug.Log($"Start game: {process.StartInfo.FileName} {process.StartInfo.Arguments}");
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                AdbHelper adb = new AdbHelper(GetAdbPath());
                Debug.Log($"Starting game.");
                adb.StartAppAsync(ThunderRoadSettings.current.game.appIdentifier, "com.unity3d.player.UnityPlayerActivity", runGameArguments);
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
                AdbHelper adb = new AdbHelper(GetAdbPath());
                Debug.Log($"Stopping game");
                adb.StopAppAsync(ThunderRoadSettings.current.game.appIdentifier);
            }
        }

        public static bool CheckGameExe(string path)
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
            AssetBundleBuilder.Build(assetBundleGroup, clearCache);

            if (assetBundleGroup.exportAfterBuild)
            {
                Export(assetBundleGroup);
            }
        }

        public static void Export(AssetBundleGroup assetBundleGroup, bool toFolder = false)
        {
            string assetsFullPath = Path.Combine(Directory.GetCurrentDirectory(), AssetBundleBuilder.assetsLocalPath);
            string folderName = assetBundleGroup.isDefault ? FileManager.defaultFolderName : FileManager.modsFolderName;
            string catalogFullPath = Path.Combine(Directory.GetCurrentDirectory(), ThunderRoadSettings.current.catalogsEditorPath, folderName, AssetBundleBuilder.exportFolderName);
            
            if (toFolder)
            {
                //dialog box to select the folder
                string manualFolderPath = EditorUtility.OpenFolderPanel($"Select folder to export {assetBundleGroup.name}", "", "");
                if (string.IsNullOrEmpty(manualFolderPath))
                {
                    Debug.LogWarning("No folder selected, export cancelled");
                    return;
                }
                //append  AssetBundleBuilder.exportFolderName to the path
                manualFolderPath = Path.Combine(manualFolderPath, AssetBundleBuilder.exportFolderName);
                // Create folders if needed
                if (!File.Exists(manualFolderPath)) Directory.CreateDirectory(manualFolderPath);

                // Clean destination path
                if (cleanDestination)
                {
                    foreach (string filePath in Directory.GetFiles(manualFolderPath, "*.*", SearchOption.AllDirectories)) File.Delete(filePath);
                }
                else
                {
                    foreach (string filePath in Directory.GetFiles(manualFolderPath, "catalog_*.json", SearchOption.AllDirectories)) File.Delete(filePath);
                    foreach (string filePath in Directory.GetFiles(manualFolderPath, "catalog_*.hash", SearchOption.AllDirectories)) File.Delete(filePath);
                }

                // Copy addressable assets to destination path
                AssetBundleBuilder.CopyDirectory(assetsFullPath, manualFolderPath);
                Debug.Log($"Copied addressable asset folder {assetsFullPath} to {manualFolderPath}");

                if (Directory.Exists(catalogFullPath))
                {
                    AssetBundleBuilder.CopyDirectory(catalogFullPath, manualFolderPath);
                    Debug.Log($"Copied json folder {catalogFullPath} to {manualFolderPath}");
                }

                if (assetBundleGroup.isMod)
                {
                    string manifestTempFolderPath = GenerateManifest(assetBundleGroup);
                    AssetBundleBuilder.CopyDirectory(manifestTempFolderPath, manualFolderPath);
                    Debug.Log($"Copied manifest {manifestTempFolderPath} to {manualFolderPath}");
                }
                //open the folder when done
                EditorUtility.RevealInFinder(manualFolderPath);
            } 
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
            {
                string destinationAssetsPath = "";
                string destinationCatalogPath = "";
                
                if (string.IsNullOrEmpty(gameExePath))
                {
                    Debug.LogError("Game executable can't be found");
                    //dialog popup
                    EditorUtility.DisplayDialog("Error", "Game executable path is not set or invalid. Please set it in the Asset Bundle Builder window.", "OK");
                    return;
                }
                var exeDirectory = Path.GetDirectoryName(gameExePath);
                
                if (assetBundleGroup.isDefault) destinationAssetsPath = destinationCatalogPath = Path.Combine(exeDirectory, $"{Path.GetFileNameWithoutExtension(gameExePath)}_Data/StreamingAssets/Default");
                if (assetBundleGroup.isMod) destinationAssetsPath = destinationCatalogPath = Path.Combine(exeDirectory, $"{Path.GetFileNameWithoutExtension(gameExePath)}_Data/StreamingAssets/Mods", AssetBundleBuilder.exportFolderName);
            
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
                Debug.Log($"Copied addressable asset folder {assetsFullPath} to {destinationAssetsPath}");

                if (Directory.Exists(catalogFullPath))
                {
                    AssetBundleBuilder.CopyDirectory(catalogFullPath, destinationAssetsPath);
                    Debug.Log($"Copied json folder {catalogFullPath} to {destinationAssetsPath}");
                }

                if (assetBundleGroup.isMod)
                {
                    string manifestTempFolderPath = GenerateManifest(assetBundleGroup);
                    AssetBundleBuilder.CopyDirectory(manifestTempFolderPath, destinationAssetsPath);
                    Debug.Log($"Copied manifest {manifestTempFolderPath} to {destinationAssetsPath}");
                }
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                string adbPath = Path.Combine(EditorPrefs.GetString("AndroidSdkRoot"), "platform-tools", "adb.exe");
                if (!EditorPrefs.HasKey("AndroidSdkRoot") || !File.Exists(adbPath))
                {
                    Debug.LogError("Android SDK is not installed!");
                    Debug.LogError($"Path not found {adbPath}");
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
                    if (assetBundleGroup.isMod)
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
            modData.Name = assetBundleGroup.folderName;
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
            AdbHelper adb = new AdbHelper(GetAdbPath());
            Debug.Log($"Connected Android devices:");
            adb.ListDevicesAsync();
        }

        public static void AndroidPushAssets(string sourcePath, string modFolderName = null)
        {
            AdbHelper adb = new AdbHelper(GetAdbPath());
            string source = $"{sourcePath}\\.";
            string destination =  $"/sdcard/Android/obb/{ThunderRoadSettings.current.game.appIdentifier}";
            
            if (!string.IsNullOrEmpty(modFolderName))
            {
                destination = $"/sdcard/Android/data/{ThunderRoadSettings.current.game.appIdentifier}/files/Mods/{modFolderName}";
            }
            Debug.Log($"Pushing: {source} to {destination}");
            adb.PushAsync(source, destination);
        }

        public static string GetAdbPath()
        {
            return Path.Combine(GetAndroidSDKPath(), "platform-tools", "adb.exe");
        }

        // From OVRConfig.cs
        // Returns the path to the base directory of the Android SDK
        public static string GetAndroidSDKPath(bool throwError = true)
        {
            string androidSDKPath = "";
    #if UNITY_2019_1_OR_NEWER
            // Check for use of embedded path or user defined
            bool useEmbedded = EditorPrefs.GetBool("SdkUseEmbedded") || string.IsNullOrEmpty(EditorPrefs.GetString("AndroidSdkRoot"));
            if (useEmbedded)
            {
                androidSDKPath = Path.Combine(BuildPipeline.GetPlaybackEngineDirectory(BuildTarget.Android, BuildOptions.None), "SDK");
            }
            else
    #endif
            {
                androidSDKPath = EditorPrefs.GetString("AndroidSdkRoot");
            }

            androidSDKPath = androidSDKPath.Replace("/", "\\");
            // Validate sdk path and notify user if path does not exist.
            if (!Directory.Exists(androidSDKPath))
            {
                androidSDKPath = Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
                if (!string.IsNullOrEmpty(androidSDKPath))
                {
                    return androidSDKPath;
                }

                if (throwError)
                {
                    EditorUtility.DisplayDialog("Android SDK not Found",
                            "Android SDK not found. Please ensure that the path is set correctly in (Edit -> Preferences -> External Tools) or that the Untiy Android module is installed correctly.",
                            "Ok");
                }
                return string.Empty;
            }

            return androidSDKPath;
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