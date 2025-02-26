// Made with Amplify Shader Editor v1.9.3.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ThunderRoad/Dev/River - Dev"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector]_CameraForward("_CameraForward", Vector) = (0,0,0,0)
		[Toggle(_PROBEVOLUME_ON)] _UseProbeVolume("Use Probe Volume", Float) = 0
		HeaderCaustics("# Caustics", Float) = 0
		[HDR]_CausticsTint("Caustics Tint [_CausticsEnable]", Color) = (1,1,1,1)
		[Feature(_CAUSTICSENABLE)]HeaderOceanFog("# Ocean Fog Caustics", Float) = 0
		[HideInInspector]_TexRight("TexRight", 2D) = "black" {}
		[HideInInspector]_TexLeft("TexLeft", 2D) = "black" {}
		[HideInInspector]_WorldPos("WorldPos", Vector) = (0,0,0,0)
		[HideInInspector]_WorldDir("WorldDir", Vector) = (0,0,0,0)
		[HideInInspector]_ForceEye("ForceEye", Float) = 0
		[Toggle(_DEBUGVISUALS_ON)] _DebugVisuals("DebugVisuals", Float) = 0
		[MaterialEnumDrawerExtended(ColorCache_RGB,0,ColorCache_A_Depth,1,DepthCache_RG_Dir,2,DepthCache_B_Dist,3,DepthCache_A_Depth,4,FluidColorRGB,5,FlowRainbow,6,FlowMix,7,FluidColorA,8,FluidVelA,9,FlowWaterTex,10,FlowWaterTexRes,11,ColorCacheRGB,12,ColorCacheA,13,ShoreMask,14)]_DebugVisual("-DebugVisual [_DebugVisuals]", Int) = 0
		HeaderGeneral("# General", Float) = 0
		[MainColor]_Color("Color", Color) = (0.15,0.15,0.15,1)
		[MainColor]_BaseColor("Color Baked", Color) = (1,1,1,0)
		_Specular("Specular", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		HeaderFlow("# Flow", Float) = 0
		_FlowTex("FlowTex &", 2D) = "white" {}
		[NoScaleOffset]_FluidColor("FluidColor &", 2D) = "white" {}
		_FluidVelocity("FluidVelocity &", 2D) = "linearGrey" {}
		[Toggle(_USEFLUIDFORFOAM_ON)] _UseFluidForFoam("UseFluidForFoam", Float) = 0
		_NormalMap("NormalMap &", 2D) = "bump" {}
		HeaderFoam3("## Flow Normals [_NormalMap]", Float) = 0
		_NormFlowStrength("Flow Strength (Normals) [_NormalMap]", Range( 0 , 1)) = 0
		_NormalSize("Flow Size (Normals) [_NormalMap]", Range( 0 , 50)) = 1
		_FlowSpeedN("Flow Speed (Normals) [_NormalMap]", Range( 0 , 5)) = 0.2
		_NormalStrength("Flow Norm Strength [_NormalMap]", Range( 0 , 2)) = 0.3574637
		HeaderFoam1("## Foam", Float) = 0
		_FoamFlowStrength("Flow Strength (Foam)", Range( 0 , 10)) = 1
		_FoamSize("Flow Size (Foam)", Range( 0 , 100)) = 1
		_FoamSize2("Flow Size (Foam) 2", Range( 0 , 100)) = 1
		_FoamFlowSpeed("Flow Speed (Foam)", Range( 0 , 1)) = 0.2
		HeaderFoam2("## Flow", Float) = 0
		_FlowStrength("Flow Strength", Range( 0 , 1)) = 0
		_Size("Flow Size", Range( 0 , 50)) = 1
		_FlowSpeed("Flow Speed", Range( 0 , 5)) = 0.2
		HeaderFoam4("## Flow Caustics [_CausticsEnable]", Float) = 0
		_CausticRotate("Caustic Rotate", Float) = 0
		_CausticFlowStrength("Caustic Flow Strength [_CausticsEnable]", Range( 0 , 10)) = 1
		_CausticFlowSpeed("Caustic Flow Speed [_CausticsEnable]", Range( 0 , 1)) = 0.5
		HeaderRefraction("# Refraction", Float) = 0
		[Toggle(_REFRACTION_ON)] _Refraction("Refraction (Grabpass Desktop)", Float) = 1
		[Feature(_REFRACTION)]_RefractionIndex("-Refraction Index [_Refraction]", Float) = 0.2
		[Feature(_REFRACTION)]_RefractionIndexUnder("-Refraction Index Under [_Refraction]", Float) = 5
		_RefractionColor("Refraction Color", Color) = (1,1,1,0)
		_RefractionColorMobile("Refraction Color (Mobile)", Color) = (1,1,1,0)
		_RefractionFake("Refraction Fake (Mobile)", Range( 0 , 50)) = 1
		_RefractionIndexUnderMobile("Refraction Index Under (Mobile)", Range( 0 , 50)) = 5
		_CubemapStrength("Cubemap Strength", Range( 0 , 1)) = 1
		_ColorMix("Color Mix Front (Mobile)", Range( 0 , 1)) = 1
		_ColorMixBack("Color Mix Back (Mobile)", Range( 0 , 1)) = 1
		[NoScaleOffset]_ColorCache("Color Cache &", 2D) = "black" {}
		HeaderFoam("# Foam", Float) = 0
		_FoamTex("Foam Tex &", 2D) = "white" {}
		_Foam_Mix("Foam Mix", Range( 0 , 1)) = 0.675
		_FoamPowA("Foam Pow A", Range( 0 , 4)) = 1.5
		_FoamPowM("Foam Pow M", Range( 0 , 10)) = 1
		_FoamColor2("Foam Color 2", Color) = (1,1,1,1)
		_FoamColor("Foam Color", Color) = (1,1,1,1)
		HeaderDepthColoring("# Depth Coloring", Float) = 0
		[Toggle(_USEDEPTHCOLORINGLUT_ON)] _UseDepthColoringLUT("Use Depth Coloring LUT", Float) = 1
		[HDR]_DepthColorLUT("DepthColor  LUT", 2D) = "white" {}
		_DRAWERGradient_DepthColorLUT("!DRAWER Gradient _DepthColorLUT", Float) = 0
		_WaterDepthPow("-Water Depth Pow", Range( 0 , 10)) = 6
		_WaterDepthPow1("-Water Depth Pow Mobile", Range( 0 , 10)) = 6
		_RemapWaterDistance("-Remap Water Distance", Range( 0 , 20)) = 2
		_RemapWaterDistance1("-Remap Water Distance Mobile", Range( 0 , 20)) = 2
		_WaterSaturation("Water Saturation", Range( 0 , 1)) = 0.9
		HeaderMisc("# Misc", Float) = 0
		_ReflectionShadowed("Reflection Shadowed", Range( 0 , 1)) = 0.8
		_ReflectionShadowedStart("Reflection Shadowed Start", Range( 0 , 1)) = 0
		_ReflectionShadowedEnd("Reflection Shadowed End", Range( 0 , 1)) = 1
		_DepthFade("DepthFade", Float) = 1
		[Toggle(_USEDEPTHFADE_ON)] _UseDepthFade("UseDepthFade", Float) = 1
		[Toggle]_ZWriteToggle("ZWriteToggle", Range( 0 , 1)) = 1
		[Toggle(_PLANARREFLECTION_ON)] _PlanarReflection("Use Planar Reflection", Float) = 0
		_Foam2Pow("Foam 2 Pow", Range( 0 , 4)) = 2
		_Foam2Con("Foam 2 Con", Range( 1 , 8)) = 1
		_PlanarNormalStrengthm("Planar Normal Strength", Range( 0 , 1)) = 1
		[NoScaleOffset]_DepthCache("DepthCache &", 2D) = "white" {}
		_RefFresnel("Reflection Fresnel", Vector) = (0,1,5,0)
		[Feature(_PROBEVOLUME)]_PRVOL("# Probe Volumes", Float) = 0
		_ProbeVolumeAmbient("ProbeVolumeAmbient & [_PROBEVOLUME_ON]", 3D) = "black" {}
		_ProbeVolumeShR("ProbeVolumeShR & [_PROBEVOLUME_ON]", 3D) = "black" {}
		_ProbeVolumeShG("ProbeVolumeShG & [_PROBEVOLUME_ON]", 3D) = "black" {}
		[HideInInspector]_ProbeVolumeMin("ProbeVolumeMin", Vector) = (0,0,0,0)
		_ProbeVolumeShB("ProbeVolumeShB & [_PROBEVOLUME_ON]", 3D) = "black" {}
		_ProbeVolumeOcc("ProbeVolumeOcc & [_PROBEVOLUME_ON]", 3D) = "black" {}
		[HideInInspector]_ProbeVolumeSizeInv("ProbeVolumeSizeInv", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}


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

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Transparent-1" "UniversalMaterialType"="Lit" }

		Cull Off
		ZWrite [_ZWriteToggle]
		ZTest LEqual
		Offset 0,0
		AlphaToMask On

		Stencil
		{
			Ref 255
			CompFront NotEqual
			CompBack Always
		}

		HLSLINCLUDE
		#pragma target 3.5
		#pragma prefer_hlslcc gles
		#pragma exclude_renderers xboxseries playstation ps4 switch // ensure rendering platforms toggle list is visible

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
			Tags { "LightMode"="UniversalForward" }

			Blend One Zero, One Zero
			//BlendOp [_BlendOp]// Shadowood
			ZWrite [_ZWriteToggle]
			ZTest LEqual
			Offset 0,0
			//ColorMask RGBA

			

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define _ASE_DEBUGVISUALS 1
			#define _ASE_TONEMAPPING 1
			#define _NORMAL_DROPOFF_TS 1
			#define _ASE_TINT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define _SPECULAR_SETUP 1
			#pragma shader_feature_local_fragment _SPECULAR_SETUP
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define REQUIRE_DEPTH_TEXTURE 1
			#define REQUIRE_OPAQUE_TEXTURE 1
			#define UNDERWATERELSEWHERE
			#define TONEMAPPINGELSEWHERE
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

			#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_BITANGENT
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_TANGENT
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma multi_compile_local __ _PROBEVOLUME_ON
			#pragma shader_feature_local _USEFLUIDFORFOAM_ON
			#pragma shader_feature_local _REFRACTION_ON
			#pragma shader_feature_local _USEDEPTHFADE_ON
			#pragma shader_feature_local _USEDEPTHCOLORINGLUT_ON
			#pragma shader_feature _PLANARREFLECTION_ON
			#pragma shader_feature_local _ProbeVolPerVertex
			#pragma shader_feature_local _FadeWithHeight


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
				float4 ase_texcoord14 : TEXCOORD14;
				float4 ase_texcoord15 : TEXCOORD15;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColor;
			float4 _Color;
			float4 _RefFresnel;
			float4 _FoamColor;
			float4 _RefractionColorMobile;
			float4 _CausticsTint;
			float4x4 _ProbeWorldToTexture;
			float4 _RefractionColor;
			float4 _FoamColor2;
			float3 _ProbeVolumeSizeInv;
			float3 _CameraForward;
			float3 _WorldPos;
			float3 _WorldDir;
			float3 _ProbeVolumeMin;
			float _Smoothness;
			float _RefractionIndexUnderMobile;
			float _CubemapStrength;
			float _ForceEye;
			float _PlanarNormalStrengthm;
			float _ColorMix;
			float _ColorMixBack;
			float _ReflectionShadowedEnd;
			float _WaterSaturation;
			float _CausticRotate;
			float _CausticFlowStrength;
			float _CausticFlowSpeed;
			float _Specular;
			float _ReflectionShadowedStart;
			float _WaterDepthPow;
			float _RemapWaterDistance;
			float _ReflectionShadowed;
			float _ZWriteToggle;
			float _Size;
			float _FlowStrength;
			float _FlowSpeed;
			float _FoamPowA;
			float _Foam2Pow;
			float _Foam2Con;
			float _FoamSize;
			float _FoamFlowStrength;
			float _FoamFlowSpeed;
			float _WaterDepthPow1;
			float _Foam_Mix;
			float _FoamPowM;
			float _RefractionIndex;
			float _RefractionIndexUnder;
			float _NormalSize;
			float _NormFlowStrength;
			float _FlowSpeedN;
			float _NormalStrength;
			float _DepthFade;
			float _RefractionFake;
			float _RemapWaterDistance1;
			float _FoamSize2;
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

			float3 _CausticsDir;
			half4 _CausticsColor;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE3D(_ProbeVolumeShR);
			SAMPLER(sampler_ProbeVolumeShR);
			TEXTURE3D(_ProbeVolumeShG);
			SAMPLER(sampler_ProbeVolumeShG);
			TEXTURE3D(_ProbeVolumeAmbient);
			SAMPLER(sampler_ProbeVolumeAmbient);
			TEXTURE3D(_ProbeVolumeShB);
			SAMPLER(sampler_ProbeVolumeShB);
			TEXTURE3D(_ProbeVolumeOcc);
			SAMPLER(sampler_ProbeVolumeOcc);
			TEXTURE2D(_FlowTex);
			TEXTURE2D(_FluidVelocity);
			SAMPLER(sampler_FluidVelocity);
			SAMPLER(sampler_Linear_Repeat);
			TEXTURE2D(_FluidColor);
			SAMPLER(sampler_FluidColor);
			TEXTURE2D(_FoamTex);
			TEXTURE2D(_NormalMap);
			TEXTURE2D(_ColorCache);
			SAMPLER(sampler_ColorCache);
			TEXTURE2D(_DepthColorLUT);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE2D(_TexLeft);
			SAMPLER(sampler_Linear_Mirror);
			TEXTURE2D(_TexRight);
			float4x4 _CausticMatrix;
			float4 _CausticsSettings;
			TEXTURE2D(_DepthCache);
			SAMPLER(sampler_DepthCache);


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
			
			int IsStereoEyeLeft11_g3305( float3 WorldCamPos, float3 WorldCamRight, float force )
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
			
			float3 MatrixMulThatWorks( float4 Pos, float4x4 Mat )
			{
				float3 result = mul(Mat,Pos.xyz);
				return result + float3(Mat[0][3],Mat[1][3],Mat[2][3]);
			}
			
			float CameraDepthTexture55_g3390( float2 uv )
			{
				float4 color = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, uv);
				return color.r;
			}
			
			float MyCustomExpression44_g3390( float rawDepth )
			{
				return LinearEyeDepth(rawDepth, _ZBufferParams);
			}
			
			float3 MyCustomExpression16_g3390( float3 view, float sceneZ )
			{
				return  _WorldSpaceCameraPos - view * sceneZ / dot(UNITY_MATRIX_I_V._13_23_33, view);
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
			
			inline float3 SampleSHPixel1073( float3 SH, float3 normalWS )
			{
				return SampleSHPixel( SH, normalWS );
			}
			
			inline float3 SampleLightmap1072( float2 lightmapUV, float3 normalWS )
			{
				return SampleLightmap( lightmapUV, 0, normalWS );
			}
			
			float4x4 RotationOnly83_g4120( float4x4 Input )
			{
				return float4x4(Input[0].xyzw,float4(1,1,1,1),Input[2].xyzw, float4(1,1,1,1));
			}
			
			float3 MyCustomExpression6_g4120( float3 vertexPos, float4x4 ProbeWorldToTexture, float3 ProbeVolumeMin, float3 ProbeVolumeSizeInv )
			{
				float3 position = mul(ProbeWorldToTexture, float4(TransformObjectToWorld(vertexPos.xyz), 1.0f)).xyz;
				float3 texCoord = (position - ProbeVolumeMin.xyz) * ProbeVolumeSizeInv;
				return texCoord;
			}
			
			float3 SHEvalLinearL0L114_g4120( float3 worldNormal, float4 ProbeVolumeShR, float4 ProbeVolumeShG, float4 ProbeVolumeShB )
			{
				// See: Library\PackageCache\com.unity.render-pipelines.core12.1.15\ShaderLibrary\EntityLighting.hlsl
				return SHEvalLinearL0L1(worldNormal, ProbeVolumeShR, ProbeVolumeShG,  ProbeVolumeShB);
			}
			
			float4 SampleLightmapHD11_g4125( float2 UV )
			{
				return SAMPLE_TEXTURE2D( unity_Lightmap, samplerunity_Lightmap, UV );
			}
			
			float4 URPDecodeInstruction19_g4125(  )
			{
				return float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0, 0);
			}
			
			float4x4 RotationOnly83_g4124( float4x4 Input )
			{
				return float4x4(Input[0].xyzw,float4(1,1,1,1),Input[2].xyzw, float4(1,1,1,1));
			}
			
			float3 MyCustomExpression6_g4124( float3 vertexPos, float4x4 ProbeWorldToTexture, float3 ProbeVolumeMin, float3 ProbeVolumeSizeInv )
			{
				float3 position = mul(ProbeWorldToTexture, float4(TransformObjectToWorld(vertexPos.xyz), 1.0f)).xyz;
				float3 texCoord = (position - ProbeVolumeMin.xyz) * ProbeVolumeSizeInv;
				return texCoord;
			}
			
			float SHADERGRAPH_SAMPLE_SCENE_DEPTH_LOD(float2 uv)
			{
				#if defined(REQUIRE_DEPTH_TEXTURE)
				#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
				 	float rawDepth = SAMPLE_TEXTURE2D_ARRAY_LOD(_CameraDepthTexture, sampler_CameraDepthTexture, uv, unity_StereoEyeIndex, 0).r;
				#else
				 	float rawDepth = SAMPLE_DEPTH_TEXTURE_LOD(_CameraDepthTexture, sampler_CameraDepthTexture, uv, 0);
				#endif
				return rawDepth;
				#endif // REQUIRE_DEPTH_TEXTURE
				return 0;
			}
			
			float3 SHEvalLinearL0L114_g4124( float3 worldNormal, float4 ProbeVolumeShR, float4 ProbeVolumeShG, float4 ProbeVolumeShB )
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
			
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			float4 CalculateContrast( float contrastValue, float4 colorTarget )
			{
				float t = 0.5 * ( 1.0 - contrastValue );
				return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
			}
			float MyCustomExpression191_g3823( float In )
			{
				return ComputeFogFactor(In);
			}
			
			inline float4 GetUnderWaterFogs240_g3843( float3 viewDir, float3 camWorldPos, float3 posWS, float4 oceanFogDensities, float oceanHeight, float4 oceanFogTop_RGB_Exponent, float4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 VertexOffset1098 = ( v.ase_normal * GlobalOceanOffset );
				
				float3 _Vector3 = float3(0,0,-1);
				float4 Pos6_g3442 = float4( _Vector3 , 0.0 );
				float4x4 Mat6_g3442 = _CausticMatrix;
				float3 localMatrixMulThatWorks6_g3442 = MatrixMulThatWorks( Pos6_g3442 , Mat6_g3442 );
				float3 normalizeResult147_g3440 = normalize( localMatrixMulThatWorks6_g3442 );
				float3 vertexToFrag144_g3440 = normalizeResult147_g3440;
				o.ase_texcoord9.xyz = vertexToFrag144_g3440;
				float3 objToWorld3_g3390 = mul( GetObjectToWorldMatrix(), float4( v.vertex.xyz, 1 ) ).xyz;
				float3 vertexToFrag7_g3390 = objToWorld3_g3390;
				o.ase_texcoord10.xyz = vertexToFrag7_g3390;
				float4 Pos6_g3446 = float4( _Vector3 , 0.0 );
				float4x4 Mat6_g3446 = _CausticMatrix;
				float3 localMatrixMulThatWorks6_g3446 = MatrixMulThatWorks( Pos6_g3446 , Mat6_g3446 );
				float3 normalizeResult147_g3444 = normalize( localMatrixMulThatWorks6_g3446 );
				float3 vertexToFrag144_g3444 = normalizeResult147_g3444;
				o.ase_texcoord11.xyz = vertexToFrag144_g3444;
				
				float3 vertexPos6_g4120 = v.vertex.xyz;
				float4x4 temp_output_28_0_g4120 = _ProbeWorldToTexture;
				float4x4 ProbeWorldToTexture6_g4120 = temp_output_28_0_g4120;
				float3 ProbeVolumeMin6_g4120 = _ProbeVolumeMin;
				float3 ProbeVolumeSizeInv6_g4120 = _ProbeVolumeSizeInv;
				float3 localMyCustomExpression6_g4120 = MyCustomExpression6_g4120( vertexPos6_g4120 , ProbeWorldToTexture6_g4120 , ProbeVolumeMin6_g4120 , ProbeVolumeSizeInv6_g4120 );
				float3 vertexToFrag7_g4120 = localMyCustomExpression6_g4120;
				o.ase_texcoord13.xyz = vertexToFrag7_g4120;
				float2 texCoord2_g4125 = v.texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 vertexToFrag10_g4125 = ( ( texCoord2_g4125 * (unity_LightmapST).xy ) + (unity_LightmapST).zw );
				o.ase_texcoord14.xy = vertexToFrag10_g4125;
				float4x4 temp_output_28_0_g4124 = _ProbeWorldToTexture;
				float4x4 Input83_g4124 = temp_output_28_0_g4124;
				float4x4 localRotationOnly83_g4124 = RotationOnly83_g4124( Input83_g4124 );
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				float3 temp_output_76_0_g4124 = mul( localRotationOnly83_g4124, float4( ase_worldNormal , 0.0 ) ).xyz;
				float3 worldNormal14_g4124 = temp_output_76_0_g4124;
				float3 vertexPos6_g4124 = v.vertex.xyz;
				float4x4 ProbeWorldToTexture6_g4124 = temp_output_28_0_g4124;
				float3 ProbeVolumeMin6_g4124 = _ProbeVolumeMin;
				float3 ProbeVolumeSizeInv6_g4124 = _ProbeVolumeSizeInv;
				float3 localMyCustomExpression6_g4124 = MyCustomExpression6_g4124( vertexPos6_g4124 , ProbeWorldToTexture6_g4124 , ProbeVolumeMin6_g4124 , ProbeVolumeSizeInv6_g4124 );
				float3 vertexToFrag7_g4124 = localMyCustomExpression6_g4124;
				float2 texCoord126 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 UV260 = texCoord126;
				float2 temp_output_185_0 = (UV260*_NormalSize + 0.0);
				float2 temp_output_2_0_g3438 = temp_output_185_0;
				float2 temp_output_4_0_g3438 = (temp_output_2_0_g3438).xy;
				float2 texCoord54 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode53 = SAMPLE_TEXTURE2D_LOD( _FluidVelocity, sampler_FluidVelocity, texCoord54, 0.0 );
				float2 appendResult55 = (float2(tex2DNode53.r , tex2DNode53.g));
				float2 temp_output_102_0 = (float2( 0,0 ) + (appendResult55 - float2( -1,-1 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( -1,-1 )));
				float2 VelNormalised242 = temp_output_102_0;
				float2 temp_output_18_0_g3438 = VelNormalised242;
				float temp_output_183_0 = ( _NormFlowStrength * ( _NormalSize / 8.0 ) );
				float2 temp_cast_4 = (temp_output_183_0).xx;
				float2 temp_output_17_0_g3438 = temp_cast_4;
				float mulTime22_g3438 = _TimeParameters.x * _FlowSpeedN;
				float temp_output_27_0_g3438 = frac( mulTime22_g3438 );
				float2 temp_output_11_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_27_0_g3438 ) );
				float2 temp_output_83_0 = (float2( -1,-1 ) + (temp_output_102_0 - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
				float VelLength236 = length( temp_output_83_0 );
				float temp_output_188_0 = ( VelLength236 * _NormalStrength );
				float temp_output_55_0_g3438 = temp_output_188_0;
				float3 unpack48_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( _NormalMap, sampler_Linear_Repeat, temp_output_11_0_g3438, 0.0 ), temp_output_55_0_g3438 );
				unpack48_g3438.z = lerp( 1, unpack48_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_29_0_g3438 = frac( ( mulTime22_g3438 + 0.5 ) );
				float2 temp_output_12_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_29_0_g3438 ) );
				float3 unpack49_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( _NormalMap, sampler_Linear_Repeat, temp_output_12_0_g3438, 0.0 ), temp_output_55_0_g3438 );
				unpack49_g3438.z = lerp( 1, unpack49_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_32_0_g3438 = ( abs( ( temp_output_27_0_g3438 - 0.5 ) ) / 0.5 );
				float3 lerpResult9_g3438 = lerp( unpack48_g3438 , unpack49_g3438 , temp_output_32_0_g3438);
				float3 NormalResult220 = lerpResult9_g3438;
				float2 appendResult210 = (float2(NormalResult220.xy));
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 appendResult639 = (float2(ase_screenPosNorm.xy));
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				float ase_vertexTangentSign = v.ase_tangent.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_tanViewDir =  tanToWorld0 * ase_worldViewDir.x + tanToWorld1 * ase_worldViewDir.y  + tanToWorld2 * ase_worldViewDir.z;
				ase_tanViewDir = normalize(ase_tanViewDir);
				float ase_faceVertex = (dot(ase_tanViewDir,float3(0,0,1)));
				float switchResult329 = (((ase_faceVertex>0)?(_RefractionIndex):(_RefractionIndexUnder)));
				float switchResult530 = (((ase_faceVertex>0)?(1.0):(saturate( (0.0 + (distance( _WorldSpaceCameraPos , ase_worldPos ) - 0.1) * (1.0 - 0.0) / (0.6 - 0.1)) ))));
				float CamNear492 = switchResult530;
				float2 temp_output_211_0 = ( switchResult329 * appendResult210 * CamNear492 );
				float2 temp_output_209_0 = ( appendResult639 + temp_output_211_0 );
				float eyeDepth637 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH_LOD( float4( temp_output_209_0, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float2 lerpResult640 = lerp( appendResult639 , temp_output_209_0 , step( ( screenPos.w - eyeDepth637 ) , 0.0 ));
				float2 ScreenSpaceRefractUV691 = lerpResult640;
				float eyeDepth6_g3347 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH_LOD( float4( ScreenSpaceRefractUV691, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3347 = mul( UNITY_MATRIX_V, float4( ase_worldPos, 1 ) ).xyz;
				float temp_output_12_0_g3347 = _DepthFade;
				float temp_output_3_0_g3347 = ( abs( ( eyeDepth6_g3347 - -worldToView21_g3347.z ) ) / temp_output_12_0_g3347 );
				#ifdef _USEDEPTHFADE_ON
				float staticSwitch315 = saturate( temp_output_3_0_g3347 );
				#else
				float staticSwitch315 = 1.0;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch776 = staticSwitch315;
				#else
				float staticSwitch776 = 1.0;
				#endif
				float ShoreMaskDepth301 = staticSwitch776;
				float2 temp_output_363_0 = ( ( _RefractionFake * 0.1 ) * appendResult210 * ShoreMaskDepth301 );
				float2 RefractedUV1084 = temp_output_363_0;
				float4 ProbeVolumeShR14_g4124 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShR, sampler_Linear_Clamp, ( vertexToFrag7_g4124 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float4 ProbeVolumeShG14_g4124 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShG, sampler_Linear_Clamp, ( vertexToFrag7_g4124 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float4 ProbeVolumeShB14_g4124 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShB, sampler_Linear_Clamp, ( vertexToFrag7_g4124 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float3 localSHEvalLinearL0L114_g4124 = SHEvalLinearL0L114_g4124( worldNormal14_g4124 , ProbeVolumeShR14_g4124 , ProbeVolumeShG14_g4124 , ProbeVolumeShB14_g4124 );
				float3 vertexToFrag31_g4124 = localSHEvalLinearL0L114_g4124;
				o.ase_texcoord15.xyz = vertexToFrag31_g4124;
				
				o.ase_texcoord8.xyz = v.texcoord.xyz;
				o.ase_texcoord12 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.w = 0;
				o.ase_texcoord9.w = 0;
				o.ase_texcoord10.w = 0;
				o.ase_texcoord11.w = 0;
				o.ase_texcoord13.w = 0;
				o.ase_texcoord14.zw = 0;
				o.ase_texcoord15.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOffset1098;

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

				float switchResult530 = (((ase_vface>0)?(1.0):(saturate( (0.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - 0.1) * (1.0 - 0.0) / (0.6 - 0.1)) ))));
				float CamNear492 = switchResult530;
				float3 appendResult454 = (float3(_FoamColor2.rgb));
				float2 texCoord126 = IN.ase_texcoord8.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float2 UV260 = texCoord126;
				float2 temp_output_141_0 = (UV260*_Size + 0.0);
				float2 temp_output_2_0_g3437 = temp_output_141_0;
				float2 temp_output_4_0_g3437 = (temp_output_2_0_g3437).xy;
				float2 texCoord54 = IN.ase_texcoord8.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode53 = SAMPLE_TEXTURE2D( _FluidVelocity, sampler_FluidVelocity, texCoord54 );
				float2 appendResult55 = (float2(tex2DNode53.r , tex2DNode53.g));
				float2 temp_output_102_0 = (float2( 0,0 ) + (appendResult55 - float2( -1,-1 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( -1,-1 )));
				float2 VelNormalised242 = temp_output_102_0;
				float2 temp_output_18_0_g3437 = VelNormalised242;
				float temp_output_144_0 = ( _FlowStrength * ( _Size / 8.0 ) );
				float2 temp_cast_1 = (temp_output_144_0).xx;
				float2 temp_output_17_0_g3437 = temp_cast_1;
				float mulTime22_g3437 = _TimeParameters.x * _FlowSpeed;
				float temp_output_27_0_g3437 = frac( mulTime22_g3437 );
				float2 temp_output_11_0_g3437 = ( temp_output_4_0_g3437 + ( -(temp_output_18_0_g3437*2.0 + -1.0) * temp_output_17_0_g3437 * temp_output_27_0_g3437 ) );
				float temp_output_29_0_g3437 = frac( ( mulTime22_g3437 + 0.5 ) );
				float2 temp_output_12_0_g3437 = ( temp_output_4_0_g3437 + ( -(temp_output_18_0_g3437*2.0 + -1.0) * temp_output_17_0_g3437 * temp_output_29_0_g3437 ) );
				float temp_output_32_0_g3437 = ( abs( ( temp_output_27_0_g3437 - 0.5 ) ) / 0.5 );
				float4 lerpResult9_g3437 = lerp( SAMPLE_TEXTURE2D( _FlowTex, sampler_Linear_Repeat, temp_output_11_0_g3437 ) , SAMPLE_TEXTURE2D( _FlowTex, sampler_Linear_Repeat, temp_output_12_0_g3437 ) , temp_output_32_0_g3437);
				float FlowWaterTex246 = saturate( pow( lerpResult9_g3437.r , _FoamPowA ) );
				float2 uv_FluidColor103 = IN.ase_texcoord8.xyz.xy;
				float4 tex2DNode103 = SAMPLE_TEXTURE2D( _FluidColor, sampler_FluidColor, uv_FluidColor103 );
				float FluidColorA435 = tex2DNode103.a;
				float temp_output_105_0 = ( saturate( FlowWaterTex246 ) * FluidColorA435 );
				float lerpResult4_g3346 = lerp( 0.5 , saturate( pow( temp_output_105_0 , _Foam2Pow ) ) , _Foam2Con);
				float clampResult11_g3346 = clamp( ( lerpResult4_g3346 + 0.0 ) , 0.0 , 1.0 );
				float3 FlowWaterTexRes445 = ( appendResult454 * clampResult11_g3346 );
				#ifdef _USEFLUIDFORFOAM_ON
				float3 staticSwitch107 = FlowWaterTexRes445;
				#else
				float3 staticSwitch107 = float3( 0,0,0 );
				#endif
				float2 temp_output_2_0_g3439 = (UV260*_FoamSize + 0.0);
				float2 temp_output_4_0_g3439 = (temp_output_2_0_g3439).xy;
				float2 temp_output_18_0_g3439 = VelNormalised242;
				float temp_output_148_0 = ( _FoamFlowStrength * ( _FoamSize / 8.0 ) );
				float2 temp_cast_2 = (temp_output_148_0).xx;
				float2 temp_output_17_0_g3439 = temp_cast_2;
				float mulTime22_g3439 = _TimeParameters.x * _FoamFlowSpeed;
				float temp_output_27_0_g3439 = frac( mulTime22_g3439 );
				float2 temp_output_11_0_g3439 = ( temp_output_4_0_g3439 + ( -(temp_output_18_0_g3439*2.0 + -1.0) * temp_output_17_0_g3439 * temp_output_27_0_g3439 ) );
				float temp_output_29_0_g3439 = frac( ( mulTime22_g3439 + 0.5 ) );
				float2 temp_output_12_0_g3439 = ( temp_output_4_0_g3439 + ( -(temp_output_18_0_g3439*2.0 + -1.0) * temp_output_17_0_g3439 * temp_output_29_0_g3439 ) );
				float temp_output_32_0_g3439 = ( abs( ( temp_output_27_0_g3439 - 0.5 ) ) / 0.5 );
				float4 lerpResult9_g3439 = lerp( SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_11_0_g3439 ) , SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_12_0_g3439 ) , temp_output_32_0_g3439);
				float FoamA265 = lerpResult9_g3439.r;
				float temp_output_125_0 = ( 1.0 - saturate( 0.0 ) );
				float FoamMask114 = ( temp_output_125_0 * _Foam_Mix );
				float2 temp_output_2_0_g3436 = (UV260*_FoamSize2 + 0.0);
				float2 temp_output_4_0_g3436 = (temp_output_2_0_g3436).xy;
				float2 temp_output_18_0_g3436 = VelNormalised242;
				float2 temp_cast_3 = (temp_output_148_0).xx;
				float2 temp_output_17_0_g3436 = temp_cast_3;
				float mulTime22_g3436 = _TimeParameters.x * _FoamFlowSpeed;
				float temp_output_27_0_g3436 = frac( mulTime22_g3436 );
				float2 temp_output_11_0_g3436 = ( temp_output_4_0_g3436 + ( -(temp_output_18_0_g3436*2.0 + -1.0) * temp_output_17_0_g3436 * temp_output_27_0_g3436 ) );
				float temp_output_29_0_g3436 = frac( ( mulTime22_g3436 + 0.5 ) );
				float2 temp_output_12_0_g3436 = ( temp_output_4_0_g3436 + ( -(temp_output_18_0_g3436*2.0 + -1.0) * temp_output_17_0_g3436 * temp_output_29_0_g3436 ) );
				float temp_output_32_0_g3436 = ( abs( ( temp_output_27_0_g3436 - 0.5 ) ) / 0.5 );
				float4 lerpResult9_g3436 = lerp( SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_11_0_g3436 ) , SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_12_0_g3436 ) , temp_output_32_0_g3436);
				float FoamB267 = lerpResult9_g3436.r;
				float temp_output_155_0 = saturate( pow( saturate( ( ( FoamA265 * FoamMask114 ) + ( FoamB267 * FoamMask114 ) ) ) , _FoamPowM ) );
				float3 appendResult337 = (float3(_FoamColor.rgb));
				float3 FoamResult271 = ( temp_output_155_0 * appendResult337 );
				float3 temp_output_118_0 = ( staticSwitch107 + FoamResult271 );
				float4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 appendResult639 = (float2(ase_screenPosNorm.xy));
				float switchResult329 = (((ase_vface>0)?(_RefractionIndex):(_RefractionIndexUnder)));
				float2 temp_output_185_0 = (UV260*_NormalSize + 0.0);
				float2 temp_output_2_0_g3438 = temp_output_185_0;
				float2 temp_output_4_0_g3438 = (temp_output_2_0_g3438).xy;
				float2 temp_output_18_0_g3438 = VelNormalised242;
				float temp_output_183_0 = ( _NormFlowStrength * ( _NormalSize / 8.0 ) );
				float2 temp_cast_6 = (temp_output_183_0).xx;
				float2 temp_output_17_0_g3438 = temp_cast_6;
				float mulTime22_g3438 = _TimeParameters.x * _FlowSpeedN;
				float temp_output_27_0_g3438 = frac( mulTime22_g3438 );
				float2 temp_output_11_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_27_0_g3438 ) );
				float2 temp_output_83_0 = (float2( -1,-1 ) + (temp_output_102_0 - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
				float VelLength236 = length( temp_output_83_0 );
				float temp_output_188_0 = ( VelLength236 * _NormalStrength );
				float temp_output_55_0_g3438 = temp_output_188_0;
				float3 unpack48_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_Linear_Repeat, temp_output_11_0_g3438 ), temp_output_55_0_g3438 );
				unpack48_g3438.z = lerp( 1, unpack48_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_29_0_g3438 = frac( ( mulTime22_g3438 + 0.5 ) );
				float2 temp_output_12_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_29_0_g3438 ) );
				float3 unpack49_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_Linear_Repeat, temp_output_12_0_g3438 ), temp_output_55_0_g3438 );
				unpack49_g3438.z = lerp( 1, unpack49_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_32_0_g3438 = ( abs( ( temp_output_27_0_g3438 - 0.5 ) ) / 0.5 );
				float3 lerpResult9_g3438 = lerp( unpack48_g3438 , unpack49_g3438 , temp_output_32_0_g3438);
				float3 NormalResult220 = lerpResult9_g3438;
				float2 appendResult210 = (float2(NormalResult220.xy));
				float2 temp_output_211_0 = ( switchResult329 * appendResult210 * CamNear492 );
				float2 temp_output_209_0 = ( appendResult639 + temp_output_211_0 );
				float eyeDepth637 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( temp_output_209_0, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float2 lerpResult640 = lerp( appendResult639 , temp_output_209_0 , step( ( ScreenPos.w - eyeDepth637 ) , 0.0 ));
				float2 ScreenSpaceRefractUV691 = lerpResult640;
				float eyeDepth6_g3347 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV691, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3347 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float temp_output_12_0_g3347 = _DepthFade;
				float temp_output_3_0_g3347 = ( abs( ( eyeDepth6_g3347 - -worldToView21_g3347.z ) ) / temp_output_12_0_g3347 );
				#ifdef _USEDEPTHFADE_ON
				float staticSwitch315 = saturate( temp_output_3_0_g3347 );
				#else
				float staticSwitch315 = 1.0;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch776 = staticSwitch315;
				#else
				float staticSwitch776 = 1.0;
				#endif
				float ShoreMaskDepth301 = staticSwitch776;
				float3 temp_output_312_0 = ( temp_output_118_0 * ShoreMaskDepth301 );
				float3 appendResult336 = (float3(_Color.rgb));
				float3 AlbedoResult225 = ( temp_output_312_0 + ( appendResult336 * _Color.a * saturate( (0.0 + (ShoreMaskDepth301 - 0.0) * (1.0 - 0.0) / (0.09 - 0.0)) ) ) );
				float3 AlbedoIn285_g3823 = AlbedoResult225;
				#ifdef _USEUNDERWATER
				float3 staticSwitch214_g3823 = AlbedoIn285_g3823;
				#else
				float3 staticSwitch214_g3823 = AlbedoIn285_g3823;
				#endif
				float3 AlbedoRes2511 = ( CamNear492 * staticSwitch214_g3823 );
				
				float3 temp_cast_11 = (1.0).xxx;
				float2 texCoord355 = IN.ase_texcoord8.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_output_363_0 = ( ( _RefractionFake * 0.1 ) * appendResult210 * ShoreMaskDepth301 );
				float4 tex2DNode350 = SAMPLE_TEXTURE2D( _ColorCache, sampler_ColorCache, ( texCoord355 + temp_output_363_0 ) );
				float ColorCacheADistorted787 = tex2DNode350.a;
				float temp_output_719_0 = saturate( ( ( ColorCacheADistorted787 * 0.1 ) / _RemapWaterDistance1 ) );
				float eyeDepth6_g3301 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV691, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3301 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float fresnelNdotV747 = dot( WorldNormal, WorldViewDirection );
				float fresnelNode747 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV747, 5.0 ) );
				float lerpResult745 = lerp( 9.0 , 30.0 , saturate( fresnelNode747 ));
				float DepthFadeDistthing744 = lerpResult745;
				float temp_output_12_0_g3301 = ( DepthFadeDistthing744 * _RemapWaterDistance );
				float temp_output_3_0_g3301 = ( abs( ( eyeDepth6_g3301 - -worldToView21_g3301.z ) ) / temp_output_12_0_g3301 );
				float temp_output_716_0 = saturate( temp_output_3_0_g3301 );
				#ifdef _REFRACTION_ON
				float staticSwitch738 = temp_output_716_0;
				#else
				float staticSwitch738 = temp_output_719_0;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch714 = _WaterDepthPow;
				#else
				float staticSwitch714 = _WaterDepthPow1;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch725 = ( 1.0 - pow( ( 1.0 - temp_output_716_0 ) , 6.0 ) );
				#else
				float staticSwitch725 = ( 1.0 - pow( ( 1.0 - temp_output_719_0 ) , 6.0 ) );
				#endif
				float2 appendResult711 = (float2(staticSwitch725 , 0.0));
				float3 appendResult705 = (float3(SAMPLE_TEXTURE2D_LOD( _DepthColorLUT, sampler_Linear_Clamp, appendResult711, 0.0 ).rgb));
				float temp_output_2_0_g3304 = saturate( ShoreMaskDepth301 );
				float temp_output_3_0_g3304 = ( 1.0 - temp_output_2_0_g3304 );
				float3 appendResult7_g3304 = (float3(temp_output_3_0_g3304 , temp_output_3_0_g3304 , temp_output_3_0_g3304));
				float3 desaturateInitialColor739 = ( ( ( saturate( pow( ( 1.0 - staticSwitch738 ) , staticSwitch714 ) ) * appendResult705 ) * temp_output_2_0_g3304 ) + appendResult7_g3304 );
				float desaturateDot739 = dot( desaturateInitialColor739, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar739 = lerp( desaturateInitialColor739, desaturateDot739.xxx, ( 1.0 - _WaterSaturation ) );
				#ifdef _USEDEPTHCOLORINGLUT_ON
				float3 staticSwitch785 = desaturateVar739;
				#else
				float3 staticSwitch785 = temp_cast_11;
				#endif
				float3 switchResult742 = (((ase_vface>0)?(staticSwitch785):(float3( 0,0,0 ))));
				float3 WaterDepthTint743 = switchResult742;
				#ifdef _REFRACTION_ON
				float2 staticSwitch917 = ( temp_output_211_0 * ShoreMaskDepth301 );
				#else
				float2 staticSwitch917 = ( appendResult210 * _RefractionIndexUnderMobile );
				#endif
				float2 RefractionBacksideWarp650 = staticSwitch917;
				float3 temp_output_647_0 = SHADERGRAPH_REFLECTION_PROBE(WorldViewDirection,float3( ( RefractionBacksideWarp650 * 4.0 ) ,  0.0 ),1.0);
				float3 temp_output_564_0 = ( (temp_output_647_0).xyz * _CubemapStrength );
				float3 switchResult592 = (((ase_vface>0)?(float3( 0,0,0 )):(temp_output_564_0)));
				float3 WorldCamPos11_g3305 = _WorldPos;
				float3 WorldCamRight11_g3305 = _WorldDir;
				float force11_g3305 = _ForceEye;
				int localIsStereoEyeLeft11_g3305 = IsStereoEyeLeft11_g3305( WorldCamPos11_g3305 , WorldCamRight11_g3305 , force11_g3305 );
				float2 appendResult4_g3305 = (float2(ase_screenPosNorm.xy));
				float2 appendResult585 = (float2(NormalResult220.xy));
				float2 temp_output_1_0_g3305 = ( appendResult4_g3305 + ( appendResult585 * CamNear492 * _PlanarNormalStrengthm ) );
				float4 tex2DNode3_g3305 = SAMPLE_TEXTURE2D( _TexLeft, sampler_Linear_Mirror, temp_output_1_0_g3305 );
				float4 tex2DNode2_g3305 = SAMPLE_TEXTURE2D( _TexRight, sampler_Linear_Mirror, temp_output_1_0_g3305 );
				float4 ifLocalVar10_g3305 = 0;
				if( localIsStereoEyeLeft11_g3305 <= 0.0 )
				ifLocalVar10_g3305 = tex2DNode2_g3305;
				else
				ifLocalVar10_g3305 = tex2DNode3_g3305;
				float3 tanToWorld0 = float3( WorldTangent.x, WorldBiTangent.x, WorldNormal.x );
				float3 tanToWorld1 = float3( WorldTangent.y, WorldBiTangent.y, WorldNormal.y );
				float3 tanToWorld2 = float3( WorldTangent.z, WorldBiTangent.z, WorldNormal.z );
				float3 tanNormal581 = NormalResult220;
				float3 worldNormal581 = float3(dot(tanToWorld0,tanNormal581), dot(tanToWorld1,tanNormal581), dot(tanToWorld2,tanNormal581));
				float fresnelNdotV579 = dot( worldNormal581, WorldViewDirection );
				float fresnelNode579 = ( _RefFresnel.x + _RefFresnel.y * pow( 1.0 - fresnelNdotV579, _RefFresnel.z ) );
				float3 switchResult588 = (((ase_vface>0)?(( (ifLocalVar10_g3305).rgb * saturate( fresnelNode579 ) )):(temp_output_564_0)));
				#ifdef _PLANARREFLECTION_ON
				float3 staticSwitch574 = switchResult588;
				#else
				float3 staticSwitch574 = switchResult592;
				#endif
				float3 ReflectionRes554 = staticSwitch574;
				float switchResult418 = (((ase_vface>0)?(_ColorMix):(_ColorMixBack)));
				float3 appendResult352 = (float3(tex2DNode350.rgb));
				#ifdef _REFRACTION_ON
				float4 staticSwitch894 = _RefractionColor;
				#else
				float4 staticSwitch894 = _RefractionColorMobile;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch889 = 1.0;
				#else
				float staticSwitch889 = saturate( (0.0 + (ColorCacheADistorted787 - 0.02) * (1.0 - 0.0) / (0.15 - 0.02)) );
				#endif
				float ShoreMaskThing883 = staticSwitch889;
				float4 lerpResult885 = lerp( float4( 1,1,1,0 ) , staticSwitch894 , ShoreMaskThing883);
				float3 appendResult331 = (float3(lerpResult885.rgb));
				float3 temp_output_354_0 = ( switchResult418 * appendResult352 * appendResult331 );
				float4 fetchOpaqueVal203 = float4( SHADERGRAPH_SAMPLE_SCENE_COLOR( ScreenSpaceRefractUV691 ), 1.0 );
				float3 appendResult330 = (float3(fetchOpaqueVal203.rgb));
				float3 lerpResult317 = lerp( appendResult330 , ( appendResult330 * appendResult331 ) , ShoreMaskDepth301);
				#ifdef _REFRACTION_ON
				float3 staticSwitch277 = lerpResult317;
				#else
				float3 staticSwitch277 = temp_output_354_0;
				#endif
				float3 temp_output_771_0 = ( ReflectionRes554 + staticSwitch277 );
				float3 lerpResult529 = lerp( appendResult330 , staticSwitch277 , CamNear492);
				float3 temp_output_18_0_g3440 = float3( 0,0,0 );
				float3 temp_output_30_0_g3440 = float3( 0,0,1 );
				float3 tanNormal12_g3440 = temp_output_30_0_g3440;
				float3 worldNormal12_g3440 = float3(dot(tanToWorld0,tanNormal12_g3440), dot(tanToWorld1,tanNormal12_g3440), dot(tanToWorld2,tanNormal12_g3440));
				float3 vertexToFrag144_g3440 = IN.ase_texcoord9.xyz;
				float dotResult1_g3440 = dot( worldNormal12_g3440 , vertexToFrag144_g3440 );
				float3 vertexToFrag7_g3390 = IN.ase_texcoord10.xyz;
				float3 positionWS_fogFactor24_g3390 = vertexToFrag7_g3390;
				float3 normalizeResult21_g3390 = normalize( ( _WorldSpaceCameraPos - positionWS_fogFactor24_g3390 ) );
				float3 view23_g3390 = normalizeResult21_g3390;
				float3 view16_g3390 = view23_g3390;
				float3 appendResult49_g3390 = (float3(ScreenSpaceRefractUV691 , ase_screenPosNorm.w));
				float3 screenPosXYW29_g3390 = (appendResult49_g3390).xyz;
				float2 uvDepth38_g3390 = ( (screenPosXYW29_g3390).xy / (screenPosXYW29_g3390).z );
				float2 uv55_g3390 = uvDepth38_g3390;
				float localCameraDepthTexture55_g3390 = CameraDepthTexture55_g3390( uv55_g3390 );
				float rawDepth40_g3390 = localCameraDepthTexture55_g3390;
				float rawDepth44_g3390 = rawDepth40_g3390;
				float localMyCustomExpression44_g3390 = MyCustomExpression44_g3390( rawDepth44_g3390 );
				float sceneZ41_g3390 = localMyCustomExpression44_g3390;
				float sceneZ16_g3390 = sceneZ41_g3390;
				float3 localMyCustomExpression16_g3390 = MyCustomExpression16_g3390( view16_g3390 , sceneZ16_g3390 );
				float3 scenePos11_g3390 = localMyCustomExpression16_g3390;
				float3 WorldPosition678 = scenePos11_g3390;
				float2 temp_output_2_0_g3452 = (WorldPosition678).xz;
				float2 temp_output_4_0_g3452 = (temp_output_2_0_g3452).xy;
				float2 VelNeg231 = temp_output_83_0;
				float2 break969 = VelNeg231;
				float3 appendResult992 = (float3(break969.x , 0.0 , break969.y));
				float3 rotatedValue998 = RotateAroundAxis( float3( 0,0,0 ), float3( (mul( GetObjectToWorldMatrix(), float4( appendResult992 , 0.0 ) ).xyz).xz ,  0.0 ), float3( 0,0,1 ), radians( _CausticRotate ) );
				float2 temp_output_18_0_g3452 = (rotatedValue998).xy;
				float2 temp_cast_23 = (-_CausticFlowStrength).xx;
				float2 temp_output_17_0_g3452 = temp_cast_23;
				float mulTime22_g3452 = _TimeParameters.x * _CausticFlowSpeed;
				float temp_output_27_0_g3452 = frac( mulTime22_g3452 );
				float2 break924 = ( temp_output_4_0_g3452 + ( -temp_output_18_0_g3452 * temp_output_17_0_g3452 * temp_output_27_0_g3452 ) );
				float temp_output_979_0 = (WorldPosition678).y;
				float3 appendResult923 = (float3(break924.x , temp_output_979_0 , break924.y));
				float3 temp_output_105_0_g3440 = appendResult923;
				float2 PanSpeed131_g3440 = (_CausticsSettings).xy;
				float mulTime28_g3440 = _TimeParameters.x * -1.0;
				float2 break101_g3440 = ( PanSpeed131_g3440 * mulTime28_g3440 );
				float3 appendResult100_g3440 = (float3(break101_g3440.x , 0.0 , break101_g3440.y));
				float4 Pos6_g3443 = float4( ( temp_output_105_0_g3440 + appendResult100_g3440 ) , 0.0 );
				float4x4 invertVal146_g3440 = Inverse4x4( _CausticMatrix );
				float4x4 Mat6_g3443 = invertVal146_g3440;
				float3 localMatrixMulThatWorks6_g3443 = MatrixMulThatWorks( Pos6_g3443 , Mat6_g3443 );
				float3 appendResult121_g3440 = (float3(SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3443).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3443).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3443).xy ).r));
				float3 appendResult62_g3440 = (float3(_CausticsColor.rgb));
				float temp_output_117_0_g3440 = (temp_output_105_0_g3440).y;
				float GlobalOceanHeight515 = ( GlobalOceanHeight + (0) );
				float temp_output_63_0_g3440 = GlobalOceanHeight515;
				float2 DistanceFade134_g3440 = (_CausticsSettings).zw;
				float2 break136_g3440 = DistanceFade134_g3440;
				float temp_output_67_0_g3440 = ( saturate( (0.0 + (max( -( temp_output_117_0_g3440 - temp_output_63_0_g3440 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) * saturate( (1.0 + (distance( temp_output_63_0_g3440 , temp_output_117_0_g3440 ) - break136_g3440.x) * (0.0 - 1.0) / (break136_g3440.y - break136_g3440.x)) ) );
				float3 temp_output_20_0_g3440 = ( temp_output_18_0_g3440 + ( max( dotResult1_g3440 , 0.0 ) * appendResult121_g3440 * appendResult62_g3440 * temp_output_67_0_g3440 ) );
				float3 temp_output_18_0_g3444 = float3( 0,0,0 );
				float3 temp_output_30_0_g3444 = float3( 0,0,1 );
				float3 tanNormal12_g3444 = temp_output_30_0_g3444;
				float3 worldNormal12_g3444 = float3(dot(tanToWorld0,tanNormal12_g3444), dot(tanToWorld1,tanNormal12_g3444), dot(tanToWorld2,tanNormal12_g3444));
				float3 vertexToFrag144_g3444 = IN.ase_texcoord11.xyz;
				float dotResult1_g3444 = dot( worldNormal12_g3444 , vertexToFrag144_g3444 );
				float temp_output_29_0_g3452 = frac( ( mulTime22_g3452 + 0.5 ) );
				float2 break965 = ( temp_output_4_0_g3452 + ( -temp_output_18_0_g3452 * temp_output_17_0_g3452 * temp_output_29_0_g3452 ) );
				float3 appendResult966 = (float3(break965.x , temp_output_979_0 , break965.y));
				float3 temp_output_105_0_g3444 = appendResult966;
				float2 PanSpeed131_g3444 = (_CausticsSettings).xy;
				float mulTime28_g3444 = _TimeParameters.x * -1.0;
				float2 break101_g3444 = ( PanSpeed131_g3444 * mulTime28_g3444 );
				float3 appendResult100_g3444 = (float3(break101_g3444.x , 0.0 , break101_g3444.y));
				float4 Pos6_g3447 = float4( ( temp_output_105_0_g3444 + appendResult100_g3444 ) , 0.0 );
				float4x4 invertVal146_g3444 = Inverse4x4( _CausticMatrix );
				float4x4 Mat6_g3447 = invertVal146_g3444;
				float3 localMatrixMulThatWorks6_g3447 = MatrixMulThatWorks( Pos6_g3447 , Mat6_g3447 );
				float3 appendResult121_g3444 = (float3(SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3447).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3447).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3447).xy ).r));
				float3 appendResult62_g3444 = (float3(_CausticsColor.rgb));
				float temp_output_117_0_g3444 = (temp_output_105_0_g3444).y;
				float temp_output_63_0_g3444 = GlobalOceanHeight515;
				float2 DistanceFade134_g3444 = (_CausticsSettings).zw;
				float2 break136_g3444 = DistanceFade134_g3444;
				float temp_output_67_0_g3444 = ( saturate( (0.0 + (max( -( temp_output_117_0_g3444 - temp_output_63_0_g3444 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) * saturate( (1.0 + (distance( temp_output_63_0_g3444 , temp_output_117_0_g3444 ) - break136_g3444.x) * (0.0 - 1.0) / (break136_g3444.y - break136_g3444.x)) ) );
				float3 temp_output_20_0_g3444 = ( temp_output_18_0_g3444 + ( max( dotResult1_g3444 , 0.0 ) * appendResult121_g3444 * appendResult62_g3444 * temp_output_67_0_g3444 ) );
				float temp_output_32_0_g3452 = ( abs( ( temp_output_27_0_g3452 - 0.5 ) ) / 0.5 );
				float3 lerpResult958 = lerp( temp_output_20_0_g3440 , temp_output_20_0_g3444 , temp_output_32_0_g3452);
				float3 appendResult859 = (float3(_CausticsTint.rgb));
				float3 temp_output_856_0 = ( lerpResult958 * appendResult859 * 3.0 );
				#ifdef _REFRACTION_ON
				float3 staticSwitch761 = temp_output_856_0;
				#else
				float3 staticSwitch761 = float3( 0,0,0 );
				#endif
				float3 Caustics671 = staticSwitch761;
				float eyeDepth6_g3387 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV691, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 temp_output_14_0_g3387 = IN.ase_texcoord12.xyz;
				float3 temp_output_5_0_g3388 = temp_output_14_0_g3387;
				float3 objToWorld2_g3388 = mul( GetObjectToWorldMatrix(), float4( temp_output_5_0_g3388, 1 ) ).xyz;
				float3 worldToView1_g3388 = mul( UNITY_MATRIX_V, float4( objToWorld2_g3388, 1 ) ).xyz;
				float temp_output_12_0_g3387 = 1.0;
				float temp_output_3_0_g3387 = ( abs( ( eyeDepth6_g3387 - -worldToView1_g3388.z ) ) / temp_output_12_0_g3387 );
				float temp_output_912_0 = saturate( temp_output_3_0_g3387 );
				float luminance1004 = Luminance(appendResult330);
				#ifdef _REFRACTION_ON
				float3 staticSwitch766 = ( WaterDepthTint743 * ( ReflectionRes554 + lerpResult529 + ( Caustics671 * appendResult331 * 30.0 * ShoreMaskDepth301 * appendResult330 * temp_output_912_0 * saturate( (0.1 + (luminance1004 - 0.0) * (1.0 - 0.1) / (1.0 - 0.0)) ) ) ) );
				#else
				float3 staticSwitch766 = ( WaterDepthTint743 * temp_output_771_0 );
				#endif
				#ifdef _REFRACTION_ON
				float3 staticSwitch568 = lerpResult529;
				#else
				float3 staticSwitch568 = temp_output_771_0;
				#endif
				float3 switchResult591 = (((ase_vface>0)?(staticSwitch766):(staticSwitch568)));
				float3 EmissionResult218 = switchResult591;
				
				#ifdef _REFRACTION_ON
				float staticSwitch882 = saturate( (0.0 + (ShoreMaskDepth301 - 0.1) * (1.0 - 0.0) / (1.0 - 0.1)) );
				#else
				float staticSwitch882 = saturate( (0.0 + (ColorCacheADistorted787 - 0.1) * (1.0 - 0.0) / (1.0 - 0.1)) );
				#endif
				float SpecularRes1093 = ( _Specular * staticSwitch882 );
				float3 temp_cast_30 = (SpecularRes1093).xxx;
				
				float switchResult632 = (((ase_vface>0)?(_Smoothness):(0.85)));
				float SmoothnessRes556 = switchResult632;
				
				float3 SH1073 = IN.lightmapUVOrVertexSH.xyz;
				float3 normalWS1073 = WorldNormal;
				float3 localSampleSHPixel1073 = SampleSHPixel1073( SH1073 , normalWS1073 );
				float2 lightmapUV1072 = IN.lightmapUVOrVertexSH.xy;
				float3 normalWS1072 = WorldNormal;
				float3 localSampleLightmap1072 = SampleLightmap1072( lightmapUV1072 , normalWS1072 );
				#ifdef LIGHTMAP_ON
				float3 staticSwitch1071 = localSampleLightmap1072;
				#else
				float3 staticSwitch1071 = localSampleSHPixel1073;
				#endif
				float3 LightmapOrSH1076 = staticSwitch1071;
				float4x4 temp_output_28_0_g4120 = _ProbeWorldToTexture;
				float4x4 Input83_g4120 = temp_output_28_0_g4120;
				float4x4 localRotationOnly83_g4120 = RotationOnly83_g4120( Input83_g4120 );
				float3 tanNormal1143 = NormalResult220;
				float3 worldNormal1143 = float3(dot(tanToWorld0,tanNormal1143), dot(tanToWorld1,tanNormal1143), dot(tanToWorld2,tanNormal1143));
				float3 worldNormal14_g4120 = mul( localRotationOnly83_g4120, float4( worldNormal1143 , 0.0 ) ).xyz;
				float3 vertexToFrag7_g4120 = IN.ase_texcoord13.xyz;
				float2 RefractedUV1084 = temp_output_363_0;
				float4 ProbeVolumeShR14_g4120 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShR, sampler_Linear_Clamp, ( vertexToFrag7_g4120 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float4 ProbeVolumeShG14_g4120 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShG, sampler_Linear_Clamp, ( vertexToFrag7_g4120 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float4 ProbeVolumeShB14_g4120 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShB, sampler_Linear_Clamp, ( vertexToFrag7_g4120 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float3 localSHEvalLinearL0L114_g4120 = SHEvalLinearL0L114_g4120( worldNormal14_g4120 , ProbeVolumeShR14_g4120 , ProbeVolumeShG14_g4120 , ProbeVolumeShB14_g4120 );
				#ifdef _PROBEVOLUME_ON
				float3 staticSwitch20_g4120 = localSHEvalLinearL0L114_g4120;
				#else
				float3 staticSwitch20_g4120 = LightmapOrSH1076;
				#endif
				float2 vertexToFrag10_g4125 = IN.ase_texcoord14.xy;
				float2 UV11_g4125 = vertexToFrag10_g4125;
				float4 localSampleLightmapHD11_g4125 = SampleLightmapHD11_g4125( UV11_g4125 );
				float4 localURPDecodeInstruction19_g4125 = URPDecodeInstruction19_g4125();
				float3 decodeLightMap6_g4125 = DecodeLightmap(localSampleLightmapHD11_g4125,localURPDecodeInstruction19_g4125);
				float3 vertexToFrag31_g4124 = IN.ase_texcoord15.xyz;
				#ifdef _PROBEVOLUME_ON
				float3 staticSwitch20_g4124 = vertexToFrag31_g4124;
				#else
				float3 staticSwitch20_g4124 = decodeLightMap6_g4125;
				#endif
				#ifdef _ProbeVolPerVertex
				float3 staticSwitch1148 = staticSwitch20_g4124;
				#else
				float3 staticSwitch1148 = staticSwitch20_g4120;
				#endif
				float3 BakedGI1077 = staticSwitch1148;
				float luminance293 = Luminance(BakedGI1077);
				float temp_output_1130_0 = saturate( (0.0 + (luminance293 - ( 1.0 - _ReflectionShadowedEnd )) * (1.0 - 0.0) / (( 1.0 - _ReflectionShadowedStart ) - ( 1.0 - _ReflectionShadowedEnd ))) );
				float temp_output_12_0_g3344 = _ReflectionShadowed;
				#ifdef _PLANARREFLECTION_ON
				float staticSwitch594 = 0.0;
				#else
				float staticSwitch594 = ( ( ( temp_output_1130_0 * temp_output_12_0_g3344 ) + ( 1.0 - temp_output_12_0_g3344 ) ) * ShoreMaskDepth301 );
				#endif
				float OcclusionRes773 = ( CamNear492 * staticSwitch594 );
				
				float4 ifLocalVars41 = 0;
				float3 ColorCacheRGB596 = appendResult352;
				if(_DebugVisual==0){ifLocalVars41 = float4( ColorCacheRGB596 , 0.0 ); };
				float ColorCacheA599 = SAMPLE_TEXTURE2D( _ColorCache, sampler_ColorCache, texCoord355 ).a;
				float4 temp_cast_42 = (ColorCacheA599).xxxx;
				if(_DebugVisual==1){ifLocalVars41 = temp_cast_42; };
				float2 uv_DepthCache616 = IN.ase_texcoord8.xyz.xy;
				float4 tex2DNode616 = SAMPLE_TEXTURE2D( _DepthCache, sampler_DepthCache, uv_DepthCache616 );
				float2 appendResult614 = (float2(tex2DNode616.r , tex2DNode616.g));
				float2 CaptureRG_Direction615 = abs( appendResult614 );
				if(_DebugVisual==2){ifLocalVars41 = float4( CaptureRG_Direction615, 0.0 , 0.0 ); };
				Gradient gradient621 = NewGradient( 0, 8, 2, float4( 0.2970481, 0, 1, 0 ), float4( 0.9772253, 0, 1, 0.1602655 ), float4( 1, 0, 0, 0.3288777 ), float4( 1, 0.3057163, 0, 0.4524148 ), float4( 1, 0.8153982, 0, 0.5926604 ), float4( 0.3415842, 1, 0, 0.6978256 ), float4( 0.01040274, 0.8851848, 0.9615906, 0.8397345 ), float4( 0, 0, 0, 1 ), float2( 1, 0.05294881 ), float2( 1, 0.9382315 ), 0, 0, 0, 0, 0, 0 );
				float CaptureB_Distancex100625 = ( abs( tex2DNode616.b ) * 100.0 );
				if(_DebugVisual==3){ifLocalVars41 = SampleGradient( gradient621, ( CaptureB_Distancex100625 * 10.0 ) ); };
				float CaptureA_Depth617 = tex2DNode616.a;
				float4 temp_cast_45 = (CaptureA_Depth617).xxxx;
				if(_DebugVisual==4){ifLocalVars41 = temp_cast_45; };
				float3 appendResult335 = (float3(tex2DNode103.rgb));
				float3 FluidColorRGB253 = appendResult335;
				if(_DebugVisual==5){ifLocalVars41 = float4( FluidColorRGB253 , 0.0 ); };
				float2 break84 = VelNeg231;
				float temp_output_77_0 = ( ( atan2( break84.x , break84.y ) / TWO_PI ) + 0.5 );
				float3 hsvTorgb70 = HSVToRGB( float3(temp_output_77_0,1.0,VelLength236) );
				float3 DebugFlowRainbow251 = hsvTorgb70;
				if(_DebugVisual==6){ifLocalVars41 = float4( DebugFlowRainbow251 , 0.0 ); };
				float3 hsvTorgb93 = HSVToRGB( float3(temp_output_77_0,( VelLength236 * 2.0 ),1.0) );
				float4 temp_cast_50 = (FlowWaterTex246).xxxx;
				float4 DebugFlowMix249 = ( float4( hsvTorgb93 , 0.0 ) * CalculateContrast(0.97,temp_cast_50) );
				if(_DebugVisual==7){ifLocalVars41 = DebugFlowMix249; };
				float4 temp_cast_52 = (FluidColorA435).xxxx;
				if(_DebugVisual==8){ifLocalVars41 = temp_cast_52; };
				float FluidVelA431 = tex2DNode53.a;
				float4 temp_cast_53 = (FluidVelA431).xxxx;
				if(_DebugVisual==9){ifLocalVars41 = temp_cast_53; };
				if(_DebugVisual==10){ifLocalVars41 = 0.0; };
				if(_DebugVisual==11){ifLocalVars41 = 0.0; };
				if(_DebugVisual==12){ifLocalVars41 = 0.0; };
				if(_DebugVisual==13){ifLocalVars41 = 0.0; };
				if(_DebugVisual==14){ifLocalVars41 = 0.0; };
				float4 DebugResult223 = ifLocalVars41;
				
				float4 temp_cast_55 = (1.0).xxxx;
				float4 temp_cast_56 = (0.0).xxxx;
				#ifdef _PROBEVOLUME_ON
				float4 staticSwitch25_g4120 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeOcc, sampler_Linear_Clamp, ( vertexToFrag7_g4120 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				#else
				float4 staticSwitch25_g4120 = temp_cast_56;
				#endif
				#ifdef SHADOWS_SHADOWMASK
				float4 staticSwitch68_g4120 = staticSwitch25_g4120;
				#else
				float4 staticSwitch68_g4120 = temp_cast_55;
				#endif
				float4 temp_cast_59 = (1.0).xxxx;
				#ifdef _ProbeVolPerVertex
				float4 staticSwitch1147 = temp_cast_59;
				#else
				float4 staticSwitch1147 = staticSwitch68_g4120;
				#endif
				float4 ShadowMask1078 = staticSwitch1147;
				
				float3 appendResult50_g3823 = (float3(unity_FogColor.rgb));
				float3 WorldPosition256_g3823 = WorldPosition;
				float3 worldToClip188_g3823 = TransformWorldToHClip(WorldPosition256_g3823).xyz;
				float In191_g3823 = worldToClip188_g3823.z;
				float localMyCustomExpression191_g3823 = MyCustomExpression191_g3823( In191_g3823 );
				float4 appendResult239_g3823 = (float4(appendResult50_g3823 , ( 1.0 - localMyCustomExpression191_g3823 )));
				#ifdef FOG_LINEAR
				float4 staticSwitch252_g3823 = appendResult239_g3823;
				#else
				float4 staticSwitch252_g3823 = float4( 0,0,0,0 );
				#endif
				float4 FogLinear53_g3823 = staticSwitch252_g3823;
				float4 appendResult47_g3823 = (float4(FogLinear53_g3823));
				float OceanUnder289_g3823 = GlobalOceanUnder;
				float3 appendResult379_g3823 = (float3(FogLinear53_g3823.xyz));
				float3 WorldPos252_g3843 = WorldPosition256_g3823;
				float temp_output_67_0_g3823 = ( GlobalOceanOffset + GlobalOceanHeight );
				float OceanHeight274_g3823 = temp_output_67_0_g3823;
				float temp_output_108_0_g3843 = OceanHeight274_g3823;
				float3 ViewDir264_g3823 = WorldViewDirection;
				float3 viewDir240_g3843 = ViewDir264_g3823;
				float3 camWorldPos240_g3843 = _WorldSpaceCameraPos;
				float3 posWS240_g3843 = WorldPos252_g3843;
				float4 oceanFogDensities240_g3843 = OceanFogDensities;
				float oceanHeight240_g3843 = temp_output_108_0_g3843;
				float4 oceanFogTop_RGB_Exponent240_g3843 = OceanFogTop_RGB_Exponent;
				float4 oceanFogBottom_RGB_Intensity240_g3843 = OceanFogBottom_RGB_Intensity;
				float4 localGetUnderWaterFogs240_g3843 = GetUnderWaterFogs240_g3843( viewDir240_g3843 , camWorldPos240_g3843 , posWS240_g3843 , oceanFogDensities240_g3843 , oceanHeight240_g3843 , oceanFogTop_RGB_Exponent240_g3843 , oceanFogBottom_RGB_Intensity240_g3843 );
				float4 ifLocalVar257_g3843 = 0;
				UNITY_BRANCH 
				if( (WorldPos252_g3843).y < ( temp_output_108_0_g3843 + 0.2 ) )
				ifLocalVar257_g3843 = localGetUnderWaterFogs240_g3843;
				float4 FogRes185_g3843 = ifLocalVar257_g3843;
				float3 appendResult94_g3843 = (float3(FogRes185_g3843.xyz));
				float3 temp_output_394_103_g3823 = appendResult94_g3843;
				float temp_output_254_0_g3823 = (WorldPosition256_g3823).y;
				float temp_output_137_0_g3823 = ( temp_output_254_0_g3823 - OceanHeight274_g3823 );
				float temp_output_24_0_g3835 = temp_output_137_0_g3823;
				float temp_output_44_0_g3835 = 0.35;
				float temp_output_45_0_g3835 = 0.31;
				float temp_output_46_0_g3835 = saturate( (0.0 + (( temp_output_24_0_g3835 - temp_output_44_0_g3835 ) - 0.0) * (1.0 - 0.0) / (temp_output_45_0_g3835 - 0.0)) );
				#ifdef _FadeWithHeight
				float staticSwitch371_g3823 = ( 1.0 - temp_output_46_0_g3835 );
				#else
				float staticSwitch371_g3823 = 1.0;
				#endif
				float FadeFromY2375_g3823 = staticSwitch371_g3823;
				float smoothstepResult366_g3823 = smoothstep( 0.0 , 1.0 , FadeFromY2375_g3823);
				float3 lerpResult378_g3823 = lerp( appendResult379_g3823 , temp_output_394_103_g3823 , smoothstepResult366_g3823);
				float temp_output_61_0_g3843 = ( 1.0 - (FogRes185_g3843).w );
				float temp_output_58_0_g3823 = ( 1.0 - temp_output_61_0_g3843 );
				float temp_output_24_0_g3833 = temp_output_137_0_g3823;
				float temp_output_44_0_g3833 = 0.1;
				float temp_output_45_0_g3833 = 0.31;
				float temp_output_46_0_g3833 = saturate( (0.0 + (( temp_output_24_0_g3833 - temp_output_44_0_g3833 ) - 0.0) * (1.0 - 0.0) / (temp_output_45_0_g3833 - 0.0)) );
				#ifdef _FadeWithHeight
				float staticSwitch238_g3823 = ( 1.0 - temp_output_46_0_g3833 );
				#else
				float staticSwitch238_g3823 = 1.0;
				#endif
				float FadeFromY295_g3823 = staticSwitch238_g3823;
				float smoothstepResult374_g3823 = smoothstep( 0.0 , 1.0 , FadeFromY295_g3823);
				float lerpResult381_g3823 = lerp( (FogLinear53_g3823).w , temp_output_58_0_g3823 , smoothstepResult374_g3823);
				float4 appendResult377_g3823 = (float4(lerpResult378_g3823 , lerpResult381_g3823));
				float4 ifLocalVar49_g3823 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g3823 >= 1.0 )
				ifLocalVar49_g3823 = appendResult377_g3823;
				else
				ifLocalVar49_g3823 = appendResult47_g3823;
				#ifdef _USEUNDERWATER
				float4 staticSwitch215_g3823 = max( ifLocalVar49_g3823 , float4( 0,0,0,0 ) );
				#else
				float4 staticSwitch215_g3823 = appendResult47_g3823;
				#endif
				float4 temp_output_1142_25 = staticSwitch215_g3823;
				float3 appendResult498 = (float3(temp_output_1142_25.xyz));
				float4 appendResult499 = (float4(appendResult498 , 0.0));
				float4 lerpResult497 = lerp( appendResult499 , temp_output_1142_25 , CamNear492);
				float4 FogColorRes500 = lerpResult497;
				
				float4 appendResult11_g3819 = (float4(_TonemappingSettings.x , _TonemappingSettings.y , _TonemappingSettings.z , _TonemappingSettings.w));
				
				float3 temp_cast_64 = (1.0).xxx;
				float3 temp_cast_65 = (1.0).xxx;
				float3 appendResult100_g3843 = (float3(OceanWaterTint_RGB.xyz));
				float3 temp_cast_67 = (1.0).xxx;
				float3 ifLocalVar170_g3823 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g3823 >= 1.0 )
				ifLocalVar170_g3823 = appendResult100_g3843;
				else
				ifLocalVar170_g3823 = temp_cast_67;
				float3 lerpResult226_g3823 = lerp( temp_cast_65 , ifLocalVar170_g3823 , FadeFromY295_g3823);
				#ifdef _USEUNDERWATER
				float3 staticSwitch212_g3823 = lerpResult226_g3823;
				#else
				float3 staticSwitch212_g3823 = temp_cast_64;
				#endif
				float switchResult504 = (((ase_vface>0)?(1.0):(1.3)));
				float3 lerpResult505 = lerp( float3( 1,1,1 ) , ( staticSwitch212_g3823 * switchResult504 ) , ( CamNear492 * 0.9 ));
				float3 TintRes508 = lerpResult505;
				

				float3 BaseColor = AlbedoRes2511;
				float3 Normal = NormalResult220;
				float3 Emission = EmissionResult218;
				float3 Specular = temp_cast_30;
				float Metallic = 0;
				float Smoothness = SmoothnessRes556;
				float Occlusion = OcclusionRes773;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = BakedGI1077;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;
				
				//Shadowood: new properties below:
				float3 DebugVisuals = DebugResult223.xyz;
				float4 ShadowMask = ShadowMask1078;
				float4 FogColor = FogColorRes500;
				half FresnelControl = 0;
				half4 SSSControl = 0;
				half4 SSSColor = 0;
				float4 Tonemapping = appendResult11_g3819;
				float3 Tint = TintRes508;
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
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			//ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#define _ASE_DEBUGVISUALS 1
			#define _ASE_TONEMAPPING 1
			#define _NORMAL_DROPOFF_TS 1
			#define _ASE_TINT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define _SPECULAR_SETUP 1
			#define ASE_FOG 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define UNDERWATERELSEWHERE
			#define TONEMAPPINGELSEWHERE
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

			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma multi_compile_local __ _PROBEVOLUME_ON


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
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColor;
			float4 _Color;
			float4 _RefFresnel;
			float4 _FoamColor;
			float4 _RefractionColorMobile;
			float4 _CausticsTint;
			float4x4 _ProbeWorldToTexture;
			float4 _RefractionColor;
			float4 _FoamColor2;
			float3 _ProbeVolumeSizeInv;
			float3 _CameraForward;
			float3 _WorldPos;
			float3 _WorldDir;
			float3 _ProbeVolumeMin;
			float _Smoothness;
			float _RefractionIndexUnderMobile;
			float _CubemapStrength;
			float _ForceEye;
			float _PlanarNormalStrengthm;
			float _ColorMix;
			float _ColorMixBack;
			float _ReflectionShadowedEnd;
			float _WaterSaturation;
			float _CausticRotate;
			float _CausticFlowStrength;
			float _CausticFlowSpeed;
			float _Specular;
			float _ReflectionShadowedStart;
			float _WaterDepthPow;
			float _RemapWaterDistance;
			float _ReflectionShadowed;
			float _ZWriteToggle;
			float _Size;
			float _FlowStrength;
			float _FlowSpeed;
			float _FoamPowA;
			float _Foam2Pow;
			float _Foam2Con;
			float _FoamSize;
			float _FoamFlowStrength;
			float _FoamFlowSpeed;
			float _WaterDepthPow1;
			float _Foam_Mix;
			float _FoamPowM;
			float _RefractionIndex;
			float _RefractionIndexUnder;
			float _NormalSize;
			float _NormFlowStrength;
			float _FlowSpeedN;
			float _NormalStrength;
			float _DepthFade;
			float _RefractionFake;
			float _RemapWaterDistance1;
			float _FoamSize2;
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

			float3 _CausticsDir;
			half4 _CausticsColor;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE3D(_ProbeVolumeShR);
			SAMPLER(sampler_ProbeVolumeShR);
			TEXTURE3D(_ProbeVolumeShG);
			SAMPLER(sampler_ProbeVolumeShG);
			TEXTURE3D(_ProbeVolumeAmbient);
			SAMPLER(sampler_ProbeVolumeAmbient);
			TEXTURE3D(_ProbeVolumeShB);
			SAMPLER(sampler_ProbeVolumeShB);
			TEXTURE3D(_ProbeVolumeOcc);
			SAMPLER(sampler_ProbeVolumeOcc);


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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 VertexOffset1098 = ( v.ase_normal * GlobalOceanOffset );
				

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOffset1098;

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

				

				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
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

			#define _ASE_DEBUGVISUALS 1
			#define _ASE_TONEMAPPING 1
			#define _NORMAL_DROPOFF_TS 1
			#define _ASE_TINT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define _SPECULAR_SETUP 1
			#pragma shader_feature_local_fragment _SPECULAR_SETUP
			#define ASE_FOG 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define REQUIRE_DEPTH_TEXTURE 1
			#define REQUIRE_OPAQUE_TEXTURE 1
			#define UNDERWATERELSEWHERE
			#define TONEMAPPINGELSEWHERE
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

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma multi_compile_local __ _PROBEVOLUME_ON
			#pragma shader_feature_local _USEFLUIDFORFOAM_ON
			#pragma shader_feature_local _REFRACTION_ON
			#pragma shader_feature_local _USEDEPTHFADE_ON
			#pragma shader_feature_local _USEDEPTHCOLORINGLUT_ON
			#pragma shader_feature _PLANARREFLECTION_ON


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
			float4 _BaseColor;
			float4 _Color;
			float4 _RefFresnel;
			float4 _FoamColor;
			float4 _RefractionColorMobile;
			float4 _CausticsTint;
			float4x4 _ProbeWorldToTexture;
			float4 _RefractionColor;
			float4 _FoamColor2;
			float3 _ProbeVolumeSizeInv;
			float3 _CameraForward;
			float3 _WorldPos;
			float3 _WorldDir;
			float3 _ProbeVolumeMin;
			float _Smoothness;
			float _RefractionIndexUnderMobile;
			float _CubemapStrength;
			float _ForceEye;
			float _PlanarNormalStrengthm;
			float _ColorMix;
			float _ColorMixBack;
			float _ReflectionShadowedEnd;
			float _WaterSaturation;
			float _CausticRotate;
			float _CausticFlowStrength;
			float _CausticFlowSpeed;
			float _Specular;
			float _ReflectionShadowedStart;
			float _WaterDepthPow;
			float _RemapWaterDistance;
			float _ReflectionShadowed;
			float _ZWriteToggle;
			float _Size;
			float _FlowStrength;
			float _FlowSpeed;
			float _FoamPowA;
			float _Foam2Pow;
			float _Foam2Con;
			float _FoamSize;
			float _FoamFlowStrength;
			float _FoamFlowSpeed;
			float _WaterDepthPow1;
			float _Foam_Mix;
			float _FoamPowM;
			float _RefractionIndex;
			float _RefractionIndexUnder;
			float _NormalSize;
			float _NormFlowStrength;
			float _FlowSpeedN;
			float _NormalStrength;
			float _DepthFade;
			float _RefractionFake;
			float _RemapWaterDistance1;
			float _FoamSize2;
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

			float3 _CausticsDir;
			half4 _CausticsColor;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE3D(_ProbeVolumeShR);
			SAMPLER(sampler_ProbeVolumeShR);
			TEXTURE3D(_ProbeVolumeShG);
			SAMPLER(sampler_ProbeVolumeShG);
			TEXTURE3D(_ProbeVolumeAmbient);
			SAMPLER(sampler_ProbeVolumeAmbient);
			TEXTURE3D(_ProbeVolumeShB);
			SAMPLER(sampler_ProbeVolumeShB);
			TEXTURE3D(_ProbeVolumeOcc);
			SAMPLER(sampler_ProbeVolumeOcc);
			TEXTURE2D(_FlowTex);
			TEXTURE2D(_FluidVelocity);
			SAMPLER(sampler_FluidVelocity);
			SAMPLER(sampler_Linear_Repeat);
			TEXTURE2D(_FluidColor);
			SAMPLER(sampler_FluidColor);
			TEXTURE2D(_FoamTex);
			TEXTURE2D(_NormalMap);
			TEXTURE2D(_ColorCache);
			SAMPLER(sampler_ColorCache);
			TEXTURE2D(_DepthColorLUT);
			SAMPLER(sampler_Linear_Clamp);
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
			
			int IsStereoEyeLeft11_g3305( float3 WorldCamPos, float3 WorldCamRight, float force )
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
			
			float3 MatrixMulThatWorks( float4 Pos, float4x4 Mat )
			{
				float3 result = mul(Mat,Pos.xyz);
				return result + float3(Mat[0][3],Mat[1][3],Mat[2][3]);
			}
			
			float CameraDepthTexture55_g3390( float2 uv )
			{
				float4 color = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, uv);
				return color.r;
			}
			
			float MyCustomExpression44_g3390( float rawDepth )
			{
				return LinearEyeDepth(rawDepth, _ZBufferParams);
			}
			
			float3 MyCustomExpression16_g3390( float3 view, float sceneZ )
			{
				return  _WorldSpaceCameraPos - view * sceneZ / dot(UNITY_MATRIX_I_V._13_23_33, view);
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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 VertexOffset1098 = ( v.ase_normal * GlobalOceanOffset );
				
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord5 = screenPos;
				
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord6.xyz = ase_worldNormal;
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord7.xyz = ase_worldTangent;
				float ase_vertexTangentSign = v.ase_tangent.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord8.xyz = ase_worldBitangent;
				float3 _Vector3 = float3(0,0,-1);
				float4 Pos6_g3442 = float4( _Vector3 , 0.0 );
				float4x4 Mat6_g3442 = _CausticMatrix;
				float3 localMatrixMulThatWorks6_g3442 = MatrixMulThatWorks( Pos6_g3442 , Mat6_g3442 );
				float3 normalizeResult147_g3440 = normalize( localMatrixMulThatWorks6_g3442 );
				float3 vertexToFrag144_g3440 = normalizeResult147_g3440;
				o.ase_texcoord9.xyz = vertexToFrag144_g3440;
				float3 objToWorld3_g3390 = mul( GetObjectToWorldMatrix(), float4( v.vertex.xyz, 1 ) ).xyz;
				float3 vertexToFrag7_g3390 = objToWorld3_g3390;
				o.ase_texcoord10.xyz = vertexToFrag7_g3390;
				float4 Pos6_g3446 = float4( _Vector3 , 0.0 );
				float4x4 Mat6_g3446 = _CausticMatrix;
				float3 localMatrixMulThatWorks6_g3446 = MatrixMulThatWorks( Pos6_g3446 , Mat6_g3446 );
				float3 normalizeResult147_g3444 = normalize( localMatrixMulThatWorks6_g3446 );
				float3 vertexToFrag144_g3444 = normalizeResult147_g3444;
				o.ase_texcoord11.xyz = vertexToFrag144_g3444;
				
				o.ase_texcoord4.xy = v.texcoord0.xy;
				o.ase_texcoord12 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord4.zw = 0;
				o.ase_texcoord6.w = 0;
				o.ase_texcoord7.w = 0;
				o.ase_texcoord8.w = 0;
				o.ase_texcoord9.w = 0;
				o.ase_texcoord10.w = 0;
				o.ase_texcoord11.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOffset1098;

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

				float switchResult530 = (((ase_vface>0)?(1.0):(saturate( (0.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - 0.1) * (1.0 - 0.0) / (0.6 - 0.1)) ))));
				float CamNear492 = switchResult530;
				float3 appendResult454 = (float3(_FoamColor2.rgb));
				float2 texCoord126 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float2 UV260 = texCoord126;
				float2 temp_output_141_0 = (UV260*_Size + 0.0);
				float2 temp_output_2_0_g3437 = temp_output_141_0;
				float2 temp_output_4_0_g3437 = (temp_output_2_0_g3437).xy;
				float2 texCoord54 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode53 = SAMPLE_TEXTURE2D( _FluidVelocity, sampler_FluidVelocity, texCoord54 );
				float2 appendResult55 = (float2(tex2DNode53.r , tex2DNode53.g));
				float2 temp_output_102_0 = (float2( 0,0 ) + (appendResult55 - float2( -1,-1 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( -1,-1 )));
				float2 VelNormalised242 = temp_output_102_0;
				float2 temp_output_18_0_g3437 = VelNormalised242;
				float temp_output_144_0 = ( _FlowStrength * ( _Size / 8.0 ) );
				float2 temp_cast_1 = (temp_output_144_0).xx;
				float2 temp_output_17_0_g3437 = temp_cast_1;
				float mulTime22_g3437 = _TimeParameters.x * _FlowSpeed;
				float temp_output_27_0_g3437 = frac( mulTime22_g3437 );
				float2 temp_output_11_0_g3437 = ( temp_output_4_0_g3437 + ( -(temp_output_18_0_g3437*2.0 + -1.0) * temp_output_17_0_g3437 * temp_output_27_0_g3437 ) );
				float temp_output_29_0_g3437 = frac( ( mulTime22_g3437 + 0.5 ) );
				float2 temp_output_12_0_g3437 = ( temp_output_4_0_g3437 + ( -(temp_output_18_0_g3437*2.0 + -1.0) * temp_output_17_0_g3437 * temp_output_29_0_g3437 ) );
				float temp_output_32_0_g3437 = ( abs( ( temp_output_27_0_g3437 - 0.5 ) ) / 0.5 );
				float4 lerpResult9_g3437 = lerp( SAMPLE_TEXTURE2D( _FlowTex, sampler_Linear_Repeat, temp_output_11_0_g3437 ) , SAMPLE_TEXTURE2D( _FlowTex, sampler_Linear_Repeat, temp_output_12_0_g3437 ) , temp_output_32_0_g3437);
				float FlowWaterTex246 = saturate( pow( lerpResult9_g3437.r , _FoamPowA ) );
				float2 uv_FluidColor103 = IN.ase_texcoord4.xy;
				float4 tex2DNode103 = SAMPLE_TEXTURE2D( _FluidColor, sampler_FluidColor, uv_FluidColor103 );
				float FluidColorA435 = tex2DNode103.a;
				float temp_output_105_0 = ( saturate( FlowWaterTex246 ) * FluidColorA435 );
				float lerpResult4_g3346 = lerp( 0.5 , saturate( pow( temp_output_105_0 , _Foam2Pow ) ) , _Foam2Con);
				float clampResult11_g3346 = clamp( ( lerpResult4_g3346 + 0.0 ) , 0.0 , 1.0 );
				float3 FlowWaterTexRes445 = ( appendResult454 * clampResult11_g3346 );
				#ifdef _USEFLUIDFORFOAM_ON
				float3 staticSwitch107 = FlowWaterTexRes445;
				#else
				float3 staticSwitch107 = float3( 0,0,0 );
				#endif
				float2 temp_output_2_0_g3439 = (UV260*_FoamSize + 0.0);
				float2 temp_output_4_0_g3439 = (temp_output_2_0_g3439).xy;
				float2 temp_output_18_0_g3439 = VelNormalised242;
				float temp_output_148_0 = ( _FoamFlowStrength * ( _FoamSize / 8.0 ) );
				float2 temp_cast_2 = (temp_output_148_0).xx;
				float2 temp_output_17_0_g3439 = temp_cast_2;
				float mulTime22_g3439 = _TimeParameters.x * _FoamFlowSpeed;
				float temp_output_27_0_g3439 = frac( mulTime22_g3439 );
				float2 temp_output_11_0_g3439 = ( temp_output_4_0_g3439 + ( -(temp_output_18_0_g3439*2.0 + -1.0) * temp_output_17_0_g3439 * temp_output_27_0_g3439 ) );
				float temp_output_29_0_g3439 = frac( ( mulTime22_g3439 + 0.5 ) );
				float2 temp_output_12_0_g3439 = ( temp_output_4_0_g3439 + ( -(temp_output_18_0_g3439*2.0 + -1.0) * temp_output_17_0_g3439 * temp_output_29_0_g3439 ) );
				float temp_output_32_0_g3439 = ( abs( ( temp_output_27_0_g3439 - 0.5 ) ) / 0.5 );
				float4 lerpResult9_g3439 = lerp( SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_11_0_g3439 ) , SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_12_0_g3439 ) , temp_output_32_0_g3439);
				float FoamA265 = lerpResult9_g3439.r;
				float temp_output_125_0 = ( 1.0 - saturate( 0.0 ) );
				float FoamMask114 = ( temp_output_125_0 * _Foam_Mix );
				float2 temp_output_2_0_g3436 = (UV260*_FoamSize2 + 0.0);
				float2 temp_output_4_0_g3436 = (temp_output_2_0_g3436).xy;
				float2 temp_output_18_0_g3436 = VelNormalised242;
				float2 temp_cast_3 = (temp_output_148_0).xx;
				float2 temp_output_17_0_g3436 = temp_cast_3;
				float mulTime22_g3436 = _TimeParameters.x * _FoamFlowSpeed;
				float temp_output_27_0_g3436 = frac( mulTime22_g3436 );
				float2 temp_output_11_0_g3436 = ( temp_output_4_0_g3436 + ( -(temp_output_18_0_g3436*2.0 + -1.0) * temp_output_17_0_g3436 * temp_output_27_0_g3436 ) );
				float temp_output_29_0_g3436 = frac( ( mulTime22_g3436 + 0.5 ) );
				float2 temp_output_12_0_g3436 = ( temp_output_4_0_g3436 + ( -(temp_output_18_0_g3436*2.0 + -1.0) * temp_output_17_0_g3436 * temp_output_29_0_g3436 ) );
				float temp_output_32_0_g3436 = ( abs( ( temp_output_27_0_g3436 - 0.5 ) ) / 0.5 );
				float4 lerpResult9_g3436 = lerp( SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_11_0_g3436 ) , SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_12_0_g3436 ) , temp_output_32_0_g3436);
				float FoamB267 = lerpResult9_g3436.r;
				float temp_output_155_0 = saturate( pow( saturate( ( ( FoamA265 * FoamMask114 ) + ( FoamB267 * FoamMask114 ) ) ) , _FoamPowM ) );
				float3 appendResult337 = (float3(_FoamColor.rgb));
				float3 FoamResult271 = ( temp_output_155_0 * appendResult337 );
				float3 temp_output_118_0 = ( staticSwitch107 + FoamResult271 );
				float4 screenPos = IN.ase_texcoord5;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 appendResult639 = (float2(ase_screenPosNorm.xy));
				float switchResult329 = (((ase_vface>0)?(_RefractionIndex):(_RefractionIndexUnder)));
				float2 temp_output_185_0 = (UV260*_NormalSize + 0.0);
				float2 temp_output_2_0_g3438 = temp_output_185_0;
				float2 temp_output_4_0_g3438 = (temp_output_2_0_g3438).xy;
				float2 temp_output_18_0_g3438 = VelNormalised242;
				float temp_output_183_0 = ( _NormFlowStrength * ( _NormalSize / 8.0 ) );
				float2 temp_cast_6 = (temp_output_183_0).xx;
				float2 temp_output_17_0_g3438 = temp_cast_6;
				float mulTime22_g3438 = _TimeParameters.x * _FlowSpeedN;
				float temp_output_27_0_g3438 = frac( mulTime22_g3438 );
				float2 temp_output_11_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_27_0_g3438 ) );
				float2 temp_output_83_0 = (float2( -1,-1 ) + (temp_output_102_0 - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
				float VelLength236 = length( temp_output_83_0 );
				float temp_output_188_0 = ( VelLength236 * _NormalStrength );
				float temp_output_55_0_g3438 = temp_output_188_0;
				float3 unpack48_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_Linear_Repeat, temp_output_11_0_g3438 ), temp_output_55_0_g3438 );
				unpack48_g3438.z = lerp( 1, unpack48_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_29_0_g3438 = frac( ( mulTime22_g3438 + 0.5 ) );
				float2 temp_output_12_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_29_0_g3438 ) );
				float3 unpack49_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_Linear_Repeat, temp_output_12_0_g3438 ), temp_output_55_0_g3438 );
				unpack49_g3438.z = lerp( 1, unpack49_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_32_0_g3438 = ( abs( ( temp_output_27_0_g3438 - 0.5 ) ) / 0.5 );
				float3 lerpResult9_g3438 = lerp( unpack48_g3438 , unpack49_g3438 , temp_output_32_0_g3438);
				float3 NormalResult220 = lerpResult9_g3438;
				float2 appendResult210 = (float2(NormalResult220.xy));
				float2 temp_output_211_0 = ( switchResult329 * appendResult210 * CamNear492 );
				float2 temp_output_209_0 = ( appendResult639 + temp_output_211_0 );
				float eyeDepth637 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( temp_output_209_0, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float2 lerpResult640 = lerp( appendResult639 , temp_output_209_0 , step( ( screenPos.w - eyeDepth637 ) , 0.0 ));
				float2 ScreenSpaceRefractUV691 = lerpResult640;
				float eyeDepth6_g3347 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV691, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3347 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float temp_output_12_0_g3347 = _DepthFade;
				float temp_output_3_0_g3347 = ( abs( ( eyeDepth6_g3347 - -worldToView21_g3347.z ) ) / temp_output_12_0_g3347 );
				#ifdef _USEDEPTHFADE_ON
				float staticSwitch315 = saturate( temp_output_3_0_g3347 );
				#else
				float staticSwitch315 = 1.0;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch776 = staticSwitch315;
				#else
				float staticSwitch776 = 1.0;
				#endif
				float ShoreMaskDepth301 = staticSwitch776;
				float3 temp_output_312_0 = ( temp_output_118_0 * ShoreMaskDepth301 );
				float3 appendResult336 = (float3(_Color.rgb));
				float3 AlbedoResult225 = ( temp_output_312_0 + ( appendResult336 * _Color.a * saturate( (0.0 + (ShoreMaskDepth301 - 0.0) * (1.0 - 0.0) / (0.09 - 0.0)) ) ) );
				float3 AlbedoIn285_g3823 = AlbedoResult225;
				#ifdef _USEUNDERWATER
				float3 staticSwitch214_g3823 = AlbedoIn285_g3823;
				#else
				float3 staticSwitch214_g3823 = AlbedoIn285_g3823;
				#endif
				float3 AlbedoRes2511 = ( CamNear492 * staticSwitch214_g3823 );
				
				float3 temp_cast_11 = (1.0).xxx;
				float2 texCoord355 = IN.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_output_363_0 = ( ( _RefractionFake * 0.1 ) * appendResult210 * ShoreMaskDepth301 );
				float4 tex2DNode350 = SAMPLE_TEXTURE2D( _ColorCache, sampler_ColorCache, ( texCoord355 + temp_output_363_0 ) );
				float ColorCacheADistorted787 = tex2DNode350.a;
				float temp_output_719_0 = saturate( ( ( ColorCacheADistorted787 * 0.1 ) / _RemapWaterDistance1 ) );
				float eyeDepth6_g3301 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV691, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3301 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord6.xyz;
				float fresnelNdotV747 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode747 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV747, 5.0 ) );
				float lerpResult745 = lerp( 9.0 , 30.0 , saturate( fresnelNode747 ));
				float DepthFadeDistthing744 = lerpResult745;
				float temp_output_12_0_g3301 = ( DepthFadeDistthing744 * _RemapWaterDistance );
				float temp_output_3_0_g3301 = ( abs( ( eyeDepth6_g3301 - -worldToView21_g3301.z ) ) / temp_output_12_0_g3301 );
				float temp_output_716_0 = saturate( temp_output_3_0_g3301 );
				#ifdef _REFRACTION_ON
				float staticSwitch738 = temp_output_716_0;
				#else
				float staticSwitch738 = temp_output_719_0;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch714 = _WaterDepthPow;
				#else
				float staticSwitch714 = _WaterDepthPow1;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch725 = ( 1.0 - pow( ( 1.0 - temp_output_716_0 ) , 6.0 ) );
				#else
				float staticSwitch725 = ( 1.0 - pow( ( 1.0 - temp_output_719_0 ) , 6.0 ) );
				#endif
				float2 appendResult711 = (float2(staticSwitch725 , 0.0));
				float3 appendResult705 = (float3(SAMPLE_TEXTURE2D_LOD( _DepthColorLUT, sampler_Linear_Clamp, appendResult711, 0.0 ).rgb));
				float temp_output_2_0_g3304 = saturate( ShoreMaskDepth301 );
				float temp_output_3_0_g3304 = ( 1.0 - temp_output_2_0_g3304 );
				float3 appendResult7_g3304 = (float3(temp_output_3_0_g3304 , temp_output_3_0_g3304 , temp_output_3_0_g3304));
				float3 desaturateInitialColor739 = ( ( ( saturate( pow( ( 1.0 - staticSwitch738 ) , staticSwitch714 ) ) * appendResult705 ) * temp_output_2_0_g3304 ) + appendResult7_g3304 );
				float desaturateDot739 = dot( desaturateInitialColor739, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar739 = lerp( desaturateInitialColor739, desaturateDot739.xxx, ( 1.0 - _WaterSaturation ) );
				#ifdef _USEDEPTHCOLORINGLUT_ON
				float3 staticSwitch785 = desaturateVar739;
				#else
				float3 staticSwitch785 = temp_cast_11;
				#endif
				float3 switchResult742 = (((ase_vface>0)?(staticSwitch785):(float3( 0,0,0 ))));
				float3 WaterDepthTint743 = switchResult742;
				#ifdef _REFRACTION_ON
				float2 staticSwitch917 = ( temp_output_211_0 * ShoreMaskDepth301 );
				#else
				float2 staticSwitch917 = ( appendResult210 * _RefractionIndexUnderMobile );
				#endif
				float2 RefractionBacksideWarp650 = staticSwitch917;
				float3 temp_output_647_0 = SHADERGRAPH_REFLECTION_PROBE(ase_worldViewDir,float3( ( RefractionBacksideWarp650 * 4.0 ) ,  0.0 ),1.0);
				float3 temp_output_564_0 = ( (temp_output_647_0).xyz * _CubemapStrength );
				float3 switchResult592 = (((ase_vface>0)?(float3( 0,0,0 )):(temp_output_564_0)));
				float3 WorldCamPos11_g3305 = _WorldPos;
				float3 WorldCamRight11_g3305 = _WorldDir;
				float force11_g3305 = _ForceEye;
				int localIsStereoEyeLeft11_g3305 = IsStereoEyeLeft11_g3305( WorldCamPos11_g3305 , WorldCamRight11_g3305 , force11_g3305 );
				float2 appendResult4_g3305 = (float2(ase_screenPosNorm.xy));
				float2 appendResult585 = (float2(NormalResult220.xy));
				float2 temp_output_1_0_g3305 = ( appendResult4_g3305 + ( appendResult585 * CamNear492 * _PlanarNormalStrengthm ) );
				float4 tex2DNode3_g3305 = SAMPLE_TEXTURE2D( _TexLeft, sampler_Linear_Mirror, temp_output_1_0_g3305 );
				float4 tex2DNode2_g3305 = SAMPLE_TEXTURE2D( _TexRight, sampler_Linear_Mirror, temp_output_1_0_g3305 );
				float4 ifLocalVar10_g3305 = 0;
				if( localIsStereoEyeLeft11_g3305 <= 0.0 )
				ifLocalVar10_g3305 = tex2DNode2_g3305;
				else
				ifLocalVar10_g3305 = tex2DNode3_g3305;
				float3 ase_worldTangent = IN.ase_texcoord7.xyz;
				float3 ase_worldBitangent = IN.ase_texcoord8.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal581 = NormalResult220;
				float3 worldNormal581 = float3(dot(tanToWorld0,tanNormal581), dot(tanToWorld1,tanNormal581), dot(tanToWorld2,tanNormal581));
				float fresnelNdotV579 = dot( worldNormal581, ase_worldViewDir );
				float fresnelNode579 = ( _RefFresnel.x + _RefFresnel.y * pow( 1.0 - fresnelNdotV579, _RefFresnel.z ) );
				float3 switchResult588 = (((ase_vface>0)?(( (ifLocalVar10_g3305).rgb * saturate( fresnelNode579 ) )):(temp_output_564_0)));
				#ifdef _PLANARREFLECTION_ON
				float3 staticSwitch574 = switchResult588;
				#else
				float3 staticSwitch574 = switchResult592;
				#endif
				float3 ReflectionRes554 = staticSwitch574;
				float switchResult418 = (((ase_vface>0)?(_ColorMix):(_ColorMixBack)));
				float3 appendResult352 = (float3(tex2DNode350.rgb));
				#ifdef _REFRACTION_ON
				float4 staticSwitch894 = _RefractionColor;
				#else
				float4 staticSwitch894 = _RefractionColorMobile;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch889 = 1.0;
				#else
				float staticSwitch889 = saturate( (0.0 + (ColorCacheADistorted787 - 0.02) * (1.0 - 0.0) / (0.15 - 0.02)) );
				#endif
				float ShoreMaskThing883 = staticSwitch889;
				float4 lerpResult885 = lerp( float4( 1,1,1,0 ) , staticSwitch894 , ShoreMaskThing883);
				float3 appendResult331 = (float3(lerpResult885.rgb));
				float3 temp_output_354_0 = ( switchResult418 * appendResult352 * appendResult331 );
				float4 fetchOpaqueVal203 = float4( SHADERGRAPH_SAMPLE_SCENE_COLOR( ScreenSpaceRefractUV691 ), 1.0 );
				float3 appendResult330 = (float3(fetchOpaqueVal203.rgb));
				float3 lerpResult317 = lerp( appendResult330 , ( appendResult330 * appendResult331 ) , ShoreMaskDepth301);
				#ifdef _REFRACTION_ON
				float3 staticSwitch277 = lerpResult317;
				#else
				float3 staticSwitch277 = temp_output_354_0;
				#endif
				float3 temp_output_771_0 = ( ReflectionRes554 + staticSwitch277 );
				float3 lerpResult529 = lerp( appendResult330 , staticSwitch277 , CamNear492);
				float3 temp_output_18_0_g3440 = float3( 0,0,0 );
				float3 temp_output_30_0_g3440 = float3( 0,0,1 );
				float3 tanNormal12_g3440 = temp_output_30_0_g3440;
				float3 worldNormal12_g3440 = float3(dot(tanToWorld0,tanNormal12_g3440), dot(tanToWorld1,tanNormal12_g3440), dot(tanToWorld2,tanNormal12_g3440));
				float3 vertexToFrag144_g3440 = IN.ase_texcoord9.xyz;
				float dotResult1_g3440 = dot( worldNormal12_g3440 , vertexToFrag144_g3440 );
				float3 vertexToFrag7_g3390 = IN.ase_texcoord10.xyz;
				float3 positionWS_fogFactor24_g3390 = vertexToFrag7_g3390;
				float3 normalizeResult21_g3390 = normalize( ( _WorldSpaceCameraPos - positionWS_fogFactor24_g3390 ) );
				float3 view23_g3390 = normalizeResult21_g3390;
				float3 view16_g3390 = view23_g3390;
				float3 appendResult49_g3390 = (float3(ScreenSpaceRefractUV691 , ase_screenPosNorm.w));
				float3 screenPosXYW29_g3390 = (appendResult49_g3390).xyz;
				float2 uvDepth38_g3390 = ( (screenPosXYW29_g3390).xy / (screenPosXYW29_g3390).z );
				float2 uv55_g3390 = uvDepth38_g3390;
				float localCameraDepthTexture55_g3390 = CameraDepthTexture55_g3390( uv55_g3390 );
				float rawDepth40_g3390 = localCameraDepthTexture55_g3390;
				float rawDepth44_g3390 = rawDepth40_g3390;
				float localMyCustomExpression44_g3390 = MyCustomExpression44_g3390( rawDepth44_g3390 );
				float sceneZ41_g3390 = localMyCustomExpression44_g3390;
				float sceneZ16_g3390 = sceneZ41_g3390;
				float3 localMyCustomExpression16_g3390 = MyCustomExpression16_g3390( view16_g3390 , sceneZ16_g3390 );
				float3 scenePos11_g3390 = localMyCustomExpression16_g3390;
				float3 WorldPosition678 = scenePos11_g3390;
				float2 temp_output_2_0_g3452 = (WorldPosition678).xz;
				float2 temp_output_4_0_g3452 = (temp_output_2_0_g3452).xy;
				float2 VelNeg231 = temp_output_83_0;
				float2 break969 = VelNeg231;
				float3 appendResult992 = (float3(break969.x , 0.0 , break969.y));
				float3 rotatedValue998 = RotateAroundAxis( float3( 0,0,0 ), float3( (mul( GetObjectToWorldMatrix(), float4( appendResult992 , 0.0 ) ).xyz).xz ,  0.0 ), float3( 0,0,1 ), radians( _CausticRotate ) );
				float2 temp_output_18_0_g3452 = (rotatedValue998).xy;
				float2 temp_cast_23 = (-_CausticFlowStrength).xx;
				float2 temp_output_17_0_g3452 = temp_cast_23;
				float mulTime22_g3452 = _TimeParameters.x * _CausticFlowSpeed;
				float temp_output_27_0_g3452 = frac( mulTime22_g3452 );
				float2 break924 = ( temp_output_4_0_g3452 + ( -temp_output_18_0_g3452 * temp_output_17_0_g3452 * temp_output_27_0_g3452 ) );
				float temp_output_979_0 = (WorldPosition678).y;
				float3 appendResult923 = (float3(break924.x , temp_output_979_0 , break924.y));
				float3 temp_output_105_0_g3440 = appendResult923;
				float2 PanSpeed131_g3440 = (_CausticsSettings).xy;
				float mulTime28_g3440 = _TimeParameters.x * -1.0;
				float2 break101_g3440 = ( PanSpeed131_g3440 * mulTime28_g3440 );
				float3 appendResult100_g3440 = (float3(break101_g3440.x , 0.0 , break101_g3440.y));
				float4 Pos6_g3443 = float4( ( temp_output_105_0_g3440 + appendResult100_g3440 ) , 0.0 );
				float4x4 invertVal146_g3440 = Inverse4x4( _CausticMatrix );
				float4x4 Mat6_g3443 = invertVal146_g3440;
				float3 localMatrixMulThatWorks6_g3443 = MatrixMulThatWorks( Pos6_g3443 , Mat6_g3443 );
				float3 appendResult121_g3440 = (float3(SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3443).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3443).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3443).xy ).r));
				float3 appendResult62_g3440 = (float3(_CausticsColor.rgb));
				float temp_output_117_0_g3440 = (temp_output_105_0_g3440).y;
				float GlobalOceanHeight515 = ( GlobalOceanHeight + (0) );
				float temp_output_63_0_g3440 = GlobalOceanHeight515;
				float2 DistanceFade134_g3440 = (_CausticsSettings).zw;
				float2 break136_g3440 = DistanceFade134_g3440;
				float temp_output_67_0_g3440 = ( saturate( (0.0 + (max( -( temp_output_117_0_g3440 - temp_output_63_0_g3440 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) * saturate( (1.0 + (distance( temp_output_63_0_g3440 , temp_output_117_0_g3440 ) - break136_g3440.x) * (0.0 - 1.0) / (break136_g3440.y - break136_g3440.x)) ) );
				float3 temp_output_20_0_g3440 = ( temp_output_18_0_g3440 + ( max( dotResult1_g3440 , 0.0 ) * appendResult121_g3440 * appendResult62_g3440 * temp_output_67_0_g3440 ) );
				float3 temp_output_18_0_g3444 = float3( 0,0,0 );
				float3 temp_output_30_0_g3444 = float3( 0,0,1 );
				float3 tanNormal12_g3444 = temp_output_30_0_g3444;
				float3 worldNormal12_g3444 = float3(dot(tanToWorld0,tanNormal12_g3444), dot(tanToWorld1,tanNormal12_g3444), dot(tanToWorld2,tanNormal12_g3444));
				float3 vertexToFrag144_g3444 = IN.ase_texcoord11.xyz;
				float dotResult1_g3444 = dot( worldNormal12_g3444 , vertexToFrag144_g3444 );
				float temp_output_29_0_g3452 = frac( ( mulTime22_g3452 + 0.5 ) );
				float2 break965 = ( temp_output_4_0_g3452 + ( -temp_output_18_0_g3452 * temp_output_17_0_g3452 * temp_output_29_0_g3452 ) );
				float3 appendResult966 = (float3(break965.x , temp_output_979_0 , break965.y));
				float3 temp_output_105_0_g3444 = appendResult966;
				float2 PanSpeed131_g3444 = (_CausticsSettings).xy;
				float mulTime28_g3444 = _TimeParameters.x * -1.0;
				float2 break101_g3444 = ( PanSpeed131_g3444 * mulTime28_g3444 );
				float3 appendResult100_g3444 = (float3(break101_g3444.x , 0.0 , break101_g3444.y));
				float4 Pos6_g3447 = float4( ( temp_output_105_0_g3444 + appendResult100_g3444 ) , 0.0 );
				float4x4 invertVal146_g3444 = Inverse4x4( _CausticMatrix );
				float4x4 Mat6_g3447 = invertVal146_g3444;
				float3 localMatrixMulThatWorks6_g3447 = MatrixMulThatWorks( Pos6_g3447 , Mat6_g3447 );
				float3 appendResult121_g3444 = (float3(SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3447).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3447).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3447).xy ).r));
				float3 appendResult62_g3444 = (float3(_CausticsColor.rgb));
				float temp_output_117_0_g3444 = (temp_output_105_0_g3444).y;
				float temp_output_63_0_g3444 = GlobalOceanHeight515;
				float2 DistanceFade134_g3444 = (_CausticsSettings).zw;
				float2 break136_g3444 = DistanceFade134_g3444;
				float temp_output_67_0_g3444 = ( saturate( (0.0 + (max( -( temp_output_117_0_g3444 - temp_output_63_0_g3444 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) * saturate( (1.0 + (distance( temp_output_63_0_g3444 , temp_output_117_0_g3444 ) - break136_g3444.x) * (0.0 - 1.0) / (break136_g3444.y - break136_g3444.x)) ) );
				float3 temp_output_20_0_g3444 = ( temp_output_18_0_g3444 + ( max( dotResult1_g3444 , 0.0 ) * appendResult121_g3444 * appendResult62_g3444 * temp_output_67_0_g3444 ) );
				float temp_output_32_0_g3452 = ( abs( ( temp_output_27_0_g3452 - 0.5 ) ) / 0.5 );
				float3 lerpResult958 = lerp( temp_output_20_0_g3440 , temp_output_20_0_g3444 , temp_output_32_0_g3452);
				float3 appendResult859 = (float3(_CausticsTint.rgb));
				float3 temp_output_856_0 = ( lerpResult958 * appendResult859 * 3.0 );
				#ifdef _REFRACTION_ON
				float3 staticSwitch761 = temp_output_856_0;
				#else
				float3 staticSwitch761 = float3( 0,0,0 );
				#endif
				float3 Caustics671 = staticSwitch761;
				float eyeDepth6_g3387 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV691, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 temp_output_14_0_g3387 = IN.ase_texcoord12.xyz;
				float3 temp_output_5_0_g3388 = temp_output_14_0_g3387;
				float3 objToWorld2_g3388 = mul( GetObjectToWorldMatrix(), float4( temp_output_5_0_g3388, 1 ) ).xyz;
				float3 worldToView1_g3388 = mul( UNITY_MATRIX_V, float4( objToWorld2_g3388, 1 ) ).xyz;
				float temp_output_12_0_g3387 = 1.0;
				float temp_output_3_0_g3387 = ( abs( ( eyeDepth6_g3387 - -worldToView1_g3388.z ) ) / temp_output_12_0_g3387 );
				float temp_output_912_0 = saturate( temp_output_3_0_g3387 );
				float luminance1004 = Luminance(appendResult330);
				#ifdef _REFRACTION_ON
				float3 staticSwitch766 = ( WaterDepthTint743 * ( ReflectionRes554 + lerpResult529 + ( Caustics671 * appendResult331 * 30.0 * ShoreMaskDepth301 * appendResult330 * temp_output_912_0 * saturate( (0.1 + (luminance1004 - 0.0) * (1.0 - 0.1) / (1.0 - 0.0)) ) ) ) );
				#else
				float3 staticSwitch766 = ( WaterDepthTint743 * temp_output_771_0 );
				#endif
				#ifdef _REFRACTION_ON
				float3 staticSwitch568 = lerpResult529;
				#else
				float3 staticSwitch568 = temp_output_771_0;
				#endif
				float3 switchResult591 = (((ase_vface>0)?(staticSwitch766):(staticSwitch568)));
				float3 EmissionResult218 = switchResult591;
				

				float3 BaseColor = AlbedoRes2511;
				float3 Emission = EmissionResult218;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

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
			Tags { "LightMode"="DepthNormals" }

			ZWrite On
			Blend One Zero
			ZTest LEqual
			ZWrite On
			AlphaToMask Off

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#define _ASE_DEBUGVISUALS 1
			#define _ASE_TONEMAPPING 1
			#define _NORMAL_DROPOFF_TS 1
			#define _ASE_TINT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define _SPECULAR_SETUP 1
			#define ASE_FOG 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define UNDERWATERELSEWHERE
			#define TONEMAPPINGELSEWHERE
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

			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma multi_compile_local __ _PROBEVOLUME_ON


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
			float4 _BaseColor;
			float4 _Color;
			float4 _RefFresnel;
			float4 _FoamColor;
			float4 _RefractionColorMobile;
			float4 _CausticsTint;
			float4x4 _ProbeWorldToTexture;
			float4 _RefractionColor;
			float4 _FoamColor2;
			float3 _ProbeVolumeSizeInv;
			float3 _CameraForward;
			float3 _WorldPos;
			float3 _WorldDir;
			float3 _ProbeVolumeMin;
			float _Smoothness;
			float _RefractionIndexUnderMobile;
			float _CubemapStrength;
			float _ForceEye;
			float _PlanarNormalStrengthm;
			float _ColorMix;
			float _ColorMixBack;
			float _ReflectionShadowedEnd;
			float _WaterSaturation;
			float _CausticRotate;
			float _CausticFlowStrength;
			float _CausticFlowSpeed;
			float _Specular;
			float _ReflectionShadowedStart;
			float _WaterDepthPow;
			float _RemapWaterDistance;
			float _ReflectionShadowed;
			float _ZWriteToggle;
			float _Size;
			float _FlowStrength;
			float _FlowSpeed;
			float _FoamPowA;
			float _Foam2Pow;
			float _Foam2Con;
			float _FoamSize;
			float _FoamFlowStrength;
			float _FoamFlowSpeed;
			float _WaterDepthPow1;
			float _Foam_Mix;
			float _FoamPowM;
			float _RefractionIndex;
			float _RefractionIndexUnder;
			float _NormalSize;
			float _NormFlowStrength;
			float _FlowSpeedN;
			float _NormalStrength;
			float _DepthFade;
			float _RefractionFake;
			float _RemapWaterDistance1;
			float _FoamSize2;
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

			float3 _CausticsDir;
			half4 _CausticsColor;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE3D(_ProbeVolumeShR);
			SAMPLER(sampler_ProbeVolumeShR);
			TEXTURE3D(_ProbeVolumeShG);
			SAMPLER(sampler_ProbeVolumeShG);
			TEXTURE3D(_ProbeVolumeAmbient);
			SAMPLER(sampler_ProbeVolumeAmbient);
			TEXTURE3D(_ProbeVolumeShB);
			SAMPLER(sampler_ProbeVolumeShB);
			TEXTURE3D(_ProbeVolumeOcc);
			SAMPLER(sampler_ProbeVolumeOcc);
			TEXTURE2D(_NormalMap);
			TEXTURE2D(_FluidVelocity);
			SAMPLER(sampler_FluidVelocity);
			SAMPLER(sampler_Linear_Repeat);


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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 VertexOffset1098 = ( v.ase_normal * GlobalOceanOffset );
				
				o.ase_texcoord5.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord5.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOffset1098;

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

				float2 texCoord126 = IN.ase_texcoord5.xy * float2( 1,1 ) + float2( 0,0 );
				float2 UV260 = texCoord126;
				float2 temp_output_185_0 = (UV260*_NormalSize + 0.0);
				float2 temp_output_2_0_g3438 = temp_output_185_0;
				float2 temp_output_4_0_g3438 = (temp_output_2_0_g3438).xy;
				float2 texCoord54 = IN.ase_texcoord5.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode53 = SAMPLE_TEXTURE2D( _FluidVelocity, sampler_FluidVelocity, texCoord54 );
				float2 appendResult55 = (float2(tex2DNode53.r , tex2DNode53.g));
				float2 temp_output_102_0 = (float2( 0,0 ) + (appendResult55 - float2( -1,-1 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( -1,-1 )));
				float2 VelNormalised242 = temp_output_102_0;
				float2 temp_output_18_0_g3438 = VelNormalised242;
				float temp_output_183_0 = ( _NormFlowStrength * ( _NormalSize / 8.0 ) );
				float2 temp_cast_0 = (temp_output_183_0).xx;
				float2 temp_output_17_0_g3438 = temp_cast_0;
				float mulTime22_g3438 = _TimeParameters.x * _FlowSpeedN;
				float temp_output_27_0_g3438 = frac( mulTime22_g3438 );
				float2 temp_output_11_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_27_0_g3438 ) );
				float2 temp_output_83_0 = (float2( -1,-1 ) + (temp_output_102_0 - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
				float VelLength236 = length( temp_output_83_0 );
				float temp_output_188_0 = ( VelLength236 * _NormalStrength );
				float temp_output_55_0_g3438 = temp_output_188_0;
				float3 unpack48_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_Linear_Repeat, temp_output_11_0_g3438 ), temp_output_55_0_g3438 );
				unpack48_g3438.z = lerp( 1, unpack48_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_29_0_g3438 = frac( ( mulTime22_g3438 + 0.5 ) );
				float2 temp_output_12_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_29_0_g3438 ) );
				float3 unpack49_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_Linear_Repeat, temp_output_12_0_g3438 ), temp_output_55_0_g3438 );
				unpack49_g3438.z = lerp( 1, unpack49_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_32_0_g3438 = ( abs( ( temp_output_27_0_g3438 - 0.5 ) ) / 0.5 );
				float3 lerpResult9_g3438 = lerp( unpack48_g3438 , unpack49_g3438 , temp_output_32_0_g3438);
				float3 NormalResult220 = lerpResult9_g3438;
				

				float3 Normal = NormalResult220;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
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
			ZWrite [_ZWriteToggle]
			ZTest LEqual
			Offset 0,0
			//ColorMask RGBA
			

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define _ASE_DEBUGVISUALS 1
			#define _ASE_TONEMAPPING 1
			#define _NORMAL_DROPOFF_TS 1
			#define _ASE_TINT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define _SPECULAR_SETUP 1
			#pragma shader_feature_local_fragment _SPECULAR_SETUP
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define REQUIRE_DEPTH_TEXTURE 1
			#define REQUIRE_OPAQUE_TEXTURE 1
			#define UNDERWATERELSEWHERE
			#define TONEMAPPINGELSEWHERE
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

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SCREEN_POSITION
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_BITANGENT
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_VERT_TANGENT
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma multi_compile_local __ _PROBEVOLUME_ON
			#pragma shader_feature_local _USEFLUIDFORFOAM_ON
			#pragma shader_feature_local _REFRACTION_ON
			#pragma shader_feature_local _USEDEPTHFADE_ON
			#pragma shader_feature_local _USEDEPTHCOLORINGLUT_ON
			#pragma shader_feature _PLANARREFLECTION_ON
			#pragma shader_feature_local _ProbeVolPerVertex


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
				float4 ase_texcoord14 : TEXCOORD14;
				float4 ase_texcoord15 : TEXCOORD15;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColor;
			float4 _Color;
			float4 _RefFresnel;
			float4 _FoamColor;
			float4 _RefractionColorMobile;
			float4 _CausticsTint;
			float4x4 _ProbeWorldToTexture;
			float4 _RefractionColor;
			float4 _FoamColor2;
			float3 _ProbeVolumeSizeInv;
			float3 _CameraForward;
			float3 _WorldPos;
			float3 _WorldDir;
			float3 _ProbeVolumeMin;
			float _Smoothness;
			float _RefractionIndexUnderMobile;
			float _CubemapStrength;
			float _ForceEye;
			float _PlanarNormalStrengthm;
			float _ColorMix;
			float _ColorMixBack;
			float _ReflectionShadowedEnd;
			float _WaterSaturation;
			float _CausticRotate;
			float _CausticFlowStrength;
			float _CausticFlowSpeed;
			float _Specular;
			float _ReflectionShadowedStart;
			float _WaterDepthPow;
			float _RemapWaterDistance;
			float _ReflectionShadowed;
			float _ZWriteToggle;
			float _Size;
			float _FlowStrength;
			float _FlowSpeed;
			float _FoamPowA;
			float _Foam2Pow;
			float _Foam2Con;
			float _FoamSize;
			float _FoamFlowStrength;
			float _FoamFlowSpeed;
			float _WaterDepthPow1;
			float _Foam_Mix;
			float _FoamPowM;
			float _RefractionIndex;
			float _RefractionIndexUnder;
			float _NormalSize;
			float _NormFlowStrength;
			float _FlowSpeedN;
			float _NormalStrength;
			float _DepthFade;
			float _RefractionFake;
			float _RemapWaterDistance1;
			float _FoamSize2;
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

			float3 _CausticsDir;
			half4 _CausticsColor;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE3D(_ProbeVolumeShR);
			SAMPLER(sampler_ProbeVolumeShR);
			TEXTURE3D(_ProbeVolumeShG);
			SAMPLER(sampler_ProbeVolumeShG);
			TEXTURE3D(_ProbeVolumeAmbient);
			SAMPLER(sampler_ProbeVolumeAmbient);
			TEXTURE3D(_ProbeVolumeShB);
			SAMPLER(sampler_ProbeVolumeShB);
			TEXTURE3D(_ProbeVolumeOcc);
			SAMPLER(sampler_ProbeVolumeOcc);
			TEXTURE2D(_FlowTex);
			TEXTURE2D(_FluidVelocity);
			SAMPLER(sampler_FluidVelocity);
			SAMPLER(sampler_Linear_Repeat);
			TEXTURE2D(_FluidColor);
			SAMPLER(sampler_FluidColor);
			TEXTURE2D(_FoamTex);
			TEXTURE2D(_NormalMap);
			TEXTURE2D(_ColorCache);
			SAMPLER(sampler_ColorCache);
			TEXTURE2D(_DepthColorLUT);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE2D(_TexLeft);
			SAMPLER(sampler_Linear_Mirror);
			TEXTURE2D(_TexRight);
			float4x4 _CausticMatrix;
			float4 _CausticsSettings;


			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
			//#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRGBufferPass.hlsl"

			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			int IsStereoEyeLeft11_g3305( float3 WorldCamPos, float3 WorldCamRight, float force )
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
			
			float3 MatrixMulThatWorks( float4 Pos, float4x4 Mat )
			{
				float3 result = mul(Mat,Pos.xyz);
				return result + float3(Mat[0][3],Mat[1][3],Mat[2][3]);
			}
			
			float CameraDepthTexture55_g3390( float2 uv )
			{
				float4 color = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, uv);
				return color.r;
			}
			
			float MyCustomExpression44_g3390( float rawDepth )
			{
				return LinearEyeDepth(rawDepth, _ZBufferParams);
			}
			
			float3 MyCustomExpression16_g3390( float3 view, float sceneZ )
			{
				return  _WorldSpaceCameraPos - view * sceneZ / dot(UNITY_MATRIX_I_V._13_23_33, view);
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
			
			inline float3 SampleSHPixel1073( float3 SH, float3 normalWS )
			{
				return SampleSHPixel( SH, normalWS );
			}
			
			inline float3 SampleLightmap1072( float2 lightmapUV, float3 normalWS )
			{
				return SampleLightmap( lightmapUV, 0, normalWS );
			}
			
			float4x4 RotationOnly83_g4120( float4x4 Input )
			{
				return float4x4(Input[0].xyzw,float4(1,1,1,1),Input[2].xyzw, float4(1,1,1,1));
			}
			
			float3 MyCustomExpression6_g4120( float3 vertexPos, float4x4 ProbeWorldToTexture, float3 ProbeVolumeMin, float3 ProbeVolumeSizeInv )
			{
				float3 position = mul(ProbeWorldToTexture, float4(TransformObjectToWorld(vertexPos.xyz), 1.0f)).xyz;
				float3 texCoord = (position - ProbeVolumeMin.xyz) * ProbeVolumeSizeInv;
				return texCoord;
			}
			
			float3 SHEvalLinearL0L114_g4120( float3 worldNormal, float4 ProbeVolumeShR, float4 ProbeVolumeShG, float4 ProbeVolumeShB )
			{
				// See: Library\PackageCache\com.unity.render-pipelines.core12.1.15\ShaderLibrary\EntityLighting.hlsl
				return SHEvalLinearL0L1(worldNormal, ProbeVolumeShR, ProbeVolumeShG,  ProbeVolumeShB);
			}
			
			float4 SampleLightmapHD11_g4125( float2 UV )
			{
				return SAMPLE_TEXTURE2D( unity_Lightmap, samplerunity_Lightmap, UV );
			}
			
			float4 URPDecodeInstruction19_g4125(  )
			{
				return float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0, 0);
			}
			
			float4x4 RotationOnly83_g4124( float4x4 Input )
			{
				return float4x4(Input[0].xyzw,float4(1,1,1,1),Input[2].xyzw, float4(1,1,1,1));
			}
			
			float3 MyCustomExpression6_g4124( float3 vertexPos, float4x4 ProbeWorldToTexture, float3 ProbeVolumeMin, float3 ProbeVolumeSizeInv )
			{
				float3 position = mul(ProbeWorldToTexture, float4(TransformObjectToWorld(vertexPos.xyz), 1.0f)).xyz;
				float3 texCoord = (position - ProbeVolumeMin.xyz) * ProbeVolumeSizeInv;
				return texCoord;
			}
			
			float SHADERGRAPH_SAMPLE_SCENE_DEPTH_LOD(float2 uv)
			{
				#if defined(REQUIRE_DEPTH_TEXTURE)
				#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
				 	float rawDepth = SAMPLE_TEXTURE2D_ARRAY_LOD(_CameraDepthTexture, sampler_CameraDepthTexture, uv, unity_StereoEyeIndex, 0).r;
				#else
				 	float rawDepth = SAMPLE_DEPTH_TEXTURE_LOD(_CameraDepthTexture, sampler_CameraDepthTexture, uv, 0);
				#endif
				return rawDepth;
				#endif // REQUIRE_DEPTH_TEXTURE
				return 0;
			}
			
			float3 SHEvalLinearL0L114_g4124( float3 worldNormal, float4 ProbeVolumeShR, float4 ProbeVolumeShG, float4 ProbeVolumeShB )
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

				float3 VertexOffset1098 = ( v.ase_normal * GlobalOceanOffset );
				
				float3 _Vector3 = float3(0,0,-1);
				float4 Pos6_g3442 = float4( _Vector3 , 0.0 );
				float4x4 Mat6_g3442 = _CausticMatrix;
				float3 localMatrixMulThatWorks6_g3442 = MatrixMulThatWorks( Pos6_g3442 , Mat6_g3442 );
				float3 normalizeResult147_g3440 = normalize( localMatrixMulThatWorks6_g3442 );
				float3 vertexToFrag144_g3440 = normalizeResult147_g3440;
				o.ase_texcoord9.xyz = vertexToFrag144_g3440;
				float3 objToWorld3_g3390 = mul( GetObjectToWorldMatrix(), float4( v.vertex.xyz, 1 ) ).xyz;
				float3 vertexToFrag7_g3390 = objToWorld3_g3390;
				o.ase_texcoord10.xyz = vertexToFrag7_g3390;
				float4 Pos6_g3446 = float4( _Vector3 , 0.0 );
				float4x4 Mat6_g3446 = _CausticMatrix;
				float3 localMatrixMulThatWorks6_g3446 = MatrixMulThatWorks( Pos6_g3446 , Mat6_g3446 );
				float3 normalizeResult147_g3444 = normalize( localMatrixMulThatWorks6_g3446 );
				float3 vertexToFrag144_g3444 = normalizeResult147_g3444;
				o.ase_texcoord11.xyz = vertexToFrag144_g3444;
				
				float3 vertexPos6_g4120 = v.vertex.xyz;
				float4x4 temp_output_28_0_g4120 = _ProbeWorldToTexture;
				float4x4 ProbeWorldToTexture6_g4120 = temp_output_28_0_g4120;
				float3 ProbeVolumeMin6_g4120 = _ProbeVolumeMin;
				float3 ProbeVolumeSizeInv6_g4120 = _ProbeVolumeSizeInv;
				float3 localMyCustomExpression6_g4120 = MyCustomExpression6_g4120( vertexPos6_g4120 , ProbeWorldToTexture6_g4120 , ProbeVolumeMin6_g4120 , ProbeVolumeSizeInv6_g4120 );
				float3 vertexToFrag7_g4120 = localMyCustomExpression6_g4120;
				o.ase_texcoord13.xyz = vertexToFrag7_g4120;
				float2 texCoord2_g4125 = v.texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 vertexToFrag10_g4125 = ( ( texCoord2_g4125 * (unity_LightmapST).xy ) + (unity_LightmapST).zw );
				o.ase_texcoord14.xy = vertexToFrag10_g4125;
				float4x4 temp_output_28_0_g4124 = _ProbeWorldToTexture;
				float4x4 Input83_g4124 = temp_output_28_0_g4124;
				float4x4 localRotationOnly83_g4124 = RotationOnly83_g4124( Input83_g4124 );
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				float3 temp_output_76_0_g4124 = mul( localRotationOnly83_g4124, float4( ase_worldNormal , 0.0 ) ).xyz;
				float3 worldNormal14_g4124 = temp_output_76_0_g4124;
				float3 vertexPos6_g4124 = v.vertex.xyz;
				float4x4 ProbeWorldToTexture6_g4124 = temp_output_28_0_g4124;
				float3 ProbeVolumeMin6_g4124 = _ProbeVolumeMin;
				float3 ProbeVolumeSizeInv6_g4124 = _ProbeVolumeSizeInv;
				float3 localMyCustomExpression6_g4124 = MyCustomExpression6_g4124( vertexPos6_g4124 , ProbeWorldToTexture6_g4124 , ProbeVolumeMin6_g4124 , ProbeVolumeSizeInv6_g4124 );
				float3 vertexToFrag7_g4124 = localMyCustomExpression6_g4124;
				float2 texCoord126 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 UV260 = texCoord126;
				float2 temp_output_185_0 = (UV260*_NormalSize + 0.0);
				float2 temp_output_2_0_g3438 = temp_output_185_0;
				float2 temp_output_4_0_g3438 = (temp_output_2_0_g3438).xy;
				float2 texCoord54 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode53 = SAMPLE_TEXTURE2D_LOD( _FluidVelocity, sampler_FluidVelocity, texCoord54, 0.0 );
				float2 appendResult55 = (float2(tex2DNode53.r , tex2DNode53.g));
				float2 temp_output_102_0 = (float2( 0,0 ) + (appendResult55 - float2( -1,-1 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( -1,-1 )));
				float2 VelNormalised242 = temp_output_102_0;
				float2 temp_output_18_0_g3438 = VelNormalised242;
				float temp_output_183_0 = ( _NormFlowStrength * ( _NormalSize / 8.0 ) );
				float2 temp_cast_4 = (temp_output_183_0).xx;
				float2 temp_output_17_0_g3438 = temp_cast_4;
				float mulTime22_g3438 = _TimeParameters.x * _FlowSpeedN;
				float temp_output_27_0_g3438 = frac( mulTime22_g3438 );
				float2 temp_output_11_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_27_0_g3438 ) );
				float2 temp_output_83_0 = (float2( -1,-1 ) + (temp_output_102_0 - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
				float VelLength236 = length( temp_output_83_0 );
				float temp_output_188_0 = ( VelLength236 * _NormalStrength );
				float temp_output_55_0_g3438 = temp_output_188_0;
				float3 unpack48_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( _NormalMap, sampler_Linear_Repeat, temp_output_11_0_g3438, 0.0 ), temp_output_55_0_g3438 );
				unpack48_g3438.z = lerp( 1, unpack48_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_29_0_g3438 = frac( ( mulTime22_g3438 + 0.5 ) );
				float2 temp_output_12_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_29_0_g3438 ) );
				float3 unpack49_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( _NormalMap, sampler_Linear_Repeat, temp_output_12_0_g3438, 0.0 ), temp_output_55_0_g3438 );
				unpack49_g3438.z = lerp( 1, unpack49_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_32_0_g3438 = ( abs( ( temp_output_27_0_g3438 - 0.5 ) ) / 0.5 );
				float3 lerpResult9_g3438 = lerp( unpack48_g3438 , unpack49_g3438 , temp_output_32_0_g3438);
				float3 NormalResult220 = lerpResult9_g3438;
				float2 appendResult210 = (float2(NormalResult220.xy));
				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 appendResult639 = (float2(ase_screenPosNorm.xy));
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				float ase_vertexTangentSign = v.ase_tangent.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_tanViewDir =  tanToWorld0 * ase_worldViewDir.x + tanToWorld1 * ase_worldViewDir.y  + tanToWorld2 * ase_worldViewDir.z;
				ase_tanViewDir = normalize(ase_tanViewDir);
				float ase_faceVertex = (dot(ase_tanViewDir,float3(0,0,1)));
				float switchResult329 = (((ase_faceVertex>0)?(_RefractionIndex):(_RefractionIndexUnder)));
				float switchResult530 = (((ase_faceVertex>0)?(1.0):(saturate( (0.0 + (distance( _WorldSpaceCameraPos , ase_worldPos ) - 0.1) * (1.0 - 0.0) / (0.6 - 0.1)) ))));
				float CamNear492 = switchResult530;
				float2 temp_output_211_0 = ( switchResult329 * appendResult210 * CamNear492 );
				float2 temp_output_209_0 = ( appendResult639 + temp_output_211_0 );
				float eyeDepth637 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH_LOD( float4( temp_output_209_0, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float2 lerpResult640 = lerp( appendResult639 , temp_output_209_0 , step( ( screenPos.w - eyeDepth637 ) , 0.0 ));
				float2 ScreenSpaceRefractUV691 = lerpResult640;
				float eyeDepth6_g3347 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH_LOD( float4( ScreenSpaceRefractUV691, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3347 = mul( UNITY_MATRIX_V, float4( ase_worldPos, 1 ) ).xyz;
				float temp_output_12_0_g3347 = _DepthFade;
				float temp_output_3_0_g3347 = ( abs( ( eyeDepth6_g3347 - -worldToView21_g3347.z ) ) / temp_output_12_0_g3347 );
				#ifdef _USEDEPTHFADE_ON
				float staticSwitch315 = saturate( temp_output_3_0_g3347 );
				#else
				float staticSwitch315 = 1.0;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch776 = staticSwitch315;
				#else
				float staticSwitch776 = 1.0;
				#endif
				float ShoreMaskDepth301 = staticSwitch776;
				float2 temp_output_363_0 = ( ( _RefractionFake * 0.1 ) * appendResult210 * ShoreMaskDepth301 );
				float2 RefractedUV1084 = temp_output_363_0;
				float4 ProbeVolumeShR14_g4124 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShR, sampler_Linear_Clamp, ( vertexToFrag7_g4124 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float4 ProbeVolumeShG14_g4124 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShG, sampler_Linear_Clamp, ( vertexToFrag7_g4124 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float4 ProbeVolumeShB14_g4124 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShB, sampler_Linear_Clamp, ( vertexToFrag7_g4124 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float3 localSHEvalLinearL0L114_g4124 = SHEvalLinearL0L114_g4124( worldNormal14_g4124 , ProbeVolumeShR14_g4124 , ProbeVolumeShG14_g4124 , ProbeVolumeShB14_g4124 );
				float3 vertexToFrag31_g4124 = localSHEvalLinearL0L114_g4124;
				o.ase_texcoord15.xyz = vertexToFrag31_g4124;
				
				o.ase_texcoord8.xyz = v.texcoord.xyz;
				o.ase_texcoord12 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.w = 0;
				o.ase_texcoord9.w = 0;
				o.ase_texcoord10.w = 0;
				o.ase_texcoord11.w = 0;
				o.ase_texcoord13.w = 0;
				o.ase_texcoord14.zw = 0;
				o.ase_texcoord15.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOffset1098;

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

				float switchResult530 = (((ase_vface>0)?(1.0):(saturate( (0.0 + (distance( _WorldSpaceCameraPos , WorldPosition ) - 0.1) * (1.0 - 0.0) / (0.6 - 0.1)) ))));
				float CamNear492 = switchResult530;
				float3 appendResult454 = (float3(_FoamColor2.rgb));
				float2 texCoord126 = IN.ase_texcoord8.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float2 UV260 = texCoord126;
				float2 temp_output_141_0 = (UV260*_Size + 0.0);
				float2 temp_output_2_0_g3437 = temp_output_141_0;
				float2 temp_output_4_0_g3437 = (temp_output_2_0_g3437).xy;
				float2 texCoord54 = IN.ase_texcoord8.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float4 tex2DNode53 = SAMPLE_TEXTURE2D( _FluidVelocity, sampler_FluidVelocity, texCoord54 );
				float2 appendResult55 = (float2(tex2DNode53.r , tex2DNode53.g));
				float2 temp_output_102_0 = (float2( 0,0 ) + (appendResult55 - float2( -1,-1 )) * (float2( 1,1 ) - float2( 0,0 )) / (float2( 1,1 ) - float2( -1,-1 )));
				float2 VelNormalised242 = temp_output_102_0;
				float2 temp_output_18_0_g3437 = VelNormalised242;
				float temp_output_144_0 = ( _FlowStrength * ( _Size / 8.0 ) );
				float2 temp_cast_1 = (temp_output_144_0).xx;
				float2 temp_output_17_0_g3437 = temp_cast_1;
				float mulTime22_g3437 = _TimeParameters.x * _FlowSpeed;
				float temp_output_27_0_g3437 = frac( mulTime22_g3437 );
				float2 temp_output_11_0_g3437 = ( temp_output_4_0_g3437 + ( -(temp_output_18_0_g3437*2.0 + -1.0) * temp_output_17_0_g3437 * temp_output_27_0_g3437 ) );
				float temp_output_29_0_g3437 = frac( ( mulTime22_g3437 + 0.5 ) );
				float2 temp_output_12_0_g3437 = ( temp_output_4_0_g3437 + ( -(temp_output_18_0_g3437*2.0 + -1.0) * temp_output_17_0_g3437 * temp_output_29_0_g3437 ) );
				float temp_output_32_0_g3437 = ( abs( ( temp_output_27_0_g3437 - 0.5 ) ) / 0.5 );
				float4 lerpResult9_g3437 = lerp( SAMPLE_TEXTURE2D( _FlowTex, sampler_Linear_Repeat, temp_output_11_0_g3437 ) , SAMPLE_TEXTURE2D( _FlowTex, sampler_Linear_Repeat, temp_output_12_0_g3437 ) , temp_output_32_0_g3437);
				float FlowWaterTex246 = saturate( pow( lerpResult9_g3437.r , _FoamPowA ) );
				float2 uv_FluidColor103 = IN.ase_texcoord8.xyz.xy;
				float4 tex2DNode103 = SAMPLE_TEXTURE2D( _FluidColor, sampler_FluidColor, uv_FluidColor103 );
				float FluidColorA435 = tex2DNode103.a;
				float temp_output_105_0 = ( saturate( FlowWaterTex246 ) * FluidColorA435 );
				float lerpResult4_g3346 = lerp( 0.5 , saturate( pow( temp_output_105_0 , _Foam2Pow ) ) , _Foam2Con);
				float clampResult11_g3346 = clamp( ( lerpResult4_g3346 + 0.0 ) , 0.0 , 1.0 );
				float3 FlowWaterTexRes445 = ( appendResult454 * clampResult11_g3346 );
				#ifdef _USEFLUIDFORFOAM_ON
				float3 staticSwitch107 = FlowWaterTexRes445;
				#else
				float3 staticSwitch107 = float3( 0,0,0 );
				#endif
				float2 temp_output_2_0_g3439 = (UV260*_FoamSize + 0.0);
				float2 temp_output_4_0_g3439 = (temp_output_2_0_g3439).xy;
				float2 temp_output_18_0_g3439 = VelNormalised242;
				float temp_output_148_0 = ( _FoamFlowStrength * ( _FoamSize / 8.0 ) );
				float2 temp_cast_2 = (temp_output_148_0).xx;
				float2 temp_output_17_0_g3439 = temp_cast_2;
				float mulTime22_g3439 = _TimeParameters.x * _FoamFlowSpeed;
				float temp_output_27_0_g3439 = frac( mulTime22_g3439 );
				float2 temp_output_11_0_g3439 = ( temp_output_4_0_g3439 + ( -(temp_output_18_0_g3439*2.0 + -1.0) * temp_output_17_0_g3439 * temp_output_27_0_g3439 ) );
				float temp_output_29_0_g3439 = frac( ( mulTime22_g3439 + 0.5 ) );
				float2 temp_output_12_0_g3439 = ( temp_output_4_0_g3439 + ( -(temp_output_18_0_g3439*2.0 + -1.0) * temp_output_17_0_g3439 * temp_output_29_0_g3439 ) );
				float temp_output_32_0_g3439 = ( abs( ( temp_output_27_0_g3439 - 0.5 ) ) / 0.5 );
				float4 lerpResult9_g3439 = lerp( SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_11_0_g3439 ) , SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_12_0_g3439 ) , temp_output_32_0_g3439);
				float FoamA265 = lerpResult9_g3439.r;
				float temp_output_125_0 = ( 1.0 - saturate( 0.0 ) );
				float FoamMask114 = ( temp_output_125_0 * _Foam_Mix );
				float2 temp_output_2_0_g3436 = (UV260*_FoamSize2 + 0.0);
				float2 temp_output_4_0_g3436 = (temp_output_2_0_g3436).xy;
				float2 temp_output_18_0_g3436 = VelNormalised242;
				float2 temp_cast_3 = (temp_output_148_0).xx;
				float2 temp_output_17_0_g3436 = temp_cast_3;
				float mulTime22_g3436 = _TimeParameters.x * _FoamFlowSpeed;
				float temp_output_27_0_g3436 = frac( mulTime22_g3436 );
				float2 temp_output_11_0_g3436 = ( temp_output_4_0_g3436 + ( -(temp_output_18_0_g3436*2.0 + -1.0) * temp_output_17_0_g3436 * temp_output_27_0_g3436 ) );
				float temp_output_29_0_g3436 = frac( ( mulTime22_g3436 + 0.5 ) );
				float2 temp_output_12_0_g3436 = ( temp_output_4_0_g3436 + ( -(temp_output_18_0_g3436*2.0 + -1.0) * temp_output_17_0_g3436 * temp_output_29_0_g3436 ) );
				float temp_output_32_0_g3436 = ( abs( ( temp_output_27_0_g3436 - 0.5 ) ) / 0.5 );
				float4 lerpResult9_g3436 = lerp( SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_11_0_g3436 ) , SAMPLE_TEXTURE2D( _FoamTex, sampler_Linear_Repeat, temp_output_12_0_g3436 ) , temp_output_32_0_g3436);
				float FoamB267 = lerpResult9_g3436.r;
				float temp_output_155_0 = saturate( pow( saturate( ( ( FoamA265 * FoamMask114 ) + ( FoamB267 * FoamMask114 ) ) ) , _FoamPowM ) );
				float3 appendResult337 = (float3(_FoamColor.rgb));
				float3 FoamResult271 = ( temp_output_155_0 * appendResult337 );
				float3 temp_output_118_0 = ( staticSwitch107 + FoamResult271 );
				float4 ase_screenPosNorm = ScreenPos / ScreenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 appendResult639 = (float2(ase_screenPosNorm.xy));
				float switchResult329 = (((ase_vface>0)?(_RefractionIndex):(_RefractionIndexUnder)));
				float2 temp_output_185_0 = (UV260*_NormalSize + 0.0);
				float2 temp_output_2_0_g3438 = temp_output_185_0;
				float2 temp_output_4_0_g3438 = (temp_output_2_0_g3438).xy;
				float2 temp_output_18_0_g3438 = VelNormalised242;
				float temp_output_183_0 = ( _NormFlowStrength * ( _NormalSize / 8.0 ) );
				float2 temp_cast_6 = (temp_output_183_0).xx;
				float2 temp_output_17_0_g3438 = temp_cast_6;
				float mulTime22_g3438 = _TimeParameters.x * _FlowSpeedN;
				float temp_output_27_0_g3438 = frac( mulTime22_g3438 );
				float2 temp_output_11_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_27_0_g3438 ) );
				float2 temp_output_83_0 = (float2( -1,-1 ) + (temp_output_102_0 - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
				float VelLength236 = length( temp_output_83_0 );
				float temp_output_188_0 = ( VelLength236 * _NormalStrength );
				float temp_output_55_0_g3438 = temp_output_188_0;
				float3 unpack48_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_Linear_Repeat, temp_output_11_0_g3438 ), temp_output_55_0_g3438 );
				unpack48_g3438.z = lerp( 1, unpack48_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_29_0_g3438 = frac( ( mulTime22_g3438 + 0.5 ) );
				float2 temp_output_12_0_g3438 = ( temp_output_4_0_g3438 + ( -(temp_output_18_0_g3438*2.0 + -1.0) * temp_output_17_0_g3438 * temp_output_29_0_g3438 ) );
				float3 unpack49_g3438 = UnpackNormalScale( SAMPLE_TEXTURE2D( _NormalMap, sampler_Linear_Repeat, temp_output_12_0_g3438 ), temp_output_55_0_g3438 );
				unpack49_g3438.z = lerp( 1, unpack49_g3438.z, saturate(temp_output_55_0_g3438) );
				float temp_output_32_0_g3438 = ( abs( ( temp_output_27_0_g3438 - 0.5 ) ) / 0.5 );
				float3 lerpResult9_g3438 = lerp( unpack48_g3438 , unpack49_g3438 , temp_output_32_0_g3438);
				float3 NormalResult220 = lerpResult9_g3438;
				float2 appendResult210 = (float2(NormalResult220.xy));
				float2 temp_output_211_0 = ( switchResult329 * appendResult210 * CamNear492 );
				float2 temp_output_209_0 = ( appendResult639 + temp_output_211_0 );
				float eyeDepth637 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( temp_output_209_0, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float2 lerpResult640 = lerp( appendResult639 , temp_output_209_0 , step( ( ScreenPos.w - eyeDepth637 ) , 0.0 ));
				float2 ScreenSpaceRefractUV691 = lerpResult640;
				float eyeDepth6_g3347 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV691, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3347 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float temp_output_12_0_g3347 = _DepthFade;
				float temp_output_3_0_g3347 = ( abs( ( eyeDepth6_g3347 - -worldToView21_g3347.z ) ) / temp_output_12_0_g3347 );
				#ifdef _USEDEPTHFADE_ON
				float staticSwitch315 = saturate( temp_output_3_0_g3347 );
				#else
				float staticSwitch315 = 1.0;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch776 = staticSwitch315;
				#else
				float staticSwitch776 = 1.0;
				#endif
				float ShoreMaskDepth301 = staticSwitch776;
				float3 temp_output_312_0 = ( temp_output_118_0 * ShoreMaskDepth301 );
				float3 appendResult336 = (float3(_Color.rgb));
				float3 AlbedoResult225 = ( temp_output_312_0 + ( appendResult336 * _Color.a * saturate( (0.0 + (ShoreMaskDepth301 - 0.0) * (1.0 - 0.0) / (0.09 - 0.0)) ) ) );
				float3 AlbedoIn285_g3823 = AlbedoResult225;
				#ifdef _USEUNDERWATER
				float3 staticSwitch214_g3823 = AlbedoIn285_g3823;
				#else
				float3 staticSwitch214_g3823 = AlbedoIn285_g3823;
				#endif
				float3 AlbedoRes2511 = ( CamNear492 * staticSwitch214_g3823 );
				
				float3 temp_cast_11 = (1.0).xxx;
				float2 texCoord355 = IN.ase_texcoord8.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_output_363_0 = ( ( _RefractionFake * 0.1 ) * appendResult210 * ShoreMaskDepth301 );
				float4 tex2DNode350 = SAMPLE_TEXTURE2D( _ColorCache, sampler_ColorCache, ( texCoord355 + temp_output_363_0 ) );
				float ColorCacheADistorted787 = tex2DNode350.a;
				float temp_output_719_0 = saturate( ( ( ColorCacheADistorted787 * 0.1 ) / _RemapWaterDistance1 ) );
				float eyeDepth6_g3301 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV691, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 worldToView21_g3301 = mul( UNITY_MATRIX_V, float4( WorldPosition, 1 ) ).xyz;
				float fresnelNdotV747 = dot( WorldNormal, WorldViewDirection );
				float fresnelNode747 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV747, 5.0 ) );
				float lerpResult745 = lerp( 9.0 , 30.0 , saturate( fresnelNode747 ));
				float DepthFadeDistthing744 = lerpResult745;
				float temp_output_12_0_g3301 = ( DepthFadeDistthing744 * _RemapWaterDistance );
				float temp_output_3_0_g3301 = ( abs( ( eyeDepth6_g3301 - -worldToView21_g3301.z ) ) / temp_output_12_0_g3301 );
				float temp_output_716_0 = saturate( temp_output_3_0_g3301 );
				#ifdef _REFRACTION_ON
				float staticSwitch738 = temp_output_716_0;
				#else
				float staticSwitch738 = temp_output_719_0;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch714 = _WaterDepthPow;
				#else
				float staticSwitch714 = _WaterDepthPow1;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch725 = ( 1.0 - pow( ( 1.0 - temp_output_716_0 ) , 6.0 ) );
				#else
				float staticSwitch725 = ( 1.0 - pow( ( 1.0 - temp_output_719_0 ) , 6.0 ) );
				#endif
				float2 appendResult711 = (float2(staticSwitch725 , 0.0));
				float3 appendResult705 = (float3(SAMPLE_TEXTURE2D_LOD( _DepthColorLUT, sampler_Linear_Clamp, appendResult711, 0.0 ).rgb));
				float temp_output_2_0_g3304 = saturate( ShoreMaskDepth301 );
				float temp_output_3_0_g3304 = ( 1.0 - temp_output_2_0_g3304 );
				float3 appendResult7_g3304 = (float3(temp_output_3_0_g3304 , temp_output_3_0_g3304 , temp_output_3_0_g3304));
				float3 desaturateInitialColor739 = ( ( ( saturate( pow( ( 1.0 - staticSwitch738 ) , staticSwitch714 ) ) * appendResult705 ) * temp_output_2_0_g3304 ) + appendResult7_g3304 );
				float desaturateDot739 = dot( desaturateInitialColor739, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar739 = lerp( desaturateInitialColor739, desaturateDot739.xxx, ( 1.0 - _WaterSaturation ) );
				#ifdef _USEDEPTHCOLORINGLUT_ON
				float3 staticSwitch785 = desaturateVar739;
				#else
				float3 staticSwitch785 = temp_cast_11;
				#endif
				float3 switchResult742 = (((ase_vface>0)?(staticSwitch785):(float3( 0,0,0 ))));
				float3 WaterDepthTint743 = switchResult742;
				#ifdef _REFRACTION_ON
				float2 staticSwitch917 = ( temp_output_211_0 * ShoreMaskDepth301 );
				#else
				float2 staticSwitch917 = ( appendResult210 * _RefractionIndexUnderMobile );
				#endif
				float2 RefractionBacksideWarp650 = staticSwitch917;
				float3 temp_output_647_0 = SHADERGRAPH_REFLECTION_PROBE(WorldViewDirection,float3( ( RefractionBacksideWarp650 * 4.0 ) ,  0.0 ),1.0);
				float3 temp_output_564_0 = ( (temp_output_647_0).xyz * _CubemapStrength );
				float3 switchResult592 = (((ase_vface>0)?(float3( 0,0,0 )):(temp_output_564_0)));
				float3 WorldCamPos11_g3305 = _WorldPos;
				float3 WorldCamRight11_g3305 = _WorldDir;
				float force11_g3305 = _ForceEye;
				int localIsStereoEyeLeft11_g3305 = IsStereoEyeLeft11_g3305( WorldCamPos11_g3305 , WorldCamRight11_g3305 , force11_g3305 );
				float2 appendResult4_g3305 = (float2(ase_screenPosNorm.xy));
				float2 appendResult585 = (float2(NormalResult220.xy));
				float2 temp_output_1_0_g3305 = ( appendResult4_g3305 + ( appendResult585 * CamNear492 * _PlanarNormalStrengthm ) );
				float4 tex2DNode3_g3305 = SAMPLE_TEXTURE2D( _TexLeft, sampler_Linear_Mirror, temp_output_1_0_g3305 );
				float4 tex2DNode2_g3305 = SAMPLE_TEXTURE2D( _TexRight, sampler_Linear_Mirror, temp_output_1_0_g3305 );
				float4 ifLocalVar10_g3305 = 0;
				if( localIsStereoEyeLeft11_g3305 <= 0.0 )
				ifLocalVar10_g3305 = tex2DNode2_g3305;
				else
				ifLocalVar10_g3305 = tex2DNode3_g3305;
				float3 tanToWorld0 = float3( WorldTangent.x, WorldBiTangent.x, WorldNormal.x );
				float3 tanToWorld1 = float3( WorldTangent.y, WorldBiTangent.y, WorldNormal.y );
				float3 tanToWorld2 = float3( WorldTangent.z, WorldBiTangent.z, WorldNormal.z );
				float3 tanNormal581 = NormalResult220;
				float3 worldNormal581 = float3(dot(tanToWorld0,tanNormal581), dot(tanToWorld1,tanNormal581), dot(tanToWorld2,tanNormal581));
				float fresnelNdotV579 = dot( worldNormal581, WorldViewDirection );
				float fresnelNode579 = ( _RefFresnel.x + _RefFresnel.y * pow( 1.0 - fresnelNdotV579, _RefFresnel.z ) );
				float3 switchResult588 = (((ase_vface>0)?(( (ifLocalVar10_g3305).rgb * saturate( fresnelNode579 ) )):(temp_output_564_0)));
				#ifdef _PLANARREFLECTION_ON
				float3 staticSwitch574 = switchResult588;
				#else
				float3 staticSwitch574 = switchResult592;
				#endif
				float3 ReflectionRes554 = staticSwitch574;
				float switchResult418 = (((ase_vface>0)?(_ColorMix):(_ColorMixBack)));
				float3 appendResult352 = (float3(tex2DNode350.rgb));
				#ifdef _REFRACTION_ON
				float4 staticSwitch894 = _RefractionColor;
				#else
				float4 staticSwitch894 = _RefractionColorMobile;
				#endif
				#ifdef _REFRACTION_ON
				float staticSwitch889 = 1.0;
				#else
				float staticSwitch889 = saturate( (0.0 + (ColorCacheADistorted787 - 0.02) * (1.0 - 0.0) / (0.15 - 0.02)) );
				#endif
				float ShoreMaskThing883 = staticSwitch889;
				float4 lerpResult885 = lerp( float4( 1,1,1,0 ) , staticSwitch894 , ShoreMaskThing883);
				float3 appendResult331 = (float3(lerpResult885.rgb));
				float3 temp_output_354_0 = ( switchResult418 * appendResult352 * appendResult331 );
				float4 fetchOpaqueVal203 = float4( SHADERGRAPH_SAMPLE_SCENE_COLOR( ScreenSpaceRefractUV691 ), 1.0 );
				float3 appendResult330 = (float3(fetchOpaqueVal203.rgb));
				float3 lerpResult317 = lerp( appendResult330 , ( appendResult330 * appendResult331 ) , ShoreMaskDepth301);
				#ifdef _REFRACTION_ON
				float3 staticSwitch277 = lerpResult317;
				#else
				float3 staticSwitch277 = temp_output_354_0;
				#endif
				float3 temp_output_771_0 = ( ReflectionRes554 + staticSwitch277 );
				float3 lerpResult529 = lerp( appendResult330 , staticSwitch277 , CamNear492);
				float3 temp_output_18_0_g3440 = float3( 0,0,0 );
				float3 temp_output_30_0_g3440 = float3( 0,0,1 );
				float3 tanNormal12_g3440 = temp_output_30_0_g3440;
				float3 worldNormal12_g3440 = float3(dot(tanToWorld0,tanNormal12_g3440), dot(tanToWorld1,tanNormal12_g3440), dot(tanToWorld2,tanNormal12_g3440));
				float3 vertexToFrag144_g3440 = IN.ase_texcoord9.xyz;
				float dotResult1_g3440 = dot( worldNormal12_g3440 , vertexToFrag144_g3440 );
				float3 vertexToFrag7_g3390 = IN.ase_texcoord10.xyz;
				float3 positionWS_fogFactor24_g3390 = vertexToFrag7_g3390;
				float3 normalizeResult21_g3390 = normalize( ( _WorldSpaceCameraPos - positionWS_fogFactor24_g3390 ) );
				float3 view23_g3390 = normalizeResult21_g3390;
				float3 view16_g3390 = view23_g3390;
				float3 appendResult49_g3390 = (float3(ScreenSpaceRefractUV691 , ase_screenPosNorm.w));
				float3 screenPosXYW29_g3390 = (appendResult49_g3390).xyz;
				float2 uvDepth38_g3390 = ( (screenPosXYW29_g3390).xy / (screenPosXYW29_g3390).z );
				float2 uv55_g3390 = uvDepth38_g3390;
				float localCameraDepthTexture55_g3390 = CameraDepthTexture55_g3390( uv55_g3390 );
				float rawDepth40_g3390 = localCameraDepthTexture55_g3390;
				float rawDepth44_g3390 = rawDepth40_g3390;
				float localMyCustomExpression44_g3390 = MyCustomExpression44_g3390( rawDepth44_g3390 );
				float sceneZ41_g3390 = localMyCustomExpression44_g3390;
				float sceneZ16_g3390 = sceneZ41_g3390;
				float3 localMyCustomExpression16_g3390 = MyCustomExpression16_g3390( view16_g3390 , sceneZ16_g3390 );
				float3 scenePos11_g3390 = localMyCustomExpression16_g3390;
				float3 WorldPosition678 = scenePos11_g3390;
				float2 temp_output_2_0_g3452 = (WorldPosition678).xz;
				float2 temp_output_4_0_g3452 = (temp_output_2_0_g3452).xy;
				float2 VelNeg231 = temp_output_83_0;
				float2 break969 = VelNeg231;
				float3 appendResult992 = (float3(break969.x , 0.0 , break969.y));
				float3 rotatedValue998 = RotateAroundAxis( float3( 0,0,0 ), float3( (mul( GetObjectToWorldMatrix(), float4( appendResult992 , 0.0 ) ).xyz).xz ,  0.0 ), float3( 0,0,1 ), radians( _CausticRotate ) );
				float2 temp_output_18_0_g3452 = (rotatedValue998).xy;
				float2 temp_cast_23 = (-_CausticFlowStrength).xx;
				float2 temp_output_17_0_g3452 = temp_cast_23;
				float mulTime22_g3452 = _TimeParameters.x * _CausticFlowSpeed;
				float temp_output_27_0_g3452 = frac( mulTime22_g3452 );
				float2 break924 = ( temp_output_4_0_g3452 + ( -temp_output_18_0_g3452 * temp_output_17_0_g3452 * temp_output_27_0_g3452 ) );
				float temp_output_979_0 = (WorldPosition678).y;
				float3 appendResult923 = (float3(break924.x , temp_output_979_0 , break924.y));
				float3 temp_output_105_0_g3440 = appendResult923;
				float2 PanSpeed131_g3440 = (_CausticsSettings).xy;
				float mulTime28_g3440 = _TimeParameters.x * -1.0;
				float2 break101_g3440 = ( PanSpeed131_g3440 * mulTime28_g3440 );
				float3 appendResult100_g3440 = (float3(break101_g3440.x , 0.0 , break101_g3440.y));
				float4 Pos6_g3443 = float4( ( temp_output_105_0_g3440 + appendResult100_g3440 ) , 0.0 );
				float4x4 invertVal146_g3440 = Inverse4x4( _CausticMatrix );
				float4x4 Mat6_g3443 = invertVal146_g3440;
				float3 localMatrixMulThatWorks6_g3443 = MatrixMulThatWorks( Pos6_g3443 , Mat6_g3443 );
				float3 appendResult121_g3440 = (float3(SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3443).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3443).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3443).xy ).r));
				float3 appendResult62_g3440 = (float3(_CausticsColor.rgb));
				float temp_output_117_0_g3440 = (temp_output_105_0_g3440).y;
				float GlobalOceanHeight515 = ( GlobalOceanHeight + (0) );
				float temp_output_63_0_g3440 = GlobalOceanHeight515;
				float2 DistanceFade134_g3440 = (_CausticsSettings).zw;
				float2 break136_g3440 = DistanceFade134_g3440;
				float temp_output_67_0_g3440 = ( saturate( (0.0 + (max( -( temp_output_117_0_g3440 - temp_output_63_0_g3440 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) * saturate( (1.0 + (distance( temp_output_63_0_g3440 , temp_output_117_0_g3440 ) - break136_g3440.x) * (0.0 - 1.0) / (break136_g3440.y - break136_g3440.x)) ) );
				float3 temp_output_20_0_g3440 = ( temp_output_18_0_g3440 + ( max( dotResult1_g3440 , 0.0 ) * appendResult121_g3440 * appendResult62_g3440 * temp_output_67_0_g3440 ) );
				float3 temp_output_18_0_g3444 = float3( 0,0,0 );
				float3 temp_output_30_0_g3444 = float3( 0,0,1 );
				float3 tanNormal12_g3444 = temp_output_30_0_g3444;
				float3 worldNormal12_g3444 = float3(dot(tanToWorld0,tanNormal12_g3444), dot(tanToWorld1,tanNormal12_g3444), dot(tanToWorld2,tanNormal12_g3444));
				float3 vertexToFrag144_g3444 = IN.ase_texcoord11.xyz;
				float dotResult1_g3444 = dot( worldNormal12_g3444 , vertexToFrag144_g3444 );
				float temp_output_29_0_g3452 = frac( ( mulTime22_g3452 + 0.5 ) );
				float2 break965 = ( temp_output_4_0_g3452 + ( -temp_output_18_0_g3452 * temp_output_17_0_g3452 * temp_output_29_0_g3452 ) );
				float3 appendResult966 = (float3(break965.x , temp_output_979_0 , break965.y));
				float3 temp_output_105_0_g3444 = appendResult966;
				float2 PanSpeed131_g3444 = (_CausticsSettings).xy;
				float mulTime28_g3444 = _TimeParameters.x * -1.0;
				float2 break101_g3444 = ( PanSpeed131_g3444 * mulTime28_g3444 );
				float3 appendResult100_g3444 = (float3(break101_g3444.x , 0.0 , break101_g3444.y));
				float4 Pos6_g3447 = float4( ( temp_output_105_0_g3444 + appendResult100_g3444 ) , 0.0 );
				float4x4 invertVal146_g3444 = Inverse4x4( _CausticMatrix );
				float4x4 Mat6_g3447 = invertVal146_g3444;
				float3 localMatrixMulThatWorks6_g3447 = MatrixMulThatWorks( Pos6_g3447 , Mat6_g3447 );
				float3 appendResult121_g3444 = (float3(SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3447).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3447).xy ).r , SAMPLE_TEXTURE2D( _Caustics, sampler_Caustics, (localMatrixMulThatWorks6_g3447).xy ).r));
				float3 appendResult62_g3444 = (float3(_CausticsColor.rgb));
				float temp_output_117_0_g3444 = (temp_output_105_0_g3444).y;
				float temp_output_63_0_g3444 = GlobalOceanHeight515;
				float2 DistanceFade134_g3444 = (_CausticsSettings).zw;
				float2 break136_g3444 = DistanceFade134_g3444;
				float temp_output_67_0_g3444 = ( saturate( (0.0 + (max( -( temp_output_117_0_g3444 - temp_output_63_0_g3444 ) , 0.0 ) - 0.2) * (1.0 - 0.0) / (1.0 - 0.2)) ) * saturate( (1.0 + (distance( temp_output_63_0_g3444 , temp_output_117_0_g3444 ) - break136_g3444.x) * (0.0 - 1.0) / (break136_g3444.y - break136_g3444.x)) ) );
				float3 temp_output_20_0_g3444 = ( temp_output_18_0_g3444 + ( max( dotResult1_g3444 , 0.0 ) * appendResult121_g3444 * appendResult62_g3444 * temp_output_67_0_g3444 ) );
				float temp_output_32_0_g3452 = ( abs( ( temp_output_27_0_g3452 - 0.5 ) ) / 0.5 );
				float3 lerpResult958 = lerp( temp_output_20_0_g3440 , temp_output_20_0_g3444 , temp_output_32_0_g3452);
				float3 appendResult859 = (float3(_CausticsTint.rgb));
				float3 temp_output_856_0 = ( lerpResult958 * appendResult859 * 3.0 );
				#ifdef _REFRACTION_ON
				float3 staticSwitch761 = temp_output_856_0;
				#else
				float3 staticSwitch761 = float3( 0,0,0 );
				#endif
				float3 Caustics671 = staticSwitch761;
				float eyeDepth6_g3387 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( ScreenSpaceRefractUV691, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float3 temp_output_14_0_g3387 = IN.ase_texcoord12.xyz;
				float3 temp_output_5_0_g3388 = temp_output_14_0_g3387;
				float3 objToWorld2_g3388 = mul( GetObjectToWorldMatrix(), float4( temp_output_5_0_g3388, 1 ) ).xyz;
				float3 worldToView1_g3388 = mul( UNITY_MATRIX_V, float4( objToWorld2_g3388, 1 ) ).xyz;
				float temp_output_12_0_g3387 = 1.0;
				float temp_output_3_0_g3387 = ( abs( ( eyeDepth6_g3387 - -worldToView1_g3388.z ) ) / temp_output_12_0_g3387 );
				float temp_output_912_0 = saturate( temp_output_3_0_g3387 );
				float luminance1004 = Luminance(appendResult330);
				#ifdef _REFRACTION_ON
				float3 staticSwitch766 = ( WaterDepthTint743 * ( ReflectionRes554 + lerpResult529 + ( Caustics671 * appendResult331 * 30.0 * ShoreMaskDepth301 * appendResult330 * temp_output_912_0 * saturate( (0.1 + (luminance1004 - 0.0) * (1.0 - 0.1) / (1.0 - 0.0)) ) ) ) );
				#else
				float3 staticSwitch766 = ( WaterDepthTint743 * temp_output_771_0 );
				#endif
				#ifdef _REFRACTION_ON
				float3 staticSwitch568 = lerpResult529;
				#else
				float3 staticSwitch568 = temp_output_771_0;
				#endif
				float3 switchResult591 = (((ase_vface>0)?(staticSwitch766):(staticSwitch568)));
				float3 EmissionResult218 = switchResult591;
				
				#ifdef _REFRACTION_ON
				float staticSwitch882 = saturate( (0.0 + (ShoreMaskDepth301 - 0.1) * (1.0 - 0.0) / (1.0 - 0.1)) );
				#else
				float staticSwitch882 = saturate( (0.0 + (ColorCacheADistorted787 - 0.1) * (1.0 - 0.0) / (1.0 - 0.1)) );
				#endif
				float SpecularRes1093 = ( _Specular * staticSwitch882 );
				float3 temp_cast_30 = (SpecularRes1093).xxx;
				
				float switchResult632 = (((ase_vface>0)?(_Smoothness):(0.85)));
				float SmoothnessRes556 = switchResult632;
				
				float3 SH1073 = half4(0,0,0,0).xyz;
				float3 normalWS1073 = WorldNormal;
				float3 localSampleSHPixel1073 = SampleSHPixel1073( SH1073 , normalWS1073 );
				float2 lightmapUV1072 = half4(0,0,0,0).xy;
				float3 normalWS1072 = WorldNormal;
				float3 localSampleLightmap1072 = SampleLightmap1072( lightmapUV1072 , normalWS1072 );
				#ifdef LIGHTMAP_ON
				float3 staticSwitch1071 = localSampleLightmap1072;
				#else
				float3 staticSwitch1071 = localSampleSHPixel1073;
				#endif
				float3 LightmapOrSH1076 = staticSwitch1071;
				float4x4 temp_output_28_0_g4120 = _ProbeWorldToTexture;
				float4x4 Input83_g4120 = temp_output_28_0_g4120;
				float4x4 localRotationOnly83_g4120 = RotationOnly83_g4120( Input83_g4120 );
				float3 tanNormal1143 = NormalResult220;
				float3 worldNormal1143 = float3(dot(tanToWorld0,tanNormal1143), dot(tanToWorld1,tanNormal1143), dot(tanToWorld2,tanNormal1143));
				float3 worldNormal14_g4120 = mul( localRotationOnly83_g4120, float4( worldNormal1143 , 0.0 ) ).xyz;
				float3 vertexToFrag7_g4120 = IN.ase_texcoord13.xyz;
				float2 RefractedUV1084 = temp_output_363_0;
				float4 ProbeVolumeShR14_g4120 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShR, sampler_Linear_Clamp, ( vertexToFrag7_g4120 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float4 ProbeVolumeShG14_g4120 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShG, sampler_Linear_Clamp, ( vertexToFrag7_g4120 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float4 ProbeVolumeShB14_g4120 = SAMPLE_TEXTURE3D_LOD( _ProbeVolumeShB, sampler_Linear_Clamp, ( vertexToFrag7_g4120 + float3( RefractedUV1084 ,  0.0 ) ), 0.0 );
				float3 localSHEvalLinearL0L114_g4120 = SHEvalLinearL0L114_g4120( worldNormal14_g4120 , ProbeVolumeShR14_g4120 , ProbeVolumeShG14_g4120 , ProbeVolumeShB14_g4120 );
				#ifdef _PROBEVOLUME_ON
				float3 staticSwitch20_g4120 = localSHEvalLinearL0L114_g4120;
				#else
				float3 staticSwitch20_g4120 = LightmapOrSH1076;
				#endif
				float2 vertexToFrag10_g4125 = IN.ase_texcoord14.xy;
				float2 UV11_g4125 = vertexToFrag10_g4125;
				float4 localSampleLightmapHD11_g4125 = SampleLightmapHD11_g4125( UV11_g4125 );
				float4 localURPDecodeInstruction19_g4125 = URPDecodeInstruction19_g4125();
				float3 decodeLightMap6_g4125 = DecodeLightmap(localSampleLightmapHD11_g4125,localURPDecodeInstruction19_g4125);
				float3 vertexToFrag31_g4124 = IN.ase_texcoord15.xyz;
				#ifdef _PROBEVOLUME_ON
				float3 staticSwitch20_g4124 = vertexToFrag31_g4124;
				#else
				float3 staticSwitch20_g4124 = decodeLightMap6_g4125;
				#endif
				#ifdef _ProbeVolPerVertex
				float3 staticSwitch1148 = staticSwitch20_g4124;
				#else
				float3 staticSwitch1148 = staticSwitch20_g4120;
				#endif
				float3 BakedGI1077 = staticSwitch1148;
				float luminance293 = Luminance(BakedGI1077);
				float temp_output_1130_0 = saturate( (0.0 + (luminance293 - ( 1.0 - _ReflectionShadowedEnd )) * (1.0 - 0.0) / (( 1.0 - _ReflectionShadowedStart ) - ( 1.0 - _ReflectionShadowedEnd ))) );
				float temp_output_12_0_g3344 = _ReflectionShadowed;
				#ifdef _PLANARREFLECTION_ON
				float staticSwitch594 = 0.0;
				#else
				float staticSwitch594 = ( ( ( temp_output_1130_0 * temp_output_12_0_g3344 ) + ( 1.0 - temp_output_12_0_g3344 ) ) * ShoreMaskDepth301 );
				#endif
				float OcclusionRes773 = ( CamNear492 * staticSwitch594 );
				

				float3 BaseColor = AlbedoRes2511;
				float3 Normal = NormalResult220;
				float3 Emission = EmissionResult218;
				float3 Specular = temp_cast_30;
				float Metallic = 0;
				float Smoothness = SmoothnessRes556;
				float Occlusion = OcclusionRes773;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = BakedGI1077;
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
		
		/*ase_pass*/
        Pass
        {
        	
            Name "MotionVectors"
            Tags{ "LightMode" = "MotionVectors" }
            Tags { "RenderType" = "Opaque" }

            ZWrite On
            Cull Back
            ZTest LEqual

            HLSLPROGRAM
            /*ase_pragma_before*/
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/OculusMotionVectorCore.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            ENDHLSL
        }
        
		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }
			
			AlphaToMask Off

			Cull Off

			HLSLPROGRAM

			#define _ASE_DEBUGVISUALS 1
			#define _ASE_TONEMAPPING 1
			#define _NORMAL_DROPOFF_TS 1
			#define _ASE_TINT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define _SPECULAR_SETUP 1
			#define ASE_FOG 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define UNDERWATERELSEWHERE
			#define TONEMAPPINGELSEWHERE
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

			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma multi_compile_local __ _PROBEVOLUME_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColor;
			float4 _Color;
			float4 _RefFresnel;
			float4 _FoamColor;
			float4 _RefractionColorMobile;
			float4 _CausticsTint;
			float4x4 _ProbeWorldToTexture;
			float4 _RefractionColor;
			float4 _FoamColor2;
			float3 _ProbeVolumeSizeInv;
			float3 _CameraForward;
			float3 _WorldPos;
			float3 _WorldDir;
			float3 _ProbeVolumeMin;
			float _Smoothness;
			float _RefractionIndexUnderMobile;
			float _CubemapStrength;
			float _ForceEye;
			float _PlanarNormalStrengthm;
			float _ColorMix;
			float _ColorMixBack;
			float _ReflectionShadowedEnd;
			float _WaterSaturation;
			float _CausticRotate;
			float _CausticFlowStrength;
			float _CausticFlowSpeed;
			float _Specular;
			float _ReflectionShadowedStart;
			float _WaterDepthPow;
			float _RemapWaterDistance;
			float _ReflectionShadowed;
			float _ZWriteToggle;
			float _Size;
			float _FlowStrength;
			float _FlowSpeed;
			float _FoamPowA;
			float _Foam2Pow;
			float _Foam2Con;
			float _FoamSize;
			float _FoamFlowStrength;
			float _FoamFlowSpeed;
			float _WaterDepthPow1;
			float _Foam_Mix;
			float _FoamPowM;
			float _RefractionIndex;
			float _RefractionIndexUnder;
			float _NormalSize;
			float _NormFlowStrength;
			float _FlowSpeedN;
			float _NormalStrength;
			float _DepthFade;
			float _RefractionFake;
			float _RemapWaterDistance1;
			float _FoamSize2;
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

			float3 _CausticsDir;
			half4 _CausticsColor;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE3D(_ProbeVolumeShR);
			SAMPLER(sampler_ProbeVolumeShR);
			TEXTURE3D(_ProbeVolumeShG);
			SAMPLER(sampler_ProbeVolumeShG);
			TEXTURE3D(_ProbeVolumeAmbient);
			SAMPLER(sampler_ProbeVolumeAmbient);
			TEXTURE3D(_ProbeVolumeShB);
			SAMPLER(sampler_ProbeVolumeShB);
			TEXTURE3D(_ProbeVolumeOcc);
			SAMPLER(sampler_ProbeVolumeOcc);


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

				float3 VertexOffset1098 = ( v.ase_normal * GlobalOceanOffset );
				

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOffset1098;

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

				

				surfaceDescription.Alpha = 1;
				surfaceDescription.AlphaClipThreshold = 0.5;

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

			#define _ASE_DEBUGVISUALS 1
			#define _ASE_TONEMAPPING 1
			#define _NORMAL_DROPOFF_TS 1
			#define _ASE_TINT 1
			#define _RECEIVE_SHADOWS_OFF 1
			#define _SPECULAR_SETUP 1
			#define ASE_FOG 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
			#define UNDERWATERELSEWHERE
			#define TONEMAPPINGELSEWHERE
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

			#define ASE_NEEDS_VERT_NORMAL
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma multi_compile_local __ _PROBEVOLUME_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColor;
			float4 _Color;
			float4 _RefFresnel;
			float4 _FoamColor;
			float4 _RefractionColorMobile;
			float4 _CausticsTint;
			float4x4 _ProbeWorldToTexture;
			float4 _RefractionColor;
			float4 _FoamColor2;
			float3 _ProbeVolumeSizeInv;
			float3 _CameraForward;
			float3 _WorldPos;
			float3 _WorldDir;
			float3 _ProbeVolumeMin;
			float _Smoothness;
			float _RefractionIndexUnderMobile;
			float _CubemapStrength;
			float _ForceEye;
			float _PlanarNormalStrengthm;
			float _ColorMix;
			float _ColorMixBack;
			float _ReflectionShadowedEnd;
			float _WaterSaturation;
			float _CausticRotate;
			float _CausticFlowStrength;
			float _CausticFlowSpeed;
			float _Specular;
			float _ReflectionShadowedStart;
			float _WaterDepthPow;
			float _RemapWaterDistance;
			float _ReflectionShadowed;
			float _ZWriteToggle;
			float _Size;
			float _FlowStrength;
			float _FlowSpeed;
			float _FoamPowA;
			float _Foam2Pow;
			float _Foam2Con;
			float _FoamSize;
			float _FoamFlowStrength;
			float _FoamFlowSpeed;
			float _WaterDepthPow1;
			float _Foam_Mix;
			float _FoamPowM;
			float _RefractionIndex;
			float _RefractionIndexUnder;
			float _NormalSize;
			float _NormFlowStrength;
			float _FlowSpeedN;
			float _NormalStrength;
			float _DepthFade;
			float _RefractionFake;
			float _RemapWaterDistance1;
			float _FoamSize2;
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

			float3 _CausticsDir;
			half4 _CausticsColor;
			half _CausticsScale;
			half2 _CausticsPanSpeed;
			TEXTURE2D(_Caustics2);
			SAMPLER(sampler_Caustics2);
			TEXTURE2D(_Caustics);
			SAMPLER(sampler_Caustics);
			TEXTURE3D(_ProbeVolumeShR);
			SAMPLER(sampler_ProbeVolumeShR);
			TEXTURE3D(_ProbeVolumeShG);
			SAMPLER(sampler_ProbeVolumeShG);
			TEXTURE3D(_ProbeVolumeAmbient);
			SAMPLER(sampler_ProbeVolumeAmbient);
			TEXTURE3D(_ProbeVolumeShB);
			SAMPLER(sampler_ProbeVolumeShB);
			TEXTURE3D(_ProbeVolumeOcc);
			SAMPLER(sampler_ProbeVolumeOcc);


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

				float3 VertexOffset1098 = ( v.ase_normal * GlobalOceanOffset );
				

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = VertexOffset1098;

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

				

				surfaceDescription.Alpha = 1;
				surfaceDescription.AlphaClipThreshold = 0.5;

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
