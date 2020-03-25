using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BS;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;

namespace BS
{
    public class WristStats : MonoBehaviour
    {
        [HideInInspector]
        public float showDistance = 0.5f;
        [HideInInspector]
        public float showAngle = 30f;
        [HideInInspector]
        public Vector3 localPosition;
        [HideInInspector]
        public Vector3 localRotation;

        protected bool isShown = true;
        protected int lastManaValue;
        protected int lastHealthValue;

        [BoxGroup("Elements")]
        public EffectSpawner lifeEffectSpawner;
        [BoxGroup("Elements")]
        public EffectSpawner manaEffectSpawner;
        [BoxGroup("Elements")]
        public EffectSpawner focusEffectSpawner;

        [BoxGroup("FX positions")]
        public Vector3 manaPosition;
        [BoxGroup("FX positions")]
        public Vector3 manaRotation;

        public bool initialized = false;
#if ProjectCore
        protected Creature creature;

        public void Init()
        {
            creature = this.GetComponentInParent<Creature>();
            StartCoroutine(DelayedInit());
        }

        protected IEnumerator DelayedInit()
        {
            BodyHand bodyHand = this.GetComponentInParent<BodyHand>();
            while (!bodyHand.body.initialized) yield return null;
            Transform armTwist = bodyHand.lowerArmBone.Find(bodyHand.side == Side.Left ? "LeftForeArmTwist" : "RightForeArmTwist");
            this.transform.SetParent(armTwist, true);
            this.transform.localPosition = localPosition;
            this.transform.localRotation = Quaternion.Euler(localRotation);

            initialized = true;
        }

        void UpdateLife()
        {
            lifeEffectSpawner.intensity = Mathf.CeilToInt((creature.health.currentHealth) / creature.health.maxHealth);
        }

        void UpdateMana()
        {
            manaEffectSpawner.intensity = Mathf.CeilToInt((creature.mana.currentMana) / creature.mana.maxMana);
        }

        void Update()
        {
            if (!creature.centerEyes) return;
            if (!initialized) return;

            float eyesDistance = Vector3.Distance(creature.centerEyes.position, this.transform.position);
            float eyesAngle = Vector3.Angle(-creature.centerEyes.forward, this.transform.forward);

            if (eyesDistance < showDistance && eyesAngle < showAngle)
            {
                if (!isShown)
                {
                    Show(true);
                    isShown = true;
                }
            }
            else if (isShown)
            {
                Show(false);
                isShown = false;
            }

            if (isShown)
            {
                if (creature.health)
                {
                    int healthInt = Mathf.CeilToInt(creature.health.currentHealth);
                    if (lastHealthValue != healthInt)
                    {
                        UpdateLife();
                    }
                }
                if (creature.mana)
                {
                    int manaInt = Mathf.CeilToInt(creature.mana.currentMana);
                    if (lastManaValue != manaInt)
                    {
                        UpdateMana();
                    }
                }
            }
        }

        [Button]
        void Show(bool active)
        {
            lifeEffectSpawner.gameObject.SetActive(active);
            manaEffectSpawner.gameObject.SetActive(active);
            focusEffectSpawner.gameObject.SetActive(active);

            lifeEffectSpawner.Spawn();
            manaEffectSpawner.Spawn();
            focusEffectSpawner.Spawn();

            manaEffectSpawner.transform.GetChild(0).localRotation = Quaternion.Euler(manaRotation);
            manaEffectSpawner.transform.GetChild(1).localRotation = Quaternion.Euler(manaRotation);

            manaEffectSpawner.transform.GetChild(0).localPosition = manaPosition;
            manaEffectSpawner.transform.GetChild(1).localPosition = manaPosition;
        }
#endif
    }
}