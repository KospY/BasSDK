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

        BaseShaderGUI.SetMaterialKeywords(material);
    }
}
