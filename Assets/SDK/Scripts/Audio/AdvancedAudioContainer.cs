using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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
            public FaceAnimator.Expression dialogExpression;
            public string transcriptionTextId;
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
        public bool showSubtitles = true;
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
                for (var i = 0; i < clips.Count; i++)
                {
                    AudioClipData data = clips[i];
                    //we use the index instead of a hash because they are unique per audio container and the clip order is retained
                    _matchedClipData.Add(i, data);
                }
                return _matchedClipData;
            }
        }
        [NonSerialized]
        private Dictionary<int, AudioClipData> _matchedClipData;

    }
}
