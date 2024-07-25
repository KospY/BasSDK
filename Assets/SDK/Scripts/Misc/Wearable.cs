using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using ThunderRoad.Manikin;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/Wearable.html")]
    [AddComponentMenu("ThunderRoad/Wearable")]
    public class Wearable : Interactable
    {
        public delegate void OnItemEquipped(Item item);
        public delegate void OnItemUnEquipped(string layer, Item item);
        public delegate void OnEditModeChanged(bool state);

#if ODIN_INSPECTOR && UNITY_EDITOR
        [PropertySpace(0, 10), BoxGroup("WARDROBE", true, true), ValueDropdown(nameof(GetAllChannels))]
#endif
        public string wardrobeChannel;

#if ODIN_INSPECTOR
        [BoxGroup("WARDROBE", true, true)]
        #if UNITY_EDITOR
        [ValueDropdown(nameof(GetAllChannelLayers))]
        #endif
#endif
        public WearableEntry[] wardrobeLayers;

        // Odin won't display the list without this...
        // Forgive me
        [System.Serializable]
        public class WearableEntry
        {
#if ODIN_INSPECTOR
            [ReadOnly]
#endif
            public string layer;
#if ODIN_INSPECTOR
            [ReadOnly]
#endif
            public int layerIndex;
        }

        #region EDITOR / ODIN

        ///////////////////////////////////////////////////////////////////////////////////////////////
        /// This is for QOL, no need to create an editor script since this will be stripped anyway. //
        //////////////////////////////////////////////////////////////////////////////////////////////

#if UNITY_EDITOR && ODIN_INSPECTOR

        [Button("Reset Layers")]
        private void GetLayers()
        {
            List<ValueDropdownItem<WearableEntry>> entries = GetAllChannelLayers();
            wardrobeLayers = new WearableEntry[entries.Count];
            for (int i = 0; i < entries.Count; i++)
            {
                wardrobeLayers[i] = entries[i].Value;
            }
        }

        /// <summary>
        /// This will allow for easier channel selection instead of manually typing strings.
        /// </summary>
        private List<ValueDropdownItem<string>> GetAllChannels()
        {
            // Cache.
            List<ValueDropdownItem<string>> tmp = new List<ValueDropdownItem<string>>();

            ManikinEditorLocationLabels manikinChannelLayers = ManikinEditorLocationLabels.Get();
            if (manikinChannelLayers != null)
            {
                var channelNames = manikinChannelLayers.GetChannelNames();

                // This automates the ID addition so we can just add to the array above.
                for (int i = 0; i < channelNames.Length; i++)
                {
                    string channelName = channelNames[i];
                    tmp.Add(new ValueDropdownItem<string>(channelName, channelName));
                }
            }
            return tmp;
        }

        private List<ValueDropdownItem<WearableEntry>> GetAllChannelLayers()
        {
            // Cache.
            List<ValueDropdownItem<WearableEntry>> tmp = new List<ValueDropdownItem<WearableEntry>>();
            //need a valid channel
            if (string.IsNullOrEmpty(wardrobeChannel))
                return tmp;

            ManikinEditorLocationLabels manikinChannelLayers = ManikinEditorLocationLabels.Get();
            if (manikinChannelLayers != null)
            {
                //get the layers for this channel
                var layers = manikinChannelLayers.GetLayerNamesByChannelName(wardrobeChannel);

                // This automates the ID addition so we can just add to the array above.
                for (int i = 0; i < layers.Length; i++)
                {
                    string layer = layers[i];
                    WearableEntry entry = new WearableEntry()
                    {
                        layer = layer,
                        layerIndex = i,
                    };
                    tmp.Add(new ValueDropdownItem<WearableEntry>(layer, entry));
                }
            }
            return tmp;
        }

#endif

        #endregion
    }
}
