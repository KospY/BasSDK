using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIMenuContentArea : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [Space]
        [SerializeField] private GameObject optionsContent;
        [SerializeField] private VerticalLayoutGroup optionsListGroup;
        
        [Space]
        [SerializeField] private GameObject errorsContent;
        [SerializeField] private VerticalLayoutGroup errorsListGroup;
        [SerializeField] private TextMeshProUGUI errorTemplate;
        [SerializeField] private UICustomisableButton showErrorsButton;
        [NonSerialized]
        private Dictionary<string, Transform> categories;

        private Dictionary<TextMeshProUGUI, ModManager.ModData.Error> textErrors;
        
        
    }
}