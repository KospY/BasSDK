using UnityEngine;
using System;

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
        public RevealMaskResolution maskResolution = RevealMaskResolution.Size_512;
        [Tooltip("Reveal type")]
        public Type type = Type.Default;

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

#if PrivateSDK
        [NonSerialized]
        public RevealMaterialController revealMaterialController;

        void Awake()
        {
            revealMaterialController = this.gameObject.AddComponent<RevealMaterialController>();
            revealMaterialController.revealMaterials = materials;
            revealMaterialController.width = (int)maskResolution;
            revealMaterialController.height = (int)maskResolution;
            revealMaterialController.maskPropertyName = "_RevealMask";
            revealMaterialController.preserveRenderQueue = true;
            revealMaterialController.renderTextureFormat = RenderTextureFormat.ARGB64;

            foreach (Material material in revealMaterialController.revealMaterials)
            {
                if (material.HasProperty("_Bitmask"))
                {
                    revealMaterialController.propertiesToPreserve = new RevealMaterialController.ShaderProperty[1];
                    revealMaterialController.propertiesToPreserve[0].name = "_Bitmask";
                    revealMaterialController.propertiesToPreserve[0].type = RevealMaterialController.ShaderProperty.ShaderPropertyType.Int;
                    break;
                }
            }
        }

        public void Reset()
        {
            revealMaterialController.Reset();
        }
#endif
    }
}