using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ThunderRoad
{
    public class GolemMeleeDamager : MonoBehaviour
    {
        public new Rigidbody rigidbody;
        public GolemController golemController;
        public GolemAnimatorEvent golemAnimatorEvent;
        public List<Collider> colliders;
        public bool disableColliderDuringAttack = true;

        public float hitDamage = 20;
        public float hitForce = 10;
        public float hitForceUpward = 3;

        public UnityEvent onHit;

    }
}
