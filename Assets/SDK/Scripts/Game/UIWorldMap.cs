using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIWorldMap : MonoBehaviour
    {
        public string mapId = "Default";
        public Transform locations;
        public Transform canvasDetails;

#if ProjectCore
        protected void Awake()
        {
            GameObject canvasGameObject = Instantiate(Resources.Load("UI/WorldMap"), this.transform.position, this.transform.rotation, this.transform) as GameObject;
            canvasGameObject.name = "WorldMap";
            WorldMap worldMap = canvasGameObject.GetComponent<WorldMap>();
            worldMap.mapId = mapId;
            foreach (Transform child in locations)
            {
                worldMap.locations.Add(child);
            }
            GameObject mapDetailGameObject = Instantiate(Resources.Load("UI/MapDetail"), canvasDetails.position, canvasDetails.rotation, canvasDetails) as GameObject;
            worldMap.Init();
        }

        private void Start()
        {
            canvasDetails.gameObject.SetActive(false);
        }
#endif

        protected void OnDrawGizmos()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 1, 0));
            if (canvasDetails != null)
            {
                Gizmos.matrix = canvasDetails.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.402f, 0.29f, 0));
            }
            if (locations != null)
            {
                foreach (Transform child in locations)
                {
                    Gizmos.matrix = child.localToWorldMatrix;
                    Gizmos.DrawRay(Vector3.zero, Vector3.back * 0.1f);
                    Gizmos.DrawWireSphere(new Vector3(0, 0, -0.1f - 0.015f), 0.03f);
                }
            }
        }
    }
}
