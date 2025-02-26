using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class Golem : GolemController
    {
        public static Golem local;

        [Header("AI")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI", Order = 1)]
#endif
        public NavMeshAgent navMeshAgent;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float cycleTime = 0.5f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float forwardAngle = 30f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float stopMoveDistance = 5.5f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float stopMoveSphereHeight = 1f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float minHeadToTargetDistance = 20f;
        private Vector2 velocity;
        private Vector2 smoothDeltaPosition;
        private float animationDampTime = 0.1f;

        [Header("Awake")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public bool awakeWhenTargetClose = true;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float awakeTargetDistance = 30;
        protected float awakeTime;

        [Header("Melee")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public bool allowMelee = true;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float meleeMaxAttackDistance = 7f;
#if ODIN_INSPECTOR
        [FormerlySerializedAs("abilityCooldownInMelee")]
        [TabGroup("GroupTabs", "AI")]
#endif
        public float abilityCooldown = 15;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public Vector2 meleeMinMaxCooldown = new Vector2(1f, 3f);
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public List<MeleeAttackRange> attackRanges;
        private bool setMeleeCooldown = false;
        private float nextMeleeAttackTime;

#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public bool blockActionWhileClimbed = true;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public bool allowClimbReact = false;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public AudioSource climbReactWarningAudio;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float climbReactWarningTime = 2f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float climbReactTime = 5f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float resetClimbedTime = 1f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public List<GolemAbility> climbReacts;
        private bool wasClimbed = false;
        private float climbTime = 0f;
        private int climbReactIntensity = 0;
        private float lastReactWarningTime = 0f;

        [Header("Crystals")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public WeakPointRandomizer golemCrystalRandomizer;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public WeakPointRandomizer arenaCrystalRandomizer;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs/AI/CrystalTiers", "Default Crystals")]
        [InlineProperty, HideLabel]
#endif
        public CrystalConfig defaultConfig;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs/AI/CrystalTiers", "Tier 1 Crystals")]
        [InlineProperty, HideLabel]
#endif
        public CrystalConfig tier1Config;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs/AI/CrystalTiers", "Tier 2 Crystals")]
        [InlineProperty, HideLabel]
#endif
        public CrystalConfig tier2Config;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs/AI/CrystalTiers", "Tier 3 Crystals")]
        [InlineProperty, HideLabel]
#endif
        public CrystalConfig tier3Config;

        public CrystalConfig activeCrystalConfig
        {
            get
            {
                switch (tier)
                {
                    case Tier.Tier1: return tier1Config;
                    case Tier.Tier2: return tier2Config;
                    case Tier.Tier3: return tier3Config;
                }
                return defaultConfig;
            }
        }

#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public bool rampageOnCrystalBreak = true;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public List<GolemAbility> crystalBreakReactions = new();
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float normalRampageChance = 0.4f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float rampageDamageMult = 2f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float rampageForceMult = 1.5f;
#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllItemID() => Catalog.GetDropdownAllID(Category.Item);

        [TabGroup("GroupTabs", "AI"), ValueDropdown(nameof(GetAllItemID), AppendNextDrawer = true)]
#endif
        public string shardItemId = "CrystalShard";
        protected ItemData shardItemData;
        protected int shardsToDrop = 0;

#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        [Range(1, 10)]
        public int crystalsBrokenToWake = 1;
        private int crystalsBrokenDuringStun = 0;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI")]
#endif
        public float shieldDisableDelay = 0f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "AI"), ColorUsage(true, true)]
#endif
        public Color defeatedEmissionColor;

#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
#endif
        public bool autoDefeatOnStart;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
#endif
        public bool autoDefeatAndKillOnStart;

#if ODIN_INSPECTOR
        [ShowInInspector, TabGroup("GroupTabs", "Debug")]
#endif
        public bool isProtected => (linkedArenaCrystals?.Count ?? 0) > 0;
#if ODIN_INSPECTOR
        [ShowInInspector, TabGroup("GroupTabs", "Debug")]
#endif
        public int crystalsLeft => crystals.Count;
        [NonSerialized]
        public List<GolemCrystal> crystals = new List<GolemCrystal>();
        [NonSerialized]
        public List<GolemCrystal> linkedArenaCrystals = new List<GolemCrystal>();
        protected int disableArenaCrystalShieldIndex = 0;
        private Quaternion climbInitialRelativeRotation;

        public delegate void GolemCrystalBreak(GolemCrystal crystal);
        public event GolemCrystalBreak OnGolemCrystalBreak;
        public event GolemCrystalBreak OnArenaCrystalBreak;

        public static event Action OnLocalGolemSet;

        [Serializable]
        public class CrystalConfig
        {
#if ODIN_INSPECTOR
            [HorizontalGroup("Counts")]
#endif
            public int golemCrystals = 8;
#if ODIN_INSPECTOR
            [HorizontalGroup("Counts")]
#endif
            public int arenaCrystals = 3;
            public float arenaCrystalNearbyStun = 10f;
            public float arenaCrystalMaxStun = 30f;
            public Vector2 minMaxShardsDropped = new Vector2(8, 16);
        }

        [Serializable]
        public class AttackRange
        {
#if ODIN_INSPECTOR
            [FoldoutGroup("$dynamicTitle")]
            [VerticalGroup("$dynamicTitle/Fields")]
            [HorizontalGroup("$dynamicTitle/Fields/MinMaxes")]
            [LabelWidth(120f)]
#endif
            public Vector2 angleMinMax;
#if ODIN_INSPECTOR
            [HorizontalGroup("$dynamicTitle/Fields/MinMaxes")]
            [LabelWidth(120f)]
#endif
            public Vector2 distanceMinMax;

            public virtual string dynamicTitle => "Attack Range";

            public bool CheckAngleDistance(float targetAngle, float targetDistance)
            {
                return targetAngle >= angleMinMax.x && targetAngle <= angleMinMax.y && targetDistance >= distanceMinMax.x && targetDistance <= distanceMinMax.y;
            }
        }

        [System.Flags]
        public enum Tier
        {
            Any = 0,
            Tier1 = 1,
            Tier2 = 2,
            Tier3 = 4,
        }

        [Serializable]
        public class MeleeAttackRange : AttackRange
        {
#if ODIN_INSPECTOR
            [Space]
            [VerticalGroup("$dynamicTitle/Fields")]
            [ListDrawerSettings(Expanded = true)]
#endif
            public WeightedAttack[] attackOptions;

            [Serializable]
            public class WeightedAttack
            {
#if ODIN_INSPECTOR
                [HorizontalGroup("Attack")]
                [LabelWidth(60f)]
#endif
                public AttackMotion attack;
#if ODIN_INSPECTOR
                [HorizontalGroup("Attack")]
                [LabelWidth(60f)]
#endif
                public float weight = 1f;
            }

            public override string dynamicTitle
            {
                get
                {
                    string result = attackOptions.IsNullOrEmpty() ? "None" : "";
                    for (int i = 0; i < (attackOptions?.Length ?? 0); i++)
                    {
                        WeightedAttack attack = attackOptions[i];
                        result += $"{attack.attack.ToString()} ({attack.weight.ToString("0.00")})";
                        if (i != attackOptions.Length - 1) result += ", ";
                    }
                    return result;
                }
            }

            public bool TryUseRange(float targetAngle, float targetDistance, out AttackMotion attack, AttackMotion lastAttack = AttackMotion.Rampage)
            {
                attack = attackOptions[UnityEngine.Random.Range(0, attackOptions.Length)].attack;
                if (!attackOptions.WeightedFilteredSelectInPlace(at => at.attack != lastAttack, at => at.weight, out WeightedAttack weightedAttack)) return false;
                attack = weightedAttack.attack;
                return CheckAngleDistance(targetAngle, targetDistance);
            }
        }

        [Serializable]
        public class InflictedStatus
        {
#if ODIN_INSPECTOR
            private List<ValueDropdownItem<string>> GetAllStatuses => Catalog.GetDropdownAllID<StatusData>();

            [ValueDropdown(nameof(GetAllStatuses))]
#endif
            public string data;
            public float duration = 3f;
            public float parameter = 0f;
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public override void SetAwake(bool awake)
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void SetAttackTarget(Transform targetTransform)
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif


        public void Rampage()
        {
        }


#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public void Rampage(RampageType type = RampageType.Melee)
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void Defeat()
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public void BreakCrystals(int num = 1)
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public void BreakArenaCrystals(int num = 1)
        {
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (!navMeshAgent) navMeshAgent = this.GetComponentInChildren<NavMeshAgent>();
            if (!golemCrystalRandomizer) golemCrystalRandomizer = this.GetComponentInChildren<WeakPointRandomizer>();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0f, stopMoveSphereHeight, 0f), stopMoveDistance);
            Gizmos.color = Color.blue;
            if (navMeshAgent.path?.corners is Vector3[] corners)
            {
                for (int i = 0; i < corners.Length - 1; i++)
                {
                    Gizmos.DrawLine(corners[i], corners[i + 1]);
                }
            }
        }
#endif

    }
}
