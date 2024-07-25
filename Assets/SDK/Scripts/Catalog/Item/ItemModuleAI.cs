using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class ItemModuleAI : ItemModule
    {
        public enum WeaponClass
        {
            None,
            Melee,
            Bow,
            Arrow,
            Shield,
            Wand,
            Crossbow,
            Bolt,
            Firearm,
            Throwable
        }

        public enum AttackType
        {
            None = 0,
            Swing = 1,
            Thrust = 2,
        }

        [Flags]
        public enum AttackTypeFlags
        {
            None = 0,
            Swing = 1,
            Thrust = 2,
        }

        public enum WeaponHandling
        {
            None,
            OneHanded,
            TwoHanded,
        }

        [Header("Weapon")]
        public WeaponClass primaryClass = WeaponClass.None;
        [JsonProperty("weaponClass")]
        private WeaponClass primaryClassSetter
        {
            set
            {
                primaryClass = value;
            }
        }
        public WeaponClass secondaryClass = WeaponClass.None;
        public WeaponHandling weaponHandling = WeaponHandling.None;
        public WeaponHandling secondaryHandling = WeaponHandling.None;

        [Header("Attack")]
        public AttackTypeFlags weaponAttackTypes = AttackTypeFlags.Swing | AttackTypeFlags.Thrust;
        public bool ignoredByDefense = false;
        [JsonProperty("attackIgnore")]
        private bool ignoredByDefenseSetter
        {
            set
            {
                ignoredByDefense = value;
            }
        }
        public bool alwaysPrimary = false;
        public StanceInfo defaultStanceInfo;
        public List<StanceInfo> stanceInfosByOffhand;

        [System.Serializable]
        public class StanceInfo
        {
            public enum Offhand
            {
                Empty,
                SameItem,
                ItemDuplicate,
                AnyMelee,
                AnyShield,
                AnyFirearm,
                AnyThrowable,
                Anything,
            }

            [JsonMergeKey]
            public Offhand offhand;
            public float grabAIHandleRadius = 0f;
            public string stanceDataID;
        }

        public RangedWeaponData rangedWeaponData;
        [JsonProperty("ammoType")]
        private string dataAmmoTypeSetter
        {
            set
            {
                if (rangedWeaponData == null) rangedWeaponData = new RangedWeaponData();
                rangedWeaponData.ammoType = value;
            }
        }

        [Header("Defense")]
        public float armResistanceMultiplier = 3;
        public bool allowDynamicHeight;
        public bool defenseHasPriority;

        public override void OnItemDataRefresh(ItemData data)
        {
            base.OnItemDataRefresh(data);
            if (primaryClass == WeaponClass.Melee)
            {
                if (defaultStanceInfo == null)
                {
                    defaultStanceInfo = new StanceInfo()
                    {
                        offhand = StanceInfo.Offhand.Anything,
                        stanceDataID = "HumanMelee1hStance"
                    };
                }
                if (stanceInfosByOffhand == null)
                {
                    if (data.id.Contains("Axe") || data.id.Contains("Mace")) weaponAttackTypes = AttackTypeFlags.Swing;
                    if (weaponHandling == WeaponHandling.OneHanded)
                    {
                        stanceInfosByOffhand = new List<StanceInfo>()
                    {
                        new StanceInfo()
                        {
                            offhand = StanceInfo.Offhand.Empty,
                            stanceDataID = "HumanMelee1hStance"
                        },
                        new StanceInfo()
                        {
                            offhand = StanceInfo.Offhand.AnyShield,
                            stanceDataID = "HumanMeleeShieldStance"
                        },
                        new StanceInfo()
                        {
                            offhand = StanceInfo.Offhand.AnyMelee,
                            stanceDataID = "HumanMeleeDualWieldStance"
                        }
                    };
                    }
                    else
                    {
                        stanceInfosByOffhand = new List<StanceInfo>()
                    {
                        new StanceInfo()
                        {
                            offhand = StanceInfo.Offhand.SameItem,
                            stanceDataID = ""
                        }
                    };
                    }
                }
            }
        }


        [System.Serializable]
        public class RangedWeaponData
        {
#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllHolderSlots()
            {
                return Catalog.GetDropdownHolderSlots();
            }

            [ValueDropdown(nameof(GetAllHolderSlots))]
#endif
            public string ammoType = "";
            public float tooCloseDistance = 5f;
            public Vector2 spread = new Vector2(1f, 1f);
            public float projectileSpeed = 20f;
            public bool accountForGravity = true;
            public Vector3 weaponAimAngleOffset = new Vector3();
            public Vector3 weaponHoldPositionOffset = new Vector3();
            public Vector3 weaponHoldAngleOffset = new Vector3();
            public string customRangedAttackAnimationData = "";
        }
    }
}
