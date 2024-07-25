using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class VoiceData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Max Pool Sizes For Each Voice Clip")]
#endif
        public int pooledCount;

#if ODIN_INSPECTOR
        [BoxGroup("Max Pool Sizes For Each Voice Clip")]
#endif
        public int androidPooledCount;

#if ODIN_INSPECTOR
        [BoxGroup("Voice"), TableList(AlwaysExpanded = true)]
#endif
        public List<Dialog> dialogs = new List<Dialog>();

        [Serializable]
        public class Dialog
        {
            public string id;
            [NonSerialized]
            public int hashId;
            [Tooltip("Defaults to voice. Can be set to another channel if necessary.")]
            public AudioMixerName mixerChannel = AudioMixerName.Voice;

            public string audioGroupAddress;

            [JsonIgnore]
            public AudioContainer audioContainer => advancedAudioContainer != null ? advancedAudioContainer.audioContainer : basicAudioContainer;
            [NonSerialized]
            public AudioContainer basicAudioContainer;
            [NonSerialized]
            public AdvancedAudioContainer advancedAudioContainer;
            [NonSerialized]
            public VoiceData rootData;
            
            public virtual void ReleaseAddressableAssets()
            {
                if (audioContainer) Catalog.ReleaseAsset(audioContainer);
                this.basicAudioContainer = null;
                this.advancedAudioContainer = null;
            }
            public virtual IEnumerator LoadAddressableAssetsCoroutine(VoiceData voiceData)
            {
yield break;
            }
            
            private void OnAudioGroupAssetLoaded(ScriptableObject scriptableObject)
            {
            }
        }



    }
}
