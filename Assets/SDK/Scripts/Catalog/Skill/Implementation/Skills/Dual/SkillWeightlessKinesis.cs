#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using ThunderRoad.Skill.SpellPower;
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillWeightlessKinesis : TkSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusId;
        protected StatusData statusData;
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ShowInInspector]
#endif
        public FloatingParams itemParams = FloatingParams.Identity;
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect"), ShowInInspector]
#endif
        public FloatingParams creatureParams = FloatingParams.Identity;
#if ODIN_INSPECTOR
        [BoxGroup("Status Effect")]
#endif
        public float throwEffectDuration = 2f;
#if ODIN_INSPECTOR
        [BoxGroup("Telekinesis")]
#endif
        public float strengthMult = 2f;

#if ODIN_INSPECTOR
        [BoxGroup("Telekinesis")]
#endif
        public bool allowCreature = false;

    }
}
