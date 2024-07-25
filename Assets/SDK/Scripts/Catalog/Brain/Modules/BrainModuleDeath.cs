using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleDeath : BrainData.Module
    {
#if ODIN_INSPECTOR
        [CustomContextMenu("Copy to other brains", nameof(CopyCurveAcrossBrains))]
#endif
        public AnimationCurve curve = AnimationCurve.Linear(0, 1, 1, 0);
        public float maxMovingVelocity = 3f;
        public float unpinDistance = 1f;
        public float unpinMaxAngle = 70f;
        public Vector2 dropObjectsDelay = new Vector2(0, 0.5f);
        public bool useDamagerDyingAnimMaxVelocity = false;
        public float grabThrowMinVelocity = 2.0f;

        [Header("Animation")]
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllAnimationID))] 
#endif
        public string defaultAnimationId;
        [NonSerialized]
        public AnimationData defaultAnimationData;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllAnimationID))] 
#endif
        public string stabAnimationId;
        [NonSerialized]
        public AnimationData stabAnimationData;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllAnimationID))] 
#endif
        public string fireAnimationId = "HumanFireDeaths";
        [NonSerialized]
        public AnimationData fireAnimationData;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllAnimationID))] 
#endif
        public string lightningAnimationId = "HumanLightningDeaths";
        [NonSerialized]
        public AnimationData lightningAnimationData;

        public enum DeathType
        {
            Default,
            Stab,
            Fire,
            Lightning
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllAnimationID()
        {
            return Catalog.GetDropdownAllID(Category.Animation);
        }

        protected void CopyCurveAcrossBrains()
        {
            foreach (BrainData bd in Catalog.GetDataList<BrainData>())
            {
                if (bd.GetModule<BrainModuleDeath>(false) is BrainModuleDeath deathModule) deathModule.curve = this.curve;
            }
        }
#endif

    }
}