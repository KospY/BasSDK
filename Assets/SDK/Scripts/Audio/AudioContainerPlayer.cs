using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class AudioContainerPlayer : MonoBehaviour
    {
        public AudioContainer audioContainer;
        public bool playOnAwake;

        [NonSerialized]
        public AudioSource audioSource;

        public AudioMixerName audioMixer = AudioMixerName.Effect;

        public bool isPlaying
        {
            get { return audioSource.isPlaying; }
        }
    }
}