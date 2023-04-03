using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/UICustomisableButton")]
    public class UICustomisableButton : ThunderBehaviour, IPointerClickHandler, UICustomisableButton.IPointerPhysicalClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        #region PhysicalClickHandler

        public interface IPointerPhysicalClickHandler : IEventSystemHandler
        {
            void OnPointerPhysicalClick(UnityEngine.EventSystems.PointerEventData eventData);
        }

        private static readonly ExecuteEvents.EventFunction<IPointerPhysicalClickHandler>
            s_PointerPhysicalClickHandler = Execute;

        public static T ValidateEventData<T>(BaseEventData data) where T : class
        {
            if ((data as T) == null)
                throw new ArgumentException($"Invalid type: {data.GetType()} passed to event expecting {typeof(T)}");
            return data as T;
        }

        private static void Execute(IPointerPhysicalClickHandler handler, BaseEventData eventData)
        {
            handler.OnPointerPhysicalClick(ValidateEventData<PointerEventData>(eventData));
        }

        public static ExecuteEvents.EventFunction<IPointerPhysicalClickHandler> pointerPhysicalClickHandler => s_PointerPhysicalClickHandler;

        #endregion

        [Header("General")] 
        [SerializeField] private bool interactable = true;
        public Toggle toggle;
        [Tooltip("Set of graphics that are enabled when the button is in normal state.")]
        public Graphic[] buttonGraphics;
        [Tooltip("Set of graphics that are enabled when the button is in highlighted or selected state.")]
        public Graphic[] buttonGraphicsSwap;
        [Tooltip("Set of labels that will change colors with the user interactions.")]
        public TextMeshProUGUI[] labels;

        public float fadeDuration = .1f;
        public bool allowPhysicalClick;
        public float clickCoolDown = 0f;
        public float hoverCoolDown = 0f;
        
        [Header("Fonts")]
        public TMP_FontAsset defaultFontAsset;
        public TMP_FontAsset outlineFontAsset;

        [Header("Colors")] 
        public Color defaultColor = Color.white;
        public Color hoverColor = Color.white;
        public Color pressedColor = Color.white;
        public Color disabledColor = Color.gray;

        [Header("Haptics")] 
        public bool useHapticOnPointerEnter = true;
        public bool useHapticOnPointerExit;
        public bool useHapticOnPointerClick = true;
        public float hapticOnPointerEnterIntensity = 1f;
        public float hapticOnPointerExitIntensity = .5f;
        public float hapticOnPointerClickIntensity = 2f;
        
        [Header("Sounds")] 
        public string onPointerEnterSoundAddress = "Bas.AudioGroup.UI.LightClick";
        public string onPointerExitSoundAddress;
        public string onPointerClickSoundAddress = "Bas.AudioGroup.UI.Validation";
        public float onPointerEnterSoundVolume = 1f;
        public float onPointerExitSoundVolume = 1f;
        public float onPointerClickSoundVolume = 1f;

        [Header("Events")] 
        public UnityEvent onPointerClick;
        public UnityEvent onPointerEnter;
        public UnityEvent onPointerExit;
        public UnityEvent onPointerDown;
        public UnityEvent onPointerUp;


        public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
        {
        }

        public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
        }

        public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
        }

        public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
        {
        }

        public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
        {
        }
        
        public void OnPointerPhysicalClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
        }

    }
}