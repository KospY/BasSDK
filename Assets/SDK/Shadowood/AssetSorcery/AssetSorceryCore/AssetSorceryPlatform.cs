using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace ThunderRoad.AssetSorcery
{
    public static class AssetSorceryPlatform
    {
        public static AssetSorceryPlatformRuntime.ePlatformAS AssetSorceryGetPlatform()
        {
            var pref = PlayerPrefs.GetInt(_ASSETSORCERY_PLATFORM, 0);
            //Debug.Log("AssetSorceryGetPlatform: " + pref);
            return (AssetSorceryPlatformRuntime.ePlatformAS) Enum.Parse(typeof(AssetSorceryPlatformRuntime.ePlatformAS), pref.ToString());
        }


        public static AssetSorceryPlatformRuntime.ePlatformAS AssetSorceryGetPlatformCalculated()
        {
            var platform = AssetSorceryGetPlatform();
            if (platform == AssetSorceryPlatformRuntime.ePlatformAS.Auto)
            {
                platform = AssetSorceryPlatformRuntime.AssetSorceryGetBuildPlatform(); // If pref is empty/auto then get the current selected build platform
                AssetSorceryShaderSetPlatform(platform); // Set the pref to the detected platform, reimport if needed
            }

            return platform;
        }


        public const string _ASSETSORCERY_PLATFORM = "_ASSETSORCERY_PLATFORM_V2";

        public const string _ASSETSORCERY_EDITORMODE = "_ASSETSORCERY_EDITORMODE_V2";

        public static void SetEditorMode(bool editorMode, bool refreshFiles = true)
        {
            if (GetEditorMode() != editorMode)
            {
                PlayerPrefs.SetInt(_ASSETSORCERY_EDITORMODE, editorMode ? 1 : 0);
                if (refreshFiles) ReloadAll(AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION);
            }

            PlayerPrefs.SetInt(_ASSETSORCERY_EDITORMODE, editorMode ? 1 : 0);
        }

        public static bool GetEditorMode()
        {
            return PlayerPrefs.GetInt(_ASSETSORCERY_EDITORMODE, 1) == 1;
        }

        public static void AssetSorceryShaderSetPlatform(AssetSorceryPlatformRuntime.ePlatformAS valIn)
        {
            // If request platform doesnt match the user pref then Reload assets, unless were in batchmode, then force a reimport
            if (valIn != AssetSorceryGetPlatform() || Application.isBatchMode)
            {
                Debug.Log($"AssetSorceryShaderSetPlatform: platform changed: {valIn}");
                PlayerPrefs.SetInt(_ASSETSORCERY_PLATFORM, (int) valIn);

                //TODO could import only if last imported platform has changed
                //TODO skip unused shaders

                var stopwatch = new Stopwatch(); // stopwatch to record how long it takes to reload all assets
                stopwatch.Start();
                ReloadAll(AssetSorceryArray.ASSETSORCERY_FILE_EXTENSION);
                ReloadAll(AssetSorceryShader.ASSETSORCERY_FILE_EXTENSION);
                stopwatch.Stop(); //log how long it took to reload all assets
                Debug.Log($"AssetSorceryShaderSetPlatform: ReloadAll took {stopwatch.ElapsedMilliseconds}ms");
            }
            else
            {
                Debug.Log("AssetSorceryShaderSetPlatform: platform is the same: " + valIn);
            }
        }

        public static void AssetSorceryShaderSetPlatformLocal(AssetSorceryPlatformRuntime.ePlatformAS valIn, string path)
        {
            var cur = PlayerPrefs.GetInt(_ASSETSORCERY_PLATFORM);
            PlayerPrefs.SetInt(_ASSETSORCERY_PLATFORM, (int) valIn);
            AssetDatabase.ImportAsset(path); //, ImportAssetOptions.ForceSynchronousImport);
            PlayerPrefs.SetInt(_ASSETSORCERY_PLATFORM, cur);
        }

        public static void ReloadAll(string extension)
        {
            var guids = AssetDatabase.FindAssets(extension);
            Debug.Log($"ReloadAll: {extension}:{guids.Length}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetExtension(path) != "." + extension) continue; // FindAssets cannot search by extension! so this double checks
                AssetDatabase.ImportAsset(path); //, ImportAssetOptions.ForceSynchronousImport);
            }
        }

        public static void DrawPlatformButtons(Object target)
        {
            var editorMode = AssetSorceryPlatform.GetEditorMode();

            GUI.color = editorMode ? Color.green : Color.red;
            if (editorMode ? GUILayout.Button($"Set Editor Mode Off") : GUILayout.Button($"Set Editor Mode On"))
            {
                AssetSorceryPlatform.SetEditorMode(!editorMode, true);
            }
            GUI.color = Color.white;
            
            foreach (AssetSorceryPlatformRuntime.ePlatformAS s in Enum.GetValues(typeof(AssetSorceryPlatformRuntime.ePlatformAS)))
            {
                if (GUILayout.Button($"Set Platform Global: {s}"))
                {
                    AssetSorceryShaderSetPlatform(s);
                }
            }

            GUILayout.Space(10);

            foreach (AssetSorceryPlatformRuntime.ePlatformAS s in Enum.GetValues(typeof(AssetSorceryPlatformRuntime.ePlatformAS)))
            {
                if (GUILayout.Button($"Set Platform Local: {s}"))
                {
                    AssetSorceryShaderSetPlatformLocal(s, AssetDatabase.GetAssetPath(target));
                    /*
                    var cur = PlayerPrefs.GetInt(_ASSETSORCERY_PLATFORM);

                    PlayerPrefs.SetInt(_ASSETSORCERY_PLATFORM, (int) s);
                    var path = AssetDatabase.GetAssetPath(target);
                    AssetDatabase.ImportAsset(path);

                    PlayerPrefs.SetInt(_ASSETSORCERY_PLATFORM, cur);*/
                }
            }
        }
    }
}
