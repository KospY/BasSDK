using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Chabuk.ManikinMono;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Creatures/Creature")]
    public class Creature : MonoBehaviour
    {
        public string creatureId;

        public Animator animator;
        public Transform centerEyes;
        public Vector3 eyeCameraOffset;

        public virtual void OnValidate()
        {
            if (!this.gameObject.activeInHierarchy) return;
            IconManager.SetIcon(this.gameObject, IconManager.LabelIcon.Gray);
        }

        [NonSerialized]
        public Ragdoll ragdoll;
        [NonSerialized]
        public Brain brain;
        [NonSerialized]
        public Container container;
        [NonSerialized]
        public Locomotion locomotion;
        [NonSerialized]
        public Mana mana;
        [NonSerialized]
        public CreatureSpeak speak;
        [NonSerialized]
        public CreatureFeetClimber climber;
        [NonSerialized]
        public LiquidReceiver liquidReceiver;
        [NonSerialized]
        public Equipment equipment;
        [NonSerialized]
        public RagdollHand handLeft;
        [NonSerialized]
        public RagdollHand handRight;
        [NonSerialized]
        public RagdollFoot footLeft;
        [NonSerialized]
        public RagdollFoot footRight;
        [NonSerialized]
        public ManikinPartList manikinParts;
        [NonSerialized]
        public ManikinLocations manikinLocations;
        [NonSerialized]
        protected ManikinLocations.JsonWardrobeLocations orgWardrobeLocations;

    }
}
