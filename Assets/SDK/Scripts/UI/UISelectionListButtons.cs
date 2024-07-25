using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UISelectionListButtons : MonoBehaviour
    {
        [SerializeField] protected UICustomisableButton nextButton;
        [SerializeField] protected UICustomisableButton previousButton;
        [SerializeField] protected TextMeshProUGUI title;
        [SerializeField] protected TextMeshProUGUI value;

        public event Action<UISelectionListButtons> onUpdateValueEvent;

        public virtual bool Loop => true;

    }
}