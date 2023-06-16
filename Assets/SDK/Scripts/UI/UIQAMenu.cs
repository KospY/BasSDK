using System.Collections.Generic;
using System.Linq;
using ThunderRoadVRKBSharedData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace ThunderRoad
{
    public class UIQAMenu : MonoBehaviour
    {
        [SerializeField] private UIScrollController levelsScroll;
        [SerializeField] private UIScrollController areasScroll;
        [SerializeField] private GameObject areasContent;
        [SerializeField] private UICustomisableButton levelButton;
        [SerializeField] private UICustomisableButton dungeonAreaButton;
        [SerializeField] private UIDropdown levelModeDropdown;
        [SerializeField] private UIDropdown levelDifficultyDropdown;
        [SerializeField] private UIDropdown dungeonLengthDropdown;
        [SerializeField] private TMP_InputField dungeonSeedInputField;
        [SerializeField] private Transform dungeonSeedKeyboardAnchor;
        [SerializeField] private UICustomisableButton loadLevelButton;

    
    }
}