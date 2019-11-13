using UnityEngine;
using System;

namespace BS
{
    public class EffectMesh : Effect
    {
        [Header("Color Gradient")]
        public LinkedGradient linkBaseColor = LinkedGradient.None;
        public LinkedGradient linkEmissionColor = LinkedGradient.None;

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

        protected MeshRenderer rend;

        private void OnValidate()
        {
            rend = GetComponent<MeshRenderer>();
            materialPropertyBlock = new MaterialPropertyBlock();
        }

        private void Awake()
        {
            if (meshDisplay)
            {
                rend = GetComponent<MeshRenderer>();
                rend.enabled = false;
            }
                
        }

        public override void Play()
        {
            if (meshDisplay)
                rend.enabled = true;
        }

        public override void Stop()
        {
            if (meshDisplay)
                rend.enabled = false;
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
            if (linkBaseColor == LinkedGradient.Main && currentMainGradient != null)
            {
                materialPropertyBlock.SetColor("_BaseColor", currentMainGradient.Evaluate(value));
                updatePropertyBlock = true;
            }
            else if (linkBaseColor == LinkedGradient.Secondary && currentSecondaryGradient != null)
            {
                materialPropertyBlock.SetColor("_BaseColor", currentSecondaryGradient.Evaluate(value));
                updatePropertyBlock = true;
            }
            if (linkEmissionColor == LinkedGradient.Main && currentMainGradient != null)
            {
                materialPropertyBlock.SetColor("_EmissionColor", currentMainGradient.Evaluate(value));
                updatePropertyBlock = true;
            }
            else if (linkEmissionColor == LinkedGradient.Secondary && currentSecondaryGradient != null)
            {
                materialPropertyBlock.SetColor("_EmissionColor", currentSecondaryGradient.Evaluate(value));
                updatePropertyBlock = true;
            }

            if (updatePropertyBlock) rend.SetPropertyBlock(materialPropertyBlock);
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
            rend.enabled = false;
#if ProjectCore
                if (Application.isPlaying)
                {
                    EffectInstance orgEffectInstance = effectInstance;
                    effectInstance = null;
                    EffectModuleVfx.Despawn(this);
                    orgEffectInstance.OnEffectDespawn();
                }
#endif
        }
    }
}
