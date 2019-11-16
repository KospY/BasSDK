using System;
using System.Collections.Generic;
using UnityEngine;

namespace BS
{
    public class ColliderGroup : MonoBehaviour
    {
        [Tooltip("Set if the colliders are imbuable like a blade or crystal")]
        public ImbueMagic imbueMagic = ImbueMagic.None;
        [Tooltip("(Optional) Use a mesh instead of colliders to apply imbue vfx and particles effects")]
        public Mesh imbueMesh;
        [Tooltip("(Optional) Set a renderer to use to apply imbue shader emissive effects")]
        public Renderer imbueEmissionRenderer;
        [Tooltip("Create a collision event for each collider hit (true) or for the whole group (false)")]
        public bool checkIndependently;
        [NonSerialized]
        public List<Collider> colliders;

        public enum ImbueMagic
        {
            None,
            Blade,
            Crystal,
        }

#if ProjectCore
        [NonSerialized]
        public Imbue imbue;

        protected void Awake()
        {
            colliders = new List<Collider>(this.GetComponentsInChildren<Collider>());
            if (imbueMagic != ImbueMagic.None)
            {
                imbue = this.gameObject.AddComponent<Imbue>();
            }
        }
#endif
    }
}