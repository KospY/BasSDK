using UnityEngine;
using System.Collections.Generic;
using EasyButtons;
using System.Reflection;
using System;
using UnityEditor;

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Audio/Audio container")]
    public class AudioContainer : ScriptableObject
    {
        public List<AudioClip> sounds;

        //this list contains all sounds except the last played
        protected AudioClip[] filteredSounds;
        protected bool filteredSoundsInitialized;

        protected AudioClip lastPlayedClip = null;

        protected int filteredSoundsCount = 0;

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

        public AudioClip PickAudioClip()
        {
            return GetRandomAudioClip(sounds);
        }

        public AudioClip PickAudioClip(int index)
        {
            return sounds[index];
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
    }
}