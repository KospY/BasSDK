using UnityEngine;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace BS
{
    [ExecuteInEditMode]
    public class EffectPaint : Effect
    {
        protected static int baseMapPropertyID;
        protected static int normalPropertyID;
        protected static int emissionPropertyID;

        public Material material;

        public MaterialData.PaintBlendMode blendMode;
        public float radius = 0.1f;
        public bool useRadiusCurve;
        public AnimationCurve radiusCurve;
        public float depth = 1;
        public float opacity = 1;
        public float hardness = 1;
        public float normalFront = 1f;
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
        protected PaintIn3D.P3dPaintDecal.Command paintBaseCommand = new PaintIn3D.P3dPaintDecal.Command();
        protected PaintIn3D.P3dPaintDecal.Command paintNormalCommand = new PaintIn3D.P3dPaintDecal.Command();
        protected PaintIn3D.P3dPaintDecal.Command paintEmissionCommand = new PaintIn3D.P3dPaintDecal.Command();
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
                        paintBaseCommand.SetLocation(this.transform.position, Quaternion.LookRotation(-this.transform.forward, this.transform.up), Vector2.one, radius, baseTexture, depth);
                        paintBaseCommand.SetMaterial((PaintIn3D.P3dBlendMode)(int)blendMode, baseTexture, hardness, normalBack, normalFront, normalFade, baseColorGradient.Evaluate(1), opacity + (1.0f - opacity), null);
                        paintable.Paint(paintBaseCommand, Paintable.MaterialProperty.PropertyType.Base);
                    }
                    if (normalTexture)
                    {
                        paintBaseCommand.SetLocation(this.transform.position, Quaternion.LookRotation(-this.transform.forward, this.transform.up), Vector2.one, radius, normalTexture, depth);
                        paintBaseCommand.SetMaterial((PaintIn3D.P3dBlendMode)(int)blendMode, normalTexture, hardness, normalBack, normalFront, normalFade, baseColorGradient.Evaluate(1), opacity + (1.0f - opacity), null);
                        paintable.Paint(paintBaseCommand, Paintable.MaterialProperty.PropertyType.Normal);
                    }
                    if (emissionTexture)
                    {
                        paintBaseCommand.SetLocation(this.transform.position, Quaternion.LookRotation(-this.transform.forward, this.transform.up), Vector2.one, radius, emissionTexture, depth);
                        paintBaseCommand.SetMaterial((PaintIn3D.P3dBlendMode)(int)blendMode, emissionTexture, hardness, normalBack, normalFront, normalFade, baseColorGradient.Evaluate(1), opacity + (1.0f - opacity), null);
                        paintable.Paint(paintBaseCommand, Paintable.MaterialProperty.PropertyType.Emission);
                    }
                }
                Invoke("Despawn", 2);
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
            if (Application.isPlaying)
            {
                EffectModulePaint.Despawn(this);
                InvokeDespawnCallback();
            }
#endif
        }
    }
}
