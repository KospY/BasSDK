using UnityEngine;
using System.Collections.Generic;

namespace BS
{
    [CreateAssetMenu(fileName = "AudioContainer", menuName = "Blade and Sorcery/AudioContainer")]
    public class AudioContainer : ScriptableObject
    {
        public List<AudioClip> sounds;

        public AudioClip PickAudioClip()
        {
            return GetRandomAudioClip(sounds);
        }

        public AudioClip PickAudioClip(int index)
        {
            return sounds[index];
        }

        public AudioClip GetRandomAudioClip(List<AudioClip> audioClips)
        {
            if (audioClips.Count == 0) return null;
            if (audioClips.Count == 1) return audioClips[0];
            int index = UnityEngine.Random.Range(0, audioClips.Count);
            return audioClips[index];
        }
    }
}
