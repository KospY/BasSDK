using UnityEngine;
using System;

#if PrivateSDK
using RainyReignGames.RevealMask;
using System.Collections.Generic;
#endif

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [AddComponentMenu("ThunderRoad/Reveal Decal")]
    public class RevealDecal : MonoBehaviour
    {
        [Tooltip("These materials are what will be switched to on the renderer once the reveal masks are activated. Corresponds with shared materials index.")]
        public Material[] materials;
        [Tooltip("Resolution of the reveal mask")]
        public RevealMaskResolution maskWidth = RevealMaskResolution.Size_512;
        [Tooltip("Resolution of the reveal mask")]
        public RevealMaskResolution maskHeight = RevealMaskResolution.Size_512;
        [Tooltip("Reveal type")]
        public Type type = Type.Default;
        [Tooltip("Transfert maps to reveal")]
        public TransferedMaps transferedMaps;

        public enum Type
        {
            Default,
            Body,
            Outfit,
        }

        [Flags]
        public enum TransferedMaps
        {
            BaseMap = 1,
            BumpMap = 2,
            ColorMask = 4,
            EmissionMap = 8,
            SSSAOMap = 16,
            MOES = 32
        }

        public enum RevealMaskResolution
        {
            Size_32 = 32,
            Size_64 = 64,
            Size_128 = 128,
            Size_256 = 256,
            Size_512 = 512,
            Size_1024 = 1024,
            Size_2048 = 2048,
            Size_4096 = 4096,
        }

        [Button()]
        public void SetMaskResolutionFull()
        {
            SetMaskResolution(1f);
        }

        [Button()]
        public void SetMaskResolutionHalf()
        {
            SetMaskResolution(0.5f);
        }

        [Button()]
        public void SetMaskResolutionQuarter()
        {
            SetMaskResolution(0.25f);
        }

        [Button()]
        public void SetMaskResolutionEighth()
        {
            SetMaskResolution(0.125f);
        }

        public void SetMaskResolution(float scale = 1)
        {
            Renderer renderer = this.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = renderer.sharedMaterial;
                if (mat != null)
                {
                    Texture baseMap = mat.GetTexture("_BaseMap");
                    if (baseMap != null)
                    {
                        maskWidth = (RevealMaskResolution)Mathf.ClosestPowerOfTwo((int)(baseMap.width * scale));
                        maskHeight = (RevealMaskResolution)Mathf.ClosestPowerOfTwo((int)(baseMap.height * scale));
                        if (maskWidth != maskHeight) Debug.Log(this.gameObject.name);
                    }
                }
            }
        }

#if PrivateSDK
        [NonSerialized]
        public RevealMaterialController revealMaterialController;

        void Awake()
        {
            revealMaterialController = this.gameObject.AddComponent<RevealMaterialController>();
            revealMaterialController.revealMaterials = materials;
            revealMaterialController.width = (int)maskWidth;
            revealMaterialController.height = (int)maskHeight;
            revealMaterialController.maskPropertyName = "_RevealMask";
            revealMaterialController.preserveRenderQueue = true;
            revealMaterialController.renderTextureFormat = RenderTextureFormat.ARGB64;

            List<RevealMaterialController.ShaderProperty> shaderProperties = new List<RevealMaterialController.ShaderProperty>();

            foreach (Material material in revealMaterialController.revealMaterials)
            {
                if (!material) continue;

                // Int
                if (material.HasProperty("_Bitmask") && !shaderProperties.Exists(s => s.name == "_Bitmask"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_Bitmask";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Int;
                    shaderProperties.Add(shaderProperty);
                }
                //Float
                if (material.HasProperty("_Smoothness") && !shaderProperties.Exists(s => s.name == "_Smoothness"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_Smoothness";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Float;
                    shaderProperties.Add(shaderProperty);
                }
                if (material.HasProperty("_OcclusionStrength") && !shaderProperties.Exists(s => s.name == "_OcclusionStrength"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_OcclusionStrength";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Float;
                    shaderProperties.Add(shaderProperty);
                }

                // Colors
                if (material.HasProperty("_BaseColor") && !shaderProperties.Exists(s=> s.name == "_BaseColor"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_BaseColor";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Color;
                    shaderProperties.Add(shaderProperty);
                }
                if (material.HasProperty("_SecondaryColor") && !shaderProperties.Exists(s => s.name == "_SecondaryColor"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_SecondaryColor";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Color;
                    shaderProperties.Add(shaderProperty);
                }
                if (material.HasProperty("_SpecColor") && !shaderProperties.Exists(s => s.name == "_SpecColor"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_SpecColor";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Color;
                    shaderProperties.Add(shaderProperty);
                }
                if (material.HasProperty("_Tint0") && !shaderProperties.Exists(s => s.name == "_Tint0"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_Tint0";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Color;
                    shaderProperties.Add(shaderProperty);
                }
                if (material.HasProperty("_Tint1") && !shaderProperties.Exists(s => s.name == "_Tint1"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_Tint1";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Color;
                    shaderProperties.Add(shaderProperty);
                }
                if (material.HasProperty("_Tint2") && !shaderProperties.Exists(s => s.name == "_Tint2"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_Tint2";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Color;
                    shaderProperties.Add(shaderProperty);
                }
                // Textures
                if (transferedMaps.HasFlag(TransferedMaps.BaseMap) && material.HasProperty("_BaseMap") && !shaderProperties.Exists(s => s.name == "_BaseMap"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_BaseMap";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Texture;
                    shaderProperties.Add(shaderProperty);
                }
                if (transferedMaps.HasFlag(TransferedMaps.BumpMap) && material.HasProperty("_BumpMap") && !shaderProperties.Exists(s => s.name == "_BumpMap"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_BumpMap";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Texture;
                    shaderProperties.Add(shaderProperty);
                }
                if (transferedMaps.HasFlag(TransferedMaps.ColorMask) && material.HasProperty("_ColorMask") && !shaderProperties.Exists(s => s.name == "_ColorMask"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_ColorMask";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Texture;
                    shaderProperties.Add(shaderProperty);
                }
                if (transferedMaps.HasFlag(TransferedMaps.EmissionMap) && material.HasProperty("_EmissionMap") && !shaderProperties.Exists(s => s.name == "_EmissionMap"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_EmissionMap";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Texture;
                    shaderProperties.Add(shaderProperty);
                }
                if (transferedMaps.HasFlag(TransferedMaps.SSSAOMap) && material.HasProperty("_SSSAOMap") && !shaderProperties.Exists(s => s.name == "_SSSAOMap"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_SSSAOMap";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Texture;
                    shaderProperties.Add(shaderProperty);
                }
                if (transferedMaps.HasFlag(TransferedMaps.MOES) && material.HasProperty("_MetallicGlossMap") && !shaderProperties.Exists(s => s.name == "_MetallicGlossMap"))
                {
                    RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty();
                    shaderProperty.name = "_MetallicGlossMap";
                    shaderProperty.type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Texture;
                    shaderProperties.Add(shaderProperty);
                }
            }
            revealMaterialController.propertiesToPreserve = shaderProperties.ToArray();
        }


#endif
    }
}