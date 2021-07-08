using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SocialPlatforms;

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

        private float aliveTime = 0.0f;

        [Header("Color Gradient")]
        public EffectTarget linkBaseColor = EffectTarget.None;
        public EffectTarget linkTintColor = EffectTarget.None;
        public EffectTarget linkEmissionColor = EffectTarget.None;

        public Vector3 meshSize = Vector3.one;
        public float meshSizeFadeDuration;
        protected bool meshSizeFading;
        protected Coroutine meshSizeFadeCoroutine;

        public Vector3 meshRotation;
        public float meshRotationFadeDuration;
        protected bool meshRotationFading;
        protected Coroutine meshRotationFadeCoroutine;

        [Header("Intensity to mesh size")]
        public bool meshSizeFromIntensity;
        public AnimationCurve curveMeshSize;

        [Header("Intensity to mesh rotation Y")]
        public bool meshRotationFromIntensity;
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

        private IEnumerator MeshSizeFadeCoroutine(bool fadeIn = true)
        {
            meshSizeFading = true;
            float meshSizeValue;
            float time = 0;
            while (time < meshSizeFadeDuration)
            {
                if (meshSizeFromIntensity)
                {
                    meshSizeValue = curveMeshSize.Evaluate(currentValue);
                    if (fadeIn) transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(meshSizeValue, meshSizeValue, meshSizeValue), time / meshSizeFadeDuration);
                    else transform.localScale = Vector3.Lerp(new Vector3(meshSizeValue, meshSizeValue, meshSizeValue), Vector3.zero, time / meshSizeFadeDuration);
                }
                else
                {
                    if (fadeIn) transform.localScale = Vector3.Lerp(Vector3.zero, meshSize, time / meshSizeFadeDuration);
                    else transform.localScale = Vector3.Lerp(meshSize, Vector3.zero, time / meshSizeFadeDuration);
                }
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            transform.localScale = meshSize;
            meshSizeFading = false;
            yield return true;
        }

        private IEnumerator MeshRotationFadeCoroutine(bool fadeIn = true)
        {
            meshRotationFading = true;
            float meshRotYValue;
            float time = 0;
            while (time < meshRotationFadeDuration)
            {
                if (meshRotationFromIntensity)
                {
                    meshRotYValue = curveMeshrotY.Evaluate(currentValue);
                    if (fadeIn) transform.localEulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(transform.localEulerAngles.x, meshRotYValue, transform.localEulerAngles.x), time / meshSizeFadeDuration);
                    transform.localEulerAngles = Vector3.Lerp(new Vector3(transform.localEulerAngles.x, meshRotYValue, transform.localEulerAngles.x), Vector3.zero, time / meshSizeFadeDuration);
                }
                else
                {
                    if (fadeIn) transform.localEulerAngles = Vector3.Lerp(Vector3.zero, meshRotation, time / meshSizeFadeDuration);
                    transform.localEulerAngles = Vector3.Lerp(meshRotation, Vector3.zero, time / meshSizeFadeDuration);
                }
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            meshRotationFading = false;
            yield return true;
        }

        private float GetLastTime(AnimationCurve animationCurve)
        {
            return (animationCurve.length == 0) ? 0 : animationCurve[animationCurve.length - 1].time;
        }

        private void Update()
        {
            aliveTime += Time.deltaTime;
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

            if (meshSizeFadeDuration > 0)
            {
                meshSizeFadeCoroutine = StartCoroutine(MeshSizeFadeCoroutine());
            }

            if (meshRotationFadeDuration > 0)
            {
                meshRotationFadeCoroutine = StartCoroutine(MeshRotationFadeCoroutine());
            }
        }

        public override void Stop()
        {
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
            aliveTime = 0.0f;
        }

        public override void End(bool loopOnly = false)
        {
            if (meshSizeFadeDuration > 0)
            {
                StopCoroutine(meshSizeFadeCoroutine);
                meshSizeFadeCoroutine = StartCoroutine(MeshSizeFadeCoroutine(false));
            }
            if (meshRotationFadeDuration > 0)
            {
                StopCoroutine(meshRotationFadeCoroutine);
                meshRotationFadeCoroutine = StartCoroutine(MeshRotationFadeCoroutine(false));
            }
            if (meshSizeFadeDuration > 0 || meshRotationFadeDuration > 0)
            {
                Invoke("Despawn", meshSizeFadeDuration);
            }
            else
            {
                if (meshRenderer != null) meshRenderer.enabled = false;
                Despawn();
            }
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

                if (meshSizeFromIntensity && !meshSizeFading)
                {
                    float meshSizeValue = curveMeshSize.Evaluate(currentValue);
                    transform.localScale = new Vector3(meshSizeValue, meshSizeValue, meshSizeValue);
                }

                if (meshRotationFromIntensity && !meshRotationFading)
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
            StopCoroutine(MeshSizeFadeCoroutine());
            StopCoroutine(MeshRotationFadeCoroutine());
            meshSizeFading = false;
            meshRotationFading = false;
            CancelInvoke();
            if (meshRenderer != null) meshRenderer.enabled = false;
            InvokeDespawnCallback();
            if (Application.isPlaying)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
            aliveTime = 0.0f;
        }
    }
}
