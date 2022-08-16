using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [RequireComponent(typeof(LODGroup))]
    public class LODGroupOverride : MonoBehaviour
    {
        public float lodSize = 1.0f;
        public Vector3 localReference = Vector3.zero;

        private void OnValidate()
        {
            Refresh();
        }

        void Start()
        {
            Refresh();
        }

        [Button]
        public void Refresh()
        {
            LODGroup lODGroup = GetComponent<LODGroup>();
            if (lODGroup)
            {
                lODGroup.size = lodSize;
                lODGroup.localReferencePoint = localReference;
            }
            else
            {
                Debug.LogErrorFormat(this, "LODGroupOverride cannot apply as there is no LODGroup attached");
            }
        }
    }
}