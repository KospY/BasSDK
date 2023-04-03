using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class SpellCastGravity : SpellCastCharge
    {
#if ODIN_INSPECTOR
        [BoxGroup("Hover")]
#endif
        public float hoverVelocityThreshold = -0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Hover")]
#endif
        public float hoverForce = 0.46f;
#if ODIN_INSPECTOR
        [BoxGroup("Hover"), ValueDropdown("GetAllEffectID")]
#endif
        public string hoverEffectId;
        protected EffectData hoverEffectData;
        protected GameObject hoverEffectObj;
        protected float hoverIntensity;

#if ODIN_INSPECTOR
        [BoxGroup("Water Jet"), ValueDropdown("GetAllEffectID")]
#endif
        public string jetEffectId;
        protected EffectData jetEffectData;
        protected GameObject jetEffectObj;
#if ODIN_INSPECTOR
        [BoxGroup("Water Jet")]
#endif
        public float jetForce = 500;

#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float halfSphereRadius = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float pushMaxForce = 10;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float pushForceOnPlayer = 0.3f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float playerLocomotionDeactivate = 0.2f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public bool pushUseExplosionForce = true;
#if ODIN_INSPECTOR
        [BoxGroup("Throw"), ShowIf("pushUseExplosionForce")]
#endif
        public float pushExplosionUpwardModifier = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public ForceMode pushForceMode = ForceMode.Impulse;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public LayerMask pushLayerMask;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float minChargeToPushCreature = 0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float playerBoostForce = 300f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw")]
#endif
        public float bubbleBoostMultiplier = 1.3f;
#if ODIN_INSPECTOR
        [BoxGroup("Throw"), ValueDropdown("GetAllEffectID")]
#endif
        public string pushEffectId;
        protected EffectData pushEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueGravityRatio = 0;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueMassRatio = 0.25f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueDrag = -1;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueAngularDrag = -1;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue")]
#endif
        public float imbueGravityFalloffThreshold = 0.3f;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit"), ValueDropdown("GetAllEffectID")]
#endif
        public string imbueHitEffectId;
        protected EffectData imbueHitEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit"), ValueDropdown("GetAllEffectID")]
#endif
        public string imbueHitItemEffectId;
        protected EffectData imbueHitItemEffectData;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit"), ValueDropdown("GetAllEffectID")]
#endif
        public string imbueHitRagdollEffectId;
        protected EffectData imbueHitRagdollEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit")]
#endif
        public float imbueHitItemZeroGravityDuration = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit")]
#endif
        public float imbueHitRagdollZeroGravityDuration = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit")]
#endif
        public float imbueHitGravityRatio = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue hit")]
#endif
        public float imbueHitMassRatio = 0.5f;


#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public int staffSlamPushLevel = 2;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamExplosionUpwardModifier = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamWaveSpeed = 12f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamWaveUpdateRate = 0.05f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamMinRadius = 2f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamMaxRadius = 8f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamMinForce = 10f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public float staffSlamMaxForce = 15f;
#if ODIN_INSPECTOR
        [BoxGroup("Staff Slam")]
#endif
        public ForceMode staffSlamForceMode = ForceMode.VelocityChange;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use"), ValueDropdown("GetAllEffectID")]
#endif
        public string staffWhooshEffectId;
        protected EffectData staffWhooshEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float minWhooshVelocity = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float maxWhooshVelocity = 12;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float staffItemThrowChargeTime = 0.6f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float staffItemThrowMult = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float staffItemPickupAngle = 30f;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public LayerMask staffItemLayerMask;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float staffJointPositionSpring = 1000;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float staffJointPositionDamper = 150;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float staffJointPositionMaxForce = 100000;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float staffJointRotationSpring = 1000;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float staffJointRotationDamper = 50;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float staffJointRotationMaxForce = 10000;
#if ODIN_INSPECTOR
        [BoxGroup("Imbue Use")]
#endif
        public float staffUseImbueDrain = 10;


        public new SpellCastGravity Clone()
        {
            return this.MemberwiseClone() as SpellCastGravity;
        }

        public override void OnCatalogRefresh()
        {
            base.OnCatalogRefresh();
            pushEffectData = Catalog.GetData<EffectData>(pushEffectId);
            hoverEffectData = Catalog.GetData<EffectData>(hoverEffectId);
            jetEffectData = Catalog.GetData<EffectData>(jetEffectId);
            imbueHitEffectData = Catalog.GetData<EffectData>(imbueHitEffectId);
            imbueHitItemEffectData = Catalog.GetData<EffectData>(imbueHitItemEffectId);
            imbueHitRagdollEffectData = Catalog.GetData<EffectData>(imbueHitRagdollEffectId);
            staffWhooshEffectData = Catalog.GetData<EffectData>(staffWhooshEffectId);
        }

    }
}
