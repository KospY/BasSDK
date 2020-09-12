using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
        public Transform playerStart;
        public List<CustomReference> customReferences;

        [NonSerialized]
        public State state = State.None;
        [NonSerialized]
        public AudioSource music;
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

        [Button]
        public static void CheckLightMapMode()
        {
            Debug.Log("Lightmap mode: " + LightmapSettings.lightmapsMode);
        }

        [Button]
        public static void TetrahedralizeLightProbes()
        {
            // Fix light probes being wrong!!!
            LightProbes.Tetrahedralize();
        }

    }
}
