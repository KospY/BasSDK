#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MaterialUpdater : EditorWindow
{
    [MenuItem("ThunderRoad (SDK)/Tools/Material Updater")]
    public static void GetWindow()
    {
        MaterialUpdater window = GetWindow<MaterialUpdater>("Material Updater");
        window.Show();
    }

    private static readonly string LITMOSS_PATH = "ThunderRoad/LitMoss - ASshader";
    private static readonly string TRFOLIAGE_PATH = "ThunderRoad/LitMoss - ASshader";

    private void OnGUI()
    {
        if (GetSelectedMaterials() is not Material[] selected ||
            selected.Length == 0)
        {
            EditorGUILayout.HelpBox("Select one or more materials to convert.", MessageType.Info);
            return;
        }

        EditorGUILayout.HelpBox($"This will convert {selected.Length} materials!", MessageType.Info);


        if (GUILayout.Button("CONVERT!"))
        {
            foreach (Material mat in GetSelectedMaterials())
            {
                //URP to lit moss
                if (mat.shader.name == "Universal Render Pipeline/Lit")
                { ConvertURPLitToMoss(mat); }
                // old lit to lit moss
                else if (mat.shader.name == "ThunderRoad/Lit_DONOTUSE_USE_TRLitMoss_Instead")
                { ConvertTRLitToMoss(mat); }
                // old foliage to new foliage
                else if (mat.shader.name == "ThunderRoad/FabricAndFoliage/FabricAndFoliage")
                { ConvertOldFoliageToNew(mat); }
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
        Debug.Log($"Converted to lit moss: {mat}!");
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
        Debug.Log($"Converted to lit moss: {mat}!");
    }

    private void ConvertOldFoliageToNew(Material mat)
    {
        if (mat == null)
        {
            Debug.LogError("Material is null!");
            return;
        }

        Undo.RecordObject(mat, "Convert To New Foliage");

        // GET OLD
        // Declare variables using the display names
        Texture Albedo = mat.GetTexture("_MainTex");
        Color ColorOverlay = mat.GetColor("Color_C776E9F7");
        float Saturation = mat.GetFloat("Vector1_2F70D75B");
        Texture MOES = mat.GetTexture("MOES");
        Texture Normal = mat.GetTexture("Texture2D_363A5C4F");
        float NormalStrength = mat.GetFloat("Vector1_b5167c4675424009928f6f8474af501f");
        float SmoothnessIntensity = mat.GetFloat("Vector1_5DA372D6");
        float AOIntensity = mat.GetFloat("Vector1_e8fb1702638644d7b05e9440a6f2c623");
        float UseEmission = mat.GetFloat("_UseEmission");
        float EmissionIntensity = mat.GetFloat("Vector1_A3B7D6BB");
        Color EmissionColor = mat.GetColor("Color_64fb66b10df2422fa7334430ca388963");
        float ScrollSpeed = mat.GetFloat("Vector1_37251d8dbf7247b6bffbfa434dcfa2cf");
        Texture WindNormal = mat.GetTexture("Texture2D_3899621E");
        float WindIntensity = mat.GetFloat("Vector1_534F54C8");
        float WindScalePrimary = mat.GetFloat("Vector1_A0C32172");
        float WindScaleSecondary = mat.GetFloat("Vector1_F7C5A4BA");
        float TimeScale = mat.GetFloat("Vector1_736F1270");
        float UseDirt = mat.GetFloat("Boolean_3d69f6b3de1c430b9c3eb5ad5ae934b5");
        Texture DirtMask = mat.GetTexture("Texture2D_5a915ec80c3f433c821da081b08a064e");
        float DirtTiling = mat.GetFloat("Vector1_700ce920b2494a218c35fd62c592ca56");
        Color DirtColor = mat.GetColor("Color_ac0e6272f0fd4543a2e94ca2b03211bd");
        float DirtOpacity = mat.GetFloat("Vector1_49f06d8066374081b2a6442d2d78648e");
        float BendStrength = mat.GetFloat("Vector1_ee74a58053f84c15afdfe943b8c4e3b9");
        Vector2 Direction = mat.GetVector("Vector2_02ebe3f1941e446b8bc3fa8506f06b8c");
        float UseWindSecondary = mat.GetFloat("BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF");
        float AlphaClip = mat.GetFloat("BOOLEAN_D63E016D74BD44708B3A4C86A8CDD876");
        float UseMOES = mat.GetFloat("BOOLEAN_956E3E8FC3244C5AB622041309F94EC2");
        Texture SampleTexture2D = mat.GetTexture("_SampleTexture2D_d7b3afec2a5b42de80ad4eb236c7c3d3_Texture_1");

        // Hidden properties (same display names as before)
        float WorkflowMode = mat.GetFloat("_WorkflowMode");
        float CastShadows = mat.GetFloat("_CastShadows");
        float ReceiveShadows = mat.GetFloat("_ReceiveShadows");
        float Surface = mat.GetFloat("_Surface");
        float Blend = mat.GetFloat("_Blend");
        float AlphaClipHidden = mat.GetFloat("_AlphaClip");
        float SrcBlend = mat.GetFloat("_SrcBlend");
        float DstBlend = mat.GetFloat("_DstBlend");
        float ZWrite = mat.GetFloat("_ZWrite");
        float ZWriteControl = mat.GetFloat("_ZWriteControl");
        float ZTest = mat.GetFloat("_ZTest");
        float Cull = mat.GetFloat("_Cull");
        float QueueOffset = mat.GetFloat("_QueueOffset");
        float QueueControl = mat.GetFloat("_QueueControl");
        Texture UnityLightmaps = mat.GetTexture("unity_Lightmaps");
        Texture UnityLightmapsInd = mat.GetTexture("unity_LightmapsInd");
        Texture UnityShadowMasks = mat.GetTexture("unity_ShadowMasks");

        // SET NEW
        mat.shader = Shader.Find(TRFOLIAGE_PATH);

        // Set new shader properties using the old ones
        mat.SetTexture("_MainTex", Albedo); // Albedo texture
        mat.SetColor("Color_C776E9F7", ColorOverlay); // Color Overlay
        mat.SetFloat("Vector1_2F70D75B", Saturation); // Saturation
        mat.SetTexture("MOES", MOES); // MOES texture
        mat.SetTexture("Texture2D_363A5C4F", Normal); // Normal map
        mat.SetFloat("Vector1_b5167c4675424009928f6f8474af501f", NormalStrength); // Normal Strength
        mat.SetFloat("Vector1_5DA372D6", SmoothnessIntensity); // Smoothness intensity
        mat.SetFloat("_Occlusion", AOIntensity); // AO intensity
      //  mat.SetFloat("_EmissionColor", EmissionIntensity); // Emission intensity
        mat.SetColor("_EmissionColor", EmissionColor); // Emission Color
                                                       // Assuming the scroll speed is used for wind normal scrolling
        mat.SetFloat("_ScrollSpeed", ScrollSpeed);
        mat.SetTexture("Texture2D_3899621E", WindNormal); // Wind normal map
        mat.SetFloat("Vector1_534F54C8", WindIntensity); // Wind Intensity
        mat.SetFloat("Vector1_A0C32172", WindScalePrimary); // Wind Scale Primary
        mat.SetFloat("Vector1_F7C5A4BA", WindScaleSecondary); // Wind Scale Secondary
        mat.SetFloat("Vector1_736F1270", TimeScale); // Time Scale
                                                     // Assuming the dirt mask and related properties correspond to the color mask in the new shader
        mat.SetFloat("_UseColorMask", UseDirt); // Use Dirt (toggle)
        mat.SetTexture("_ColorMask", DirtMask); // Dirt Mask texture
        mat.SetFloat("_ColorMaskRemap", DirtTiling); // Dirt Tiling
        //mat.SetColor("_ColorMask", DirtColor); // Dirt Color
        mat.SetFloat("_Strength", DirtOpacity); // Dirt Opacity
        mat.SetFloat("_BendStrength", BendStrength); // Bend Strength
        mat.SetVector("_Direction", Direction); // Direction vector for bend
        mat.SetFloat("BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF", UseWindSecondary); // Use Wind Secondary (toggle)
        mat.SetFloat("_AlphaClip", AlphaClip); // Alpha Clip (toggle)
        mat.SetFloat("BOOLEAN_956E3E8FC3244C5AB622041309F94EC2", UseMOES); // Use MOES (toggle)
        mat.SetTexture("_SampleTexture2D", SampleTexture2D); // Sample texture 2D

        // Additional properties (if they exist in the new shader)
        mat.SetFloat("_WorkflowMode", WorkflowMode);
        mat.SetFloat("_CastShadows", CastShadows);
        mat.SetFloat("_ReceiveShadows", ReceiveShadows);
        mat.SetFloat("_Surface", Surface);
        mat.SetFloat("_Blend", Blend);
        mat.SetFloat("_AlphaClip", AlphaClipHidden);
        mat.SetFloat("_SrcBlend", SrcBlend);
        mat.SetFloat("_DstBlend", DstBlend);
        mat.SetFloat("_ZWrite", ZWrite);
        mat.SetFloat("_ZWriteControl", ZWriteControl);
        mat.SetFloat("_ZTest", ZTest);
        mat.SetFloat("_Cull", Cull);
        mat.SetFloat("_QueueOffset", QueueOffset);
        mat.SetFloat("_QueueControl", QueueControl);
        mat.SetTexture("unity_Lightmaps", UnityLightmaps);
        mat.SetTexture("unity_LightmapsInd", UnityLightmapsInd);
        mat.SetTexture("unity_ShadowMasks", UnityShadowMasks);

        Undo.RecordObject(mat, "Convert To New Foliage");
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(mat));
        Debug.Log($"Converted to new foliage: {mat}!");
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