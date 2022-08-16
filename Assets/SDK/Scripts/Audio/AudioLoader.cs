using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AudioLoader")]
    public class AudioLoader : MonoBehaviour
    {
        public AssetReferenceT<AudioClip> audioClipReference;
        public bool useAudioClipAddress;
        public string audioClipAddress;

        public AudioMixerName audioMixer;

        [NonSerialized]
        public AudioSource audioSource;

        private void OnValidate()
        {
            AudioSource audioSource = this.GetComponent<AudioSource>();
            if (audioSource) audioSource.clip = null;
        }

        protected void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
            audioSource.clip = null;
            audioSource.playOnAwake = false;
#if PrivateSDK
            if (Level.current && Level.current.dungeon && !Level.current.dungeon.initialized)
            {
                Level.current.dungeon.onDungeonGenerated += OnDungeonGenerated;
            }
#endif
        }

        private void Start()
        {
        }

        protected void OnDungeonGenerated(EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd && gameObject.activeInHierarchy && enabled)
            {
                OnEnable();
            }
        }

        protected void OnEnable()
        {
#if PrivateSDK
            if (Level.current && Level.current.dungeon && !Level.current.dungeon.initialized) return;
#endif
            if (useAudioClipAddress)
            {
                Addressables.LoadAssetAsync<AudioClip>(audioClipAddress).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (!gameObject.activeInHierarchy || !enabled)
                        {
                            // Unload if object get disabled before async loading finish
                            Addressables.Release(handle.Result);
                        }
                        else
                        {
                            audioSource.clip = handle.Result;
                            audioSource.Play();
                        }
                    }
                    else
                    {
                        Debug.LogError("Could not find audio at address: " + audioClipAddress);
                    }
                };
            }
            else
            {
                if (audioClipReference == null)
                {
                    Debug.LogError("audioClipReference is missing on " + this.name);
                    return;
                }
                Addressables.LoadAssetAsync<AudioClip>(audioClipReference).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (!gameObject.activeInHierarchy || !enabled)
                        {
                            // Unload if object get disabled before async loading finish
                            Addressables.Release(handle.Result);
                        }
                        else
                        {
                            audioSource.clip = handle.Result;
                            audioSource.Play();
                        }
                    }
                };
            }
        }

        protected void OnDisable()
        {
            if (audioSource.clip) Addressables.Release(audioSource.clip);
            audioSource.clip = null;
        }
    }
}
