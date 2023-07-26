using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class UIDropdown : TMP_Dropdown
    {
        [Header("References")]
        public TMP_Text title;
        [SerializeField] private Image arrowImage;
        [SerializeField] private UICustomisableButton itemButton;
        [Space]
        [Header("Settings")]
        [SerializeField] private bool updateTitlePosition;
        [SerializeField] private Vector3 openedTitlePosition;
        [SerializeField] private Vector3 closedTitlePosition;
        [Space]
        [Header("Events")]
        public UnityEvent OnDropdownShow;
        public UnityEvent OnDropdownHide;
        
        private UICustomisableButton dropdownButton;
        private Image backgroundImage;

        /// <summary>
        /// Show the dropdown.
        /// </summary>
        public override bool Show()
        {
            // Check if the dropdown show failed
            if (!base.Show()) return false;

            if (updateTitlePosition)
            {
                title.transform.localPosition = openedTitlePosition;
            }

            OnDropdownShow?.Invoke();

            return true;
        }
        
        /// <summary>
        /// Hide the dropdown list. I.e. close it.
        /// </summary>
        public override void Hide()
        {
            base.Hide();

            if (updateTitlePosition)
            {
                title.transform.localPosition = closedTitlePosition;
            }

            OnDropdownHide?.Invoke();
        }

    }
}