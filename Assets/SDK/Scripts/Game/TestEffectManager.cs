using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System.Collections;
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
        public class EffectAnim
        {
            public Animator animator;
            public string animTrigger;
        }

            [System.Serializable]
        public class EffectCollection
        {
            public string name = "Effect Name";
            public List<TestEffect> effects = new List<TestEffect>();

            [Header("Animation")]
            public List<EffectAnim> animations = new List<EffectAnim>();

            [Button]
            public void PauseAnimation ()
            {
                if (animations.Count != 0)
                {
                    foreach (EffectAnim anim in animations)
                        anim.animator.speed = 0f;
                }
            }

            [Button]
            public void PlayAnimation()
            {
                if (animations.Count != 0)
                {
                    foreach (EffectAnim anim in animations)
                        anim.animator.speed = 1f;
                }
            }

            [Header("Timelines")]
            public PlayableDirector director;
            public TimelineAsset timeline;


            [Button]
            public void Play_Pause()
            {
                if (director == null) { return; }

                if (director.state == PlayState.Playing)
                    director.Pause();
                else
                    director.Play();
            }

            [Button]
            public void Stop()
            {
                if (director == null) { return; }
                director.Stop();
                director.playableGraph.GetRootPlayable(0).
            }


            [Range(0, 1)]
            public float intensity = 1f;
            private float lastIntensity;

            [Button]
            public void PlayEffect()
            {
                //Animation
                if (animations.Count != 0)
                {
                    foreach (EffectAnim anim in animations)
                    {
                        if (anim.animator != null && anim.animTrigger != null)
                        {
                            anim.animator.enabled = true;
                            anim.animator.SetTrigger(anim.animTrigger);
                        }
                    }
                }

                foreach (TestEffect effect in effects)
                {
                    effect.Play();
                }
            }
            /*
            private IEnumerator PlayEffectActions()
            {
                //Animation
                if (animations.Count != 0)
                {
                    foreach (EffectAnim anim in animations)
                    {
                        if (anim.animator != null && anim.animTrigger != null)
                        {
                            anim.animator.enabled = true;
                            anim.animator.SetTrigger(anim.animTrigger);
                        }
                    }
                }

                yield return new WaitForSeconds(0.001f);
                foreach (TestEffect effect in effects)
                {
                    effect.Play();
                }
            }*/

            [Button]
            public void StopEffect()
            {
                //Animation
                foreach (EffectAnim anim in animations)
                {
                    if (anim.animator != null && anim.animTrigger != null)
                    {
                        anim.animator.enabled = false;
                    }
                }

                foreach (TestEffect effect in effects)
                {
                    effect.Stop();
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
