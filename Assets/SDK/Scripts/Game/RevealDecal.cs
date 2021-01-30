using UnityEngine;


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
        public Material[] revealMaterials;
        [Tooltip("Resolution of the reveal mask")]
        public RevealMaskResolution revealMaskResolution = RevealMaskResolution.Size_512;

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

    }
}