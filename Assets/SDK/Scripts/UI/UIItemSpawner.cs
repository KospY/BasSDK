using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using TMPro;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class UIItemSpawner : MonoBehaviour
    {
        [Header("References")]
        public Container container;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private UiItemSpawnerCategoryElement categoryElement;
        [SerializeField] private UiItemSpawnerItemElement itemElement;
        [SerializeField] private ToggleGroup categoriesLayout;
        [SerializeField] private ToggleGroup itemsLayout;
        [SerializeField] private UIGridRow itemsRow;
        [SerializeField] private UIGridRow categoriesRow;
        [SerializeField] private UIGridRow categoriesTitle;
        [SerializeField] private GameObject categoriesSpace;
        [SerializeField] private GameObject itemObjectsPool;
        [SerializeField] private Zone retrieveZone;
        [SerializeField] private TMP_Text textPlacedCount;
        [SerializeField] private GameObject placedViewTitle;
        [SerializeField] private GameObject storageViewTitle;
        [SerializeField] private GameObject sandboxViewTitle;

        // Tabs
        [SerializeField] private UICustomisableButton containerTabButton;
        [SerializeField] private UICustomisableButton placedTabButton;
        [SerializeField] private UICustomisableButton sandboxTabButton;

        // Pages
        [SerializeField] private GameObject categoriesPage;
        [SerializeField] private UIText itemsPageTitle;
        [SerializeField] private GameObject itemsPage;
        [SerializeField] private UIItemSpawnerItemInfoPage itemInfoPage;

        // Scrolls
        [SerializeField] private UIScrollController categoriesScroll;
        [SerializeField] private UIScrollController itemsScroll;

        // Item Buttons
        [SerializeField] private UICustomisableButton infoButton;
        [SerializeField] private UICustomisableButton spawnButton;
        [SerializeField] private UICustomisableButton equipButton;
        [SerializeField] private UICustomisableButton backButton;
        [SerializeField] private UICustomisableButton retrieveAllButton;
        [SerializeField] private UICustomisableButton despawnAllButton;

        [Header("Setup")]
        [SerializeField] private bool showExistingOnly = true;
        [SerializeField] private bool showArmors = true;
        [SerializeField] private bool showSandboxTab = true;
        [SerializeField] private bool showPlacedTab = true;
        [SerializeField] private bool showContainerTab = true;
        [SerializeField] private bool autoEnableSandboxTab = true;
        [SerializeField] private bool showInstancedPlacedItem = false;
        [SerializeField] private bool frozeContents;

        private Tab currentTab = Tab.None;

        public enum Tab
        {
            None,
            Sandbox,
            Container,
            Placed,
        }

        public class ItemInfo
        {
            public Item item;
            public ItemContent itemContent;
            
        }

        public ItemInfo selectedItemInfo { get; protected set; }
        protected void OnDrawGizmos()
        {
            if (spawnPoint)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, 0.1f);
            }

            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.402f, 0.29f, 0));
        }
        public void OnContentLoadedEvent()
        {
        }

        public void OnContentChanged(ContainerContent content, EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd)
                SetDirty();
        }

        /// <summary>
        /// Enable and disable the categories' grid elements canvas when they enter and leave the book page while being scrolled.
        /// This method is called from the Categories scroll rect OnValueChanged callback
        /// </summary>
        public void CheckCategoriesCanvasesVisibility()
        {
            
        }

        /// <summary>
        /// Enable and disable the items' grid elements canvas when they enter and leave the book page while being scrolled.
        /// This method is called from the Items scroll rect OnValueChanged callback
        /// </summary>
        public void CheckItemsCanvasesVisibility()
        {
            
        }

        [Button]
        public bool HasSameCategoryNames(List<string> categoryNames)
        {
            return false;
        }

        [Button]
        public UiItemSpawnerCategoryElement RefreshCategories()
        {
            return null;
        }
        public void OnCategoryChanged(string name, UiItemSpawnerCategoryElement categoryElement)
        {
        }


        [Button]
        public void RefreshItems()
        {
            SetDirty();
        }

        public IEnumerator RefreshItemsCoroutine(string storageCategoryFilter = null)
        {
            yield break;
        }
        public virtual void OnItemChanged(ItemInfo itemInfo, UiItemSpawnerItemElement itemElement)
        {
        }
        public Color GetTierColor(int tier)
        {
            return Color.white;
        }

        #region Buttons Callbacks

        public void ShowPageItemInfo()
        {
        }

        public void HidePageItemInfo()
        {
        }

        public void SetPlayerCanGrab(bool active)
        {
        }

        public void EquipSelectedItem()
        {
        }

        public void SpawnSelectedItem()
        {
        }

        public void RetrieveItems()
        {
        }

        public void DespawnItems()
        {
            
        }
        public void SetSandboxTab()
        {
            SetTab(Tab.Sandbox);
        }

        public void SetContainerTab()
        {
            SetTab(Tab.Container);
        }

        public void SetPlacedTab()
        {
            SetTab(Tab.Placed);
        }

        public void SetTab(Tab tab)
        {
            
        }

        public void SetDirty(string storageCategoryFilter = null)
        {
        }
        public UICustomisableButton AddCustomButton(string textGroupId, string textKey, UnityAction call)
        {
    return null;

        }

        #endregion

    }
}
