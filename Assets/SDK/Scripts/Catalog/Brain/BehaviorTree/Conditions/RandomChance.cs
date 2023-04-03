using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.AI.Condition
{
    public class RandomChance : ConditionNode
    {
        [Range(0.01f, 0.99f)]
        public float chance = 0.5f;
        public float valueUpdateFrequency = 10f;

    }
}
