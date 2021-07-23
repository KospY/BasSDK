using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Text;
using UnityEngine.AI;

#if DUNGEN
using DunGen;
using DunGen.Adapters;
using DunGen.Graph;
using UnityEngine.Events;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class Level : MonoBehaviour
    {
        public static Level current;
        public static Level master;

        public Transform playerStart;

        public List<CustomReference> customReferences;

        [NonSerialized]
        public State state = State.None;

        [NonSerialized]
        public bool loaded;

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
            dungeon = GameObject.FindObjectOfType<Dungeon>();
        }

        [NonSerialized, ShowInInspector, ReadOnly]
        public Dungeon dungeon;

    }
}
