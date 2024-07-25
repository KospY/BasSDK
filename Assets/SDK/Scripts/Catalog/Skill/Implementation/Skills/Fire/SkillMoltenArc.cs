#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillMoltenArc : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Effect"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string moltenEffectId;
        protected EffectData moltenEffectData;

        [BoxGroup("Effect"), MinMaxSlider(0f, 20f, true)]
        public Vector2 minMaxVelocity = new(5, 16);

        [BoxGroup("Effect"), Range(0, 1)]
        public float maxIntensityEnemy = 1;

        [BoxGroup("Effect"), Range(0, 1)]
        public float maxIntensityPlayer = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Damage")]
#endif
        public float damage = 3;

#if ODIN_INSPECTOR
        [BoxGroup("Status"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string statusId = "Burning";

        protected StatusData statusData;

#if ODIN_INSPECTOR
        [BoxGroup("Status")]
#endif
        public float statusDuration = 3;

#if ODIN_INSPECTOR
        [BoxGroup("Status")]
#endif
        public float sparkHeatTransferEnemy = 3f;

#if ODIN_INSPECTOR
        [BoxGroup("Status")]
#endif
        public float sparkHeatTransferPlayer = 5f;
    }
}
