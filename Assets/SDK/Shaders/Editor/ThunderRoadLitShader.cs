using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class ThunderRoadLitShader : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        EditorGUI.BeginChangeCheck();
        base.OnGUI(materialEditor, properties);
        if(EditorGUI.EndChangeCheck())
        {
            foreach (var obj in materialEditor.targets)
                MaterialChanged((Material)obj);
        }
    }

    private void MaterialChanged(Material material)
    {
        if (material == null)
            throw new ArgumentNullException("material");

        BaseShaderGUI.SetMaterialKeywords(material, SetMaterialKeywords);
    }

    public static void SetMaterialKeywords(Material material)
    {
        //Sadly, this makes the BetterHeader attributes unnecessary

        //TR/LIT is only setup for Metallic workflow, currently.
        CoreUtils.SetKeyword(material, "_METALLICSPECGLOSSMAP", true);

        if (material.HasProperty("_UseEmission"))
        {
            CoreUtils.SetKeyword(material, "_EMISSION", material.GetFloat("_UseEmission") > 0.5f);
        }

        if (material.HasProperty("_UseDetailMap"))
        {
            CoreUtils.SetKeyword(material, "_DETAIL", material.GetFloat("_UseDetailMap") > 0.5f);
        }

        if (material.HasProperty("_UseColorMask"))
        {
            CoreUtils.SetKeyword(material, "_COLORMASK_ON", material.GetFloat("_UseColorMask") > 0.5f);
        }

        if (material.HasProperty("_UseReveal"))
        {
            CoreUtils.SetKeyword(material, "_REVEALLAYERS", material.GetFloat("_UseReveal") > 0.5f);
        }

        if (material.HasProperty("_UseVertexOcclusion"))
        {
            CoreUtils.SetKeyword(material, "_VERTEXOCCLUSION_ON", material.GetFloat("_UseVertexOcclusion") > 0.5f);
        }

        if (material.HasProperty("_UseProbeVolume"))
        {
            CoreUtils.SetKeyword(material, "_PROBEVOLUME_ON", material.GetFloat("_UseProbeVolume") > 0.5f);
        }
    }
}
