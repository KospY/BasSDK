using UnityEngine;
using System;
using System.Collections.Generic;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/Items/RevealDecal.html")]
    [AddComponentMenu("ThunderRoad/Reveal Decal")]
    public class RevealDecal : MonoBehaviour
    {
        [Tooltip("Resolution of the width of the reveal mask")]
        public RevealMaskResolution maskWidth = RevealMaskResolution.Size_512;
        [Tooltip("Resolution of the height of the reveal mask")]
        public RevealMaskResolution maskHeight = RevealMaskResolution.Size_512;
        [Tooltip("Specifies what type of reveal is used.\nDefault is for Items and Weapons.\nOutfit is for clothing/armor.\nBody is for NPC/Player.\n\nThe Body type will make it so blood/reveal is removed/fades when the Player/NPC drinks a potion, but only on the body, not on Outfit.")]
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


    }
}