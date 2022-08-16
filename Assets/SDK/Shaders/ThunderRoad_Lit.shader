////////////////////////////////////////
// Generated with Better Shaders
//
// Auto-generated shader code, don't hand edit!
//
//   Unity Version: 2020.3.18f1
//   Render Pipeline: URP2020
//   Platform: WindowsEditor
////////////////////////////////////////


Shader "ThunderRoad/Lit"
{
   Properties
   {
      [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
      [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
      [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
      




	[Toggle]_DebugView("Debug View", Float) = 0

	//[BetterHeader(ThunderRoad Lit)]
	// Blending state
	[Enum(Opaque,0,Transparent,1)]_Surface("Surface", Float) = 0
    [Enum(Alpha,0,Premultiply,1,Additive,2,Multiply,3)]_Blend("Transparent Blend Mode", Float) = 0.0
    [HideInInspector] _SrcBlend("__src", Float) = 1.0
    [HideInInspector] _DstBlend("__dst", Float) = 0.0
    [HideInInspector] _ZWrite("__zw", Float) = 1.0
	[Enum(Off,0,Front,1,Back,2)]_CullMode("Culling Mode", Float) = 2
	[Toggle]_AlphaClip("Alpha Clipping", Float) = 0
	[ShowIfDrawer(_AlphaClip)]_Cutoff("Alpha Threshold", Range(0,1)) = 0
	_AlphaStrength("Alpha Strength", Range(0,1)) = 1
	[Toggle]_ReceiveShadows("Receive Shadows", Float) = 1.0

	_BaseMap("Base Map", 2D) = "white" {}
	_BaseColor ("Base Color", Color) = (1, 1, 1, 1)

	[Normal][NoScaleOffset]_BumpMap("Normal", 2D) = "bump" {}
	_NormalStrength("Normal Strength", Range(0,2)) = 1

	[NoScaleOffset]_MetallicGlossMap("Metallic (R) Occlusion (G) Emission Mask (B) Smoothness (A)", 2D) = "black" {}
	_Smoothness("Smoothness", Range(0,1)) = 1
	_OcclusionStrength("Occlusion Strength", Range(0,1)) = 1
	[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
	[Toggle(_EMISSION)] _UseEmission ("Use Emission Map", Float) = 0
    [ShowIfDrawer(_UseEmission)][NoScaleOffset]_EmissionMap("Emission Map", 2D) = "black" {}

	[Toggle(_DETAIL)]_UseDetailMap("Use Detail Map", Float) = 0
	[ShowIfDrawer(_UseDetailMap)]_DetailAlbedoMap("Detail Albedo Map", 2D) = "white" {}
	[ShowIfDrawer(_UseDetailMap)]_DetailAlbedoMapScale("Detail Albedo Map Scale", Range(0,2)) = 1
	[ShowIfDrawer(_UseDetailMap)][Normal][NoScaleOffset]_DetailNormalMap("Detail Normal Map", 2D) = "bump" {}
	[ShowIfDrawer(_UseDetailMap)]_DetailNormalMapScale("Detail Normal Map Scale", Range(0,2)) = 1
	[ShowIfDrawer(_UseDetailMap)]_DetailWeightOverDistance("Weight Over Distance. (Start Distance, Start Weight, End Distance, End Weight)", Vector) = (1,1,1,1)

	// Editmode props
    _QueueOffset("Queue offset", Float) = 0.0

	// ObsoleteProperties
    [HideInInspector] _MainTex("BaseMap", 2D) = "white" {} //need this for lightmapper.


	/*[BetterHeaderToggleKeywordDrawer(_COLORMASK_ON)]*/ [Toggle] _UseColorMask("Use Color Mask", Float) = 0
	[ShowIfDrawer(_UseColorMask)][NoScaleOffset] _ColorMask("Color Tint Mask", 2D) = "black" {}
	[ShowIfDrawer(_UseColorMask)] _Tint0("Tint0 (R) Color", Color) = (1,1,1,1)
	[ShowIfDrawer(_UseColorMask)] _Tint1("Tint1 (G) Color", Color) = (1,1,1,1)
	[ShowIfDrawer(_UseColorMask)] _Tint2("Tint2 (B) Color", Color) = (1,1,1,1)
	[ShowIfDrawer(_UseColorMask)] _Tint3("Tint3 (A) Color", Color) = (1,1,1,1)


	/*[BetterHeaderToggleKeywordDrawer(_REVEALLAYERS)]*/ [Toggle] _UseReveal("Reveal Layers", Float) = 0
	[ShowIfDrawer(_UseReveal)][NoScaleOffset]_RevealMask("Reveal Mask", 2D) = "black" {}
	[ShowIfDrawer(_UseReveal)][NoScaleOffset]_LayerMask("Layer Mask (Optional)", 2D) = "white" {}
	[ShowIfDrawer(_UseReveal)]_LayerSurfaceExp("Layer Surface Exponents (Albedo, Normal, Metallic, Smoothness)", Vector) = (1,1,1,1)

	//[GroupFoldout(Layer0 Red)]
	[Group(Layer0 Red)]_Layer0("Layer0 (R)", 2D) = "black" {}
	[Group(Layer0 Red)][Normal][NoScaleOffset]_Layer0NormalMap("Layer0 (R) Normal", 2D) = "bump" {}
	[Group(Layer0 Red)]_Layer0NormalStrength("Layer0 (R) Normal Strength", Range(0,2)) = 1
	[Group(Layer0 Red)]_Layer0Smoothness("Layer0 (R) Smoothness", Range(0,1)) = 0.5
	[Group(Layer0 Red)]_Layer0Metallic("Layer0 (R) Metallic", Range(0,1)) = 0

	//[GroupFoldout(Layer1 Green)]
	[Group(Layer1 Green)]_Layer1("Layer1 (G)", 2D) = "black" {}
	[Group(Layer1 Green)][Normal][NoScaleOffset]_Layer1NormalMap("Layer1 (G) Normal", 2D) = "bump" {}
	[Group(Layer1 Green)]_Layer1NormalStrength("Layer1 (G) Normal Strength", Range(0,2)) = 1
	[Group(Layer1 Green)]_Layer1Smoothness("Layer1 (G) Smoothness", Range(0,1)) = 0.5
	[Group(Layer1 Green)]_Layer1Metallic("Layer1 (G) Metallic", Range(0,1)) = 0

	//[GroupFoldout(Layer2 Blue)]
	[Group(Layer2 Blue)]_Layer2Height("Layer2 (B) Height", Range(-20,20)) = -1

	//[GroupFoldout(Layer3 Alpha)]
	[Group(Layer3 Alpha)][HDR]_Layer3EmissionColor("Layer3 (A) Emission Color", Color) = (0,0,0,1)


    /*[BetterHeaderToggleKeywordDrawer(_VERTEXOCCLUSION_ON)]*/ [Toggle] _UseVertexOcclusion("Use Vertex Occlusion", Float) = 0
    [ShowIfDrawer(_UseVertexOcclusion)] _Bitmask("Vertex Occlusion Bitmask", Int) = 0


    /*[BetterHeaderToggleKeywordDrawer(_PROBEVOLUME_ON)]*/ [Toggle] _UseProbeVolume("Use Probe Volume", Float) = 0
    [ShowIfDrawer(_UseProbeVolume)][NoScaleOffset]_ProbeVolumeShR("Probe Volume SH Red", 3D) = "black" {}
    [ShowIfDrawer(_UseProbeVolume)][NoScaleOffset]_ProbeVolumeShG("Probe Volume SH Green", 3D) = "black" {}
    [ShowIfDrawer(_UseProbeVolume)][NoScaleOffset]_ProbeVolumeShB("Probe Volume SH Blue", 3D) = "black" {}
    [ShowIfDrawer(_UseProbeVolume)][NoScaleOffset]_ProbeVolumeOcc("Probe Volume Occlusion", 3D) = "black" {}

    [HideInInspector]_ProbeVolumeMin("Probe Volume Min", Vector) = (0,0,0)
    [HideInInspector]_ProbeVolumeSizeInv("Probe Volume Size Inverse", Vector) = (0,0,0)


   }
   SubShader
   {
      Tags { "RenderPipeline"="UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "Lit" "Queue" = "Geometry" }

      

      
        Pass
        {
            Name "Universal Forward"
            Tags 
            { 
                "LightMode" = "UniversalForward"
            }
            Cull Back
            Blend One Zero
            ZTest LEqual
            ZWrite On

            Blend One Zero, One Zero
Cull Back
ZTest LEqual
ZWrite On

               Blend[_SrcBlend][_DstBlend]
   ZWrite[_ZWrite]
   Cull [_CullMode]


            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
        
            // Keywords
            #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            // GraphKeywords: <None>

            #define SHADER_PASS SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define _PASSFORWARD 1
            

            


   #pragma shader_feature_local_fragment _ALPHATEST_ON
   #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
   #pragma shader_feature_local_fragment _ _DETAIL
   #pragma shader_feature_local_fragment _ _EMISSION


   //#pragma multi_compile_local_fragment _ _COLORMASK_ON
   #pragma shader_feature_local_fragment _ _COLORMASK_ON


   #pragma multi_compile_local_fragment _ _REVEALLAYERS
   //#pragma shader_feature_local_fragment _ _REVEALLAYERS


   #pragma multi_compile_local_vertex _ _VERTEXOCCLUSION_ON


   #pragma multi_compile_local _ _PROBEVOLUME_ON
   //#pragma shader_feature_local _ _PROBEVOLUME_ON

   #if defined(_PROBEVOLUME_ON)
       #define _OVERRIDE_BAKEDGI
   #endif


   #define _URP 1


            // this has to be here or specular color will be ignored. Not in SG code
            #if _SIMPLELIT
               #define _SPECULAR_COLOR
            #endif


            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"


        

               #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
     
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D

      

      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCCOORD3;
         // float4 texcoord1 : TEXCCOORD4;
         // float4 texcoord2 : TEXCCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
          float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
          float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif
            float4 fogFactorAndVertexLight : TEXCOORD10;
            float4 shadowCoord : TEXCOORD11;
         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
      };


         
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;
            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
	float2 baseuv;


                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // uint vertexID : SV_VertexID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // would love to strip these, but they are used in certain
               // combinations of the lighting system, and may be used
               // by the user as well, so no easy way to strip them.

               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetObjectToWorldMatrix()
               #undef GetWorldToObjectMatrix()
               #undef GetWorldToViewMatrix()
               #undef UNITY_MATRIX_I_V
               #undef UNITY_MATRIX_P
               #undef GetWorldToHClipMatrix()
               #undef GetObjectToWorldMatrix()V
               #undef UNITY_MATRIX_T_MV
               #undef UNITY_MATRIX_IT_MV
               #undef GetObjectToWorldMatrix()VP

               #define GetObjectToWorldMatrix()     unity_ObjectToWorld
               #define GetWorldToObjectMatrix()   unity_WorldToObject
               #define GetWorldToViewMatrix()     unity_MatrixV
               #define UNITY_MATRIX_I_V   unity_MatrixInvV
               #define GetViewToHClipMatrix()     OptimizeProjectionMatrix(glstate_matrix_projection)
               #define GetWorldToHClipMatrix()    unity_MatrixVP
               #define GetObjectToWorldMatrix()V    mul(GetWorldToViewMatrix(), GetObjectToWorldMatrix())
               #define UNITY_MATRIX_T_MV  transpose(GetObjectToWorldMatrix()V)
               #define UNITY_MATRIX_IT_MV transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V))
               #define GetObjectToWorldMatrix()VP   mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix())


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }


            
         CBUFFER_START(UnityPerMaterial)

            



	half _AlphaClip;
	half _Cutoff;
	half _AlphaStrength;

	float4 _BaseMap_ST;
	half4 _BaseColor;
	half _NormalStrength;
	half _Smoothness;
	half _OcclusionStrength;
	float4 _EmissionColor;

	float4 _DetailAlbedoMap_ST;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
	float4 _DetailWeightOverDistance;


	half4 _Tint0;
	half4 _Tint1;
	half4 _Tint2;
	half4 _Tint3;


	float4 _RevealMask_TexelSize;
	float4 _LayerSurfaceExp;

	half _Layer0NormalStrength;
	half _Layer0Smoothness;
	half _Layer0Metallic;
	half _Layer1NormalStrength;
	half _Layer1Smoothness;
	half _Layer1Metallic;
	half _Layer2Height;
	float4 _Layer3EmissionColor;

	float4 _Layer0_ST;
	float4 _Layer1_ST;


	int _Bitmask;


    float4x4 _ProbeWorldToTexture;
    float3 _ProbeVolumeMin;
    float3 _ProbeVolumeSizeInv;	



         CBUFFER_END

         

         

         #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()


    float Dither8x8Bayer( int x, int y )
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
        return dither[r] / 64; 
    }

    void ApplyDitherCrossFadeVSP(float2 vpos, float fadeValue)
    {
        float dither = Dither8x8Bayer( fmod(vpos.x, 8), fmod(vpos.y, 8) );
        float sgn = fadeValue > 0 ? 1.0f : -1.0f;
        clip(dither - (1-fadeValue) * sgn);
    }
    

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
		#if LOD_FADE_CROSSFADE
            float4 screenPosNorm = d.screenPos / d.screenPos.w;
            screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;
            float2 clipScreen = screenPosNorm.xy * _ScreenParams.xy;
            ApplyDitherCrossFadeVSP(clipScreen,unity_LODFade.x);
        #endif
	}




   TEXTURE2D(_BaseMap);
   SAMPLER(sampler_BaseMap);
   TEXTURE2D(_BumpMap);
   SAMPLER(sampler_BumpMap);
   TEXTURE2D(_MetallicGlossMap);
   SAMPLER(sampler_MetallicGlossMap);

   #if defined(_EMISSION)
   TEXTURE2D(_EmissionMap);
   SAMPLER(sampler_EmissionMap);
   #endif

   #if defined(_DETAIL)
   TEXTURE2D(_DetailAlbedoMap);
   SAMPLER(sampler_DetailAlbedoMap);
   TEXTURE2D(_DetailNormalMap);
   SAMPLER(sampler_DetailNormalMap);
   #endif

	float4 Unity_Blend_Overlay(float4 Base, float4 Blend, float Opacity)
	{
		float4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
		float4 result2 = 2.0 * Base * Blend;
		float4 zeroOrOne = step(Base, 0.5);
		float4 Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
		return lerp(Base, Out, Opacity);
	}

	float3 Unity_NormalBlend_Reoriented(float3 A, float3 B)
	{
		float3 t = A.xyz + float3(0.0, 0.0, 1.0);
		float3 u = B.xyz * float3(-1.0, -1.0, 1.0);
		return ((t / t.z) * dot(t, u) - u);
	}

	void Ext_SurfaceFunction1 (inout Surface o, inout ShaderData d)
	{
		float2 uv = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
		d.blackboard.baseuv = uv;
		half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);

		o.Albedo = albedo.rgb * _BaseColor.rgb;

		#if defined(_ALPHATEST_ON)
			clip(albedo.a - _Cutoff);
		#endif

		o.Alpha = albedo.a * _AlphaStrength;
		o.Normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uv), _NormalStrength);

		float4 mask = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv);
        o.Metallic = mask.r;
        o.Occlusion = LerpWhiteTo(mask.g, _OcclusionStrength);
		float emissionMask = mask.b;
        o.Smoothness = mask.a * _Smoothness;

		half3 emission = 0;
		#if defined(_EMISSION)
           o.Emission = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, uv).rgb * _EmissionColor.rgb * emissionMask;
		#else
		   o.Emission = _EmissionColor.rgb * emissionMask;
		#endif

		#if defined(_DETAIL) && !defined(SHADER_API_MOBILE)
			float weight = 1.0;
		    if (_DetailWeightOverDistance.y + _DetailWeightOverDistance.w < 2)
            {
               float camDist = distance(d.worldSpacePosition, _WorldSpaceCameraPos);
               weight = lerp(_DetailWeightOverDistance.y, _DetailWeightOverDistance.w, saturate((camDist-_DetailWeightOverDistance.x) / _DetailWeightOverDistance.z));
            }

			float2 detailuv = d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			o.Albedo = Unity_Blend_Overlay(albedo, SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailuv), _DetailAlbedoMapScale * weight).rgb * _BaseColor.rgb;
			o.Normal = Unity_NormalBlend_Reoriented(o.Normal, UnpackScaleNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailuv), _DetailNormalMapScale * weight));
		#endif

		o.Height = albedo.a;
	}




