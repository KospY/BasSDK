// Made with Amplify Shader Editor v1.9.3.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ThunderRoad/Dev/LitMoss - Dev"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[Feature(_CAUSTICSENABLE)]HeaderOceanFog("# Ocean Fog Caustics", Float) = 0
		[Toggle(_USECAUSTICEXTRASAMPLER_ON)] _UseCausticExtraSampler("UseCausticExtraSamp[ler", Float) = 1
		[Toggle(_USECAUSTICRAINBOW_ON)] _UseCausticRainbow("UseCausticRainbow", Float) = 1
		[Toggle(_USECAUSTICSFROMABOVE_ON)] _UseCausticsFromAbove("Use Caustics From Above", Float) = 1
		[Feature(_UseReveal)]_RevealLayers("# Reveal Layers", Float) = 0
		[Toggle(_REVEALLAYERS)] _UseReveal("Use Reveal Layers", Float) = 0
		[NoScaleOffset]_LayerMask("Layer Mask &", 2D) = "white" {}
		[NoScaleOffset]_RevealMask("Reveal Mask &", 2D) = "black" {}
		_LayerSurfaceExp("Layer Surface Exponents (Albedo, Normal, Metallic, Smoothness)", Vector) = (1,1,1,1)
		_Layer0Red("## Layer0 Red", Float) = 0
		_Layer0("-Layer0 (R) &", 2D) = "black" {}
		[NoScaleOffset]_Layer0NormalMap("-Layer0 (R) Normal &", 2D) = "bump" {}
		_Layer0Metallic("-Layer0 (R) Metallic", Range( 0 , 1)) = 0
		_Layer0Smoothness("-Layer0 (R) Smoothness", Range( 0 , 1)) = 0.5
		_Layer0NormalStrength("-Layer0 (R) Normal Strength", Range( 0 , 2)) = 1
		_Layer1Green("## Layer1 Green", Float) = 0
		_Layer1("-Layer1 (G) &", 2D) = "black" {}
		[NoScaleOffset]_Layer1NormalMap("-Layer1 (G) Normal &", 2D) = "bump" {}
		_Layer1Metallic("-Layer1 (G) Metallic", Range( 0 , 1)) = 0
		_Layer1NormalStrength("-Layer1 (G) Normal Strength", Range( 0 , 2)) = 1
		_Layer1Smoothness("-Layer1 (G) Smoothness", Range( 0 , 1)) = 0.5
		_Layer2Blue("## Layer2 Blue", Float) = 0
		_Layer2Height("-Layer2 (B) Height", Range( -20 , 20)) = -1
		_Layer3Alpha("## Layer3 Alpha", Float) = 0
		[HDR]_Layer3EmissionColor("-Layer3 (A) Emission Color", Color) = (0,0,0,1)
		[Feature(_USE_SSS)]HeaderSSS("# SSS", Int) = 0
		[Toggle(_USE_SSS_ON)] _USE_SSS("Use SSS", Float) = 0
		_SSSRadius("-Radius [_USE_SSS]", Range( 0 , 1)) = 0.8
		_SSScattering("-Scattering [_USE_SSS]", Range( 0 , 1)) = 0.2
		_SSSColor("-SSSColor [_USE_SSS]", Color) = (0.945098,0.7137255,0.6078432,0)
		_NormalStrength_USE_SSS("-Normal Strength [_USE_SSS]", Range( 0 , 1)) = 0.5
		_SSSDimAlbedo("-Dim Albedo [_USE_SSS]", Range( 0 , 1)) = 0.5
		_SSShadcowMix("-Shadow Mix [_USE_SSS]", Range( 0 , 1)) = 1
		HeaderTransparency("# Transparency", Int) = 0
		[HideInInspector][Toggle]_Int0("Use AlphaToCoverage", Int) = 0
		[MaterialEnumDrawerExtended(Opaque,0,Transparent,1)]_Surface("Surface", Float) = 0
		[MaterialEnumDrawerExtended(Alpha,0,Premultiply,1,Additive,2,Multiply,3)]_Blend("-Transparent Blend Mode [_Surface]", Float) = 0
		[MaterialEnumDrawerExtended(UnityEngine.Rendering.BlendOp)]_BlendOp("Blend Op RGB", Float) = 0
		[MaterialEnumDrawerExtended(UnityEngine.Rendering.BlendMode)]_SrcBlend("Source Blend RGB", Float) = 1
		[MaterialEnumDrawerExtended(UnityEngine.Rendering.BlendMode)]_DstBlend("Dest Blend RGB", Float) = 1
		[Toggle(_ALPHATEST_ON)]_AlphaClip("Alpha Clipping", Range( 0 , 1)) = 1
		[Toggle]_Dither("-Dither [_AlphaClip&&!_Surface]", Range( 0 , 1)) = 0
		_AlphaClipThreshold("-AlphaClip Threshold [!_AlphaToCoverage&&_AlphaClip&&!_Dither]", Range( 0 , 1.01)) = 0.5
		_ShadowThreshold("-ShadowThreshold [_AlphaClip]", Range( 0 , 1.01)) = 0.5
		_Cutoff("Cutoff (Baked Lighting)", Range( 0 , 1.01)) = 0.5
		[Feature(_USESPLAT)]HeaderSplat("# Splat", Int) = 0
		[Toggle(_USESPLAT_ON)] _UseSplat("Use Splat", Float) = 0
		[NoScaleOffset]_DiffuseArray("Diffuse Array & [_UseSplat]", 2DArray) = "white" {}
		[NoScaleOffset]_ExtraArray("Extra Array & [_UseSplat]", 2DArray) = "black" {}
		[NoScaleOffset]_NormalArray("Normal Array & [_UseSplat]", 2DArray) = "bump" {}
		[Toggle(_SIMPLESTOCHASTIC)] _SIMPLESTOCHASTIC("Use Simple Stochastic [_UseSplat]", Float) = 1
		[Toggle(_ALPHAMODE)] _ALPHAMODE("Alpha Mode [_UseSplat]", Float) = 0
		[Feature(_UseSimLighting)]_SimLighting("# Sim Lighting", Float) = 0
		[Feature(_COLORMASK)]HeaderColorMask("# Color Mask", Float) = 0
		[Toggle(_COLORMASK_ON)] _UseColorMask("Use Color Mask", Float) = 0
		[HideInInspector]_UseColorMask("_UseColorMask", Int) = 0
		[MaterialEnumDrawerExtended(Original,0,Multiply,1,Screen,2,Overlay,3)]_BlendMode_UseColorMask("BlendMode [_UseColorMask]", Int) = 0
		[NoScaleOffset][ShowIfDrawer(_UseColorMask)]_ColorMask("Color Mask & [_UseColorMask]", 2D) = "black" {}
		_Tint0("Tint 0 [_UseColorMask]", Color) = (1,0.72183,0,1)
		_Tint1("Tint 1 [_UseColorMask]", Color) = (0,1,0.2660749,1)
		_Tint2("Tint 2 [_UseColorMask]", Color) = (0,0,1,1)
		_Tint3("Tint 3 [_UseColorMask]", Color) = (1,0,0.4031587,1)
		_Bitmask("Bitmask [_VERTEXOCCLUSION]", Int) = 0
		[Feature(_SKYFOG)]_SkyFogHeader("# Sky Fog", Float) = 0
		[Feature(_SKYFOG)][Toggle(_SKYFOG_ON)] _SKYFOG("Use Sky Fog", Float) = 0
		[FeatureCommentOut(_DebugVisuals)][FeatureCommentOut(_DEBUGVISUALS)]HeaderDebug("# Debug", Int) = 0
		[Toggle(_DEBUGVISUALS_ON)] _DebugVisuals("DebugVisuals", Float) = 0
		[MaterialEnumDrawerExtended(ShaderCost,0,WorldNormal,1,MossMask,2,MossFresnel,3,MaskDirection,4,MossNoiseAB,5,MossVertexMask,6,NOTHING,7,DetailMapAlbedo,8,DetailMapNormal,9,DetailMapSmoothness,10,Smoothness,11,NormalRes,12,SlopeMask,13,SlopeNormal,14,MaskDebug,15)][Feature(_DebugVisuals)]_DebugVisual("DebugVisual [_DebugVisuals]", Int) = 0
		[HideInInspector]_DebugCounter("DebugCounter", Float) = 0
		_DebugScale("DebugScale [_DebugVisuals]", Range( 1 , 32)) = 4
		HeaderGeneral("# General", Float) = 0
		[Header(Albedo)][MainTexture]_BaseMap("Base Map", 2D) = "white" {}
		[MainColor]_BaseColor("Base Color", Color) = (1,1,1,1)
		[Toggle]_UseNormalMap("Use Normal Map", Float) = 1
		[NoScaleOffset][Normal]_BumpMap("-Normal Map && [_UseNormalMap]", 2D) = "bump" {}
		_NormalStrength("Normal Strength [_UseNormalMap]", Range( -5 , 5)) = 1
		[FeatureCommentOut(_UseAnisotropy)]_Anisotrophy("# Anisotropy", Int) = 0
		[Feature(_UseGlitter)]_Glitter("# Glitter", Int) = 0
		[Feature(_PROBEVOLUME)]_PRVOL("# Probe Volumes", Float) = 0
		[Toggle(_PROBEVOLUME_ON)] _UseProbeVolume("Use Probe Volume", Float) = 0
		_ProbeVolumeShR("ProbeVolumeShR & [_PROBEVOLUME_ON]", 3D) = "black" {}
		_ProbeVolumeShG("ProbeVolumeShG & [_PROBEVOLUME_ON]", 3D) = "black" {}
		[HideInInspector]_ProbeVolumeMin("ProbeVolumeMin", Vector) = (0,0,0,0)
		_ProbeVolumeShB("ProbeVolumeShB & [_PROBEVOLUME_ON]", 3D) = "black" {}
		_ProbeVolumeOcc("ProbeVolumeOcc & [_PROBEVOLUME_ON]", 3D) = "black" {}
		[HideInInspector]_ProbeVolumeSizeInv("ProbeVolumeSizeInv", Vector) = (0,0,0,0)
		HeaderMetallic("# Metallic (MOES/MODS/Moss ANSN)", Int) = 0
		[Toggle(_MOSSMETALMODE_ON)] _MossMetalMode("Moss Metal Mode", Float) = 0
		[MaterialEnumDrawerExtended(Metal MOES MODS,0,Moss ANSN,1)]_MossMode("MossMode [_MossMetalMode]", Int) = 0
		[MaterialEnumDrawerExtended(MODS DetailMask,0,MOES EmissionMask,1)]_MODE("Mode [!_MossMode]", Int) = 1
		HeaderMetallic8("###   [(_MossMetalMode&&_MossMode)||!MossMetalMode&&_MossMode]", Int) = 0
		HeaderMetallic1("### 1 Map used for Moss ANSN (Albedo, Norm, Smoothness, Norm) [_MossMetalMode&&_MossMode]", Int) = 0
		HeaderMetallic3("### 1 Map used for MOES (Metal, Occlusion, EmissionMask, Smooth) [_MossMetalMode&&!_MossMode&&_MODE]", Int) = 0
		HeaderMetallic4("### 1 Map used for MODS (Metal, Occlusion, DetailMask, Smooth) [_MossMetalMode&&!_MossMode&&!_MODE]", Int) = 0
		HeaderMetallic6("###  [!_MossMetalMode]", Int) = 0
		HeaderMetallic2("### 2 Maps used for MOES and MossPacked [!_MossMetalMode&&_MODE]", Int) = 0
		HeaderMetallic5("### 2 Map used for MODS and MossPacked [!_MossMetalMode&&!_MODE]", Int) = 0
		[MaterialEnumDrawerExtended(UV0,0,UV1,1,UV2,2)]_MetalUV("Metal UV [!_MossMetalMode||(_MossMetalMode&&!_MossMode)]", Int) = 0
		[Toggle]_UseMossMetalMap("Use MossMetal Map [_MossMetalMode]", Float) = 0
		[NoScaleOffset]_MossMetalMap("-MossMetal & [_MossMetalMode&&_UseMossMetalMap]", 2D) = "white" {}
		LabelWarn3("-### TEXTURE MISSING - will screw up [!_MossMetalMap&&_MossMetalMode&&_UseMossMetalMap]", Float) = 0
		[Toggle]_MetallicSpecGlossMap("Use Metallic Map [!_MossMetalMode]", Float) = 0
		[NoScaleOffset]_MetallicGlossMap("-Metallic Map & [!_MossMetalMode&&_MetallicSpecGlossMap]", 2D) = "white" {}
		LabelWarning2("-### TEXTURE MISSING - normal will screw up [!_MetallicGlossMap&&!_MossMetalMode&&_MetallicSpecGlossMap]", Int) = 0
		HeaderMetallic7("###   [(!_MossMetalMode&&!_MetallicSpecGlossMap)||(_MossMetalMode&&!_UseMossMetalMap)]", Int) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		[Feature(_REMAPPERS)]_DRAWERPacked_MetallicGlossMapMetallic_Remap_Metal_RR_MetallicRemap_MOSSMETALMODE_ON_MetallicSpecGlossMap_Remappers("!DRAWER Packed _MetallicGlossMap Metallic_Remap_Metal_(R) R  _MetallicRemap [!_MOSSMETALMODE_ON&&_MetallicSpecGlossMap && _Remappers]", Float) = 0
		[Feature(_REMAPPERS)]_DRAWERPacked_MossMetalMapMetallic_Remap_Moss_RR_MetallicRemap_MOSSMETALMODE_ON_MossMode_UseMossMetalMap_Remappers("!DRAWER Packed _MossMetalMap Metallic_Remap_Moss_(R) R  _MetallicRemap [_MOSSMETALMODE_ON&&!_MossMode&&_UseMossMetalMap && _Remappers]", Float) = 0
		[HideInInspector]_MetallicRemap("MetallicRemap", Vector) = (0,1,0,1)
		[Toggle]_UseOcclusion("Use Occlusion [(!_MossMetalMode&&_MetallicSpecGlossMap)||(_MossMetalMode&&!_MossMode&&_UseMossMetalMap)]", Float) = 1
		_Occlusion("-Occlusion [_UseOcclusion]", Range( 0 , 1)) = 1
		[HideInInspector]_OcclusionRemap("OcclusionRemap", Vector) = (0,1,0,1)
		[Feature(_REMAPPERS)]_DRAWERPacked_MossMetalMapOcclusion_Remap_Moss_GG_OcclusionRemap_MOSSMETALMODE_ON_MossMode_UseOcclusion_UseMossMetalMap_Remappers("!DRAWER Packed _MossMetalMap Occlusion_Remap_Moss_(G) G  _OcclusionRemap [_MOSSMETALMODE_ON&&!_MossMode&&_UseOcclusion&&_UseMossMetalMap && _Remappers]", Float) = 0
		[Feature(_REMAPPERS)]_DRAWERPacked_MetallicGlossMapOcclusion_Remap_Metal_GG_OcclusionRemap_MOSSMETALMODE_ON_UseOcclusion("!DRAWER Packed _MetallicGlossMap Occlusion_Remap_Metal_(G) G  _OcclusionRemap [!_MOSSMETALMODE_ON&&_MetallicSpecGlossMap&&_UseOcclusion&&_MetallicSpecGlossMap && _Remappers]", Float) = 0
		_OcclusionMossMask("OcclusionMossMask [(!_MossMetalMode&&_UseOcclusion&&_MetallicSpecGlossMap)||(_MossMetalMode&&_UseOcclusion&&!_MossMode&&_UseMossMetalMap)]", Range( -2 , 2)) = -1
		[Toggle]_UseEmission("Use Emission", Float) = 0
		[HDR]_EmissionColor("-Emission Color [_UseEmission]", Color) = (0,0,0,0)
		[HideInInspector]_EmissionRemap("EmissionRemap", Vector) = (0,1,0,1)
		[Feature(_REMAPPERS)]Drawer_EmissionRemap2("!DRAWER Packed _MetallicGlossMap Emission_Remap_Metal_(B) B  _EmissionRemap [!_MOSSMETALMODE_ON&&_UseEmission&&_MODE&&_MetallicSpecGlossMap && _Remappers]", Float) = 0
		[Feature(_REMAPPERS)]Drawer_EmissionRemap4("!DRAWER Packed _MossMetalMap Emission_Remap_Moss_(B) B  _EmissionRemap [_MOSSMETALMODE_ON&&!_MossMode&&_UseEmission&&_MODE&&_UseMossMetalMap && _Remappers]", Float) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.5
		[HideInInspector]_SmoothnessRemap("SmoothnessRemap", Vector) = (0,1,0,1)
		[Feature(_REMAPPERS)]_DRAWERPacked_MetallicGlossMapSmoothness_Remap_MetalAA_SmoothnessRemap_MOSSMETALMODE_ON_MossMode1("!DRAWER Packed _MossMetalMap Smoothness_Remap_Moss_(A) A  _SmoothnessRemap [_MOSSMETALMODE_ON && !_MossMode && _UseMossMetalMap && _Remappers]", Float) = 0
		[Feature(_REMAPPERS)]_DRAWERPacked_MetallicGlossMapSmoothness_Remap_MetalAA_SmoothnessRemap_MOSSMETALMODE_ON_MossMode("!DRAWER Packed _MetallicGlossMap Smoothness_Remap_Metal_(A) A  _SmoothnessRemap [!_MOSSMETALMODE_ON && _MetallicSpecGlossMap && _Remappers]", Float) = 0
		[Feature(_DETAIL)]HeaderDetailMaps("# Detail Maps", Int) = 0
		[Feature(_DETAIL)]HeaderDetailMapsSpace("###  [!_Moss&&_DETAIL]", Int) = 0
		[Toggle(_DETAIL_ON)] _DETAIL("Use Details", Float) = 0
		_DetailsOverMoss("-Details Over Moss [_MOSS && _DETAIL_ON && _DetailMapPacked]", Range( -2 , 2)) = -1
		[NoScaleOffset]_DetailMapPacked("Detail Map Packed (ANSN) & [_DETAIL_ON]", 2D) = "black" {}
		[Feature(_DETAIL)]WarningTexMissing("-### TEXTURE MISSING - will break! [_DETAIL_ON && !_DetailMapPacked]", Int) = 0
		[HideInInspector]_DetailMaskRemap("DetailMaskRemap", Vector) = (0,1,0,1)
		[Toggle][Feature(_DETAIL)]_UseDetailMask("-Use Detail Mask [_DETAIL_ON && !_MossMode && ((_MossMetalMode&&_UseMossMetalMap&&!_MODE)||(!_MossMetalMode&&_MetallicSpecGlossMap&&!_MODE))]", Float) = 1
		[Feature(_REMAPPERS)]Drawer_EmissionRemap3("!DRAWER Packed _MossMetalMap DetailMask_Remap_Moss_(B) B  _DetailMaskRemap [_MOSSMETALMODE_ON&&!_MossMode&&!_MODE&&_UseDetailMask&&_DETAIL&&_DetailMapPacked&&_UseMossMetalMap && _Remappers]", Float) = 0
		[Feature(_REMAPPERS)]Drawer_EmissionRemap1("!DRAWER Packed _MetallicGlossMap DetailMask_Remap_Metal_(B) B  _DetailMaskRemap [!_MossMetalMode && !_MODE && _UseDetailMask && _DETAIL && _DetailMapPacked && _Remappers]", Float) = 0
		_DetailMapScale("Detail Map Scale [_DETAIL_ON && _DetailMapPacked]", Vector) = (1,1,0,0)
		_DetailAlbedoMapScale("-Detail Albedo Scale [_DETAIL_ON && _DetailMapPacked]", Range( 0 , 2)) = 0
		_DetailNormalMapScale("Detail Normal Scale [_DETAIL_ON&&_DetailMapPacked]", Range( -5 , 5)) = -2.190476
		[Feature(_REMAPPERS)]_DRAWER_PackedDM2("!DRAWER Packed _DetailMapPacked Detail_Smoothness_Remap_(B) B Mode_Negative _DetailSmoothnessRemap [_DETAIL_ON && _Remappers]", Float) = 0
		[HideInInspector]_DetailSmoothnessRemap("Smoothness Remap ", Vector) = (0,1,-1,1)
		_DetailNormalMapScale1("Detail Normal Scale Moss [_DETAIL_ON&&_DetailMapPacked&&_MOSS]", Range( -5 , 5)) = 0.3
		_MossDetailNormalMapScale("Normal Scale [_UseMossDetails&&_MOSS]", Range( -2 , 2)) = -2
		_MossDetailRemapping_MOSS_UseMossDetails("### Moss Detail Remapping [_MOSS&&_UseMossDetails]", Int) = 0
		_DRAWERMinMax_MossDetailRemapx_MossDetailRemapyRange_MOSS_UseMossDetails("-!DRAWER MinMax _MossDetailRemap.x _MossDetailRemap.y Range [_MOSS&&_UseMossDetails]", Float) = 0
		_DRAWERMinMax_MossDetailRemapz_MossDetailRemapwClamp_MOSS_UseMossDetails("-!DRAWER MinMax _MossDetailRemap.z _MossDetailRemap.w Clamp [_MOSS&&_UseMossDetails]", Float) = 0
		[Feature(_MOSS)]HeaderMoss("# Moss", Int) = 0
		[Toggle(_MOSS)] _Moss("Use Moss", Float) = 0
		[MaterialEnumDrawerExtended(UV0,0,UV1,1,UV2,2)]_MossUV("Moss UV [_MOSS&&_MossMode]", Int) = 0
		_MossScale("Moss Scale [_MOSS&&( !_MossMetalMode || _MossMode)]", Range( 0.01 , 1)) = 1
		HeaderSubMossAlbedo("## Moss Albedo [_MOSS]", Float) = 0
		[NoScaleOffset]_MossPacked("Moss Packed & [_MOSS && !_MossMetalMode]", 2D) = "white" {}
		LabelWarn("-### TEXTURE MISSING - will break! [!_MossPacked && _MOSS && !_MossMetalMode]", Float) = 0
		_MossColor("Moss Color [_MOSS]", Color) = (1,1,1,0)
		[HideInInspector]_MossSmoothnessRemap("MossSmoothnessRemap", Vector) = (0,1,0,1)
		[HideInInspector]_MossAlbedoRemap("MossAlbedoRemap", Vector) = (0,1,0,1)
		[Feature(_REMAPPERS)]Drawer_Moss_Albedo_Remap2("!DRAWER Packed _MossMetalMap Moss_Albedo_Remap_Metal_(R) R  _MossAlbedoRemap [_MOSS&&_MOSSMETALMODE_ON&&_MossMode && _Remappers]", Float) = 0
		[Feature(_REMAPPERS)]Drawer_Moss_Albedo_Remap1("!DRAWER Packed _MossPacked Moss_Albedo_Remap_Packed_(R) R  _MossAlbedoRemap [_MOSS&&!_MOSSMETALMODE_ON && _Remappers]", Float) = 0
		[Feature(_REMAPPERS)]DRAWER_MossSmoothnessRemap1("!DRAWER Packed _MossPacked Moss_Smoothness_Remap_Packed_(B) B  _MossSmoothnessRemap [_MOSS&&!_MOSSMETALMODE_ON && _Remappers]", Float) = 0
		[Feature(_REMAPPERS)]DRAWER_MossSmoothnessRemap2("!DRAWER Packed _MossMetalMap Moss_Smoothness_Remap_Metal_(B) B  _MossSmoothnessRemap [_MOSS&&_MOSSMETALMODE_ON&&_MossMode && _Remappers]", Float) = 0
		_MossSmoothness("Moss Smoothness [_MOSS]", Range( 0 , 1)) = 0
		_MossMetallic("Moss Metallic [_MOSS]", Range( 0 , 1)) = 0
		_MossMultAlbedo("Moss Mult Albedo [_MOSS]", Range( 0 , 1)) = 0
		HeaderSubNormal("## Normal [_MOSS]", Float) = 0
		_MossNormalStrength("Moss Normal Strength [_MOSS]", Range( -5 , 5)) = 0
		[HideInInspector]_Moss("_Moss", Int) = 0
		HeaderSubStochastic("## Stochastic [_MOSS]", Float) = 0
		[Toggle(_USESTOCHASTICMOSS_ON)] _UseStochasticMoss("Use Stochastic Moss [_MOSS&&!UseMossNoise3D]", Float) = 0
		_MossStochasticScale("-Moss Stochastic Scale [_MOSS&&_UseStochasticMoss]", Float) = 1
		_MossStochasticContrast("-Moss Stochastic Contrast [_MOSS&&_UseStochasticMoss]", Range( 0.01 , 1)) = 0.5
		HeaderMossMasks("# Moss Masks [_MOSS]", Float) = 0
		_MapContrast("Map Contrast [_MOSS]", Range( 0 , 1)) = 0.5
		_MapContrastOffset("Map Contrast Offset [_MOSS]", Range( 0 , 1)) = 0.5
		[Toggle]_UseMossDirection("Use Moss Direction [_MOSS]", Float) = 1
		_MossBase("-Moss Base [!_UseMossDirection]", Range( 0 , 1)) = 0.5
		_MossLevel("-Moss Level [_MOSS&&_UseMossDirection]", Range( -1 , 1)) = 0.1500682
		_MossDirContrast("-Moss Dir Contrast [_MOSS&&_UseMossDirection]", Range( 0 , 1)) = 0.9
		_MossDirection("-Moss Direction [_MOSS && _UseMossDirection]", Vector) = (0,1,0,0)
		_MossNormalAffectStrength("Moss Normal Affect [_MOSS&&_UseNormalMap]", Range( 0 , 1)) = 1
		_MossNormalSubtract("-Moss Normal Subtract [_MOSS&&_UseNormalMap]", Range( 0 , 1)) = 0.5
		_MossNormalContrast("-Moss Normal Contrast [_MOSS&&_UseNormalMap]", Range( 0 , 1)) = 0.5
		[Toggle][Header(Mask Vertex)]_UseMossVertexMask("Use Moss Vertex Mask [_MOSS]", Float) = 0
		Label_UseMossMaskWithAlpha2("### ", Float) = 0
		Label_UseMossMaskWithAlpha1("### Use Moss Alpha Mask ( Disabled when 'UseAlbedoAlpha' ) [_UseAlbedoAlpha]", Float) = 0
		[Toggle]_UseMossMaskWithAlpha("Use Moss Alpha Mask [!_UseAlbedoAlpha]", Float) = 0
		_DRAWERPacked_BaseMapMoss_Alpha_Remap_Albedo_AAMode_Negative_MossAlphaMaskMM_MOSS_UseMossMaskWithAlpha_UseAlbedoAlpha("!DRAWER Packed _BaseMap Moss_Alpha_Remap_(Albedo_A) A Mode_Negative _MossAlphaMaskMM [_MOSS&&_UseMossMaskWithAlpha&&!_UseAlbedoAlpha]", Float) = 0
		[HideInInspector]_MossAlphaMaskMM("Moss Alpha Mask Min Max", Vector) = (0,1,0,1)
		HeaderFresnel1("# Moss Fresnel [_MOSS]", Int) = 0
		[Toggle(_USEMOSSFRESNEL_ON)] _UseMossFresnel("Use Moss Fresnel [_MOSS]", Float) = 0
		[HideInInspector]_UseMossFresnel("FresnelInt", Float) = 0
		[HDR]_MossFresnelColor("-Moss Fresnel Color [_MOSS&&_UseMossFresnel]", Color) = (1,1,1,0)
		_MossFresnel("Moss Fresnel (Bias, Scale, Power, Clamp)  [_MOSS]", Vector) = (0,1,5,0.5)
		_DRAWERVectorSlider_MossFresnelFirstSliderSecondSliderThirdSlider_MOSS_UseMossFresnel("-!DRAWER VectorSlider _MossFresnel (First Slider, Second Slider, Third Slider) [_MOSS&&_UseMossFresnel]", Vector) = (0,0,0,0)
		[Toggle(_USEMOSSNOISE_ON)] _UseMossNoise("Use Moss Noise [_MOSS]", Float) = 0
		_DRAWERPacked_MossNoiseNoise_RemapRMode_Negative_MossNoiseMM_MOSS_UseMossNoise("!DRAWER Packed _MossNoise Noise_Remap R Mode_Negative _MossNoiseMM [_MOSS&&_UseMossNoise]", Float) = 0
		[Feature(_UseMossSlope)]HeaderMossSlope("# Moss Slope [_MOSS]", Float) = 0
		[Toggle(_USEMOSSSLOPEDISTORT_ON)] _UseMossSlopeDistort("UseMossSlopeDistort [_UseMossSlope]", Float) = 0
		_MossSlopeDistort("--Moss Slope Distort [_MOSS&&_UseMossSlopeDistort&&_UseMossSlope]", Range( 0 , 1)) = 1
		_DRAWERMinMax_MossSlopeMM("-!DRAWER MinMax _MossSlopeMM [_MOSS&&_UseMossSlope]", Float) = 0
		[RemapSlidersFull]_MossSlopeMM("Moss Slope Min Max", Vector) = (0,1,0,1)
		_MossSlopeNormRotate("-Moss Slope Norm Rotate [_MOSS&&_UseMossSlope]", Range( 0 , 360)) = 0
		[Normal]_MossSlopeNormal("-MossSlopeNormal  [_MOSS&&_UseMossSlope]", 2D) = "bump" {}
		[ShowIfDrawer(_UseNormalMap)]_MossNormalStrength2("-Moss Slope Normal Strength [_MOSS&&_UseMossSlope]", Range( -2 , 2)) = 0
		[HideInInspector][KeywordEnum(MOSS,MOES,MODS,OFF)] MODED("Mode Dropdown", Float) = 0
		[KeywordEnum(UNLIMITED,MAX2,MAX3)] LAYER("Layer Limit [_UseSplat]", Float) = 2
		[KeywordEnum(MAX4,MAX8,MAX12,MAX16)] TEXTURE("Texture Limit [_UseSplat]", Float) = 1
		[Toggle(_AUTONORMAL)] _AUTONORMAL("Normal from Height [_UseSplat]", Float) = 0
		HeaderMisc("# Misc", Int) = 0
		[Toggle(_REMAPPERS_ON)] _Remappers("Remappers", Float) = 1
		[MaterialEnumDrawerExtended(Off,0,On,1)]_ZWrite("ZWrite", Int) = 1
		[MaterialEnumDrawerExtended(UnityEngine.Rendering.CullMode)]_Cullmode("Cullmode", Int) = 2


		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
		
		//Shadowood: reveal these checkboxes:
		//[HideInInspector]
		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		//[HideInInspector]
		[ToggleOff] _EnvironmentReflections("Environment Reflections", Float) = 1.0
		//[HideInInspector]
		[ToggleOff] _ReceiveShadows("Receive Shadows", Float) = 1.0

		[HideInInspector] _QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector] _QueueControl("_QueueControl", Float) = -1

        [HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" "UniversalMaterialType"="Lit" }

		Cull [_Cullmode]
		ZWrite [_ZWrite]
		ZTest LEqual
		Offset 0,0
		AlphaToMask [AlphaToCoverage]

		

		HLSLINCLUDE
		#pragma target 5.0
		#pragma prefer_hlslcc gles
		// ensure rendering platforms toggle list is visible

		//#define SHADOWOOD_WATER_SHADER_VARIABLES_INCLUDED

		#ifdef _ASE_TONEMAPPING
			//#define TONEMAPPINGELSEWHERE
		#endif

		//#ifdef ASE_FOGCOLOR
			//#define UNDERWATERELSEWHERE
		//#endif

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
				
		// Shadowood: undef ase_fog and def DEBUG_BRANCH stuff below:
		#ifdef ASE_FOGCOLOR
		    #undef ASE_FOG
		#endif
				
		#ifdef _DEBUGVISUALS_ON
			#define DEBUG_BRANCH(w,c) if (w > 0.001) _DebugCounter+=c;
		#else
			#define DEBUG_BRANCH(w,c) UNITY_BRANCH
		#endif

		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}

		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForwardOnly" }

			Blend [_SrcBlend] [_DstBlend], One Zero
			//BlendOp [_BlendOp], [_Blend]// Shadowood
			ZWrite [_ZWrite]
			ZTest LEqual
			Offset 0,0
			//ColorMask RGBA

			

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ASE_SSSCONTROL 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define TONEMAPPINGELSEWHERE
			#define UNDERWATERELSEWHERE
			#define ASE_USING_SAMPLING_MACROS 1


			#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
			#pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
			#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
			#pragma multi_compile_fragment _ _SHADOWS_SOFT
			#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
			#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma multi_compile_fragment _ _LIGHT_LAYERS
			#pragma multi_compile_fragment _ _LIGHT_COOKIES
			#pragma multi_compile _ _CLUSTERED_RENDERING

			// Shadowood
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature_local _ALPHAPREMULTIPLY_ON

			#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile_fragment _ DEBUG_DISPLAY

			//Shadowood: set _ALPHATEST_ON keyword
			#ifdef _ASE_ALPHATEST
				#pragma shader_feature_local _ _ALPHATEST_ON
			#endif
			//#ifdef SW_TESSELLATION
			//	#define ASE_TESSELLATION 1
			//	#pragma require tessellation tessHW
			//	#pragma hull HullFunction
			//	#pragma domain DomainFunction
			//	//#define ASE_DISTANCE_TESSELLATION
			//#endif

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_FORWARD

			// Shadowood: add tonemapping include
			//#ifndef TONEMAPPINGELSEWHERE
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
			//#endif
			
			//#ifndef UNDERWATERELSEWHERE
			#ifndef UNDERWATER_GEAR
				#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
			#endif

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include_with_pragmas  "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			// From: Assets\Plugins\Lux URP Essentials\Shaders\Includes\Lux URP Hair Lighting.hlsl

			// Ref: Donald Revie - Implementing Fur Using Deferred Shading (GPU Pro 2)
			// The grain direction (e.g. hair or brush direction) is assumed to be orthogonal to the normal.
			// The returned normal is NOT normalized.
			half3 ComputeGrainNormal_Lux(half3 grainDir, half3 V)
			{
				half3 B = cross(-V, grainDir);
				return cross(B, grainDir);
			}
			
			// Fake anisotropic by distorting the normal.
			// The grain direction (e.g. hair or brush direction) is assumed to be orthogonal to N.
			// Anisotropic ratio (0->no isotropic; 1->full anisotropy in tangent direction)
			half3 GetAnisotropicModifiedNormal_Lux(half3 grainDir, half3 N, half3 V, half anisotropy)
			{
				half3 grainNormal = ComputeGrainNormal_Lux(grainDir, V);
				return lerp(N, grainNormal, anisotropy);
			}

			struct AdditionalData {
				half3   tangentWS;
				half3   bitangentWS;
				
				//
				
				//half    specularShift;
				//half3   specularTint;
				//half    primarySmoothness;
				//half    secondarySpecularShift;
				//half3   secondarySpecularTint;
				//half    secondarySmoothness;
				
				//

				float   partLambdaV;
				half    roughnessT;
				half    roughnessB;
				half3   anisoReflectionNormal;
			};
			
			// End of hair
			
			

			// Shadowood - hack to replace all Texture2DArray with Texture2D when not in VR for use with _CameraDepthTexture for example
			// Stereo singlepass instancing turns various textures into arrays with 2 textures in one per eye. So normally you'd use TEXTURE2D_X and it turns into the right one for you
			#if defined (_ASE_Texture2DX)
				#undef TEXTURE2D_ARRAY
				#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
					#define TEXTURE2D_ARRAY(textureName) Texture2DArray textureName
				#else // UNITY_STEREO
					#define TEXTURE2D_ARRAY(textureName) Texture2D textureName
				#endif
			#endif
			
			// Shadowood - modified from: Packages\com.unity.render-pipelines.universal@12.1.15\ShaderLibrary\GlobalIllumination.hlsl
			
			half3 GlobalIlluminationCustom(BRDFData brdfData, BRDFData brdfDataClearCoat, float clearCoatMask,
				half3 bakedGI, half occlusion, float3 positionWS,
				half3 normalWS, half3 viewDirectionWS, half fresnelControl, half3 anisoReflectionNormal, half environmentReflections)
			{

				#if defined(_ASE_ANIS) && defined(_USE_ANIS_ON)
					half3 reflectVector = reflect(-viewDirectionWS, anisoReflectionNormal);
				#else
					half3 reflectVector = reflect(-viewDirectionWS, normalWS);
				#endif

				#if defined(_ASE_FRESNELCONTROL)
					half fresnelTerm = fresnelControl;
				#else
					half NoV = saturate(dot(normalWS, viewDirectionWS));
					half fresnelTerm = Pow4(1.0 - NoV); // Original
					//half fresnelExponent = fresnelControl.x;
					//half fresnelStrength = fresnelControl.y;
					//half fresnelTerm = pow(1.0 - NoV, fresnelExponent) * fresnelStrength;
				#endif

				
				half3 indirectDiffuse = bakedGI;
				
				float3 reflectVectorNew = reflectVector;
				/* // TODO: Rotate reflection, instead pass the float2x2 matrix from CPU side via shader global
				float degrees = 9;
				float alpha = degrees * 3.142 / 180.0;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);
				float3 reflectVectorNew = float3(mul(m, reflectVector.xz), reflectVector.y).xzy;
				*/
				half3 indirectSpecular = half3(0,0,0);
				
				//if(environmentReflections > 0.5){ // Added so you can disable the env reflection without need for keyword '_ENVIRONMENTREFLECTIONS_OFF'
					indirectSpecular = GlossyEnvironmentReflection(reflectVectorNew, positionWS, brdfData.perceptualRoughness, 1.0h);
				//}
				
				half3 color = EnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);
			
				if (IsOnlyAOLightingFeatureEnabled())
				{
					color = half3(1,1,1); // "Base white" for AO debug lighting mode
				}
				
			/* // Don't need the clearcoat code
			#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
				//half3 coatIndirectSpecular = GlossyEnvironmentReflection(reflectVector, positionWS, brdfDataClearCoat.perceptualRoughness, 1.0h);
				half3 coatIndirectSpecular = GlossyEnvironmentReflectionCustom(reflectVector, positionWS, brdfDataClearCoat.perceptualRoughness, 1.0h);
				
				// TODO: "grazing term" causes problems on full roughness
				half3 coatColor = EnvironmentBRDFClearCoat(brdfDataClearCoat, clearCoatMask, coatIndirectSpecular, fresnelTerm);
			
				// Blend with base layer using khronos glTF recommended way using NoV
				// Smooth surface & "ambiguous" lighting
				// NOTE: fresnelTerm (above) is pow4 instead of pow5, but should be ok as blend weight.
				half coatFresnel = kDielectricSpec.x + kDielectricSpec.a * fresnelTerm;
				return (color * (1.0 - coatFresnel * clearCoatMask) + coatColor) * occlusion;
			#else*/
				return color * occlusion;
			//#endif
			}
			


			// Modified version of 'CalculateLightingColor' from 'Lighting.hlsl'
			// Added: subsurfaceContribution
			half3 CalculateLightingColorCustom(LightingData lightingData, half3 albedo, half4 subsurfaceContribution, half4 subsurfaceContribution2)
			{
				half3 lightingColor = 0;
				//half3 lightingColorSub = half3(0,0,0);
				
				float _SubsurfaceScattering = subsurfaceContribution.a;
				
				if (IsOnlyAOLightingFeatureEnabled())
				{
					return lightingData.giColor; // Contains white + AO
				}
				
				if (IsLightingFeatureEnabled(DEBUGLIGHTINGFEATUREFLAGS_GLOBAL_ILLUMINATION))
				{
					lightingColor += lightingData.giColor;
				}
				
				if (IsLightingFeatureEnabled(DEBUGLIGHTINGFEATUREFLAGS_MAIN_LIGHT))
				{
					//lightingColor += lightingData.mainLightColor; // Normal lighting
					
					// SSS
					lightingColor += lightingData.mainLightColor;// * (1-_SubsurfaceScattering);
					lightingColor += subsurfaceContribution.rgb * (_SubsurfaceScattering) * albedo;
				}
				
				if (IsLightingFeatureEnabled(DEBUGLIGHTINGFEATUREFLAGS_ADDITIONAL_LIGHTS))
				{
					//lightingColor += lightingData.additionalLightsColor; // Normal lighting
					
					// SSS
					lightingColor += lightingData.additionalLightsColor;// * (1-_SubsurfaceScattering);
					lightingColor += subsurfaceContribution2.rgb * (_SubsurfaceScattering)  * albedo;
				}
				
				if (IsLightingFeatureEnabled(DEBUGLIGHTINGFEATUREFLAGS_VERTEX_LIGHTING))
				{
					lightingColor += lightingData.vertexLightingColor;
				}
				
				//lightingColor *= albedo;// * (1-dimAlbedo);
				//lightingColorSub *= albedo;
				
				if (IsLightingFeatureEnabled(DEBUGLIGHTINGFEATUREFLAGS_EMISSION))
				{
					lightingColor += lightingData.emissionColor;
				}
				
				return lightingColor;// + lightingColorSub;
			}
			
			// TODO move this to LightingPhysicallyBased so it can reuse the NdotL
			// Calculates the subsurface light radiating out from the current fragment. This is a simple approximation using wrapped lighting.
			// Note: This does not use distance attenuation, as it is intented to be used with a sun light.
			// Note: This does not subtract out cast shadows (light.shadowAttenuation), as it is intended to be used on non-shadowed objects. (for now)
			half4 LightingSubsurface(Light light, half3 normalWS, half3 subsurfaceColor, half subsurfaceRadius, half _SubsurfaceScattering) {
				// Calculate normalized wrapped lighting. This spreads the light without adding energy.
				// This is a normal lambertian lighting calculation (using N dot L), but warping NdotL
				// to wrap the light further around an object.
				//
				// A normalization term is applied to make sure we do not add energy.
				// http://www.cim.mcgill.ca/~derek/files/jgt_wrap.pdf

				//return half4(normalWS.x,normalWS.y,normalWS.z,0);
				half NdotL = dot(normalWS, light.direction);
				half alpha = subsurfaceRadius;
				half theta_m = acos(-alpha); // boundary of the lighting function
				
				half theta = max(0, NdotL + alpha) - alpha;
				half normalization_jgt = (2 + alpha) / (2 * (1 + alpha));
				half wrapped_jgt = (pow(((theta + alpha) / (1 + alpha)), 1 + alpha)) * normalization_jgt;
				
				//half wrapped_valve = 0.25 * (NdotL + 1) * (NdotL + 1);
				//half wrapped_simple = (NdotL + alpha) / (1 + alpha);
				
				half3 subsurface_radiance = light.color * subsurfaceColor.rgb * wrapped_jgt;

				//
				//half tintMul = subsurfaceColor.a;
				//half3 col = saturate(subsurfaceColor.rgb);//saturate( min( subsurfaceColor.rgb, half3(0.99,0.99,0.99) ) );
				//wrapped_jgt *= 20;
				//half3 sss = (exp( (-(half3(1.0,1.0,1.0)-col)) * half3(wrapped_jgt,wrapped_jgt,wrapped_jgt) ));
				//half3 subsurface_radiance_tinted = lerp(half3(0,0,0),subsurface_radiance,sss);
//return float4(sss,0);
				//subsurface_radiance = lerp(subsurface_radiance, subsurface_radiance_tinted, tintMul);
				//return half4(NdotL,0,0,_SubsurfaceScattering);
				return half4(subsurface_radiance, _SubsurfaceScattering);
			}

			// From: Packages\com.unity.render-pipelines.universal@12.1.15\ShaderLibrary\Lighting.hlsl
			/*
			half3 LightingPhysicallyBasedCustom(BRDFData brdfData, AdditionalData addData, BRDFData brdfDataClearCoat,
				half3 lightColor, half3 lightDirectionWS, half lightAttenuation,
				half3 normalWS, half3 viewDirectionWS,
				half clearCoatMask, bool specularHighlightsOff)
			{
				half NdotL = saturate(dot(normalWS, lightDirectionWS));
				half3 radiance = lightColor * (lightAttenuation * NdotL);
				
				half3 brdf = brdfData.diffuse;
				#ifndef _SPECULARHIGHLIGHTS_OFF
				[branch] if (!specularHighlightsOff)
				{
					brdf += brdfData.specular * DirectBRDFSpecular(brdfData, normalWS, lightDirectionWS, viewDirectionWS);
				
					#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
						// Clear coat evaluates the specular a second timw and has some common terms with the base specular.
						// We rely on the compiler to merge these and compute them only once.
						half brdfCoat = kDielectricSpec.r * DirectBRDFSpecular(brdfDataClearCoat, normalWS, lightDirectionWS, viewDirectionWS);
						
						// Mix clear coat and base layer using khronos glTF recommended formula
						// https://github.com/KhronosGroup/glTF/blob/master/extensions/2.0/Khronos/KHR_materials_clearcoat/README.md
						// Use NoV for direct too instead of LoH as an optimization (NoV is light invariant).
						half NoV = saturate(dot(normalWS, viewDirectionWS));
						// Use slightly simpler fresnelTerm (Pow4 vs Pow5) as a small optimization.
						// It is matching fresnel used in the GI/Env, so should produce a consistent clear coat blend (env vs. direct)
						half coatFresnel = kDielectricSpec.x + kDielectricSpec.a * Pow4(1.0 - NoV);
						
						brdf = brdf * (1.0 - clearCoatMask * coatFresnel) + brdfCoat * clearCoatMask;
					#endif // _CLEARCOAT
					}
				#endif // _SPECULARHIGHLIGHTS_OFF
				
				return brdf * radiance;
			}*/

			half3 DirectBDRF_LuxGGXAniso(BRDFData brdfData, AdditionalData addData, half3 normalWS, half3 lightDirectionWS, half3 viewDirectionWS, half NdotL)
			{
				#ifndef _SPECULARHIGHLIGHTS_OFF
					float3 lightDirectionWSFloat3 = float3(lightDirectionWS);
					float3 halfDir = SafeNormalize(lightDirectionWSFloat3 + float3(viewDirectionWS));
					
					float NoH = saturate(dot(float3(normalWS), halfDir));
					half LoH = half(saturate(dot(lightDirectionWSFloat3, halfDir)));
					
					half NdotV = saturate(dot(normalWS, viewDirectionWS ));
					
					//  GGX Aniso
					float3 tangentWS = float3(addData.tangentWS);
					float3 bitangentWS = float3(addData.bitangentWS);
					
					float TdotH = dot(tangentWS, halfDir);
					float TdotL = dot(tangentWS, lightDirectionWSFloat3);
					float BdotH = dot(bitangentWS, halfDir);
					float BdotL = dot(bitangentWS, lightDirectionWSFloat3);
					
					half3 F = F_Schlick(brdfData.specular, LoH); // 1.91: was float3
					
					//float TdotV = dot(tangentWS, viewDirectionWS);
					//float BdotV = dot(bitangentWS, viewDirectionWS);
					
					float DV = DV_SmithJointGGXAniso(
						TdotH, BdotH, NoH, NdotV, TdotL, BdotL, NdotL,
						addData.roughnessT, addData.roughnessB, addData.partLambdaV
					);
					half3 specularLighting = F * DV;
					
					return specularLighting + brdfData.diffuse;
				#else
					return brdfData.diffuse;
				#endif
			}

			half3 LightingPhysicallyBasedCustom(BRDFData brdfData, AdditionalData addData, BRDFData brdfDataClearCoat, Light light, half3 normalWS, half3 viewDirectionWS, half clearCoatMask, bool specularHighlightsOff)
			{
				#if defined(_ASE_ANIS) && defined(_USE_ANIS_ON)
					half3 lightColor = light.color;
					half lightAttenuation = light.distanceAttenuation * light.shadowAttenuation;
					half3 lightDirectionWS = light.direction;
					half NdotL = saturate(dot(normalWS, lightDirectionWS));
					half3 radiance = lightColor * (lightAttenuation * NdotL);
					return DirectBDRF_LuxGGXAniso(brdfData, addData, normalWS, lightDirectionWS, viewDirectionWS, NdotL) * radiance;
					//return LightingPhysicallyBasedCustom(brdfData, AdditionalData addData, brdfDataClearCoat, light.color, light.direction, light.distanceAttenuation * light.shadowAttenuation, normalWS, viewDirectionWS, clearCoatMask, specularHighlightsOff);
				#else
					return LightingPhysicallyBased(brdfData, brdfDataClearCoat, light.color, light.direction, light.distanceAttenuation * light.shadowAttenuation, normalWS, viewDirectionWS, clearCoatMask, specularHighlightsOff);
				#endif
			}


			// Modified version of 'UniversalFragmentPBR' from 'Lighting.hlsl'
			// Added SSS enabled by _USE_SSS_ON and _ASE_SSSCONTROL
			// Added custom fresnel enabled by _ASE_FRESNELCONTROL
			half4 UniversalFragmentPBRCustom(InputData inputData, SurfaceData surfaceData, half fresnelControl, half4 sssColor, half4 sssControl, float3 worldNormal, float3 worldTangent, float4 AnisotropicControl, half environmentReflections)
			{
				#if defined(_USE_SSS_ON) && defined(_ASE_SSSCONTROL)
					half4 subsurfaceContribution2 = half4(0,0,0,0);
					half3 _SubsurfaceColor = sssColor.rgb;//half3(0.8,0.6,0.6);
					half1 _SubsurfaceRadius = sssControl.x;//0.21;
					half1 _SubsurfaceScattering = sssControl.y;//1.21;
					half1 _SubsurfaceNormalStr = sssControl.z;//0.2;
					half1 _SubsurfaceDimAlbedo = saturate(sssControl.w);//0.2;
					half1 _SubsurfaceShadowMix = sssColor.w;
				#endif
				
				#if defined(_SPECULARHIGHLIGHTS_OFF)
					bool specularHighlightsOff = true;
				#else
					bool specularHighlightsOff = false;
				#endif
				
				BRDFData brdfData;
			
				// NOTE: can modify "surfaceData"...
				InitializeBRDFData(surfaceData, brdfData);
			
				#if defined(DEBUG_DISPLAY)
					half4 debugColor;
					if (CanDebugOverrideOutputColor(inputData, surfaceData, brdfData, debugColor))return debugColor;
				#endif
			
				// Clear-coat calculation...
				BRDFData brdfDataClearCoat = CreateClearCoatBRDFData(surfaceData, brdfData);
				half4 shadowMask = CalculateShadowMask(inputData); // Packages\com.unity.render-pipelines.universal@12.1.15\ShaderLibrary\RealtimeLights.hlsl
				AmbientOcclusionFactor aoFactor = CreateAmbientOcclusionFactor(inputData, surfaceData);
				
				#if defined(_USE_SSS_ON) && defined(_ASE_SSSCONTROL)
					half3 albedoOg = brdfData.diffuse;
					//brdfData.diffuse *= 1-_SubsurfaceDimAlbedo;
				#endif
				
				// Handle different URP versions, See ShaderLibrary/Version.hlsl
				#if UNITY_VERSION >= 202200
					uint meshRenderingLayers = GetMeshRenderingLayer();
				#else // 2021
					uint meshRenderingLayers = GetMeshRenderingLightLayer();
				#endif

				Light mainLight = GetMainLight(inputData, shadowMask, aoFactor);
			
				// NOTE: We don't apply AO to the GI here because it's done in the lighting calculation below...
				MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI);
			
				LightingData lightingData = CreateLightingData(inputData, surfaceData);

				
				#if defined(_ASE_ANIS) && defined(_USE_ANIS_ON)
					half anisotropy = AnisotropicControl.x;

					AdditionalData addData = (AdditionalData)0;

					float3 tangentWS = worldTangent;
					//if (enableNormalMapping) {   
					tangentWS = Orthonormalize(tangentWS, inputData.normalWS);
					//}
					addData.tangentWS = tangentWS;
					addData.bitangentWS = cross(inputData.normalWS, tangentWS);
					int strandirection = 0;

					half3 strandDirWS = strandirection ? addData.tangentWS : addData.bitangentWS;

					//  Set reflection normal and roughness – derived from GetGGXAnisotropicModifiedNormalAndRoughness
					//half stretch = saturate(1.5h * sqrt(brdfData.perceptualRoughness));
					//addData.anisoReflectionNormal = GetAnisotropicModifiedNormal_Lux(strandDirWS, inputData.normalWS, inputData.viewDirectionWS, stretch);
					half3 grainDirWS = (anisotropy >= 0.0) ? addData.bitangentWS : addData.tangentWS;
					half stretch = abs(anisotropy) * saturate(1.5h * sqrt(brdfData.perceptualRoughness));
					addData.anisoReflectionNormal = GetAnisotropicModifiedNormal(grainDirWS, inputData.normalWS, inputData.viewDirectionWS, stretch);
					half iblPerceptualRoughness = brdfData.perceptualRoughness * saturate(1.2 - abs(anisotropy));

					//  Overwrite perceptual roughness for ambient specular reflections
					brdfData.perceptualRoughness = iblPerceptualRoughness;

					//  GGX Aniso
					addData.roughnessT = brdfData.roughness * (1 + anisotropy);
					addData.roughnessB = brdfData.roughness * (1 - anisotropy);

					float TdotV = dot(addData.tangentWS, inputData.viewDirectionWS);
					float BdotV = dot(addData.bitangentWS, inputData.viewDirectionWS);
					float NdotV = dot(inputData.normalWS, inputData.viewDirectionWS);
					addData.partLambdaV = GetSmithJointGGXAnisoPartLambdaV(TdotV, BdotV, NdotV, addData.roughnessT, addData.roughnessB);
				#else
					AdditionalData addData = (AdditionalData)0;
					//addData.anisoReflectionNormal = float3(0,0,0);
				#endif

				// Environment lighting / skybox lighting
				//#if defined(_ASE_FRESNELCONTROL) || (defined(_ASE_ANIS) && defined(_USE_ANIS_ON))
					lightingData.giColor = GlobalIlluminationCustom(brdfData, brdfDataClearCoat, surfaceData.clearCoatMask, inputData.bakedGI, aoFactor.indirectAmbientOcclusion, inputData.positionWS,	inputData.normalWS, inputData.viewDirectionWS, fresnelControl, addData.anisoReflectionNormal, environmentReflections);
				//#else
				//	lightingData.giColor = GlobalIllumination(brdfData, brdfDataClearCoat, surfaceData.clearCoatMask, inputData.bakedGI, aoFactor.indirectAmbientOcclusion, inputData.positionWS, inputData.normalWS, inputData.viewDirectionWS);
				//#endif

				#if defined(_USE_SSS_ON) && defined(_ASE_SSSCONTROL)
					brdfData.diffuse = albedoOg * (1-_SubsurfaceDimAlbedo);
				#endif

				#if defined(_LIGHT_LAYERS)
					if (IsMatchingLightLayer(mainLight.layerMask, meshRenderingLayers))
				#endif
					{
						lightingData.mainLightColor = LightingPhysicallyBasedCustom(brdfData, addData, brdfDataClearCoat, mainLight, inputData.normalWS, inputData.viewDirectionWS, surfaceData.clearCoatMask, specularHighlightsOff);
					}
				
				#if defined(_ADDITIONAL_LIGHTS)
					uint pixelLightCount = GetAdditionalLightsCount();
					
					#if USE_CLUSTERED_LIGHTING
						for (uint lightIndex = 0; lightIndex < min(_AdditionalLightsDirectionalCount, MAX_VISIBLE_LIGHTS); lightIndex++)
						{
							Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);
							#if defined(_LIGHT_LAYERS)
								if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
							#endif
								{
									lightingData.additionalLightsColor += LightingPhysicallyBasedCustom(brdfData, addData, brdfDataClearCoat, light, inputData.normalWS, inputData.viewDirectionWS, surfaceData.clearCoatMask, specularHighlightsOff);
								}
						}
					#endif
					
					LIGHT_LOOP_BEGIN(pixelLightCount)
						Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);
						#if defined(_LIGHT_LAYERS)
							if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
						#endif
							{
								lightingData.additionalLightsColor += LightingPhysicallyBasedCustom(brdfData, addData, brdfDataClearCoat, light, inputData.normalWS, inputData.viewDirectionWS, surfaceData.clearCoatMask, specularHighlightsOff);
								
								#if defined(_USE_SSS_ON) && defined(_ASE_SSSCONTROL)
									half4 subsurfaceContributionT = LightingSubsurface(light, lerp(worldNormal, inputData.normalWS, half3(_SubsurfaceNormalStr,_SubsurfaceNormalStr,_SubsurfaceNormalStr)), _SubsurfaceColor, _SubsurfaceRadius, _SubsurfaceScattering);
									subsurfaceContributionT = saturate(subsurfaceContributionT);
									subsurfaceContribution2 += subsurfaceContributionT;
								#endif
							}
					LIGHT_LOOP_END
				#endif
			
				#if defined(_ADDITIONAL_LIGHTS_VERTEX)
					lightingData.vertexLightingColor += inputData.vertexLighting * brdfData.diffuse;
				#endif
				
				//return CalculateFinalColor(lightingData, surfaceData.alpha); // Normal

				#if defined(_USE_SSS_ON) && defined(_ASE_SSSCONTROL)
					half4 subsurfaceContribution = LightingSubsurface(mainLight, lerp(worldNormal, inputData.normalWS, half3(_SubsurfaceNormalStr,_SubsurfaceNormalStr,_SubsurfaceNormalStr)), _SubsurfaceColor, _SubsurfaceRadius, _SubsurfaceScattering);	
					//return subsurfaceContribution;
					subsurfaceContribution = saturate(subsurfaceContribution);
					half shadow = lerp(1.0,mainLight.distanceAttenuation * mainLight.shadowAttenuation,_SubsurfaceShadowMix);
					subsurfaceContribution *= shadow;

					float4 sssres = half4(CalculateLightingColorCustom(lightingData, albedoOg, subsurfaceContribution, subsurfaceContribution2), surfaceData.alpha); // SSS customised
					//sssres.rgb = ApplyTonemapAlways(sssres.rgb,_TonemappingSettings);
					return sssres;
				#else
					return half4(CalculateLightingColor(lightingData, 1), surfaceData.alpha); // Original;
				#endif
			}
			
			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
				#define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#include "Assets/AmplifyStochasticNode/StochasticSampling.cginc"
			#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_BITANGENT
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#pragma shader_feature_local _USEMOSSNOISE_ON
			#pragma shader_feature_local MODED_MOSS MODED_MOES MODED_MODS MODED_OFF
			#pragma shader_feature_local _MOSS
			#pragma shader_feature_local _AUTONORMAL
			#pragma shader_feature_local LAYER_UNLIMITED LAYER_MAX2 LAYER_MAX3
			#pragma shader_feature_local TEXTURE_MAX4 TEXTURE_MAX8 TEXTURE_MAX12 TEXTURE_MAX16
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _MOSSMETALMODE_ON
			#pragma shader_feature_local _ALPHAMODE
			#pragma shader_feature_local _SIMPLESTOCHASTIC
			#pragma shader_feature_local _USESPLAT_ON
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USE_SSS_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON
			#pragma shader_feature_local _REVEALLAYERS
			#pragma shader_feature_local _DETAIL_ON
			#pragma shader_feature _COLORMASK_ON
			#pragma shader_feature_local _REMAPPERS_ON
			#pragma shader_feature_local _USESTOCHASTICMOSS_ON
			#pragma shader_feature_local _USEMOSSFRESNEL_ON
			#pragma shader_feature_local _USECAUSTICSFROMABOVE_ON
			#pragma shader_feature_local _USECAUSTICEXTRASAMPLER_ON
			#pragma shader_feature_local _USECAUSTICRAINBOW_ON
			#pragma shader_feature_local _FadeWithHeight
			#pragma shader_feature_local _USEMOSSSLOPEDISTORT_ON
			#define _UseMossPacked
			#define _DetailMapPacking


			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE) && (SHADER_TARGET >= 45)
				#define ASE_SV_DEPTH SV_DepthLessEqual
				#define ASE_SV_POSITION_QUALIFIERS linear noperspective centroid
			#else
				#define ASE_SV_DEPTH SV_Depth
				#define ASE_SV_POSITION_QUALIFIERS
			#endif

			//#if defined(OCEANGRID_ON)
			//#include "Assets/Plugins/Crest/Crest/Shaders/ShaderLibrary/Common.hlsl"
			//#include "Assets/Plugins/Crest/Crest/Shaders/OceanVertHelpers.hlsl"

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				half4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				ASE_SV_POSITION_QUALIFIERS float4 clipPos : SV_POSITION;
				float4 clipPosV : TEXCOORD0;
				float4 lightmapUVOrVertexSH : TEXCOORD1;
				half4 fogFactorAndVertexLight : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					float4 shadowCoord : TEXCOORD6;
				#endif
				#if defined(DYNAMICLIGHTMAP_ON)
					float2 dynamicLightmapUV : TEXCOORD7;
				#endif
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				float4 ase_texcoord10 : TEXCOORD10;
				float4 ase_texcoord11 : TEXCOORD11;
				float4 ase_texcoord12 : TEXCOORD12;
				float4 ase_texcoord13 : TEXCOORD13;
				float4 ase_texcoord14 : TEXCOORD14;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Tint3;
			half4 _DetailMapScale;
			half4 _SmoothnessRemap;
			half4 _DetailMaskRemap;
			half4 _MossAlbedoRemap;
			half4 _EmissionRemap;
			half4 _MossAlphaMaskMM;
			half4 _OcclusionRemap;
			half4 _DetailSmoothnessRemap;
			half4x4 _ProbeWorldToTexture;
			half4 _MetallicRemap;
			half4 _MossColor;
			half4 _EmissionColor;
			float4 _RevealMask_TexelSize;
			half4 _MossSmoothnessRemap;
			half4 _Tint1;
			half4 _MossFresnel;
			half4 _MossFresnelColor;
			half4 _Layer3EmissionColor;
			half4 _Layer1_ST;
			half4 _LayerSurfaceExp;
			half4 _Layer0_ST;
			half4 _Tint2;
			half4 _MossSlopeNormal_ST;
			half4 _BaseMap_ST;
			half4 _BaseColor;
			half4 _MossSlopeMM;
			half4 _Tint0;
			half4 _SSSColor;
			half3 _ProbeVolumeMin;
			half3 _MossDirection;
			half3 _ProbeVolumeSizeInv;
			half _MossDetailNormalMapScale;
			half _UseEmission;
			half _Occlusion;
			half _UseOcclusion;
			half _Metallic;
			half _UseMossFresnel;
			half _Smoothness;
			half _MossMetallic;
			half _Layer1Smoothness;
			half _Layer1Metallic;
			half _SSSDimAlbedo;
			half _NormalStrength_USE_SSS;
			half _SSScattering;
			half _SSSRadius;
			half _MossNormalStrength2;
			half _MossSlopeDistort;
			half _MossSlopeNormRotate;
			half _DebugScale;
			int _DebugVisual;
			half _ShadowThreshold;
			half _AlphaClip;
			half _AlphaClipThreshold;
			half _Surface;
			half _Dither;
			half _OcclusionMossMask;
			half _Layer0Smoothness;
			half _MossSmoothness;
			half _Layer0Metallic;
			half _Layer2Height;
			int _DETAIL;
			half _Layer0NormalStrength;
			int _MetalUV;
			int _Moss;
			half _UseMossMetalMap;
			int _MossMode;
			half _MossStochasticContrast;
			half _MossStochasticScale;
			float _MossScale;
			int _MossUV;
			half _MossMultAlbedo;
			int _UseColorMask;
			int _Bitmask;
			int _ZWrite;
			int _Cullmode;
			half _Blend;
			half _SrcBlend;
			half _DstBlend;
			half _Cutoff;
			int _Int0;
			int _BlendMode_UseColorMask;
			half _Layer1NormalStrength;
			half _MossBase;
			half _MetallicSpecGlossMap;
			half _DetailNormalMapScale;
			half _MossNormalStrength;
			half _UseNormalMap;
			half _DetailsOverMoss;
			half _MapContrast;
			half _MapContrastOffset;
			half _MossNormalAffectStrength;
			half _MossNormalSubtract;
			half _DetailAlbedoMapScale;
			half _MossNormalContrast;
			half _UseMossVertexMask;
			half _UseMossMaskWithAlpha;
			half _UseMossDirection;
			half _MossDirContrast;
			half _MossLevel;
			half _DetailNormalMapScale1;
			half _UseDetailMask;
			int _MODE;
			half _NormalStrength;
			half _SSShadcowMix;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			float _DebugCounter; // Shadowood
			half _EnvironmentReflections; // Shadowood
			CBUFFER_END

			// Property used by ScenePickingPass
			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			// Properties used by SceneSelectionPass
			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			TEXTURE2D_ARRAY(_ExtraArray);
			SAMPLER(sampler_ExtraArray);
			TEXTURE2D_ARRAY(_DiffuseArray);
			SAMPLER(sampler_DiffuseArray);
			TEXTURE2D_ARRAY(_NormalArray);
			SAMPLER(sampler_NormalArray);
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_Linear_Repeat_Aniso2);
			TEXTURE2D(_ColorMask);
			TEXTURE2D(_MossPacked);
			SAMPLER(sampler_MossPacked);
			TEXTURE2D(_MossMetalMap);
			TEXTURE2D(_DetailMapPacked);
			TEXTURE2D(_MetallicGlossMap);
			TEXTURE2D(_BumpMap);
			TEXTURE2D(_Layer0);
			TEXTURE2D(_RevealMask);
			TEXTURE2D(_LayerMask);
			TEXTURE2D(_Layer1);
			TEXTURE2D(_Layer0NormalMap);
			TEXTURE2D(_Layer1NormalMap);
			half4x4 _CausticMatrix;
			SAMPLER(sampler_Linear_Repeat);
			half4 _CausticsSettings;
			int AlphaToCoverage;
			TEXTURE3D(_ProbeVolumeShR);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE3D(_ProbeVolumeShG);
			TEXTURE3D(_ProbeVolumeShB);
			TEXTURE2D(_MossSlopeNormal);
			TEXTURE3D(_ProbeVolumeOcc);


			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

			//#ifdef HAVE_VFX_MODIFICATION
			//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
			//#endif

			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			half3 VertVanisher3872( half3 vertex, half2 vtexcoord1, int bitMask )
			{
				 if(bitMask & (int)vtexcoord1.x){
					vertex *= (0.0 / 0.0);
				}
				return vertex;
			}
			
			half MyCustomExpression3902( half detailAlbedo, half scale )
			{
				return half(2.0) * detailAlbedo * scale - scale + half(1.0);
			}
			
			half3 RevealMaskNormalCrossFilter83_g4236( half2 uv, half height, half2 texelSize, SamplerState sampler_RevealMask )
			{
						//float2 texelSize = float2(1.0 / texWidth, 1.0 / texHeight);
						float4 h;
						h[0] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0, -1) * texelSize).b) * height;
						h[1] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(-1, 0) * texelSize).b) * height;
						h[2] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(1, 0) * texelSize).b) * height;
						h[3] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0, 1) * texelSize).b) * height;
						float3 n;
						n.z = 2;
						n.x = h[1] - h[2];
						n.y = h[0] - h[3];
						return normalize(n);
			}
			
			inline half3 SampleSHPixel3558( half3 SH, half3 normalWS )
			{
				return SampleSHPixel( SH, normalWS );
			}
			
			inline half3 SampleLightmap3567( half2 lightmapUV, half3 normalWS )
			{
				return SampleLightmap( lightmapUV, 0, normalWS );
			}
			
			half3 MatrixMulThatWorks( half4 Pos, half4x4 Mat )
			{
				float3 result = mul( Mat, float4(Pos.xyz,1.0) ).xyz;
				return result + float3(Mat[0][3],Mat[1][3],Mat[2][3]);
			}
			
			half ShaderAPIMobile151_g4260( half desktop, half mobile )
			{
				#if SHADER_API_MOBILE
					return desktop;
				#else
					return mobile;
				#endif
			}
			
			half4x4 Inverse4x4(half4x4 input)
			{
				#define minor(a,b,c) determinant(half3x3(input.a, input.b, input.c))
				half4x4 cofactors = half4x4(
				minor( _22_23_24, _32_33_34, _42_43_44 ),
				-minor( _21_23_24, _31_33_34, _41_43_44 ),
				minor( _21_22_24, _31_32_34, _41_42_44 ),
				-minor( _21_22_23, _31_32_33, _41_42_43 ),
			
				-minor( _12_13_14, _32_33_34, _42_43_44 ),
				minor( _11_13_14, _31_33_34, _41_43_44 ),
				-minor( _11_12_14, _31_32_34, _41_42_44 ),
				minor( _11_12_13, _31_32_33, _41_42_43 ),
			
				minor( _12_13_14, _22_23_24, _42_43_44 ),
				-minor( _11_13_14, _21_23_24, _41_43_44 ),
				minor( _11_12_14, _21_22_24, _41_42_44 ),
				-minor( _11_12_13, _21_22_23, _41_42_43 ),
			
				-minor( _12_13_14, _22_23_24, _32_33_34 ),
				minor( _11_13_14, _21_23_24, _31_33_34 ),
				-minor( _11_12_14, _21_22_24, _31_32_34 ),
				minor( _11_12_13, _21_22_23, _31_32_33 ));
				#undef minor
				return transpose( cofactors ) / determinant( input );
			}
			
			half ShaderAPIMobile153_g4260( half desktop, half mobile )
			{
				#if SHADER_API_MOBILE
					return desktop;
				#else
					return mobile;
				#endif
			}
			
			inline half4 GetUnderWaterFogs240_g4251( half3 viewDir, float3 camWorldPos, half3 posWS, half4 oceanFogDensities, half oceanHeight, half4 oceanFogTop_RGB_Exponent, half4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			
			inline float Dither4x4Bayer( int x, int y )
			{
				const float dither[ 16 ] = {
			 1,  9,  3, 11,
			13,  5, 15,  7,
			 4, 12,  2, 10,
			16,  8, 14,  6 };
				int r = y * 4 + x;
				return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
			}
			
			half4x4 RotationOnly90_g4351( half4x4 Input )
			{
				float4x4 Output = float4x4(
				        float4(Input[0].xyz, 0.0),
				        float4(Input[1].xyz, 0.0),
				        float4(Input[2].xyz, 0.0),
				        float4(0.0,0.0,0.0, 1.0)
				    );
				return Output;
			}
			
			half3 MyCustomExpression6_g4351( half3 vertexPos, half4x4 ProbeWorldToTexture, half3 ProbeVolumeMin, half3 ProbeVolumeSizeInv )
			{
				float3 position = mul(ProbeWorldToTexture, float4(TransformObjectToWorld(vertexPos.xyz), 1.0f)).xyz;
				float3 texCoord = (position - ProbeVolumeMin.xyz) * ProbeVolumeSizeInv;
				return texCoord;
			}
			
			half3 SHEvalLinearL0L114_g4351( half3 worldNormal, half4 ProbeVolumeShR, half4 ProbeVolumeShG, half4 ProbeVolumeShB )
			{
				// See: Library\PackageCache\com.unity.render-pipelines.core12.1.15\ShaderLibrary\EntityLighting.hlsl
				return SHEvalLinearL0L1(worldNormal, ProbeVolumeShR, ProbeVolumeShG,  ProbeVolumeShB);
			}
			
			
			float4 SampleGradient( Gradient gradient, float time )
			{
				float3 color = gradient.colors[0].rgb;
				UNITY_UNROLL
				for (int c = 1; c < 8; c++)
				{
				float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, gradient.colorsLength-1));
				color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
				}
				#ifndef UNITY_COLORSPACE_GAMMA
				color = SRGBToLinear(color);
				#endif
				float alpha = gradient.alphas[0].x;
				UNITY_UNROLL
				for (int a = 1; a < 8; a++)
				{
				float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, gradient.alphasLength-1));
				alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
				}
				return float4(color, alpha);
			}
			
			half4 CalculateShadowMask1_g4353( half2 LightmapUV )
			{
				#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
				return SAMPLE_SHADOWMASK( LightmapUV.xy );
				#elif !defined (LIGHTMAP_ON)
				return unity_ProbesOcclusion;
				#else
				return half4( 1, 1, 1, 1 );
				#endif
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				half3 vertex3872 = v.vertex.xyz;
				half2 vtexcoord13872 = v.texcoord1.xy;
				int bitMask3872 = _Bitmask;
				half3 localVertVanisher3872 = VertVanisher3872( vertex3872 , vtexcoord13872 , bitMask3872 );
				half3 VertexOcclusionPosition3873 = localVertVanisher3872;
				
				half4 ifLocalVars3708 = 0;
				half2 texCoord117 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==0){ifLocalVars3708 = half4( texCoord117, 0.0 , 0.0 ); };
				half2 texCoord3710 = v.texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==1){ifLocalVars3708 = half4( texCoord3710, 0.0 , 0.0 ); };
				half2 texCoord3712 = v.texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==2){ifLocalVars3708 = half4( texCoord3712, 0.0 , 0.0 ); };
				half2 appendResult3711 = (half2(ifLocalVars3708.xy));
				half2 vertexToFrag3370 = ( appendResult3711 * ( 1.0 / _MossScale ) );
				o.ase_texcoord9.xy = vertexToFrag3370;
				int MossInt2938 = _Moss;
				int MossModeInt3200 = _MossMode;
				#ifdef _MOSSMETALMODE_ON
				half staticSwitch3230 = (float)( MossInt2938 * MossModeInt3200 );
				#else
				half staticSwitch3230 = (float)MossInt2938;
				#endif
				half2 MossUVScaled1663 = vertexToFrag3370;
				half4 ifLocalVars3717 = 0;
				half2 uv_BaseMap = v.texcoord.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==0){ifLocalVars3717 = half4( uv_BaseMap, 0.0 , 0.0 ); };
				half2 uv2_BaseMap = v.texcoord1.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==1){ifLocalVars3717 = half4( uv2_BaseMap, 0.0 , 0.0 ); };
				half2 uv3_BaseMap = v.texcoord2.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==2){ifLocalVars3717 = half4( uv3_BaseMap, 0.0 , 0.0 ); };
				half2 appendResult3718 = (half2(ifLocalVars3717.xy));
				half2 vertexToFrag3723 = appendResult3718;
				half2 UVBasemapPicked3719 = vertexToFrag3723;
				half2 ifLocalVar2916 = 0;
				if( staticSwitch3230 <= 0.0 )
				ifLocalVar2916 = UVBasemapPicked3719;
				else
				ifLocalVar2916 = MossUVScaled1663;
				half2 vertexToFrag3371 = ifLocalVar2916;
				o.ase_texcoord9.zw = vertexToFrag3371;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				half4 _DetailDistanceHardcoded = half4(25,30,0,0);
				half temp_output_3155_0 = saturate( (1.0 + (distance( _WorldSpaceCameraPos , ase_worldPos ) - _DetailDistanceHardcoded.x) * (0.0 - 1.0) / (_DetailDistanceHardcoded.y - _DetailDistanceHardcoded.x)) );
				half vertexToFrag3157 = temp_output_3155_0;
				o.ase_texcoord8.w = vertexToFrag3157;
				half2 texCoord1007 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				half2 appendResult2490 = (half2(_DetailMapScale.x , _DetailMapScale.y));
				half2 appendResult2491 = (half2(_DetailMapScale.z , _DetailMapScale.w));
				half2 vertexToFrag3400 = (texCoord1007*appendResult2490 + appendResult2491);
				o.ase_texcoord10.xy = vertexToFrag3400;
				o.ase_texcoord10.zw = vertexToFrag3723;
				half temp_output_225_0 = ( ( v.ase_color.r - 0.5 ) * 2.0 );
				half ifLocalVar4428 = 0;
				if( _UseMossVertexMask > 0.0 )
				ifLocalVar4428 = temp_output_225_0;
				half vertexToFrag3925 = ifLocalVar4428;
				o.ase_texcoord11.x = vertexToFrag3925;
				
				half3 _Vector3 = half3(0,0,-1);
				half4 Pos6_g4262 = half4( _Vector3 , 0.0 );
				half4x4 Mat6_g4262 = _CausticMatrix;
				half3 localMatrixMulThatWorks6_g4262 = MatrixMulThatWorks( Pos6_g4262 , Mat6_g4262 );
				half3 normalizeResult147_g4260 = normalize( localMatrixMulThatWorks6_g4262 );
				half3 vertexToFrag144_g4260 = normalizeResult147_g4260;
				o.ase_texcoord11.yzw = vertexToFrag144_g4260;
				half3 WorldPosition256_g4237 = ase_worldPos;
				half3 temp_output_105_0_g4260 = WorldPosition256_g4237;
				half4 Pos6_g4261 = half4( temp_output_105_0_g4260 , 0.0 );
				half4x4 invertVal146_g4260 = Inverse4x4( _CausticMatrix );
				half4x4 Mat6_g4261 = invertVal146_g4260;
				half3 localMatrixMulThatWorks6_g4261 = MatrixMulThatWorks( Pos6_g4261 , Mat6_g4261 );
				half2 vertexToFrag52_g4260 = (localMatrixMulThatWorks6_g4261).xy;
				o.ase_texcoord12.xy = vertexToFrag52_g4260;
				
				half3 vertexPos6_g4351 = v.vertex.xyz;
				half4x4 temp_output_28_0_g4351 = _ProbeWorldToTexture;
				half4x4 ProbeWorldToTexture6_g4351 = temp_output_28_0_g4351;
				half3 ProbeVolumeMin6_g4351 = _ProbeVolumeMin;
				half3 ProbeVolumeSizeInv6_g4351 = _ProbeVolumeSizeInv;
				half3 localMyCustomExpression6_g4351 = MyCustomExpression6_g4351( vertexPos6_g4351 , ProbeWorldToTexture6_g4351 , ProbeVolumeMin6_g4351 , ProbeVolumeSizeInv6_g4351 );
				half3 vertexToFrag7_g4351 = localMyCustomExpression6_g4351;
				o.ase_texcoord13.xyz = vertexToFrag7_g4351;
				
				half2 texCoord861 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float cos916 = cos( radians( _MossSlopeNormRotate ) );
				float sin916 = sin( radians( _MossSlopeNormRotate ) );
				half2 rotator916 = mul( texCoord861 - float2( 0.5,0.51 ) , float2x2( cos916 , -sin916 , sin916 , cos916 )) + float2( 0.5,0.51 );
				half2 vertexToFrag1159 = (rotator916*_MossSlopeNormal_ST.xy + _MossSlopeNormal_ST.zw);
				o.ase_texcoord12.zw = vertexToFrag1159;
				
				o.ase_texcoord8.xyz = v.texcoord.xyz;
				o.ase_texcoord14.xy = v.texcoord1.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord13.w = 0;
				o.ase_texcoord14.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOcclusionPosition3873;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				// Shadowood: from Crest Ocean.shader for infinite mesh grid
				#if defined(OCEANGRID_ON)
					// Ocean data
					const CascadeParams cascadeData0 = _CrestCascadeData[_LD_SliceIndex];
					const PerCascadeInstanceData instanceData = _CrestPerCascadeInstanceData[_LD_SliceIndex];
					
					// Vertex snapping and lod transition
					float lodAlpha;
					const float meshScaleLerp = instanceData._meshScaleLerp;
					const float gridSize = instanceData._geoGridWidth;
					SnapAndTransitionVertLayout(meshScaleLerp, cascadeData0, gridSize, positionWS.xyz, lodAlpha);
				
					{
						// Scale up by small "epsilon" to solve numerical issues. Expand slightly about tile center.
						// :OceanGridPrecisionErrors
						const float2 tileCenterXZ = UNITY_MATRIX_M._m03_m23;
						const float2 cameraPositionXZ = abs(_WorldSpaceCameraPos.xz);
						// Scale "epsilon" by distance from zero. There is an issue where overlaps can cause SV_IsFrontFace
						// to be flipped (needs to be investigated). Gaps look bad from above surface, and overlaps look bad
						// from below surface. We want to close gaps without introducing overlaps. A fixed "epsilon" will
						// either not solve gaps at large distances or introduce too many overlaps at small distances. Even
						// with scaling, there are still unsolvable overlaps underwater (especially at large distances).
						// 100,000 (0.00001) is the maximum position before Unity warns the user of precision issues.
						positionWS.xz = lerp(tileCenterXZ, positionWS.xz, lerp(1.0, 1.01, max(cameraPositionXZ.x, cameraPositionXZ.y) * 0.00001));
					}
				#endif
				
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				#if defined(OCEANGRID_ON)
					normalInput.normalWS = float3(0,1,0);
				#endif
				
				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				#if defined(LIGHTMAP_ON)
					OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				#endif

				#if !defined(LIGHTMAP_ON)
					OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );
				#endif

				#if defined(DYNAMICLIGHTMAP_ON)
					o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord.xy;
					o.lightmapUVOrVertexSH.xy = v.texcoord.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );

				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif

				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				o.clipPosV = positionCS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				half4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif

				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				float4 ClipPos = IN.clipPosV;
				float4 ScreenPos = ComputeScreenPos( IN.clipPosV );

				float2 NormalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.clipPos);

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif

				WorldViewDirection = SafeNormalize( WorldViewDirection );

				half2 uv_BaseMap = IN.ase_texcoord8.xyz.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				half2 UVBasemap02088 = uv_BaseMap;
				half4 tex2DNode80 = SAMPLE_TEXTURE2D( _BaseMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half3 temp_output_82_0 = ( (tex2DNode80).rgb * (_BaseColor).rgb );
				half3 temp_output_17_0_g2518 = temp_output_82_0;
				half4 ifLocalVars72_g2518 = 0;
				int clampResult74_g2518 = clamp( _BlendMode_UseColorMask , 0 , 1 );
				half4 tex2DNode1_g2518 = SAMPLE_TEXTURE2D( _ColorMask, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half temp_output_2_0_g2518 = ( 1.0 - tex2DNode1_g2518.a );
				half3 blendOpSrc26_g2518 = temp_output_17_0_g2518;
				half3 blendOpDest26_g2518 = ( ( (_Tint0).rgb * tex2DNode1_g2518.r * 1.0 ) + ( (_Tint1).rgb * tex2DNode1_g2518.g * 1.0 ) + ( (_Tint2).rgb * tex2DNode1_g2518.b * 1.0 ) + ( (_Tint3).rgb * temp_output_2_0_g2518 * 1.0 ) );
				half3 temp_output_26_0_g2518 = ( saturate( ( blendOpSrc26_g2518 * blendOpDest26_g2518 ) ));
				if(clampResult74_g2518==0){ifLocalVars72_g2518 = half4( temp_output_26_0_g2518 , 0.0 ); };
				half4 ifLocalVars29_g2518 = 0;
				if(_BlendMode_UseColorMask==0){ifLocalVars29_g2518 = half4( half3(0,0,0) , 0.0 ); };
				if(_BlendMode_UseColorMask==1){ifLocalVars29_g2518 = half4( temp_output_26_0_g2518 , 0.0 ); };
				half3 blendOpSrc27_g2518 = temp_output_17_0_g2518;
				half3 blendOpDest27_g2518 = ( ( (_Tint0).rgb * tex2DNode1_g2518.r * 1.0 ) + ( (_Tint1).rgb * tex2DNode1_g2518.g * 1.0 ) + ( (_Tint2).rgb * tex2DNode1_g2518.b * 1.0 ) + ( (_Tint3).rgb * temp_output_2_0_g2518 * 1.0 ) );
				if(_BlendMode_UseColorMask==2){ifLocalVars29_g2518 = half4( ( saturate( ( 1.0 - ( 1.0 - blendOpSrc27_g2518 ) * ( 1.0 - blendOpDest27_g2518 ) ) )) , 0.0 ); };
				half3 blendOpSrc28_g2518 = temp_output_17_0_g2518;
				half3 blendOpDest28_g2518 = ( ( (_Tint0).rgb * tex2DNode1_g2518.r * 1.0 ) + ( (_Tint1).rgb * tex2DNode1_g2518.g * 1.0 ) + ( (_Tint2).rgb * tex2DNode1_g2518.b * 1.0 ) + ( (_Tint3).rgb * temp_output_2_0_g2518 * 1.0 ) );
				if(_BlendMode_UseColorMask==3){ifLocalVars29_g2518 = half4( ( saturate( (( blendOpDest28_g2518 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest28_g2518 ) * ( 1.0 - blendOpSrc28_g2518 ) ) : ( 2.0 * blendOpDest28_g2518 * blendOpSrc28_g2518 ) ) )) , 0.0 ); };
				if(clampResult74_g2518==1){ifLocalVars72_g2518 = half4( (ifLocalVars29_g2518).xyz , 0.0 ); };
				half3 lerpResult64_g2518 = lerp( temp_output_17_0_g2518 , (ifLocalVars72_g2518).xyz , ( saturate( ( ( 1.0 * tex2DNode1_g2518.r ) + ( 1.0 * tex2DNode1_g2518.g ) + ( 1.0 * tex2DNode1_g2518.b ) + ( 1.0 * temp_output_2_0_g2518 ) ) ) * _UseColorMask ));
				#ifdef _COLORMASK_ON
				half3 staticSwitch15_g2518 = lerpResult64_g2518;
				#else
				half3 staticSwitch15_g2518 = temp_output_17_0_g2518;
				#endif
				half3 AlbedoColorMask2007 = staticSwitch15_g2518;
				half3 AlbedoBaseColor92 = AlbedoColorMask2007;
				half3 appendResult3785 = (half3(_MossColor.rgb));
				half2 vertexToFrag3370 = IN.ase_texcoord9.xy;
				half2 MossUVScaled1663 = vertexToFrag3370;
				half StochasticScale1447 = ( 1.0 / _MossStochasticScale );
				half StochasticContrast1448 = _MossStochasticContrast;
				half3 cw4270 = 0;
				float2 uv14270 = 0;
				float2 uv24270 = 0;
				float2 uv34270 = 0;
				float2 dx4270 = 0;
				float2 dy4270 = 0;
				half4 stochasticSample4270 = StochasticSample2DWeightsR(_MossPacked,sampler_MossPacked,MossUVScaled1663,cw4270,uv14270,uv24270,uv34270,dx4270,dy4270,StochasticScale1447,StochasticContrast1448);
				#ifdef _USESTOCHASTICMOSS_ON
				half4 staticSwitch121 = stochasticSample4270;
				#else
				half4 staticSwitch121 = SAMPLE_TEXTURE2D( _MossPacked, sampler_Linear_Repeat_Aniso2, MossUVScaled1663 );
				#endif
				int MossModeInt3200 = _MossMode;
				half2 vertexToFrag3371 = IN.ase_texcoord9.zw;
				half4 _Vector17 = half4(1,0,1,0);
				half4 ifLocalVar3181 = 0;
				if( _UseMossMetalMap <= 0.0 )
				ifLocalVar3181 = _Vector17;
				else
				ifLocalVar3181 = SAMPLE_TEXTURE2D( _MossMetalMap, sampler_Linear_Repeat_Aniso2, vertexToFrag3371 );
				half4 ifLocalVar3232 = 0;
				if( MossModeInt3200 <= 0.0 )
				ifLocalVar3232 = _Vector17;
				else
				ifLocalVar3232 = ifLocalVar3181;
				half4 MossMetalMap2943 = ifLocalVar3232;
				#ifdef _MOSSMETALMODE_ON
				half4 staticSwitch3303 = MossMetalMap2943;
				#else
				half4 staticSwitch3303 = staticSwitch121;
				#endif
				half4 MossMetalRes3077 = staticSwitch3303;
				half temp_output_3043_0 = (MossMetalRes3077).r;
				half temp_output_8_0_g2546 = temp_output_3043_0;
				half4 break5_g2546 = _MossAlbedoRemap;
				half temp_output_3_0_g2546 = (0.0 + (temp_output_8_0_g2546 - break5_g2546.x) * (1.0 - 0.0) / (break5_g2546.y - break5_g2546.x));
				half clampResult2_g2546 = clamp( temp_output_3_0_g2546 , break5_g2546.z , break5_g2546.w );
				half temp_output_27_0_g2546 = saturate( clampResult2_g2546 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3747 = temp_output_27_0_g2546;
				#else
				half staticSwitch3747 = temp_output_3043_0;
				#endif
				half3 MossPackedAlbedo1643 = ( appendResult3785 * staticSwitch3747 );
				half3 lerpResult2137 = lerp( MossPackedAlbedo1643 , ( MossPackedAlbedo1643 * AlbedoBaseColor92 ) , _MossMultAlbedo);
				half3 MossAlbedoTinted101 = lerpResult2137;
				half2 _Vector5 = half2(0,0);
				half2 NormalMossSigned50 = _Vector5;
				half3 appendResult3794 = (half3(NormalMossSigned50 , 1.0));
				half4 appendResult3336 = (half4(1.0 , 0.0 , 0.0 , 0.0));
				half vertexToFrag3157 = IN.ase_texcoord8.w;
				half DDistanceMaskVert3160 = step( 0.01 , vertexToFrag3157 );
				half2 vertexToFrag3400 = IN.ase_texcoord10.xy;
				half4 tex2DNode980 = SAMPLE_TEXTURE2D( _DetailMapPacked, sampler_Linear_Repeat_Aniso2, vertexToFrag3400 );
				half4 DetailMapPacked3088 = tex2DNode980;
				half temp_output_3090_0 = (DetailMapPacked3088).r;
				half detailAlbedo3902 = temp_output_3090_0;
				half scale3902 = _DetailAlbedoMapScale;
				half localMyCustomExpression3902 = MyCustomExpression3902( detailAlbedo3902 , scale3902 );
				half2 vertexToFrag3723 = IN.ase_texcoord10.zw;
				half2 UVBasemapPicked3719 = vertexToFrag3723;
				half4 _Vector16 = half4(0,1,1,1);
				half4 ifLocalVar3177 = 0;
				if( _MetallicSpecGlossMap <= 0.0 )
				ifLocalVar3177 = _Vector16;
				else
				ifLocalVar3177 = SAMPLE_TEXTURE2D( _MetallicGlossMap, sampler_Linear_Repeat_Aniso2, UVBasemapPicked3719 );
				half4 ifLocalVar3189 = 0;
				if( MossModeInt3200 <= 0.0 )
				ifLocalVar3189 = ifLocalVar3181;
				else
				ifLocalVar3189 = half4(1,1,1,1);
				half4 MossMetal2928 = ifLocalVar3189;
				#ifdef _MOSSMETALMODE_ON
				half4 staticSwitch2913 = MossMetal2928;
				#else
				half4 staticSwitch2913 = ifLocalVar3177;
				#endif
				half4 MossMetalORMetallicMap2911 = staticSwitch2913;
				half temp_output_3050_0 = (MossMetalORMetallicMap2911).b;
				half temp_output_8_0_g2548 = temp_output_3050_0;
				half4 ifLocalVars3100 = 0;
				int Mode3840 = _MODE;
				if(Mode3840==0){ifLocalVars3100 = _DetailMaskRemap; };
				if(Mode3840==1){ifLocalVars3100 = _EmissionRemap; };
				half4 break5_g2548 = ifLocalVars3100;
				half temp_output_3_0_g2548 = (0.0 + (temp_output_8_0_g2548 - break5_g2548.x) * (1.0 - 0.0) / (break5_g2548.y - break5_g2548.x));
				half clampResult2_g2548 = clamp( temp_output_3_0_g2548 , break5_g2548.z , break5_g2548.w );
				half temp_output_27_0_g2548 = saturate( clampResult2_g2548 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3738 = temp_output_27_0_g2548;
				#else
				half staticSwitch3738 = temp_output_3050_0;
				#endif
				int temp_output_3109_0 = ( 1.0 - Mode3840 );
				half temp_output_12_0_g2513 = ( temp_output_3109_0 * _UseDetailMask );
				half DetalMaskDoodad3103 = ( ( staticSwitch3738 * temp_output_12_0_g2513 ) + ( 1.0 - temp_output_12_0_g2513 ) );
				half temp_output_12_0_g2557 = DetalMaskDoodad3103;
				half temp_output_3327_0 = (DetailMapPacked3088).b;
				half temp_output_8_0_g2534 = temp_output_3327_0;
				half4 break5_g2534 = _DetailSmoothnessRemap;
				half temp_output_32_0_g2534 = (break5_g2534.z + (temp_output_8_0_g2534 - break5_g2534.x) * (break5_g2534.w - break5_g2534.z) / (break5_g2534.y - break5_g2534.x));
				#ifdef _REMAPPERS_ON
				half staticSwitch3743 = temp_output_32_0_g2534;
				#else
				half staticSwitch3743 = (-1.0 + (temp_output_3327_0 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0));
				#endif
				half4 appendResult3337 = (half4(( ( localMyCustomExpression3902 * temp_output_12_0_g2557 ) + ( 1.0 - temp_output_12_0_g2557 ) ) , ( ( (DetailMapPacked3088).ag * float2( 2,2 ) ) - float2( 1,1 ) ) , staticSwitch3743));
				half4 _DetailDistanceHardcoded = half4(25,30,0,0);
				half temp_output_3155_0 = saturate( (1.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - _DetailDistanceHardcoded.x) * (0.0 - 1.0) / (_DetailDistanceHardcoded.y - _DetailDistanceHardcoded.x)) );
				half DDistanceMaskPixel3159 = temp_output_3155_0;
				half4 lerpResult3339 = lerp( appendResult3336 , appendResult3337 , DDistanceMaskPixel3159);
				half4 ifLocalVar3338 = 0;
				UNITY_BRANCH 
				if( DDistanceMaskVert3160 <= 0.0 )
				ifLocalVar3338 = appendResult3336;
				else
				ifLocalVar3338 = lerpResult3339;
				#ifdef _DETAIL_ON
				half4 staticSwitch4130 = ifLocalVar3338;
				#else
				half4 staticSwitch4130 = appendResult3336;
				#endif
				half2 DetailNormalSigned3509 = ( (staticSwitch4130).yz * _DetailNormalMapScale1 );
				half3 appendResult3801 = (half3(( NormalMossSigned50 + DetailNormalSigned3509 ) , 1.0));
				half3 normalizeResult3795 = normalize( appendResult3801 );
				#ifdef _DETAIL_ON
				half3 staticSwitch4129 = normalizeResult3795;
				#else
				half3 staticSwitch4129 = appendResult3794;
				#endif
				half3 tanToWorld0 = float3( WorldTangent.x, WorldBiTangent.x, WorldNormal.x );
				half3 tanToWorld1 = float3( WorldTangent.y, WorldBiTangent.y, WorldNormal.y );
				half3 tanToWorld2 = float3( WorldTangent.z, WorldBiTangent.z, WorldNormal.z );
				float3 tanNormal16 = staticSwitch4129;
				half3 worldNormal16 = float3(dot(tanToWorld0,tanNormal16), dot(tanToWorld1,tanNormal16), dot(tanToWorld2,tanNormal16));
				half dotResult15 = dot( worldNormal16 , _MossDirection );
				half temp_output_13_0_g2463 = dotResult15;
				half temp_output_82_0_g2463 = ( temp_output_13_0_g2463 * 0.5 );
				half temp_output_5_0_g2467 = ( temp_output_82_0_g2463 - -0.5 );
				half temp_output_48_0_g2467 = saturate( temp_output_5_0_g2467 );
				half temp_output_14_0_g2463 = _MossLevel;
				half temp_output_17_0_g2467 = temp_output_14_0_g2463;
				half temp_output_16_0_g2463 = ( 1.0 - _MossDirContrast );
				half temp_output_30_0_g2467 = ( 1.0 - saturate( ( 1.0 - temp_output_16_0_g2463 ) ) );
				half temp_output_35_0_g2467 = ( temp_output_30_0_g2467 * 0.5 );
				half temp_output_32_0_g2467 = ( (( 1.0 + temp_output_30_0_g2467 ) + (abs( temp_output_17_0_g2467 ) - 0.0) * (( 0.0 - temp_output_30_0_g2467 ) - ( 1.0 + temp_output_30_0_g2467 )) / (1.0 - 0.0)) - temp_output_35_0_g2467 );
				half temp_output_31_0_g2467 = ( temp_output_35_0_g2467 + (( 1.0 + temp_output_30_0_g2467 ) + (abs( temp_output_17_0_g2467 ) - 0.0) * (( 0.0 - temp_output_30_0_g2467 ) - ( 1.0 + temp_output_30_0_g2467 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2467 = saturate( (0.0 + (temp_output_48_0_g2467 - temp_output_32_0_g2467) * (1.0 - 0.0) / (temp_output_31_0_g2467 - temp_output_32_0_g2467)) );
				half temp_output_3796_0 = temp_output_51_0_g2467;
				half lerpResult4109 = lerp( _MossBase , temp_output_3796_0 , _UseMossDirection);
				half MossMaskDirection1179 = lerpResult4109;
				half temp_output_1811_0 = saturate( MossMaskDirection1179 );
				half temp_output_3952_0 = ( tex2DNode80.a * _BaseColor.a );
				#ifdef _ALPHATEST_ON
				half staticSwitch3756 = 1.0;
				#else
				half staticSwitch3756 = temp_output_3952_0;
				#endif
				half AlbedoAlphaMoss1953 = staticSwitch3756;
				half temp_output_8_0_g4229 = AlbedoAlphaMoss1953;
				half4 break5_g4229 = _MossAlphaMaskMM;
				half temp_output_32_0_g4229 = (break5_g4229.z + (temp_output_8_0_g4229 - break5_g4229.x) * (break5_g4229.w - break5_g4229.z) / (break5_g4229.y - break5_g4229.x));
				half clampResult31_g4229 = clamp( temp_output_32_0_g4229 , break5_g4229.z , break5_g4229.w );
				half temp_output_3896_0 = clampResult31_g4229;
				#ifdef _REMAPPERS_ON
				half staticSwitch3745 = temp_output_3896_0;
				#else
				half staticSwitch3745 = temp_output_3896_0;
				#endif
				half MossMaskAlpha1182 = ( staticSwitch3745 * _UseMossMaskWithAlpha );
				#ifdef _ALPHATEST_ON
				half staticSwitch3938 = temp_output_1811_0;
				#else
				half staticSwitch3938 = saturate( ( temp_output_1811_0 + MossMaskAlpha1182 ) );
				#endif
				half vertexToFrag3925 = IN.ase_texcoord11.x;
				half MossMaskVertex1181 = vertexToFrag3925;
				half4 tex2DNode32 = SAMPLE_TEXTURE2D( _BumpMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half3 unpack2617 = UnpackNormalScale( tex2DNode32, _NormalStrength );
				unpack2617.z = lerp( 1, unpack2617.z, saturate(_NormalStrength) );
				half2 break3507 = DetailNormalSigned3509;
				half dotResult3505 = dot( break3507.x , break3507.y );
				#ifdef _DETAIL_ON
				half staticSwitch3500 = ( unpack2617.z + abs( dotResult3505 ) );
				#else
				half staticSwitch3500 = unpack2617.z;
				#endif
				half NormalHeightSigned1759 = staticSwitch3500;
				half temp_output_5_0_g2451 = NormalHeightSigned1759;
				half temp_output_48_0_g2451 = saturate( temp_output_5_0_g2451 );
				half temp_output_17_0_g2451 = saturate( ( staticSwitch3938 + MossMaskVertex1181 ) );
				half temp_output_30_0_g2451 = ( 1.0 - saturate( _MossNormalContrast ) );
				half temp_output_35_0_g2451 = ( temp_output_30_0_g2451 * 0.5 );
				half temp_output_32_0_g2451 = ( (( 1.0 + temp_output_30_0_g2451 ) + (abs( temp_output_17_0_g2451 ) - 0.0) * (( 0.0 - temp_output_30_0_g2451 ) - ( 1.0 + temp_output_30_0_g2451 )) / (1.0 - 0.0)) - temp_output_35_0_g2451 );
				half temp_output_31_0_g2451 = ( temp_output_35_0_g2451 + (( 1.0 + temp_output_30_0_g2451 ) + (abs( temp_output_17_0_g2451 ) - 0.0) * (( 0.0 - temp_output_30_0_g2451 ) - ( 1.0 + temp_output_30_0_g2451 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2451 = saturate( (0.0 + (temp_output_48_0_g2451 - temp_output_32_0_g2451) * (1.0 - 0.0) / (temp_output_31_0_g2451 - temp_output_32_0_g2451)) );
				half temp_output_137_0_g2451 = ( 1.0 - temp_output_48_0_g2451 );
				half temp_output_128_0_g2451 = ( 1.0 - saturate( _MossNormalContrast ) );
				half temp_output_121_0_g2451 = ( temp_output_128_0_g2451 * 0.5 );
				half temp_output_118_0_g2451 = (0.0 + (temp_output_137_0_g2451 - ( (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) - temp_output_121_0_g2451 )) * (1.0 - 0.0) / (( temp_output_121_0_g2451 + (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) ) - ( (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) - temp_output_121_0_g2451 )));
				half lerpResult2905 = lerp( saturate( ( staticSwitch3938 + MossMaskVertex1181 ) ) , ( temp_output_51_0_g2451 - saturate( temp_output_118_0_g2451 ) ) , _MossNormalAffectStrength);
				half temp_output_5_0_g2498 = lerpResult2905;
				half temp_output_48_0_g2498 = saturate( temp_output_5_0_g2498 );
				half temp_output_17_0_g2498 = _MapContrastOffset;
				half temp_output_30_0_g2498 = ( 1.0 - saturate( _MapContrast ) );
				half temp_output_35_0_g2498 = ( temp_output_30_0_g2498 * 0.5 );
				half temp_output_32_0_g2498 = ( (( 1.0 + temp_output_30_0_g2498 ) + (abs( temp_output_17_0_g2498 ) - 0.0) * (( 0.0 - temp_output_30_0_g2498 ) - ( 1.0 + temp_output_30_0_g2498 )) / (1.0 - 0.0)) - temp_output_35_0_g2498 );
				half temp_output_31_0_g2498 = ( temp_output_35_0_g2498 + (( 1.0 + temp_output_30_0_g2498 ) + (abs( temp_output_17_0_g2498 ) - 0.0) * (( 0.0 - temp_output_30_0_g2498 ) - ( 1.0 + temp_output_30_0_g2498 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2498 = saturate( (0.0 + (temp_output_48_0_g2498 - temp_output_32_0_g2498) * (1.0 - 0.0) / (temp_output_31_0_g2498 - temp_output_32_0_g2498)) );
				#ifdef _MOSS
				half staticSwitch14 = temp_output_51_0_g2498;
				#else
				half staticSwitch14 = 0.0;
				#endif
				half MossMask45 = staticSwitch14;
				half3 lerpResult103 = lerp( AlbedoBaseColor92 , MossAlbedoTinted101 , MossMask45);
				#ifdef _MOSS
				half3 staticSwitch1236 = lerpResult103;
				#else
				half3 staticSwitch1236 = AlbedoBaseColor92;
				#endif
				half temp_output_10_0_g2209 = _DetailsOverMoss;
				half temp_output_17_0_g2209 = saturate( MossMask45 );
				half4 lerpResult7_g2209 = lerp( appendResult3336 , ifLocalVar3338 , saturate( ( saturate( ( abs( temp_output_10_0_g2209 ) - 1.0 ) ) + ( ( ( 1.0 - temp_output_17_0_g2209 ) * abs( min( temp_output_10_0_g2209 , 0.0 ) ) ) + ( temp_output_17_0_g2209 * max( temp_output_10_0_g2209 , 0.0 ) ) ) ) ));
				#ifdef _MOSS
				half4 staticSwitch4126 = lerpResult7_g2209;
				#else
				half4 staticSwitch4126 = ifLocalVar3338;
				#endif
				half4 lerpResult4232 = lerp( appendResult3336 , staticSwitch4126 , (float)_DETAIL);
				#ifdef _DETAIL_ON
				half4 staticSwitch4132 = lerpResult4232;
				#else
				half4 staticSwitch4132 = appendResult3336;
				#endif
				half DetailMapAlbedo987 = (staticSwitch4132).x;
				#ifdef _DETAIL_ON
				half3 staticSwitch1045 = saturate( ( staticSwitch1236 * DetailMapAlbedo987 ) );
				#else
				half3 staticSwitch1045 = staticSwitch1236;
				#endif
				half3 AlbedoRes106 = staticSwitch1045;
				half3 temp_output_35_0_g4236 = AlbedoRes106;
				half2 uv_Layer0 = IN.ase_texcoord8.xyz.xy * _Layer0_ST.xy + _Layer0_ST.zw;
				half2 uvLayer0112_g4236 = uv_Layer0;
				half3 appendResult37_g4236 = (half3(SAMPLE_TEXTURE2D( _Layer0, sampler_Linear_Repeat_Aniso2, uvLayer0112_g4236 ).rgb));
				half2 texCoord24_g4236 = IN.ase_texcoord8.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				half2 baseuv25_g4236 = texCoord24_g4236;
				half4 tex2DNode3_g4236 = SAMPLE_TEXTURE2D( _RevealMask, sampler_Linear_Repeat_Aniso2, baseuv25_g4236 );
				half4 tex2DNode4_g4236 = SAMPLE_TEXTURE2D( _LayerMask, sampler_Linear_Repeat_Aniso2, baseuv25_g4236 );
				half rt31_g4236 = saturate( ( tex2DNode3_g4236.r * tex2DNode4_g4236.r ) );
				half LSAlbedo100_g4236 = _LayerSurfaceExp.x;
				half3 lerpResult34_g4236 = lerp( temp_output_35_0_g4236 , appendResult37_g4236 , pow( rt31_g4236 , LSAlbedo100_g4236 ));
				half2 uv_Layer1 = IN.ase_texcoord8.xyz.xy * _Layer1_ST.xy + _Layer1_ST.zw;
				half2 uvLayer1114_g4236 = uv_Layer1;
				half3 appendResult73_g4236 = (half3(SAMPLE_TEXTURE2D( _Layer1, sampler_Linear_Repeat_Aniso2, uvLayer1114_g4236 ).rgb));
				half gt57_g4236 = saturate( ( tex2DNode3_g4236.g * tex2DNode4_g4236.g ) );
				half3 lerpResult61_g4236 = lerp( lerpResult34_g4236 , appendResult73_g4236 , pow( gt57_g4236 , LSAlbedo100_g4236 ));
				#ifdef _REVEALLAYERS
				half3 staticSwitch1_g4236 = lerpResult61_g4236;
				#else
				half3 staticSwitch1_g4236 = temp_output_35_0_g4236;
				#endif
				half3 temp_output_4603_36 = staticSwitch1_g4236;
				
				half3 appendResult2618 = (half3(unpack2617));
				half3 _Vector14 = half3(0,0,1);
				half3 ifLocalVar3262 = 0;
				if( _UseNormalMap <= 0.5 )
				ifLocalVar3262 = _Vector14;
				else
				ifLocalVar3262 = appendResult2618;
				half3 SimpleNormalXYSigned30 = ifLocalVar3262;
				int MossInt2938 = _Moss;
				half2 _Vector0 = half2(0,0);
				half2 temp_output_3045_0 = (MossMetalRes3077).ag;
				half2 MossPackedNormal1656 = temp_output_3045_0;
				half4 temp_output_5_0_g2453 = half4( MossPackedNormal1656, 0.0 , 0.0 );
				half2 appendResult12_g2453 = (half2(temp_output_5_0_g2453.xy));
				half temp_output_6_0_g2453 = _MossNormalStrength;
				half2 lerpResult2_g2453 = lerp( float2( 0,0 ) , ( appendResult12_g2453 - float2( 0.5,0.5 ) ) , ( temp_output_6_0_g2453 * 2.0 ));
				half2 lerpResult2976 = lerp( _Vector0 , lerpResult2_g2453 , MossMask45);
				half2 MossNormalSigned100 = lerpResult2976;
				half2 temp_output_3610_0 = ( MossInt2938 * MossNormalSigned100 );
				half3 appendResult4006 = (half3(temp_output_3610_0 , 1.0));
				half3 lerpResult4431 = lerp( SimpleNormalXYSigned30 , float3( 0,0,1 ) , MossMask45);
				half2 appendResult1363 = (half2(lerpResult4431.xy));
				#ifdef _MOSS
				half3 staticSwitch4131 = ( appendResult4006 + half3( appendResult1363 ,  0.0 ) );
				#else
				half3 staticSwitch4131 = SimpleNormalXYSigned30;
				#endif
				half2 DetailNormaMossMaskedSigned988 = ( _DetailNormalMapScale * (staticSwitch4132).yz );
				half3 appendResult4397 = (half3(DetailNormaMossMaskedSigned988 , 1.0));
				#ifdef _DETAIL_ON
				half3 staticSwitch1105 = BlendNormal( staticSwitch4131 , appendResult4397 );
				#else
				half3 staticSwitch1105 = staticSwitch4131;
				#endif
				half3 NormalPre2265 = staticSwitch1105;
				half3 normalizeResult4009 = normalize( NormalPre2265 );
				half3 NormalRes109 = normalizeResult4009;
				half3 temp_output_42_0_g4236 = NormalRes109;
				half3 unpack9_g4236 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Layer0NormalMap, sampler_Linear_Repeat_Aniso2, uvLayer0112_g4236 ), _Layer0NormalStrength );
				unpack9_g4236.z = lerp( 1, unpack9_g4236.z, saturate(_Layer0NormalStrength) );
				half LSNormal101_g4236 = _LayerSurfaceExp.y;
				half3 lerpResult41_g4236 = lerp( temp_output_42_0_g4236 , unpack9_g4236 , pow( rt31_g4236 , LSNormal101_g4236 ));
				half3 unpack18_g4236 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Layer1NormalMap, sampler_Linear_Repeat_Aniso2, uvLayer1114_g4236 ), _Layer1NormalStrength );
				unpack18_g4236.z = lerp( 1, unpack18_g4236.z, saturate(_Layer1NormalStrength) );
				half3 lerpResult64_g4236 = lerp( lerpResult41_g4236 , unpack18_g4236 , pow( gt57_g4236 , LSNormal101_g4236 ));
				half2 uv83_g4236 = baseuv25_g4236;
				half bt76_g4236 = saturate( ( tex2DNode3_g4236.b * tex2DNode4_g4236.b ) );
				half height83_g4236 = ( _Layer2Height * bt76_g4236 );
				half2 texelSize83_g4236 = (_RevealMask_TexelSize).xy;
				SamplerState sampler_RevealMask83_g4236 = sampler_Linear_Repeat_Aniso2;
				half3 localRevealMaskNormalCrossFilter83_g4236 = RevealMaskNormalCrossFilter83_g4236( uv83_g4236 , height83_g4236 , texelSize83_g4236 , sampler_RevealMask83_g4236 );
				half3 heightNormal89_g4236 = localRevealMaskNormalCrossFilter83_g4236;
				#ifdef _REVEALLAYERS
				half3 staticSwitch58_g4236 = BlendNormal( lerpResult64_g4236 , heightNormal89_g4236 );
				#else
				half3 staticSwitch58_g4236 = temp_output_42_0_g4236;
				#endif
				half3 temp_output_4603_43 = staticSwitch58_g4236;
				half3 NormalRes24487 = temp_output_4603_43;
				
				half fresnelNdotV135 = dot( WorldNormal, WorldViewDirection );
				half fresnelNode135 = ( _MossFresnel.x + _MossFresnel.y * pow( 1.0 - fresnelNdotV135, ( _MossFresnel.z * 10.0 ) ) );
				half3 SH3558 = IN.lightmapUVOrVertexSH.xyz;
				half3 normalWS3558 = WorldNormal;
				half3 localSampleSHPixel3558 = SampleSHPixel3558( SH3558 , normalWS3558 );
				half2 lightmapUV3567 = IN.lightmapUVOrVertexSH.xy;
				half3 normalWS3567 = WorldNormal;
				half3 localSampleLightmap3567 = SampleLightmap3567( lightmapUV3567 , normalWS3567 );
				#ifdef LIGHTMAP_ON
				half3 staticSwitch3554 = localSampleLightmap3567;
				#else
				half3 staticSwitch3554 = localSampleSHPixel3558;
				#endif
				half3 LightmapOrSH2480 = staticSwitch3554;
				half grayscale4604 = (LightmapOrSH2480.r + LightmapOrSH2480.g + LightmapOrSH2480.b) / 3;
				half3 AlbedoResPre2289 = staticSwitch1045;
				half4 temp_output_2664_0 = ( ( ( min( _MossFresnel.w , saturate( fresnelNode135 ) ) * _MossFresnelColor * grayscale4604 ) * half4( AlbedoResPre2289 , 0.0 ) * saturate( ( MossMask45 - 0.3 ) ) ) * _UseMossFresnel );
				#ifdef _USEMOSSFRESNEL_ON
				half4 staticSwitch1195 = temp_output_2664_0;
				#else
				half4 staticSwitch1195 = float4( 0,0,0,0 );
				#endif
				#ifdef _MOSS
				half4 staticSwitch1237 = staticSwitch1195;
				#else
				half4 staticSwitch1237 = float4( 0,0,0,0 );
				#endif
				half4 MossFresnel203 = staticSwitch1237;
				half3 appendResult3069 = (half3(_EmissionColor.rgb));
				half _MetallicSpecGlossMap3184 = _MetallicSpecGlossMap;
				half temp_output_2918_0 = (MossMetalORMetallicMap2911).r;
				half temp_output_8_0_g2532 = temp_output_2918_0;
				half4 break5_g2532 = _MetallicRemap;
				half temp_output_3_0_g2532 = (0.0 + (temp_output_8_0_g2532 - break5_g2532.x) * (1.0 - 0.0) / (break5_g2532.y - break5_g2532.x));
				half clampResult2_g2532 = clamp( temp_output_3_0_g2532 , break5_g2532.z , break5_g2532.w );
				half temp_output_27_0_g2532 = saturate( clampResult2_g2532 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3732 = temp_output_27_0_g2532;
				#else
				half staticSwitch3732 = temp_output_2918_0;
				#endif
				half temp_output_3002_0 = (MossMetalORMetallicMap2911).g;
				half temp_output_8_0_g2550 = temp_output_3002_0;
				half4 break5_g2550 = _OcclusionRemap;
				half temp_output_3_0_g2550 = (0.0 + (temp_output_8_0_g2550 - break5_g2550.x) * (1.0 - 0.0) / (break5_g2550.y - break5_g2550.x));
				half clampResult2_g2550 = clamp( temp_output_3_0_g2550 , break5_g2550.z , break5_g2550.w );
				half temp_output_27_0_g2550 = saturate( clampResult2_g2550 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3735 = temp_output_27_0_g2550;
				#else
				half staticSwitch3735 = temp_output_3002_0;
				#endif
				half temp_output_12_0_g2502 = ( _UseOcclusion * _MetallicSpecGlossMap3184 * _Occlusion );
				half temp_output_12_0_g2514 = (float)Mode3840;
				half temp_output_392_0 = (MossMetalORMetallicMap2911).a;
				half temp_output_8_0_g2540 = temp_output_392_0;
				half4 break5_g2540 = _SmoothnessRemap;
				half temp_output_3_0_g2540 = (0.0 + (temp_output_8_0_g2540 - break5_g2540.x) * (1.0 - 0.0) / (break5_g2540.y - break5_g2540.x));
				half clampResult2_g2540 = clamp( temp_output_3_0_g2540 , break5_g2540.z , break5_g2540.w );
				half temp_output_27_0_g2540 = saturate( clampResult2_g2540 );
				half temp_output_3894_0 = temp_output_27_0_g2540;
				#ifdef _REMAPPERS_ON
				half staticSwitch3739 = temp_output_3894_0;
				#else
				half staticSwitch3739 = temp_output_392_0;
				#endif
				half4 appendResult3479 = (half4(( staticSwitch3732 * _Metallic ) , ( ( staticSwitch3735 * temp_output_12_0_g2502 ) + ( 1.0 - temp_output_12_0_g2502 ) ) , ( ( ( staticSwitch3738 * temp_output_12_0_g2514 ) + ( 1.0 - temp_output_12_0_g2514 ) ) * _UseEmission ) , ( staticSwitch3739 * _Smoothness )));
				half4 appendResult3481 = (half4(_Metallic , 1.0 , _UseEmission , _Smoothness));
				half4 ifLocalVar3480 = 0;
				if( _MetallicSpecGlossMap3184 <= 0.0 )
				ifLocalVar3480 = appendResult3481;
				else
				ifLocalVar3480 = appendResult3479;
				half UseMossMetalMapInt3239 = _UseMossMetalMap;
				half4 appendResult3487 = (half4(_Metallic , 1.0 , _UseEmission , _Smoothness));
				half4 appendResult3485 = (half4(( staticSwitch3732 * _Metallic ) , ( ( staticSwitch3735 * temp_output_12_0_g2502 ) + ( 1.0 - temp_output_12_0_g2502 ) ) , ( ( ( staticSwitch3738 * temp_output_12_0_g2514 ) + ( 1.0 - temp_output_12_0_g2514 ) ) * _UseEmission ) , ( staticSwitch3739 * _Smoothness )));
				half4 ifLocalVar3486 = 0;
				if( ( MossModeInt3200 + ( 1.0 - UseMossMetalMapInt3239 ) ) <= 0.0 )
				ifLocalVar3486 = appendResult3485;
				else
				ifLocalVar3486 = appendResult3487;
				#ifdef _MOSSMETALMODE_ON
				half4 staticSwitch3494 = ifLocalVar3486;
				#else
				half4 staticSwitch3494 = ifLocalVar3480;
				#endif
				half4 break3482 = staticSwitch3494;
				half EmissionMasked3065 = break3482.z;
				half3 temp_output_3496_0 = ( appendResult3069 * EmissionMasked3065 );
				half4 EmissionRes806 = ( MossFresnel203 + half4( temp_output_3496_0 , 0.0 ) );
				half3 temp_output_91_0_g4236 = EmissionRes806.rgb;
				half at79_g4236 = saturate( ( tex2DNode3_g4236.a * tex2DNode4_g4236.a ) );
				half3 appendResult99_g4236 = (half3(_Layer3EmissionColor.rgb));
				#ifdef _REVEALLAYERS
				half3 staticSwitch93_g4236 = ( ( temp_output_91_0_g4236 * ( 1.0 - at79_g4236 ) ) + ( appendResult99_g4236 * at79_g4236 ) );
				#else
				half3 staticSwitch93_g4236 = temp_output_91_0_g4236;
				#endif
				half OceanUnder289_g4237 = GlobalOceanUnder;
				half3 EmissionIn281_g4237 = float3( 0,0,0 );
				half3 WorldPosition256_g4237 = WorldPosition;
				half3 temp_output_105_0_g4260 = WorldPosition256_g4237;
				half temp_output_117_0_g4260 = (temp_output_105_0_g4260).y;
				half temp_output_67_0_g4237 = ( GlobalOceanOffset + GlobalOceanHeight );
				half OceanHeight274_g4237 = temp_output_67_0_g4237;
				half temp_output_63_0_g4260 = OceanHeight274_g4237;
				half3 temp_output_9_0_g4237 = temp_output_4603_43;
				half3 temp_output_30_0_g4260 = temp_output_9_0_g4237;
				float3 tanNormal12_g4260 = temp_output_30_0_g4260;
				half3 worldNormal12_g4260 = float3(dot(tanToWorld0,tanNormal12_g4260), dot(tanToWorld1,tanNormal12_g4260), dot(tanToWorld2,tanNormal12_g4260));
				half3 vertexToFrag144_g4260 = IN.ase_texcoord11.yzw;
				half dotResult1_g4260 = dot( worldNormal12_g4260 , vertexToFrag144_g4260 );
				half desktop151_g4260 = max( dotResult1_g4260 , 0.0 );
				half mobile151_g4260 = 1.0;
				half localShaderAPIMobile151_g4260 = ShaderAPIMobile151_g4260( desktop151_g4260 , mobile151_g4260 );
				half2 vertexToFrag52_g4260 = IN.ase_texcoord12.xy;
				half4 tex2DNode120_g4260 = SAMPLE_TEXTURE2D( _Caustics, sampler_Linear_Repeat, vertexToFrag52_g4260 );
				half3 appendResult121_g4260 = (half3(tex2DNode120_g4260.r , tex2DNode120_g4260.r , tex2DNode120_g4260.r));
				half2 DistanceFade134_g4260 = (_CausticsSettings).zw;
				half2 break136_g4260 = DistanceFade134_g4260;
				half temp_output_67_0_g4260 = ( saturate( (1.0 + (distance( temp_output_63_0_g4260 , temp_output_117_0_g4260 ) - break136_g4260.x) * (0.0 - 1.0) / (break136_g4260.y - break136_g4260.x)) ) * saturate( (0.0 + (max( -( temp_output_117_0_g4260 - temp_output_63_0_g4260 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) );
				half desktop153_g4260 = temp_output_67_0_g4260;
				half mobile153_g4260 = 1.0;
				half localShaderAPIMobile153_g4260 = ShaderAPIMobile153_g4260( desktop153_g4260 , mobile153_g4260 );
				half CausticMipLevel118_g4260 = ( ( 1.0 - localShaderAPIMobile153_g4260 ) * 4.0 );
				#ifdef _USECAUSTICRAINBOW_ON
				half3 staticSwitch73_g4260 = ( ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Linear_Repeat, ( vertexToFrag52_g4260 + ( 0.0045 * 2.0 ) ), CausticMipLevel118_g4260 ).r * half3(0,0,1) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Linear_Repeat, ( vertexToFrag52_g4260 + 0.0045 ), CausticMipLevel118_g4260 ).r * half3(0,1,0) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Linear_Repeat, vertexToFrag52_g4260, CausticMipLevel118_g4260 ).r * half3(1,0,0) ) );
				#else
				half3 staticSwitch73_g4260 = appendResult121_g4260;
				#endif
				#ifdef _USECAUSTICEXTRASAMPLER_ON
				half3 staticSwitch57_g4260 = staticSwitch73_g4260;
				#else
				half3 staticSwitch57_g4260 = staticSwitch73_g4260;
				#endif
				half3 appendResult62_g4260 = (half3(_CausticsColor.rgb));
				half3 ifLocalVar155_g4260 = 0;
				UNITY_BRANCH 
				if( temp_output_117_0_g4260 < ( temp_output_63_0_g4260 + 0.2 ) )
				ifLocalVar155_g4260 = ( localShaderAPIMobile151_g4260 * staticSwitch57_g4260 * appendResult62_g4260 * localShaderAPIMobile153_g4260 );
				half3 lerpResult205_g4237 = lerp( ( EmissionIn281_g4237 + ifLocalVar155_g4260 ) , ( ( EmissionIn281_g4237 + ifLocalVar155_g4260 ) * 30.0 ) , OceanUnder289_g4237);
				half3 appendResult100_g4251 = (half3(OceanWaterTint_RGB.xyz));
				half3 WorldPos252_g4251 = WorldPosition256_g4237;
				half temp_output_108_0_g4251 = OceanHeight274_g4237;
				half3 ViewDir264_g4237 = WorldViewDirection;
				half3 viewDir240_g4251 = ViewDir264_g4237;
				half3 camWorldPos240_g4251 = _WorldSpaceCameraPos;
				half3 posWS240_g4251 = WorldPos252_g4251;
				half4 oceanFogDensities240_g4251 = OceanFogDensities;
				half oceanHeight240_g4251 = temp_output_108_0_g4251;
				half4 oceanFogTop_RGB_Exponent240_g4251 = OceanFogTop_RGB_Exponent;
				half4 oceanFogBottom_RGB_Intensity240_g4251 = OceanFogBottom_RGB_Intensity;
				half4 localGetUnderWaterFogs240_g4251 = GetUnderWaterFogs240_g4251( viewDir240_g4251 , camWorldPos240_g4251 , posWS240_g4251 , oceanFogDensities240_g4251 , oceanHeight240_g4251 , oceanFogTop_RGB_Exponent240_g4251 , oceanFogBottom_RGB_Intensity240_g4251 );
				half4 ifLocalVar257_g4251 = 0;
				UNITY_BRANCH 
				if( (WorldPos252_g4251).y < ( temp_output_108_0_g4251 + 0.2 ) )
				ifLocalVar257_g4251 = localGetUnderWaterFogs240_g4251;
				half4 FogRes185_g4251 = ifLocalVar257_g4251;
				half3 appendResult94_g4251 = (half3(FogRes185_g4251.xyz));
				half3 lerpResult36_g4251 = lerp( ( lerpResult205_g4237 * appendResult100_g4251 ) , appendResult94_g4251 , (FogRes185_g4251).w);
				half3 ifLocalVar5_g4237 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g4237 >= 1.0 )
				ifLocalVar5_g4237 = lerpResult36_g4251;
				else
				ifLocalVar5_g4237 = EmissionIn281_g4237;
				half temp_output_254_0_g4237 = (WorldPosition256_g4237).y;
				half temp_output_137_0_g4237 = ( temp_output_254_0_g4237 - OceanHeight274_g4237 );
				half temp_output_24_0_g4247 = temp_output_137_0_g4237;
				half temp_output_44_0_g4247 = 0.1;
				half temp_output_45_0_g4247 = 0.31;
				half temp_output_46_0_g4247 = saturate( (0.0 + (( temp_output_24_0_g4247 - temp_output_44_0_g4247 ) - 0.0) * (1.0 - 0.0) / (temp_output_45_0_g4247 - 0.0)) );
				#ifdef _FadeWithHeight
				half staticSwitch238_g4237 = ( 1.0 - temp_output_46_0_g4247 );
				#else
				half staticSwitch238_g4237 = 1.0;
				#endif
				half FadeFromY295_g4237 = staticSwitch238_g4237;
				half3 lerpResult174_g4237 = lerp( ( ifLocalVar5_g4237 + lerpResult205_g4237 ) , ifLocalVar5_g4237 , ( OceanUnder289_g4237 * FadeFromY295_g4237 ));
				half3 lerpResult242_g4237 = lerp( EmissionIn281_g4237 , lerpResult174_g4237 , FadeFromY295_g4237);
				#ifdef _USECAUSTICSFROMABOVE_ON
				half3 staticSwitch172_g4237 = lerpResult242_g4237;
				#else
				half3 staticSwitch172_g4237 = ifLocalVar5_g4237;
				#endif
				#ifdef _USEUNDERWATER
				half3 staticSwitch211_g4237 = staticSwitch172_g4237;
				#else
				half3 staticSwitch211_g4237 = float3( 0,0,0 );
				#endif
				half3 temp_output_4575_8 = staticSwitch211_g4237;
				
				half Metallic399 = break3482.x;
				half MetallicSimpler398 = Metallic399;
				half lerpResult653 = lerp( MetallicSimpler398 , _MossMetallic , MossMask45);
				#ifdef _MOSS
				half staticSwitch1238 = lerpResult653;
				#else
				half staticSwitch1238 = MetallicSimpler398;
				#endif
				half MetallicResult1087 = staticSwitch1238;
				half temp_output_47_0_g4236 = MetallicResult1087;
				half LSMetallic102_g4236 = _LayerSurfaceExp.z;
				half lerpResult46_g4236 = lerp( temp_output_47_0_g4236 , _Layer0Metallic , pow( rt31_g4236 , LSMetallic102_g4236 ));
				half lerpResult67_g4236 = lerp( lerpResult46_g4236 , _Layer1Metallic , pow( gt57_g4236 , LSMetallic102_g4236 ));
				#ifdef _REVEALLAYERS
				half staticSwitch59_g4236 = lerpResult67_g4236;
				#else
				half staticSwitch59_g4236 = temp_output_47_0_g4236;
				#endif
				
				half Smoothness400 = break3482.w;
				half temp_output_3044_0 = (MossMetalRes3077).b;
				half temp_output_8_0_g2536 = temp_output_3044_0;
				half4 break5_g2536 = _MossSmoothnessRemap;
				half temp_output_3_0_g2536 = (0.0 + (temp_output_8_0_g2536 - break5_g2536.x) * (1.0 - 0.0) / (break5_g2536.y - break5_g2536.x));
				half clampResult2_g2536 = clamp( temp_output_3_0_g2536 , break5_g2536.z , break5_g2536.w );
				half temp_output_27_0_g2536 = saturate( clampResult2_g2536 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3746 = temp_output_27_0_g2536;
				#else
				half staticSwitch3746 = temp_output_3044_0;
				#endif
				half MossPackedSmoothness1660 = staticSwitch3746;
				half lerpResult650 = lerp( Smoothness400 , ( _MossSmoothness * MossPackedSmoothness1660 ) , MossMask45);
				#ifdef _MOSS
				half staticSwitch4121 = lerpResult650;
				#else
				half staticSwitch4121 = Smoothness400;
				#endif
				half DetailMapSmoothness1078 = (staticSwitch4132).w;
				#ifdef _DETAIL_ON
				half staticSwitch1093 = saturate( ( staticSwitch4121 + DetailMapSmoothness1078 ) );
				#else
				half staticSwitch1093 = staticSwitch4121;
				#endif
				half SmoothnessResult1085 = staticSwitch1093;
				half temp_output_54_0_g4236 = SmoothnessResult1085;
				half LSSmoothness103_g4236 = _LayerSurfaceExp.w;
				half lerpResult51_g4236 = lerp( temp_output_54_0_g4236 , _Layer0Smoothness , pow( rt31_g4236 , LSSmoothness103_g4236 ));
				half lerpResult70_g4236 = lerp( lerpResult51_g4236 , _Layer1Smoothness , pow( gt57_g4236 , LSSmoothness103_g4236 ));
				#ifdef _REVEALLAYERS
				half staticSwitch60_g4236 = lerpResult70_g4236;
				#else
				half staticSwitch60_g4236 = temp_output_54_0_g4236;
				#endif
				
				half OcclusionDoodad3006 = break3482.y;
				half temp_output_10_0_g2275 = _OcclusionMossMask;
				half temp_output_17_0_g2275 = saturate( MossMask45 );
				half lerpResult7_g2275 = lerp( (float)1 , OcclusionDoodad3006 , saturate( ( saturate( ( abs( temp_output_10_0_g2275 ) - 1.0 ) ) + ( ( ( 1.0 - temp_output_17_0_g2275 ) * abs( min( temp_output_10_0_g2275 , 0.0 ) ) ) + ( temp_output_17_0_g2275 * max( temp_output_10_0_g2275 , 0.0 ) ) ) ) ));
				#ifdef _MOSS
				half staticSwitch3730 = lerpResult7_g2275;
				#else
				half staticSwitch3730 = OcclusionDoodad3006;
				#endif
				#ifdef _MOSSMETALMODE_ON
				half staticSwitch3019 = OcclusionDoodad3006;
				#else
				half staticSwitch3019 = staticSwitch3730;
				#endif
				half OcclusionResMStrength369 = staticSwitch3019;
				
				half4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				half2 clipScreen3958 = ase_screenPosNorm.xy * _ScreenParams.xy;
				half dither3958 = Dither4x4Bayer( fmod(clipScreen3958.x, 4), fmod(clipScreen3958.y, 4) );
				dither3958 = step( dither3958, temp_output_3952_0 );
				half lerpResult3962 = lerp( (float)AlphaToCoverage , 1.0 , _Surface);
				half Dither3961 = ( _Dither * ( 1.0 - lerpResult3962 ) );
				half lerpResult3959 = lerp( temp_output_3952_0 , dither3958 , Dither3961);
				#ifdef _ALPHATEST_ON
				half staticSwitch3334 = lerpResult3959;
				#else
				half staticSwitch3334 = 1.0;
				#endif
				half AlbedoAlpha84 = staticSwitch3334;
				
				half lerpResult3950 = lerp( 0.0 , _AlphaClipThreshold , _AlphaClip);
				half lerpResult3291 = lerp( lerpResult3950 , 0.0 , (float)AlphaToCoverage);
				half ClipCalc3294 = lerpResult3291;
				
				half ShadowThreshold2627 = _ShadowThreshold;
				
				half4x4 temp_output_28_0_g4351 = _ProbeWorldToTexture;
				half4x4 Input90_g4351 = temp_output_28_0_g4351;
				half4x4 localRotationOnly90_g4351 = RotationOnly90_g4351( Input90_g4351 );
				float3 tanNormal4461 = NormalRes24487;
				half3 worldNormal4461 = float3(dot(tanToWorld0,tanNormal4461), dot(tanToWorld1,tanNormal4461), dot(tanToWorld2,tanNormal4461));
				half3 worldNormal14_g4351 = mul( localRotationOnly90_g4351, half4( worldNormal4461 , 0.0 ) ).xyz;
				half3 vertexToFrag7_g4351 = IN.ase_texcoord13.xyz;
				half4 ProbeVolumeShR14_g4351 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShR, sampler_Linear_Clamp, ( vertexToFrag7_g4351 + half3( float2( 0,0 ) ,  0.0 ) ), 0.0 );
				half4 ProbeVolumeShG14_g4351 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShG, sampler_Linear_Clamp, ( vertexToFrag7_g4351 + half3( float2( 0,0 ) ,  0.0 ) ), 0.0 );
				half4 ProbeVolumeShB14_g4351 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShB, sampler_Linear_Clamp, ( vertexToFrag7_g4351 + half3( float2( 0,0 ) ,  0.0 ) ), 0.0 );
				half3 localSHEvalLinearL0L114_g4351 = SHEvalLinearL0L114_g4351( worldNormal14_g4351 , ProbeVolumeShR14_g4351 , ProbeVolumeShG14_g4351 , ProbeVolumeShB14_g4351 );
				#ifdef _PROBEVOLUME_ON
				half3 staticSwitch20_g4351 = localSHEvalLinearL0L114_g4351;
				#else
				half3 staticSwitch20_g4351 = LightmapOrSH2480;
				#endif
				half3 temp_output_4622_0 = staticSwitch20_g4351;
				half3 BakedGI1212 = temp_output_4622_0;
				
				half4 ifLocalVars221 = 0;
				Gradient gradient1_g3048 = NewGradient( 0, 3, 2, float4( 0, 0, 0, 0 ), float4( 0, 0.1608469, 1, 0.4974899 ), float4( 1, 0, 0.1051435, 1 ), 0, 0, 0, 0, 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
				if(_DebugVisual==0){ifLocalVars221 = SampleGradient( gradient1_g3048, ( _DebugCounter / _DebugScale ) ); };
				if(_DebugVisual==1){ifLocalVars221 = half4( WorldNormal , 0.0 ); };
				half4 temp_cast_51 = (MossMask45).xxxx;
				if(_DebugVisual==2){ifLocalVars221 = temp_cast_51; };
				if(_DebugVisual==3){ifLocalVars221 = MossFresnel203; };
				half4 temp_cast_53 = (MossMaskDirection1179).xxxx;
				if(_DebugVisual==4){ifLocalVars221 = temp_cast_53; };
				if(_DebugVisual==5){ifLocalVars221 = float4( 0,0,0,0 ); };
				half4 temp_output_16_0_g2574 = float4( 1,0,0,0 );
				half temp_output_14_0_g2574 = MossMaskVertex1181;
				half temp_output_6_0_g2574 = saturate( temp_output_14_0_g2574 );
				half4 temp_output_15_0_g2574 = float4( 0,0,1,0 );
				half temp_output_12_0_g2574 = (0.0 + (temp_output_14_0_g2574 - -1.0) * (1.0 - 0.0) / (0.0 - -1.0));
				half temp_output_13_0_g2574 = step( temp_output_12_0_g2574 , 1.0 );
				half temp_output_11_0_g2574 = ( 1.0 - saturate( ( temp_output_12_0_g2574 * temp_output_13_0_g2574 ) ) );
				half4 temp_output_2_0_g2574 = ( ( temp_output_16_0_g2574 * temp_output_6_0_g2574 ) + ( temp_output_15_0_g2574 * temp_output_11_0_g2574 * temp_output_13_0_g2574 ) );
				if(_DebugVisual==6){ifLocalVars221 = temp_output_2_0_g2574; };
				if(_DebugVisual==7){ifLocalVars221 = 0.0; };
				half4 temp_cast_55 = (DetailMapAlbedo987).xxxx;
				if(_DebugVisual==8){ifLocalVars221 = temp_cast_55; };
				if(_DebugVisual==9){ifLocalVars221 = half4( DetailNormaMossMaskedSigned988, 0.0 , 0.0 ); };
				half4 temp_output_16_0_g723 = float4( 1,0,0,0 );
				half temp_output_14_0_g723 = DetailMapSmoothness1078;
				half temp_output_6_0_g723 = saturate( temp_output_14_0_g723 );
				half4 temp_output_15_0_g723 = float4( 0,0,1,0 );
				half temp_output_12_0_g723 = (0.0 + (temp_output_14_0_g723 - -1.0) * (1.0 - 0.0) / (0.0 - -1.0));
				half temp_output_13_0_g723 = step( temp_output_12_0_g723 , 1.0 );
				half temp_output_11_0_g723 = ( 1.0 - saturate( ( temp_output_12_0_g723 * temp_output_13_0_g723 ) ) );
				half4 temp_output_2_0_g723 = ( ( temp_output_16_0_g723 * temp_output_6_0_g723 ) + ( temp_output_15_0_g723 * temp_output_11_0_g723 * temp_output_13_0_g723 ) );
				if(_DebugVisual==10){ifLocalVars221 = temp_output_2_0_g723; };
				half4 temp_cast_58 = (SmoothnessResult1085).xxxx;
				if(_DebugVisual==11){ifLocalVars221 = temp_cast_58; };
				if(_DebugVisual==12){ifLocalVars221 = half4( NormalRes109 , 0.0 ); };
				half dotResult833 = dot( half3(0,1,0) , WorldNormal );
				half MossSlopeMask1202 = saturate( (0.0 + (dotResult833 - _MossSlopeMM.x) * (1.0 - 0.0) / (_MossSlopeMM.y - _MossSlopeMM.x)) );
				half4 temp_cast_60 = (MossSlopeMask1202).xxxx;
				if(_DebugVisual==13){ifLocalVars221 = temp_cast_60; };
				half2 vertexToFrag1159 = IN.ase_texcoord12.zw;
				half dotResult856 = dot( float3( 1,0,0 ) , WorldNormal );
				half dotResult853 = dot( float3( 0,1,0 ) , WorldNormal );
				half2 appendResult865 = (half2(dotResult856 , dotResult853));
				half2 lerpResult871 = lerp( float2( 1,1 ) , appendResult865 , _MossSlopeDistort);
				#ifdef _USEMOSSSLOPEDISTORT_ON
				half2 staticSwitch887 = ( vertexToFrag1159 * lerpResult871 );
				#else
				half2 staticSwitch887 = vertexToFrag1159;
				#endif
				half4 temp_output_5_0_g2280 = SAMPLE_TEXTURE2D( _MossSlopeNormal, sampler_Linear_Repeat_Aniso2, staticSwitch887 );
				half2 appendResult12_g2280 = (half2(temp_output_5_0_g2280.xy));
				half temp_output_6_0_g2280 = _MossNormalStrength2;
				half2 lerpResult2_g2280 = lerp( float2( 0,0 ) , ( appendResult12_g2280 - float2( 0.5,0.5 ) ) , ( temp_output_6_0_g2280 * 2.0 ));
				half4 temp_output_5_0_g2279 = float4( 0,0,1,0 );
				half2 appendResult12_g2279 = (half2(temp_output_5_0_g2279.xy));
				half temp_output_6_0_g2279 = _MossNormalStrength2;
				half2 lerpResult2_g2279 = lerp( float2( 0,0 ) , ( appendResult12_g2279 - float2( 0.5,0.5 ) ) , ( temp_output_6_0_g2279 * 2.0 ));
				#ifdef _USESTOCHASTICMOSS_ON
				half2 staticSwitch894 = lerpResult2_g2279;
				#else
				half2 staticSwitch894 = lerpResult2_g2280;
				#endif
				half2 MossSlopeNormal1200 = staticSwitch894;
				if(_DebugVisual==14){ifLocalVars221 = half4( MossSlopeNormal1200, 0.0 , 0.0 ); };
				half4 temp_output_16_0_g937 = float4( 1,0,0,0 );
				half MaskDebug1801 = 0.0;
				half temp_output_14_0_g937 = MaskDebug1801;
				half temp_output_6_0_g937 = saturate( temp_output_14_0_g937 );
				half4 temp_output_15_0_g937 = float4( 0,0,1,0 );
				half temp_output_12_0_g937 = (0.0 + (temp_output_14_0_g937 - -1.0) * (1.0 - 0.0) / (0.0 - -1.0));
				half temp_output_13_0_g937 = step( temp_output_12_0_g937 , 1.0 );
				half temp_output_11_0_g937 = ( 1.0 - saturate( ( temp_output_12_0_g937 * temp_output_13_0_g937 ) ) );
				half4 temp_output_2_0_g937 = ( ( temp_output_16_0_g937 * temp_output_6_0_g937 ) + ( temp_output_15_0_g937 * temp_output_11_0_g937 * temp_output_13_0_g937 ) );
				if(_DebugVisual==15){ifLocalVars221 = temp_output_2_0_g937; };
				half4 DebugVisuals210 = ifLocalVars221;
				
				half2 LightmapUV1_g4353 = (IN.ase_texcoord14.xy*(unity_LightmapST).xy + (unity_LightmapST).zw);
				half4 localCalculateShadowMask1_g4353 = CalculateShadowMask1_g4353( LightmapUV1_g4353 );
				#ifdef _PROBEVOLUME_ON
				half4 staticSwitch25_g4351 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeOcc, sampler_Linear_Clamp, ( vertexToFrag7_g4351 + half3( float2( 0,0 ) ,  0.0 ) ), 0.0 );
				#else
				half4 staticSwitch25_g4351 = localCalculateShadowMask1_g4353;
				#endif
				half4 temp_output_4622_19 = staticSwitch25_g4351;
				half4 ShadowMask1213 = temp_output_4622_19;
				
				half3 appendResult50_g4237 = (half3(unity_FogColor.rgb));
				half4 appendResult52_g4237 = (half4(appendResult50_g4237 , ( 1.0 - IN.fogFactorAndVertexLight.x )));
				#ifdef FOG_LINEAR
				half4 staticSwitch252_g4237 = appendResult52_g4237;
				#else
				half4 staticSwitch252_g4237 = float4( 0,0,0,0 );
				#endif
				half4 FogLinear53_g4237 = staticSwitch252_g4237;
				half4 appendResult47_g4237 = (half4(FogLinear53_g4237));
				half3 appendResult379_g4237 = (half3(FogLinear53_g4237.xyz));
				half3 temp_output_394_103_g4237 = appendResult94_g4251;
				half temp_output_24_0_g4249 = temp_output_137_0_g4237;
				half temp_output_44_0_g4249 = 0.35;
				half temp_output_45_0_g4249 = 0.31;
				half temp_output_46_0_g4249 = saturate( (0.0 + (( temp_output_24_0_g4249 - temp_output_44_0_g4249 ) - 0.0) * (1.0 - 0.0) / (temp_output_45_0_g4249 - 0.0)) );
				#ifdef _FadeWithHeight
				half staticSwitch371_g4237 = ( 1.0 - temp_output_46_0_g4249 );
				#else
				half staticSwitch371_g4237 = 1.0;
				#endif
				half FadeFromY2375_g4237 = staticSwitch371_g4237;
				half smoothstepResult366_g4237 = smoothstep( 0.0 , 1.0 , FadeFromY2375_g4237);
				half3 lerpResult378_g4237 = lerp( appendResult379_g4237 , temp_output_394_103_g4237 , smoothstepResult366_g4237);
				half temp_output_61_0_g4251 = ( 1.0 - (FogRes185_g4251).w );
				half temp_output_58_0_g4237 = ( 1.0 - temp_output_61_0_g4251 );
				half smoothstepResult374_g4237 = smoothstep( 0.0 , 1.0 , FadeFromY295_g4237);
				half lerpResult381_g4237 = lerp( (FogLinear53_g4237).w , temp_output_58_0_g4237 , smoothstepResult374_g4237);
				half4 appendResult377_g4237 = (half4(lerpResult378_g4237 , lerpResult381_g4237));
				half4 ifLocalVar49_g4237 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g4237 >= 1.0 )
				ifLocalVar49_g4237 = appendResult377_g4237;
				else
				ifLocalVar49_g4237 = appendResult47_g4237;
				#ifdef _USEUNDERWATER
				half4 staticSwitch215_g4237 = max( ifLocalVar49_g4237 , float4( 0,0,0,0 ) );
				#else
				half4 staticSwitch215_g4237 = appendResult47_g4237;
				#endif
				
				half lerpResult16_g4270 = lerp( 0.0 , _SSSDimAlbedo , _SSScattering);
				half4 appendResult3_g4270 = (half4(_SSSRadius , _SSScattering , _NormalStrength_USE_SSS , lerpResult16_g4270));
				
				half3 appendResult8_g4270 = (half3(_SSSColor.rgb));
				half4 appendResult18_g4270 = (half4(appendResult8_g4270 , _SSShadcowMix));
				
				half4 appendResult11_g4269 = (half4(_TonemappingSettings.x , _TonemappingSettings.y , _TonemappingSettings.z , _TonemappingSettings.w));
				
				half3 temp_cast_72 = (1.0).xxx;
				half3 temp_cast_73 = (1.0).xxx;
				half3 temp_cast_74 = (1.0).xxx;
				half3 ifLocalVar170_g4237 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g4237 >= 1.0 )
				ifLocalVar170_g4237 = appendResult100_g4251;
				else
				ifLocalVar170_g4237 = temp_cast_74;
				half3 lerpResult226_g4237 = lerp( temp_cast_73 , ifLocalVar170_g4237 , FadeFromY295_g4237);
				#ifdef _USEUNDERWATER
				half3 staticSwitch212_g4237 = lerpResult226_g4237;
				#else
				half3 staticSwitch212_g4237 = temp_cast_72;
				#endif
				

				float3 BaseColor = temp_output_4603_36;
				float3 Normal = NormalRes24487;
				float3 Emission = ( staticSwitch93_g4236 + ( temp_output_4603_36 * temp_output_4575_8 ) );
				float3 Specular = 0.5;
				float Metallic = staticSwitch59_g4236;
				float Smoothness = staticSwitch60_g4236;
				float Occlusion = OcclusionResMStrength369;
				float Alpha = AlbedoAlpha84;
				float AlphaClipThreshold = ClipCalc3294;
				float AlphaClipThresholdShadow = ShadowThreshold2627;
				float3 BakedGI = BakedGI1212;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;
				
				//Shadowood: new properties below:
				float3 DebugVisuals = DebugVisuals210.xyz;
				float4 ShadowMask = ShadowMask1213;
				float4 FogColor = staticSwitch215_g4237;
				half FresnelControl = 0;
				half4 SSSControl = appendResult3_g4270;
				half4 SSSColor = appendResult18_g4270;
				float4 Tonemapping = appendResult11_g4269;
				float3 Tint = staticSwitch212_g4237;
				float4 AnisotropicControl = 1;
				
				//float4 PostTonemapping = 0;

				#ifdef ASE_DEPTH_WRITE_ON
					float DepthValue = IN.clipPos.z;
				#endif
				
				//Shadowood: remove clear coat, id of 20 will collide
				//#ifdef _CLEARCOAT
				//	float CoatMask = 0;
				//	float CoatSmoothness = 0;
				//#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData = (InputData)0;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				inputData.shadowCoord = ShadowCoords; //Shadowood

				#ifdef _NORMALMAP
						#if _NORMAL_DROPOFF_TS
							inputData.normalWS = TransformTangentToWorld(Normal, half3x3(WorldTangent, WorldBiTangent, WorldNormal));
						#elif _NORMAL_DROPOFF_OS
							inputData.normalWS = TransformObjectToWorldNormal(Normal);
						#elif _NORMAL_DROPOFF_WS
							inputData.normalWS = Normal;
						#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					inputData.shadowCoord = ShadowCoords;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
				#else
					inputData.shadowCoord = float4(0, 0, 0, 0);
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif
					inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				//Shadowood: Use passed in BakedGI // See GlobalIllumination.hlsl
				#ifdef ASE_BAKEDGI
					#if defined(LIGHTMAP_ON)
						inputData.bakedGI = BakedGI;
					#else
						inputData.bakedGI = BakedGI;// + SampleSHPixel(SH, inputData.normalWS); // Add LightProbes to passed in BakedGI
					#endif
				#else
					#if defined(DYNAMICLIGHTMAP_ON)
						inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, IN.dynamicLightmapUV.xy, SH, inputData.normalWS);
					#else
						inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
					#endif
				#endif
				
				//Shadooowd: removed: 
				/*
				#if defined(DYNAMICLIGHTMAP_ON)
					inputData.bakedGI = SAMPLE_GI(IN.lightmapUVOrVertexSH.xy, IN.dynamicLightmapUV.xy, SH, inputData.normalWS);
				#else
					inputData.bakedGI = SAMPLE_GI(IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS);
				#endif
				*/
				
				//Shadowood: old, remove this
				//#if defined(_PROBEVOLUME_ON)
					//#define ASE_BAKEDGI
					//#define ASE_SHADOWMASK
					//#ifdef ASE_BAKEDGI
					//	inputData.bakedGI = BakedGI;
					//#endif
				//#endif

				inputData.normalizedScreenSpaceUV = NormalizedScreenSpaceUV;
				
				
				//Shadowood: ProbeVolume
				#if defined(_PROBEVOLUME_ON)
					#ifdef ASE_SHADOWMASK
						unity_ProbesOcclusion = ShadowMask;
						inputData.shadowMask = unity_ProbesOcclusion;
					#else
						inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUVOrVertexSH.xy);
					#endif
				#else
					#ifdef ASE_SHADOWMASK
						inputData.shadowMask = ShadowMask;
					#else
						inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUVOrVertexSH.xy);
					#endif
				#endif

				#if defined(DEBUG_DISPLAY)
					#if defined(DYNAMICLIGHTMAP_ON)
						inputData.dynamicLightmapUV = IN.dynamicLightmapUV.xy;
					#endif
					#if defined(LIGHTMAP_ON)
						inputData.staticLightmapUV = IN.lightmapUVOrVertexSH.xy;
					#else
						inputData.vertexSH = SH;
					#endif
				#endif

				SurfaceData surfaceData;
				surfaceData.albedo              = BaseColor;
				surfaceData.metallic            = saturate(Metallic);
				surfaceData.specular            = Specular;
				surfaceData.smoothness          = saturate(Smoothness),
				surfaceData.occlusion           = Occlusion,
				surfaceData.emission            = Emission,
				surfaceData.alpha               = saturate(Alpha);
				surfaceData.normalTS            = Normal;
				surfaceData.clearCoatMask       = 0;
				surfaceData.clearCoatSmoothness = 1;

				#ifdef _CLEARCOAT
					surfaceData.clearCoatMask       = saturate(CoatMask);
					surfaceData.clearCoatSmoothness = saturate(CoatSmoothness);
				#endif

				#ifdef _DBUFFER
					ApplyDecalToSurfaceData(IN.clipPos, surfaceData, inputData);
				#endif

				//Shadowood: add custom PBR
				//#if ((defined(_ASE_ANIS) && defined(_USE_ANIS_ON)) || defined(_ASE_SSSCONTROL) && defined(_USE_SSS_ON)) || defined(_ASE_FRESNELCONTROL)
					half4 color = UniversalFragmentPBRCustom( inputData, surfaceData, FresnelControl, SSSColor, SSSControl, WorldNormal, WorldTangent, AnisotropicControl, _EnvironmentReflections );
				//#else
				//	half4 color = UniversalFragmentPBR( inputData, surfaceData); // Original
				//#endif

				//Shadowood: set alpha
				color.a = surfaceData.alpha;
				//color.a = (color.a - 0.1) / max( fwidth(color.a), 0.0001) + 0.5;
				//color.a = saturate(Alpha);

				//Shadowood: add _USE_TRANSMISSION checkbox
				#if defined(ASE_TRANSMISSION) && defined(_USE_TRANSMISSION)
				{
					float shadow = _TransmissionShadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += BaseColor * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += BaseColor * transmission;
						}
					#endif
				}
				#endif
				
				//Shadowood: add _USE_TRANSMISSION checkbox
				#if defined(ASE_TRANSLUCENCY) && defined(_TRANSLUCENCY)
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += BaseColor * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += BaseColor * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef ASE_REFRACTION
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, float4( WorldNormal,0 ) ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos.xy ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a; //Shadowood: should this be disabled?
				#endif

				// Shadowood: Moved below with ASE_FOGCOLOR
				//#ifdef ASE_FOG
				//	#ifdef TERRAIN_SPLAT_ADDPASS
				//		color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
				//	#else
				//		color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
				//	#endif
				//#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				#if defined(_ASE_TINT)
					color.rgb *= Tint;
				#endif

				//Shadowood: add custom fog, tonemapping, debug visuals
				#ifdef ASE_FOGCOLOR 
					color.rgb = lerp(color.rgb,FogColor.rgb,FogColor.a);
				#else
					#ifdef ASE_FOG
						#ifdef TERRAIN_SPLAT_ADDPASS
							color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
						#else
							color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
							
							/*
							//half3 MixFogColor(half3 fragColor, half3 fogColor, half fogFactor)
							
							half3 fogColor = unity_FogColor.rgb;
							half fogFactor = IN.fogFactorAndVertexLight.x;
							half fogIntensity = saturate(ComputeFogIntensity(fogFactor));
							//fogIntensity = saturate(exp2(-fogFactor * fogFactor));
							half fogIntensity2 = saturate(1-saturate(pow(1-fogIntensity,18)));
							
							half fogIntensity3 = saturate(1-saturate(pow(1-fogIntensity,2)));
							
							fogIntensity = saturate(1-saturate(pow(1-fogIntensity,5)));
							//color.rgb = lerp( saturate(lerp(fogColor,fogColor*color.rgb,fogIntensity)) , color.rgb, fogIntensity);
							
							half3 fogColorB = fogColor*pow(fogIntensity3,1.2);
							
							
							color.rgb = lerp(lerp(fogColorB,lerp(fogColor*color.rgb*20,color.rgb,fogIntensity),fogIntensity2), MixFog(color.rgb, IN.fogFactorAndVertexLight.x),unity_FogColor.a);
							*/
						#endif
					#endif
				#endif
				
				//#if defined(_ASE_TONEMAPPING) && defined(_TONEMAPPING_ON)
				#if defined(_ASE_TONEMAPPING) //&& !defined(TONEMAPPINGELSEWHERE)//&& defined(GLOBALTONEMAPPING)
					color.rgb = ApplyTonemap(color.rgb, Tonemapping);
				#endif
				
				#if defined (_ASE_DEBUGVISUALS) && defined(_DEBUGVISUALS_ON)
					color.rgb = DebugVisuals;
					color.a = 1;
				#endif
				
				return color;
			}

			ENDHLSL
		} // Shadowood: End of "Forward" Pass

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off
			//ColorMask 0

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_FOG 1
			#define _ASE_SSSCONTROL 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define TONEMAPPINGELSEWHERE
			#define UNDERWATERELSEWHERE
			#define ASE_USING_SAMPLING_MACROS 1


			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

			#define SHADERPASS SHADERPASS_SHADOWCASTER
			
			//Shadowood
			#ifdef _ASE_ALPHATEST
				#pragma shader_feature_local _ _ALPHATEST_ON
			#endif
			//#ifdef SW_TESSELLATION
			//	#define ASE_TESSELLATION 1
			//	#pragma require tessellation tessHW
			//	#pragma hull HullFunction
			//	#pragma domain DomainFunction
			//	//#define ASE_DISTANCE_TESSELLATION
			//#endif

			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
			
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include_with_pragmas  "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
			
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#pragma shader_feature_local _USEMOSSNOISE_ON
			#pragma shader_feature_local MODED_MOSS MODED_MOES MODED_MODS MODED_OFF
			#pragma shader_feature_local _MOSS
			#pragma shader_feature_local _AUTONORMAL
			#pragma shader_feature_local LAYER_UNLIMITED LAYER_MAX2 LAYER_MAX3
			#pragma shader_feature_local TEXTURE_MAX4 TEXTURE_MAX8 TEXTURE_MAX12 TEXTURE_MAX16
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _MOSSMETALMODE_ON
			#pragma shader_feature_local _ALPHAMODE
			#pragma shader_feature_local _SIMPLESTOCHASTIC
			#pragma shader_feature_local _USESPLAT_ON
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USE_SSS_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON
			#define _UseMossPacked
			#define _DetailMapPacking


			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE) && (SHADER_TARGET >= 45)
				#define ASE_SV_DEPTH SV_DepthLessEqual
				#define ASE_SV_POSITION_QUALIFIERS linear noperspective centroid
			#else
				#define ASE_SV_DEPTH SV_Depth
				#define ASE_SV_POSITION_QUALIFIERS
			#endif

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				ASE_SV_POSITION_QUALIFIERS float4 clipPos : SV_POSITION;
				float4 clipPosV : TEXCOORD0;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 worldPos : TEXCOORD1;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD2;
				#endif				
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Tint3;
			half4 _DetailMapScale;
			half4 _SmoothnessRemap;
			half4 _DetailMaskRemap;
			half4 _MossAlbedoRemap;
			half4 _EmissionRemap;
			half4 _MossAlphaMaskMM;
			half4 _OcclusionRemap;
			half4 _DetailSmoothnessRemap;
			half4x4 _ProbeWorldToTexture;
			half4 _MetallicRemap;
			half4 _MossColor;
			half4 _EmissionColor;
			float4 _RevealMask_TexelSize;
			half4 _MossSmoothnessRemap;
			half4 _Tint1;
			half4 _MossFresnel;
			half4 _MossFresnelColor;
			half4 _Layer3EmissionColor;
			half4 _Layer1_ST;
			half4 _LayerSurfaceExp;
			half4 _Layer0_ST;
			half4 _Tint2;
			half4 _MossSlopeNormal_ST;
			half4 _BaseMap_ST;
			half4 _BaseColor;
			half4 _MossSlopeMM;
			half4 _Tint0;
			half4 _SSSColor;
			half3 _ProbeVolumeMin;
			half3 _MossDirection;
			half3 _ProbeVolumeSizeInv;
			half _MossDetailNormalMapScale;
			half _UseEmission;
			half _Occlusion;
			half _UseOcclusion;
			half _Metallic;
			half _UseMossFresnel;
			half _Smoothness;
			half _MossMetallic;
			half _Layer1Smoothness;
			half _Layer1Metallic;
			half _SSSDimAlbedo;
			half _NormalStrength_USE_SSS;
			half _SSScattering;
			half _SSSRadius;
			half _MossNormalStrength2;
			half _MossSlopeDistort;
			half _MossSlopeNormRotate;
			half _DebugScale;
			int _DebugVisual;
			half _ShadowThreshold;
			half _AlphaClip;
			half _AlphaClipThreshold;
			half _Surface;
			half _Dither;
			half _OcclusionMossMask;
			half _Layer0Smoothness;
			half _MossSmoothness;
			half _Layer0Metallic;
			half _Layer2Height;
			int _DETAIL;
			half _Layer0NormalStrength;
			int _MetalUV;
			int _Moss;
			half _UseMossMetalMap;
			int _MossMode;
			half _MossStochasticContrast;
			half _MossStochasticScale;
			float _MossScale;
			int _MossUV;
			half _MossMultAlbedo;
			int _UseColorMask;
			int _Bitmask;
			int _ZWrite;
			int _Cullmode;
			half _Blend;
			half _SrcBlend;
			half _DstBlend;
			half _Cutoff;
			int _Int0;
			int _BlendMode_UseColorMask;
			half _Layer1NormalStrength;
			half _MossBase;
			half _MetallicSpecGlossMap;
			half _DetailNormalMapScale;
			half _MossNormalStrength;
			half _UseNormalMap;
			half _DetailsOverMoss;
			half _MapContrast;
			half _MapContrastOffset;
			half _MossNormalAffectStrength;
			half _MossNormalSubtract;
			half _DetailAlbedoMapScale;
			half _MossNormalContrast;
			half _UseMossVertexMask;
			half _UseMossMaskWithAlpha;
			half _UseMossDirection;
			half _MossDirContrast;
			half _MossLevel;
			half _DetailNormalMapScale1;
			half _UseDetailMask;
			int _MODE;
			half _NormalStrength;
			half _SSShadcowMix;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			float _DebugCounter; //Shadowood
			half _EnvironmentReflections; // Shadowood
			CBUFFER_END

			// Property used by ScenePickingPass
			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			// Properties used by SceneSelectionPass
			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			TEXTURE2D_ARRAY(_ExtraArray);
			SAMPLER(sampler_ExtraArray);
			TEXTURE2D_ARRAY(_DiffuseArray);
			SAMPLER(sampler_DiffuseArray);
			TEXTURE2D_ARRAY(_NormalArray);
			SAMPLER(sampler_NormalArray);
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_Linear_Repeat_Aniso2);
			int AlphaToCoverage;


			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

			//#ifdef HAVE_VFX_MODIFICATION
			//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
			//#endif

			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			half3 VertVanisher3872( half3 vertex, half2 vtexcoord1, int bitMask )
			{
				 if(bitMask & (int)vtexcoord1.x){
					vertex *= (0.0 / 0.0);
				}
				return vertex;
			}
			
			inline float Dither4x4Bayer( int x, int y )
			{
				const float dither[ 16 ] = {
			 1,  9,  3, 11,
			13,  5, 15,  7,
			 4, 12,  2, 10,
			16,  8, 14,  6 };
				int r = y * 4 + x;
				return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
			}
			

			float3 _LightDirection;
			float3 _LightPosition;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				half3 vertex3872 = v.vertex.xyz;
				half2 vtexcoord13872 = v.ase_texcoord1.xy;
				int bitMask3872 = _Bitmask;
				half3 localVertVanisher3872 = VertVanisher3872( vertex3872 , vtexcoord13872 , bitMask3872 );
				half3 VertexOcclusionPosition3873 = localVertVanisher3872;
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOcclusionPosition3873;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.worldPos = positionWS;
				#endif

				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

				#if _CASTING_PUNCTUAL_LIGHT_SHADOW
					float3 lightDirectionWS = normalize(_LightPosition - positionWS);
				#else
					float3 lightDirectionWS = _LightDirection;
				#endif

				float4 clipPos = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, UNITY_NEAR_CLIP_VALUE);
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = clipPos;
				o.clipPosV = clipPos;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(	VertexOutput IN
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.worldPos;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				float4 ClipPos = IN.clipPosV;
				float4 ScreenPos = ComputeScreenPos( IN.clipPosV );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				half2 uv_BaseMap = IN.ase_texcoord3.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				half2 UVBasemap02088 = uv_BaseMap;
				half4 tex2DNode80 = SAMPLE_TEXTURE2D( _BaseMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half temp_output_3952_0 = ( tex2DNode80.a * _BaseColor.a );
				half4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				half2 clipScreen3958 = ase_screenPosNorm.xy * _ScreenParams.xy;
				half dither3958 = Dither4x4Bayer( fmod(clipScreen3958.x, 4), fmod(clipScreen3958.y, 4) );
				dither3958 = step( dither3958, temp_output_3952_0 );
				half lerpResult3962 = lerp( (float)AlphaToCoverage , 1.0 , _Surface);
				half Dither3961 = ( _Dither * ( 1.0 - lerpResult3962 ) );
				half lerpResult3959 = lerp( temp_output_3952_0 , dither3958 , Dither3961);
				#ifdef _ALPHATEST_ON
				half staticSwitch3334 = lerpResult3959;
				#else
				half staticSwitch3334 = 1.0;
				#endif
				half AlbedoAlpha84 = staticSwitch3334;
				
				half lerpResult3950 = lerp( 0.0 , _AlphaClipThreshold , _AlphaClip);
				half lerpResult3291 = lerp( lerpResult3950 , 0.0 , (float)AlphaToCoverage);
				half ClipCalc3294 = lerpResult3291;
				
				half ShadowThreshold2627 = _ShadowThreshold;
				

				float Alpha = AlbedoAlpha84;
				float AlphaClipThreshold = ClipCalc3294;
				float AlphaClipThresholdShadow = ShadowThreshold2627;

				#ifdef ASE_DEPTH_WRITE_ON
					float DepthValue = IN.clipPos.z;
				#endif

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			//ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_FOG 1
			#define _ASE_SSSCONTROL 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define TONEMAPPINGELSEWHERE
			#define UNDERWATERELSEWHERE
			#define ASE_USING_SAMPLING_MACROS 1


			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_DEPTHONLY
			
			//Shadowood
			#ifdef _ASE_ALPHATEST
				#pragma shader_feature_local _ _ALPHATEST_ON
			#endif
			//#ifdef SW_TESSELLATION
			//	#define ASE_TESSELLATION 1
			//	#pragma require tessellation tessHW
			//	#pragma hull HullFunction
			//	#pragma domain DomainFunction
			//	//#define ASE_DISTANCE_TESSELLATION
			//#else
			//	#undef ASE_DISTANCE_TESSELLATION
			//#endif

			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
			
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include_with_pragmas  "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#pragma shader_feature_local _USEMOSSNOISE_ON
			#pragma shader_feature_local MODED_MOSS MODED_MOES MODED_MODS MODED_OFF
			#pragma shader_feature_local _MOSS
			#pragma shader_feature_local _AUTONORMAL
			#pragma shader_feature_local LAYER_UNLIMITED LAYER_MAX2 LAYER_MAX3
			#pragma shader_feature_local TEXTURE_MAX4 TEXTURE_MAX8 TEXTURE_MAX12 TEXTURE_MAX16
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _MOSSMETALMODE_ON
			#pragma shader_feature_local _ALPHAMODE
			#pragma shader_feature_local _SIMPLESTOCHASTIC
			#pragma shader_feature_local _USESPLAT_ON
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USE_SSS_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON
			#define _UseMossPacked
			#define _DetailMapPacking


			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE) && (SHADER_TARGET >= 45)
				#define ASE_SV_DEPTH SV_DepthLessEqual
				#define ASE_SV_POSITION_QUALIFIERS linear noperspective centroid
			#else
				#define ASE_SV_DEPTH SV_Depth
				#define ASE_SV_POSITION_QUALIFIERS
			#endif

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				ASE_SV_POSITION_QUALIFIERS float4 clipPos : SV_POSITION;
				float4 clipPosV : TEXCOORD0;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD1;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Tint3;
			half4 _DetailMapScale;
			half4 _SmoothnessRemap;
			half4 _DetailMaskRemap;
			half4 _MossAlbedoRemap;
			half4 _EmissionRemap;
			half4 _MossAlphaMaskMM;
			half4 _OcclusionRemap;
			half4 _DetailSmoothnessRemap;
			half4x4 _ProbeWorldToTexture;
			half4 _MetallicRemap;
			half4 _MossColor;
			half4 _EmissionColor;
			float4 _RevealMask_TexelSize;
			half4 _MossSmoothnessRemap;
			half4 _Tint1;
			half4 _MossFresnel;
			half4 _MossFresnelColor;
			half4 _Layer3EmissionColor;
			half4 _Layer1_ST;
			half4 _LayerSurfaceExp;
			half4 _Layer0_ST;
			half4 _Tint2;
			half4 _MossSlopeNormal_ST;
			half4 _BaseMap_ST;
			half4 _BaseColor;
			half4 _MossSlopeMM;
			half4 _Tint0;
			half4 _SSSColor;
			half3 _ProbeVolumeMin;
			half3 _MossDirection;
			half3 _ProbeVolumeSizeInv;
			half _MossDetailNormalMapScale;
			half _UseEmission;
			half _Occlusion;
			half _UseOcclusion;
			half _Metallic;
			half _UseMossFresnel;
			half _Smoothness;
			half _MossMetallic;
			half _Layer1Smoothness;
			half _Layer1Metallic;
			half _SSSDimAlbedo;
			half _NormalStrength_USE_SSS;
			half _SSScattering;
			half _SSSRadius;
			half _MossNormalStrength2;
			half _MossSlopeDistort;
			half _MossSlopeNormRotate;
			half _DebugScale;
			int _DebugVisual;
			half _ShadowThreshold;
			half _AlphaClip;
			half _AlphaClipThreshold;
			half _Surface;
			half _Dither;
			half _OcclusionMossMask;
			half _Layer0Smoothness;
			half _MossSmoothness;
			half _Layer0Metallic;
			half _Layer2Height;
			int _DETAIL;
			half _Layer0NormalStrength;
			int _MetalUV;
			int _Moss;
			half _UseMossMetalMap;
			int _MossMode;
			half _MossStochasticContrast;
			half _MossStochasticScale;
			float _MossScale;
			int _MossUV;
			half _MossMultAlbedo;
			int _UseColorMask;
			int _Bitmask;
			int _ZWrite;
			int _Cullmode;
			half _Blend;
			half _SrcBlend;
			half _DstBlend;
			half _Cutoff;
			int _Int0;
			int _BlendMode_UseColorMask;
			half _Layer1NormalStrength;
			half _MossBase;
			half _MetallicSpecGlossMap;
			half _DetailNormalMapScale;
			half _MossNormalStrength;
			half _UseNormalMap;
			half _DetailsOverMoss;
			half _MapContrast;
			half _MapContrastOffset;
			half _MossNormalAffectStrength;
			half _MossNormalSubtract;
			half _DetailAlbedoMapScale;
			half _MossNormalContrast;
			half _UseMossVertexMask;
			half _UseMossMaskWithAlpha;
			half _UseMossDirection;
			half _MossDirContrast;
			half _MossLevel;
			half _DetailNormalMapScale1;
			half _UseDetailMask;
			int _MODE;
			half _NormalStrength;
			half _SSShadcowMix;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			float _DebugCounter; //Shadowood
			half _EnvironmentReflections; // Shadowood
			CBUFFER_END

			// Property used by ScenePickingPass
			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			// Properties used by SceneSelectionPass
			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			TEXTURE2D_ARRAY(_ExtraArray);
			SAMPLER(sampler_ExtraArray);
			TEXTURE2D_ARRAY(_DiffuseArray);
			SAMPLER(sampler_DiffuseArray);
			TEXTURE2D_ARRAY(_NormalArray);
			SAMPLER(sampler_NormalArray);
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_Linear_Repeat_Aniso2);
			int AlphaToCoverage;


			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

			//#ifdef HAVE_VFX_MODIFICATION
			//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
			//#endif

			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			half3 VertVanisher3872( half3 vertex, half2 vtexcoord1, int bitMask )
			{
				 if(bitMask & (int)vtexcoord1.x){
					vertex *= (0.0 / 0.0);
				}
				return vertex;
			}
			
			inline float Dither4x4Bayer( int x, int y )
			{
				const float dither[ 16 ] = {
			 1,  9,  3, 11,
			13,  5, 15,  7,
			 4, 12,  2, 10,
			16,  8, 14,  6 };
				int r = y * 4 + x;
				return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				half3 vertex3872 = v.vertex.xyz;
				half2 vtexcoord13872 = v.ase_texcoord1.xy;
				int bitMask3872 = _Bitmask;
				half3 localVertVanisher3872 = VertVanisher3872( vertex3872 , vtexcoord13872 , bitMask3872 );
				half3 VertexOcclusionPosition3873 = localVertVanisher3872;
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOcclusionPosition3873;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				o.clipPosV = positionCS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(	VertexOutput IN
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				float4 ClipPos = IN.clipPosV;
				float4 ScreenPos = ComputeScreenPos( IN.clipPosV );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				half2 uv_BaseMap = IN.ase_texcoord3.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				half2 UVBasemap02088 = uv_BaseMap;
				half4 tex2DNode80 = SAMPLE_TEXTURE2D( _BaseMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half temp_output_3952_0 = ( tex2DNode80.a * _BaseColor.a );
				half4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				half2 clipScreen3958 = ase_screenPosNorm.xy * _ScreenParams.xy;
				half dither3958 = Dither4x4Bayer( fmod(clipScreen3958.x, 4), fmod(clipScreen3958.y, 4) );
				dither3958 = step( dither3958, temp_output_3952_0 );
				half lerpResult3962 = lerp( (float)AlphaToCoverage , 1.0 , _Surface);
				half Dither3961 = ( _Dither * ( 1.0 - lerpResult3962 ) );
				half lerpResult3959 = lerp( temp_output_3952_0 , dither3958 , Dither3961);
				#ifdef _ALPHATEST_ON
				half staticSwitch3334 = lerpResult3959;
				#else
				half staticSwitch3334 = 1.0;
				#endif
				half AlbedoAlpha84 = staticSwitch3334;
				
				half lerpResult3950 = lerp( 0.0 , _AlphaClipThreshold , _AlphaClip);
				half lerpResult3291 = lerp( lerpResult3950 , 0.0 , (float)AlphaToCoverage);
				half ClipCalc3294 = lerpResult3291;
				

				float Alpha = AlbedoAlpha84;
				float AlphaClipThreshold = ClipCalc3294;
				#ifdef ASE_DEPTH_WRITE_ON
					float DepthValue = IN.clipPos.z;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_FOG 1
			#define _ASE_SSSCONTROL 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define TONEMAPPINGELSEWHERE
			#define UNDERWATERELSEWHERE
			#define ASE_USING_SAMPLING_MACROS 1


			#pragma vertex vert
			#pragma fragment frag

			#pragma shader_feature EDITOR_VISUALIZATION

			#define SHADERPASS SHADERPASS_META
			
			//Shadowood	
			#ifdef _ASE_ALPHATEST
				#pragma shader_feature_local _ _ALPHATEST_ON
			#endif
			//#ifdef SW_TESSELLATION
			//	#define ASE_TESSELLATION 1
			//	#pragma require tessellation tessHW
			//	#pragma hull HullFunction
			//	#pragma domain DomainFunction
			//	//#define ASE_DISTANCE_TESSELLATION
			//#endif

			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
			
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include_with_pragmas  "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Assets/AmplifyStochasticNode/StochasticSampling.cginc"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _USEMOSSNOISE_ON
			#pragma shader_feature_local MODED_MOSS MODED_MOES MODED_MODS MODED_OFF
			#pragma shader_feature_local _MOSS
			#pragma shader_feature_local _AUTONORMAL
			#pragma shader_feature_local LAYER_UNLIMITED LAYER_MAX2 LAYER_MAX3
			#pragma shader_feature_local TEXTURE_MAX4 TEXTURE_MAX8 TEXTURE_MAX12 TEXTURE_MAX16
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _MOSSMETALMODE_ON
			#pragma shader_feature_local _ALPHAMODE
			#pragma shader_feature_local _SIMPLESTOCHASTIC
			#pragma shader_feature_local _USESPLAT_ON
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USE_SSS_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON
			#pragma shader_feature_local _REVEALLAYERS
			#pragma shader_feature_local _DETAIL_ON
			#pragma shader_feature _COLORMASK_ON
			#pragma shader_feature_local _REMAPPERS_ON
			#pragma shader_feature_local _USESTOCHASTICMOSS_ON
			#pragma shader_feature_local _USEMOSSFRESNEL_ON
			#pragma shader_feature_local _USECAUSTICSFROMABOVE_ON
			#pragma shader_feature_local _USECAUSTICEXTRASAMPLER_ON
			#pragma shader_feature_local _USECAUSTICRAINBOW_ON
			#pragma shader_feature_local _FadeWithHeight
			#define _UseMossPacked
			#define _DetailMapPacking


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				half4 ase_tangent : TANGENT;
				half4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef EDITOR_VISUALIZATION
					float4 VizUV : TEXCOORD2;
					float4 LightCoord : TEXCOORD3;
				#endif
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				float4 ase_texcoord10 : TEXCOORD10;
				float4 ase_texcoord11 : TEXCOORD11;
				float4 ase_texcoord12 : TEXCOORD12;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Tint3;
			half4 _DetailMapScale;
			half4 _SmoothnessRemap;
			half4 _DetailMaskRemap;
			half4 _MossAlbedoRemap;
			half4 _EmissionRemap;
			half4 _MossAlphaMaskMM;
			half4 _OcclusionRemap;
			half4 _DetailSmoothnessRemap;
			half4x4 _ProbeWorldToTexture;
			half4 _MetallicRemap;
			half4 _MossColor;
			half4 _EmissionColor;
			float4 _RevealMask_TexelSize;
			half4 _MossSmoothnessRemap;
			half4 _Tint1;
			half4 _MossFresnel;
			half4 _MossFresnelColor;
			half4 _Layer3EmissionColor;
			half4 _Layer1_ST;
			half4 _LayerSurfaceExp;
			half4 _Layer0_ST;
			half4 _Tint2;
			half4 _MossSlopeNormal_ST;
			half4 _BaseMap_ST;
			half4 _BaseColor;
			half4 _MossSlopeMM;
			half4 _Tint0;
			half4 _SSSColor;
			half3 _ProbeVolumeMin;
			half3 _MossDirection;
			half3 _ProbeVolumeSizeInv;
			half _MossDetailNormalMapScale;
			half _UseEmission;
			half _Occlusion;
			half _UseOcclusion;
			half _Metallic;
			half _UseMossFresnel;
			half _Smoothness;
			half _MossMetallic;
			half _Layer1Smoothness;
			half _Layer1Metallic;
			half _SSSDimAlbedo;
			half _NormalStrength_USE_SSS;
			half _SSScattering;
			half _SSSRadius;
			half _MossNormalStrength2;
			half _MossSlopeDistort;
			half _MossSlopeNormRotate;
			half _DebugScale;
			int _DebugVisual;
			half _ShadowThreshold;
			half _AlphaClip;
			half _AlphaClipThreshold;
			half _Surface;
			half _Dither;
			half _OcclusionMossMask;
			half _Layer0Smoothness;
			half _MossSmoothness;
			half _Layer0Metallic;
			half _Layer2Height;
			int _DETAIL;
			half _Layer0NormalStrength;
			int _MetalUV;
			int _Moss;
			half _UseMossMetalMap;
			int _MossMode;
			half _MossStochasticContrast;
			half _MossStochasticScale;
			float _MossScale;
			int _MossUV;
			half _MossMultAlbedo;
			int _UseColorMask;
			int _Bitmask;
			int _ZWrite;
			int _Cullmode;
			half _Blend;
			half _SrcBlend;
			half _DstBlend;
			half _Cutoff;
			int _Int0;
			int _BlendMode_UseColorMask;
			half _Layer1NormalStrength;
			half _MossBase;
			half _MetallicSpecGlossMap;
			half _DetailNormalMapScale;
			half _MossNormalStrength;
			half _UseNormalMap;
			half _DetailsOverMoss;
			half _MapContrast;
			half _MapContrastOffset;
			half _MossNormalAffectStrength;
			half _MossNormalSubtract;
			half _DetailAlbedoMapScale;
			half _MossNormalContrast;
			half _UseMossVertexMask;
			half _UseMossMaskWithAlpha;
			half _UseMossDirection;
			half _MossDirContrast;
			half _MossLevel;
			half _DetailNormalMapScale1;
			half _UseDetailMask;
			int _MODE;
			half _NormalStrength;
			half _SSShadcowMix;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			float _DebugCounter; //Shadowood
			half _EnvironmentReflections; // Shadowood
			CBUFFER_END

			// Property used by ScenePickingPass
			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			// Properties used by SceneSelectionPass
			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			TEXTURE2D_ARRAY(_ExtraArray);
			SAMPLER(sampler_ExtraArray);
			TEXTURE2D_ARRAY(_DiffuseArray);
			SAMPLER(sampler_DiffuseArray);
			TEXTURE2D_ARRAY(_NormalArray);
			SAMPLER(sampler_NormalArray);
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_Linear_Repeat_Aniso2);
			TEXTURE2D(_ColorMask);
			TEXTURE2D(_MossPacked);
			SAMPLER(sampler_MossPacked);
			TEXTURE2D(_MossMetalMap);
			TEXTURE2D(_DetailMapPacked);
			TEXTURE2D(_MetallicGlossMap);
			TEXTURE2D(_BumpMap);
			TEXTURE2D(_Layer0);
			TEXTURE2D(_RevealMask);
			TEXTURE2D(_LayerMask);
			TEXTURE2D(_Layer1);
			TEXTURE2D(_Layer0NormalMap);
			TEXTURE2D(_Layer1NormalMap);
			half4x4 _CausticMatrix;
			SAMPLER(sampler_Linear_Repeat);
			half4 _CausticsSettings;
			int AlphaToCoverage;


			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"

			//#ifdef HAVE_VFX_MODIFICATION
			//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
			//#endif

			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			half3 VertVanisher3872( half3 vertex, half2 vtexcoord1, int bitMask )
			{
				 if(bitMask & (int)vtexcoord1.x){
					vertex *= (0.0 / 0.0);
				}
				return vertex;
			}
			
			half MyCustomExpression3902( half detailAlbedo, half scale )
			{
				return half(2.0) * detailAlbedo * scale - scale + half(1.0);
			}
			
			inline half3 SampleSHPixel3558( half3 SH, half3 normalWS )
			{
				return SampleSHPixel( SH, normalWS );
			}
			
			inline half3 SampleLightmap3567( half2 lightmapUV, half3 normalWS )
			{
				return SampleLightmap( lightmapUV, 0, normalWS );
			}
			
			half3 RevealMaskNormalCrossFilter83_g4236( half2 uv, half height, half2 texelSize, SamplerState sampler_RevealMask )
			{
						//float2 texelSize = float2(1.0 / texWidth, 1.0 / texHeight);
						float4 h;
						h[0] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0, -1) * texelSize).b) * height;
						h[1] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(-1, 0) * texelSize).b) * height;
						h[2] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(1, 0) * texelSize).b) * height;
						h[3] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0, 1) * texelSize).b) * height;
						float3 n;
						n.z = 2;
						n.x = h[1] - h[2];
						n.y = h[0] - h[3];
						return normalize(n);
			}
			
			half3 MatrixMulThatWorks( half4 Pos, half4x4 Mat )
			{
				float3 result = mul( Mat, float4(Pos.xyz,1.0) ).xyz;
				return result + float3(Mat[0][3],Mat[1][3],Mat[2][3]);
			}
			
			half ShaderAPIMobile151_g4260( half desktop, half mobile )
			{
				#if SHADER_API_MOBILE
					return desktop;
				#else
					return mobile;
				#endif
			}
			
			half4x4 Inverse4x4(half4x4 input)
			{
				#define minor(a,b,c) determinant(half3x3(input.a, input.b, input.c))
				half4x4 cofactors = half4x4(
				minor( _22_23_24, _32_33_34, _42_43_44 ),
				-minor( _21_23_24, _31_33_34, _41_43_44 ),
				minor( _21_22_24, _31_32_34, _41_42_44 ),
				-minor( _21_22_23, _31_32_33, _41_42_43 ),
			
				-minor( _12_13_14, _32_33_34, _42_43_44 ),
				minor( _11_13_14, _31_33_34, _41_43_44 ),
				-minor( _11_12_14, _31_32_34, _41_42_44 ),
				minor( _11_12_13, _31_32_33, _41_42_43 ),
			
				minor( _12_13_14, _22_23_24, _42_43_44 ),
				-minor( _11_13_14, _21_23_24, _41_43_44 ),
				minor( _11_12_14, _21_22_24, _41_42_44 ),
				-minor( _11_12_13, _21_22_23, _41_42_43 ),
			
				-minor( _12_13_14, _22_23_24, _32_33_34 ),
				minor( _11_13_14, _21_23_24, _31_33_34 ),
				-minor( _11_12_14, _21_22_24, _31_32_34 ),
				minor( _11_12_13, _21_22_23, _31_32_33 ));
				#undef minor
				return transpose( cofactors ) / determinant( input );
			}
			
			half ShaderAPIMobile153_g4260( half desktop, half mobile )
			{
				#if SHADER_API_MOBILE
					return desktop;
				#else
					return mobile;
				#endif
			}
			
			inline half4 GetUnderWaterFogs240_g4251( half3 viewDir, float3 camWorldPos, half3 posWS, half4 oceanFogDensities, half oceanHeight, half4 oceanFogTop_RGB_Exponent, half4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			
			inline float Dither4x4Bayer( int x, int y )
			{
				const float dither[ 16 ] = {
			 1,  9,  3, 11,
			13,  5, 15,  7,
			 4, 12,  2, 10,
			16,  8, 14,  6 };
				int r = y * 4 + x;
				return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				half3 vertex3872 = v.vertex.xyz;
				half2 vtexcoord13872 = v.texcoord1.xy;
				int bitMask3872 = _Bitmask;
				half3 localVertVanisher3872 = VertVanisher3872( vertex3872 , vtexcoord13872 , bitMask3872 );
				half3 VertexOcclusionPosition3873 = localVertVanisher3872;
				
				half4 ifLocalVars3708 = 0;
				half2 texCoord117 = v.texcoord0.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==0){ifLocalVars3708 = half4( texCoord117, 0.0 , 0.0 ); };
				half2 texCoord3710 = v.texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==1){ifLocalVars3708 = half4( texCoord3710, 0.0 , 0.0 ); };
				half2 texCoord3712 = v.texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==2){ifLocalVars3708 = half4( texCoord3712, 0.0 , 0.0 ); };
				half2 appendResult3711 = (half2(ifLocalVars3708.xy));
				half2 vertexToFrag3370 = ( appendResult3711 * ( 1.0 / _MossScale ) );
				o.ase_texcoord4.zw = vertexToFrag3370;
				int MossInt2938 = _Moss;
				int MossModeInt3200 = _MossMode;
				#ifdef _MOSSMETALMODE_ON
				half staticSwitch3230 = (float)( MossInt2938 * MossModeInt3200 );
				#else
				half staticSwitch3230 = (float)MossInt2938;
				#endif
				half2 MossUVScaled1663 = vertexToFrag3370;
				half4 ifLocalVars3717 = 0;
				half2 uv_BaseMap = v.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==0){ifLocalVars3717 = half4( uv_BaseMap, 0.0 , 0.0 ); };
				half2 uv2_BaseMap = v.texcoord1.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==1){ifLocalVars3717 = half4( uv2_BaseMap, 0.0 , 0.0 ); };
				half2 uv3_BaseMap = v.texcoord2.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==2){ifLocalVars3717 = half4( uv3_BaseMap, 0.0 , 0.0 ); };
				half2 appendResult3718 = (half2(ifLocalVars3717.xy));
				half2 vertexToFrag3723 = appendResult3718;
				half2 UVBasemapPicked3719 = vertexToFrag3723;
				half2 ifLocalVar2916 = 0;
				if( staticSwitch3230 <= 0.0 )
				ifLocalVar2916 = UVBasemapPicked3719;
				else
				ifLocalVar2916 = MossUVScaled1663;
				half2 vertexToFrag3371 = ifLocalVar2916;
				o.ase_texcoord5.xy = vertexToFrag3371;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				half4 _DetailDistanceHardcoded = half4(25,30,0,0);
				half temp_output_3155_0 = saturate( (1.0 + (distance( _WorldSpaceCameraPos , ase_worldPos ) - _DetailDistanceHardcoded.x) * (0.0 - 1.0) / (_DetailDistanceHardcoded.y - _DetailDistanceHardcoded.x)) );
				half vertexToFrag3157 = temp_output_3155_0;
				o.ase_texcoord5.z = vertexToFrag3157;
				half2 texCoord1007 = v.texcoord0.xy * float2( 1,1 ) + float2( 0,0 );
				half2 appendResult2490 = (half2(_DetailMapScale.x , _DetailMapScale.y));
				half2 appendResult2491 = (half2(_DetailMapScale.z , _DetailMapScale.w));
				half2 vertexToFrag3400 = (texCoord1007*appendResult2490 + appendResult2491);
				o.ase_texcoord6.xy = vertexToFrag3400;
				o.ase_texcoord6.zw = vertexToFrag3723;
				half3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord7.xyz = ase_worldTangent;
				half3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord8.xyz = ase_worldNormal;
				half ase_vertexTangentSign = v.ase_tangent.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord9.xyz = ase_worldBitangent;
				half temp_output_225_0 = ( ( v.ase_color.r - 0.5 ) * 2.0 );
				half ifLocalVar4428 = 0;
				if( _UseMossVertexMask > 0.0 )
				ifLocalVar4428 = temp_output_225_0;
				half vertexToFrag3925 = ifLocalVar4428;
				o.ase_texcoord5.w = vertexToFrag3925;
				
				half3 _Vector3 = half3(0,0,-1);
				half4 Pos6_g4262 = half4( _Vector3 , 0.0 );
				half4x4 Mat6_g4262 = _CausticMatrix;
				half3 localMatrixMulThatWorks6_g4262 = MatrixMulThatWorks( Pos6_g4262 , Mat6_g4262 );
				half3 normalizeResult147_g4260 = normalize( localMatrixMulThatWorks6_g4262 );
				half3 vertexToFrag144_g4260 = normalizeResult147_g4260;
				o.ase_texcoord10.xyz = vertexToFrag144_g4260;
				half3 WorldPosition256_g4237 = ase_worldPos;
				half3 temp_output_105_0_g4260 = WorldPosition256_g4237;
				half4 Pos6_g4261 = half4( temp_output_105_0_g4260 , 0.0 );
				half4x4 invertVal146_g4260 = Inverse4x4( _CausticMatrix );
				half4x4 Mat6_g4261 = invertVal146_g4260;
				half3 localMatrixMulThatWorks6_g4261 = MatrixMulThatWorks( Pos6_g4261 , Mat6_g4261 );
				half2 vertexToFrag52_g4260 = (localMatrixMulThatWorks6_g4261).xy;
				o.ase_texcoord11.xy = vertexToFrag52_g4260;
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord12 = screenPos;
				
				o.ase_texcoord4.xy = v.texcoord0.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.w = 0;
				o.ase_texcoord8.w = 0;
				o.ase_texcoord9.w = 0;
				o.ase_texcoord10.w = 0;
				o.ase_texcoord11.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOcclusionPosition3873;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.worldPos = positionWS;
				#endif

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );

				#ifdef EDITOR_VISUALIZATION
					float2 VizUV = 0;
					float4 LightCoord = 0;
					UnityEditorVizData(v.vertex.xyz, v.texcoord0.xy, v.texcoord1.xy, v.texcoord2.xy, VizUV, LightCoord);
					o.VizUV = float4(VizUV, 0, 0);
					o.LightCoord = LightCoord;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				half4 ase_tangent : TANGENT;
				half4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.texcoord0 = v.texcoord0;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_tangent = v.ase_tangent;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.texcoord0 = patch[0].texcoord0 * bary.x + patch[1].texcoord0 * bary.y + patch[2].texcoord0 * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.worldPos;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				half2 uv_BaseMap = IN.ase_texcoord4.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				half2 UVBasemap02088 = uv_BaseMap;
				half4 tex2DNode80 = SAMPLE_TEXTURE2D( _BaseMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half3 temp_output_82_0 = ( (tex2DNode80).rgb * (_BaseColor).rgb );
				half3 temp_output_17_0_g2518 = temp_output_82_0;
				half4 ifLocalVars72_g2518 = 0;
				int clampResult74_g2518 = clamp( _BlendMode_UseColorMask , 0 , 1 );
				half4 tex2DNode1_g2518 = SAMPLE_TEXTURE2D( _ColorMask, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half temp_output_2_0_g2518 = ( 1.0 - tex2DNode1_g2518.a );
				half3 blendOpSrc26_g2518 = temp_output_17_0_g2518;
				half3 blendOpDest26_g2518 = ( ( (_Tint0).rgb * tex2DNode1_g2518.r * 1.0 ) + ( (_Tint1).rgb * tex2DNode1_g2518.g * 1.0 ) + ( (_Tint2).rgb * tex2DNode1_g2518.b * 1.0 ) + ( (_Tint3).rgb * temp_output_2_0_g2518 * 1.0 ) );
				half3 temp_output_26_0_g2518 = ( saturate( ( blendOpSrc26_g2518 * blendOpDest26_g2518 ) ));
				if(clampResult74_g2518==0){ifLocalVars72_g2518 = half4( temp_output_26_0_g2518 , 0.0 ); };
				half4 ifLocalVars29_g2518 = 0;
				if(_BlendMode_UseColorMask==0){ifLocalVars29_g2518 = half4( half3(0,0,0) , 0.0 ); };
				if(_BlendMode_UseColorMask==1){ifLocalVars29_g2518 = half4( temp_output_26_0_g2518 , 0.0 ); };
				half3 blendOpSrc27_g2518 = temp_output_17_0_g2518;
				half3 blendOpDest27_g2518 = ( ( (_Tint0).rgb * tex2DNode1_g2518.r * 1.0 ) + ( (_Tint1).rgb * tex2DNode1_g2518.g * 1.0 ) + ( (_Tint2).rgb * tex2DNode1_g2518.b * 1.0 ) + ( (_Tint3).rgb * temp_output_2_0_g2518 * 1.0 ) );
				if(_BlendMode_UseColorMask==2){ifLocalVars29_g2518 = half4( ( saturate( ( 1.0 - ( 1.0 - blendOpSrc27_g2518 ) * ( 1.0 - blendOpDest27_g2518 ) ) )) , 0.0 ); };
				half3 blendOpSrc28_g2518 = temp_output_17_0_g2518;
				half3 blendOpDest28_g2518 = ( ( (_Tint0).rgb * tex2DNode1_g2518.r * 1.0 ) + ( (_Tint1).rgb * tex2DNode1_g2518.g * 1.0 ) + ( (_Tint2).rgb * tex2DNode1_g2518.b * 1.0 ) + ( (_Tint3).rgb * temp_output_2_0_g2518 * 1.0 ) );
				if(_BlendMode_UseColorMask==3){ifLocalVars29_g2518 = half4( ( saturate( (( blendOpDest28_g2518 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest28_g2518 ) * ( 1.0 - blendOpSrc28_g2518 ) ) : ( 2.0 * blendOpDest28_g2518 * blendOpSrc28_g2518 ) ) )) , 0.0 ); };
				if(clampResult74_g2518==1){ifLocalVars72_g2518 = half4( (ifLocalVars29_g2518).xyz , 0.0 ); };
				half3 lerpResult64_g2518 = lerp( temp_output_17_0_g2518 , (ifLocalVars72_g2518).xyz , ( saturate( ( ( 1.0 * tex2DNode1_g2518.r ) + ( 1.0 * tex2DNode1_g2518.g ) + ( 1.0 * tex2DNode1_g2518.b ) + ( 1.0 * temp_output_2_0_g2518 ) ) ) * _UseColorMask ));
				#ifdef _COLORMASK_ON
				half3 staticSwitch15_g2518 = lerpResult64_g2518;
				#else
				half3 staticSwitch15_g2518 = temp_output_17_0_g2518;
				#endif
				half3 AlbedoColorMask2007 = staticSwitch15_g2518;
				half3 AlbedoBaseColor92 = AlbedoColorMask2007;
				half3 appendResult3785 = (half3(_MossColor.rgb));
				half2 vertexToFrag3370 = IN.ase_texcoord4.zw;
				half2 MossUVScaled1663 = vertexToFrag3370;
				half StochasticScale1447 = ( 1.0 / _MossStochasticScale );
				half StochasticContrast1448 = _MossStochasticContrast;
				half3 cw4270 = 0;
				float2 uv14270 = 0;
				float2 uv24270 = 0;
				float2 uv34270 = 0;
				float2 dx4270 = 0;
				float2 dy4270 = 0;
				half4 stochasticSample4270 = StochasticSample2DWeightsR(_MossPacked,sampler_MossPacked,MossUVScaled1663,cw4270,uv14270,uv24270,uv34270,dx4270,dy4270,StochasticScale1447,StochasticContrast1448);
				#ifdef _USESTOCHASTICMOSS_ON
				half4 staticSwitch121 = stochasticSample4270;
				#else
				half4 staticSwitch121 = SAMPLE_TEXTURE2D( _MossPacked, sampler_Linear_Repeat_Aniso2, MossUVScaled1663 );
				#endif
				int MossModeInt3200 = _MossMode;
				half2 vertexToFrag3371 = IN.ase_texcoord5.xy;
				half4 _Vector17 = half4(1,0,1,0);
				half4 ifLocalVar3181 = 0;
				if( _UseMossMetalMap <= 0.0 )
				ifLocalVar3181 = _Vector17;
				else
				ifLocalVar3181 = SAMPLE_TEXTURE2D( _MossMetalMap, sampler_Linear_Repeat_Aniso2, vertexToFrag3371 );
				half4 ifLocalVar3232 = 0;
				if( MossModeInt3200 <= 0.0 )
				ifLocalVar3232 = _Vector17;
				else
				ifLocalVar3232 = ifLocalVar3181;
				half4 MossMetalMap2943 = ifLocalVar3232;
				#ifdef _MOSSMETALMODE_ON
				half4 staticSwitch3303 = MossMetalMap2943;
				#else
				half4 staticSwitch3303 = staticSwitch121;
				#endif
				half4 MossMetalRes3077 = staticSwitch3303;
				half temp_output_3043_0 = (MossMetalRes3077).r;
				half temp_output_8_0_g2546 = temp_output_3043_0;
				half4 break5_g2546 = _MossAlbedoRemap;
				half temp_output_3_0_g2546 = (0.0 + (temp_output_8_0_g2546 - break5_g2546.x) * (1.0 - 0.0) / (break5_g2546.y - break5_g2546.x));
				half clampResult2_g2546 = clamp( temp_output_3_0_g2546 , break5_g2546.z , break5_g2546.w );
				half temp_output_27_0_g2546 = saturate( clampResult2_g2546 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3747 = temp_output_27_0_g2546;
				#else
				half staticSwitch3747 = temp_output_3043_0;
				#endif
				half3 MossPackedAlbedo1643 = ( appendResult3785 * staticSwitch3747 );
				half3 lerpResult2137 = lerp( MossPackedAlbedo1643 , ( MossPackedAlbedo1643 * AlbedoBaseColor92 ) , _MossMultAlbedo);
				half3 MossAlbedoTinted101 = lerpResult2137;
				half2 _Vector5 = half2(0,0);
				half2 NormalMossSigned50 = _Vector5;
				half3 appendResult3794 = (half3(NormalMossSigned50 , 1.0));
				half4 appendResult3336 = (half4(1.0 , 0.0 , 0.0 , 0.0));
				half vertexToFrag3157 = IN.ase_texcoord5.z;
				half DDistanceMaskVert3160 = step( 0.01 , vertexToFrag3157 );
				half2 vertexToFrag3400 = IN.ase_texcoord6.xy;
				half4 tex2DNode980 = SAMPLE_TEXTURE2D( _DetailMapPacked, sampler_Linear_Repeat_Aniso2, vertexToFrag3400 );
				half4 DetailMapPacked3088 = tex2DNode980;
				half temp_output_3090_0 = (DetailMapPacked3088).r;
				half detailAlbedo3902 = temp_output_3090_0;
				half scale3902 = _DetailAlbedoMapScale;
				half localMyCustomExpression3902 = MyCustomExpression3902( detailAlbedo3902 , scale3902 );
				half2 vertexToFrag3723 = IN.ase_texcoord6.zw;
				half2 UVBasemapPicked3719 = vertexToFrag3723;
				half4 _Vector16 = half4(0,1,1,1);
				half4 ifLocalVar3177 = 0;
				if( _MetallicSpecGlossMap <= 0.0 )
				ifLocalVar3177 = _Vector16;
				else
				ifLocalVar3177 = SAMPLE_TEXTURE2D( _MetallicGlossMap, sampler_Linear_Repeat_Aniso2, UVBasemapPicked3719 );
				half4 ifLocalVar3189 = 0;
				if( MossModeInt3200 <= 0.0 )
				ifLocalVar3189 = ifLocalVar3181;
				else
				ifLocalVar3189 = half4(1,1,1,1);
				half4 MossMetal2928 = ifLocalVar3189;
				#ifdef _MOSSMETALMODE_ON
				half4 staticSwitch2913 = MossMetal2928;
				#else
				half4 staticSwitch2913 = ifLocalVar3177;
				#endif
				half4 MossMetalORMetallicMap2911 = staticSwitch2913;
				half temp_output_3050_0 = (MossMetalORMetallicMap2911).b;
				half temp_output_8_0_g2548 = temp_output_3050_0;
				half4 ifLocalVars3100 = 0;
				int Mode3840 = _MODE;
				if(Mode3840==0){ifLocalVars3100 = _DetailMaskRemap; };
				if(Mode3840==1){ifLocalVars3100 = _EmissionRemap; };
				half4 break5_g2548 = ifLocalVars3100;
				half temp_output_3_0_g2548 = (0.0 + (temp_output_8_0_g2548 - break5_g2548.x) * (1.0 - 0.0) / (break5_g2548.y - break5_g2548.x));
				half clampResult2_g2548 = clamp( temp_output_3_0_g2548 , break5_g2548.z , break5_g2548.w );
				half temp_output_27_0_g2548 = saturate( clampResult2_g2548 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3738 = temp_output_27_0_g2548;
				#else
				half staticSwitch3738 = temp_output_3050_0;
				#endif
				int temp_output_3109_0 = ( 1.0 - Mode3840 );
				half temp_output_12_0_g2513 = ( temp_output_3109_0 * _UseDetailMask );
				half DetalMaskDoodad3103 = ( ( staticSwitch3738 * temp_output_12_0_g2513 ) + ( 1.0 - temp_output_12_0_g2513 ) );
				half temp_output_12_0_g2557 = DetalMaskDoodad3103;
				half temp_output_3327_0 = (DetailMapPacked3088).b;
				half temp_output_8_0_g2534 = temp_output_3327_0;
				half4 break5_g2534 = _DetailSmoothnessRemap;
				half temp_output_32_0_g2534 = (break5_g2534.z + (temp_output_8_0_g2534 - break5_g2534.x) * (break5_g2534.w - break5_g2534.z) / (break5_g2534.y - break5_g2534.x));
				#ifdef _REMAPPERS_ON
				half staticSwitch3743 = temp_output_32_0_g2534;
				#else
				half staticSwitch3743 = (-1.0 + (temp_output_3327_0 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0));
				#endif
				half4 appendResult3337 = (half4(( ( localMyCustomExpression3902 * temp_output_12_0_g2557 ) + ( 1.0 - temp_output_12_0_g2557 ) ) , ( ( (DetailMapPacked3088).ag * float2( 2,2 ) ) - float2( 1,1 ) ) , staticSwitch3743));
				half4 _DetailDistanceHardcoded = half4(25,30,0,0);
				half temp_output_3155_0 = saturate( (1.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - _DetailDistanceHardcoded.x) * (0.0 - 1.0) / (_DetailDistanceHardcoded.y - _DetailDistanceHardcoded.x)) );
				half DDistanceMaskPixel3159 = temp_output_3155_0;
				half4 lerpResult3339 = lerp( appendResult3336 , appendResult3337 , DDistanceMaskPixel3159);
				half4 ifLocalVar3338 = 0;
				UNITY_BRANCH 
				if( DDistanceMaskVert3160 <= 0.0 )
				ifLocalVar3338 = appendResult3336;
				else
				ifLocalVar3338 = lerpResult3339;
				#ifdef _DETAIL_ON
				half4 staticSwitch4130 = ifLocalVar3338;
				#else
				half4 staticSwitch4130 = appendResult3336;
				#endif
				half2 DetailNormalSigned3509 = ( (staticSwitch4130).yz * _DetailNormalMapScale1 );
				half3 appendResult3801 = (half3(( NormalMossSigned50 + DetailNormalSigned3509 ) , 1.0));
				half3 normalizeResult3795 = normalize( appendResult3801 );
				#ifdef _DETAIL_ON
				half3 staticSwitch4129 = normalizeResult3795;
				#else
				half3 staticSwitch4129 = appendResult3794;
				#endif
				half3 ase_worldTangent = IN.ase_texcoord7.xyz;
				half3 ase_worldNormal = IN.ase_texcoord8.xyz;
				float3 ase_worldBitangent = IN.ase_texcoord9.xyz;
				half3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				half3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				half3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal16 = staticSwitch4129;
				half3 worldNormal16 = float3(dot(tanToWorld0,tanNormal16), dot(tanToWorld1,tanNormal16), dot(tanToWorld2,tanNormal16));
				half dotResult15 = dot( worldNormal16 , _MossDirection );
				half temp_output_13_0_g2463 = dotResult15;
				half temp_output_82_0_g2463 = ( temp_output_13_0_g2463 * 0.5 );
				half temp_output_5_0_g2467 = ( temp_output_82_0_g2463 - -0.5 );
				half temp_output_48_0_g2467 = saturate( temp_output_5_0_g2467 );
				half temp_output_14_0_g2463 = _MossLevel;
				half temp_output_17_0_g2467 = temp_output_14_0_g2463;
				half temp_output_16_0_g2463 = ( 1.0 - _MossDirContrast );
				half temp_output_30_0_g2467 = ( 1.0 - saturate( ( 1.0 - temp_output_16_0_g2463 ) ) );
				half temp_output_35_0_g2467 = ( temp_output_30_0_g2467 * 0.5 );
				half temp_output_32_0_g2467 = ( (( 1.0 + temp_output_30_0_g2467 ) + (abs( temp_output_17_0_g2467 ) - 0.0) * (( 0.0 - temp_output_30_0_g2467 ) - ( 1.0 + temp_output_30_0_g2467 )) / (1.0 - 0.0)) - temp_output_35_0_g2467 );
				half temp_output_31_0_g2467 = ( temp_output_35_0_g2467 + (( 1.0 + temp_output_30_0_g2467 ) + (abs( temp_output_17_0_g2467 ) - 0.0) * (( 0.0 - temp_output_30_0_g2467 ) - ( 1.0 + temp_output_30_0_g2467 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2467 = saturate( (0.0 + (temp_output_48_0_g2467 - temp_output_32_0_g2467) * (1.0 - 0.0) / (temp_output_31_0_g2467 - temp_output_32_0_g2467)) );
				half temp_output_3796_0 = temp_output_51_0_g2467;
				half lerpResult4109 = lerp( _MossBase , temp_output_3796_0 , _UseMossDirection);
				half MossMaskDirection1179 = lerpResult4109;
				half temp_output_1811_0 = saturate( MossMaskDirection1179 );
				half temp_output_3952_0 = ( tex2DNode80.a * _BaseColor.a );
				#ifdef _ALPHATEST_ON
				half staticSwitch3756 = 1.0;
				#else
				half staticSwitch3756 = temp_output_3952_0;
				#endif
				half AlbedoAlphaMoss1953 = staticSwitch3756;
				half temp_output_8_0_g4229 = AlbedoAlphaMoss1953;
				half4 break5_g4229 = _MossAlphaMaskMM;
				half temp_output_32_0_g4229 = (break5_g4229.z + (temp_output_8_0_g4229 - break5_g4229.x) * (break5_g4229.w - break5_g4229.z) / (break5_g4229.y - break5_g4229.x));
				half clampResult31_g4229 = clamp( temp_output_32_0_g4229 , break5_g4229.z , break5_g4229.w );
				half temp_output_3896_0 = clampResult31_g4229;
				#ifdef _REMAPPERS_ON
				half staticSwitch3745 = temp_output_3896_0;
				#else
				half staticSwitch3745 = temp_output_3896_0;
				#endif
				half MossMaskAlpha1182 = ( staticSwitch3745 * _UseMossMaskWithAlpha );
				#ifdef _ALPHATEST_ON
				half staticSwitch3938 = temp_output_1811_0;
				#else
				half staticSwitch3938 = saturate( ( temp_output_1811_0 + MossMaskAlpha1182 ) );
				#endif
				half vertexToFrag3925 = IN.ase_texcoord5.w;
				half MossMaskVertex1181 = vertexToFrag3925;
				half4 tex2DNode32 = SAMPLE_TEXTURE2D( _BumpMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half3 unpack2617 = UnpackNormalScale( tex2DNode32, _NormalStrength );
				unpack2617.z = lerp( 1, unpack2617.z, saturate(_NormalStrength) );
				half2 break3507 = DetailNormalSigned3509;
				half dotResult3505 = dot( break3507.x , break3507.y );
				#ifdef _DETAIL_ON
				half staticSwitch3500 = ( unpack2617.z + abs( dotResult3505 ) );
				#else
				half staticSwitch3500 = unpack2617.z;
				#endif
				half NormalHeightSigned1759 = staticSwitch3500;
				half temp_output_5_0_g2451 = NormalHeightSigned1759;
				half temp_output_48_0_g2451 = saturate( temp_output_5_0_g2451 );
				half temp_output_17_0_g2451 = saturate( ( staticSwitch3938 + MossMaskVertex1181 ) );
				half temp_output_30_0_g2451 = ( 1.0 - saturate( _MossNormalContrast ) );
				half temp_output_35_0_g2451 = ( temp_output_30_0_g2451 * 0.5 );
				half temp_output_32_0_g2451 = ( (( 1.0 + temp_output_30_0_g2451 ) + (abs( temp_output_17_0_g2451 ) - 0.0) * (( 0.0 - temp_output_30_0_g2451 ) - ( 1.0 + temp_output_30_0_g2451 )) / (1.0 - 0.0)) - temp_output_35_0_g2451 );
				half temp_output_31_0_g2451 = ( temp_output_35_0_g2451 + (( 1.0 + temp_output_30_0_g2451 ) + (abs( temp_output_17_0_g2451 ) - 0.0) * (( 0.0 - temp_output_30_0_g2451 ) - ( 1.0 + temp_output_30_0_g2451 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2451 = saturate( (0.0 + (temp_output_48_0_g2451 - temp_output_32_0_g2451) * (1.0 - 0.0) / (temp_output_31_0_g2451 - temp_output_32_0_g2451)) );
				half temp_output_137_0_g2451 = ( 1.0 - temp_output_48_0_g2451 );
				half temp_output_128_0_g2451 = ( 1.0 - saturate( _MossNormalContrast ) );
				half temp_output_121_0_g2451 = ( temp_output_128_0_g2451 * 0.5 );
				half temp_output_118_0_g2451 = (0.0 + (temp_output_137_0_g2451 - ( (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) - temp_output_121_0_g2451 )) * (1.0 - 0.0) / (( temp_output_121_0_g2451 + (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) ) - ( (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) - temp_output_121_0_g2451 )));
				half lerpResult2905 = lerp( saturate( ( staticSwitch3938 + MossMaskVertex1181 ) ) , ( temp_output_51_0_g2451 - saturate( temp_output_118_0_g2451 ) ) , _MossNormalAffectStrength);
				half temp_output_5_0_g2498 = lerpResult2905;
				half temp_output_48_0_g2498 = saturate( temp_output_5_0_g2498 );
				half temp_output_17_0_g2498 = _MapContrastOffset;
				half temp_output_30_0_g2498 = ( 1.0 - saturate( _MapContrast ) );
				half temp_output_35_0_g2498 = ( temp_output_30_0_g2498 * 0.5 );
				half temp_output_32_0_g2498 = ( (( 1.0 + temp_output_30_0_g2498 ) + (abs( temp_output_17_0_g2498 ) - 0.0) * (( 0.0 - temp_output_30_0_g2498 ) - ( 1.0 + temp_output_30_0_g2498 )) / (1.0 - 0.0)) - temp_output_35_0_g2498 );
				half temp_output_31_0_g2498 = ( temp_output_35_0_g2498 + (( 1.0 + temp_output_30_0_g2498 ) + (abs( temp_output_17_0_g2498 ) - 0.0) * (( 0.0 - temp_output_30_0_g2498 ) - ( 1.0 + temp_output_30_0_g2498 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2498 = saturate( (0.0 + (temp_output_48_0_g2498 - temp_output_32_0_g2498) * (1.0 - 0.0) / (temp_output_31_0_g2498 - temp_output_32_0_g2498)) );
				#ifdef _MOSS
				half staticSwitch14 = temp_output_51_0_g2498;
				#else
				half staticSwitch14 = 0.0;
				#endif
				half MossMask45 = staticSwitch14;
				half3 lerpResult103 = lerp( AlbedoBaseColor92 , MossAlbedoTinted101 , MossMask45);
				#ifdef _MOSS
				half3 staticSwitch1236 = lerpResult103;
				#else
				half3 staticSwitch1236 = AlbedoBaseColor92;
				#endif
				half temp_output_10_0_g2209 = _DetailsOverMoss;
				half temp_output_17_0_g2209 = saturate( MossMask45 );
				half4 lerpResult7_g2209 = lerp( appendResult3336 , ifLocalVar3338 , saturate( ( saturate( ( abs( temp_output_10_0_g2209 ) - 1.0 ) ) + ( ( ( 1.0 - temp_output_17_0_g2209 ) * abs( min( temp_output_10_0_g2209 , 0.0 ) ) ) + ( temp_output_17_0_g2209 * max( temp_output_10_0_g2209 , 0.0 ) ) ) ) ));
				#ifdef _MOSS
				half4 staticSwitch4126 = lerpResult7_g2209;
				#else
				half4 staticSwitch4126 = ifLocalVar3338;
				#endif
				half4 lerpResult4232 = lerp( appendResult3336 , staticSwitch4126 , (float)_DETAIL);
				#ifdef _DETAIL_ON
				half4 staticSwitch4132 = lerpResult4232;
				#else
				half4 staticSwitch4132 = appendResult3336;
				#endif
				half DetailMapAlbedo987 = (staticSwitch4132).x;
				#ifdef _DETAIL_ON
				half3 staticSwitch1045 = saturate( ( staticSwitch1236 * DetailMapAlbedo987 ) );
				#else
				half3 staticSwitch1045 = staticSwitch1236;
				#endif
				half3 AlbedoRes106 = staticSwitch1045;
				half3 temp_output_35_0_g4236 = AlbedoRes106;
				half2 uv_Layer0 = IN.ase_texcoord4.xy * _Layer0_ST.xy + _Layer0_ST.zw;
				half2 uvLayer0112_g4236 = uv_Layer0;
				half3 appendResult37_g4236 = (half3(SAMPLE_TEXTURE2D( _Layer0, sampler_Linear_Repeat_Aniso2, uvLayer0112_g4236 ).rgb));
				half2 texCoord24_g4236 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				half2 baseuv25_g4236 = texCoord24_g4236;
				half4 tex2DNode3_g4236 = SAMPLE_TEXTURE2D( _RevealMask, sampler_Linear_Repeat_Aniso2, baseuv25_g4236 );
				half4 tex2DNode4_g4236 = SAMPLE_TEXTURE2D( _LayerMask, sampler_Linear_Repeat_Aniso2, baseuv25_g4236 );
				half rt31_g4236 = saturate( ( tex2DNode3_g4236.r * tex2DNode4_g4236.r ) );
				half LSAlbedo100_g4236 = _LayerSurfaceExp.x;
				half3 lerpResult34_g4236 = lerp( temp_output_35_0_g4236 , appendResult37_g4236 , pow( rt31_g4236 , LSAlbedo100_g4236 ));
				half2 uv_Layer1 = IN.ase_texcoord4.xy * _Layer1_ST.xy + _Layer1_ST.zw;
				half2 uvLayer1114_g4236 = uv_Layer1;
				half3 appendResult73_g4236 = (half3(SAMPLE_TEXTURE2D( _Layer1, sampler_Linear_Repeat_Aniso2, uvLayer1114_g4236 ).rgb));
				half gt57_g4236 = saturate( ( tex2DNode3_g4236.g * tex2DNode4_g4236.g ) );
				half3 lerpResult61_g4236 = lerp( lerpResult34_g4236 , appendResult73_g4236 , pow( gt57_g4236 , LSAlbedo100_g4236 ));
				#ifdef _REVEALLAYERS
				half3 staticSwitch1_g4236 = lerpResult61_g4236;
				#else
				half3 staticSwitch1_g4236 = temp_output_35_0_g4236;
				#endif
				half3 temp_output_4603_36 = staticSwitch1_g4236;
				
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				half fresnelNdotV135 = dot( ase_worldNormal, ase_worldViewDir );
				half fresnelNode135 = ( _MossFresnel.x + _MossFresnel.y * pow( 1.0 - fresnelNdotV135, ( _MossFresnel.z * 10.0 ) ) );
				half3 SH3558 = half4(0,0,0,0).xyz;
				half3 normalWS3558 = ase_worldNormal;
				half3 localSampleSHPixel3558 = SampleSHPixel3558( SH3558 , normalWS3558 );
				half2 lightmapUV3567 = half4(0,0,0,0).xy;
				half3 normalWS3567 = ase_worldNormal;
				half3 localSampleLightmap3567 = SampleLightmap3567( lightmapUV3567 , normalWS3567 );
				#ifdef LIGHTMAP_ON
				half3 staticSwitch3554 = localSampleLightmap3567;
				#else
				half3 staticSwitch3554 = localSampleSHPixel3558;
				#endif
				half3 LightmapOrSH2480 = staticSwitch3554;
				half grayscale4604 = (LightmapOrSH2480.r + LightmapOrSH2480.g + LightmapOrSH2480.b) / 3;
				half3 AlbedoResPre2289 = staticSwitch1045;
				half4 temp_output_2664_0 = ( ( ( min( _MossFresnel.w , saturate( fresnelNode135 ) ) * _MossFresnelColor * grayscale4604 ) * half4( AlbedoResPre2289 , 0.0 ) * saturate( ( MossMask45 - 0.3 ) ) ) * _UseMossFresnel );
				#ifdef _USEMOSSFRESNEL_ON
				half4 staticSwitch1195 = temp_output_2664_0;
				#else
				half4 staticSwitch1195 = float4( 0,0,0,0 );
				#endif
				#ifdef _MOSS
				half4 staticSwitch1237 = staticSwitch1195;
				#else
				half4 staticSwitch1237 = float4( 0,0,0,0 );
				#endif
				half4 MossFresnel203 = staticSwitch1237;
				half3 appendResult3069 = (half3(_EmissionColor.rgb));
				half _MetallicSpecGlossMap3184 = _MetallicSpecGlossMap;
				half temp_output_2918_0 = (MossMetalORMetallicMap2911).r;
				half temp_output_8_0_g2532 = temp_output_2918_0;
				half4 break5_g2532 = _MetallicRemap;
				half temp_output_3_0_g2532 = (0.0 + (temp_output_8_0_g2532 - break5_g2532.x) * (1.0 - 0.0) / (break5_g2532.y - break5_g2532.x));
				half clampResult2_g2532 = clamp( temp_output_3_0_g2532 , break5_g2532.z , break5_g2532.w );
				half temp_output_27_0_g2532 = saturate( clampResult2_g2532 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3732 = temp_output_27_0_g2532;
				#else
				half staticSwitch3732 = temp_output_2918_0;
				#endif
				half temp_output_3002_0 = (MossMetalORMetallicMap2911).g;
				half temp_output_8_0_g2550 = temp_output_3002_0;
				half4 break5_g2550 = _OcclusionRemap;
				half temp_output_3_0_g2550 = (0.0 + (temp_output_8_0_g2550 - break5_g2550.x) * (1.0 - 0.0) / (break5_g2550.y - break5_g2550.x));
				half clampResult2_g2550 = clamp( temp_output_3_0_g2550 , break5_g2550.z , break5_g2550.w );
				half temp_output_27_0_g2550 = saturate( clampResult2_g2550 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3735 = temp_output_27_0_g2550;
				#else
				half staticSwitch3735 = temp_output_3002_0;
				#endif
				half temp_output_12_0_g2502 = ( _UseOcclusion * _MetallicSpecGlossMap3184 * _Occlusion );
				half temp_output_12_0_g2514 = (float)Mode3840;
				half temp_output_392_0 = (MossMetalORMetallicMap2911).a;
				half temp_output_8_0_g2540 = temp_output_392_0;
				half4 break5_g2540 = _SmoothnessRemap;
				half temp_output_3_0_g2540 = (0.0 + (temp_output_8_0_g2540 - break5_g2540.x) * (1.0 - 0.0) / (break5_g2540.y - break5_g2540.x));
				half clampResult2_g2540 = clamp( temp_output_3_0_g2540 , break5_g2540.z , break5_g2540.w );
				half temp_output_27_0_g2540 = saturate( clampResult2_g2540 );
				half temp_output_3894_0 = temp_output_27_0_g2540;
				#ifdef _REMAPPERS_ON
				half staticSwitch3739 = temp_output_3894_0;
				#else
				half staticSwitch3739 = temp_output_392_0;
				#endif
				half4 appendResult3479 = (half4(( staticSwitch3732 * _Metallic ) , ( ( staticSwitch3735 * temp_output_12_0_g2502 ) + ( 1.0 - temp_output_12_0_g2502 ) ) , ( ( ( staticSwitch3738 * temp_output_12_0_g2514 ) + ( 1.0 - temp_output_12_0_g2514 ) ) * _UseEmission ) , ( staticSwitch3739 * _Smoothness )));
				half4 appendResult3481 = (half4(_Metallic , 1.0 , _UseEmission , _Smoothness));
				half4 ifLocalVar3480 = 0;
				if( _MetallicSpecGlossMap3184 <= 0.0 )
				ifLocalVar3480 = appendResult3481;
				else
				ifLocalVar3480 = appendResult3479;
				half UseMossMetalMapInt3239 = _UseMossMetalMap;
				half4 appendResult3487 = (half4(_Metallic , 1.0 , _UseEmission , _Smoothness));
				half4 appendResult3485 = (half4(( staticSwitch3732 * _Metallic ) , ( ( staticSwitch3735 * temp_output_12_0_g2502 ) + ( 1.0 - temp_output_12_0_g2502 ) ) , ( ( ( staticSwitch3738 * temp_output_12_0_g2514 ) + ( 1.0 - temp_output_12_0_g2514 ) ) * _UseEmission ) , ( staticSwitch3739 * _Smoothness )));
				half4 ifLocalVar3486 = 0;
				if( ( MossModeInt3200 + ( 1.0 - UseMossMetalMapInt3239 ) ) <= 0.0 )
				ifLocalVar3486 = appendResult3485;
				else
				ifLocalVar3486 = appendResult3487;
				#ifdef _MOSSMETALMODE_ON
				half4 staticSwitch3494 = ifLocalVar3486;
				#else
				half4 staticSwitch3494 = ifLocalVar3480;
				#endif
				half4 break3482 = staticSwitch3494;
				half EmissionMasked3065 = break3482.z;
				half3 temp_output_3496_0 = ( appendResult3069 * EmissionMasked3065 );
				half4 EmissionRes806 = ( MossFresnel203 + half4( temp_output_3496_0 , 0.0 ) );
				half3 temp_output_91_0_g4236 = EmissionRes806.rgb;
				half at79_g4236 = saturate( ( tex2DNode3_g4236.a * tex2DNode4_g4236.a ) );
				half3 appendResult99_g4236 = (half3(_Layer3EmissionColor.rgb));
				#ifdef _REVEALLAYERS
				half3 staticSwitch93_g4236 = ( ( temp_output_91_0_g4236 * ( 1.0 - at79_g4236 ) ) + ( appendResult99_g4236 * at79_g4236 ) );
				#else
				half3 staticSwitch93_g4236 = temp_output_91_0_g4236;
				#endif
				half OceanUnder289_g4237 = GlobalOceanUnder;
				half3 EmissionIn281_g4237 = float3( 0,0,0 );
				half3 WorldPosition256_g4237 = WorldPosition;
				half3 temp_output_105_0_g4260 = WorldPosition256_g4237;
				half temp_output_117_0_g4260 = (temp_output_105_0_g4260).y;
				half temp_output_67_0_g4237 = ( GlobalOceanOffset + GlobalOceanHeight );
				half OceanHeight274_g4237 = temp_output_67_0_g4237;
				half temp_output_63_0_g4260 = OceanHeight274_g4237;
				half3 appendResult2618 = (half3(unpack2617));
				half3 _Vector14 = half3(0,0,1);
				half3 ifLocalVar3262 = 0;
				if( _UseNormalMap <= 0.5 )
				ifLocalVar3262 = _Vector14;
				else
				ifLocalVar3262 = appendResult2618;
				half3 SimpleNormalXYSigned30 = ifLocalVar3262;
				int MossInt2938 = _Moss;
				half2 _Vector0 = half2(0,0);
				half2 temp_output_3045_0 = (MossMetalRes3077).ag;
				half2 MossPackedNormal1656 = temp_output_3045_0;
				half4 temp_output_5_0_g2453 = half4( MossPackedNormal1656, 0.0 , 0.0 );
				half2 appendResult12_g2453 = (half2(temp_output_5_0_g2453.xy));
				half temp_output_6_0_g2453 = _MossNormalStrength;
				half2 lerpResult2_g2453 = lerp( float2( 0,0 ) , ( appendResult12_g2453 - float2( 0.5,0.5 ) ) , ( temp_output_6_0_g2453 * 2.0 ));
				half2 lerpResult2976 = lerp( _Vector0 , lerpResult2_g2453 , MossMask45);
				half2 MossNormalSigned100 = lerpResult2976;
				half2 temp_output_3610_0 = ( MossInt2938 * MossNormalSigned100 );
				half3 appendResult4006 = (half3(temp_output_3610_0 , 1.0));
				half3 lerpResult4431 = lerp( SimpleNormalXYSigned30 , float3( 0,0,1 ) , MossMask45);
				half2 appendResult1363 = (half2(lerpResult4431.xy));
				#ifdef _MOSS
				half3 staticSwitch4131 = ( appendResult4006 + half3( appendResult1363 ,  0.0 ) );
				#else
				half3 staticSwitch4131 = SimpleNormalXYSigned30;
				#endif
				half2 DetailNormaMossMaskedSigned988 = ( _DetailNormalMapScale * (staticSwitch4132).yz );
				half3 appendResult4397 = (half3(DetailNormaMossMaskedSigned988 , 1.0));
				#ifdef _DETAIL_ON
				half3 staticSwitch1105 = BlendNormal( staticSwitch4131 , appendResult4397 );
				#else
				half3 staticSwitch1105 = staticSwitch4131;
				#endif
				half3 NormalPre2265 = staticSwitch1105;
				half3 normalizeResult4009 = normalize( NormalPre2265 );
				half3 NormalRes109 = normalizeResult4009;
				half3 temp_output_42_0_g4236 = NormalRes109;
				half3 unpack9_g4236 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Layer0NormalMap, sampler_Linear_Repeat_Aniso2, uvLayer0112_g4236 ), _Layer0NormalStrength );
				unpack9_g4236.z = lerp( 1, unpack9_g4236.z, saturate(_Layer0NormalStrength) );
				half LSNormal101_g4236 = _LayerSurfaceExp.y;
				half3 lerpResult41_g4236 = lerp( temp_output_42_0_g4236 , unpack9_g4236 , pow( rt31_g4236 , LSNormal101_g4236 ));
				half3 unpack18_g4236 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Layer1NormalMap, sampler_Linear_Repeat_Aniso2, uvLayer1114_g4236 ), _Layer1NormalStrength );
				unpack18_g4236.z = lerp( 1, unpack18_g4236.z, saturate(_Layer1NormalStrength) );
				half3 lerpResult64_g4236 = lerp( lerpResult41_g4236 , unpack18_g4236 , pow( gt57_g4236 , LSNormal101_g4236 ));
				half2 uv83_g4236 = baseuv25_g4236;
				half bt76_g4236 = saturate( ( tex2DNode3_g4236.b * tex2DNode4_g4236.b ) );
				half height83_g4236 = ( _Layer2Height * bt76_g4236 );
				half2 texelSize83_g4236 = (_RevealMask_TexelSize).xy;
				SamplerState sampler_RevealMask83_g4236 = sampler_Linear_Repeat_Aniso2;
				half3 localRevealMaskNormalCrossFilter83_g4236 = RevealMaskNormalCrossFilter83_g4236( uv83_g4236 , height83_g4236 , texelSize83_g4236 , sampler_RevealMask83_g4236 );
				half3 heightNormal89_g4236 = localRevealMaskNormalCrossFilter83_g4236;
				#ifdef _REVEALLAYERS
				half3 staticSwitch58_g4236 = BlendNormal( lerpResult64_g4236 , heightNormal89_g4236 );
				#else
				half3 staticSwitch58_g4236 = temp_output_42_0_g4236;
				#endif
				half3 temp_output_4603_43 = staticSwitch58_g4236;
				half3 temp_output_9_0_g4237 = temp_output_4603_43;
				half3 temp_output_30_0_g4260 = temp_output_9_0_g4237;
				float3 tanNormal12_g4260 = temp_output_30_0_g4260;
				half3 worldNormal12_g4260 = float3(dot(tanToWorld0,tanNormal12_g4260), dot(tanToWorld1,tanNormal12_g4260), dot(tanToWorld2,tanNormal12_g4260));
				half3 vertexToFrag144_g4260 = IN.ase_texcoord10.xyz;
				half dotResult1_g4260 = dot( worldNormal12_g4260 , vertexToFrag144_g4260 );
				half desktop151_g4260 = max( dotResult1_g4260 , 0.0 );
				half mobile151_g4260 = 1.0;
				half localShaderAPIMobile151_g4260 = ShaderAPIMobile151_g4260( desktop151_g4260 , mobile151_g4260 );
				half2 vertexToFrag52_g4260 = IN.ase_texcoord11.xy;
				half4 tex2DNode120_g4260 = SAMPLE_TEXTURE2D( _Caustics, sampler_Linear_Repeat, vertexToFrag52_g4260 );
				half3 appendResult121_g4260 = (half3(tex2DNode120_g4260.r , tex2DNode120_g4260.r , tex2DNode120_g4260.r));
				half2 DistanceFade134_g4260 = (_CausticsSettings).zw;
				half2 break136_g4260 = DistanceFade134_g4260;
				half temp_output_67_0_g4260 = ( saturate( (1.0 + (distance( temp_output_63_0_g4260 , temp_output_117_0_g4260 ) - break136_g4260.x) * (0.0 - 1.0) / (break136_g4260.y - break136_g4260.x)) ) * saturate( (0.0 + (max( -( temp_output_117_0_g4260 - temp_output_63_0_g4260 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) );
				half desktop153_g4260 = temp_output_67_0_g4260;
				half mobile153_g4260 = 1.0;
				half localShaderAPIMobile153_g4260 = ShaderAPIMobile153_g4260( desktop153_g4260 , mobile153_g4260 );
				half CausticMipLevel118_g4260 = ( ( 1.0 - localShaderAPIMobile153_g4260 ) * 4.0 );
				#ifdef _USECAUSTICRAINBOW_ON
				half3 staticSwitch73_g4260 = ( ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Linear_Repeat, ( vertexToFrag52_g4260 + ( 0.0045 * 2.0 ) ), CausticMipLevel118_g4260 ).r * half3(0,0,1) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Linear_Repeat, ( vertexToFrag52_g4260 + 0.0045 ), CausticMipLevel118_g4260 ).r * half3(0,1,0) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Linear_Repeat, vertexToFrag52_g4260, CausticMipLevel118_g4260 ).r * half3(1,0,0) ) );
				#else
				half3 staticSwitch73_g4260 = appendResult121_g4260;
				#endif
				#ifdef _USECAUSTICEXTRASAMPLER_ON
				half3 staticSwitch57_g4260 = staticSwitch73_g4260;
				#else
				half3 staticSwitch57_g4260 = staticSwitch73_g4260;
				#endif
				half3 appendResult62_g4260 = (half3(_CausticsColor.rgb));
				half3 ifLocalVar155_g4260 = 0;
				UNITY_BRANCH 
				if( temp_output_117_0_g4260 < ( temp_output_63_0_g4260 + 0.2 ) )
				ifLocalVar155_g4260 = ( localShaderAPIMobile151_g4260 * staticSwitch57_g4260 * appendResult62_g4260 * localShaderAPIMobile153_g4260 );
				half3 lerpResult205_g4237 = lerp( ( EmissionIn281_g4237 + ifLocalVar155_g4260 ) , ( ( EmissionIn281_g4237 + ifLocalVar155_g4260 ) * 30.0 ) , OceanUnder289_g4237);
				half3 appendResult100_g4251 = (half3(OceanWaterTint_RGB.xyz));
				half3 WorldPos252_g4251 = WorldPosition256_g4237;
				half temp_output_108_0_g4251 = OceanHeight274_g4237;
				half3 ViewDir264_g4237 = ase_worldViewDir;
				half3 viewDir240_g4251 = ViewDir264_g4237;
				half3 camWorldPos240_g4251 = _WorldSpaceCameraPos;
				half3 posWS240_g4251 = WorldPos252_g4251;
				half4 oceanFogDensities240_g4251 = OceanFogDensities;
				half oceanHeight240_g4251 = temp_output_108_0_g4251;
				half4 oceanFogTop_RGB_Exponent240_g4251 = OceanFogTop_RGB_Exponent;
				half4 oceanFogBottom_RGB_Intensity240_g4251 = OceanFogBottom_RGB_Intensity;
				half4 localGetUnderWaterFogs240_g4251 = GetUnderWaterFogs240_g4251( viewDir240_g4251 , camWorldPos240_g4251 , posWS240_g4251 , oceanFogDensities240_g4251 , oceanHeight240_g4251 , oceanFogTop_RGB_Exponent240_g4251 , oceanFogBottom_RGB_Intensity240_g4251 );
				half4 ifLocalVar257_g4251 = 0;
				UNITY_BRANCH 
				if( (WorldPos252_g4251).y < ( temp_output_108_0_g4251 + 0.2 ) )
				ifLocalVar257_g4251 = localGetUnderWaterFogs240_g4251;
				half4 FogRes185_g4251 = ifLocalVar257_g4251;
				half3 appendResult94_g4251 = (half3(FogRes185_g4251.xyz));
				half3 lerpResult36_g4251 = lerp( ( lerpResult205_g4237 * appendResult100_g4251 ) , appendResult94_g4251 , (FogRes185_g4251).w);
				half3 ifLocalVar5_g4237 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g4237 >= 1.0 )
				ifLocalVar5_g4237 = lerpResult36_g4251;
				else
				ifLocalVar5_g4237 = EmissionIn281_g4237;
				half temp_output_254_0_g4237 = (WorldPosition256_g4237).y;
				half temp_output_137_0_g4237 = ( temp_output_254_0_g4237 - OceanHeight274_g4237 );
				half temp_output_24_0_g4247 = temp_output_137_0_g4237;
				half temp_output_44_0_g4247 = 0.1;
				half temp_output_45_0_g4247 = 0.31;
				half temp_output_46_0_g4247 = saturate( (0.0 + (( temp_output_24_0_g4247 - temp_output_44_0_g4247 ) - 0.0) * (1.0 - 0.0) / (temp_output_45_0_g4247 - 0.0)) );
				#ifdef _FadeWithHeight
				half staticSwitch238_g4237 = ( 1.0 - temp_output_46_0_g4247 );
				#else
				half staticSwitch238_g4237 = 1.0;
				#endif
				half FadeFromY295_g4237 = staticSwitch238_g4237;
				half3 lerpResult174_g4237 = lerp( ( ifLocalVar5_g4237 + lerpResult205_g4237 ) , ifLocalVar5_g4237 , ( OceanUnder289_g4237 * FadeFromY295_g4237 ));
				half3 lerpResult242_g4237 = lerp( EmissionIn281_g4237 , lerpResult174_g4237 , FadeFromY295_g4237);
				#ifdef _USECAUSTICSFROMABOVE_ON
				half3 staticSwitch172_g4237 = lerpResult242_g4237;
				#else
				half3 staticSwitch172_g4237 = ifLocalVar5_g4237;
				#endif
				#ifdef _USEUNDERWATER
				half3 staticSwitch211_g4237 = staticSwitch172_g4237;
				#else
				half3 staticSwitch211_g4237 = float3( 0,0,0 );
				#endif
				half3 temp_output_4575_8 = staticSwitch211_g4237;
				
				float4 screenPos = IN.ase_texcoord12;
				half4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				half2 clipScreen3958 = ase_screenPosNorm.xy * _ScreenParams.xy;
				half dither3958 = Dither4x4Bayer( fmod(clipScreen3958.x, 4), fmod(clipScreen3958.y, 4) );
				dither3958 = step( dither3958, temp_output_3952_0 );
				half lerpResult3962 = lerp( (float)AlphaToCoverage , 1.0 , _Surface);
				half Dither3961 = ( _Dither * ( 1.0 - lerpResult3962 ) );
				half lerpResult3959 = lerp( temp_output_3952_0 , dither3958 , Dither3961);
				#ifdef _ALPHATEST_ON
				half staticSwitch3334 = lerpResult3959;
				#else
				half staticSwitch3334 = 1.0;
				#endif
				half AlbedoAlpha84 = staticSwitch3334;
				
				half lerpResult3950 = lerp( 0.0 , _AlphaClipThreshold , _AlphaClip);
				half lerpResult3291 = lerp( lerpResult3950 , 0.0 , (float)AlphaToCoverage);
				half ClipCalc3294 = lerpResult3291;
				

				float3 BaseColor = temp_output_4603_36;
				float3 Emission = ( staticSwitch93_g4236 + ( temp_output_4603_36 * temp_output_4575_8 ) );
				float Alpha = AlbedoAlpha84;
				float AlphaClipThreshold = ClipCalc3294;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = BaseColor;
				metaInput.Emission = Emission;
				#ifdef EDITOR_VISUALIZATION
					metaInput.VizUV = IN.VizUV.xy;
					metaInput.LightCoord = IN.LightCoord;
				#endif

				return UnityMetaFragment(metaInput);
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthNormals"
			Tags { "LightMode"="DepthNormalsOnly" }

			ZWrite On
			Blend One Zero
			ZTest LEqual
			ZWrite On
			AlphaToMask Off

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_FOG 1
			#define _ASE_SSSCONTROL 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define TONEMAPPINGELSEWHERE
			#define UNDERWATERELSEWHERE
			#define ASE_USING_SAMPLING_MACROS 1


			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
			
			//Shadowood
			#ifdef _ASE_ALPHATEST
				#pragma shader_feature_local _ _ALPHATEST_ON
			#endif
			//#ifdef SW_TESSELLATION
			//	#define ASE_TESSELLATION 1
			//	#pragma require tessellation tessHW
			//	#pragma hull HullFunction
			//	#pragma domain DomainFunction
			//	//#define ASE_DISTANCE_TESSELLATION
			//#endif
			
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
			
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include_with_pragmas  "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Assets/AmplifyStochasticNode/StochasticSampling.cginc"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_TANGENT
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#pragma shader_feature_local _USEMOSSNOISE_ON
			#pragma shader_feature_local MODED_MOSS MODED_MOES MODED_MODS MODED_OFF
			#pragma shader_feature_local _MOSS
			#pragma shader_feature_local _AUTONORMAL
			#pragma shader_feature_local LAYER_UNLIMITED LAYER_MAX2 LAYER_MAX3
			#pragma shader_feature_local TEXTURE_MAX4 TEXTURE_MAX8 TEXTURE_MAX12 TEXTURE_MAX16
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _MOSSMETALMODE_ON
			#pragma shader_feature_local _ALPHAMODE
			#pragma shader_feature_local _SIMPLESTOCHASTIC
			#pragma shader_feature_local _USESPLAT_ON
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USE_SSS_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON
			#pragma shader_feature_local _REVEALLAYERS
			#pragma shader_feature_local _DETAIL_ON
			#pragma shader_feature_local _USESTOCHASTICMOSS_ON
			#pragma shader_feature_local _REMAPPERS_ON
			#define _UseMossPacked
			#define _DetailMapPacking


			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE) && (SHADER_TARGET >= 45)
				#define ASE_SV_DEPTH SV_DepthLessEqual
				#define ASE_SV_POSITION_QUALIFIERS linear noperspective centroid
			#else
				#define ASE_SV_DEPTH SV_Depth
				#define ASE_SV_POSITION_QUALIFIERS
			#endif

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord2 : TEXCOORD2;
				half4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				ASE_SV_POSITION_QUALIFIERS float4 clipPos : SV_POSITION;
				float4 clipPosV : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				float4 worldTangent : TEXCOORD2;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 worldPos : TEXCOORD3;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD4;
				#endif
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Tint3;
			half4 _DetailMapScale;
			half4 _SmoothnessRemap;
			half4 _DetailMaskRemap;
			half4 _MossAlbedoRemap;
			half4 _EmissionRemap;
			half4 _MossAlphaMaskMM;
			half4 _OcclusionRemap;
			half4 _DetailSmoothnessRemap;
			half4x4 _ProbeWorldToTexture;
			half4 _MetallicRemap;
			half4 _MossColor;
			half4 _EmissionColor;
			float4 _RevealMask_TexelSize;
			half4 _MossSmoothnessRemap;
			half4 _Tint1;
			half4 _MossFresnel;
			half4 _MossFresnelColor;
			half4 _Layer3EmissionColor;
			half4 _Layer1_ST;
			half4 _LayerSurfaceExp;
			half4 _Layer0_ST;
			half4 _Tint2;
			half4 _MossSlopeNormal_ST;
			half4 _BaseMap_ST;
			half4 _BaseColor;
			half4 _MossSlopeMM;
			half4 _Tint0;
			half4 _SSSColor;
			half3 _ProbeVolumeMin;
			half3 _MossDirection;
			half3 _ProbeVolumeSizeInv;
			half _MossDetailNormalMapScale;
			half _UseEmission;
			half _Occlusion;
			half _UseOcclusion;
			half _Metallic;
			half _UseMossFresnel;
			half _Smoothness;
			half _MossMetallic;
			half _Layer1Smoothness;
			half _Layer1Metallic;
			half _SSSDimAlbedo;
			half _NormalStrength_USE_SSS;
			half _SSScattering;
			half _SSSRadius;
			half _MossNormalStrength2;
			half _MossSlopeDistort;
			half _MossSlopeNormRotate;
			half _DebugScale;
			int _DebugVisual;
			half _ShadowThreshold;
			half _AlphaClip;
			half _AlphaClipThreshold;
			half _Surface;
			half _Dither;
			half _OcclusionMossMask;
			half _Layer0Smoothness;
			half _MossSmoothness;
			half _Layer0Metallic;
			half _Layer2Height;
			int _DETAIL;
			half _Layer0NormalStrength;
			int _MetalUV;
			int _Moss;
			half _UseMossMetalMap;
			int _MossMode;
			half _MossStochasticContrast;
			half _MossStochasticScale;
			float _MossScale;
			int _MossUV;
			half _MossMultAlbedo;
			int _UseColorMask;
			int _Bitmask;
			int _ZWrite;
			int _Cullmode;
			half _Blend;
			half _SrcBlend;
			half _DstBlend;
			half _Cutoff;
			int _Int0;
			int _BlendMode_UseColorMask;
			half _Layer1NormalStrength;
			half _MossBase;
			half _MetallicSpecGlossMap;
			half _DetailNormalMapScale;
			half _MossNormalStrength;
			half _UseNormalMap;
			half _DetailsOverMoss;
			half _MapContrast;
			half _MapContrastOffset;
			half _MossNormalAffectStrength;
			half _MossNormalSubtract;
			half _DetailAlbedoMapScale;
			half _MossNormalContrast;
			half _UseMossVertexMask;
			half _UseMossMaskWithAlpha;
			half _UseMossDirection;
			half _MossDirContrast;
			half _MossLevel;
			half _DetailNormalMapScale1;
			half _UseDetailMask;
			int _MODE;
			half _NormalStrength;
			half _SSShadcowMix;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			float _DebugCounter; //Shadowood
			half _EnvironmentReflections; // Shadowood
			CBUFFER_END

			// Property used by ScenePickingPass
			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			// Properties used by SceneSelectionPass
			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			TEXTURE2D_ARRAY(_ExtraArray);
			SAMPLER(sampler_ExtraArray);
			TEXTURE2D_ARRAY(_DiffuseArray);
			SAMPLER(sampler_DiffuseArray);
			TEXTURE2D_ARRAY(_NormalArray);
			SAMPLER(sampler_NormalArray);
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_BumpMap);
			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_Linear_Repeat_Aniso2);
			TEXTURE2D(_MossPacked);
			SAMPLER(sampler_MossPacked);
			TEXTURE2D(_MossMetalMap);
			TEXTURE2D(_DetailMapPacked);
			TEXTURE2D(_MetallicGlossMap);
			TEXTURE2D(_Layer0NormalMap);
			TEXTURE2D(_Layer0);
			TEXTURE2D(_RevealMask);
			TEXTURE2D(_LayerMask);
			TEXTURE2D(_Layer1NormalMap);
			TEXTURE2D(_Layer1);
			int AlphaToCoverage;


			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"

			//#ifdef HAVE_VFX_MODIFICATION
			//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
			//#endif

			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			half3 VertVanisher3872( half3 vertex, half2 vtexcoord1, int bitMask )
			{
				 if(bitMask & (int)vtexcoord1.x){
					vertex *= (0.0 / 0.0);
				}
				return vertex;
			}
			
			half MyCustomExpression3902( half detailAlbedo, half scale )
			{
				return half(2.0) * detailAlbedo * scale - scale + half(1.0);
			}
			
			half3 RevealMaskNormalCrossFilter83_g4236( half2 uv, half height, half2 texelSize, SamplerState sampler_RevealMask )
			{
						//float2 texelSize = float2(1.0 / texWidth, 1.0 / texHeight);
						float4 h;
						h[0] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0, -1) * texelSize).b) * height;
						h[1] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(-1, 0) * texelSize).b) * height;
						h[2] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(1, 0) * texelSize).b) * height;
						h[3] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0, 1) * texelSize).b) * height;
						float3 n;
						n.z = 2;
						n.x = h[1] - h[2];
						n.y = h[0] - h[3];
						return normalize(n);
			}
			
			inline float Dither4x4Bayer( int x, int y )
			{
				const float dither[ 16 ] = {
			 1,  9,  3, 11,
			13,  5, 15,  7,
			 4, 12,  2, 10,
			16,  8, 14,  6 };
				int r = y * 4 + x;
				return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				half3 vertex3872 = v.vertex.xyz;
				half2 vtexcoord13872 = v.ase_texcoord1.xy;
				int bitMask3872 = _Bitmask;
				half3 localVertVanisher3872 = VertVanisher3872( vertex3872 , vtexcoord13872 , bitMask3872 );
				half3 VertexOcclusionPosition3873 = localVertVanisher3872;
				
				half4 ifLocalVars3708 = 0;
				half2 texCoord117 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==0){ifLocalVars3708 = half4( texCoord117, 0.0 , 0.0 ); };
				half2 texCoord3710 = v.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==1){ifLocalVars3708 = half4( texCoord3710, 0.0 , 0.0 ); };
				half2 texCoord3712 = v.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==2){ifLocalVars3708 = half4( texCoord3712, 0.0 , 0.0 ); };
				half2 appendResult3711 = (half2(ifLocalVars3708.xy));
				half2 vertexToFrag3370 = ( appendResult3711 * ( 1.0 / _MossScale ) );
				o.ase_texcoord5.zw = vertexToFrag3370;
				int MossInt2938 = _Moss;
				int MossModeInt3200 = _MossMode;
				#ifdef _MOSSMETALMODE_ON
				half staticSwitch3230 = (float)( MossInt2938 * MossModeInt3200 );
				#else
				half staticSwitch3230 = (float)MossInt2938;
				#endif
				half2 MossUVScaled1663 = vertexToFrag3370;
				half4 ifLocalVars3717 = 0;
				half2 uv_BaseMap = v.ase_texcoord.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==0){ifLocalVars3717 = half4( uv_BaseMap, 0.0 , 0.0 ); };
				half2 uv2_BaseMap = v.ase_texcoord1.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==1){ifLocalVars3717 = half4( uv2_BaseMap, 0.0 , 0.0 ); };
				half2 uv3_BaseMap = v.ase_texcoord2.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==2){ifLocalVars3717 = half4( uv3_BaseMap, 0.0 , 0.0 ); };
				half2 appendResult3718 = (half2(ifLocalVars3717.xy));
				half2 vertexToFrag3723 = appendResult3718;
				half2 UVBasemapPicked3719 = vertexToFrag3723;
				half2 ifLocalVar2916 = 0;
				if( staticSwitch3230 <= 0.0 )
				ifLocalVar2916 = UVBasemapPicked3719;
				else
				ifLocalVar2916 = MossUVScaled1663;
				half2 vertexToFrag3371 = ifLocalVar2916;
				o.ase_texcoord6.xy = vertexToFrag3371;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				half4 _DetailDistanceHardcoded = half4(25,30,0,0);
				half temp_output_3155_0 = saturate( (1.0 + (distance( _WorldSpaceCameraPos , ase_worldPos ) - _DetailDistanceHardcoded.x) * (0.0 - 1.0) / (_DetailDistanceHardcoded.y - _DetailDistanceHardcoded.x)) );
				half vertexToFrag3157 = temp_output_3155_0;
				o.ase_texcoord6.z = vertexToFrag3157;
				half2 texCoord1007 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				half2 appendResult2490 = (half2(_DetailMapScale.x , _DetailMapScale.y));
				half2 appendResult2491 = (half2(_DetailMapScale.z , _DetailMapScale.w));
				half2 vertexToFrag3400 = (texCoord1007*appendResult2490 + appendResult2491);
				o.ase_texcoord7.xy = vertexToFrag3400;
				o.ase_texcoord7.zw = vertexToFrag3723;
				half3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				half3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				half ase_vertexTangentSign = v.ase_tangent.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord8.xyz = ase_worldBitangent;
				half temp_output_225_0 = ( ( v.ase_color.r - 0.5 ) * 2.0 );
				half ifLocalVar4428 = 0;
				if( _UseMossVertexMask > 0.0 )
				ifLocalVar4428 = temp_output_225_0;
				half vertexToFrag3925 = ifLocalVar4428;
				o.ase_texcoord6.w = vertexToFrag3925;
				
				o.ase_texcoord5.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOcclusionPosition3873;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 normalWS = TransformObjectToWorldNormal( v.ase_normal );
				float4 tangentWS = float4(TransformObjectToWorldDir( v.ase_tangent.xyz), v.ase_tangent.w);
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.worldPos = positionWS;
				#endif

				o.worldNormal = normalWS;
				o.worldTangent = tangentWS;

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				o.clipPosV = positionCS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord2 : TEXCOORD2;
				half4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord2 = v.ase_texcoord2;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_texcoord2 = patch[0].ase_texcoord2 * bary.x + patch[1].ase_texcoord2 * bary.y + patch[2].ase_texcoord2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(	VertexOutput IN
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.worldPos;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				float3 WorldNormal = IN.worldNormal;
				float4 WorldTangent = IN.worldTangent;

				float4 ClipPos = IN.clipPosV;
				float4 ScreenPos = ComputeScreenPos( IN.clipPosV );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				half2 uv_BaseMap = IN.ase_texcoord5.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				half2 UVBasemap02088 = uv_BaseMap;
				half4 tex2DNode32 = SAMPLE_TEXTURE2D( _BumpMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half3 unpack2617 = UnpackNormalScale( tex2DNode32, _NormalStrength );
				unpack2617.z = lerp( 1, unpack2617.z, saturate(_NormalStrength) );
				half3 appendResult2618 = (half3(unpack2617));
				half3 _Vector14 = half3(0,0,1);
				half3 ifLocalVar3262 = 0;
				if( _UseNormalMap <= 0.5 )
				ifLocalVar3262 = _Vector14;
				else
				ifLocalVar3262 = appendResult2618;
				half3 SimpleNormalXYSigned30 = ifLocalVar3262;
				int MossInt2938 = _Moss;
				half2 _Vector0 = half2(0,0);
				half2 vertexToFrag3370 = IN.ase_texcoord5.zw;
				half2 MossUVScaled1663 = vertexToFrag3370;
				half StochasticScale1447 = ( 1.0 / _MossStochasticScale );
				half StochasticContrast1448 = _MossStochasticContrast;
				half3 cw4270 = 0;
				float2 uv14270 = 0;
				float2 uv24270 = 0;
				float2 uv34270 = 0;
				float2 dx4270 = 0;
				float2 dy4270 = 0;
				half4 stochasticSample4270 = StochasticSample2DWeightsR(_MossPacked,sampler_MossPacked,MossUVScaled1663,cw4270,uv14270,uv24270,uv34270,dx4270,dy4270,StochasticScale1447,StochasticContrast1448);
				#ifdef _USESTOCHASTICMOSS_ON
				half4 staticSwitch121 = stochasticSample4270;
				#else
				half4 staticSwitch121 = SAMPLE_TEXTURE2D( _MossPacked, sampler_Linear_Repeat_Aniso2, MossUVScaled1663 );
				#endif
				int MossModeInt3200 = _MossMode;
				half2 vertexToFrag3371 = IN.ase_texcoord6.xy;
				half4 _Vector17 = half4(1,0,1,0);
				half4 ifLocalVar3181 = 0;
				if( _UseMossMetalMap <= 0.0 )
				ifLocalVar3181 = _Vector17;
				else
				ifLocalVar3181 = SAMPLE_TEXTURE2D( _MossMetalMap, sampler_Linear_Repeat_Aniso2, vertexToFrag3371 );
				half4 ifLocalVar3232 = 0;
				if( MossModeInt3200 <= 0.0 )
				ifLocalVar3232 = _Vector17;
				else
				ifLocalVar3232 = ifLocalVar3181;
				half4 MossMetalMap2943 = ifLocalVar3232;
				#ifdef _MOSSMETALMODE_ON
				half4 staticSwitch3303 = MossMetalMap2943;
				#else
				half4 staticSwitch3303 = staticSwitch121;
				#endif
				half4 MossMetalRes3077 = staticSwitch3303;
				half2 temp_output_3045_0 = (MossMetalRes3077).ag;
				half2 MossPackedNormal1656 = temp_output_3045_0;
				half4 temp_output_5_0_g2453 = half4( MossPackedNormal1656, 0.0 , 0.0 );
				half2 appendResult12_g2453 = (half2(temp_output_5_0_g2453.xy));
				half temp_output_6_0_g2453 = _MossNormalStrength;
				half2 lerpResult2_g2453 = lerp( float2( 0,0 ) , ( appendResult12_g2453 - float2( 0.5,0.5 ) ) , ( temp_output_6_0_g2453 * 2.0 ));
				half2 _Vector5 = half2(0,0);
				half2 NormalMossSigned50 = _Vector5;
				half3 appendResult3794 = (half3(NormalMossSigned50 , 1.0));
				half4 appendResult3336 = (half4(1.0 , 0.0 , 0.0 , 0.0));
				half vertexToFrag3157 = IN.ase_texcoord6.z;
				half DDistanceMaskVert3160 = step( 0.01 , vertexToFrag3157 );
				half2 vertexToFrag3400 = IN.ase_texcoord7.xy;
				half4 tex2DNode980 = SAMPLE_TEXTURE2D( _DetailMapPacked, sampler_Linear_Repeat_Aniso2, vertexToFrag3400 );
				half4 DetailMapPacked3088 = tex2DNode980;
				half temp_output_3090_0 = (DetailMapPacked3088).r;
				half detailAlbedo3902 = temp_output_3090_0;
				half scale3902 = _DetailAlbedoMapScale;
				half localMyCustomExpression3902 = MyCustomExpression3902( detailAlbedo3902 , scale3902 );
				half2 vertexToFrag3723 = IN.ase_texcoord7.zw;
				half2 UVBasemapPicked3719 = vertexToFrag3723;
				half4 _Vector16 = half4(0,1,1,1);
				half4 ifLocalVar3177 = 0;
				if( _MetallicSpecGlossMap <= 0.0 )
				ifLocalVar3177 = _Vector16;
				else
				ifLocalVar3177 = SAMPLE_TEXTURE2D( _MetallicGlossMap, sampler_Linear_Repeat_Aniso2, UVBasemapPicked3719 );
				half4 ifLocalVar3189 = 0;
				if( MossModeInt3200 <= 0.0 )
				ifLocalVar3189 = ifLocalVar3181;
				else
				ifLocalVar3189 = half4(1,1,1,1);
				half4 MossMetal2928 = ifLocalVar3189;
				#ifdef _MOSSMETALMODE_ON
				half4 staticSwitch2913 = MossMetal2928;
				#else
				half4 staticSwitch2913 = ifLocalVar3177;
				#endif
				half4 MossMetalORMetallicMap2911 = staticSwitch2913;
				half temp_output_3050_0 = (MossMetalORMetallicMap2911).b;
				half temp_output_8_0_g2548 = temp_output_3050_0;
				half4 ifLocalVars3100 = 0;
				int Mode3840 = _MODE;
				if(Mode3840==0){ifLocalVars3100 = _DetailMaskRemap; };
				if(Mode3840==1){ifLocalVars3100 = _EmissionRemap; };
				half4 break5_g2548 = ifLocalVars3100;
				half temp_output_3_0_g2548 = (0.0 + (temp_output_8_0_g2548 - break5_g2548.x) * (1.0 - 0.0) / (break5_g2548.y - break5_g2548.x));
				half clampResult2_g2548 = clamp( temp_output_3_0_g2548 , break5_g2548.z , break5_g2548.w );
				half temp_output_27_0_g2548 = saturate( clampResult2_g2548 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3738 = temp_output_27_0_g2548;
				#else
				half staticSwitch3738 = temp_output_3050_0;
				#endif
				int temp_output_3109_0 = ( 1.0 - Mode3840 );
				half temp_output_12_0_g2513 = ( temp_output_3109_0 * _UseDetailMask );
				half DetalMaskDoodad3103 = ( ( staticSwitch3738 * temp_output_12_0_g2513 ) + ( 1.0 - temp_output_12_0_g2513 ) );
				half temp_output_12_0_g2557 = DetalMaskDoodad3103;
				half temp_output_3327_0 = (DetailMapPacked3088).b;
				half temp_output_8_0_g2534 = temp_output_3327_0;
				half4 break5_g2534 = _DetailSmoothnessRemap;
				half temp_output_32_0_g2534 = (break5_g2534.z + (temp_output_8_0_g2534 - break5_g2534.x) * (break5_g2534.w - break5_g2534.z) / (break5_g2534.y - break5_g2534.x));
				#ifdef _REMAPPERS_ON
				half staticSwitch3743 = temp_output_32_0_g2534;
				#else
				half staticSwitch3743 = (-1.0 + (temp_output_3327_0 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0));
				#endif
				half4 appendResult3337 = (half4(( ( localMyCustomExpression3902 * temp_output_12_0_g2557 ) + ( 1.0 - temp_output_12_0_g2557 ) ) , ( ( (DetailMapPacked3088).ag * float2( 2,2 ) ) - float2( 1,1 ) ) , staticSwitch3743));
				half4 _DetailDistanceHardcoded = half4(25,30,0,0);
				half temp_output_3155_0 = saturate( (1.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - _DetailDistanceHardcoded.x) * (0.0 - 1.0) / (_DetailDistanceHardcoded.y - _DetailDistanceHardcoded.x)) );
				half DDistanceMaskPixel3159 = temp_output_3155_0;
				half4 lerpResult3339 = lerp( appendResult3336 , appendResult3337 , DDistanceMaskPixel3159);
				half4 ifLocalVar3338 = 0;
				UNITY_BRANCH 
				if( DDistanceMaskVert3160 <= 0.0 )
				ifLocalVar3338 = appendResult3336;
				else
				ifLocalVar3338 = lerpResult3339;
				#ifdef _DETAIL_ON
				half4 staticSwitch4130 = ifLocalVar3338;
				#else
				half4 staticSwitch4130 = appendResult3336;
				#endif
				half2 DetailNormalSigned3509 = ( (staticSwitch4130).yz * _DetailNormalMapScale1 );
				half3 appendResult3801 = (half3(( NormalMossSigned50 + DetailNormalSigned3509 ) , 1.0));
				half3 normalizeResult3795 = normalize( appendResult3801 );
				#ifdef _DETAIL_ON
				half3 staticSwitch4129 = normalizeResult3795;
				#else
				half3 staticSwitch4129 = appendResult3794;
				#endif
				float3 ase_worldBitangent = IN.ase_texcoord8.xyz;
				half3 tanToWorld0 = float3( WorldTangent.xyz.x, ase_worldBitangent.x, WorldNormal.x );
				half3 tanToWorld1 = float3( WorldTangent.xyz.y, ase_worldBitangent.y, WorldNormal.y );
				half3 tanToWorld2 = float3( WorldTangent.xyz.z, ase_worldBitangent.z, WorldNormal.z );
				float3 tanNormal16 = staticSwitch4129;
				half3 worldNormal16 = float3(dot(tanToWorld0,tanNormal16), dot(tanToWorld1,tanNormal16), dot(tanToWorld2,tanNormal16));
				half dotResult15 = dot( worldNormal16 , _MossDirection );
				half temp_output_13_0_g2463 = dotResult15;
				half temp_output_82_0_g2463 = ( temp_output_13_0_g2463 * 0.5 );
				half temp_output_5_0_g2467 = ( temp_output_82_0_g2463 - -0.5 );
				half temp_output_48_0_g2467 = saturate( temp_output_5_0_g2467 );
				half temp_output_14_0_g2463 = _MossLevel;
				half temp_output_17_0_g2467 = temp_output_14_0_g2463;
				half temp_output_16_0_g2463 = ( 1.0 - _MossDirContrast );
				half temp_output_30_0_g2467 = ( 1.0 - saturate( ( 1.0 - temp_output_16_0_g2463 ) ) );
				half temp_output_35_0_g2467 = ( temp_output_30_0_g2467 * 0.5 );
				half temp_output_32_0_g2467 = ( (( 1.0 + temp_output_30_0_g2467 ) + (abs( temp_output_17_0_g2467 ) - 0.0) * (( 0.0 - temp_output_30_0_g2467 ) - ( 1.0 + temp_output_30_0_g2467 )) / (1.0 - 0.0)) - temp_output_35_0_g2467 );
				half temp_output_31_0_g2467 = ( temp_output_35_0_g2467 + (( 1.0 + temp_output_30_0_g2467 ) + (abs( temp_output_17_0_g2467 ) - 0.0) * (( 0.0 - temp_output_30_0_g2467 ) - ( 1.0 + temp_output_30_0_g2467 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2467 = saturate( (0.0 + (temp_output_48_0_g2467 - temp_output_32_0_g2467) * (1.0 - 0.0) / (temp_output_31_0_g2467 - temp_output_32_0_g2467)) );
				half temp_output_3796_0 = temp_output_51_0_g2467;
				half lerpResult4109 = lerp( _MossBase , temp_output_3796_0 , _UseMossDirection);
				half MossMaskDirection1179 = lerpResult4109;
				half temp_output_1811_0 = saturate( MossMaskDirection1179 );
				half4 tex2DNode80 = SAMPLE_TEXTURE2D( _BaseMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half temp_output_3952_0 = ( tex2DNode80.a * _BaseColor.a );
				#ifdef _ALPHATEST_ON
				half staticSwitch3756 = 1.0;
				#else
				half staticSwitch3756 = temp_output_3952_0;
				#endif
				half AlbedoAlphaMoss1953 = staticSwitch3756;
				half temp_output_8_0_g4229 = AlbedoAlphaMoss1953;
				half4 break5_g4229 = _MossAlphaMaskMM;
				half temp_output_32_0_g4229 = (break5_g4229.z + (temp_output_8_0_g4229 - break5_g4229.x) * (break5_g4229.w - break5_g4229.z) / (break5_g4229.y - break5_g4229.x));
				half clampResult31_g4229 = clamp( temp_output_32_0_g4229 , break5_g4229.z , break5_g4229.w );
				half temp_output_3896_0 = clampResult31_g4229;
				#ifdef _REMAPPERS_ON
				half staticSwitch3745 = temp_output_3896_0;
				#else
				half staticSwitch3745 = temp_output_3896_0;
				#endif
				half MossMaskAlpha1182 = ( staticSwitch3745 * _UseMossMaskWithAlpha );
				#ifdef _ALPHATEST_ON
				half staticSwitch3938 = temp_output_1811_0;
				#else
				half staticSwitch3938 = saturate( ( temp_output_1811_0 + MossMaskAlpha1182 ) );
				#endif
				half vertexToFrag3925 = IN.ase_texcoord6.w;
				half MossMaskVertex1181 = vertexToFrag3925;
				half2 break3507 = DetailNormalSigned3509;
				half dotResult3505 = dot( break3507.x , break3507.y );
				#ifdef _DETAIL_ON
				half staticSwitch3500 = ( unpack2617.z + abs( dotResult3505 ) );
				#else
				half staticSwitch3500 = unpack2617.z;
				#endif
				half NormalHeightSigned1759 = staticSwitch3500;
				half temp_output_5_0_g2451 = NormalHeightSigned1759;
				half temp_output_48_0_g2451 = saturate( temp_output_5_0_g2451 );
				half temp_output_17_0_g2451 = saturate( ( staticSwitch3938 + MossMaskVertex1181 ) );
				half temp_output_30_0_g2451 = ( 1.0 - saturate( _MossNormalContrast ) );
				half temp_output_35_0_g2451 = ( temp_output_30_0_g2451 * 0.5 );
				half temp_output_32_0_g2451 = ( (( 1.0 + temp_output_30_0_g2451 ) + (abs( temp_output_17_0_g2451 ) - 0.0) * (( 0.0 - temp_output_30_0_g2451 ) - ( 1.0 + temp_output_30_0_g2451 )) / (1.0 - 0.0)) - temp_output_35_0_g2451 );
				half temp_output_31_0_g2451 = ( temp_output_35_0_g2451 + (( 1.0 + temp_output_30_0_g2451 ) + (abs( temp_output_17_0_g2451 ) - 0.0) * (( 0.0 - temp_output_30_0_g2451 ) - ( 1.0 + temp_output_30_0_g2451 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2451 = saturate( (0.0 + (temp_output_48_0_g2451 - temp_output_32_0_g2451) * (1.0 - 0.0) / (temp_output_31_0_g2451 - temp_output_32_0_g2451)) );
				half temp_output_137_0_g2451 = ( 1.0 - temp_output_48_0_g2451 );
				half temp_output_128_0_g2451 = ( 1.0 - saturate( _MossNormalContrast ) );
				half temp_output_121_0_g2451 = ( temp_output_128_0_g2451 * 0.5 );
				half temp_output_118_0_g2451 = (0.0 + (temp_output_137_0_g2451 - ( (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) - temp_output_121_0_g2451 )) * (1.0 - 0.0) / (( temp_output_121_0_g2451 + (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) ) - ( (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) - temp_output_121_0_g2451 )));
				half lerpResult2905 = lerp( saturate( ( staticSwitch3938 + MossMaskVertex1181 ) ) , ( temp_output_51_0_g2451 - saturate( temp_output_118_0_g2451 ) ) , _MossNormalAffectStrength);
				half temp_output_5_0_g2498 = lerpResult2905;
				half temp_output_48_0_g2498 = saturate( temp_output_5_0_g2498 );
				half temp_output_17_0_g2498 = _MapContrastOffset;
				half temp_output_30_0_g2498 = ( 1.0 - saturate( _MapContrast ) );
				half temp_output_35_0_g2498 = ( temp_output_30_0_g2498 * 0.5 );
				half temp_output_32_0_g2498 = ( (( 1.0 + temp_output_30_0_g2498 ) + (abs( temp_output_17_0_g2498 ) - 0.0) * (( 0.0 - temp_output_30_0_g2498 ) - ( 1.0 + temp_output_30_0_g2498 )) / (1.0 - 0.0)) - temp_output_35_0_g2498 );
				half temp_output_31_0_g2498 = ( temp_output_35_0_g2498 + (( 1.0 + temp_output_30_0_g2498 ) + (abs( temp_output_17_0_g2498 ) - 0.0) * (( 0.0 - temp_output_30_0_g2498 ) - ( 1.0 + temp_output_30_0_g2498 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2498 = saturate( (0.0 + (temp_output_48_0_g2498 - temp_output_32_0_g2498) * (1.0 - 0.0) / (temp_output_31_0_g2498 - temp_output_32_0_g2498)) );
				#ifdef _MOSS
				half staticSwitch14 = temp_output_51_0_g2498;
				#else
				half staticSwitch14 = 0.0;
				#endif
				half MossMask45 = staticSwitch14;
				half2 lerpResult2976 = lerp( _Vector0 , lerpResult2_g2453 , MossMask45);
				half2 MossNormalSigned100 = lerpResult2976;
				half2 temp_output_3610_0 = ( MossInt2938 * MossNormalSigned100 );
				half3 appendResult4006 = (half3(temp_output_3610_0 , 1.0));
				half3 lerpResult4431 = lerp( SimpleNormalXYSigned30 , float3( 0,0,1 ) , MossMask45);
				half2 appendResult1363 = (half2(lerpResult4431.xy));
				#ifdef _MOSS
				half3 staticSwitch4131 = ( appendResult4006 + half3( appendResult1363 ,  0.0 ) );
				#else
				half3 staticSwitch4131 = SimpleNormalXYSigned30;
				#endif
				half temp_output_10_0_g2209 = _DetailsOverMoss;
				half temp_output_17_0_g2209 = saturate( MossMask45 );
				half4 lerpResult7_g2209 = lerp( appendResult3336 , ifLocalVar3338 , saturate( ( saturate( ( abs( temp_output_10_0_g2209 ) - 1.0 ) ) + ( ( ( 1.0 - temp_output_17_0_g2209 ) * abs( min( temp_output_10_0_g2209 , 0.0 ) ) ) + ( temp_output_17_0_g2209 * max( temp_output_10_0_g2209 , 0.0 ) ) ) ) ));
				#ifdef _MOSS
				half4 staticSwitch4126 = lerpResult7_g2209;
				#else
				half4 staticSwitch4126 = ifLocalVar3338;
				#endif
				half4 lerpResult4232 = lerp( appendResult3336 , staticSwitch4126 , (float)_DETAIL);
				#ifdef _DETAIL_ON
				half4 staticSwitch4132 = lerpResult4232;
				#else
				half4 staticSwitch4132 = appendResult3336;
				#endif
				half2 DetailNormaMossMaskedSigned988 = ( _DetailNormalMapScale * (staticSwitch4132).yz );
				half3 appendResult4397 = (half3(DetailNormaMossMaskedSigned988 , 1.0));
				#ifdef _DETAIL_ON
				half3 staticSwitch1105 = BlendNormal( staticSwitch4131 , appendResult4397 );
				#else
				half3 staticSwitch1105 = staticSwitch4131;
				#endif
				half3 NormalPre2265 = staticSwitch1105;
				half3 normalizeResult4009 = normalize( NormalPre2265 );
				half3 NormalRes109 = normalizeResult4009;
				half3 temp_output_42_0_g4236 = NormalRes109;
				half2 uv_Layer0 = IN.ase_texcoord5.xy * _Layer0_ST.xy + _Layer0_ST.zw;
				half2 uvLayer0112_g4236 = uv_Layer0;
				half3 unpack9_g4236 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Layer0NormalMap, sampler_Linear_Repeat_Aniso2, uvLayer0112_g4236 ), _Layer0NormalStrength );
				unpack9_g4236.z = lerp( 1, unpack9_g4236.z, saturate(_Layer0NormalStrength) );
				half2 texCoord24_g4236 = IN.ase_texcoord5.xy * float2( 1,1 ) + float2( 0,0 );
				half2 baseuv25_g4236 = texCoord24_g4236;
				half4 tex2DNode3_g4236 = SAMPLE_TEXTURE2D( _RevealMask, sampler_Linear_Repeat_Aniso2, baseuv25_g4236 );
				half4 tex2DNode4_g4236 = SAMPLE_TEXTURE2D( _LayerMask, sampler_Linear_Repeat_Aniso2, baseuv25_g4236 );
				half rt31_g4236 = saturate( ( tex2DNode3_g4236.r * tex2DNode4_g4236.r ) );
				half LSNormal101_g4236 = _LayerSurfaceExp.y;
				half3 lerpResult41_g4236 = lerp( temp_output_42_0_g4236 , unpack9_g4236 , pow( rt31_g4236 , LSNormal101_g4236 ));
				half2 uv_Layer1 = IN.ase_texcoord5.xy * _Layer1_ST.xy + _Layer1_ST.zw;
				half2 uvLayer1114_g4236 = uv_Layer1;
				half3 unpack18_g4236 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Layer1NormalMap, sampler_Linear_Repeat_Aniso2, uvLayer1114_g4236 ), _Layer1NormalStrength );
				unpack18_g4236.z = lerp( 1, unpack18_g4236.z, saturate(_Layer1NormalStrength) );
				half gt57_g4236 = saturate( ( tex2DNode3_g4236.g * tex2DNode4_g4236.g ) );
				half3 lerpResult64_g4236 = lerp( lerpResult41_g4236 , unpack18_g4236 , pow( gt57_g4236 , LSNormal101_g4236 ));
				half2 uv83_g4236 = baseuv25_g4236;
				half bt76_g4236 = saturate( ( tex2DNode3_g4236.b * tex2DNode4_g4236.b ) );
				half height83_g4236 = ( _Layer2Height * bt76_g4236 );
				half2 texelSize83_g4236 = (_RevealMask_TexelSize).xy;
				SamplerState sampler_RevealMask83_g4236 = sampler_Linear_Repeat_Aniso2;
				half3 localRevealMaskNormalCrossFilter83_g4236 = RevealMaskNormalCrossFilter83_g4236( uv83_g4236 , height83_g4236 , texelSize83_g4236 , sampler_RevealMask83_g4236 );
				half3 heightNormal89_g4236 = localRevealMaskNormalCrossFilter83_g4236;
				#ifdef _REVEALLAYERS
				half3 staticSwitch58_g4236 = BlendNormal( lerpResult64_g4236 , heightNormal89_g4236 );
				#else
				half3 staticSwitch58_g4236 = temp_output_42_0_g4236;
				#endif
				half3 temp_output_4603_43 = staticSwitch58_g4236;
				half3 NormalRes24487 = temp_output_4603_43;
				
				half4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				half2 clipScreen3958 = ase_screenPosNorm.xy * _ScreenParams.xy;
				half dither3958 = Dither4x4Bayer( fmod(clipScreen3958.x, 4), fmod(clipScreen3958.y, 4) );
				dither3958 = step( dither3958, temp_output_3952_0 );
				half lerpResult3962 = lerp( (float)AlphaToCoverage , 1.0 , _Surface);
				half Dither3961 = ( _Dither * ( 1.0 - lerpResult3962 ) );
				half lerpResult3959 = lerp( temp_output_3952_0 , dither3958 , Dither3961);
				#ifdef _ALPHATEST_ON
				half staticSwitch3334 = lerpResult3959;
				#else
				half staticSwitch3334 = 1.0;
				#endif
				half AlbedoAlpha84 = staticSwitch3334;
				
				half lerpResult3950 = lerp( 0.0 , _AlphaClipThreshold , _AlphaClip);
				half lerpResult3291 = lerp( lerpResult3950 , 0.0 , (float)AlphaToCoverage);
				half ClipCalc3294 = lerpResult3291;
				

				float3 Normal = NormalRes24487;
				float Alpha = AlbedoAlpha84;
				float AlphaClipThreshold = ClipCalc3294;
				#ifdef ASE_DEPTH_WRITE_ON
					float DepthValue = IN.clipPos.z;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				#if defined(_GBUFFER_NORMALS_OCT)
					float2 octNormalWS = PackNormalOctQuadEncode(WorldNormal);
					float2 remappedOctNormalWS = saturate(octNormalWS * 0.5 + 0.5);
					half3 packedNormalWS = PackFloat2To888(remappedOctNormalWS);
					return half4(packedNormalWS, 0.0);
				#else
					#if defined(_NORMALMAP)
						#if _NORMAL_DROPOFF_TS
							float crossSign = (WorldTangent.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
							float3 bitangent = crossSign * cross(WorldNormal.xyz, WorldTangent.xyz);
							float3 normalWS = TransformTangentToWorld(Normal, half3x3(WorldTangent.xyz, bitangent, WorldNormal.xyz));
						#elif _NORMAL_DROPOFF_OS
							float3 normalWS = TransformObjectToWorldNormal(Normal);
						#elif _NORMAL_DROPOFF_WS
							float3 normalWS = Normal;
						#endif
					#else
						float3 normalWS = WorldNormal;
					#endif
					return half4(NormalizeNormalPerPixel(normalWS), 0.0);
				#endif
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "GBuffer"
			Tags { "LightMode"="UniversalGBuffer" }

			Blend [_SrcBlend] [_DstBlend], One Zero
			ZWrite [_ZWrite]
			ZTest LEqual
			Offset 0,0
			//ColorMask RGBA
			

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ASE_SSSCONTROL 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define TONEMAPPINGELSEWHERE
			#define UNDERWATERELSEWHERE
			#define ASE_USING_SAMPLING_MACROS 1


			#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
			#pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
			#pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
			#pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
			#pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
			#pragma multi_compile_fragment _ _SHADOWS_SOFT
			#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
			#pragma multi_compile_fragment _ _LIGHT_LAYERS
			#pragma multi_compile_fragment _ _RENDER_PASS_ENABLED

			#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
			#pragma multi_compile _ SHADOWS_SHADOWMASK
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_GBUFFER
			
			//Shadowood
			#ifdef _ASE_ALPHATEST
				#pragma shader_feature_local _ _ALPHATEST_ON
			#endif
			//#ifdef SW_TESSELLATION
			//	#define ASE_TESSELLATION 1
			//	#pragma require tessellation tessHW
			//	#pragma hull HullFunction
			//	#pragma domain DomainFunction
			//	//#define ASE_DISTANCE_TESSELLATION
			//#endif

			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
			
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include_with_pragmas  "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
			
			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
				#define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#include "Assets/AmplifyStochasticNode/StochasticSampling.cginc"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_BITANGENT
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#pragma shader_feature_local _USEMOSSNOISE_ON
			#pragma shader_feature_local MODED_MOSS MODED_MOES MODED_MODS MODED_OFF
			#pragma shader_feature_local _MOSS
			#pragma shader_feature_local _AUTONORMAL
			#pragma shader_feature_local LAYER_UNLIMITED LAYER_MAX2 LAYER_MAX3
			#pragma shader_feature_local TEXTURE_MAX4 TEXTURE_MAX8 TEXTURE_MAX12 TEXTURE_MAX16
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _MOSSMETALMODE_ON
			#pragma shader_feature_local _ALPHAMODE
			#pragma shader_feature_local _SIMPLESTOCHASTIC
			#pragma shader_feature_local _USESPLAT_ON
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USE_SSS_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON
			#pragma shader_feature_local _REVEALLAYERS
			#pragma shader_feature_local _DETAIL_ON
			#pragma shader_feature _COLORMASK_ON
			#pragma shader_feature_local _REMAPPERS_ON
			#pragma shader_feature_local _USESTOCHASTICMOSS_ON
			#pragma shader_feature_local _USEMOSSFRESNEL_ON
			#pragma shader_feature_local _USECAUSTICSFROMABOVE_ON
			#pragma shader_feature_local _USECAUSTICEXTRASAMPLER_ON
			#pragma shader_feature_local _USECAUSTICRAINBOW_ON
			#pragma shader_feature_local _FadeWithHeight
			#define _UseMossPacked
			#define _DetailMapPacking


			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE) && (SHADER_TARGET >= 45)
				#define ASE_SV_DEPTH SV_DepthLessEqual
				#define ASE_SV_POSITION_QUALIFIERS linear noperspective centroid
			#else
				#define ASE_SV_DEPTH SV_Depth
				#define ASE_SV_POSITION_QUALIFIERS
			#endif

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				half4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				ASE_SV_POSITION_QUALIFIERS float4 clipPos : SV_POSITION;
				float4 clipPosV : TEXCOORD0;
				float4 lightmapUVOrVertexSH : TEXCOORD1;
				half4 fogFactorAndVertexLight : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD6;
				#endif
				#if defined(DYNAMICLIGHTMAP_ON)
				float2 dynamicLightmapUV : TEXCOORD7;
				#endif
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_texcoord9 : TEXCOORD9;
				float4 ase_texcoord10 : TEXCOORD10;
				float4 ase_texcoord11 : TEXCOORD11;
				float4 ase_texcoord12 : TEXCOORD12;
				float4 ase_texcoord13 : TEXCOORD13;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Tint3;
			half4 _DetailMapScale;
			half4 _SmoothnessRemap;
			half4 _DetailMaskRemap;
			half4 _MossAlbedoRemap;
			half4 _EmissionRemap;
			half4 _MossAlphaMaskMM;
			half4 _OcclusionRemap;
			half4 _DetailSmoothnessRemap;
			half4x4 _ProbeWorldToTexture;
			half4 _MetallicRemap;
			half4 _MossColor;
			half4 _EmissionColor;
			float4 _RevealMask_TexelSize;
			half4 _MossSmoothnessRemap;
			half4 _Tint1;
			half4 _MossFresnel;
			half4 _MossFresnelColor;
			half4 _Layer3EmissionColor;
			half4 _Layer1_ST;
			half4 _LayerSurfaceExp;
			half4 _Layer0_ST;
			half4 _Tint2;
			half4 _MossSlopeNormal_ST;
			half4 _BaseMap_ST;
			half4 _BaseColor;
			half4 _MossSlopeMM;
			half4 _Tint0;
			half4 _SSSColor;
			half3 _ProbeVolumeMin;
			half3 _MossDirection;
			half3 _ProbeVolumeSizeInv;
			half _MossDetailNormalMapScale;
			half _UseEmission;
			half _Occlusion;
			half _UseOcclusion;
			half _Metallic;
			half _UseMossFresnel;
			half _Smoothness;
			half _MossMetallic;
			half _Layer1Smoothness;
			half _Layer1Metallic;
			half _SSSDimAlbedo;
			half _NormalStrength_USE_SSS;
			half _SSScattering;
			half _SSSRadius;
			half _MossNormalStrength2;
			half _MossSlopeDistort;
			half _MossSlopeNormRotate;
			half _DebugScale;
			int _DebugVisual;
			half _ShadowThreshold;
			half _AlphaClip;
			half _AlphaClipThreshold;
			half _Surface;
			half _Dither;
			half _OcclusionMossMask;
			half _Layer0Smoothness;
			half _MossSmoothness;
			half _Layer0Metallic;
			half _Layer2Height;
			int _DETAIL;
			half _Layer0NormalStrength;
			int _MetalUV;
			int _Moss;
			half _UseMossMetalMap;
			int _MossMode;
			half _MossStochasticContrast;
			half _MossStochasticScale;
			float _MossScale;
			int _MossUV;
			half _MossMultAlbedo;
			int _UseColorMask;
			int _Bitmask;
			int _ZWrite;
			int _Cullmode;
			half _Blend;
			half _SrcBlend;
			half _DstBlend;
			half _Cutoff;
			int _Int0;
			int _BlendMode_UseColorMask;
			half _Layer1NormalStrength;
			half _MossBase;
			half _MetallicSpecGlossMap;
			half _DetailNormalMapScale;
			half _MossNormalStrength;
			half _UseNormalMap;
			half _DetailsOverMoss;
			half _MapContrast;
			half _MapContrastOffset;
			half _MossNormalAffectStrength;
			half _MossNormalSubtract;
			half _DetailAlbedoMapScale;
			half _MossNormalContrast;
			half _UseMossVertexMask;
			half _UseMossMaskWithAlpha;
			half _UseMossDirection;
			half _MossDirContrast;
			half _MossLevel;
			half _DetailNormalMapScale1;
			half _UseDetailMask;
			int _MODE;
			half _NormalStrength;
			half _SSShadcowMix;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			float _DebugCounter; //Shadowood
			half _EnvironmentReflections; // Shadowood
			CBUFFER_END

			// Property used by ScenePickingPass
			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			// Properties used by SceneSelectionPass
			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			TEXTURE2D_ARRAY(_ExtraArray);
			SAMPLER(sampler_ExtraArray);
			TEXTURE2D_ARRAY(_DiffuseArray);
			SAMPLER(sampler_DiffuseArray);
			TEXTURE2D_ARRAY(_NormalArray);
			SAMPLER(sampler_NormalArray);
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_Linear_Repeat_Aniso2);
			TEXTURE2D(_ColorMask);
			TEXTURE2D(_MossPacked);
			SAMPLER(sampler_MossPacked);
			TEXTURE2D(_MossMetalMap);
			TEXTURE2D(_DetailMapPacked);
			TEXTURE2D(_MetallicGlossMap);
			TEXTURE2D(_BumpMap);
			TEXTURE2D(_Layer0);
			TEXTURE2D(_RevealMask);
			TEXTURE2D(_LayerMask);
			TEXTURE2D(_Layer1);
			TEXTURE2D(_Layer0NormalMap);
			TEXTURE2D(_Layer1NormalMap);
			half4x4 _CausticMatrix;
			SAMPLER(sampler_Linear_Repeat);
			half4 _CausticsSettings;
			int AlphaToCoverage;
			TEXTURE3D(_ProbeVolumeShR);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE3D(_ProbeVolumeShG);
			TEXTURE3D(_ProbeVolumeShB);


			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRGBufferPass.hlsl"

			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			half3 VertVanisher3872( half3 vertex, half2 vtexcoord1, int bitMask )
			{
				 if(bitMask & (int)vtexcoord1.x){
					vertex *= (0.0 / 0.0);
				}
				return vertex;
			}
			
			half MyCustomExpression3902( half detailAlbedo, half scale )
			{
				return half(2.0) * detailAlbedo * scale - scale + half(1.0);
			}
			
			half3 RevealMaskNormalCrossFilter83_g4236( half2 uv, half height, half2 texelSize, SamplerState sampler_RevealMask )
			{
						//float2 texelSize = float2(1.0 / texWidth, 1.0 / texHeight);
						float4 h;
						h[0] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0, -1) * texelSize).b) * height;
						h[1] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(-1, 0) * texelSize).b) * height;
						h[2] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(1, 0) * texelSize).b) * height;
						h[3] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0, 1) * texelSize).b) * height;
						float3 n;
						n.z = 2;
						n.x = h[1] - h[2];
						n.y = h[0] - h[3];
						return normalize(n);
			}
			
			inline half3 SampleSHPixel3558( half3 SH, half3 normalWS )
			{
				return SampleSHPixel( SH, normalWS );
			}
			
			inline half3 SampleLightmap3567( half2 lightmapUV, half3 normalWS )
			{
				return SampleLightmap( lightmapUV, 0, normalWS );
			}
			
			half3 MatrixMulThatWorks( half4 Pos, half4x4 Mat )
			{
				float3 result = mul( Mat, float4(Pos.xyz,1.0) ).xyz;
				return result + float3(Mat[0][3],Mat[1][3],Mat[2][3]);
			}
			
			half ShaderAPIMobile151_g4260( half desktop, half mobile )
			{
				#if SHADER_API_MOBILE
					return desktop;
				#else
					return mobile;
				#endif
			}
			
			half4x4 Inverse4x4(half4x4 input)
			{
				#define minor(a,b,c) determinant(half3x3(input.a, input.b, input.c))
				half4x4 cofactors = half4x4(
				minor( _22_23_24, _32_33_34, _42_43_44 ),
				-minor( _21_23_24, _31_33_34, _41_43_44 ),
				minor( _21_22_24, _31_32_34, _41_42_44 ),
				-minor( _21_22_23, _31_32_33, _41_42_43 ),
			
				-minor( _12_13_14, _32_33_34, _42_43_44 ),
				minor( _11_13_14, _31_33_34, _41_43_44 ),
				-minor( _11_12_14, _31_32_34, _41_42_44 ),
				minor( _11_12_13, _31_32_33, _41_42_43 ),
			
				minor( _12_13_14, _22_23_24, _42_43_44 ),
				-minor( _11_13_14, _21_23_24, _41_43_44 ),
				minor( _11_12_14, _21_22_24, _41_42_44 ),
				-minor( _11_12_13, _21_22_23, _41_42_43 ),
			
				-minor( _12_13_14, _22_23_24, _32_33_34 ),
				minor( _11_13_14, _21_23_24, _31_33_34 ),
				-minor( _11_12_14, _21_22_24, _31_32_34 ),
				minor( _11_12_13, _21_22_23, _31_32_33 ));
				#undef minor
				return transpose( cofactors ) / determinant( input );
			}
			
			half ShaderAPIMobile153_g4260( half desktop, half mobile )
			{
				#if SHADER_API_MOBILE
					return desktop;
				#else
					return mobile;
				#endif
			}
			
			inline half4 GetUnderWaterFogs240_g4251( half3 viewDir, float3 camWorldPos, half3 posWS, half4 oceanFogDensities, half oceanHeight, half4 oceanFogTop_RGB_Exponent, half4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			
			inline float Dither4x4Bayer( int x, int y )
			{
				const float dither[ 16 ] = {
			 1,  9,  3, 11,
			13,  5, 15,  7,
			 4, 12,  2, 10,
			16,  8, 14,  6 };
				int r = y * 4 + x;
				return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
			}
			
			half4x4 RotationOnly90_g4351( half4x4 Input )
			{
				float4x4 Output = float4x4(
				        float4(Input[0].xyz, 0.0),
				        float4(Input[1].xyz, 0.0),
				        float4(Input[2].xyz, 0.0),
				        float4(0.0,0.0,0.0, 1.0)
				    );
				return Output;
			}
			
			half3 MyCustomExpression6_g4351( half3 vertexPos, half4x4 ProbeWorldToTexture, half3 ProbeVolumeMin, half3 ProbeVolumeSizeInv )
			{
				float3 position = mul(ProbeWorldToTexture, float4(TransformObjectToWorld(vertexPos.xyz), 1.0f)).xyz;
				float3 texCoord = (position - ProbeVolumeMin.xyz) * ProbeVolumeSizeInv;
				return texCoord;
			}
			
			half3 SHEvalLinearL0L114_g4351( half3 worldNormal, half4 ProbeVolumeShR, half4 ProbeVolumeShG, half4 ProbeVolumeShB )
			{
				// See: Library\PackageCache\com.unity.render-pipelines.core12.1.15\ShaderLibrary\EntityLighting.hlsl
				return SHEvalLinearL0L1(worldNormal, ProbeVolumeShR, ProbeVolumeShG,  ProbeVolumeShB);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				half3 vertex3872 = v.vertex.xyz;
				half2 vtexcoord13872 = v.texcoord1.xy;
				int bitMask3872 = _Bitmask;
				half3 localVertVanisher3872 = VertVanisher3872( vertex3872 , vtexcoord13872 , bitMask3872 );
				half3 VertexOcclusionPosition3873 = localVertVanisher3872;
				
				half4 ifLocalVars3708 = 0;
				half2 texCoord117 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==0){ifLocalVars3708 = half4( texCoord117, 0.0 , 0.0 ); };
				half2 texCoord3710 = v.texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==1){ifLocalVars3708 = half4( texCoord3710, 0.0 , 0.0 ); };
				half2 texCoord3712 = v.texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				if(_MossUV==2){ifLocalVars3708 = half4( texCoord3712, 0.0 , 0.0 ); };
				half2 appendResult3711 = (half2(ifLocalVars3708.xy));
				half2 vertexToFrag3370 = ( appendResult3711 * ( 1.0 / _MossScale ) );
				o.ase_texcoord9.xy = vertexToFrag3370;
				int MossInt2938 = _Moss;
				int MossModeInt3200 = _MossMode;
				#ifdef _MOSSMETALMODE_ON
				half staticSwitch3230 = (float)( MossInt2938 * MossModeInt3200 );
				#else
				half staticSwitch3230 = (float)MossInt2938;
				#endif
				half2 MossUVScaled1663 = vertexToFrag3370;
				half4 ifLocalVars3717 = 0;
				half2 uv_BaseMap = v.texcoord.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==0){ifLocalVars3717 = half4( uv_BaseMap, 0.0 , 0.0 ); };
				half2 uv2_BaseMap = v.texcoord1.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==1){ifLocalVars3717 = half4( uv2_BaseMap, 0.0 , 0.0 ); };
				half2 uv3_BaseMap = v.texcoord2.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				if(_MetalUV==2){ifLocalVars3717 = half4( uv3_BaseMap, 0.0 , 0.0 ); };
				half2 appendResult3718 = (half2(ifLocalVars3717.xy));
				half2 vertexToFrag3723 = appendResult3718;
				half2 UVBasemapPicked3719 = vertexToFrag3723;
				half2 ifLocalVar2916 = 0;
				if( staticSwitch3230 <= 0.0 )
				ifLocalVar2916 = UVBasemapPicked3719;
				else
				ifLocalVar2916 = MossUVScaled1663;
				half2 vertexToFrag3371 = ifLocalVar2916;
				o.ase_texcoord9.zw = vertexToFrag3371;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				half4 _DetailDistanceHardcoded = half4(25,30,0,0);
				half temp_output_3155_0 = saturate( (1.0 + (distance( _WorldSpaceCameraPos , ase_worldPos ) - _DetailDistanceHardcoded.x) * (0.0 - 1.0) / (_DetailDistanceHardcoded.y - _DetailDistanceHardcoded.x)) );
				half vertexToFrag3157 = temp_output_3155_0;
				o.ase_texcoord8.w = vertexToFrag3157;
				half2 texCoord1007 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				half2 appendResult2490 = (half2(_DetailMapScale.x , _DetailMapScale.y));
				half2 appendResult2491 = (half2(_DetailMapScale.z , _DetailMapScale.w));
				half2 vertexToFrag3400 = (texCoord1007*appendResult2490 + appendResult2491);
				o.ase_texcoord10.xy = vertexToFrag3400;
				o.ase_texcoord10.zw = vertexToFrag3723;
				half temp_output_225_0 = ( ( v.ase_color.r - 0.5 ) * 2.0 );
				half ifLocalVar4428 = 0;
				if( _UseMossVertexMask > 0.0 )
				ifLocalVar4428 = temp_output_225_0;
				half vertexToFrag3925 = ifLocalVar4428;
				o.ase_texcoord11.x = vertexToFrag3925;
				
				half3 _Vector3 = half3(0,0,-1);
				half4 Pos6_g4262 = half4( _Vector3 , 0.0 );
				half4x4 Mat6_g4262 = _CausticMatrix;
				half3 localMatrixMulThatWorks6_g4262 = MatrixMulThatWorks( Pos6_g4262 , Mat6_g4262 );
				half3 normalizeResult147_g4260 = normalize( localMatrixMulThatWorks6_g4262 );
				half3 vertexToFrag144_g4260 = normalizeResult147_g4260;
				o.ase_texcoord11.yzw = vertexToFrag144_g4260;
				half3 WorldPosition256_g4237 = ase_worldPos;
				half3 temp_output_105_0_g4260 = WorldPosition256_g4237;
				half4 Pos6_g4261 = half4( temp_output_105_0_g4260 , 0.0 );
				half4x4 invertVal146_g4260 = Inverse4x4( _CausticMatrix );
				half4x4 Mat6_g4261 = invertVal146_g4260;
				half3 localMatrixMulThatWorks6_g4261 = MatrixMulThatWorks( Pos6_g4261 , Mat6_g4261 );
				half2 vertexToFrag52_g4260 = (localMatrixMulThatWorks6_g4261).xy;
				o.ase_texcoord12.xy = vertexToFrag52_g4260;
				
				half3 vertexPos6_g4351 = v.vertex.xyz;
				half4x4 temp_output_28_0_g4351 = _ProbeWorldToTexture;
				half4x4 ProbeWorldToTexture6_g4351 = temp_output_28_0_g4351;
				half3 ProbeVolumeMin6_g4351 = _ProbeVolumeMin;
				half3 ProbeVolumeSizeInv6_g4351 = _ProbeVolumeSizeInv;
				half3 localMyCustomExpression6_g4351 = MyCustomExpression6_g4351( vertexPos6_g4351 , ProbeWorldToTexture6_g4351 , ProbeVolumeMin6_g4351 , ProbeVolumeSizeInv6_g4351 );
				half3 vertexToFrag7_g4351 = localMyCustomExpression6_g4351;
				o.ase_texcoord13.xyz = vertexToFrag7_g4351;
				
				o.ase_texcoord8.xyz = v.texcoord.xyz;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord12.zw = 0;
				o.ase_texcoord13.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOcclusionPosition3873;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				#if defined(LIGHTMAP_ON)
					OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy);
				#endif

				#if defined(DYNAMICLIGHTMAP_ON)
					o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif

				#if !defined(LIGHTMAP_ON)
					OUTPUT_SH(normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz);
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord.xy;
					o.lightmapUVOrVertexSH.xy = v.texcoord.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );

				o.fogFactorAndVertexLight = half4(0, vertexLight);

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				o.clipPosV = positionCS;
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				half4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			FragmentOutput frag ( VertexOutput IN
								#ifdef ASE_DEPTH_WRITE_ON
								,out float outputDepth : ASE_SV_DEPTH
								#endif
								 )
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif

				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				float4 ClipPos = IN.clipPosV;
				float4 ScreenPos = ComputeScreenPos( IN.clipPosV );

				float2 NormalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.clipPos);

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#else
					ShadowCoords = float4(0, 0, 0, 0);
				#endif

				WorldViewDirection = SafeNormalize( WorldViewDirection );

				half2 uv_BaseMap = IN.ase_texcoord8.xyz.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				half2 UVBasemap02088 = uv_BaseMap;
				half4 tex2DNode80 = SAMPLE_TEXTURE2D( _BaseMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half3 temp_output_82_0 = ( (tex2DNode80).rgb * (_BaseColor).rgb );
				half3 temp_output_17_0_g2518 = temp_output_82_0;
				half4 ifLocalVars72_g2518 = 0;
				int clampResult74_g2518 = clamp( _BlendMode_UseColorMask , 0 , 1 );
				half4 tex2DNode1_g2518 = SAMPLE_TEXTURE2D( _ColorMask, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half temp_output_2_0_g2518 = ( 1.0 - tex2DNode1_g2518.a );
				half3 blendOpSrc26_g2518 = temp_output_17_0_g2518;
				half3 blendOpDest26_g2518 = ( ( (_Tint0).rgb * tex2DNode1_g2518.r * 1.0 ) + ( (_Tint1).rgb * tex2DNode1_g2518.g * 1.0 ) + ( (_Tint2).rgb * tex2DNode1_g2518.b * 1.0 ) + ( (_Tint3).rgb * temp_output_2_0_g2518 * 1.0 ) );
				half3 temp_output_26_0_g2518 = ( saturate( ( blendOpSrc26_g2518 * blendOpDest26_g2518 ) ));
				if(clampResult74_g2518==0){ifLocalVars72_g2518 = half4( temp_output_26_0_g2518 , 0.0 ); };
				half4 ifLocalVars29_g2518 = 0;
				if(_BlendMode_UseColorMask==0){ifLocalVars29_g2518 = half4( half3(0,0,0) , 0.0 ); };
				if(_BlendMode_UseColorMask==1){ifLocalVars29_g2518 = half4( temp_output_26_0_g2518 , 0.0 ); };
				half3 blendOpSrc27_g2518 = temp_output_17_0_g2518;
				half3 blendOpDest27_g2518 = ( ( (_Tint0).rgb * tex2DNode1_g2518.r * 1.0 ) + ( (_Tint1).rgb * tex2DNode1_g2518.g * 1.0 ) + ( (_Tint2).rgb * tex2DNode1_g2518.b * 1.0 ) + ( (_Tint3).rgb * temp_output_2_0_g2518 * 1.0 ) );
				if(_BlendMode_UseColorMask==2){ifLocalVars29_g2518 = half4( ( saturate( ( 1.0 - ( 1.0 - blendOpSrc27_g2518 ) * ( 1.0 - blendOpDest27_g2518 ) ) )) , 0.0 ); };
				half3 blendOpSrc28_g2518 = temp_output_17_0_g2518;
				half3 blendOpDest28_g2518 = ( ( (_Tint0).rgb * tex2DNode1_g2518.r * 1.0 ) + ( (_Tint1).rgb * tex2DNode1_g2518.g * 1.0 ) + ( (_Tint2).rgb * tex2DNode1_g2518.b * 1.0 ) + ( (_Tint3).rgb * temp_output_2_0_g2518 * 1.0 ) );
				if(_BlendMode_UseColorMask==3){ifLocalVars29_g2518 = half4( ( saturate( (( blendOpDest28_g2518 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest28_g2518 ) * ( 1.0 - blendOpSrc28_g2518 ) ) : ( 2.0 * blendOpDest28_g2518 * blendOpSrc28_g2518 ) ) )) , 0.0 ); };
				if(clampResult74_g2518==1){ifLocalVars72_g2518 = half4( (ifLocalVars29_g2518).xyz , 0.0 ); };
				half3 lerpResult64_g2518 = lerp( temp_output_17_0_g2518 , (ifLocalVars72_g2518).xyz , ( saturate( ( ( 1.0 * tex2DNode1_g2518.r ) + ( 1.0 * tex2DNode1_g2518.g ) + ( 1.0 * tex2DNode1_g2518.b ) + ( 1.0 * temp_output_2_0_g2518 ) ) ) * _UseColorMask ));
				#ifdef _COLORMASK_ON
				half3 staticSwitch15_g2518 = lerpResult64_g2518;
				#else
				half3 staticSwitch15_g2518 = temp_output_17_0_g2518;
				#endif
				half3 AlbedoColorMask2007 = staticSwitch15_g2518;
				half3 AlbedoBaseColor92 = AlbedoColorMask2007;
				half3 appendResult3785 = (half3(_MossColor.rgb));
				half2 vertexToFrag3370 = IN.ase_texcoord9.xy;
				half2 MossUVScaled1663 = vertexToFrag3370;
				half StochasticScale1447 = ( 1.0 / _MossStochasticScale );
				half StochasticContrast1448 = _MossStochasticContrast;
				half3 cw4270 = 0;
				float2 uv14270 = 0;
				float2 uv24270 = 0;
				float2 uv34270 = 0;
				float2 dx4270 = 0;
				float2 dy4270 = 0;
				half4 stochasticSample4270 = StochasticSample2DWeightsR(_MossPacked,sampler_MossPacked,MossUVScaled1663,cw4270,uv14270,uv24270,uv34270,dx4270,dy4270,StochasticScale1447,StochasticContrast1448);
				#ifdef _USESTOCHASTICMOSS_ON
				half4 staticSwitch121 = stochasticSample4270;
				#else
				half4 staticSwitch121 = SAMPLE_TEXTURE2D( _MossPacked, sampler_Linear_Repeat_Aniso2, MossUVScaled1663 );
				#endif
				int MossModeInt3200 = _MossMode;
				half2 vertexToFrag3371 = IN.ase_texcoord9.zw;
				half4 _Vector17 = half4(1,0,1,0);
				half4 ifLocalVar3181 = 0;
				if( _UseMossMetalMap <= 0.0 )
				ifLocalVar3181 = _Vector17;
				else
				ifLocalVar3181 = SAMPLE_TEXTURE2D( _MossMetalMap, sampler_Linear_Repeat_Aniso2, vertexToFrag3371 );
				half4 ifLocalVar3232 = 0;
				if( MossModeInt3200 <= 0.0 )
				ifLocalVar3232 = _Vector17;
				else
				ifLocalVar3232 = ifLocalVar3181;
				half4 MossMetalMap2943 = ifLocalVar3232;
				#ifdef _MOSSMETALMODE_ON
				half4 staticSwitch3303 = MossMetalMap2943;
				#else
				half4 staticSwitch3303 = staticSwitch121;
				#endif
				half4 MossMetalRes3077 = staticSwitch3303;
				half temp_output_3043_0 = (MossMetalRes3077).r;
				half temp_output_8_0_g2546 = temp_output_3043_0;
				half4 break5_g2546 = _MossAlbedoRemap;
				half temp_output_3_0_g2546 = (0.0 + (temp_output_8_0_g2546 - break5_g2546.x) * (1.0 - 0.0) / (break5_g2546.y - break5_g2546.x));
				half clampResult2_g2546 = clamp( temp_output_3_0_g2546 , break5_g2546.z , break5_g2546.w );
				half temp_output_27_0_g2546 = saturate( clampResult2_g2546 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3747 = temp_output_27_0_g2546;
				#else
				half staticSwitch3747 = temp_output_3043_0;
				#endif
				half3 MossPackedAlbedo1643 = ( appendResult3785 * staticSwitch3747 );
				half3 lerpResult2137 = lerp( MossPackedAlbedo1643 , ( MossPackedAlbedo1643 * AlbedoBaseColor92 ) , _MossMultAlbedo);
				half3 MossAlbedoTinted101 = lerpResult2137;
				half2 _Vector5 = half2(0,0);
				half2 NormalMossSigned50 = _Vector5;
				half3 appendResult3794 = (half3(NormalMossSigned50 , 1.0));
				half4 appendResult3336 = (half4(1.0 , 0.0 , 0.0 , 0.0));
				half vertexToFrag3157 = IN.ase_texcoord8.w;
				half DDistanceMaskVert3160 = step( 0.01 , vertexToFrag3157 );
				half2 vertexToFrag3400 = IN.ase_texcoord10.xy;
				half4 tex2DNode980 = SAMPLE_TEXTURE2D( _DetailMapPacked, sampler_Linear_Repeat_Aniso2, vertexToFrag3400 );
				half4 DetailMapPacked3088 = tex2DNode980;
				half temp_output_3090_0 = (DetailMapPacked3088).r;
				half detailAlbedo3902 = temp_output_3090_0;
				half scale3902 = _DetailAlbedoMapScale;
				half localMyCustomExpression3902 = MyCustomExpression3902( detailAlbedo3902 , scale3902 );
				half2 vertexToFrag3723 = IN.ase_texcoord10.zw;
				half2 UVBasemapPicked3719 = vertexToFrag3723;
				half4 _Vector16 = half4(0,1,1,1);
				half4 ifLocalVar3177 = 0;
				if( _MetallicSpecGlossMap <= 0.0 )
				ifLocalVar3177 = _Vector16;
				else
				ifLocalVar3177 = SAMPLE_TEXTURE2D( _MetallicGlossMap, sampler_Linear_Repeat_Aniso2, UVBasemapPicked3719 );
				half4 ifLocalVar3189 = 0;
				if( MossModeInt3200 <= 0.0 )
				ifLocalVar3189 = ifLocalVar3181;
				else
				ifLocalVar3189 = half4(1,1,1,1);
				half4 MossMetal2928 = ifLocalVar3189;
				#ifdef _MOSSMETALMODE_ON
				half4 staticSwitch2913 = MossMetal2928;
				#else
				half4 staticSwitch2913 = ifLocalVar3177;
				#endif
				half4 MossMetalORMetallicMap2911 = staticSwitch2913;
				half temp_output_3050_0 = (MossMetalORMetallicMap2911).b;
				half temp_output_8_0_g2548 = temp_output_3050_0;
				half4 ifLocalVars3100 = 0;
				int Mode3840 = _MODE;
				if(Mode3840==0){ifLocalVars3100 = _DetailMaskRemap; };
				if(Mode3840==1){ifLocalVars3100 = _EmissionRemap; };
				half4 break5_g2548 = ifLocalVars3100;
				half temp_output_3_0_g2548 = (0.0 + (temp_output_8_0_g2548 - break5_g2548.x) * (1.0 - 0.0) / (break5_g2548.y - break5_g2548.x));
				half clampResult2_g2548 = clamp( temp_output_3_0_g2548 , break5_g2548.z , break5_g2548.w );
				half temp_output_27_0_g2548 = saturate( clampResult2_g2548 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3738 = temp_output_27_0_g2548;
				#else
				half staticSwitch3738 = temp_output_3050_0;
				#endif
				int temp_output_3109_0 = ( 1.0 - Mode3840 );
				half temp_output_12_0_g2513 = ( temp_output_3109_0 * _UseDetailMask );
				half DetalMaskDoodad3103 = ( ( staticSwitch3738 * temp_output_12_0_g2513 ) + ( 1.0 - temp_output_12_0_g2513 ) );
				half temp_output_12_0_g2557 = DetalMaskDoodad3103;
				half temp_output_3327_0 = (DetailMapPacked3088).b;
				half temp_output_8_0_g2534 = temp_output_3327_0;
				half4 break5_g2534 = _DetailSmoothnessRemap;
				half temp_output_32_0_g2534 = (break5_g2534.z + (temp_output_8_0_g2534 - break5_g2534.x) * (break5_g2534.w - break5_g2534.z) / (break5_g2534.y - break5_g2534.x));
				#ifdef _REMAPPERS_ON
				half staticSwitch3743 = temp_output_32_0_g2534;
				#else
				half staticSwitch3743 = (-1.0 + (temp_output_3327_0 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0));
				#endif
				half4 appendResult3337 = (half4(( ( localMyCustomExpression3902 * temp_output_12_0_g2557 ) + ( 1.0 - temp_output_12_0_g2557 ) ) , ( ( (DetailMapPacked3088).ag * float2( 2,2 ) ) - float2( 1,1 ) ) , staticSwitch3743));
				half4 _DetailDistanceHardcoded = half4(25,30,0,0);
				half temp_output_3155_0 = saturate( (1.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - _DetailDistanceHardcoded.x) * (0.0 - 1.0) / (_DetailDistanceHardcoded.y - _DetailDistanceHardcoded.x)) );
				half DDistanceMaskPixel3159 = temp_output_3155_0;
				half4 lerpResult3339 = lerp( appendResult3336 , appendResult3337 , DDistanceMaskPixel3159);
				half4 ifLocalVar3338 = 0;
				UNITY_BRANCH 
				if( DDistanceMaskVert3160 <= 0.0 )
				ifLocalVar3338 = appendResult3336;
				else
				ifLocalVar3338 = lerpResult3339;
				#ifdef _DETAIL_ON
				half4 staticSwitch4130 = ifLocalVar3338;
				#else
				half4 staticSwitch4130 = appendResult3336;
				#endif
				half2 DetailNormalSigned3509 = ( (staticSwitch4130).yz * _DetailNormalMapScale1 );
				half3 appendResult3801 = (half3(( NormalMossSigned50 + DetailNormalSigned3509 ) , 1.0));
				half3 normalizeResult3795 = normalize( appendResult3801 );
				#ifdef _DETAIL_ON
				half3 staticSwitch4129 = normalizeResult3795;
				#else
				half3 staticSwitch4129 = appendResult3794;
				#endif
				half3 tanToWorld0 = float3( WorldTangent.x, WorldBiTangent.x, WorldNormal.x );
				half3 tanToWorld1 = float3( WorldTangent.y, WorldBiTangent.y, WorldNormal.y );
				half3 tanToWorld2 = float3( WorldTangent.z, WorldBiTangent.z, WorldNormal.z );
				float3 tanNormal16 = staticSwitch4129;
				half3 worldNormal16 = float3(dot(tanToWorld0,tanNormal16), dot(tanToWorld1,tanNormal16), dot(tanToWorld2,tanNormal16));
				half dotResult15 = dot( worldNormal16 , _MossDirection );
				half temp_output_13_0_g2463 = dotResult15;
				half temp_output_82_0_g2463 = ( temp_output_13_0_g2463 * 0.5 );
				half temp_output_5_0_g2467 = ( temp_output_82_0_g2463 - -0.5 );
				half temp_output_48_0_g2467 = saturate( temp_output_5_0_g2467 );
				half temp_output_14_0_g2463 = _MossLevel;
				half temp_output_17_0_g2467 = temp_output_14_0_g2463;
				half temp_output_16_0_g2463 = ( 1.0 - _MossDirContrast );
				half temp_output_30_0_g2467 = ( 1.0 - saturate( ( 1.0 - temp_output_16_0_g2463 ) ) );
				half temp_output_35_0_g2467 = ( temp_output_30_0_g2467 * 0.5 );
				half temp_output_32_0_g2467 = ( (( 1.0 + temp_output_30_0_g2467 ) + (abs( temp_output_17_0_g2467 ) - 0.0) * (( 0.0 - temp_output_30_0_g2467 ) - ( 1.0 + temp_output_30_0_g2467 )) / (1.0 - 0.0)) - temp_output_35_0_g2467 );
				half temp_output_31_0_g2467 = ( temp_output_35_0_g2467 + (( 1.0 + temp_output_30_0_g2467 ) + (abs( temp_output_17_0_g2467 ) - 0.0) * (( 0.0 - temp_output_30_0_g2467 ) - ( 1.0 + temp_output_30_0_g2467 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2467 = saturate( (0.0 + (temp_output_48_0_g2467 - temp_output_32_0_g2467) * (1.0 - 0.0) / (temp_output_31_0_g2467 - temp_output_32_0_g2467)) );
				half temp_output_3796_0 = temp_output_51_0_g2467;
				half lerpResult4109 = lerp( _MossBase , temp_output_3796_0 , _UseMossDirection);
				half MossMaskDirection1179 = lerpResult4109;
				half temp_output_1811_0 = saturate( MossMaskDirection1179 );
				half temp_output_3952_0 = ( tex2DNode80.a * _BaseColor.a );
				#ifdef _ALPHATEST_ON
				half staticSwitch3756 = 1.0;
				#else
				half staticSwitch3756 = temp_output_3952_0;
				#endif
				half AlbedoAlphaMoss1953 = staticSwitch3756;
				half temp_output_8_0_g4229 = AlbedoAlphaMoss1953;
				half4 break5_g4229 = _MossAlphaMaskMM;
				half temp_output_32_0_g4229 = (break5_g4229.z + (temp_output_8_0_g4229 - break5_g4229.x) * (break5_g4229.w - break5_g4229.z) / (break5_g4229.y - break5_g4229.x));
				half clampResult31_g4229 = clamp( temp_output_32_0_g4229 , break5_g4229.z , break5_g4229.w );
				half temp_output_3896_0 = clampResult31_g4229;
				#ifdef _REMAPPERS_ON
				half staticSwitch3745 = temp_output_3896_0;
				#else
				half staticSwitch3745 = temp_output_3896_0;
				#endif
				half MossMaskAlpha1182 = ( staticSwitch3745 * _UseMossMaskWithAlpha );
				#ifdef _ALPHATEST_ON
				half staticSwitch3938 = temp_output_1811_0;
				#else
				half staticSwitch3938 = saturate( ( temp_output_1811_0 + MossMaskAlpha1182 ) );
				#endif
				half vertexToFrag3925 = IN.ase_texcoord11.x;
				half MossMaskVertex1181 = vertexToFrag3925;
				half4 tex2DNode32 = SAMPLE_TEXTURE2D( _BumpMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half3 unpack2617 = UnpackNormalScale( tex2DNode32, _NormalStrength );
				unpack2617.z = lerp( 1, unpack2617.z, saturate(_NormalStrength) );
				half2 break3507 = DetailNormalSigned3509;
				half dotResult3505 = dot( break3507.x , break3507.y );
				#ifdef _DETAIL_ON
				half staticSwitch3500 = ( unpack2617.z + abs( dotResult3505 ) );
				#else
				half staticSwitch3500 = unpack2617.z;
				#endif
				half NormalHeightSigned1759 = staticSwitch3500;
				half temp_output_5_0_g2451 = NormalHeightSigned1759;
				half temp_output_48_0_g2451 = saturate( temp_output_5_0_g2451 );
				half temp_output_17_0_g2451 = saturate( ( staticSwitch3938 + MossMaskVertex1181 ) );
				half temp_output_30_0_g2451 = ( 1.0 - saturate( _MossNormalContrast ) );
				half temp_output_35_0_g2451 = ( temp_output_30_0_g2451 * 0.5 );
				half temp_output_32_0_g2451 = ( (( 1.0 + temp_output_30_0_g2451 ) + (abs( temp_output_17_0_g2451 ) - 0.0) * (( 0.0 - temp_output_30_0_g2451 ) - ( 1.0 + temp_output_30_0_g2451 )) / (1.0 - 0.0)) - temp_output_35_0_g2451 );
				half temp_output_31_0_g2451 = ( temp_output_35_0_g2451 + (( 1.0 + temp_output_30_0_g2451 ) + (abs( temp_output_17_0_g2451 ) - 0.0) * (( 0.0 - temp_output_30_0_g2451 ) - ( 1.0 + temp_output_30_0_g2451 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2451 = saturate( (0.0 + (temp_output_48_0_g2451 - temp_output_32_0_g2451) * (1.0 - 0.0) / (temp_output_31_0_g2451 - temp_output_32_0_g2451)) );
				half temp_output_137_0_g2451 = ( 1.0 - temp_output_48_0_g2451 );
				half temp_output_128_0_g2451 = ( 1.0 - saturate( _MossNormalContrast ) );
				half temp_output_121_0_g2451 = ( temp_output_128_0_g2451 * 0.5 );
				half temp_output_118_0_g2451 = (0.0 + (temp_output_137_0_g2451 - ( (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) - temp_output_121_0_g2451 )) * (1.0 - 0.0) / (( temp_output_121_0_g2451 + (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) ) - ( (( 1.0 + temp_output_128_0_g2451 ) + (abs( _MossNormalSubtract ) - 0.0) * (( 0.0 - temp_output_128_0_g2451 ) - ( 1.0 + temp_output_128_0_g2451 )) / (1.0 - 0.0)) - temp_output_121_0_g2451 )));
				half lerpResult2905 = lerp( saturate( ( staticSwitch3938 + MossMaskVertex1181 ) ) , ( temp_output_51_0_g2451 - saturate( temp_output_118_0_g2451 ) ) , _MossNormalAffectStrength);
				half temp_output_5_0_g2498 = lerpResult2905;
				half temp_output_48_0_g2498 = saturate( temp_output_5_0_g2498 );
				half temp_output_17_0_g2498 = _MapContrastOffset;
				half temp_output_30_0_g2498 = ( 1.0 - saturate( _MapContrast ) );
				half temp_output_35_0_g2498 = ( temp_output_30_0_g2498 * 0.5 );
				half temp_output_32_0_g2498 = ( (( 1.0 + temp_output_30_0_g2498 ) + (abs( temp_output_17_0_g2498 ) - 0.0) * (( 0.0 - temp_output_30_0_g2498 ) - ( 1.0 + temp_output_30_0_g2498 )) / (1.0 - 0.0)) - temp_output_35_0_g2498 );
				half temp_output_31_0_g2498 = ( temp_output_35_0_g2498 + (( 1.0 + temp_output_30_0_g2498 ) + (abs( temp_output_17_0_g2498 ) - 0.0) * (( 0.0 - temp_output_30_0_g2498 ) - ( 1.0 + temp_output_30_0_g2498 )) / (1.0 - 0.0)) );
				half temp_output_51_0_g2498 = saturate( (0.0 + (temp_output_48_0_g2498 - temp_output_32_0_g2498) * (1.0 - 0.0) / (temp_output_31_0_g2498 - temp_output_32_0_g2498)) );
				#ifdef _MOSS
				half staticSwitch14 = temp_output_51_0_g2498;
				#else
				half staticSwitch14 = 0.0;
				#endif
				half MossMask45 = staticSwitch14;
				half3 lerpResult103 = lerp( AlbedoBaseColor92 , MossAlbedoTinted101 , MossMask45);
				#ifdef _MOSS
				half3 staticSwitch1236 = lerpResult103;
				#else
				half3 staticSwitch1236 = AlbedoBaseColor92;
				#endif
				half temp_output_10_0_g2209 = _DetailsOverMoss;
				half temp_output_17_0_g2209 = saturate( MossMask45 );
				half4 lerpResult7_g2209 = lerp( appendResult3336 , ifLocalVar3338 , saturate( ( saturate( ( abs( temp_output_10_0_g2209 ) - 1.0 ) ) + ( ( ( 1.0 - temp_output_17_0_g2209 ) * abs( min( temp_output_10_0_g2209 , 0.0 ) ) ) + ( temp_output_17_0_g2209 * max( temp_output_10_0_g2209 , 0.0 ) ) ) ) ));
				#ifdef _MOSS
				half4 staticSwitch4126 = lerpResult7_g2209;
				#else
				half4 staticSwitch4126 = ifLocalVar3338;
				#endif
				half4 lerpResult4232 = lerp( appendResult3336 , staticSwitch4126 , (float)_DETAIL);
				#ifdef _DETAIL_ON
				half4 staticSwitch4132 = lerpResult4232;
				#else
				half4 staticSwitch4132 = appendResult3336;
				#endif
				half DetailMapAlbedo987 = (staticSwitch4132).x;
				#ifdef _DETAIL_ON
				half3 staticSwitch1045 = saturate( ( staticSwitch1236 * DetailMapAlbedo987 ) );
				#else
				half3 staticSwitch1045 = staticSwitch1236;
				#endif
				half3 AlbedoRes106 = staticSwitch1045;
				half3 temp_output_35_0_g4236 = AlbedoRes106;
				half2 uv_Layer0 = IN.ase_texcoord8.xyz.xy * _Layer0_ST.xy + _Layer0_ST.zw;
				half2 uvLayer0112_g4236 = uv_Layer0;
				half3 appendResult37_g4236 = (half3(SAMPLE_TEXTURE2D( _Layer0, sampler_Linear_Repeat_Aniso2, uvLayer0112_g4236 ).rgb));
				half2 texCoord24_g4236 = IN.ase_texcoord8.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				half2 baseuv25_g4236 = texCoord24_g4236;
				half4 tex2DNode3_g4236 = SAMPLE_TEXTURE2D( _RevealMask, sampler_Linear_Repeat_Aniso2, baseuv25_g4236 );
				half4 tex2DNode4_g4236 = SAMPLE_TEXTURE2D( _LayerMask, sampler_Linear_Repeat_Aniso2, baseuv25_g4236 );
				half rt31_g4236 = saturate( ( tex2DNode3_g4236.r * tex2DNode4_g4236.r ) );
				half LSAlbedo100_g4236 = _LayerSurfaceExp.x;
				half3 lerpResult34_g4236 = lerp( temp_output_35_0_g4236 , appendResult37_g4236 , pow( rt31_g4236 , LSAlbedo100_g4236 ));
				half2 uv_Layer1 = IN.ase_texcoord8.xyz.xy * _Layer1_ST.xy + _Layer1_ST.zw;
				half2 uvLayer1114_g4236 = uv_Layer1;
				half3 appendResult73_g4236 = (half3(SAMPLE_TEXTURE2D( _Layer1, sampler_Linear_Repeat_Aniso2, uvLayer1114_g4236 ).rgb));
				half gt57_g4236 = saturate( ( tex2DNode3_g4236.g * tex2DNode4_g4236.g ) );
				half3 lerpResult61_g4236 = lerp( lerpResult34_g4236 , appendResult73_g4236 , pow( gt57_g4236 , LSAlbedo100_g4236 ));
				#ifdef _REVEALLAYERS
				half3 staticSwitch1_g4236 = lerpResult61_g4236;
				#else
				half3 staticSwitch1_g4236 = temp_output_35_0_g4236;
				#endif
				half3 temp_output_4603_36 = staticSwitch1_g4236;
				
				half3 appendResult2618 = (half3(unpack2617));
				half3 _Vector14 = half3(0,0,1);
				half3 ifLocalVar3262 = 0;
				if( _UseNormalMap <= 0.5 )
				ifLocalVar3262 = _Vector14;
				else
				ifLocalVar3262 = appendResult2618;
				half3 SimpleNormalXYSigned30 = ifLocalVar3262;
				int MossInt2938 = _Moss;
				half2 _Vector0 = half2(0,0);
				half2 temp_output_3045_0 = (MossMetalRes3077).ag;
				half2 MossPackedNormal1656 = temp_output_3045_0;
				half4 temp_output_5_0_g2453 = half4( MossPackedNormal1656, 0.0 , 0.0 );
				half2 appendResult12_g2453 = (half2(temp_output_5_0_g2453.xy));
				half temp_output_6_0_g2453 = _MossNormalStrength;
				half2 lerpResult2_g2453 = lerp( float2( 0,0 ) , ( appendResult12_g2453 - float2( 0.5,0.5 ) ) , ( temp_output_6_0_g2453 * 2.0 ));
				half2 lerpResult2976 = lerp( _Vector0 , lerpResult2_g2453 , MossMask45);
				half2 MossNormalSigned100 = lerpResult2976;
				half2 temp_output_3610_0 = ( MossInt2938 * MossNormalSigned100 );
				half3 appendResult4006 = (half3(temp_output_3610_0 , 1.0));
				half3 lerpResult4431 = lerp( SimpleNormalXYSigned30 , float3( 0,0,1 ) , MossMask45);
				half2 appendResult1363 = (half2(lerpResult4431.xy));
				#ifdef _MOSS
				half3 staticSwitch4131 = ( appendResult4006 + half3( appendResult1363 ,  0.0 ) );
				#else
				half3 staticSwitch4131 = SimpleNormalXYSigned30;
				#endif
				half2 DetailNormaMossMaskedSigned988 = ( _DetailNormalMapScale * (staticSwitch4132).yz );
				half3 appendResult4397 = (half3(DetailNormaMossMaskedSigned988 , 1.0));
				#ifdef _DETAIL_ON
				half3 staticSwitch1105 = BlendNormal( staticSwitch4131 , appendResult4397 );
				#else
				half3 staticSwitch1105 = staticSwitch4131;
				#endif
				half3 NormalPre2265 = staticSwitch1105;
				half3 normalizeResult4009 = normalize( NormalPre2265 );
				half3 NormalRes109 = normalizeResult4009;
				half3 temp_output_42_0_g4236 = NormalRes109;
				half3 unpack9_g4236 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Layer0NormalMap, sampler_Linear_Repeat_Aniso2, uvLayer0112_g4236 ), _Layer0NormalStrength );
				unpack9_g4236.z = lerp( 1, unpack9_g4236.z, saturate(_Layer0NormalStrength) );
				half LSNormal101_g4236 = _LayerSurfaceExp.y;
				half3 lerpResult41_g4236 = lerp( temp_output_42_0_g4236 , unpack9_g4236 , pow( rt31_g4236 , LSNormal101_g4236 ));
				half3 unpack18_g4236 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Layer1NormalMap, sampler_Linear_Repeat_Aniso2, uvLayer1114_g4236 ), _Layer1NormalStrength );
				unpack18_g4236.z = lerp( 1, unpack18_g4236.z, saturate(_Layer1NormalStrength) );
				half3 lerpResult64_g4236 = lerp( lerpResult41_g4236 , unpack18_g4236 , pow( gt57_g4236 , LSNormal101_g4236 ));
				half2 uv83_g4236 = baseuv25_g4236;
				half bt76_g4236 = saturate( ( tex2DNode3_g4236.b * tex2DNode4_g4236.b ) );
				half height83_g4236 = ( _Layer2Height * bt76_g4236 );
				half2 texelSize83_g4236 = (_RevealMask_TexelSize).xy;
				SamplerState sampler_RevealMask83_g4236 = sampler_Linear_Repeat_Aniso2;
				half3 localRevealMaskNormalCrossFilter83_g4236 = RevealMaskNormalCrossFilter83_g4236( uv83_g4236 , height83_g4236 , texelSize83_g4236 , sampler_RevealMask83_g4236 );
				half3 heightNormal89_g4236 = localRevealMaskNormalCrossFilter83_g4236;
				#ifdef _REVEALLAYERS
				half3 staticSwitch58_g4236 = BlendNormal( lerpResult64_g4236 , heightNormal89_g4236 );
				#else
				half3 staticSwitch58_g4236 = temp_output_42_0_g4236;
				#endif
				half3 temp_output_4603_43 = staticSwitch58_g4236;
				half3 NormalRes24487 = temp_output_4603_43;
				
				half fresnelNdotV135 = dot( WorldNormal, WorldViewDirection );
				half fresnelNode135 = ( _MossFresnel.x + _MossFresnel.y * pow( 1.0 - fresnelNdotV135, ( _MossFresnel.z * 10.0 ) ) );
				half3 SH3558 = half4(0,0,0,0).xyz;
				half3 normalWS3558 = WorldNormal;
				half3 localSampleSHPixel3558 = SampleSHPixel3558( SH3558 , normalWS3558 );
				half2 lightmapUV3567 = half4(0,0,0,0).xy;
				half3 normalWS3567 = WorldNormal;
				half3 localSampleLightmap3567 = SampleLightmap3567( lightmapUV3567 , normalWS3567 );
				#ifdef LIGHTMAP_ON
				half3 staticSwitch3554 = localSampleLightmap3567;
				#else
				half3 staticSwitch3554 = localSampleSHPixel3558;
				#endif
				half3 LightmapOrSH2480 = staticSwitch3554;
				half grayscale4604 = (LightmapOrSH2480.r + LightmapOrSH2480.g + LightmapOrSH2480.b) / 3;
				half3 AlbedoResPre2289 = staticSwitch1045;
				half4 temp_output_2664_0 = ( ( ( min( _MossFresnel.w , saturate( fresnelNode135 ) ) * _MossFresnelColor * grayscale4604 ) * half4( AlbedoResPre2289 , 0.0 ) * saturate( ( MossMask45 - 0.3 ) ) ) * _UseMossFresnel );
				#ifdef _USEMOSSFRESNEL_ON
				half4 staticSwitch1195 = temp_output_2664_0;
				#else
				half4 staticSwitch1195 = float4( 0,0,0,0 );
				#endif
				#ifdef _MOSS
				half4 staticSwitch1237 = staticSwitch1195;
				#else
				half4 staticSwitch1237 = float4( 0,0,0,0 );
				#endif
				half4 MossFresnel203 = staticSwitch1237;
				half3 appendResult3069 = (half3(_EmissionColor.rgb));
				half _MetallicSpecGlossMap3184 = _MetallicSpecGlossMap;
				half temp_output_2918_0 = (MossMetalORMetallicMap2911).r;
				half temp_output_8_0_g2532 = temp_output_2918_0;
				half4 break5_g2532 = _MetallicRemap;
				half temp_output_3_0_g2532 = (0.0 + (temp_output_8_0_g2532 - break5_g2532.x) * (1.0 - 0.0) / (break5_g2532.y - break5_g2532.x));
				half clampResult2_g2532 = clamp( temp_output_3_0_g2532 , break5_g2532.z , break5_g2532.w );
				half temp_output_27_0_g2532 = saturate( clampResult2_g2532 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3732 = temp_output_27_0_g2532;
				#else
				half staticSwitch3732 = temp_output_2918_0;
				#endif
				half temp_output_3002_0 = (MossMetalORMetallicMap2911).g;
				half temp_output_8_0_g2550 = temp_output_3002_0;
				half4 break5_g2550 = _OcclusionRemap;
				half temp_output_3_0_g2550 = (0.0 + (temp_output_8_0_g2550 - break5_g2550.x) * (1.0 - 0.0) / (break5_g2550.y - break5_g2550.x));
				half clampResult2_g2550 = clamp( temp_output_3_0_g2550 , break5_g2550.z , break5_g2550.w );
				half temp_output_27_0_g2550 = saturate( clampResult2_g2550 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3735 = temp_output_27_0_g2550;
				#else
				half staticSwitch3735 = temp_output_3002_0;
				#endif
				half temp_output_12_0_g2502 = ( _UseOcclusion * _MetallicSpecGlossMap3184 * _Occlusion );
				half temp_output_12_0_g2514 = (float)Mode3840;
				half temp_output_392_0 = (MossMetalORMetallicMap2911).a;
				half temp_output_8_0_g2540 = temp_output_392_0;
				half4 break5_g2540 = _SmoothnessRemap;
				half temp_output_3_0_g2540 = (0.0 + (temp_output_8_0_g2540 - break5_g2540.x) * (1.0 - 0.0) / (break5_g2540.y - break5_g2540.x));
				half clampResult2_g2540 = clamp( temp_output_3_0_g2540 , break5_g2540.z , break5_g2540.w );
				half temp_output_27_0_g2540 = saturate( clampResult2_g2540 );
				half temp_output_3894_0 = temp_output_27_0_g2540;
				#ifdef _REMAPPERS_ON
				half staticSwitch3739 = temp_output_3894_0;
				#else
				half staticSwitch3739 = temp_output_392_0;
				#endif
				half4 appendResult3479 = (half4(( staticSwitch3732 * _Metallic ) , ( ( staticSwitch3735 * temp_output_12_0_g2502 ) + ( 1.0 - temp_output_12_0_g2502 ) ) , ( ( ( staticSwitch3738 * temp_output_12_0_g2514 ) + ( 1.0 - temp_output_12_0_g2514 ) ) * _UseEmission ) , ( staticSwitch3739 * _Smoothness )));
				half4 appendResult3481 = (half4(_Metallic , 1.0 , _UseEmission , _Smoothness));
				half4 ifLocalVar3480 = 0;
				if( _MetallicSpecGlossMap3184 <= 0.0 )
				ifLocalVar3480 = appendResult3481;
				else
				ifLocalVar3480 = appendResult3479;
				half UseMossMetalMapInt3239 = _UseMossMetalMap;
				half4 appendResult3487 = (half4(_Metallic , 1.0 , _UseEmission , _Smoothness));
				half4 appendResult3485 = (half4(( staticSwitch3732 * _Metallic ) , ( ( staticSwitch3735 * temp_output_12_0_g2502 ) + ( 1.0 - temp_output_12_0_g2502 ) ) , ( ( ( staticSwitch3738 * temp_output_12_0_g2514 ) + ( 1.0 - temp_output_12_0_g2514 ) ) * _UseEmission ) , ( staticSwitch3739 * _Smoothness )));
				half4 ifLocalVar3486 = 0;
				if( ( MossModeInt3200 + ( 1.0 - UseMossMetalMapInt3239 ) ) <= 0.0 )
				ifLocalVar3486 = appendResult3485;
				else
				ifLocalVar3486 = appendResult3487;
				#ifdef _MOSSMETALMODE_ON
				half4 staticSwitch3494 = ifLocalVar3486;
				#else
				half4 staticSwitch3494 = ifLocalVar3480;
				#endif
				half4 break3482 = staticSwitch3494;
				half EmissionMasked3065 = break3482.z;
				half3 temp_output_3496_0 = ( appendResult3069 * EmissionMasked3065 );
				half4 EmissionRes806 = ( MossFresnel203 + half4( temp_output_3496_0 , 0.0 ) );
				half3 temp_output_91_0_g4236 = EmissionRes806.rgb;
				half at79_g4236 = saturate( ( tex2DNode3_g4236.a * tex2DNode4_g4236.a ) );
				half3 appendResult99_g4236 = (half3(_Layer3EmissionColor.rgb));
				#ifdef _REVEALLAYERS
				half3 staticSwitch93_g4236 = ( ( temp_output_91_0_g4236 * ( 1.0 - at79_g4236 ) ) + ( appendResult99_g4236 * at79_g4236 ) );
				#else
				half3 staticSwitch93_g4236 = temp_output_91_0_g4236;
				#endif
				half OceanUnder289_g4237 = GlobalOceanUnder;
				half3 EmissionIn281_g4237 = float3( 0,0,0 );
				half3 WorldPosition256_g4237 = WorldPosition;
				half3 temp_output_105_0_g4260 = WorldPosition256_g4237;
				half temp_output_117_0_g4260 = (temp_output_105_0_g4260).y;
				half temp_output_67_0_g4237 = ( GlobalOceanOffset + GlobalOceanHeight );
				half OceanHeight274_g4237 = temp_output_67_0_g4237;
				half temp_output_63_0_g4260 = OceanHeight274_g4237;
				half3 temp_output_9_0_g4237 = temp_output_4603_43;
				half3 temp_output_30_0_g4260 = temp_output_9_0_g4237;
				float3 tanNormal12_g4260 = temp_output_30_0_g4260;
				half3 worldNormal12_g4260 = float3(dot(tanToWorld0,tanNormal12_g4260), dot(tanToWorld1,tanNormal12_g4260), dot(tanToWorld2,tanNormal12_g4260));
				half3 vertexToFrag144_g4260 = IN.ase_texcoord11.yzw;
				half dotResult1_g4260 = dot( worldNormal12_g4260 , vertexToFrag144_g4260 );
				half desktop151_g4260 = max( dotResult1_g4260 , 0.0 );
				half mobile151_g4260 = 1.0;
				half localShaderAPIMobile151_g4260 = ShaderAPIMobile151_g4260( desktop151_g4260 , mobile151_g4260 );
				half2 vertexToFrag52_g4260 = IN.ase_texcoord12.xy;
				half4 tex2DNode120_g4260 = SAMPLE_TEXTURE2D( _Caustics, sampler_Linear_Repeat, vertexToFrag52_g4260 );
				half3 appendResult121_g4260 = (half3(tex2DNode120_g4260.r , tex2DNode120_g4260.r , tex2DNode120_g4260.r));
				half2 DistanceFade134_g4260 = (_CausticsSettings).zw;
				half2 break136_g4260 = DistanceFade134_g4260;
				half temp_output_67_0_g4260 = ( saturate( (1.0 + (distance( temp_output_63_0_g4260 , temp_output_117_0_g4260 ) - break136_g4260.x) * (0.0 - 1.0) / (break136_g4260.y - break136_g4260.x)) ) * saturate( (0.0 + (max( -( temp_output_117_0_g4260 - temp_output_63_0_g4260 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) );
				half desktop153_g4260 = temp_output_67_0_g4260;
				half mobile153_g4260 = 1.0;
				half localShaderAPIMobile153_g4260 = ShaderAPIMobile153_g4260( desktop153_g4260 , mobile153_g4260 );
				half CausticMipLevel118_g4260 = ( ( 1.0 - localShaderAPIMobile153_g4260 ) * 4.0 );
				#ifdef _USECAUSTICRAINBOW_ON
				half3 staticSwitch73_g4260 = ( ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Linear_Repeat, ( vertexToFrag52_g4260 + ( 0.0045 * 2.0 ) ), CausticMipLevel118_g4260 ).r * half3(0,0,1) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Linear_Repeat, ( vertexToFrag52_g4260 + 0.0045 ), CausticMipLevel118_g4260 ).r * half3(0,1,0) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Linear_Repeat, vertexToFrag52_g4260, CausticMipLevel118_g4260 ).r * half3(1,0,0) ) );
				#else
				half3 staticSwitch73_g4260 = appendResult121_g4260;
				#endif
				#ifdef _USECAUSTICEXTRASAMPLER_ON
				half3 staticSwitch57_g4260 = staticSwitch73_g4260;
				#else
				half3 staticSwitch57_g4260 = staticSwitch73_g4260;
				#endif
				half3 appendResult62_g4260 = (half3(_CausticsColor.rgb));
				half3 ifLocalVar155_g4260 = 0;
				UNITY_BRANCH 
				if( temp_output_117_0_g4260 < ( temp_output_63_0_g4260 + 0.2 ) )
				ifLocalVar155_g4260 = ( localShaderAPIMobile151_g4260 * staticSwitch57_g4260 * appendResult62_g4260 * localShaderAPIMobile153_g4260 );
				half3 lerpResult205_g4237 = lerp( ( EmissionIn281_g4237 + ifLocalVar155_g4260 ) , ( ( EmissionIn281_g4237 + ifLocalVar155_g4260 ) * 30.0 ) , OceanUnder289_g4237);
				half3 appendResult100_g4251 = (half3(OceanWaterTint_RGB.xyz));
				half3 WorldPos252_g4251 = WorldPosition256_g4237;
				half temp_output_108_0_g4251 = OceanHeight274_g4237;
				half3 ViewDir264_g4237 = WorldViewDirection;
				half3 viewDir240_g4251 = ViewDir264_g4237;
				half3 camWorldPos240_g4251 = _WorldSpaceCameraPos;
				half3 posWS240_g4251 = WorldPos252_g4251;
				half4 oceanFogDensities240_g4251 = OceanFogDensities;
				half oceanHeight240_g4251 = temp_output_108_0_g4251;
				half4 oceanFogTop_RGB_Exponent240_g4251 = OceanFogTop_RGB_Exponent;
				half4 oceanFogBottom_RGB_Intensity240_g4251 = OceanFogBottom_RGB_Intensity;
				half4 localGetUnderWaterFogs240_g4251 = GetUnderWaterFogs240_g4251( viewDir240_g4251 , camWorldPos240_g4251 , posWS240_g4251 , oceanFogDensities240_g4251 , oceanHeight240_g4251 , oceanFogTop_RGB_Exponent240_g4251 , oceanFogBottom_RGB_Intensity240_g4251 );
				half4 ifLocalVar257_g4251 = 0;
				UNITY_BRANCH 
				if( (WorldPos252_g4251).y < ( temp_output_108_0_g4251 + 0.2 ) )
				ifLocalVar257_g4251 = localGetUnderWaterFogs240_g4251;
				half4 FogRes185_g4251 = ifLocalVar257_g4251;
				half3 appendResult94_g4251 = (half3(FogRes185_g4251.xyz));
				half3 lerpResult36_g4251 = lerp( ( lerpResult205_g4237 * appendResult100_g4251 ) , appendResult94_g4251 , (FogRes185_g4251).w);
				half3 ifLocalVar5_g4237 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g4237 >= 1.0 )
				ifLocalVar5_g4237 = lerpResult36_g4251;
				else
				ifLocalVar5_g4237 = EmissionIn281_g4237;
				half temp_output_254_0_g4237 = (WorldPosition256_g4237).y;
				half temp_output_137_0_g4237 = ( temp_output_254_0_g4237 - OceanHeight274_g4237 );
				half temp_output_24_0_g4247 = temp_output_137_0_g4237;
				half temp_output_44_0_g4247 = 0.1;
				half temp_output_45_0_g4247 = 0.31;
				half temp_output_46_0_g4247 = saturate( (0.0 + (( temp_output_24_0_g4247 - temp_output_44_0_g4247 ) - 0.0) * (1.0 - 0.0) / (temp_output_45_0_g4247 - 0.0)) );
				#ifdef _FadeWithHeight
				half staticSwitch238_g4237 = ( 1.0 - temp_output_46_0_g4247 );
				#else
				half staticSwitch238_g4237 = 1.0;
				#endif
				half FadeFromY295_g4237 = staticSwitch238_g4237;
				half3 lerpResult174_g4237 = lerp( ( ifLocalVar5_g4237 + lerpResult205_g4237 ) , ifLocalVar5_g4237 , ( OceanUnder289_g4237 * FadeFromY295_g4237 ));
				half3 lerpResult242_g4237 = lerp( EmissionIn281_g4237 , lerpResult174_g4237 , FadeFromY295_g4237);
				#ifdef _USECAUSTICSFROMABOVE_ON
				half3 staticSwitch172_g4237 = lerpResult242_g4237;
				#else
				half3 staticSwitch172_g4237 = ifLocalVar5_g4237;
				#endif
				#ifdef _USEUNDERWATER
				half3 staticSwitch211_g4237 = staticSwitch172_g4237;
				#else
				half3 staticSwitch211_g4237 = float3( 0,0,0 );
				#endif
				half3 temp_output_4575_8 = staticSwitch211_g4237;
				
				half Metallic399 = break3482.x;
				half MetallicSimpler398 = Metallic399;
				half lerpResult653 = lerp( MetallicSimpler398 , _MossMetallic , MossMask45);
				#ifdef _MOSS
				half staticSwitch1238 = lerpResult653;
				#else
				half staticSwitch1238 = MetallicSimpler398;
				#endif
				half MetallicResult1087 = staticSwitch1238;
				half temp_output_47_0_g4236 = MetallicResult1087;
				half LSMetallic102_g4236 = _LayerSurfaceExp.z;
				half lerpResult46_g4236 = lerp( temp_output_47_0_g4236 , _Layer0Metallic , pow( rt31_g4236 , LSMetallic102_g4236 ));
				half lerpResult67_g4236 = lerp( lerpResult46_g4236 , _Layer1Metallic , pow( gt57_g4236 , LSMetallic102_g4236 ));
				#ifdef _REVEALLAYERS
				half staticSwitch59_g4236 = lerpResult67_g4236;
				#else
				half staticSwitch59_g4236 = temp_output_47_0_g4236;
				#endif
				
				half Smoothness400 = break3482.w;
				half temp_output_3044_0 = (MossMetalRes3077).b;
				half temp_output_8_0_g2536 = temp_output_3044_0;
				half4 break5_g2536 = _MossSmoothnessRemap;
				half temp_output_3_0_g2536 = (0.0 + (temp_output_8_0_g2536 - break5_g2536.x) * (1.0 - 0.0) / (break5_g2536.y - break5_g2536.x));
				half clampResult2_g2536 = clamp( temp_output_3_0_g2536 , break5_g2536.z , break5_g2536.w );
				half temp_output_27_0_g2536 = saturate( clampResult2_g2536 );
				#ifdef _REMAPPERS_ON
				half staticSwitch3746 = temp_output_27_0_g2536;
				#else
				half staticSwitch3746 = temp_output_3044_0;
				#endif
				half MossPackedSmoothness1660 = staticSwitch3746;
				half lerpResult650 = lerp( Smoothness400 , ( _MossSmoothness * MossPackedSmoothness1660 ) , MossMask45);
				#ifdef _MOSS
				half staticSwitch4121 = lerpResult650;
				#else
				half staticSwitch4121 = Smoothness400;
				#endif
				half DetailMapSmoothness1078 = (staticSwitch4132).w;
				#ifdef _DETAIL_ON
				half staticSwitch1093 = saturate( ( staticSwitch4121 + DetailMapSmoothness1078 ) );
				#else
				half staticSwitch1093 = staticSwitch4121;
				#endif
				half SmoothnessResult1085 = staticSwitch1093;
				half temp_output_54_0_g4236 = SmoothnessResult1085;
				half LSSmoothness103_g4236 = _LayerSurfaceExp.w;
				half lerpResult51_g4236 = lerp( temp_output_54_0_g4236 , _Layer0Smoothness , pow( rt31_g4236 , LSSmoothness103_g4236 ));
				half lerpResult70_g4236 = lerp( lerpResult51_g4236 , _Layer1Smoothness , pow( gt57_g4236 , LSSmoothness103_g4236 ));
				#ifdef _REVEALLAYERS
				half staticSwitch60_g4236 = lerpResult70_g4236;
				#else
				half staticSwitch60_g4236 = temp_output_54_0_g4236;
				#endif
				
				half OcclusionDoodad3006 = break3482.y;
				half temp_output_10_0_g2275 = _OcclusionMossMask;
				half temp_output_17_0_g2275 = saturate( MossMask45 );
				half lerpResult7_g2275 = lerp( (float)1 , OcclusionDoodad3006 , saturate( ( saturate( ( abs( temp_output_10_0_g2275 ) - 1.0 ) ) + ( ( ( 1.0 - temp_output_17_0_g2275 ) * abs( min( temp_output_10_0_g2275 , 0.0 ) ) ) + ( temp_output_17_0_g2275 * max( temp_output_10_0_g2275 , 0.0 ) ) ) ) ));
				#ifdef _MOSS
				half staticSwitch3730 = lerpResult7_g2275;
				#else
				half staticSwitch3730 = OcclusionDoodad3006;
				#endif
				#ifdef _MOSSMETALMODE_ON
				half staticSwitch3019 = OcclusionDoodad3006;
				#else
				half staticSwitch3019 = staticSwitch3730;
				#endif
				half OcclusionResMStrength369 = staticSwitch3019;
				
				half4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				half2 clipScreen3958 = ase_screenPosNorm.xy * _ScreenParams.xy;
				half dither3958 = Dither4x4Bayer( fmod(clipScreen3958.x, 4), fmod(clipScreen3958.y, 4) );
				dither3958 = step( dither3958, temp_output_3952_0 );
				half lerpResult3962 = lerp( (float)AlphaToCoverage , 1.0 , _Surface);
				half Dither3961 = ( _Dither * ( 1.0 - lerpResult3962 ) );
				half lerpResult3959 = lerp( temp_output_3952_0 , dither3958 , Dither3961);
				#ifdef _ALPHATEST_ON
				half staticSwitch3334 = lerpResult3959;
				#else
				half staticSwitch3334 = 1.0;
				#endif
				half AlbedoAlpha84 = staticSwitch3334;
				
				half lerpResult3950 = lerp( 0.0 , _AlphaClipThreshold , _AlphaClip);
				half lerpResult3291 = lerp( lerpResult3950 , 0.0 , (float)AlphaToCoverage);
				half ClipCalc3294 = lerpResult3291;
				
				half ShadowThreshold2627 = _ShadowThreshold;
				
				half4x4 temp_output_28_0_g4351 = _ProbeWorldToTexture;
				half4x4 Input90_g4351 = temp_output_28_0_g4351;
				half4x4 localRotationOnly90_g4351 = RotationOnly90_g4351( Input90_g4351 );
				float3 tanNormal4461 = NormalRes24487;
				half3 worldNormal4461 = float3(dot(tanToWorld0,tanNormal4461), dot(tanToWorld1,tanNormal4461), dot(tanToWorld2,tanNormal4461));
				half3 worldNormal14_g4351 = mul( localRotationOnly90_g4351, half4( worldNormal4461 , 0.0 ) ).xyz;
				half3 vertexToFrag7_g4351 = IN.ase_texcoord13.xyz;
				half4 ProbeVolumeShR14_g4351 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShR, sampler_Linear_Clamp, ( vertexToFrag7_g4351 + half3( float2( 0,0 ) ,  0.0 ) ), 0.0 );
				half4 ProbeVolumeShG14_g4351 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShG, sampler_Linear_Clamp, ( vertexToFrag7_g4351 + half3( float2( 0,0 ) ,  0.0 ) ), 0.0 );
				half4 ProbeVolumeShB14_g4351 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShB, sampler_Linear_Clamp, ( vertexToFrag7_g4351 + half3( float2( 0,0 ) ,  0.0 ) ), 0.0 );
				half3 localSHEvalLinearL0L114_g4351 = SHEvalLinearL0L114_g4351( worldNormal14_g4351 , ProbeVolumeShR14_g4351 , ProbeVolumeShG14_g4351 , ProbeVolumeShB14_g4351 );
				#ifdef _PROBEVOLUME_ON
				half3 staticSwitch20_g4351 = localSHEvalLinearL0L114_g4351;
				#else
				half3 staticSwitch20_g4351 = LightmapOrSH2480;
				#endif
				half3 temp_output_4622_0 = staticSwitch20_g4351;
				half3 BakedGI1212 = temp_output_4622_0;
				

				float3 BaseColor = temp_output_4603_36;
				float3 Normal = NormalRes24487;
				float3 Emission = ( staticSwitch93_g4236 + ( temp_output_4603_36 * temp_output_4575_8 ) );
				float3 Specular = 0.5;
				float Metallic = staticSwitch59_g4236;
				float Smoothness = staticSwitch60_g4236;
				float Occlusion = OcclusionResMStrength369;
				float Alpha = AlbedoAlpha84;
				float AlphaClipThreshold = ClipCalc3294;
				float AlphaClipThresholdShadow = ShadowThreshold2627;
				float3 BakedGI = BakedGI1212;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;

				#ifdef ASE_DEPTH_WRITE_ON
					float DepthValue = IN.clipPos.z;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData = (InputData)0;
				inputData.positionWS = WorldPosition;
				inputData.positionCS = IN.clipPos;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
						inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
						inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
						inputData.normalWS = Normal;
					#endif
				#else
					inputData.normalWS = WorldNormal;
				#endif

				inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				inputData.viewDirectionWS = SafeNormalize( WorldViewDirection );

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				#ifdef ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#else
					#if defined(DYNAMICLIGHTMAP_ON)
						inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, IN.dynamicLightmapUV.xy, SH, inputData.normalWS);
					#else
						inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
					#endif
				#endif

				inputData.normalizedScreenSpaceUV = NormalizedScreenSpaceUV;
				inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUVOrVertexSH.xy);

				#if defined(DEBUG_DISPLAY)
					#if defined(DYNAMICLIGHTMAP_ON)
						inputData.dynamicLightmapUV = IN.dynamicLightmapUV.xy;
						#endif
					#if defined(LIGHTMAP_ON)
						inputData.staticLightmapUV = IN.lightmapUVOrVertexSH.xy;
					#else
						inputData.vertexSH = SH;
					#endif
				#endif

				#ifdef _DBUFFER
					ApplyDecal(IN.clipPos,
						BaseColor,
						Specular,
						inputData.normalWS,
						Metallic,
						Occlusion,
						Smoothness);
				#endif

				BRDFData brdfData;
				InitializeBRDFData
				(BaseColor, Metallic, Specular, Smoothness, Alpha, brdfData);

				Light mainLight = GetMainLight(inputData.shadowCoord, inputData.positionWS, inputData.shadowMask);
				half4 color;
				MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI, inputData.shadowMask);
				color.rgb = GlobalIllumination(brdfData, inputData.bakedGI, Occlusion, inputData.positionWS, inputData.normalWS, inputData.viewDirectionWS);
				color.a = Alpha;

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return BRDFDataToGbuffer(brdfData, inputData, Smoothness, Emission + color.rgb, Occlusion);
			}

			ENDHLSL
		}
		
		// Shadowood: Application SpaceWarp: https://developers.meta.com/horizon/documentation/unity/unity-asw/
		///*ase_pass*/
        //Pass
        //{
        //	
        //    Name "MotionVectors"
        //    Tags{ "LightMode" = "MotionVectors" }
        //    Tags { "RenderType" = "Opaque" }
        //    ZWrite On
        //    Cull Back
        //    ZTest LEqual
        //    HLSLPROGRAM
        /*ase_pragma_before*/
        //    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/OculusMotionVectorCore.hlsl"
        //    #pragma vertex vert
        //    #pragma fragment frag
        //    ENDHLSL
        //}
        
		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }
			
			AlphaToMask Off

			Cull Off

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_FOG 1
			#define _ASE_SSSCONTROL 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define TONEMAPPINGELSEWHERE
			#define UNDERWATERELSEWHERE
			#define ASE_USING_SAMPLING_MACROS 1


			#pragma vertex vert
			#pragma fragment frag

			#define SCENESELECTIONPASS 1

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY
			
			//Shadowood
			#ifdef _ASE_ALPHATEST
				#pragma shader_feature_local _ _ALPHATEST_ON
			#endif
			//#ifdef SW_TESSELLATION
			//	#define ASE_TESSELLATION 1
			//	#pragma require tessellation tessHW
			//	#pragma hull HullFunction
			//	#pragma domain DomainFunction
			//	//#define ASE_DISTANCE_TESSELLATION
			//#endif

			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
			
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include_with_pragmas  "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _USEMOSSNOISE_ON
			#pragma shader_feature_local MODED_MOSS MODED_MOES MODED_MODS MODED_OFF
			#pragma shader_feature_local _MOSS
			#pragma shader_feature_local _AUTONORMAL
			#pragma shader_feature_local LAYER_UNLIMITED LAYER_MAX2 LAYER_MAX3
			#pragma shader_feature_local TEXTURE_MAX4 TEXTURE_MAX8 TEXTURE_MAX12 TEXTURE_MAX16
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _MOSSMETALMODE_ON
			#pragma shader_feature_local _ALPHAMODE
			#pragma shader_feature_local _SIMPLESTOCHASTIC
			#pragma shader_feature_local _USESPLAT_ON
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USE_SSS_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON
			#define _UseMossPacked
			#define _DetailMapPacking


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Tint3;
			half4 _DetailMapScale;
			half4 _SmoothnessRemap;
			half4 _DetailMaskRemap;
			half4 _MossAlbedoRemap;
			half4 _EmissionRemap;
			half4 _MossAlphaMaskMM;
			half4 _OcclusionRemap;
			half4 _DetailSmoothnessRemap;
			half4x4 _ProbeWorldToTexture;
			half4 _MetallicRemap;
			half4 _MossColor;
			half4 _EmissionColor;
			float4 _RevealMask_TexelSize;
			half4 _MossSmoothnessRemap;
			half4 _Tint1;
			half4 _MossFresnel;
			half4 _MossFresnelColor;
			half4 _Layer3EmissionColor;
			half4 _Layer1_ST;
			half4 _LayerSurfaceExp;
			half4 _Layer0_ST;
			half4 _Tint2;
			half4 _MossSlopeNormal_ST;
			half4 _BaseMap_ST;
			half4 _BaseColor;
			half4 _MossSlopeMM;
			half4 _Tint0;
			half4 _SSSColor;
			half3 _ProbeVolumeMin;
			half3 _MossDirection;
			half3 _ProbeVolumeSizeInv;
			half _MossDetailNormalMapScale;
			half _UseEmission;
			half _Occlusion;
			half _UseOcclusion;
			half _Metallic;
			half _UseMossFresnel;
			half _Smoothness;
			half _MossMetallic;
			half _Layer1Smoothness;
			half _Layer1Metallic;
			half _SSSDimAlbedo;
			half _NormalStrength_USE_SSS;
			half _SSScattering;
			half _SSSRadius;
			half _MossNormalStrength2;
			half _MossSlopeDistort;
			half _MossSlopeNormRotate;
			half _DebugScale;
			int _DebugVisual;
			half _ShadowThreshold;
			half _AlphaClip;
			half _AlphaClipThreshold;
			half _Surface;
			half _Dither;
			half _OcclusionMossMask;
			half _Layer0Smoothness;
			half _MossSmoothness;
			half _Layer0Metallic;
			half _Layer2Height;
			int _DETAIL;
			half _Layer0NormalStrength;
			int _MetalUV;
			int _Moss;
			half _UseMossMetalMap;
			int _MossMode;
			half _MossStochasticContrast;
			half _MossStochasticScale;
			float _MossScale;
			int _MossUV;
			half _MossMultAlbedo;
			int _UseColorMask;
			int _Bitmask;
			int _ZWrite;
			int _Cullmode;
			half _Blend;
			half _SrcBlend;
			half _DstBlend;
			half _Cutoff;
			int _Int0;
			int _BlendMode_UseColorMask;
			half _Layer1NormalStrength;
			half _MossBase;
			half _MetallicSpecGlossMap;
			half _DetailNormalMapScale;
			half _MossNormalStrength;
			half _UseNormalMap;
			half _DetailsOverMoss;
			half _MapContrast;
			half _MapContrastOffset;
			half _MossNormalAffectStrength;
			half _MossNormalSubtract;
			half _DetailAlbedoMapScale;
			half _MossNormalContrast;
			half _UseMossVertexMask;
			half _UseMossMaskWithAlpha;
			half _UseMossDirection;
			half _MossDirContrast;
			half _MossLevel;
			half _DetailNormalMapScale1;
			half _UseDetailMask;
			int _MODE;
			half _NormalStrength;
			half _SSShadcowMix;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			float _DebugCounter; //Shadowood
			half _EnvironmentReflections; // Shadowood
			CBUFFER_END

			// Property used by ScenePickingPass
			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			// Properties used by SceneSelectionPass
			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			TEXTURE2D_ARRAY(_ExtraArray);
			SAMPLER(sampler_ExtraArray);
			TEXTURE2D_ARRAY(_DiffuseArray);
			SAMPLER(sampler_DiffuseArray);
			TEXTURE2D_ARRAY(_NormalArray);
			SAMPLER(sampler_NormalArray);
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_Linear_Repeat_Aniso2);
			int AlphaToCoverage;


			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

			//#ifdef HAVE_VFX_MODIFICATION
			//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
			//#endif

			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			half3 VertVanisher3872( half3 vertex, half2 vtexcoord1, int bitMask )
			{
				 if(bitMask & (int)vtexcoord1.x){
					vertex *= (0.0 / 0.0);
				}
				return vertex;
			}
			
			inline float Dither4x4Bayer( int x, int y )
			{
				const float dither[ 16 ] = {
			 1,  9,  3, 11,
			13,  5, 15,  7,
			 4, 12,  2, 10,
			16,  8, 14,  6 };
				int r = y * 4 + x;
				return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				half3 vertex3872 = v.vertex.xyz;
				half2 vtexcoord13872 = v.ase_texcoord1.xy;
				int bitMask3872 = _Bitmask;
				half3 localVertVanisher3872 = VertVanisher3872( vertex3872 , vtexcoord13872 , bitMask3872 );
				half3 VertexOcclusionPosition3873 = localVertVanisher3872;
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord1 = screenPos;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOcclusionPosition3873;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				o.clipPos = TransformWorldToHClip(positionWS);

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				half2 uv_BaseMap = IN.ase_texcoord.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				half2 UVBasemap02088 = uv_BaseMap;
				half4 tex2DNode80 = SAMPLE_TEXTURE2D( _BaseMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half temp_output_3952_0 = ( tex2DNode80.a * _BaseColor.a );
				float4 screenPos = IN.ase_texcoord1;
				half4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				half2 clipScreen3958 = ase_screenPosNorm.xy * _ScreenParams.xy;
				half dither3958 = Dither4x4Bayer( fmod(clipScreen3958.x, 4), fmod(clipScreen3958.y, 4) );
				dither3958 = step( dither3958, temp_output_3952_0 );
				half lerpResult3962 = lerp( (float)AlphaToCoverage , 1.0 , _Surface);
				half Dither3961 = ( _Dither * ( 1.0 - lerpResult3962 ) );
				half lerpResult3959 = lerp( temp_output_3952_0 , dither3958 , Dither3961);
				#ifdef _ALPHATEST_ON
				half staticSwitch3334 = lerpResult3959;
				#else
				half staticSwitch3334 = 1.0;
				#endif
				half AlbedoAlpha84 = staticSwitch3334;
				
				half lerpResult3950 = lerp( 0.0 , _AlphaClipThreshold , _AlphaClip);
				half lerpResult3291 = lerp( lerpResult3950 , 0.0 , (float)AlphaToCoverage);
				half ClipCalc3294 = lerpResult3291;
				

				surfaceDescription.Alpha = AlbedoAlpha84;
				surfaceDescription.AlphaClipThreshold = ClipCalc3294;

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = 0;

				#ifdef SCENESELECTIONPASS
					outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				#elif defined(SCENEPICKINGPASS)
					outColor = _SelectionID;
				#endif

				return outColor;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ScenePickingPass"
			Tags { "LightMode"="Picking" }
			
			AlphaToMask Off

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_FOG 1
			#define _ASE_SSSCONTROL 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define TONEMAPPINGELSEWHERE
			#define UNDERWATERELSEWHERE
			#define ASE_USING_SAMPLING_MACROS 1


			#pragma vertex vert
			#pragma fragment frag

		    #define SCENEPICKINGPASS 1

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY
			
			//Shadowood
			#ifdef _ASE_ALPHATEST
				#pragma shader_feature_local _ _ALPHATEST_ON
			#endif
			//#ifdef SW_TESSELLATION
			//	#define ASE_TESSELLATION 1
			//	#pragma require tessellation tessHW
			//	#pragma hull HullFunction
			//	#pragma domain DomainFunction
			//	//#define ASE_DISTANCE_TESSELLATION
			//#endif

			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
			
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include_with_pragmas  "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _USEMOSSNOISE_ON
			#pragma shader_feature_local MODED_MOSS MODED_MOES MODED_MODS MODED_OFF
			#pragma shader_feature_local _MOSS
			#pragma shader_feature_local _AUTONORMAL
			#pragma shader_feature_local LAYER_UNLIMITED LAYER_MAX2 LAYER_MAX3
			#pragma shader_feature_local TEXTURE_MAX4 TEXTURE_MAX8 TEXTURE_MAX12 TEXTURE_MAX16
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _MOSSMETALMODE_ON
			#pragma shader_feature_local _ALPHAMODE
			#pragma shader_feature_local _SIMPLESTOCHASTIC
			#pragma shader_feature_local _USESPLAT_ON
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USE_SSS_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON
			#define _UseMossPacked
			#define _DetailMapPacking


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			half4 _Tint3;
			half4 _DetailMapScale;
			half4 _SmoothnessRemap;
			half4 _DetailMaskRemap;
			half4 _MossAlbedoRemap;
			half4 _EmissionRemap;
			half4 _MossAlphaMaskMM;
			half4 _OcclusionRemap;
			half4 _DetailSmoothnessRemap;
			half4x4 _ProbeWorldToTexture;
			half4 _MetallicRemap;
			half4 _MossColor;
			half4 _EmissionColor;
			float4 _RevealMask_TexelSize;
			half4 _MossSmoothnessRemap;
			half4 _Tint1;
			half4 _MossFresnel;
			half4 _MossFresnelColor;
			half4 _Layer3EmissionColor;
			half4 _Layer1_ST;
			half4 _LayerSurfaceExp;
			half4 _Layer0_ST;
			half4 _Tint2;
			half4 _MossSlopeNormal_ST;
			half4 _BaseMap_ST;
			half4 _BaseColor;
			half4 _MossSlopeMM;
			half4 _Tint0;
			half4 _SSSColor;
			half3 _ProbeVolumeMin;
			half3 _MossDirection;
			half3 _ProbeVolumeSizeInv;
			half _MossDetailNormalMapScale;
			half _UseEmission;
			half _Occlusion;
			half _UseOcclusion;
			half _Metallic;
			half _UseMossFresnel;
			half _Smoothness;
			half _MossMetallic;
			half _Layer1Smoothness;
			half _Layer1Metallic;
			half _SSSDimAlbedo;
			half _NormalStrength_USE_SSS;
			half _SSScattering;
			half _SSSRadius;
			half _MossNormalStrength2;
			half _MossSlopeDistort;
			half _MossSlopeNormRotate;
			half _DebugScale;
			int _DebugVisual;
			half _ShadowThreshold;
			half _AlphaClip;
			half _AlphaClipThreshold;
			half _Surface;
			half _Dither;
			half _OcclusionMossMask;
			half _Layer0Smoothness;
			half _MossSmoothness;
			half _Layer0Metallic;
			half _Layer2Height;
			int _DETAIL;
			half _Layer0NormalStrength;
			int _MetalUV;
			int _Moss;
			half _UseMossMetalMap;
			int _MossMode;
			half _MossStochasticContrast;
			half _MossStochasticScale;
			float _MossScale;
			int _MossUV;
			half _MossMultAlbedo;
			int _UseColorMask;
			int _Bitmask;
			int _ZWrite;
			int _Cullmode;
			half _Blend;
			half _SrcBlend;
			half _DstBlend;
			half _Cutoff;
			int _Int0;
			int _BlendMode_UseColorMask;
			half _Layer1NormalStrength;
			half _MossBase;
			half _MetallicSpecGlossMap;
			half _DetailNormalMapScale;
			half _MossNormalStrength;
			half _UseNormalMap;
			half _DetailsOverMoss;
			half _MapContrast;
			half _MapContrastOffset;
			half _MossNormalAffectStrength;
			half _MossNormalSubtract;
			half _DetailAlbedoMapScale;
			half _MossNormalContrast;
			half _UseMossVertexMask;
			half _UseMossMaskWithAlpha;
			half _UseMossDirection;
			half _MossDirContrast;
			half _MossLevel;
			half _DetailNormalMapScale1;
			half _UseDetailMask;
			int _MODE;
			half _NormalStrength;
			half _SSShadcowMix;
			#ifdef ASE_TRANSMISSION
				float _TransmissionShadow;
			#endif
			#ifdef ASE_TRANSLUCENCY
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			float _DebugCounter; //Shadowood
			half _EnvironmentReflections; // Shadowood
			CBUFFER_END

			// Property used by ScenePickingPass
			#ifdef SCENEPICKINGPASS
				float4 _SelectionID;
			#endif

			// Properties used by SceneSelectionPass
			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif

			TEXTURE2D_ARRAY(_ExtraArray);
			SAMPLER(sampler_ExtraArray);
			TEXTURE2D_ARRAY(_DiffuseArray);
			SAMPLER(sampler_DiffuseArray);
			TEXTURE2D_ARRAY(_NormalArray);
			SAMPLER(sampler_NormalArray);
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_Linear_Repeat_Aniso2);
			int AlphaToCoverage;


			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

			//#ifdef HAVE_VFX_MODIFICATION
			//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
			//#endif

			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			half3 VertVanisher3872( half3 vertex, half2 vtexcoord1, int bitMask )
			{
				 if(bitMask & (int)vtexcoord1.x){
					vertex *= (0.0 / 0.0);
				}
				return vertex;
			}
			
			inline float Dither4x4Bayer( int x, int y )
			{
				const float dither[ 16 ] = {
			 1,  9,  3, 11,
			13,  5, 15,  7,
			 4, 12,  2, 10,
			16,  8, 14,  6 };
				int r = y * 4 + x;
				return dither[r] / 16; // same # of instructions as pre-dividing due to compiler magic
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				half3 vertex3872 = v.vertex.xyz;
				half2 vtexcoord13872 = v.ase_texcoord1.xy;
				int bitMask3872 = _Bitmask;
				half3 localVertVanisher3872 = VertVanisher3872( vertex3872 , vtexcoord13872 , bitMask3872 );
				half3 VertexOcclusionPosition3873 = localVertVanisher3872;
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord1 = screenPos;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOcclusionPosition3873;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				o.clipPos = TransformWorldToHClip(positionWS);

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				half2 uv_BaseMap = IN.ase_texcoord.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
				half2 UVBasemap02088 = uv_BaseMap;
				half4 tex2DNode80 = SAMPLE_TEXTURE2D( _BaseMap, sampler_Linear_Repeat_Aniso2, UVBasemap02088 );
				half temp_output_3952_0 = ( tex2DNode80.a * _BaseColor.a );
				float4 screenPos = IN.ase_texcoord1;
				half4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				half2 clipScreen3958 = ase_screenPosNorm.xy * _ScreenParams.xy;
				half dither3958 = Dither4x4Bayer( fmod(clipScreen3958.x, 4), fmod(clipScreen3958.y, 4) );
				dither3958 = step( dither3958, temp_output_3952_0 );
				half lerpResult3962 = lerp( (float)AlphaToCoverage , 1.0 , _Surface);
				half Dither3961 = ( _Dither * ( 1.0 - lerpResult3962 ) );
				half lerpResult3959 = lerp( temp_output_3952_0 , dither3958 , Dither3961);
				#ifdef _ALPHATEST_ON
				half staticSwitch3334 = lerpResult3959;
				#else
				half staticSwitch3334 = 1.0;
				#endif
				half AlbedoAlpha84 = staticSwitch3334;
				
				half lerpResult3950 = lerp( 0.0 , _AlphaClipThreshold , _AlphaClip);
				half lerpResult3291 = lerp( lerpResult3950 , 0.0 , (float)AlphaToCoverage);
				half ClipCalc3294 = lerpResult3291;
				

				surfaceDescription.Alpha = AlbedoAlpha84;
				surfaceDescription.AlphaClipThreshold = ClipCalc3294;

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
						clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = 0;

				#ifdef SCENESELECTIONPASS
					outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				#elif defined(SCENEPICKINGPASS)
					outColor = _SelectionID;
				#endif

				return outColor;
			}

			ENDHLSL
		}
		
	}
	
	CustomEditor "ShaderSorceryInspector" //Shadowood: was "UnityEditor.ShaderGraphLitGUI"
	FallBack Off //"Hidden/Shader Graph/FallbackError"
	
	Fallback Off
}
