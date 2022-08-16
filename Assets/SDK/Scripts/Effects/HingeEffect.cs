using UnityEngine;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/HingeEffect")]
    [AddComponentMenu("ThunderRoad/Effects/Hinge effect")]
    public class HingeEffect : ThunderBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllEffectID")]
#endif
        public string effectId;
        public float minTorque = 5;
        public float maxTorque = 12;
        public HingeJoint joint;

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Effect);
        }
#endif

    }
}
