using UnityEngine;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class UISelectionListButtonsLevelModeOption : UISelectionListButtons
    {
        public Image starImagePrefab = null;
        public Sprite starImage = null;
        public Sprite starImageFill = null;

        public UISelectionListButtonsBool toggle = null;
        public Transform starPlace = null;

    }
}