using UnityEngine;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/ContainerSpawner")]
    public class ContainerSpawner : MonoBehaviour
    {
        public enum Tier
        {
            None,
            Common,
            Rare
        }

        #region VARIABLES
#if ODIN_INSPECTOR
        [BoxGroup("Properties", true, true)]
        [ValueDropdown("GetAllContainerId")]
#endif
        public string containerId;

#if ODIN_INSPECTOR
        [BoxGroup("Properties", true, true)]
#endif
        public Tier tier = Tier.None;

#if ODIN_INSPECTOR
        [BoxGroup("Properties", true, true)]
#endif
        public bool pooled;

#if ODIN_INSPECTOR
        [BoxGroup("Properties", true, true)]
#endif
        public bool spawnAll = false;

#if ODIN_INSPECTOR
        [BoxGroup("Properties", true, true)]
#endif
        public bool allowDuplicates = true;

#if ODIN_INSPECTOR
        [BoxGroup("Properties", true, true)]
#endif
        public bool spawnOnStart = true;

#if ODIN_INSPECTOR
        [BoxGroup("Properties", true, true)]
#endif
        public bool disallowDespawn;

#if ODIN_INSPECTOR
        [BoxGroup("Properties", true, true)]
#endif
        public bool holderIsPivot = false;

#if ODIN_INSPECTOR
        [PropertySpace(5, 20)]
#endif
        public List<Transform> spawnPoints = new List<Transform>();

        #endregion

    }
}
