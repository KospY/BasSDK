using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Levels/Spawn locations")]
    public class SpawnLocation : MonoBehaviour
    {
        public List<Transform> list = new List<Transform>();
        public bool addAsFleepointOnStart = true;

        private void Awake()
        {
            if (addAsFleepointOnStart)
            {
                foreach (Transform point in list)
                {
                    if (!point.gameObject.GetComponent<FleePoint>())
                    {
                        point.gameObject.AddComponent<FleePoint>();
                    }
                }
            }
        }
    }
}
