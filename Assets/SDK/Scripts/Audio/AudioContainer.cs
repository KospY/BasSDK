using UnityEngine;
using System.Collections.Generic;
using System;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AudioContainer")]
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



        public bool TryPickAudioClip(out AudioClip audioClip)
        {
            audioClip = PickAudioClip();
            return audioClip != null;
        }
        public AudioClip PickAudioClip()
        {
            if (useShuffle)
            {
                if(_shuffleOrder == null)
                {
                    _shuffleOrder = new ShuffleOrder(sounds.Count);
                }

                int index = _shuffleOrder.Next();
                if(index < 0)
                {
                    return null;
                }

                return sounds[index];
            }

            return GetRandomAudioClip();
        }
        public bool TryPickAudioClipHash(out int audioClipHash)
        {
            GenerateAudioClipHashes();
            return clipToHashes.TryGetValue(PickAudioClip(), out audioClipHash);
        }
        public int PickAudioClipHash()
        {
            GenerateAudioClipHashes();
            return clipToHashes[PickAudioClip()];
        }

        public bool TryGetAudioClipHash(AudioClip audioClip, out int audioClipHash)
        {
            GenerateAudioClipHashes();
            return clipToHashes.TryGetValue(audioClip, out audioClipHash);
        }
        public bool TryGetAudioClip(int audioClipHash, out AudioClip audioClip)
        {
            GenerateAudioClipHashes();
            return hashesToClips.TryGetValue(audioClipHash, out audioClip);
        }
        public AudioClip PickAudioClip(int index)
        {
            return sounds[index];
        }

        public void CachePcmData(bool force = false)
        {
            if (!force && pcmDataCached) return;
            pcmDataDic = new Dictionary<AudioClip, PcmData>();
            foreach (AudioClip audioClip in sounds)
            {
                pcmDataDic.Add(audioClip, new PcmData(audioClip));
            }
            pcmDataCached = true;
        }

        public PcmData GetPcmData(AudioClip audioClip)
        {
            if (!pcmDataCached) CachePcmData();
            if (pcmDataDic.TryGetValue(audioClip, out PcmData pcmData))
            {
                return pcmData;
            }
            else
            {
                Debug.LogWarning("You are getting pcmdata of an audioclip that is not cached in this audioContainer, this is less performant");
                return new PcmData(audioClip);
            }
        }

        protected void FilterClips(int[] audioClips)
        {
            if (!filteredSoundsInitialized)
            {
                filteredSounds = new int[sounds.Count];
                filteredSoundsInitialized = true;
            }
            filteredSoundsCount = 0;

            for (int i = 0; i < hashes.Length; i++)
            {
                if (hashes[i] != lastPlayedClipHash)
                {
                    filteredSounds[filteredSoundsCount] = hashes[i];
                    filteredSoundsCount++;
                }
            }
        }

        public AudioClip GetRandomAudioClip()
        {
            GenerateAudioClipHashes();
            if (hashes.Length == 0) return null;
            if (hashes.Length == 1) return sounds[0];
 
            int newClipHash = lastPlayedClipHash;
            //make sure we dont play the same one as last time;
            while (lastPlayedClipHash == newClipHash)
            {
                int index = UnityEngine.Random.Range(0, hashes.Length);
                newClipHash = hashes[index];
            }
            
            if (TryGetAudioClip(newClipHash, out var clip))
            {
                lastPlayedClipHash = newClipHash;
                return clip;
            }
            return null;
        }
        
        public void GenerateAudioClipHashes()
        {
            //generate hashes for the audioClip names in the audioContainer for quicker comparison later
            if (!hashesToClips.IsNullOrEmpty()) return;
            int soundsCount = sounds.Count;
            hashesToClips = new Dictionary<int, AudioClip>(soundsCount);
            clipToHashes = new Dictionary<AudioClip, int>(soundsCount);
            hashes = new int[soundsCount];
            for (int i = 0; i < soundsCount; i++)
            {
                AudioClip audioClip = sounds[i];
                int key = Animator.StringToHash(audioClip.name);
                hashes[i] = key;
                hashesToClips.Add(key, audioClip);
                clipToHashes.Add(audioClip, key);
            }
        }
    }
}