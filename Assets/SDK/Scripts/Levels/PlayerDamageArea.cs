using UnityEngine;
using System;
using System.Collections;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    public class PlayerDamageArea : MonoBehaviour
    {
        [ReadOnly]
        public Collider triggerCollider;
        [Header("General")]
        public float damageStartupDelay = 2;
        public float damageInterval = 2;
        public DamageType damageType = DamageType.Unknown;
        public float damageValue = 5;
        public Vector3 attractionForce;

        [Header("Audio")]
        public AudioContainer damageAudioContainer;
        public AudioContainer hurtMaleAudioContainer;
        public AudioContainer hurtFemaleAudioContainer;
        protected AudioSource audioSource;

 //projectcore
        private void OnValidate()
        {

            gameObject.layer = Common.GetLayer(LayerName.Zone);
            triggerCollider = this.GetComponent<Collider>();
            if (!triggerCollider)
            {
                Debug.LogErrorFormat(this, $"{nameof(PlayerDamageArea)} need a trigger collider!");
            }
            else
            {
                triggerCollider.isTrigger = true;
            }
        }
 //projectcore
    }
}
