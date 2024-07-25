using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/PlayerLock.html")]
    public class PlayerLock : MonoBehaviour
    {
        //mock the hand button enum
        public class PlayerControl {
            public class Hand
            {
                public enum Button
                {
                    Use,
                    AlternateUse,
                    Grip,
                    Stick
                }
            }
        }
        
        [Header("Player lock")]
        [Tooltip("Disables Locomotion/Movement when locked")]
        public bool disableLocomotion = true;
        [Tooltip("Disables Player-Controlled movement when locked")]
        public bool disableMove = true;
        [Tooltip("Disables player rotation when locked")]
        public bool disableTurn = true;
        [Tooltip("Disables jumping when locked")]
        public bool disableJump = true;
        [Tooltip("Disables Slow-Motion/Hyperfocus when locked")]
        public bool disableSlowMotion = true;
        [Tooltip("Disables casting when locked")]
        public bool disableCasting = true;
        [Tooltip("When locked, player is invincible")]
        public bool invincible = false;
        [Tooltip("When enabled, player camera is disabled when locked. This can be used for other camera uses, like cutscenes")]
        public bool disablePlayerCamera = false;
        [Tooltip("Disables the Option Menu when locked")]
        public bool disableOptionMenu = false;

        [Header("Button press event")]
        [Tooltip("Will play the On Button Press () event if the \"Trigger Button\" is pressed.")]
        public PlayerControl.Hand.Button triggerButton = PlayerControl.Hand.Button.Use;
        public UnityEvent onButtonPress;

        public void OnButtonPress(PlayerControl.Hand.Button button, bool pressed)
        {
 //projectcore
        }

        [Button]
        public void Lock()
        {
 //projectcore
        }

        [Button]
        public void Unlock()
        {
 //projectcore
        }
    }
}