using System.Collections;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIWaveSpawner : MonoBehaviour
    {

        [Header("Setup")]
        public string id;
        [SerializeField] private float startDelay = 5;

        [Header("References")]
        public WaveSpawner waveSpawner;
        
        // Left Page (Waves list and fight progress)
        [SerializeField] private UIText leftPageTitle;
        [SerializeField] private ToggleGroup wavesGrid;
        [SerializeField] private UiWaveSpawnerCategoryElement categoryElement;
        [SerializeField] private GameObject fightProgress;
        [SerializeField] private UIText fightState;
        [SerializeField] private UIScrollController wavesScroll;

        // Right Page (Wave Details)
        [SerializeField] private UIText waveTitle;
        [SerializeField] private UIText waveDescription;
        [SerializeField] private Text npcMaxAliveCount;
        [SerializeField] private Text npcTotalCount;
        [SerializeField] private Text playerHealth;
        [SerializeField] private Text npcHealth;

        // Buttons
        [SerializeField] private UIButtonBook startButton;
        [SerializeField] private UIButtonBook stopButton;

    }
}