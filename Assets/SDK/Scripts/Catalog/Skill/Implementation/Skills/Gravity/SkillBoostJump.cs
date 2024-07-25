using ThunderRoad.Skill.SpellMerge;
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillBoostJump : SpellSkillData
    {
        public const string JumpCountVar = "GravityJumpCount";
        public float playerBoostForce = 300f;
        public float bubbleBoostMultiplier = 1.3f;
        public int diminishingLimit = 8;

        public delegate void OnJump(SkillBoostJump skill, SpellCastGravity gravity, Vector3 pushForce, Vector3 jumpForce, bool onGround);
        public event OnJump OnJumpEvent;

    }
}
