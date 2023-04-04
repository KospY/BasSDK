using UnityEngine;
using UnityEngine.Audio;
using System;
using System.IO;
using UnityEngine.AddressableAssets;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class ThunderRoadSettings : ScriptableObject
    {
        const string address = "ThunderRoad.Settings";
        public static bool initialized { get; private set; }

        public static ThunderRoadSettings current
        {
            get
            {
                if (!initialized)
                {
                    ThunderRoadSettings thunderRoadSettings = null;
                    if (Application.isBatchMode)
                    {
                        // Seem that Addressables.LoadAssetAsync or op.WaitForCompletion() don't work in batch mode 
                        thunderRoadSettings = Catalog.EditorLoad<ThunderRoadSettings>(address);     
                    }
                    else
                    {
                        thunderRoadSettings = Catalog.EditorLoad<ThunderRoadSettings>(address);  
                    }

                    if (thunderRoadSettings)
                    {
                        thunderRoadSettings.Init();
                    }
                    else
                    {
                        Debug.LogError("Could not load ThunderRoad settings from address: " + address);
                        Application.Quit();
                    }
                }
                return _current;
            }
        }

        private static ThunderRoadSettings _current;

#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string addressableEditorPath = "BuildStaging/AddressableAssets";
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string catalogsEditorPath = "BuildStaging/Catalogs";
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string areaSceneAddress = "Level.Areas";
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public bool overrideData = true;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public LayerMask groundLayer = 1 << 0;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public GameSettings game;

        private void Init()
        {
            _current = this;
#if UNITY_EDITOR
            // Create folders if don't exist
            string catalogFullPath = Path.Combine(Directory.GetCurrentDirectory(), catalogsEditorPath);
            if (!Directory.Exists(catalogFullPath))
            {
                Directory.CreateDirectory(catalogFullPath);
                Debug.Log("Created folder " + catalogFullPath);
            }
            string buildFullPath = Path.Combine(Directory.GetCurrentDirectory(), addressableEditorPath);
            if (!Directory.Exists(buildFullPath))
            {
                Directory.CreateDirectory(buildFullPath);
                Debug.Log("Created folder " + buildFullPath);
            }
#endif


            initialized = true;
            
        }

    }
}