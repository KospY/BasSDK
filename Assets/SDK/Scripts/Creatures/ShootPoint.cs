﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ShootPoint")]
    public class ShootPoint : ThunderBehaviour
    {
        //[ShowInInspector]
        public static List<ShootPoint> list = new List<ShootPoint>();

        [Range(0, 360)]
        public float allowedAngle = 60;

        [NonSerialized]
        public Vector3 navPosition;
        [NonSerialized]
#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        public Creature currentCreature;

        protected override void ManagedOnEnable()
        {
            base.ManagedOnEnable();
            list.Add(this);
            if (NavMesh.SamplePosition(this.transform.position, out NavMeshHit navMeshHit, 100, -1) && this.transform.position.y > navMeshHit.position.y)
            {
                navPosition = navMeshHit.position;
            }
            else
            {
                navPosition = this.transform.position;
            }
        }

        public override ManagedLoops EnabledManagedLoops => ManagedLoops.LateUpdate;
        protected internal override void ManagedLateUpdate()
        {
            if (currentCreature?.state == Creature.State.Dead) currentCreature = null;
        }

        protected override void ManagedOnDisable()
        {
            base.ManagedOnDisable();
            list.Remove(this);
        }

        public void OnDrawGizmos()
        {
            if (allowedAngle > 0 && allowedAngle < 360)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(this.transform.position, Quaternion.AngleAxis(allowedAngle * 0.5f, Vector3.up) * this.transform.forward);
                Gizmos.DrawRay(this.transform.position, Quaternion.AngleAxis(-allowedAngle * 0.5f, Vector3.up) * this.transform.forward);
            }
            if (NavMesh.SamplePosition(this.transform.position, out NavMeshHit navMeshHit, 100, -1) && this.transform.position.y > navMeshHit.position.y)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawSphere(this.transform.position, 0.15f);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(this.transform.position, navMeshHit.position);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(this.transform.position, 0.15f);
            }
        }
    }
}