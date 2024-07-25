using UnityEngine;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/ContainerSpawner.html")]
    public class ContainerSpawner : MonoBehaviour
    {
        #region VARIABLES
#if ODIN_INSPECTOR
        [BoxGroup("Properties", true, true)]
        [ValueDropdown(nameof(GetAllContainerId))]
#endif
        public string containerId;

#if ODIN_INSPECTOR
        [BoxGroup("Properties", true, true)]
#endif

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

        public Item.Owner spawnOwner = Item.Owner.None;

        #endregion

        #region ODIN / UTILITIES

#if ODIN_INSPECTOR
        /// <summary>
        /// If the spawner isn't registering the files this will quickly allow the artist to do a one-click fix.
        /// </summary>
        [BoxGroup("Utilities", true, true), Button("Reload JSON")]
        private void ReloadJson()
        {
            Catalog.EditorLoadAllJson(true);
        }

        private List<ValueDropdownItem<string>> GetAllContainerId()
        {
            List<ValueDropdownItem<string>> items = new List<ValueDropdownItem<string>>();
            items.AddRange(Catalog.GetDropdownAllID<ContainerData>());
            items.AddRange(Catalog.GetDropdownAllID<LootTableBase>());

            return items;
        }

#endif
        #endregion

    }
}
