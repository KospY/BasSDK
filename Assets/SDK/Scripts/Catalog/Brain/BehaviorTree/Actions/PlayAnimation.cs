using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.AI.Action
{
    public class PlayAnimation : ActionNode
    {
        [Header("Animation")]
        public bool useAddressableAddress = false;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllAnimationID))]
        [HideIf("useAddressableAddress")]
#endif
        public string animationDataID;
#if ODIN_INSPECTOR
        [HideIf("useAddressableAddress")]
#endif
        public bool overrideSpeed;
#if ODIN_INSPECTOR
        [ShowIf("useAddressableAddress")]
#endif
        public string animationAddress;
#if ODIN_INSPECTOR
        [ShowIf("@this.useAddressableAddress || this.overrideSpeed")]
#endif
        public float animationSpeed = 1f;
        public PlayType playType = PlayType.OneShot;
        public bool avoidLastPick = false;
        public bool upperOnly = false;
        public Vector2 animDurationMinMax = new Vector2(1f, 3f);
        public bool exitAutomatically = true;
        public bool interruptStop = true;
        public bool mirror = false;
        [Header("Tree")]
        public bool doCallbackNode;
#if ODIN_INSPECTOR
        [EnableIf("doCallbackNode")]
#endif
        public Node callbackNode;

        public enum PlayType
        {
            OneShot,
            Loop,
            ThreeStep,
            LimitedOneShot,
            LimitedThreeStep,
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllAnimationID()
        {
            return Catalog.GetDropdownAllID(Category.Animation);
        } 
#endif

    }
}
