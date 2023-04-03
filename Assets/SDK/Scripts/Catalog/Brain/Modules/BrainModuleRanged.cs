using UnityEngine;
using System;
using System.Collections.Generic;
using ThunderRoad.AI.Action;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public abstract class BrainModuleRanged : BrainModuleAttack
    {
        public float aimMaxAngleHorizontal = 30;
#if ODIN_INSPECTOR
        [BoxGroup("Speed")]
#endif
        public float aimMoveSpeed = 10;
        public float turnSpeed = 1;
        public Vector2 minMaxTimeToAttackFromAim = new Vector2(0, 4);
#if ODIN_INSPECTOR
        [BoxGroup("Force")]
#endif
        public float aimArmSpringMultiplier = 0.5f;

        public float aimTime { get; protected set; }

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public AttackState state = AttackState.None;

        public enum AttackState
        {
            None,
            Arming, //BowState Reloading
            Aiming,
            Attacking, //BowState Shooting
            AttackDone, //BowState ShootDone
        }

    }
}