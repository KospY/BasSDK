// Made with Amplify Shader Editor v1.9.3.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ThunderRoad/Dev/Shoreline-Dev"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector]_CameraForward("_CameraForward", Vector) = (0,0,0,0)
		_HeaderBakedLighting("# Baked Lighting", Float) = 0
		_bakeLightmapST("bakeLightmapST", Vector) = (1,1,0,0)
		[NoScaleOffset][SingleLineTexture]_bakeLightmap("bakeLightmap", 2D) = "white" {}
		[NoScaleOffset][SingleLineTexture]_bakeLightmapSM("bakeLightmapSM", 2D) = "white" {}
		_BakedLightingFeather("Baked Lighting Feather", Range( 0 , 1)) = 0.05
		[HDR]_BakeLightMapAmbient("BakeLightMapAmbient", Color) = (1,1,1,0)
		[Feature(_DEBUGVISUALS)]_HeaderDebug("# Debug", Float) = 0
		[Toggle(_DEBUGVISUALS_ON)] _DebugVisuals("Debug Visuals", Float) = 0
		[MaterialEnumDrawerExtended(ColorCache_RGB,0,ColorCache_A_Depth,1,DepthCache_RG_Dir,2,DepthCache_B_Dist,3,DepthCache_A_Depth,4,Waves_Raw,5,Waves_FoamMaskResult,6,RawReflection,7,DepthTint,8,ShoreMaskDepthAlpha,9,ShoreMaskVert,10,ShoreMaskSpec,11,VertAdded,12,Lightmap,13,ShadowMask,14)][Feature(_DEBUGVISUALS)]_DebugVisual("Debug Visual [_DebugVisuals]", Int) = 0
		[Toggle(LIGHTMAP_ON)] _LIGHTMAP_ON("LIGHTMAP_ON", Float) = 1
		_HeaderGeneral("# General", Float) = 0
		_Smoothness("Smoothness (Cubemap Only)", Range( 0 , 1)) = 0.8
		[HideInInspector]_TexRight("TexRight", 2D) = "black" {}
		[HideInInspector]_TexLeft("TexLeft", 2D) = "black" {}
		[HideInInspector]_WorldPos("WorldPos", Vector) = (0,0,0,0)
		[HideInInspector]_WorldDir("WorldDir", Vector) = (0,0,0,0)
		[HideInInspector]_ForceEye("ForceEye", Float) = 0
		[NoScaleOffset]_ColorCache("ColorCache &", 2D) = "white" {}
		[NoScaleOffset]_DepthCache("DepthCache &", 2D) = "white" {}
		[Toggle(_HORIZONMAPPED_ON)] _HorizonMapped("HorizonMapped", Float) = 0
		[KeywordEnum(WorldSpace,UVSpace)] _Space("Space", Float) = 0
		_HeaderNormals("# Normals", Float) = 0
		_RefDistortion("Reflection Bump Distortion", Range( 0 , 0.1)) = 0.02
		_NormalDarken("Normal Darken", Range( 0 , 1)) = 0.25
		_NormalDarken1("Normal Darken Range", Float) = 1000
		_Normals("Normals", 2D) = "bump" {}
		_NormalPow("Normal Pow", Range( 0 , 1)) = 0.1869633
		[Toggle(_USEEXTRANORMALS_ON)] _UseExtraNormals("Use Extra Normals", Float) = 1
		_Normals2("Normals Extra [_UseExtraNormals]", 2D) = "bump" {}
		_NormalPow2("Normal Pow 2 [_UseExtraNormals]", Range( 0 , 1)) = 1
		HeaderCaustics("# Caustics", Float) = 0
		[Toggle(_CAUSTICSENABLE_ON)] _CausticsEnable("CausticsEnable", Float) = 1
		[Toggle(_USECAUSTICEXTRASAMPLER_ON)] _UseCausticExtraSampler("Use Extra Sampler [_CausticsEnable]", Float) = 1
		[Toggle(_USECAUSTICRAINBOW_ON)] _UseCausticRainbow("Use Rainbow [_CausticsEnable]", Float) = 1
		[HDR]_CausticsTint("Caustics Tint [_CausticsEnable]", Color) = (1,1,1,1)
		[Feature(_CAUSTICSENABLE)]HeaderOceanFog("# Ocean Fog Caustics", Float) = 0
		[Toggle(_USEUNITYFOG_ON)] _UseUnityFog("Use Unity Fog", Float) = 0
		[Toggle(_USEGRADIENTFOG_ON)] _UseGradientFog("UseGradientFog", Float) = 1
		_HeaderOther("# Other", Float) = 0
		_HeaderFoam("# Foam", Float) = 0
		[Toggle(_USEFOAM_ON)] _UseFoam("Use Foam", Float) = 1
		[NoScaleOffset]_FoamTexture("Foam Texture & [_UseFoam]", 2D) = "white" {}
		_FoamScale_UseFoam("Foam Scale [_UseFoam]", Float) = 1
		_RemapFoam("Remap Foam (Shore A, Shore B, Sea A, Sea Bmm) & [_UseFoam]", Vector) = (0.4,0.3,-1,0.3)
		_FoamMaxDistance(" Foam Max Distance [_UseFoam]", Float) = 10
		[HDR]_FoamColor("Foam Color [_UseFoam]", Color) = (1,1,1,0)
		[Toggle(_USEFOAMEXTRASAMPLER_ON)] _UseFoamExtraSampler("Use Foam Extra Sampler [_UseFoam]", Float) = 1
		_FoamPow("FoamPow [_UseFoam]", Range( 1 , 5)) = 3
		[HDR]_FoamLUT("Foam LUT", 2D) = "white" {}
		_DRAWERGradient_FoamLUT("!DRAWER Gradient _FoamLUT", Float) = 0
		_HeaderWaves("# Waves", Float) = 0
		_WaveSpeed("Wave Speed", Range( -1 , 1)) = 1
		_WaveScale("Wave Scale", Float) = 1
		_Motion2("Motion 2", Vector) = (0,0,0,0)
		_Motion("Motion", Vector) = (0,0,0,0)
		[Toggle(_USEWAVENOISEDISPLACE_ON)] _UseWaveNoiseDisplace("Use Wave Noise Displace", Float) = 1
		_FoamNoiseScale("-Noise Scale [_UseWaveNoiseDisplace]", Range( 0 , 5)) = 1
		_FoamNoiseScale2("-Noise Scale 2 [_UseWaveNoiseDisplace]", Range( 0 , 5)) = 1
		_FoamNoiseSpeed("-Noise Speed [_UseWaveNoiseDisplace]", Range( 0 , 5)) = 1
		_FoamNoiseCon("-Noise Con [_UseWaveNoiseDisplace]", Range( 0 , 3)) = 1
		_FoamNoiseCon2("-Noise Con 2 [_UseWaveNoiseDisplace]", Range( 0 , 3)) = 1
		_WaveNoiseDisplace("-Wave Noise Displace & [_UseWaveNoiseDisplace]", 2D) = "white" {}
		[Toggle(_USEWAVEDIRECTION_ON)] _UseWaveDirection("Use Wave Direction", Float) = 1
		_WaveDirectionSettings("-Wave Direction Settings [_UseWaveDirection]", Vector) = (0,0,1,1)
		_HeaderTesselation("# Tesselation", Float) = 0
		[Toggle(_USEVERTDISPLACEMENT_ON)] _UseVertDisplacement("Use Vert Displacement", Float) = 0
		_VertDispMult("-Vert Disp Mult [_UseVertDisplacement]", Range( 0 , 55)) = 1
		_RemapVertShore("Remap Vert Shore (Shore A, Shore B) & [_UseVertDisplacement]", Vector) = (0,1,0,0)
		[Toggle(ASE_TESSELLATION_ON)] ASE_TESSELLATION("Use Tesselation", Float) = 1
		_TessNum("-Tess Num [ASE_TESSELLATION]", Range( 1 , 320)) = 3
		_TesMin("-Tes Min [ASE_TESSELLATION]", Float) = 1
		_TesMax("-Tes Max [ASE_TESSELLATION]", Float) = 6
		_HeaderWaterDepth("# Water Depth", Float) = 0
		_WaterSaturation("Water Saturation", Range( 0 , 1)) = 0.9
		_WaterAlbedo("Water Albedo", Color) = (0.0627451,0.07843138,0.07843138,0)
		_WaterTint("Water Tint", Color) = (1,1,1,0)
		[HDR]_DepthColorLUT("DepthColor  LUT", 2D) = "white" {}
		_WaterDepthPow1("-Water Depth Pow Mobile", Range( 0 , 10)) = 6
		_WaterDepthPow("-Water Depth Pow", Range( 0 , 10)) = 0
		_DRAWERGradient_DepthColorLUT("!DRAWER Gradient _DepthColorLUT", Float) = 0
		_RemapWaterDistance("-Remap Water Distance", Range( 0 , 20)) = 2
		_RemapWaterDistance1("-Remap Water Distance Mobile", Range( 0 , 20)) = 2
		_HeaderReflections("# Reflections", Float) = 0
		_RefFresnel("Reflection Fresnel", Vector) = (0,1,5,0)
		_Cubemap("Cubemap (HDR) &", CUBE) = "black" {}
		_CubemapExposure("-Exposure", Range( 0 , 10)) = 1
		_HeaderEdge("# Edge And Fade", Float) = 0
		_RemapAlpha("Remap Alpha (Shore A, Shore B, Spec A, Spec B) &", Vector) = (0,0.18,0,0.18)
		_DistanceFadeA("Distance Fade A", Float) = 2000
		_DistanceFadeB("Distance Fade B", Float) = 600
		[Toggle(_USEINFINITYEDGE_ON)] _UseInfinityEdge("Use Infinity Edge", Float) = 0
		[Toggle(_USEINFINITYFEATHER_ON)] _UseInfinityFeather("Use Infinity Feather", Float) = 1
		_Tolerance("-Tolerance [_UseInfinityFeather]", Range( 0 , 1)) = 0.04003376
		_Feather("-Feather [_UseInfinityFeather]", Range( 0 , 1)) = 0.1978261
		_HeaderTransparency("# Transparency", Float) = 0
		_RefractionWarp("Refraction Warp", Range( 0 , 1)) = 0.35
		_WaveRefractionStrength("Wave Refraction Strength", Range( 0 , 1)) = 0.5
		[Toggle(_USEDITHERING_ON)] _UseDithering("Use Dithering", Float) = 1
		_ClipThreshold("ClipThreshold [!_USEDITHERING_ON]", Range( 0 , 1)) = 0.1
		[Toggle(_TRANSPARENTGRABPASS_ON)] _TransparentGrabPass("Use Transparent GrabPass", Float) = 1
		_WaterRefractonMobile("Water Refracton Mobile", Range( 0 , 1)) = 0.2
		[Toggle(_RAINBOWGRABPASS_ON)] _RainbowGrabPass("-Use Rainbow GrabPass [_TransparentGrabPass]", Float) = 1
		_HeaderBakedLughting("# Baked Lighting", Float) = 0
		_HeaderMisc("# Misc", Float) = 0
		[Toggle]_ZWriteToggle("ZWriteToggle", Range( 0 , 1)) = 1
		[MaterialEnumDrawerExtended(Less,0,Greater,1,Less of Equal,2,Greater of Equal,3,Equal,4,Not Equal,5,Always,6)]_ZTestMode("ZTest Mode", Int) = 2
		[HideInInspector]_DepthPosition("_DepthPosition", Vector) = (0,0,0,0)


		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 16
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

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Transparent-1" "UniversalMaterialType"="Lit" }

		Cull Off
		ZWrite On
		ZTest LEqual
		Offset 0,0
		AlphaToMask Off

		Stencil
		{
			Ref 255
			CompFront NotEqual
			CompBack Always
		}

		HLSLINCLUDE
		#pragma target 5.0
		#pragma prefer_hlslcc gles
		#pragma exclude_renderers xboxone xboxseries playstation ps4 switch // ensure rendering platforms toggle list is visible

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

			Blend One Zero, One Zero
			//BlendOp [_BlendOp]// Shadowood
			ZWrite On
			ZTest LEqual
			Offset 0,0
			ColorMask RGBA

			

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_TESSELLATION 1
			#pragma require tessellation tessHW
			#pragma hull HullFunction
			#pragma domain DomainFunction
			#define ASE_DISTANCE_TESSELLATION
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_TONEMAPPING 1
			#define _ASE_TINT 1
			#define _ASE_DEBUGVISUALS 1
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMAL_DROPOFF_TS 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define ASE_SRP_VERSION 120115
			#define REQUIRE_DEPTH_TEXTURE 1
			#define REQUIRE_OPAQUE_TEXTURE 1
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
				half3 normalWS, half3 viewDirectionWS, half fresnelControl, half3 anisoReflectionNormal)
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
				/* // Rotate reflection, instead pass the float2x2 matrix from CPU side via shader global
				float degrees = 9;
				float alpha = degrees * 3.142 / 180.0;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);
				float3 reflectVectorNew = float3(mul(m, reflectVector.xz), reflectVector.y).xzy;
				*/
				half3 indirectSpecular = GlossyEnvironmentReflection(reflectVectorNew, positionWS, brdfData.perceptualRoughness, 1.0h);

				half3 color = EnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);
			
				if (IsOnlyAOLightingFeatureEnabled())
				{
					color = half3(1,1,1); // "Base white" for AO debug lighting mode
				}
			
			#if defined(_CLEARCOAT) || defined(_CLEARCOATMAP)
				half3 coatIndirectSpecular = GlossyEnvironmentReflection(reflectVector, positionWS, brdfDataClearCoat.perceptualRoughness, 1.0h);
				// TODO: "grazing term" causes problems on full roughness
				half3 coatColor = EnvironmentBRDFClearCoat(brdfDataClearCoat, clearCoatMask, coatIndirectSpecular, fresnelTerm);
			
				// Blend with base layer using khronos glTF recommended way using NoV
				// Smooth surface & "ambiguous" lighting
				// NOTE: fresnelTerm (above) is pow4 instead of pow5, but should be ok as blend weight.
				half coatFresnel = kDielectricSpec.x + kDielectricSpec.a * fresnelTerm;
				return (color * (1.0 - coatFresnel * clearCoatMask) + coatColor) * occlusion;
			#else
				return color * occlusion;
			#endif
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
			half4 UniversalFragmentPBRCustom(InputData inputData, SurfaceData surfaceData, half fresnelControl, half4 sssColor, half4 sssControl, float3 worldNormal, float3 worldTangent, float4 AnisotropicControl)
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
				half4 shadowMask = CalculateShadowMask(inputData);
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
				#if defined(_ASE_FRESNELCONTROL) || (defined(_ASE_ANIS) && defined(_USE_ANIS_ON))
					lightingData.giColor = GlobalIlluminationCustom(brdfData, brdfDataClearCoat, surfaceData.clearCoatMask, inputData.bakedGI, aoFactor.indirectAmbientOcclusion, inputData.positionWS,	inputData.normalWS, inputData.viewDirectionWS, fresnelControl, addData.anisoReflectionNormal);
				#else
					lightingData.giColor = GlobalIllumination(brdfData, brdfDataClearCoat, surfaceData.clearCoatMask, inputData.bakedGI, aoFactor.indirectAmbientOcclusion, inputData.positionWS, inputData.normalWS, inputData.viewDirectionWS);
				#endif

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

			#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_BITANGENT
			#pragma shader_feature_local ASE_TESSELLATION_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local LIGHTMAP_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USEINFINITYEDGE_ON
			#pragma shader_feature_local _USEVERTDISPLACEMENT_ON
			#pragma shader_feature_local _USEWAVENOISEDISPLACE_ON
			#pragma shader_feature_local _SPACE_WORLDSPACE _SPACE_UVSPACE
			#pragma shader_feature_local _HORIZONMAPPED_ON
			#pragma shader_feature_local _USEWAVEDIRECTION_ON
			#pragma shader_feature_local _USEINFINITYFEATHER_ON
			#pragma shader_feature_local _USEFOAM_ON
			#pragma shader_feature_local _USEFOAMEXTRASAMPLER_ON
			#pragma shader_feature_local _TRANSPARENTGRABPASS_ON
			#pragma shader_feature_local _USEEXTRANORMALS_ON
			#pragma multi_compile __ GlobalPlanarReflection
			#pragma shader_feature_local _USEGRADIENTFOG_ON
			#pragma shader_feature_local _RAINBOWGRABPASS_ON
			#pragma shader_feature_local _CAUSTICSENABLE_ON
			#pragma shader_feature_local _USECAUSTICEXTRASAMPLER_ON
			#pragma shader_feature_local _USECAUSTICRAINBOW_ON
			#pragma shader_feature_local _USEDITHERING_ON
			#pragma shader_feature_local _USEUNITYFOG_ON


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
			float4 _FoamColor;
			float4 _Motion;
			float4 _Normals_ST;
			float4 _WaterTint;
			float4 _RemapVertShore;
			float4 _WaterAlbedo;
			float4 _WaveDirectionSettings;
			float4 _Normals2_ST;
			float4 _Motion2;
			float4 _RefFresnel;
			float4 _CausticsTint;
			float4x4 _DepthMatrix;
			float4 _RemapAlpha;
			float4 _BakeLightMapAmbient;
			float4x4 _BakeMatrix;
			float4 _bakeLightmapST;
			float4 _RemapFoam;
			float3 _WorldDir;
			float3 _WorldPos;
			float3 _DepthPosition;
			float3 _CameraForward;
			float _WaveRefractionStrength;
			float _RefractionWarp;
			float _RemapWaterDistance;
			float _RemapWaterDistance1;
			float _Smoothness;
			float _WaterDepthPow;
			float _WaterSaturation;
			float _CubemapExposure;
			float _NormalDarken1;
			float _NormalDarken;
			float _ForceEye;
			float _ClipThreshold;
			float _WaterDepthPow1;
			int _ZTestMode;
			float _NormalPow2;
			float _FoamMaxDistance;
			float _ZWriteToggle;
			float _TessNum;
			float _TesMin;
			float _TesMax;
			float _WaveScale;
			float _FoamNoiseSpeed;
			float _FoamNoiseScale;
			float _FoamNoiseCon;
			float _FoamNoiseScale2;
			float _FoamNoiseCon2;
			float _DistanceFadeA;
			float _WaveSpeed;
			float _DistanceFadeB;
			float _VertDispMult;
			float _Tolerance;
			float _Feather;
			float _FoamScale_UseFoam;
			float _NormalPow;
			float _BakedLightingFeather;
			float _RefDistortion;
			float _WaterRefractonMobile;
			float _FoamPow;
			int _DebugVisual;
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

			float4 _skyGradientColor1;
			float4 _skyGradientColor2;
			float3 _CausticsDir;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_FoamLUT);
			TEXTURE2D(_DepthCache);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE2D(_WaveNoiseDisplace);
			SAMPLER(sampler_WaveNoiseDisplace);
			TEXTURE2D(_FoamTexture);
			SAMPLER(sampler_FoamTexture);
			TEXTURE2D(_ColorCache);
			TEXTURE2D(_Normals);
			SAMPLER(sampler_Normals);
			TEXTURE2D(_Normals2);
			SAMPLER(sampler_Normals2);
			TEXTURE2D(_DepthColorLUT);
			TEXTURECUBE(_Cubemap);
			float GlobalSkyRotation;
			SAMPLER(sampler_Cubemap);
			TEXTURE2D(_TexLeft);
			SAMPLER(sampler_Linear_Mirror);
			TEXTURE2D(_TexRight);
			float4x4 _CausticMatrix;
			float4 _CausticsSettings;
			TEXTURE2D(_bakeLightmap);
			SAMPLER(sampler_bakeLightmap);
			TEXTURE2D(_bakeLightmapSM);


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
			
			float4 Billboard2078( float3 worldPosIN, float2 screenPos, float3 QuadNormal, float3 QuadPosition )
			{
				float3 worldPos = worldPosIN;
				float3 viewDir = worldPos - _WorldSpaceCameraPos;
				float3 reflectDir = -viewDir;
				float3 nReflDirection = normalize(reflectDir);
				float4 result = float4(0,0,0,0);
				// Ray-quad intersection
				half planeIntersectDistance = dot( worldPos - QuadPosition,  QuadNormal.xyz ) / dot( nReflDirection, QuadNormal.xyz );
				half3 intersectPosition = worldPos - nReflDirection * planeIntersectDistance;
				//half4 localPlaneIntersectPosition = mul( QuadInverseMatrix, float4( intersectPosition, 1.0 ));
				half4 localPlaneIntersectPosition = float4( intersectPosition, 1.0 );
				half2 billboardUV = half2( localPlaneIntersectPosition.x, localPlaneIntersectPosition.z );
				//billboardUV*=1;
				//float4 billboardCol = BillboardTex.Sample( BillboardSampler , billboardUV );
				float4 billboardCol = float4( billboardUV.x, billboardUV.y, 0, 1);
				float3 quadNorm = QuadNormal.xyz;
				half reflectDot = dot( nReflDirection, quadNorm );
				float w = 1;
				// Cull when beyond the quad bounds
				//w = billboardUV.x > 1.0 ? 0.0 : w;
				//w = billboardUV.y > 1.0 ? 0.0 : w;
				//w = billboardUV.x < 0.0 ? 0.0 : w;
				//w = billboardUV.y < 0.0 ? 0.0 : w;
				// Cull if distance is behind the surface
				//w = planeIntersectDistance < 0 ? 0 : w;
				// Cull if quad facing backwards
				w = reflectDot <= 0.0 ? 0.0 : w; 
				// Cull if quad facing backwards and culling is ticked on the quad ( passed in via the arrays W )
				//w = QuadNormal.w == 1.0 ? (reflectDot <= 0.0 ? 0.0 : w) : w;
				billboardCol = billboardCol * billboardCol.w * w;
				billboardCol.a = w;
				result*=1-w;
				result+= billboardCol;
				return result;
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			int IsStereoEyeLeft11_g3216( float3 WorldCamPos, float3 WorldCamRight, float force )
			{
				int Out = 0;	
				if(force >= 0){
						return force == 0;
					}
				#if defined(USING_STEREO_MATRICES)
					// Unity 5.4 has this new variable
					return (unity_StereoEyeIndex == 0);
				#elif defined (UNITY_DECLARE_MULTIVIEW)
					// OVR_multiview extension
					return (UNITY_VIEWID == 0);
				#else
					// NOTE: Bug #1165: _WorldSpaceCameraPos is not correct in multipass VR (when skybox is used) but UNITY_MATRIX_I_V seems to be
					#if defined(UNITY_MATRIX_I_V)
						float3 renderCameraPos = UNITY_MATRIX_I_V._m03_m13_m23;
					#else
						float3 renderCameraPos = _WorldSpaceCameraPos.xyz;
					#endif
					float fL = distance(WorldCamPos - WorldCamRight, renderCameraPos);
					float fR = distance(WorldCamPos + WorldCamRight, renderCameraPos);
					return (fL < fR);
				#endif
				return Out;
			}
			
			inline float3 ApplyTonemapper( float4 settings, float3 color )
			{
				return ApplyTonemap( color, settings );;
			}
			
			float3 MatrixMulThatWorks( float4 Pos, float4x4 Mat )
			{
				float3 result = mul(Mat,Pos.xyz);
				return result + float3(Mat[0][3],Mat[1][3],Mat[2][3]);
			}
			
			float CameraDepthTexture55_g3383( float2 uv )
			{
				float4 color = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, uv);
				return color.r;
			}
			
			float MyCustomExpression44_g3383( float rawDepth )
			{
				return LinearEyeDepth(rawDepth, _ZBufferParams);
			}
			
			float3 MyCustomExpression16_g3383( float3 view, float sceneZ )
			{
				return  _WorldSpaceCameraPos - view * sceneZ / dot(UNITY_MATRIX_I_V._13_23_33, view);
			}
			
			float4x4 Inverse4x4(float4x4 input)
			{
				#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
				float4x4 cofactors = float4x4(
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
			
			inline float Dither8x8Bayer( int x, int y )
			{
				const float dither[ 64 ] = {
			 1, 49, 13, 61,  4, 52, 16, 64,
			33, 17, 45, 29, 36, 20, 48, 32,
			 9, 57,  5, 53, 12, 60,  8, 56,
			41, 25, 37, 21, 44, 28, 40, 24,
			 3, 51, 15, 63,  2, 50, 14, 62,
			35, 19, 47, 31, 34, 18, 46, 30,
			11, 59,  7, 55, 10, 58,  6, 54,
			43, 27, 39, 23, 42, 26, 38, 22};
				int r = y * 8 + x;
				return dither[r] / 64; // same # of instructions as pre-dividing due to compiler magic
			}
			
			float4 URPDecodeInstruction19_g3432(  )
			{
				return float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0, 0);
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
			
			float4 URPDecodeInstruction19_g3433(  )
			{
				return float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0, 0);
			}
			
			float MyCustomExpression191_g3347( float In )
			{
				return ComputeFogFactor(In);
			}
			
			inline float4 GetUnderWaterFogs240_g3357( float3 viewDir, float3 camWorldPos, float3 posWS, float4 oceanFogDensities, float oceanHeight, float4 oceanFogTop_RGB_Exponent, float4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float2 texCoord2052 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_2054_0 = distance( texCoord2052 , float2( 0.5,0.5 ) );
				float ifLocalVar2053 = 0;
				if( temp_output_2054_0 <= 1.0 )
				ifLocalVar2053 = 1.0;
				else
				ifLocalVar2053 = 5000.0;
				#ifdef _USEINFINITYEDGE_ON
				float4 staticSwitch1498 = ( v.vertex * ifLocalVar2053 );
				#else
				float4 staticSwitch1498 = v.vertex;
				#endif
				float4 break1605 = staticSwitch1498;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 break19 = mul( _DepthMatrix, float4( ( ase_worldPos - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D_LOD( _DepthCache, sampler_Linear_Clamp, CacheUV1109, 0.0 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_2 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_3 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_2) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_3 - temp_cast_2)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 temp_cast_4 = (_FoamNoiseSpeed).xx;
				float3 break35 = ase_worldPos;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = ase_worldPos;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner138 = ( 1.0 * _Time.y * temp_cast_4 + ( WorldXZ1087 * _FoamNoiseScale ));
				float lerpResult133 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( 0.04 * panner138 ), 0.0 ).r , _FoamNoiseCon);
				float2 temp_cast_6 = (_FoamNoiseSpeed).xx;
				float2 panner168 = ( 1.0 * _Time.y * temp_cast_6 + ( WorldXZ1087 * _FoamNoiseScale2 ));
				float lerpResult161 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( panner168 * 0.04 ), 0.0 ).r , _FoamNoiseCon2);
				float UnderOffset1634 = GlobalOceanOffset;
				float3 appendResult2127 = (float3(break1605.x , ( break1605.y + UnderOffset1634 ) , break1605.z));
				float3 VertResOffset2125 = appendResult2127;
				float3 customSurfaceDepth1169 = VertResOffset2125;
				float customEye1169 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1169)).z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				#ifdef _USEWAVENOISEDISPLACE_ON
				float staticSwitch680 = ( Foamness294 + ( ( lerpResult133 + lerpResult161 ) * ( ( 1.0 - DistanceFadeA1294 ) * 0.4 ) ) );
				#else
				float staticSwitch680 = Foamness294;
				#endif
				float DepthCacheRemap109 = staticSwitch680;
				float temp_output_5_0_g3212 = ( 1.0 - DepthCacheRemap109 );
				float mulTime209 = _TimeParameters.x * ( _WaveSpeed * 0.1 );
				float2 appendResult868 = (float2(_WaveDirectionSettings.x , _WaveDirectionSettings.y));
				float WaveCenterDistance1051 = distance( WorldXZ1087 , appendResult868 );
				#ifdef _USEWAVEDIRECTION_ON
				float staticSwitch762 = ( mulTime209 + ( ( WaveCenterDistance1051 * -0.01 ) / _WaveDirectionSettings.z ) );
				#else
				float staticSwitch762 = ( 0.0 - mulTime209 );
				#endif
				float temp_output_17_0_g3212 = staticSwitch762;
				float temp_output_30_0_g3212 = ( 1.0 - saturate( 0.7 ) );
				float temp_output_35_0_g3212 = ( temp_output_30_0_g3212 * 0.5 );
				float temp_output_32_0_g3212 = ( (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) - temp_output_35_0_g3212 );
				float temp_output_31_0_g3212 = ( temp_output_35_0_g3212 + (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) );
				float DepthCacheWave184 = ( 1.0 - frac( (0.0 + (temp_output_5_0_g3212 - temp_output_32_0_g3212) * (1.0 - 0.0) / (temp_output_31_0_g3212 - temp_output_32_0_g3212)) ) );
				float clampResult124 = clamp( DepthCacheWave184 , 0.01 , 0.99 );
				float2 temp_cast_7 = (clampResult124).xx;
				float4 tex2DNode76 = SAMPLE_TEXTURE2D_LOD( _FoamLUT, sampler_Linear_Clamp, temp_cast_7, 0.0 );
				float smoothstepResult279 = smoothstep( 0.0 , 1.0 , tex2DNode76.a);
				float3 customSurfaceDepth1325 = VertResOffset2125;
				float customEye1325 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1325)).z;
				float cameraDepthFade1325 = (( customEye1325 -_ProjectionParams.y - 0.0 ) / _DistanceFadeB);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2111 = 0.0;
				#else
				float staticSwitch2111 = saturate( cameraDepthFade1325 );
				#endif
				float DistanceFadeB1326 = staticSwitch2111;
				float lerpResult1311 = lerp( smoothstepResult279 , 0.0 , DistanceFadeB1326);
				float FoamVertDisp261 = saturate( lerpResult1311 );
				float temp_output_240_0 = ( FoamVertDisp261 * _VertDispMult * 0.1 );
				float ShoreMaskVert586 = saturate( (0.0 + (( CaptureB_Distancex100200 * 100.0 ) - _RemapVertShore.x) * (1.0 - 0.0) / (_RemapVertShore.y - _RemapVertShore.x)) );
				float VertDispDistMask22014 = ( 1.0 - saturate( (0.0 + (distance( WorldXZ1087 , (_WorldSpaceCameraPos).xz ) - 5.0) * (1.0 - 0.0) / (7.9 - 5.0)) ) );
				float3 customSurfaceDepth1484 = VertResOffset2125;
				float customEye1484 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1484)).z;
				float cameraDepthFade1484 = (( customEye1484 -_ProjectionParams.y - 0.0 ) / 1.0);
				float CamDepthFade1495 = cameraDepthFade1484;
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float VertDispMaskRes1691 = ( ShoreMaskVert586 * saturate( VertDispDistMask22014 ) * saturate( (0.0 + (CamDepthFade1495 - 0.2) * (1.0 - 0.0) / (1.5 - 0.2)) ) * InfinityFeather1410 );
				float temp_output_238_0 = ( temp_output_240_0 * VertDispMaskRes1691 );
				#ifdef _USEVERTDISPLACEMENT_ON
				float staticSwitch1688 = temp_output_238_0;
				#else
				float staticSwitch1688 = 0.0;
				#endif
				float VertAdded1524 = staticSwitch1688;
				float3 appendResult1606 = (float3(break1605.x , ( break1605.y + VertAdded1524 + UnderOffset1634 ) , break1605.z));
				float3 VertResDisplaced328 = appendResult1606;
				
				float3 vertexPos1850 = VertResDisplaced328;
				float4 ase_clipPos1850 = TransformObjectToHClip((vertexPos1850).xyz);
				float4 screenPos1850 = ComputeScreenPos(ase_clipPos1850);
				o.ase_texcoord9 = screenPos1850;
				o.ase_texcoord8.z = customEye1169;
				
				o.ase_texcoord8.w = customEye1325;
				float3 customSurfaceDepth2817 = VertResOffset2125;
				float customEye2817 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth2817)).z;
				o.ase_texcoord10.x = customEye2817;
				o.ase_texcoord10.y = customEye1484;
				float3 customSurfaceDepth2418 = VertResDisplaced328;
				float customEye2418 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth2418)).z;
				o.ase_texcoord10.z = customEye2418;
				float3 customSurfaceDepth2812 = VertResOffset2125;
				float customEye2812 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth2812)).z;
				o.ase_texcoord10.w = customEye2812;
				float3 _Vector3 = float3(0,0,-1);
				float4 Pos6_g3381 = float4( _Vector3 , 0.0 );
				float4x4 Mat6_g3381 = _CausticMatrix;
				float3 localMatrixMulThatWorks6_g3381 = MatrixMulThatWorks( Pos6_g3381 , Mat6_g3381 );
				float3 normalizeResult147_g3379 = normalize( localMatrixMulThatWorks6_g3381 );
				float3 vertexToFrag144_g3379 = normalizeResult147_g3379;
				o.ase_texcoord11.xyz = vertexToFrag144_g3379;
				float3 objToWorld3_g3383 = mul( GetObjectToWorldMatrix(), float4( v.vertex.xyz, 1 ) ).xyz;
				float3 vertexToFrag7_g3383 = objToWorld3_g3383;
				o.ase_texcoord12.xyz = vertexToFrag7_g3383;
				
				float4 Pos6_g3428 = float4( ase_worldPos , 0.0 );
				float4x4 Mat6_g3428 = _BakeMatrix;
				float3 localMatrixMulThatWorks6_g3428 = MatrixMulThatWorks( Pos6_g3428 , Mat6_g3428 );
				float3 break4_g3427 = localMatrixMulThatWorks6_g3428;
				float2 appendResult3_g3427 = (float2(break4_g3427.x , break4_g3427.z));
				float2 vertexToFrag7_g3427 = ( float2( 0.5,0.5 ) + appendResult3_g3427 );
				float2 LightingUV11_g3427 = vertexToFrag7_g3427;
				float4 temp_output_21_0_g3432 = _bakeLightmapST;
				float2 vertexToFrag10_g3432 = ( ( LightingUV11_g3427 * (temp_output_21_0_g3432).xy ) + (temp_output_21_0_g3432).zw );
				o.ase_texcoord13.xy = vertexToFrag10_g3432;
				
				float4 temp_output_21_0_g3433 = _bakeLightmapST;
				float2 vertexToFrag10_g3433 = ( ( LightingUV11_g3427 * (temp_output_21_0_g3433).xy ) + (temp_output_21_0_g3433).zw );
				o.ase_texcoord13.zw = vertexToFrag10_g3433;
				
				o.ase_texcoord8.xy = v.texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord11.w = 0;
				o.ase_texcoord12.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertResDisplaced328;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = float3(0,1,0);

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
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessNum; float tessMin = _TesMin; float tessMax = _TesMax;
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
						, bool ase_vface : SV_IsFrontFace ) : SV_Target
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

				float3 break19 = mul( _DepthMatrix, float4( ( WorldPosition - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = IN.ase_texcoord8.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_2 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_3 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_2) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_3 - temp_cast_2)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float4 tex2DNode13 = SAMPLE_TEXTURE2D( _DepthCache, sampler_Linear_Clamp, CacheUV1109 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_4 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_5 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_4) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_5 - temp_cast_4)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_6 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_7 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_6) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_7 - temp_cast_6)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float CamNear1615 = saturate( (0.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - 0.1) * (1.0 - 0.0) / (0.6 - 0.1)) );
				float4 screenPos1850 = IN.ase_texcoord9;
				float4 ase_screenPosNorm1850 = screenPos1850 / screenPos1850.w;
				ase_screenPosNorm1850.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm1850.z : ase_screenPosNorm1850.z * 0.5 + 0.5;
				float screenDepth1850 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm1850.xy ),_ZBufferParams);
				float distanceDepth1850 = saturate( abs( ( screenDepth1850 - LinearEyeDepth( ase_screenPosNorm1850.z,_ZBufferParams ) ) / ( 0.2 ) ) );
				float DepthFader1847 = distanceDepth1850;
				float2 appendResult87 = (float2(_Motion.x , _Motion.y));
				float2 MotionXY1084 = appendResult87;
				float3 break35 = WorldPosition;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = WorldPosition;
				float4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner527 = ( -0.03 * _Time.y * MotionXY1084 + WorldXZ1087);
				float4 tex2DNode91 = SAMPLE_TEXTURE2D( _FoamTexture, sampler_FoamTexture, (panner527*( _FoamScale_UseFoam * 0.2 ) + 0.0) );
				float2 panner74 = ( 0.05 * _Time.y * MotionXY1084 + WorldXZ1087);
				#ifdef _USEFOAMEXTRASAMPLER_ON
				float staticSwitch1679 = ( SAMPLE_TEXTURE2D( _FoamTexture, sampler_FoamTexture, (panner74*_FoamScale_UseFoam + 0.0) ).r + tex2DNode91.r );
				#else
				float staticSwitch1679 = ( tex2DNode91.r * 1.8 );
				#endif
				float4 break984 = _RemapFoam;
				float temp_output_172_0 = saturate( (0.0 + (( 100.0 * CaptureB_Distancex100200 ) - break984.x) * (1.0 - 0.0) / (break984.y - break984.x)) );
				float2 appendResult892 = (float2(_Motion2.x , _Motion2.y));
				float2 Motion2XY1096 = appendResult892;
				float2 panner890 = ( 0.05 * _Time.y * Motion2XY1096 + WorldXZ1087);
				#ifdef GlobalPlanarReflection
				float staticSwitch1591 = _NormalPow;
				#else
				float staticSwitch1591 = ( _NormalPow * 0.7 );
				#endif
				float temp_output_1436_0 = ( _NormalPow * 6.0 );
				float switchResult1435 = (((ase_vface>0)?(staticSwitch1591):(temp_output_1436_0)));
				float3 unpack11 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals, sampler_Normals, (panner890*_Normals_ST.xy + _Normals_ST.zw) ), switchResult1435 );
				unpack11.z = lerp( 1, unpack11.z, saturate(switchResult1435) );
				float3 tex2DNode11 = unpack11;
				float2 appendResult88 = (float2(_Motion.z , _Motion.w));
				float2 MotionZW1085 = appendResult88;
				float2 panner85 = ( 0.1 * _Time.y * MotionZW1085 + WorldXZ1087);
				float switchResult1432 = (((ase_vface>0)?(( _NormalPow2 * 2.0 )):(( _NormalPow2 * 12.0 ))));
				float3 unpack84 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner85*_Normals2_ST.xy + _Normals2_ST.zw) ), switchResult1432 );
				unpack84.z = lerp( 1, unpack84.z, saturate(switchResult1432) );
				float2 panner1722 = ( -0.05 * _Time.y * MotionZW1085 + WorldXZ1087);
				float3 unpack1718 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner1722*( _Normals2_ST.xy * float2( 0.5,0.5 ) ) + _Normals2_ST.zw) ), switchResult1432 );
				unpack1718.z = lerp( 1, unpack1718.z, saturate(switchResult1432) );
				float3 temp_output_1719_0 = BlendNormal( unpack84 , unpack1718 );
				float3 temp_output_83_0 = BlendNormal( tex2DNode11 , temp_output_1719_0 );
				#ifdef _USEEXTRANORMALS_ON
				float3 staticSwitch1683 = temp_output_83_0;
				#else
				float3 staticSwitch1683 = temp_output_83_0;
				#endif
				float2 appendResult1049 = (float2(staticSwitch1683.xy));
				float2 RefNorm886 = appendResult1049;
				float RefBumpSlider1337 = _RefDistortion;
				float3 normalizeResult2273 = normalize( ( staticSwitch1683 + float3( ( RefNorm886 * RefBumpSlider1337 ) ,  0.0 ) ) );
				float2 appendResult2519 = (float2(normalizeResult2273.xy));
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 lerpResult2274 = lerp( float2( 0,0 ) , appendResult2519 , ( _WaterRefractonMobile * 0.5 * saturate( Foamness294 ) ));
				float customEye1169 = IN.ase_texcoord8.z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				float temp_output_1414_0 = ( ( 1.0 - InfinityFeather1410 ) * saturate( DistanceFadeA1294 ) );
				float2 lerpResult2275 = lerp( lerpResult2274 , float2( 0,0 ) , temp_output_1414_0);
				#ifdef _TRANSPARENTGRABPASS_ON
				float2 staticSwitch2542 = float2( 0,0 );
				#else
				float2 staticSwitch2542 = lerpResult2275;
				#endif
				float2 RefHackeryForColorCache2269 = staticSwitch2542;
				float2 temp_output_2198_0 = ( RefHackeryForColorCache2269 + CacheUV1109 );
				float4 tex2DNode657 = SAMPLE_TEXTURE2D( _ColorCache, sampler_Linear_Clamp, temp_output_2198_0 );
				float ColorCacheA2171 = tex2DNode657.a;
				float temp_output_12_0_g3214 = InfinityFeather1410;
				float CaptureDepthAOffsetShore1177 = ( ( ColorCacheA2171 * temp_output_12_0_g3214 ) + ( 1.0 - temp_output_12_0_g3214 ) );
				float4 break993 = _RemapAlpha;
				float switchResult2294 = (((ase_vface>0)?(saturate( (0.0 + (CaptureDepthAOffsetShore1177 - break993.x) * (1.0 - 0.0) / (break993.y - break993.x)) )):(1.0)));
				float ShoreMaskDepthAlpha141 = switchResult2294;
				float temp_output_199_0 = saturate( (0.0 + (( 5.0 * CaptureB_Distancex100200 ) - break984.z) * (1.0 - 0.0) / (( break984.w * _FoamMaxDistance ) - break984.z)) );
				float temp_output_981_0 = ( 1.0 - temp_output_199_0 );
				float temp_output_982_0 = ( temp_output_172_0 * ShoreMaskDepthAlpha141 * temp_output_981_0 );
				float FoamFeather2372 = temp_output_982_0;
				float2 temp_cast_12 = (_FoamNoiseSpeed).xx;
				float2 panner138 = ( 1.0 * _Time.y * temp_cast_12 + ( WorldXZ1087 * _FoamNoiseScale ));
				float lerpResult133 = lerp( 1.0 , SAMPLE_TEXTURE2D( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( 0.04 * panner138 ) ).r , _FoamNoiseCon);
				float2 temp_cast_13 = (_FoamNoiseSpeed).xx;
				float2 panner168 = ( 1.0 * _Time.y * temp_cast_13 + ( WorldXZ1087 * _FoamNoiseScale2 ));
				float lerpResult161 = lerp( 1.0 , SAMPLE_TEXTURE2D( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( panner168 * 0.04 ) ).r , _FoamNoiseCon2);
				#ifdef _USEWAVENOISEDISPLACE_ON
				float staticSwitch680 = ( Foamness294 + ( ( lerpResult133 + lerpResult161 ) * ( ( 1.0 - DistanceFadeA1294 ) * 0.4 ) ) );
				#else
				float staticSwitch680 = Foamness294;
				#endif
				float DepthCacheRemap109 = staticSwitch680;
				float temp_output_5_0_g3212 = ( 1.0 - DepthCacheRemap109 );
				float mulTime209 = _TimeParameters.x * ( _WaveSpeed * 0.1 );
				float2 appendResult868 = (float2(_WaveDirectionSettings.x , _WaveDirectionSettings.y));
				float WaveCenterDistance1051 = distance( WorldXZ1087 , appendResult868 );
				#ifdef _USEWAVEDIRECTION_ON
				float staticSwitch762 = ( mulTime209 + ( ( WaveCenterDistance1051 * -0.01 ) / _WaveDirectionSettings.z ) );
				#else
				float staticSwitch762 = ( 0.0 - mulTime209 );
				#endif
				float temp_output_17_0_g3212 = staticSwitch762;
				float temp_output_30_0_g3212 = ( 1.0 - saturate( 0.7 ) );
				float temp_output_35_0_g3212 = ( temp_output_30_0_g3212 * 0.5 );
				float temp_output_32_0_g3212 = ( (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) - temp_output_35_0_g3212 );
				float temp_output_31_0_g3212 = ( temp_output_35_0_g3212 + (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) );
				float DepthCacheWave184 = ( 1.0 - frac( (0.0 + (temp_output_5_0_g3212 - temp_output_32_0_g3212) * (1.0 - 0.0) / (temp_output_31_0_g3212 - temp_output_32_0_g3212)) ) );
				float clampResult124 = clamp( DepthCacheWave184 , 0.01 , 0.99 );
				float2 temp_cast_14 = (clampResult124).xx;
				float4 tex2DNode76 = SAMPLE_TEXTURE2D_LOD( _FoamLUT, sampler_Linear_Clamp, temp_cast_14, 0.0 );
				float FoamMaskResult122 = frac( ( FoamFeather2372 * tex2DNode76.r ) );
				float temp_output_142_0 = saturate( FoamMaskResult122 );
				float temp_output_39_0 = ( staticSwitch1679 * temp_output_142_0 );
				float3 temp_output_1099_0 = ( ( ( pow( temp_output_39_0 , _FoamPow ) * 0.5 ) + saturate( ( pow( temp_output_39_0 , 3.0 ) + ( ( saturate( pow( temp_output_142_0 , 4.0 ) ) - 0.5 ) * 0.3 ) ) ) ) * (_FoamColor).rgb * InfinityFeather1410 * 0.4 );
				#ifdef _USEFOAM_ON
				float3 staticSwitch1687 = temp_output_1099_0;
				#else
				float3 staticSwitch1687 = float3( 0,0,0 );
				#endif
				float3 AlbedoRes465 = staticSwitch1687;
				float3 temp_output_11_0_g3347 = AlbedoRes465;
				float temp_output_23_0_g3347 = GlobalOceanUnder;
				float3 ifLocalVar4_g3347 = 0;
				UNITY_BRANCH 
				if( temp_output_23_0_g3347 >= 1.0 )
				ifLocalVar4_g3347 = temp_output_11_0_g3347;
				else
				ifLocalVar4_g3347 = temp_output_11_0_g3347;
				#ifdef _USEGRADIENTFOG_ON
				float3 staticSwitch6_g3347 = ifLocalVar4_g3347;
				#else
				float3 staticSwitch6_g3347 = temp_output_11_0_g3347;
				#endif
				#ifdef _USEUNDERWATER
				float3 staticSwitch214_g3347 = staticSwitch6_g3347;
				#else
				float3 staticSwitch214_g3347 = temp_output_11_0_g3347;
				#endif
				float3 temp_output_3047_0 = staticSwitch214_g3347;
				float temp_output_2951_0 = distance( (WorldPosition).xz , (_WorldSpaceCameraPos).xz );
				float temp_output_2955_0 = saturate( (1.0 + (temp_output_2951_0 - 100.0) * (0.0 - 1.0) / (8000.0 - 100.0)) );
				float temp_output_2949_0 = pow( temp_output_2955_0 , 8.0 );
				float4 temp_output_2892_0 = saturate( ( float4( ( InfinityFeather1410 * CamNear1615 * DepthFader1847 * temp_output_3047_0 * ShoreMaskDepthAlpha141 ) , 0.0 ) + ( _WaterAlbedo * temp_output_2949_0 * CamNear1615 * DepthFader1847 * ShoreMaskDepthAlpha141 ) ) );
				float4 AlbedoRes22337 = temp_output_2892_0;
				
				float3 appendResult1062 = (float3(tex2DNode657.rgb));
				float3 ColorCacheRGB1061 = ( InfinityFeather1410 * appendResult1062 );
				float temp_output_2998_0 = saturate( ( ( -0.2 * min( -CaptureDepthAOffsetShore1177 , 0.0 ) ) / _RemapWaterDistance1 ) );
				float2 appendResult2445 = (float2(ase_screenPosNorm.xy));
				float2 appendResult2856 = (float2(temp_output_1719_0.xy));
				float SeaAlpha2FoamAndNormal205 = InfinityFeather1410;
				float smoothstepResult279 = smoothstep( 0.0 , 1.0 , tex2DNode76.a);
				float customEye1325 = IN.ase_texcoord8.w;
				float cameraDepthFade1325 = (( customEye1325 -_ProjectionParams.y - 0.0 ) / _DistanceFadeB);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2111 = 0.0;
				#else
				float staticSwitch2111 = saturate( cameraDepthFade1325 );
				#endif
				float DistanceFadeB1326 = staticSwitch2111;
				float lerpResult1311 = lerp( smoothstepResult279 , 0.0 , DistanceFadeB1326);
				float FoamVertDisp261 = saturate( lerpResult1311 );
				float temp_output_1332_0 = ( RefBumpSlider1337 * SeaAlpha2FoamAndNormal205 * (1.0 + (FoamVertDisp261 - 0.0) * (5.0 - 1.0) / (1.0 - 0.0)) * 22.0 );
				float3 normalizeResult2866 = normalize( ( temp_output_1719_0 + float3( ( appendResult2856 * temp_output_1332_0 ) ,  0.0 ) ) );
				float3 lerpResult2865 = lerp( normalizeResult2866 , float3( 0,0,1 ) , float3( 0.5,0.5,0.5 ));
				float switchResult1438 = (((ase_vface>0)?(FoamVertDisp261):(( FoamVertDisp261 * 0.8 ))));
				float customEye2817 = IN.ase_texcoord10.x;
				float cameraDepthFade2817 = (( customEye2817 -_ProjectionParams.y - 0.0 ) / 300.0);
				float3 appendResult2798 = (float3(0.0 , ( saturate( switchResult1438 ) * 0.1 * _WaveRefractionStrength * ( 1.0 - saturate( cameraDepthFade2817 ) ) ) , 0.0));
				float3 lerpResult2861 = lerp( ( lerpResult2865 + appendResult2798 ) , float3( 0,0,1 ) , temp_output_1414_0);
				float UnderOffset1634 = GlobalOceanOffset;
				float GlobalOceanHeight1552 = ( GlobalOceanHeight + UnderOffset1634 );
				float temp_output_240_0 = ( FoamVertDisp261 * _VertDispMult * 0.1 );
				float ShoreMaskVert586 = saturate( (0.0 + (( CaptureB_Distancex100200 * 100.0 ) - _RemapVertShore.x) * (1.0 - 0.0) / (_RemapVertShore.y - _RemapVertShore.x)) );
				float VertDispDistMask22014 = ( 1.0 - saturate( (0.0 + (distance( WorldXZ1087 , (_WorldSpaceCameraPos).xz ) - 5.0) * (1.0 - 0.0) / (7.9 - 5.0)) ) );
				float customEye1484 = IN.ase_texcoord10.y;
				float cameraDepthFade1484 = (( customEye1484 -_ProjectionParams.y - 0.0 ) / 1.0);
				float CamDepthFade1495 = cameraDepthFade1484;
				float VertDispMaskRes1691 = ( ShoreMaskVert586 * saturate( VertDispDistMask22014 ) * saturate( (0.0 + (CamDepthFade1495 - 0.2) * (1.0 - 0.0) / (1.5 - 0.2)) ) * InfinityFeather1410 );
				float temp_output_238_0 = ( temp_output_240_0 * VertDispMaskRes1691 );
				#ifdef _USEVERTDISPLACEMENT_ON
				float staticSwitch1688 = temp_output_238_0;
				#else
				float staticSwitch1688 = 0.0;
				#endif
				float VertAdded1524 = staticSwitch1688;
				float temp_output_1537_0 = ( ( GlobalOceanHeight1552 + (VertAdded1524).x ) - ( _WorldSpaceCameraPos.y - 0.2 ) );
				float temp_output_1571_0 = saturate( ( temp_output_1537_0 * 5.0 ) );
				float RefractHack1578 = temp_output_1571_0;
				float temp_output_1582_0 = (1.0 + (RefractHack1578 - 0.0) * (11.0 - 1.0) / (1.0 - 0.0));
				float3 lerpResult2862 = lerp( float3( 0,0,1 ) , lerpResult2861 , temp_output_1582_0);
				float3 switchResult2863 = (((ase_vface>0)?(lerpResult2862):(lerpResult2861)));
				float3 NormalResRefracted2803 = switchResult2863;
				float3 NormalResBelow1480 = staticSwitch1683;
				float3 switchResult1755 = (((ase_vface>0)?(NormalResRefracted2803):(NormalResBelow1480)));
				float2 appendResult3029 = (float2(switchResult1755.xy));
				float customEye2418 = IN.ase_texcoord10.z;
				float cameraDepthFade2418 = (( customEye2418 -_ProjectionParams.y - 0.0 ) / 40.0);
				float temp_output_2417_0 = ( 1.0 - saturate( cameraDepthFade2418 ) );
				float2 temp_output_2389_0 = ( appendResult2445 + ( appendResult3029 * DepthFader1847 * CamNear1615 * ShoreMaskDepthAlpha141 * temp_output_2417_0 ) );
				float eyeDepth2780 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( temp_output_2389_0, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float2 lerpResult2452 = lerp( appendResult2445 , temp_output_2389_0 , ( _RefractionWarp * step( ( ScreenPos.w - eyeDepth2780 ) , 0.0 ) ));
				float2 ScreenSpaceRefractUV2387 = lerpResult2452;
				float eyeDepth6_g3371 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV2387, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3371 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float fresnelNdotV2830 = dot( WorldNormal, WorldViewDirection );
				float fresnelNode2830 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV2830, 5.0 ) );
				float lerpResult2832 = lerp( 9.0 , 30.0 , saturate( fresnelNode2830 ));
				float DepthFadeDistthing2939 = lerpResult2832;
				float temp_output_12_0_g3371 = ( DepthFadeDistthing2939 * _RemapWaterDistance );
				float temp_output_3_0_g3371 = ( abs( ( eyeDepth6_g3371 - -worldToView21_g3371.z ) ) / temp_output_12_0_g3371 );
				float temp_output_2907_0 = saturate( temp_output_3_0_g3371 );
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch3001 = temp_output_2907_0;
				#else
				float staticSwitch3001 = temp_output_2998_0;
				#endif
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch2993 = _WaterDepthPow;
				#else
				float staticSwitch2993 = _WaterDepthPow1;
				#endif
				float temp_output_2909_0 = ( 1.0 - pow( ( 1.0 - temp_output_2907_0 ) , 6.0 ) );
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch2990 = temp_output_2909_0;
				#else
				float staticSwitch2990 = ( 1.0 - pow( ( 1.0 - temp_output_2998_0 ) , 6.0 ) );
				#endif
				float2 appendResult2183 = (float2(staticSwitch2990 , 0.0));
				float3 appendResult2142 = (float3(SAMPLE_TEXTURE2D_LOD( _DepthColorLUT, sampler_Linear_Clamp, appendResult2183, 0.0 ).rgb));
				float3 appendResult2828 = (float3(_WaterTint.rgb));
				float3 temp_output_1964_0 = ( ( saturate( pow( ( 1.0 - staticSwitch3001 ) , staticSwitch2993 ) ) * appendResult2142 ) * appendResult2828 * InfinityFeather1410 );
				float3 desaturateInitialColor2848 = temp_output_1964_0;
				float desaturateDot2848 = dot( desaturateInitialColor2848, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar2848 = lerp( desaturateInitialColor2848, desaturateDot2848.xxx, ( 1.0 - _WaterSaturation ) );
				float3 switchResult2135 = (((ase_vface>0)?(desaturateVar2848):(float3( 0,0,0 ))));
				float3 WaterDepthTint342 = switchResult2135;
				float switchResult2482 = (((ase_vface>0)?(saturate( (0.0 + (CaptureDepthAOffsetShore1177 - break993.z) * (1.0 - 0.0) / (break993.w - break993.z)) )):(1.0)));
				float ShoreMaskSpec2479 = switchResult2482;
				float3 lerpResult3187 = lerp( float3( 1,1,1 ) , WaterDepthTint342 , ShoreMaskSpec2479);
				float switchResult1429 = (((ase_vface>0)?(radians( GlobalSkyRotation )):(radians( ( 0.0 + 180.0 ) ))));
				float3 switchResult1425 = (((ase_vface>0)?(-WorldViewDirection):(WorldViewDirection)));
				float3 normalizeResult257 = normalize( ( staticSwitch1683 + float3( ( RefNorm886 * temp_output_1332_0 ) ,  0.0 ) ) );
				float3 lerpResult1261 = lerp( normalizeResult257 , float3( 0,0,1 ) , float3( 0.5,0.5,0.5 ));
				float3 lerpResult1264 = lerp( ( lerpResult1261 + appendResult2798 ) , float3( 0,0,1 ) , temp_output_1414_0);
				float3 lerpResult1581 = lerp( float3( 0,0,1 ) , lerpResult1264 , temp_output_1582_0);
				float3 switchResult2804 = (((ase_vface>0)?(lerpResult1581):(lerpResult1264)));
				float3 NormalRes466 = switchResult2804;
				float HorizonMask2300 = saturate( (1.0 + (distance( WorldPosition , _WorldSpaceCameraPos ) - 200.0) * (0.0 - 1.0) / (3000.0 - 200.0)) );
				float3 lerpResult1488 = lerp( float3( 0,0,1 ) , NormalRes466 , saturate( ( CamNear1615 * CamDepthFade1495 * HorizonMask2300 ) ));
				float3 tanToWorld0 = float3( WorldTangent.x, WorldBiTangent.x, WorldNormal.x );
				float3 tanToWorld1 = float3( WorldTangent.y, WorldBiTangent.y, WorldNormal.y );
				float3 tanToWorld2 = float3( WorldTangent.z, WorldBiTangent.z, WorldNormal.z );
				float3 tanNormal1153 = lerpResult1488;
				float3 worldNormal1153 = float3(dot(tanToWorld0,tanNormal1153), dot(tanToWorld1,tanNormal1153), dot(tanToWorld2,tanNormal1153));
				float3 temp_output_1151_0 = reflect( switchResult1425 , worldNormal1153 );
				float3 rotatedValue1157 = RotateAroundAxis( float3( 0,0,0 ), temp_output_1151_0, float3( 0,1,0 ), switchResult1429 );
				float SmoothnessRes468 = _Smoothness;
				float lerpResult2215 = lerp( 1.0 , SmoothnessRes468 , HorizonMask2300);
				float temp_output_1147_0 = ( ( 1.0 - lerpResult2215 ) * 8.0 );
				float clampResult1494 = clamp( (0.0 + (distance( GlobalOceanHeight1552 , _WorldSpaceCameraPos.y ) - 0.0) * (0.7 - 0.0) / (1.0 - 0.0)) , 0.0 , 0.7 );
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch3051 = 2.0;
				#else
				float staticSwitch3051 = clampResult1494;
				#endif
				float switchResult1466 = (((ase_vface>0)?(temp_output_1147_0):(staticSwitch3051)));
				float3 temp_output_1550_0 = (SAMPLE_TEXTURECUBE_LOD( _Cubemap, sampler_Cubemap, rotatedValue1157, switchResult1466 )).rgb;
				float3 normalizeResult1285 = normalize( ( tex2DNode11 * float3( 55,55,1 ) ) );
				float3 tanNormal1237 = normalizeResult1285;
				float3 worldNormal1237 = float3(dot(tanToWorld0,tanNormal1237), dot(tanToWorld1,tanNormal1237), dot(tanToWorld2,tanNormal1237));
				float dotResult1229 = dot( worldNormal1237 , WorldViewDirection );
				float customEye2812 = IN.ase_texcoord10.w;
				float cameraDepthFade2812 = (( customEye2812 -_ProjectionParams.y - 0.0 ) / _NormalDarken1);
				float lerpResult1280 = lerp( 1.0 , ( 1.0 - saturate( dotResult1229 ) ) , ( ( 1.0 - saturate( cameraDepthFade2812 ) ) * _NormalDarken ));
				float NormDarken1230 = lerpResult1280;
				float3 tanNormal901 = NormalRes466;
				float3 worldNormal901 = float3(dot(tanToWorld0,tanNormal901), dot(tanToWorld1,tanNormal901), dot(tanToWorld2,tanNormal901));
				#ifdef _HORIZONMAPPED_ON
				float3 staticSwitch2104 = float3(0,1,0);
				#else
				float3 staticSwitch2104 = worldNormal901;
				#endif
				float fresnelNdotV899 = dot( staticSwitch2104, WorldViewDirection );
				float fresnelNode899 = ( _RefFresnel.x + _RefFresnel.y * pow( 1.0 - fresnelNdotV899, _RefFresnel.z ) );
				float temp_output_904_0 = saturate( fresnelNode899 );
				float3 temp_output_1162_0 = ( temp_output_1550_0 * _CubemapExposure * (0.4 + (NormDarken1230 - 0.0) * (1.0 - 0.4) / (1.0 - 0.0)) * temp_output_904_0 * ShoreMaskSpec2479 );
				float3 WorldCamPos11_g3216 = _WorldPos;
				float3 WorldCamRight11_g3216 = _WorldDir;
				float force11_g3216 = _ForceEye;
				int localIsStereoEyeLeft11_g3216 = IsStereoEyeLeft11_g3216( WorldCamPos11_g3216 , WorldCamRight11_g3216 , force11_g3216 );
				float2 appendResult4_g3216 = (float2(ase_screenPosNorm.xy));
				float2 appendResult1245 = (float2(NormalRes466.xy));
				float clampResult1520 = clamp( (VertAdded1524).x , 0.0 , 0.3 );
				float2 temp_output_1_0_g3216 = ( appendResult4_g3216 + ( appendResult1245 * (0.0 + (clampResult1520 - 0.1) * (2.0 - 0.0) / (0.3 - 0.1)) * CamNear1615 * 0.5 ) );
				float4 tex2DNode3_g3216 = SAMPLE_TEXTURE2D( _TexLeft, sampler_Linear_Mirror, temp_output_1_0_g3216 );
				float4 tex2DNode2_g3216 = SAMPLE_TEXTURE2D( _TexRight, sampler_Linear_Mirror, temp_output_1_0_g3216 );
				float4 ifLocalVar10_g3216 = 0;
				if( localIsStereoEyeLeft11_g3216 <= 0.0 )
				ifLocalVar10_g3216 = tex2DNode2_g3216;
				else
				ifLocalVar10_g3216 = tex2DNode3_g3216;
				float3 temp_output_1549_0 = (ifLocalVar10_g3216).rgb;
				float3 UnderReflection1610 = ( temp_output_1549_0 * temp_output_904_0 );
				float3 switchResult1453 = (((ase_vface>0)?(( UnderReflection1610 * (0.4 + (NormDarken1230 - 0.0) * (1.0 - 0.4) / (1.0 - 0.0)) )):(temp_output_1162_0)));
				#ifdef GlobalPlanarReflection
				float3 staticSwitch945 = switchResult1453;
				#else
				float3 staticSwitch945 = temp_output_1162_0;
				#endif
				float4 appendResult11_g3229 = (float4(_TonemappingSettings.x , _TonemappingSettings.y , _TonemappingSettings.z , _TonemappingSettings.w));
				float4 temp_output_2533_0 = appendResult11_g3229;
				float4 settings2527 = temp_output_2533_0;
				float3 color2527 = staticSwitch945;
				float3 localApplyTonemapper2527 = ApplyTonemapper( settings2527 , color2527 );
				#ifdef _TRANSPARENTGRABPASS_ON
				float3 staticSwitch2528 = localApplyTonemapper2527;
				#else
				float3 staticSwitch2528 = staticSwitch945;
				#endif
				float4 fetchOpaqueVal1753 = float4( SHADERGRAPH_SAMPLE_SCENE_COLOR( ScreenSpaceRefractUV2387 ), 1.0 );
				float3 appendResult1754 = (float3(fetchOpaqueVal1753.rgb));
				#ifdef _RAINBOWGRABPASS_ON
				float3 staticSwitch2395 = appendResult1754;
				#else
				float3 staticSwitch2395 = appendResult1754;
				#endif
				float3 temp_output_18_0_g3379 = float3( 0,0,0 );
				float3 temp_output_30_0_g3379 = float3( 0,0,1 );
				float3 tanNormal12_g3379 = temp_output_30_0_g3379;
				float3 worldNormal12_g3379 = float3(dot(tanToWorld0,tanNormal12_g3379), dot(tanToWorld1,tanNormal12_g3379), dot(tanToWorld2,tanNormal12_g3379));
				float3 vertexToFrag144_g3379 = IN.ase_texcoord11.xyz;
				float dotResult1_g3379 = dot( worldNormal12_g3379 , vertexToFrag144_g3379 );
				float3 vertexToFrag7_g3383 = IN.ase_texcoord12.xyz;
				float3 positionWS_fogFactor24_g3383 = vertexToFrag7_g3383;
				float3 normalizeResult21_g3383 = normalize( ( _WorldSpaceCameraPos - positionWS_fogFactor24_g3383 ) );
				float3 view23_g3383 = normalizeResult21_g3383;
				float3 view16_g3383 = view23_g3383;
				float3 appendResult49_g3383 = (float3(ScreenSpaceRefractUV2387 , ase_screenPosNorm.w));
				float3 screenPosXYW29_g3383 = (appendResult49_g3383).xyz;
				float2 uvDepth38_g3383 = ( (screenPosXYW29_g3383).xy / (screenPosXYW29_g3383).z );
				float2 uv55_g3383 = uvDepth38_g3383;
				float localCameraDepthTexture55_g3383 = CameraDepthTexture55_g3383( uv55_g3383 );
				float rawDepth40_g3383 = localCameraDepthTexture55_g3383;
				float rawDepth44_g3383 = rawDepth40_g3383;
				float localMyCustomExpression44_g3383 = MyCustomExpression44_g3383( rawDepth44_g3383 );
				float sceneZ41_g3383 = localMyCustomExpression44_g3383;
				float sceneZ16_g3383 = sceneZ41_g3383;
				float3 localMyCustomExpression16_g3383 = MyCustomExpression16_g3383( view16_g3383 , sceneZ16_g3383 );
				float3 scenePos11_g3383 = localMyCustomExpression16_g3383;
				float3 WorldPosition2704 = scenePos11_g3383;
				float3 temp_output_105_0_g3379 = WorldPosition2704;
				float2 PanSpeed131_g3379 = (_CausticsSettings).xy;
				float mulTime28_g3379 = _TimeParameters.x * -1.0;
				float2 break101_g3379 = ( PanSpeed131_g3379 * mulTime28_g3379 );
				float3 appendResult100_g3379 = (float3(break101_g3379.x , 0.0 , break101_g3379.y));
				float4 Pos6_g3382 = float4( ( temp_output_105_0_g3379 + appendResult100_g3379 ) , 0.0 );
				float4x4 invertVal146_g3379 = Inverse4x4( _CausticMatrix );
				float4x4 Mat6_g3382 = invertVal146_g3379;
				float3 localMatrixMulThatWorks6_g3382 = MatrixMulThatWorks( Pos6_g3382 , Mat6_g3382 );
				float2 temp_output_87_0_g3379 = ( (localMatrixMulThatWorks6_g3382).xy + ( 0.0045 * 2.0 ) );
				float temp_output_117_0_g3379 = (temp_output_105_0_g3379).y;
				float temp_output_63_0_g3379 = GlobalOceanHeight1552;
				float2 DistanceFade134_g3379 = (_CausticsSettings).zw;
				float2 break136_g3379 = DistanceFade134_g3379;
				float temp_output_67_0_g3379 = ( saturate( (0.0 + (max( -( temp_output_117_0_g3379 - temp_output_63_0_g3379 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) * saturate( (1.0 + (distance( temp_output_63_0_g3379 , temp_output_117_0_g3379 ) - break136_g3379.x) * (0.0 - 1.0) / (break136_g3379.y - break136_g3379.x)) ) );
				float CausticMipLevel118_g3379 = ( ( 1.0 - temp_output_67_0_g3379 ) * 4.0 );
				float2 temp_output_75_0_g3379 = ( (localMatrixMulThatWorks6_g3382).xy + 0.0045 );
				#ifdef _USECAUSTICRAINBOW_ON
				float3 staticSwitch73_g3379 = ( ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Caustics, temp_output_87_0_g3379, CausticMipLevel118_g3379 ).r * float3(0,0,1) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Caustics, temp_output_75_0_g3379, CausticMipLevel118_g3379 ).r * float3(0,1,0) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3382).xy, CausticMipLevel118_g3379 ).r * float3(1,0,0) ) );
				#else
				float3 staticSwitch73_g3379 = float3( 0,0,0 );
				#endif
				#ifdef _USECAUSTICEXTRASAMPLER_ON
				float3 staticSwitch57_g3379 = staticSwitch73_g3379;
				#else
				float3 staticSwitch57_g3379 = float3( 0,0,0 );
				#endif
				float3 appendResult62_g3379 = (float3(_CausticsColor.rgb));
				#ifdef _CAUSTICSENABLE_ON
				float3 staticSwitch16_g3379 = ( temp_output_18_0_g3379 + ( max( dotResult1_g3379 , 0.0 ) * staticSwitch57_g3379 * appendResult62_g3379 * temp_output_67_0_g3379 ) );
				#else
				float3 staticSwitch16_g3379 = temp_output_18_0_g3379;
				#endif
				float3 appendResult3040 = (float3(_CausticsTint.rgb));
				float3 temp_output_3042_0 = ( staticSwitch16_g3379 * appendResult3040 * 8.0 );
				float3 switchResult3046 = (((ase_vface>0)?(temp_output_3042_0):(float3( 0,0,0 ))));
				float3 Caustics2711 = switchResult3046;
				float3 GrabPassOG1994 = ( staticSwitch2395 + ( staticSwitch2395 * Caustics2711 ) );
				float3 temp_cast_33 = (1.0).xxx;
				float eyeDepth6_g3373 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV2387, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3373 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float temp_output_12_0_g3373 = 1.0;
				float temp_output_3_0_g3373 = ( abs( ( eyeDepth6_g3373 - -worldToView21_g3373.z ) ) / temp_output_12_0_g3373 );
				float temp_output_1843_0 = saturate( pow( ( 1.0 - saturate( temp_output_3_0_g3373 ) ) , 6.0 ) );
				float ShallowMask2838 = ( ( 1.0 - temp_output_1843_0 ) * ShoreMaskDepthAlpha141 );
				float3 lerpResult2839 = lerp( temp_cast_33 , WaterDepthTint342 , ShallowMask2838);
				float3 switchResult3072 = (((ase_vface>0)?(lerpResult2839):(float3( 1,1,1 ))));
				float switchResult1513 = (((ase_vface>0)?(1.0):(CamNear1615)));
				float3 lerpResult1900 = lerp( GrabPassOG1994 , ( switchResult3072 * GrabPassOG1994 ) , switchResult1513);
				float3 GrabPassTinted1768 = lerpResult1900;
				float switchResult1945 = (((ase_vface>0)?(1.0):(CamNear1615)));
				float3 lerpResult1746 = lerp( GrabPassTinted1768 , ( staticSwitch2528 + GrabPassTinted1768 ) , ( ShoreMaskDepthAlpha141 * DepthFader1847 * switchResult1945 ));
				float3 NormalResBelow23057 = temp_output_1719_0;
				float3 tanNormal1468 = NormalResBelow23057;
				float3 worldNormal1468 = float3(dot(tanToWorld0,tanNormal1468), dot(tanToWorld1,tanNormal1468), dot(tanToWorld2,tanNormal1468));
				#ifdef _HORIZONMAPPED_ON
				float3 staticSwitch2108 = float3(0,1,0);
				#else
				float3 staticSwitch2108 = worldNormal1468;
				#endif
				float clampResult3065 = clamp( (-4.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - 0.1) * (-0.5 - -4.0) / (10.0 - 0.1)) , -4.0 , -0.5 );
				float fresnelNdotV1454 = dot( normalize( staticSwitch2108 ), -WorldViewDirection );
				float fresnelNode1454 = ( clampResult3065 + 4.0 * pow( max( 1.0 - fresnelNdotV1454 , 0.0001 ), 1.0 ) );
				float temp_output_1460_0 = saturate( fresnelNode1454 );
				float3 lerpResult1923 = lerp( lerpResult1746 , staticSwitch2528 , ( temp_output_1460_0 * CamNear1615 ));
				float3 switchResult1925 = (((ase_vface>0)?(lerpResult1746):(lerpResult1923)));
				#ifdef _TRANSPARENTGRABPASS_ON
				float3 staticSwitch1588 = switchResult1925;
				#else
				float3 staticSwitch1588 = ( ( ColorCacheRGB1061 * lerpResult3187 ) + ( saturate( (0.0 + (ShoreMaskSpec2479 - 0.3) * (1.0 - 0.0) / (1.0 - 0.3)) ) * staticSwitch2528 ) );
				#endif
				float3 appendResult2329 = (float3(staticSwitch1588));
				float3 EmissionRes467 = appendResult2329;
				float3 EmissionRes22320 = EmissionRes467;
				
				float2 clipScreen639 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither639 = Dither8x8Bayer( fmod(clipScreen639.x, 8), fmod(clipScreen639.y, 8) );
				dither639 = step( dither639, ShoreMaskDepthAlpha141 );
				#ifdef _USEDITHERING_ON
				float staticSwitch1685 = dither639;
				#else
				float staticSwitch1685 = ShoreMaskDepthAlpha141;
				#endif
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch1748 = 1.0;
				#else
				float staticSwitch1748 = staticSwitch1685;
				#endif
				float HorizonAlpha2083 = temp_output_2080_0;
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2085 = HorizonAlpha2083;
				#else
				float staticSwitch2085 = staticSwitch1748;
				#endif
				float AlphaRes469 = staticSwitch2085;
				
				float3 appendResult18_g3427 = (float3(_BakeLightMapAmbient.rgb));
				float2 vertexToFrag10_g3432 = IN.ase_texcoord13.xy;
				float4 tex2DNode7_g3432 = SAMPLE_TEXTURE2D_BIAS( _bakeLightmap, sampler_bakeLightmap, vertexToFrag10_g3432, 0.0 );
				float4 localURPDecodeInstruction19_g3432 = URPDecodeInstruction19_g3432();
				float3 decodeLightMap6_g3432 = DecodeLightmap(tex2DNode7_g3432,localURPDecodeInstruction19_g3432);
				float temp_output_17_0_g3429 = -0.001;
				float2 temp_cast_37 = (temp_output_17_0_g3429).xx;
				float2 temp_cast_38 = (( 1.0 - temp_output_17_0_g3429 )).xx;
				float2 break5_g3429 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_37) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_38 - temp_cast_37)) );
				float temp_output_1_0_g3431 = break5_g3429.x;
				float temp_output_8_0_g3429 = ( _BakedLightingFeather + -1.0 );
				float temp_output_1_0_g3430 = break5_g3429.y;
				float temp_output_17_0_g3427 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3431 - floor( ( temp_output_1_0_g3431 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3429 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3430 - floor( ( temp_output_1_0_g3430 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3429 - -1.0)) ) ) );
				float3 lerpResult12_g3427 = lerp( appendResult18_g3427 , decodeLightMap6_g3432 , temp_output_17_0_g3427);
				float3 LightmapRes26_g3427 = lerpResult12_g3427;
				float3 LightmapRes2657 = LightmapRes26_g3427;
				
				float4 settings2751 = temp_output_2533_0;
				float4 ifLocalVars112 = 0;
				float3 ColorCacheRGBDebug2443 = appendResult1062;
				if(_DebugVisual==0){ifLocalVars112 = float4( ColorCacheRGBDebug2443 , 0.0 ); };
				float4 temp_cast_40 = (CaptureDepthAOffsetShore1177).xxxx;
				if(_DebugVisual==1){ifLocalVars112 = temp_cast_40; };
				float2 appendResult955 = (float2(tex2DNode13.r , tex2DNode13.g));
				float2 CaptureRG_Direction956 = ( temp_output_2766_0 * abs( appendResult955 ) );
				if(_DebugVisual==2){ifLocalVars112 = float4( CaptureRG_Direction956, 0.0 , 0.0 ); };
				Gradient gradient2366 = NewGradient( 0, 8, 2, float4( 0.2970481, 0, 1, 0 ), float4( 0.9772253, 0, 1, 0.1602655 ), float4( 1, 0, 0, 0.3288777 ), float4( 1, 0.3057163, 0, 0.4524148 ), float4( 1, 0.8153982, 0, 0.5926604 ), float4( 0.3415842, 1, 0, 0.6978256 ), float4( 0.01040274, 0.8851848, 0.9615906, 0.8397345 ), float4( 0, 0, 0, 1 ), float2( 1, 0.05294881 ), float2( 1, 0.9382315 ), 0, 0, 0, 0, 0, 0 );
				float temp_output_2369_0 = ( CaptureB_Distancex100200 * 10.0 );
				if(_DebugVisual==3){ifLocalVars112 = SampleGradient( gradient2366, temp_output_2369_0 ); };
				float CaptureA_Depth320 = ( temp_output_2766_0 * tex2DNode13.a );
				float4 temp_cast_43 = (CaptureA_Depth320).xxxx;
				if(_DebugVisual==4){ifLocalVars112 = temp_cast_43; };
				float4 temp_cast_44 = (DepthCacheWave184).xxxx;
				if(_DebugVisual==5){ifLocalVars112 = temp_cast_44; };
				float4 temp_cast_45 = (FoamMaskResult122).xxxx;
				if(_DebugVisual==6){ifLocalVars112 = temp_cast_45; };
				#ifdef GlobalPlanarReflection
				float3 staticSwitch2350 = temp_output_1549_0;
				#else
				float3 staticSwitch2350 = temp_output_1550_0;
				#endif
				float3 RawReflection1737 = staticSwitch2350;
				if(_DebugVisual==7){ifLocalVars112 = float4( RawReflection1737 , 0.0 ); };
				if(_DebugVisual==8){ifLocalVars112 = float4( WaterDepthTint342 , 0.0 ); };
				if(_DebugVisual==9){ifLocalVars112 = float4( LightmapRes2657 , 0.0 ); };
				float2 vertexToFrag10_g3433 = IN.ase_texcoord13.zw;
				float4 tex2DNode7_g3433 = SAMPLE_TEXTURE2D_BIAS( _bakeLightmapSM, sampler_bakeLightmap, vertexToFrag10_g3433, 0.0 );
				float4 localURPDecodeInstruction19_g3433 = URPDecodeInstruction19_g3433();
				float3 decodeLightMap6_g3433 = DecodeLightmap(tex2DNode7_g3433,localURPDecodeInstruction19_g3433);
				float3 lerpResult13_g3427 = lerp( float3(1,0,0) , saturate( decodeLightMap6_g3433 ) , temp_output_17_0_g3427);
				float3 LightmapSWRes25_g3427 = lerpResult13_g3427;
				float3 temp_output_3209_27 = LightmapSWRes25_g3427;
				float3 LightmapSWRes2658 = temp_output_3209_27;
				if(_DebugVisual==10){ifLocalVars112 = float4( LightmapSWRes2658 , 0.0 ); };
				float4 temp_cast_51 = (ShoreMaskSpec2479).xxxx;
				if(_DebugVisual==11){ifLocalVars112 = temp_cast_51; };
				float VVVV2491 = temp_output_240_0;
				float4 temp_cast_52 = (VVVV2491).xxxx;
				if(_DebugVisual==12){ifLocalVars112 = temp_cast_52; };
				float4 DebugRes608 = ifLocalVars112;
				float3 color2751 = DebugRes608.xyz;
				float3 localApplyTonemapper2751 = ApplyTonemapper( settings2751 , color2751 );
				
				float4 _Vector3 = float4(1,1,1,0);
				float3 appendResult50_g3347 = (float3(unity_FogColor.rgb));
				float3 WorldPosition256_g3347 = WorldPosition;
				float3 worldToClip188_g3347 = TransformWorldToHClip(WorldPosition256_g3347).xyz;
				float In191_g3347 = worldToClip188_g3347.z;
				float localMyCustomExpression191_g3347 = MyCustomExpression191_g3347( In191_g3347 );
				float4 appendResult239_g3347 = (float4(appendResult50_g3347 , ( 1.0 - localMyCustomExpression191_g3347 )));
				#ifdef FOG_LINEAR
				float4 staticSwitch252_g3347 = appendResult239_g3347;
				#else
				float4 staticSwitch252_g3347 = float4( 0,0,0,0 );
				#endif
				float4 FogLinear53_g3347 = staticSwitch252_g3347;
				#ifdef _USEUNITYFOG_ON
				float4 staticSwitch267_g3347 = FogLinear53_g3347;
				#else
				float4 staticSwitch267_g3347 = _Vector3;
				#endif
				float4 appendResult47_g3347 = (float4(staticSwitch267_g3347));
				float3 ViewDir264_g3347 = WorldViewDirection;
				float3 viewDir240_g3357 = ViewDir264_g3347;
				float3 camWorldPos240_g3357 = _WorldSpaceCameraPos;
				float3 WorldPos252_g3357 = WorldPosition256_g3347;
				float3 posWS240_g3357 = WorldPos252_g3357;
				float4 oceanFogDensities240_g3357 = OceanFogDensities;
				float temp_output_67_0_g3347 = GlobalOceanHeight1552;
				float temp_output_108_0_g3357 = temp_output_67_0_g3347;
				float oceanHeight240_g3357 = temp_output_108_0_g3357;
				float4 oceanFogTop_RGB_Exponent240_g3357 = OceanFogTop_RGB_Exponent;
				float4 oceanFogBottom_RGB_Intensity240_g3357 = OceanFogBottom_RGB_Intensity;
				float4 localGetUnderWaterFogs240_g3357 = GetUnderWaterFogs240_g3357( viewDir240_g3357 , camWorldPos240_g3357 , posWS240_g3357 , oceanFogDensities240_g3357 , oceanHeight240_g3357 , oceanFogTop_RGB_Exponent240_g3357 , oceanFogBottom_RGB_Intensity240_g3357 );
				float4 FogRes185_g3357 = localGetUnderWaterFogs240_g3357;
				float3 appendResult94_g3357 = (float3(FogRes185_g3357.xyz));
				float3 temp_output_261_103_g3347 = appendResult94_g3357;
				float temp_output_61_0_g3357 = ( 1.0 - (FogRes185_g3357).w );
				float4 appendResult44_g3347 = (float4(temp_output_261_103_g3347 , ( 1.0 - temp_output_61_0_g3357 )));
				float4 ifLocalVar49_g3347 = 0;
				UNITY_BRANCH 
				if( temp_output_23_0_g3347 >= 1.0 )
				ifLocalVar49_g3347 = appendResult44_g3347;
				else
				ifLocalVar49_g3347 = appendResult47_g3347;
				#ifdef _USEGRADIENTFOG_ON
				float4 staticSwitch43_g3347 = ifLocalVar49_g3347;
				#else
				float4 staticSwitch43_g3347 = appendResult47_g3347;
				#endif
				#ifdef _USEUNDERWATER
				float4 staticSwitch215_g3347 = max( staticSwitch43_g3347 , float4( 0,0,0,0 ) );
				#else
				float4 staticSwitch215_g3347 = appendResult47_g3347;
				#endif
				float4 temp_output_3047_25 = staticSwitch215_g3347;
				float3 appendResult2432 = (float3(temp_output_3047_25.xyz));
				float4 appendResult2433 = (float4(appendResult2432 , 0.0));
				float4 lerpResult1917 = lerp( appendResult2433 , temp_output_3047_25 , CamNear1615);
				float4 FogColorRes2321 = lerpResult1917;
				
				float3 temp_cast_58 = (1.0).xxx;
				float3 appendResult100_g3357 = (float3(OceanWaterTint_RGB.xyz));
				float3 temp_cast_60 = (1.0).xxx;
				float3 ifLocalVar170_g3347 = 0;
				UNITY_BRANCH 
				if( temp_output_23_0_g3347 >= 1.0 )
				ifLocalVar170_g3347 = appendResult100_g3357;
				else
				ifLocalVar170_g3347 = temp_cast_60;
				#ifdef _USEUNDERWATER
				float3 staticSwitch212_g3347 = ifLocalVar170_g3347;
				#else
				float3 staticSwitch212_g3347 = temp_cast_58;
				#endif
				float switchResult2429 = (((ase_vface>0)?(1.0):(1.3)));
				float3 lerpResult2314 = lerp( float3( 1,1,1 ) , ( staticSwitch212_g3347 * switchResult2429 ) , ( CamNear1615 * 0.9 ));
				float3 TintRes2322 = lerpResult2314;
				

				float3 BaseColor = AlbedoRes22337.rgb;
				float3 Normal = float3(0, 0, 1);
				float3 Emission = EmissionRes22320;
				float3 Specular = 0.5;
				float Metallic = 0;
				float Smoothness = ( ShoreMaskDepthAlpha141 * SmoothnessRes468 * DepthFader1847 );
				float Occlusion = 1;
				float Alpha = AlphaRes469;
				float AlphaClipThreshold = _ClipThreshold;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = LightmapRes2657;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;
				
				//Shadowood: new properties below:
				float3 DebugVisuals = localApplyTonemapper2751;
				float4 ShadowMask = float4( LightmapSWRes2658 , 0.0 );
				float4 FogColor = FogColorRes2321;
				half FresnelControl = 0;
				half4 SSSControl = 0;
				half4 SSSColor = 0;
				float4 Tonemapping = temp_output_2533_0;
				float3 Tint = TintRes2322;
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
				#if ((defined(_ASE_ANIS) && defined(_USE_ANIS_ON)) || defined(_ASE_SSSCONTROL) && defined(_USE_SSS_ON)) || defined(_ASE_FRESNELCONTROL)
					half4 color = UniversalFragmentPBRCustom( inputData, surfaceData, FresnelControl, SSSColor, SSSControl, WorldNormal, WorldTangent, AnisotropicControl );
				#else
					half4 color = UniversalFragmentPBR( inputData, surfaceData); // Original
				#endif

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
			ColorMask 0

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#define ASE_TESSELLATION 1
			#pragma require tessellation tessHW
			#pragma hull HullFunction
			#pragma domain DomainFunction
			#define ASE_DISTANCE_TESSELLATION
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_TONEMAPPING 1
			#define _ASE_TINT 1
			#define _ASE_DEBUGVISUALS 1
			#define ASE_FOG 1
			#define _NORMAL_DROPOFF_TS 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
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
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#pragma shader_feature_local ASE_TESSELLATION_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature_local LIGHTMAP_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USEINFINITYEDGE_ON
			#pragma shader_feature_local _USEVERTDISPLACEMENT_ON
			#pragma shader_feature_local _USEWAVENOISEDISPLACE_ON
			#pragma shader_feature_local _SPACE_WORLDSPACE _SPACE_UVSPACE
			#pragma shader_feature_local _HORIZONMAPPED_ON
			#pragma shader_feature_local _USEWAVEDIRECTION_ON
			#pragma shader_feature_local _USEINFINITYFEATHER_ON
			#pragma shader_feature_local _TRANSPARENTGRABPASS_ON
			#pragma shader_feature_local _USEDITHERING_ON
			#pragma shader_feature_local _USEEXTRANORMALS_ON
			#pragma multi_compile __ GlobalPlanarReflection


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
			float4 _FoamColor;
			float4 _Motion;
			float4 _Normals_ST;
			float4 _WaterTint;
			float4 _RemapVertShore;
			float4 _WaterAlbedo;
			float4 _WaveDirectionSettings;
			float4 _Normals2_ST;
			float4 _Motion2;
			float4 _RefFresnel;
			float4 _CausticsTint;
			float4x4 _DepthMatrix;
			float4 _RemapAlpha;
			float4 _BakeLightMapAmbient;
			float4x4 _BakeMatrix;
			float4 _bakeLightmapST;
			float4 _RemapFoam;
			float3 _WorldDir;
			float3 _WorldPos;
			float3 _DepthPosition;
			float3 _CameraForward;
			float _WaveRefractionStrength;
			float _RefractionWarp;
			float _RemapWaterDistance;
			float _RemapWaterDistance1;
			float _Smoothness;
			float _WaterDepthPow;
			float _WaterSaturation;
			float _CubemapExposure;
			float _NormalDarken1;
			float _NormalDarken;
			float _ForceEye;
			float _ClipThreshold;
			float _WaterDepthPow1;
			int _ZTestMode;
			float _NormalPow2;
			float _FoamMaxDistance;
			float _ZWriteToggle;
			float _TessNum;
			float _TesMin;
			float _TesMax;
			float _WaveScale;
			float _FoamNoiseSpeed;
			float _FoamNoiseScale;
			float _FoamNoiseCon;
			float _FoamNoiseScale2;
			float _FoamNoiseCon2;
			float _DistanceFadeA;
			float _WaveSpeed;
			float _DistanceFadeB;
			float _VertDispMult;
			float _Tolerance;
			float _Feather;
			float _FoamScale_UseFoam;
			float _NormalPow;
			float _BakedLightingFeather;
			float _RefDistortion;
			float _WaterRefractonMobile;
			float _FoamPow;
			int _DebugVisual;
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

			float4 _skyGradientColor1;
			float4 _skyGradientColor2;
			float3 _CausticsDir;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_FoamLUT);
			TEXTURE2D(_DepthCache);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE2D(_WaveNoiseDisplace);
			SAMPLER(sampler_WaveNoiseDisplace);
			TEXTURE2D(_ColorCache);
			TEXTURE2D(_Normals);
			SAMPLER(sampler_Normals);
			TEXTURE2D(_Normals2);
			SAMPLER(sampler_Normals2);


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
			
			float4 Billboard2078( float3 worldPosIN, float2 screenPos, float3 QuadNormal, float3 QuadPosition )
			{
				float3 worldPos = worldPosIN;
				float3 viewDir = worldPos - _WorldSpaceCameraPos;
				float3 reflectDir = -viewDir;
				float3 nReflDirection = normalize(reflectDir);
				float4 result = float4(0,0,0,0);
				// Ray-quad intersection
				half planeIntersectDistance = dot( worldPos - QuadPosition,  QuadNormal.xyz ) / dot( nReflDirection, QuadNormal.xyz );
				half3 intersectPosition = worldPos - nReflDirection * planeIntersectDistance;
				//half4 localPlaneIntersectPosition = mul( QuadInverseMatrix, float4( intersectPosition, 1.0 ));
				half4 localPlaneIntersectPosition = float4( intersectPosition, 1.0 );
				half2 billboardUV = half2( localPlaneIntersectPosition.x, localPlaneIntersectPosition.z );
				//billboardUV*=1;
				//float4 billboardCol = BillboardTex.Sample( BillboardSampler , billboardUV );
				float4 billboardCol = float4( billboardUV.x, billboardUV.y, 0, 1);
				float3 quadNorm = QuadNormal.xyz;
				half reflectDot = dot( nReflDirection, quadNorm );
				float w = 1;
				// Cull when beyond the quad bounds
				//w = billboardUV.x > 1.0 ? 0.0 : w;
				//w = billboardUV.y > 1.0 ? 0.0 : w;
				//w = billboardUV.x < 0.0 ? 0.0 : w;
				//w = billboardUV.y < 0.0 ? 0.0 : w;
				// Cull if distance is behind the surface
				//w = planeIntersectDistance < 0 ? 0 : w;
				// Cull if quad facing backwards
				w = reflectDot <= 0.0 ? 0.0 : w; 
				// Cull if quad facing backwards and culling is ticked on the quad ( passed in via the arrays W )
				//w = QuadNormal.w == 1.0 ? (reflectDot <= 0.0 ? 0.0 : w) : w;
				billboardCol = billboardCol * billboardCol.w * w;
				billboardCol.a = w;
				result*=1-w;
				result+= billboardCol;
				return result;
			}
			
			inline float Dither8x8Bayer( int x, int y )
			{
				const float dither[ 64 ] = {
			 1, 49, 13, 61,  4, 52, 16, 64,
			33, 17, 45, 29, 36, 20, 48, 32,
			 9, 57,  5, 53, 12, 60,  8, 56,
			41, 25, 37, 21, 44, 28, 40, 24,
			 3, 51, 15, 63,  2, 50, 14, 62,
			35, 19, 47, 31, 34, 18, 46, 30,
			11, 59,  7, 55, 10, 58,  6, 54,
			43, 27, 39, 23, 42, 26, 38, 22};
				int r = y * 8 + x;
				return dither[r] / 64; // same # of instructions as pre-dividing due to compiler magic
			}
			

			float3 _LightDirection;
			float3 _LightPosition;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float2 texCoord2052 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_2054_0 = distance( texCoord2052 , float2( 0.5,0.5 ) );
				float ifLocalVar2053 = 0;
				if( temp_output_2054_0 <= 1.0 )
				ifLocalVar2053 = 1.0;
				else
				ifLocalVar2053 = 5000.0;
				#ifdef _USEINFINITYEDGE_ON
				float4 staticSwitch1498 = ( v.vertex * ifLocalVar2053 );
				#else
				float4 staticSwitch1498 = v.vertex;
				#endif
				float4 break1605 = staticSwitch1498;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 break19 = mul( _DepthMatrix, float4( ( ase_worldPos - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D_LOD( _DepthCache, sampler_Linear_Clamp, CacheUV1109, 0.0 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_2 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_3 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_2) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_3 - temp_cast_2)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 temp_cast_4 = (_FoamNoiseSpeed).xx;
				float3 break35 = ase_worldPos;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = ase_worldPos;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner138 = ( 1.0 * _Time.y * temp_cast_4 + ( WorldXZ1087 * _FoamNoiseScale ));
				float lerpResult133 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( 0.04 * panner138 ), 0.0 ).r , _FoamNoiseCon);
				float2 temp_cast_6 = (_FoamNoiseSpeed).xx;
				float2 panner168 = ( 1.0 * _Time.y * temp_cast_6 + ( WorldXZ1087 * _FoamNoiseScale2 ));
				float lerpResult161 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( panner168 * 0.04 ), 0.0 ).r , _FoamNoiseCon2);
				float UnderOffset1634 = GlobalOceanOffset;
				float3 appendResult2127 = (float3(break1605.x , ( break1605.y + UnderOffset1634 ) , break1605.z));
				float3 VertResOffset2125 = appendResult2127;
				float3 customSurfaceDepth1169 = VertResOffset2125;
				float customEye1169 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1169)).z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				#ifdef _USEWAVENOISEDISPLACE_ON
				float staticSwitch680 = ( Foamness294 + ( ( lerpResult133 + lerpResult161 ) * ( ( 1.0 - DistanceFadeA1294 ) * 0.4 ) ) );
				#else
				float staticSwitch680 = Foamness294;
				#endif
				float DepthCacheRemap109 = staticSwitch680;
				float temp_output_5_0_g3212 = ( 1.0 - DepthCacheRemap109 );
				float mulTime209 = _TimeParameters.x * ( _WaveSpeed * 0.1 );
				float2 appendResult868 = (float2(_WaveDirectionSettings.x , _WaveDirectionSettings.y));
				float WaveCenterDistance1051 = distance( WorldXZ1087 , appendResult868 );
				#ifdef _USEWAVEDIRECTION_ON
				float staticSwitch762 = ( mulTime209 + ( ( WaveCenterDistance1051 * -0.01 ) / _WaveDirectionSettings.z ) );
				#else
				float staticSwitch762 = ( 0.0 - mulTime209 );
				#endif
				float temp_output_17_0_g3212 = staticSwitch762;
				float temp_output_30_0_g3212 = ( 1.0 - saturate( 0.7 ) );
				float temp_output_35_0_g3212 = ( temp_output_30_0_g3212 * 0.5 );
				float temp_output_32_0_g3212 = ( (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) - temp_output_35_0_g3212 );
				float temp_output_31_0_g3212 = ( temp_output_35_0_g3212 + (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) );
				float DepthCacheWave184 = ( 1.0 - frac( (0.0 + (temp_output_5_0_g3212 - temp_output_32_0_g3212) * (1.0 - 0.0) / (temp_output_31_0_g3212 - temp_output_32_0_g3212)) ) );
				float clampResult124 = clamp( DepthCacheWave184 , 0.01 , 0.99 );
				float2 temp_cast_7 = (clampResult124).xx;
				float4 tex2DNode76 = SAMPLE_TEXTURE2D_LOD( _FoamLUT, sampler_Linear_Clamp, temp_cast_7, 0.0 );
				float smoothstepResult279 = smoothstep( 0.0 , 1.0 , tex2DNode76.a);
				float3 customSurfaceDepth1325 = VertResOffset2125;
				float customEye1325 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1325)).z;
				float cameraDepthFade1325 = (( customEye1325 -_ProjectionParams.y - 0.0 ) / _DistanceFadeB);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2111 = 0.0;
				#else
				float staticSwitch2111 = saturate( cameraDepthFade1325 );
				#endif
				float DistanceFadeB1326 = staticSwitch2111;
				float lerpResult1311 = lerp( smoothstepResult279 , 0.0 , DistanceFadeB1326);
				float FoamVertDisp261 = saturate( lerpResult1311 );
				float temp_output_240_0 = ( FoamVertDisp261 * _VertDispMult * 0.1 );
				float ShoreMaskVert586 = saturate( (0.0 + (( CaptureB_Distancex100200 * 100.0 ) - _RemapVertShore.x) * (1.0 - 0.0) / (_RemapVertShore.y - _RemapVertShore.x)) );
				float VertDispDistMask22014 = ( 1.0 - saturate( (0.0 + (distance( WorldXZ1087 , (_WorldSpaceCameraPos).xz ) - 5.0) * (1.0 - 0.0) / (7.9 - 5.0)) ) );
				float3 customSurfaceDepth1484 = VertResOffset2125;
				float customEye1484 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1484)).z;
				float cameraDepthFade1484 = (( customEye1484 -_ProjectionParams.y - 0.0 ) / 1.0);
				float CamDepthFade1495 = cameraDepthFade1484;
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float VertDispMaskRes1691 = ( ShoreMaskVert586 * saturate( VertDispDistMask22014 ) * saturate( (0.0 + (CamDepthFade1495 - 0.2) * (1.0 - 0.0) / (1.5 - 0.2)) ) * InfinityFeather1410 );
				float temp_output_238_0 = ( temp_output_240_0 * VertDispMaskRes1691 );
				#ifdef _USEVERTDISPLACEMENT_ON
				float staticSwitch1688 = temp_output_238_0;
				#else
				float staticSwitch1688 = 0.0;
				#endif
				float VertAdded1524 = staticSwitch1688;
				float3 appendResult1606 = (float3(break1605.x , ( break1605.y + VertAdded1524 + UnderOffset1634 ) , break1605.z));
				float3 VertResDisplaced328 = appendResult1606;
				
				o.ase_texcoord3.z = customEye1169;
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertResDisplaced328;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = float3(0,1,0);

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
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessNum; float tessMin = _TesMin; float tessMax = _TesMax;
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
						, bool ase_vface : SV_IsFrontFace ) : SV_TARGET
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

				float2 appendResult892 = (float2(_Motion2.x , _Motion2.y));
				float2 Motion2XY1096 = appendResult892;
				float3 break35 = WorldPosition;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = WorldPosition;
				float4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner890 = ( 0.05 * _Time.y * Motion2XY1096 + WorldXZ1087);
				#ifdef GlobalPlanarReflection
				float staticSwitch1591 = _NormalPow;
				#else
				float staticSwitch1591 = ( _NormalPow * 0.7 );
				#endif
				float temp_output_1436_0 = ( _NormalPow * 6.0 );
				float switchResult1435 = (((ase_vface>0)?(staticSwitch1591):(temp_output_1436_0)));
				float3 unpack11 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals, sampler_Normals, (panner890*_Normals_ST.xy + _Normals_ST.zw) ), switchResult1435 );
				unpack11.z = lerp( 1, unpack11.z, saturate(switchResult1435) );
				float3 tex2DNode11 = unpack11;
				float2 appendResult88 = (float2(_Motion.z , _Motion.w));
				float2 MotionZW1085 = appendResult88;
				float2 panner85 = ( 0.1 * _Time.y * MotionZW1085 + WorldXZ1087);
				float switchResult1432 = (((ase_vface>0)?(( _NormalPow2 * 2.0 )):(( _NormalPow2 * 12.0 ))));
				float3 unpack84 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner85*_Normals2_ST.xy + _Normals2_ST.zw) ), switchResult1432 );
				unpack84.z = lerp( 1, unpack84.z, saturate(switchResult1432) );
				float2 panner1722 = ( -0.05 * _Time.y * MotionZW1085 + WorldXZ1087);
				float3 unpack1718 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner1722*( _Normals2_ST.xy * float2( 0.5,0.5 ) ) + _Normals2_ST.zw) ), switchResult1432 );
				unpack1718.z = lerp( 1, unpack1718.z, saturate(switchResult1432) );
				float3 temp_output_1719_0 = BlendNormal( unpack84 , unpack1718 );
				float3 temp_output_83_0 = BlendNormal( tex2DNode11 , temp_output_1719_0 );
				#ifdef _USEEXTRANORMALS_ON
				float3 staticSwitch1683 = temp_output_83_0;
				#else
				float3 staticSwitch1683 = temp_output_83_0;
				#endif
				float2 appendResult1049 = (float2(staticSwitch1683.xy));
				float2 RefNorm886 = appendResult1049;
				float RefBumpSlider1337 = _RefDistortion;
				float3 normalizeResult2273 = normalize( ( staticSwitch1683 + float3( ( RefNorm886 * RefBumpSlider1337 ) ,  0.0 ) ) );
				float2 appendResult2519 = (float2(normalizeResult2273.xy));
				float3 break19 = mul( _DepthMatrix, float4( ( WorldPosition - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D( _DepthCache, sampler_Linear_Clamp, CacheUV1109 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_6 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_7 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_6) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_7 - temp_cast_6)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 lerpResult2274 = lerp( float2( 0,0 ) , appendResult2519 , ( _WaterRefractonMobile * 0.5 * saturate( Foamness294 ) ));
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float customEye1169 = IN.ase_texcoord3.z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				float temp_output_1414_0 = ( ( 1.0 - InfinityFeather1410 ) * saturate( DistanceFadeA1294 ) );
				float2 lerpResult2275 = lerp( lerpResult2274 , float2( 0,0 ) , temp_output_1414_0);
				#ifdef _TRANSPARENTGRABPASS_ON
				float2 staticSwitch2542 = float2( 0,0 );
				#else
				float2 staticSwitch2542 = lerpResult2275;
				#endif
				float2 RefHackeryForColorCache2269 = staticSwitch2542;
				float2 temp_output_2198_0 = ( RefHackeryForColorCache2269 + CacheUV1109 );
				float4 tex2DNode657 = SAMPLE_TEXTURE2D( _ColorCache, sampler_Linear_Clamp, temp_output_2198_0 );
				float ColorCacheA2171 = tex2DNode657.a;
				float temp_output_12_0_g3214 = InfinityFeather1410;
				float CaptureDepthAOffsetShore1177 = ( ( ColorCacheA2171 * temp_output_12_0_g3214 ) + ( 1.0 - temp_output_12_0_g3214 ) );
				float4 break993 = _RemapAlpha;
				float switchResult2294 = (((ase_vface>0)?(saturate( (0.0 + (CaptureDepthAOffsetShore1177 - break993.x) * (1.0 - 0.0) / (break993.y - break993.x)) )):(1.0)));
				float ShoreMaskDepthAlpha141 = switchResult2294;
				float2 clipScreen639 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither639 = Dither8x8Bayer( fmod(clipScreen639.x, 8), fmod(clipScreen639.y, 8) );
				dither639 = step( dither639, ShoreMaskDepthAlpha141 );
				#ifdef _USEDITHERING_ON
				float staticSwitch1685 = dither639;
				#else
				float staticSwitch1685 = ShoreMaskDepthAlpha141;
				#endif
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch1748 = 1.0;
				#else
				float staticSwitch1748 = staticSwitch1685;
				#endif
				float HorizonAlpha2083 = temp_output_2080_0;
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2085 = HorizonAlpha2083;
				#else
				float staticSwitch2085 = staticSwitch1748;
				#endif
				float AlphaRes469 = staticSwitch2085;
				

				float Alpha = AlphaRes469;
				float AlphaClipThreshold = _ClipThreshold;
				float AlphaClipThresholdShadow = 0.5;

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
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#define ASE_TESSELLATION 1
			#pragma require tessellation tessHW
			#pragma hull HullFunction
			#pragma domain DomainFunction
			#define ASE_DISTANCE_TESSELLATION
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_TONEMAPPING 1
			#define _ASE_TINT 1
			#define _ASE_DEBUGVISUALS 1
			#define ASE_FOG 1
			#define _NORMAL_DROPOFF_TS 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
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
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#pragma shader_feature_local ASE_TESSELLATION_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature_local LIGHTMAP_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USEINFINITYEDGE_ON
			#pragma shader_feature_local _USEVERTDISPLACEMENT_ON
			#pragma shader_feature_local _USEWAVENOISEDISPLACE_ON
			#pragma shader_feature_local _SPACE_WORLDSPACE _SPACE_UVSPACE
			#pragma shader_feature_local _HORIZONMAPPED_ON
			#pragma shader_feature_local _USEWAVEDIRECTION_ON
			#pragma shader_feature_local _USEINFINITYFEATHER_ON
			#pragma shader_feature_local _TRANSPARENTGRABPASS_ON
			#pragma shader_feature_local _USEDITHERING_ON
			#pragma shader_feature_local _USEEXTRANORMALS_ON
			#pragma multi_compile __ GlobalPlanarReflection


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
			float4 _FoamColor;
			float4 _Motion;
			float4 _Normals_ST;
			float4 _WaterTint;
			float4 _RemapVertShore;
			float4 _WaterAlbedo;
			float4 _WaveDirectionSettings;
			float4 _Normals2_ST;
			float4 _Motion2;
			float4 _RefFresnel;
			float4 _CausticsTint;
			float4x4 _DepthMatrix;
			float4 _RemapAlpha;
			float4 _BakeLightMapAmbient;
			float4x4 _BakeMatrix;
			float4 _bakeLightmapST;
			float4 _RemapFoam;
			float3 _WorldDir;
			float3 _WorldPos;
			float3 _DepthPosition;
			float3 _CameraForward;
			float _WaveRefractionStrength;
			float _RefractionWarp;
			float _RemapWaterDistance;
			float _RemapWaterDistance1;
			float _Smoothness;
			float _WaterDepthPow;
			float _WaterSaturation;
			float _CubemapExposure;
			float _NormalDarken1;
			float _NormalDarken;
			float _ForceEye;
			float _ClipThreshold;
			float _WaterDepthPow1;
			int _ZTestMode;
			float _NormalPow2;
			float _FoamMaxDistance;
			float _ZWriteToggle;
			float _TessNum;
			float _TesMin;
			float _TesMax;
			float _WaveScale;
			float _FoamNoiseSpeed;
			float _FoamNoiseScale;
			float _FoamNoiseCon;
			float _FoamNoiseScale2;
			float _FoamNoiseCon2;
			float _DistanceFadeA;
			float _WaveSpeed;
			float _DistanceFadeB;
			float _VertDispMult;
			float _Tolerance;
			float _Feather;
			float _FoamScale_UseFoam;
			float _NormalPow;
			float _BakedLightingFeather;
			float _RefDistortion;
			float _WaterRefractonMobile;
			float _FoamPow;
			int _DebugVisual;
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

			float4 _skyGradientColor1;
			float4 _skyGradientColor2;
			float3 _CausticsDir;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_FoamLUT);
			TEXTURE2D(_DepthCache);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE2D(_WaveNoiseDisplace);
			SAMPLER(sampler_WaveNoiseDisplace);
			TEXTURE2D(_ColorCache);
			TEXTURE2D(_Normals);
			SAMPLER(sampler_Normals);
			TEXTURE2D(_Normals2);
			SAMPLER(sampler_Normals2);


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
			
			float4 Billboard2078( float3 worldPosIN, float2 screenPos, float3 QuadNormal, float3 QuadPosition )
			{
				float3 worldPos = worldPosIN;
				float3 viewDir = worldPos - _WorldSpaceCameraPos;
				float3 reflectDir = -viewDir;
				float3 nReflDirection = normalize(reflectDir);
				float4 result = float4(0,0,0,0);
				// Ray-quad intersection
				half planeIntersectDistance = dot( worldPos - QuadPosition,  QuadNormal.xyz ) / dot( nReflDirection, QuadNormal.xyz );
				half3 intersectPosition = worldPos - nReflDirection * planeIntersectDistance;
				//half4 localPlaneIntersectPosition = mul( QuadInverseMatrix, float4( intersectPosition, 1.0 ));
				half4 localPlaneIntersectPosition = float4( intersectPosition, 1.0 );
				half2 billboardUV = half2( localPlaneIntersectPosition.x, localPlaneIntersectPosition.z );
				//billboardUV*=1;
				//float4 billboardCol = BillboardTex.Sample( BillboardSampler , billboardUV );
				float4 billboardCol = float4( billboardUV.x, billboardUV.y, 0, 1);
				float3 quadNorm = QuadNormal.xyz;
				half reflectDot = dot( nReflDirection, quadNorm );
				float w = 1;
				// Cull when beyond the quad bounds
				//w = billboardUV.x > 1.0 ? 0.0 : w;
				//w = billboardUV.y > 1.0 ? 0.0 : w;
				//w = billboardUV.x < 0.0 ? 0.0 : w;
				//w = billboardUV.y < 0.0 ? 0.0 : w;
				// Cull if distance is behind the surface
				//w = planeIntersectDistance < 0 ? 0 : w;
				// Cull if quad facing backwards
				w = reflectDot <= 0.0 ? 0.0 : w; 
				// Cull if quad facing backwards and culling is ticked on the quad ( passed in via the arrays W )
				//w = QuadNormal.w == 1.0 ? (reflectDot <= 0.0 ? 0.0 : w) : w;
				billboardCol = billboardCol * billboardCol.w * w;
				billboardCol.a = w;
				result*=1-w;
				result+= billboardCol;
				return result;
			}
			
			inline float Dither8x8Bayer( int x, int y )
			{
				const float dither[ 64 ] = {
			 1, 49, 13, 61,  4, 52, 16, 64,
			33, 17, 45, 29, 36, 20, 48, 32,
			 9, 57,  5, 53, 12, 60,  8, 56,
			41, 25, 37, 21, 44, 28, 40, 24,
			 3, 51, 15, 63,  2, 50, 14, 62,
			35, 19, 47, 31, 34, 18, 46, 30,
			11, 59,  7, 55, 10, 58,  6, 54,
			43, 27, 39, 23, 42, 26, 38, 22};
				int r = y * 8 + x;
				return dither[r] / 64; // same # of instructions as pre-dividing due to compiler magic
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float2 texCoord2052 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_2054_0 = distance( texCoord2052 , float2( 0.5,0.5 ) );
				float ifLocalVar2053 = 0;
				if( temp_output_2054_0 <= 1.0 )
				ifLocalVar2053 = 1.0;
				else
				ifLocalVar2053 = 5000.0;
				#ifdef _USEINFINITYEDGE_ON
				float4 staticSwitch1498 = ( v.vertex * ifLocalVar2053 );
				#else
				float4 staticSwitch1498 = v.vertex;
				#endif
				float4 break1605 = staticSwitch1498;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 break19 = mul( _DepthMatrix, float4( ( ase_worldPos - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D_LOD( _DepthCache, sampler_Linear_Clamp, CacheUV1109, 0.0 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_2 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_3 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_2) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_3 - temp_cast_2)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 temp_cast_4 = (_FoamNoiseSpeed).xx;
				float3 break35 = ase_worldPos;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = ase_worldPos;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner138 = ( 1.0 * _Time.y * temp_cast_4 + ( WorldXZ1087 * _FoamNoiseScale ));
				float lerpResult133 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( 0.04 * panner138 ), 0.0 ).r , _FoamNoiseCon);
				float2 temp_cast_6 = (_FoamNoiseSpeed).xx;
				float2 panner168 = ( 1.0 * _Time.y * temp_cast_6 + ( WorldXZ1087 * _FoamNoiseScale2 ));
				float lerpResult161 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( panner168 * 0.04 ), 0.0 ).r , _FoamNoiseCon2);
				float UnderOffset1634 = GlobalOceanOffset;
				float3 appendResult2127 = (float3(break1605.x , ( break1605.y + UnderOffset1634 ) , break1605.z));
				float3 VertResOffset2125 = appendResult2127;
				float3 customSurfaceDepth1169 = VertResOffset2125;
				float customEye1169 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1169)).z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				#ifdef _USEWAVENOISEDISPLACE_ON
				float staticSwitch680 = ( Foamness294 + ( ( lerpResult133 + lerpResult161 ) * ( ( 1.0 - DistanceFadeA1294 ) * 0.4 ) ) );
				#else
				float staticSwitch680 = Foamness294;
				#endif
				float DepthCacheRemap109 = staticSwitch680;
				float temp_output_5_0_g3212 = ( 1.0 - DepthCacheRemap109 );
				float mulTime209 = _TimeParameters.x * ( _WaveSpeed * 0.1 );
				float2 appendResult868 = (float2(_WaveDirectionSettings.x , _WaveDirectionSettings.y));
				float WaveCenterDistance1051 = distance( WorldXZ1087 , appendResult868 );
				#ifdef _USEWAVEDIRECTION_ON
				float staticSwitch762 = ( mulTime209 + ( ( WaveCenterDistance1051 * -0.01 ) / _WaveDirectionSettings.z ) );
				#else
				float staticSwitch762 = ( 0.0 - mulTime209 );
				#endif
				float temp_output_17_0_g3212 = staticSwitch762;
				float temp_output_30_0_g3212 = ( 1.0 - saturate( 0.7 ) );
				float temp_output_35_0_g3212 = ( temp_output_30_0_g3212 * 0.5 );
				float temp_output_32_0_g3212 = ( (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) - temp_output_35_0_g3212 );
				float temp_output_31_0_g3212 = ( temp_output_35_0_g3212 + (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) );
				float DepthCacheWave184 = ( 1.0 - frac( (0.0 + (temp_output_5_0_g3212 - temp_output_32_0_g3212) * (1.0 - 0.0) / (temp_output_31_0_g3212 - temp_output_32_0_g3212)) ) );
				float clampResult124 = clamp( DepthCacheWave184 , 0.01 , 0.99 );
				float2 temp_cast_7 = (clampResult124).xx;
				float4 tex2DNode76 = SAMPLE_TEXTURE2D_LOD( _FoamLUT, sampler_Linear_Clamp, temp_cast_7, 0.0 );
				float smoothstepResult279 = smoothstep( 0.0 , 1.0 , tex2DNode76.a);
				float3 customSurfaceDepth1325 = VertResOffset2125;
				float customEye1325 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1325)).z;
				float cameraDepthFade1325 = (( customEye1325 -_ProjectionParams.y - 0.0 ) / _DistanceFadeB);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2111 = 0.0;
				#else
				float staticSwitch2111 = saturate( cameraDepthFade1325 );
				#endif
				float DistanceFadeB1326 = staticSwitch2111;
				float lerpResult1311 = lerp( smoothstepResult279 , 0.0 , DistanceFadeB1326);
				float FoamVertDisp261 = saturate( lerpResult1311 );
				float temp_output_240_0 = ( FoamVertDisp261 * _VertDispMult * 0.1 );
				float ShoreMaskVert586 = saturate( (0.0 + (( CaptureB_Distancex100200 * 100.0 ) - _RemapVertShore.x) * (1.0 - 0.0) / (_RemapVertShore.y - _RemapVertShore.x)) );
				float VertDispDistMask22014 = ( 1.0 - saturate( (0.0 + (distance( WorldXZ1087 , (_WorldSpaceCameraPos).xz ) - 5.0) * (1.0 - 0.0) / (7.9 - 5.0)) ) );
				float3 customSurfaceDepth1484 = VertResOffset2125;
				float customEye1484 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1484)).z;
				float cameraDepthFade1484 = (( customEye1484 -_ProjectionParams.y - 0.0 ) / 1.0);
				float CamDepthFade1495 = cameraDepthFade1484;
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float VertDispMaskRes1691 = ( ShoreMaskVert586 * saturate( VertDispDistMask22014 ) * saturate( (0.0 + (CamDepthFade1495 - 0.2) * (1.0 - 0.0) / (1.5 - 0.2)) ) * InfinityFeather1410 );
				float temp_output_238_0 = ( temp_output_240_0 * VertDispMaskRes1691 );
				#ifdef _USEVERTDISPLACEMENT_ON
				float staticSwitch1688 = temp_output_238_0;
				#else
				float staticSwitch1688 = 0.0;
				#endif
				float VertAdded1524 = staticSwitch1688;
				float3 appendResult1606 = (float3(break1605.x , ( break1605.y + VertAdded1524 + UnderOffset1634 ) , break1605.z));
				float3 VertResDisplaced328 = appendResult1606;
				
				o.ase_texcoord3.z = customEye1169;
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertResDisplaced328;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = float3(0,1,0);
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
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessNum; float tessMin = _TesMin; float tessMax = _TesMax;
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
						, bool ase_vface : SV_IsFrontFace ) : SV_TARGET
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

				float2 appendResult892 = (float2(_Motion2.x , _Motion2.y));
				float2 Motion2XY1096 = appendResult892;
				float3 break35 = WorldPosition;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = WorldPosition;
				float4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner890 = ( 0.05 * _Time.y * Motion2XY1096 + WorldXZ1087);
				#ifdef GlobalPlanarReflection
				float staticSwitch1591 = _NormalPow;
				#else
				float staticSwitch1591 = ( _NormalPow * 0.7 );
				#endif
				float temp_output_1436_0 = ( _NormalPow * 6.0 );
				float switchResult1435 = (((ase_vface>0)?(staticSwitch1591):(temp_output_1436_0)));
				float3 unpack11 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals, sampler_Normals, (panner890*_Normals_ST.xy + _Normals_ST.zw) ), switchResult1435 );
				unpack11.z = lerp( 1, unpack11.z, saturate(switchResult1435) );
				float3 tex2DNode11 = unpack11;
				float2 appendResult88 = (float2(_Motion.z , _Motion.w));
				float2 MotionZW1085 = appendResult88;
				float2 panner85 = ( 0.1 * _Time.y * MotionZW1085 + WorldXZ1087);
				float switchResult1432 = (((ase_vface>0)?(( _NormalPow2 * 2.0 )):(( _NormalPow2 * 12.0 ))));
				float3 unpack84 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner85*_Normals2_ST.xy + _Normals2_ST.zw) ), switchResult1432 );
				unpack84.z = lerp( 1, unpack84.z, saturate(switchResult1432) );
				float2 panner1722 = ( -0.05 * _Time.y * MotionZW1085 + WorldXZ1087);
				float3 unpack1718 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner1722*( _Normals2_ST.xy * float2( 0.5,0.5 ) ) + _Normals2_ST.zw) ), switchResult1432 );
				unpack1718.z = lerp( 1, unpack1718.z, saturate(switchResult1432) );
				float3 temp_output_1719_0 = BlendNormal( unpack84 , unpack1718 );
				float3 temp_output_83_0 = BlendNormal( tex2DNode11 , temp_output_1719_0 );
				#ifdef _USEEXTRANORMALS_ON
				float3 staticSwitch1683 = temp_output_83_0;
				#else
				float3 staticSwitch1683 = temp_output_83_0;
				#endif
				float2 appendResult1049 = (float2(staticSwitch1683.xy));
				float2 RefNorm886 = appendResult1049;
				float RefBumpSlider1337 = _RefDistortion;
				float3 normalizeResult2273 = normalize( ( staticSwitch1683 + float3( ( RefNorm886 * RefBumpSlider1337 ) ,  0.0 ) ) );
				float2 appendResult2519 = (float2(normalizeResult2273.xy));
				float3 break19 = mul( _DepthMatrix, float4( ( WorldPosition - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D( _DepthCache, sampler_Linear_Clamp, CacheUV1109 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_6 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_7 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_6) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_7 - temp_cast_6)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 lerpResult2274 = lerp( float2( 0,0 ) , appendResult2519 , ( _WaterRefractonMobile * 0.5 * saturate( Foamness294 ) ));
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float customEye1169 = IN.ase_texcoord3.z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				float temp_output_1414_0 = ( ( 1.0 - InfinityFeather1410 ) * saturate( DistanceFadeA1294 ) );
				float2 lerpResult2275 = lerp( lerpResult2274 , float2( 0,0 ) , temp_output_1414_0);
				#ifdef _TRANSPARENTGRABPASS_ON
				float2 staticSwitch2542 = float2( 0,0 );
				#else
				float2 staticSwitch2542 = lerpResult2275;
				#endif
				float2 RefHackeryForColorCache2269 = staticSwitch2542;
				float2 temp_output_2198_0 = ( RefHackeryForColorCache2269 + CacheUV1109 );
				float4 tex2DNode657 = SAMPLE_TEXTURE2D( _ColorCache, sampler_Linear_Clamp, temp_output_2198_0 );
				float ColorCacheA2171 = tex2DNode657.a;
				float temp_output_12_0_g3214 = InfinityFeather1410;
				float CaptureDepthAOffsetShore1177 = ( ( ColorCacheA2171 * temp_output_12_0_g3214 ) + ( 1.0 - temp_output_12_0_g3214 ) );
				float4 break993 = _RemapAlpha;
				float switchResult2294 = (((ase_vface>0)?(saturate( (0.0 + (CaptureDepthAOffsetShore1177 - break993.x) * (1.0 - 0.0) / (break993.y - break993.x)) )):(1.0)));
				float ShoreMaskDepthAlpha141 = switchResult2294;
				float2 clipScreen639 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither639 = Dither8x8Bayer( fmod(clipScreen639.x, 8), fmod(clipScreen639.y, 8) );
				dither639 = step( dither639, ShoreMaskDepthAlpha141 );
				#ifdef _USEDITHERING_ON
				float staticSwitch1685 = dither639;
				#else
				float staticSwitch1685 = ShoreMaskDepthAlpha141;
				#endif
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch1748 = 1.0;
				#else
				float staticSwitch1748 = staticSwitch1685;
				#endif
				float HorizonAlpha2083 = temp_output_2080_0;
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2085 = HorizonAlpha2083;
				#else
				float staticSwitch2085 = staticSwitch1748;
				#endif
				float AlphaRes469 = staticSwitch2085;
				

				float Alpha = AlphaRes469;
				float AlphaClipThreshold = _ClipThreshold;
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

			#define ASE_TESSELLATION 1
			#pragma require tessellation tessHW
			#pragma hull HullFunction
			#pragma domain DomainFunction
			#define ASE_DISTANCE_TESSELLATION
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_TONEMAPPING 1
			#define _ASE_TINT 1
			#define _ASE_DEBUGVISUALS 1
			#define ASE_FOG 1
			#define _NORMAL_DROPOFF_TS 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define ASE_SRP_VERSION 120115
			#define REQUIRE_DEPTH_TEXTURE 1
			#define REQUIRE_OPAQUE_TEXTURE 1
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

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local ASE_TESSELLATION_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature_local LIGHTMAP_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USEINFINITYEDGE_ON
			#pragma shader_feature_local _USEVERTDISPLACEMENT_ON
			#pragma shader_feature_local _USEWAVENOISEDISPLACE_ON
			#pragma shader_feature_local _SPACE_WORLDSPACE _SPACE_UVSPACE
			#pragma shader_feature_local _HORIZONMAPPED_ON
			#pragma shader_feature_local _USEWAVEDIRECTION_ON
			#pragma shader_feature_local _USEINFINITYFEATHER_ON
			#pragma shader_feature_local _USEFOAM_ON
			#pragma shader_feature_local _USEFOAMEXTRASAMPLER_ON
			#pragma shader_feature_local _TRANSPARENTGRABPASS_ON
			#pragma shader_feature_local _USEEXTRANORMALS_ON
			#pragma multi_compile __ GlobalPlanarReflection
			#pragma shader_feature_local _USEGRADIENTFOG_ON
			#pragma shader_feature_local _RAINBOWGRABPASS_ON
			#pragma shader_feature_local _CAUSTICSENABLE_ON
			#pragma shader_feature_local _USECAUSTICEXTRASAMPLER_ON
			#pragma shader_feature_local _USECAUSTICRAINBOW_ON
			#pragma shader_feature_local _USEDITHERING_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_tangent : TANGENT;
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
			float4 _FoamColor;
			float4 _Motion;
			float4 _Normals_ST;
			float4 _WaterTint;
			float4 _RemapVertShore;
			float4 _WaterAlbedo;
			float4 _WaveDirectionSettings;
			float4 _Normals2_ST;
			float4 _Motion2;
			float4 _RefFresnel;
			float4 _CausticsTint;
			float4x4 _DepthMatrix;
			float4 _RemapAlpha;
			float4 _BakeLightMapAmbient;
			float4x4 _BakeMatrix;
			float4 _bakeLightmapST;
			float4 _RemapFoam;
			float3 _WorldDir;
			float3 _WorldPos;
			float3 _DepthPosition;
			float3 _CameraForward;
			float _WaveRefractionStrength;
			float _RefractionWarp;
			float _RemapWaterDistance;
			float _RemapWaterDistance1;
			float _Smoothness;
			float _WaterDepthPow;
			float _WaterSaturation;
			float _CubemapExposure;
			float _NormalDarken1;
			float _NormalDarken;
			float _ForceEye;
			float _ClipThreshold;
			float _WaterDepthPow1;
			int _ZTestMode;
			float _NormalPow2;
			float _FoamMaxDistance;
			float _ZWriteToggle;
			float _TessNum;
			float _TesMin;
			float _TesMax;
			float _WaveScale;
			float _FoamNoiseSpeed;
			float _FoamNoiseScale;
			float _FoamNoiseCon;
			float _FoamNoiseScale2;
			float _FoamNoiseCon2;
			float _DistanceFadeA;
			float _WaveSpeed;
			float _DistanceFadeB;
			float _VertDispMult;
			float _Tolerance;
			float _Feather;
			float _FoamScale_UseFoam;
			float _NormalPow;
			float _BakedLightingFeather;
			float _RefDistortion;
			float _WaterRefractonMobile;
			float _FoamPow;
			int _DebugVisual;
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

			float4 _skyGradientColor1;
			float4 _skyGradientColor2;
			float3 _CausticsDir;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_FoamLUT);
			TEXTURE2D(_DepthCache);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE2D(_WaveNoiseDisplace);
			SAMPLER(sampler_WaveNoiseDisplace);
			TEXTURE2D(_FoamTexture);
			SAMPLER(sampler_FoamTexture);
			TEXTURE2D(_ColorCache);
			TEXTURE2D(_Normals);
			SAMPLER(sampler_Normals);
			TEXTURE2D(_Normals2);
			SAMPLER(sampler_Normals2);
			TEXTURE2D(_DepthColorLUT);
			TEXTURECUBE(_Cubemap);
			float GlobalSkyRotation;
			SAMPLER(sampler_Cubemap);
			TEXTURE2D(_TexLeft);
			SAMPLER(sampler_Linear_Mirror);
			TEXTURE2D(_TexRight);
			float4x4 _CausticMatrix;
			float4 _CausticsSettings;


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
			
			float4 Billboard2078( float3 worldPosIN, float2 screenPos, float3 QuadNormal, float3 QuadPosition )
			{
				float3 worldPos = worldPosIN;
				float3 viewDir = worldPos - _WorldSpaceCameraPos;
				float3 reflectDir = -viewDir;
				float3 nReflDirection = normalize(reflectDir);
				float4 result = float4(0,0,0,0);
				// Ray-quad intersection
				half planeIntersectDistance = dot( worldPos - QuadPosition,  QuadNormal.xyz ) / dot( nReflDirection, QuadNormal.xyz );
				half3 intersectPosition = worldPos - nReflDirection * planeIntersectDistance;
				//half4 localPlaneIntersectPosition = mul( QuadInverseMatrix, float4( intersectPosition, 1.0 ));
				half4 localPlaneIntersectPosition = float4( intersectPosition, 1.0 );
				half2 billboardUV = half2( localPlaneIntersectPosition.x, localPlaneIntersectPosition.z );
				//billboardUV*=1;
				//float4 billboardCol = BillboardTex.Sample( BillboardSampler , billboardUV );
				float4 billboardCol = float4( billboardUV.x, billboardUV.y, 0, 1);
				float3 quadNorm = QuadNormal.xyz;
				half reflectDot = dot( nReflDirection, quadNorm );
				float w = 1;
				// Cull when beyond the quad bounds
				//w = billboardUV.x > 1.0 ? 0.0 : w;
				//w = billboardUV.y > 1.0 ? 0.0 : w;
				//w = billboardUV.x < 0.0 ? 0.0 : w;
				//w = billboardUV.y < 0.0 ? 0.0 : w;
				// Cull if distance is behind the surface
				//w = planeIntersectDistance < 0 ? 0 : w;
				// Cull if quad facing backwards
				w = reflectDot <= 0.0 ? 0.0 : w; 
				// Cull if quad facing backwards and culling is ticked on the quad ( passed in via the arrays W )
				//w = QuadNormal.w == 1.0 ? (reflectDot <= 0.0 ? 0.0 : w) : w;
				billboardCol = billboardCol * billboardCol.w * w;
				billboardCol.a = w;
				result*=1-w;
				result+= billboardCol;
				return result;
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			int IsStereoEyeLeft11_g3216( float3 WorldCamPos, float3 WorldCamRight, float force )
			{
				int Out = 0;	
				if(force >= 0){
						return force == 0;
					}
				#if defined(USING_STEREO_MATRICES)
					// Unity 5.4 has this new variable
					return (unity_StereoEyeIndex == 0);
				#elif defined (UNITY_DECLARE_MULTIVIEW)
					// OVR_multiview extension
					return (UNITY_VIEWID == 0);
				#else
					// NOTE: Bug #1165: _WorldSpaceCameraPos is not correct in multipass VR (when skybox is used) but UNITY_MATRIX_I_V seems to be
					#if defined(UNITY_MATRIX_I_V)
						float3 renderCameraPos = UNITY_MATRIX_I_V._m03_m13_m23;
					#else
						float3 renderCameraPos = _WorldSpaceCameraPos.xyz;
					#endif
					float fL = distance(WorldCamPos - WorldCamRight, renderCameraPos);
					float fR = distance(WorldCamPos + WorldCamRight, renderCameraPos);
					return (fL < fR);
				#endif
				return Out;
			}
			
			inline float3 ApplyTonemapper( float4 settings, float3 color )
			{
				return ApplyTonemap( color, settings );;
			}
			
			float3 MatrixMulThatWorks( float4 Pos, float4x4 Mat )
			{
				float3 result = mul(Mat,Pos.xyz);
				return result + float3(Mat[0][3],Mat[1][3],Mat[2][3]);
			}
			
			float CameraDepthTexture55_g3383( float2 uv )
			{
				float4 color = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, uv);
				return color.r;
			}
			
			float MyCustomExpression44_g3383( float rawDepth )
			{
				return LinearEyeDepth(rawDepth, _ZBufferParams);
			}
			
			float3 MyCustomExpression16_g3383( float3 view, float sceneZ )
			{
				return  _WorldSpaceCameraPos - view * sceneZ / dot(UNITY_MATRIX_I_V._13_23_33, view);
			}
			
			float4x4 Inverse4x4(float4x4 input)
			{
				#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
				float4x4 cofactors = float4x4(
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
			
			inline float Dither8x8Bayer( int x, int y )
			{
				const float dither[ 64 ] = {
			 1, 49, 13, 61,  4, 52, 16, 64,
			33, 17, 45, 29, 36, 20, 48, 32,
			 9, 57,  5, 53, 12, 60,  8, 56,
			41, 25, 37, 21, 44, 28, 40, 24,
			 3, 51, 15, 63,  2, 50, 14, 62,
			35, 19, 47, 31, 34, 18, 46, 30,
			11, 59,  7, 55, 10, 58,  6, 54,
			43, 27, 39, 23, 42, 26, 38, 22};
				int r = y * 8 + x;
				return dither[r] / 64; // same # of instructions as pre-dividing due to compiler magic
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float2 texCoord2052 = v.texcoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_2054_0 = distance( texCoord2052 , float2( 0.5,0.5 ) );
				float ifLocalVar2053 = 0;
				if( temp_output_2054_0 <= 1.0 )
				ifLocalVar2053 = 1.0;
				else
				ifLocalVar2053 = 5000.0;
				#ifdef _USEINFINITYEDGE_ON
				float4 staticSwitch1498 = ( v.vertex * ifLocalVar2053 );
				#else
				float4 staticSwitch1498 = v.vertex;
				#endif
				float4 break1605 = staticSwitch1498;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 break19 = mul( _DepthMatrix, float4( ( ase_worldPos - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = v.texcoord0.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D_LOD( _DepthCache, sampler_Linear_Clamp, CacheUV1109, 0.0 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_2 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_3 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_2) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_3 - temp_cast_2)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 temp_cast_4 = (_FoamNoiseSpeed).xx;
				float3 break35 = ase_worldPos;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = ase_worldPos;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner138 = ( 1.0 * _Time.y * temp_cast_4 + ( WorldXZ1087 * _FoamNoiseScale ));
				float lerpResult133 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( 0.04 * panner138 ), 0.0 ).r , _FoamNoiseCon);
				float2 temp_cast_6 = (_FoamNoiseSpeed).xx;
				float2 panner168 = ( 1.0 * _Time.y * temp_cast_6 + ( WorldXZ1087 * _FoamNoiseScale2 ));
				float lerpResult161 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( panner168 * 0.04 ), 0.0 ).r , _FoamNoiseCon2);
				float UnderOffset1634 = GlobalOceanOffset;
				float3 appendResult2127 = (float3(break1605.x , ( break1605.y + UnderOffset1634 ) , break1605.z));
				float3 VertResOffset2125 = appendResult2127;
				float3 customSurfaceDepth1169 = VertResOffset2125;
				float customEye1169 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1169)).z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				#ifdef _USEWAVENOISEDISPLACE_ON
				float staticSwitch680 = ( Foamness294 + ( ( lerpResult133 + lerpResult161 ) * ( ( 1.0 - DistanceFadeA1294 ) * 0.4 ) ) );
				#else
				float staticSwitch680 = Foamness294;
				#endif
				float DepthCacheRemap109 = staticSwitch680;
				float temp_output_5_0_g3212 = ( 1.0 - DepthCacheRemap109 );
				float mulTime209 = _TimeParameters.x * ( _WaveSpeed * 0.1 );
				float2 appendResult868 = (float2(_WaveDirectionSettings.x , _WaveDirectionSettings.y));
				float WaveCenterDistance1051 = distance( WorldXZ1087 , appendResult868 );
				#ifdef _USEWAVEDIRECTION_ON
				float staticSwitch762 = ( mulTime209 + ( ( WaveCenterDistance1051 * -0.01 ) / _WaveDirectionSettings.z ) );
				#else
				float staticSwitch762 = ( 0.0 - mulTime209 );
				#endif
				float temp_output_17_0_g3212 = staticSwitch762;
				float temp_output_30_0_g3212 = ( 1.0 - saturate( 0.7 ) );
				float temp_output_35_0_g3212 = ( temp_output_30_0_g3212 * 0.5 );
				float temp_output_32_0_g3212 = ( (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) - temp_output_35_0_g3212 );
				float temp_output_31_0_g3212 = ( temp_output_35_0_g3212 + (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) );
				float DepthCacheWave184 = ( 1.0 - frac( (0.0 + (temp_output_5_0_g3212 - temp_output_32_0_g3212) * (1.0 - 0.0) / (temp_output_31_0_g3212 - temp_output_32_0_g3212)) ) );
				float clampResult124 = clamp( DepthCacheWave184 , 0.01 , 0.99 );
				float2 temp_cast_7 = (clampResult124).xx;
				float4 tex2DNode76 = SAMPLE_TEXTURE2D_LOD( _FoamLUT, sampler_Linear_Clamp, temp_cast_7, 0.0 );
				float smoothstepResult279 = smoothstep( 0.0 , 1.0 , tex2DNode76.a);
				float3 customSurfaceDepth1325 = VertResOffset2125;
				float customEye1325 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1325)).z;
				float cameraDepthFade1325 = (( customEye1325 -_ProjectionParams.y - 0.0 ) / _DistanceFadeB);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2111 = 0.0;
				#else
				float staticSwitch2111 = saturate( cameraDepthFade1325 );
				#endif
				float DistanceFadeB1326 = staticSwitch2111;
				float lerpResult1311 = lerp( smoothstepResult279 , 0.0 , DistanceFadeB1326);
				float FoamVertDisp261 = saturate( lerpResult1311 );
				float temp_output_240_0 = ( FoamVertDisp261 * _VertDispMult * 0.1 );
				float ShoreMaskVert586 = saturate( (0.0 + (( CaptureB_Distancex100200 * 100.0 ) - _RemapVertShore.x) * (1.0 - 0.0) / (_RemapVertShore.y - _RemapVertShore.x)) );
				float VertDispDistMask22014 = ( 1.0 - saturate( (0.0 + (distance( WorldXZ1087 , (_WorldSpaceCameraPos).xz ) - 5.0) * (1.0 - 0.0) / (7.9 - 5.0)) ) );
				float3 customSurfaceDepth1484 = VertResOffset2125;
				float customEye1484 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1484)).z;
				float cameraDepthFade1484 = (( customEye1484 -_ProjectionParams.y - 0.0 ) / 1.0);
				float CamDepthFade1495 = cameraDepthFade1484;
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float VertDispMaskRes1691 = ( ShoreMaskVert586 * saturate( VertDispDistMask22014 ) * saturate( (0.0 + (CamDepthFade1495 - 0.2) * (1.0 - 0.0) / (1.5 - 0.2)) ) * InfinityFeather1410 );
				float temp_output_238_0 = ( temp_output_240_0 * VertDispMaskRes1691 );
				#ifdef _USEVERTDISPLACEMENT_ON
				float staticSwitch1688 = temp_output_238_0;
				#else
				float staticSwitch1688 = 0.0;
				#endif
				float VertAdded1524 = staticSwitch1688;
				float3 appendResult1606 = (float3(break1605.x , ( break1605.y + VertAdded1524 + UnderOffset1634 ) , break1605.z));
				float3 VertResDisplaced328 = appendResult1606;
				
				float3 vertexPos1850 = VertResDisplaced328;
				float4 ase_clipPos1850 = TransformObjectToHClip((vertexPos1850).xyz);
				float4 screenPos1850 = ComputeScreenPos(ase_clipPos1850);
				o.ase_texcoord5 = screenPos1850;
				o.ase_texcoord6 = screenPos;
				o.ase_texcoord4.z = customEye1169;
				
				o.ase_texcoord4.w = customEye1325;
				float3 customSurfaceDepth2817 = VertResOffset2125;
				float customEye2817 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth2817)).z;
				o.ase_texcoord7.x = customEye2817;
				o.ase_texcoord7.y = customEye1484;
				float3 customSurfaceDepth2418 = VertResDisplaced328;
				float customEye2418 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth2418)).z;
				o.ase_texcoord7.z = customEye2418;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord8.xyz = ase_worldNormal;
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord9.xyz = ase_worldTangent;
				float ase_vertexTangentSign = v.ase_tangent.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord10.xyz = ase_worldBitangent;
				float3 customSurfaceDepth2812 = VertResOffset2125;
				float customEye2812 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth2812)).z;
				o.ase_texcoord7.w = customEye2812;
				float3 _Vector3 = float3(0,0,-1);
				float4 Pos6_g3381 = float4( _Vector3 , 0.0 );
				float4x4 Mat6_g3381 = _CausticMatrix;
				float3 localMatrixMulThatWorks6_g3381 = MatrixMulThatWorks( Pos6_g3381 , Mat6_g3381 );
				float3 normalizeResult147_g3379 = normalize( localMatrixMulThatWorks6_g3381 );
				float3 vertexToFrag144_g3379 = normalizeResult147_g3379;
				o.ase_texcoord11.xyz = vertexToFrag144_g3379;
				float3 objToWorld3_g3383 = mul( GetObjectToWorldMatrix(), float4( v.vertex.xyz, 1 ) ).xyz;
				float3 vertexToFrag7_g3383 = objToWorld3_g3383;
				o.ase_texcoord12.xyz = vertexToFrag7_g3383;
				
				o.ase_texcoord4.xy = v.texcoord0.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.w = 0;
				o.ase_texcoord9.w = 0;
				o.ase_texcoord10.w = 0;
				o.ase_texcoord11.w = 0;
				o.ase_texcoord12.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertResDisplaced328;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = float3(0,1,0);

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
				float4 ase_tangent : TANGENT;

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
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessNum; float tessMin = _TesMin; float tessMax = _TesMax;
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

			half4 frag(VertexOutput IN , bool ase_vface : SV_IsFrontFace ) : SV_TARGET
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

				float3 break19 = mul( _DepthMatrix, float4( ( WorldPosition - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_2 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_3 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_2) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_3 - temp_cast_2)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float4 tex2DNode13 = SAMPLE_TEXTURE2D( _DepthCache, sampler_Linear_Clamp, CacheUV1109 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_4 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_5 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_4) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_5 - temp_cast_4)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_6 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_7 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_6) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_7 - temp_cast_6)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float CamNear1615 = saturate( (0.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - 0.1) * (1.0 - 0.0) / (0.6 - 0.1)) );
				float4 screenPos1850 = IN.ase_texcoord5;
				float4 ase_screenPosNorm1850 = screenPos1850 / screenPos1850.w;
				ase_screenPosNorm1850.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm1850.z : ase_screenPosNorm1850.z * 0.5 + 0.5;
				float screenDepth1850 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm1850.xy ),_ZBufferParams);
				float distanceDepth1850 = saturate( abs( ( screenDepth1850 - LinearEyeDepth( ase_screenPosNorm1850.z,_ZBufferParams ) ) / ( 0.2 ) ) );
				float DepthFader1847 = distanceDepth1850;
				float2 appendResult87 = (float2(_Motion.x , _Motion.y));
				float2 MotionXY1084 = appendResult87;
				float3 break35 = WorldPosition;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = WorldPosition;
				float4 screenPos = IN.ase_texcoord6;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner527 = ( -0.03 * _Time.y * MotionXY1084 + WorldXZ1087);
				float4 tex2DNode91 = SAMPLE_TEXTURE2D( _FoamTexture, sampler_FoamTexture, (panner527*( _FoamScale_UseFoam * 0.2 ) + 0.0) );
				float2 panner74 = ( 0.05 * _Time.y * MotionXY1084 + WorldXZ1087);
				#ifdef _USEFOAMEXTRASAMPLER_ON
				float staticSwitch1679 = ( SAMPLE_TEXTURE2D( _FoamTexture, sampler_FoamTexture, (panner74*_FoamScale_UseFoam + 0.0) ).r + tex2DNode91.r );
				#else
				float staticSwitch1679 = ( tex2DNode91.r * 1.8 );
				#endif
				float4 break984 = _RemapFoam;
				float temp_output_172_0 = saturate( (0.0 + (( 100.0 * CaptureB_Distancex100200 ) - break984.x) * (1.0 - 0.0) / (break984.y - break984.x)) );
				float2 appendResult892 = (float2(_Motion2.x , _Motion2.y));
				float2 Motion2XY1096 = appendResult892;
				float2 panner890 = ( 0.05 * _Time.y * Motion2XY1096 + WorldXZ1087);
				#ifdef GlobalPlanarReflection
				float staticSwitch1591 = _NormalPow;
				#else
				float staticSwitch1591 = ( _NormalPow * 0.7 );
				#endif
				float temp_output_1436_0 = ( _NormalPow * 6.0 );
				float switchResult1435 = (((ase_vface>0)?(staticSwitch1591):(temp_output_1436_0)));
				float3 unpack11 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals, sampler_Normals, (panner890*_Normals_ST.xy + _Normals_ST.zw) ), switchResult1435 );
				unpack11.z = lerp( 1, unpack11.z, saturate(switchResult1435) );
				float3 tex2DNode11 = unpack11;
				float2 appendResult88 = (float2(_Motion.z , _Motion.w));
				float2 MotionZW1085 = appendResult88;
				float2 panner85 = ( 0.1 * _Time.y * MotionZW1085 + WorldXZ1087);
				float switchResult1432 = (((ase_vface>0)?(( _NormalPow2 * 2.0 )):(( _NormalPow2 * 12.0 ))));
				float3 unpack84 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner85*_Normals2_ST.xy + _Normals2_ST.zw) ), switchResult1432 );
				unpack84.z = lerp( 1, unpack84.z, saturate(switchResult1432) );
				float2 panner1722 = ( -0.05 * _Time.y * MotionZW1085 + WorldXZ1087);
				float3 unpack1718 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner1722*( _Normals2_ST.xy * float2( 0.5,0.5 ) ) + _Normals2_ST.zw) ), switchResult1432 );
				unpack1718.z = lerp( 1, unpack1718.z, saturate(switchResult1432) );
				float3 temp_output_1719_0 = BlendNormal( unpack84 , unpack1718 );
				float3 temp_output_83_0 = BlendNormal( tex2DNode11 , temp_output_1719_0 );
				#ifdef _USEEXTRANORMALS_ON
				float3 staticSwitch1683 = temp_output_83_0;
				#else
				float3 staticSwitch1683 = temp_output_83_0;
				#endif
				float2 appendResult1049 = (float2(staticSwitch1683.xy));
				float2 RefNorm886 = appendResult1049;
				float RefBumpSlider1337 = _RefDistortion;
				float3 normalizeResult2273 = normalize( ( staticSwitch1683 + float3( ( RefNorm886 * RefBumpSlider1337 ) ,  0.0 ) ) );
				float2 appendResult2519 = (float2(normalizeResult2273.xy));
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 lerpResult2274 = lerp( float2( 0,0 ) , appendResult2519 , ( _WaterRefractonMobile * 0.5 * saturate( Foamness294 ) ));
				float customEye1169 = IN.ase_texcoord4.z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				float temp_output_1414_0 = ( ( 1.0 - InfinityFeather1410 ) * saturate( DistanceFadeA1294 ) );
				float2 lerpResult2275 = lerp( lerpResult2274 , float2( 0,0 ) , temp_output_1414_0);
				#ifdef _TRANSPARENTGRABPASS_ON
				float2 staticSwitch2542 = float2( 0,0 );
				#else
				float2 staticSwitch2542 = lerpResult2275;
				#endif
				float2 RefHackeryForColorCache2269 = staticSwitch2542;
				float2 temp_output_2198_0 = ( RefHackeryForColorCache2269 + CacheUV1109 );
				float4 tex2DNode657 = SAMPLE_TEXTURE2D( _ColorCache, sampler_Linear_Clamp, temp_output_2198_0 );
				float ColorCacheA2171 = tex2DNode657.a;
				float temp_output_12_0_g3214 = InfinityFeather1410;
				float CaptureDepthAOffsetShore1177 = ( ( ColorCacheA2171 * temp_output_12_0_g3214 ) + ( 1.0 - temp_output_12_0_g3214 ) );
				float4 break993 = _RemapAlpha;
				float switchResult2294 = (((ase_vface>0)?(saturate( (0.0 + (CaptureDepthAOffsetShore1177 - break993.x) * (1.0 - 0.0) / (break993.y - break993.x)) )):(1.0)));
				float ShoreMaskDepthAlpha141 = switchResult2294;
				float temp_output_199_0 = saturate( (0.0 + (( 5.0 * CaptureB_Distancex100200 ) - break984.z) * (1.0 - 0.0) / (( break984.w * _FoamMaxDistance ) - break984.z)) );
				float temp_output_981_0 = ( 1.0 - temp_output_199_0 );
				float temp_output_982_0 = ( temp_output_172_0 * ShoreMaskDepthAlpha141 * temp_output_981_0 );
				float FoamFeather2372 = temp_output_982_0;
				float2 temp_cast_12 = (_FoamNoiseSpeed).xx;
				float2 panner138 = ( 1.0 * _Time.y * temp_cast_12 + ( WorldXZ1087 * _FoamNoiseScale ));
				float lerpResult133 = lerp( 1.0 , SAMPLE_TEXTURE2D( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( 0.04 * panner138 ) ).r , _FoamNoiseCon);
				float2 temp_cast_13 = (_FoamNoiseSpeed).xx;
				float2 panner168 = ( 1.0 * _Time.y * temp_cast_13 + ( WorldXZ1087 * _FoamNoiseScale2 ));
				float lerpResult161 = lerp( 1.0 , SAMPLE_TEXTURE2D( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( panner168 * 0.04 ) ).r , _FoamNoiseCon2);
				#ifdef _USEWAVENOISEDISPLACE_ON
				float staticSwitch680 = ( Foamness294 + ( ( lerpResult133 + lerpResult161 ) * ( ( 1.0 - DistanceFadeA1294 ) * 0.4 ) ) );
				#else
				float staticSwitch680 = Foamness294;
				#endif
				float DepthCacheRemap109 = staticSwitch680;
				float temp_output_5_0_g3212 = ( 1.0 - DepthCacheRemap109 );
				float mulTime209 = _TimeParameters.x * ( _WaveSpeed * 0.1 );
				float2 appendResult868 = (float2(_WaveDirectionSettings.x , _WaveDirectionSettings.y));
				float WaveCenterDistance1051 = distance( WorldXZ1087 , appendResult868 );
				#ifdef _USEWAVEDIRECTION_ON
				float staticSwitch762 = ( mulTime209 + ( ( WaveCenterDistance1051 * -0.01 ) / _WaveDirectionSettings.z ) );
				#else
				float staticSwitch762 = ( 0.0 - mulTime209 );
				#endif
				float temp_output_17_0_g3212 = staticSwitch762;
				float temp_output_30_0_g3212 = ( 1.0 - saturate( 0.7 ) );
				float temp_output_35_0_g3212 = ( temp_output_30_0_g3212 * 0.5 );
				float temp_output_32_0_g3212 = ( (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) - temp_output_35_0_g3212 );
				float temp_output_31_0_g3212 = ( temp_output_35_0_g3212 + (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) );
				float DepthCacheWave184 = ( 1.0 - frac( (0.0 + (temp_output_5_0_g3212 - temp_output_32_0_g3212) * (1.0 - 0.0) / (temp_output_31_0_g3212 - temp_output_32_0_g3212)) ) );
				float clampResult124 = clamp( DepthCacheWave184 , 0.01 , 0.99 );
				float2 temp_cast_14 = (clampResult124).xx;
				float4 tex2DNode76 = SAMPLE_TEXTURE2D_LOD( _FoamLUT, sampler_Linear_Clamp, temp_cast_14, 0.0 );
				float FoamMaskResult122 = frac( ( FoamFeather2372 * tex2DNode76.r ) );
				float temp_output_142_0 = saturate( FoamMaskResult122 );
				float temp_output_39_0 = ( staticSwitch1679 * temp_output_142_0 );
				float3 temp_output_1099_0 = ( ( ( pow( temp_output_39_0 , _FoamPow ) * 0.5 ) + saturate( ( pow( temp_output_39_0 , 3.0 ) + ( ( saturate( pow( temp_output_142_0 , 4.0 ) ) - 0.5 ) * 0.3 ) ) ) ) * (_FoamColor).rgb * InfinityFeather1410 * 0.4 );
				#ifdef _USEFOAM_ON
				float3 staticSwitch1687 = temp_output_1099_0;
				#else
				float3 staticSwitch1687 = float3( 0,0,0 );
				#endif
				float3 AlbedoRes465 = staticSwitch1687;
				float3 temp_output_11_0_g3347 = AlbedoRes465;
				float temp_output_23_0_g3347 = GlobalOceanUnder;
				float3 ifLocalVar4_g3347 = 0;
				UNITY_BRANCH 
				if( temp_output_23_0_g3347 >= 1.0 )
				ifLocalVar4_g3347 = temp_output_11_0_g3347;
				else
				ifLocalVar4_g3347 = temp_output_11_0_g3347;
				#ifdef _USEGRADIENTFOG_ON
				float3 staticSwitch6_g3347 = ifLocalVar4_g3347;
				#else
				float3 staticSwitch6_g3347 = temp_output_11_0_g3347;
				#endif
				#ifdef _USEUNDERWATER
				float3 staticSwitch214_g3347 = staticSwitch6_g3347;
				#else
				float3 staticSwitch214_g3347 = temp_output_11_0_g3347;
				#endif
				float3 temp_output_3047_0 = staticSwitch214_g3347;
				float temp_output_2951_0 = distance( (WorldPosition).xz , (_WorldSpaceCameraPos).xz );
				float temp_output_2955_0 = saturate( (1.0 + (temp_output_2951_0 - 100.0) * (0.0 - 1.0) / (8000.0 - 100.0)) );
				float temp_output_2949_0 = pow( temp_output_2955_0 , 8.0 );
				float4 temp_output_2892_0 = saturate( ( float4( ( InfinityFeather1410 * CamNear1615 * DepthFader1847 * temp_output_3047_0 * ShoreMaskDepthAlpha141 ) , 0.0 ) + ( _WaterAlbedo * temp_output_2949_0 * CamNear1615 * DepthFader1847 * ShoreMaskDepthAlpha141 ) ) );
				float4 AlbedoRes22337 = temp_output_2892_0;
				
				float3 appendResult1062 = (float3(tex2DNode657.rgb));
				float3 ColorCacheRGB1061 = ( InfinityFeather1410 * appendResult1062 );
				float temp_output_2998_0 = saturate( ( ( -0.2 * min( -CaptureDepthAOffsetShore1177 , 0.0 ) ) / _RemapWaterDistance1 ) );
				float2 appendResult2445 = (float2(ase_screenPosNorm.xy));
				float2 appendResult2856 = (float2(temp_output_1719_0.xy));
				float SeaAlpha2FoamAndNormal205 = InfinityFeather1410;
				float smoothstepResult279 = smoothstep( 0.0 , 1.0 , tex2DNode76.a);
				float customEye1325 = IN.ase_texcoord4.w;
				float cameraDepthFade1325 = (( customEye1325 -_ProjectionParams.y - 0.0 ) / _DistanceFadeB);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2111 = 0.0;
				#else
				float staticSwitch2111 = saturate( cameraDepthFade1325 );
				#endif
				float DistanceFadeB1326 = staticSwitch2111;
				float lerpResult1311 = lerp( smoothstepResult279 , 0.0 , DistanceFadeB1326);
				float FoamVertDisp261 = saturate( lerpResult1311 );
				float temp_output_1332_0 = ( RefBumpSlider1337 * SeaAlpha2FoamAndNormal205 * (1.0 + (FoamVertDisp261 - 0.0) * (5.0 - 1.0) / (1.0 - 0.0)) * 22.0 );
				float3 normalizeResult2866 = normalize( ( temp_output_1719_0 + float3( ( appendResult2856 * temp_output_1332_0 ) ,  0.0 ) ) );
				float3 lerpResult2865 = lerp( normalizeResult2866 , float3( 0,0,1 ) , float3( 0.5,0.5,0.5 ));
				float switchResult1438 = (((ase_vface>0)?(FoamVertDisp261):(( FoamVertDisp261 * 0.8 ))));
				float customEye2817 = IN.ase_texcoord7.x;
				float cameraDepthFade2817 = (( customEye2817 -_ProjectionParams.y - 0.0 ) / 300.0);
				float3 appendResult2798 = (float3(0.0 , ( saturate( switchResult1438 ) * 0.1 * _WaveRefractionStrength * ( 1.0 - saturate( cameraDepthFade2817 ) ) ) , 0.0));
				float3 lerpResult2861 = lerp( ( lerpResult2865 + appendResult2798 ) , float3( 0,0,1 ) , temp_output_1414_0);
				float UnderOffset1634 = GlobalOceanOffset;
				float GlobalOceanHeight1552 = ( GlobalOceanHeight + UnderOffset1634 );
				float temp_output_240_0 = ( FoamVertDisp261 * _VertDispMult * 0.1 );
				float ShoreMaskVert586 = saturate( (0.0 + (( CaptureB_Distancex100200 * 100.0 ) - _RemapVertShore.x) * (1.0 - 0.0) / (_RemapVertShore.y - _RemapVertShore.x)) );
				float VertDispDistMask22014 = ( 1.0 - saturate( (0.0 + (distance( WorldXZ1087 , (_WorldSpaceCameraPos).xz ) - 5.0) * (1.0 - 0.0) / (7.9 - 5.0)) ) );
				float customEye1484 = IN.ase_texcoord7.y;
				float cameraDepthFade1484 = (( customEye1484 -_ProjectionParams.y - 0.0 ) / 1.0);
				float CamDepthFade1495 = cameraDepthFade1484;
				float VertDispMaskRes1691 = ( ShoreMaskVert586 * saturate( VertDispDistMask22014 ) * saturate( (0.0 + (CamDepthFade1495 - 0.2) * (1.0 - 0.0) / (1.5 - 0.2)) ) * InfinityFeather1410 );
				float temp_output_238_0 = ( temp_output_240_0 * VertDispMaskRes1691 );
				#ifdef _USEVERTDISPLACEMENT_ON
				float staticSwitch1688 = temp_output_238_0;
				#else
				float staticSwitch1688 = 0.0;
				#endif
				float VertAdded1524 = staticSwitch1688;
				float temp_output_1537_0 = ( ( GlobalOceanHeight1552 + (VertAdded1524).x ) - ( _WorldSpaceCameraPos.y - 0.2 ) );
				float temp_output_1571_0 = saturate( ( temp_output_1537_0 * 5.0 ) );
				float RefractHack1578 = temp_output_1571_0;
				float temp_output_1582_0 = (1.0 + (RefractHack1578 - 0.0) * (11.0 - 1.0) / (1.0 - 0.0));
				float3 lerpResult2862 = lerp( float3( 0,0,1 ) , lerpResult2861 , temp_output_1582_0);
				float3 switchResult2863 = (((ase_vface>0)?(lerpResult2862):(lerpResult2861)));
				float3 NormalResRefracted2803 = switchResult2863;
				float3 NormalResBelow1480 = staticSwitch1683;
				float3 switchResult1755 = (((ase_vface>0)?(NormalResRefracted2803):(NormalResBelow1480)));
				float2 appendResult3029 = (float2(switchResult1755.xy));
				float customEye2418 = IN.ase_texcoord7.z;
				float cameraDepthFade2418 = (( customEye2418 -_ProjectionParams.y - 0.0 ) / 40.0);
				float temp_output_2417_0 = ( 1.0 - saturate( cameraDepthFade2418 ) );
				float2 temp_output_2389_0 = ( appendResult2445 + ( appendResult3029 * DepthFader1847 * CamNear1615 * ShoreMaskDepthAlpha141 * temp_output_2417_0 ) );
				float eyeDepth2780 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( temp_output_2389_0, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float2 lerpResult2452 = lerp( appendResult2445 , temp_output_2389_0 , ( _RefractionWarp * step( ( screenPos.w - eyeDepth2780 ) , 0.0 ) ));
				float2 ScreenSpaceRefractUV2387 = lerpResult2452;
				float eyeDepth6_g3371 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV2387, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3371 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord8.xyz;
				float fresnelNdotV2830 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode2830 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV2830, 5.0 ) );
				float lerpResult2832 = lerp( 9.0 , 30.0 , saturate( fresnelNode2830 ));
				float DepthFadeDistthing2939 = lerpResult2832;
				float temp_output_12_0_g3371 = ( DepthFadeDistthing2939 * _RemapWaterDistance );
				float temp_output_3_0_g3371 = ( abs( ( eyeDepth6_g3371 - -worldToView21_g3371.z ) ) / temp_output_12_0_g3371 );
				float temp_output_2907_0 = saturate( temp_output_3_0_g3371 );
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch3001 = temp_output_2907_0;
				#else
				float staticSwitch3001 = temp_output_2998_0;
				#endif
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch2993 = _WaterDepthPow;
				#else
				float staticSwitch2993 = _WaterDepthPow1;
				#endif
				float temp_output_2909_0 = ( 1.0 - pow( ( 1.0 - temp_output_2907_0 ) , 6.0 ) );
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch2990 = temp_output_2909_0;
				#else
				float staticSwitch2990 = ( 1.0 - pow( ( 1.0 - temp_output_2998_0 ) , 6.0 ) );
				#endif
				float2 appendResult2183 = (float2(staticSwitch2990 , 0.0));
				float3 appendResult2142 = (float3(SAMPLE_TEXTURE2D_LOD( _DepthColorLUT, sampler_Linear_Clamp, appendResult2183, 0.0 ).rgb));
				float3 appendResult2828 = (float3(_WaterTint.rgb));
				float3 temp_output_1964_0 = ( ( saturate( pow( ( 1.0 - staticSwitch3001 ) , staticSwitch2993 ) ) * appendResult2142 ) * appendResult2828 * InfinityFeather1410 );
				float3 desaturateInitialColor2848 = temp_output_1964_0;
				float desaturateDot2848 = dot( desaturateInitialColor2848, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar2848 = lerp( desaturateInitialColor2848, desaturateDot2848.xxx, ( 1.0 - _WaterSaturation ) );
				float3 switchResult2135 = (((ase_vface>0)?(desaturateVar2848):(float3( 0,0,0 ))));
				float3 WaterDepthTint342 = switchResult2135;
				float switchResult2482 = (((ase_vface>0)?(saturate( (0.0 + (CaptureDepthAOffsetShore1177 - break993.z) * (1.0 - 0.0) / (break993.w - break993.z)) )):(1.0)));
				float ShoreMaskSpec2479 = switchResult2482;
				float3 lerpResult3187 = lerp( float3( 1,1,1 ) , WaterDepthTint342 , ShoreMaskSpec2479);
				float switchResult1429 = (((ase_vface>0)?(radians( GlobalSkyRotation )):(radians( ( 0.0 + 180.0 ) ))));
				float3 switchResult1425 = (((ase_vface>0)?(-ase_worldViewDir):(ase_worldViewDir)));
				float3 normalizeResult257 = normalize( ( staticSwitch1683 + float3( ( RefNorm886 * temp_output_1332_0 ) ,  0.0 ) ) );
				float3 lerpResult1261 = lerp( normalizeResult257 , float3( 0,0,1 ) , float3( 0.5,0.5,0.5 ));
				float3 lerpResult1264 = lerp( ( lerpResult1261 + appendResult2798 ) , float3( 0,0,1 ) , temp_output_1414_0);
				float3 lerpResult1581 = lerp( float3( 0,0,1 ) , lerpResult1264 , temp_output_1582_0);
				float3 switchResult2804 = (((ase_vface>0)?(lerpResult1581):(lerpResult1264)));
				float3 NormalRes466 = switchResult2804;
				float HorizonMask2300 = saturate( (1.0 + (distance( WorldPosition , _WorldSpaceCameraPos ) - 200.0) * (0.0 - 1.0) / (3000.0 - 200.0)) );
				float3 lerpResult1488 = lerp( float3( 0,0,1 ) , NormalRes466 , saturate( ( CamNear1615 * CamDepthFade1495 * HorizonMask2300 ) ));
				float3 ase_worldTangent = IN.ase_texcoord9.xyz;
				float3 ase_worldBitangent = IN.ase_texcoord10.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal1153 = lerpResult1488;
				float3 worldNormal1153 = float3(dot(tanToWorld0,tanNormal1153), dot(tanToWorld1,tanNormal1153), dot(tanToWorld2,tanNormal1153));
				float3 temp_output_1151_0 = reflect( switchResult1425 , worldNormal1153 );
				float3 rotatedValue1157 = RotateAroundAxis( float3( 0,0,0 ), temp_output_1151_0, float3( 0,1,0 ), switchResult1429 );
				float SmoothnessRes468 = _Smoothness;
				float lerpResult2215 = lerp( 1.0 , SmoothnessRes468 , HorizonMask2300);
				float temp_output_1147_0 = ( ( 1.0 - lerpResult2215 ) * 8.0 );
				float clampResult1494 = clamp( (0.0 + (distance( GlobalOceanHeight1552 , _WorldSpaceCameraPos.y ) - 0.0) * (0.7 - 0.0) / (1.0 - 0.0)) , 0.0 , 0.7 );
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch3051 = 2.0;
				#else
				float staticSwitch3051 = clampResult1494;
				#endif
				float switchResult1466 = (((ase_vface>0)?(temp_output_1147_0):(staticSwitch3051)));
				float3 temp_output_1550_0 = (SAMPLE_TEXTURECUBE_LOD( _Cubemap, sampler_Cubemap, rotatedValue1157, switchResult1466 )).rgb;
				float3 normalizeResult1285 = normalize( ( tex2DNode11 * float3( 55,55,1 ) ) );
				float3 tanNormal1237 = normalizeResult1285;
				float3 worldNormal1237 = float3(dot(tanToWorld0,tanNormal1237), dot(tanToWorld1,tanNormal1237), dot(tanToWorld2,tanNormal1237));
				float dotResult1229 = dot( worldNormal1237 , ase_worldViewDir );
				float customEye2812 = IN.ase_texcoord7.w;
				float cameraDepthFade2812 = (( customEye2812 -_ProjectionParams.y - 0.0 ) / _NormalDarken1);
				float lerpResult1280 = lerp( 1.0 , ( 1.0 - saturate( dotResult1229 ) ) , ( ( 1.0 - saturate( cameraDepthFade2812 ) ) * _NormalDarken ));
				float NormDarken1230 = lerpResult1280;
				float3 tanNormal901 = NormalRes466;
				float3 worldNormal901 = float3(dot(tanToWorld0,tanNormal901), dot(tanToWorld1,tanNormal901), dot(tanToWorld2,tanNormal901));
				#ifdef _HORIZONMAPPED_ON
				float3 staticSwitch2104 = float3(0,1,0);
				#else
				float3 staticSwitch2104 = worldNormal901;
				#endif
				float fresnelNdotV899 = dot( staticSwitch2104, ase_worldViewDir );
				float fresnelNode899 = ( _RefFresnel.x + _RefFresnel.y * pow( 1.0 - fresnelNdotV899, _RefFresnel.z ) );
				float temp_output_904_0 = saturate( fresnelNode899 );
				float3 temp_output_1162_0 = ( temp_output_1550_0 * _CubemapExposure * (0.4 + (NormDarken1230 - 0.0) * (1.0 - 0.4) / (1.0 - 0.0)) * temp_output_904_0 * ShoreMaskSpec2479 );
				float3 WorldCamPos11_g3216 = _WorldPos;
				float3 WorldCamRight11_g3216 = _WorldDir;
				float force11_g3216 = _ForceEye;
				int localIsStereoEyeLeft11_g3216 = IsStereoEyeLeft11_g3216( WorldCamPos11_g3216 , WorldCamRight11_g3216 , force11_g3216 );
				float2 appendResult4_g3216 = (float2(ase_screenPosNorm.xy));
				float2 appendResult1245 = (float2(NormalRes466.xy));
				float clampResult1520 = clamp( (VertAdded1524).x , 0.0 , 0.3 );
				float2 temp_output_1_0_g3216 = ( appendResult4_g3216 + ( appendResult1245 * (0.0 + (clampResult1520 - 0.1) * (2.0 - 0.0) / (0.3 - 0.1)) * CamNear1615 * 0.5 ) );
				float4 tex2DNode3_g3216 = SAMPLE_TEXTURE2D( _TexLeft, sampler_Linear_Mirror, temp_output_1_0_g3216 );
				float4 tex2DNode2_g3216 = SAMPLE_TEXTURE2D( _TexRight, sampler_Linear_Mirror, temp_output_1_0_g3216 );
				float4 ifLocalVar10_g3216 = 0;
				if( localIsStereoEyeLeft11_g3216 <= 0.0 )
				ifLocalVar10_g3216 = tex2DNode2_g3216;
				else
				ifLocalVar10_g3216 = tex2DNode3_g3216;
				float3 temp_output_1549_0 = (ifLocalVar10_g3216).rgb;
				float3 UnderReflection1610 = ( temp_output_1549_0 * temp_output_904_0 );
				float3 switchResult1453 = (((ase_vface>0)?(( UnderReflection1610 * (0.4 + (NormDarken1230 - 0.0) * (1.0 - 0.4) / (1.0 - 0.0)) )):(temp_output_1162_0)));
				#ifdef GlobalPlanarReflection
				float3 staticSwitch945 = switchResult1453;
				#else
				float3 staticSwitch945 = temp_output_1162_0;
				#endif
				float4 appendResult11_g3229 = (float4(_TonemappingSettings.x , _TonemappingSettings.y , _TonemappingSettings.z , _TonemappingSettings.w));
				float4 temp_output_2533_0 = appendResult11_g3229;
				float4 settings2527 = temp_output_2533_0;
				float3 color2527 = staticSwitch945;
				float3 localApplyTonemapper2527 = ApplyTonemapper( settings2527 , color2527 );
				#ifdef _TRANSPARENTGRABPASS_ON
				float3 staticSwitch2528 = localApplyTonemapper2527;
				#else
				float3 staticSwitch2528 = staticSwitch945;
				#endif
				float4 fetchOpaqueVal1753 = float4( SHADERGRAPH_SAMPLE_SCENE_COLOR( ScreenSpaceRefractUV2387 ), 1.0 );
				float3 appendResult1754 = (float3(fetchOpaqueVal1753.rgb));
				#ifdef _RAINBOWGRABPASS_ON
				float3 staticSwitch2395 = appendResult1754;
				#else
				float3 staticSwitch2395 = appendResult1754;
				#endif
				float3 temp_output_18_0_g3379 = float3( 0,0,0 );
				float3 temp_output_30_0_g3379 = float3( 0,0,1 );
				float3 tanNormal12_g3379 = temp_output_30_0_g3379;
				float3 worldNormal12_g3379 = float3(dot(tanToWorld0,tanNormal12_g3379), dot(tanToWorld1,tanNormal12_g3379), dot(tanToWorld2,tanNormal12_g3379));
				float3 vertexToFrag144_g3379 = IN.ase_texcoord11.xyz;
				float dotResult1_g3379 = dot( worldNormal12_g3379 , vertexToFrag144_g3379 );
				float3 vertexToFrag7_g3383 = IN.ase_texcoord12.xyz;
				float3 positionWS_fogFactor24_g3383 = vertexToFrag7_g3383;
				float3 normalizeResult21_g3383 = normalize( ( _WorldSpaceCameraPos - positionWS_fogFactor24_g3383 ) );
				float3 view23_g3383 = normalizeResult21_g3383;
				float3 view16_g3383 = view23_g3383;
				float3 appendResult49_g3383 = (float3(ScreenSpaceRefractUV2387 , ase_screenPosNorm.w));
				float3 screenPosXYW29_g3383 = (appendResult49_g3383).xyz;
				float2 uvDepth38_g3383 = ( (screenPosXYW29_g3383).xy / (screenPosXYW29_g3383).z );
				float2 uv55_g3383 = uvDepth38_g3383;
				float localCameraDepthTexture55_g3383 = CameraDepthTexture55_g3383( uv55_g3383 );
				float rawDepth40_g3383 = localCameraDepthTexture55_g3383;
				float rawDepth44_g3383 = rawDepth40_g3383;
				float localMyCustomExpression44_g3383 = MyCustomExpression44_g3383( rawDepth44_g3383 );
				float sceneZ41_g3383 = localMyCustomExpression44_g3383;
				float sceneZ16_g3383 = sceneZ41_g3383;
				float3 localMyCustomExpression16_g3383 = MyCustomExpression16_g3383( view16_g3383 , sceneZ16_g3383 );
				float3 scenePos11_g3383 = localMyCustomExpression16_g3383;
				float3 WorldPosition2704 = scenePos11_g3383;
				float3 temp_output_105_0_g3379 = WorldPosition2704;
				float2 PanSpeed131_g3379 = (_CausticsSettings).xy;
				float mulTime28_g3379 = _TimeParameters.x * -1.0;
				float2 break101_g3379 = ( PanSpeed131_g3379 * mulTime28_g3379 );
				float3 appendResult100_g3379 = (float3(break101_g3379.x , 0.0 , break101_g3379.y));
				float4 Pos6_g3382 = float4( ( temp_output_105_0_g3379 + appendResult100_g3379 ) , 0.0 );
				float4x4 invertVal146_g3379 = Inverse4x4( _CausticMatrix );
				float4x4 Mat6_g3382 = invertVal146_g3379;
				float3 localMatrixMulThatWorks6_g3382 = MatrixMulThatWorks( Pos6_g3382 , Mat6_g3382 );
				float2 temp_output_87_0_g3379 = ( (localMatrixMulThatWorks6_g3382).xy + ( 0.0045 * 2.0 ) );
				float temp_output_117_0_g3379 = (temp_output_105_0_g3379).y;
				float temp_output_63_0_g3379 = GlobalOceanHeight1552;
				float2 DistanceFade134_g3379 = (_CausticsSettings).zw;
				float2 break136_g3379 = DistanceFade134_g3379;
				float temp_output_67_0_g3379 = ( saturate( (0.0 + (max( -( temp_output_117_0_g3379 - temp_output_63_0_g3379 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) * saturate( (1.0 + (distance( temp_output_63_0_g3379 , temp_output_117_0_g3379 ) - break136_g3379.x) * (0.0 - 1.0) / (break136_g3379.y - break136_g3379.x)) ) );
				float CausticMipLevel118_g3379 = ( ( 1.0 - temp_output_67_0_g3379 ) * 4.0 );
				float2 temp_output_75_0_g3379 = ( (localMatrixMulThatWorks6_g3382).xy + 0.0045 );
				#ifdef _USECAUSTICRAINBOW_ON
				float3 staticSwitch73_g3379 = ( ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Caustics, temp_output_87_0_g3379, CausticMipLevel118_g3379 ).r * float3(0,0,1) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Caustics, temp_output_75_0_g3379, CausticMipLevel118_g3379 ).r * float3(0,1,0) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3382).xy, CausticMipLevel118_g3379 ).r * float3(1,0,0) ) );
				#else
				float3 staticSwitch73_g3379 = float3( 0,0,0 );
				#endif
				#ifdef _USECAUSTICEXTRASAMPLER_ON
				float3 staticSwitch57_g3379 = staticSwitch73_g3379;
				#else
				float3 staticSwitch57_g3379 = float3( 0,0,0 );
				#endif
				float3 appendResult62_g3379 = (float3(_CausticsColor.rgb));
				#ifdef _CAUSTICSENABLE_ON
				float3 staticSwitch16_g3379 = ( temp_output_18_0_g3379 + ( max( dotResult1_g3379 , 0.0 ) * staticSwitch57_g3379 * appendResult62_g3379 * temp_output_67_0_g3379 ) );
				#else
				float3 staticSwitch16_g3379 = temp_output_18_0_g3379;
				#endif
				float3 appendResult3040 = (float3(_CausticsTint.rgb));
				float3 temp_output_3042_0 = ( staticSwitch16_g3379 * appendResult3040 * 8.0 );
				float3 switchResult3046 = (((ase_vface>0)?(temp_output_3042_0):(float3( 0,0,0 ))));
				float3 Caustics2711 = switchResult3046;
				float3 GrabPassOG1994 = ( staticSwitch2395 + ( staticSwitch2395 * Caustics2711 ) );
				float3 temp_cast_33 = (1.0).xxx;
				float eyeDepth6_g3373 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV2387, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3373 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float temp_output_12_0_g3373 = 1.0;
				float temp_output_3_0_g3373 = ( abs( ( eyeDepth6_g3373 - -worldToView21_g3373.z ) ) / temp_output_12_0_g3373 );
				float temp_output_1843_0 = saturate( pow( ( 1.0 - saturate( temp_output_3_0_g3373 ) ) , 6.0 ) );
				float ShallowMask2838 = ( ( 1.0 - temp_output_1843_0 ) * ShoreMaskDepthAlpha141 );
				float3 lerpResult2839 = lerp( temp_cast_33 , WaterDepthTint342 , ShallowMask2838);
				float3 switchResult3072 = (((ase_vface>0)?(lerpResult2839):(float3( 1,1,1 ))));
				float switchResult1513 = (((ase_vface>0)?(1.0):(CamNear1615)));
				float3 lerpResult1900 = lerp( GrabPassOG1994 , ( switchResult3072 * GrabPassOG1994 ) , switchResult1513);
				float3 GrabPassTinted1768 = lerpResult1900;
				float switchResult1945 = (((ase_vface>0)?(1.0):(CamNear1615)));
				float3 lerpResult1746 = lerp( GrabPassTinted1768 , ( staticSwitch2528 + GrabPassTinted1768 ) , ( ShoreMaskDepthAlpha141 * DepthFader1847 * switchResult1945 ));
				float3 NormalResBelow23057 = temp_output_1719_0;
				float3 tanNormal1468 = NormalResBelow23057;
				float3 worldNormal1468 = float3(dot(tanToWorld0,tanNormal1468), dot(tanToWorld1,tanNormal1468), dot(tanToWorld2,tanNormal1468));
				#ifdef _HORIZONMAPPED_ON
				float3 staticSwitch2108 = float3(0,1,0);
				#else
				float3 staticSwitch2108 = worldNormal1468;
				#endif
				float clampResult3065 = clamp( (-4.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - 0.1) * (-0.5 - -4.0) / (10.0 - 0.1)) , -4.0 , -0.5 );
				float fresnelNdotV1454 = dot( normalize( staticSwitch2108 ), -ase_worldViewDir );
				float fresnelNode1454 = ( clampResult3065 + 4.0 * pow( max( 1.0 - fresnelNdotV1454 , 0.0001 ), 1.0 ) );
				float temp_output_1460_0 = saturate( fresnelNode1454 );
				float3 lerpResult1923 = lerp( lerpResult1746 , staticSwitch2528 , ( temp_output_1460_0 * CamNear1615 ));
				float3 switchResult1925 = (((ase_vface>0)?(lerpResult1746):(lerpResult1923)));
				#ifdef _TRANSPARENTGRABPASS_ON
				float3 staticSwitch1588 = switchResult1925;
				#else
				float3 staticSwitch1588 = ( ( ColorCacheRGB1061 * lerpResult3187 ) + ( saturate( (0.0 + (ShoreMaskSpec2479 - 0.3) * (1.0 - 0.0) / (1.0 - 0.3)) ) * staticSwitch2528 ) );
				#endif
				float3 appendResult2329 = (float3(staticSwitch1588));
				float3 EmissionRes467 = appendResult2329;
				float3 EmissionRes22320 = EmissionRes467;
				
				float2 clipScreen639 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither639 = Dither8x8Bayer( fmod(clipScreen639.x, 8), fmod(clipScreen639.y, 8) );
				dither639 = step( dither639, ShoreMaskDepthAlpha141 );
				#ifdef _USEDITHERING_ON
				float staticSwitch1685 = dither639;
				#else
				float staticSwitch1685 = ShoreMaskDepthAlpha141;
				#endif
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch1748 = 1.0;
				#else
				float staticSwitch1748 = staticSwitch1685;
				#endif
				float HorizonAlpha2083 = temp_output_2080_0;
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2085 = HorizonAlpha2083;
				#else
				float staticSwitch2085 = staticSwitch1748;
				#endif
				float AlphaRes469 = staticSwitch2085;
				

				float3 BaseColor = AlbedoRes22337.rgb;
				float3 Emission = EmissionRes22320;
				float Alpha = AlphaRes469;
				float AlphaClipThreshold = _ClipThreshold;

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

			#pragma multi_compile_instancing
			#define ASE_TESSELLATION 1
			#pragma require tessellation tessHW
			#pragma hull HullFunction
			#pragma domain DomainFunction
			#define ASE_DISTANCE_TESSELLATION
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_TONEMAPPING 1
			#define _ASE_TINT 1
			#define _ASE_DEBUGVISUALS 1
			#define ASE_FOG 1
			#define _NORMAL_DROPOFF_TS 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
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

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#pragma shader_feature_local ASE_TESSELLATION_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature_local LIGHTMAP_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USEINFINITYEDGE_ON
			#pragma shader_feature_local _USEVERTDISPLACEMENT_ON
			#pragma shader_feature_local _USEWAVENOISEDISPLACE_ON
			#pragma shader_feature_local _SPACE_WORLDSPACE _SPACE_UVSPACE
			#pragma shader_feature_local _HORIZONMAPPED_ON
			#pragma shader_feature_local _USEWAVEDIRECTION_ON
			#pragma shader_feature_local _USEINFINITYFEATHER_ON
			#pragma shader_feature_local _TRANSPARENTGRABPASS_ON
			#pragma shader_feature_local _USEDITHERING_ON
			#pragma shader_feature_local _USEEXTRANORMALS_ON
			#pragma multi_compile __ GlobalPlanarReflection


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
				float4 ase_texcoord : TEXCOORD0;
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _FoamColor;
			float4 _Motion;
			float4 _Normals_ST;
			float4 _WaterTint;
			float4 _RemapVertShore;
			float4 _WaterAlbedo;
			float4 _WaveDirectionSettings;
			float4 _Normals2_ST;
			float4 _Motion2;
			float4 _RefFresnel;
			float4 _CausticsTint;
			float4x4 _DepthMatrix;
			float4 _RemapAlpha;
			float4 _BakeLightMapAmbient;
			float4x4 _BakeMatrix;
			float4 _bakeLightmapST;
			float4 _RemapFoam;
			float3 _WorldDir;
			float3 _WorldPos;
			float3 _DepthPosition;
			float3 _CameraForward;
			float _WaveRefractionStrength;
			float _RefractionWarp;
			float _RemapWaterDistance;
			float _RemapWaterDistance1;
			float _Smoothness;
			float _WaterDepthPow;
			float _WaterSaturation;
			float _CubemapExposure;
			float _NormalDarken1;
			float _NormalDarken;
			float _ForceEye;
			float _ClipThreshold;
			float _WaterDepthPow1;
			int _ZTestMode;
			float _NormalPow2;
			float _FoamMaxDistance;
			float _ZWriteToggle;
			float _TessNum;
			float _TesMin;
			float _TesMax;
			float _WaveScale;
			float _FoamNoiseSpeed;
			float _FoamNoiseScale;
			float _FoamNoiseCon;
			float _FoamNoiseScale2;
			float _FoamNoiseCon2;
			float _DistanceFadeA;
			float _WaveSpeed;
			float _DistanceFadeB;
			float _VertDispMult;
			float _Tolerance;
			float _Feather;
			float _FoamScale_UseFoam;
			float _NormalPow;
			float _BakedLightingFeather;
			float _RefDistortion;
			float _WaterRefractonMobile;
			float _FoamPow;
			int _DebugVisual;
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

			float4 _skyGradientColor1;
			float4 _skyGradientColor2;
			float3 _CausticsDir;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_FoamLUT);
			TEXTURE2D(_DepthCache);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE2D(_WaveNoiseDisplace);
			SAMPLER(sampler_WaveNoiseDisplace);
			TEXTURE2D(_ColorCache);
			TEXTURE2D(_Normals);
			SAMPLER(sampler_Normals);
			TEXTURE2D(_Normals2);
			SAMPLER(sampler_Normals2);


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
			
			float4 Billboard2078( float3 worldPosIN, float2 screenPos, float3 QuadNormal, float3 QuadPosition )
			{
				float3 worldPos = worldPosIN;
				float3 viewDir = worldPos - _WorldSpaceCameraPos;
				float3 reflectDir = -viewDir;
				float3 nReflDirection = normalize(reflectDir);
				float4 result = float4(0,0,0,0);
				// Ray-quad intersection
				half planeIntersectDistance = dot( worldPos - QuadPosition,  QuadNormal.xyz ) / dot( nReflDirection, QuadNormal.xyz );
				half3 intersectPosition = worldPos - nReflDirection * planeIntersectDistance;
				//half4 localPlaneIntersectPosition = mul( QuadInverseMatrix, float4( intersectPosition, 1.0 ));
				half4 localPlaneIntersectPosition = float4( intersectPosition, 1.0 );
				half2 billboardUV = half2( localPlaneIntersectPosition.x, localPlaneIntersectPosition.z );
				//billboardUV*=1;
				//float4 billboardCol = BillboardTex.Sample( BillboardSampler , billboardUV );
				float4 billboardCol = float4( billboardUV.x, billboardUV.y, 0, 1);
				float3 quadNorm = QuadNormal.xyz;
				half reflectDot = dot( nReflDirection, quadNorm );
				float w = 1;
				// Cull when beyond the quad bounds
				//w = billboardUV.x > 1.0 ? 0.0 : w;
				//w = billboardUV.y > 1.0 ? 0.0 : w;
				//w = billboardUV.x < 0.0 ? 0.0 : w;
				//w = billboardUV.y < 0.0 ? 0.0 : w;
				// Cull if distance is behind the surface
				//w = planeIntersectDistance < 0 ? 0 : w;
				// Cull if quad facing backwards
				w = reflectDot <= 0.0 ? 0.0 : w; 
				// Cull if quad facing backwards and culling is ticked on the quad ( passed in via the arrays W )
				//w = QuadNormal.w == 1.0 ? (reflectDot <= 0.0 ? 0.0 : w) : w;
				billboardCol = billboardCol * billboardCol.w * w;
				billboardCol.a = w;
				result*=1-w;
				result+= billboardCol;
				return result;
			}
			
			inline float Dither8x8Bayer( int x, int y )
			{
				const float dither[ 64 ] = {
			 1, 49, 13, 61,  4, 52, 16, 64,
			33, 17, 45, 29, 36, 20, 48, 32,
			 9, 57,  5, 53, 12, 60,  8, 56,
			41, 25, 37, 21, 44, 28, 40, 24,
			 3, 51, 15, 63,  2, 50, 14, 62,
			35, 19, 47, 31, 34, 18, 46, 30,
			11, 59,  7, 55, 10, 58,  6, 54,
			43, 27, 39, 23, 42, 26, 38, 22};
				int r = y * 8 + x;
				return dither[r] / 64; // same # of instructions as pre-dividing due to compiler magic
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float2 texCoord2052 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_2054_0 = distance( texCoord2052 , float2( 0.5,0.5 ) );
				float ifLocalVar2053 = 0;
				if( temp_output_2054_0 <= 1.0 )
				ifLocalVar2053 = 1.0;
				else
				ifLocalVar2053 = 5000.0;
				#ifdef _USEINFINITYEDGE_ON
				float4 staticSwitch1498 = ( v.vertex * ifLocalVar2053 );
				#else
				float4 staticSwitch1498 = v.vertex;
				#endif
				float4 break1605 = staticSwitch1498;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 break19 = mul( _DepthMatrix, float4( ( ase_worldPos - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D_LOD( _DepthCache, sampler_Linear_Clamp, CacheUV1109, 0.0 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_2 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_3 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_2) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_3 - temp_cast_2)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 temp_cast_4 = (_FoamNoiseSpeed).xx;
				float3 break35 = ase_worldPos;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = ase_worldPos;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner138 = ( 1.0 * _Time.y * temp_cast_4 + ( WorldXZ1087 * _FoamNoiseScale ));
				float lerpResult133 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( 0.04 * panner138 ), 0.0 ).r , _FoamNoiseCon);
				float2 temp_cast_6 = (_FoamNoiseSpeed).xx;
				float2 panner168 = ( 1.0 * _Time.y * temp_cast_6 + ( WorldXZ1087 * _FoamNoiseScale2 ));
				float lerpResult161 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( panner168 * 0.04 ), 0.0 ).r , _FoamNoiseCon2);
				float UnderOffset1634 = GlobalOceanOffset;
				float3 appendResult2127 = (float3(break1605.x , ( break1605.y + UnderOffset1634 ) , break1605.z));
				float3 VertResOffset2125 = appendResult2127;
				float3 customSurfaceDepth1169 = VertResOffset2125;
				float customEye1169 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1169)).z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				#ifdef _USEWAVENOISEDISPLACE_ON
				float staticSwitch680 = ( Foamness294 + ( ( lerpResult133 + lerpResult161 ) * ( ( 1.0 - DistanceFadeA1294 ) * 0.4 ) ) );
				#else
				float staticSwitch680 = Foamness294;
				#endif
				float DepthCacheRemap109 = staticSwitch680;
				float temp_output_5_0_g3212 = ( 1.0 - DepthCacheRemap109 );
				float mulTime209 = _TimeParameters.x * ( _WaveSpeed * 0.1 );
				float2 appendResult868 = (float2(_WaveDirectionSettings.x , _WaveDirectionSettings.y));
				float WaveCenterDistance1051 = distance( WorldXZ1087 , appendResult868 );
				#ifdef _USEWAVEDIRECTION_ON
				float staticSwitch762 = ( mulTime209 + ( ( WaveCenterDistance1051 * -0.01 ) / _WaveDirectionSettings.z ) );
				#else
				float staticSwitch762 = ( 0.0 - mulTime209 );
				#endif
				float temp_output_17_0_g3212 = staticSwitch762;
				float temp_output_30_0_g3212 = ( 1.0 - saturate( 0.7 ) );
				float temp_output_35_0_g3212 = ( temp_output_30_0_g3212 * 0.5 );
				float temp_output_32_0_g3212 = ( (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) - temp_output_35_0_g3212 );
				float temp_output_31_0_g3212 = ( temp_output_35_0_g3212 + (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) );
				float DepthCacheWave184 = ( 1.0 - frac( (0.0 + (temp_output_5_0_g3212 - temp_output_32_0_g3212) * (1.0 - 0.0) / (temp_output_31_0_g3212 - temp_output_32_0_g3212)) ) );
				float clampResult124 = clamp( DepthCacheWave184 , 0.01 , 0.99 );
				float2 temp_cast_7 = (clampResult124).xx;
				float4 tex2DNode76 = SAMPLE_TEXTURE2D_LOD( _FoamLUT, sampler_Linear_Clamp, temp_cast_7, 0.0 );
				float smoothstepResult279 = smoothstep( 0.0 , 1.0 , tex2DNode76.a);
				float3 customSurfaceDepth1325 = VertResOffset2125;
				float customEye1325 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1325)).z;
				float cameraDepthFade1325 = (( customEye1325 -_ProjectionParams.y - 0.0 ) / _DistanceFadeB);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2111 = 0.0;
				#else
				float staticSwitch2111 = saturate( cameraDepthFade1325 );
				#endif
				float DistanceFadeB1326 = staticSwitch2111;
				float lerpResult1311 = lerp( smoothstepResult279 , 0.0 , DistanceFadeB1326);
				float FoamVertDisp261 = saturate( lerpResult1311 );
				float temp_output_240_0 = ( FoamVertDisp261 * _VertDispMult * 0.1 );
				float ShoreMaskVert586 = saturate( (0.0 + (( CaptureB_Distancex100200 * 100.0 ) - _RemapVertShore.x) * (1.0 - 0.0) / (_RemapVertShore.y - _RemapVertShore.x)) );
				float VertDispDistMask22014 = ( 1.0 - saturate( (0.0 + (distance( WorldXZ1087 , (_WorldSpaceCameraPos).xz ) - 5.0) * (1.0 - 0.0) / (7.9 - 5.0)) ) );
				float3 customSurfaceDepth1484 = VertResOffset2125;
				float customEye1484 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1484)).z;
				float cameraDepthFade1484 = (( customEye1484 -_ProjectionParams.y - 0.0 ) / 1.0);
				float CamDepthFade1495 = cameraDepthFade1484;
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float VertDispMaskRes1691 = ( ShoreMaskVert586 * saturate( VertDispDistMask22014 ) * saturate( (0.0 + (CamDepthFade1495 - 0.2) * (1.0 - 0.0) / (1.5 - 0.2)) ) * InfinityFeather1410 );
				float temp_output_238_0 = ( temp_output_240_0 * VertDispMaskRes1691 );
				#ifdef _USEVERTDISPLACEMENT_ON
				float staticSwitch1688 = temp_output_238_0;
				#else
				float staticSwitch1688 = 0.0;
				#endif
				float VertAdded1524 = staticSwitch1688;
				float3 appendResult1606 = (float3(break1605.x , ( break1605.y + VertAdded1524 + UnderOffset1634 ) , break1605.z));
				float3 VertResDisplaced328 = appendResult1606;
				
				o.ase_texcoord5.z = customEye1169;
				
				o.ase_texcoord5.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord5.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertResDisplaced328;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = float3(0,1,0);
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
				o.ase_tangent = v.ase_tangent;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessNum; float tessMin = _TesMin; float tessMax = _TesMax;
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
						, bool ase_vface : SV_IsFrontFace ) : SV_TARGET
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

				float2 appendResult892 = (float2(_Motion2.x , _Motion2.y));
				float2 Motion2XY1096 = appendResult892;
				float3 break35 = WorldPosition;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = WorldPosition;
				float4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner890 = ( 0.05 * _Time.y * Motion2XY1096 + WorldXZ1087);
				#ifdef GlobalPlanarReflection
				float staticSwitch1591 = _NormalPow;
				#else
				float staticSwitch1591 = ( _NormalPow * 0.7 );
				#endif
				float temp_output_1436_0 = ( _NormalPow * 6.0 );
				float switchResult1435 = (((ase_vface>0)?(staticSwitch1591):(temp_output_1436_0)));
				float3 unpack11 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals, sampler_Normals, (panner890*_Normals_ST.xy + _Normals_ST.zw) ), switchResult1435 );
				unpack11.z = lerp( 1, unpack11.z, saturate(switchResult1435) );
				float3 tex2DNode11 = unpack11;
				float2 appendResult88 = (float2(_Motion.z , _Motion.w));
				float2 MotionZW1085 = appendResult88;
				float2 panner85 = ( 0.1 * _Time.y * MotionZW1085 + WorldXZ1087);
				float switchResult1432 = (((ase_vface>0)?(( _NormalPow2 * 2.0 )):(( _NormalPow2 * 12.0 ))));
				float3 unpack84 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner85*_Normals2_ST.xy + _Normals2_ST.zw) ), switchResult1432 );
				unpack84.z = lerp( 1, unpack84.z, saturate(switchResult1432) );
				float2 panner1722 = ( -0.05 * _Time.y * MotionZW1085 + WorldXZ1087);
				float3 unpack1718 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner1722*( _Normals2_ST.xy * float2( 0.5,0.5 ) ) + _Normals2_ST.zw) ), switchResult1432 );
				unpack1718.z = lerp( 1, unpack1718.z, saturate(switchResult1432) );
				float3 temp_output_1719_0 = BlendNormal( unpack84 , unpack1718 );
				float3 temp_output_83_0 = BlendNormal( tex2DNode11 , temp_output_1719_0 );
				#ifdef _USEEXTRANORMALS_ON
				float3 staticSwitch1683 = temp_output_83_0;
				#else
				float3 staticSwitch1683 = temp_output_83_0;
				#endif
				float2 appendResult1049 = (float2(staticSwitch1683.xy));
				float2 RefNorm886 = appendResult1049;
				float RefBumpSlider1337 = _RefDistortion;
				float3 normalizeResult2273 = normalize( ( staticSwitch1683 + float3( ( RefNorm886 * RefBumpSlider1337 ) ,  0.0 ) ) );
				float2 appendResult2519 = (float2(normalizeResult2273.xy));
				float3 break19 = mul( _DepthMatrix, float4( ( WorldPosition - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = IN.ase_texcoord5.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D( _DepthCache, sampler_Linear_Clamp, CacheUV1109 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_6 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_7 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_6) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_7 - temp_cast_6)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 lerpResult2274 = lerp( float2( 0,0 ) , appendResult2519 , ( _WaterRefractonMobile * 0.5 * saturate( Foamness294 ) ));
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float customEye1169 = IN.ase_texcoord5.z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				float temp_output_1414_0 = ( ( 1.0 - InfinityFeather1410 ) * saturate( DistanceFadeA1294 ) );
				float2 lerpResult2275 = lerp( lerpResult2274 , float2( 0,0 ) , temp_output_1414_0);
				#ifdef _TRANSPARENTGRABPASS_ON
				float2 staticSwitch2542 = float2( 0,0 );
				#else
				float2 staticSwitch2542 = lerpResult2275;
				#endif
				float2 RefHackeryForColorCache2269 = staticSwitch2542;
				float2 temp_output_2198_0 = ( RefHackeryForColorCache2269 + CacheUV1109 );
				float4 tex2DNode657 = SAMPLE_TEXTURE2D( _ColorCache, sampler_Linear_Clamp, temp_output_2198_0 );
				float ColorCacheA2171 = tex2DNode657.a;
				float temp_output_12_0_g3214 = InfinityFeather1410;
				float CaptureDepthAOffsetShore1177 = ( ( ColorCacheA2171 * temp_output_12_0_g3214 ) + ( 1.0 - temp_output_12_0_g3214 ) );
				float4 break993 = _RemapAlpha;
				float switchResult2294 = (((ase_vface>0)?(saturate( (0.0 + (CaptureDepthAOffsetShore1177 - break993.x) * (1.0 - 0.0) / (break993.y - break993.x)) )):(1.0)));
				float ShoreMaskDepthAlpha141 = switchResult2294;
				float2 clipScreen639 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither639 = Dither8x8Bayer( fmod(clipScreen639.x, 8), fmod(clipScreen639.y, 8) );
				dither639 = step( dither639, ShoreMaskDepthAlpha141 );
				#ifdef _USEDITHERING_ON
				float staticSwitch1685 = dither639;
				#else
				float staticSwitch1685 = ShoreMaskDepthAlpha141;
				#endif
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch1748 = 1.0;
				#else
				float staticSwitch1748 = staticSwitch1685;
				#endif
				float HorizonAlpha2083 = temp_output_2080_0;
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2085 = HorizonAlpha2083;
				#else
				float staticSwitch2085 = staticSwitch1748;
				#endif
				float AlphaRes469 = staticSwitch2085;
				

				float3 Normal = float3(0, 0, 1);
				float Alpha = AlphaRes469;
				float AlphaClipThreshold = _ClipThreshold;
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

			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0,0
			ColorMask RGBA
			

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_TESSELLATION 1
			#pragma require tessellation tessHW
			#pragma hull HullFunction
			#pragma domain DomainFunction
			#define ASE_DISTANCE_TESSELLATION
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_TONEMAPPING 1
			#define _ASE_TINT 1
			#define _ASE_DEBUGVISUALS 1
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _NORMAL_DROPOFF_TS 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define ASE_SRP_VERSION 120115
			#define REQUIRE_DEPTH_TEXTURE 1
			#define REQUIRE_OPAQUE_TEXTURE 1
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

			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_BITANGENT
			#pragma shader_feature_local ASE_TESSELLATION_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature_local LIGHTMAP_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USEINFINITYEDGE_ON
			#pragma shader_feature_local _USEVERTDISPLACEMENT_ON
			#pragma shader_feature_local _USEWAVENOISEDISPLACE_ON
			#pragma shader_feature_local _SPACE_WORLDSPACE _SPACE_UVSPACE
			#pragma shader_feature_local _HORIZONMAPPED_ON
			#pragma shader_feature_local _USEWAVEDIRECTION_ON
			#pragma shader_feature_local _USEINFINITYFEATHER_ON
			#pragma shader_feature_local _USEFOAM_ON
			#pragma shader_feature_local _USEFOAMEXTRASAMPLER_ON
			#pragma shader_feature_local _TRANSPARENTGRABPASS_ON
			#pragma shader_feature_local _USEEXTRANORMALS_ON
			#pragma multi_compile __ GlobalPlanarReflection
			#pragma shader_feature_local _USEGRADIENTFOG_ON
			#pragma shader_feature_local _RAINBOWGRABPASS_ON
			#pragma shader_feature_local _CAUSTICSENABLE_ON
			#pragma shader_feature_local _USECAUSTICEXTRASAMPLER_ON
			#pragma shader_feature_local _USECAUSTICRAINBOW_ON
			#pragma shader_feature_local _USEDITHERING_ON


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
			float4 _FoamColor;
			float4 _Motion;
			float4 _Normals_ST;
			float4 _WaterTint;
			float4 _RemapVertShore;
			float4 _WaterAlbedo;
			float4 _WaveDirectionSettings;
			float4 _Normals2_ST;
			float4 _Motion2;
			float4 _RefFresnel;
			float4 _CausticsTint;
			float4x4 _DepthMatrix;
			float4 _RemapAlpha;
			float4 _BakeLightMapAmbient;
			float4x4 _BakeMatrix;
			float4 _bakeLightmapST;
			float4 _RemapFoam;
			float3 _WorldDir;
			float3 _WorldPos;
			float3 _DepthPosition;
			float3 _CameraForward;
			float _WaveRefractionStrength;
			float _RefractionWarp;
			float _RemapWaterDistance;
			float _RemapWaterDistance1;
			float _Smoothness;
			float _WaterDepthPow;
			float _WaterSaturation;
			float _CubemapExposure;
			float _NormalDarken1;
			float _NormalDarken;
			float _ForceEye;
			float _ClipThreshold;
			float _WaterDepthPow1;
			int _ZTestMode;
			float _NormalPow2;
			float _FoamMaxDistance;
			float _ZWriteToggle;
			float _TessNum;
			float _TesMin;
			float _TesMax;
			float _WaveScale;
			float _FoamNoiseSpeed;
			float _FoamNoiseScale;
			float _FoamNoiseCon;
			float _FoamNoiseScale2;
			float _FoamNoiseCon2;
			float _DistanceFadeA;
			float _WaveSpeed;
			float _DistanceFadeB;
			float _VertDispMult;
			float _Tolerance;
			float _Feather;
			float _FoamScale_UseFoam;
			float _NormalPow;
			float _BakedLightingFeather;
			float _RefDistortion;
			float _WaterRefractonMobile;
			float _FoamPow;
			int _DebugVisual;
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

			float4 _skyGradientColor1;
			float4 _skyGradientColor2;
			float3 _CausticsDir;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_FoamLUT);
			TEXTURE2D(_DepthCache);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE2D(_WaveNoiseDisplace);
			SAMPLER(sampler_WaveNoiseDisplace);
			TEXTURE2D(_FoamTexture);
			SAMPLER(sampler_FoamTexture);
			TEXTURE2D(_ColorCache);
			TEXTURE2D(_Normals);
			SAMPLER(sampler_Normals);
			TEXTURE2D(_Normals2);
			SAMPLER(sampler_Normals2);
			TEXTURE2D(_DepthColorLUT);
			TEXTURECUBE(_Cubemap);
			float GlobalSkyRotation;
			SAMPLER(sampler_Cubemap);
			TEXTURE2D(_TexLeft);
			SAMPLER(sampler_Linear_Mirror);
			TEXTURE2D(_TexRight);
			float4x4 _CausticMatrix;
			float4 _CausticsSettings;
			TEXTURE2D(_bakeLightmap);
			SAMPLER(sampler_bakeLightmap);


			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRGBufferPass.hlsl"

			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			float4 Billboard2078( float3 worldPosIN, float2 screenPos, float3 QuadNormal, float3 QuadPosition )
			{
				float3 worldPos = worldPosIN;
				float3 viewDir = worldPos - _WorldSpaceCameraPos;
				float3 reflectDir = -viewDir;
				float3 nReflDirection = normalize(reflectDir);
				float4 result = float4(0,0,0,0);
				// Ray-quad intersection
				half planeIntersectDistance = dot( worldPos - QuadPosition,  QuadNormal.xyz ) / dot( nReflDirection, QuadNormal.xyz );
				half3 intersectPosition = worldPos - nReflDirection * planeIntersectDistance;
				//half4 localPlaneIntersectPosition = mul( QuadInverseMatrix, float4( intersectPosition, 1.0 ));
				half4 localPlaneIntersectPosition = float4( intersectPosition, 1.0 );
				half2 billboardUV = half2( localPlaneIntersectPosition.x, localPlaneIntersectPosition.z );
				//billboardUV*=1;
				//float4 billboardCol = BillboardTex.Sample( BillboardSampler , billboardUV );
				float4 billboardCol = float4( billboardUV.x, billboardUV.y, 0, 1);
				float3 quadNorm = QuadNormal.xyz;
				half reflectDot = dot( nReflDirection, quadNorm );
				float w = 1;
				// Cull when beyond the quad bounds
				//w = billboardUV.x > 1.0 ? 0.0 : w;
				//w = billboardUV.y > 1.0 ? 0.0 : w;
				//w = billboardUV.x < 0.0 ? 0.0 : w;
				//w = billboardUV.y < 0.0 ? 0.0 : w;
				// Cull if distance is behind the surface
				//w = planeIntersectDistance < 0 ? 0 : w;
				// Cull if quad facing backwards
				w = reflectDot <= 0.0 ? 0.0 : w; 
				// Cull if quad facing backwards and culling is ticked on the quad ( passed in via the arrays W )
				//w = QuadNormal.w == 1.0 ? (reflectDot <= 0.0 ? 0.0 : w) : w;
				billboardCol = billboardCol * billboardCol.w * w;
				billboardCol.a = w;
				result*=1-w;
				result+= billboardCol;
				return result;
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			int IsStereoEyeLeft11_g3216( float3 WorldCamPos, float3 WorldCamRight, float force )
			{
				int Out = 0;	
				if(force >= 0){
						return force == 0;
					}
				#if defined(USING_STEREO_MATRICES)
					// Unity 5.4 has this new variable
					return (unity_StereoEyeIndex == 0);
				#elif defined (UNITY_DECLARE_MULTIVIEW)
					// OVR_multiview extension
					return (UNITY_VIEWID == 0);
				#else
					// NOTE: Bug #1165: _WorldSpaceCameraPos is not correct in multipass VR (when skybox is used) but UNITY_MATRIX_I_V seems to be
					#if defined(UNITY_MATRIX_I_V)
						float3 renderCameraPos = UNITY_MATRIX_I_V._m03_m13_m23;
					#else
						float3 renderCameraPos = _WorldSpaceCameraPos.xyz;
					#endif
					float fL = distance(WorldCamPos - WorldCamRight, renderCameraPos);
					float fR = distance(WorldCamPos + WorldCamRight, renderCameraPos);
					return (fL < fR);
				#endif
				return Out;
			}
			
			inline float3 ApplyTonemapper( float4 settings, float3 color )
			{
				return ApplyTonemap( color, settings );;
			}
			
			float3 MatrixMulThatWorks( float4 Pos, float4x4 Mat )
			{
				float3 result = mul(Mat,Pos.xyz);
				return result + float3(Mat[0][3],Mat[1][3],Mat[2][3]);
			}
			
			float CameraDepthTexture55_g3383( float2 uv )
			{
				float4 color = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, uv);
				return color.r;
			}
			
			float MyCustomExpression44_g3383( float rawDepth )
			{
				return LinearEyeDepth(rawDepth, _ZBufferParams);
			}
			
			float3 MyCustomExpression16_g3383( float3 view, float sceneZ )
			{
				return  _WorldSpaceCameraPos - view * sceneZ / dot(UNITY_MATRIX_I_V._13_23_33, view);
			}
			
			float4x4 Inverse4x4(float4x4 input)
			{
				#define minor(a,b,c) determinant(float3x3(input.a, input.b, input.c))
				float4x4 cofactors = float4x4(
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
			
			inline float Dither8x8Bayer( int x, int y )
			{
				const float dither[ 64 ] = {
			 1, 49, 13, 61,  4, 52, 16, 64,
			33, 17, 45, 29, 36, 20, 48, 32,
			 9, 57,  5, 53, 12, 60,  8, 56,
			41, 25, 37, 21, 44, 28, 40, 24,
			 3, 51, 15, 63,  2, 50, 14, 62,
			35, 19, 47, 31, 34, 18, 46, 30,
			11, 59,  7, 55, 10, 58,  6, 54,
			43, 27, 39, 23, 42, 26, 38, 22};
				int r = y * 8 + x;
				return dither[r] / 64; // same # of instructions as pre-dividing due to compiler magic
			}
			
			float4 URPDecodeInstruction19_g3432(  )
			{
				return float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0, 0);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float2 texCoord2052 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_2054_0 = distance( texCoord2052 , float2( 0.5,0.5 ) );
				float ifLocalVar2053 = 0;
				if( temp_output_2054_0 <= 1.0 )
				ifLocalVar2053 = 1.0;
				else
				ifLocalVar2053 = 5000.0;
				#ifdef _USEINFINITYEDGE_ON
				float4 staticSwitch1498 = ( v.vertex * ifLocalVar2053 );
				#else
				float4 staticSwitch1498 = v.vertex;
				#endif
				float4 break1605 = staticSwitch1498;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 break19 = mul( _DepthMatrix, float4( ( ase_worldPos - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D_LOD( _DepthCache, sampler_Linear_Clamp, CacheUV1109, 0.0 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_2 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_3 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_2) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_3 - temp_cast_2)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 temp_cast_4 = (_FoamNoiseSpeed).xx;
				float3 break35 = ase_worldPos;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = ase_worldPos;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner138 = ( 1.0 * _Time.y * temp_cast_4 + ( WorldXZ1087 * _FoamNoiseScale ));
				float lerpResult133 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( 0.04 * panner138 ), 0.0 ).r , _FoamNoiseCon);
				float2 temp_cast_6 = (_FoamNoiseSpeed).xx;
				float2 panner168 = ( 1.0 * _Time.y * temp_cast_6 + ( WorldXZ1087 * _FoamNoiseScale2 ));
				float lerpResult161 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( panner168 * 0.04 ), 0.0 ).r , _FoamNoiseCon2);
				float UnderOffset1634 = GlobalOceanOffset;
				float3 appendResult2127 = (float3(break1605.x , ( break1605.y + UnderOffset1634 ) , break1605.z));
				float3 VertResOffset2125 = appendResult2127;
				float3 customSurfaceDepth1169 = VertResOffset2125;
				float customEye1169 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1169)).z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				#ifdef _USEWAVENOISEDISPLACE_ON
				float staticSwitch680 = ( Foamness294 + ( ( lerpResult133 + lerpResult161 ) * ( ( 1.0 - DistanceFadeA1294 ) * 0.4 ) ) );
				#else
				float staticSwitch680 = Foamness294;
				#endif
				float DepthCacheRemap109 = staticSwitch680;
				float temp_output_5_0_g3212 = ( 1.0 - DepthCacheRemap109 );
				float mulTime209 = _TimeParameters.x * ( _WaveSpeed * 0.1 );
				float2 appendResult868 = (float2(_WaveDirectionSettings.x , _WaveDirectionSettings.y));
				float WaveCenterDistance1051 = distance( WorldXZ1087 , appendResult868 );
				#ifdef _USEWAVEDIRECTION_ON
				float staticSwitch762 = ( mulTime209 + ( ( WaveCenterDistance1051 * -0.01 ) / _WaveDirectionSettings.z ) );
				#else
				float staticSwitch762 = ( 0.0 - mulTime209 );
				#endif
				float temp_output_17_0_g3212 = staticSwitch762;
				float temp_output_30_0_g3212 = ( 1.0 - saturate( 0.7 ) );
				float temp_output_35_0_g3212 = ( temp_output_30_0_g3212 * 0.5 );
				float temp_output_32_0_g3212 = ( (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) - temp_output_35_0_g3212 );
				float temp_output_31_0_g3212 = ( temp_output_35_0_g3212 + (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) );
				float DepthCacheWave184 = ( 1.0 - frac( (0.0 + (temp_output_5_0_g3212 - temp_output_32_0_g3212) * (1.0 - 0.0) / (temp_output_31_0_g3212 - temp_output_32_0_g3212)) ) );
				float clampResult124 = clamp( DepthCacheWave184 , 0.01 , 0.99 );
				float2 temp_cast_7 = (clampResult124).xx;
				float4 tex2DNode76 = SAMPLE_TEXTURE2D_LOD( _FoamLUT, sampler_Linear_Clamp, temp_cast_7, 0.0 );
				float smoothstepResult279 = smoothstep( 0.0 , 1.0 , tex2DNode76.a);
				float3 customSurfaceDepth1325 = VertResOffset2125;
				float customEye1325 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1325)).z;
				float cameraDepthFade1325 = (( customEye1325 -_ProjectionParams.y - 0.0 ) / _DistanceFadeB);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2111 = 0.0;
				#else
				float staticSwitch2111 = saturate( cameraDepthFade1325 );
				#endif
				float DistanceFadeB1326 = staticSwitch2111;
				float lerpResult1311 = lerp( smoothstepResult279 , 0.0 , DistanceFadeB1326);
				float FoamVertDisp261 = saturate( lerpResult1311 );
				float temp_output_240_0 = ( FoamVertDisp261 * _VertDispMult * 0.1 );
				float ShoreMaskVert586 = saturate( (0.0 + (( CaptureB_Distancex100200 * 100.0 ) - _RemapVertShore.x) * (1.0 - 0.0) / (_RemapVertShore.y - _RemapVertShore.x)) );
				float VertDispDistMask22014 = ( 1.0 - saturate( (0.0 + (distance( WorldXZ1087 , (_WorldSpaceCameraPos).xz ) - 5.0) * (1.0 - 0.0) / (7.9 - 5.0)) ) );
				float3 customSurfaceDepth1484 = VertResOffset2125;
				float customEye1484 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1484)).z;
				float cameraDepthFade1484 = (( customEye1484 -_ProjectionParams.y - 0.0 ) / 1.0);
				float CamDepthFade1495 = cameraDepthFade1484;
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float VertDispMaskRes1691 = ( ShoreMaskVert586 * saturate( VertDispDistMask22014 ) * saturate( (0.0 + (CamDepthFade1495 - 0.2) * (1.0 - 0.0) / (1.5 - 0.2)) ) * InfinityFeather1410 );
				float temp_output_238_0 = ( temp_output_240_0 * VertDispMaskRes1691 );
				#ifdef _USEVERTDISPLACEMENT_ON
				float staticSwitch1688 = temp_output_238_0;
				#else
				float staticSwitch1688 = 0.0;
				#endif
				float VertAdded1524 = staticSwitch1688;
				float3 appendResult1606 = (float3(break1605.x , ( break1605.y + VertAdded1524 + UnderOffset1634 ) , break1605.z));
				float3 VertResDisplaced328 = appendResult1606;
				
				float3 vertexPos1850 = VertResDisplaced328;
				float4 ase_clipPos1850 = TransformObjectToHClip((vertexPos1850).xyz);
				float4 screenPos1850 = ComputeScreenPos(ase_clipPos1850);
				o.ase_texcoord9 = screenPos1850;
				o.ase_texcoord8.z = customEye1169;
				
				o.ase_texcoord8.w = customEye1325;
				float3 customSurfaceDepth2817 = VertResOffset2125;
				float customEye2817 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth2817)).z;
				o.ase_texcoord10.x = customEye2817;
				o.ase_texcoord10.y = customEye1484;
				float3 customSurfaceDepth2418 = VertResDisplaced328;
				float customEye2418 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth2418)).z;
				o.ase_texcoord10.z = customEye2418;
				float3 customSurfaceDepth2812 = VertResOffset2125;
				float customEye2812 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth2812)).z;
				o.ase_texcoord10.w = customEye2812;
				float3 _Vector3 = float3(0,0,-1);
				float4 Pos6_g3381 = float4( _Vector3 , 0.0 );
				float4x4 Mat6_g3381 = _CausticMatrix;
				float3 localMatrixMulThatWorks6_g3381 = MatrixMulThatWorks( Pos6_g3381 , Mat6_g3381 );
				float3 normalizeResult147_g3379 = normalize( localMatrixMulThatWorks6_g3381 );
				float3 vertexToFrag144_g3379 = normalizeResult147_g3379;
				o.ase_texcoord11.xyz = vertexToFrag144_g3379;
				float3 objToWorld3_g3383 = mul( GetObjectToWorldMatrix(), float4( v.vertex.xyz, 1 ) ).xyz;
				float3 vertexToFrag7_g3383 = objToWorld3_g3383;
				o.ase_texcoord12.xyz = vertexToFrag7_g3383;
				
				float4 Pos6_g3428 = float4( ase_worldPos , 0.0 );
				float4x4 Mat6_g3428 = _BakeMatrix;
				float3 localMatrixMulThatWorks6_g3428 = MatrixMulThatWorks( Pos6_g3428 , Mat6_g3428 );
				float3 break4_g3427 = localMatrixMulThatWorks6_g3428;
				float2 appendResult3_g3427 = (float2(break4_g3427.x , break4_g3427.z));
				float2 vertexToFrag7_g3427 = ( float2( 0.5,0.5 ) + appendResult3_g3427 );
				float2 LightingUV11_g3427 = vertexToFrag7_g3427;
				float4 temp_output_21_0_g3432 = _bakeLightmapST;
				float2 vertexToFrag10_g3432 = ( ( LightingUV11_g3427 * (temp_output_21_0_g3432).xy ) + (temp_output_21_0_g3432).zw );
				o.ase_texcoord13.xy = vertexToFrag10_g3432;
				
				o.ase_texcoord8.xy = v.texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord11.w = 0;
				o.ase_texcoord12.w = 0;
				o.ase_texcoord13.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertResDisplaced328;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = float3(0,1,0);

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
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessNum; float tessMin = _TesMin; float tessMax = _TesMax;
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
								, bool ase_vface : SV_IsFrontFace )
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

				float3 break19 = mul( _DepthMatrix, float4( ( WorldPosition - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = IN.ase_texcoord8.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_2 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_3 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_2) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_3 - temp_cast_2)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float4 tex2DNode13 = SAMPLE_TEXTURE2D( _DepthCache, sampler_Linear_Clamp, CacheUV1109 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_4 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_5 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_4) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_5 - temp_cast_4)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_6 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_7 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_6) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_7 - temp_cast_6)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float CamNear1615 = saturate( (0.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - 0.1) * (1.0 - 0.0) / (0.6 - 0.1)) );
				float4 screenPos1850 = IN.ase_texcoord9;
				float4 ase_screenPosNorm1850 = screenPos1850 / screenPos1850.w;
				ase_screenPosNorm1850.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm1850.z : ase_screenPosNorm1850.z * 0.5 + 0.5;
				float screenDepth1850 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm1850.xy ),_ZBufferParams);
				float distanceDepth1850 = saturate( abs( ( screenDepth1850 - LinearEyeDepth( ase_screenPosNorm1850.z,_ZBufferParams ) ) / ( 0.2 ) ) );
				float DepthFader1847 = distanceDepth1850;
				float2 appendResult87 = (float2(_Motion.x , _Motion.y));
				float2 MotionXY1084 = appendResult87;
				float3 break35 = WorldPosition;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = WorldPosition;
				float4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner527 = ( -0.03 * _Time.y * MotionXY1084 + WorldXZ1087);
				float4 tex2DNode91 = SAMPLE_TEXTURE2D( _FoamTexture, sampler_FoamTexture, (panner527*( _FoamScale_UseFoam * 0.2 ) + 0.0) );
				float2 panner74 = ( 0.05 * _Time.y * MotionXY1084 + WorldXZ1087);
				#ifdef _USEFOAMEXTRASAMPLER_ON
				float staticSwitch1679 = ( SAMPLE_TEXTURE2D( _FoamTexture, sampler_FoamTexture, (panner74*_FoamScale_UseFoam + 0.0) ).r + tex2DNode91.r );
				#else
				float staticSwitch1679 = ( tex2DNode91.r * 1.8 );
				#endif
				float4 break984 = _RemapFoam;
				float temp_output_172_0 = saturate( (0.0 + (( 100.0 * CaptureB_Distancex100200 ) - break984.x) * (1.0 - 0.0) / (break984.y - break984.x)) );
				float2 appendResult892 = (float2(_Motion2.x , _Motion2.y));
				float2 Motion2XY1096 = appendResult892;
				float2 panner890 = ( 0.05 * _Time.y * Motion2XY1096 + WorldXZ1087);
				#ifdef GlobalPlanarReflection
				float staticSwitch1591 = _NormalPow;
				#else
				float staticSwitch1591 = ( _NormalPow * 0.7 );
				#endif
				float temp_output_1436_0 = ( _NormalPow * 6.0 );
				float switchResult1435 = (((ase_vface>0)?(staticSwitch1591):(temp_output_1436_0)));
				float3 unpack11 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals, sampler_Normals, (panner890*_Normals_ST.xy + _Normals_ST.zw) ), switchResult1435 );
				unpack11.z = lerp( 1, unpack11.z, saturate(switchResult1435) );
				float3 tex2DNode11 = unpack11;
				float2 appendResult88 = (float2(_Motion.z , _Motion.w));
				float2 MotionZW1085 = appendResult88;
				float2 panner85 = ( 0.1 * _Time.y * MotionZW1085 + WorldXZ1087);
				float switchResult1432 = (((ase_vface>0)?(( _NormalPow2 * 2.0 )):(( _NormalPow2 * 12.0 ))));
				float3 unpack84 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner85*_Normals2_ST.xy + _Normals2_ST.zw) ), switchResult1432 );
				unpack84.z = lerp( 1, unpack84.z, saturate(switchResult1432) );
				float2 panner1722 = ( -0.05 * _Time.y * MotionZW1085 + WorldXZ1087);
				float3 unpack1718 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner1722*( _Normals2_ST.xy * float2( 0.5,0.5 ) ) + _Normals2_ST.zw) ), switchResult1432 );
				unpack1718.z = lerp( 1, unpack1718.z, saturate(switchResult1432) );
				float3 temp_output_1719_0 = BlendNormal( unpack84 , unpack1718 );
				float3 temp_output_83_0 = BlendNormal( tex2DNode11 , temp_output_1719_0 );
				#ifdef _USEEXTRANORMALS_ON
				float3 staticSwitch1683 = temp_output_83_0;
				#else
				float3 staticSwitch1683 = temp_output_83_0;
				#endif
				float2 appendResult1049 = (float2(staticSwitch1683.xy));
				float2 RefNorm886 = appendResult1049;
				float RefBumpSlider1337 = _RefDistortion;
				float3 normalizeResult2273 = normalize( ( staticSwitch1683 + float3( ( RefNorm886 * RefBumpSlider1337 ) ,  0.0 ) ) );
				float2 appendResult2519 = (float2(normalizeResult2273.xy));
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 lerpResult2274 = lerp( float2( 0,0 ) , appendResult2519 , ( _WaterRefractonMobile * 0.5 * saturate( Foamness294 ) ));
				float customEye1169 = IN.ase_texcoord8.z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				float temp_output_1414_0 = ( ( 1.0 - InfinityFeather1410 ) * saturate( DistanceFadeA1294 ) );
				float2 lerpResult2275 = lerp( lerpResult2274 , float2( 0,0 ) , temp_output_1414_0);
				#ifdef _TRANSPARENTGRABPASS_ON
				float2 staticSwitch2542 = float2( 0,0 );
				#else
				float2 staticSwitch2542 = lerpResult2275;
				#endif
				float2 RefHackeryForColorCache2269 = staticSwitch2542;
				float2 temp_output_2198_0 = ( RefHackeryForColorCache2269 + CacheUV1109 );
				float4 tex2DNode657 = SAMPLE_TEXTURE2D( _ColorCache, sampler_Linear_Clamp, temp_output_2198_0 );
				float ColorCacheA2171 = tex2DNode657.a;
				float temp_output_12_0_g3214 = InfinityFeather1410;
				float CaptureDepthAOffsetShore1177 = ( ( ColorCacheA2171 * temp_output_12_0_g3214 ) + ( 1.0 - temp_output_12_0_g3214 ) );
				float4 break993 = _RemapAlpha;
				float switchResult2294 = (((ase_vface>0)?(saturate( (0.0 + (CaptureDepthAOffsetShore1177 - break993.x) * (1.0 - 0.0) / (break993.y - break993.x)) )):(1.0)));
				float ShoreMaskDepthAlpha141 = switchResult2294;
				float temp_output_199_0 = saturate( (0.0 + (( 5.0 * CaptureB_Distancex100200 ) - break984.z) * (1.0 - 0.0) / (( break984.w * _FoamMaxDistance ) - break984.z)) );
				float temp_output_981_0 = ( 1.0 - temp_output_199_0 );
				float temp_output_982_0 = ( temp_output_172_0 * ShoreMaskDepthAlpha141 * temp_output_981_0 );
				float FoamFeather2372 = temp_output_982_0;
				float2 temp_cast_12 = (_FoamNoiseSpeed).xx;
				float2 panner138 = ( 1.0 * _Time.y * temp_cast_12 + ( WorldXZ1087 * _FoamNoiseScale ));
				float lerpResult133 = lerp( 1.0 , SAMPLE_TEXTURE2D( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( 0.04 * panner138 ) ).r , _FoamNoiseCon);
				float2 temp_cast_13 = (_FoamNoiseSpeed).xx;
				float2 panner168 = ( 1.0 * _Time.y * temp_cast_13 + ( WorldXZ1087 * _FoamNoiseScale2 ));
				float lerpResult161 = lerp( 1.0 , SAMPLE_TEXTURE2D( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( panner168 * 0.04 ) ).r , _FoamNoiseCon2);
				#ifdef _USEWAVENOISEDISPLACE_ON
				float staticSwitch680 = ( Foamness294 + ( ( lerpResult133 + lerpResult161 ) * ( ( 1.0 - DistanceFadeA1294 ) * 0.4 ) ) );
				#else
				float staticSwitch680 = Foamness294;
				#endif
				float DepthCacheRemap109 = staticSwitch680;
				float temp_output_5_0_g3212 = ( 1.0 - DepthCacheRemap109 );
				float mulTime209 = _TimeParameters.x * ( _WaveSpeed * 0.1 );
				float2 appendResult868 = (float2(_WaveDirectionSettings.x , _WaveDirectionSettings.y));
				float WaveCenterDistance1051 = distance( WorldXZ1087 , appendResult868 );
				#ifdef _USEWAVEDIRECTION_ON
				float staticSwitch762 = ( mulTime209 + ( ( WaveCenterDistance1051 * -0.01 ) / _WaveDirectionSettings.z ) );
				#else
				float staticSwitch762 = ( 0.0 - mulTime209 );
				#endif
				float temp_output_17_0_g3212 = staticSwitch762;
				float temp_output_30_0_g3212 = ( 1.0 - saturate( 0.7 ) );
				float temp_output_35_0_g3212 = ( temp_output_30_0_g3212 * 0.5 );
				float temp_output_32_0_g3212 = ( (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) - temp_output_35_0_g3212 );
				float temp_output_31_0_g3212 = ( temp_output_35_0_g3212 + (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) );
				float DepthCacheWave184 = ( 1.0 - frac( (0.0 + (temp_output_5_0_g3212 - temp_output_32_0_g3212) * (1.0 - 0.0) / (temp_output_31_0_g3212 - temp_output_32_0_g3212)) ) );
				float clampResult124 = clamp( DepthCacheWave184 , 0.01 , 0.99 );
				float2 temp_cast_14 = (clampResult124).xx;
				float4 tex2DNode76 = SAMPLE_TEXTURE2D_LOD( _FoamLUT, sampler_Linear_Clamp, temp_cast_14, 0.0 );
				float FoamMaskResult122 = frac( ( FoamFeather2372 * tex2DNode76.r ) );
				float temp_output_142_0 = saturate( FoamMaskResult122 );
				float temp_output_39_0 = ( staticSwitch1679 * temp_output_142_0 );
				float3 temp_output_1099_0 = ( ( ( pow( temp_output_39_0 , _FoamPow ) * 0.5 ) + saturate( ( pow( temp_output_39_0 , 3.0 ) + ( ( saturate( pow( temp_output_142_0 , 4.0 ) ) - 0.5 ) * 0.3 ) ) ) ) * (_FoamColor).rgb * InfinityFeather1410 * 0.4 );
				#ifdef _USEFOAM_ON
				float3 staticSwitch1687 = temp_output_1099_0;
				#else
				float3 staticSwitch1687 = float3( 0,0,0 );
				#endif
				float3 AlbedoRes465 = staticSwitch1687;
				float3 temp_output_11_0_g3347 = AlbedoRes465;
				float temp_output_23_0_g3347 = GlobalOceanUnder;
				float3 ifLocalVar4_g3347 = 0;
				UNITY_BRANCH 
				if( temp_output_23_0_g3347 >= 1.0 )
				ifLocalVar4_g3347 = temp_output_11_0_g3347;
				else
				ifLocalVar4_g3347 = temp_output_11_0_g3347;
				#ifdef _USEGRADIENTFOG_ON
				float3 staticSwitch6_g3347 = ifLocalVar4_g3347;
				#else
				float3 staticSwitch6_g3347 = temp_output_11_0_g3347;
				#endif
				#ifdef _USEUNDERWATER
				float3 staticSwitch214_g3347 = staticSwitch6_g3347;
				#else
				float3 staticSwitch214_g3347 = temp_output_11_0_g3347;
				#endif
				float3 temp_output_3047_0 = staticSwitch214_g3347;
				float temp_output_2951_0 = distance( (WorldPosition).xz , (_WorldSpaceCameraPos).xz );
				float temp_output_2955_0 = saturate( (1.0 + (temp_output_2951_0 - 100.0) * (0.0 - 1.0) / (8000.0 - 100.0)) );
				float temp_output_2949_0 = pow( temp_output_2955_0 , 8.0 );
				float4 temp_output_2892_0 = saturate( ( float4( ( InfinityFeather1410 * CamNear1615 * DepthFader1847 * temp_output_3047_0 * ShoreMaskDepthAlpha141 ) , 0.0 ) + ( _WaterAlbedo * temp_output_2949_0 * CamNear1615 * DepthFader1847 * ShoreMaskDepthAlpha141 ) ) );
				float4 AlbedoRes22337 = temp_output_2892_0;
				
				float3 appendResult1062 = (float3(tex2DNode657.rgb));
				float3 ColorCacheRGB1061 = ( InfinityFeather1410 * appendResult1062 );
				float temp_output_2998_0 = saturate( ( ( -0.2 * min( -CaptureDepthAOffsetShore1177 , 0.0 ) ) / _RemapWaterDistance1 ) );
				float2 appendResult2445 = (float2(ase_screenPosNorm.xy));
				float2 appendResult2856 = (float2(temp_output_1719_0.xy));
				float SeaAlpha2FoamAndNormal205 = InfinityFeather1410;
				float smoothstepResult279 = smoothstep( 0.0 , 1.0 , tex2DNode76.a);
				float customEye1325 = IN.ase_texcoord8.w;
				float cameraDepthFade1325 = (( customEye1325 -_ProjectionParams.y - 0.0 ) / _DistanceFadeB);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2111 = 0.0;
				#else
				float staticSwitch2111 = saturate( cameraDepthFade1325 );
				#endif
				float DistanceFadeB1326 = staticSwitch2111;
				float lerpResult1311 = lerp( smoothstepResult279 , 0.0 , DistanceFadeB1326);
				float FoamVertDisp261 = saturate( lerpResult1311 );
				float temp_output_1332_0 = ( RefBumpSlider1337 * SeaAlpha2FoamAndNormal205 * (1.0 + (FoamVertDisp261 - 0.0) * (5.0 - 1.0) / (1.0 - 0.0)) * 22.0 );
				float3 normalizeResult2866 = normalize( ( temp_output_1719_0 + float3( ( appendResult2856 * temp_output_1332_0 ) ,  0.0 ) ) );
				float3 lerpResult2865 = lerp( normalizeResult2866 , float3( 0,0,1 ) , float3( 0.5,0.5,0.5 ));
				float switchResult1438 = (((ase_vface>0)?(FoamVertDisp261):(( FoamVertDisp261 * 0.8 ))));
				float customEye2817 = IN.ase_texcoord10.x;
				float cameraDepthFade2817 = (( customEye2817 -_ProjectionParams.y - 0.0 ) / 300.0);
				float3 appendResult2798 = (float3(0.0 , ( saturate( switchResult1438 ) * 0.1 * _WaveRefractionStrength * ( 1.0 - saturate( cameraDepthFade2817 ) ) ) , 0.0));
				float3 lerpResult2861 = lerp( ( lerpResult2865 + appendResult2798 ) , float3( 0,0,1 ) , temp_output_1414_0);
				float UnderOffset1634 = GlobalOceanOffset;
				float GlobalOceanHeight1552 = ( GlobalOceanHeight + UnderOffset1634 );
				float temp_output_240_0 = ( FoamVertDisp261 * _VertDispMult * 0.1 );
				float ShoreMaskVert586 = saturate( (0.0 + (( CaptureB_Distancex100200 * 100.0 ) - _RemapVertShore.x) * (1.0 - 0.0) / (_RemapVertShore.y - _RemapVertShore.x)) );
				float VertDispDistMask22014 = ( 1.0 - saturate( (0.0 + (distance( WorldXZ1087 , (_WorldSpaceCameraPos).xz ) - 5.0) * (1.0 - 0.0) / (7.9 - 5.0)) ) );
				float customEye1484 = IN.ase_texcoord10.y;
				float cameraDepthFade1484 = (( customEye1484 -_ProjectionParams.y - 0.0 ) / 1.0);
				float CamDepthFade1495 = cameraDepthFade1484;
				float VertDispMaskRes1691 = ( ShoreMaskVert586 * saturate( VertDispDistMask22014 ) * saturate( (0.0 + (CamDepthFade1495 - 0.2) * (1.0 - 0.0) / (1.5 - 0.2)) ) * InfinityFeather1410 );
				float temp_output_238_0 = ( temp_output_240_0 * VertDispMaskRes1691 );
				#ifdef _USEVERTDISPLACEMENT_ON
				float staticSwitch1688 = temp_output_238_0;
				#else
				float staticSwitch1688 = 0.0;
				#endif
				float VertAdded1524 = staticSwitch1688;
				float temp_output_1537_0 = ( ( GlobalOceanHeight1552 + (VertAdded1524).x ) - ( _WorldSpaceCameraPos.y - 0.2 ) );
				float temp_output_1571_0 = saturate( ( temp_output_1537_0 * 5.0 ) );
				float RefractHack1578 = temp_output_1571_0;
				float temp_output_1582_0 = (1.0 + (RefractHack1578 - 0.0) * (11.0 - 1.0) / (1.0 - 0.0));
				float3 lerpResult2862 = lerp( float3( 0,0,1 ) , lerpResult2861 , temp_output_1582_0);
				float3 switchResult2863 = (((ase_vface>0)?(lerpResult2862):(lerpResult2861)));
				float3 NormalResRefracted2803 = switchResult2863;
				float3 NormalResBelow1480 = staticSwitch1683;
				float3 switchResult1755 = (((ase_vface>0)?(NormalResRefracted2803):(NormalResBelow1480)));
				float2 appendResult3029 = (float2(switchResult1755.xy));
				float customEye2418 = IN.ase_texcoord10.z;
				float cameraDepthFade2418 = (( customEye2418 -_ProjectionParams.y - 0.0 ) / 40.0);
				float temp_output_2417_0 = ( 1.0 - saturate( cameraDepthFade2418 ) );
				float2 temp_output_2389_0 = ( appendResult2445 + ( appendResult3029 * DepthFader1847 * CamNear1615 * ShoreMaskDepthAlpha141 * temp_output_2417_0 ) );
				float eyeDepth2780 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( temp_output_2389_0, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float2 lerpResult2452 = lerp( appendResult2445 , temp_output_2389_0 , ( _RefractionWarp * step( ( ScreenPos.w - eyeDepth2780 ) , 0.0 ) ));
				float2 ScreenSpaceRefractUV2387 = lerpResult2452;
				float eyeDepth6_g3371 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV2387, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3371 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float fresnelNdotV2830 = dot( WorldNormal, WorldViewDirection );
				float fresnelNode2830 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV2830, 5.0 ) );
				float lerpResult2832 = lerp( 9.0 , 30.0 , saturate( fresnelNode2830 ));
				float DepthFadeDistthing2939 = lerpResult2832;
				float temp_output_12_0_g3371 = ( DepthFadeDistthing2939 * _RemapWaterDistance );
				float temp_output_3_0_g3371 = ( abs( ( eyeDepth6_g3371 - -worldToView21_g3371.z ) ) / temp_output_12_0_g3371 );
				float temp_output_2907_0 = saturate( temp_output_3_0_g3371 );
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch3001 = temp_output_2907_0;
				#else
				float staticSwitch3001 = temp_output_2998_0;
				#endif
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch2993 = _WaterDepthPow;
				#else
				float staticSwitch2993 = _WaterDepthPow1;
				#endif
				float temp_output_2909_0 = ( 1.0 - pow( ( 1.0 - temp_output_2907_0 ) , 6.0 ) );
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch2990 = temp_output_2909_0;
				#else
				float staticSwitch2990 = ( 1.0 - pow( ( 1.0 - temp_output_2998_0 ) , 6.0 ) );
				#endif
				float2 appendResult2183 = (float2(staticSwitch2990 , 0.0));
				float3 appendResult2142 = (float3(SAMPLE_TEXTURE2D_LOD( _DepthColorLUT, sampler_Linear_Clamp, appendResult2183, 0.0 ).rgb));
				float3 appendResult2828 = (float3(_WaterTint.rgb));
				float3 temp_output_1964_0 = ( ( saturate( pow( ( 1.0 - staticSwitch3001 ) , staticSwitch2993 ) ) * appendResult2142 ) * appendResult2828 * InfinityFeather1410 );
				float3 desaturateInitialColor2848 = temp_output_1964_0;
				float desaturateDot2848 = dot( desaturateInitialColor2848, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar2848 = lerp( desaturateInitialColor2848, desaturateDot2848.xxx, ( 1.0 - _WaterSaturation ) );
				float3 switchResult2135 = (((ase_vface>0)?(desaturateVar2848):(float3( 0,0,0 ))));
				float3 WaterDepthTint342 = switchResult2135;
				float switchResult2482 = (((ase_vface>0)?(saturate( (0.0 + (CaptureDepthAOffsetShore1177 - break993.z) * (1.0 - 0.0) / (break993.w - break993.z)) )):(1.0)));
				float ShoreMaskSpec2479 = switchResult2482;
				float3 lerpResult3187 = lerp( float3( 1,1,1 ) , WaterDepthTint342 , ShoreMaskSpec2479);
				float switchResult1429 = (((ase_vface>0)?(radians( GlobalSkyRotation )):(radians( ( 0.0 + 180.0 ) ))));
				float3 switchResult1425 = (((ase_vface>0)?(-WorldViewDirection):(WorldViewDirection)));
				float3 normalizeResult257 = normalize( ( staticSwitch1683 + float3( ( RefNorm886 * temp_output_1332_0 ) ,  0.0 ) ) );
				float3 lerpResult1261 = lerp( normalizeResult257 , float3( 0,0,1 ) , float3( 0.5,0.5,0.5 ));
				float3 lerpResult1264 = lerp( ( lerpResult1261 + appendResult2798 ) , float3( 0,0,1 ) , temp_output_1414_0);
				float3 lerpResult1581 = lerp( float3( 0,0,1 ) , lerpResult1264 , temp_output_1582_0);
				float3 switchResult2804 = (((ase_vface>0)?(lerpResult1581):(lerpResult1264)));
				float3 NormalRes466 = switchResult2804;
				float HorizonMask2300 = saturate( (1.0 + (distance( WorldPosition , _WorldSpaceCameraPos ) - 200.0) * (0.0 - 1.0) / (3000.0 - 200.0)) );
				float3 lerpResult1488 = lerp( float3( 0,0,1 ) , NormalRes466 , saturate( ( CamNear1615 * CamDepthFade1495 * HorizonMask2300 ) ));
				float3 tanToWorld0 = float3( WorldTangent.x, WorldBiTangent.x, WorldNormal.x );
				float3 tanToWorld1 = float3( WorldTangent.y, WorldBiTangent.y, WorldNormal.y );
				float3 tanToWorld2 = float3( WorldTangent.z, WorldBiTangent.z, WorldNormal.z );
				float3 tanNormal1153 = lerpResult1488;
				float3 worldNormal1153 = float3(dot(tanToWorld0,tanNormal1153), dot(tanToWorld1,tanNormal1153), dot(tanToWorld2,tanNormal1153));
				float3 temp_output_1151_0 = reflect( switchResult1425 , worldNormal1153 );
				float3 rotatedValue1157 = RotateAroundAxis( float3( 0,0,0 ), temp_output_1151_0, float3( 0,1,0 ), switchResult1429 );
				float SmoothnessRes468 = _Smoothness;
				float lerpResult2215 = lerp( 1.0 , SmoothnessRes468 , HorizonMask2300);
				float temp_output_1147_0 = ( ( 1.0 - lerpResult2215 ) * 8.0 );
				float clampResult1494 = clamp( (0.0 + (distance( GlobalOceanHeight1552 , _WorldSpaceCameraPos.y ) - 0.0) * (0.7 - 0.0) / (1.0 - 0.0)) , 0.0 , 0.7 );
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch3051 = 2.0;
				#else
				float staticSwitch3051 = clampResult1494;
				#endif
				float switchResult1466 = (((ase_vface>0)?(temp_output_1147_0):(staticSwitch3051)));
				float3 temp_output_1550_0 = (SAMPLE_TEXTURECUBE_LOD( _Cubemap, sampler_Cubemap, rotatedValue1157, switchResult1466 )).rgb;
				float3 normalizeResult1285 = normalize( ( tex2DNode11 * float3( 55,55,1 ) ) );
				float3 tanNormal1237 = normalizeResult1285;
				float3 worldNormal1237 = float3(dot(tanToWorld0,tanNormal1237), dot(tanToWorld1,tanNormal1237), dot(tanToWorld2,tanNormal1237));
				float dotResult1229 = dot( worldNormal1237 , WorldViewDirection );
				float customEye2812 = IN.ase_texcoord10.w;
				float cameraDepthFade2812 = (( customEye2812 -_ProjectionParams.y - 0.0 ) / _NormalDarken1);
				float lerpResult1280 = lerp( 1.0 , ( 1.0 - saturate( dotResult1229 ) ) , ( ( 1.0 - saturate( cameraDepthFade2812 ) ) * _NormalDarken ));
				float NormDarken1230 = lerpResult1280;
				float3 tanNormal901 = NormalRes466;
				float3 worldNormal901 = float3(dot(tanToWorld0,tanNormal901), dot(tanToWorld1,tanNormal901), dot(tanToWorld2,tanNormal901));
				#ifdef _HORIZONMAPPED_ON
				float3 staticSwitch2104 = float3(0,1,0);
				#else
				float3 staticSwitch2104 = worldNormal901;
				#endif
				float fresnelNdotV899 = dot( staticSwitch2104, WorldViewDirection );
				float fresnelNode899 = ( _RefFresnel.x + _RefFresnel.y * pow( 1.0 - fresnelNdotV899, _RefFresnel.z ) );
				float temp_output_904_0 = saturate( fresnelNode899 );
				float3 temp_output_1162_0 = ( temp_output_1550_0 * _CubemapExposure * (0.4 + (NormDarken1230 - 0.0) * (1.0 - 0.4) / (1.0 - 0.0)) * temp_output_904_0 * ShoreMaskSpec2479 );
				float3 WorldCamPos11_g3216 = _WorldPos;
				float3 WorldCamRight11_g3216 = _WorldDir;
				float force11_g3216 = _ForceEye;
				int localIsStereoEyeLeft11_g3216 = IsStereoEyeLeft11_g3216( WorldCamPos11_g3216 , WorldCamRight11_g3216 , force11_g3216 );
				float2 appendResult4_g3216 = (float2(ase_screenPosNorm.xy));
				float2 appendResult1245 = (float2(NormalRes466.xy));
				float clampResult1520 = clamp( (VertAdded1524).x , 0.0 , 0.3 );
				float2 temp_output_1_0_g3216 = ( appendResult4_g3216 + ( appendResult1245 * (0.0 + (clampResult1520 - 0.1) * (2.0 - 0.0) / (0.3 - 0.1)) * CamNear1615 * 0.5 ) );
				float4 tex2DNode3_g3216 = SAMPLE_TEXTURE2D( _TexLeft, sampler_Linear_Mirror, temp_output_1_0_g3216 );
				float4 tex2DNode2_g3216 = SAMPLE_TEXTURE2D( _TexRight, sampler_Linear_Mirror, temp_output_1_0_g3216 );
				float4 ifLocalVar10_g3216 = 0;
				if( localIsStereoEyeLeft11_g3216 <= 0.0 )
				ifLocalVar10_g3216 = tex2DNode2_g3216;
				else
				ifLocalVar10_g3216 = tex2DNode3_g3216;
				float3 temp_output_1549_0 = (ifLocalVar10_g3216).rgb;
				float3 UnderReflection1610 = ( temp_output_1549_0 * temp_output_904_0 );
				float3 switchResult1453 = (((ase_vface>0)?(( UnderReflection1610 * (0.4 + (NormDarken1230 - 0.0) * (1.0 - 0.4) / (1.0 - 0.0)) )):(temp_output_1162_0)));
				#ifdef GlobalPlanarReflection
				float3 staticSwitch945 = switchResult1453;
				#else
				float3 staticSwitch945 = temp_output_1162_0;
				#endif
				float4 appendResult11_g3229 = (float4(_TonemappingSettings.x , _TonemappingSettings.y , _TonemappingSettings.z , _TonemappingSettings.w));
				float4 temp_output_2533_0 = appendResult11_g3229;
				float4 settings2527 = temp_output_2533_0;
				float3 color2527 = staticSwitch945;
				float3 localApplyTonemapper2527 = ApplyTonemapper( settings2527 , color2527 );
				#ifdef _TRANSPARENTGRABPASS_ON
				float3 staticSwitch2528 = localApplyTonemapper2527;
				#else
				float3 staticSwitch2528 = staticSwitch945;
				#endif
				float4 fetchOpaqueVal1753 = float4( SHADERGRAPH_SAMPLE_SCENE_COLOR( ScreenSpaceRefractUV2387 ), 1.0 );
				float3 appendResult1754 = (float3(fetchOpaqueVal1753.rgb));
				#ifdef _RAINBOWGRABPASS_ON
				float3 staticSwitch2395 = appendResult1754;
				#else
				float3 staticSwitch2395 = appendResult1754;
				#endif
				float3 temp_output_18_0_g3379 = float3( 0,0,0 );
				float3 temp_output_30_0_g3379 = float3( 0,0,1 );
				float3 tanNormal12_g3379 = temp_output_30_0_g3379;
				float3 worldNormal12_g3379 = float3(dot(tanToWorld0,tanNormal12_g3379), dot(tanToWorld1,tanNormal12_g3379), dot(tanToWorld2,tanNormal12_g3379));
				float3 vertexToFrag144_g3379 = IN.ase_texcoord11.xyz;
				float dotResult1_g3379 = dot( worldNormal12_g3379 , vertexToFrag144_g3379 );
				float3 vertexToFrag7_g3383 = IN.ase_texcoord12.xyz;
				float3 positionWS_fogFactor24_g3383 = vertexToFrag7_g3383;
				float3 normalizeResult21_g3383 = normalize( ( _WorldSpaceCameraPos - positionWS_fogFactor24_g3383 ) );
				float3 view23_g3383 = normalizeResult21_g3383;
				float3 view16_g3383 = view23_g3383;
				float3 appendResult49_g3383 = (float3(ScreenSpaceRefractUV2387 , ase_screenPosNorm.w));
				float3 screenPosXYW29_g3383 = (appendResult49_g3383).xyz;
				float2 uvDepth38_g3383 = ( (screenPosXYW29_g3383).xy / (screenPosXYW29_g3383).z );
				float2 uv55_g3383 = uvDepth38_g3383;
				float localCameraDepthTexture55_g3383 = CameraDepthTexture55_g3383( uv55_g3383 );
				float rawDepth40_g3383 = localCameraDepthTexture55_g3383;
				float rawDepth44_g3383 = rawDepth40_g3383;
				float localMyCustomExpression44_g3383 = MyCustomExpression44_g3383( rawDepth44_g3383 );
				float sceneZ41_g3383 = localMyCustomExpression44_g3383;
				float sceneZ16_g3383 = sceneZ41_g3383;
				float3 localMyCustomExpression16_g3383 = MyCustomExpression16_g3383( view16_g3383 , sceneZ16_g3383 );
				float3 scenePos11_g3383 = localMyCustomExpression16_g3383;
				float3 WorldPosition2704 = scenePos11_g3383;
				float3 temp_output_105_0_g3379 = WorldPosition2704;
				float2 PanSpeed131_g3379 = (_CausticsSettings).xy;
				float mulTime28_g3379 = _TimeParameters.x * -1.0;
				float2 break101_g3379 = ( PanSpeed131_g3379 * mulTime28_g3379 );
				float3 appendResult100_g3379 = (float3(break101_g3379.x , 0.0 , break101_g3379.y));
				float4 Pos6_g3382 = float4( ( temp_output_105_0_g3379 + appendResult100_g3379 ) , 0.0 );
				float4x4 invertVal146_g3379 = Inverse4x4( _CausticMatrix );
				float4x4 Mat6_g3382 = invertVal146_g3379;
				float3 localMatrixMulThatWorks6_g3382 = MatrixMulThatWorks( Pos6_g3382 , Mat6_g3382 );
				float2 temp_output_87_0_g3379 = ( (localMatrixMulThatWorks6_g3382).xy + ( 0.0045 * 2.0 ) );
				float temp_output_117_0_g3379 = (temp_output_105_0_g3379).y;
				float temp_output_63_0_g3379 = GlobalOceanHeight1552;
				float2 DistanceFade134_g3379 = (_CausticsSettings).zw;
				float2 break136_g3379 = DistanceFade134_g3379;
				float temp_output_67_0_g3379 = ( saturate( (0.0 + (max( -( temp_output_117_0_g3379 - temp_output_63_0_g3379 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) * saturate( (1.0 + (distance( temp_output_63_0_g3379 , temp_output_117_0_g3379 ) - break136_g3379.x) * (0.0 - 1.0) / (break136_g3379.y - break136_g3379.x)) ) );
				float CausticMipLevel118_g3379 = ( ( 1.0 - temp_output_67_0_g3379 ) * 4.0 );
				float2 temp_output_75_0_g3379 = ( (localMatrixMulThatWorks6_g3382).xy + 0.0045 );
				#ifdef _USECAUSTICRAINBOW_ON
				float3 staticSwitch73_g3379 = ( ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Caustics, temp_output_87_0_g3379, CausticMipLevel118_g3379 ).r * float3(0,0,1) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Caustics, temp_output_75_0_g3379, CausticMipLevel118_g3379 ).r * float3(0,1,0) ) + ( SAMPLE_TEXTURE2D_LOD( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3382).xy, CausticMipLevel118_g3379 ).r * float3(1,0,0) ) );
				#else
				float3 staticSwitch73_g3379 = float3( 0,0,0 );
				#endif
				#ifdef _USECAUSTICEXTRASAMPLER_ON
				float3 staticSwitch57_g3379 = staticSwitch73_g3379;
				#else
				float3 staticSwitch57_g3379 = float3( 0,0,0 );
				#endif
				float3 appendResult62_g3379 = (float3(_CausticsColor.rgb));
				#ifdef _CAUSTICSENABLE_ON
				float3 staticSwitch16_g3379 = ( temp_output_18_0_g3379 + ( max( dotResult1_g3379 , 0.0 ) * staticSwitch57_g3379 * appendResult62_g3379 * temp_output_67_0_g3379 ) );
				#else
				float3 staticSwitch16_g3379 = temp_output_18_0_g3379;
				#endif
				float3 appendResult3040 = (float3(_CausticsTint.rgb));
				float3 temp_output_3042_0 = ( staticSwitch16_g3379 * appendResult3040 * 8.0 );
				float3 switchResult3046 = (((ase_vface>0)?(temp_output_3042_0):(float3( 0,0,0 ))));
				float3 Caustics2711 = switchResult3046;
				float3 GrabPassOG1994 = ( staticSwitch2395 + ( staticSwitch2395 * Caustics2711 ) );
				float3 temp_cast_33 = (1.0).xxx;
				float eyeDepth6_g3373 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV2387, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3373 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float temp_output_12_0_g3373 = 1.0;
				float temp_output_3_0_g3373 = ( abs( ( eyeDepth6_g3373 - -worldToView21_g3373.z ) ) / temp_output_12_0_g3373 );
				float temp_output_1843_0 = saturate( pow( ( 1.0 - saturate( temp_output_3_0_g3373 ) ) , 6.0 ) );
				float ShallowMask2838 = ( ( 1.0 - temp_output_1843_0 ) * ShoreMaskDepthAlpha141 );
				float3 lerpResult2839 = lerp( temp_cast_33 , WaterDepthTint342 , ShallowMask2838);
				float3 switchResult3072 = (((ase_vface>0)?(lerpResult2839):(float3( 1,1,1 ))));
				float switchResult1513 = (((ase_vface>0)?(1.0):(CamNear1615)));
				float3 lerpResult1900 = lerp( GrabPassOG1994 , ( switchResult3072 * GrabPassOG1994 ) , switchResult1513);
				float3 GrabPassTinted1768 = lerpResult1900;
				float switchResult1945 = (((ase_vface>0)?(1.0):(CamNear1615)));
				float3 lerpResult1746 = lerp( GrabPassTinted1768 , ( staticSwitch2528 + GrabPassTinted1768 ) , ( ShoreMaskDepthAlpha141 * DepthFader1847 * switchResult1945 ));
				float3 NormalResBelow23057 = temp_output_1719_0;
				float3 tanNormal1468 = NormalResBelow23057;
				float3 worldNormal1468 = float3(dot(tanToWorld0,tanNormal1468), dot(tanToWorld1,tanNormal1468), dot(tanToWorld2,tanNormal1468));
				#ifdef _HORIZONMAPPED_ON
				float3 staticSwitch2108 = float3(0,1,0);
				#else
				float3 staticSwitch2108 = worldNormal1468;
				#endif
				float clampResult3065 = clamp( (-4.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - 0.1) * (-0.5 - -4.0) / (10.0 - 0.1)) , -4.0 , -0.5 );
				float fresnelNdotV1454 = dot( normalize( staticSwitch2108 ), -WorldViewDirection );
				float fresnelNode1454 = ( clampResult3065 + 4.0 * pow( max( 1.0 - fresnelNdotV1454 , 0.0001 ), 1.0 ) );
				float temp_output_1460_0 = saturate( fresnelNode1454 );
				float3 lerpResult1923 = lerp( lerpResult1746 , staticSwitch2528 , ( temp_output_1460_0 * CamNear1615 ));
				float3 switchResult1925 = (((ase_vface>0)?(lerpResult1746):(lerpResult1923)));
				#ifdef _TRANSPARENTGRABPASS_ON
				float3 staticSwitch1588 = switchResult1925;
				#else
				float3 staticSwitch1588 = ( ( ColorCacheRGB1061 * lerpResult3187 ) + ( saturate( (0.0 + (ShoreMaskSpec2479 - 0.3) * (1.0 - 0.0) / (1.0 - 0.3)) ) * staticSwitch2528 ) );
				#endif
				float3 appendResult2329 = (float3(staticSwitch1588));
				float3 EmissionRes467 = appendResult2329;
				float3 EmissionRes22320 = EmissionRes467;
				
				float2 clipScreen639 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither639 = Dither8x8Bayer( fmod(clipScreen639.x, 8), fmod(clipScreen639.y, 8) );
				dither639 = step( dither639, ShoreMaskDepthAlpha141 );
				#ifdef _USEDITHERING_ON
				float staticSwitch1685 = dither639;
				#else
				float staticSwitch1685 = ShoreMaskDepthAlpha141;
				#endif
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch1748 = 1.0;
				#else
				float staticSwitch1748 = staticSwitch1685;
				#endif
				float HorizonAlpha2083 = temp_output_2080_0;
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2085 = HorizonAlpha2083;
				#else
				float staticSwitch2085 = staticSwitch1748;
				#endif
				float AlphaRes469 = staticSwitch2085;
				
				float3 appendResult18_g3427 = (float3(_BakeLightMapAmbient.rgb));
				float2 vertexToFrag10_g3432 = IN.ase_texcoord13.xy;
				float4 tex2DNode7_g3432 = SAMPLE_TEXTURE2D_BIAS( _bakeLightmap, sampler_bakeLightmap, vertexToFrag10_g3432, 0.0 );
				float4 localURPDecodeInstruction19_g3432 = URPDecodeInstruction19_g3432();
				float3 decodeLightMap6_g3432 = DecodeLightmap(tex2DNode7_g3432,localURPDecodeInstruction19_g3432);
				float temp_output_17_0_g3429 = -0.001;
				float2 temp_cast_37 = (temp_output_17_0_g3429).xx;
				float2 temp_cast_38 = (( 1.0 - temp_output_17_0_g3429 )).xx;
				float2 break5_g3429 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_37) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_38 - temp_cast_37)) );
				float temp_output_1_0_g3431 = break5_g3429.x;
				float temp_output_8_0_g3429 = ( _BakedLightingFeather + -1.0 );
				float temp_output_1_0_g3430 = break5_g3429.y;
				float temp_output_17_0_g3427 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3431 - floor( ( temp_output_1_0_g3431 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3429 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3430 - floor( ( temp_output_1_0_g3430 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3429 - -1.0)) ) ) );
				float3 lerpResult12_g3427 = lerp( appendResult18_g3427 , decodeLightMap6_g3432 , temp_output_17_0_g3427);
				float3 LightmapRes26_g3427 = lerpResult12_g3427;
				float3 LightmapRes2657 = LightmapRes26_g3427;
				

				float3 BaseColor = AlbedoRes22337.rgb;
				float3 Normal = float3(0, 0, 1);
				float3 Emission = EmissionRes22320;
				float3 Specular = 0.5;
				float Metallic = 0;
				float Smoothness = ( ShoreMaskDepthAlpha141 * SmoothnessRes468 * DepthFader1847 );
				float Occlusion = 1;
				float Alpha = AlphaRes469;
				float AlphaClipThreshold = _ClipThreshold;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = LightmapRes2657;
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

		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }
			
			AlphaToMask Off

			Cull Off

			HLSLPROGRAM

			#define ASE_TESSELLATION 1
			#pragma require tessellation tessHW
			#pragma hull HullFunction
			#pragma domain DomainFunction
			#define ASE_DISTANCE_TESSELLATION
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_TONEMAPPING 1
			#define _ASE_TINT 1
			#define _ASE_DEBUGVISUALS 1
			#define ASE_FOG 1
			#define _NORMAL_DROPOFF_TS 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
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
			#pragma shader_feature_local ASE_TESSELLATION_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature_local LIGHTMAP_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USEINFINITYEDGE_ON
			#pragma shader_feature_local _USEVERTDISPLACEMENT_ON
			#pragma shader_feature_local _USEWAVENOISEDISPLACE_ON
			#pragma shader_feature_local _SPACE_WORLDSPACE _SPACE_UVSPACE
			#pragma shader_feature_local _HORIZONMAPPED_ON
			#pragma shader_feature_local _USEWAVEDIRECTION_ON
			#pragma shader_feature_local _USEINFINITYFEATHER_ON
			#pragma shader_feature_local _TRANSPARENTGRABPASS_ON
			#pragma shader_feature_local _USEDITHERING_ON
			#pragma shader_feature_local _USEEXTRANORMALS_ON
			#pragma multi_compile __ GlobalPlanarReflection


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _FoamColor;
			float4 _Motion;
			float4 _Normals_ST;
			float4 _WaterTint;
			float4 _RemapVertShore;
			float4 _WaterAlbedo;
			float4 _WaveDirectionSettings;
			float4 _Normals2_ST;
			float4 _Motion2;
			float4 _RefFresnel;
			float4 _CausticsTint;
			float4x4 _DepthMatrix;
			float4 _RemapAlpha;
			float4 _BakeLightMapAmbient;
			float4x4 _BakeMatrix;
			float4 _bakeLightmapST;
			float4 _RemapFoam;
			float3 _WorldDir;
			float3 _WorldPos;
			float3 _DepthPosition;
			float3 _CameraForward;
			float _WaveRefractionStrength;
			float _RefractionWarp;
			float _RemapWaterDistance;
			float _RemapWaterDistance1;
			float _Smoothness;
			float _WaterDepthPow;
			float _WaterSaturation;
			float _CubemapExposure;
			float _NormalDarken1;
			float _NormalDarken;
			float _ForceEye;
			float _ClipThreshold;
			float _WaterDepthPow1;
			int _ZTestMode;
			float _NormalPow2;
			float _FoamMaxDistance;
			float _ZWriteToggle;
			float _TessNum;
			float _TesMin;
			float _TesMax;
			float _WaveScale;
			float _FoamNoiseSpeed;
			float _FoamNoiseScale;
			float _FoamNoiseCon;
			float _FoamNoiseScale2;
			float _FoamNoiseCon2;
			float _DistanceFadeA;
			float _WaveSpeed;
			float _DistanceFadeB;
			float _VertDispMult;
			float _Tolerance;
			float _Feather;
			float _FoamScale_UseFoam;
			float _NormalPow;
			float _BakedLightingFeather;
			float _RefDistortion;
			float _WaterRefractonMobile;
			float _FoamPow;
			int _DebugVisual;
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

			float4 _skyGradientColor1;
			float4 _skyGradientColor2;
			float3 _CausticsDir;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_FoamLUT);
			TEXTURE2D(_DepthCache);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE2D(_WaveNoiseDisplace);
			SAMPLER(sampler_WaveNoiseDisplace);
			TEXTURE2D(_ColorCache);
			TEXTURE2D(_Normals);
			SAMPLER(sampler_Normals);
			TEXTURE2D(_Normals2);
			SAMPLER(sampler_Normals2);


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
			
			float4 Billboard2078( float3 worldPosIN, float2 screenPos, float3 QuadNormal, float3 QuadPosition )
			{
				float3 worldPos = worldPosIN;
				float3 viewDir = worldPos - _WorldSpaceCameraPos;
				float3 reflectDir = -viewDir;
				float3 nReflDirection = normalize(reflectDir);
				float4 result = float4(0,0,0,0);
				// Ray-quad intersection
				half planeIntersectDistance = dot( worldPos - QuadPosition,  QuadNormal.xyz ) / dot( nReflDirection, QuadNormal.xyz );
				half3 intersectPosition = worldPos - nReflDirection * planeIntersectDistance;
				//half4 localPlaneIntersectPosition = mul( QuadInverseMatrix, float4( intersectPosition, 1.0 ));
				half4 localPlaneIntersectPosition = float4( intersectPosition, 1.0 );
				half2 billboardUV = half2( localPlaneIntersectPosition.x, localPlaneIntersectPosition.z );
				//billboardUV*=1;
				//float4 billboardCol = BillboardTex.Sample( BillboardSampler , billboardUV );
				float4 billboardCol = float4( billboardUV.x, billboardUV.y, 0, 1);
				float3 quadNorm = QuadNormal.xyz;
				half reflectDot = dot( nReflDirection, quadNorm );
				float w = 1;
				// Cull when beyond the quad bounds
				//w = billboardUV.x > 1.0 ? 0.0 : w;
				//w = billboardUV.y > 1.0 ? 0.0 : w;
				//w = billboardUV.x < 0.0 ? 0.0 : w;
				//w = billboardUV.y < 0.0 ? 0.0 : w;
				// Cull if distance is behind the surface
				//w = planeIntersectDistance < 0 ? 0 : w;
				// Cull if quad facing backwards
				w = reflectDot <= 0.0 ? 0.0 : w; 
				// Cull if quad facing backwards and culling is ticked on the quad ( passed in via the arrays W )
				//w = QuadNormal.w == 1.0 ? (reflectDot <= 0.0 ? 0.0 : w) : w;
				billboardCol = billboardCol * billboardCol.w * w;
				billboardCol.a = w;
				result*=1-w;
				result+= billboardCol;
				return result;
			}
			
			inline float Dither8x8Bayer( int x, int y )
			{
				const float dither[ 64 ] = {
			 1, 49, 13, 61,  4, 52, 16, 64,
			33, 17, 45, 29, 36, 20, 48, 32,
			 9, 57,  5, 53, 12, 60,  8, 56,
			41, 25, 37, 21, 44, 28, 40, 24,
			 3, 51, 15, 63,  2, 50, 14, 62,
			35, 19, 47, 31, 34, 18, 46, 30,
			11, 59,  7, 55, 10, 58,  6, 54,
			43, 27, 39, 23, 42, 26, 38, 22};
				int r = y * 8 + x;
				return dither[r] / 64; // same # of instructions as pre-dividing due to compiler magic
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

				float2 texCoord2052 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_2054_0 = distance( texCoord2052 , float2( 0.5,0.5 ) );
				float ifLocalVar2053 = 0;
				if( temp_output_2054_0 <= 1.0 )
				ifLocalVar2053 = 1.0;
				else
				ifLocalVar2053 = 5000.0;
				#ifdef _USEINFINITYEDGE_ON
				float4 staticSwitch1498 = ( v.vertex * ifLocalVar2053 );
				#else
				float4 staticSwitch1498 = v.vertex;
				#endif
				float4 break1605 = staticSwitch1498;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 break19 = mul( _DepthMatrix, float4( ( ase_worldPos - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D_LOD( _DepthCache, sampler_Linear_Clamp, CacheUV1109, 0.0 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_2 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_3 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_2) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_3 - temp_cast_2)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 temp_cast_4 = (_FoamNoiseSpeed).xx;
				float3 break35 = ase_worldPos;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = ase_worldPos;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner138 = ( 1.0 * _Time.y * temp_cast_4 + ( WorldXZ1087 * _FoamNoiseScale ));
				float lerpResult133 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( 0.04 * panner138 ), 0.0 ).r , _FoamNoiseCon);
				float2 temp_cast_6 = (_FoamNoiseSpeed).xx;
				float2 panner168 = ( 1.0 * _Time.y * temp_cast_6 + ( WorldXZ1087 * _FoamNoiseScale2 ));
				float lerpResult161 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( panner168 * 0.04 ), 0.0 ).r , _FoamNoiseCon2);
				float UnderOffset1634 = GlobalOceanOffset;
				float3 appendResult2127 = (float3(break1605.x , ( break1605.y + UnderOffset1634 ) , break1605.z));
				float3 VertResOffset2125 = appendResult2127;
				float3 customSurfaceDepth1169 = VertResOffset2125;
				float customEye1169 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1169)).z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				#ifdef _USEWAVENOISEDISPLACE_ON
				float staticSwitch680 = ( Foamness294 + ( ( lerpResult133 + lerpResult161 ) * ( ( 1.0 - DistanceFadeA1294 ) * 0.4 ) ) );
				#else
				float staticSwitch680 = Foamness294;
				#endif
				float DepthCacheRemap109 = staticSwitch680;
				float temp_output_5_0_g3212 = ( 1.0 - DepthCacheRemap109 );
				float mulTime209 = _TimeParameters.x * ( _WaveSpeed * 0.1 );
				float2 appendResult868 = (float2(_WaveDirectionSettings.x , _WaveDirectionSettings.y));
				float WaveCenterDistance1051 = distance( WorldXZ1087 , appendResult868 );
				#ifdef _USEWAVEDIRECTION_ON
				float staticSwitch762 = ( mulTime209 + ( ( WaveCenterDistance1051 * -0.01 ) / _WaveDirectionSettings.z ) );
				#else
				float staticSwitch762 = ( 0.0 - mulTime209 );
				#endif
				float temp_output_17_0_g3212 = staticSwitch762;
				float temp_output_30_0_g3212 = ( 1.0 - saturate( 0.7 ) );
				float temp_output_35_0_g3212 = ( temp_output_30_0_g3212 * 0.5 );
				float temp_output_32_0_g3212 = ( (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) - temp_output_35_0_g3212 );
				float temp_output_31_0_g3212 = ( temp_output_35_0_g3212 + (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) );
				float DepthCacheWave184 = ( 1.0 - frac( (0.0 + (temp_output_5_0_g3212 - temp_output_32_0_g3212) * (1.0 - 0.0) / (temp_output_31_0_g3212 - temp_output_32_0_g3212)) ) );
				float clampResult124 = clamp( DepthCacheWave184 , 0.01 , 0.99 );
				float2 temp_cast_7 = (clampResult124).xx;
				float4 tex2DNode76 = SAMPLE_TEXTURE2D_LOD( _FoamLUT, sampler_Linear_Clamp, temp_cast_7, 0.0 );
				float smoothstepResult279 = smoothstep( 0.0 , 1.0 , tex2DNode76.a);
				float3 customSurfaceDepth1325 = VertResOffset2125;
				float customEye1325 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1325)).z;
				float cameraDepthFade1325 = (( customEye1325 -_ProjectionParams.y - 0.0 ) / _DistanceFadeB);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2111 = 0.0;
				#else
				float staticSwitch2111 = saturate( cameraDepthFade1325 );
				#endif
				float DistanceFadeB1326 = staticSwitch2111;
				float lerpResult1311 = lerp( smoothstepResult279 , 0.0 , DistanceFadeB1326);
				float FoamVertDisp261 = saturate( lerpResult1311 );
				float temp_output_240_0 = ( FoamVertDisp261 * _VertDispMult * 0.1 );
				float ShoreMaskVert586 = saturate( (0.0 + (( CaptureB_Distancex100200 * 100.0 ) - _RemapVertShore.x) * (1.0 - 0.0) / (_RemapVertShore.y - _RemapVertShore.x)) );
				float VertDispDistMask22014 = ( 1.0 - saturate( (0.0 + (distance( WorldXZ1087 , (_WorldSpaceCameraPos).xz ) - 5.0) * (1.0 - 0.0) / (7.9 - 5.0)) ) );
				float3 customSurfaceDepth1484 = VertResOffset2125;
				float customEye1484 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1484)).z;
				float cameraDepthFade1484 = (( customEye1484 -_ProjectionParams.y - 0.0 ) / 1.0);
				float CamDepthFade1495 = cameraDepthFade1484;
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float VertDispMaskRes1691 = ( ShoreMaskVert586 * saturate( VertDispDistMask22014 ) * saturate( (0.0 + (CamDepthFade1495 - 0.2) * (1.0 - 0.0) / (1.5 - 0.2)) ) * InfinityFeather1410 );
				float temp_output_238_0 = ( temp_output_240_0 * VertDispMaskRes1691 );
				#ifdef _USEVERTDISPLACEMENT_ON
				float staticSwitch1688 = temp_output_238_0;
				#else
				float staticSwitch1688 = 0.0;
				#endif
				float VertAdded1524 = staticSwitch1688;
				float3 appendResult1606 = (float3(break1605.x , ( break1605.y + VertAdded1524 + UnderOffset1634 ) , break1605.z));
				float3 VertResDisplaced328 = appendResult1606;
				
				o.ase_texcoord.xyz = ase_worldPos;
				o.ase_texcoord1 = screenPos;
				o.ase_texcoord.w = customEye1169;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertResDisplaced328;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = float3(0,1,0);

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				o.clipPos = TransformWorldToHClip(positionWS);

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
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
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessNum; float tessMin = _TesMin; float tessMax = _TesMax;
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

			half4 frag(VertexOutput IN , bool ase_vface : SV_IsFrontFace) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float2 appendResult892 = (float2(_Motion2.x , _Motion2.y));
				float2 Motion2XY1096 = appendResult892;
				float3 ase_worldPos = IN.ase_texcoord.xyz;
				float3 break35 = ase_worldPos;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = ase_worldPos;
				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner890 = ( 0.05 * _Time.y * Motion2XY1096 + WorldXZ1087);
				#ifdef GlobalPlanarReflection
				float staticSwitch1591 = _NormalPow;
				#else
				float staticSwitch1591 = ( _NormalPow * 0.7 );
				#endif
				float temp_output_1436_0 = ( _NormalPow * 6.0 );
				float switchResult1435 = (((ase_vface>0)?(staticSwitch1591):(temp_output_1436_0)));
				float3 unpack11 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals, sampler_Normals, (panner890*_Normals_ST.xy + _Normals_ST.zw) ), switchResult1435 );
				unpack11.z = lerp( 1, unpack11.z, saturate(switchResult1435) );
				float3 tex2DNode11 = unpack11;
				float2 appendResult88 = (float2(_Motion.z , _Motion.w));
				float2 MotionZW1085 = appendResult88;
				float2 panner85 = ( 0.1 * _Time.y * MotionZW1085 + WorldXZ1087);
				float switchResult1432 = (((ase_vface>0)?(( _NormalPow2 * 2.0 )):(( _NormalPow2 * 12.0 ))));
				float3 unpack84 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner85*_Normals2_ST.xy + _Normals2_ST.zw) ), switchResult1432 );
				unpack84.z = lerp( 1, unpack84.z, saturate(switchResult1432) );
				float2 panner1722 = ( -0.05 * _Time.y * MotionZW1085 + WorldXZ1087);
				float3 unpack1718 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner1722*( _Normals2_ST.xy * float2( 0.5,0.5 ) ) + _Normals2_ST.zw) ), switchResult1432 );
				unpack1718.z = lerp( 1, unpack1718.z, saturate(switchResult1432) );
				float3 temp_output_1719_0 = BlendNormal( unpack84 , unpack1718 );
				float3 temp_output_83_0 = BlendNormal( tex2DNode11 , temp_output_1719_0 );
				#ifdef _USEEXTRANORMALS_ON
				float3 staticSwitch1683 = temp_output_83_0;
				#else
				float3 staticSwitch1683 = temp_output_83_0;
				#endif
				float2 appendResult1049 = (float2(staticSwitch1683.xy));
				float2 RefNorm886 = appendResult1049;
				float RefBumpSlider1337 = _RefDistortion;
				float3 normalizeResult2273 = normalize( ( staticSwitch1683 + float3( ( RefNorm886 * RefBumpSlider1337 ) ,  0.0 ) ) );
				float2 appendResult2519 = (float2(normalizeResult2273.xy));
				float3 break19 = mul( _DepthMatrix, float4( ( ase_worldPos - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D( _DepthCache, sampler_Linear_Clamp, CacheUV1109 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_6 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_7 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_6) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_7 - temp_cast_6)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 lerpResult2274 = lerp( float2( 0,0 ) , appendResult2519 , ( _WaterRefractonMobile * 0.5 * saturate( Foamness294 ) ));
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float customEye1169 = IN.ase_texcoord.w;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				float temp_output_1414_0 = ( ( 1.0 - InfinityFeather1410 ) * saturate( DistanceFadeA1294 ) );
				float2 lerpResult2275 = lerp( lerpResult2274 , float2( 0,0 ) , temp_output_1414_0);
				#ifdef _TRANSPARENTGRABPASS_ON
				float2 staticSwitch2542 = float2( 0,0 );
				#else
				float2 staticSwitch2542 = lerpResult2275;
				#endif
				float2 RefHackeryForColorCache2269 = staticSwitch2542;
				float2 temp_output_2198_0 = ( RefHackeryForColorCache2269 + CacheUV1109 );
				float4 tex2DNode657 = SAMPLE_TEXTURE2D( _ColorCache, sampler_Linear_Clamp, temp_output_2198_0 );
				float ColorCacheA2171 = tex2DNode657.a;
				float temp_output_12_0_g3214 = InfinityFeather1410;
				float CaptureDepthAOffsetShore1177 = ( ( ColorCacheA2171 * temp_output_12_0_g3214 ) + ( 1.0 - temp_output_12_0_g3214 ) );
				float4 break993 = _RemapAlpha;
				float switchResult2294 = (((ase_vface>0)?(saturate( (0.0 + (CaptureDepthAOffsetShore1177 - break993.x) * (1.0 - 0.0) / (break993.y - break993.x)) )):(1.0)));
				float ShoreMaskDepthAlpha141 = switchResult2294;
				float2 clipScreen639 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither639 = Dither8x8Bayer( fmod(clipScreen639.x, 8), fmod(clipScreen639.y, 8) );
				dither639 = step( dither639, ShoreMaskDepthAlpha141 );
				#ifdef _USEDITHERING_ON
				float staticSwitch1685 = dither639;
				#else
				float staticSwitch1685 = ShoreMaskDepthAlpha141;
				#endif
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch1748 = 1.0;
				#else
				float staticSwitch1748 = staticSwitch1685;
				#endif
				float HorizonAlpha2083 = temp_output_2080_0;
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2085 = HorizonAlpha2083;
				#else
				float staticSwitch2085 = staticSwitch1748;
				#endif
				float AlphaRes469 = staticSwitch2085;
				

				surfaceDescription.Alpha = AlphaRes469;
				surfaceDescription.AlphaClipThreshold = _ClipThreshold;

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

			#define ASE_TESSELLATION 1
			#pragma require tessellation tessHW
			#pragma hull HullFunction
			#pragma domain DomainFunction
			#define ASE_DISTANCE_TESSELLATION
			#define ASE_ABSOLUTE_VERTEX_POS 1
			#define _ASE_TONEMAPPING 1
			#define _ASE_TINT 1
			#define _ASE_DEBUGVISUALS 1
			#define ASE_FOG 1
			#define _NORMAL_DROPOFF_TS 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
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
			#pragma shader_feature_local ASE_TESSELLATION_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature_local LIGHTMAP_ON
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature_local _USEINFINITYEDGE_ON
			#pragma shader_feature_local _USEVERTDISPLACEMENT_ON
			#pragma shader_feature_local _USEWAVENOISEDISPLACE_ON
			#pragma shader_feature_local _SPACE_WORLDSPACE _SPACE_UVSPACE
			#pragma shader_feature_local _HORIZONMAPPED_ON
			#pragma shader_feature_local _USEWAVEDIRECTION_ON
			#pragma shader_feature_local _USEINFINITYFEATHER_ON
			#pragma shader_feature_local _TRANSPARENTGRABPASS_ON
			#pragma shader_feature_local _USEDITHERING_ON
			#pragma shader_feature_local _USEEXTRANORMALS_ON
			#pragma multi_compile __ GlobalPlanarReflection


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _FoamColor;
			float4 _Motion;
			float4 _Normals_ST;
			float4 _WaterTint;
			float4 _RemapVertShore;
			float4 _WaterAlbedo;
			float4 _WaveDirectionSettings;
			float4 _Normals2_ST;
			float4 _Motion2;
			float4 _RefFresnel;
			float4 _CausticsTint;
			float4x4 _DepthMatrix;
			float4 _RemapAlpha;
			float4 _BakeLightMapAmbient;
			float4x4 _BakeMatrix;
			float4 _bakeLightmapST;
			float4 _RemapFoam;
			float3 _WorldDir;
			float3 _WorldPos;
			float3 _DepthPosition;
			float3 _CameraForward;
			float _WaveRefractionStrength;
			float _RefractionWarp;
			float _RemapWaterDistance;
			float _RemapWaterDistance1;
			float _Smoothness;
			float _WaterDepthPow;
			float _WaterSaturation;
			float _CubemapExposure;
			float _NormalDarken1;
			float _NormalDarken;
			float _ForceEye;
			float _ClipThreshold;
			float _WaterDepthPow1;
			int _ZTestMode;
			float _NormalPow2;
			float _FoamMaxDistance;
			float _ZWriteToggle;
			float _TessNum;
			float _TesMin;
			float _TesMax;
			float _WaveScale;
			float _FoamNoiseSpeed;
			float _FoamNoiseScale;
			float _FoamNoiseCon;
			float _FoamNoiseScale2;
			float _FoamNoiseCon2;
			float _DistanceFadeA;
			float _WaveSpeed;
			float _DistanceFadeB;
			float _VertDispMult;
			float _Tolerance;
			float _Feather;
			float _FoamScale_UseFoam;
			float _NormalPow;
			float _BakedLightingFeather;
			float _RefDistortion;
			float _WaterRefractonMobile;
			float _FoamPow;
			int _DebugVisual;
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

			float4 _skyGradientColor1;
			float4 _skyGradientColor2;
			float3 _CausticsDir;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_FoamLUT);
			TEXTURE2D(_DepthCache);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE2D(_WaveNoiseDisplace);
			SAMPLER(sampler_WaveNoiseDisplace);
			TEXTURE2D(_ColorCache);
			TEXTURE2D(_Normals);
			SAMPLER(sampler_Normals);
			TEXTURE2D(_Normals2);
			SAMPLER(sampler_Normals2);


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
			
			float4 Billboard2078( float3 worldPosIN, float2 screenPos, float3 QuadNormal, float3 QuadPosition )
			{
				float3 worldPos = worldPosIN;
				float3 viewDir = worldPos - _WorldSpaceCameraPos;
				float3 reflectDir = -viewDir;
				float3 nReflDirection = normalize(reflectDir);
				float4 result = float4(0,0,0,0);
				// Ray-quad intersection
				half planeIntersectDistance = dot( worldPos - QuadPosition,  QuadNormal.xyz ) / dot( nReflDirection, QuadNormal.xyz );
				half3 intersectPosition = worldPos - nReflDirection * planeIntersectDistance;
				//half4 localPlaneIntersectPosition = mul( QuadInverseMatrix, float4( intersectPosition, 1.0 ));
				half4 localPlaneIntersectPosition = float4( intersectPosition, 1.0 );
				half2 billboardUV = half2( localPlaneIntersectPosition.x, localPlaneIntersectPosition.z );
				//billboardUV*=1;
				//float4 billboardCol = BillboardTex.Sample( BillboardSampler , billboardUV );
				float4 billboardCol = float4( billboardUV.x, billboardUV.y, 0, 1);
				float3 quadNorm = QuadNormal.xyz;
				half reflectDot = dot( nReflDirection, quadNorm );
				float w = 1;
				// Cull when beyond the quad bounds
				//w = billboardUV.x > 1.0 ? 0.0 : w;
				//w = billboardUV.y > 1.0 ? 0.0 : w;
				//w = billboardUV.x < 0.0 ? 0.0 : w;
				//w = billboardUV.y < 0.0 ? 0.0 : w;
				// Cull if distance is behind the surface
				//w = planeIntersectDistance < 0 ? 0 : w;
				// Cull if quad facing backwards
				w = reflectDot <= 0.0 ? 0.0 : w; 
				// Cull if quad facing backwards and culling is ticked on the quad ( passed in via the arrays W )
				//w = QuadNormal.w == 1.0 ? (reflectDot <= 0.0 ? 0.0 : w) : w;
				billboardCol = billboardCol * billboardCol.w * w;
				billboardCol.a = w;
				result*=1-w;
				result+= billboardCol;
				return result;
			}
			
			inline float Dither8x8Bayer( int x, int y )
			{
				const float dither[ 64 ] = {
			 1, 49, 13, 61,  4, 52, 16, 64,
			33, 17, 45, 29, 36, 20, 48, 32,
			 9, 57,  5, 53, 12, 60,  8, 56,
			41, 25, 37, 21, 44, 28, 40, 24,
			 3, 51, 15, 63,  2, 50, 14, 62,
			35, 19, 47, 31, 34, 18, 46, 30,
			11, 59,  7, 55, 10, 58,  6, 54,
			43, 27, 39, 23, 42, 26, 38, 22};
				int r = y * 8 + x;
				return dither[r] / 64; // same # of instructions as pre-dividing due to compiler magic
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

				float2 texCoord2052 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_2054_0 = distance( texCoord2052 , float2( 0.5,0.5 ) );
				float ifLocalVar2053 = 0;
				if( temp_output_2054_0 <= 1.0 )
				ifLocalVar2053 = 1.0;
				else
				ifLocalVar2053 = 5000.0;
				#ifdef _USEINFINITYEDGE_ON
				float4 staticSwitch1498 = ( v.vertex * ifLocalVar2053 );
				#else
				float4 staticSwitch1498 = v.vertex;
				#endif
				float4 break1605 = staticSwitch1498;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 break19 = mul( _DepthMatrix, float4( ( ase_worldPos - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D_LOD( _DepthCache, sampler_Linear_Clamp, CacheUV1109, 0.0 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_2 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_3 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_2) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_3 - temp_cast_2)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 temp_cast_4 = (_FoamNoiseSpeed).xx;
				float3 break35 = ase_worldPos;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = ase_worldPos;
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner138 = ( 1.0 * _Time.y * temp_cast_4 + ( WorldXZ1087 * _FoamNoiseScale ));
				float lerpResult133 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( 0.04 * panner138 ), 0.0 ).r , _FoamNoiseCon);
				float2 temp_cast_6 = (_FoamNoiseSpeed).xx;
				float2 panner168 = ( 1.0 * _Time.y * temp_cast_6 + ( WorldXZ1087 * _FoamNoiseScale2 ));
				float lerpResult161 = lerp( 1.0 , SAMPLE_TEXTURE2D_LOD( _WaveNoiseDisplace, sampler_WaveNoiseDisplace, ( panner168 * 0.04 ), 0.0 ).r , _FoamNoiseCon2);
				float UnderOffset1634 = GlobalOceanOffset;
				float3 appendResult2127 = (float3(break1605.x , ( break1605.y + UnderOffset1634 ) , break1605.z));
				float3 VertResOffset2125 = appendResult2127;
				float3 customSurfaceDepth1169 = VertResOffset2125;
				float customEye1169 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1169)).z;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				#ifdef _USEWAVENOISEDISPLACE_ON
				float staticSwitch680 = ( Foamness294 + ( ( lerpResult133 + lerpResult161 ) * ( ( 1.0 - DistanceFadeA1294 ) * 0.4 ) ) );
				#else
				float staticSwitch680 = Foamness294;
				#endif
				float DepthCacheRemap109 = staticSwitch680;
				float temp_output_5_0_g3212 = ( 1.0 - DepthCacheRemap109 );
				float mulTime209 = _TimeParameters.x * ( _WaveSpeed * 0.1 );
				float2 appendResult868 = (float2(_WaveDirectionSettings.x , _WaveDirectionSettings.y));
				float WaveCenterDistance1051 = distance( WorldXZ1087 , appendResult868 );
				#ifdef _USEWAVEDIRECTION_ON
				float staticSwitch762 = ( mulTime209 + ( ( WaveCenterDistance1051 * -0.01 ) / _WaveDirectionSettings.z ) );
				#else
				float staticSwitch762 = ( 0.0 - mulTime209 );
				#endif
				float temp_output_17_0_g3212 = staticSwitch762;
				float temp_output_30_0_g3212 = ( 1.0 - saturate( 0.7 ) );
				float temp_output_35_0_g3212 = ( temp_output_30_0_g3212 * 0.5 );
				float temp_output_32_0_g3212 = ( (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) - temp_output_35_0_g3212 );
				float temp_output_31_0_g3212 = ( temp_output_35_0_g3212 + (( 1.0 + temp_output_30_0_g3212 ) + (abs( temp_output_17_0_g3212 ) - 0.0) * (( 0.0 - temp_output_30_0_g3212 ) - ( 1.0 + temp_output_30_0_g3212 )) / (1.0 - 0.0)) );
				float DepthCacheWave184 = ( 1.0 - frac( (0.0 + (temp_output_5_0_g3212 - temp_output_32_0_g3212) * (1.0 - 0.0) / (temp_output_31_0_g3212 - temp_output_32_0_g3212)) ) );
				float clampResult124 = clamp( DepthCacheWave184 , 0.01 , 0.99 );
				float2 temp_cast_7 = (clampResult124).xx;
				float4 tex2DNode76 = SAMPLE_TEXTURE2D_LOD( _FoamLUT, sampler_Linear_Clamp, temp_cast_7, 0.0 );
				float smoothstepResult279 = smoothstep( 0.0 , 1.0 , tex2DNode76.a);
				float3 customSurfaceDepth1325 = VertResOffset2125;
				float customEye1325 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1325)).z;
				float cameraDepthFade1325 = (( customEye1325 -_ProjectionParams.y - 0.0 ) / _DistanceFadeB);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2111 = 0.0;
				#else
				float staticSwitch2111 = saturate( cameraDepthFade1325 );
				#endif
				float DistanceFadeB1326 = staticSwitch2111;
				float lerpResult1311 = lerp( smoothstepResult279 , 0.0 , DistanceFadeB1326);
				float FoamVertDisp261 = saturate( lerpResult1311 );
				float temp_output_240_0 = ( FoamVertDisp261 * _VertDispMult * 0.1 );
				float ShoreMaskVert586 = saturate( (0.0 + (( CaptureB_Distancex100200 * 100.0 ) - _RemapVertShore.x) * (1.0 - 0.0) / (_RemapVertShore.y - _RemapVertShore.x)) );
				float VertDispDistMask22014 = ( 1.0 - saturate( (0.0 + (distance( WorldXZ1087 , (_WorldSpaceCameraPos).xz ) - 5.0) * (1.0 - 0.0) / (7.9 - 5.0)) ) );
				float3 customSurfaceDepth1484 = VertResOffset2125;
				float customEye1484 = -TransformWorldToView(TransformObjectToWorld(customSurfaceDepth1484)).z;
				float cameraDepthFade1484 = (( customEye1484 -_ProjectionParams.y - 0.0 ) / 1.0);
				float CamDepthFade1495 = cameraDepthFade1484;
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float VertDispMaskRes1691 = ( ShoreMaskVert586 * saturate( VertDispDistMask22014 ) * saturate( (0.0 + (CamDepthFade1495 - 0.2) * (1.0 - 0.0) / (1.5 - 0.2)) ) * InfinityFeather1410 );
				float temp_output_238_0 = ( temp_output_240_0 * VertDispMaskRes1691 );
				#ifdef _USEVERTDISPLACEMENT_ON
				float staticSwitch1688 = temp_output_238_0;
				#else
				float staticSwitch1688 = 0.0;
				#endif
				float VertAdded1524 = staticSwitch1688;
				float3 appendResult1606 = (float3(break1605.x , ( break1605.y + VertAdded1524 + UnderOffset1634 ) , break1605.z));
				float3 VertResDisplaced328 = appendResult1606;
				
				o.ase_texcoord.xyz = ase_worldPos;
				o.ase_texcoord1 = screenPos;
				o.ase_texcoord.w = customEye1169;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertResDisplaced328;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = float3(0,1,0);

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				o.clipPos = TransformWorldToHClip(positionWS);

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
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
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessNum; float tessMin = _TesMin; float tessMax = _TesMax;
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

			half4 frag(VertexOutput IN , bool ase_vface : SV_IsFrontFace) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float2 appendResult892 = (float2(_Motion2.x , _Motion2.y));
				float2 Motion2XY1096 = appendResult892;
				float3 ase_worldPos = IN.ase_texcoord.xyz;
				float3 break35 = ase_worldPos;
				float2 appendResult36 = (float2(break35.x , break35.z));
				float3 worldPosIN2078 = ase_worldPos;
				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 screenPos2078 = ase_screenPosNorm.xy;
				float3 QuadNormal2078 = float3( 0,1,0 );
				float3 QuadPosition2078 = float3(0,-4,0);
				float4 localBillboard2078 = Billboard2078( worldPosIN2078 , screenPos2078 , QuadNormal2078 , QuadPosition2078 );
				float temp_output_2080_0 = (localBillboard2078).w;
				float2 temp_output_2096_0 = ( (localBillboard2078).xy * temp_output_2080_0 );
				#ifdef _HORIZONMAPPED_ON
				float2 staticSwitch2082 = temp_output_2096_0;
				#else
				float2 staticSwitch2082 = appendResult36;
				#endif
				float2 WorldXZ1087 = staticSwitch2082;
				float2 panner890 = ( 0.05 * _Time.y * Motion2XY1096 + WorldXZ1087);
				#ifdef GlobalPlanarReflection
				float staticSwitch1591 = _NormalPow;
				#else
				float staticSwitch1591 = ( _NormalPow * 0.7 );
				#endif
				float temp_output_1436_0 = ( _NormalPow * 6.0 );
				float switchResult1435 = (((ase_vface>0)?(staticSwitch1591):(temp_output_1436_0)));
				float3 unpack11 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals, sampler_Normals, (panner890*_Normals_ST.xy + _Normals_ST.zw) ), switchResult1435 );
				unpack11.z = lerp( 1, unpack11.z, saturate(switchResult1435) );
				float3 tex2DNode11 = unpack11;
				float2 appendResult88 = (float2(_Motion.z , _Motion.w));
				float2 MotionZW1085 = appendResult88;
				float2 panner85 = ( 0.1 * _Time.y * MotionZW1085 + WorldXZ1087);
				float switchResult1432 = (((ase_vface>0)?(( _NormalPow2 * 2.0 )):(( _NormalPow2 * 12.0 ))));
				float3 unpack84 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner85*_Normals2_ST.xy + _Normals2_ST.zw) ), switchResult1432 );
				unpack84.z = lerp( 1, unpack84.z, saturate(switchResult1432) );
				float2 panner1722 = ( -0.05 * _Time.y * MotionZW1085 + WorldXZ1087);
				float3 unpack1718 = UnpackNormalScale( SAMPLE_TEXTURE2D( _Normals2, sampler_Normals2, (panner1722*( _Normals2_ST.xy * float2( 0.5,0.5 ) ) + _Normals2_ST.zw) ), switchResult1432 );
				unpack1718.z = lerp( 1, unpack1718.z, saturate(switchResult1432) );
				float3 temp_output_1719_0 = BlendNormal( unpack84 , unpack1718 );
				float3 temp_output_83_0 = BlendNormal( tex2DNode11 , temp_output_1719_0 );
				#ifdef _USEEXTRANORMALS_ON
				float3 staticSwitch1683 = temp_output_83_0;
				#else
				float3 staticSwitch1683 = temp_output_83_0;
				#endif
				float2 appendResult1049 = (float2(staticSwitch1683.xy));
				float2 RefNorm886 = appendResult1049;
				float RefBumpSlider1337 = _RefDistortion;
				float3 normalizeResult2273 = normalize( ( staticSwitch1683 + float3( ( RefNorm886 * RefBumpSlider1337 ) ,  0.0 ) ) );
				float2 appendResult2519 = (float2(normalizeResult2273.xy));
				float3 break19 = mul( _DepthMatrix, float4( ( ase_worldPos - _DepthPosition ) , 0.0 ) ).xyz;
				float2 appendResult16 = (float2(break19.x , break19.z));
				float2 texCoord1345 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				#if defined(_SPACE_WORLDSPACE)
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#elif defined(_SPACE_UVSPACE)
				float2 staticSwitch1346 = texCoord1345;
				#else
				float2 staticSwitch1346 = ( float2( 0.5,0.5 ) + appendResult16 );
				#endif
				float2 CacheUV1109 = staticSwitch1346;
				float4 tex2DNode13 = SAMPLE_TEXTURE2D( _DepthCache, sampler_Linear_Clamp, CacheUV1109 );
				float temp_output_17_0_g3203 = 0.001;
				float2 temp_cast_6 = (temp_output_17_0_g3203).xx;
				float2 temp_cast_7 = (( 1.0 - temp_output_17_0_g3203 )).xx;
				float2 break5_g3203 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_6) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_7 - temp_cast_6)) );
				float temp_output_1_0_g3205 = break5_g3203.x;
				float temp_output_8_0_g3203 = ( 0.001 + -1.0 );
				float temp_output_1_0_g3204 = break5_g3203.y;
				float temp_output_2766_0 = saturate( saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3205 - floor( ( temp_output_1_0_g3205 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3204 - floor( ( temp_output_1_0_g3204 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3203 - -1.0)) ) ) ) );
				float lerpResult3090 = lerp( 1000.0 , ( tex2DNode13.b * 100.0 ) , temp_output_2766_0);
				float CaptureB_Distancex100200 = lerpResult3090;
				float temp_output_307_0 = ( 1.0 - saturate( abs( CaptureB_Distancex100200 ) ) );
				float Foamness294 = ( saturate( ( ( temp_output_307_0 + pow( pow( pow( temp_output_307_0 , 5.0 ) , 5.0 ) , 5.0 ) + pow( temp_output_307_0 , 15.0 ) ) * 0.2 ) ) * _WaveScale );
				float2 lerpResult2274 = lerp( float2( 0,0 ) , appendResult2519 , ( _WaterRefractonMobile * 0.5 * saturate( Foamness294 ) ));
				float temp_output_17_0_g3206 = 0.01;
				float2 temp_cast_8 = (temp_output_17_0_g3206).xx;
				float2 temp_cast_9 = (( 1.0 - temp_output_17_0_g3206 )).xx;
				float2 break5_g3206 = saturate( (float2( 0,0 ) + (saturate( CacheUV1109 ) - temp_cast_8) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_9 - temp_cast_8)) );
				float temp_output_1_0_g3208 = break5_g3206.x;
				float temp_output_8_0_g3206 = ( 0.1 + -1.0 );
				float temp_output_1_0_g3207 = break5_g3206.y;
				float temp_output_2768_0 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3208 - floor( ( temp_output_1_0_g3208 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3207 - floor( ( temp_output_1_0_g3207 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3206 - -1.0)) ) ) );
				float temp_output_2763_0 = saturate( ( temp_output_2768_0 * (1.0 + (CaptureB_Distancex100200 - 0.0) * (0.0 - 1.0) / (0.5 - 0.0)) ) );
				float temp_output_17_0_g3209 = _Tolerance;
				float2 temp_cast_10 = (temp_output_17_0_g3209).xx;
				float2 temp_cast_11 = (( 1.0 - temp_output_17_0_g3209 )).xx;
				float2 break5_g3209 = saturate( (float2( 0,0 ) + (CacheUV1109 - temp_cast_10) * (float2( 1,1 ) - float2( 0,0 )) / (temp_cast_11 - temp_cast_10)) );
				float temp_output_1_0_g3211 = break5_g3209.x;
				float temp_output_8_0_g3209 = ( _Feather + -1.0 );
				float temp_output_1_0_g3210 = break5_g3209.y;
				#ifdef _USEINFINITYFEATHER_ON
				float staticSwitch1654 = saturate( ( saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3211 - floor( ( temp_output_1_0_g3211 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) * saturate( (0.0 + (( ( abs( ( ( temp_output_1_0_g3210 - floor( ( temp_output_1_0_g3210 + 0.5 ) ) ) * 2 ) ) * 2 ) - 1.0 ) - -1.0) * (0.5 - 0.0) / (temp_output_8_0_g3209 - -1.0)) ) ) );
				#else
				float staticSwitch1654 = 1.0;
				#endif
				float InfinityFeather1410 = saturate( ( temp_output_2763_0 + staticSwitch1654 ) );
				float customEye1169 = IN.ase_texcoord.w;
				float cameraDepthFade1169 = (( customEye1169 -_ProjectionParams.y - 0.0 ) / _DistanceFadeA);
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2110 = 0.0;
				#else
				float staticSwitch2110 = saturate( cameraDepthFade1169 );
				#endif
				float DistanceFadeA1294 = staticSwitch2110;
				float temp_output_1414_0 = ( ( 1.0 - InfinityFeather1410 ) * saturate( DistanceFadeA1294 ) );
				float2 lerpResult2275 = lerp( lerpResult2274 , float2( 0,0 ) , temp_output_1414_0);
				#ifdef _TRANSPARENTGRABPASS_ON
				float2 staticSwitch2542 = float2( 0,0 );
				#else
				float2 staticSwitch2542 = lerpResult2275;
				#endif
				float2 RefHackeryForColorCache2269 = staticSwitch2542;
				float2 temp_output_2198_0 = ( RefHackeryForColorCache2269 + CacheUV1109 );
				float4 tex2DNode657 = SAMPLE_TEXTURE2D( _ColorCache, sampler_Linear_Clamp, temp_output_2198_0 );
				float ColorCacheA2171 = tex2DNode657.a;
				float temp_output_12_0_g3214 = InfinityFeather1410;
				float CaptureDepthAOffsetShore1177 = ( ( ColorCacheA2171 * temp_output_12_0_g3214 ) + ( 1.0 - temp_output_12_0_g3214 ) );
				float4 break993 = _RemapAlpha;
				float switchResult2294 = (((ase_vface>0)?(saturate( (0.0 + (CaptureDepthAOffsetShore1177 - break993.x) * (1.0 - 0.0) / (break993.y - break993.x)) )):(1.0)));
				float ShoreMaskDepthAlpha141 = switchResult2294;
				float2 clipScreen639 = ase_screenPosNorm.xy * _ScreenParams.xy;
				float dither639 = Dither8x8Bayer( fmod(clipScreen639.x, 8), fmod(clipScreen639.y, 8) );
				dither639 = step( dither639, ShoreMaskDepthAlpha141 );
				#ifdef _USEDITHERING_ON
				float staticSwitch1685 = dither639;
				#else
				float staticSwitch1685 = ShoreMaskDepthAlpha141;
				#endif
				#ifdef _TRANSPARENTGRABPASS_ON
				float staticSwitch1748 = 1.0;
				#else
				float staticSwitch1748 = staticSwitch1685;
				#endif
				float HorizonAlpha2083 = temp_output_2080_0;
				#ifdef _HORIZONMAPPED_ON
				float staticSwitch2085 = HorizonAlpha2083;
				#else
				float staticSwitch2085 = staticSwitch1748;
				#endif
				float AlphaRes469 = staticSwitch2085;
				

				surfaceDescription.Alpha = AlphaRes469;
				surfaceDescription.AlphaClipThreshold = _ClipThreshold;

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
	FallBack "Hidden/Shader Graph/FallbackError"
	
	Fallback Off
}
