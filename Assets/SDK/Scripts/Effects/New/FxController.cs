using System.Collections.Generic;
using UnityEngine;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Effects/FxController.html")]
    public class FxController : MonoBehaviour
    {
        [Header("Variables")]
        [Range(0, 1)]
        public float intensity;
        [Range(0, 1)]
        public float speed;
        public Vector3 direction;

        [Header("Options")]
        public bool playOnStart = false;
        public float lifeTime = 0;

        [Header("Detected Modules")]
        public List<FxModule> modules;

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public object source;

        public event Action onLifetimeExpired;

        protected bool initialized;

        [NonSerialized]
        public bool isPlaying;

    }
}
