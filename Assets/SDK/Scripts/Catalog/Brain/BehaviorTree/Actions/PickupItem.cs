using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.AI.Action
{
	public class PickupItem : ActionNode
    {
        public string inputItemHandleVariableName = "";
        public float pickupIKDuration = 0.75f;
        public float pickupDuration = 1.05f;
        public bool useIK = false;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllAnimationID))] 
#endif
        [Header("Animation")]
        public string pickupAnimationId;
        [NonSerialized]
        public AnimationData pickupAnimationData;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllAnimationID()
        {
            return Catalog.GetDropdownAllID(Category.Animation);
        } 
#endif

    }
}

