using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.Events;

#if DUNGEN
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/Level.html")]
    public class Level : ThunderBehaviour
    {
        public static Level current;
        public static Level master;
        public static bool IsDungeon => current != null && AreaManager.Instance != null;

        [Tooltip("When ticked, player will spawn.")]
        public bool spawnPlayer = true;
        [Tooltip("Container of the player when the player loads in to this level.")]
        public string playerSpawnerId = "default";

        [System.NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public LightingPreset currentLightingPreset;

        public List<CustomReference> customReferences;

        [NonSerialized]
        public State state = State.None;

        [NonSerialized, ShowInInspector]
        public LevelData.Mode.PlayerDeathAction currentPlayerDeathAction = LevelData.Mode.PlayerDeathAction.None;

        [NonSerialized]
        public bool loaded;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        public static int seed
        {
            get
            {
                return _seed;
            }
            set
            {
                _seed = value;
                UnityEngine.Random.InitState(_seed);
            }
        }

        private static int _seed;

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

        public static void GenerateNewSeed()
        {
            seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }

    }
}
