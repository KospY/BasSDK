using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ThunderRoad.Reveal
{
    [System.Serializable]
    public class RevealData
    {
        [Tooltip("Blending Operation that the shader will perform.")]
        public BlendOp blendOp = BlendOp.Add;
        [Tooltip("Color channel masking for which channels to apply the reveal on.")]
        public ColorWriteMask colorMask = ColorWriteMask.All;
        [Tooltip("Time delay to begin this blit.")]
        public float delay = 0f;

        [Tooltip("How much time to apply this projection over. Zero for instantaneous.")]
        public float overTime = 0f;
        [Tooltip("Delta time multipler to way longer between delta time updates. Use for longer times or very low color levels.")]
        public float deltaMultiplier = 1f;

        //[Header("RenderTexture Settings (Only applicable if Over Time.)")]
        public float widthMultiplier = 1f;
        public float heightMultiplier = 1f;
        [Tooltip("RenderTextureFormat to use for the temporary overTime mask.")]
        public RenderTextureFormat overTimeRenderTextureFormat = RenderTextureFormat.ARGB32;
        public bool generateMipMaps = false;
        [Tooltip("Whether to swap the Alpha and Red data in the overTime mask for uses like using a single Red channel format.")]
        public bool swizzleAlphaRed = false;
        [Tooltip("ColorMask used for writing to the temporary overTime mask. When using Swizzle Alpha and Red, a common usuage would be TextureFormat: R8 and ColorMask: Red")]
        public ColorWriteMask overTimeSwizzleColorMask = ColorWriteMask.Red;
    }
}
