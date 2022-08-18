using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.InputSystem.Controls;

#if DUNGEN
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Level")]
    public class Level : ThunderBehaviour
    {
        public static Level current;
        public static Level master;

        public bool spawnPlayer = true;
#if UNITY_EDITOR
        public bool forceLinearFog = true;
#endif

        public string playerSpawnerId = "default";


        public List<CustomReference> customReferences;

        [NonSerialized]
        public State state = State.None;

        [NonSerialized]
        public bool loaded;

        [NonSerialized]
        public Color originalFogColor;
        [NonSerialized]
        public Color originalShadowColor;

        [Serializable]
        public class CustomReference
        {
            public string name;
            public List<Transform> transforms;
        }

        public enum State
        {
            None,
            Failure,
            Success,
        }

        public UnityEvent loadedEvent;

#if UNITY_EDITOR
       private void OnValidate () 
       {
            UnityEditor.EditorPrefs.SetBool("TRAB.ForceLinearFog", forceLinearFog);
       }
#endif

#if PrivateSDK
        [Button]
        public static void CheckLightMapMode()
        {
            Debug.Log("Lightmap mode: " + LightmapSettings.lightmapsMode);
        }

        [Button]
        public static void ChangeLightMapMode(LightmapsMode lightmapsMode)
        {
            LightmapSettings.lightmapsMode = lightmapsMode;
            Debug.Log("Lightmap mode set to: " + LightmapSettings.lightmapsMode);
        }

        [Button]
        public static void TetrahedralizeLightProbes()
        {
            // Fix light probes being wrong!!!
            LightProbes.Tetrahedralize();
        }

        protected virtual void Awake()
        {
            if (gameObject.scene.name.ToLower() == "master")
            {
                master = this;
                return;
            }
            else
            {
                current = this;
            }
            originalFogColor = RenderSettings.fogColor;
            originalShadowColor = RenderSettings.subtractiveShadowColor;
            dungeon = GameObject.FindObjectOfType<Dungeon>();
        }

        [NonSerialized, ShowInInspector, ReadOnly]
        public Dungeon dungeon;
#endif

    }
}
