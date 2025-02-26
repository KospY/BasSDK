#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif
using System;
using System.Collections.Generic;

using UnityEngine;

namespace ThunderRoad
{
    public class PersistentItemAdder : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [Header("Item")]
        [HideLabel]
        [InlineProperty]
#endif
        public ItemContent item = new ItemContent();
#if ODIN_INSPECTOR
        [InlineButton(nameof(PlacementToTransform), "Auto-set")]
        [Header("Placement")]
        [HideLabel]
        [InlineProperty]
#endif
        public ContentStatePlaced placement = new ContentStatePlaced();
        [Header("Container")]
        public Container persistenceContainer;
        public bool addOnStart = true;

#if !ODIN_INSPECTOR
        [Button]
#endif
        private void PlacementToTransform()
        {
            placement = new ContentStatePlaced()
            {
                position = transform.position,
                rotation = transform.rotation,
            };
        }


        public void AddItem()
        {
        }
    }
}
