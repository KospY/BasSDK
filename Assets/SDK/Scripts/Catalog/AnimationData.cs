using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class AnimationData : CatalogData
    {
#if ODIN_INSPECTOR
        [BoxGroup("General"), HorizontalGroup("General/Horiz"), LabelWidth(150)] 
#endif
        public bool autoMinMaxRange = true;
#if ODIN_INSPECTOR
        [HorizontalGroup("General/Horiz"), DisableIf("autoMinMaxRange", true), LabelWidth(120)] 
#endif
        public float minRange, maxRange;
#if ODIN_INSPECTOR
        [HorizontalGroup("General/Horiz"), LabelWidth(100)] 
#endif
        public bool usesDifficulty = false;

#if UNITY_EDITOR
#if ODIN_INSPECTOR
        [HorizontalGroup("General/Horiz"), LabelWidth(70), ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public string difficulties = "";

#if ODIN_INSPECTOR
        [BoxGroup("Test"), HorizontalGroup("Test/Horiz"), LabelWidth(170), ShowInInspector] 
#endif
        [NonSerialized]
        public float editorTargetDistance = 1.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Test"), HorizontalGroup("Test/Horiz"), LabelWidth(170), ShowInInspector] 
#endif
        [NonSerialized]
        public float editorTargetMovedDis = 1.5f;
#if ODIN_INSPECTOR
        [HorizontalGroup("Test/Horiz"), LabelWidth(150), ShowInInspector, InlineButton("FindDuplicates", "Find Duplicates"), InlineButton("PickRNG", "Pick (RNG)"), InlineButton("EditorPickDelta", "Pick (Delta)"), InlineButton("OrderByRange", "Order by range"), InlineButton("OrderByWeight", "Sort by weight")] 
#endif
        [NonSerialized]
        public float editorMaxDelta = 0.25f;
#if ODIN_INSPECTOR
        [BoxGroup("Auto-fill"), HorizontalGroup("Auto-fill/Horiz"), LabelWidth(170), ShowInInspector, ValueDropdown("GetAllAnimationDataID"), InlineButton("CrossReference", "Copy attack window info"), InlineButton("SetAllNonAttack", "Set all clips non-attack")] 
#endif
        [NonSerialized]
        public string crossReference;

#endif

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllAnimationDataID()
        {
            return Catalog.GetDropdownAllID(Category.Animation);
        } 
#endif

#if ODIN_INSPECTOR
        [BoxGroup("Animation"), OnValueChanged("CalculateWeight")] 
#endif
        public List<Clip> animationClips = new List<Clip>();

        [NonSerialized]
        protected AnimationData[] difficultyFilteredDatas;

        public AnimationData this[int difficulty]
        {
            get
            {
                if (!usesDifficulty || difficulty >= difficultyFilteredDatas.Length) return this;
                return difficultyFilteredDatas[difficulty];
            }
        }


        private float probabilityRNGTotalWeight, probabilityDeltaTotalWeight;

#if UNITY_EDITOR
        protected void FindDuplicates()
        {
            HashSet<AnimationClip> clips = new HashSet<AnimationClip>();
            for (int i = 0; i < animationClips.Count; i++)
            {
                animationClips[i].isDuplicate = !clips.Add(animationClips[i].animationClip);
            }
        }

        protected void EditorPickDelta()
        {
            PickDeltaRNG(editorTargetDistance, editorTargetMovedDis, 0f, editorMaxDelta);
        }

        protected void OrderByRange()
        {
            animationClips = new List<Clip>(animationClips.OrderBy(c => c.range));
        }

        protected void OrderByWeight()
        {
            animationClips = new List<Clip>(animationClips.OrderBy(c => c.weight));
            while (animationClips[0].weight == 0) {
                Clip toBack = animationClips[0];
                animationClips.RemoveAt(0);
                animationClips.Add(toBack);
            }
        }

        protected void SetAllNonAttack()
        {
            for (int i = 0; i < animationClips.Count; i++)
            {
                animationClips[i].attackType = Clip.AttackType.None;
            }
        }
#endif

        [Serializable]
        public class Clip
        {
#if ODIN_INSPECTOR
            [GUIColor("EditorGetColor")]
            [HorizontalGroup("Horiz"), BoxGroup("Horiz/Default"), LabelWidth(50)] 
#endif
            [JsonMergeKey]
            public string address;
#if ODIN_INSPECTOR
            [GUIColor("EditorGetColor")]
            [HorizontalGroup("Horiz/Default/Horiz"), LabelWidth(100), ShowInInspector, DisableInPlayMode, OnValueChanged("EditorUpdateAddress")] 
#endif
            [NonSerialized]
            public AnimationClip animationClip;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Default/Horiz", Width = 110), LabelWidth(30)] 
#endif
            public Step step = Step.Start;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Default/Horiz", Width = 110), LabelWidth(70), LabelText("Anim Speed")] 
#endif
            public float animationSpeed = 1;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Default/Horiz2", Width = 200), LabelWidth(70), ShowIf("step", optionalValue: Step.Loop)] 
#endif
            [Tooltip("Include Sub-Stance means it gets included in this stance's pool by default. Exclude Sub-Stance means it gets excluded unless the held items include it specifically.")]
            public LoopType loopType = LoopType.NonSpecific;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Default/Horiz2"), LabelWidth(100), ShowIf("step", optionalValue: Step.Loop), DisableIf("loopType", optionalValue: LoopType.NonSpecific), OnValueChanged("ValidateName")] 
#endif
            public string subStanceName = "";

#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz", MinWidth = 200), BoxGroup("Horiz/Move"), LabelWidth(100)] 
#endif
            public Direction moveDirection = Direction.None;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Move/Horiz2"), LabelWidth(40)] 
#endif
            public float speed;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Move/Horiz3"), LabelWidth(40)] 
#endif
            public float range;

#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Attack info/Horiz1", Width = 250), BoxGroup("Horiz/Attack info"), LabelWidth(120)] 
#endif
            public AttackType attackType = AttackType.None;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Attack info/Horiz1", Width = 250), LabelWidth(120), DisableIf("attackType", optionalValue: AttackType.None)] 
#endif
            public MotionType attackMotion = MotionType.RightSwing;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Attack info/Horiz2", Width = 250), LabelWidth(120), DisableIf("attackType", optionalValue: AttackType.None)] 
#endif
            public Vector2 startAndEndTime;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Attack info/Horiz2", Width = 250), LabelWidth(120), ValueDropdown("GetSourceSubStanceOptions"), DisableIf("attackType", optionalValue: AttackType.None)] 
#endif
            public string sourceSubStance = "Any";
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Attack info/Horiz3", Width = 250), LabelWidth(120), DisableIf("attackType", optionalValue: AttackType.None)] 
#endif
            public float peakRangeTime;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Attack info/Horiz3", Width = 250), LabelWidth(120), ValueDropdown("GetEndSubStanceOptions"), DisableIf("attackType", optionalValue: AttackType.None)] 
#endif
            public string endSubStance = "Previous";

#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz", MinWidth = 180), BoxGroup("Horiz/Aim"), LabelWidth(80)] 
#endif
            public Direction aimDirection = Direction.None;
#if ODIN_INSPECTOR
            [BoxGroup("Horiz/Aim"), LabelWidth(70)] 
#endif
            public Height aimHeight = Height.None;

#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz", MinWidth = 180), BoxGroup("Horiz/RNG"), HorizontalGroup("Horiz/RNG/Horiz1"), LabelWidth(50), OnValueChanged("EditorCalculateWeight")] 
#endif
            public float weight = 1;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/RNG/Horiz1"), LabelWidth(50)] 
#endif
            public float chance = 1;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/RNG/Horiz2"), HideLabel, ReadOnly, ShowInInspector, ProgressBar(0, 100)] 
#endif
            [NonSerialized]
            public float lastCalculatedProbabilityRNG;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/RNG/Horiz2"), HideLabel, ReadOnly, ShowInInspector, ProgressBar(0, 100, 0.74f, 0.47f, 0.15f)] 
#endif
            [NonSerialized]
            public float probabilityDelta;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/RNG/Horiz3"), LabelWidth(55)] 
#endif
            public int difficulty = 1;

#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz"), BoxGroup("Horiz/Target movement (Times and distances)"), LabelWidth(50)]
            [HideLabel, ReadOnly, ShowInInspector] 
#endif
            [NonSerialized]
            public Vector3 lastCalculatedTimes = new Vector3();
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Target movement (Times and distances)/Horiz2"), HideLabel, ReadOnly, ShowInInspector] 
#endif
            [NonSerialized]
            public Vector3 lastCalculatedDistances = new Vector3();

            [NonSerialized]
            public AnimationData container;
            [NonSerialized]
            public bool lastPicked;
            [NonSerialized]
            public float lastCalculatedDelta;
            [NonSerialized]
            public float lastCalculatedDeltaRatio;
            [NonSerialized]
            public float lastCalculatedWeightFrom, lastCalculatedWeightTo;

            public enum LoopType { NonSpecific, IncludeSubStance, ExcludeSubStance, GuardPose }

            public enum AttackType { None, SingleStrike, ComboAttack, Riposte, ChainRiposte, Push, Ranged }

            public enum MotionType { RightSwing, RightThrust, LeftSwing, LeftThrust, }

            public enum Direction { None, Forward, Backward, Left, Right, Up, Bottom, }

            public enum Height { None, Lower, Mid, Up, }

            public bool ShouldSerializemoveDirection() { return (moveDirection != Direction.None); }

            public bool ShouldSerializespeed() { return (speed > 0); }

            public bool ShouldSerializerange() { return (range > 0); }

            public bool ShouldSerializeaimDirection() { return (aimDirection != Direction.None); }

            public bool ShouldSerializeaimHeight() { return (aimHeight != Height.None); }

            public bool ShouldSerializesubStanceIdle() { return (step == Step.Loop); }

            public bool ShouldSerializesubStanceName() { return (step == Step.Loop && loopType != LoopType.NonSpecific); }

#if UNITY_EDITOR
            [NonSerialized]
            public bool isDuplicate = false;
            [NonSerialized]
            public bool isMissing = false;

            protected Color EditorGetColor() { return isMissing ? Color.yellow : (isDuplicate ? Color.red : (lastPicked ? Color.green : Color.white)); }

            protected void EditorCalculateWeight()
            {
                List<CatalogData> list = Catalog.GetDataList(Category.Animation);
                for (int i = 0; i < list.Count; i++)
                {
                    AnimationData animationData = (AnimationData)list[i];
                    animationData.CalculateWeight();
                    animationData.CalculateWeightDelta(animationData.editorTargetDistance, animationData.editorTargetMovedDis, 0f, animationData.editorMaxDelta, 1f);
                }
            }

            protected void EditorUpdateAddress()
            {
                if (animationClip) address = Catalog.GetAddressFromPrefab(animationClip);
            }

            protected void ValidateName()
            {
                if (subStanceName == "Any" || subStanceName == "Previous" || subStanceName == "RandomInc" || subStanceName == "RandomExc") subStanceName = "";
            }

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetSourceSubStanceOptions() => GetSubStanceOptions(true);

            public List<ValueDropdownItem<string>> GetEndSubStanceOptions() => GetSubStanceOptions(false);

            public List<ValueDropdownItem<string>> GetSubStanceOptions(bool source)
            {
                List<ValueDropdownItem<string>> options = new List<ValueDropdownItem<string>>
                {
                    source ? new ValueDropdownItem<string>("Any", "Any") : new ValueDropdownItem<string>("Previous", "Previous"),
                };
                if (!source)
                {
                    options.Add(new ValueDropdownItem<string>("RandomInc", "RandomInc"));
                    options.Add(new ValueDropdownItem<string>("RandomExc", "RandomExc"));
                }
                if (container == null)
                {
                    List<CatalogData> list = Catalog.GetDataList(Category.Animation);
                    for (int i = 0; i < list.Count; i++)
                    {
                        AnimationData animationData = (AnimationData)list[i];
                        if (animationData.animationClips.Contains(this))
                        {
                            container = animationData;
                            break;
                        }
                    }
                }
                for (int i = 0; i < container.animationClips.Count; i++)
                {
                    Clip clip = container.animationClips[i];
                    if (clip.step == Clip.Step.Loop && (clip.loopType == AnimationData.Clip.LoopType.IncludeSubStance || clip.loopType == AnimationData.Clip.LoopType.ExcludeSubStance))
                    {
                        options.Add(new ValueDropdownItem<string>(clip.subStanceName, clip.subStanceName));
                    }
                }
                return options;
            } 
#endif

#endif
            public enum Step
            {
                Start,
                Loop,
                End,
            }
        }

        public override void OnCatalogRefresh()
        {
            int maxDiff = -1;
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                maxDiff = Mathf.Max(maxDiff, clip.difficulty);
                if (clip.animationClip) Catalog.ReleaseAsset(clip.animationClip);
                Catalog.LoadAssetAsync<AnimationClip>(clip.address, value => clip.animationClip = value, id);
#if UNITY_EDITOR
                if (clip.animationClip == null && !Application.isPlaying)
                {
                    //Debug.LogError($"Clip index {i} in AnimationData [ {id} ] points to an address that doesn't exist! Currently trying to access: [ {clip.address} ] but it does not exist in the addressables!");
                    clip.isMissing = true;
                }
#endif
            }
            CalculateWeight();

            if (autoMinMaxRange)
            {
                // Update min range
                minRange = Mathf.Infinity;
                for (int i = 0; i < animationClips.Count; i++)
                {
                    Clip clip = animationClips[i];
                    if (clip.range < minRange && clip.range > 0)
                    {
                        minRange = clip.range;
                    }
                }
                if (minRange == Mathf.Infinity) minRange = 0f;
                // Update max range
                maxRange = 0;
                for (int i = 0; i < animationClips.Count; i++)
                {
                    Clip clip = animationClips[i];
                    if (clip.range > maxRange)
                    {
                        maxRange = clip.range;
                    }
                }
            }

#if UNITY_EDITOR
            difficulties = "";
#endif
            if (usesDifficulty)
            {
                difficultyFilteredDatas = new AnimationData[maxDiff + 1];
                for (int i = maxDiff; i >= 0; i--)
                {
#if UNITY_EDITOR
                    difficulties = i + ", " + difficulties;
#endif
                    AnimationData newData = difficultyFilteredDatas[i] = new AnimationData();
                    newData.id = id + i;
                    newData.autoMinMaxRange = autoMinMaxRange;
                    if (!autoMinMaxRange)
                    {
                        newData.minRange = minRange;
                        newData.maxRange = maxRange;
                    }
                    newData.usesDifficulty = false;
                    newData.animationClips = new List<Clip>();
                    for (int j = 0; j < animationClips.Count; j++)
                    {
                        Clip copy = animationClips[j];
                        if (copy.difficulty <= i)
                        {
                            newData.animationClips.Add(copy);
                        }
                    }
                    newData.OnCatalogRefresh();
                }
            }
        }

        public void CalculateWeight() => CalculateWeight("None");

        public void CalculateWeight(string requiredSourceSubStance, bool avoidLastPick = false)
        {
            if (string.IsNullOrEmpty(requiredSourceSubStance)) requiredSourceSubStance = "None";
            if (animationClips == null || animationClips.Count == 0) return;

            float weightMaximum = 0f;
            // Sets the weight ranges of the selected items.
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance) continue;
                if (clip.lastPicked && avoidLastPick)
                {
                    clip.lastCalculatedWeightFrom = Mathf.Infinity;
                    clip.lastCalculatedWeightTo = Mathf.NegativeInfinity;
                    continue;
                }
                clip.probabilityDelta = 0;
                if (clip.weight < 0)
                {
                    // Prevent usage of negative weight.
                    clip.weight = 0;
                }
                else
                {
                    clip.lastCalculatedWeightFrom = weightMaximum;
                    weightMaximum += clip.weight;
                    clip.lastCalculatedWeightTo = weightMaximum;
                }
            }
            probabilityRNGTotalWeight = weightMaximum;
            // Calculate percentage of item drop select rate.
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                clip.lastCalculatedProbabilityRNG = 0;
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance) continue;
                if (clip.lastPicked)
                {
                    clip.lastPicked = false;
                    if (avoidLastPick) continue;
                }
                clip.lastCalculatedProbabilityRNG = ((clip.weight) / probabilityRNGTotalWeight) * 100;
            }
        }

        public void CalculateWeightDelta(float targetDistance, float movedDistance, float weaponReach, float maxdelta, float animSpeedMult) => CalculateWeightDelta(targetDistance, movedDistance, weaponReach, maxdelta, animSpeedMult, "None");

        public void CalculateWeightDelta(float targetDistance, float movedDistance, float weaponReach, float maxdelta, float animSpeedMult, string requiredSourceSubStance, bool avoidLastPick = false)
        {
            if (string.IsNullOrEmpty(requiredSourceSubStance)) requiredSourceSubStance = "None";
            if (animationClips == null || animationClips.Count == 0) return;

            float weightMaximum = 0f;
            // Sets the weight ranges of the selected items.
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                // Skip clips which are loop or end clips
                if (clip.step != Clip.Step.Start) continue;
                // If there's a required source sub-stance and it doesn't match, skip this clip
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance) continue;
                if (clip.lastPicked && avoidLastPick)
                {
                    clip.lastCalculatedWeightFrom = Mathf.Infinity;
                    clip.lastCalculatedWeightTo = Mathf.NegativeInfinity;
                    continue;
                }
                if (clip.weight < 0)
                {
                    // Prevent usage of negative weight.
                    clip.weight = 0;
                }
                else
                {
                    float animationFinalSpeed = clip.animationSpeed * animSpeedMult;
                    Vector3 animationKeyTimes = new Vector3(clip.startAndEndTime.x, 0f, clip.startAndEndTime.y);
                    if (animationKeyTimes.sqrMagnitude == 0)
                    {
                        // Make sure we have a defined start and end even if the JSON doesn't have it
                        // Will be used to figure out the player's 
                        animationKeyTimes = new Vector3(0f, 0f, clip.animationClip.length * 0.75f);
                    }
                    animationKeyTimes.y = clip.peakRangeTime;
                    if (animationKeyTimes.y == 0) {
                        // Make sure there's a defined peak range time even if the JSON doesn't have it
                        // Peak range time is used to determine how close to the peak attack range the player is/will be, to help calculate the delta
                        animationKeyTimes.y = clip.animationClip.length * 0.5f;
                    }
                    // Scales the attack window based on the animation speed
                    clip.lastCalculatedTimes = new Vector3(animationKeyTimes.x / animationFinalSpeed, animationKeyTimes.y / animationFinalSpeed, animationKeyTimes.z / animationFinalSpeed);

                    bool useWeaponReach = clip.attackType == Clip.AttackType.SingleStrike || clip.attackType == Clip.AttackType.ComboAttack;
                    float reachTargetDistance = useWeaponReach ? targetDistance - weaponReach : targetDistance;
                    float reachMovedDistance = useWeaponReach ? movedDistance - weaponReach : movedDistance;

                    // Calculates the player's expected positions through the attack window
                    clip.lastCalculatedDistances = new Vector3(Mathf.LerpUnclamped(reachTargetDistance, reachMovedDistance, clip.lastCalculatedTimes.x), Mathf.LerpUnclamped(reachTargetDistance, reachMovedDistance, clip.lastCalculatedTimes.y), Mathf.LerpUnclamped(reachTargetDistance, reachMovedDistance, clip.lastCalculatedTimes.z));

                    clip.lastCalculatedDelta = Math.Abs(clip.range - clip.lastCalculatedDistances.y);
                    //Debug.Log("Old-system delta: " + Math.Abs(clip.range - targetDistance) + " / New-system delta: " + clip.lastCalculatedDelta);

                    // If the player is moving closer, be more forgiving with the delta
                    float deltaShift = clip.lastCalculatedDistances.z < clip.lastCalculatedDistances.x ? 0.6f : 0.4f;
                    // If the weapon's attack range peak is greater than the player's expected distance at that time, be more forgiving with delta
                    deltaShift += clip.range > clip.lastCalculatedDistances.y ? 0.1f : -0.1f;
                    // Apply the delta shift
                    float directionDelta = maxdelta * 2 * deltaShift;

                    if (clip.lastCalculatedDelta < directionDelta)
                    {
                        clip.lastCalculatedDeltaRatio = 1 - (clip.lastCalculatedDelta / directionDelta);
                        clip.lastCalculatedWeightFrom = weightMaximum;
                        weightMaximum += clip.weight * clip.lastCalculatedDeltaRatio;
                        clip.lastCalculatedWeightTo = weightMaximum;
                    }
                    else
                    {
                        clip.lastCalculatedDeltaRatio = 0;
                        clip.lastCalculatedWeightFrom = 0;
                        clip.lastCalculatedWeightTo = 0;
                    }
                }
            }
            probabilityDeltaTotalWeight = weightMaximum;

            // Calculate percentage of item drop select rate.
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                if (clip.step != Clip.Step.Start) continue;
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance) continue;
                if (clip.lastPicked)
                {
                    clip.probabilityDelta = 0;
                    clip.lastPicked = false;
                    if (avoidLastPick) continue;
                }
                if (clip.lastCalculatedDelta < maxdelta)
                {
                    clip.probabilityDelta = ((clip.weight * clip.lastCalculatedDeltaRatio) / probabilityDeltaTotalWeight) * 100;
                }
                else
                {
                    clip.probabilityDelta = 0;
                }
            }
        }

        public Clip PickRNG() => PickRNG("None");

        public Clip PickRNG(string requiredSourceSubStance, bool avoidLastPick = false)
        {
            //foreach (Clip clip in animationClips) { clip.lastPicked = false; };
            if (string.IsNullOrEmpty(requiredSourceSubStance)) requiredSourceSubStance = "None";
            CalculateWeight(requiredSourceSubStance, avoidLastPick);
            if (avoidLastPick && probabilityRNGTotalWeight == 0) CalculateWeight(requiredSourceSubStance, false);
            float pickedNumber = UnityEngine.Random.Range(0, probabilityRNGTotalWeight);
            // Find an item whose range contains pickedNumber
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance) continue;
                //if (clip.lastPicked && avoidLastPick) continue;
                // If the picked number matches the item's range, return item
                if (pickedNumber > clip.lastCalculatedWeightFrom && pickedNumber < clip.lastCalculatedWeightTo)
                {
                    clip.lastPicked = true;
                    return clip;
                }
            }
            return null;
        }

        public Clip PickDeltaRNG(float targetDistance, float movedDistance, float weaponReach, float maxdelta, float animSpeedMult = 1f, bool avoidLastPick = false) => PickDeltaRNG(targetDistance, movedDistance, weaponReach, maxdelta, "None", animSpeedMult, avoidLastPick);

        public Clip PickDeltaRNG(float targetDistance, float movedDistance, float weaponReach, float maxdelta, string requiredSourceSubStance, float animSpeedMult = 1f, bool avoidLastPick = false)
        {
            if (string.IsNullOrEmpty(requiredSourceSubStance)) requiredSourceSubStance = "None";
            CalculateWeightDelta(targetDistance, movedDistance, weaponReach, maxdelta, animSpeedMult, requiredSourceSubStance, avoidLastPick);
            if (avoidLastPick && probabilityDeltaTotalWeight == 0f) CalculateWeightDelta(targetDistance, movedDistance, weaponReach, maxdelta, animSpeedMult, requiredSourceSubStance, false);
#if UNITY_EDITOR
            //foreach (Clip clip in animationClips) { clip.lastPicked = false; };
            if (Application.isPlaying)
            {
                editorTargetDistance = targetDistance;
                editorTargetMovedDis = movedDistance;
                editorMaxDelta = maxdelta;
            }
#endif
            float pickedNumber = UnityEngine.Random.Range(0, probabilityDeltaTotalWeight);
            // Find an item whose range contains pickedNumber
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                if (clip.step != Clip.Step.Start) continue;
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance) continue;
                //if (clip.lastPicked && avoidLastPick) continue;
                // If the picked number matches the item's range, return item
                if (pickedNumber > clip.lastCalculatedWeightFrom && pickedNumber < clip.lastCalculatedWeightTo)
                {
                    clip.lastPicked = true;
                    return clip;
                }
            }
            return null;
        }

