using UnityEngine;
using System.Collections.Generic;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Interactable")]
	public class Interactable : ThunderBehaviour
    {
#if ODIN_INSPECTOR
        [ValueDropdown("GetAllInteractableID")]
#endif
        [Tooltip("(Only needed for non-json handles)\nInsert Interactable ID here.")]
        public string interactableId;

        [Tooltip("What hand is allowed to grab the handle.")]
        public HandSide allowedHandSide = HandSide.Both;

        public enum HandSide
        {
            Both,
            Right,
            Left,
        }

        [Tooltip("The length of which the player can grab along.\nIf >0, a button will appear and allow you to adjust the length along its points")]
        public float axisLength = 0;
        [Tooltip("The radius of which the player can grab the handle")]
        public float touchRadius = 0.1f;
        [Tooltip("When the player's hand is within the range of multiple interactables, the closest one is prioritized. \n \nArtifical distance is a fake distance added to the player's hand while checking which interactable is the nearest.\nSetting this to a high value gives it a low priority when working out which interactable to use, while a low (or negative) value will give it a high priority compared to other interactables. \n \n Generally, you can leave this value at 0.")]
        public float artificialDistance = 0f;
        [Tooltip("Determines the center of the touchRadius")]
        public Vector3 touchCenter;

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.matrix = this.transform.localToWorldMatrix;
            if (axisLength > 0)
            {
                Gizmos.color = Color.white;
                Common.DrawGizmoArrow(Vector3.zero, Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorName.White), 0.05f);
                Common.DrawGizmoArrow(Vector3.zero, -Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorName.White), 0.05f);
                Common.DrawGizmoCapsule(Vector3.zero, Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorName.White), axisLength / 2, touchRadius);
                Common.DrawGizmoCapsule(Vector3.zero, -Vector3.up * (axisLength / 2), Common.HueColourValue(HueColorName.White), axisLength / 2, touchRadius);
            }
            else
            {
                Gizmos.matrix = this.transform.localToWorldMatrix;
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(touchCenter, touchRadius);
            }
        }

        protected override void ManagedOnDisable()
        {
            base.ManagedOnDisable();
            TouchEndPlayerHands(false);
        }
        
        public virtual void SetTouch(bool active)
        {
        }
        
        protected void TouchEndPlayerHands(bool active)
        {
            
        }

        public virtual void SetTouchPersistent(bool active)
        {
        }

        public static bool showTouchHighlighter = true;

#if ODIN_INSPECTOR
        [BoxGroup("Interactable"), NonSerialized, ShowInInspector]
#endif
        public InteractableData data;

        [NonSerialized]
        public Collider touchCollider;
        [NonSerialized]
        public bool touchActive = true;

        private float previousCenterDistance = Mathf.Infinity;

        protected bool initialized;

        public delegate void ActionDelegate(RagdollHand ragdollHand, Action action);
        public event ActionDelegate OnTouchActionEvent;

        public enum Action
        {
            UseStart,
            UseStop,
            AlternateUseStart,
            AlternateUseStop,
            Grab,
            Ungrab,
        }

        public class InteractionResult
        {
            public InteractionResult(RagdollHand ragdollHand, bool isInteractable, bool showHint = false, string hintTitle = null, string hintDesignation = null, Color? hintColor = null, AudioClip audioClip = null)
            {
                this.ragdollHand = ragdollHand;
                this.isInteractable = isInteractable;
                this.showHint = showHint;
                this.hintTitle = hintTitle;
                this.hintDesignation = hintDesignation;
                this.hintColor = hintColor ?? Color.white;
                this.audioClip = audioClip;
            }
            public RagdollHand ragdollHand;
            public bool isInteractable;
            public bool showHint;
            public string hintTitle;
            public string hintDesignation;
            public Color hintColor;
            public AudioClip audioClip;
        }

        [Serializable]
        public class HighlightParams
        {
            public HighlightParams Copy()
            {
                return (HighlightParams)this.MemberwiseClone();
            }
            public Color touchColor = Color.yellow;
            public Color proximityColor = Color.white;
            public float proximityDistance = 2;
        }

#if ODIN_INSPECTOR
        public List<ValueDropdownItem<string>> GetAllInteractableID()
        {
            return Catalog.GetDropdownAllID(Category.Interactable);
        }
#endif

    }
}
