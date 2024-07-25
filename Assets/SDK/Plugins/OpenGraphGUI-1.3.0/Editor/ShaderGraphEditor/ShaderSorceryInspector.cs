//#define SGMARKDOWN

#if UNITY_2021_2_OR_NEWER
#define HAVE_VALIDATE_MATERIAL
#endif

using UnityEditor;
using UnityEngine;

#if SGMARKDOWN

public class ShaderSorceryInspector : Needle.MarkdownShaderGUI
{
    protected new virtual void MaterialChanged(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        Debug.Log("ShaderSorceryInspector: Caught: MaterialChanged");

        foreach (var mat in materialEditor.targets)
        {
            Material material = (Material) mat;
            ShadowoodMaterialValidation(material);
        }

        base.MaterialChanged(materialEditor, properties);
    }

    // Shadowood: Unity changed it from "MaterialChanged" to "ValidateMaterial"
#if HAVE_VALIDATE_MATERIAL
    public override void ValidateMaterial(Material material)
    {
#else
    public void ValidateMaterial(Material material)
    {

#endif
        base.ValidateMaterial(material);
        ShadowoodMaterialValidation(material);
    }

    private void ShadowoodMaterialValidation(Material material)
    {
        // Shadowood: handled transparent/opaque dropdowns and blend modes

        BaseShaderGUI.SurfaceType surfaceType = BaseShaderGUI.SurfaceType.Opaque;
        if (material.HasFloat("_Surface")) surfaceType = (BaseShaderGUI.SurfaceType) material.GetFloat("_Surface"); // UnityEditor.Rendering.Universal.Property.SurfaceType

        float alphaClip = 0;
        if (material.HasFloat("_AlphaClip")) alphaClip = material.GetFloat("_AlphaClip");


        //if (alphaClip == 0) material.SetFloat("_AlphaClipThreshold",0);
        if (material.HasFloat("_Surface"))
        {
            BaseShaderGUI.SetMaterialKeywords(material); // Also calls SetupMaterialBlendMode()
        }

        if (surfaceType == BaseShaderGUI.SurfaceType.Transparent || alphaClip == 1)
        {
            material.EnableKeyword("_ALPHATEST_ON");
        }
        else
        {
            material.DisableKeyword("_ALPHATEST_ON");
        }

        //if (surfaceType == BaseShaderGUI.SurfaceType.Opaque)
        //{
        //    var blendOpaque = material.GetFloat("_BlendOpaque");
        //    //BaseShaderGUI.SetupMaterialBlendMode(material);
        //}
    }
}
#else
public class ShaderSorceryInspector : RobProductions.OpenGraphGUI.Editor.OpenGraphGUIEditor{}
#endif
