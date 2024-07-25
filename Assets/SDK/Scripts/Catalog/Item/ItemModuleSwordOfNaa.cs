using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class ItemModuleSwordOfNaa : ItemModule
    {
        public float maxOrbIntensity = 0.5f;
        public GameData.HapticClip lockHaptic;
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.GetOrAddComponent<SwordOfNaa>().module = this;
        }
    }

    public class SwordOfNaa : ThunderBehaviour
    {
        public Item item;
        public Transform orbTarget;
        public SphereCollider imbueTrigger;
        public ItemModuleSwordOfNaa module;
        public Animator animator;
        public MeshRenderer crystal;
    }
}