#if UNITY_EDITOR
        public void CrossReference()
        {
            if (string.IsNullOrEmpty(crossReference)) return;
            AnimationData animation = Catalog.GetData<AnimationData>(crossReference);
            Dictionary<string, Clip> clipsByAddressableName = new Dictionary<string, Clip>();
            foreach (Clip clip in animation.animationClips)
            {
                if (!clipsByAddressableName.ContainsKey(clip.address))
                {
                    clipsByAddressableName.Add(clip.address, clip);
                }
            }
            foreach (Clip clip in animationClips)
            {
                if (clipsByAddressableName.TryGetValue(clip.address, out Clip otherClip))
                {
                    clip.peakRangeTime = otherClip.peakRangeTime;
                    clip.startAndEndTime = otherClip.startAndEndTime;
                }
            }
        }
#endif

        public Clip Pick(Clip.Direction moveDirection, string requiredSourceSubStance = "None", bool avoidLastPick = false)
        {
            //foreach (Clip clip in animationClips) { clip.lastPicked = false; };

            List<Clip> clips = new List<Clip>();
            Clip lastPicked = null;
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance) continue;
                if (clip.moveDirection == moveDirection)
                {
                    if (clip.lastPicked)
                    {
                        clip.lastPicked = false;
                        lastPicked = clip;
                        if (avoidLastPick) continue;
                    }
                    clips.Add(clip);
                }
            }
            if (clips.Count == 0)
            {
                if (avoidLastPick && lastPicked != null) return lastPicked;
                return null;
            }

            int randomIndex = Common.GetRandomWeightedIndex(clips.Select(c => c.weight).ToArray());

            clips[randomIndex].lastPicked = true;

            return clips[randomIndex];
        }

        public Clip Pick(Clip.Direction moveDirection, Clip.Direction aimDirection, string requiredSourceSubStance = "None", bool avoidLastPick = false)
        {
            //foreach (Clip clip in animationClips) { clip.lastPicked = false; };

            List<Clip> clips = new List<Clip>();
            Clip lastPicked = null;
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance) continue;
                if (clip.moveDirection == moveDirection && clip.aimDirection == aimDirection)
                {
                    if (clip.lastPicked)
                    {
                        clip.lastPicked = false;
                        lastPicked = clip;
                        if (avoidLastPick) continue;
                    }
                    clips.Add(clip);
                }
            }
            if (clips.Count == 0)
            {
                if (avoidLastPick && lastPicked != null) return lastPicked;
                return null;
            }

            int randomIndex = Common.GetRandomWeightedIndex(clips.Select(c => c.weight).ToArray());

            clips[randomIndex].lastPicked = true;

            return clips[randomIndex];
        }

        public Clip Pick(Clip.Step step, string requiredSourceSubStance = "None", bool avoidLastPick = false)
        {
            //foreach (Clip clip in animationClips) { clip.lastPicked = false; };

            List<Clip> clips = new List<Clip>();
            Clip lastPicked = null;
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance) continue;
                if (clip.step == step)
                {
                    if (clip.lastPicked)
                    {
                        clip.lastPicked = false;
                        lastPicked = clip;
                        if (avoidLastPick) continue;
                    }
                    clips.Add(clip);
                }
            }
            if (clips.Count == 0)
            {
                if (avoidLastPick && lastPicked != null) return lastPicked;
                return null;
            }

            int randomIndex = Common.GetRandomWeightedIndex(clips.Select(c => c.weight).ToArray());

            clips[randomIndex].lastPicked = true;

            return clips[randomIndex];
        }

        public Clip Pick(Clip.AttackType attackType, string requiredSourceSubStance = "None", bool avoidLastPick = false)
        {
            //foreach (Clip clip in animationClips) { clip.lastPicked = false; };

            List<Clip> clips = new List<Clip>();
            Clip lastPicked = null;
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance) continue;
                if (clip.attackType == attackType)
                {
                    if (clip.lastPicked)
                    {
                        clip.lastPicked = false;
                        lastPicked = clip;
                        if (avoidLastPick) continue;
                    }
                    clips.Add(clip);
                }
            }
            if (clips.Count == 0)
            {
                if (avoidLastPick && lastPicked != null) return lastPicked;
                return null;
            }

            int randomIndex = Common.GetRandomWeightedIndex(clips.Select(c => c.weight).ToArray());

            clips[randomIndex].lastPicked = true;

            return clips[randomIndex];
        }

        public List<Clip> GetSubStances(ItemModuleAI.StanceData rightHand, ItemModuleAI.StanceData leftHand)
        {
            List<Clip> clips = new List<Clip>();
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                if (clip.step == Clip.Step.Loop && (clip.loopType == Clip.LoopType.IncludeSubStance || clip.loopType == Clip.LoopType.ExcludeSubStance))
                {
                    if (clip.loopType == Clip.LoopType.IncludeSubStance)
                    {
                        if (rightHand != null && rightHand.exclusionList && rightHand.subStanceNames?.Contains(clip.subStanceName) == true) continue;
                        if (leftHand != null && leftHand.exclusionList && leftHand.subStanceNames?.Contains(clip.subStanceName) == true) continue;
                    }
                    if (clip.loopType == Clip.LoopType.ExcludeSubStance)
                    {
                        // We can't use an excluded sub-stance if neither weapon has stance data to be *able* to include this sub-stance
                        if (rightHand == null && leftHand == null) continue;
                        // Determine which of the two are inclusion lists. if either rightHand or leftHand is null, null != false.
                        bool rightIsInclusion = rightHand?.exclusionList == false;
                        bool leftIsInclusion = leftHand?.exclusionList == false;
                        // If neither are inclusion, continue
                        if (!rightIsInclusion && !leftIsInclusion) continue;
                        // If right is inclusion and the clip name is in the list or the left is inclusion and the name is in the list, this sub-stance is valid for this item pairing. flip output for continue
                        if (!((rightIsInclusion && rightHand?.subStanceNames?.Contains(clip.subStanceName) == true) || (leftIsInclusion && leftHand?.subStanceNames?.Contains(clip.subStanceName) == true))) continue;
                    }
                    clips.Add(clip);
                }
            }
            return clips;
        }
    }
}