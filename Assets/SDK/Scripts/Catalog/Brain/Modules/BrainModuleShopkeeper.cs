using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

using System.Linq;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [System.Serializable]
    public class BrainModuleShopkeeper : BrainData.Module
    {
        public float maxPlayerRangeToSpeak = 10;
        public float checkoutDialogCycleChance = 10f;
        public float maxCheckoutDelay = 60f;
        public float maxItemCommentDelay = 6f;
        public float longDialogCooldown = 30f;
        public float itemMaxViewAngle = 80f;
        public float hurtSpeakDelay = 3f;
        [Range(0f, 1f)]
        public float loreChance = 0.2f;

        public string welcomeVoiceDialogId;
        public string newUpdateWelcomeVoiceDialogId;
        public string newUpdatesWelcomeVoiceDialogId;
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

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllAnimationID()
        {
            return Catalog.GetDropdownAllID(Category.Animation);
        }
#endif

        public string notEnoughMoneyVoiceDialogId;

        public string leavingDialogId;
        public string checkoutDialogId;

        [LabelWidth(0.0001f)]
        public DialogDictionary idExactMatchDialogs;
        [LabelWidth(0.0001f)]
        public DialogDictionary idPartialMatchDialogs;
        [LabelWidth(0.0001f)]
        public DialogDictionary categoryDialogs;

        #region Compatibility
        private void SetInDictionary(int idMatchLevel, string key, string value, bool defaultBuySell = true)
        {
            var dialog = new DialogId(value, defaultBuySell, !defaultBuySell);
            switch (idMatchLevel)
            {
                case 0:
                    if (categoryDialogs == null) categoryDialogs = new();
                    categoryDialogs[key] = dialog;
                    break;
                case 1:
                    if (idPartialMatchDialogs == null) idPartialMatchDialogs = new();
                    idPartialMatchDialogs[key] = dialog;
                    break;
                case 2:
                    if (idExactMatchDialogs == null) idExactMatchDialogs = new();
                    idExactMatchDialogs[key] = dialog;
                    break;
            }
        }

        [JsonProperty("itemFalchionDialogId")]
        private string itemFalchionDialogIdSetter { set => SetInDictionary(2, "SwordOldieFalchion", value, false); }
        [JsonProperty("itemHatDialogId")]
        private string itemHatDialogIdSetter { set => SetInDictionary(2, "BaronHat", value, false); }
        [JsonProperty("itemAppleDialogId")]
        private string itemAppleDialogIdSetter { set => SetInDictionary(2, "FoodApple", value); }

        [JsonProperty("itemEggsDialogId")]
        private string itemEggsDialogIdSetter { set => SetInDictionary(1, "Egg", value); }

        [JsonProperty("itemAxeDialogId")]
        private string itemAxeDialogIdSetter { set => SetInDictionary(0, "Axes", value); }
        [JsonProperty("itemBowArrowDialogId")]
        private string itemBowArrowDialogIdSetter { set => SetInDictionary(0, "Bows", value); }
        [JsonProperty("itemDaggerDialogId")]
        private string itemDaggerDialogIdSetter { set => SetInDictionary(0, "Daggers", value); }
        [JsonProperty("itemMaceDialogId")]
        private string itemMaceDialogIdSetter { set => SetInDictionary(0, "Blunt", value); }
        [JsonProperty("itemShieldDialogId")]
        private string itemShieldDialogIdSetter { set => SetInDictionary(0, "Shields", value); }
        [JsonProperty("itemSpearDialogId")]
        private string itemSpearDialogIdSetter { set => SetInDictionary(0, "Spears", value); }
        [JsonProperty("itemStaffDialogId")]
        private string itemStaffDialogIdSetter { set => SetInDictionary(0, "Staves", value); }
        [JsonProperty("itemSwordDialogId")]
        private string itemSwordDialogIdSetter
        {
            set
            {
                SetInDictionary(0, "Swords", value);
                SetInDictionary(0, "Greatswords", value);
            }
        }
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemFalchionDialogId => idExactMatchDialogs["SwordOldieFalchion"].id;
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemHatDialogId => idExactMatchDialogs["BaronHat"].id;
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemAppleDialogId => idExactMatchDialogs["FoodApple"].id;
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemEggsDialogId => idPartialMatchDialogs["Egg"].id;
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemAxeDialogId => categoryDialogs["Axes"].id;
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemBowArrowDialogId => categoryDialogs["Bows"].id;
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemDaggerDialogId => categoryDialogs["Daggers"].id;
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemMaceDialogId => categoryDialogs["Blunt"].id;
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemShieldDialogId => categoryDialogs["Shields"].id;
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemSpearDialogId => categoryDialogs["Spears"].id;
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemStaffDialogId => categoryDialogs["Staves"].id;
        [JsonIgnore]
        [Obsolete("Hardcoded IDs are no longer used!")]
        public string itemSwordDialogId => categoryDialogs["Swords"].id;
        #endregion

        public string itemGenericDialogId;

        public string tutorial1DialogId;
        public string tutorial2DialogId;
        public string tutorial3DialogId;

        public List<UpdateLoreDialogSet> updateLoreSets = new List<UpdateLoreDialogSet>();

        [Serializable]
        public class UpdateLoreDialogSet
        {
            [JsonMergeKey]
            public string updateName;
            public string firstLore;
            public List<string> purchaseDialogs;
        }

        [Serializable]
        public class DialogDictionary : UnitySerializedDictionary<string, DialogId>
        {
            private List<KeyValuePair<string, DialogId>> orderedList = null;

            public List<KeyValuePair<string, DialogId>> GetOrderedList()
            {
                if (orderedList == null || orderedList.Count != this.Count)
                {
                    orderedList = new List<KeyValuePair<string, DialogId>>();
                    foreach (var orderedItem in this.OrderByDescending(kvp => kvp.Key.Length))
                    {
                        orderedList.Add(new KeyValuePair<string, DialogId>(orderedItem.Key, orderedItem.Value));
                    }
                }
                return orderedList;
            }
        }

        [Serializable]
        public class DialogId
        {
#if ODIN_INSPECTOR
            [LabelWidth(25f)]
            [Sirenix.OdinInspector.HorizontalGroup("Info")]
#endif
            public string id;
#if ODIN_INSPECTOR
            [LabelWidth(40f)]
            [Sirenix.OdinInspector.HorizontalGroup("Info", Width = 60f)]
#endif
            public bool buy = true;
#if ODIN_INSPECTOR
            [LabelWidth(40f)]
            [Sirenix.OdinInspector.HorizontalGroup("Info", Width = 60f)]
#endif
            public bool sell = false;

            public DialogId() { }

            public DialogId(string id, bool buy, bool sell)
            {
                this.id = id;
                this.buy = buy;
                this.sell = sell;
            }
        }

    }
}