#if defined(_COLORMASK_ON)
	TEXTURE2D(_ColorMask);
    SAMPLER(sampler_ColorMask);
#endif

	void Ext_SurfaceFunction2 (inout Surface o, inout ShaderData d)
	{
		#if defined(_COLORMASK_ON)
		float4 colorMask = SAMPLE_TEXTURE2D(_ColorMask, sampler_ColorMask, d.blackboard.baseuv);
		float t = colorMask.r + colorMask.g + colorMask.b + (1.0 - colorMask.a);
		float3 c = ((colorMask.r * _Tint0) + (colorMask.g * _Tint1) + (colorMask.b * _Tint2) + ((1.0 - colorMask.a) * _Tint3)).rgb * o.Albedo;
		o.Albedo = lerp(o.Albedo, c, t);
		#endif
	}




#if defined(_REVEALLAYERS)
   TEXTURE2D(_RevealMask);
   SAMPLER(sampler_RevealMask);
   TEXTURE2D(_LayerMask);
   SAMPLER(sampler_LayerMask);
   TEXTURE2D(_Layer0);
   SAMPLER(sampler_Layer0);
   TEXTURE2D(_Layer0NormalMap);
   SAMPLER(sampler_Layer0NormalMap);
   TEXTURE2D(_Layer1);
   SAMPLER(sampler_Layer1);
   TEXTURE2D(_Layer1NormalMap);
   SAMPLER(sampler_Layer1NormalMap);

	float3 RevealMaskNormalCrossFilter(float2 uv, float height, float2 texelSize)
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
#endif

	void Ext_SurfaceFunction3 (inout Surface o, inout ShaderData d)
	{
		#if defined(_REVEALLAYERS)
		
		half4 revealMask = SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, d.blackboard.baseuv);
		half4 layerMask = SAMPLE_TEXTURE2D(_LayerMask, sampler_LayerMask, d.blackboard.baseuv);

		float2 layer0uv = d.texcoord0.xy * _Layer0_ST.xy + _Layer0_ST.zw;
		float2 layer1uv = d.texcoord0.xy * _Layer1_ST.xy + _Layer1_ST.zw;

		half4 layer0col = SAMPLE_TEXTURE2D(_Layer0, sampler_Layer0, layer0uv);
		float3 layer0normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_Layer0NormalMap, sampler_Layer0NormalMap, layer0uv), _NormalStrength);

		float rt = saturate(revealMask.r * layerMask.r);
		o.Albedo = lerp(o.Albedo, layer0col.rgb, pow(rt, _LayerSurfaceExp.x));
		o.Normal = lerp(o.Normal, layer0normal, pow(rt, _LayerSurfaceExp.y));
		o.Metallic = lerp(o.Metallic, _Layer0Metallic, pow(rt, _LayerSurfaceExp.z));
		o.Smoothness = lerp(o.Smoothness, _Layer0Smoothness, pow(rt, _LayerSurfaceExp.w));

		half4 layer1col = SAMPLE_TEXTURE2D(_Layer1, sampler_Layer1, layer1uv);
		float3 layer1normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_Layer1NormalMap, sampler_Layer1NormalMap, layer1uv), _NormalStrength);

		float gt = saturate(revealMask.g * layerMask.g);
		o.Albedo = lerp(o.Albedo, layer1col.rgb, pow(gt, _LayerSurfaceExp.x));
		o.Normal = lerp(o.Normal, layer1normal, pow(gt, _LayerSurfaceExp.y));
		o.Metallic = lerp(o.Metallic, _Layer1Metallic, pow(gt, _LayerSurfaceExp.z));
		o.Smoothness = lerp(o.Smoothness, _Layer1Smoothness, pow(gt, _LayerSurfaceExp.w));

		float bt = saturate(revealMask.b * layerMask.b);
		float height = _Layer2Height * bt;
		float3 heightNormal = RevealMaskNormalCrossFilter(d.blackboard.baseuv, height, _RevealMask_TexelSize.xy);
		o.Normal = normalize(float3(o.Normal.rg + heightNormal.rg, o.Normal.b * heightNormal.b)); //Blend normal

		float at = saturate(revealMask.a * layerMask.a);
		o.Emission = ((1.0 - at) * o.Emission.rgb) + (at * _Layer3EmissionColor.rgb);

		#endif
	}




    void Ext_ModifyVertex4 (inout VertexData v, inout ExtraV2F d)
    {
        #if defined(_VERTEXOCCLUSION_ON)
        if(_Bitmask & (int)v.texcoord1.x)
        {
            v.vertex *= (0.0 / 0.0);
        }
        #endif
    }




    #if defined(_PROBEVOLUME_ON)
    TEXTURE3D(_ProbeVolumeShR);
    TEXTURE3D(_ProbeVolumeShG);
    TEXTURE3D(_ProbeVolumeShB);
    TEXTURE3D(_ProbeVolumeOcc);
    SAMPLER(sampler_ProbeVolumeShR);
    #endif

    void Ext_ModifyVertex5 (inout VertexData v, inout ExtraV2F d)
    {
        #if defined(_PROBEVOLUME_ON)
            float3 position = mul(_ProbeWorldToTexture, float4(TransformObjectToWorld(v.vertex.xyz), 1.0f)).xyz;
            float3 texCoord = (position - _ProbeVolumeMin.xyz) * _ProbeVolumeSizeInv;
            
            d.extraV2F3.xyz = texCoord; //position in space relative to the volume.
        #endif
    }

    void Ext_SurfaceFunction5 (inout Surface o, inout ShaderData d)
    {
        #if defined(_PROBEVOLUME_ON)
            float3 texCoord = d.extraV2F3.xyz;

            //unity_SHAr = SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord);
            //unity_SHAg = SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShG, texCoord);
            //unity_SHAb = SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShB, texCoord);
            //unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeOcc, texCoord);
            
            //Custom baked gi data
            o.DiffuseGI = SHEvalLinearL0L1( d.worldSpaceNormal, SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord),
                                            SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShR, texCoord), SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShR, texCoord));
            unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeShR, texCoord);
            o.ShadowMask = unity_ProbesOcclusion;
        #endif
    }




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                  Ext_SurfaceFunction1(l, d);
                  Ext_SurfaceFunction2(l, d);
                  Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                  Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                  Ext_ModifyVertex4(v, d);
                  Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                  v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



         

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
             d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

         
         #if defined(SHADERPASS_SHADOWCASTER)
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if defined(SHADERPASS_SHADOWCASTER)
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif defined(SHADERPASS_META)
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif


          // #if %SCREENPOSREQUIREKEY%
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if defined(SHADERPASS_FORWARD) || (SHADERPASS == SHADERPASS_GBUFFER)
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              // o.texcoord1.xy = uv1;
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


         

         // fragment shader
         half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
         ) : SV_Target
         {
           UNITY_SETUP_INSTANCE_ID(IN);
           UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

           ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );
           Surface l = (Surface)0;

           #ifdef _DEPTHOFFSET_ON
              l.outputDepth = outputDepth;
           #endif

           l.Albedo = half3(0.5, 0.5, 0.5);
           l.Normal = float3(0,0,1);
           l.Occlusion = 1;
           l.Alpha = 1;

           ChainSurfaceFunction(l, d);

           #ifdef _DEPTHOFFSET_ON
              outputDepth = l.outputDepth;
           #endif

           #if _USESPECULAR || _SIMPLELIT
              float3 specular = l.Specular;
              float metallic = 1;
           #else   
              float3 specular = 0;
              float metallic = l.Metallic;
           #endif


            
           
            InputData inputData;

            inputData.positionWS = IN.worldPos;
            #if _WORLDSPACENORMAL
              inputData.normalWS = l.Normal;
            #else
              inputData.normalWS = normalize(TangentToWorldSpace(d, l.Normal));
            #endif

            inputData.viewDirectionWS = SafeNormalize(d.worldSpaceViewDir);


            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                  inputData.shadowCoord = IN.shadowCoord;
            #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                  inputData.shadowCoord = TransformWorldToShadowCoord(IN.worldPos);
            #else
                  inputData.shadowCoord = float4(0, 0, 0, 0);
            #endif

            inputData.fogCoord = IN.fogFactorAndVertexLight.x;
            inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
            #if defined(_OVERRIDE_BAKEDGI)
               inputData.bakedGI = l.DiffuseGI;
            #else
               inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.sh, inputData.normalWS);
            #endif
            inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.pos);

            #if defined(_OVERRIDE_SHADOWMASK)
               inputData.shadowMask = l.ShadowMask;
            #else
               inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUV);
            #endif

            #if !_UNLIT
               #if _SIMPLELIT
                  half4 color = UniversalFragmentBlinnPhong(
                     inputData,
                     l.Albedo,
                     float4(specular * l.Smoothness, 0),
                     l.SpecularPower * 128,
                     l.Emission,
                     l.Alpha);
                  color.a = l.Alpha;
               #else

                  
                  SurfaceData surface         = (SurfaceData)0;
                  surface.albedo              = l.Albedo;
                  surface.metallic            = saturate(metallic);
                  surface.specular            = specular;
                  surface.smoothness          = saturate(l.Smoothness),
                  surface.occlusion           = l.Occlusion,
                  surface.emission            = l.Emission,
                  surface.alpha               = saturate(l.Alpha);
                  surface.clearCoatMask       = 0;
                  surface.clearCoatSmoothness = 1;

                  #ifdef _CLEARCOAT
                      surface.clearCoatMask       = saturate(l.CoatMask);
                      surface.clearCoatSmoothness = saturate(l.CoatSmoothness);
                  #endif

                  half4 color = UniversalFragmentPBR(inputData, surface);

               #endif
               color.rgb = MixFog(color.rgb, inputData.fogCoord);

            #else
               half4 color = half4(l.Albedo, l.Alpha);
               color.rgb = MixFog(color.rgb, inputData.fogCoord);
            #endif
            ChainFinalColorForward(l, d, color);

            return color;

         }

         ENDHLSL

      }


      
      
        Pass
        {
            Name "ShadowCaster"
            Tags 
            { 
                "LightMode" = "ShadowCaster"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>

               Blend[_SrcBlend][_DstBlend]
   ZWrite[_ZWrite]
   Cull [_CullMode]


            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_instancing
        
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define SHADERPASS_SHADOWCASTER
            #define _PASSSHADOW 1

            


   #pragma shader_feature_local_fragment _ALPHATEST_ON
   #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
   #pragma shader_feature_local_fragment _ _DETAIL
   #pragma shader_feature_local_fragment _ _EMISSION


   //#pragma multi_compile_local_fragment _ _COLORMASK_ON
   #pragma shader_feature_local_fragment _ _COLORMASK_ON


   #pragma multi_compile_local_fragment _ _REVEALLAYERS
   //#pragma shader_feature_local_fragment _ _REVEALLAYERS


   #pragma multi_compile_local_vertex _ _VERTEXOCCLUSION_ON


   #pragma multi_compile_local _ _PROBEVOLUME_ON
   //#pragma shader_feature_local _ _PROBEVOLUME_ON

   #if defined(_PROBEVOLUME_ON)
       #define _OVERRIDE_BAKEDGI
   #endif


   #define _URP 1

                 
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
     
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D

      

      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCCOORD3;
         // float4 texcoord1 : TEXCCOORD4;
         // float4 texcoord2 : TEXCCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
          float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
          float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif
            float4 fogFactorAndVertexLight : TEXCOORD10;
            float4 shadowCoord : TEXCOORD11;
         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
      };

         
            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;
            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
	float2 baseuv;


                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // uint vertexID : SV_VertexID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // would love to strip these, but they are used in certain
               // combinations of the lighting system, and may be used
               // by the user as well, so no easy way to strip them.

               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetObjectToWorldMatrix()
               #undef GetWorldToObjectMatrix()
               #undef GetWorldToViewMatrix()
               #undef UNITY_MATRIX_I_V
               #undef UNITY_MATRIX_P
               #undef GetWorldToHClipMatrix()
               #undef GetObjectToWorldMatrix()V
               #undef UNITY_MATRIX_T_MV
               #undef UNITY_MATRIX_IT_MV
               #undef GetObjectToWorldMatrix()VP

               #define GetObjectToWorldMatrix()     unity_ObjectToWorld
               #define GetWorldToObjectMatrix()   unity_WorldToObject
               #define GetWorldToViewMatrix()     unity_MatrixV
               #define UNITY_MATRIX_I_V   unity_MatrixInvV
               #define GetViewToHClipMatrix()     OptimizeProjectionMatrix(glstate_matrix_projection)
               #define GetWorldToHClipMatrix()    unity_MatrixVP
               #define GetObjectToWorldMatrix()V    mul(GetWorldToViewMatrix(), GetObjectToWorldMatrix())
               #define UNITY_MATRIX_T_MV  transpose(GetObjectToWorldMatrix()V)
               #define UNITY_MATRIX_IT_MV transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V))
               #define GetObjectToWorldMatrix()VP   mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix())


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }


            
            CBUFFER_START(UnityPerMaterial)

               



	half _AlphaClip;
	half _Cutoff;
	half _AlphaStrength;

	float4 _BaseMap_ST;
	half4 _BaseColor;
	half _NormalStrength;
	half _Smoothness;
	half _OcclusionStrength;
	float4 _EmissionColor;

	float4 _DetailAlbedoMap_ST;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
	float4 _DetailWeightOverDistance;


	half4 _Tint0;
	half4 _Tint1;
	half4 _Tint2;
	half4 _Tint3;


	float4 _RevealMask_TexelSize;
	float4 _LayerSurfaceExp;

	half _Layer0NormalStrength;
	half _Layer0Smoothness;
	half _Layer0Metallic;
	half _Layer1NormalStrength;
	half _Layer1Smoothness;
	half _Layer1Metallic;
	half _Layer2Height;
	float4 _Layer3EmissionColor;

	float4 _Layer0_ST;
	float4 _Layer1_ST;


	int _Bitmask;


    float4x4 _ProbeWorldToTexture;
    float3 _ProbeVolumeMin;
    float3 _ProbeVolumeSizeInv;	



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()


    float Dither8x8Bayer( int x, int y )
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
        return dither[r] / 64; 
    }

    void ApplyDitherCrossFadeVSP(float2 vpos, float fadeValue)
    {
        float dither = Dither8x8Bayer( fmod(vpos.x, 8), fmod(vpos.y, 8) );
        float sgn = fadeValue > 0 ? 1.0f : -1.0f;
        clip(dither - (1-fadeValue) * sgn);
    }
    

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
		#if LOD_FADE_CROSSFADE
            float4 screenPosNorm = d.screenPos / d.screenPos.w;
            screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;
            float2 clipScreen = screenPosNorm.xy * _ScreenParams.xy;
            ApplyDitherCrossFadeVSP(clipScreen,unity_LODFade.x);
        #endif
	}




   TEXTURE2D(_BaseMap);
   SAMPLER(sampler_BaseMap);
   TEXTURE2D(_BumpMap);
   SAMPLER(sampler_BumpMap);
   TEXTURE2D(_MetallicGlossMap);
   SAMPLER(sampler_MetallicGlossMap);

   #if defined(_EMISSION)
   TEXTURE2D(_EmissionMap);
   SAMPLER(sampler_EmissionMap);
   #endif

   #if defined(_DETAIL)
   TEXTURE2D(_DetailAlbedoMap);
   SAMPLER(sampler_DetailAlbedoMap);
   TEXTURE2D(_DetailNormalMap);
   SAMPLER(sampler_DetailNormalMap);
   #endif

	float4 Unity_Blend_Overlay(float4 Base, float4 Blend, float Opacity)
	{
		float4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
		float4 result2 = 2.0 * Base * Blend;
		float4 zeroOrOne = step(Base, 0.5);
		float4 Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
		return lerp(Base, Out, Opacity);
	}

	float3 Unity_NormalBlend_Reoriented(float3 A, float3 B)
	{
		float3 t = A.xyz + float3(0.0, 0.0, 1.0);
		float3 u = B.xyz * float3(-1.0, -1.0, 1.0);
		return ((t / t.z) * dot(t, u) - u);
	}

	void Ext_SurfaceFunction1 (inout Surface o, inout ShaderData d)
	{
		float2 uv = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
		d.blackboard.baseuv = uv;
		half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);

		o.Albedo = albedo.rgb * _BaseColor.rgb;

		#if defined(_ALPHATEST_ON)
			clip(albedo.a - _Cutoff);
		#endif

		o.Alpha = albedo.a * _AlphaStrength;
		o.Normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uv), _NormalStrength);

		float4 mask = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv);
        o.Metallic = mask.r;
        o.Occlusion = LerpWhiteTo(mask.g, _OcclusionStrength);
		float emissionMask = mask.b;
        o.Smoothness = mask.a * _Smoothness;

		half3 emission = 0;
		#if defined(_EMISSION)
           o.Emission = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, uv).rgb * _EmissionColor.rgb * emissionMask;
		#else
		   o.Emission = _EmissionColor.rgb * emissionMask;
		#endif

		#if defined(_DETAIL) && !defined(SHADER_API_MOBILE)
			float weight = 1.0;
		    if (_DetailWeightOverDistance.y + _DetailWeightOverDistance.w < 2)
            {
               float camDist = distance(d.worldSpacePosition, _WorldSpaceCameraPos);
               weight = lerp(_DetailWeightOverDistance.y, _DetailWeightOverDistance.w, saturate((camDist-_DetailWeightOverDistance.x) / _DetailWeightOverDistance.z));
            }

			float2 detailuv = d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			o.Albedo = Unity_Blend_Overlay(albedo, SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailuv), _DetailAlbedoMapScale * weight).rgb * _BaseColor.rgb;
			o.Normal = Unity_NormalBlend_Reoriented(o.Normal, UnpackScaleNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailuv), _DetailNormalMapScale * weight));
		#endif

		o.Height = albedo.a;
	}




