using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleHitReaction : BrainData.Module
    {
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public bool ignoreDamagingCreature = false;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float recoverySpeed = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float parryRecoilCooldown = 1f;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float waitPhysicDelay = 0.2f;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float speedMultWhileWait = -0.5f;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float staggerLight = 0.2f;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float staggerMedium = 0.6f;
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public float jumpFootHeightThreshold = 0.1f;
#if ODIN_INSPECTOR
        [BoxGroup("General")]
#endif
        public string stunAnimationID = "HumanStun";
#if ODIN_INSPECTOR
        [BoxGroup("General")] 
#endif
        public bool cancelGetUp = true;

#if ODIN_INSPECTOR
        [BoxGroup("Behaviours"), TableList(AlwaysExpanded = true)] 
#endif
        public List<PushBehaviour> pushMagicBehaviors = new List<PushBehaviour>();
#if ODIN_INSPECTOR
        [BoxGroup("Behaviours"), TableList(AlwaysExpanded = true)] 
#endif
        public List<PushBehaviour> pushGrabThrowBehaviors = new List<PushBehaviour>();
#if ODIN_INSPECTOR
        [BoxGroup("Behaviours"), TableList(AlwaysExpanded = true)] 
#endif
        public List<PushBehaviour> pushHitBehaviors = new List<PushBehaviour>();

        [NonSerialized]
        public float maxPushMagicBehaviourLevel;
        [NonSerialized]
        public float maxPushGrabThrowBehaviourLevel;
        [NonSerialized]
        public float maxPushHitBehaviourLevel;

#if ODIN_INSPECTOR
        [BoxGroup("Resistances")]
#endif
        public bool resistDestabilizeFromMagic;

        [Serializable]
        public class PushBehaviour
        {
#if ODIN_INSPECTOR
            [TableColumnWidth(50, false)] 
#endif
            public int level;

#if ODIN_INSPECTOR
            [TableColumnWidth(220, false)] 
#endif
            public List<RagdollPart.Type> onlyForBodyParts = new List<RagdollPart.Type>();

            public Effect duringIdle = Effect.Destabilize;
            public Effect duringCombat = Effect.StaggerLight;
            public Effect duringStagger = Effect.StaggerMedium;
            public Effect duringStaggerFull = Effect.StaggerFull;
            public Effect duringAttack = Effect.StaggerMedium;
            public Effect duringAttackJump = Effect.Destabilize;

            public enum Effect
            {
                None,
                StaggerLight,
                StaggerMedium,
                StaggerFull,
                Destabilize,
            }
        }

    }
}