using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class SpellCastLightning : SpellCastCharge
    {
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit"), ValueDropdown("GetAllEffectID")] 
#endif
        public string imbueHitEffectId;

        [NonSerialized]
        public EffectData imbueHitEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit"), ValueDropdown("GetAllEffectID")] 
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
        public float staffSlamExpandDuration = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public float beamShockDuration = 3f;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public float beamThickness = 0.1f;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public float beamDuration = 5f;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public float minBeamNodeDistance = 0.3f;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public float maxBeamNodeInputVelocity = 8f;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public float beamNodeVelocityMult = 2;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public float beamNodeDrag = 3f;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public float imbueCostPerBeamNode = 2;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public float dismembermentMaxAngle = 60;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public float sliceWidthMultiplier = 2.5f;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire"), ValueDropdown("GetAllEffectID")] 
#endif
        public string arcEffectId;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public int arcWirePushLevel = 1;

        [NonSerialized]
        public EffectData arcEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire"), ValueDropdown("GetAllEffectID")] 
#endif
        public string arcStaffEffectId;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire"), ValueDropdown("GetAllEffectID")] 
#endif
        public string arcWireHitEffectId;

        [NonSerialized]
        protected EffectData arcStaffEffectData;
        [NonSerialized]
        public EffectData arcWireHitEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), ValueDropdown("GetAllEffectID")] 
#endif
        public string boltEffectId;
        [NonSerialized]
        public EffectData boltEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), ValueDropdown("GetAllEffectID")] 
#endif
        public string boltLoopEffectId;
        [NonSerialized]
        public EffectData boltLoopEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), ValueDropdown("GetAllEffectID")] 
#endif
        public string boltHitEffectId;
        [NonSerialized]
        public EffectData boltHitEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Bolts"), ValueDropdown("GetAllEffectID")] 
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
        public float disarmChanceNPC = 0.8f;
#if ODIN_INSPECTOR
        [BoxGroup("Bolts")] 
#endif
        public float disarmChancePlayer = 0.3f;

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
        public float chainChance = 0.5f;

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

#if ODIN_INSPECTOR
        [BoxGroup("Gripped Ragdoll Electrocution")] 
#endif
        public float gripZapDuration = 5;


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
            boltEffectData = Catalog.GetData<EffectData>(boltEffectId);
            arcEffectData = Catalog.GetData<EffectData>(arcEffectId);
            arcStaffEffectData = Catalog.GetData<EffectData>(arcStaffEffectId);
            arcWireHitEffectData = Catalog.GetData<EffectData>(arcWireHitEffectId);
            boltLoopEffectData = Catalog.GetData<EffectData>(boltLoopEffectId);
            boltHitEffectData = Catalog.GetData<EffectData>(boltHitEffectId);
            boltHitImbueEffectData = Catalog.GetData<EffectData>(boltHitImbueEffectId);
            imbueHitRagdollEffectData = Catalog.GetData<EffectData>(imbueHitRagdollEffectId);
            imbueHitEffectData = Catalog.GetData<EffectData>(imbueHitEffectId);
        }

    }

}
