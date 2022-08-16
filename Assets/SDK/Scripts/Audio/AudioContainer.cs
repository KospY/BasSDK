using UnityEngine;
using System.Collections.Generic;
using EasyButtons;
using System.Reflection;
using System;
using UnityEditor;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AudioContainer")]
    [CreateAssetMenu(menuName = "ThunderRoad/Audio/Audio container")]
    public class AudioContainer : ScriptableObject
    {
        public List<AudioClip> sounds;

        [NonSerialized]
        protected AudioClip[] filteredSounds;
        [NonSerialized]
        protected bool filteredSoundsInitialized;
        [NonSerialized]
        protected AudioClip lastPlayedClip = null;
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

#if (UNITY_EDITOR)
        [Button]
        public void TestRandomAudioClip()
        {
            Assembly assembly = typeof(AudioImporter).Assembly;
            Type audioUtilType = assembly.GetType("UnityEditor.AudioUtil");

            Type[] typeParams = { typeof(AudioClip), typeof(int), typeof(bool) };

            MethodInfo method = audioUtilType.GetMethod("PlayClip", typeParams);

            AudioClip clip = GetRandomAudioClip(sounds);
            object[] objParams = { clip, 0, false };

            method.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, objParams, null);

            Debug.Log("Playing clip : " + clip);
        }
#endif

        public bool TryPickAudioClip(out AudioClip audioClip)
        {
            audioClip = PickAudioClip();
            return audioClip != null;
        }
        public AudioClip PickAudioClip()
        {
            return GetRandomAudioClip(sounds);
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

        protected void FilterClips(List<AudioClip> audioClips)
        {
            if (!filteredSoundsInitialized)
            {
                filteredSounds = new AudioClip[sounds.Count];
                filteredSoundsInitialized = true;
            }
            filteredSoundsCount = 0;

            for (int i = 0; i < sounds.Count; i++)
            {
                if (sounds[i] != lastPlayedClip)
                {
                    filteredSounds[filteredSoundsCount] = sounds[i];
                    filteredSoundsCount++;
                }
            }
        }

        public AudioClip GetRandomAudioClip(List<AudioClip> audioClips)
        {
            if (audioClips.Count == 0) return null;
            if (audioClips.Count == 1) return audioClips[0];
            FilterClips(sounds);
            int index = UnityEngine.Random.Range(0, filteredSoundsCount);
            lastPlayedClip = filteredSounds[index];
            return lastPlayedClip;
        }

        public void GenerateAudioClipHashes()
        {
        }
    }
}