////////////////////////////////////////
// Generated with Better Shaders - but very very hand edited
//   Unity Version: 2021.3.14f1
//   Render Pipeline: URP2021
//   Platform: WindowsEditor
////////////////////////////////////////

Shader "ThunderRoad/Lit"
{
   Properties
   {
      [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
      [HideInInspector]_QueueControl("_QueueControl", Float) = -1
      [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
      [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
      [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}

      [Toggle]_DebugView("Debug View", Float) = 0

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

      [MainTexture]_BaseMap("Base Map", 2D) = "white" {}
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

      _QueueOffset("Queue offset", Float) = 0.0

      [HideInInspector] _MainTex("BaseMap", 2D) = "white" {} 

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

      [Group(Layer0 Red)]_Layer0("Layer0 (R)", 2D) = "black" {}
      [Group(Layer0 Red)][Normal][NoScaleOffset]_Layer0NormalMap("Layer0 (R) Normal", 2D) = "bump" {}
      [Group(Layer0 Red)]_Layer0NormalStrength("Layer0 (R) Normal Strength", Range(0,2)) = 1
      [Group(Layer0 Red)]_Layer0Smoothness("Layer0 (R) Smoothness", Range(0,1)) = 0.5
      [Group(Layer0 Red)]_Layer0Metallic("Layer0 (R) Metallic", Range(0,1)) = 0

      [Group(Layer1 Green)]_Layer1("Layer1 (G)", 2D) = "black" {}
      [Group(Layer1 Green)][Normal][NoScaleOffset]_Layer1NormalMap("Layer1 (G) Normal", 2D) = "bump" {}
      [Group(Layer1 Green)]_Layer1NormalStrength("Layer1 (G) Normal Strength", Range(0,2)) = 1
      [Group(Layer1 Green)]_Layer1Smoothness("Layer1 (G) Smoothness", Range(0,1)) = 0.5
      [Group(Layer1 Green)]_Layer1Metallic("Layer1 (G) Metallic", Range(0,1)) = 0

      [Group(Layer2 Blue)]_Layer2Height("Layer2 (B) Height", Range(-20,20)) = -1

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

      _VertexColorIntensity ("Vertex Color Intensity", Range(0.0, 1.0)) = 0.0

   }
   SubShader
   {
      Tags
      {
         "RenderPipeline"="UniversalPipeline" "RenderType" = "Opaque" "UniversalMaterialType" = "Lit" "Queue" = "Geometry"
      }

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
         #pragma multi_compile_fog //we shouldnt need FOG_EXP, FOG_EXP2 but  switching this for #define FOG_LINEAR breaks the mirror in home
         #pragma skip_variants FOG_EXP FOG_EXP2 // attempt tp skip the fog keywords we dont want
         #pragma multi_compile_instancing
         #pragma instancing_options renderinglayer

         //#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
         #pragma multi_compile _ LIGHTMAP_ON
         //#pragma multi_compile _ DYNAMICLIGHTMAP_ON // I dont think we use dynamic lightmaps? (enlighten)
         #pragma multi_compile _ DIRLIGHTMAP_COMBINED
         #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
         #pragma multi_compile _ _ADDITIONAL_LIGHTS
         #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
         #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
         #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
         #pragma multi_compile_fragment _ _SHADOWS_SOFT
         #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
         #pragma multi_compile _ SHADOWS_SHADOWMASK
         //#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3 // this doesnt use decals
         #pragma multi_compile_fragment _ _LIGHT_LAYERS
         #pragma multi_compile_fragment _ DEBUG_DISPLAY
         //#pragma multi_compile_fragment _ _LIGHT_COOKIES // no light cookies until 1.0
         #pragma multi_compile _ _CLUSTERED_RENDERING

         #define SHADER_PASS SHADERPASS_FORWARD
         #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
         #define _PASSFORWARD 1
         #define _FOG_FRAGMENT 1

         //#pragma multi_compile_local _ LOD_FADE_CROSSFADE

         #pragma shader_feature_local_fragment _ALPHATEST_ON
         #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
         #pragma shader_feature_local_fragment _ _DETAIL
         #pragma shader_feature_local_fragment _ _EMISSION

         #pragma shader_feature_local_fragment _ _COLORMASK_ON

         #pragma multi_compile_local_fragment _ _REVEALLAYERS

         #pragma multi_compile_local_vertex _ _VERTEXOCCLUSION_ON

         #pragma multi_compile_local _ _PROBEVOLUME_ON

         #if defined(_PROBEVOLUME_ON)
            #define _OVERRIDE_BAKEDGI
         #endif

         #define _URP 1
         #define _USINGTEXCOORD1 1
         

         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

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

         #include "ThunderRoad_Lit_functions.hlsl"
         
         VertexToPixel Vert(VertexData v)
         {
            VertexToPixel o = (VertexToPixel)0;

            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_TRANSFER_INSTANCE_ID(v, o);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            #if !_TESSELLATION_ON
               VertexFunction(v, o);//, _Time);
            #endif

            o.texcoord0 = v.texcoord0;
            o.worldPos = TransformObjectToWorld(v.vertex.xyz);
            o.worldNormal = TransformObjectToWorldNormal(v.normal);
            o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);
            o.pos = TransformWorldToHClip(o.worldPos);
            o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);

            #if _PASSFORWARD || _PASSGBUFFER
               float2 uv1 = v.texcoord1.xy;
               OUTPUT_LIGHTMAP_UV(uv1, unity_LightmapST, o.lightmapUV);
               OUTPUT_SH(o.worldNormal, o.sh);
               #if defined(DYNAMICLIGHTMAP_ON)
                  o.dynamicLightmapUV.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
               #endif
            #endif

            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
               half fogFactor = 0;
               #if defined(_FOG_FRAGMENT)
                  fogFactor = ComputeFogFactor(o.pos.z);
               #endif
               #if _BAKEDLIT
                  o.fogFactorAndVertexLight = half4(fogFactor, 0, 0, 0);
               #else
                  half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
                  o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
               #endif
            #endif

            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
               VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
               o.shadowCoord = GetShadowCoord(vertexInput);
            #endif

            return o;
         }

         #if _UNLIT
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Unlit.hlsl"  
         #endif

         half4 Frag(VertexToPixel IN) : SV_Target
         {
            UNITY_SETUP_INSTANCE_ID(IN);
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

            ShaderData d = CreateShaderData(IN);
            Surface l = (Surface)0;
            
            l.Albedo = half3(0.5, 0.5, 0.5);
            l.Normal = float3(0, 0, 1);
            l.Occlusion = 1;
            l.Alpha = 1;

            SurfaceFunction(l, d);
            
            float metallic = l.Metallic;
            
            InputData inputData = (InputData)0;

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

            #if _BAKEDLIT
               inputData.fogCoord = IN.fogFactorAndVertexLight.x;
               inputData.vertexLighting = 0;
            #else
               inputData.fogCoord = InitializeInputDataFog(float4(IN.worldPos, 1.0), IN.fogFactorAndVertexLight.x);
               inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
            #endif

            #if defined(_OVERRIDE_BAKEDGI)
               inputData.bakedGI = l.DiffuseGI;
               l.Emission += l.SpecularGI;
            #elif _BAKEDLIT
               inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.sh, inputData.normalWS);
            #else
               #if defined(DYNAMICLIGHTMAP_ON)
                  inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.dynamicLightmapUV.xy, IN.sh, inputData.normalWS);
               #else
                  inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.sh, inputData.normalWS);
               #endif
            #endif
            inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.pos);
            #if !_BAKEDLIT
               inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUV);

               #if defined(_OVERRIDE_SHADOWMASK)
                  float4 mulColor = saturate(dot(l.ShadowMask, _MainLightOcclusionProbes)); 
                  inputData.shadowMask = mulColor;
               #endif
            #else
               inputData.shadowMask = float4(1,1,1,1);
            #endif

            #if defined(DEBUG_DISPLAY)
               #if defined(DYNAMICLIGHTMAP_ON)
                  inputData.dynamicLightmapUV = IN.dynamicLightmapUV.xy;
               #endif
               #if defined(LIGHTMAP_ON)
                  inputData.staticLightmapUV = IN.lightmapUV;
               #else
                  inputData.vertexSH = IN.sh;
               #endif
            #endif

            #if _WORLDSPACENORMAL
               float3 normalTS = WorldToTangentSpace(d, l.Normal);
            #else
               float3 normalTS = l.Normal;
            #endif

            SurfaceData surface = (SurfaceData)0;
            surface.albedo = l.Albedo;
            surface.metallic = saturate(metallic);
            surface.specular = 0;
            surface.smoothness = saturate(l.Smoothness),
            surface.occlusion = l.Occlusion,
            surface.emission = l.Emission,
            surface.alpha = saturate(l.Alpha);
            surface.clearCoatMask = 0;
            surface.clearCoatSmoothness = 1;
            
            #if !_UNLIT
               half4 color = half4(l.Albedo, l.Alpha);
               #ifdef _DBUFFER
                  #if _BAKEDLIT
                     ApplyDecalToBaseColorAndNormal(IN.pos, color, inputData.normalWS);
                  #else
                     ApplyDecalToSurfaceData(IN.pos, surface, inputData);
                  #endif
               #endif
               #if _SIMPLELIT
                  color = UniversalFragmentBlinnPhong(
                  inputData,
                  surface);
               #elif _BAKEDLIT
                  color = UniversalFragmentBakedLit(inputData, color.rgb, color.a, normalTS);
               #else
                  color = UniversalFragmentPBR(inputData, surface);
               #endif

               #if !DISABLEFOG
                  color.rgb = MixFog(color.rgb, inputData.fogCoord);
               #endif

            #else 
               #ifdef _DBUFFER
                  ApplyDecalToSurfaceData(IN.pos, surface, inputData);
               #endif
               half4 color = UniversalFragmentUnlit(inputData, l.Albedo, l.Alpha);
               #if !DISABLEFOG
                  color.rgb = MixFog(color.rgb, inputData.fogCoord);
               #endif
            #endif
            
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
         #pragma multi_compile_instancing

         #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

         #define _NORMAL_DROPOFF_TS 1
         #define ATTRIBUTES_NEED_NORMAL
         #define ATTRIBUTES_NEED_TANGENT
         #define _PASSSHADOW 1

         //#pragma multi_compile_local _ LOD_FADE_CROSSFADE

         #pragma shader_feature_local_fragment _ALPHATEST_ON
         #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
         // #pragma shader_feature_local_fragment _ _DETAIL
         // #pragma shader_feature_local_fragment _ _EMISSION
         //
         // #pragma shader_feature_local_fragment _ _COLORMASK_ON
         //
         // #pragma multi_compile_local_fragment _ _REVEALLAYERS

         #pragma multi_compile_local_vertex _ _VERTEXOCCLUSION_ON

         #pragma multi_compile_local_vertex _ _PROBEVOLUME_ON

         #if defined(_PROBEVOLUME_ON)
            #define _OVERRIDE_BAKEDGI
         #endif

         #define _URP 1
         #define _USINGTEXCOORD1 1

         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

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

         #include "ThunderRoad_Lit_functions.hlsl"
         
         VertexToPixel Vert(VertexData v)
         {
            VertexToPixel o = (VertexToPixel)0;

            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_TRANSFER_INSTANCE_ID(v, o);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            #if !_TESSELLATION_ON
               VertexFunction(v, o);//, _Time);
            #endif

            o.texcoord0 = v.texcoord0;
            o.worldPos = TransformObjectToWorld(v.vertex.xyz);
            o.worldNormal = TransformObjectToWorldNormal(v.normal);
            o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);

            #if _PASSSHADOW
               #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                  float3 lightDirectionWS = normalize(_LightPosition - o.worldPos);
               #else
                  float3 lightDirectionWS = _LightDirection;
               #endif
               o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, lightDirectionWS));
               #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
               #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
               #endif
            #endif

            o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
            
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
               VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
               o.shadowCoord = GetShadowCoord(vertexInput);
            #endif

            return o;
         }

         half4 Frag(VertexToPixel IN) : SV_Target
         {
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

         Cull Off

         Blend[_SrcBlend][_DstBlend]
         ZWrite[_ZWrite]
         Cull [_CullMode]

         HLSLPROGRAM
         #pragma vertex Vert
         #pragma fragment Frag

         #pragma target 3.0

         #pragma prefer_hlslcc gles
         #pragma exclude_renderers d3d11_9x

         #define SHADERPASS SHADERPASS_META
         #define _PASSMETA 1

         //#pragma multi_compile_local _ LOD_FADE_CROSSFADE

         #pragma shader_feature_local_fragment _ALPHATEST_ON
         #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
         #pragma shader_feature_local_fragment _ _DETAIL
         #pragma shader_feature_local_fragment _ _EMISSION

         #pragma shader_feature_local_fragment _ _COLORMASK_ON

         #pragma multi_compile_local_fragment _ _REVEALLAYERS

         #pragma multi_compile_local_vertex _ _VERTEXOCCLUSION_ON

         #pragma multi_compile_local _ _PROBEVOLUME_ON

         #if defined(_PROBEVOLUME_ON)
            #define _OVERRIDE_BAKEDGI
         #endif

         #define _URP 1
         #define _USINGTEXCOORD1 1

         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
         #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
         #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
         #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

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

         #include "ThunderRoad_Lit_functions.hlsl"
         
         VertexToPixel Vert(VertexData v)
         {
            VertexToPixel o = (VertexToPixel)0;

            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_TRANSFER_INSTANCE_ID(v, o);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            #if !_TESSELLATION_ON
               VertexFunction(v, o);//, _Time);
            #endif

            o.texcoord0 = v.texcoord0;
            o.worldPos = TransformObjectToWorld(v.vertex.xyz);
            o.worldNormal = TransformObjectToWorldNormal(v.normal);
            o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);
            
            #if _PASSMETA
               o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST,
               unity_DynamicLightmapST);
            #endif

            o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
            
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
               VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
               o.shadowCoord = GetShadowCoord(vertexInput);
            #endif

            return o;
         }

         half4 Frag(VertexToPixel IN) : SV_Target
         {
            UNITY_SETUP_INSTANCE_ID(IN);

            ShaderData d = CreateShaderData(IN);

            Surface l = (Surface)0;

            l.Albedo = half3(0.5, 0.5, 0.5);
            l.Normal = float3(0, 0, 1);
            l.Occlusion = 1;
            l.Alpha = 1;

            SurfaceFunction(l, d);

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
         #pragma multi_compile_fog //we shouldnt need FOG_EXP, FOG_EXP2 but  switching this for #define FOG_LINEAR breaks the mirror in home
         #pragma skip_variants FOG_EXP FOG_EXP2 // attempt tp skip the fog keywords we dont want
         #pragma multi_compile_instancing
         //#pragma multi_compile _ DOTS_INSTANCING_ON

         #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
         #define _PASSDEPTH 1
         #define _PASSDEPTHNORMALS 1

         //#pragma multi_compile_local _ LOD_FADE_CROSSFADE

         #pragma shader_feature_local_fragment _ALPHATEST_ON
         #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
         #pragma shader_feature_local_fragment _ _DETAIL
         //#pragma shader_feature_local_fragment _ _EMISSION //not used in this pass

         //#pragma shader_feature_local_fragment _ _COLORMASK_ON //not used in this pass

         #pragma multi_compile_local_fragment _ _REVEALLAYERS

         #pragma multi_compile_local_vertex _ _VERTEXOCCLUSION_ON

         //#pragma multi_compile_local _ _PROBEVOLUME_ON //not used for normals

         #if defined(_PROBEVOLUME_ON)
            #define _OVERRIDE_BAKEDGI
         #endif

         #define _URP 1
         #define _USINGTEXCOORD1 1
         

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

         #include "ThunderRoad_Lit_functions.hlsl"
         
         VertexToPixel Vert(VertexData v)
         {
            VertexToPixel o = (VertexToPixel)0;

            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_TRANSFER_INSTANCE_ID(v, o);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            #if !_TESSELLATION_ON
               VertexFunction(v, o);//, _Time);
            #endif

            o.texcoord0 = v.texcoord0;
            o.worldPos = TransformObjectToWorld(v.vertex.xyz);
            o.worldNormal = TransformObjectToWorldNormal(v.normal);
            o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);
            o.pos = TransformWorldToHClip(o.worldPos);
            o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
            
            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
               VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
               o.shadowCoord = GetShadowCoord(vertexInput);
            #endif

            return o;
         }

         half4 Frag(VertexToPixel IN) : SV_Target
         {
            UNITY_SETUP_INSTANCE_ID(IN);
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

            ShaderData d = CreateShaderData(IN);
            Surface l = (Surface)0;
            
            l.Albedo = half3(0.5, 0.5, 0.5);
            l.Normal = float3(0, 0, 1);
            l.Occlusion = 1;
            l.Alpha = 1;

            SurfaceFunction(l, d);
            
            float3 wsn = l.Normal;
            #if !_WORLDSPACENORMAL
               wsn = TangentToWorldSpace(d, l.Normal);
            #endif
            return half4(NormalizeNormalPerPixel(wsn), 0.0);
         }
         ENDHLSL

      }
   }

   CustomEditor "ThunderRoadLitShader"
}