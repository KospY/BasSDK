using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;


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
    public class MeleeStanceData : StanceIdlesData<MeleeIdle>
    {
        /*[Range(0f, 10f)]
#if ODIN_INSPECTOR
        [FoldoutGroup("Attacking")]
#endif
        public float debugTargetRange;*/
#if ODIN_INSPECTOR
        [BoxGroup("Attacking")]
#endif
        public bool alternateAttackSides = false;
#if ODIN_INSPECTOR
        [BoxGroup("Attacking")]
#endif
        public BodyChainBehaviour chainAttacksOnBody = BodyChainBehaviour.Disabled;
#if ODIN_INSPECTOR
        [BoxGroup("Attacking"), OnValueChanged(nameof(SetOpenNoMovement))]
#endif
        public List<AttackMotion> openingAttacks = new List<AttackMotion>();
#if ODIN_INSPECTOR
        [BoxGroup("Attacking"), OnValueChanged(nameof(SetChainNoMovement))]
#endif
        public List<AttackMotion> chainAttacks = new List<AttackMotion>();
#if ODIN_INSPECTOR
        [FoldoutGroup("Parrying", expanded: true)]
#endif
        public BrainModuleDefense.StanceDefenseSettings defenseSettings;
#if ODIN_INSPECTOR
        [HorizontalGroup("Parrying/Upper", Width = (1f / 3f) - 0.02f, MarginLeft = 0.01f + ((1f - (0.04f + (2 * ((1 / 3f) - 0.02f)))) / 2f), MarginRight = 0.01f)]
        [BoxGroup("Parrying/Upper/Upper left", ShowLabel = false), HideLabel]
#endif
        public GuardPose upperLeftGuard = new GuardPose() { id = "GuardUpperLeft" };
#if ODIN_INSPECTOR
        [HorizontalGroup("Parrying/Upper", Width = (1f / 3f) - 0.02f, MarginLeft = 0.01f)]
        [BoxGroup("Parrying/Upper/Upper right", ShowLabel = false), HideLabel]
#endif
        public GuardPose upperRightGuard = new GuardPose() { id = "GuardUpperRight" };
#if ODIN_INSPECTOR
        [HorizontalGroup("Parrying/Mid", Width = (1f / 3f) - 0.02f, MarginLeft = 0.01f, MarginRight = 0.01f)]
        [BoxGroup("Parrying/Mid/Middle left", ShowLabel = false), HideLabel]
#endif
        public GuardPose midLeftGuard = new GuardPose() { id = "GuardMidLeft" };
#if ODIN_INSPECTOR
        [HorizontalGroup("Parrying/Mid", Width = (1f / 3f) - 0.02f, MarginLeft = 0.01f, MarginRight = 0.01f)]
        [BoxGroup("Parrying/Mid/Neutral", ShowLabel = false), HideLabel]
#endif
        public GuardPose neutralGuard = new GuardPose() { id = "GuardNeutral" };
#if ODIN_INSPECTOR
        [HorizontalGroup("Parrying/Mid", Width = (1f / 3f) - 0.02f, MarginLeft = 0.01f, MarginRight = 0.01f)]
        [BoxGroup("Parrying/Mid/Middle right", ShowLabel = false), HideLabel]
#endif
        public GuardPose midRightGuard = new GuardPose() { id = "GuardMidRight" };
#if ODIN_INSPECTOR
        [HorizontalGroup("Parrying/Lower", Width = (1f / 3f) - 0.02f, MarginLeft = 0.01f + ((1f - (0.04f + (2 * ((1 / 3f) - 0.02f)))) / 2f), MarginRight = 0.01f)]
        [BoxGroup("Parrying/Lower/Lower left", ShowLabel = false), HideLabel]
#endif
        public GuardPose lowerLeftGuard = new GuardPose() { id = "GuardLowerLeft" };
#if ODIN_INSPECTOR
        [HorizontalGroup("Parrying/Lower", Width = (1f / 3f) - 0.02f, MarginLeft = 0.01f)]
        [BoxGroup("Parrying/Lower/Lower right", ShowLabel = false), HideLabel]
#endif
        public GuardPose lowerRightGuard = new GuardPose() { id = "GuardLowerRight" };
#if ODIN_INSPECTOR
        [FoldoutGroup("Parrying"), OnValueChanged(nameof(SetRiposteNoMovement))]
#endif
        public List<AttackMotion> ripostes = new List<AttackMotion>();

        public enum BodyChainBehaviour { Disabled, UseChance, Always }

        private int openersLastCount = 0;

        private int chainsLastCount = 0;

        private int ripostesLastCount = 0;

        protected void SetOpenNoMovement() => SetNewestNoMovement(openingAttacks, ref openersLastCount);

        protected void SetChainNoMovement() => SetNewestNoMovement(chainAttacks, ref chainsLastCount);

        protected void SetRiposteNoMovement() => SetNewestNoMovement(ripostes, ref ripostesLastCount);

        protected void SetNewestNoMovement(List<AttackMotion> list, ref int lastCount)
        {
            if (lastCount == 0 && list.Count > 1)
            {
                lastCount = list.Count;
            }
            if (lastCount < list.Count)
            {
                list[list.Count - 1].allowPlayAndMove = false;
            }
            lastCount = list.Count;
        }
        
    }
}
