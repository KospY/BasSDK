using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class MusicSegment
    {
#if ODIN_INSPECTOR
        [BoxGroup("Music")] 
#endif
        public float volumeDb = 0.0f;

#if ODIN_INSPECTOR
        [BoxGroup("Music")] 
#endif
        public string musicAddress = "";

        public int timesPlayedInSerie = 1;

        [NonSerialized]
        public IResourceLocation musicLocation;

        [NonSerialized]
        public AudioClip audioClip;

    }
}