using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class CreatureHierarchyCopier : MonoBehaviour
    {
        public Transform copyFrom;
        public Transform fromRig;
        public Transform copyTo;
        public Transform toRig;
        public Transform ragdollParts;
        public bool copyMeshParts = true;
        public bool copyReveal = true;
        public bool reassignPartBones = true;
        public bool alignParts = true;

        private void Awake()
        {
            if (Application.isPlaying) Destroy(this);
        }
        
    }
}
