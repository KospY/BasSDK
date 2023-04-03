using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AudioLoader")]
    public class AudioLoader : MonoBehaviour
    {
        [Tooltip("Audio Clip Reference (Pulls from GameObject)")]
        public AssetReferenceT<AudioClip> audioClipReference;
        [Tooltip("Instead of using Reference, will pull from Addressables instead")]
        public bool useAudioClipAddress;
        [Tooltip("Addressables Address of Audio Clip")]
        public string audioClipAddress;

        [Tooltip("Will adjust volume of audio from the in-game audio settings.")]
        public AudioMixerName audioMixer;

        [NonSerialized]
        public AudioSource audioSource;
        private Coroutine waitForLoadingCoroutine;
        private bool playOnEnable = true;

        private void OnValidate()
        {
            if (!gameObject.activeInHierarchy) return;
            AudioSource audioSource = this.GetComponent<AudioSource>();
            if (audioSource) audioSource.clip = null;
        }

        protected void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
            audioSource.clip = null;
            audioSource.playOnAwake = false;
 //ProjectCore
        }

        private void Start()
        {
        }


 //ProjectCore

        protected void OnEnable()
        {
            if (!playOnEnable) return;

            if (useAudioClipAddress)
            {
                Catalog.LoadAssetAsync<AudioClip>(audioClipAddress, clip => {

                        if (!gameObject.activeInHierarchy || !enabled)
                        {
                            // Unload if object get disabled before async loading finish
                            Catalog.ReleaseAsset(clip);
                        }
                        else
                        {
                            audioSource.clip = clip;
                            if (audioSource.clip.loadState == AudioDataLoadState.Loaded)
                            {
                                audioSource.Play();
                            }
                            else
                            {
                                waitForLoadingCoroutine = StartCoroutine(PlayOnAudioLoaded());
                            }
                        }
                }, audioClipAddress);
 
            }
            else
            {
                if (audioClipReference == null)
                {
                    Debug.LogError($"audioClipReference is missing on {this.name}");
                    return;
                }
                Catalog.LoadAssetAsync<AudioClip>(audioClipReference, clip => {
                    if (!gameObject.activeInHierarchy || !enabled)
                    {
                        // Unload if object get disabled before async loading finish
                        Catalog.ReleaseAsset(clip);
                    }
                    else
                    {
                        audioSource.clip = clip;
                        if (audioSource.clip.loadState == AudioDataLoadState.Loaded)
                        {
                            audioSource.Play();
                        }
                        else
                        {
                            waitForLoadingCoroutine = StartCoroutine(PlayOnAudioLoaded());
                        }
                    }
                }, "audioLoader");
            }
        }

        protected void OnDisable()
        {
            if (audioSource.clip) Addressables.Release(audioSource.clip);
            audioSource.clip = null;
            if (waitForLoadingCoroutine != null)
            {
                StopCoroutine(waitForLoadingCoroutine);
                waitForLoadingCoroutine = null;
            }
        }

        private IEnumerator PlayOnAudioLoaded()
        {
            if (audioSource.clip.loadState != AudioDataLoadState.Loaded
                && audioSource.clip.loadState != AudioDataLoadState.Loading)
            {
                audioSource.clip.LoadAudioData();
            }

            while (audioSource.clip.loadState == AudioDataLoadState.Loading)
            {
                yield return null;
            }

            if (audioSource.clip.loadState == AudioDataLoadState.Loaded)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogError("Fail to load audio : " + audioSource.clip.name);
            }

            waitForLoadingCoroutine = null;
        }
    }
}
