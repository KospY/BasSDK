using UnityEngine;
using System.Collections.Generic;

namespace BS
{
    [CreateAssetMenu(fileName = "AudioContainer", menuName = "Blade and Sorcery/AudioContainer")]
    public class AudioContainer : ScriptableObject
    {
        public List<AudioClip> sounds;
    }
}
