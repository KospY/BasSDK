using UnityEngine;

namespace ThunderRoad
{
    public class AudioMixerLinker : MonoBehaviour
    {
        public AudioMixerName audioMixer;

#if ProjectCore
        protected void Awake()
        {
            foreach (AudioSource audioSource in this.GetComponents<AudioSource>())
            {
                audioSource.outputAudioMixerGroup = GameManager.GetAudioMixerGroup(audioMixer);
            }
        }
#endif
    }
}
