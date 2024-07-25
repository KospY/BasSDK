using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/LadderBuilder.html")]
    [RequireComponent(typeof(Rigidbody))]
    public class LadderBuilder : MonoBehaviour
    {
#if UNITY_EDITOR
        [Tooltip("How many ladder rungs spawn when ladder is created")]
        public int rungCount = 1;
        [Tooltip("Height between each rung.")]
        public float rungHeight = 0.5f;
        [Tooltip("Disables the mesh of the rungs")]
        public bool disableRungMesh = false;
        [Tooltip("Prefab used as the rung for the ladder.")]
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