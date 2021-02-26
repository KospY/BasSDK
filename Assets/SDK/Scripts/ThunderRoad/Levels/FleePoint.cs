using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Levels/Flee point")]
    public class FleePoint : MonoBehaviour
    {
        public static List<FleePoint> list = new List<FleePoint>();

        protected virtual void OnEnable()
        {
            list.Add(this);
        }

        protected virtual void OnDisable()
        {
            list.Remove(this);
        }
    }
}
