using UnityEngine;
using System;

namespace BS
{
    public class EffectMesh : Effect
    {
        [Header("Color Gradient")]
        public EffectTarget linkBaseColor = EffectTarget.None;
        public EffectTarget linkEmissionColor = EffectTarget.None;

        [Header("Mesh Display")]
        public bool meshDisplay;

        [Header("Intensity to mesh size")]
        public bool meshSize;
        public AnimationCurve curveMeshSize;

        [NonSerialized]
        public float currentValue;

        [NonSerialized, GradientUsage(true)]
        public Gradient currentMainGradient;
        [NonSerialized, GradientUsage(true)]
        public Gradient currentSecondaryGradient;

        [NonSerialized]
        public MaterialPropertyBlock materialPropertyBlock;

        protected new Renderer renderer;

        private void OnValidate()
        {
            renderer = GetComponent<Renderer>();
            materialPropertyBlock = new MaterialPropertyBlock();
        }

        private void Awake()
        {
            if (meshDisplay)
            {
                renderer = GetComponent<Renderer>();
                renderer.enabled = false;
            }
                
        }

        public override void Play()
        {
            if (meshDisplay)
            {
                renderer.enabled = true;
            }
        }

        public override void Stop()
        {
            if (meshDisplay)
            {
                renderer.enabled = false;
            }
        }

        public override void SetIntensity(float value)
        {
            currentValue = value;

            if (meshSize)
            {
                float meshSizeValue = curveMeshSize.Evaluate(value);
                transform.localScale = new Vector3(meshSizeValue, meshSizeValue, meshSizeValue);
            }

            // Set material color
            bool updatePropertyBlock = false;
            if (linkBaseColor == EffectTarget.Main && currentMainGradient != null)
            {
                materialPropertyBlock.SetColor("_BaseColor", currentMainGradient.Evaluate(value));
                updatePropertyBlock = true;
            }
            else if (linkBaseColor == EffectTarget.Secondary && currentSecondaryGradient != null)
            {
                materialPropertyBlock.SetColor("_BaseColor", currentSecondaryGradient.Evaluate(value));
                updatePropertyBlock = true;
            }
            if (linkEmissionColor == EffectTarget.Main && currentMainGradient != null)
            {
                materialPropertyBlock.SetColor("_EmissionColor", currentMainGradient.Evaluate(value));
                updatePropertyBlock = true;
            }
            else if (linkEmissionColor == EffectTarget.Secondary && currentSecondaryGradient != null)
            {
                materialPropertyBlock.SetColor("_EmissionColor", currentSecondaryGradient.Evaluate(value));
                updatePropertyBlock = true;
            }

            if (updatePropertyBlock) renderer.SetPropertyBlock(materialPropertyBlock);
        }

        public override void SetMainGradient(Gradient gradient)
        {
            currentMainGradient = gradient;
            SetIntensity(currentValue);
        }

        public override void SetSecondaryGradient(Gradient gradient)
        {
            currentSecondaryGradient = gradient;
            SetIntensity(currentValue);
        }

        public override void Despawn()
        {
            renderer.enabled = false;
#if ProjectCore
                if (Application.isPlaying)
                {
                    EffectInstance orgEffectInstance = effectInstance;
                    effectInstance = null;
                    //EffectModuleMesh.Despawn(this);
                    orgEffectInstance.OnEffectDespawn();
                }
#endif
        }
    }
}
