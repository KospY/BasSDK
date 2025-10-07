using System.Collections;
using UnityEngine;

#if ODIN_INSPECTOR
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class SessionPrefabData : CustomData
    {
        public string prefabAddress;

    }
}
