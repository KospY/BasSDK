using System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace ThunderRoad
{
	///<summary>
	/// UI element, used to display a slice of a double layer radial menu.
	/// Used both for "Category" or "Item" slice.
	///</summary>
	public class RadialMenuSlice : MonoBehaviour
    {
        public enum TypeOfQuantityDisplay
        {
            None,
            SingleNumber,
            NumberStack,
            Gauge,
            Percentage
        }

        [Serializable]
        public struct TypeOfDisplayToGraphicSwapper
        {
            public TypeOfQuantityDisplay type;
            public RectTransform root;
            public RadialMenuUI.GraphicSwapper graphic;
        }

        // Label to display the element's name
        public TextMeshProUGUI nameDisplay;

        // Label to display the element's quantity
        public TextMeshProUGUI quantityDisplay;

        public Transform quantityGaugeHolder;
        public Image quantityFillGauge;

        // Label to display the element's icon
        public RawImage iconDisplay;

        public TypeOfQuantityDisplay typeOfQuantityDisplay = TypeOfQuantityDisplay.NumberStack;

        public TypeOfDisplayToGraphicSwapper[] typeOfDisplayToGraphicSwappers;

        // Holds references to change colors easily on specific events, like hover or locking
        public RadialMenuUI.GraphicSwapper[] graphicSwappers;

        // Hold reference to the loaded texture to release it on destroy since it's an addressable
        private Texture addressableTexture;

        private RadialMenuUI.GraphicSwapper backgroundGraphic;

        /// <summary>
        /// Update the UI elements to match the given parameters
        /// Load the icon from the address
        /// </summary>
        /// <param name="nameToDisplay">Name of the item to display</param>
        /// <param name="quantity">Name of the item to display</param>
        /// <param name="maxStack">Max stack amount of the item to display</param>
        /// <param name="isStackable">Is the item to display stackable or not</param>
        /// <param name="iconAddress">Icon address of the item to display</param>
        /// <param name="spawnCallBack">Name of the item to display</param>
        public void UpdateUi(string nameToDisplay, int quantity, int maxStack, bool isStackable, string iconAddress,
            Action<RadialMenuSlice> spawnCallBack)
        {
            nameDisplay.text = nameToDisplay.ToUpper();
            UpdateQuantityDisplay(quantity, maxStack, isStackable);

            Catalog.LoadAssetAsync<Texture>(iconAddress, value =>
            {
                addressableTexture = value; // Hold a reference to release it on destroy
                
                if (iconDisplay)
                {
                    iconDisplay.texture = addressableTexture;
                    iconDisplay.enabled = value;
                }

                if (nameDisplay) nameDisplay.enabled = !value; // toggle the name label only if icon is null
                
                spawnCallBack.Invoke(this);
            }, "RadialMenuSlice");
        }

        private void UpdateQuantityDisplay(int quantity, int maxStack, bool isStackable)
        {
            string text;

            switch (typeOfQuantityDisplay)
            {
                case TypeOfQuantityDisplay.None:
                    text = "";
                    break;
                case TypeOfQuantityDisplay.SingleNumber:
                    text = quantity.ToString();
                    break;
                case TypeOfQuantityDisplay.NumberStack:
                    text = $"{quantity}/{maxStack}";
                    break;
                case TypeOfQuantityDisplay.Gauge:
                    text = "";
                    UpdateQuantityGauge(quantity, maxStack, isStackable);
                    break;
                case TypeOfQuantityDisplay.Percentage:
                    text = Mathf.FloorToInt(maxStack / (float)quantity * 100f) + "%";
                    break;
                default:
                    text = quantity.ToString();
                    break;
            }

            // Hide gauge
            if (typeOfQuantityDisplay != TypeOfQuantityDisplay.Gauge && quantityGaugeHolder)
                quantityGaugeHolder.gameObject.SetActive(false);

            PickBackgroundGraphic();
            quantityDisplay.text = text;
        }

        private void PickBackgroundGraphic()
        {
            if (typeOfDisplayToGraphicSwappers == null) return;

            GameObject displayToEnable = null;
            // Hide every type of display, and toggle only the one with the same type
            for (int i = 0; i < typeOfDisplayToGraphicSwappers.Length; i++)
            {
                typeOfDisplayToGraphicSwappers[i].root.gameObject.SetActive(false);
                if (typeOfDisplayToGraphicSwappers[i].type != typeOfQuantityDisplay) continue;

                displayToEnable = typeOfDisplayToGraphicSwappers[i].root.gameObject;
                backgroundGraphic = typeOfDisplayToGraphicSwappers[i].graphic;
            }

            if (displayToEnable != null) displayToEnable.SetActive(true);
        }

        private void UpdateQuantityGauge(int quantity, int maxStack, bool isStackable)
        {
            if (quantityGaugeHolder)
                quantityGaugeHolder.gameObject.SetActive(true);

            if (quantityFillGauge)
            {
                var percentage = quantity > 0 ? Mathf.Clamp01(quantity / (float)maxStack) : 0;
                quantityFillGauge.fillAmount = percentage;
            }
        }

        /// <summary>
        /// Release the addressable and destroy the object
        /// </summary>
        public void Clean()
        {
            if (addressableTexture)
                Catalog.ReleaseAsset(addressableTexture);

            Destroy(gameObject);
        }

        /// <summary>
        /// Enables or disables the slice
        /// </summary>
        /// <param name="display">Should we activate or deactivate the GameObject</param>
        /// <param name="toggle">Should the element use its toggleColor ?</param>
        /// <param name="highlight">Should the element use its highlightColor ?</param>
        /// <param name="swapGraphics">Should the graphic swap ?</param>
        public void Toggle(bool display, bool toggle = false, bool highlight = false, bool swapGraphics = false)
        {
            gameObject.SetActive(display);

            // We handle the background graphic separately from the rest
            backgroundGraphic.ResetColor();
            if (toggle) backgroundGraphic.ToggleColor();
            if (highlight) backgroundGraphic.HighlightColor();
            backgroundGraphic.SwapGraphics(!swapGraphics); // true is default, false is the swapped one

            for (var i = 0; i < graphicSwappers.Length; i++)
            {
                var graphicSwapper = graphicSwappers[i];

                graphicSwapper.ResetColor();
                if (toggle) graphicSwapper.ToggleColor();
                if (highlight) graphicSwapper.HighlightColor();

                graphicSwapper.SwapGraphics(!swapGraphics); // true is default, false is the swapped one
            }
        }

        /// <summary>
        /// Spawn a radial slice instance from the given address, and hook a callback to it since it is async
        /// </summary>
        /// <param name="parent">Parent to assign to the spawn instance</param>
        /// <param name="prefabAddress">Address of the prefab to instantiate asynchronously</param>
        /// <param name="spawnCallBack">Action to execute after getting an instance of a radial slice</param>
        public static void Get(Transform parent, AssetReference prefabAddress, Action<RadialMenuSlice> spawnCallBack)
        {
            Addressables.InstantiateAsync(prefabAddress, parent).Completed +=
                radialMenu =>
                {
                    radialMenu.Result.name = "RadialMenuSlice-" + prefabAddress;

                    var radialMenuSlice = radialMenu.Result.GetComponent<RadialMenuSlice>();
                    if (!radialMenuSlice) radialMenuSlice = radialMenu.Result.AddComponent<RadialMenuSlice>();

                    spawnCallBack.Invoke(radialMenuSlice);
                };
        }
    }
}