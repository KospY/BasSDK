using System.Collections;
using System.Globalization;
using System.Linq;
using TMPro;
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
        [SerializeField] private UIText selectWaveText;
        [SerializeField] private UIText waveTitle;
        [SerializeField] private UIText waveDescription;
        [SerializeField] private TextMeshProUGUI npcMaxAliveCount;
        [SerializeField] private TextMeshProUGUI npcTotalCount;
        [SerializeField] private TextMeshProUGUI playerHealth;
        [SerializeField] private TextMeshProUGUI npcHealth;

        // Buttons
        [SerializeField] private UICustomisableButton startButton;
        [SerializeField] private UICustomisableButton stopButton;

    }
}