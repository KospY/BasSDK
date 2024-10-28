using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using Random = UnityEngine.Random;
using ThunderRoad.Manikin;
using ThunderRoad.Skill;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Creatures/Creature.html")]
    [AddComponentMenu("ThunderRoad/Creatures/Creature")]
    public class Creature : ThunderEntity
    {
        public static string secondWindId = "SecondWind";

        [Tooltip("The Creature ID of the creature.")]
        public string creatureId;

        [Tooltip("The animator the creature will use for animations")]
        public Animator animator;
        [Tooltip("The LOD Group for the creature meshes, if it has one.")]
        public LODGroup lodGroup;
        [Tooltip("The container the creature uses for its parts.")]
        public Container container;
        [Tooltip("The transform used for eye rotation")]
        public Transform centerEyes;
        [Tooltip("The offset used for the eye camera")]
        public Vector3 eyeCameraOffset;
        [Tooltip("If the creature use a VFX renderer, put it here.")]
        public Renderer vfxRenderer;

        [NonSerialized]
        public Ragdoll ragdoll;
        [NonSerialized]
        public Brain brain;
        [NonSerialized]
        public Locomotion locomotion;
        [NonSerialized]
        public RagdollHand handLeft;
        [NonSerialized]
        public RagdollHand handRight;
        [NonSerialized]
        public Equipment equipment;
        [NonSerialized]
        public Mana mana;
        [NonSerialized]
        public FeetClimber climber;
        [NonSerialized]
        public RagdollFoot footLeft;
        [NonSerialized]
        public RagdollFoot footRight;
        [NonSerialized]
        public LightVolumeReceiver lightVolumeReceiver;
        [NonSerialized]
        public bool wasLoadedForCharacterSelect;

        [NonSerialized]
        public HashSet<string> heldCrystalImbues;

        [Tooltip("References the class to tell in-game skills if the player is airborne.")]
        public AirHelper airHelper;

        [Tooltip("References the class for armor SFX")]
        public ArmorSFX armorSFX;


        [Header("Speak")]
        [Tooltip("Reference the jaw bone for creature speaking")]
        public Transform jaw;
        [Tooltip("Max rotation of the jaw when it speaks.")]
        public Vector3 jawMaxRotation = new Vector3(0, -30, 0);

        [Header("Head")]
        [Tooltip("When enabled, the creature blinks")]
        public bool autoEyeClipsActive = true;
        [Tooltip("Reference the eyes for blinking")]
        public List<CreatureEye> allEyes = new List<CreatureEye>();
        [Tooltip("Reference the eye animation clips")]
        public List<CreatureData.EyeClip> eyeClips = new List<CreatureData.EyeClip>();
        [Tooltip("Reference the meshes to hide when in first person.")]
        public List<SkinnedMeshRenderer> meshesToHideForFPV;

        [Header("Fall")]
        [Tooltip("The height off the ground the creature needs to be before they play the fall animation")]
        public float fallAliveAnimationHeight = 0.5f;
        [Tooltip("The height the creautre needs to fall before their ragdoll distabilizes")]
        public float fallAliveDestabilizeHeight = 3;
        [Tooltip("The maximum velocity for the creatures body before it can stand up")]
        public float groundStabilizationMaxVelocity = 1;
        [Tooltip("The minimum duration a creature is on the ground before getting up.")]
        public float groundStabilizationMinDuration = 3;
        [Tooltip("How submerged the creature needs to be before they can start the swimming animations.")]
        [Range(0f, 1f)]
        public float swimFallAnimationRatio = 0.6f;

        [Tooltip("Toggle T Pose for the creature.")]
        public bool toogleTPose;

        [Header("Movement")]
        public bool stepEnabled;
        public float stepThreshold = 0.2f;

        public bool turnRelativeToHand = true;
        public float headMinAngle = 30;
        public float headMaxAngle = 80;
        public float handToBodyRotationMaxVelocity = 2;
        public float handToBodyRotationMaxAngle = 30;
        public float turnSpeed = 6;
        public float ikLocomotionSpeedThreshold = 1;
        public float ikLocomotionAngularSpeedThreshold = 30f;

        public FloatHandler detectionFOVModifier;
        public FloatHandler hitEnvironmentDamageModifier;
        public FloatHandler healthModifier;

        public static int hashDynamicOneShot, hashDynamicLoop, hashDynamicLoopAdd, hashDynamicLoop3, hashDynamicInterrupt, hashDynamicSpeedMultiplier, hashDynamicMirror;
        public static int hashDynamicUpperOneShot, hashDynamicUpperLoop, hashDynamicUpperMultiplier, hashDynamicUpperMirror;
        public static int hashExitDynamic, hashInvokeCallback, hashIsBusy, hashFeminity, hashHeight, hashFalling, hashUnderwater, hashGetUp, hashTstance, hashStaticIdle, hashFreeHands;
        public static bool hashInitialized;

        public enum StaggerAnimation
        {
            Default,
            Parry,
            Head,
            Torso,
            Legs,
            FallGround,
            Riposte,
        }

        public enum PushType
        {
            Magic,
            Grab,
            Hit,
            Parry,
        }

#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public FallState fallState = FallState.None;

        public enum FallState
        {
            None,
            Falling,
            NearGround,
            Stabilizing,
            StabilizedOnGround,
        }


#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public List<Holder> holders;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [NonSerialized]
        public List<RendererData> renderers = new List<RendererData>();
        [NonSerialized]
        public List<RevealDecal> revealDecals = new List<RevealDecal>();

        [NonSerialized]
        public CreatureMouthRelay mouthRelay;

        public class RendererData
        {
            public SkinnedMeshRenderer renderer;
            public SkinnedMeshRenderer splitRenderer;
            public MeshPart meshPart;
            public RevealDecal revealDecal;
            public RevealDecal splitReveal;
            public int lod;

        }

        public static List<Creature> all = new List<Creature>();
        public static List<Creature> allActive = new List<Creature>();

        public static Action<Creature> onAllActiveRemoved;

        [NonSerialized]
        public static Dictionary<string, AnimatorBundle> creatureAnimatorControllers = new Dictionary<string, AnimatorBundle>();

        [NonSerialized]
        public bool isPlayer;

        [NonSerialized]
        public bool hidden;
        [NonSerialized]
        public bool holsterItemsHidden;

        [NonSerialized]
        public HashSet<string> heldImbueIDs;

#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllFactionID))]
#endif
        public int factionId;
        public GameData.Faction faction;

#if ODIN_INSPECTOR
        [ShowInInspector, ReadOnly]
#endif
        [NonSerialized]
        public CreatureData data;

        [NonSerialized]
        public bool pooled;

        [NonSerialized]
        public WaveData.Group spawnGroup = null;
        [NonSerialized]
        public CreatureSpawner creatureSpawner;
        [NonSerialized]
        public bool countTowardsMaxAlive = false;

        [NonSerialized]
        public float spawnTime;
        [NonSerialized]
        public float lastInteractionTime;
        [NonSerialized]
        public Creature lastInteractionCreature;
        [NonSerialized]
        public float swimVerticalRatio;

        [NonSerialized] public CreatureData.EthnicGroup currentEthnicGroup;

#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public bool initialized = false;
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public bool loaded = false;

        public bool canPlayDynamicAnimation => animatorOverrideController != null;
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public bool isPlayingDynamicAnimation;
        protected Action dynamicAnimationendEndCallback;
        protected Action upperDynamicAnimationendEndCallback;
        protected AnimatorOverrideController animatorOverrideController;
        protected KeyValuePair<AnimationClip, AnimationClip>[] animationClipOverrides;

        public delegate void ZoneEvent(Zone zone, bool enter);
        public event ZoneEvent OnZoneEvent;

        public delegate void SimpleDelegate();
        public event SimpleDelegate OnDataLoaded;
        public event SimpleDelegate OnHeightChanged;

        [NonSerialized]
        public bool updateReveal;

#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        public bool isKilled = false;

        public enum State
        {
            Dead,
            Destabilized,
            Alive,
        }

        public State state
        {
            get
            {
                if (isKilled) return State.Dead;
                if (ragdoll.state == Ragdoll.State.Destabilized || ragdoll.state == Ragdoll.State.Inert) return State.Destabilized;
                return State.Alive;
            }
        }


        public enum ProtectToAim
        {
            Protect,
            Idle,
            Aim,
        }


        public enum AnimFootStep
        {
            Slow = 0,
            Walk = 1,
            Run = 2,
        }

        public enum ColorModifier
        {
            Hair,
            HairSecondary,
            HairSpecular,
            EyesIris,
            EyesSclera,
            Skin,
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllCreatureID()
        {
            return Catalog.GetDropdownAllID(Category.Creature);
        }

        public List<ValueDropdownItem<int>> GetAllFactionID()
        {
            if (Catalog.gameData == null) return null;
            return Catalog.gameData.GetFactions();
        }
#endif

        protected void Awake()
        {
            detectionFOVModifier = new FloatHandler();
            hitEnvironmentDamageModifier = new FloatHandler();
            if (!lodGroup) lodGroup = this.GetComponentInChildren<LODGroup>();


            ragdoll = this.GetComponentInChildren<Ragdoll>();

            brain = this.GetComponentInChildren<Brain>();
            equipment = this.GetComponentInChildren<Equipment>();
            if (!container) container = this.GetComponentInChildren<Container>();

            locomotion = this.GetComponent<Locomotion>();
            mana = this.GetComponent<Mana>();
            climber = this.GetComponentInChildren<FeetClimber>();

            airHelper = this.GetOrAddComponent<AirHelper>();

            holders = new List<Holder>(this.GetComponentsInChildren<Holder>());
            heldCrystalImbues = new HashSet<string>();
            heldImbueIDs = new HashSet<string>();

            jointForceMultipliers = new Dictionary<object, (float position, float rotation)>();


            foreach (RagdollHand hand in this.GetComponentsInChildren<RagdollHand>())
            {
                if (hand.side == Side.Right) handRight = hand;
                if (hand.side == Side.Left) handLeft = hand;
            }
            foreach (RagdollFoot foot in this.GetComponentsInChildren<RagdollFoot>())
            {
                if (foot.side == Side.Right) footRight = foot;
                if (foot.side == Side.Left) footLeft = foot;
            }

            lightVolumeReceiver = this.GetComponent<LightVolumeReceiver>();
            if (!lightVolumeReceiver) lightVolumeReceiver = gameObject.AddComponent<LightVolumeReceiver>();
            lightVolumeReceiver.initRenderersOnStart = false;
            lightVolumeReceiver.addMaterialInstances = false;

            if (!hashInitialized) InitAnimatorHashs();

            Rigidbody locomotionRb = locomotion.GetComponent<Rigidbody>();
            locomotionRb.isKinematic = true;

            foreach (RagdollPart ragdollPart in this.GetComponentsInChildren<RagdollPart>())
            {
                Rigidbody ragdollPartRb = ragdollPart.GetComponent<Rigidbody>();
                ragdollPartRb.isKinematic = true;
                ragdollPart.transform.SetParent(ragdollPart.meshBone);
                ragdollPart.SetPositionToBone();
            }

        }

        public class ReplaceClipIndexHolder
        {
            public int count { get; protected set; }
            public int dynamicStartClipA { get; protected set; }
            public int dynamicStartClipB { get; protected set; }
            public int dynamicLoopClip { get; protected set; }
            public int dynamicLoopAddClip { get; protected set; }
            public int dynamicEndClip { get; protected set; }
            public int upperBodyDynamicClipA { get; protected set; }
            public int upperBodyDynamicClipB { get; protected set; }
            public int upperBodyDynamicLoopClip { get; protected set; }
            public int subStanceClipA { get; protected set; }
            public int subStanceClipB { get; protected set; }
            public int upperLeftGuard { get; protected set; }
            public int upperRightGuard { get; protected set; }
            public int leftGuard { get; protected set; }
            public int midGuard { get; protected set; }
            public int rightGuard { get; protected set; }
            public int lowerLeftGuard { get; protected set; }
            public int lowerRightGuard { get; protected set; }

            public ReplaceClipIndexHolder()
            {
                count = 0;
                dynamicStartClipA = count++;
                dynamicStartClipB = count++;
                dynamicLoopClip = count++;
                dynamicLoopAddClip = count++;
                dynamicEndClip = count++;
                upperBodyDynamicClipA = count++;
                upperBodyDynamicClipB = count++;
                upperBodyDynamicLoopClip = count++;
                subStanceClipA = count++;
                subStanceClipB = count++;
                upperLeftGuard = count++;
                upperRightGuard = count++;
                leftGuard = count++;
                midGuard = count++;
                rightGuard = count++;
                lowerLeftGuard = count++;
                lowerRightGuard = count++;
            }
        }

        public static ReplaceClipIndexHolder clipIndex = new ReplaceClipIndexHolder();

        protected void InitAnimatorHashs()
        {
            hashFeminity = Animator.StringToHash("Feminity");
            hashHeight = Animator.StringToHash("Height");
            hashFalling = Animator.StringToHash("Falling");
            hashUnderwater = Animator.StringToHash("Underwater");
            hashGetUp = Animator.StringToHash("GetUp");
            hashIsBusy = Animator.StringToHash("IsBusy");
            hashTstance = Animator.StringToHash("TStance");
            hashStaticIdle = Animator.StringToHash("StaticIdle");
            hashFreeHands = Animator.StringToHash("FreeHands");
            hashDynamicOneShot = Animator.StringToHash("DynamicOneShot");
            hashDynamicLoop = Animator.StringToHash("DynamicLoop");
            hashDynamicLoopAdd = Animator.StringToHash("DynamicLoopAdd");
            hashDynamicLoop3 = Animator.StringToHash("DynamicLoop3");
            hashDynamicInterrupt = Animator.StringToHash("DynamicInterrupt");
            hashDynamicSpeedMultiplier = Animator.StringToHash("DynamicSpeedMultiplier");
            hashDynamicMirror = Animator.StringToHash("DynamicMirror");
            hashDynamicUpperOneShot = Animator.StringToHash("UpperBodyDynamicOneShot");
            hashDynamicUpperLoop = Animator.StringToHash("UpperBodyDynamicLoop");
            hashDynamicUpperMultiplier = Animator.StringToHash("UpperBodyDynamicSpeed");
            hashDynamicUpperMirror = Animator.StringToHash("UpperBodyDynamicMirror");
            hashExitDynamic = Animator.StringToHash("ExitDynamic");
            hashInvokeCallback = Animator.StringToHash("InvokeCallback");
            hashInitialized = true;
        }

        public RagdollHand GetHand(Side side)
        {
            if (side == Side.Left) return handLeft;
            return handRight;
        }

        public RagdollFoot GetFoot(Side side)
        {
            if (side == Side.Left) return footLeft;
            return footRight;
        }

        public override ManagedLoops EnabledManagedLoops => ManagedLoops.Update | ManagedLoops.LateUpdate;
        protected internal override void ManagedUpdate()
        {
        }


        public virtual float GetAnimatorHeightRatio()
        {
            return animator.GetFloat(hashHeight);
        }

#if !PrivateSDK
        [Button]
        protected virtual void SetRagdoll()
        {
            animator.enabled = false;
            foreach (RagdollPart ragdollPart in this.GetComponentsInChildren<RagdollPart>())
            {
                ragdollPart.transform.SetParent(ragdoll.transform, true);
                ragdollPart.meshBone.SetParentOrigin(ragdollPart.transform);
                Rigidbody ragdollPartRb = ragdollPart.GetComponent<Rigidbody>();
                ragdollPartRb.isKinematic = false;
            }
        }
#endif

        #region Exposed SDK methods
        [Button]
        public void Heal(float healing)
        {
        }

        [Button]
        public bool TryAddSkill(string id)
        {
            return true;
        }

        [Button]
        public bool TryRemoveSkill(string id)
        {
            return true;
        }

        [Button]
        public void ResurrectMaxHealth()
        {
        }

        [Button]
        public void Resurrect(float healing)
        {
        }

        [Button]
        public void SetFaction(int factionId)
        {
        }

        [Button]
        public void Damage(float amount)
        {
        }

        [Button]
        public void Kill()
        {
        }

        [Button]
        public void Despawn(float delay)
        {
        }

        [Button]
        public override void Despawn()
        {
        }
        #endregion


        private Dictionary<object, (float position, float rotation)> jointForceMultipliers;
        private float jointPosForceMult = 1;
        private float jointRotForceMult = 1;

        public bool AddJointForceMultiplier(object handler, float position, float rotation)
        {
            if (jointForceMultipliers.TryGetValue(handler, out var current) && current == (position, rotation))
                return false;
            jointForceMultipliers[handler] = (position, rotation);
            RefreshJointForceMultipliers();
            return true;
        }

        public void RemoveJointForceMultiplier(object handler)
        {
            if (!jointForceMultipliers.ContainsKey(handler)) return;
            jointForceMultipliers.Remove(handler);
            RefreshJointForceMultipliers();
        }

        public void ClearJointForceMultipliers()
        {
            bool hadAny = jointForceMultipliers.Count > 0;
            jointForceMultipliers.Clear();
            if (hadAny)
                RefreshJointForceMultipliers();
        }

        public void RefreshJointForceMultipliers()
        {
            jointPosForceMult = 1;
            jointRotForceMult = 1;
            foreach ((float position, float rotation) in jointForceMultipliers.Values)
            {
                jointPosForceMult *= position;
                jointRotForceMult *= rotation;
            }

            // Refresh grabbed handle and player links
        }

        public Vector3 GetPositionJointConfig() => new(data.forcePositionSpringDamper.x * jointPosForceMult,
            data.forcePositionSpringDamper.y
            * Mathf.Max(1, jointPosForceMult / 2), jointPosForceMult);

        public Vector3 GetRotationJointConfig() => new(data.forceRotationSpringDamper.x * jointRotForceMult,
            data.forceRotationSpringDamper.y
            * Mathf.Max(1, jointRotForceMult / 2), jointRotForceMult);
    }
}
