using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.VFX;
using ThunderRoad.Skill.SpellPower;
using ThunderRoad.Modules;
using UnityEngine.Events;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class GolemController : ThunderEntity
    {
        [Header("References")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General", TabLayouting = TabLayouting.MultiRow, Order = 0)]
#endif
        public Animator animator;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public CharacterController characterController;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public GolemAnimatorEvent animatorEvent;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public List<CollisionListener> headListeners = new List<CollisionListener>();
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public List<Rigidbody> bodyParts = new List<Rigidbody>();
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public List<Collider> bodyPartColliders = new List<Collider>();
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public List<Collider> ignoreCollisionColliders = new List<Collider>();
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public List<Transform> magicSprayPoints = new List<Transform>();

        [Header("Global")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public bool awakeOnStart = false;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float moveSpeedMultiplier = 1.1f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float hitDamageMultiplier = 1f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float hitForceMultiplier = 1.1f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public Renderer headRenderer;

        [Header("Head look")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public MultiAimConstraint headAimConstraint;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public Transform headIktarget;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float headLookSpeed = 3;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public LookMode lookMode = LookMode.Follow;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public LayerMask sightLayer = 1 << 0;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float headLookSpeedMultiplier = 1;

#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public Transform eyeTransform;

        [Header("Face plate")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public Rigidbody facePlateBody;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public SimpleBreakable facePlateBreakable;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public ConfigurableJoint facePlateJoint;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float facePlateUnlockAngle = 5;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float facePlateOpenAngle = 110;

        [Header("Head crystal")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public ConfigurableJoint headCrystalJoint;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public VisualEffect headCrystalLinkVfx;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public ParticleSystem headCrystalParticle;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public FxController headCrystalEffectController;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public AudioSource headCrystalAudioSourceLoop;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public AudioSource headCrystalTearingAudioSource;

#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public AnimationCurve headCrystalLoopAudioPitchCurve;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public AnimationCurve headCrystalTearingAudioPitchCurve;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public AnimationCurve headCrystalTearingAudioVolumeCurve;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public Rigidbody headCrystalBody;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public Handle headCrystalHandle;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float headCrystalGrabMass = 5f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float headCrystalGrabDrag = 100f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float headCrystalShutdownDuration = 8f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float headCrystalTearingDistance = 1;

        [Header("Death")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public Vector2 stunCheckCapsuleHeights = new Vector2(1f, 2f);
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public Vector3 radiusMinMaxCapsuleCast = new Vector3(1.5f, 3.5f, 6f);
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public AudioSource killAudioSource;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public ParticleSystem killparticle;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float killExplosionForce = 5;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float killExplosionRadius = 5;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public float killExplosionUpward = 0.5f;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public ForceMode killExplosionForceMode = ForceMode.VelocityChange;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public Transform killExplosionSourceTransform;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "General")]
#endif
        public List<Transform> colliderResizeOnDeath;

#if ODIN_INSPECTOR
        [FoldoutGroup("GroupTabs/General/Events")]
#endif
        public UnityEvent wakeEvent;
#if ODIN_INSPECTOR
        [FoldoutGroup("GroupTabs/General/Events")]
#endif
        public UnityEvent startStunEvent;
#if ODIN_INSPECTOR
        [FoldoutGroup("GroupTabs/General/Events")]
#endif
        public UnityEvent endStunEvent;
#if ODIN_INSPECTOR
        [FoldoutGroup("GroupTabs/General/Events")]
#endif
        public UnityEvent crystalBreakEvent;
#if ODIN_INSPECTOR
        [FoldoutGroup("GroupTabs/General/Events")]
#endif
        public UnityEvent defeatEvent;
#if ODIN_INSPECTOR
        [FoldoutGroup("GroupTabs/General/Events")]
#endif
        public UnityEvent killEvent;

        [Header("Swing")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Abilities", Order = 2), MinMaxSlider(0, 50, showFields: true)]
#endif
        public Vector2 swingVelocity = new(2, 5);
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Abilities"), ValueDropdown(nameof(GetAllEffectID), AppendNextDrawer = true)]
#endif
        public string swingEffectId = "GolemSwingArm";
        protected EffectData swingEffectData;

#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Abilities")]
#endif
        public VelocityTracker[] swingTrackers = new VelocityTracker[2];


#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Abilities")]
#endif
        public Transform handLeft;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Abilities")]
#endif
        public Transform handRight;

#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Abilities")]
#endif
        public Rigidbody[] armRigidbodies = new Rigidbody[2];

        [Header("Powers")]
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Abilities")]
#endif
        public Golem.Tier tier;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Abilities"), ListDrawerSettings(Expanded = true), InlineEditor(InlineEditorObjectFieldModes.Foldout)]
#endif
        public List<GolemAbility> abilities = new();
        
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        public enum AttackMotion
        {
            Rampage = 0,
            SwingRight = 1,
            SwingLeft = 2,
            ComboSwing = 3,
            ComboSwingAndSlam = 4,
            SwingBehindRight = 5,
            SwingBehindLeft = 6,
            SwingBehindRightTurnBack = 7,
            SwingBehindLeftTurnBack = 8,
            SwingLeftStep = 9,
            SwingRightStep = 10,
            Slam = 11,
            Stampede = 12,
            Breakdance = 13,
            SlamLeftTurn90 = 14,
            SlamRightTurn90 = 15,
            SwingLeftTurn90 = 16,
            SwingRightTurn90 = 17,
            Spray = 18,
            SprayDance = 19,
            Throw = 20,
            Beam = 21,
            SelfImbue = 22,
            RadialBurst = 23,
            ShakeOff = 24,
            LightShake = 25,
        }

        [Flags]
        public enum AttackSide
        {
            None  = 0,
            Left  = 0b01,
            Right = 0b10,
            Both  = 0b11
        }

        public static AttackSide GetAttackSide(AttackMotion attack) => attack switch
        {
            AttackMotion.Rampage => AttackSide.Both,
            AttackMotion.SwingRight => AttackSide.Left,
            AttackMotion.SwingLeft => AttackSide.Right,
            AttackMotion.ComboSwing => AttackSide.Both,
            AttackMotion.ComboSwingAndSlam => AttackSide.Both,
            AttackMotion.SwingBehindRight => AttackSide.Left,
            AttackMotion.SwingBehindLeft => AttackSide.Right,
            AttackMotion.SwingBehindRightTurnBack => AttackSide.Left,
            AttackMotion.SwingBehindLeftTurnBack => AttackSide.Right,
            AttackMotion.SwingLeftStep => AttackSide.Right,
            AttackMotion.SwingRightStep => AttackSide.Left,
            AttackMotion.Slam => AttackSide.Both,
            AttackMotion.Stampede => AttackSide.Both,
            AttackMotion.Breakdance => AttackSide.Both,
            AttackMotion.SlamLeftTurn90 => AttackSide.Right,
            AttackMotion.SlamRightTurn90 => AttackSide.Left,
            AttackMotion.SwingLeftTurn90 => AttackSide.Right,
            AttackMotion.SwingRightTurn90 => AttackSide.Left,
            AttackMotion.Spray => AttackSide.None,
            AttackMotion.SprayDance => AttackSide.Left,
            AttackMotion.Throw => AttackSide.None,
            AttackMotion.Beam => AttackSide.None,
            AttackMotion.SelfImbue => AttackSide.None,
            AttackMotion.RadialBurst => AttackSide.None,
            AttackMotion.ShakeOff => AttackSide.Both,
            _ => AttackSide.None,
        };

        public static AttackSide GetAttackSide(Side side) => side switch
        {
            Side.Left => AttackSide.Left,
            Side.Right => AttackSide.Right,
            _ => AttackSide.None
        };

#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug", Order = 3)]
        [ShowInInspector, ReadOnly]
#endif
        public bool animatorIsRoot { get; protected set; }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public State state { get; protected set; } = State.Inactive;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool isDefeated { get; protected set; }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool isKilled { get; protected set; }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool isLooking { get; protected set; }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public Transform lookingTarget { get; protected set; }

        // Animator states
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, Range(0, 2)]
#endif
        protected int wakeMotion = 0;
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool isAwake { get { return animator.GetBool(awakeHash); } set { animator.SetBool(awakeHash, value); } }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool isBusy { get { return animator.GetBool(isBusyHash); } set { animator.SetBool(isBusyHash, value); } }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool inMovement { get { return animator.GetBool(inMovementHash); } }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool inAttackMotion { get { return animator.GetBool(inAttackMotionHash); } set { animator.SetBool(inAttackMotionHash, value); } }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool isDeployed
        {
            get => animator.GetBool(isDeployedHash);
            protected set => animator.SetBool(isDeployedHash, value);
        }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool deployInProgress { get { return animator.GetBool(isDeployedHash) || animator.GetBool(deployStartedHash); } }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool isStunned { get { return animator.GetBool(isStunnedHash); } }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool stunInProgress { get { return animator.GetBool(isStunnedHash) || animator.GetBool(stunStartedHash); } }
#if ODIN_INSPECTOR
        [TabGroup("GroupTabs", "Debug")]
        [ShowInInspector, ReadOnly]
#endif
        public bool isActiveState => isBusy || inAttackMotion || deployInProgress || stunInProgress;
#if ODIN_INSPECTOR
        [ShowInInspector, TabGroup("GroupTabs", "Debug")]
#endif


        public Color HeadEmissionColor
        {
            get => headRenderer.material.GetColor(EmissionColor);
            set => headRenderer.material.SetColor(EmissionColor, value);
        }

        public static int awakeHash, wakeMotionHash, moveSpeedMultiplierHash, isBusyHash, moveHash, inMovementHash, locomotionMultHash, staggerHash, staggerLateralHash, staggerAxialHash, resistPushHash, attackHash, attackMotionHash, deployHash, deployStartedHash, isDeployedHash, stunHash, stunDirectionHash, stunStartedHash, isStunnedHash, inAttackMotionHash;

        public delegate void GolemStateChange(State newState);
        public event GolemStateChange OnGolemStateChange;

        public delegate void GolemAttackEvent(AttackMotion motion, GolemAbility ability);
        public event GolemAttackEvent OnGolemAttackEvent;

        public delegate void GolemRampageEvent();
        public event GolemRampageEvent OnGolemRampage;

        public delegate void GolemStaggerEvent(Vector2 direction);
        public event GolemStaggerEvent OnGolemStagger;

        public delegate void GolemStunEvent(float duration);
        public event GolemStunEvent OnGolemStun;

        public delegate void GolemInterrupt();
        public event GolemInterrupt OnGolemInterrupted;
        public event GolemInterrupt OnGolemHeadshotInterrupt;

        public delegate void GolemDealDamage(Creature target, float damage);
        public event GolemDealDamage OnDamageDealt;

        public enum State
        {
            Inactive = 0,
            WakingUp = 6,
            Active = 1,
            Stunned = 2,
            Rampage = 3,
            Defeated = 4,
            Dead = 5,
        }

#if ODIN_INSPECTOR
        protected List<ValueDropdownItem<string>> GetAllEffectID() => Catalog.GetDropdownAllID(Category.Effect);
#endif

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        private void GetBodyParts()
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        private void GetBodyPartColliders()
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void LookAt(Transform target)
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void UnlockFacePlate(bool open)
        {
        }

#if ODIN_INSPECTOR
        [Button("Kill (SKIPS DEFEAT)"), TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void Kill()
        {
            this.StartCoroutine(KillCoroutine());
        }
        public virtual IEnumerator KillCoroutine(){
            yield break;
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void Resurrect()
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void SetAwake(bool awake)
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public void Stun(float duration)
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void StopStun()
        {
        }

        public virtual void StaggerImpact(Vector3 point)
        {
        }

        public virtual void Stagger(float lateral, float axial) => Stagger(new Vector2(lateral, axial));

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void Stagger(Vector2 direction)
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void ResistPush(bool active)
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void PerformAttackMotion(AttackMotion meleeAttack, Action onMeleeEnd = null)
        {
        }

#if ODIN_INSPECTOR
        [Button, TabGroup("GroupTabs", "Debug")]
#endif
        public virtual void UseAbility(int index)
        {
        }

        public void StartAbility(GolemAbility ability, Action endCallback = null)
        {
        }

        public void EndAbility()
        {
        }

        public void SetMoveSpeedMultiplier(float value)
        {
            animator.SetFloat(moveSpeedMultiplierHash, value);
        }

        protected virtual void OnValidate()
        {
            if (!animator) animator = this.GetComponentInChildren<Animator>();
            if (!animatorEvent) animatorEvent = this.GetComponentInChildren<GolemAnimatorEvent>();
            if (!headAimConstraint) headAimConstraint = this.GetComponentInChildren<MultiAimConstraint>();
            if (!characterController) characterController = this.GetComponentInParent<CharacterController>();
            //remove any null skinned mesh renderers
            if (!skinnedMeshRenderers.IsNullOrEmpty()) skinnedMeshRenderers.RemoveAll(x => x == null);
            if (skinnedMeshRenderers.IsNullOrEmpty()) skinnedMeshRenderers = new List<SkinnedMeshRenderer>(this.GetComponentsInChildren<SkinnedMeshRenderer>());
            
            if (bodyParts.Count == 0) GetBodyParts();
            if (Application.isPlaying) SetMoveSpeedMultiplier(moveSpeedMultiplier);
        }

    }

    public enum LookMode
    {
        Follow,
        HorizontalSweep,
        VerticalSweep,
    }
}