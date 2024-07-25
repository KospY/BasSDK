using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad.Skill.Spell
{
    public class SpellCastLightning : SpellCastCharge
    {
        public bool speedUpByTimeScale = false;
        public static int modifierChainRange = Animator.StringToHash("ChainRange");
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string imbueHitEffectId;

        [NonSerialized]
        public EffectData imbueHitEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string imbueHitRagdollEffectId;

        [NonSerialized]
        public EffectData imbueHitRagdollEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")] 
#endif
        public float staffSlamRadius = 10f;

#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")] 
#endif
        public float staffSlamStunDuration = 5f;

#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")] 
#endif
        public float staffSlamExpandDuration = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")]
#endif
        public AnimationCurve boltIntensityCurve;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string boltEffectId;
        [NonSerialized]
        public EffectData boltEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string boltLoopEffectId;
        [NonSerialized]
        public EffectData boltLoopEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string boltHitEffectId;
        [NonSerialized]
        public EffectData boltHitEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string boltHitImbueEffectId;
        [NonSerialized]
        public EffectData boltHitImbueEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public int simultaneousBolts = 3;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), MinMaxSlider(0.01f, 0.5f, ShowFields = true)] 
#endif
        public Vector2 intervalMinRange = new Vector2(0.1f, 0.2f);

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), MinMaxSlider(0.01f, 0.5f, ShowFields = true)] 
#endif
        public Vector2 intervalMaxRange = new Vector2(0.05f, 0.1f);

        public float MinDuration => Mathf.Lerp(intervalMaxRange.x, intervalMaxRange.y, 0.5f) / drainPerBolt;

        public float MaxDuration => (1 + efficiencyPerSkill * 3) * (Mathf.Lerp(intervalMaxRange.x, intervalMaxRange.y, 0.5f) / drainPerBolt);
        public string DrainPerBoltLabel
            => $"Drain per Bolt (spray for ~{MinDuration:G02}s base, ~{MaxDuration:G02}s with unlocks)";

        public string StunDurationPerSkillLabel => $"Stun Duration per Skill ({boltElectrocuteDuration}s base, {boltElectrocuteDuration * (1 + durationPerSkill * 3)}s with unlocks)";

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), LabelText("@DrainPerBoltLabel")]
#endif
        public float drainPerBolt = 0.015f;
        
#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), LabelText(@"@SkillPassiveLabel(""Efficiency"", efficiencyPerSkill)")]
#endif
        public float efficiencyPerSkill = 0.05f;
#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), LabelText("@StunDurationPerSkillLabel")]
#endif
        public float durationPerSkill = 0.1f;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float coneAngle = 30;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float boltMaxRange = 5;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float fireDirectionAngle = 70;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float boltDamage = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public int pushLevel = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float boltHaptic = 0.2f;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public int boltElectrocuteDuration = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public int boltElectrocuteDurationMetal = 6;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float boltPushForce = 5f;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float headToFireMaxAngle = 45;
#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float boltLoopFadoutDelay = 0.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public LayerMask rayMaskPlayer;
#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public LayerMask rayMaskNpc;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float hitLookupMaxDiffRange = 0.25f;
#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float boltEnergyTransfer = 2f;
#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float boltElectrocuteChance = 0.3f;

#if ODIN_INSPECTOR
        [BoxGroup("Electrocution"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string electrocuteStatusId = "Electrocute";
        [NonSerialized]
        public StatusData electrocuteStatusData;
        
#if ODIN_INSPECTOR
        [BoxGroup("Chaining")] 
#endif
        public float minChainIntensity = 0.6f;
#if ODIN_INSPECTOR
        [BoxGroup("Chaining")] 
#endif
        public float minChainLength = 0.4f;
#if ODIN_INSPECTOR
        [BoxGroup("Chaining")] 
#endif
        public float maxChainLength = 1.2f;
#if ODIN_INSPECTOR
        [BoxGroup("Chaining")] 
#endif
        public float chainIntensityMultiplier = 0.75f;
#if ODIN_INSPECTOR
        [BoxGroup("Chaining")]
#endif
        public int maxChainsPerBolt = 3;
#if ODIN_INSPECTOR
        [BoxGroup("Chaining")] 
#endif
        public float chainChance = 0.5f;
        
        [NonSerialized]
        public bool allowChainEnemies;
        [NonSerialized]
        public bool allowChainItems;


#if ODIN_INSPECTOR
        [BoxGroup("Arcing Bolt"), ShowIf("allowThrow")] 
#endif
        public float arcDuration = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Arcing Bolt"), ShowIf("allowThrow")] 
#endif
        public float arcJumpRadius = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("Arcing Bolt"), ShowIf("allowThrow")] 
#endif
        public float arcThrowAngle = 30f;
#if ODIN_INSPECTOR
        [BoxGroup("Arcing Bolt"), ShowIf("allowThrow")] 
#endif
        public float arcShockDuration = 1.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Arcing Bolt"), ShowIf("allowThrow")] 
#endif
        public float arcJumpDelay = 1.5f;
        
        public delegate void SprayEvent(SpellCastLightning spell);

        public event SprayEvent OnSprayLoopEvent;
        public event SprayEvent OnSprayStopEvent;

        public delegate void ChainEvent(ColliderGroup source, ColliderGroup target);
        public event ChainEvent OnChainEvent;

        public delegate void BoltHitColliderGroupEvent(
            SpellCastLightning spell,
            ColliderGroup colliderGroup,
            Vector3 position,
            Vector3 normal,
            Vector3 velocity,
            float intensity,
            ColliderGroup source,
            HashSet<ThunderEntity> seenEntities);
        public event BoltHitColliderGroupEvent OnBoltHitColliderGroupEvent;
        public delegate void BoltHitEvent(SpellCastLightning spell, BoltHit hit);
        public event BoltHitEvent OnBoltHitEvent;

        public delegate void ChargeSappingEvent(SpellCastLightning spell, SkillChargeSapping skill, EventTime time, SpellCastCharge other);

        public event ChargeSappingEvent OnChargeSappingEvent;

        public delegate void LightningEvent(SpellCastLightning spell);

        public event LightningEvent OnChargeSappingResetEvent;

        [NonSerialized]
        public Gradient boltColorOverride;

        public void ResetBoltColor()
        {
            boltColorOverride = null;
        }
        public void OverrideBoltColor(Gradient gradient)
        {
            boltColorOverride = gradient;
        }
        
        [Serializable]
        public struct BoltHit
        {
            public BoltHit(Collider collider, Vector3 point, float distance, float angle, Vector3 normal, Vector3 direction)
            {
                this.collider = collider;
                this.closestPoint = point;
                this.distance = distance;
                this.angle = angle;
                this.normal = normal;
                this.direction = direction;
            }
            public Collider collider;
            public Vector3 closestPoint;
            public float distance;
            public float angle;
            public Vector3 normal;
            public Vector3 direction;
        }

        public new SpellCastLightning Clone()
        {
            return this.MemberwiseClone() as SpellCastLightning;
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();

        }

    }
}
