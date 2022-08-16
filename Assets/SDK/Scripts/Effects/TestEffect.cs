using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif



namespace ThunderRoad
{
    public class TestEffect : MonoBehaviour
    {
        [Range(0, 1)]
        public float intensity;
        [Range(0, 1)]
        public float speed;

        public bool useMainGradient;
        [GradientUsage(true)]
        public Gradient mainGradient;

        public bool useSecondaryGradient;
        [GradientUsage(true)]
        public Gradient secondaryGradient;

        public Transform target;

        public Mesh mesh;
        public Renderer mainRenderer;
        public Renderer secondaryRenderer;
        public new Collider collider;

        protected ParticleSystem rootParticleSystem;
        protected List<Effect> effects = new List<Effect>();

        public void OnValidate()
        {
            rootParticleSystem = this.GetComponent<ParticleSystem>();
            effects = new List<Effect>(this.GetComponentsInChildren<Effect>());

            if (!rootParticleSystem)
            {
                rootParticleSystem = this.gameObject.AddComponent<ParticleSystem>();
                ParticleSystem.EmissionModule emissionModule = rootParticleSystem.emission;
                emissionModule.enabled = false;
                ParticleSystem.ShapeModule shapeModule = rootParticleSystem.shape;
                shapeModule.enabled = false;
                rootParticleSystem.GetComponent<ParticleSystemRenderer>().enabled = false;
            }
            Refresh();
        }

        public void Refresh()
        {
            foreach (Effect effect in effects)
            {
                if (target) effect.SetTarget(target);
                if (mesh) effect.SetMesh(mesh);
                if (mainRenderer) effect.SetRenderer(mainRenderer, false);
                if (secondaryRenderer) effect.SetRenderer(secondaryRenderer, true);
                if (collider) effect.SetCollider(collider);
                if (useMainGradient) effect.SetMainGradient(mainGradient);
                if (useSecondaryGradient) effect.SetSecondaryGradient(secondaryGradient);
                effect.SetIntensity(intensity);
                effect.SetSpeed(speed);
            }
        }

        [Button]
        public void Play()
        {
            Refresh();
            rootParticleSystem.Play();
            foreach (Effect effect in effects)
            {
                if (effect.step != Effect.Step.End)
                {
                    effect.Play();
                }
            }
        }

        [Button]
        public void TestIntensity(float duration)
        {
            Play();
            StartCoroutine (TestIntensityAction(duration));

        }

        private IEnumerator TestIntensityAction (float duration)
        {
            float startTime = Time.time;
            float t = 0;

            while (t <= 1)
            {
                t = (Time.time - startTime) / duration;
                intensity = Mathf.Lerp(0f, 1f, t);
                Refresh();
                yield return null;
            }
        }

        [Button]
        public void Stop()
        {
            rootParticleSystem.Stop();
            foreach (Effect effect in effects)
            {
                if (effect.step == Effect.Step.End)
                {
                    effect.Play();
                }
                else
                {
                    effect.End();
                }
            }
        }
    }
}
