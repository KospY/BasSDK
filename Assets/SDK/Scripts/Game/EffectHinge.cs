using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class EffectHinge : MonoBehaviour
    {
        public new HingeJoint hingeJoint;

#if ProjectCore
        [ReadOnly]
        public float currentIntensity;
        protected ItemPhysic.EffectHinge data;
        protected EffectInstance effectInstance;
        protected Rigidbody jointRb;

        protected bool loaded;

        private void OnDisable()
        {
            effectInstance.Stop();
            loaded = false;
        }

        public virtual void Load(ItemPhysic.EffectHinge effectHingeData)
        {
            if (effectHingeData != null && hingeJoint != null)
            {
                data = effectHingeData;
                EffectData effectData = Catalog.GetData<EffectData>(effectHingeData.effectId);
                effectInstance = effectData.Spawn(this.transform.position, this.transform.rotation, this.transform);
                effectInstance.SetIntensity(0);
                effectInstance.Play();
                jointRb = hingeJoint.GetComponent<Rigidbody>();
                loaded = true;
            }
        }

        protected virtual void Update()
        {
            if (loaded)
            {
                currentIntensity = Mathf.InverseLerp(data.minTorque, data.maxTorque, jointRb.angularVelocity.magnitude);
                effectInstance.SetIntensity(currentIntensity);
            }
        }
#endif
    }
}
