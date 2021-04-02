using UnityEngine;
using System.Collections.Generic;


namespace ThunderRoad
{
    public class Rope : Handle
    {
        public Transform ropeStart;
        public Transform ropeTarget;
        public float ropeRadius = 0.03f;
        public Material ropeMaterial;
        public bool ropeUseCollider;
        public int ropeLayer;
        public PhysicMaterial ropePhysicMaterial;


        protected virtual void OnDrawGizmos()
        {
            if (ropeTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(ropeStart.position, ropeTarget.position);
            }
        }
    }
}
