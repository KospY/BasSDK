using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using ThunderRoad;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class AudioClipVolumeExtension
{
    public static float GetRMSVolumeAtTime(this AudioClip audioClip, float time, int samples, int mode = 1, float[] outputSamples = null)
    {
        if (outputSamples == null) outputSamples = new float[samples];
        int clipSamples = audioClip.samples;
        int offset = Mathf.RoundToInt(clipSamples * (time / audioClip.length));
        if (mode == -1) offset -= samples;
        if (mode == 0) offset -= Mathf.RoundToInt(samples / 2f);
        if (offset + samples >= clipSamples) offset = clipSamples - (samples + 1);
        offset = Mathf.Clamp(offset, 0, int.MaxValue);
        audioClip.GetData(outputSamples, offset);
        float sum = 0f;
        foreach (var singleSample in outputSamples) sum += singleSample * singleSample;
        return Mathf.Sqrt(sum / outputSamples.Length);
    }
}

public class CaptureAudioSyncer : MonoBehaviour
{
    [System.Serializable]
    public class Pair
    {
#if ODIN_INSPECTOR
        [BoxGroup("$indexedName")]
        [HorizontalGroup("$indexedName/Pair")]
        [BoxGroup("$indexedName/Pair/Animation", Order = 0)]
        [LabelWidth(100)]
#endif
        public AnimationClip animation;
#if ODIN_INSPECTOR
        [BoxGroup("$indexedName/Pair/Audio", Order = 1)]
        [LabelWidth(100)]
#endif
        public AudioClip audio;
        [Range(0f, 1f)]
#if ODIN_INSPECTOR
        [BoxGroup("$indexedName/Pair/Animation")]
        [LabelWidth(100)]
#endif
        public float animStartTime = 0f;
        [Range(0f, 1f)]
#if ODIN_INSPECTOR
        [BoxGroup("$indexedName/Pair/Audio")]
        [LabelWidth(100)]
#endif
        public float audStartTime = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("$indexedName/Timing", Order = 2)]
        [MinMaxSlider("pairMinMaxMax", ShowFields = true)]
#endif
        public Vector2 startEndTimes = new Vector2(0f, 1f);
#if ODIN_INSPECTOR
        [BoxGroup("$indexedName/Clipping", Order = 4)]
        [HorizontalGroup("$indexedName/Clipping/Horiz", Width = 0.5f, MarginRight = 2.5f)]
        [LabelWidth(100f)]
#endif
        public float silenceBetween = 0.6f;
#if ODIN_INSPECTOR
        [HorizontalGroup("$indexedName/Clipping/Horiz", Width = 0.5f, MarginLeft = 2.5f)]
        [LabelWidth(60f)]
#endif
        public float clipTails = 0.1f;
        public ClipFlag clipFlag = ClipFlag.None;

        [HideInInspector]
        public int subClipIndex = -1;

        [HideInInspector]
        public CaptureAudioSyncer container;

        public enum ClipFlag
        {
            None,
            Scratch,
            BadSync,
            BadCapture,
            MergeWithLast,
            Good,
            ScratchWithLast,
        }

        private string indexedName => $"({container.pairs.IndexOf(this)}) {name}";
        private string name => $"{(audio == null ? "NULL" : audio.name.Replace("Audio_", "").Replace("Baron_", "").Replace("Jan19_", ""))}{(subClipIndex > -1 ? $"_{subClipIndex}" : "")}";
        private bool canSync => container != null;
        private bool showButton => Application.isPlaying;

        public float animationLength => animation == null ? 1f : animation.length - (animStartTime * animation.length);
        public float audioLength => audio == null ? 1f : audio.length - (audStartTime * audio.length);
        public Vector2 pairMinMaxMin => new Vector2(0f, Mathf.Min(animationLength, audioLength));
        public Vector2 pairMinMaxMax => new Vector2(0f, Mathf.Max(animationLength, audioLength));
        [NonSerialized]
        public Coroutine playEndCoroutine = null;
        [NonSerialized]
        public float playStartTime = 0f;

