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
        protected float lastManaValue;
        protected float lastHealthValue;

        [BoxGroup("Effects"), ValueDropdown("GetAllEffectID")]
        public string lifeEffectId;
        [BoxGroup("Effects")]
        public Transform lifeEffectParent;
        [BoxGroup("Effects"), ValueDropdown("GetAllEffectID")]
        public string manaEffectId;
        [BoxGroup("Effects")]
        public Transform manaEffectParent;
        [BoxGroup("Effects"), ValueDropdown("GetAllEffectID")]
        public string focusEffectId;
        [BoxGroup("Effects")]
        public Transform focusEffectParent;

        private EffectData lifeEffectData;
        private EffectData manaEffectData;
        private EffectData focusEffectData;

        private EffectInstance lifeEffectInstance;
        private EffectInstance manaEffectInstance;
        private EffectInstance focusEffectInstance;

        public bool initialized = false;
        public List<ValueDropdownItem<string>> GetAllEffectID()
        {
            return Catalog.GetDropdownAllID(Catalog.Category.Effect);
        }

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

            lifeEffectData = Catalog.GetData<EffectData>(lifeEffectId);
            manaEffectData = Catalog.GetData<EffectData>(manaEffectId);
            focusEffectData = Catalog.GetData<EffectData>(focusEffectId);

            initialized = true;
        }

        void UpdateLife()
        {
            lifeEffectInstance.SetIntensity(creature.health.currentHealth / creature.health.maxHealth);
        }

        void UpdateMana()
        {
            manaEffectInstance.SetIntensity(creature.mana.currentMana / creature.mana.maxMana);
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
                    if (lastHealthValue != creature.health.currentHealth)
                    {
                        UpdateLife();
                    }
                }
                if (creature.mana)
                {
                    if (lastManaValue != creature.mana.currentMana)
                    {
                        UpdateMana();
                    }
                }
            }
        }

        [Button]
        void Show(bool active)
        {
            if (active)
            {
                lifeEffectInstance = lifeEffectData.Spawn(lifeEffectParent);
                manaEffectInstance = manaEffectData.Spawn(manaEffectParent);
                focusEffectInstance = focusEffectData.Spawn(focusEffectParent);

                lifeEffectInstance.Play();
                manaEffectInstance.Play();
                focusEffectInstance.Play();
            }
            else
            {
                if (lifeEffectInstance != null) lifeEffectInstance.Stop();
                if (manaEffectInstance != null) manaEffectInstance.Stop();
                if (focusEffectInstance != null) focusEffectInstance.Stop();
            }
        }
#endif
    }
}