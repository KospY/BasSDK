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
        public List<ValueDropdownItem<string>> GetAllAnimationDataID()
        {
            return Catalog.GetDropdownAllID(Category.Animation);
        }
#endif

#if ODIN_INSPECTOR
        [BoxGroup("Animation"), OnValueChanged(nameof(CalculateWeight))] 
#endif
        public List<Clip> animationClips = new List<Clip>();

#if ODIN_INSPECTOR
        [BoxGroup("Death Settings"), HorizontalGroup("Death Settings/Horiz", Width = 125), LabelWidth(100)]
#endif
        public bool useDeathCurve = true;

#if ODIN_INSPECTOR
        [BoxGroup("Death Settings"), HorizontalGroup("Death Settings/Horiz", Width = 135), LabelWidth(110)]
#endif
        public bool allowRootMotion = false;

        private float probabilityRNGTotalWeight, probabilityDeltaTotalWeight;



        [Serializable]
        public class Clip
        {
#if ODIN_INSPECTOR
            [GUIColor(nameof(EditorGetColor))]
            [HorizontalGroup("MasterHoriz"), BoxGroup("MasterHoriz/Default"), LabelWidth(50)] 
#endif
            [JsonMergeKey]
            public string address;
#if ODIN_INSPECTOR
            [GUIColor(nameof(EditorGetColor))]
            [HorizontalGroup("MasterHoriz/Default/Horiz"), LabelWidth(100), ShowInInspector, DisableInPlayMode]
#if UNITY_EDITOR
            [OnValueChanged(nameof(EditorUpdateAddress))]
#endif
            
#endif
            [NonSerialized]
            public AnimationClip animationClip;
#if ODIN_INSPECTOR
            [HorizontalGroup("MasterHoriz/Default/Horiz", Width = 110), LabelWidth(30)] 
#endif
            public Step step = Step.Start;
#if ODIN_INSPECTOR
            [HorizontalGroup("MasterHoriz/Default/Horiz", Width = 110), LabelWidth(70), LabelText("Anim Speed")] 
#endif
            public float animationSpeed = 1;

#if ODIN_INSPECTOR
            [HorizontalGroup("MasterHoriz", MinWidth = 180), BoxGroup("MasterHoriz/Config"), HorizontalGroup("MasterHoriz/Config/Horiz1"), LabelWidth(100)] 
#endif
            public Direction moveDirection = Direction.None;

#if ODIN_INSPECTOR
            [HorizontalGroup("MasterHoriz/Config/Horiz1"), LabelWidth(80)] 
#endif
            public Direction aimDirection = Direction.None;
#if ODIN_INSPECTOR
            [HorizontalGroup("MasterHoriz/Config/Horiz2"), LabelWidth(70)] 
#endif
            public Height aimHeight = Height.None;
#if ODIN_INSPECTOR
            [HorizontalGroup("MasterHoriz/Config/Horiz2"), LabelWidth(80)]
#endif
            public float aimEndTime;

#if ODIN_INSPECTOR
            [HorizontalGroup("MasterHoriz", MinWidth = 180), BoxGroup("MasterHoriz/RNG"), HorizontalGroup("MasterHoriz/RNG/Horiz1"), LabelWidth(50)] 
#if UNITY_EDITOR
            [OnValueChanged(nameof(EditorCalculateWeight))]
#endif
#endif
            public float weight = 1;
#if ODIN_INSPECTOR
            [HorizontalGroup("MasterHoriz/RNG/Horiz1"), LabelWidth(50)] 
#endif
            public float chance = 1;
#if ODIN_INSPECTOR
            [HorizontalGroup("MasterHoriz/RNG/Horiz2"), HideLabel, ReadOnly, ShowInInspector, ProgressBar(0, 100)] 
#endif
            [NonSerialized]
            public float lastCalculatedProbabilityRNG;
#if ODIN_INSPECTOR
            [HorizontalGroup("MasterHoriz/RNG/Horiz2"), HideLabel, ReadOnly, ShowInInspector, ProgressBar(0, 100, 0.74f, 0.47f, 0.15f)] 
#endif
            [NonSerialized]
            public float probabilityDelta;

            [NonSerialized]
            public AnimationData container;
            [NonSerialized]
            public bool lastPicked;
            [NonSerialized]
            public float lastCalculatedWeightFrom, lastCalculatedWeightTo;

            public enum Direction { None, Forward, Backward, Left, Right, Up, Bottom, }

            public enum Height { None, Lower, Mid, Up, }



            [NonSerialized]
            public bool isDuplicate = false;
            [NonSerialized]
            public bool isMissing = false;

            protected Color EditorGetColor() { return isMissing ? Color.yellow : (isDuplicate ? Color.red : (lastPicked ? Color.green : Color.white)); }
#if UNITY_EDITOR
            protected void EditorCalculateWeight()
            {
                List<CatalogData> list = Catalog.GetDataList(Category.Animation);
                for (int i = 0; i < list.Count; i++)
                {
                    AnimationData animationData = (AnimationData)list[i];
                    animationData.CalculateWeight();
                }
            }

            protected void EditorUpdateAddress()
            {
                if (animationClip)
                    address = Catalog.GetAddressFromPrefab(animationClip);
            }
#endif

            public enum Step
            {
                Start,
                Loop,
                End,
            }
        }

        public void CalculateWeight() => CalculateWeightStrict();

        public void CalculateWeightStrict(bool avoidLastPick = false)
        {
            if (animationClips == null || animationClips.Count == 0)
                return;

            float weightMaximum = 0f;
            // Sets the weight ranges of the selected items.
            for (int i = 0; i < animationClips.Count; i++)
            {
                Clip clip = animationClips[i];
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
                if (clip.lastPicked)
                {
                    clip.lastPicked = false;
                    if (avoidLastPick)
                        continue;
                }
                clip.lastCalculatedProbabilityRNG = ((clip.weight) / probabilityRNGTotalWeight) * 100;
            }
        }

    }
}