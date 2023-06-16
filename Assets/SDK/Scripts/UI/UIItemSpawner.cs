using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
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

        [Header("Setup")]
        [SerializeField] private bool showExistingOnly = true;
        [SerializeField] private bool showArmors = true;

        [SerializeField] private Color tier0Color;
        [SerializeField] private Color tier1Color;
        [SerializeField] private Color tier2Color;
        [SerializeField] private Color tier3Color;
        [SerializeField] private Color tier4Color;
        
        public ContainerData.Content SelectedItem { get; protected set; }
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
        public void RefreshCategories()
        {
            
        }
        public void OnCategoryChanged(string name, UiItemSpawnerCategoryElement categoryElement)
        {
        }
        public void UpdatePageItemInfo()
        {
            
        }
        
        [Button]
        public List<ItemData> RefreshItems(Container container, string storageCategoryFilter = null)
        {
            return new List<ItemData>();
            
        }
        public virtual void OnItemChanged(ContainerData.Content content, UiItemSpawnerItemElement itemElement)
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

        public void EquipSelectedItem()
        {
            
        }

        public void SpawnSelectedItem()
        {
            
        }

        #endregion
        
    }
}
