using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor;

using EasyButtons;

namespace ThunderRoad
{
    public class LevelDefinition : MonoBehaviour
    {
        public static LevelDefinition current;
        public Transform playerStart;
        public List<CustomReference> customReferences;
        public bool loadDefaultCharIfNeeded = true;

        [NonSerialized]
        public AudioSource music;
        [NonSerialized]
        public bool initialized;

        [Serializable]
        public class CustomReference
        {
            public string name;
            public List<Transform> transforms;
        }

        [Button]
        public static void CheckLightMapMode()
        {
            Debug.Log("Lightmap mode: " + LightmapSettings.lightmapsMode);
        }

    }
}
