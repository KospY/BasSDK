using UnityEngine;

namespace ThunderRoad
{
    public class LiquidReceiver : MonoBehaviour
    {
        public float maxAngle = 30;
        public float stopDelay = 0.1f;
        public float effectRate = 1f;

#if ProjectCore
        protected AudioSource audioSource;
        protected LiquidContainer liquidContainer;
        protected float lastLiquidReceivedTime;
        protected float lastEffectTime;
        protected bool isReceiving;
        protected Collider colliderReceiver;

        public delegate void ReceptionEvent(ItemModulePotion.Content content);
        public event ReceptionEvent OnReceptionEvent;

        void Awake()
        {
            audioSource = this.GetComponent<AudioSource>();
            liquidContainer = this.GetComponentInParent<LiquidContainer>();
            colliderReceiver = this.GetComponent<Collider>();
        }

        void Start()
        {
            colliderReceiver.gameObject.SetLayerRecursively(VRManager.GetLayer(LayerName.LiquidFlow));
        }

        void Update()
        {
            if (Vector3.Angle(Vector3.up, this.transform.up) < maxAngle)
            {
                if (!colliderReceiver.enabled) colliderReceiver.enabled = true;
            }
            else
            {
                if (colliderReceiver.enabled) colliderReceiver.enabled = false;
            }

            if (isReceiving && (Time.time - lastLiquidReceivedTime) >= stopDelay)
            {
                if (audioSource) audioSource.Stop();
                isReceiving = false;
            }
        }

        void OnParticleCollision(GameObject other)
        {
            var liquidContainer = other.GetComponentInParent<LiquidContainer>();
            if (liquidContainer == null) Debug.LogError("liquidcontainer is null " + other.name);
            OnLiquidReceived(other.GetComponentInParent<LiquidContainer>());
        }

        public virtual void OnLiquidReceived(LiquidContainer liquidContainer)
        {
            lastLiquidReceivedTime = Time.time;
            isReceiving = true;
            // Apply effects
            if ((Time.time - lastEffectTime) > effectRate)
            {
                if (liquidContainer.liquids == null) Debug.LogError("liquidcontainer content is null " + liquidContainer.name);
                foreach (ItemModulePotion.Content content in liquidContainer.liquids)
                {
                    OnReceptionEvent?.Invoke(content);
                }
                lastEffectTime = Time.time;
                if (audioSource) audioSource.Play();
            }
        }
#endif
    }
}