using UnityEngine;
using System.Collections.Generic;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AdvancedAudioContainer")]
    [CreateAssetMenu(menuName = "ThunderRoad/Audio/Advanced audio container")]
    public class AdvancedAudioContainer : ScriptableObject
    {
        [Serializable]
        public class AudioClipData
        {
#if ODIN_INSPECTOR
            [ReadOnly]
#endif
            public AudioClip audio;
            public AnimationClip animation;
            public int weight = 1;

            public AudioClipData(AudioClip aud)
            {
                audio = aud;
            }
        }

#if ODIN_INSPECTOR
        [TableList(AlwaysExpanded = true, HideToolbar = true)]
#endif
        public List<AudioClipData> clips = new List<AudioClipData>();
        public bool useShuffle;
        [Header("Add clip")]
        public AudioClip newClip;

        private void OnValidate()
        {
            clips ??= new List<AudioClipData>();
            if (newClip != null)
            {
                foreach (AudioClipData data in clips)
                {
                    if (data.audio == newClip)
                    {
                        newClip = null;
                        return;
                    }
                }
                clips.Add(new AudioClipData(newClip));
            }
            newClip = null;
        }

        public AudioContainer audioContainer
        {
            get
            {
                if (_audioContainer != null) return _audioContainer;
                _audioContainer = ScriptableObject.CreateInstance<AudioContainer>();
                _audioContainer.sounds = new List<AudioClip>();
                foreach (AudioClipData data in clips)
                {
                    for (int i = 0; i < data.weight; i++)
                    {
                        _audioContainer.sounds.Add(data.audio);
                    }
                }
                _audioContainer.useShuffle = useShuffle;
                return _audioContainer;
            }
        }
        [NonSerialized]
        private AudioContainer _audioContainer;

        public Dictionary<int, AudioClipData> matchedClipData
        {
            get
            {
                if (!_matchedClipData.IsNullOrEmpty()) return _matchedClipData;
                _matchedClipData = new Dictionary<int, AudioClipData>();
                foreach (AudioClipData data in clips)
                {
                    _matchedClipData.Add(Animator.StringToHash(data.audio.name), data);
                }
                return _matchedClipData;
            }
        }
        [NonSerialized]
        private Dictionary<int, AudioClipData> _matchedClipData;
    }
}