        public Pair(CaptureAudioSyncer con, AnimationClip anim, AudioClip aud)
        {
            container = con;
            animation = anim;
            audio = aud;
        }

#if ODIN_INSPECTOR
        [BoxGroup("$indexedName/Testing", Order = 3)]
        [EnableIf("showButton")]
#endif
        [Button("Play (Raw)")]
        public void PlayFromFileStart() => Play(0f, 0f);

#if ODIN_INSPECTOR
        [BoxGroup("$indexedName/Testing")]
        [EnableIf("showButton")]
#endif
        [Button("Play (Using configs)")]
        public void PlaySyncStart() => Play(animStartTime, audStartTime);

        private void Play(float animStart, float audStart)
        {
#if UNITY_EDITOR
            if (animation == null || audio == null)
            {
                Debug.LogError("Can't play a pair with a missing animation or audio!");
                return;
            }
            var animator = container.GetComponent<FaceAnimator>();
            AudioSource source = container.GetComponentInChildren<AudioSource>() ?? new GameObject("PlaySource").AddComponent<AudioSource>();
            container.playingPair?.Stop(animator, source);
            container.playingPair = this;
            container.sequenceTestText.text = $"{container.pairs.IndexOf(this)}: {name}";
            animator?.PlayAnimation(animation, false);
            animator?.animator.Play("DynamicFaceA", 0, (startEndTimes.x / animation.length) + animStart);
            Vector3 center = container.GetComponent<Renderer>().bounds.center;
            source.transform.parent = container.transform;
            source.transform.position = center;
            source.clip = audio;
            source.Play();
            source.time = (audio.length * audStart) + startEndTimes.x;
            playEndCoroutine = container.StartCoroutine(EndPair(startEndTimes.y - startEndTimes.x, animator, source));
            playStartTime = Time.time;
#endif
        }

        private IEnumerator EndPair(float time, FaceAnimator animator, AudioSource source)
        {
            yield return new WaitForSeconds(time);
            Stop(animator, source);
        }

        public void Stop(FaceAnimator animator, AudioSource source)
        {
            if (container.playingPair != this) return;
            animator?.StopAnimation();
            source?.Stop();
            if (playEndCoroutine != null) container.StopCoroutine(playEndCoroutine);
            playEndCoroutine = null;
            container.playingPair = null;
        }

#if ODIN_INSPECTOR
        [BoxGroup("$indexedName/Timing")]
        [EnableIf("canSync")]
#endif
        [Button]
        public void SyncPair()
        {
            GetSync(container);
            PlaySyncStart();
        }

#if ODIN_INSPECTOR
        [BoxGroup("$indexedName/Clipping")]
        [EnableIf("showButton")]
#endif
        [Button]
        public void SplitToSubClips()
        {
            container.pairs.Remove(this);
            container.StartCoroutine(SubClipCoroutine(silenceBetween, clipTails));
        }

        public IEnumerator SubClipCoroutine(float timeBetween, float tails)
        {
            bool waitFirstVolume = true;
            bool waitVolume = true;
            float startTime = audio.length * audStartTime;
            float time = startTime;
            float volumeTime = -1000f;
            float silenceTime = -1000f;
            float[] samples = new float[512];
            int found = 0;
            WaitForEndOfFrame eof = new WaitForEndOfFrame();
            Debug.Log($"Starting sub clip find in {name}");
            while (time < audio.length)
            {
                float volume = audio.GetRMSVolumeAtTime(time, 512, 0, samples);
                bool above = volume >= container.clapSilenceThreshold;
                //Debug.Log($"{audio.name}: {samples[i]}");
                if (above)
                {
                    if (waitFirstVolume)
                    {
                        volumeTime = time;
                        silenceTime = time - timeBetween;
                        waitFirstVolume = false;
                        waitVolume = false;
                    }
                    else if (waitVolume)
                    {
                        if (time - silenceTime >= timeBetween)
                        {
                            container.pairs.Add(new Pair(container, animation, audio)
                            {
                                animStartTime = animStartTime,
                                audStartTime = audStartTime,
                                startEndTimes = new Vector2((volumeTime - startTime) - tails, (silenceTime - startTime) + tails),
                                subClipIndex = found,
                            });
                            found++;
                            volumeTime = time;
                        }
                        waitVolume = false;
                    }
                }
                if (!waitVolume && !above)
                {
                    silenceTime = time;
                    waitVolume = true;
                }
                time += 1f / 90f;
                Debug.Log($"Find progress: {Mathf.Clamp01(time / audio.length) * 100f}% ({name})");
                yield return eof;
            }
            if (volumeTime < silenceTime)
            {
                container.pairs.Add(new Pair(container, animation, audio)
                {
                    animStartTime = animStartTime,
                    audStartTime = audStartTime,
                    startEndTimes = new Vector2((volumeTime - startTime) - tails, (silenceTime - startTime) + tails),
                    subClipIndex = found,
                });
            }
            Debug.Log($"Done finding! ({name})");
        }

