using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Object = UnityEngine.Object;

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
            [ReadOnly]
            public AudioClip audio;
            public AnimationClip animation;
            public FaceAnimator.Expression dialogExpression;
            public string transcriptionTextId;
# if UNITY_EDITOR
            [OnValueChanged(nameof(WeightUpdated))]
#endif
            public int weight = 1;
# if UNITY_EDITOR
            [ShowInInspector]
            public float percent => Mathf.RoundToInt(1000f * ((float)weight / (Mathf.Approximately(totalWeight, 0f) ? weight : totalWeight))) / 10f;
            [ShowInInspector]
            [ReadOnly]
            public string transcriptionText
            {
                get
                {
                    if (_transcriptionText.IsNullOrEmptyOrWhitespace())
                    {
                        if (!Catalog.IsJsonLoaded()) Catalog.EditorLoadAllJson(true, true, true);
                        _transcriptionText = Catalog.GetData<TextData>("English").textGroups.FirstOrDefault(group => group.id == "Dialog")?.texts.FirstOrDefault(text => text.id == transcriptionTextId)?.text;
                    }
                    return _transcriptionText;
                }
            }
            private string _transcriptionText;
            [NonSerialized]
            public int totalWeight;
            public Action weightUpdate;

            private void WeightUpdated()
            {
                weightUpdate?.Invoke();
            }
#endif

            public AudioClipData(AudioClip aud) => audio = aud;
        }
#if ODIN_INSPECTOR
        [HorizontalGroup("Horiz")]
#endif
        public bool useShuffle;
#if ODIN_INSPECTOR        
        [HorizontalGroup("Horiz")]
#endif
        public bool showSubtitles = true;
#if UNITY_EDITOR
        [TableList(AlwaysExpanded = true)]
        [OnValueChanged(nameof(UpdateWeights))]
#endif
        public List<AudioClipData> clips = new();

#if UNITY_EDITOR
        public void UpdateWeights()
        {
            int totalWeight = 0;
            foreach (AudioClipData clipData in clips)
            {
                clipData.weightUpdate = UpdateWeights;
                totalWeight += clipData.weight;
            }
            foreach (AudioClipData clipData in clips)
            {
                clipData.totalWeight = totalWeight;
            }
        }

        [Header("Modify Container")]
        [NonSerialized]
        public AudioClip newClip;

        [Button]
        public void SetAllExpressions(FaceAnimator.Expression exp)
        {
            foreach (AudioClipData clip in clips)
            {
                clip.dialogExpression = exp;
            }
        }

        [Button]
        public void IDFindAndReplace(string find = "", string replace = "")
        {
            foreach (AudioClipData data in clips)
            {
                if (string.IsNullOrEmpty(data.transcriptionTextId))
                {
                    //set the transcription text id to the audio name
                    string cleanedName = data.audio.name;
                    cleanedName = cleanedName.Replace("_TRIMMED", "");
                    //remove "Audio_" from the start of the name
                    cleanedName = cleanedName.Replace("Audio_", "");
                    data.transcriptionTextId = cleanedName;
                }
                data.transcriptionTextId = data.transcriptionTextId.Replace(find, replace);
            }
        }

        [Button]
        public void AddTranscriptionIDsToTextJSON(string textID = "English", string groupID = "Dialog")
        {
            TextData text = Catalog.GetData<TextData>(textID);
            TextData.TextGroup group = text.textGroups.First(groupInText => groupInText.id == groupID);
            foreach (AudioClipData data in clips)
            {
                if (data.transcriptionTextId.IsNullOrEmptyOrWhitespace())
                {
                    Debug.LogError($"Missing subtitle for {name}/{data.audio.name}");
                    continue;
                }
                group.texts.Add(new TextData.TextID() { id = data.transcriptionTextId, text = "L:" + data.transcriptionTextId });
            }
        }
