using UnityEngine;
using System;
using System.Collections.Generic;

#if PrivateSDK
using RainyReignGames.RevealMask;
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

        public List<string> textureProperties;
        public List<string> colorProperties;
        public List<string> floatProperties;
        public List<string> vectorProperties;
        //public List<string> intProperties; //unused for now. ShaderUtils doesn't differentiate between float and int.

        public enum Type
        {
            Default,
            Body,
            Outfit,
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

            if (type == Type.Outfit || type == Type.Body)
            {
                if (!floatProperties.Contains("_Bitmask")) floatProperties.Add("_Bitmask");
            }


            List<RevealMaterialController.ShaderProperty> shaderProperties = new List<RevealMaterialController.ShaderProperty>();

            foreach (Material material in revealMaterialController.revealMaterials)
            {
                if (!material) continue;

                foreach (string textureProperty in textureProperties)
                {
                    TransferMaterialProperty(material, shaderProperties, textureProperty, RevealMaterialController.ShaderProperty.ShaderPropertyType.Texture);
                }

                foreach (string colorProperty in colorProperties)
                {
                    TransferMaterialProperty(material, shaderProperties, colorProperty, RevealMaterialController.ShaderProperty.ShaderPropertyType.Color);
                }

                foreach (string floatProperty in floatProperties)
                {
                    TransferMaterialProperty(material, shaderProperties, floatProperty, RevealMaterialController.ShaderProperty.ShaderPropertyType.Float);
                }

                foreach (string vectorProperty in vectorProperties)
                {
                    TransferMaterialProperty(material, shaderProperties, vectorProperty, RevealMaterialController.ShaderProperty.ShaderPropertyType.Vector);
                }

                /*foreach (string intProperty in intProperties)
                {
                    TransferMaterialProperty(material, shaderProperties, intProperty, RevealMaterialController.ShaderProperty.ShaderPropertyType.Int);
                }*/
            }
            revealMaterialController.propertiesToPreserve = shaderProperties.ToArray();
        }

        bool TransferMaterialProperty(Material material, List<RevealMaterialController.ShaderProperty> shaderProperties, string propertyName, RevealMaterialController.ShaderProperty.ShaderPropertyType shaderType)
        {
            if (material.HasProperty(propertyName) && !shaderProperties.Exists(s => s.name == propertyName))
            {
                RevealMaterialController.ShaderProperty shaderProperty = new RevealMaterialController.ShaderProperty
                {
                    name = propertyName,
                    type = shaderType
                };
                shaderProperties.Add(shaderProperty);
                return true;
            }
            return false;
        }
#endif

    }
}