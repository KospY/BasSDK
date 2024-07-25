using System.Collections.Generic;
using UnityEngine;

namespace ThunderRoad
{
    public class UiWaveSpawnerCategoryElement : MonoBehaviour
    {
        [SerializeField] private UIExpandableList categoryList;
        [SerializeField] private UiWaveSpawnerWaveElement waveElement;

        public void SetCategory(UIWaveSpawner waveSpawner, string category, List<CatalogData> wavesData)
        {
        }
    }
}