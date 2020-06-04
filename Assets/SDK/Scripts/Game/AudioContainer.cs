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
        protected List<AudioClip> filteredSounds;

        protected AudioClip lastPlayedClip = null;

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
            filteredSounds = audioClips.Where(x => x != lastPlayedClip).ToList();
        }

        public AudioClip GetRandomAudioClip(List<AudioClip> audioClips)
        {
            if (audioClips.Count == 0) return null;
            if (audioClips.Count == 1) return audioClips[0];
            FilterClips(sounds);
            int index = UnityEngine.Random.Range(0, filteredSounds.Count);
            lastPlayedClip = filteredSounds[index];
            return lastPlayedClip;
        }
    }
}