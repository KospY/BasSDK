#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LitToMoss : EditorWindow
{
    [MenuItem("ThunderRoad (SDK)/Tools/Lit To Moss")]
    public static void GetWindow()
    {
        LitToMoss window = GetWindow<LitToMoss>("Lit To Moss");
        window.Show();
    }

    private static readonly string LITMOSS_PATH = "ThunderRoad/LitMoss - ASshader";

    private void OnGUI()
    {
        if (GetSelectedMaterials() is not Material[] selected ||
            selected.Length == 0)
        {
            EditorGUILayout.HelpBox("Select one or more materials to convert.", MessageType.Info);
            return;
        }

        EditorGUILayout.HelpBox($"This will convert {selected.Length} materials to TRMoss!", MessageType.Info);


        if (GUILayout.Button("CONVERT!"))
        {
            foreach (Material mat in GetSelectedMaterials())
            {
                if (mat.shader.name == "Universal Render Pipeline/Lit")
                { ConvertURPLitToMoss(mat); }
                else
                { ConvertTRLitToMoss(mat); }
            }
        }
    }

    private void ConvertURPLitToMoss(Material mat)
    {
        if (mat == null)
        {
            Debug.LogError("Material is null!");
            return;
        }

        Texture2D baseMap = mat.GetTexture("_BaseMap") as Texture2D;
        Texture2D bumpMap = mat.GetTexture("_BumpMap") as Texture2D;
        Texture2D metallicGlossMap = mat.GetTexture("_MetallicGlossMap") as Texture2D;
        Texture2D emissionMap = mat.GetTexture("_EmissionMap") as Texture2D;
        Texture2D detailAlbedoMap = mat.GetTexture("_DetailAlbedoMap") as Texture2D;
        Color baseColor = mat.GetColor("_BaseColor");
        float smoothness = mat.GetFloat("_Smoothness");
        Color emissionColor = mat.GetColor("_EmissionColor");
        float detailAlbedoMapScale = mat.GetFloat("_DetailAlbedoMapScale");
        float detailNormalMapScale = mat.GetFloat("_DetailNormalMapScale");

        Undo.RecordObject(mat, "Convert To Moss Lit");

        mat.shader = Shader.Find(LITMOSS_PATH);
        mat.SetTexture("_AlbedoMap", baseMap);
        mat.SetColor("_Tint", baseColor);
        mat.SetTexture("_NormalMap", bumpMap);
        mat.SetTexture("_MaskMap", metallicGlossMap);
        mat.SetFloat("_UseMaskMap", metallicGlossMap != null ? 1 : 0);
        mat.SetFloat("_Smoothness", smoothness);
        mat.SetColor("_EmissionColor", emissionColor);
        mat.SetFloat("_UseEmission", emissionMap != null ? 1 : 0);
        mat.SetTexture("_EmissionMap", emissionMap);
        mat.SetTexture("_DetailMap", detailAlbedoMap);
        mat.SetFloat("_DetailAlbedoStrength", detailAlbedoMapScale);
        mat.SetFloat("_DetailNormalStrength", detailNormalMapScale);

        Undo.RecordObject(mat, "Convert To Moss Lit");
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(mat));
        Debug.Log($"Converted: {mat}!");
    }

    private void ConvertTRLitToMoss(Material mat)
    {
        if (mat == null)
        {
            Debug.LogError("Material is null!");
            return;
        }

        Undo.RecordObject(mat, "Convert To Moss Lit");

        // Retrieve Float properties
        float queueOffset = mat.GetFloat("_QueueOffset");
        float queueControl = mat.GetFloat("_QueueControl");
        float debugView = mat.GetFloat("_DebugView");
        float surface = mat.GetFloat("_Surface");
        float blend = mat.GetFloat("_Blend");
        float srcBlend = mat.GetFloat("_SrcBlend");
        float dstBlend = mat.GetFloat("_DstBlend");
        float zWrite = mat.GetFloat("_ZWrite");
        float cullMode = mat.GetFloat("_CullMode");
        float alphaClip = mat.GetFloat("_AlphaClip");
        float cutoff = mat.GetFloat("_Cutoff");
        float alphaStrength = mat.GetFloat("_AlphaStrength");
        float receiveShadows = mat.GetFloat("_ReceiveShadows");
        float smoothness = mat.GetFloat("_Smoothness");
        float occlusionStrength = mat.GetFloat("_OcclusionStrength");
        float useEmission = mat.GetFloat("_UseEmission");
        float useDetailMap = mat.GetFloat("_UseDetailMap");
        float detailAlbedoMapScale = mat.GetFloat("_DetailAlbedoMapScale");
        float detailNormalMapScale = mat.GetFloat("_DetailNormalMapScale");
        float useColorMask = mat.GetFloat("_UseColorMask");
        float useReveal = mat.GetFloat("_UseReveal");
        float layer0NormalStrength = mat.GetFloat("_Layer0NormalStrength");
        float layer0Smoothness = mat.GetFloat("_Layer0Smoothness");
        float layer0Metallic = mat.GetFloat("_Layer0Metallic");
        float layer1NormalStrength = mat.GetFloat("_Layer1NormalStrength");
        float layer1Smoothness = mat.GetFloat("_Layer1Smoothness");
        float layer1Metallic = mat.GetFloat("_Layer1Metallic");
        float layer2Height = mat.GetFloat("_Layer2Height");
        float useVertexOcclusion = mat.GetFloat("_UseVertexOcclusion");
        int bitmask = mat.GetInt("_Bitmask");
        float useProbeVolume = mat.GetFloat("_UseProbeVolume");
        float vertexColorIntensity = mat.GetFloat("_VertexColorIntensity");

        // Retrieve Texture properties
        Texture2D baseMap = mat.GetTexture("_BaseMap") as Texture2D;
        Texture2D bumpMap = mat.GetTexture("_BumpMap") as Texture2D;
        Texture2D metallicGlossMap = mat.GetTexture("_MetallicGlossMap") as Texture2D;
        Texture2D emissionMap = mat.GetTexture("_EmissionMap") as Texture2D;
        Texture2D detailAlbedoMap = mat.GetTexture("_DetailAlbedoMap") as Texture2D;
        Texture2D detailNormalMap = mat.GetTexture("_DetailNormalMap") as Texture2D;
        Texture2D colorMask = mat.GetTexture("_ColorMask") as Texture2D;
        Texture2D revealMask = mat.GetTexture("_RevealMask") as Texture2D;
        Texture2D layerMask = mat.GetTexture("_LayerMask") as Texture2D;
        Texture2D layer0 = mat.GetTexture("_Layer0") as Texture2D;
        Texture2D layer0NormalMap = mat.GetTexture("_Layer0NormalMap") as Texture2D;
        Texture2D layer1 = mat.GetTexture("_Layer1") as Texture2D;
        Texture2D layer1NormalMap = mat.GetTexture("_Layer1NormalMap") as Texture2D;
        Texture3D probeVolumeShR = mat.GetTexture("_ProbeVolumeShR") as Texture3D;
        Texture3D probeVolumeShG = mat.GetTexture("_ProbeVolumeShG") as Texture3D;
        Texture3D probeVolumeShB = mat.GetTexture("_ProbeVolumeShB") as Texture3D;
        Texture3D probeVolumeOcc = mat.GetTexture("_ProbeVolumeOcc") as Texture3D;

        // Retrieve Color properties
        Color baseColor = mat.GetColor("_BaseColor");
        Color emissionColor = mat.GetColor("_EmissionColor");
        Color tint0 = mat.GetColor("_Tint0");
        Color tint1 = mat.GetColor("_Tint1");
        Color tint2 = mat.GetColor("_Tint2");
        Color tint3 = mat.GetColor("_Tint3");
        Color layer3EmissionColor = mat.GetColor("_Layer3EmissionColor");

        // Retrieve Vector properties
        Vector4 detailWeightOverDistance = mat.GetVector("_DetailWeightOverDistance");
        Vector4 layerSurfaceExp = mat.GetVector("_LayerSurfaceExp");
        Vector4 probeVolumeMin = mat.GetVector("_ProbeVolumeMin");
        Vector4 probeVolumeSizeInv = mat.GetVector("_ProbeVolumeSizeInv");

        mat.shader = Shader.Find(LITMOSS_PATH);
        mat.SetTexture("_AlbedoMap", baseMap);
        mat.SetColor("_Tint", baseColor);
        mat.SetTexture("_NormalMap", bumpMap);
        mat.SetFloat("_NormalStrength", layer0NormalStrength);
        mat.SetTexture("_MossMetalMap", metallicGlossMap);
        mat.SetFloat("_MossMetalMode", metallicGlossMap != null ? 1 : 0);
        mat.SetFloat("_UseMossMetalMap", metallicGlossMap != null ? 1 : 0);
        mat.SetFloat("_Metallic", layer0Metallic);
        mat.SetFloat("_Smoothness", smoothness);
        mat.SetColor("_EmissionColor", emissionColor);
        mat.SetFloat("_UseEmission", useEmission);
        mat.SetTexture("_EmissionMap", emissionMap);
        mat.SetFloat("_UseDetail", useDetailMap);
        mat.SetTexture("_DetailMap", detailAlbedoMap);
        mat.SetFloat("_DetailAlbedoStrength", detailAlbedoMapScale);
        mat.SetFloat("_DetailNormalStrength", detailNormalMapScale);

        Undo.RecordObject(mat, "Convert To Moss Lit");
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(mat));
        Debug.Log($"Converted: {mat}!");
    }

    private Material[] GetSelectedMaterials()
    {
        List<Material> mats = new List<Material>();

        foreach (UnityEngine.Object obj in Selection.objects)
        {
            if (obj is Material mat &&
                mat.shader != null)
            { mats.Add(mat); }
        }

        return mats.ToArray();
    }
}
#endif