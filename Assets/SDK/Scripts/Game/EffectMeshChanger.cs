using UnityEngine;
using System;

namespace BS
{
    public class EffectMeshChanger : Effect
    {
        public LinkedGradient linkBaseColor = LinkedGradient.None;
        public LinkedGradient linkEmissionColor = LinkedGradient.None;

        public float lifeTime = 0;
        public float refreshSpeed = 0.1f;

        public enum LinkedGradient
        {
            None,
            Main,
            Secondary,
        }

        [NonSerialized]
        public MeshRenderer meshRenderer;

        protected static int colorPropertyID;
        protected static int emissionPropertyID;

        protected MaterialPropertyBlock materialPropertyBlock;

        [NonSerialized]
        public float currentValue;
        [NonSerialized, GradientUsage(true)]
        public Gradient currentMainGradient;
        [NonSerialized, GradientUsage(true)]
        public Gradient currentSecondaryGradient;

        private void OnValidate()
        {
            if (!meshRenderer) meshRenderer = this.GetComponent<MeshRenderer>();
            materialPropertyBlock = new MaterialPropertyBlock();
            if (colorPropertyID == 0) colorPropertyID = Shader.PropertyToID("_Color");
            if (emissionPropertyID == 0) emissionPropertyID = Shader.PropertyToID("_EmissionColor");
        }

        private void Awake()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            if (colorPropertyID == 0) colorPropertyID = Shader.PropertyToID("_Color");
            if (emissionPropertyID == 0) emissionPropertyID = Shader.PropertyToID("_EmissionColor");
        }

        public override void Play()
        {
            if (step != Step.Loop && lifeTime > 0)
            {
                InvokeRepeating("UpdateLifeTime", 0, refreshSpeed);
            }
        }

        public override void Stop()
        {
            Despawn();
        }

        protected void UpdateLifeTime()
        {
            float value = Mathf.Clamp01((Time.time - spawnTime) / lifeTime);
            SetIntensity(value);
            if (value == 1) Despawn();
        }

        public override void SetMeshRenderer(MeshRenderer meshRenderer)
        {
            this.meshRenderer = meshRenderer;
        }

        public override void SetIntensity(float value)
        {
            currentValue = value;
            if (meshRenderer.isVisible)
            {
                if (linkBaseColor == LinkedGradient.Main)
                {
                    materialPropertyBlock.SetColor(colorPropertyID, currentMainGradient.Evaluate(value));
                }
                else if (linkBaseColor == LinkedGradient.Secondary)
                {
                    materialPropertyBlock.SetColor(colorPropertyID, currentSecondaryGradient.Evaluate(value));
                }
                if (linkEmissionColor == LinkedGradient.Main)
                {
                    materialPropertyBlock.SetColor(emissionPropertyID, currentMainGradient.Evaluate(value));
                }    
                else if (linkEmissionColor == LinkedGradient.Secondary)
                {
                    materialPropertyBlock.SetColor(emissionPropertyID, currentSecondaryGradient.Evaluate(value));
                }
                meshRenderer.SetPropertyBlock(materialPropertyBlock);
            }
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
            SetIntensity(0);
#if ProjectCore
            EffectInstance orgEffectInstance = effectInstance;
            effectInstance = null;
            //EffectModuleDecal.Despawn(this);
            orgEffectInstance.OnEffectDespawn();
#endif
        }
    }
}
