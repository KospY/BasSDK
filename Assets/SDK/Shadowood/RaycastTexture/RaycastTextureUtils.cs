using UnityEngine;

namespace Shadowood.RaycastTexture
{
    public static class RaycastTextureUtils
    {
        public static Texture2D ToTexture2D(this Texture rTex)
        {
            if (rTex == null) return null;
            if (rTex is Texture2D texA)
            {
                return texA;
            }

            if (rTex is RenderTexture texB)
            {
                return texB.ToTexture2D();
            }

            return null;
        }

        public static Texture2D ToTexture2D(this RenderTexture rTex, TextureFormat format = TextureFormat.RGBAFloat, bool linear = true)
        {
            //Debug.Log("Convert RT to Tex2D: " + rTex.name);
            Texture2D tex = new Texture2D(rTex.width, rTex.height, format, false, linear);
            tex.hideFlags = HideFlags.DontSave;
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0, false);
            tex.Apply();
            tex.name = rTex.name + " - Tex2D";
            return tex;
        }

        public static Texture2D ToTexture2D(this RenderTexture rTex, ref Texture2D tex, TextureFormat format = TextureFormat.RGBAFloat, bool linear = true)
        {
            //Debug.Log("Convert RT to Tex2D: " + rTex.name);
            if (tex == null || tex.width != rTex.width || tex.height != rTex.height || tex.format != format)
            {
                tex = new Texture2D(rTex.width, rTex.height, format, false, linear);
            }

            tex.hideFlags = HideFlags.DontSave;
            RenderTexture.active = rTex;

            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0, false);
            tex.Apply();
            tex.name = rTex.name + " - Tex2D";
            return tex;
        }
    }
}
