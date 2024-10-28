using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class BrainModuleShopkeeper : BrainData.Module
    {
        public float maxPlayerRangeToSpeak = 10;
        public float checkoutDialogCycleChance = 10f;
        public float maxCheckoutDelay = 60f;
        public float maxItemCommentDelay = 6f;
        public float itemMaxViewAngle = 80f;
        public float hurtSpeakDelay = 3f;

        public string welcomeVoiceDialogId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllAnimationID))]
#endif
        public string welcomeAnimationId;

        public string buyVoiceDialogId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllAnimationID))]
#endif
        public string buyAnimationId;

        public string sellVoiceDialogId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllAnimationID))]
#endif
        public string sellAnimationId;

        public string angryVoiceDialogId;
#if ODIN_INSPECTOR
        [ValueDropdown(nameof(GetAllAnimationID))]
#endif
        public string angryAnimationId;

        public string notEnoughMoneyVoiceDialogId;

        public string leavingDialogId;
        public string checkoutDialogId;

        public string itemAppleDialogId;
        public string itemAxeDialogId;
        public string itemBowArrowDialogId;
        public string itemDaggerDialogId;
        public string itemEggsDialogId;
        public string itemFalchionDialogId;
        public string itemGenericDialogId;
        public string itemHatDialogId;
        public string itemMaceDialogId;
        public string itemShieldDialogId;
        public string itemSpearDialogId;
        public string itemStaffDialogId;
        public string itemSwordDialogId;

        public string tutorial1DialogId;
        public string tutorial2DialogId;
        public string tutorial3DialogId;


#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllAnimationID()
        {
            return Catalog.GetDropdownAllID(Category.Animation);
        }
#endif
    }
}