#if defined(_COLORMASK_ON)
	TEXTURE2D(_ColorMask);
    SAMPLER(sampler_ColorMask);
#endif

	void Ext_SurfaceFunction2 (inout Surface o, inout ShaderData d)
	{
		#if defined(_COLORMASK_ON)
		float4 colorMask = SAMPLE_TEXTURE2D(_ColorMask, sampler_ColorMask, d.blackboard.baseuv);
		float t = colorMask.r + colorMask.g + colorMask.b + (1.0 - colorMask.a);
		float3 c = ((colorMask.r * _Tint0) + (colorMask.g * _Tint1) + (colorMask.b * _Tint2) + ((1.0 - colorMask.a) * _Tint3)).rgb * o.Albedo;
		o.Albedo = lerp(o.Albedo, c, t);
		#endif
	}




#if defined(_REVEALLAYERS)
   TEXTURE2D(_RevealMask);
   SAMPLER(sampler_RevealMask);
   TEXTURE2D(_LayerMask);
   SAMPLER(sampler_LayerMask);
   TEXTURE2D(_Layer0);
   SAMPLER(sampler_Layer0);
   TEXTURE2D(_Layer0NormalMap);
   SAMPLER(sampler_Layer0NormalMap);
   TEXTURE2D(_Layer1);
   SAMPLER(sampler_Layer1);
   TEXTURE2D(_Layer1NormalMap);
   SAMPLER(sampler_Layer1NormalMap);

	float3 RevealMaskNormalCrossFilter(float2 uv, float height, float2 texelSize)
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
#endif

	void Ext_SurfaceFunction3 (inout Surface o, inout ShaderData d)
	{
		#if defined(_REVEALLAYERS)
		
		half4 revealMask = SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, d.blackboard.baseuv);
		half4 layerMask = SAMPLE_TEXTURE2D(_LayerMask, sampler_LayerMask, d.blackboard.baseuv);

		float2 layer0uv = d.texcoord0.xy * _Layer0_ST.xy + _Layer0_ST.zw;
		float2 layer1uv = d.texcoord0.xy * _Layer1_ST.xy + _Layer1_ST.zw;

		half4 layer0col = SAMPLE_TEXTURE2D(_Layer0, sampler_Layer0, layer0uv);
		float3 layer0normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_Layer0NormalMap, sampler_Layer0NormalMap, layer0uv), _NormalStrength);

		float rt = saturate(revealMask.r * layerMask.r);
		o.Albedo = lerp(o.Albedo, layer0col.rgb, pow(rt, _LayerSurfaceExp.x));
		o.Normal = lerp(o.Normal, layer0normal, pow(rt, _LayerSurfaceExp.y));
		o.Metallic = lerp(o.Metallic, _Layer0Metallic, pow(rt, _LayerSurfaceExp.z));
		o.Smoothness = lerp(o.Smoothness, _Layer0Smoothness, pow(rt, _LayerSurfaceExp.w));

		half4 layer1col = SAMPLE_TEXTURE2D(_Layer1, sampler_Layer1, layer1uv);
		float3 layer1normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_Layer1NormalMap, sampler_Layer1NormalMap, layer1uv), _NormalStrength);

		float gt = saturate(revealMask.g * layerMask.g);
		o.Albedo = lerp(o.Albedo, layer1col.rgb, pow(gt, _LayerSurfaceExp.x));
		o.Normal = lerp(o.Normal, layer1normal, pow(gt, _LayerSurfaceExp.y));
		o.Metallic = lerp(o.Metallic, _Layer1Metallic, pow(gt, _LayerSurfaceExp.z));
		o.Smoothness = lerp(o.Smoothness, _Layer1Smoothness, pow(gt, _LayerSurfaceExp.w));

		float bt = saturate(revealMask.b * layerMask.b);
		float height = _Layer2Height * bt;
		float3 heightNormal = RevealMaskNormalCrossFilter(d.blackboard.baseuv, height, _RevealMask_TexelSize.xy);
		o.Normal = normalize(float3(o.Normal.rg + heightNormal.rg, o.Normal.b * heightNormal.b)); //Blend normal

		float at = saturate(revealMask.a * layerMask.a);
		o.Emission = ((1.0 - at) * o.Emission.rgb) + (at * _Layer3EmissionColor.rgb);

		#endif
	}




    void Ext_ModifyVertex4 (inout VertexData v, inout ExtraV2F d)
    {
        #if defined(_VERTEXOCCLUSION_ON)
        if(_Bitmask & (int)v.texcoord1.x)
        {
            v.vertex *= (0.0 / 0.0);
        }
        #endif
    }




    #if defined(_PROBEVOLUME_ON)
    TEXTURE3D(_ProbeVolumeShR);
    TEXTURE3D(_ProbeVolumeShG);
    TEXTURE3D(_ProbeVolumeShB);
    TEXTURE3D(_ProbeVolumeOcc);
    SAMPLER(sampler_ProbeVolumeShR);
    #endif

    void Ext_ModifyVertex5 (inout VertexData v, inout ExtraV2F d)
    {
        #if defined(_PROBEVOLUME_ON)
            float3 position = mul(_ProbeWorldToTexture, float4(TransformObjectToWorld(v.vertex.xyz), 1.0f)).xyz;
            float3 texCoord = (position - _ProbeVolumeMin.xyz) * _ProbeVolumeSizeInv;
            
            d.extraV2F3.xyz = texCoord; //position in space relative to the volume.
        #endif
    }

    void Ext_SurfaceFunction5 (inout Surface o, inout ShaderData d)
    {
        #if defined(_PROBEVOLUME_ON)
            float3 texCoord = d.extraV2F3.xyz;

            //unity_SHAr = SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord);
            //unity_SHAg = SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShG, texCoord);
            //unity_SHAb = SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShB, texCoord);
            //unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeOcc, texCoord);
            
            //Custom baked gi data
            o.DiffuseGI = SHEvalLinearL0L1( d.worldSpaceNormal, SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord),
                                            SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShR, texCoord), SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShR, texCoord));
            unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeShR, texCoord);
            o.ShadowMask = unity_ProbesOcclusion;
        #endif
    }




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                  Ext_SurfaceFunction1(l, d);
                  Ext_SurfaceFunction2(l, d);
                  Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                  Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                  Ext_ModifyVertex4(v, d);
                  Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                  v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
             d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            
         #if defined(SHADERPASS_SHADOWCASTER)
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if defined(SHADERPASS_SHADOWCASTER)
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif defined(SHADERPASS_META)
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif


          // #if %SCREENPOSREQUIREKEY%
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if defined(SHADERPASS_FORWARD) || (SHADERPASS == SHADERPASS_GBUFFER)
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              // o.texcoord1.xy = uv1;
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


            

            // fragment shader
            half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);

               ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );
               Surface l = (Surface)0;

               #ifdef _DEPTHOFFSET_ON
                  l.outputDepth = outputDepth;
               #endif

               l.Albedo = half3(0.5, 0.5, 0.5);
               l.Normal = float3(0,0,1);
               l.Occlusion = 1;
               l.Alpha = 1;

               ChainSurfaceFunction(l, d);

               #ifdef _DEPTHOFFSET_ON
                  outputDepth = l.outputDepth;
               #endif

             return 0;

            }

         ENDHLSL

      }


      
        Pass
        {
            Name "DepthOnly"
            Tags 
            { 
                "LightMode" = "DepthOnly"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            ColorMask 0
            
               Blend[_SrcBlend][_DstBlend]
   ZWrite[_ZWrite]
   Cull [_CullMode]


            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag


            #define SHADERPASS_DEPTHONLY
            #define _PASSDEPTH 1

            #pragma target 3.0
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            


   #pragma shader_feature_local_fragment _ALPHATEST_ON
   #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
   #pragma shader_feature_local_fragment _ _DETAIL
   #pragma shader_feature_local_fragment _ _EMISSION


   //#pragma multi_compile_local_fragment _ _COLORMASK_ON
   #pragma shader_feature_local_fragment _ _COLORMASK_ON


   #pragma multi_compile_local_fragment _ _REVEALLAYERS
   //#pragma shader_feature_local_fragment _ _REVEALLAYERS


   #pragma multi_compile_local_vertex _ _VERTEXOCCLUSION_ON


   #pragma multi_compile_local _ _PROBEVOLUME_ON
   //#pragma shader_feature_local _ _PROBEVOLUME_ON

   #if defined(_PROBEVOLUME_ON)
       #define _OVERRIDE_BAKEDGI
   #endif


   #define _URP 1

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"


                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
     
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D

      

      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCCOORD3;
         // float4 texcoord1 : TEXCCOORD4;
         // float4 texcoord2 : TEXCCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
          float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
          float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif
            float4 fogFactorAndVertexLight : TEXCOORD10;
            float4 shadowCoord : TEXCOORD11;
         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
      };

         
            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;
            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
	float2 baseuv;


                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // uint vertexID : SV_VertexID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // would love to strip these, but they are used in certain
               // combinations of the lighting system, and may be used
               // by the user as well, so no easy way to strip them.

               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetObjectToWorldMatrix()
               #undef GetWorldToObjectMatrix()
               #undef GetWorldToViewMatrix()
               #undef UNITY_MATRIX_I_V
               #undef UNITY_MATRIX_P
               #undef GetWorldToHClipMatrix()
               #undef GetObjectToWorldMatrix()V
               #undef UNITY_MATRIX_T_MV
               #undef UNITY_MATRIX_IT_MV
               #undef GetObjectToWorldMatrix()VP

               #define GetObjectToWorldMatrix()     unity_ObjectToWorld
               #define GetWorldToObjectMatrix()   unity_WorldToObject
               #define GetWorldToViewMatrix()     unity_MatrixV
               #define UNITY_MATRIX_I_V   unity_MatrixInvV
               #define GetViewToHClipMatrix()     OptimizeProjectionMatrix(glstate_matrix_projection)
               #define GetWorldToHClipMatrix()    unity_MatrixVP
               #define GetObjectToWorldMatrix()V    mul(GetWorldToViewMatrix(), GetObjectToWorldMatrix())
               #define UNITY_MATRIX_T_MV  transpose(GetObjectToWorldMatrix()V)
               #define UNITY_MATRIX_IT_MV transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V))
               #define GetObjectToWorldMatrix()VP   mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix())


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }


            
            CBUFFER_START(UnityPerMaterial)

               



	half _AlphaClip;
	half _Cutoff;
	half _AlphaStrength;

	float4 _BaseMap_ST;
	half4 _BaseColor;
	half _NormalStrength;
	half _Smoothness;
	half _OcclusionStrength;
	float4 _EmissionColor;

	float4 _DetailAlbedoMap_ST;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
	float4 _DetailWeightOverDistance;


	half4 _Tint0;
	half4 _Tint1;
	half4 _Tint2;
	half4 _Tint3;


	float4 _RevealMask_TexelSize;
	float4 _LayerSurfaceExp;

	half _Layer0NormalStrength;
	half _Layer0Smoothness;
	half _Layer0Metallic;
	half _Layer1NormalStrength;
	half _Layer1Smoothness;
	half _Layer1Metallic;
	half _Layer2Height;
	float4 _Layer3EmissionColor;

	float4 _Layer0_ST;
	float4 _Layer1_ST;


	int _Bitmask;


    float4x4 _ProbeWorldToTexture;
    float3 _ProbeVolumeMin;
    float3 _ProbeVolumeSizeInv;	



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()


    float Dither8x8Bayer( int x, int y )
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
        return dither[r] / 64; 
    }

    void ApplyDitherCrossFadeVSP(float2 vpos, float fadeValue)
    {
        float dither = Dither8x8Bayer( fmod(vpos.x, 8), fmod(vpos.y, 8) );
        float sgn = fadeValue > 0 ? 1.0f : -1.0f;
        clip(dither - (1-fadeValue) * sgn);
    }
    

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
		#if LOD_FADE_CROSSFADE
            float4 screenPosNorm = d.screenPos / d.screenPos.w;
            screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;
            float2 clipScreen = screenPosNorm.xy * _ScreenParams.xy;
            ApplyDitherCrossFadeVSP(clipScreen,unity_LODFade.x);
        #endif
	}




   TEXTURE2D(_BaseMap);
   SAMPLER(sampler_BaseMap);
   TEXTURE2D(_BumpMap);
   SAMPLER(sampler_BumpMap);
   TEXTURE2D(_MetallicGlossMap);
   SAMPLER(sampler_MetallicGlossMap);

   #if defined(_EMISSION)
   TEXTURE2D(_EmissionMap);
   SAMPLER(sampler_EmissionMap);
   #endif

   #if defined(_DETAIL)
   TEXTURE2D(_DetailAlbedoMap);
   SAMPLER(sampler_DetailAlbedoMap);
   TEXTURE2D(_DetailNormalMap);
   SAMPLER(sampler_DetailNormalMap);
   #endif

	float4 Unity_Blend_Overlay(float4 Base, float4 Blend, float Opacity)
	{
		float4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
		float4 result2 = 2.0 * Base * Blend;
		float4 zeroOrOne = step(Base, 0.5);
		float4 Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
		return lerp(Base, Out, Opacity);
	}

	float3 Unity_NormalBlend_Reoriented(float3 A, float3 B)
	{
		float3 t = A.xyz + float3(0.0, 0.0, 1.0);
		float3 u = B.xyz * float3(-1.0, -1.0, 1.0);
		return ((t / t.z) * dot(t, u) - u);
	}

	void Ext_SurfaceFunction1 (inout Surface o, inout ShaderData d)
	{
		float2 uv = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
		d.blackboard.baseuv = uv;
		half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);

		o.Albedo = albedo.rgb * _BaseColor.rgb;

		#if defined(_ALPHATEST_ON)
			clip(albedo.a - _Cutoff);
		#endif

		o.Alpha = albedo.a * _AlphaStrength;
		o.Normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uv), _NormalStrength);

		float4 mask = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv);
        o.Metallic = mask.r;
        o.Occlusion = LerpWhiteTo(mask.g, _OcclusionStrength);
		float emissionMask = mask.b;
        o.Smoothness = mask.a * _Smoothness;

		half3 emission = 0;
		#if defined(_EMISSION)
           o.Emission = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, uv).rgb * _EmissionColor.rgb * emissionMask;
		#else
		   o.Emission = _EmissionColor.rgb * emissionMask;
		#endif

		#if defined(_DETAIL) && !defined(SHADER_API_MOBILE)
			float weight = 1.0;
		    if (_DetailWeightOverDistance.y + _DetailWeightOverDistance.w < 2)
            {
               float camDist = distance(d.worldSpacePosition, _WorldSpaceCameraPos);
               weight = lerp(_DetailWeightOverDistance.y, _DetailWeightOverDistance.w, saturate((camDist-_DetailWeightOverDistance.x) / _DetailWeightOverDistance.z));
            }

			float2 detailuv = d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			o.Albedo = Unity_Blend_Overlay(albedo, SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailuv), _DetailAlbedoMapScale * weight).rgb * _BaseColor.rgb;
			o.Normal = Unity_NormalBlend_Reoriented(o.Normal, UnpackScaleNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailuv), _DetailNormalMapScale * weight));
		#endif

		o.Height = albedo.a;
	}




