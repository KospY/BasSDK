using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ThunderRoadLitCutoffEditor : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        materialEditor.SetDefaultGUIWidths();

        MaterialProperty BaseMap = ShaderGUI.FindProperty("_BaseMap", properties);
        MaterialProperty BaseColor = ShaderGUI.FindProperty("_BaseColor", properties);
        MaterialProperty NormalMap = ShaderGUI.FindProperty("_BumpMap", properties);
        MaterialProperty MOES = ShaderGUI.FindProperty("_MetallicGlossMap", properties);
        MaterialProperty Smoothness = ShaderGUI.FindProperty("_Smoothness", properties);
        MaterialProperty OcclusionStr = ShaderGUI.FindProperty("_OcclusionStrength", properties);
        MaterialProperty Cutoff = ShaderGUI.FindProperty("_Cutoff", properties);
        MaterialProperty UseColorMask = ShaderGUI.FindProperty("_COLORMASK", properties);
        MaterialProperty ColorMask = ShaderGUI.FindProperty("_ColorMask", properties);

        /*MaterialProperty UseDetailMap = ShaderGUI.FindProperty("_DETAILMAP", properties);
        MaterialProperty DetailAlbedoMap = ShaderGUI.FindProperty("_DetailAlbedoMap", properties);
        MaterialProperty DetailAlbedoMapScale = ShaderGUI.FindProperty("_DetailAlbedoMapScale", properties);
        MaterialProperty DetailNormalMap = ShaderGUI.FindProperty("_DetailNormalMap", properties);
        MaterialProperty DetailNormalMapScale = ShaderGUI.FindProperty("_DetailNormalMapScale", properties);*/

        MaterialProperty EmissionColor = ShaderGUI.FindProperty("_EmissionColor", properties);
        MaterialProperty EmissionMap = ShaderGUI.FindProperty("_EmissionMap", properties);
        MaterialProperty UseEmissionMap = ShaderGUI.FindProperty("_EMISSIONMAP", properties);

        MaterialProperty UseVertexOcclusion = ShaderGUI.FindProperty("_VERTEXOCCLUSION", properties);
        MaterialProperty VertexOcclusionBitmask = ShaderGUI.FindProperty("_Bitmask", properties);

        MaterialProperty UseProbeVolume = ShaderGUI.FindProperty("_PROBEVOLUME", properties);

        EditorGUILayout.BeginVertical("HelpBox");
        materialEditor.DefaultShaderProperty(BaseMap, BaseMap.displayName);
        materialEditor.DefaultShaderProperty(BaseColor, BaseColor.displayName);
        materialEditor.DefaultShaderProperty(NormalMap, NormalMap.displayName);
        materialEditor.DefaultShaderProperty(MOES, MOES.displayName);
        materialEditor.DefaultShaderProperty(Smoothness, Smoothness.displayName);
        materialEditor.DefaultShaderProperty(OcclusionStr, OcclusionStr.displayName);
        materialEditor.DefaultShaderProperty(Cutoff, Cutoff.displayName);

        float lastLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth * 0.4f;
        materialEditor.TextureScaleOffsetProperty(BaseMap);
        EditorGUIUtility.labelWidth = lastLabelWidth;
        EditorGUILayout.EndVertical();

        /*EditorGUILayout.BeginVertical("HelpBox");
        materialEditor.ShaderProperty(UseDetailMap, UseDetailMap.displayName);
        if(UseDetailMap.floatValue > 0)
        {
            materialEditor.ShaderProperty(DetailAlbedoMap, DetailAlbedoMap.displayName);
            materialEditor.ShaderProperty(DetailAlbedoMapScale, DetailAlbedoMapScale.displayName);
            materialEditor.ShaderProperty(DetailNormalMap, DetailNormalMap.displayName);
            materialEditor.ShaderProperty(DetailNormalMapScale, DetailNormalMapScale.displayName);

            lastLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth * 0.4f;
            materialEditor.TextureScaleOffsetProperty(DetailAlbedoMap);
            EditorGUIUtility.labelWidth = lastLabelWidth;
        }
        EditorGUILayout.EndVertical();*/

        EditorGUILayout.BeginVertical("HelpBox");
        materialEditor.ShaderProperty(UseColorMask, UseColorMask.displayName);
        if(UseColorMask.floatValue > 0)
        {
            MaterialProperty Tint0 = ShaderGUI.FindProperty("_Tint0", properties);
            MaterialProperty Tint1 = ShaderGUI.FindProperty("_Tint1", properties);
            MaterialProperty Tint2 = ShaderGUI.FindProperty("_Tint2", properties);

            materialEditor.DefaultShaderProperty(ColorMask, ColorMask.displayName);
            materialEditor.DefaultShaderProperty(Tint0, Tint0.displayName);
            materialEditor.DefaultShaderProperty(Tint1, Tint1.displayName);
            materialEditor.DefaultShaderProperty(Tint2, Tint2.displayName);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("HelpBox");
        materialEditor.DefaultShaderProperty(EmissionColor, EmissionColor.displayName);
        materialEditor.ShaderProperty(UseEmissionMap, UseEmissionMap.displayName);
        if(UseEmissionMap.floatValue > 0)
        {
            materialEditor.DefaultShaderProperty(EmissionMap, EmissionMap.displayName);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("HelpBox");
        materialEditor.ShaderProperty(UseVertexOcclusion, UseVertexOcclusion.displayName);
        if (UseVertexOcclusion.floatValue > 0)
        {
            materialEditor.DefaultShaderProperty(VertexOcclusionBitmask, VertexOcclusionBitmask.displayName);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("HelpBox");
        materialEditor.ShaderProperty(UseProbeVolume, UseProbeVolume.displayName);
        if (UseProbeVolume.floatValue > 0 && Application.isPlaying)
        {
            MaterialProperty ProbeVolumeSHAr = ShaderGUI.FindProperty("_ProbeVolumeShR", properties);
            MaterialProperty ProbeVolumeSHAg = ShaderGUI.FindProperty("_ProbeVolumeShG", properties);
            MaterialProperty ProbeVolumeSHAb = ShaderGUI.FindProperty("_ProbeVolumeShB", properties);
            MaterialProperty ProbeVolumeOcc = ShaderGUI.FindProperty("_ProbeVolumeOcc", properties);

            materialEditor.DefaultShaderProperty(ProbeVolumeSHAr, ProbeVolumeSHAr.displayName);
            materialEditor.DefaultShaderProperty(ProbeVolumeSHAg, ProbeVolumeSHAg.displayName);
            materialEditor.DefaultShaderProperty(ProbeVolumeSHAb, ProbeVolumeSHAb.displayName);
            materialEditor.DefaultShaderProperty(ProbeVolumeOcc, ProbeVolumeOcc.displayName);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("HelpBox");
        materialEditor.RenderQueueField();
        materialEditor.EnableInstancingField();
        materialEditor.DoubleSidedGIField();
        materialEditor.EmissionEnabledProperty();
        materialEditor.LightmapEmissionProperty();
        EditorGUILayout.EndVertical();
    }
}
