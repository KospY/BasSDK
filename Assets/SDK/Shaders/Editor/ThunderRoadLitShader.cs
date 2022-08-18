using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class ThunderRoadLitShader : ShaderGUI
{
    GUIStyle header;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        if (header == null)
        {
            header = new GUIStyle(GUI.skin.box);
            header.fontStyle = FontStyle.Bold;
            header.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
        }

        EditorGUI.BeginChangeCheck();
        //=====================================================================
        //base.OnGUI(materialEditor, properties);
        materialEditor.SetDefaultGUIWidths();

        MaterialProperty surface = ShaderGUI.FindProperty("_Surface", properties);
        MaterialProperty blend = ShaderGUI.FindProperty("_Blend", properties);
        MaterialProperty cullMode = ShaderGUI.FindProperty("_CullMode", properties);
        MaterialProperty alphaClip = ShaderGUI.FindProperty("_AlphaClip", properties);
        MaterialProperty cutoff = ShaderGUI.FindProperty("_Cutoff", properties);
        MaterialProperty receiveShadows = ShaderGUI.FindProperty("_ReceiveShadows", properties);
        MaterialProperty baseMap = ShaderGUI.FindProperty("_BaseMap", properties);
        MaterialProperty baseColor = ShaderGUI.FindProperty("_BaseColor", properties);
        MaterialProperty bumpMap = ShaderGUI.FindProperty("_BumpMap", properties);
        MaterialProperty normalStrength = ShaderGUI.FindProperty("_NormalStrength", properties);
        MaterialProperty metallicMap = ShaderGUI.FindProperty("_MetallicGlossMap", properties);
        MaterialProperty smoothness = ShaderGUI.FindProperty("_Smoothness", properties);
        MaterialProperty occlusionStrength = ShaderGUI.FindProperty("_OcclusionStrength", properties);
        MaterialProperty emissionColor = ShaderGUI.FindProperty("_EmissionColor", properties);
        MaterialProperty useEmission = ShaderGUI.FindProperty("_UseEmission", properties);
        MaterialProperty emissionMap = ShaderGUI.FindProperty("_EmissionMap", properties);
        MaterialProperty useDetailMap = ShaderGUI.FindProperty("_UseDetailMap", properties);
        MaterialProperty detailAlbedoMap = ShaderGUI.FindProperty("_DetailAlbedoMap", properties);
        MaterialProperty detailAlbedoMapScale = ShaderGUI.FindProperty("_DetailAlbedoMapScale", properties);
        MaterialProperty detailNormalMap = ShaderGUI.FindProperty("_DetailNormalMap", properties);
        MaterialProperty detailNormalMapScale = ShaderGUI.FindProperty("_DetailNormalMapScale", properties);
        MaterialProperty detailWeightOverDistance = ShaderGUI.FindProperty("_DetailWeightOverDistance", properties);
        MaterialProperty queueOffest = ShaderGUI.FindProperty("_QueueOffset", properties);

        //ColorMask
        MaterialProperty useColorMask = ShaderGUI.FindProperty("_UseColorMask", properties);
        MaterialProperty colorMask = ShaderGUI.FindProperty("_ColorMask", properties);
        MaterialProperty tint0 = ShaderGUI.FindProperty("_Tint0", properties);
        MaterialProperty tint1 = ShaderGUI.FindProperty("_Tint1", properties);
        MaterialProperty tint2 = ShaderGUI.FindProperty("_Tint2", properties);
        MaterialProperty tint3 = ShaderGUI.FindProperty("_Tint3", properties);
        //Reveal
        MaterialProperty useReveal = ShaderGUI.FindProperty("_UseReveal", properties);
        MaterialProperty revealMask = ShaderGUI.FindProperty("_RevealMask", properties);
        MaterialProperty layerMask = ShaderGUI.FindProperty("_LayerMask", properties);
        MaterialProperty layerSurfaceExp = ShaderGUI.FindProperty("_LayerSurfaceExp", properties);
        MaterialProperty layer0 = ShaderGUI.FindProperty("_Layer0", properties);
        MaterialProperty layer0NormalMap = ShaderGUI.FindProperty("_Layer0NormalMap", properties);
        MaterialProperty layer0NormalStrength = ShaderGUI.FindProperty("_Layer0NormalStrength", properties);
        MaterialProperty layer0Smoothness = ShaderGUI.FindProperty("_Layer0Smoothness", properties);
        MaterialProperty layer0Metallic = ShaderGUI.FindProperty("_Layer0Metallic", properties);
        MaterialProperty layer1 = ShaderGUI.FindProperty("_Layer1", properties);
        MaterialProperty layer1NormalMap = ShaderGUI.FindProperty("_Layer1NormalMap", properties);
        MaterialProperty layer1NormalStrength = ShaderGUI.FindProperty("_Layer1NormalStrength", properties);
        MaterialProperty layer1Smoothness = ShaderGUI.FindProperty("_Layer1Smoothness", properties);
        MaterialProperty layer1Metallic = ShaderGUI.FindProperty("_Layer1Metallic", properties);
        MaterialProperty layer2Height = ShaderGUI.FindProperty("_Layer2Height", properties);
        MaterialProperty layer3EmissionColor = ShaderGUI.FindProperty("_Layer3EmissionColor", properties);
        //Vertex Occlusion
        MaterialProperty useVertexOcclusion = ShaderGUI.FindProperty("_UseVertexOcclusion", properties);
        MaterialProperty bitmask = ShaderGUI.FindProperty("_Bitmask", properties);
        //Probe Volume
        MaterialProperty useProbeVolume = ShaderGUI.FindProperty("_UseProbeVolume", properties);
        MaterialProperty probeVolumeSHAr = ShaderGUI.FindProperty("_ProbeVolumeShR", properties);
        MaterialProperty probeVolumeSHAg = ShaderGUI.FindProperty("_ProbeVolumeShG", properties);
        MaterialProperty probeVolumeSHAb = ShaderGUI.FindProperty("_ProbeVolumeShB", properties);
        MaterialProperty probeVolumeOcc = ShaderGUI.FindProperty("_ProbeVolumeOcc", properties);

        MaterialProperty debugView = ShaderGUI.FindProperty("_DebugView", properties);
        //EditorGUILayout.HelpBox("Test", MessageType.Info);

        EditorGUILayout.LabelField("Surface Options", header, GUILayout.ExpandWidth(true));
        EditorGUILayout.BeginVertical("HelpBox");
        materialEditor.ShaderProperty(surface, surface.displayName);
        if (surface.floatValue > 0.5)
        {
            materialEditor.ShaderProperty(blend, blend.displayName);
        }
        else
        {
            materialEditor.ShaderProperty(alphaClip, alphaClip.displayName);
            if (alphaClip.floatValue > 0.5)
            {
                materialEditor.ShaderProperty(cutoff, cutoff.displayName);
            }
        }
        materialEditor.ShaderProperty(cullMode, cullMode.displayName);
        materialEditor.ShaderProperty(receiveShadows, receiveShadows.displayName);
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("Base Layer", header, GUILayout.ExpandWidth(true));
        EditorGUILayout.BeginVertical("HelpBox");
        materialEditor.ShaderProperty(baseMap, baseMap.displayName);
        materialEditor.ShaderProperty(baseColor, baseColor.displayName);
        materialEditor.ShaderProperty(bumpMap, bumpMap.displayName);
        materialEditor.ShaderProperty(normalStrength, normalStrength.displayName);
        materialEditor.ShaderProperty(metallicMap, metallicMap.displayName);
        materialEditor.ShaderProperty(smoothness, smoothness.displayName);
        materialEditor.ShaderProperty(occlusionStrength, occlusionStrength.displayName);
        materialEditor.ShaderProperty(emissionColor, emissionColor.displayName);
        materialEditor.ShaderProperty(useEmission, useEmission.displayName);
        if (useEmission.floatValue > 0.5)
        {
            materialEditor.ShaderProperty(emissionMap, emissionMap.displayName);
        }
        materialEditor.ShaderProperty(useDetailMap, useDetailMap.displayName);
        if (useDetailMap.floatValue > 0.5)
        {
            materialEditor.ShaderProperty(detailAlbedoMap, detailAlbedoMap.displayName);
            materialEditor.ShaderProperty(detailAlbedoMapScale, detailAlbedoMapScale.displayName);
            materialEditor.ShaderProperty(detailNormalMap, detailNormalMap.displayName);
            materialEditor.ShaderProperty(detailNormalMapScale, detailNormalMapScale.displayName);
            materialEditor.ShaderProperty(detailWeightOverDistance, detailWeightOverDistance.displayName);
        }
        EditorGUILayout.EndVertical();

        useColorMask.floatValue = EditorGUILayout.ToggleLeft(useColorMask.displayName, useColorMask.floatValue > 0.5, header) ? 1 : 0;
        //materialEditor.ShaderProperty(useColorMask, useColorMask.displayName);
        EditorGUILayout.BeginVertical("HelpBox");
        if (useColorMask.floatValue > 0.5)
        {
            materialEditor.ShaderProperty(colorMask, colorMask.displayName);
            materialEditor.ShaderProperty(tint0, tint0.displayName);
            materialEditor.ShaderProperty(tint1, tint1.displayName);
            materialEditor.ShaderProperty(tint2, tint2.displayName);
            materialEditor.ShaderProperty(tint3, tint3.displayName);
        }
        EditorGUILayout.EndVertical();

        useReveal.floatValue = EditorGUILayout.ToggleLeft(useReveal.displayName, useReveal.floatValue > 0.5, header) ? 1 : 0;
        if (useReveal.floatValue > 0.5)
        {
            EditorGUILayout.HelpBox("Reveal Layers should not be enabled unless debugging.\nThis leads to increased memory requirements as the game will have reveal always active.", MessageType.Warning);
        }

        //materialEditor.ShaderProperty(useReveal, useReveal.displayName);
        EditorGUILayout.BeginVertical("HelpBox");
        if (debugView.floatValue > 0.5)
        {
            materialEditor.ShaderProperty(revealMask, revealMask.displayName);
        }
        if (GUILayout.Button("Auto-fill layer materials"))
        {
            layerMask.textureValue = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Reveal_WeaponBlood_Mask.png", typeof(Texture2D));
            layer0.textureValue = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Revealed_WeaponBlood_c.png", typeof(Texture2D));
            layer0NormalMap.textureValue = bumpMap.textureValue;

            layer1.textureValue = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Revealed_Burn_c.png", typeof(Texture2D));
            layer1NormalMap.textureValue = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SDK/Examples/Reveal/Revealed_Burn_n.png", typeof(Texture2D));
        }
        materialEditor.ShaderProperty(layerMask, layerMask.displayName);
        materialEditor.ShaderProperty(layerSurfaceExp, layerSurfaceExp.displayName);
        materialEditor.ShaderProperty(layer0, layer0.displayName);
        materialEditor.ShaderProperty(layer0NormalMap, layer0NormalMap.displayName);
        materialEditor.ShaderProperty(layer0NormalStrength, layer0NormalStrength.displayName);
        materialEditor.ShaderProperty(layer0Smoothness, layer0Smoothness.displayName);
        materialEditor.ShaderProperty(layer0Metallic, layer0Metallic.displayName);
        materialEditor.ShaderProperty(layer1, layer1.displayName);
        materialEditor.ShaderProperty(layer1NormalMap, layer1NormalMap.displayName);
        materialEditor.ShaderProperty(layer1NormalStrength, layer1NormalStrength.displayName);
        materialEditor.ShaderProperty(layer1Smoothness, layer1Smoothness.displayName);
        materialEditor.ShaderProperty(layer1Metallic, layer1Metallic.displayName);
        materialEditor.ShaderProperty(layer2Height, layer2Height.displayName);
        materialEditor.ShaderProperty(layer3EmissionColor, layer3EmissionColor.displayName);
        EditorGUILayout.EndVertical();

        useVertexOcclusion.floatValue = EditorGUILayout.ToggleLeft(useVertexOcclusion.displayName, useVertexOcclusion.floatValue > 0.5, header) ? 1 : 0;
        //materialEditor.ShaderProperty(useVertexOcclusion, useVertexOcclusion.displayName);
        EditorGUILayout.BeginVertical("HelpBox");
        if (useVertexOcclusion.floatValue > 0.5 && debugView.floatValue > 0.5)
        {
            materialEditor.ShaderProperty(bitmask, bitmask.displayName);
        }
        EditorGUILayout.EndVertical();

        //Only show probe volume when debug view is on.
        if (debugView.floatValue > 0.5)
        {
            useProbeVolume.floatValue = EditorGUILayout.ToggleLeft(useProbeVolume.displayName, useProbeVolume.floatValue > 0.5, header) ? 1 : 0;
            //materialEditor.ShaderProperty(useProbeVolume, useProbeVolume.displayName);
            EditorGUILayout.BeginVertical("HelpBox");
            if (useProbeVolume.floatValue > 0.5)
            {
                materialEditor.ShaderProperty(probeVolumeSHAr, probeVolumeSHAr.displayName);
                materialEditor.ShaderProperty(probeVolumeSHAg, probeVolumeSHAg.displayName);
                materialEditor.ShaderProperty(probeVolumeSHAb, probeVolumeSHAb.displayName);
                materialEditor.ShaderProperty(probeVolumeOcc, probeVolumeOcc.displayName);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.LabelField("Misc", header, GUILayout.ExpandWidth(true));
        materialEditor.ShaderProperty(debugView, debugView.displayName);
        materialEditor.ShaderProperty(queueOffest, queueOffest.displayName);
        materialEditor.RenderQueueField();
        materialEditor.EnableInstancingField();
        materialEditor.DoubleSidedGIField();

        //=====================================================================

        materialEditor.LightmapEmissionProperty();

        if (EditorGUI.EndChangeCheck())
        {
            foreach (Material material in materialEditor.targets)
            {
                MaterialChanged(material);

                material.globalIlluminationFlags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            }
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