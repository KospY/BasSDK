using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Linq;
using Random = UnityEngine.Random;
using ThunderRoad.Manikin;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Creature")]
    [AddComponentMenu("ThunderRoad/Creatures/Creature")]
    public class Creature : ThunderBehaviour
    {
        public string creatureId;

        public Animator animator;
        public LODGroup lodGroup;
        public Container container;
        public Transform centerEyes;
        public Vector3 eyeCameraOffset;
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


        [Header("Speak")]
        public Transform jaw;
        public Vector3 jawMaxRotation = new Vector3(0, -30, 0);

        [Header("Face")]
        public bool autoEyeClipsActive = true;
        public List<CreatureEye> allEyes = new List<CreatureEye>();
        public List<CreatureData.EyeClip> eyeClips = new List<CreatureData.EyeClip>();

        [Header("Fall")]
        public float fallAliveAnimationHeight = 0.5f;
        public float fallAliveDestabilizeHeight = 3;
        public float groundStabilizationMaxVelocity = 1;
        public float groundStabilizationMinDuration = 3;
        [Range(0f, 1f)]
        public float swimFallAnimationRatio = 0.6f;

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

        public static int hashDynamicOneShot, hashDynamicLoop, hashDynamicLoop3, hashDynamicInterrupt, hashDynamicSpeedMultiplier, hashDynamicMirror;
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
        [NonSerialized]
        public static Dictionary<string, AnimatorBundle> creatureAnimatorControllers = new Dictionary<string, AnimatorBundle>();

        [NonSerialized]
        public AsyncOperationHandle<GameObject> addressableHandle;

        [NonSerialized]
        public bool isPlayer;

        [NonSerialized]
        public bool hidden;
        [NonSerialized]
        public bool holsterItemsHidden;

#if ODIN_INSPECTOR
        [ValueDropdown("GetAllFactionID")]
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
            foreach (SkinnedMeshRenderer smr in this.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                smr.updateWhenOffscreen = true;
            }
            if (!lodGroup) lodGroup = this.GetComponentInChildren<LODGroup>();


            ragdoll = this.GetComponentInChildren<Ragdoll>();

            brain = this.GetComponentInChildren<Brain>();
            equipment = this.GetComponentInChildren<Equipment>();
            if (!container) container = this.GetComponentInChildren<Container>();

            locomotion = this.GetComponent<Locomotion>();
            mana = this.GetComponent<Mana>();
            climber = this.GetComponentInChildren<FeetClimber>();

            holders = new List<Holder>(this.GetComponentsInChildren<Holder>());

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
            public int dynamicEndClip { get; protected set; }
            public int upperBodyDynamicClipA { get; protected set; }
            public int upperBodyDynamicClipB { get; protected set; }
            public int upperBodyDynamicLoopClip { get; protected set; }
            public int subStanceClipA { get; protected set; }
            public int subStanceClipB { get; protected set; }

            public ReplaceClipIndexHolder()
            {
                count = 0;
                dynamicStartClipA = count++;
                dynamicStartClipB = count++;
                dynamicLoopClip = count++;
                dynamicEndClip = count++;
                upperBodyDynamicClipA = count++;
                upperBodyDynamicClipB = count++;
                upperBodyDynamicLoopClip = count++;
                subStanceClipA = count++;
                subStanceClipB = count++;
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


    }
}
