
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SkillInfernoStaff : SpellSkillData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Flamethrower"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string flamethrowerEffectId = "Flamethrower";

        public EffectData flamethrowerEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Flamethrower")]
#endif
        public float sphereRadius = 3f;
#if ODIN_INSPECTOR
        [BoxGroup("Flamethrower")]
#endif
        public float tickDelay = 0.3f;
#if ODIN_INSPECTOR
        [BoxGroup("Flamethrower")]
#endif
        public float damagePerTick = 1;
#if ODIN_INSPECTOR
        [BoxGroup("Flamethrower")]
#endif
        public LayerMask layerMask;
#if ODIN_INSPECTOR
        [BoxGroup("Flamethrower")]
#endif
        public float forceMult = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Flamethrower")]
#endif
        public float heatTransferPerSecond = 70;
#if ODIN_INSPECTOR
        [BoxGroup("Flamethrower"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string burningId = "Burning";
#if ODIN_INSPECTOR
        [BoxGroup("Flamethrower")]
#endif
        public AnimationCurve pushbackCurve = AnimationCurve.EaseInOut(0, 2, 0.5f, 1);
#if ODIN_INSPECTOR
        [BoxGroup("Flamethrower")]
#endif
        public float pushbackForce = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Flamethrower")]
#endif
        public ForceMode pushbackForceMode = ForceMode.Acceleration;

        [NonSerialized]
        public StatusData burning;
    }
}