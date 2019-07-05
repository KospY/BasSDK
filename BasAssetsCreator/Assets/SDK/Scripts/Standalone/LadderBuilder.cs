using System.Collections;
using UnityEngine;

namespace BS
{
    public class LadderBuilder : MonoBehaviour
    {
        public int rungCount = 1;
        public float rungHeight = 0.5f;
        public GameObject rungPrefab;
        public bool build;

        IEnumerator DestroyCoroutine(Object obj)
        {
            yield return new WaitForEndOfFrame();
            DestroyImmediate(obj);
        }

        [ContextMenu("Create rungs")]
        protected virtual void CreateRungs()
        {
            if (!rungPrefab || rungCount < 1) return;
            for (int i = 0; i <= rungCount; i++)
            {
                GameObject rung = Instantiate(rungPrefab, this.transform);
                rung.transform.localRotation = Quaternion.identity;
                rung.transform.localPosition = new Vector3(0, i * rungHeight, 0);
            }
        }

        [ContextMenu("Clear rungs")]
        protected virtual void ClearRungs()
        {
            foreach (Transform child in this.transform)
            {
                if (Application.isEditor) StartCoroutine(DestroyCoroutine(child.gameObject));
                else Destroy(child.gameObject);
            }
        }
    }
}