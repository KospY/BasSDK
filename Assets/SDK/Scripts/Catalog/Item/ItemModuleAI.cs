using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class ItemModuleAI : ItemModule
    {
        [Header("Weapon")]
        public WeaponClass weaponClass = WeaponClass.None;
        public WeaponClass secondaryClass = WeaponClass.None;
        public WeaponHandling weaponHandling = WeaponHandling.None;
        public WeaponHandling secondaryHandling = WeaponHandling.None;

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

        public enum WeaponHandling
        {
            None,
            OneHanded,
            TwoHanded,
        }

        [Header("Defense")]
        public bool parryIgnoreRotation = false;
        public float parryRotation = 90;
        public float parryDualRotation = 60;
        public float armResistanceMultiplier = 3;
        public Vector2 parryRevertAngleRange = new Vector2(-80, -180);
        public Vector3 parryDefaultPosition;
        public Vector3 parryDefaultLeftRotation = new Vector3(25, -10, -65);
        public Vector3 parryDefaultRightRotation = new Vector3(30, 5, 60);
        public bool allowDynamicHeight;
        public bool defenseHasPriority;

#if ODIN_INSPECTOR
        [LabelText("Stance settings")] 
#endif
        [Header("Attack")]
        [SerializeField]
        private List<StanceData> stanceDatas = new List<StanceData>();
        [NonSerialized]
        private Dictionary<BrainModuleStance.Stance, StanceData> stanceDatasByStance;
        public bool attackIgnore = false;
        public bool attackForceParryIgnoreRotation = false;
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllHolderSlots")] 
#endif
        public string ammoType = "";
        public RangedWeaponData rangedWeaponData;


        [System.Serializable]
        public class StanceData
        {
            public BrainModuleStance.Stance stance;
            [Tooltip("If set true, Sub-Stance list is an EXCLUSION list. If set false, Sub-Stance list is an INCLUSION list.")]
            public bool exclusionList;
            public List<string> subStanceNames;
        }

        [System.Serializable]
        public class RangedWeaponData
        {
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
