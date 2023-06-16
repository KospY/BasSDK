using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class VoiceData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Pooling")]
#endif
        public int pooledCount;

#if ODIN_INSPECTOR
        [BoxGroup("Pooling")]
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
            [NonSerialized]
            public AudioContainer audioContainer;
            [NonSerialized]
            public AdvancedAudioContainer advancedAudioContainer;
        }


    }
}
