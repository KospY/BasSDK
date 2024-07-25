using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif //ODIN_INSPECTOR
using UnityEngine;
using Object = UnityEngine.Object;
using Plane = UnityEngine.Plane;

namespace ThunderRoad.Skill.Spell
{
    public class SkillArcwire : SpellSkillData
    {
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
        public float minBeamNodeInputVelocity = 0f;

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
        [BoxGroup("Arc Wire"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string arcEffectId;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")] 
#endif
        public int arcWirePushLevel = 1;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")]
#endif
        public float playerBumpForceMultiplier = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")]
#endif
        public float playerHitDamage = 0f;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire")]
#endif
        public bool allowChargeSapping = true;
        
#if ODIN_INSPECTOR
        [BoxGroup("Electrocution"), ValueDropdown(nameof(GetAllStatusEffectID))]
#endif
        public string electrocuteStatusId = "Electrocute";
        [NonSerialized]
        public StatusData electrocuteStatusData;

        [NonSerialized]
        public EffectData arcEffectData;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string arcStaffEffectId;

#if ODIN_INSPECTOR
        [BoxGroup("Arc Wire"), ValueDropdown(nameof(GetAllEffectID))] 
#endif
        public string arcWireHitEffectId;

        [NonSerialized]
        public EffectData arcStaffEffectData;
        [NonSerialized]
        public EffectData arcWireHitEffectData;

        public static float lastEffectSpawn;

    }

    public class ArcWire : ThunderBehaviour
    {
        public bool drawingArcs;
        public Side drawingSide;
        public LightningTrailNode prevNode;
        public LightningTrailNode nextNode;
        public float arcWhooshIntensity;
        public Transform shootPoint;
        public PhysicBody sourceBody;
        public SkillArcwire skill;
        public Creature caster;

        private bool end;
        private float lastChargeSap;

        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update;

    }

    public class LightningTrailNode : ThunderBehaviour
    {
        public static HashSet<LightningTrailNode> all = new();
        private SkillArcwire skill;
        private CapsuleCollider trigger;
        public Rigidbody rb;
        public float? duration;
        private LightningTrailNode nextNode;
        private bool active;
        private float startTime;

        public Creature ignored;
        public bool tips;

    }
}
