using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Brain")]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Brain : MonoBehaviour
    {
        public float runDistance = 3f;
        public float navRagdollMult = 2f;
        public bool useAcceleration;
        public float acceleration = 0.3f;
        public float actionCycleSpeed = 1;


        [NonSerialized]
        public Creature creature;
        [NonSerialized]
        public NavMeshAgent navMeshAgent;

        public static int hashAim, hashBlock, hashDraw, hashDrawSlot, hashHit, hashHitDirX, hashHitDirY, hashDyingType, hashEffect, hashEffectType, hashIsCastingLeft, hashIsCastingRight, hashCast, hashCastCurve, hashCastSide;
        public static int hashDrawHand, hashIsDrawingRight, hashIsDrawingLeft, hashAttack, hashAttackType, hashDying, hashIsReloading, hashIsShooting, hashShoot, hashReload, hashParryMagic, hashHitType, hashInjured, hashDodge, hashDodgeType;
        public static bool hashInitialized;

        [NonSerialized]
        public bool rotate;
        [NonSerialized]
        public float navRemainingDistance;
        [NonSerialized]
        public NavigationState navState = NavigationState.Done;
        [NonSerialized]
        public float runSpeedRatio = 0f;
        [NonSerialized]
        public float currentRunSpeedRatio;
        [NonSerialized]
        public Vector3 navPosition;
        [NonSerialized]
        public Transform turnTarget;
        [NonSerialized]
        public bool IsAttacking = false;
        protected float navRange;

        public enum NavigationState
        {
            ResolvingPath,
            Moving,
            Done,
            Failed,
        }


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllBrainID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Brain);
        }
#endif

        private void OnValidate()
        {
            navMeshAgent = this.GetComponent<NavMeshAgent>();
            if (navMeshAgent) navMeshAgent.enabled = false;
        }

        protected virtual void Awake()
        {
            if (!hashInitialized) InitAnimatorHashs();
            navMeshAgent = this.GetComponent<NavMeshAgent>();
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.speed = 0;
            navMeshAgent.stoppingDistance = 0;
        }

        protected void InitAnimatorHashs()
        {
            hashAim = Animator.StringToHash("Aim");
            hashDrawSlot = Animator.StringToHash("DrawSlot");
            hashDrawHand = Animator.StringToHash("DrawHand");
            hashDraw = Animator.StringToHash("Draw");
            hashIsDrawingRight = Animator.StringToHash("IsDrawingRight");
            hashIsDrawingLeft = Animator.StringToHash("IsDrawingLeft");
            hashBlock = Animator.StringToHash("Block");
            hashHitDirX = Animator.StringToHash("HitDirX");
            hashHitDirY = Animator.StringToHash("HitDirY");
            hashDyingType = Animator.StringToHash("DyingType");
            hashDying = Animator.StringToHash("Dying");
            hashAttack = Animator.StringToHash("Attack");
            hashIsReloading = Animator.StringToHash("IsReloading");
            hashIsShooting = Animator.StringToHash("IsShooting");
            hashAttackType = Animator.StringToHash("AttackType");
            hashEffect = Animator.StringToHash("Effect");
            hashEffectType = Animator.StringToHash("EffectType");
            hashReload = Animator.StringToHash("Reload");
            hashShoot = Animator.StringToHash("Shoot");
            hashHit = Animator.StringToHash("Hit");
            hashIsCastingLeft = Animator.StringToHash("IsCastingLeft");
            hashIsCastingRight = Animator.StringToHash("IsCastingRight");
            hashCast = Animator.StringToHash("Cast");
            hashCastSide = Animator.StringToHash("CastSide");
            hashParryMagic = Animator.StringToHash("ParryMagic");
            hashHitType = Animator.StringToHash("HitType");
            hashInjured = Animator.StringToHash("Injured");
            hashDodge = Animator.StringToHash("Dodge");
            hashDodgeType = Animator.StringToHash("DodgeType");
            hashCastCurve = Animator.StringToHash("CastCurve");
            hashInitialized = true;
        }

        void UpdateNavigation()
        {
            if (turnTarget != null && (navState != NavigationState.Moving || rotate == false))
            {
                Vector3 targetDirection = new Vector3(turnTarget.position.x, creature.transform.position.y, turnTarget.position.z) - creature.transform.position;
                float targetAngle = Vector3.SignedAngle(creature.transform.forward, targetDirection, creature.transform.up);
                creature.transform.Rotate(creature.transform.up, targetAngle * creature.locomotion.turnSpeed * Time.deltaTime);
            }
            else if (rotate)
            {
                Vector3 targetDirection = navMeshAgent.desiredVelocity.normalized;
                float targetAngle = Vector3.SignedAngle(creature.transform.forward, targetDirection, creature.transform.up);
                creature.transform.Rotate(creature.transform.up, targetAngle * creature.locomotion.turnSpeed * Time.deltaTime);
            }
            if (navState == NavigationState.Done || navState == NavigationState.Failed) return;
            if (navMeshAgent.pathPending)
            {
                navState = NavigationState.ResolvingPath;
                return;
            }

            if (navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                navState = NavigationState.Failed;
                return;
            }
            //navMeshAgent.nextPosition = navMeshAgent.transform.position;
            navMeshAgent.nextPosition = creature.transform.position; //fix for navmesh moving alone
            navRemainingDistance = navMeshAgent.remainingDistance;

            if (navState != NavigationState.Moving)
            {
                navState = NavigationState.Moving;
                OnNavigationStarted();
            }

            if (navRemainingDistance <= navRange)
            {
                StopNavigation();
                return;
            }

            navMeshAgent.speed = 1;
            creature.locomotion.MoveWeighted(navMeshAgent.desiredVelocity, creature.transform, creature ? creature.GetAnimatorHeightRatio() : 1, currentRunSpeedRatio, useAcceleration ? acceleration : 0);
        }

        public void NavigateTo(Vector3 position, bool withRotation, float range, bool useAcceleration = false)
        {
            if (!navMeshAgent) return;
            if (navMeshAgent.enabled == false) navMeshAgent.enabled = true;
            this.useAcceleration = useAcceleration;
            navRange = range;
            rotate = withRotation;
            //navMeshAgent.nextPosition = navMeshAgent.transform.position;
            navMeshAgent.nextPosition = creature.transform.position; //fix for navmesh moving alone
            navMeshAgent.SetDestination(position);
            navPosition = position;
            navState = NavigationState.ResolvingPath;
        }

        public void TurnTo(Transform target)
        {
            turnTarget = target;
        }

        public void StopTurn()
        {
            turnTarget = null;
        }

        public void StopNavigation()
        {
            if (!navMeshAgent) return;
            useAcceleration = false;
            navRemainingDistance = 0;
            navState = NavigationState.Done;
            navPosition = Vector3.zero;
            OnNavigationEnded();
        }

        protected void OnNavigationStarted()
        {
            navMeshAgent.isStopped = false;
        }

        protected void OnNavigationEnded()
        {
            creature.locomotion.MoveStop();
            if (navMeshAgent.enabled && navMeshAgent.isOnNavMesh) navMeshAgent.isStopped = true;
        }

        protected void OnDrawGizmosNavigation()
        {
            if (navState == NavigationState.Moving)
            {
                for (int i = 0; i < navMeshAgent.path.corners.Length - 1; i++)
                {
                    Debug.DrawLine(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1], Color.blue);
                }
            }
        }

    }
}
