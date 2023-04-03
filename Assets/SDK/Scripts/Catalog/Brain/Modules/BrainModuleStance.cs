using UnityEngine;
using System.Collections.Generic;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class BrainModuleStance : BrainData.Module
    {
        public enum Stance
        {
            Idle,
            Fists,
            MeleeShield,
            Melee1H,
            Melee2H,
            DualWield,
            Bow,
            Flee,
            Staff,
            PoleWeapon,
        }

        public float subStanceSwitchTime = 0.25f;
        public AnimationCurve switchCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
#if ODIN_INSPECTOR
        [MinMaxSlider(0f, 20f, showFields: true)]
#endif
        public Vector2 minMaxTimeBetweenChangeSubStance = new Vector2();
        public bool forceHoldUpperOff = false;

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly] 
#endif
        [NonSerialized]
        public Stance stance = Stance.Idle;

    }
}