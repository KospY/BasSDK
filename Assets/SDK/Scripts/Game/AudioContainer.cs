using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ThunderRoad
{
    [CreateAssetMenu(fileName = "AudioContainer", menuName = "Audio/AudioContainer")]
    public class AudioContainer : ScriptableObject
    {
        public List<AudioClip> sounds;

        //this list contains all sounds except the last played
        protected AudioClip[] filteredSounds;

        protected AudioClip lastPlayedClip = null;

        protected int filteredSoundsCount = 0;

        private void Awake()
        {
            filteredSounds = new AudioClip[sounds.Count];
        }

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
            filteredSoundsCount = 0;

            for (int i = 0; i < sounds.Count; i++)
            {
                if (sounds[i] != lastPlayedClip)
                {
                    filteredSounds[i] = sounds[i];
                    filteredSoundsCount++;
                }
            }
        }

        public AudioClip GetRandomAudioClip(List<AudioClip> audioClips)
        {
            if (audioClips.Count == 0) return null;
            if (audioClips.Count == 1) return audioClips[0];
            FilterClips(sounds);
            int index = UnityEngine.Random.Range(0, filteredSoundsCount - 1);
            lastPlayedClip = filteredSounds[index];
            return lastPlayedClip;
        }
    }
}