#if defined(_COLORMASK_ON)
	TEXTURE2D(_ColorMask);
    SAMPLER(sampler_ColorMask);
#endif

	void Ext_SurfaceFunction2 (inout Surface o, inout ShaderData d)
	{
		#if defined(_COLORMASK_ON)
		float4 colorMask = SAMPLE_TEXTURE2D(_ColorMask, sampler_ColorMask, d.blackboard.baseuv);
		float t = colorMask.r + colorMask.g + colorMask.b + (1.0 - colorMask.a);
		float3 c = ((colorMask.r * _Tint0) + (colorMask.g * _Tint1) + (colorMask.b * _Tint2) + ((1.0 - colorMask.a) * _Tint3)).rgb * o.Albedo;
		o.Albedo = lerp(o.Albedo, c, t);
		#endif
	}




#if defined(_REVEALLAYERS)
   TEXTURE2D(_RevealMask);
   SAMPLER(sampler_RevealMask);
   TEXTURE2D(_LayerMask);
   SAMPLER(sampler_LayerMask);
   TEXTURE2D(_Layer0);
   SAMPLER(sampler_Layer0);
   TEXTURE2D(_Layer0NormalMap);
   SAMPLER(sampler_Layer0NormalMap);
   TEXTURE2D(_Layer1);
   SAMPLER(sampler_Layer1);
   TEXTURE2D(_Layer1NormalMap);
   SAMPLER(sampler_Layer1NormalMap);

	float3 RevealMaskNormalCrossFilter(float2 uv, float height, float2 texelSize)
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
#endif

	void Ext_SurfaceFunction3 (inout Surface o, inout ShaderData d)
	{
		#if defined(_REVEALLAYERS)
		
		half4 revealMask = SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, d.blackboard.baseuv);
		half4 layerMask = SAMPLE_TEXTURE2D(_LayerMask, sampler_LayerMask, d.blackboard.baseuv);

		float2 layer0uv = d.texcoord0.xy * _Layer0_ST.xy + _Layer0_ST.zw;
		float2 layer1uv = d.texcoord0.xy * _Layer1_ST.xy + _Layer1_ST.zw;

		half4 layer0col = SAMPLE_TEXTURE2D(_Layer0, sampler_Layer0, layer0uv);
		float3 layer0normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_Layer0NormalMap, sampler_Layer0NormalMap, layer0uv), _NormalStrength);

		float rt = saturate(revealMask.r * layerMask.r);
		o.Albedo = lerp(o.Albedo, layer0col.rgb, pow(rt, _LayerSurfaceExp.x));
		o.Normal = lerp(o.Normal, layer0normal, pow(rt, _LayerSurfaceExp.y));
		o.Metallic = lerp(o.Metallic, _Layer0Metallic, pow(rt, _LayerSurfaceExp.z));
		o.Smoothness = lerp(o.Smoothness, _Layer0Smoothness, pow(rt, _LayerSurfaceExp.w));

		half4 layer1col = SAMPLE_TEXTURE2D(_Layer1, sampler_Layer1, layer1uv);
		float3 layer1normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_Layer1NormalMap, sampler_Layer1NormalMap, layer1uv), _NormalStrength);

		float gt = saturate(revealMask.g * layerMask.g);
		o.Albedo = lerp(o.Albedo, layer1col.rgb, pow(gt, _LayerSurfaceExp.x));
		o.Normal = lerp(o.Normal, layer1normal, pow(gt, _LayerSurfaceExp.y));
		o.Metallic = lerp(o.Metallic, _Layer1Metallic, pow(gt, _LayerSurfaceExp.z));
		o.Smoothness = lerp(o.Smoothness, _Layer1Smoothness, pow(gt, _LayerSurfaceExp.w));

		float bt = saturate(revealMask.b * layerMask.b);
		float height = _Layer2Height * bt;
		float3 heightNormal = RevealMaskNormalCrossFilter(d.blackboard.baseuv, height, _RevealMask_TexelSize.xy);
		o.Normal = normalize(float3(o.Normal.rg + heightNormal.rg, o.Normal.b * heightNormal.b)); //Blend normal

		float at = saturate(revealMask.a * layerMask.a);
		o.Emission = ((1.0 - at) * o.Emission.rgb) + (at * _Layer3EmissionColor.rgb);

		#endif
	}




    void Ext_ModifyVertex4 (inout VertexData v, inout ExtraV2F d)
    {
        #if defined(_VERTEXOCCLUSION_ON)
        if(_Bitmask & (int)v.texcoord1.x)
        {
            v.vertex *= (0.0 / 0.0);
        }
        #endif
    }




    #if defined(_PROBEVOLUME_ON)
    TEXTURE3D(_ProbeVolumeShR);
    TEXTURE3D(_ProbeVolumeShG);
    TEXTURE3D(_ProbeVolumeShB);
    TEXTURE3D(_ProbeVolumeOcc);
    SAMPLER(sampler_ProbeVolumeShR);
    #endif

    void Ext_ModifyVertex5 (inout VertexData v, inout ExtraV2F d)
    {
        #if defined(_PROBEVOLUME_ON)
            float3 position = mul(_ProbeWorldToTexture, float4(TransformObjectToWorld(v.vertex.xyz), 1.0f)).xyz;
            float3 texCoord = (position - _ProbeVolumeMin.xyz) * _ProbeVolumeSizeInv;
            
            d.extraV2F3.xyz = texCoord; //position in space relative to the volume.
        #endif
    }

    void Ext_SurfaceFunction5 (inout Surface o, inout ShaderData d)
    {
        #if defined(_PROBEVOLUME_ON)
            float3 texCoord = d.extraV2F3.xyz;

            //unity_SHAr = SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord);
            //unity_SHAg = SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShG, texCoord);
            //unity_SHAb = SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShB, texCoord);
            //unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeOcc, texCoord);
            
            //Custom baked gi data
            o.DiffuseGI = SHEvalLinearL0L1( d.worldSpaceNormal, SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord),
                                            SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShR, texCoord), SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShR, texCoord));
            unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeShR, texCoord);
            o.ShadowMask = unity_ProbesOcclusion;
        #endif
    }




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                  Ext_SurfaceFunction1(l, d);
                  Ext_SurfaceFunction2(l, d);
                  Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                  Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                  Ext_ModifyVertex4(v, d);
                  Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                  v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
             d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            
         #if defined(SHADERPASS_SHADOWCASTER)
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if defined(SHADERPASS_SHADOWCASTER)
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif defined(SHADERPASS_META)
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif


          // #if %SCREENPOSREQUIREKEY%
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if defined(SHADERPASS_FORWARD) || (SHADERPASS == SHADERPASS_GBUFFER)
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              // o.texcoord1.xy = uv1;
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


            

            // fragment shader
            half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);
               UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

               ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );
               Surface l = (Surface)0;

               #ifdef _DEPTHOFFSET_ON
                  l.outputDepth = outputDepth;
               #endif

               l.Albedo = half3(0.5, 0.5, 0.5);
               l.Normal = float3(0,0,1);
               l.Occlusion = 1;
               l.Alpha = 1;

               ChainSurfaceFunction(l, d);

               #ifdef _DEPTHOFFSET_ON
                  outputDepth = l.outputDepth;
               #endif

               return 0;

            }

         ENDHLSL

      }


      
        Pass
        {
            Name "Meta"
            Tags 
            { 
                "LightMode" = "Meta"
            }

             // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>

               Blend[_SrcBlend][_DstBlend]
   ZWrite[_ZWrite]
   Cull [_CullMode]


            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
        
            #define SHADERPASS_META
            #define _PASSMETA 1


            


   #pragma shader_feature_local_fragment _ALPHATEST_ON
   #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
   #pragma shader_feature_local_fragment _ _DETAIL
   #pragma shader_feature_local_fragment _ _EMISSION


   //#pragma multi_compile_local_fragment _ _COLORMASK_ON
   #pragma shader_feature_local_fragment _ _COLORMASK_ON


   #pragma multi_compile_local_fragment _ _REVEALLAYERS
   //#pragma shader_feature_local_fragment _ _REVEALLAYERS


   #pragma multi_compile_local_vertex _ _VERTEXOCCLUSION_ON


   #pragma multi_compile_local _ _PROBEVOLUME_ON
   //#pragma shader_feature_local _ _PROBEVOLUME_ON

   #if defined(_PROBEVOLUME_ON)
       #define _OVERRIDE_BAKEDGI
   #endif


   #define _URP 1



            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        

                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
     
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D

      

      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCCOORD3;
         // float4 texcoord1 : TEXCCOORD4;
         // float4 texcoord2 : TEXCCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
          float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
          float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif
            float4 fogFactorAndVertexLight : TEXCOORD10;
            float4 shadowCoord : TEXCOORD11;
         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
      };


            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;
            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
	float2 baseuv;


                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // uint vertexID : SV_VertexID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // would love to strip these, but they are used in certain
               // combinations of the lighting system, and may be used
               // by the user as well, so no easy way to strip them.

               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetObjectToWorldMatrix()
               #undef GetWorldToObjectMatrix()
               #undef GetWorldToViewMatrix()
               #undef UNITY_MATRIX_I_V
               #undef UNITY_MATRIX_P
               #undef GetWorldToHClipMatrix()
               #undef GetObjectToWorldMatrix()V
               #undef UNITY_MATRIX_T_MV
               #undef UNITY_MATRIX_IT_MV
               #undef GetObjectToWorldMatrix()VP

               #define GetObjectToWorldMatrix()     unity_ObjectToWorld
               #define GetWorldToObjectMatrix()   unity_WorldToObject
               #define GetWorldToViewMatrix()     unity_MatrixV
               #define UNITY_MATRIX_I_V   unity_MatrixInvV
               #define GetViewToHClipMatrix()     OptimizeProjectionMatrix(glstate_matrix_projection)
               #define GetWorldToHClipMatrix()    unity_MatrixVP
               #define GetObjectToWorldMatrix()V    mul(GetWorldToViewMatrix(), GetObjectToWorldMatrix())
               #define UNITY_MATRIX_T_MV  transpose(GetObjectToWorldMatrix()V)
               #define UNITY_MATRIX_IT_MV transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V))
               #define GetObjectToWorldMatrix()VP   mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix())


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }


            
            CBUFFER_START(UnityPerMaterial)

               



	half _AlphaClip;
	half _Cutoff;
	half _AlphaStrength;

	float4 _BaseMap_ST;
	half4 _BaseColor;
	half _NormalStrength;
	half _Smoothness;
	half _OcclusionStrength;
	float4 _EmissionColor;

	float4 _DetailAlbedoMap_ST;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
	float4 _DetailWeightOverDistance;


	half4 _Tint0;
	half4 _Tint1;
	half4 _Tint2;
	half4 _Tint3;


	float4 _RevealMask_TexelSize;
	float4 _LayerSurfaceExp;

	half _Layer0NormalStrength;
	half _Layer0Smoothness;
	half _Layer0Metallic;
	half _Layer1NormalStrength;
	half _Layer1Smoothness;
	half _Layer1Metallic;
	half _Layer2Height;
	float4 _Layer3EmissionColor;

	float4 _Layer0_ST;
	float4 _Layer1_ST;


	int _Bitmask;


    float4x4 _ProbeWorldToTexture;
    float3 _ProbeVolumeMin;
    float3 _ProbeVolumeSizeInv;	



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()


    float Dither8x8Bayer( int x, int y )
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
        return dither[r] / 64; 
    }

    void ApplyDitherCrossFadeVSP(float2 vpos, float fadeValue)
    {
        float dither = Dither8x8Bayer( fmod(vpos.x, 8), fmod(vpos.y, 8) );
        float sgn = fadeValue > 0 ? 1.0f : -1.0f;
        clip(dither - (1-fadeValue) * sgn);
    }
    

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
		#if LOD_FADE_CROSSFADE
            float4 screenPosNorm = d.screenPos / d.screenPos.w;
            screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;
            float2 clipScreen = screenPosNorm.xy * _ScreenParams.xy;
            ApplyDitherCrossFadeVSP(clipScreen,unity_LODFade.x);
        #endif
	}




   TEXTURE2D(_BaseMap);
   SAMPLER(sampler_BaseMap);
   TEXTURE2D(_BumpMap);
   SAMPLER(sampler_BumpMap);
   TEXTURE2D(_MetallicGlossMap);
   SAMPLER(sampler_MetallicGlossMap);

   #if defined(_EMISSION)
   TEXTURE2D(_EmissionMap);
   SAMPLER(sampler_EmissionMap);
   #endif

   #if defined(_DETAIL)
   TEXTURE2D(_DetailAlbedoMap);
   SAMPLER(sampler_DetailAlbedoMap);
   TEXTURE2D(_DetailNormalMap);
   SAMPLER(sampler_DetailNormalMap);
   #endif

	float4 Unity_Blend_Overlay(float4 Base, float4 Blend, float Opacity)
	{
		float4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
		float4 result2 = 2.0 * Base * Blend;
		float4 zeroOrOne = step(Base, 0.5);
		float4 Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
		return lerp(Base, Out, Opacity);
	}

	float3 Unity_NormalBlend_Reoriented(float3 A, float3 B)
	{
		float3 t = A.xyz + float3(0.0, 0.0, 1.0);
		float3 u = B.xyz * float3(-1.0, -1.0, 1.0);
		return ((t / t.z) * dot(t, u) - u);
	}

	void Ext_SurfaceFunction1 (inout Surface o, inout ShaderData d)
	{
		float2 uv = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
		d.blackboard.baseuv = uv;
		half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);

		o.Albedo = albedo.rgb * _BaseColor.rgb;

		#if defined(_ALPHATEST_ON)
			clip(albedo.a - _Cutoff);
		#endif

		o.Alpha = albedo.a * _AlphaStrength;
		o.Normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uv), _NormalStrength);

		float4 mask = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv);
        o.Metallic = mask.r;
        o.Occlusion = LerpWhiteTo(mask.g, _OcclusionStrength);
		float emissionMask = mask.b;
        o.Smoothness = mask.a * _Smoothness;

		half3 emission = 0;
		#if defined(_EMISSION)
           o.Emission = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, uv).rgb * _EmissionColor.rgb * emissionMask;
		#else
		   o.Emission = _EmissionColor.rgb * emissionMask;
		#endif

		#if defined(_DETAIL) && !defined(SHADER_API_MOBILE)
			float weight = 1.0;
		    if (_DetailWeightOverDistance.y + _DetailWeightOverDistance.w < 2)
            {
               float camDist = distance(d.worldSpacePosition, _WorldSpaceCameraPos);
               weight = lerp(_DetailWeightOverDistance.y, _DetailWeightOverDistance.w, saturate((camDist-_DetailWeightOverDistance.x) / _DetailWeightOverDistance.z));
            }

			float2 detailuv = d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			o.Albedo = Unity_Blend_Overlay(albedo, SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailuv), _DetailAlbedoMapScale * weight).rgb * _BaseColor.rgb;
			o.Normal = Unity_NormalBlend_Reoriented(o.Normal, UnpackScaleNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailuv), _DetailNormalMapScale * weight));
		#endif

		o.Height = albedo.a;
	}




