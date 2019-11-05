using UnityEngine;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace BS
{
    public class TestEffectManager : MonoBehaviour
    {
        public List<EffectCollection> effects = new List<EffectCollection>();

        [System.Serializable]
        public class EffectCollection
        {
            public string name = "Effect Name";
            public List<TestEffect> effects = new List<TestEffect>();

            [Header("Animation")]
            public Animator animator;
            public string animTrigger;

            [Button]
            public void PauseAnimation ()
            {
                animator.speed = 0f;
            }

            [Button]
            public void PlayAnimation()
            {
                animator.speed = 1f;
            }


            [Range(0, 1)]
            public float intensity = 1f;
            private float lastIntensity;

            [Button]
            public void PlayEffect()
            {
                foreach (TestEffect effect in effects)
                {
                    effect.Play();
                }
                //Animation
                if (animator != null && animator != null)
                {
                    animator.enabled = true;
                    animator.SetTrigger(animTrigger);
                }
            }

            [Button]
            public void StopEffect()
            {
                foreach (TestEffect effect in effects)
                {
                    effect.Stop();
                }
                //Animation
                if (animator != null && animator != null)
                {
                    animator.enabled = false;
                }
            }

            public void CheckIntensityChange()
            {
                if (intensity == lastIntensity) { return; }
                foreach (TestEffect effect in effects)
                {
                    effect.intensity = intensity;
                    effect.OnValidate();
                }

                lastIntensity = intensity;
            }
        }

        private void Update()
        {
            foreach (EffectCollection effect in effects)
            {
                effect.CheckIntensityChange();
            }
        }


    }
}
