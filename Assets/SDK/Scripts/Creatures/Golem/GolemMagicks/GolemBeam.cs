using System;
using System.Collections.Generic;
using UnityEngine;
using ThunderRoad.Skill.SpellMerge;
using System.Linq;
using ThunderRoad.Skill.Spell;
using Plane = UnityEngine.Plane;
using Random = UnityEngine.Random;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [CreateAssetMenu(menuName = "ThunderRoad/Creatures/Golem/Beam config")]
    public class GolemBeam : GolemAbility
    {
        public static float lastBeamTime;

        [Header("Beam")]
#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float hitRange = 30f;
#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float radius = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float headSweepRange = 5;
#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public bool lockSweep;
#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public LayerMask beamRaycastMask;
#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamMaxDistance = 50;
#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamStartMaxAngle = 30;
#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamAngleSoftMax = 60;
#if ODIN_INSPECTOR
        [BoxGroup("Beam")]
#endif
        public float beamAngleHardMax = 80;


        [Header("Effects")]
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Effects"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string chargeEffectID = "";
#if ODIN_INSPECTOR && UNITY_EDITOR
        protected List<ValueDropdownItem<string>> GetAllEffectID() => Catalog.GetDropdownAllID(Category.Effect);
#endif
#if ODIN_INSPECTOR && UNITY_EDITOR
        [BoxGroup("Effects"), ValueDropdown(nameof(GetAllEffectID))]
#endif
        public string beamEffectID = "";
#if ODIN_INSPECTOR
        [BoxGroup("Effects")]
#endif
        public bool stopChargeOnShoot = false;
        
        [Header("Impact")]
#if ODIN_INSPECTOR
        [BoxGroup("Impact")]
#endif
        public float damagePerSecond = 5f;
#if ODIN_INSPECTOR
        [BoxGroup("Impact")]
#endif
        public float damagePeriodTime = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("Impact")]
#endif
        public bool blockable = true;
#if ODIN_INSPECTOR
        [BoxGroup("Impact")]
#endif
        public bool deflectOnBlock = true;
#if ODIN_INSPECTOR
        [BoxGroup("Impact")]
#endif
        public float appliedForce = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("Impact")]
#endif
        public float blockedPushForce = 0f;
#if ODIN_INSPECTOR
        [BoxGroup("Impact")]
#endif
        public ForceMode appliedForceMode = ForceMode.Force;
#if ODIN_INSPECTOR
        [TableList, BoxGroup("Impact")]
#endif
        public List<Golem.InflictedStatus> appliedStatuses = new();
        
        [Header("Timing")]
#if ODIN_INSPECTOR
        [BoxGroup("Timing")]
#endif
        public float beamCooldownDuration = 20;
#if ODIN_INSPECTOR
        [BoxGroup("Timing")]
#endif
        public float beamStopDelayTargetLost = 3;
#if ODIN_INSPECTOR
        [BoxGroup("Timing")]
#endif
        public Vector2 beamingMinMaxDuration = new Vector2(5, 10);

        public enum State
        {
            Deploying,
            Firing,
            Finished
        }
        
        [Header("Trail")]
#if ODIN_INSPECTOR
        [BoxGroup("Trail")]
#endif
        public bool leaveMoltenTrail;
#if ODIN_INSPECTOR
        private List<ValueDropdownItem<string>> GetAllSpellIDs() => Catalog.GetDropdownAllID<SpellData>();

        [BoxGroup("Trail"), ValueDropdown(nameof(GetAllSpellIDs)), ShowIf(nameof(leaveMoltenTrail))]
#endif
        public string moltenBeamSpellId = "PlasmaBeam";

    }
}
