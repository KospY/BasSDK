using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Item magnet")]
    [RequireComponent(typeof(Collider))]
    public class ItemMagnet : MonoBehaviour
    {
        public FilterLogic tagFilter = FilterLogic.AnyExcept;
#if PrivateSDK
        [ValueDropdown("GetAllHolderSlots")]
#endif
        public List<string> slots;

        public bool autoUngrab;
        public float gravityRatio = 0;
        public float sleepThresholdRatio = 0;
        public int maxCount = 1;

        public float stabilizedMaxDistance = 0.01f;
        public float stabilizedMaxAngle = 360f;
        public float stabilizedMaxUpAngle = 10f;

        public float positionSpring = 200f;
        public float positionDamper = 10f;
        public float positionMaxForce = 1000f;

        public float rotationSpring = 1000f;
        public float rotationDamper = 10f;
        public float rotationMaxForce = 10000f;

#if PrivateSDK
        public List<ValueDropdownItem<string>> GetAllHolderSlots()
        {
            return Catalog.GetDropdownHolderSlots();
        }
#endif

    }
}
