using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIInputField : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI placeHolderText;
        [SerializeField] private TextMeshProUGUI inputText;
        [SerializeField] private Image frameSelected;
        [SerializeField] private Sprite medievalFrameSelected;
        [SerializeField] private Sprite modernFrameSelected;

    }
}