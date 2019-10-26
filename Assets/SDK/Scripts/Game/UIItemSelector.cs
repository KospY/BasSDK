using UnityEngine;

namespace BS
{
    public class UIItemSelector : MonoBehaviour
    {
        public Transform spawnPoint;

#if FULLGAME
        protected void Awake()
        {
            GameObject canvasGameObject = Instantiate(Resources.Load("UI/ItemSpawner"), this.transform.position, this.transform.rotation, this.transform) as GameObject;
            canvasGameObject.name = "ItemSpawner";
            canvasGameObject.GetComponent<Inventory>().spawnPoint = spawnPoint;
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
