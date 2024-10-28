using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Levels/Shaker")]
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Levels/Shaker.html")]
    public class Shaker : MonoBehaviour
    {
        [Header("Global")]
        [Tooltip("References the zone of which apples shaking to items/players")]
        public Zone zone;
        [Tooltip("The audio that plays when shaking is active.")]
        public AudioSource audioSource;
        [Tooltip("How long the startup of the shaking takes.")]
        public float startupDuration = 2;
        [Tooltip("How long the ending of the shaking takes.")]
        public float endDuration = 2;

        [Header("Items")]
        [Tooltip("The amount of force added to items in shake (X=Minimum, Y=Maximum)")]
        public Vector2 itemShakeMinMaxForce = new Vector2(0.01f, 0.1f);
        [Tooltip("Time between each individual shake for items (X=Minimum, Y=Maximum)")]
        public Vector2 itemShakeInterval = new Vector2(0.005f, 0.01f);
        [Tooltip("The maximum random angle (away from the shake axis), along which shake force is applied.")]
        public float coneAngle = 20f;
        [Tooltip("The axis that the item shakes on.")]
        public Vector3 direction = Vector3.up;
        [Tooltip("Ignores listed items in the scene.")]
        public List<Item> ignoreItems;
        [Tooltip("Ignores items inside listed item magnets.")]
        public List<ItemMagnet> ignoreItemMagnets;
        [Tooltip("List items that will recieve zero drag during shake.")]
        public List<Item> zeroDragItems;

        [Header("Player")]
        [Tooltip("Does the player camera shake?")]
        public bool playerCameraShake = true;
        [Tooltip("The intensity of the camera shaking.")]
        public Vector2 cameraShakeMinMaxIntensity = new Vector2(0.005f, 0.01f);

        [Header("Events")]
        public UnityEvent onShakeBegin;
        public UnityEvent onShakeEnd;

        protected Vector3 orgPlayerOffsetLocalPosition;
        protected float shakeIntensity;
        protected Coroutine audioCoroutine;
        protected Coroutine fadeIntensityCoroutine;
        protected Coroutine itemShakeCoroutine;
        protected Coroutine playerShakeCoroutine;
        protected Coroutine endCoroutine;

        [Button]
        public void End()
        {
        }

        [Button]
        public void Begin()
        {
        }
        
    }
}