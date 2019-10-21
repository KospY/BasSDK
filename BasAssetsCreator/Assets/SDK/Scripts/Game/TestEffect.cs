using UnityEngine;
using System;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine.Experimental.VFX;

namespace BS
{
    public class TestEffect : MonoBehaviour
    {
        [Range(0, 1)]
        public float intensity;

        [ColorUsage(true, true)]
        public Color mainColor = Color.white;
        [ColorUsage(true, true)]
        public Color secondaryColor = Color.white;

        public Transform target;

        public Mesh mesh;

        protected ParticleSystem rootParticleSystem;
        protected List<ParticleController> particleControllers = new List<ParticleController>();
        protected List<VisualEffect> visualEffects = new List<VisualEffect>();
        protected List<LightController> lightControllers = new List<LightController>();

        private void OnValidate()
        {
            particleControllers = new List<ParticleController>(this.GetComponentsInChildren<ParticleController>());
            visualEffects = new List<VisualEffect>(this.GetComponentsInChildren<VisualEffect>());
            lightControllers = new List<LightController>(this.GetComponentsInChildren<LightController>());
            rootParticleSystem = this.GetComponent<ParticleSystem>();
            if (!rootParticleSystem)
            {
                rootParticleSystem = this.gameObject.AddComponent<ParticleSystem>();
                ParticleSystem.EmissionModule emissionModule = rootParticleSystem.emission;
                emissionModule.enabled = false;
                ParticleSystem.ShapeModule shapeModule = rootParticleSystem.shape;
                shapeModule.enabled = false;
                rootParticleSystem.GetComponent<ParticleSystemRenderer>().enabled = false;
            }

            foreach (ParticleController p in particleControllers)
            {
                p.SetIntensity(intensity);
                p.SetColor(mainColor, secondaryColor);
                p.SetTarget(transform);
                p.SetMesh(mesh);

            }
            foreach (VisualEffect vfx in visualEffects)
            {
                vfx.SetFloat("Intensity", intensity);
                vfx.SetVector4("MainColor", new Vector4(mainColor.r, mainColor.g, mainColor.b, mainColor.a));
                vfx.SetVector4("SecondaryColor", new Vector4(secondaryColor.r, secondaryColor.g, secondaryColor.b, secondaryColor.a));
                if (mesh) vfx.SetMesh("Mesh", mesh);
            }
            foreach (LightController lc in lightControllers)
            {
                lc.SetIntensity(intensity);
                lc.SetColor(mainColor);
            }
        }

        protected void Update()
        {
            if (target)
            {
                foreach (VisualEffect vfx in visualEffects)
                {
                    vfx.SetVector3("Target_position", target.position);
                    vfx.SetVector3("Target_angles", target.eulerAngles);
                    vfx.SetVector3("Target_scale", target.localScale);
                }
            }
        }

        [Button]
        public virtual void Play()
        {
            rootParticleSystem.Play();
            foreach (VisualEffect vfx in visualEffects)
            {
                vfx.Play();
            }
            foreach (LightController lc in lightControllers)
            {
                lc.SetActive(true);
            }
        }

        [Button]
        public virtual void Stop()
        {
            rootParticleSystem.Stop();
            foreach (VisualEffect vfx in visualEffects)
            {
                vfx.Stop();
            }
            foreach (LightController lc in lightControllers)
            {
                lc.SetActive(false);
            }
        }
    }
}
