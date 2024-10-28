// Made with Amplify Shader Editor v1.9.3.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ThunderRoad/Dev/Foliage - Dev"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[Feature(_CAUSTICSENABLE)]HeaderOceanFog("# Ocean Fog Caustics", Float) = 0
		_TransparencyHeader("# Transparency", Int) = 0
		[Toggle(_ALPHATEST_ON)]_AlphaClip("Alpha Clipping", Range( 0 , 1)) = 1
		_AlphaClipThreshold("-AlphaClip Threshold [!_AlphaToCoverage&&_AlphaClip]", Range( 0 , 1.01)) = 0.5
		_ShadowThreshold("-Shadow Threshold (Realtime) [_AlphaClip]", Range( 0 , 1.01)) = 0.5
		_Cutoff("Cutoff (Baked Lighting)", Range( 0 , 1.01)) = 0.5
		_RemapAlpha("RemapAlpha", Vector) = (0,1,0,1)
		DRAWER_RemapAlpha("!DRAWER MinMax _RemapAlpha RemapAlpha", Float) = 0
		HeaderGeneral("# General", Int) = 0
		[NoScaleOffset]_MainTex("Albedo &", 2D) = "white" {}
		Vector1_2F70D75B("Saturation", Range( 0 , 2)) = 1
		[HDR][Gamma][MainColor]Color_C776E9F7("Color Overlay", Color) = (1,1,1,1)
		_HueShift("HueShift", Range( -1 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		Vector1_5DA372D6("Smoothness", Range( 0 , 1)) = 0.2
		_Occlusion("Occlusion", Range( 0 , 1)) = 0
		[NoScaleOffset]Texture2D_363A5C4F("Normal Map &&", 2D) = "bump" {}
		Vector1_b5167c4675424009928f6f8474af501f("Normal Strength", Range( -2 , 2)) = 1
		[Feature(_PROBEVOLUME)]_PRVOL("# Probe Volumes", Float) = 0
		[Toggle(_PROBEVOLUME_ON)] _UseProbeVolume("Use Probe Volume", Float) = 0
		_ProbeVolumeShR("ProbeVolumeShR & [_PROBEVOLUME_ON]", 3D) = "black" {}
		_ProbeVolumeShG("ProbeVolumeShG & [_PROBEVOLUME_ON]", 3D) = "black" {}
		[HideInInspector]_ProbeVolumeMin("ProbeVolumeMin", Vector) = (0,0,0,0)
		_ProbeVolumeShB("ProbeVolumeShB & [_PROBEVOLUME_ON]", 3D) = "black" {}
		[HideInInspector]_ProbeVolumeSizeInv("ProbeVolumeSizeInv", Vector) = (0,0,0,0)
		_ProbeVolumeOcc("ProbeVolumeOcc & [_PROBEVOLUME_ON]", 3D) = "black" {}
		HeaderWind("# Wind", Int) = 0
		[NoScaleOffset]Texture2D_3899621E("Wind Normal &", 2D) = "bump" {}
		Vector1_534F54C8("Wind Intensity", Float) = 0.1
		Vector1_A0C32172("Wind Scale Primary", Float) = 55
		[Toggle]BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF("Use Wind Secondary", Float) = 1
		Vector1_F7C5A4BA("-Wind Scale Secondary [BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF]", Float) = 20
		Vector1_736F1270("Time Scale", Float) = 0.03
		HeaderBackLighting("# Back Lighting", Int) = 0
		[Toggle]_UseRForAmbientOcclusion("Use R For Ambient Occlusion", Float) = 0
		_OcclusionBackLit("Occlusion Back Lit", Range( 0 , 1)) = 0
		[Toggle(_USE_TRANSMISSION)] _USE_TRANSMISSION("Use Transmission", Float) = 0
		[HDR]_TransmissionColor("-Color [_USE_TRANSMISSION]", Color) = (0.9943649,1,0.5801887,0)
		_TransmissionShadow("-Shadow [_USE_TRANSMISSION]", Range( 0 , 1)) = 0.5
		[Toggle(_TRANSLUCENCY)] _TRANSLUCENCY("Use Translucency", Float) = 0
		[HDR]_TranslucencyColor("-Color [_TRANSLUCENCY]", Color) = (0.9949981,1,0.6273585,0)
		_TranslucencyStrength("-Strength [_TRANSLUCENCY]", Range( 0 , 50)) = 2
		_TranslucencyDirect("-Direct [_TRANSLUCENCY]", Range( 0 , 1)) = 0.9
		_TranslucencyShadow("-Shadow [_TRANSLUCENCY]", Range( 0 , 1)) = 0.9
		_TranslucencyScattering("-Scattering [_TRANSLUCENCY]", Range( 0.01 , 2)) = 0.5
		_TranslucencyNormalBias("-Normal Bias [_TRANSLUCENCY]", Range( 0 , 1)) = 0.5
		_TranslucencyNormalDistortion("-Normal Distortion [_TRANSLUCENCY]", Range( 0 , 1)) = 0.5
		_TranslucencyAmbient("-Ambient [_TRANSLUCENCY]", Range( 0 , 1)) = 0.1
		[FeatureCommentOut(_UseColorMask)]HeaderColorMask("# ColorMask", Int) = 0
		[Toggle]_UseColorMask("Use Color Mask", Float) = 0
		[HDR]_ColorMask("Color Mask [_UseColorMask]", Color) = (1,0.3563676,0,1)
		_Strength("Strength [_UseColorMask]", Range( 0 , 1)) = 1
		_ColorMaskRemap("ColorMask Remap [_UseColorMask]", Vector) = (0,1,0,1)
		DRAWER_ColorMaskRemap("!DRAWER MinMax _ColorMaskRemap Remap [_UseColorMask] ", Float) = 0
		[FeatureCommentOut(_DebugVisuals)]_Debug("# Debug", Int) = 0
		[Toggle(_DEBUGVISUALS_ON)] _DebugVisuals("DebugVisuals", Float) = 0
		[MaterialEnumDrawerExtended(VertexR,0,VertexG,1,VertexB,2,VertexA,3)]_DebugVisual("DebugVisual [_DebugVisuals]", Int) = 0
		[Feature(_SKYFOG)]_SkyFogHeader("# Sky Fog", Float) = 0
		[Feature(_SKYFOG)][Toggle(_SKYFOG_ON)] _SKYFOG("Use Sky Fog", Float) = 0
		HeaderFresnel("# Fresnel", Float) = 0
		_OtherHeader("# Other", Int) = 0
		[MaterialEnumDrawerExtended(UnityEngine.Rendering.CullMode)]_Cullmode("Cullmode", Int) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}


		_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		_TransStrength( "Strength", Range( 0, 50 ) ) = 1
		_TransNormal( "Normal Distortion", Range( 0, 1 ) ) = 0.5
		_TransScattering( "Scattering", Range( 1, 50 ) ) = 2
		_TransDirect( "Direct", Range( 0, 1 ) ) = 0.9
		_TransAmbient( "Ambient", Range( 0, 1 ) ) = 0.1
		_TransShadow( "Shadow", Range( 0, 1 ) ) = 0.5
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

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="TransparentCutout" "Queue"="AlphaTest" "UniversalMaterialType"="Lit" }

		Cull [_Cullmode]
		ZWrite On
		ZTest LEqual
		Offset 0,0
		AlphaToMask [AlphaToCoverage]

		

		HLSLINCLUDE
		#pragma target 4.5
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

			Blend One Zero, One Zero
			//BlendOp [_BlendOp]// Shadowood
			ZWrite On
			ZTest LEqual
			Offset 0,0
			ColorMask RGBA

			

			HLSLPROGRAM

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_TRANSLUCENCY 1
			#define ASE_TRANSMISSION 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
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

			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma shader_feature _TRANSLUCENCY
			#pragma shader_feature _USE_TRANSMISSION
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON
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
				float4 ase_color : COLOR;
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
				float4 ase_color : COLOR;
				float4 ase_texcoord12 : TEXCOORD12;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _RemapAlpha;
			float4 _ColorMaskRemap;
			float4 Color_C776E9F7;
			float4 _ColorMask;
			float4x4 _ProbeWorldToTexture;
			float4 _TranslucencyColor;
			float4 _TransmissionColor;
			float3 _ProbeVolumeSizeInv;
			float3 _ProbeVolumeMin;
			float _ShadowThreshold;
			float _AlphaClip;
			float _AlphaClipThreshold;
			float _Occlusion;
			float Vector1_5DA372D6;
			float _Metallic;
			float Vector1_b5167c4675424009928f6f8474af501f;
			float _Strength;
			float _OcclusionBackLit;
			int _Cullmode;
			float Vector1_2F70D75B;
			float _UseColorMask;
			float _Cutoff;
			float _TranslucencyNormalBias;
			float _TranslucencyNormalDistortion;
			float _TranslucencyDirect;
			float _TranslucencyAmbient;
			float _TranslucencyShadow;
			float _HueShift;
			float _TranslucencyScattering;
			float _UseRForAmbientOcclusion;
			float BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF;
			float Vector1_A0C32172;
			float Vector1_736F1270;
			float Vector1_F7C5A4BA;
			float Vector1_534F54C8;
			float _TranslucencyStrength;
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
			TEXTURE2D(Texture2D_3899621E);
			SAMPLER(sampler_Linear_Repeat_Aniso8);
			TEXTURE2D(_MainTex);
			TEXTURE2D(Texture2D_363A5C4F);
			int AlphaToCoverage;
			TEXTURE3D(_ProbeVolumeShR);
			SAMPLER(sampler_Linear_Clamp);
			TEXTURE3D(_ProbeVolumeShG);
			TEXTURE3D(_ProbeVolumeShB);
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
			
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}
			inline float3 SampleSHPixel381( float3 SH, float3 normalWS )
			{
				return SampleSHPixel( SH, normalWS );
			}
			
			inline float3 SampleLightmap380( float2 lightmapUV, float3 normalWS )
			{
				return SampleLightmap( lightmapUV, 0, normalWS );
			}
			
			float3x3 CastToFloat3x354_g2827( float3x3 Input )
			{
				return Input;
			}
			
			float3 MyCustomExpression6_g2827( float3 vertexPos, float4x4 ProbeWorldToTexture, float3 ProbeVolumeMin, float3 ProbeVolumeSizeInv )
			{
				float3 position = mul(ProbeWorldToTexture, float4(TransformObjectToWorld(vertexPos.xyz), 1.0f)).xyz;
				float3 texCoord = (position - ProbeVolumeMin.xyz) * ProbeVolumeSizeInv;
				return texCoord;
			}
			
			float3 SHEvalLinearL0L114_g2827( float3 worldNormal, float4 ProbeVolumeShR, float4 ProbeVolumeShG, float4 ProbeVolumeShB )
			{
				return SHEvalLinearL0L1(worldNormal, ProbeVolumeShR, ProbeVolumeShG,  ProbeVolumeShB);
			}
			
			half4 CalculateShadowMask1_g2829( half2 LightmapUV )
			{
				#if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
				return SAMPLE_SHADOWMASK( LightmapUV.xy );
				#elif !defined (LIGHTMAP_ON)
				return unity_ProbesOcclusion;
				#else
				return half4( 1, 1, 1, 1 );
				#endif
			}
			
			inline float4 GetUnderWaterFogs240_g2806( float3 viewDir, float3 camWorldPos, float3 posWS, float4 oceanFogDensities, float oceanHeight, float4 oceanFogTop_RGB_Exponent, float4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 temp_output_390_0 = saturate( v.ase_color );
				float4 break391 = temp_output_390_0;
				float ifLocalVar432 = 0;
				if( _UseRForAmbientOcclusion <= 0.5 )
				ifLocalVar432 = 1.0;
				else
				ifLocalVar432 = break391.r;
				float4 appendResult393 = (float4(break391.r , saturate( ifLocalVar432 ) , break391.b , 0.0));
				float4 vertexToFrag395 = appendResult393;
				float4 break394 = vertexToFrag395;
				float VCWindMaskR238 = break394.x;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float2 appendResult27 = (float2(ase_worldPos.x , ase_worldPos.z));
				float temp_output_33_0 = ( Vector1_736F1270 * _TimeParameters.x );
				float2 appendResult37 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 appendResult38 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 temp_output_41_0 = ( appendResult37 + ( appendResult38 * 0.5 ) );
				float2 ifLocalVar439 = 0;
				if( BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF <= 0.0 )
				ifLocalVar439 = appendResult37;
				else
				ifLocalVar439 = temp_output_41_0;
				float2 break49 = ( VCWindMaskR238 * ifLocalVar439 * Vector1_534F54C8 );
				float3 appendResult48 = (float3(break49.x , 0.0 , break49.y));
				float3 VertexPos46 = appendResult48;
				float3 vertexToFrag392 = VertexPos46;
				
				o.ase_texcoord9 = vertexToFrag395;
				
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				float4x4 temp_output_28_0_g2827 = _ProbeWorldToTexture;
				float3x3 Input54_g2827 = 0;
				float3x3 localCastToFloat3x354_g2827 = CastToFloat3x354_g2827( Input54_g2827 );
				float3 vertexToFrag56_g2827 = mul( ase_worldNormal, localCastToFloat3x354_g2827 );
				o.ase_texcoord10.xyz = vertexToFrag56_g2827;
				float3 vertexPos6_g2827 = v.vertex.xyz;
				float4x4 ProbeWorldToTexture6_g2827 = temp_output_28_0_g2827;
				float3 ProbeVolumeMin6_g2827 = _ProbeVolumeMin;
				float3 ProbeVolumeSizeInv6_g2827 = _ProbeVolumeSizeInv;
				float3 localMyCustomExpression6_g2827 = MyCustomExpression6_g2827( vertexPos6_g2827 , ProbeWorldToTexture6_g2827 , ProbeVolumeMin6_g2827 , ProbeVolumeSizeInv6_g2827 );
				float3 vertexToFrag7_g2827 = localMyCustomExpression6_g2827;
				o.ase_texcoord11.xyz = vertexToFrag7_g2827;
				
				o.ase_texcoord8.xyz = v.texcoord.xyz;
				o.ase_color = v.ase_color;
				o.ase_texcoord12.xy = v.texcoord1.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.w = 0;
				o.ase_texcoord10.w = 0;
				o.ase_texcoord11.w = 0;
				o.ase_texcoord12.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = vertexToFrag392;

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
				float4 ase_color : COLOR;

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

				float3 appendResult316 = (float3(Color_C776E9F7.rgb));
				float2 uv_MainTex8 = IN.ase_texcoord8.xyz.xy;
				float4 tex2DNode8 = SAMPLE_TEXTURE2D( _MainTex, sampler_Linear_Repeat_Aniso8, uv_MainTex8 );
				float3 hsvTorgb300 = RGBToHSV( tex2DNode8.rgb );
				float temp_output_302_0 = ( _HueShift + hsvTorgb300.x );
				float temp_output_354_0 = ( hsvTorgb300.y * Vector1_2F70D75B );
				float3 appendResult405 = (float3(temp_output_302_0 , temp_output_354_0 , hsvTorgb300.z));
				float3 hsvTorgb357 = RGBToHSV( _ColorMask.rgb );
				float3 appendResult407 = (float3(hsvTorgb357.x , hsvTorgb357.y , ( hsvTorgb357.x + _HueShift )));
				float4 vertexToFrag395 = IN.ase_texcoord9;
				float4 break394 = vertexToFrag395;
				float VCColorMaskB240 = break394.z;
				float temp_output_8_0_g2834 = saturate( VCColorMaskB240 );
				float4 break5_g2834 = _ColorMaskRemap;
				float temp_output_32_0_g2834 = (break5_g2834.z + (temp_output_8_0_g2834 - break5_g2834.x) * (break5_g2834.w - break5_g2834.z) / (break5_g2834.y - break5_g2834.x));
				float temp_output_270_0 = temp_output_32_0_g2834;
				float3 appendResult365 = (float3(temp_output_270_0 , temp_output_270_0 , temp_output_270_0));
				float3 lerpResult356 = lerp( appendResult405 , appendResult407 , saturate( ( appendResult365 * _Strength ) ));
				float3 break360 = lerpResult356;
				float ifLocalVar435 = 0;
				if( _UseColorMask <= 0.5 )
				ifLocalVar435 = temp_output_302_0;
				else
				ifLocalVar435 = break360.x;
				float ifLocalVar437 = 0;
				if( _UseColorMask <= 0.5 )
				ifLocalVar437 = temp_output_354_0;
				else
				ifLocalVar437 = break360.y;
				float3 hsvTorgb301 = HSVToRGB( float3(ifLocalVar435,ifLocalVar437,hsvTorgb300.z) );
				float4 appendResult312 = (float4(( appendResult316 * hsvTorgb301 ) , tex2DNode8.a));
				float4 Albedo74 = appendResult312;
				
				float2 uvTexture2D_363A5C4F11 = IN.ase_texcoord8.xyz.xy;
				float3 unpack11 = UnpackNormalScale( SAMPLE_TEXTURE2D( Texture2D_363A5C4F, sampler_Linear_Repeat_Aniso8, uvTexture2D_363A5C4F11 ), Vector1_b5167c4675424009928f6f8474af501f );
				unpack11.z = lerp( 1, unpack11.z, saturate(Vector1_b5167c4675424009928f6f8474af501f) );
				float3 tex2DNode11 = unpack11;
				float3 switchResult103 = (((ase_vface>0)?(tex2DNode11):(( float3( 0,0,0 ) - tex2DNode11 ))));
				float3 NormalRes233 = switchResult103;
				
				int BackLit142 = 0;
				float3 temp_cast_5 = BackLit142;
				
				float Smoothness88 = Vector1_5DA372D6;
				
				float VCAmbientOcclusionG239 = break394.y;
				float3 temp_cast_6 = (VCAmbientOcclusionG239).xxx;
				float temp_output_2_0_g1416 = _Occlusion;
				float temp_output_3_0_g1416 = ( 1.0 - temp_output_2_0_g1416 );
				float3 appendResult7_g1416 = (float3(temp_output_3_0_g1416 , temp_output_3_0_g1416 , temp_output_3_0_g1416));
				float3 OcclusionRes252 = ( ( temp_cast_6 * temp_output_2_0_g1416 ) + appendResult7_g1416 );
				
				float Alpha157 = (0.0 + (tex2DNode8.a - _RemapAlpha.x) * (1.0 - 0.0) / (_RemapAlpha.y - _RemapAlpha.x));
				
				float ifLocalVar412 = 0;
				if( _AlphaClip <= 0 )
				ifLocalVar412 = 0.0;
				else
				ifLocalVar412 = _AlphaClipThreshold;
				float ifLocalVar411 = 0;
				if( AlphaToCoverage <= 0 )
				ifLocalVar411 = ifLocalVar412;
				else
				ifLocalVar411 = _AlphaClipThreshold;
				float ClipCalc397 = ifLocalVar411;
				
				float ShadowThreshold335 = _ShadowThreshold;
				
				float3 SH381 = IN.lightmapUVOrVertexSH.xyz;
				float3 normalWS381 = WorldNormal;
				float3 localSampleSHPixel381 = SampleSHPixel381( SH381 , normalWS381 );
				float2 lightmapUV380 = IN.lightmapUVOrVertexSH.xy;
				float3 normalWS380 = WorldNormal;
				float3 localSampleLightmap380 = SampleLightmap380( lightmapUV380 , normalWS380 );
				#ifdef LIGHTMAP_ON
				float3 staticSwitch379 = localSampleLightmap380;
				#else
				float3 staticSwitch379 = localSampleSHPixel381;
				#endif
				float3 vertexToFrag56_g2827 = IN.ase_texcoord10.xyz;
				float3 worldNormal14_g2827 = vertexToFrag56_g2827;
				float3 vertexToFrag7_g2827 = IN.ase_texcoord11.xyz;
				float4 ProbeVolumeShR14_g2827 = SAMPLE_TEXTURE3D( _ProbeVolumeShR, sampler_Linear_Clamp, ( vertexToFrag7_g2827 + float3( float2( 0,0 ) ,  0.0 ) ) );
				float4 ProbeVolumeShG14_g2827 = SAMPLE_TEXTURE3D( _ProbeVolumeShG, sampler_Linear_Clamp, ( vertexToFrag7_g2827 + float3( float2( 0,0 ) ,  0.0 ) ) );
				float4 ProbeVolumeShB14_g2827 = SAMPLE_TEXTURE3D( _ProbeVolumeShB, sampler_Linear_Clamp, ( vertexToFrag7_g2827 + float3( float2( 0,0 ) ,  0.0 ) ) );
				float3 localSHEvalLinearL0L114_g2827 = SHEvalLinearL0L114_g2827( worldNormal14_g2827 , ProbeVolumeShR14_g2827 , ProbeVolumeShG14_g2827 , ProbeVolumeShB14_g2827 );
				#ifdef _PROBEVOLUME_ON
				float3 staticSwitch20_g2827 = localSHEvalLinearL0L114_g2827;
				#else
				float3 staticSwitch20_g2827 = staticSwitch379;
				#endif
				float3 BakedGI217 = staticSwitch20_g2827;
				
				float3 temp_cast_16 = (VCAmbientOcclusionG239).xxx;
				float temp_output_2_0_g1420 = _OcclusionBackLit;
				float temp_output_3_0_g1420 = ( 1.0 - temp_output_2_0_g1420 );
				float3 appendResult7_g1420 = (float3(temp_output_3_0_g1420 , temp_output_3_0_g1420 , temp_output_3_0_g1420));
				float3 temp_output_266_0 = ( ( temp_cast_16 * temp_output_2_0_g1420 ) + appendResult7_g1420 );
				float4 TransmissionRes338 = ( float4( temp_output_266_0 , 0.0 ) * _TransmissionColor * Albedo74 );
				
				float4 TranslucencyRes342 = ( float4( temp_output_266_0 , 0.0 ) * _TranslucencyColor );
				
				float4 ifLocalVars223 = 0;
				float4 temp_cast_22 = (IN.ase_color.r).xxxx;
				if(_DebugVisual==0){ifLocalVars223 = temp_cast_22; };
				float4 temp_cast_23 = (IN.ase_color.g).xxxx;
				if(_DebugVisual==1){ifLocalVars223 = temp_cast_23; };
				float4 temp_cast_24 = (IN.ase_color.b).xxxx;
				if(_DebugVisual==2){ifLocalVars223 = temp_cast_24; };
				float4 temp_cast_25 = (IN.ase_color.a).xxxx;
				if(_DebugVisual==3){ifLocalVars223 = temp_cast_25; };
				float4 DebugVisuals225 = ifLocalVars223;
				
				half2 LightmapUV1_g2829 = (IN.ase_texcoord12.xy*(unity_LightmapST).xy + (unity_LightmapST).zw);
				half4 localCalculateShadowMask1_g2829 = CalculateShadowMask1_g2829( LightmapUV1_g2829 );
				float4 tex3DNode16_g2827 = SAMPLE_TEXTURE3D( _ProbeVolumeOcc, sampler_Linear_Clamp, ( vertexToFrag7_g2827 + float3( float2( 0,0 ) ,  0.0 ) ) );
				#ifdef _PROBEVOLUME_ON
				float4 staticSwitch25_g2827 = tex3DNode16_g2827;
				#else
				float4 staticSwitch25_g2827 = float4( localCalculateShadowMask1_g2829.xyz , 0.0 );
				#endif
				float4 ShadowMask218 = staticSwitch25_g2827;
				
				float3 appendResult50_g2753 = (float3(unity_FogColor.rgb));
				float4 appendResult52_g2753 = (float4(appendResult50_g2753 , ( 1.0 - IN.fogFactorAndVertexLight.x )));
				#ifdef FOG_LINEAR
				float4 staticSwitch252_g2753 = appendResult52_g2753;
				#else
				float4 staticSwitch252_g2753 = float4( 0,0,0,0 );
				#endif
				float4 FogLinear53_g2753 = staticSwitch252_g2753;
				float4 appendResult47_g2753 = (float4(FogLinear53_g2753));
				float temp_output_23_0_g2753 = GlobalOceanUnder;
				float3 ViewDir264_g2753 = WorldViewDirection;
				float3 viewDir240_g2806 = ViewDir264_g2753;
				float3 camWorldPos240_g2806 = _WorldSpaceCameraPos;
				float3 WorldPosition256_g2753 = WorldPosition;
				float3 WorldPos252_g2806 = WorldPosition256_g2753;
				float3 posWS240_g2806 = WorldPos252_g2806;
				float4 oceanFogDensities240_g2806 = OceanFogDensities;
				float temp_output_67_0_g2753 = ( GlobalOceanOffset + GlobalOceanHeight );
				float temp_output_108_0_g2806 = temp_output_67_0_g2753;
				float oceanHeight240_g2806 = temp_output_108_0_g2806;
				float4 oceanFogTop_RGB_Exponent240_g2806 = OceanFogTop_RGB_Exponent;
				float4 oceanFogBottom_RGB_Intensity240_g2806 = OceanFogBottom_RGB_Intensity;
				float4 localGetUnderWaterFogs240_g2806 = GetUnderWaterFogs240_g2806( viewDir240_g2806 , camWorldPos240_g2806 , posWS240_g2806 , oceanFogDensities240_g2806 , oceanHeight240_g2806 , oceanFogTop_RGB_Exponent240_g2806 , oceanFogBottom_RGB_Intensity240_g2806 );
				float4 FogRes185_g2806 = localGetUnderWaterFogs240_g2806;
				float3 appendResult94_g2806 = (float3(FogRes185_g2806.xyz));
				float3 temp_output_261_103_g2753 = appendResult94_g2806;
				float temp_output_61_0_g2806 = ( 1.0 - (FogRes185_g2806).w );
				float4 appendResult44_g2753 = (float4(temp_output_261_103_g2753 , ( 1.0 - temp_output_61_0_g2806 )));
				float temp_output_254_0_g2753 = (WorldPosition256_g2753).y;
				float temp_output_24_0_g2812 = ( temp_output_254_0_g2753 - temp_output_67_0_g2753 );
				float temp_output_44_0_g2812 = 0.1;
				float temp_output_45_0_g2812 = 0.31;
				float temp_output_46_0_g2812 = saturate( (0.0 + (( temp_output_24_0_g2812 - temp_output_44_0_g2812 ) - 0.0) * (1.0 - 0.0) / (temp_output_45_0_g2812 - 0.0)) );
				float temp_output_160_47_g2753 = ( 1.0 - temp_output_46_0_g2812 );
				#ifdef _FadeWithHeight
				float staticSwitch238_g2753 = temp_output_160_47_g2753;
				#else
				float staticSwitch238_g2753 = 1.0;
				#endif
				float4 lerpResult227_g2753 = lerp( appendResult47_g2753 , appendResult44_g2753 , staticSwitch238_g2753);
				float4 ifLocalVar49_g2753 = 0;
				UNITY_BRANCH 
				if( temp_output_23_0_g2753 >= 1.0 )
				ifLocalVar49_g2753 = lerpResult227_g2753;
				else
				ifLocalVar49_g2753 = appendResult47_g2753;
				#ifdef _USEUNDERWATER
				float4 staticSwitch215_g2753 = max( ifLocalVar49_g2753 , float4( 0,0,0,0 ) );
				#else
				float4 staticSwitch215_g2753 = appendResult47_g2753;
				#endif
				
				float4 appendResult11_g2752 = (float4(_TonemappingSettings.x , _TonemappingSettings.y , _TonemappingSettings.z , _TonemappingSettings.w));
				
				float3 temp_cast_34 = (1.0).xxx;
				float3 temp_cast_35 = (1.0).xxx;
				float3 appendResult100_g2806 = (float3(OceanWaterTint_RGB.xyz));
				float3 temp_cast_37 = (1.0).xxx;
				float3 ifLocalVar170_g2753 = 0;
				UNITY_BRANCH 
				if( temp_output_23_0_g2753 >= 1.0 )
				ifLocalVar170_g2753 = appendResult100_g2806;
				else
				ifLocalVar170_g2753 = temp_cast_37;
				float3 lerpResult226_g2753 = lerp( temp_cast_35 , ifLocalVar170_g2753 , staticSwitch238_g2753);
				#ifdef _USEUNDERWATER
				float3 staticSwitch212_g2753 = lerpResult226_g2753;
				#else
				float3 staticSwitch212_g2753 = temp_cast_34;
				#endif
				

				float3 BaseColor = Albedo74.xyz;
				float3 Normal = NormalRes233;
				float3 Emission = temp_cast_5;
				float3 Specular = 0.5;
				float Metallic = _Metallic;
				float Smoothness = Smoothness88;
				float Occlusion = OcclusionRes252.x;
				float Alpha = Alpha157;
				float AlphaClipThreshold = ClipCalc397;
				float AlphaClipThresholdShadow = ShadowThreshold335;
				float3 BakedGI = BakedGI217;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = TransmissionRes338.rgb;
				float3 Translucency = TranslucencyRes342.rgb;
				
				//Shadowood: new properties below:
				float3 DebugVisuals = DebugVisuals225.xyz;
				float4 ShadowMask = ShadowMask218;
				float4 FogColor = staticSwitch215_g2753;
				half FresnelControl = 0;
				half4 SSSControl = 0;
				half4 SSSColor = 0;
				float4 Tonemapping = appendResult11_g2752;
				float3 Tint = staticSwitch212_g2753;
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

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#define ASE_FOG 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_TRANSLUCENCY 1
			#define ASE_TRANSMISSION 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
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
			
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _TRANSLUCENCY
			#pragma shader_feature _USE_TRANSMISSION
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature _SKYFOG_ON
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
				float4 ase_color : COLOR;
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
			float4 _RemapAlpha;
			float4 _ColorMaskRemap;
			float4 Color_C776E9F7;
			float4 _ColorMask;
			float4x4 _ProbeWorldToTexture;
			float4 _TranslucencyColor;
			float4 _TransmissionColor;
			float3 _ProbeVolumeSizeInv;
			float3 _ProbeVolumeMin;
			float _ShadowThreshold;
			float _AlphaClip;
			float _AlphaClipThreshold;
			float _Occlusion;
			float Vector1_5DA372D6;
			float _Metallic;
			float Vector1_b5167c4675424009928f6f8474af501f;
			float _Strength;
			float _OcclusionBackLit;
			int _Cullmode;
			float Vector1_2F70D75B;
			float _UseColorMask;
			float _Cutoff;
			float _TranslucencyNormalBias;
			float _TranslucencyNormalDistortion;
			float _TranslucencyDirect;
			float _TranslucencyAmbient;
			float _TranslucencyShadow;
			float _HueShift;
			float _TranslucencyScattering;
			float _UseRForAmbientOcclusion;
			float BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF;
			float Vector1_A0C32172;
			float Vector1_736F1270;
			float Vector1_F7C5A4BA;
			float Vector1_534F54C8;
			float _TranslucencyStrength;
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
			TEXTURE2D(Texture2D_3899621E);
			SAMPLER(sampler_Linear_Repeat_Aniso8);
			TEXTURE2D(_MainTex);
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
			

			float3 _LightDirection;
			float3 _LightPosition;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float4 temp_output_390_0 = saturate( v.ase_color );
				float4 break391 = temp_output_390_0;
				float ifLocalVar432 = 0;
				if( _UseRForAmbientOcclusion <= 0.5 )
				ifLocalVar432 = 1.0;
				else
				ifLocalVar432 = break391.r;
				float4 appendResult393 = (float4(break391.r , saturate( ifLocalVar432 ) , break391.b , 0.0));
				float4 vertexToFrag395 = appendResult393;
				float4 break394 = vertexToFrag395;
				float VCWindMaskR238 = break394.x;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float2 appendResult27 = (float2(ase_worldPos.x , ase_worldPos.z));
				float temp_output_33_0 = ( Vector1_736F1270 * _TimeParameters.x );
				float2 appendResult37 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 appendResult38 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 temp_output_41_0 = ( appendResult37 + ( appendResult38 * 0.5 ) );
				float2 ifLocalVar439 = 0;
				if( BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF <= 0.0 )
				ifLocalVar439 = appendResult37;
				else
				ifLocalVar439 = temp_output_41_0;
				float2 break49 = ( VCWindMaskR238 * ifLocalVar439 * Vector1_534F54C8 );
				float3 appendResult48 = (float3(break49.x , 0.0 , break49.y));
				float3 VertexPos46 = appendResult48;
				float3 vertexToFrag392 = VertexPos46;
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = vertexToFrag392;
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
				float4 ase_color : COLOR;
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
				o.ase_color = v.ase_color;
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
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
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

				float2 uv_MainTex8 = IN.ase_texcoord3.xy;
				float4 tex2DNode8 = SAMPLE_TEXTURE2D( _MainTex, sampler_Linear_Repeat_Aniso8, uv_MainTex8 );
				float Alpha157 = (0.0 + (tex2DNode8.a - _RemapAlpha.x) * (1.0 - 0.0) / (_RemapAlpha.y - _RemapAlpha.x));
				
				float ifLocalVar412 = 0;
				if( _AlphaClip <= 0 )
				ifLocalVar412 = 0.0;
				else
				ifLocalVar412 = _AlphaClipThreshold;
				float ifLocalVar411 = 0;
				if( AlphaToCoverage <= 0 )
				ifLocalVar411 = ifLocalVar412;
				else
				ifLocalVar411 = 0.0;
				float ClipCalc397 = ifLocalVar411;
				
				float ShadowThreshold335 = _ShadowThreshold;
				

				float Alpha = Alpha157;
				float AlphaClipThreshold = ClipCalc397;
				float AlphaClipThresholdShadow = ShadowThreshold335;

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

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#define ASE_FOG 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_TRANSLUCENCY 1
			#define ASE_TRANSMISSION 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
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

			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _TRANSLUCENCY
			#pragma shader_feature _USE_TRANSMISSION
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature _SKYFOG_ON
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
				float4 ase_color : COLOR;
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
			float4 _RemapAlpha;
			float4 _ColorMaskRemap;
			float4 Color_C776E9F7;
			float4 _ColorMask;
			float4x4 _ProbeWorldToTexture;
			float4 _TranslucencyColor;
			float4 _TransmissionColor;
			float3 _ProbeVolumeSizeInv;
			float3 _ProbeVolumeMin;
			float _ShadowThreshold;
			float _AlphaClip;
			float _AlphaClipThreshold;
			float _Occlusion;
			float Vector1_5DA372D6;
			float _Metallic;
			float Vector1_b5167c4675424009928f6f8474af501f;
			float _Strength;
			float _OcclusionBackLit;
			int _Cullmode;
			float Vector1_2F70D75B;
			float _UseColorMask;
			float _Cutoff;
			float _TranslucencyNormalBias;
			float _TranslucencyNormalDistortion;
			float _TranslucencyDirect;
			float _TranslucencyAmbient;
			float _TranslucencyShadow;
			float _HueShift;
			float _TranslucencyScattering;
			float _UseRForAmbientOcclusion;
			float BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF;
			float Vector1_A0C32172;
			float Vector1_736F1270;
			float Vector1_F7C5A4BA;
			float Vector1_534F54C8;
			float _TranslucencyStrength;
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
			TEXTURE2D(Texture2D_3899621E);
			SAMPLER(sampler_Linear_Repeat_Aniso8);
			TEXTURE2D(_MainTex);
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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 temp_output_390_0 = saturate( v.ase_color );
				float4 break391 = temp_output_390_0;
				float ifLocalVar432 = 0;
				if( _UseRForAmbientOcclusion <= 0.5 )
				ifLocalVar432 = 1.0;
				else
				ifLocalVar432 = break391.r;
				float4 appendResult393 = (float4(break391.r , saturate( ifLocalVar432 ) , break391.b , 0.0));
				float4 vertexToFrag395 = appendResult393;
				float4 break394 = vertexToFrag395;
				float VCWindMaskR238 = break394.x;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float2 appendResult27 = (float2(ase_worldPos.x , ase_worldPos.z));
				float temp_output_33_0 = ( Vector1_736F1270 * _TimeParameters.x );
				float2 appendResult37 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 appendResult38 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 temp_output_41_0 = ( appendResult37 + ( appendResult38 * 0.5 ) );
				float2 ifLocalVar439 = 0;
				if( BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF <= 0.0 )
				ifLocalVar439 = appendResult37;
				else
				ifLocalVar439 = temp_output_41_0;
				float2 break49 = ( VCWindMaskR238 * ifLocalVar439 * Vector1_534F54C8 );
				float3 appendResult48 = (float3(break49.x , 0.0 , break49.y));
				float3 VertexPos46 = appendResult48;
				float3 vertexToFrag392 = VertexPos46;
				
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = vertexToFrag392;

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
				float4 ase_color : COLOR;
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
				o.ase_color = v.ase_color;
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
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
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

				float2 uv_MainTex8 = IN.ase_texcoord3.xy;
				float4 tex2DNode8 = SAMPLE_TEXTURE2D( _MainTex, sampler_Linear_Repeat_Aniso8, uv_MainTex8 );
				float Alpha157 = (0.0 + (tex2DNode8.a - _RemapAlpha.x) * (1.0 - 0.0) / (_RemapAlpha.y - _RemapAlpha.x));
				
				float ifLocalVar412 = 0;
				if( _AlphaClip <= 0 )
				ifLocalVar412 = 0.0;
				else
				ifLocalVar412 = _AlphaClipThreshold;
				float ifLocalVar411 = 0;
				if( AlphaToCoverage <= 0 )
				ifLocalVar411 = ifLocalVar412;
				else
				ifLocalVar411 = 0.0;
				float ClipCalc397 = ifLocalVar411;
				

				float Alpha = Alpha157;
				float AlphaClipThreshold = ClipCalc397;
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
			#define ASE_FOG 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_TRANSLUCENCY 1
			#define ASE_TRANSMISSION 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
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

			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _TRANSLUCENCY
			#pragma shader_feature _USE_TRANSMISSION
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord0 : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _RemapAlpha;
			float4 _ColorMaskRemap;
			float4 Color_C776E9F7;
			float4 _ColorMask;
			float4x4 _ProbeWorldToTexture;
			float4 _TranslucencyColor;
			float4 _TransmissionColor;
			float3 _ProbeVolumeSizeInv;
			float3 _ProbeVolumeMin;
			float _ShadowThreshold;
			float _AlphaClip;
			float _AlphaClipThreshold;
			float _Occlusion;
			float Vector1_5DA372D6;
			float _Metallic;
			float Vector1_b5167c4675424009928f6f8474af501f;
			float _Strength;
			float _OcclusionBackLit;
			int _Cullmode;
			float Vector1_2F70D75B;
			float _UseColorMask;
			float _Cutoff;
			float _TranslucencyNormalBias;
			float _TranslucencyNormalDistortion;
			float _TranslucencyDirect;
			float _TranslucencyAmbient;
			float _TranslucencyShadow;
			float _HueShift;
			float _TranslucencyScattering;
			float _UseRForAmbientOcclusion;
			float BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF;
			float Vector1_A0C32172;
			float Vector1_736F1270;
			float Vector1_F7C5A4BA;
			float Vector1_534F54C8;
			float _TranslucencyStrength;
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
			TEXTURE2D(Texture2D_3899621E);
			SAMPLER(sampler_Linear_Repeat_Aniso8);
			TEXTURE2D(_MainTex);
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
			
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 temp_output_390_0 = saturate( v.ase_color );
				float4 break391 = temp_output_390_0;
				float ifLocalVar432 = 0;
				if( _UseRForAmbientOcclusion <= 0.5 )
				ifLocalVar432 = 1.0;
				else
				ifLocalVar432 = break391.r;
				float4 appendResult393 = (float4(break391.r , saturate( ifLocalVar432 ) , break391.b , 0.0));
				float4 vertexToFrag395 = appendResult393;
				float4 break394 = vertexToFrag395;
				float VCWindMaskR238 = break394.x;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float2 appendResult27 = (float2(ase_worldPos.x , ase_worldPos.z));
				float temp_output_33_0 = ( Vector1_736F1270 * _TimeParameters.x );
				float2 appendResult37 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 appendResult38 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 temp_output_41_0 = ( appendResult37 + ( appendResult38 * 0.5 ) );
				float2 ifLocalVar439 = 0;
				if( BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF <= 0.0 )
				ifLocalVar439 = appendResult37;
				else
				ifLocalVar439 = temp_output_41_0;
				float2 break49 = ( VCWindMaskR238 * ifLocalVar439 * Vector1_534F54C8 );
				float3 appendResult48 = (float3(break49.x , 0.0 , break49.y));
				float3 VertexPos46 = appendResult48;
				float3 vertexToFrag392 = VertexPos46;
				
				o.ase_texcoord5 = vertexToFrag395;
				
				o.ase_texcoord4.xy = v.texcoord0.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord4.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = vertexToFrag392;

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
				float4 ase_color : COLOR;

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

				float3 appendResult316 = (float3(Color_C776E9F7.rgb));
				float2 uv_MainTex8 = IN.ase_texcoord4.xy;
				float4 tex2DNode8 = SAMPLE_TEXTURE2D( _MainTex, sampler_Linear_Repeat_Aniso8, uv_MainTex8 );
				float3 hsvTorgb300 = RGBToHSV( tex2DNode8.rgb );
				float temp_output_302_0 = ( _HueShift + hsvTorgb300.x );
				float temp_output_354_0 = ( hsvTorgb300.y * Vector1_2F70D75B );
				float3 appendResult405 = (float3(temp_output_302_0 , temp_output_354_0 , hsvTorgb300.z));
				float3 hsvTorgb357 = RGBToHSV( _ColorMask.rgb );
				float3 appendResult407 = (float3(hsvTorgb357.x , hsvTorgb357.y , ( hsvTorgb357.x + _HueShift )));
				float4 vertexToFrag395 = IN.ase_texcoord5;
				float4 break394 = vertexToFrag395;
				float VCColorMaskB240 = break394.z;
				float temp_output_8_0_g2834 = saturate( VCColorMaskB240 );
				float4 break5_g2834 = _ColorMaskRemap;
				float temp_output_32_0_g2834 = (break5_g2834.z + (temp_output_8_0_g2834 - break5_g2834.x) * (break5_g2834.w - break5_g2834.z) / (break5_g2834.y - break5_g2834.x));
				float temp_output_270_0 = temp_output_32_0_g2834;
				float3 appendResult365 = (float3(temp_output_270_0 , temp_output_270_0 , temp_output_270_0));
				float3 lerpResult356 = lerp( appendResult405 , appendResult407 , saturate( ( appendResult365 * _Strength ) ));
				float3 break360 = lerpResult356;
				float ifLocalVar435 = 0;
				if( _UseColorMask <= 0.5 )
				ifLocalVar435 = temp_output_302_0;
				else
				ifLocalVar435 = break360.x;
				float ifLocalVar437 = 0;
				if( _UseColorMask <= 0.5 )
				ifLocalVar437 = temp_output_354_0;
				else
				ifLocalVar437 = break360.y;
				float3 hsvTorgb301 = HSVToRGB( float3(ifLocalVar435,ifLocalVar437,hsvTorgb300.z) );
				float4 appendResult312 = (float4(( appendResult316 * hsvTorgb301 ) , tex2DNode8.a));
				float4 Albedo74 = appendResult312;
				
				int BackLit142 = 0;
				float3 temp_cast_5 = BackLit142;
				
				float Alpha157 = (0.0 + (tex2DNode8.a - _RemapAlpha.x) * (1.0 - 0.0) / (_RemapAlpha.y - _RemapAlpha.x));
				
				float ifLocalVar412 = 0;
				if( _AlphaClip <= 0 )
				ifLocalVar412 = 0.0;
				else
				ifLocalVar412 = _AlphaClipThreshold;
				float ifLocalVar411 = 0;
				if( AlphaToCoverage <= 0 )
				ifLocalVar411 = ifLocalVar412;
				else
				ifLocalVar411 = 0.0;
				float ClipCalc397 = ifLocalVar411;
				

				float3 BaseColor = Albedo74.xyz;
				float3 Emission = temp_cast_5;
				float Alpha = Alpha157;
				float AlphaClipThreshold = ClipCalc397;

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
			#define ASE_FOG 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_TRANSLUCENCY 1
			#define ASE_TRANSMISSION 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
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

			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _TRANSLUCENCY
			#pragma shader_feature _USE_TRANSMISSION
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature _SKYFOG_ON
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
				float4 ase_color : COLOR;
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
			float4 _RemapAlpha;
			float4 _ColorMaskRemap;
			float4 Color_C776E9F7;
			float4 _ColorMask;
			float4x4 _ProbeWorldToTexture;
			float4 _TranslucencyColor;
			float4 _TransmissionColor;
			float3 _ProbeVolumeSizeInv;
			float3 _ProbeVolumeMin;
			float _ShadowThreshold;
			float _AlphaClip;
			float _AlphaClipThreshold;
			float _Occlusion;
			float Vector1_5DA372D6;
			float _Metallic;
			float Vector1_b5167c4675424009928f6f8474af501f;
			float _Strength;
			float _OcclusionBackLit;
			int _Cullmode;
			float Vector1_2F70D75B;
			float _UseColorMask;
			float _Cutoff;
			float _TranslucencyNormalBias;
			float _TranslucencyNormalDistortion;
			float _TranslucencyDirect;
			float _TranslucencyAmbient;
			float _TranslucencyShadow;
			float _HueShift;
			float _TranslucencyScattering;
			float _UseRForAmbientOcclusion;
			float BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF;
			float Vector1_A0C32172;
			float Vector1_736F1270;
			float Vector1_F7C5A4BA;
			float Vector1_534F54C8;
			float _TranslucencyStrength;
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
			TEXTURE2D(Texture2D_3899621E);
			SAMPLER(sampler_Linear_Repeat_Aniso8);
			TEXTURE2D(Texture2D_363A5C4F);
			TEXTURE2D(_MainTex);
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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 temp_output_390_0 = saturate( v.ase_color );
				float4 break391 = temp_output_390_0;
				float ifLocalVar432 = 0;
				if( _UseRForAmbientOcclusion <= 0.5 )
				ifLocalVar432 = 1.0;
				else
				ifLocalVar432 = break391.r;
				float4 appendResult393 = (float4(break391.r , saturate( ifLocalVar432 ) , break391.b , 0.0));
				float4 vertexToFrag395 = appendResult393;
				float4 break394 = vertexToFrag395;
				float VCWindMaskR238 = break394.x;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float2 appendResult27 = (float2(ase_worldPos.x , ase_worldPos.z));
				float temp_output_33_0 = ( Vector1_736F1270 * _TimeParameters.x );
				float2 appendResult37 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 appendResult38 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 temp_output_41_0 = ( appendResult37 + ( appendResult38 * 0.5 ) );
				float2 ifLocalVar439 = 0;
				if( BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF <= 0.0 )
				ifLocalVar439 = appendResult37;
				else
				ifLocalVar439 = temp_output_41_0;
				float2 break49 = ( VCWindMaskR238 * ifLocalVar439 * Vector1_534F54C8 );
				float3 appendResult48 = (float3(break49.x , 0.0 , break49.y));
				float3 VertexPos46 = appendResult48;
				float3 vertexToFrag392 = VertexPos46;
				
				o.ase_texcoord5.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord5.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = vertexToFrag392;

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
				float4 ase_color : COLOR;
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
				o.ase_color = v.ase_color;
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
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
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

				float2 uvTexture2D_363A5C4F11 = IN.ase_texcoord5.xy;
				float3 unpack11 = UnpackNormalScale( SAMPLE_TEXTURE2D( Texture2D_363A5C4F, sampler_Linear_Repeat_Aniso8, uvTexture2D_363A5C4F11 ), Vector1_b5167c4675424009928f6f8474af501f );
				unpack11.z = lerp( 1, unpack11.z, saturate(Vector1_b5167c4675424009928f6f8474af501f) );
				float3 tex2DNode11 = unpack11;
				float3 switchResult103 = (((ase_vface>0)?(tex2DNode11):(( float3( 0,0,0 ) - tex2DNode11 ))));
				float3 NormalRes233 = switchResult103;
				
				float2 uv_MainTex8 = IN.ase_texcoord5.xy;
				float4 tex2DNode8 = SAMPLE_TEXTURE2D( _MainTex, sampler_Linear_Repeat_Aniso8, uv_MainTex8 );
				float Alpha157 = (0.0 + (tex2DNode8.a - _RemapAlpha.x) * (1.0 - 0.0) / (_RemapAlpha.y - _RemapAlpha.x));
				
				float ifLocalVar412 = 0;
				if( _AlphaClip <= 0 )
				ifLocalVar412 = 0.0;
				else
				ifLocalVar412 = _AlphaClipThreshold;
				float ifLocalVar411 = 0;
				if( AlphaToCoverage <= 0 )
				ifLocalVar411 = ifLocalVar412;
				else
				ifLocalVar411 = 0.99;
				float ClipCalc397 = ifLocalVar411;
				

				float3 Normal = NormalRes233;
				float Alpha = Alpha157;
				float AlphaClipThreshold = ClipCalc397;
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

			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#pragma multi_compile_fragment _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_TRANSLUCENCY 1
			#define ASE_TRANSMISSION 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
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

			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _TRANSLUCENCY
			#pragma shader_feature _USE_TRANSMISSION
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature _SKYFOG_ON
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
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_color : COLOR;
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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _RemapAlpha;
			float4 _ColorMaskRemap;
			float4 Color_C776E9F7;
			float4 _ColorMask;
			float4x4 _ProbeWorldToTexture;
			float4 _TranslucencyColor;
			float4 _TransmissionColor;
			float3 _ProbeVolumeSizeInv;
			float3 _ProbeVolumeMin;
			float _ShadowThreshold;
			float _AlphaClip;
			float _AlphaClipThreshold;
			float _Occlusion;
			float Vector1_5DA372D6;
			float _Metallic;
			float Vector1_b5167c4675424009928f6f8474af501f;
			float _Strength;
			float _OcclusionBackLit;
			int _Cullmode;
			float Vector1_2F70D75B;
			float _UseColorMask;
			float _Cutoff;
			float _TranslucencyNormalBias;
			float _TranslucencyNormalDistortion;
			float _TranslucencyDirect;
			float _TranslucencyAmbient;
			float _TranslucencyShadow;
			float _HueShift;
			float _TranslucencyScattering;
			float _UseRForAmbientOcclusion;
			float BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF;
			float Vector1_A0C32172;
			float Vector1_736F1270;
			float Vector1_F7C5A4BA;
			float Vector1_534F54C8;
			float _TranslucencyStrength;
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
			TEXTURE2D(Texture2D_3899621E);
			SAMPLER(sampler_Linear_Repeat_Aniso8);
			TEXTURE2D(_MainTex);
			TEXTURE2D(Texture2D_363A5C4F);
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
			
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}
			inline float3 SampleSHPixel381( float3 SH, float3 normalWS )
			{
				return SampleSHPixel( SH, normalWS );
			}
			
			inline float3 SampleLightmap380( float2 lightmapUV, float3 normalWS )
			{
				return SampleLightmap( lightmapUV, 0, normalWS );
			}
			
			float3x3 CastToFloat3x354_g2827( float3x3 Input )
			{
				return Input;
			}
			
			float3 MyCustomExpression6_g2827( float3 vertexPos, float4x4 ProbeWorldToTexture, float3 ProbeVolumeMin, float3 ProbeVolumeSizeInv )
			{
				float3 position = mul(ProbeWorldToTexture, float4(TransformObjectToWorld(vertexPos.xyz), 1.0f)).xyz;
				float3 texCoord = (position - ProbeVolumeMin.xyz) * ProbeVolumeSizeInv;
				return texCoord;
			}
			
			float3 SHEvalLinearL0L114_g2827( float3 worldNormal, float4 ProbeVolumeShR, float4 ProbeVolumeShG, float4 ProbeVolumeShB )
			{
				return SHEvalLinearL0L1(worldNormal, ProbeVolumeShR, ProbeVolumeShG,  ProbeVolumeShB);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 temp_output_390_0 = saturate( v.ase_color );
				float4 break391 = temp_output_390_0;
				float ifLocalVar432 = 0;
				if( _UseRForAmbientOcclusion <= 0.5 )
				ifLocalVar432 = 1.0;
				else
				ifLocalVar432 = break391.r;
				float4 appendResult393 = (float4(break391.r , saturate( ifLocalVar432 ) , break391.b , 0.0));
				float4 vertexToFrag395 = appendResult393;
				float4 break394 = vertexToFrag395;
				float VCWindMaskR238 = break394.x;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float2 appendResult27 = (float2(ase_worldPos.x , ase_worldPos.z));
				float temp_output_33_0 = ( Vector1_736F1270 * _TimeParameters.x );
				float2 appendResult37 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 appendResult38 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 temp_output_41_0 = ( appendResult37 + ( appendResult38 * 0.5 ) );
				float2 ifLocalVar439 = 0;
				if( BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF <= 0.0 )
				ifLocalVar439 = appendResult37;
				else
				ifLocalVar439 = temp_output_41_0;
				float2 break49 = ( VCWindMaskR238 * ifLocalVar439 * Vector1_534F54C8 );
				float3 appendResult48 = (float3(break49.x , 0.0 , break49.y));
				float3 VertexPos46 = appendResult48;
				float3 vertexToFrag392 = VertexPos46;
				
				o.ase_texcoord9 = vertexToFrag395;
				
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				float4x4 temp_output_28_0_g2827 = _ProbeWorldToTexture;
				float3x3 Input54_g2827 = 0;
				float3x3 localCastToFloat3x354_g2827 = CastToFloat3x354_g2827( Input54_g2827 );
				float3 vertexToFrag56_g2827 = mul( ase_worldNormal, localCastToFloat3x354_g2827 );
				o.ase_texcoord10.xyz = vertexToFrag56_g2827;
				float3 vertexPos6_g2827 = v.vertex.xyz;
				float4x4 ProbeWorldToTexture6_g2827 = temp_output_28_0_g2827;
				float3 ProbeVolumeMin6_g2827 = _ProbeVolumeMin;
				float3 ProbeVolumeSizeInv6_g2827 = _ProbeVolumeSizeInv;
				float3 localMyCustomExpression6_g2827 = MyCustomExpression6_g2827( vertexPos6_g2827 , ProbeWorldToTexture6_g2827 , ProbeVolumeMin6_g2827 , ProbeVolumeSizeInv6_g2827 );
				float3 vertexToFrag7_g2827 = localMyCustomExpression6_g2827;
				o.ase_texcoord11.xyz = vertexToFrag7_g2827;
				
				o.ase_texcoord8.xyz = v.texcoord.xyz;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.w = 0;
				o.ase_texcoord10.w = 0;
				o.ase_texcoord11.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = vertexToFrag392;

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
				float4 ase_color : COLOR;

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

				float3 appendResult316 = (float3(Color_C776E9F7.rgb));
				float2 uv_MainTex8 = IN.ase_texcoord8.xyz.xy;
				float4 tex2DNode8 = SAMPLE_TEXTURE2D( _MainTex, sampler_Linear_Repeat_Aniso8, uv_MainTex8 );
				float3 hsvTorgb300 = RGBToHSV( tex2DNode8.rgb );
				float temp_output_302_0 = ( _HueShift + hsvTorgb300.x );
				float temp_output_354_0 = ( hsvTorgb300.y * Vector1_2F70D75B );
				float3 appendResult405 = (float3(temp_output_302_0 , temp_output_354_0 , hsvTorgb300.z));
				float3 hsvTorgb357 = RGBToHSV( _ColorMask.rgb );
				float3 appendResult407 = (float3(hsvTorgb357.x , hsvTorgb357.y , ( hsvTorgb357.x + _HueShift )));
				float4 vertexToFrag395 = IN.ase_texcoord9;
				float4 break394 = vertexToFrag395;
				float VCColorMaskB240 = break394.z;
				float temp_output_8_0_g2834 = saturate( VCColorMaskB240 );
				float4 break5_g2834 = _ColorMaskRemap;
				float temp_output_32_0_g2834 = (break5_g2834.z + (temp_output_8_0_g2834 - break5_g2834.x) * (break5_g2834.w - break5_g2834.z) / (break5_g2834.y - break5_g2834.x));
				float temp_output_270_0 = temp_output_32_0_g2834;
				float3 appendResult365 = (float3(temp_output_270_0 , temp_output_270_0 , temp_output_270_0));
				float3 lerpResult356 = lerp( appendResult405 , appendResult407 , saturate( ( appendResult365 * _Strength ) ));
				float3 break360 = lerpResult356;
				float ifLocalVar435 = 0;
				if( _UseColorMask <= 0.5 )
				ifLocalVar435 = temp_output_302_0;
				else
				ifLocalVar435 = break360.x;
				float ifLocalVar437 = 0;
				if( _UseColorMask <= 0.5 )
				ifLocalVar437 = temp_output_354_0;
				else
				ifLocalVar437 = break360.y;
				float3 hsvTorgb301 = HSVToRGB( float3(ifLocalVar435,ifLocalVar437,hsvTorgb300.z) );
				float4 appendResult312 = (float4(( appendResult316 * hsvTorgb301 ) , tex2DNode8.a));
				float4 Albedo74 = appendResult312;
				
				float2 uvTexture2D_363A5C4F11 = IN.ase_texcoord8.xyz.xy;
				float3 unpack11 = UnpackNormalScale( SAMPLE_TEXTURE2D( Texture2D_363A5C4F, sampler_Linear_Repeat_Aniso8, uvTexture2D_363A5C4F11 ), Vector1_b5167c4675424009928f6f8474af501f );
				unpack11.z = lerp( 1, unpack11.z, saturate(Vector1_b5167c4675424009928f6f8474af501f) );
				float3 tex2DNode11 = unpack11;
				float3 switchResult103 = (((ase_vface>0)?(tex2DNode11):(( float3( 0,0,0 ) - tex2DNode11 ))));
				float3 NormalRes233 = switchResult103;
				
				int BackLit142 = 0;
				float3 temp_cast_5 = BackLit142;
				
				float Smoothness88 = Vector1_5DA372D6;
				
				float VCAmbientOcclusionG239 = break394.y;
				float3 temp_cast_6 = (VCAmbientOcclusionG239).xxx;
				float temp_output_2_0_g1416 = _Occlusion;
				float temp_output_3_0_g1416 = ( 1.0 - temp_output_2_0_g1416 );
				float3 appendResult7_g1416 = (float3(temp_output_3_0_g1416 , temp_output_3_0_g1416 , temp_output_3_0_g1416));
				float3 OcclusionRes252 = ( ( temp_cast_6 * temp_output_2_0_g1416 ) + appendResult7_g1416 );
				
				float Alpha157 = (0.0 + (tex2DNode8.a - _RemapAlpha.x) * (1.0 - 0.0) / (_RemapAlpha.y - _RemapAlpha.x));
				
				float ifLocalVar412 = 0;
				if( _AlphaClip <= 0 )
				ifLocalVar412 = 0.0;
				else
				ifLocalVar412 = _AlphaClipThreshold;
				float ifLocalVar411 = 0;
				if( AlphaToCoverage <= 0 )
				ifLocalVar411 = ifLocalVar412;
				else
				ifLocalVar411 = 0.0;
				float ClipCalc397 = ifLocalVar411;
				
				float ShadowThreshold335 = _ShadowThreshold;
				
				float3 SH381 = half4(0,0,0,0).xyz;
				float3 normalWS381 = WorldNormal;
				float3 localSampleSHPixel381 = SampleSHPixel381( SH381 , normalWS381 );
				float2 lightmapUV380 = half4(0,0,0,0).xy;
				float3 normalWS380 = WorldNormal;
				float3 localSampleLightmap380 = SampleLightmap380( lightmapUV380 , normalWS380 );
				#ifdef LIGHTMAP_ON
				float3 staticSwitch379 = localSampleLightmap380;
				#else
				float3 staticSwitch379 = localSampleSHPixel381;
				#endif
				float3 vertexToFrag56_g2827 = IN.ase_texcoord10.xyz;
				float3 worldNormal14_g2827 = vertexToFrag56_g2827;
				float3 vertexToFrag7_g2827 = IN.ase_texcoord11.xyz;
				float4 ProbeVolumeShR14_g2827 = SAMPLE_TEXTURE3D( _ProbeVolumeShR, sampler_Linear_Clamp, ( vertexToFrag7_g2827 + float3( float2( 0,0 ) ,  0.0 ) ) );
				float4 ProbeVolumeShG14_g2827 = SAMPLE_TEXTURE3D( _ProbeVolumeShG, sampler_Linear_Clamp, ( vertexToFrag7_g2827 + float3( float2( 0,0 ) ,  0.0 ) ) );
				float4 ProbeVolumeShB14_g2827 = SAMPLE_TEXTURE3D( _ProbeVolumeShB, sampler_Linear_Clamp, ( vertexToFrag7_g2827 + float3( float2( 0,0 ) ,  0.0 ) ) );
				float3 localSHEvalLinearL0L114_g2827 = SHEvalLinearL0L114_g2827( worldNormal14_g2827 , ProbeVolumeShR14_g2827 , ProbeVolumeShG14_g2827 , ProbeVolumeShB14_g2827 );
				#ifdef _PROBEVOLUME_ON
				float3 staticSwitch20_g2827 = localSHEvalLinearL0L114_g2827;
				#else
				float3 staticSwitch20_g2827 = staticSwitch379;
				#endif
				float3 BakedGI217 = staticSwitch20_g2827;
				
				float3 temp_cast_16 = (VCAmbientOcclusionG239).xxx;
				float temp_output_2_0_g1420 = _OcclusionBackLit;
				float temp_output_3_0_g1420 = ( 1.0 - temp_output_2_0_g1420 );
				float3 appendResult7_g1420 = (float3(temp_output_3_0_g1420 , temp_output_3_0_g1420 , temp_output_3_0_g1420));
				float3 temp_output_266_0 = ( ( temp_cast_16 * temp_output_2_0_g1420 ) + appendResult7_g1420 );
				float4 TransmissionRes338 = ( float4( temp_output_266_0 , 0.0 ) * _TransmissionColor * Albedo74 );
				
				float4 TranslucencyRes342 = ( float4( temp_output_266_0 , 0.0 ) * _TranslucencyColor );
				

				float3 BaseColor = Albedo74.xyz;
				float3 Normal = NormalRes233;
				float3 Emission = temp_cast_5;
				float3 Specular = 0.5;
				float Metallic = _Metallic;
				float Smoothness = Smoothness88;
				float Occlusion = OcclusionRes252.x;
				float Alpha = Alpha157;
				float AlphaClipThreshold = ClipCalc397;
				float AlphaClipThresholdShadow = ShadowThreshold335;
				float3 BakedGI = BakedGI217;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = TransmissionRes338.rgb;
				float3 Translucency = TranslucencyRes342.rgb;

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

			#define _NORMAL_DROPOFF_TS 1
			#define ASE_FOG 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_TRANSLUCENCY 1
			#define ASE_TRANSMISSION 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
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

			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _TRANSLUCENCY
			#pragma shader_feature _USE_TRANSMISSION
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _RemapAlpha;
			float4 _ColorMaskRemap;
			float4 Color_C776E9F7;
			float4 _ColorMask;
			float4x4 _ProbeWorldToTexture;
			float4 _TranslucencyColor;
			float4 _TransmissionColor;
			float3 _ProbeVolumeSizeInv;
			float3 _ProbeVolumeMin;
			float _ShadowThreshold;
			float _AlphaClip;
			float _AlphaClipThreshold;
			float _Occlusion;
			float Vector1_5DA372D6;
			float _Metallic;
			float Vector1_b5167c4675424009928f6f8474af501f;
			float _Strength;
			float _OcclusionBackLit;
			int _Cullmode;
			float Vector1_2F70D75B;
			float _UseColorMask;
			float _Cutoff;
			float _TranslucencyNormalBias;
			float _TranslucencyNormalDistortion;
			float _TranslucencyDirect;
			float _TranslucencyAmbient;
			float _TranslucencyShadow;
			float _HueShift;
			float _TranslucencyScattering;
			float _UseRForAmbientOcclusion;
			float BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF;
			float Vector1_A0C32172;
			float Vector1_736F1270;
			float Vector1_F7C5A4BA;
			float Vector1_534F54C8;
			float _TranslucencyStrength;
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
			TEXTURE2D(Texture2D_3899621E);
			SAMPLER(sampler_Linear_Repeat_Aniso8);
			TEXTURE2D(_MainTex);
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

				float4 temp_output_390_0 = saturate( v.ase_color );
				float4 break391 = temp_output_390_0;
				float ifLocalVar432 = 0;
				if( _UseRForAmbientOcclusion <= 0.5 )
				ifLocalVar432 = 1.0;
				else
				ifLocalVar432 = break391.r;
				float4 appendResult393 = (float4(break391.r , saturate( ifLocalVar432 ) , break391.b , 0.0));
				float4 vertexToFrag395 = appendResult393;
				float4 break394 = vertexToFrag395;
				float VCWindMaskR238 = break394.x;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float2 appendResult27 = (float2(ase_worldPos.x , ase_worldPos.z));
				float temp_output_33_0 = ( Vector1_736F1270 * _TimeParameters.x );
				float2 appendResult37 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 appendResult38 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 temp_output_41_0 = ( appendResult37 + ( appendResult38 * 0.5 ) );
				float2 ifLocalVar439 = 0;
				if( BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF <= 0.0 )
				ifLocalVar439 = appendResult37;
				else
				ifLocalVar439 = temp_output_41_0;
				float2 break49 = ( VCWindMaskR238 * ifLocalVar439 * Vector1_534F54C8 );
				float3 appendResult48 = (float3(break49.x , 0.0 , break49.y));
				float3 VertexPos46 = appendResult48;
				float3 vertexToFrag392 = VertexPos46;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = vertexToFrag392;

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
				float4 ase_color : COLOR;
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
				o.ase_color = v.ase_color;
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
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
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

				float2 uv_MainTex8 = IN.ase_texcoord.xy;
				float4 tex2DNode8 = SAMPLE_TEXTURE2D( _MainTex, sampler_Linear_Repeat_Aniso8, uv_MainTex8 );
				float Alpha157 = (0.0 + (tex2DNode8.a - _RemapAlpha.x) * (1.0 - 0.0) / (_RemapAlpha.y - _RemapAlpha.x));
				
				float ifLocalVar412 = 0;
				if( _AlphaClip <= 0 )
				ifLocalVar412 = 0.0;
				else
				ifLocalVar412 = _AlphaClipThreshold;
				float ifLocalVar411 = 0;
				if( AlphaToCoverage <= 0 )
				ifLocalVar411 = ifLocalVar412;
				else
				ifLocalVar411 = 0.0;
				float ClipCalc397 = ifLocalVar411;
				

				surfaceDescription.Alpha = Alpha157;
				surfaceDescription.AlphaClipThreshold = ClipCalc397;

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
			#define ASE_FOG 1
			#define _ASE_DEBUGVISUALS 1
			#define _ALPHATEST_SHADOW_ON 1
			#define ASE_TRANSLUCENCY 1
			#define ASE_TRANSMISSION 1
			#define _ASE_TINT 1
			#define _ASE_TONEMAPPING 1
			#define ASE_BAKEDGI 1
			#define ASE_SHADOWMASK 1
			#define ASE_FOGCOLOR 1
			#define _ASE_ALPHATEST 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 120115
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

			#pragma shader_feature_local _DEBUGVISUALS_ON
			#pragma shader_feature _TRANSLUCENCY
			#pragma shader_feature _USE_TRANSMISSION
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature _SKYFOG_ON
			#pragma multi_compile_local __ _PROBEVOLUME_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _RemapAlpha;
			float4 _ColorMaskRemap;
			float4 Color_C776E9F7;
			float4 _ColorMask;
			float4x4 _ProbeWorldToTexture;
			float4 _TranslucencyColor;
			float4 _TransmissionColor;
			float3 _ProbeVolumeSizeInv;
			float3 _ProbeVolumeMin;
			float _ShadowThreshold;
			float _AlphaClip;
			float _AlphaClipThreshold;
			float _Occlusion;
			float Vector1_5DA372D6;
			float _Metallic;
			float Vector1_b5167c4675424009928f6f8474af501f;
			float _Strength;
			float _OcclusionBackLit;
			int _Cullmode;
			float Vector1_2F70D75B;
			float _UseColorMask;
			float _Cutoff;
			float _TranslucencyNormalBias;
			float _TranslucencyNormalDistortion;
			float _TranslucencyDirect;
			float _TranslucencyAmbient;
			float _TranslucencyShadow;
			float _HueShift;
			float _TranslucencyScattering;
			float _UseRForAmbientOcclusion;
			float BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF;
			float Vector1_A0C32172;
			float Vector1_736F1270;
			float Vector1_F7C5A4BA;
			float Vector1_534F54C8;
			float _TranslucencyStrength;
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
			TEXTURE2D(Texture2D_3899621E);
			SAMPLER(sampler_Linear_Repeat_Aniso8);
			TEXTURE2D(_MainTex);
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

				float4 temp_output_390_0 = saturate( v.ase_color );
				float4 break391 = temp_output_390_0;
				float ifLocalVar432 = 0;
				if( _UseRForAmbientOcclusion <= 0.5 )
				ifLocalVar432 = 1.0;
				else
				ifLocalVar432 = break391.r;
				float4 appendResult393 = (float4(break391.r , saturate( ifLocalVar432 ) , break391.b , 0.0));
				float4 vertexToFrag395 = appendResult393;
				float4 break394 = vertexToFrag395;
				float VCWindMaskR238 = break394.x;
				float3 ase_worldPos = TransformObjectToWorld( (v.vertex).xyz );
				float2 appendResult27 = (float2(ase_worldPos.x , ase_worldPos.z));
				float temp_output_33_0 = ( Vector1_736F1270 * _TimeParameters.x );
				float2 appendResult37 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_A0C32172 )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 appendResult38 = (float2(UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).x , UnpackNormalScale( SAMPLE_TEXTURE2D_LOD( Texture2D_3899621E, sampler_Linear_Repeat_Aniso8, (( appendResult27 / Vector1_F7C5A4BA )*1.0 + temp_output_33_0), 0.0 ), 1.0 ).y));
				float2 temp_output_41_0 = ( appendResult37 + ( appendResult38 * 0.5 ) );
				float2 ifLocalVar439 = 0;
				if( BOOLEAN_CFF036ECFE8941F8B02A5D46ABC1B3CF <= 0.0 )
				ifLocalVar439 = appendResult37;
				else
				ifLocalVar439 = temp_output_41_0;
				float2 break49 = ( VCWindMaskR238 * ifLocalVar439 * Vector1_534F54C8 );
				float3 appendResult48 = (float3(break49.x , 0.0 , break49.y));
				float3 VertexPos46 = appendResult48;
				float3 vertexToFrag392 = VertexPos46;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = vertexToFrag392;

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
				float4 ase_color : COLOR;
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
				o.ase_color = v.ase_color;
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
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
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

				float2 uv_MainTex8 = IN.ase_texcoord.xy;
				float4 tex2DNode8 = SAMPLE_TEXTURE2D( _MainTex, sampler_Linear_Repeat_Aniso8, uv_MainTex8 );
				float Alpha157 = (0.0 + (tex2DNode8.a - _RemapAlpha.x) * (1.0 - 0.0) / (_RemapAlpha.y - _RemapAlpha.x));
				
				float ifLocalVar412 = 0;
				if( _AlphaClip <= 0 )
				ifLocalVar412 = 0.0;
				else
				ifLocalVar412 = _AlphaClipThreshold;
				float ifLocalVar411 = 0;
				if( AlphaToCoverage <= 0 )
				ifLocalVar411 = ifLocalVar412;
				else
				ifLocalVar411 = 0.0;
				float ClipCalc397 = ifLocalVar411;
				

				surfaceDescription.Alpha = Alpha157;
				surfaceDescription.AlphaClipThreshold = ClipCalc397;

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
	
	Fallback "Hidden/InternalErrorShader"
}
