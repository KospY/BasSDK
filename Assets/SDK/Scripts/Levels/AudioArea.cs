using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Levels/Audio Area")]
    public class AudioArea : MonoBehaviour
    {
        public static List<AudioArea> all = new List<AudioArea>();
        public bool setAreaOnstart;
        public float volumeChangeSpeed = 0.5f;
        public List<AudioSourceParam> audioSources;

        [Serializable]
        public class AudioSourceParam
        {
            public AudioSource audioSource;
            public Vector2 minMaxVolume = new Vector2(0, 1);
            public AudioLowPassFilter audioLowPassFilter;
            public AnimationCurve cutoffFrequencyCurve;
            [NonSerialized, ShowInInspector, ReadOnly]
            public bool targetReached;

            [Button]
            public void ResetcutoffFrequencyCurve()
            {
                cutoffFrequencyCurve = new AnimationCurve(new Keyframe(0, 100f), new Keyframe(1f, 22000f, 30000f, 30000f));
            }
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnEnterPlayMode]
        static void OnEnterPlaymodeInEditor(UnityEditor.EnterPlayModeOptions options)
        {
            if (options.HasFlag(UnityEditor.EnterPlayModeOptions.DisableDomainReload))
            {
                all = new List<AudioArea>();
            }
        }
#endif

    }
}