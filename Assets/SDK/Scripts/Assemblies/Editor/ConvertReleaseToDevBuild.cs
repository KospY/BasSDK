using UnityEditor;
using UnityEngine;

namespace ThunderRoad
{
    public static class ConvertReleaseToDevBuild
    {
        [MenuItem("ThunderRoad (SDK)/Convert Release to Dev Build")]
        public static void ConvertToDev()
        {
            var gameExePath = EditorPrefs.GetString("TRAB.GameExePath");
            if (string.IsNullOrEmpty(gameExePath))
            {
                gameExePath = EditorUtility.OpenFilePanel("Select game executable", "", "exe");
                if (!AssetBundleBuilderGUI.CheckGameExe(gameExePath))
                {
                    gameExePath = null;
                }
                EditorPrefs.SetString("TRAB.GameExePath", gameExePath);
            }
            
            Debug.Log("Game exe path: " + gameExePath);
            if(string.IsNullOrEmpty(gameExePath))
            {
                Debug.LogError("Game exe path is empty");
                return;
            }
            
            //Get the path to the current unity editor exe
            var editorExePath = EditorApplication.applicationPath;
            //get the data folder beside the exe
            var editorDataPath = editorExePath.Replace("Unity.exe", "Data");
            //get the win 64 develop folder
            //\Data\PlaybackEngines\windowsstandalonesupport\Variations\win64_player_development_mono
            var devBuildPath = editorDataPath + "/PlaybackEngines/windowsstandalonesupport/Variations/win64_player_development_mono";
            Debug.Log("Dev player path: " + devBuildPath);
            
            
            // get the folder the exe is in
            var gameExeFolder = System.IO.Path.GetDirectoryName(gameExePath);
            
            //copy the WindowsPlayer.exe in the dev build to the game exe path, rename it to the gameExeName
            System.IO.File.Copy(devBuildPath + "/WindowsPlayer.exe", gameExePath, true);
            
            //copy the UnityPlayer.dll from the dev build to the game exe folder
            System.IO.File.Copy(devBuildPath + "/UnityPlayer.dll", gameExeFolder + "/UnityPlayer.dll", true);
            
            //append player-connection-debug=1 to the BladeAndSorcery_Data/boot.config in the game exe folder if it doesn't already exist
            var bootConfigPath = gameExeFolder + "/BladeAndSorcery_Data/boot.config";
            if (!System.IO.File.ReadAllText(bootConfigPath).Contains("player-connection-debug=1"))
            {
                System.IO.File.AppendAllText(bootConfigPath, "\nplayer-connection-debug=1");
            }
        }
    }
}
