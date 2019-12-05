using UnityEngine;
using System;

namespace BS
{
    public class EffectMesh : Effect
    {
        public int poolCount = 20;
        public float lifeTime = 5;
        public float refreshSpeed = 0.1f;

        [NonSerialized]
        public float playTime;

        [Header("Color Gradient")]
        public EffectTarget linkBaseColor = EffectTarget.None;
        public EffectTarget linkEmissionColor = EffectTarget.None;

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
            materialPropertyBlock = new MaterialPropertyBlock();
            renderer = GetComponent<Renderer>();
        }

        private void Awake()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            renderer = GetComponent<Renderer>();
            renderer.enabled = false;
        }

        public override void Play()
        {
            CancelInvoke();
            playTime = Time.time;
            renderer.enabled = true;
            if (step != Step.Loop && lifeTime > 0)
            {
                InvokeRepeating("UpdateLifeTime", 0, refreshSpeed);
            }
        }

        public override void Stop()
        {
            renderer.enabled = false;
            Despawn();
        }

        protected void UpdateLifeTime()
        {
            float value = Mathf.Clamp01(1 - ((Time.time - playTime) / lifeTime));
            SetIntensity(value);
            if (value == 0) Despawn();
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
            CancelInvoke();
            renderer.enabled = false;
#if ProjectCore
            if (Application.isPlaying)
            {
                EffectInstance orgEffectInstance = effectInstance;
                effectInstance = null;
                EffectModuleMesh.Despawn(this);
                orgEffectInstance.OnEffectDespawn();
            }
#endif
        }
    }
}
