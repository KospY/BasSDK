using UnityEngine;
using System;

namespace BS
{
    [ExecuteInEditMode]
    public class EffectDecal : Effect
    {
        [NonSerialized]
        public MeshRenderer meshRenderer;

        protected static int colorPropertyID;
        protected static int emissionPropertyID;
        protected static Mesh defaultCubeMesh;

        protected MaterialPropertyBlock materialPropertyBlock;

        public float lifeTime = 60;
        public float fadeRefreshSpeed = 0.1f;

        [NonSerialized]
        public float playTime;

        [Header("Size")]
        public Vector3 size = new Vector3(1, 1, 1);
        public bool useSizeCurve;
        public AnimationCurve sizeCurve;

        [Header("Size")]
        public bool useBaseColorGradient;
        [GradientUsage(true)]
        public Gradient baseColorGradient;
        public bool useEmissionColorGradient;
        [GradientUsage(true)]
        public Gradient emissionColorGradient;

        private void OnValidate()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            meshRenderer = this.GetComponent<MeshRenderer>();
        }

        private void Awake()
        {
            materialPropertyBlock = new MaterialPropertyBlock();

            MeshFilter meshFilter = this.GetComponent<MeshFilter>();
            if (!meshFilter)
            {
                if (!defaultCubeMesh)
                {
                    MeshFilter primitiveMeshFilter = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshFilter>();
                    defaultCubeMesh = primitiveMeshFilter.sharedMesh;
                    Destroy(primitiveMeshFilter.gameObject);
                }
                meshFilter = this.gameObject.AddComponent<MeshFilter>();
                meshFilter.mesh = defaultCubeMesh;
            }

            meshRenderer = this.GetComponent<MeshRenderer>();
            if (!meshRenderer) meshRenderer = this.gameObject.AddComponent<MeshRenderer>();

            if (colorPropertyID == 0) colorPropertyID = Shader.PropertyToID("_Color");
            if (emissionPropertyID == 0) emissionPropertyID = Shader.PropertyToID("_EmissionColor");

            meshRenderer.enabled = false;
        }

        public override void Play()
        {
            playTime = Time.time;
            CancelInvoke();
            meshRenderer.transform.localScale = new Vector3(size.x / meshRenderer.transform.lossyScale.x, size.y / meshRenderer.transform.lossyScale.y, size.z / meshRenderer.transform.lossyScale.z);
            if (step != Step.Loop)
            {
                InvokeRepeating("UpdateLifeTime", 0, fadeRefreshSpeed);
            }
            meshRenderer.enabled = true;
        }

        public override void Stop()
        {
            Despawn();
        }

        protected void UpdateLifeTime()
        {
            float value = Mathf.Clamp01((Time.time - playTime) / lifeTime);
            SetIntensity(value);
            if (value == 1) Despawn();
        }

        public override void SetIntensity(float value)
        {
            if (meshRenderer.isVisible)
            {
                if (useBaseColorGradient)
                {
                    materialPropertyBlock.SetColor(colorPropertyID, baseColorGradient.Evaluate(value));
                }
                if (useEmissionColorGradient)
                {
                    materialPropertyBlock.SetColor(emissionPropertyID, emissionColorGradient.Evaluate(value));
                }
                if (useBaseColorGradient || useEmissionColorGradient)
                {
                    meshRenderer.SetPropertyBlock(materialPropertyBlock);
                }
                if (useSizeCurve)
                {
                    float eval = sizeCurve.Evaluate(value);
                    meshRenderer.transform.localScale = Vector3.one;
                    meshRenderer.transform.localScale = new Vector3((eval / meshRenderer.transform.lossyScale.x) * size.x, (eval / meshRenderer.transform.lossyScale.y) * size.y, (eval / meshRenderer.transform.lossyScale.z) * size.z);
                }
            }
        }

        public override void Despawn()
        {
            CancelInvoke();
            meshRenderer.enabled = false;
#if ProjectCore
            EffectInstance orgEffectInstance = effectInstance;
            effectInstance = null;
            EffectModuleDecal.Despawn(this);
            orgEffectInstance.OnEffectDespawn();
#endif
        }
    }
}
