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
            Inventory inventory = Instantiate(Resources.Load("UI/ItemSpawner", typeof(Inventory)), this.transform.position, this.transform.rotation, this.transform) as Inventory;
            inventory.name = "ItemSpawner";
            inventory.spawnPoint = spawnPoint;
            inventory.Init();
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
