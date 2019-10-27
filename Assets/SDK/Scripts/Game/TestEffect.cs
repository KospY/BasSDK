using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Experimental.VFX;
#if ProjectCore
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

        public Gradient mainGradient;
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
                effect.SetMainGradient(mainGradient);
                effect.SetSecondaryGradient(secondaryGradient);
                effect.SetTarget(transform);
                if (mesh) effect.SetMesh(mesh);
                if (collider) effect.SetCollider(collider);
            }
        }

        [Button]
        public virtual void Play()
        {
            rootParticleSystem.Play();
            foreach (Effect effect in effects)
            {
                effect.Play();
            }
        }

        [Button]
        public virtual void Stop()
        {
            rootParticleSystem.Stop();
            foreach (Effect effect in effects)
            {
                effect.Stop();
            }
        }
    }
}
