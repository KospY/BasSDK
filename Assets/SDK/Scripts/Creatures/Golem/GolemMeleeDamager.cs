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

        public bool shieldBlocksDamage = true;
        public bool shieldBlocksForce = false;
        public float shieldBlockRadius = 0.75f;
        public AudioSource blockAudio;

        public UnityEvent onHit;

    }
}
