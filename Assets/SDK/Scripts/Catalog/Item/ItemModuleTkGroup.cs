using System.Collections.Generic;
using ThunderRoad.Skill.SpellPower;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    public class ItemModuleTkGroup : ItemModule
    {
        public float range = 0.1f;
        public float spring = 100;
        public float damper = 10f;
        public float massScale = 10f;
#if ODIN_INSPECTOR
        [MinMaxSlider(0, 0.03f, showFields: true)]
#endif
        public Vector2 distance = new(0, 0.03f);

    }

}
