using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIItemSelector : MonoBehaviour
    {
        public Transform spawnPoint;

#if ProjectCore
        protected void Awake()
        {
            GameObject canvasGameObject = Instantiate(Resources.Load("UI/ItemSpawner"), this.transform.position, this.transform.rotation, this.transform) as GameObject;
            canvasGameObject.name = "ItemSpawner";
            canvasGameObject.GetComponent<Inventory>().spawnPoint = spawnPoint;
            foreach (ScrollRect scrollRect in this.GetComponentsInChildren<ScrollRect>(true))
            {
                // Prevent performance issue (will be enabled when the pointer go on it)
                scrollRect.enabled = false;
            }    
        }

        private void Start()
        {
            this.gameObject.SetActive(false);
        }

#endif

        protected void OnDrawGizmos()
        {
            if (spawnPoint) Gizmos.DrawWireSphere(spawnPoint.position, 0.1f);
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.402f, 0.29f, 0));
        }
    }
}
