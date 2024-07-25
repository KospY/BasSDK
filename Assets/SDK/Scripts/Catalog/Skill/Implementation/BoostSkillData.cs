#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class BoostSkillData : SkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Boost"), ValueDropdown(nameof(GetAllSkillID))]
#endif
        public string boostJumpSkillId = "BoostJump";


        public virtual void OnJump(
            SkillBoostJump skill,
            SpellCastGravity gravity,
            Vector3 pushForce,
            Vector3 jumpForce,
            bool onGround)
        {
        }
    }
}
