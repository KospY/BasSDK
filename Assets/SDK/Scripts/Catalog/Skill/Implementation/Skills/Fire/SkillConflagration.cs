
using UnityEngine;
using UnityEngine.Serialization;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillConflagration : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Spread")] 
#endif
        public float radius = 3;
#if ODIN_INSPECTOR
        [BoxGroup("Spread")] 
#endif
        public float heatTransfer = 0.3f;
#if ODIN_INSPECTOR
        [BoxGroup("Spread")] 
#endif
        public float heatTransferToDeadEnemy = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Spread")] 
#endif
        public float heatTransferToCharredEnemy = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("Spread")] 
#endif
        public AnimationCurve rangeCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}
