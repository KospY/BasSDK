using UnityEngine;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [ExecuteInEditMode]
    public class EffectPaint : Effect
    {
        protected static int baseMapPropertyID;
        protected static int normalPropertyID;
        protected static int emissionPropertyID;

        public Material material;
#if ProjectCore
        public PaintIn3D.P3dBlendMode blendMode;
#endif
        public float radius = 0.1f;
        public bool useRadiusCurve;
        public AnimationCurve radiusCurve;
        public float depth = 1;
        public float opacity = 1;
        public float hardness = 1;
        public float wrapping = 1;
        public float normalFront = 1f;
        public float tileTransition = 4f;
        public float normalBack = 0;
        public float normalFade = 0.5f;

        [NonSerialized]
        public float playTime;

        [Header("Gradient")]
        public EffectTarget linkBaseColor = EffectTarget.None;
        public EffectTarget linkEmissionColor = EffectTarget.None;
        [GradientUsage(true)]
        public Gradient baseColorGradient;
        [GradientUsage(true)]
        public Gradient emissionColorGradient;

#if ProjectCore
        protected CollisionHandler collisionHandler;

        private void Awake()
        {
            if (baseMapPropertyID == 0) baseMapPropertyID = Shader.PropertyToID("_BaseMap");
            if (normalPropertyID == 0) normalPropertyID = Shader.PropertyToID("_BumpMap");
            if (emissionPropertyID == 0) emissionPropertyID = Shader.PropertyToID("_MaskMap");
        }

        public override void SetCollisionHandler(CollisionHandler collisionHandler)
        {

            this.collisionHandler = collisionHandler;
            if (collisionHandler)
            {
                if (collisionHandler.item)
                {
                    if (!(module as EffectModulePaint).allowItem)
                    {
                        Despawn();
                        return;
                    }
                }
                else if (collisionHandler.ragdollPart)
                {
                    if (!(module as EffectModulePaint).allowRagdollPart)
                    {
                        Despawn();
                        return;
                    }
                }
                else
                {
                    Despawn();
                    return;
                }
            }
            else
            {
                Despawn();
                return;
            }
        }

        [Button]
        public override void Play()
        {
            playTime = Time.time;
            CancelInvoke();

            if (collisionHandler && (collisionHandler.item || collisionHandler.ragdollPart))
            {
                Texture baseTexture = material.GetTexture(baseMapPropertyID);
                Texture normalTexture = material.GetTexture(normalPropertyID);
                Texture emissionTexture = material.GetTexture(emissionPropertyID);

                foreach (Paintable paintable in (collisionHandler.item ? collisionHandler.item.definition.paintables : collisionHandler.ragdollPart.ragdoll.creature.paintables))
                {
                    if (baseTexture)
                    {
                        PaintIn3D.P3dCommandDecal.Instance.SetState(false, 0);
                        PaintIn3D.P3dCommandDecal.Instance.SetLocation(this.transform.position);
                        PaintIn3D.P3dCommandDecal.Instance.SetShape(Quaternion.LookRotation(-this.transform.forward, this.transform.up), new Vector3(radius, radius, radius), 0);
                        PaintIn3D.P3dCommandDecal.Instance.SetMaterial(blendMode, baseTexture, null, PaintIn3D.P3dChannel.Alpha, hardness, wrapping, normalBack, normalFront, normalFade, baseColorGradient.Evaluate(1), opacity, null, Matrix4x4.identity, 1, tileTransition);
                        paintable.Paint(PaintIn3D.P3dCommandDecal.Instance, Paintable.MaterialProperty.PropertyType.Base);
                    }
                    if (normalTexture)
                    {
                        PaintIn3D.P3dCommandDecal.Instance.SetState(false, 0);
                        PaintIn3D.P3dCommandDecal.Instance.SetLocation(this.transform.position);
                        PaintIn3D.P3dCommandDecal.Instance.SetShape(Quaternion.LookRotation(-this.transform.forward, this.transform.up), new Vector3(radius, radius, radius), 0);
                        PaintIn3D.P3dCommandDecal.Instance.SetMaterial(blendMode, normalTexture, null, PaintIn3D.P3dChannel.Alpha, hardness, wrapping, normalBack, normalFront, normalFade, baseColorGradient.Evaluate(1), opacity, null, Matrix4x4.identity, 1, tileTransition);
                        paintable.Paint(PaintIn3D.P3dCommandDecal.Instance, Paintable.MaterialProperty.PropertyType.Normal);
                    }
                    if (emissionTexture)
                    {
                        PaintIn3D.P3dCommandDecal.Instance.SetState(false, 0);
                        PaintIn3D.P3dCommandDecal.Instance.SetLocation(this.transform.position);
                        PaintIn3D.P3dCommandDecal.Instance.SetShape(Quaternion.LookRotation(-this.transform.forward, this.transform.up), new Vector3(radius, radius, radius), 0);
                        PaintIn3D.P3dCommandDecal.Instance.SetMaterial(blendMode, emissionTexture, null, PaintIn3D.P3dChannel.Alpha, hardness, wrapping, normalBack, normalFront, normalFade, baseColorGradient.Evaluate(1), opacity, null, Matrix4x4.identity, 1, tileTransition);
                        paintable.Paint(PaintIn3D.P3dCommandDecal.Instance, Paintable.MaterialProperty.PropertyType.Emission);
                    }
                }
                if (step == Step.Start || step == Step.End)
                {
                    Invoke("Despawn", 2);
                }
            }
            else
            {
                Despawn();
            }
        }
#endif

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

        public override void CollisionStay(Vector3 position, Quaternion rotation, float intensity)
        {
            // Prevent decal to move when rubbing
        }

        public override void Despawn()
        {  
            CancelInvoke();
#if ProjectCore
            collisionHandler = null;
            if (Application.isPlaying)
            {
                EffectModulePaint.Despawn(this);
                InvokeDespawnCallback();
            }
#endif
        }
    }
}