#endif

        private void OnValidate()
        {
            clips ??= new List<AudioClipData>();
#if UNITY_EDITOR
            UpdateWeights();
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
            foreach (var clip in clips)
            {
                clip.weightUpdate = UpdateWeights;
            }
#endif
        }

        public AudioContainer audioContainer
        {
            get
            {
                if (_audioContainer != null)
                {
                    return _audioContainer;
                }
                _audioContainer = CreateInstance<AudioContainer>();
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
                if (!_matchedClipData.IsNullOrEmpty())
                {
                    return _matchedClipData;
                }
                Dictionary<AudioClip, AudioClipData> clipData = new Dictionary<AudioClip, AudioClipData>();
                foreach (AudioClipData clip in clips) clipData.Add(clip.audio, clip);
                _matchedClipData = new Dictionary<int, AudioClipData>();
                for (int i = 0; i < audioContainer.sounds.Count; i++)
                {
                    AudioClip clip = audioContainer.sounds[i];
                    //we use the index instead of a hash because they are unique per audio container and the clip order is retained
                    _matchedClipData.Add(i, clipData[clip]);
                }
                return _matchedClipData;
            }
        }

        [NonSerialized]
        private Dictionary<int, AudioClipData> _matchedClipData;

#if UNITY_EDITOR
        [MenuItem("Assets/ThunderRoad/AudioClips + AnimClips > AdvancedAudioContainers")]
        public static void CreateFromClips(MenuCommand menuCommand)
        {
            AdvancedAudioContainer advancedAudioContainer = new();
            advancedAudioContainer.clips = new List<AudioClipData>();
            advancedAudioContainer.name = "NewAdvancedAudioContainer";
            string rootPath = null;
            Dictionary<string, UnityEngine.Object> collectedObjects = new();
            foreach (string guid in Selection.assetGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                if (rootPath == null)
                {
                    rootPath = path.Split('.')[0].Replace(asset.name, "");
                }
                AudioClipData clipData = null;
                if (asset is AnimationClip anim)
                {
                    string audioKey = asset.name.Replace("Animation", "Audio");
                    if (collectedObjects.ContainsKey(audioKey))
                    {
                        clipData = new AudioClipData((AudioClip)collectedObjects[audioKey]) { animation = anim };
                    }
                }
                if (asset is AudioClip audio)
                {
                    string animKey = asset.name.Replace("Audio", "Animation");
                    if (collectedObjects.ContainsKey(animKey))
                    {
                        clipData = new AudioClipData(audio) { animation = (AnimationClip)collectedObjects[animKey] };
                    }
                }
                if (clipData == null)
                {
                    collectedObjects.Add(asset.name, asset);
                }
                else
                {
                    advancedAudioContainer.clips.Add(clipData);
                }
            }
            advancedAudioContainer.clips = advancedAudioContainer.clips.OrderBy(clip => clip.audio.name).ToList();
            advancedAudioContainer.RefreshDialogIDs();
            AssetDatabase.CreateAsset(advancedAudioContainer, $"{rootPath}/{advancedAudioContainer.name}.asset");
        }

        public void MatchFilenamesToTranscriptionIDs()
        {
            foreach (AudioClipData data in clips)
            {
                string audioPath = AssetDatabase.GetAssetPath(data.audio);
                string newAudioPath = $"{audioPath.Split('.')[0].Replace(data.audio.name, "Audio_" + data.transcriptionTextId)}.{audioPath.Split('.')[1]}";
                AssetDatabase.CreateAsset(data.audio, newAudioPath);
                AssetDatabase.DeleteAsset(audioPath);
                string animPath = AssetDatabase.GetAssetPath(data.animation);
                string newAnimPath = $"{animPath.Split('.')[0].Replace(data.animation.name, "Audio_" + data.transcriptionTextId)}.{animPath.Split('.')[1]}";
                Debug.Log($"{newAudioPath}, {newAnimPath}");
                AssetDatabase.CreateAsset(data.animation, newAnimPath);
                AssetDatabase.DeleteAsset(animPath);
            }
        }

        public void RefreshDialogIDs()
        {
            foreach (AudioClipData data in clips)
            {
                if (string.IsNullOrEmpty(data.transcriptionTextId))
                {
                    //set the transcription text id to the audio name
                    string cleanedName = data.audio.name;
                    cleanedName = cleanedName.Replace("_TRIMMED", "");
                    //remove "Audio_" from the start of the name
                    cleanedName = cleanedName.Replace("Audio_", "");
                    data.transcriptionTextId = cleanedName;
                }
            }
        }
#endif
    }
}
