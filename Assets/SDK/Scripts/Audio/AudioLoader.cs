using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ThunderRoad
{
    public class AudioLoader : MonoBehaviour
    {
        public AssetReferenceT<AudioClip> audioClipReference;
        public bool useAudioClipAddress;
        public string audioClipAddress;

        public AudioMixerName audioMixer;

        [NonSerialized]
        public AudioClip audioClip;
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
            if (audioSource)
            {
                audioSource.clip = null;
                audioSource.playOnAwake = false;
            }
            if (Level.current && Level.current.dungeon && !Level.current.dungeon.initialized)
            {
                Level.current.dungeon.onDungeonGenerated += OnDungeonGenerated;
            }
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
            if (audioClip) return;
            if (Level.current && Level.current.dungeon && !Level.current.dungeon.initialized) return;

            if (useAudioClipAddress)
            {
                Addressables.LoadAssetAsync<AudioClip>(audioClipAddress).Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        audioClip = handle.Result;
                        if (!gameObject.activeInHierarchy || !enabled)
                        {
                            // Unload if object get disabled before async loading finish
                            Addressables.Release(audioClip);
                        }
                        if (audioSource)
                        {
                            audioSource.clip = audioClip;
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
                        audioClip = handle.Result;
                        if (!gameObject.activeInHierarchy || !enabled)
                        {
                            // Unload if object get disabled before async loading finish
                            Addressables.Release(audioClip);
                        }
                        if (audioSource)
                        {
                            audioSource.clip = audioClip;
                            audioSource.Play();
                        }
                    }
                };
            }
        }

        protected void OnDisable()
        {
            if (audioClip) Addressables.Release(audioClip);
            if (audioSource) audioSource.clip = null;
        }
    }
}
