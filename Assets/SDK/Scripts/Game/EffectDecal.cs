using UnityEngine;
using System;

namespace ThunderRoad
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

        public float baseLifeTime = 60;
        public float emissionLifeTime = 10;
        public float fadeRefreshSpeed = 0.1f;

        [NonSerialized]
        public float playTime;

        [Header("Size")]
        public Vector3 size = new Vector3(1, 1, 1);
        public bool useSizeCurve;
        public AnimationCurve sizeCurve;

        [Header("Gradient")]
        public EffectTarget linkBaseColor = EffectTarget.None;
        public EffectTarget linkEmissionColor = EffectTarget.None;
        [GradientUsage(true)]
        public Gradient baseColorGradient;
        [GradientUsage(true)]
        public Gradient emissionColorGradient;

        private void OnValidate()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            meshRenderer = this.GetComponentInChildren<MeshRenderer>();
            if (colorPropertyID == 0) colorPropertyID = Shader.PropertyToID("_Color");
            if (emissionPropertyID == 0) emissionPropertyID = Shader.PropertyToID("_EmissionColor");
        }

        private void Awake()
        {
            materialPropertyBlock = new MaterialPropertyBlock();

            MeshFilter meshFilter = this.GetComponentInChildren<MeshFilter>();
            if (!meshFilter)
            {
                if (!defaultCubeMesh)
                {
                    MeshFilter primitiveMeshFilter = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<MeshFilter>();
                    defaultCubeMesh = primitiveMeshFilter.sharedMesh;
                    Destroy(primitiveMeshFilter.gameObject);
                }
                Transform meshTransform = this.transform.Find("Mesh");
                if (!meshTransform)
                {
                    meshTransform = new GameObject("Mesh").transform;
                    meshTransform.SetParent(this.transform);
                    meshTransform.localPosition = Vector3.zero;
                    meshTransform.rotation = Quaternion.LookRotation(this.transform.up, this.transform.forward);
                    meshTransform.localScale = Vector3.one;
                }
                meshFilter = meshTransform.gameObject.AddComponent<MeshFilter>();
                meshFilter.mesh = defaultCubeMesh;
            }

            meshRenderer = this.GetComponentInChildren<MeshRenderer>();
            if (!meshRenderer) meshRenderer = meshFilter.gameObject.AddComponent<MeshRenderer>();

            if (colorPropertyID == 0) colorPropertyID = Shader.PropertyToID("_Color");
            if (emissionPropertyID == 0) emissionPropertyID = Shader.PropertyToID("_EmissionColor");

            meshRenderer.enabled = false;
        }

#if ProjectCore
        public override void SetCollisionHandler(CollisionHandler collisionHandler)
        {
            if (collisionHandler)
            {
                if (collisionHandler.item)
                {
                    if (!(module as EffectModuleDecal).allowItem || !collisionHandler.item.data.allowDecals)
                    {
                        Despawn();
                        return;
                    }
                    meshRenderer.material.SetInt("_StencilRef", collisionHandler.item.stencilReference);
                }
                else if (collisionHandler.ragdollPart && !collisionHandler.ragdollPart.ragdoll.creature.body.player)
                {
                    if (!(module as EffectModuleDecal).allowRagdollPart)
                    {
                        Despawn();
                        return;
                    }
                    meshRenderer.material.SetInt("_StencilRef", collisionHandler.ragdollPart.ragdoll.creature.stencilReference);
                }
                else
                {
                    meshRenderer.material.SetInt("_StencilRef", 0);
                }
            }
            else
            {
                meshRenderer.material.SetInt("_StencilRef", 0);
            }

        }
#endif
        public override void Play()
        {
            playTime = Time.time;
            CancelInvoke();
            meshRenderer.transform.localScale = Vector3.one;
            meshRenderer.transform.localScale = new Vector3(size.x / meshRenderer.transform.lossyScale.x, size.y / meshRenderer.transform.lossyScale.y, size.z / meshRenderer.transform.lossyScale.z);
            if (step == Step.Start || step == Step.End)
            {
                InvokeRepeating("UpdateLifeTime", 0, fadeRefreshSpeed);
            }
            meshRenderer.enabled = true;
        }

        public override void Stop()
        {
            meshRenderer.enabled = false;
        }

        public override void End(bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                Despawn();
            }
        }

        protected void UpdateLifeTime()
        {
            float baseValue = Mathf.Clamp01(1 - ((Time.time - playTime) / baseLifeTime));
            if (meshRenderer.isVisible)
            {
                if (linkBaseColor != EffectTarget.None)
                {
                    materialPropertyBlock.SetColor(colorPropertyID, baseColorGradient.Evaluate(baseValue));
                }
                if (linkEmissionColor != EffectTarget.None)
                {
                    float emissionValue = Mathf.Clamp01(1 - ((Time.time - playTime) / emissionLifeTime));
                    materialPropertyBlock.SetColor(emissionPropertyID, emissionColorGradient.Evaluate(emissionValue));
                }
                if (linkBaseColor != EffectTarget.None || linkEmissionColor != EffectTarget.None)
                {
                    meshRenderer.SetPropertyBlock(materialPropertyBlock);
                }
            }
            if (baseValue == 0) Despawn();
        }

        public override void SetMainGradient(Gradient gradient)
        {
            if (linkBaseColor == EffectTarget.Main)
            {
                baseColorGradient = gradient;
            }
            if (linkEmissionColor == EffectTarget.Main)
            {
                emissionColorGradient = gradient;
            }
        }

        public override void SetSecondaryGradient(Gradient gradient)
        {
            if (linkBaseColor == EffectTarget.Secondary)
            {
                baseColorGradient = gradient;
            }
            if (linkEmissionColor == EffectTarget.Secondary)
            {
                emissionColorGradient = gradient;
            }
        }

        public override void SetIntensity(float value, bool loopOnly = false)
        {
            if (!loopOnly || (loopOnly && step == Step.Loop))
            {
                if (linkBaseColor != EffectTarget.None)
                {
                    materialPropertyBlock.SetColor(colorPropertyID, baseColorGradient.Evaluate(value));
                }
                if (linkEmissionColor != EffectTarget.None)
                {
                    materialPropertyBlock.SetColor(emissionPropertyID, emissionColorGradient.Evaluate(value));
                }
                if (linkBaseColor != EffectTarget.None || linkEmissionColor != EffectTarget.None)
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

        public override void CollisionStay(Vector3 position, Quaternion rotation, float intensity)
        {
            // Prevent decal to move when rubbing
        }

        public override void Despawn()
        {
            CancelInvoke();
            meshRenderer.enabled = false;
#if ProjectCore
            if (Application.isPlaying)
            {
                EffectModuleDecal.Despawn(this);
                InvokeDespawnCallback();
            }
#endif
        }
    }
}