        //[Button]
        public void TrimFiles()
        {
            if (animation == null || audio == null)
            {
                Debug.LogError("Can't trim a pair with a missing animation or audio!");
                return;
            }
            if (animStartTime <= 0f || audStartTime <= 0f)
            {
                Debug.LogError($"Files not synced! There may be problems with this pair: {animation.name}");
                return;
            }
            AnimationClip newAnim = new AnimationClip() { name = name + "_ANIM_TRIMMED" };
            EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(animation);
            float trimBefore = (animation.length * animStartTime) + startEndTimes.x;
            float maxLength = startEndTimes.y - startEndTimes.x;
            //float skipAfterTime = animation.length * container.configuredExitTime;
            for (int b = bindings.Length - 1; b >= 0; b--)
            {
                EditorCurveBinding binding = bindings[b];
                EditorCurveBinding newBinding = new EditorCurveBinding
                {
                    propertyName = binding.propertyName,
                    type = typeof(SkinnedMeshRenderer),
                    path = binding.path
                };
                AnimationCurve oldCurve = AnimationUtility.GetEditorCurve(animation, binding);
                AnimationCurve newCurve = new AnimationCurve();
                for (int i = 0; i < oldCurve.length; i++)
                {
                    Keyframe oldKey = oldCurve.keys[i];
                    //if (oldKey.time > skipAfterTime) continue;
                    float newTime = oldKey.time - trimBefore;
                    if (newTime < 0f) continue;
                    if (newTime > maxLength) continue;
                    Keyframe newKey = new Keyframe(newTime, oldKey.value, oldKey.inTangent, oldKey.outTangent, oldKey.inWeight, oldKey.outWeight);
                    newCurve.AddKey(newKey);
                    AnimationUtility.SetKeyLeftTangentMode(newCurve, newCurve.length - 1, AnimationUtility.GetKeyLeftTangentMode(oldCurve, i));
                    AnimationUtility.SetKeyRightTangentMode(newCurve, newCurve.length - 1, AnimationUtility.GetKeyRightTangentMode(oldCurve, i));
                }
                AnimationUtility.SetEditorCurve(newAnim, newBinding, newCurve);
            }
            AssetDatabase.CreateAsset(newAnim, GetAssetPath(animation, true, name));

            var orgSampleCount = audio.samples;
            var audioOffset = Mathf.RoundToInt(audio.samples * (((audio.length * audStartTime) + startEndTimes.x) / audio.length));
            var sampleCount = Mathf.Min(Mathf.RoundToInt(audio.samples * ((startEndTimes.y - startEndTimes.x) / audio.length)) * 2, (orgSampleCount - audioOffset) * 2);
            AudioClip newAudio = AudioClip.Create(name + "_AUDIO_TRIMMED", sampleCount, audio.channels, audio.frequency, false);
            float[] samples = new float[sampleCount];
            audio.GetData(samples, audioOffset);
            for (int i = 0; i < samples.Length; i++)
            {
                samples[i] *= container.audioVolumeMultiplier;
            }
            newAudio.SetData(samples, 0);
            //AudioSource.PlayClipAtPoint(newAudio, new Vector3());
            SavWav.Save(GetAssetPath(audio, true, name), newAudio);
            if (!container.exportingAll) AssetDatabase.Refresh();
            startEndTimes = new Vector2(0f, Mathf.Max(animationLength, audioLength));
            animation = newAnim;
            audio = AssetDatabase.LoadAssetAtPath<AudioClip>(GetAssetPath(audio, true, name));
            animStartTime = 0f;
            audStartTime = 0f;
        }

