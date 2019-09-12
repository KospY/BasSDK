using UnityEngine;

namespace BS
{
    public class AudioMixerLinker : MonoBehaviour
    {
        public AudioMixer audioMixer;

        public enum AudioMixer
        {
            Master,
            Effect,
            Ambient,
            UI,
            Voice,
            Music,
        }

#if FULLGAME
        protected void Awake()
        {
            AudioSource audioSource = this.GetComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = GameManager.local.audioMixer.FindMatchingGroups(audioMixer.ToString())[0];
        }
#endif
    }
}
