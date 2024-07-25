using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif //ODIN_INSPECTOR

namespace ThunderRoad
{
    public class UISelectionListButtonsGameModeInventoryStart : UISelectionListButtons
    {
        public Image valueImage;
        [SerializeField] protected TextMeshProUGUI valueDescription;
    }
}