        public void GetSync(CaptureAudioSyncer con)
        {
            if (container == null) container = con;
            if (animation == null || audio == null)
            {
                Debug.LogError("Can't sync a pair with a missing animation or audio!");
                return;
            }
            AnimationCurve curve = null;
            EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(animation);
            for (int b = bindings.Length - 1; b >= 0; b--)
            {
                EditorCurveBinding binding = bindings[b];
                if (binding.propertyName == container.clapPropertyName)
                {
                    curve = AnimationUtility.GetEditorCurve(animation, binding);
                    break;
                }
            }
            if (curve == null)
            {
                Debug.LogError($"Missing clap property in animation {animation.name}!");
                return;
            }
            bool waitBreak = true;
            for (int i = 0; i < curve.keys.Length; i++)
            {
                var key = curve.keys[i];
                bool above = key.value >= container.clapPropertyThreshold;
                if (waitBreak && above)
                {
                    waitBreak = false;
                }
                if (!waitBreak && !above)
                {
                    animStartTime = key.time;
                    break;
                }
            }
            animStartTime /= animation.length;
            waitBreak = true;
            float time = 0f;
            while (time < audio.length)
            {
                float volume = audio.GetRMSVolumeAtTime(time, 512, 0);
                bool above = volume >= container.clapSilenceThreshold;
                //Debug.Log($"{audio.name}: {samples[i]}");
                if (waitBreak && above)
                {
                    waitBreak = false;
                }
                if (!waitBreak && !above)
                {
                    audStartTime = time;
                    break;
                }
                time += 1f / 90f;
            }
            audStartTime /= audio.length;
        }

        private string GetAssetPath(UnityEngine.Object asset, bool trimmed, string swapFileName = null)
        {
            string originalPath = AssetDatabase.GetAssetPath(asset);
            var basePathAndExt = originalPath.Split('.');
            string type = asset.GetType().Name.Replace("Clip", "");
            var extra = trimmed ? "_TRIMMED" : "";
            string result = $"{$"{basePathAndExt[0]}.".Replace($"/{asset.name}.", $"/{type}_{(swapFileName != null ? swapFileName : audio.name)}")}{extra}.{basePathAndExt[1].Replace("mp3", "wav")}";
            return result;
        }
    }

#if ODIN_INSPECTOR
    [Title("Results")]
    [ListDrawerSettings(CustomAddFunction = "AddNewPair")]
#endif
    public List<Pair> pairs = new List<Pair>();

#if ODIN_INSPECTOR
    [Title("Config")]
#endif
    public string searchDirectory = "";
    public AnimationClip sampleAnim;
#if ODIN_INSPECTOR
    [ValueDropdown(nameof(GetOptions))]
#endif
    public string clapPropertyName = "";
    public float clapPropertyThreshold = 40f;
    public AudioClip sampleAudio;
    public float clapSilenceThreshold = 0.05f;
    public Text sequenceTestText;
    public Text playTimeText;
    public Transform flagButtons;
    public float audioVolumeMultiplier = 1f;
    //public float configuredExitTime = 0.75f;

    [NonSerialized]
    public bool exportingAll = false;
    private float confirmTime = 0f;
    private bool sequentialTest => Application.isPlaying;
    private bool canConfirm => Time.realtimeSinceStartup < confirmTime + 3f;
    private Pair playingPair;

    public void AddNewPair() => pairs.Add(new Pair(this, null, null));

#if ODIN_INSPECTOR
    [Title("Utilities")]
#endif
    [Button]
    public void SampleClipVolumes(int samples = 512)
    {
        float time = 0f;
        while (time < sampleAudio.length)
        {
            Debug.Log($"{time} volume: {sampleAudio.GetRMSVolumeAtTime(time, samples, 0)}");
            time += 1f / 90f;
        }
    }

    [Button]
#if ODIN_INSPECTOR
    [HorizontalGroup("CollectButtons")]
#endif
    public void CollectPairs() => PairCollection(true);

    [Button]
#if ODIN_INSPECTOR
    [HorizontalGroup("CollectButtons")]
