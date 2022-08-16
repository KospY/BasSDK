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


        private const int PAGE_COLUMNS = 3;
        private const int ITEM_ROWS_POOL_COUNT = 15;
        private const int ITEM_POOL_COUNT = 45; // 15 rows * 3 columns
        private const string ARMOR_CATEGORY = "Apparels";

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
        [SerializeField] private UIButtonBook infoButton;
        [SerializeField] private UIButtonBook spawnButton;
        [SerializeField] private UIButtonBook equipButton;
        [SerializeField] private UIButtonBook backButton;

        [Header("Setup")]
        [SerializeField] private bool showExistingOnly = true;
        [SerializeField] private bool showArmors = true;

        [SerializeField] private Color tier0Color;
        [SerializeField] private Color tier1Color;
        [SerializeField] private Color tier2Color;
        [SerializeField] private Color tier3Color;
        [SerializeField] private Color tier4Color;

    }
}
