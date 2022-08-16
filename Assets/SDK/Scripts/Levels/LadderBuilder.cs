using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/LadderBuilder")]
    [RequireComponent(typeof(Rigidbody))]
    public class LadderBuilder : MonoBehaviour
    {
#if UNITY_EDITOR
        public int rungCount = 1;
        public float rungHeight = 0.5f;
        public bool disableRungMesh = false;
        public GameObject rungPrefab;

        [Button]
        protected virtual void CreateRungs()
        {
            ClearRungs();
            if (!rungPrefab || rungCount < 1) return;
            for (int i = 0; i <= rungCount; i++)
            {
                GameObject rung = UnityEditor.PrefabUtility.InstantiatePrefab(rungPrefab, this.transform) as GameObject;
                rung.name = "Rung" + (i + 1);
                rung.transform.localRotation = Quaternion.identity;
                rung.transform.localPosition = new Vector3(0, i * rungHeight, 0);
                if (disableRungMesh)
                {
                    foreach (MeshRenderer meshRenderer in rung.GetComponentsInChildren<MeshRenderer>())
                    {
                        meshRenderer.enabled = false;
                    }
                }
            }
        }

        [Button]
        protected virtual void ClearRungs()
        {
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                var child = transform.GetChild(i).gameObject;
                DestroyImmediate(child);
            }
        }
#endif
    }
}