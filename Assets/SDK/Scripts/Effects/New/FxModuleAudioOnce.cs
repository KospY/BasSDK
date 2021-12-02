using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class FxModuleAudioOnce : FxModule
    {
        [Header("Audio")]
        public string audioClipAddress;
        public AssetReference audioClipReference;
        public bool abnormalNoise;
        public float volumeDb;
        public PlayEvent playEvent = PlayEvent.Play;

        public AudioMixerName audioMixer = AudioMixerName.Effect;

        public enum PlayEvent
        {
            Play,
            Stop,
        }

        [NonSerialized]
        public AudioSource audioSource;

        protected bool playOnLoad;
        protected bool clipLoadedFromAddressable;

        private void OnDestroy()
        {
            if (clipLoadedFromAddressable && audioSource.clip) Addressables.Release(audioSource.clip);
        }

        private void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
            if (!audioSource) audioSource = this.gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1;
            audioSource.dopplerLevel = 0;
            audioSource.playOnAwake = false;
            audioSource.loop = true;

            // Load audioclip
            if (audioClipAddress != null && audioClipAddress != "")
            {
                Addressables.LoadAssetAsync<AudioClip>(audioClipAddress).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        audioSource.clip = handle.Result;
                        if (playOnLoad) Play();
                        playOnLoad = false;
                        clipLoadedFromAddressable = true;
                    }
                };
            }
            else if (audioClipReference != null)
            {
                Addressables.LoadAssetAsync<AudioClip>(audioClipReference).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        audioSource.clip = handle.Result;
                        if (playOnLoad) Play();
                        playOnLoad = false;
                        clipLoadedFromAddressable = true;
                    }
                };
            }
        }

        public override bool IsPlaying()
        {
            return audioSource.isPlaying;
        }

        public override void Play()
        {
            if (playEvent == PlayEvent.Play)
            {
                if (audioSource.clip != null)
                {
                    audioSource.Play();
                }
                else
                {
                    playOnLoad = true;
                }
            }
        }

        public override void Stop()
        {
            if (playEvent == PlayEvent.Stop)
            {
                if (audioSource.clip != null)
                {
                    audioSource.Play();
                }
                else
                {
                    playOnLoad = true;
                }
            }
        }
    }
}
