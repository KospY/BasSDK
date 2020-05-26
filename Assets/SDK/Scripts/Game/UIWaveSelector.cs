using UnityEngine;

namespace ThunderRoad
{
    public class UIWaveSelector : MonoBehaviour
    {
        public string id;
        public SpawnLocation spawnLocation;

        void OnValidate()
        {
            if (id == null || id == "") id = this.gameObject.scene.name;
        }

#if ProjectCore
        protected void Awake()
        {
            GameObject canvasGameObject = Instantiate(Resources.Load("UI/WaveSelector"), this.transform.position, this.transform.rotation, this.transform) as GameObject;
            canvasGameObject.name = "WaveSelector";
            canvasGameObject.GetComponentInChildren<UIPageWaves>().id = id;
            canvasGameObject.GetComponentInChildren<UIPageWaves>().spawnLocation = spawnLocation;
        }
#endif

        protected void OnDrawGizmos()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.402f, 0.29f, 0));
        }
    }
}
