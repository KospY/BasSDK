using UnityEngine;
using System;

namespace ThunderRoad
{
    public class EffectMesh : Effect
    {
        public int poolCount = 20;
        public float lifeTime = 5;
        public float refreshSpeed = 0.1f;

        public AnimationCurve intensityCurve = new AnimationCurve(new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1));

        [NonSerialized]
        public float playTime;

        [Header("Color Gradient")]
        public EffectTarget linkBaseColor = EffectTarget.None;
        public EffectTarget linkTintColor = EffectTarget.None;
        public EffectTarget linkEmissionColor = EffectTarget.None;

        [Header("Intensity to mesh size")]
        public bool meshSize;
        public AnimationCurve curveMeshSize;

        [Header("Intensity to mesh rotation Y")]
        public bool meshRotY;
        public AnimationCurve curveMeshrotY;

        [NonSerialized]
        public float currentValue;

        [NonSerialized, GradientUsage(true)]
        public Gradient currentMainGradient;
        [NonSerialized, GradientUsage(true)]
        public Gradient currentSecondaryGradient;

        [NonSerialized]
        public MaterialPropertyBlock materialPropertyBlock;
        [NonSerialized]
        public MeshFilter meshFilter;
        [NonSerialized]
        public MeshRenderer meshRenderer;

        private void OnValidate()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Awake()
        {
            materialPropertyBlock = new MaterialPropertyBlock();

            meshFilter = this.GetComponent<MeshFilter>();
            if (!meshFilter) meshFilter = this.gameObject.AddComponent<MeshFilter>();

            meshRenderer = this.GetComponent<MeshRenderer>();
            if (!meshRenderer) meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
            meshRenderer.enabled = false;
        }

        public override void Play()
        {
            CancelInvoke();
            playTime = Time.time;
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }

            if (linkEmissionColor != EffectTarget.None && meshRenderer != null)
            {
                foreach (Material material in meshRenderer.materials)
                {
                    material.EnableKeyword("_EMISSION");
                }
            }

            if ((step == Step.Start || step == Step.End) && lifeTime > 0)
            {
                InvokeRepeating("UpdateLifeTime", 0, refreshSpeed);
            }
        }

        public override void Stop()
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }

        public override void End(bool loopOnly = false)
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
            Despawn();
        }

        protected void UpdateLifeTime()
        {
            float value = Mathf.Clamp01(1 - ((Time.time - playTime) / lifeTime));
            SetIntensity(value);
            if (value == 0) Despawn();
        }

        public override void SetIntensity(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                currentValue = intensityCurve.Evaluate(value);

                if (meshSize)
                {
                    float meshSizeValue = curveMeshSize.Evaluate(currentValue);
                    transform.localScale = new Vector3(meshSizeValue, meshSizeValue, meshSizeValue);
                }

                if (meshRotY)
                {
                    float meshRotYValue = curveMeshrotY.Evaluate(currentValue);
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, meshRotYValue, transform.localEulerAngles.z);
                }

                // Set material color
                bool updatePropertyBlock = false;
                if (linkTintColor == EffectTarget.Main && currentMainGradient != null)
                {
                    materialPropertyBlock.SetColor("_TintColor", currentMainGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }
                else if (linkTintColor == EffectTarget.Secondary && currentSecondaryGradient != null)
                {
                    materialPropertyBlock.SetColor("_TintColor", currentSecondaryGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }
                else if (linkBaseColor == EffectTarget.Main && currentMainGradient != null)
                {
                    materialPropertyBlock.SetColor("_BaseColor", currentMainGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }
                else if (linkBaseColor == EffectTarget.Secondary && currentSecondaryGradient != null)
                {
                    materialPropertyBlock.SetColor("_BaseColor", currentSecondaryGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }
                if (linkEmissionColor == EffectTarget.Main && currentMainGradient != null)
                {
                    materialPropertyBlock.SetColor("_EmissionColor", currentMainGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }
                else if (linkEmissionColor == EffectTarget.Secondary && currentSecondaryGradient != null)
                {
                    materialPropertyBlock.SetColor("_EmissionColor", currentSecondaryGradient.Evaluate(currentValue));
                    updatePropertyBlock = true;
                }

                if (meshRenderer != null && updatePropertyBlock) meshRenderer.SetPropertyBlock(materialPropertyBlock);
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
            if (meshRenderer != null) meshRenderer.enabled = false;
#if ProjectCore
            if (Application.isPlaying)
            {
                EffectModuleMesh.Despawn(this);
                InvokeDespawnCallback();
            }
#endif
        }
    }
}
