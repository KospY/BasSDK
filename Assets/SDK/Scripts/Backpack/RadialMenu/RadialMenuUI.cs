using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ThunderRoad
{
    public class RadialMenuUI : MonoBehaviour
    {
        /// <summary>
        /// Offset used by the cursor to avoid quickly swapping from a hovered element to the other.
        /// Prevents a cursor perfectly placed in the middle of two elements from flickering.
        /// </summary>
        public float swapElementOffset01 = .175f;

        /// <summary>
        /// Simple struct to hold colors and sprite swapping to hook to UI events:
        /// At rest → defaultColor
        /// Hovering → toggleColor
        /// Locking → highlightColor
        /// </summary>
        [Serializable]
        public struct GraphicSwapper
        {
            public Graphic defaultGraphic;
            public Graphic swapGraphic;

            public Color defaultColor;
            public Color toggleColor;
            public Color highlightColor;

            /// <summary>
            /// Enables one of the graphics, and disables the other according to the bool parameter
            /// </summary>
            /// <param name="useDefaultGraphic">Enabled graphic is the default one</param>
            public void SwapGraphics(bool useDefaultGraphic = false)
            {
                if (!defaultGraphic || !swapGraphic) return;

                defaultGraphic.enabled = useDefaultGraphic;
                swapGraphic.enabled = !useDefaultGraphic;
            }

            /// <summary>
            /// Set the alpha of the graphics
            /// </summary>
            /// <param name="alpha"></param>
            public void SetAlpha(float alpha)
            {
                var a = Mathf.Clamp01(alpha);

                if (defaultGraphic)
                    defaultGraphic.color =
                        new Color(defaultGraphic.color.r, defaultGraphic.color.g, defaultGraphic.color.b, a);

                if (swapGraphic)
                    swapGraphic.color
                        = new Color(swapGraphic.color.r, swapGraphic.color.g, swapGraphic.color.b, a);
            }

            /// <summary>
            /// Set default color to the graphics
            /// </summary>
            public void ResetColor()
            {
                if (defaultGraphic) defaultGraphic.color = defaultColor;
                if (swapGraphic) swapGraphic.color = defaultColor;
            }

            /// <summary>
            /// Set toggled color to the graphics
            /// </summary>
            public void ToggleColor()
            {
                if (defaultGraphic) defaultGraphic.color = toggleColor;
                if (swapGraphic) swapGraphic.color = toggleColor;
            }

            /// <summary>
            /// Set highlighted color to the graphics
            /// </summary>
            public void HighlightColor()
            {
                if (defaultGraphic) defaultGraphic.color = highlightColor;
                if (swapGraphic) swapGraphic.color = highlightColor;
            }
        }

        /// <summary>
        /// Structure that hold references to graphical elements to have quick access when opened
        /// </summary>
        private struct RadialMenuUIElements
        {
            // Category related slice elements
            public RadialMenuSlice catergoryElement;

            // Category related slice elements
            public RadialMenuSlice[] itemElements;

            public RadialMenuUIElements(RadialMenuSlice catergoryElement, RadialMenuSlice[] itemElements)
            {
                this.catergoryElement = catergoryElement;
                this.itemElements = itemElements;
            }

            /// <summary>
            /// Call clean on every slices, to release any addressable (icon textures)
            /// </summary>
            public void Clean()
            {
                foreach (var secondLayerItem in itemElements)
                    secondLayerItem.Clean();

                catergoryElement.Clean();
            }

            /// <summary>
            /// Display or hide slices, used to display every item of a category when hovering
            /// </summary>
            /// <param name="display">Should we activate or deactivate the GameObject</param>
            /// <param name="isCategoryHovered">Should the category element use its toggleColor ?</param>
            /// <param name="isCategoryHighlighted">Should the category element use its highlightColor ?</param>
            /// <param name="isCategoryDisabled">Should the category element force the use of its default color ?</param>
            /// <param name="hoveredItem">Should the item element use its toggleColor ?</param>
            public void Toggle(bool display,
                bool isCategoryHovered = false,
                bool isCategoryHighlighted = false,
                bool isCategoryDisabled = false,
                int hoveredItem = 0)
            {
                // Categories are always visible
                // Sprites are swapped when hovered
                // Color is default when empty, not hovered or locked
                // Color is toggled
                catergoryElement.Toggle(true,
                    !isCategoryDisabled,
                    isCategoryHighlighted && !isCategoryDisabled,
                    isCategoryHovered && !isCategoryDisabled);

                // But elements are enabled only if they should be
                for (var i = 0; i < itemElements.Length; i++)
                {
                    var radialMenuSlice = itemElements[i];
                    radialMenuSlice.Toggle(display, // displayed only if the category holding it is
                        hoveredItem == i, // if the item is hovered, toggle it
                        false, // items are never highlighted

                        // if the category holding it is highlighted (locked) and the item is hovered, swap sprites
                        isCategoryHighlighted && hoveredItem == i
                    );
                }
            }
        }

        //Holds every address to be able to customize quickly
        [Header("Addresses references")]
        [Tooltip("Prefab address of the category UI elements")]
        public AssetReference innerSlicePrefabAddress = new AssetReference("Bas.Menu.RadialMenuInnerSlice");

        [Tooltip("Prefab address of the item UI elements")]
        public AssetReference outerSlicePrefabAddress = new AssetReference("Bas.Menu.RadialMenuOuterSlice");

        [Tooltip("Prefab address of the category separators UI elements")]
        public AssetReference innerSeparatorPrefabAddress = new AssetReference("Bas.Menu.RadialMenuInnerSeparator");

        [Tooltip("Prefab address of the item separators UI elements")]
        public AssetReference outerSeparatorPrefabAddress = new AssetReference("Bas.Menu.RadialMenuOuterSeparator");

        //

        [Header("Transform references (radial layouts)")]
        [Tooltip("Radial layout holding categories(1st layer)")]
        public RectTransform firstLayerElementHolder;

        [Tooltip("Radial layout holding items (2nd layer)")]
        public RectTransform secondaryLayerElementHolder;

        [Tooltip("Enable the spawning of inner separators")]
        public bool useFirstLayerSeparators;

        [Tooltip("Enable the spawning of outer separators")]
        public bool useSecondaryLayerSeparators;

        [Tooltip("Radial layout holding category separators (1st layer)")]
        public RectTransform firstLayerSeparatorElementHolder;

        [Tooltip("Radial layout holding item separator (2nd layer)")]
        public RectTransform secondaryLayerSeparatorElementHolder;

        //

        [Header("UI references")] public Canvas worldCanvas;
        public RectTransform innerCursorRect;
        public RectTransform outerCursorRect;
        public GraphicSwapper innerCursor;
        public GraphicSwapper outerCursor;
        public TextMeshProUGUI nameLabel;
        public TextMeshProUGUI quantityLabel;

        [Header("Side buttons references")] public bool useInnerButton = true;
        public bool useOuterButton = true;
        public bool useInnerLeftButton = true;
        public GraphicSwapper innerButton;
        public GraphicSwapper outerButton;
        public GraphicSwapper innerLeftButton;

        [Header("Animation values")]
        [Range(0.01f, 100f)]
        public float cursorRotationEaseSpeed = 15f;

        [Header("Anchor")] public bool useAnchor;
        public float anchorFollowSpeed = 5;

        [Header("Cursor")] public Transform rotationReference;
        public float rotationSpeedFactor = 2;

        // Using unity events allows for designers to hook UI elements animations, color changes, etc.
        // Could use delegates too, but won't be as practical since it's for UI.
        [Header("Category events")] public UnityEvent onOpen = new UnityEvent();
        public UnityEvent<bool> onClose = new UnityEvent<bool>();

        public UnityEvent<DoubleLayeredRadialMenu.RadialMenuCategory> onCategoryLock =
            new UnityEvent<DoubleLayeredRadialMenu.RadialMenuCategory>();

        public UnityEvent<DoubleLayeredRadialMenu.RadialMenuCategory> onCategoryUnLock =
            new UnityEvent<DoubleLayeredRadialMenu.RadialMenuCategory>();

        public UnityEvent<DoubleLayeredRadialMenu.RadialMenuCategory> onCategoryHover =
            new UnityEvent<DoubleLayeredRadialMenu.RadialMenuCategory>();

        public UnityEvent<DoubleLayeredRadialMenu.RadialMenuCategory> onCategoryUnHover =
            new UnityEvent<DoubleLayeredRadialMenu.RadialMenuCategory>();

        [Header("Second layer elements events (items)")]
        public UnityEvent<DoubleLayeredRadialMenu.RadialMenuItem> onElementHover =
            new UnityEvent<DoubleLayeredRadialMenu.RadialMenuItem>();

        public UnityEvent<DoubleLayeredRadialMenu.RadialMenuItem> onElementUnHover =
            new UnityEvent<DoubleLayeredRadialMenu.RadialMenuItem>();

        public UnityEvent<DoubleLayeredRadialMenu.RadialMenuItem> onElementSelect =
            new UnityEvent<DoubleLayeredRadialMenu.RadialMenuItem>();

        [Header("Buttons events")]
        public UnityEvent onInnerButtonHover =
            new UnityEvent();

        public UnityEvent onInnerButtonUnHover =
            new UnityEvent();

        public UnityEvent onInnerButtonPress =
            new UnityEvent();

        public UnityEvent onInnerButtonRelease =
            new UnityEvent();

        public UnityEvent<float> onInnerButtonHold =
            new UnityEvent<float>();

        public UnityEvent onInnerButtonHoldEnd =
            new UnityEvent();

        //

        public UnityEvent onOuterButtonHover =
            new UnityEvent();

        public UnityEvent onOuterButtonUnHover =
            new UnityEvent();

        public UnityEvent onOuterButtonPress =
            new UnityEvent();

        public UnityEvent onOuterButtonRelease =
            new UnityEvent();

        public UnityEvent<float> onOuterButtonHold =
            new UnityEvent<float>();

        public UnityEvent onOuterButtonHoldEnd =
            new UnityEvent();

        //

        public UnityEvent onInnerLeftButtonHover =
            new UnityEvent();

        public UnityEvent onInnerLeftButtonUnHover =
            new UnityEvent();

        public UnityEvent onInnerLeftButtonPress =
            new UnityEvent();

        public UnityEvent onInnerLeftButtonRelease =
            new UnityEvent();

        public UnityEvent<float> onInnerLeftButtonHold =
            new UnityEvent<float>();

        public UnityEvent onInnerLeftButtonHoldEnd =
            new UnityEvent();

    }
}