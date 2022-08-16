using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ImbueController")]
    [AddComponentMenu("ThunderRoad/Items/Imbue Controller")]
    [DisallowMultipleComponent]
    public class ImbueController : ThunderBehaviour
    {
        public ColliderGroup imbueGroup;
        [Range(-100f, 100f)]
        public float imbueRate;
        [Range(0f, 100f)]
        public float imbueMaxPercent = 50f;
        public string imbueSpellId;


        private void OnValidate()
        {
            TryAssignGroup();
        }

        private void TryAssignGroup() => imbueGroup ??= GetComponent<ColliderGroup>();


        [Button]
        public void SetImbueRate(float newRate)
        {
            imbueRate = newRate;
        }

        [Button]
        public void SetImbueMaxPercent(float newMax)
        {
            imbueMaxPercent = newMax;
        }

    }
}
