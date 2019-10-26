using System;
using System.Collections.Generic;
using UnityEngine;

namespace BS
{
    public class ColliderGroup : MonoBehaviour
    {
        public bool imbueMagic;
        public bool checkIndependently;
        [NonSerialized]
        public List<Collider> colliders;

        protected void Awake()
        {
            colliders = new List<Collider>(this.GetComponentsInChildren<Collider>());
        }
    }
}