#endif
    public void ForceCollectPairs() => PairCollection(false);

    private void PairCollection(bool skipMismatch)
    {
        pairs = new List<Pair>();
        List<string> terminatingFolders = new List<string>();
        CollectRecursive(searchDirectory, folder => terminatingFolders.Add(folder));
        foreach (string folder in terminatingFolders)
        {
            List<string> GetAssetPaths(string type)
            {
                var assets = AssetDatabase.FindAssets($"t:{type}", new[] { folder }).ToList();
                for (int i = 0; i < assets.Count; i++)
                {
                    assets[i] = AssetDatabase.GUIDToAssetPath(assets[i]);
                }
                assets.Sort();
                //foreach (var a in assets) Debug.Log(a);
                return assets;
            }
            var animations = GetAssetPaths("AnimationClip");
            var audios = GetAssetPaths("AudioClip");
            if (animations.Count != audios.Count)
            {
                Debug.LogWarning($"Mismatch in animation/audio count! Manual configuration may be required for files in this folder: {folder}");
                if (skipMismatch) continue;
            }
            for (int i = 0; i < Mathf.Min(animations.Count, audios.Count); i++)
            {
                var animClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animations[i]);
                //Debug.Log(animations[i] + "  / " + animClip);
                var audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(audios[i]);
                //Debug.Log(audios[i] + "  / " + audioClip);
                pairs.Add(new Pair(this, animClip, audioClip));
            }
        }
    }

    private void CollectRecursive(string folder, Action<string> noSubAction)
    {
        //Debug.Log(folder);
        var folders = AssetDatabase.GetSubFolders(folder);
        if (folders.Length == 0)
        {
            noSubAction?.Invoke(folder);
            return;
        }
        foreach (var fld in folders)
        {
            CollectRecursive(fld, noSubAction);
        }
    }

    [Button]
    public void SyncPairs()
    {
        foreach (Pair pair in pairs)
        {
            pair.GetSync(this);
        }
    }

    [Button]
#if ODIN_INSPECTOR
    [EnableIf("sequentialTest")]
#endif
    public void SubClipAll()
    {
        StartCoroutine(SubClipCoroutine());
    }

    private IEnumerator SubClipCoroutine()
    {
        int startCount = pairs.Count;
        for (int i = 0; i < startCount; i++)
        {
            yield return pairs[0].SubClipCoroutine(pairs[0].silenceBetween, pairs[0].clipTails);
            pairs.RemoveAt(0);
        }
    }

    [Button("Trim Pairs (Make sure everything is perfectly synced up before proceeding!)")]
#if ODIN_INSPECTOR
    [HideIf("canConfirm")]
    public void TrimPairsInitiate()
    {
        confirmTime = Time.realtimeSinceStartup;
    }

    [Button("CONFIRM: This will create duplicates of all paired files!")]
    [ShowIf("canConfirm")]
#endif
    public void TrimPairs()
    {
        confirmTime = 0f;
        exportingAll = true;
        foreach (Pair pair in pairs)
        {
            pair.TrimFiles();
        }
        exportingAll = false;
        AssetDatabase.Refresh();
    }

    [Button]
#if ODIN_INSPECTOR
    [HorizontalGroup("TestButtons")]
#endif
    public void TestPairSync(int index)
    {
        PlayPair(pairs[index], true);
    }

    [Button]
#if ODIN_INSPECTOR
    [HorizontalGroup("TestButtons")]
#endif
    public void TestPairStarts(int index)
    {
        PlayPair(pairs[index], true);
    }

    [Button]
#if ODIN_INSPECTOR
    [EnableIf("sequentialTest")]
