using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UiItemSpawnerCategoryElement : UIItemSpawnerGridElement
    {
        [SerializeField] private Text itemsCount;

        private GameData.Category categoryData;

    }
}