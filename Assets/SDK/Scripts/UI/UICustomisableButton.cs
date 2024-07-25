using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

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
        public TMP_Text[] labels;

        public float fadeDuration = .1f;
        public bool allowPhysicalClick;
        public float clickCoolDown = 0f;

        [Header("Press and Hold")]
        public bool isPressAndHoldEnabled;
#if ODIN_INSPECTOR
        [ShowIf("isPressAndHoldEnabled")]
#endif
        public float minPressAndHoldDuration = 0.5f;
#if ODIN_INSPECTOR
        [ShowIf("isPressAndHoldEnabled")]
#endif
        public float holdSlowClicksInterval = 0.3f;
#if ODIN_INSPECTOR
        [ShowIf("isPressAndHoldEnabled")]
#endif
        public float holdFastClicksInterval = 0.125f;
#if ODIN_INSPECTOR
        [ShowIf("isPressAndHoldEnabled")]
#endif
        public int holdSlowClicksCount = 3;
        
        [Header("Fonts")]
        public TMP_FontAsset defaultFontAsset;
        public TMP_FontAsset outlineFontAsset;
        public Material customOutlineFontMaterial;

        [Header("Colors")]
        public bool multiplyColor = false;
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
        public string onPointerClickSoundAddress = "Bas.AudioGroup.UI.PlayerMenuValidationDefault";
        public float onPointerEnterSoundVolume = 1f;
        public float onPointerExitSoundVolume = 1f;
        public float onPointerClickSoundVolume = 1f;

        [Header("Events")] 
        public UnityEvent onPointerClick;
        public UnityEvent onPointerEnter;
        public UnityEvent onPointerExit;
        public UnityEvent onPointerDown;
        public UnityEvent onPointerUp;


        public bool IsInteractable
        {
            get => interactable;
            set => SetInteractable(value);
        }


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

        /// <summary>
        /// Change the visual feedback when the button IsInteractable flag changes
        /// </summary>
        /// <param name="isInteractable"></param>
        private void SetInteractable(bool isInteractable)
        {
        }

    }
}