#endif
    public void TestAllInOrder(bool sync = true, bool waitForAssignment = true, float waitExtraTime = 1f)
    {
        if (sequenceTestText == null)
        {
            Debug.LogError("Please assign a text object to write the pair info to!");
            return;
        }
        float seconds = 0f;
        for (int i = 0; i < pairs.Count; i++) seconds += 1 + (pairs[i].startEndTimes.y - pairs[i].startEndTimes.x);
        int minutes = Mathf.CeilToInt((seconds - (seconds % 60)) / 60f);
        Debug.Log($"Expected test duration: {minutes}:{seconds % 60}");
        StartCoroutine(RunTest(sync, waitForAssignment, waitExtraTime));
    }

    private IEnumerator RunTest(bool sync, bool waitForAssignment, float waitExtraTime)
    {
        WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
        Vector3 center = GetComponent<Renderer>().bounds.center;
        AudioSource source = GetComponentInChildren<AudioSource>() ?? new GameObject("PlaySource").AddComponent<AudioSource>();
        source.transform.parent = transform;
        source.transform.position = center;
        for (int i = 0; i < pairs.Count; i++)
        {
            PlayPair(pairs[i], sync);
            yield return endOfFrame;
            while (pairs[i].playEndCoroutine != null) yield return endOfFrame;
            playingPair = pairs[i];
            if (waitForAssignment)
            {
                while (pairs[i].clipFlag == Pair.ClipFlag.None) yield return endOfFrame;
            }
            yield return new WaitForSeconds(waitExtraTime);
        }
        playingPair = null;
    }

    public void SetClipEndTime()
    {
        playingPair.startEndTimes = new Vector2(playingPair.startEndTimes.x, Time.time - playingPair.playStartTime);
        Debug.Log($"Updated {playingPair.audio.name} end time!");
    }

    [Button]
    public void SortByName()
    {
        pairs = pairs.OrderBy(pair => pair.audio.name).Reverse().ToList();
    }

    public void SetFlag(int flag)
    {
        if (playingPair == null) return;
        playingPair.clipFlag = (Pair.ClipFlag)flag;
    }

    public void SkipCurrent()
    {
        if (playingPair == null) return;
        playingPair.Stop(GetComponent<FaceAnimator>(), GetComponentInChildren<AudioSource>());
    }

    [Button]
    public void AutoProcessFlags()
    {
        for (int i = pairs.Count - 1; i >= 0; i--)
        {
            var pair = pairs[i];
            switch (pair.clipFlag)
            {
                case Pair.ClipFlag.ScratchWithLast:
                    var scratchPair = pairs[i - 1];
                    if (scratchPair.clipFlag == Pair.ClipFlag.MergeWithLast || scratchPair.clipFlag == Pair.ClipFlag.ScratchWithLast) scratchPair.clipFlag = Pair.ClipFlag.ScratchWithLast;
                    else scratchPair.clipFlag = Pair.ClipFlag.Scratch;
                    pairs.RemoveAt(i);
                    break;
                case Pair.ClipFlag.MergeWithLast:
                    var mergePair = pairs[i - 1];
                    mergePair.startEndTimes.y = pair.startEndTimes.y;
                    pairs.RemoveAt(i);
                    break;
                case Pair.ClipFlag.Scratch:
                    pairs.RemoveAt(i);
                    break;
                default:
                    break;
            }
        }
        pairs = pairs.OrderByDescending(pair => (int)pair.clipFlag).Reverse().ToList();
    }

    private void OnValidate()
    {
        confirmTime = Time.realtimeSinceStartup - 3f;
    }

    private void Start()
    {
        foreach (Pair pair in pairs)
        {
            pair.container = this;
            if (pair.startEndTimes.y.IsApproximately(0f))
            {
                pair.startEndTimes = pair.pairMinMaxMin;
            }
            pair.silenceBetween = 0.6f;
            pair.clipTails = 0.1f;
        }
        var button = flagButtons.GetComponentInChildren<Button>().gameObject;
        foreach (Pair.ClipFlag flag in Enum.GetValues(typeof(Pair.ClipFlag)))
        {
            if (flag == Pair.ClipFlag.None) continue;
            var newButton = Instantiate(button, button.transform.parent);
            newButton.GetComponentInChildren<Text>().text = flag.ToString();
            newButton.GetComponent<Button>().onClick.AddListener(() => SetFlag((int)flag));
        }
        button.SetActive(false);
#if UNITY_EDITOR
        return;
#endif
        Destroy(this);
    }

    private void Update()
    {
        if (playingPair == null)
        {
            playTimeText.text = "0.0s";
        }
        else
        {
            playTimeText.text = $"{(Time.time - playingPair.playStartTime).ToString("0.00")}s";
        }
    }

    public void PlayPair(Pair pair, bool sync)
    {
        if (sync) pair.PlaySyncStart();
        else pair.PlayFromFileStart();
    }

#if ODIN_INSPECTOR
    private List<ValueDropdownItem<string>> GetOptions()
    {
        List<ValueDropdownItem<string>> options = new List<ValueDropdownItem<string>>();
        if (sampleAnim == null)
        {
            Debug.LogError("Need a clip in the clip field to provide property name options!");
            options.Add(new ValueDropdownItem<string>("None", ""));
            return options;
        }
        EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(sampleAnim);
        for (int b = bindings.Length - 1; b >= 0; b--)
        {
            EditorCurveBinding binding = bindings[b];
            options.Add(new ValueDropdownItem<string>(binding.propertyName, binding.propertyName));
        }
        return options;
    }
#endif
}
