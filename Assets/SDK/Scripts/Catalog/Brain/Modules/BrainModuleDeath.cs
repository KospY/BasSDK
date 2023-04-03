using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class BrainModuleDeath : BrainData.Module
    {
        public AnimationCurve curve = AnimationCurve.Linear(0, 1, 1, 0);
        public float maxMovingVelocity = 3f;
        public float unpinDistance = 0.3f;
        public float unpinMaxAngle = 70f;
        public Vector2 dropObjectsDelay = new Vector2(0, 0.5f);
        public bool useDamagerDyingAnimMaxVelocity = false;
        public float grabThrowMinVelocity = 2.0f;

        [Header("Animation")]
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllAnimationID")] 
#endif
        public string defaultAnimationId;
        [NonSerialized]
        public AnimationData defaultAnimationData;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllAnimationID")] 
#endif
        public string stabAnimationId;
        [NonSerialized]
        public AnimationData stabAnimationData;

        public enum DeathType
        {
            Default,
            Stab,
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllAnimationID()
        {
            return Catalog.GetDropdownAllID(Category.Animation);
        } 
#endif

    }
}