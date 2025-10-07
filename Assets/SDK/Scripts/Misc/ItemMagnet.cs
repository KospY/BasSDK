using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/ItemMagnet.html")]
    [AddComponentMenu("ThunderRoad/Item magnet")]
    [RequireComponent(typeof(Collider))]
    public class ItemMagnet : MonoBehaviour
    {
        [Tooltip("Select if it accepts any but selected or accepts only selected.")]
        public FilterLogic tagFilter = FilterLogic.AnyExcept;
        [Tooltip("Slots that accept the magnet.")]
        public List<string> slots;

        public delegate void ItemEvent(Item item, EventTime time);
        public event ItemEvent OnItemCatchEvent;
        public event ItemEvent OnItemReleaseEvent;

        public UnityEvent OnItemCatched;
        public UnityEvent OnItemReleased;
        public UnityEvent<bool> OnItemStabilized;

        [Tooltip("Does it lock the item in to a kinematic state?")]
        public bool kinematicLock;
        [Tooltip("Will it release the kinematic state if the item is ONLY grabbed or telekinesis grabbed.")]
        public bool releaseOnGrabOrTKOnly;
        [Tooltip("Will the kinematic lock be ignored by Gravity push?")]
        public bool catchedItemIgnoreGravityPush;
        [Tooltip("When enabled, it enables the collission with the joint rigidbody")]
        public bool enableCollisionWithJointRigidbody;

        [Tooltip("When ticked, it will auto-ungrab the item when magnetised.")]
        public bool autoUngrab;
        [Tooltip("When released, the item will have this delay before the magnet takes place")]
        public float magnetReactivateDurationOnRelease;
        [Tooltip("What is the gravity multiplier of the item held by the magnet?")]
        public float gravityMultiplier = 0;
        [Tooltip("What is the mass multiplier of them item held by the magnet?")]
        public float massMultiplier = 1;
        [Tooltip("What is the rigidbody sleep threshold of the held item?")]
        public float sleepThresholdRatio = 0;
        [Tooltip("How many items can be held by the magnet?")]
        public int maxCount = 1;
        [Tooltip("The radius of the progressive force movement of the item to the magnet")]
        public float progressiveForceRadius = 0.5f;

        [Tooltip("The max distance before the item is stabilized.")]
        public float stabilizedMaxDistance = 0.01f;
        [Tooltip("The max angle of the item when it is stabilized")]
        public float stabilizedMaxAngle = 360f;
        [Tooltip("The max up angle of the item when it is stabilized.")]
        public float stabilizedMaxUpAngle = 10f;
        [Tooltip("The maximum velocity of the item before it is stabilized.")]
        public float stabilizedMaxVelocity = 0.2f;

        [Tooltip("The position mover spring of the magnetized item.")]
        public float positionSpring = 50f;
        [Tooltip("The position mover damper of the magnetized item")]
        public float positionDamper = 5f;
        [Tooltip("The maximum force the item takes to go to the magnetized area.")]
        public float positionMaxForce = 500f;

        [Tooltip("The rotation spring of the magnetized item.")]
        public float rotationSpring = 50f;
        [Tooltip("The rotation damper of the magnetized item")]
        public float rotationDamper = 1f;
        [Tooltip("The maximum rotation force of the item")]
        public float rotationMaxForce = 300f;

        [NonSerialized]
        public bool isLocked;



        public void Lock()
        {
        }

        public void Unlock()
        {
        }

        public void ReleaseItem(Item item)
        {
        }

        /// <summary>
        /// Only work for one item
        /// </summary>
        /// <param name="target"></param>
        public void ReleaseAndMove(Transform target)
        {
        }

        public void ReleaseAllItems()
        {
        }
    }
}
