using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
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

#if ODIN_INSPECTOR
        [BoxGroup("General"), HorizontalGroup("General/Death"), LabelWidth(100)] 
#endif
        public bool useDeathCurve = true;

#if ODIN_INSPECTOR
        [BoxGroup("General"), HorizontalGroup("General/Death"), LabelWidth(100)] 
#endif
        public bool allowRootMotion = false;

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
        [BoxGroup("Auto-fill"), HorizontalGroup("Auto-fill/Horiz"), LabelWidth(170), ShowInInspector, ValueDropdown(nameof(GetAllAnimationDataID)), InlineButton("CrossReference", "Copy attack window info"), InlineButton("SetAllNonAttack", "Set all clips non-attack")] 
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
        [BoxGroup("Animation"), OnValueChanged(nameof(CalculateWeight))] 
#endif
        public List<Clip> animationClips = new List<Clip>();

        [NonSerialized]
        protected AnimationData[] difficultyFilteredDatas;

        public AnimationData this[int difficulty]
        {
            get
            {
                if (!usesDifficulty || difficulty >= difficultyFilteredDatas.Length)
                    return this;
                return difficultyFilteredDatas[difficulty];
            }
        }
        
        private float probabilityRNGTotalWeight, probabilityDeltaTotalWeight;



        [Serializable]
        public class Clip
        {
#if ODIN_INSPECTOR
            [GUIColor(nameof(EditorGetColor))]
            [HorizontalGroup("Horiz"), BoxGroup("Horiz/Default"), LabelWidth(50)] 
#endif
            [JsonMergeKey]
            public string address;
#if ODIN_INSPECTOR
            [GUIColor(nameof(EditorGetColor))]
            [HorizontalGroup("Horiz/Default/Horiz"), LabelWidth(100), ShowInInspector, DisableInPlayMode]
#if UNITY_EDITOR
            [OnValueChanged(nameof(EditorUpdateAddress))]
#endif
            
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
            [HorizontalGroup("Horiz/Default/Horiz2"), LabelWidth(100), ShowIf("step", optionalValue: Step.Loop), DisableIf("loopType", optionalValue: LoopType.NonSpecific)] 
#if UNITY_EDITOR
            [OnValueChanged(nameof(ValidateName))]
#endif
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
            [HorizontalGroup("Horiz/Combat info/Horiz1", Width = 250), BoxGroup("Horiz/Combat info"), LabelWidth(120)] 
#endif
            public CombatMotion attackType = CombatMotion.None;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Combat info/Horiz1", Width = 250), LabelWidth(120), DisableIf("attackType", optionalValue: CombatMotion.None)] 
#endif
            public MotionType attackMotion = MotionType.RightSwing;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Combat info/Horiz2", Width = 250), LabelWidth(120), DisableIf("attackType", optionalValue: CombatMotion.None)] 
#endif
            public Vector2 startAndEndTime;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Combat info/Horiz2", Width = 250), LabelWidth(120), DisableIf("attackType", optionalValue: CombatMotion.None)]
#if UNITY_EDITOR
	        [ValueDropdown(nameof(GetSourceSubStanceOptions))]
#endif
#endif
            public string sourceSubStance = "Any";
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Combat info/Horiz3", Width = 250), LabelWidth(120), DisableIf("attackType", optionalValue: CombatMotion.None)] 
#endif
            public float peakRangeTime;
#if ODIN_INSPECTOR
            [HorizontalGroup("Horiz/Combat info/Horiz3", Width = 250), LabelWidth(120),  DisableIf("attackType", optionalValue: CombatMotion.None)] 
#if UNITY_EDITOR
            [ValueDropdown(nameof(GetEndSubStanceOptions))]
#endif
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
            [HorizontalGroup("Horiz", MinWidth = 180), BoxGroup("Horiz/RNG"), HorizontalGroup("Horiz/RNG/Horiz1"), LabelWidth(50)] 
#if UNITY_EDITOR
            [OnValueChanged(nameof(EditorCalculateWeight))]
#endif
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

            public enum CombatMotion { None, SingleStrike, ComboAttack, Riposte, ChainRiposte, Push, Ranged }

            public enum MotionType { RightSwing, RightThrust, LeftSwing, LeftThrust, }

            public enum Direction { None, Forward, Backward, Left, Right, Up, Bottom, }

            public enum Height { None, Lower, Mid, Up, }


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
                if (animationClip)
                    address = Catalog.GetAddressFromPrefab(animationClip);
            }

            protected void ValidateName()
            {
                if (subStanceName == "Any" || subStanceName == "Previous" || subStanceName == "RandomInc" || subStanceName == "RandomExc")
                    subStanceName = "";
            }
#endif
            
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

            public enum Step
            {
                Start,
                Loop,
                End,
            }
        }

        public void CalculateWeight() => CalculateWeight("None");

        public void CalculateWeight(string requiredSourceSubStance, bool avoidLastPick = false)
        {
            if (string.IsNullOrEmpty(requiredSourceSubStance))
                requiredSourceSubStance = "None";
            if (animationClips == null || animationClips.Count == 0)
                return;

            float weightMaximum = 0f;
            // Sets the weight ranges of the selected items.
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance)
                    continue;
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
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance)
                    continue;
                if (clip.lastPicked)
                {
                    clip.lastPicked = false;
                    if (avoidLastPick)
                        continue;
                }
                clip.lastCalculatedProbabilityRNG = ((clip.weight) / probabilityRNGTotalWeight) * 100;
            }
        }

                public void CalculateWeightDelta(float targetDistance, float movedDistance, float weaponReach, float maxdelta, float animSpeedMult) => CalculateWeightDelta(targetDistance, movedDistance, weaponReach, maxdelta, animSpeedMult, "None");

        public void CalculateWeightDelta(float targetDistance, float movedDistance, float weaponReach, float maxdelta, float animSpeedMult, string requiredSourceSubStance, bool avoidLastPick = false)
        {
            if (string.IsNullOrEmpty(requiredSourceSubStance))
                requiredSourceSubStance = "None";
            if (animationClips == null || animationClips.Count == 0)
                return;

            float weightMaximum = 0f;
            // Sets the weight ranges of the selected items.
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
                // Skip clips which are loop or end clips
                if (clip.step != Clip.Step.Start)
                    continue;
                // If there's a required source sub-stance and it doesn't match, skip this clip
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance)
                    continue;
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
                    if (animationKeyTimes.y == 0)
                    {
                        // Make sure there's a defined peak range time even if the JSON doesn't have it
                        // Peak range time is used to determine how close to the peak attack range the player is/will be, to help calculate the delta
                        animationKeyTimes.y = clip.animationClip.length * 0.5f;
                    }
                    // Scales the attack window based on the animation speed
                    clip.lastCalculatedTimes = new Vector3(animationKeyTimes.x / animationFinalSpeed, animationKeyTimes.y / animationFinalSpeed, animationKeyTimes.z / animationFinalSpeed);

                    bool useWeaponReach = clip.attackType == Clip.CombatMotion.SingleStrike || clip.attackType == Clip.CombatMotion.ComboAttack;
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
                if (clip.step != Clip.Step.Start)
                    continue;
                if (requiredSourceSubStance != "None" && clip.sourceSubStance != "Any" && clip.sourceSubStance != requiredSourceSubStance)
                    continue;
                if (clip.lastPicked)
                {
                    clip.probabilityDelta = 0;
                    clip.lastPicked = false;
                    if (avoidLastPick)
                        continue;
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

    }
}