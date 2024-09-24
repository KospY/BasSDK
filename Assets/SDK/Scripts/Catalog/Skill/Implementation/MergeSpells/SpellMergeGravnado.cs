using System.Collections;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using ThunderRoad.Skill.Spell;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ThunderRoad.Skill.SpellMerge
{
    public class SpellMergeGravnado : SpellMergeData
    {
#if ODIN_INSPECTOR
        [BoxGroup("Tornado"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string tornadoId;

#if ODIN_INSPECTOR
        [BoxGroup("Tornado")]
#endif
        public float tornadoDuration = 10f;

#if ODIN_INSPECTOR
        [BoxGroup("Tornado")]
#endif
        public float aimDistance = 3.5f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Preview"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string previewEffectId = "SpellLightningBoltZapSingle";

        protected EffectData previewEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Preview"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string previewLoopEffectId;
        protected EffectData previewLoopEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Preview")]
#endif
        public float minChargeForPreview = 0.3f;
#if ODIN_INSPECTOR
        [BoxGroup("Preview")]
#endif
        public float previewBoltDelay = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Preview")]
#endif
        public float previewZapBoltDelay = 2f;

#if ODIN_INSPECTOR
        [BoxGroup("Preview")]
#endif
        public float previewZapForce = 10f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Preview")]
#endif
        public Gradient previewBoltGradient;

#if ODIN_INSPECTOR
        [BoxGroup("Preview"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string electrocuteStatusId = "Electrocute";

        protected StatusData electrocuteStatusData;

#if ODIN_INSPECTOR
        [BoxGroup("Preview"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string floatingStatusId = "Floating";
        protected StatusData floatingStatusData;

#if ODIN_INSPECTOR
        [BoxGroup("Hand Gesture")]
#endif
        public float handSeparationMaxAngle = 45f;

#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float radius = 5f;

#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public AnimationCurve radialForce = Utils.Curve(0, 1, 1, 1).Multiply(-50);
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public AnimationCurve swirlForce = Utils.Curve(0, 1, 1, 1).Multiply(50);
        
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public AnimationCurve linearForce = Utils.Curve(0, 1, 1, 1).Multiply(50);

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), MinMaxSlider(0, 5, true)]
#endif
        public Vector2 boltDelay = new(1, 3);
    
#if ODIN_INSPECTOR
        [BoxGroup("Bolts")]
#endif
        public string thunderboltId = "Thunderbolt";

        protected SkillThunderbolt thunderboltData;
    
#if ODIN_INSPECTOR
        [BoxGroup("Bolts")]
#endif
        public float boltRadius = 10f;
    
        protected EffectData tornadoData;
        protected float lastPreviewBolt;
        protected float lastPreviewZapBolt;
        protected Coroutine routine;
        public float yeetRadius = 1f;
        public float yeetHeight = 10f;
        public static bool tornadoActive;

    }
}
