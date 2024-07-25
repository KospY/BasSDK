using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/AudioLoader.html")]
    [RequireComponent(typeof(AudioSource))]
    public class AudioLoader : MonoBehaviour
    {
        [Tooltip("Audio Clip Reference of the audio you want to play.\n\nEnsure that if you have an audio clip, the Audio Clip Address field is empty.")]
        public AssetReferenceT<AudioClip> audioClipReference;
        [Tooltip("Instead of using Reference, will pull from Addressables instead")]
        public bool useAudioClipAddress;
        [Tooltip("Addressables Address of Audio Clip.\n\nEnsure that if you have an address, the Audio Clip Reference is empty.")]
        public string audioClipAddress;

        [Tooltip("Will adjust volume of audio from the in-game audio settings.")]
        public AudioMixerName audioMixer;

        [NonSerialized]
        public AudioSource audioSource;
    }
}
