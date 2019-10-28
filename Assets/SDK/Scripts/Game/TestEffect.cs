using UnityEngine;
using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace BS
{
    public class TestEffect : MonoBehaviour
    {
        [Range(0, 1)]
        public float intensity;

        public bool useMainGradient;
        [GradientUsage(true)]
        public Gradient mainGradient;

        public bool useSecondaryGradient;
        [GradientUsage(true)]
        public Gradient secondaryGradient;

        public Transform target;

        public Mesh mesh;
        public new Collider collider;

        protected ParticleSystem rootParticleSystem;
        protected List<Effect> effects = new List<Effect>();

        private void OnValidate()
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

            foreach (Effect effect in effects)
            {
                effect.SetIntensity(intensity);
                if (useMainGradient) effect.SetMainGradient(mainGradient);
                if (useSecondaryGradient) effect.SetSecondaryGradient(secondaryGradient);
                if (target) effect.SetTarget(target);
                if (mesh) effect.SetMesh(mesh);
                if (collider) effect.SetCollider(collider);
            }
        }

        [Button]
        public void Play()
        {
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
                    effect.Stop();
                }
            }
        }
    }
}
