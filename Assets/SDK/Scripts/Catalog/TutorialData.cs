using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [Serializable]
    public class TutorialData : CatalogData
    {
        public float textFollowCamSpeed = 2.0f;
        public List<Step> steps;

        public enum InputToWait
        {
            NONE,
            JUMP,
            TURN,
            MOVE,
            MENU,
            SPELLMENU,
            SPELLSELECTED,
            SPELLCASTING,
            SLOWMOTION,
            CONFIRMATION,
            ITEMSELECTION,
            ITEMSPAWN,
            WAVESELECTION,
            WAVELAUNCH,
            SPELLSELECTED_LIGHTNING,
            IMBUE,
            SPELLSELECTED_FIRE,
            KICK,
            DRAWFROMHOLSTER,
            SPELLCASTING_FIRE,
            SPELLCASTING_LIGHTNING,
            WRISTSTATS,
            GRAB,
            SPAWNANDTKWEAPON,
        }

        [Serializable]
        public class Step
        {
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllTextGroupID"), HorizontalGroup("Text", LabelWidth = 150)]
#endif
            public string textGroupId;
#if ODIN_INSPECTOR
            [ValueDropdown("GetAllTextId"), HorizontalGroup("Text"), HideLabel]
#endif
            public string textId;

#if ODIN_INSPECTOR
            [HorizontalGroup("Media", LabelWidth = 150)]
#endif
            public string imageAddress = "";
            [NonSerialized]
            public Texture image = null;

#if ODIN_INSPECTOR
            [HorizontalGroup("Media", LabelWidth = 150)]
#endif
            public string videoAddress = "";
            [NonSerialized]
            public VideoClip video = null;

#if ODIN_INSPECTOR
            [HorizontalGroup("Wait", LabelWidth = 150)]
#endif
            public float showDelay = 0f;
#if ODIN_INSPECTOR
            [HorizontalGroup("Wait", LabelWidth = 150)]
#endif
            public float actionStayMinTime = 0;
#if ODIN_INSPECTOR
            [HorizontalGroup("Wait", LabelWidth = 150)]
#endif
            public float waitAfterDone = 0;

#if ODIN_INSPECTOR
            [HorizontalGroup("Event", LabelWidth = 150)]
#endif
            public InputToWait inputToWait = InputToWait.NONE;
#if ODIN_INSPECTOR
            [HorizontalGroup("Event", LabelWidth = 150)]
#endif
            public bool skip = false;

            [NonSerialized]
            public bool isDone = false;

#if ODIN_INSPECTOR
            public List<ValueDropdownItem<string>> GetAllTextGroupID()
            {
                return Catalog.GetTextData().GetDropdownAllTextGroups();
            }

            public List<ValueDropdownItem<string>> GetAllTextId()
            {
                return Catalog.GetTextData().GetDropdownAllTexts(textGroupId);
            }
#endif
        }
    }
}