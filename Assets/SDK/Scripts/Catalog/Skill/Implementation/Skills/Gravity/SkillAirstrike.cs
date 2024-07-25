#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace ThunderRoad.Skill.Spell
{
    public class SkillAirstrike : SkillSpellPunch
    {
#if ODIN_INSPECTOR
        [BoxGroup("Fall Damage")]
#endif
        public float fallDamageScale = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public float maxAngle = 70f;
#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public float minDownwardVelocity = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public float minHeight = 4f;
#if ODIN_INSPECTOR
        [BoxGroup("Dive")]
#endif
        public float maxHeightDistance = 3f;
#if ODIN_INSPECTOR
        [BoxGroup("Dive"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string whooshEffectId;

        protected EffectData whooshEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Shockwave")]
#endif
        public float force = 4f;
#if ODIN_INSPECTOR
        [BoxGroup("Shockwave")]
#endif
        public float upwardsModifier = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("Shockwave")]
#endif
        public int shockwavePushLevel = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Shockwave")]
#endif
        public float distanceMult = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Shockwave")]
#endif
        public float shockwaveDamage = 30f;
#if ODIN_INSPECTOR
        [BoxGroup("Shockwave"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string shockwaveEffectId;

        protected EffectData shockwaveEffectData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Shockwave"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string playerEffectId;

        protected EffectData playerEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Shockwave")]
#endif
        public float radius = 4f;
#if ODIN_INSPECTOR
        [BoxGroup("Shake")]
#endif
        public bool shake = true;
#if ODIN_INSPECTOR
        [BoxGroup("Shake"), ShowIf(nameof(shake))]
#endif
        public float shakeDuration = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Shake"), ShowIf(nameof(shake))]
#endif
        public float shakeIntensity = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Shake"), ShowIf(nameof(shake)), MinMaxSlider(0, 0.1f)]
#endif
        public Vector2 minMaxShakeIntensity = new Vector2(0.005f, 0.01f);
#if ODIN_INSPECTOR
        [BoxGroup("Shake"), ShowIf(nameof(shake))]
#endif
        public AnimationCurve shakeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    }
}