#if defined(_COLORMASK_ON)
	TEXTURE2D(_ColorMask);
    SAMPLER(sampler_ColorMask);
#endif

	void Ext_SurfaceFunction2 (inout Surface o, inout ShaderData d)
	{
		#if defined(_COLORMASK_ON)
		float4 colorMask = SAMPLE_TEXTURE2D(_ColorMask, sampler_ColorMask, d.blackboard.baseuv);
		float t = colorMask.r + colorMask.g + colorMask.b + (1.0 - colorMask.a);
		float3 c = ((colorMask.r * _Tint0) + (colorMask.g * _Tint1) + (colorMask.b * _Tint2) + ((1.0 - colorMask.a) * _Tint3)).rgb * o.Albedo;
		o.Albedo = lerp(o.Albedo, c, t);
		#endif
	}




#if defined(_REVEALLAYERS)
   TEXTURE2D(_RevealMask);
   SAMPLER(sampler_RevealMask);
   TEXTURE2D(_LayerMask);
   SAMPLER(sampler_LayerMask);
   TEXTURE2D(_Layer0);
   SAMPLER(sampler_Layer0);
   TEXTURE2D(_Layer0NormalMap);
   SAMPLER(sampler_Layer0NormalMap);
   TEXTURE2D(_Layer1);
   SAMPLER(sampler_Layer1);
   TEXTURE2D(_Layer1NormalMap);
   SAMPLER(sampler_Layer1NormalMap);

	float3 RevealMaskNormalCrossFilter(float2 uv, float height, float2 texelSize)
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
#endif

	void Ext_SurfaceFunction3 (inout Surface o, inout ShaderData d)
	{
		#if defined(_REVEALLAYERS)
		
		half4 revealMask = SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, d.blackboard.baseuv);
		half4 layerMask = SAMPLE_TEXTURE2D(_LayerMask, sampler_LayerMask, d.blackboard.baseuv);

		float2 layer0uv = d.texcoord0.xy * _Layer0_ST.xy + _Layer0_ST.zw;
		float2 layer1uv = d.texcoord0.xy * _Layer1_ST.xy + _Layer1_ST.zw;

		half4 layer0col = SAMPLE_TEXTURE2D(_Layer0, sampler_Layer0, layer0uv);
		float3 layer0normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_Layer0NormalMap, sampler_Layer0NormalMap, layer0uv), _NormalStrength);

		float rt = saturate(revealMask.r * layerMask.r);
		o.Albedo = lerp(o.Albedo, layer0col.rgb, pow(rt, _LayerSurfaceExp.x));
		o.Normal = lerp(o.Normal, layer0normal, pow(rt, _LayerSurfaceExp.y));
		o.Metallic = lerp(o.Metallic, _Layer0Metallic, pow(rt, _LayerSurfaceExp.z));
		o.Smoothness = lerp(o.Smoothness, _Layer0Smoothness, pow(rt, _LayerSurfaceExp.w));

		half4 layer1col = SAMPLE_TEXTURE2D(_Layer1, sampler_Layer1, layer1uv);
		float3 layer1normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_Layer1NormalMap, sampler_Layer1NormalMap, layer1uv), _NormalStrength);

		float gt = saturate(revealMask.g * layerMask.g);
		o.Albedo = lerp(o.Albedo, layer1col.rgb, pow(gt, _LayerSurfaceExp.x));
		o.Normal = lerp(o.Normal, layer1normal, pow(gt, _LayerSurfaceExp.y));
		o.Metallic = lerp(o.Metallic, _Layer1Metallic, pow(gt, _LayerSurfaceExp.z));
		o.Smoothness = lerp(o.Smoothness, _Layer1Smoothness, pow(gt, _LayerSurfaceExp.w));

		float bt = saturate(revealMask.b * layerMask.b);
		float height = _Layer2Height * bt;
		float3 heightNormal = RevealMaskNormalCrossFilter(d.blackboard.baseuv, height, _RevealMask_TexelSize.xy);
		o.Normal = normalize(float3(o.Normal.rg + heightNormal.rg, o.Normal.b * heightNormal.b)); //Blend normal

		float at = saturate(revealMask.a * layerMask.a);
		o.Emission = ((1.0 - at) * o.Emission.rgb) + (at * _Layer3EmissionColor.rgb);

		#endif
	}




    void Ext_ModifyVertex4 (inout VertexData v, inout ExtraV2F d)
    {
        #if defined(_VERTEXOCCLUSION_ON)
        if(_Bitmask & (int)v.texcoord1.x)
        {
            v.vertex *= (0.0 / 0.0);
        }
        #endif
    }




    #if defined(_PROBEVOLUME_ON)
    TEXTURE3D(_ProbeVolumeShR);
    TEXTURE3D(_ProbeVolumeShG);
    TEXTURE3D(_ProbeVolumeShB);
    TEXTURE3D(_ProbeVolumeOcc);
    SAMPLER(sampler_ProbeVolumeShR);
    #endif

    void Ext_ModifyVertex5 (inout VertexData v, inout ExtraV2F d)
    {
        #if defined(_PROBEVOLUME_ON)
            float3 position = mul(_ProbeWorldToTexture, float4(TransformObjectToWorld(v.vertex.xyz), 1.0f)).xyz;
            float3 texCoord = (position - _ProbeVolumeMin.xyz) * _ProbeVolumeSizeInv;
            
            d.extraV2F3.xyz = texCoord; //position in space relative to the volume.
        #endif
    }

    void Ext_SurfaceFunction5 (inout Surface o, inout ShaderData d)
    {
        #if defined(_PROBEVOLUME_ON)
            float3 texCoord = d.extraV2F3.xyz;

            //unity_SHAr = SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord);
            //unity_SHAg = SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShG, texCoord);
            //unity_SHAb = SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShB, texCoord);
            //unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeOcc, texCoord);
            
            //Custom baked gi data
            o.DiffuseGI = SHEvalLinearL0L1( d.worldSpaceNormal, SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord),
                                            SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShR, texCoord), SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShR, texCoord));
            unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeShR, texCoord);
            o.ShadowMask = unity_ProbesOcclusion;
        #endif
    }




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                  Ext_SurfaceFunction1(l, d);
                  Ext_SurfaceFunction2(l, d);
                  Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                  Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                  Ext_ModifyVertex4(v, d);
                  Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                  v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
             d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            
         #if defined(SHADERPASS_SHADOWCASTER)
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if defined(SHADERPASS_SHADOWCASTER)
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif defined(SHADERPASS_META)
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif


          // #if %SCREENPOSREQUIREKEY%
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if defined(SHADERPASS_FORWARD) || (SHADERPASS == SHADERPASS_GBUFFER)
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              // o.texcoord1.xy = uv1;
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


            

            // fragment shader
            half4 Frag (VertexToPixel IN
               #if NEED_FACING
                  , bool facing : SV_IsFrontFace
               #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);

               ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );

               Surface l = (Surface)0;

               l.Albedo = half3(0.5, 0.5, 0.5);
               l.Normal = float3(0,0,1);
               l.Occlusion = 1;
               l.Alpha = 1;

               ChainSurfaceFunction(l, d);

               MetaInput metaInput = (MetaInput)0;
               metaInput.Albedo = l.Albedo;
               metaInput.Emission = l.Emission;

               return MetaFragment(metaInput);

            }

         ENDHLSL

      }


      
        Pass
        {
            Name "DepthNormals"
            Tags
            {
               "LightMode" = "DepthNormals"
            }
    
            // Render State
            Cull Back
            Blend One Zero
            ZTest LEqual
            ZWrite On

               Blend[_SrcBlend][_DstBlend]
   ZWrite[_ZWrite]
   Cull [_CullMode]


            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
        
            #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
            #define _PASSDEPTH 1
            #define _PASSDEPTHNORMALS 1


            


   #pragma shader_feature_local_fragment _ALPHATEST_ON
   #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
   #pragma shader_feature_local_fragment _ _DETAIL
   #pragma shader_feature_local_fragment _ _EMISSION


   //#pragma multi_compile_local_fragment _ _COLORMASK_ON
   #pragma shader_feature_local_fragment _ _COLORMASK_ON


   #pragma multi_compile_local_fragment _ _REVEALLAYERS
   //#pragma shader_feature_local_fragment _ _REVEALLAYERS


   #pragma multi_compile_local_vertex _ _VERTEXOCCLUSION_ON


   #pragma multi_compile_local _ _PROBEVOLUME_ON
   //#pragma shader_feature_local _ _PROBEVOLUME_ON

   #if defined(_PROBEVOLUME_ON)
       #define _OVERRIDE_BAKEDGI
   #endif


   #define _URP 1


            // this has to be here or specular color will be ignored. Not in SG code
            #if _SIMPLELIT
               #define _SPECULAR_COLOR
            #endif


            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"


        

               #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
     
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D

      

      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCCOORD3;
         // float4 texcoord1 : TEXCCOORD4;
         // float4 texcoord2 : TEXCCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
          float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
          float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif
            float4 fogFactorAndVertexLight : TEXCOORD10;
            float4 shadowCoord : TEXCOORD11;
         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
      };


         
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;
            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
	float2 baseuv;


                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // uint vertexID : SV_VertexID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // would love to strip these, but they are used in certain
               // combinations of the lighting system, and may be used
               // by the user as well, so no easy way to strip them.

               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetObjectToWorldMatrix()
               #undef GetWorldToObjectMatrix()
               #undef GetWorldToViewMatrix()
               #undef UNITY_MATRIX_I_V
               #undef UNITY_MATRIX_P
               #undef GetWorldToHClipMatrix()
               #undef GetObjectToWorldMatrix()V
               #undef UNITY_MATRIX_T_MV
               #undef UNITY_MATRIX_IT_MV
               #undef GetObjectToWorldMatrix()VP

               #define GetObjectToWorldMatrix()     unity_ObjectToWorld
               #define GetWorldToObjectMatrix()   unity_WorldToObject
               #define GetWorldToViewMatrix()     unity_MatrixV
               #define UNITY_MATRIX_I_V   unity_MatrixInvV
               #define GetViewToHClipMatrix()     OptimizeProjectionMatrix(glstate_matrix_projection)
               #define GetWorldToHClipMatrix()    unity_MatrixVP
               #define GetObjectToWorldMatrix()V    mul(GetWorldToViewMatrix(), GetObjectToWorldMatrix())
               #define UNITY_MATRIX_T_MV  transpose(GetObjectToWorldMatrix()V)
               #define UNITY_MATRIX_IT_MV transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V))
               #define GetObjectToWorldMatrix()VP   mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix())


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }


            
         CBUFFER_START(UnityPerMaterial)

            



	half _AlphaClip;
	half _Cutoff;
	half _AlphaStrength;

	float4 _BaseMap_ST;
	half4 _BaseColor;
	half _NormalStrength;
	half _Smoothness;
	half _OcclusionStrength;
	float4 _EmissionColor;

	float4 _DetailAlbedoMap_ST;
	half _DetailAlbedoMapScale;
	half _DetailNormalMapScale;
	float4 _DetailWeightOverDistance;


	half4 _Tint0;
	half4 _Tint1;
	half4 _Tint2;
	half4 _Tint3;


	float4 _RevealMask_TexelSize;
	float4 _LayerSurfaceExp;

	half _Layer0NormalStrength;
	half _Layer0Smoothness;
	half _Layer0Metallic;
	half _Layer1NormalStrength;
	half _Layer1Smoothness;
	half _Layer1Metallic;
	half _Layer2Height;
	float4 _Layer3EmissionColor;

	float4 _Layer0_ST;
	float4 _Layer1_ST;


	int _Bitmask;


    float4x4 _ProbeWorldToTexture;
    float3 _ProbeVolumeMin;
    float3 _ProbeVolumeSizeInv;	



         CBUFFER_END

         

         

         #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()


    float Dither8x8Bayer( int x, int y )
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
        return dither[r] / 64; 
    }

    void ApplyDitherCrossFadeVSP(float2 vpos, float fadeValue)
    {
        float dither = Dither8x8Bayer( fmod(vpos.x, 8), fmod(vpos.y, 8) );
        float sgn = fadeValue > 0 ? 1.0f : -1.0f;
        clip(dither - (1-fadeValue) * sgn);
    }
    

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
		#if LOD_FADE_CROSSFADE
            float4 screenPosNorm = d.screenPos / d.screenPos.w;
            screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;
            float2 clipScreen = screenPosNorm.xy * _ScreenParams.xy;
            ApplyDitherCrossFadeVSP(clipScreen,unity_LODFade.x);
        #endif
	}




   TEXTURE2D(_BaseMap);
   SAMPLER(sampler_BaseMap);
   TEXTURE2D(_BumpMap);
   SAMPLER(sampler_BumpMap);
   TEXTURE2D(_MetallicGlossMap);
   SAMPLER(sampler_MetallicGlossMap);

   #if defined(_EMISSION)
   TEXTURE2D(_EmissionMap);
   SAMPLER(sampler_EmissionMap);
   #endif

   #if defined(_DETAIL)
   TEXTURE2D(_DetailAlbedoMap);
   SAMPLER(sampler_DetailAlbedoMap);
   TEXTURE2D(_DetailNormalMap);
   SAMPLER(sampler_DetailNormalMap);
   #endif

	float4 Unity_Blend_Overlay(float4 Base, float4 Blend, float Opacity)
	{
		float4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
		float4 result2 = 2.0 * Base * Blend;
		float4 zeroOrOne = step(Base, 0.5);
		float4 Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
		return lerp(Base, Out, Opacity);
	}

	float3 Unity_NormalBlend_Reoriented(float3 A, float3 B)
	{
		float3 t = A.xyz + float3(0.0, 0.0, 1.0);
		float3 u = B.xyz * float3(-1.0, -1.0, 1.0);
		return ((t / t.z) * dot(t, u) - u);
	}

	void Ext_SurfaceFunction1 (inout Surface o, inout ShaderData d)
	{
		float2 uv = d.texcoord0.xy * _BaseMap_ST.xy + _BaseMap_ST.zw;
		d.blackboard.baseuv = uv;
		half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);

		o.Albedo = albedo.rgb * _BaseColor.rgb;

		#if defined(_ALPHATEST_ON)
			clip(albedo.a - _Cutoff);
		#endif

		o.Alpha = albedo.a * _AlphaStrength;
		o.Normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, uv), _NormalStrength);

		float4 mask = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, uv);
        o.Metallic = mask.r;
        o.Occlusion = LerpWhiteTo(mask.g, _OcclusionStrength);
		float emissionMask = mask.b;
        o.Smoothness = mask.a * _Smoothness;

		half3 emission = 0;
		#if defined(_EMISSION)
           o.Emission = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, uv).rgb * _EmissionColor.rgb * emissionMask;
		#else
		   o.Emission = _EmissionColor.rgb * emissionMask;
		#endif

		#if defined(_DETAIL) && !defined(SHADER_API_MOBILE)
			float weight = 1.0;
		    if (_DetailWeightOverDistance.y + _DetailWeightOverDistance.w < 2)
            {
               float camDist = distance(d.worldSpacePosition, _WorldSpaceCameraPos);
               weight = lerp(_DetailWeightOverDistance.y, _DetailWeightOverDistance.w, saturate((camDist-_DetailWeightOverDistance.x) / _DetailWeightOverDistance.z));
            }

			float2 detailuv = d.texcoord0.xy * _DetailAlbedoMap_ST.xy + _DetailAlbedoMap_ST.zw;
			o.Albedo = Unity_Blend_Overlay(albedo, SAMPLE_TEXTURE2D(_DetailAlbedoMap, sampler_DetailAlbedoMap, detailuv), _DetailAlbedoMapScale * weight).rgb * _BaseColor.rgb;
			o.Normal = Unity_NormalBlend_Reoriented(o.Normal, UnpackScaleNormal(SAMPLE_TEXTURE2D(_DetailNormalMap, sampler_DetailNormalMap, detailuv), _DetailNormalMapScale * weight));
		#endif

		o.Height = albedo.a;
	}




