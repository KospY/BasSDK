using UnityEngine;
using System;
using System.Collections;
using ThunderRoad.AI.Action;

namespace ThunderRoad
{
    public class BrainModuleFirearm : BrainModuleRanged
    {
        public float maxShootAngle = 1;

        public ItemModuleAI.RangedWeaponData mainFirearmData { get; protected set; } = null;
        public Transform mainHandAnchor { get; protected set; } = null;

        public ItemModuleAI.RangedWeaponData offhandFirearmData { get; protected set; } = null;
        public Transform offhandAnchor { get; protected set; } = null;

    }
}