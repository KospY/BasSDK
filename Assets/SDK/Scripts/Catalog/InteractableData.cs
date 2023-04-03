using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
	[Serializable]
    public class InteractableData : CatalogData
    {
        public static bool showTouchHighlighter = true;
        public static bool showHudHighlighter = true;
        public static float highlighterProximityDistance = 0;

#if ODIN_INSPECTOR
        [BoxGroup("Interactable")] 
#endif
        public bool disableTouch;
#if ODIN_INSPECTOR
        [BoxGroup("Interactable")] 
#endif
        public bool highlighterViewHandFreeOnly = false;

#if ODIN_INSPECTOR
        [BoxGroup("Interactable")] 
#endif
        public string localizationId;
#if ODIN_INSPECTOR
        [BoxGroup("Interactable")] 
#endif
        public string highlightDefaultTitle;
#if ODIN_INSPECTOR
        [BoxGroup("Interactable")] 
#endif
        [Multiline]
        public string highlightDefaultDesignation;

        public enum Action
        {
            UseStart,
            UseStop,
            AlternateUseStart,
            AlternateUseStop,
            Grab,
            Ungrab,
            DirLeft,
            DirRight,
            DirUp,
            DirDown,
            DirStop,
            Custom,
        }

        public override int GetCurrentVersion()
        {
            return 3;
        }
    }
}