#if defined(_COLORMASK_ON)
	TEXTURE2D(_ColorMask);
    SAMPLER(sampler_ColorMask);
#endif

	void Ext_SurfaceFunction2 (inout Surface o, inout ShaderData d)
	{
		#if defined(_COLORMASK_ON)
		float4 colorMask = SAMPLE_TEXTURE2D(_ColorMask, sampler_ColorMask, d.blackboard.baseuv);
		float t = colorMask.r + colorMask.g + colorMask.b + (1.0 - colorMask.a);
		float3 c = ((colorMask.r * _Tint0) + (colorMask.g * _Tint1) + (colorMask.b * _Tint2) + ((1.0 - colorMask.a) * _Tint3)).rgb * o.Albedo;
		o.Albedo = lerp(o.Albedo, c, t);
		#endif
	}




#if defined(_REVEALLAYERS)
   TEXTURE2D(_RevealMask);
   SAMPLER(sampler_RevealMask);
   TEXTURE2D(_LayerMask);
   SAMPLER(sampler_LayerMask);
   TEXTURE2D(_Layer0);
   SAMPLER(sampler_Layer0);
   TEXTURE2D(_Layer0NormalMap);
   SAMPLER(sampler_Layer0NormalMap);
   TEXTURE2D(_Layer1);
   SAMPLER(sampler_Layer1);
   TEXTURE2D(_Layer1NormalMap);
   SAMPLER(sampler_Layer1NormalMap);

	float3 RevealMaskNormalCrossFilter(float2 uv, float height, float2 texelSize)
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
#endif

	void Ext_SurfaceFunction3 (inout Surface o, inout ShaderData d)
	{
		#if defined(_REVEALLAYERS)
		
		half4 revealMask = SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, d.blackboard.baseuv);
		half4 layerMask = SAMPLE_TEXTURE2D(_LayerMask, sampler_LayerMask, d.blackboard.baseuv);

		float2 layer0uv = d.texcoord0.xy * _Layer0_ST.xy + _Layer0_ST.zw;
		float2 layer1uv = d.texcoord0.xy * _Layer1_ST.xy + _Layer1_ST.zw;

		half4 layer0col = SAMPLE_TEXTURE2D(_Layer0, sampler_Layer0, layer0uv);
		float3 layer0normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_Layer0NormalMap, sampler_Layer0NormalMap, layer0uv), _NormalStrength);

		float rt = saturate(revealMask.r * layerMask.r);
		o.Albedo = lerp(o.Albedo, layer0col.rgb, pow(rt, _LayerSurfaceExp.x));
		o.Normal = lerp(o.Normal, layer0normal, pow(rt, _LayerSurfaceExp.y));
		o.Metallic = lerp(o.Metallic, _Layer0Metallic, pow(rt, _LayerSurfaceExp.z));
		o.Smoothness = lerp(o.Smoothness, _Layer0Smoothness, pow(rt, _LayerSurfaceExp.w));

		half4 layer1col = SAMPLE_TEXTURE2D(_Layer1, sampler_Layer1, layer1uv);
		float3 layer1normal = UnpackScaleNormal(SAMPLE_TEXTURE2D(_Layer1NormalMap, sampler_Layer1NormalMap, layer1uv), _NormalStrength);

		float gt = saturate(revealMask.g * layerMask.g);
		o.Albedo = lerp(o.Albedo, layer1col.rgb, pow(gt, _LayerSurfaceExp.x));
		o.Normal = lerp(o.Normal, layer1normal, pow(gt, _LayerSurfaceExp.y));
		o.Metallic = lerp(o.Metallic, _Layer1Metallic, pow(gt, _LayerSurfaceExp.z));
		o.Smoothness = lerp(o.Smoothness, _Layer1Smoothness, pow(gt, _LayerSurfaceExp.w));

		float bt = saturate(revealMask.b * layerMask.b);
		float height = _Layer2Height * bt;
		float3 heightNormal = RevealMaskNormalCrossFilter(d.blackboard.baseuv, height, _RevealMask_TexelSize.xy);
		o.Normal = normalize(float3(o.Normal.rg + heightNormal.rg, o.Normal.b * heightNormal.b)); //Blend normal

		float at = saturate(revealMask.a * layerMask.a);
		o.Emission = ((1.0 - at) * o.Emission.rgb) + (at * _Layer3EmissionColor.rgb);

		#endif
	}




    void Ext_ModifyVertex4 (inout VertexData v, inout ExtraV2F d)
    {
        #if defined(_VERTEXOCCLUSION_ON)
        if(_Bitmask & (int)v.texcoord1.x)
        {
            v.vertex *= (0.0 / 0.0);
        }
        #endif
    }




    #if defined(_PROBEVOLUME_ON)
    TEXTURE3D(_ProbeVolumeShR);
    TEXTURE3D(_ProbeVolumeShG);
    TEXTURE3D(_ProbeVolumeShB);
    TEXTURE3D(_ProbeVolumeOcc);
    SAMPLER(sampler_ProbeVolumeShR);
    #endif

    void Ext_ModifyVertex5 (inout VertexData v, inout ExtraV2F d)
    {
        #if defined(_PROBEVOLUME_ON)
            float3 position = mul(_ProbeWorldToTexture, float4(TransformObjectToWorld(v.vertex.xyz), 1.0f)).xyz;
            float3 texCoord = (position - _ProbeVolumeMin.xyz) * _ProbeVolumeSizeInv;
            
            d.extraV2F3.xyz = texCoord; //position in space relative to the volume.
        #endif
    }

    void Ext_SurfaceFunction5 (inout Surface o, inout ShaderData d)
    {
        #if defined(_PROBEVOLUME_ON)
            float3 texCoord = d.extraV2F3.xyz;

            //unity_SHAr = SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord);
            //unity_SHAg = SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShG, texCoord);
            //unity_SHAb = SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShB, texCoord);
            //unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeOcc, texCoord);
            
            //Custom baked gi data
            o.DiffuseGI = SHEvalLinearL0L1( d.worldSpaceNormal, SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord),
                                            SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShR, texCoord), SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShR, texCoord));
            unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeShR, texCoord);
            o.ShadowMask = unity_ProbesOcclusion;
        #endif
    }




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                  Ext_SurfaceFunction1(l, d);
                  Ext_SurfaceFunction2(l, d);
                  Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                  Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                  Ext_ModifyVertex4(v, d);
                  Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                  v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
                v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



         

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
             d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

         
         #if defined(SHADERPASS_SHADOWCASTER)
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if defined(SHADERPASS_SHADOWCASTER)
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif defined(SHADERPASS_META)
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif


          // #if %SCREENPOSREQUIREKEY%
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if defined(SHADERPASS_FORWARD) || (SHADERPASS == SHADERPASS_GBUFFER)
              float2 uv1 = v.texcoord1.xy;
              OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
              // o.texcoord1.xy = uv1;
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
             o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


         

         // fragment shader
         half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
         ) : SV_Target
         {
           UNITY_SETUP_INSTANCE_ID(IN);
           UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

           ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );
           Surface l = (Surface)0;

           #ifdef _DEPTHOFFSET_ON
              l.outputDepth = outputDepth;
           #endif

           l.Albedo = half3(0.5, 0.5, 0.5);
           l.Normal = float3(0,0,1);
           l.Occlusion = 1;
           l.Alpha = 1;

           ChainSurfaceFunction(l, d);

           #ifdef _DEPTHOFFSET_ON
              outputDepth = l.outputDepth;
           #endif

            #if _WORLDSPACENORMAL
              l.Normal = l.Normal;
            #else
              l.Normal = normalize(TangentToWorldSpace(d, l.Normal));
            #endif

           return float4(PackNormalOctRectEncode(TransformWorldToViewDir(l.Normal, true)), 0.0, 0.0);

         }

         ENDHLSL

      }


      












      

   }
   
   
   CustomEditor "ThunderRoadLitShader"
}
