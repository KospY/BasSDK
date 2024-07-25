using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/AudioContainer.html")]
    [CreateAssetMenu(menuName = "ThunderRoad/Audio/Audio container")]
    public class AudioContainer : ScriptableObject
    {
        public List<AudioClip> sounds;
        public bool useShuffle = false;

        //this list contains all sounds except the last played
        [NonSerialized]
        protected int[] filteredSounds;
        [NonSerialized]
        protected bool filteredSoundsInitialized;
        [NonSerialized]
        protected ShuffleOrder _shuffleOrder = null;
        [NonSerialized]
        protected int lastPlayedClipHash = -1;
        [NonSerialized]
        protected int filteredSoundsCount = 0;

        [HideInInspector]
        public Dictionary<int, AudioClip> hashesToClips;
        [HideInInspector]
        public Dictionary<AudioClip, int> clipToHashes;
        [HideInInspector]
        public int[] hashes;

        [NonSerialized]
        protected Dictionary<AudioClip, PcmData> pcmDataDic;
        [NonSerialized]
        protected bool pcmDataCached;

        [NonSerialized]
        public int maxPoolCountPerClip = 1;


        public AudioClip PickAudioClip()
        {
return default;
        }

    }
}