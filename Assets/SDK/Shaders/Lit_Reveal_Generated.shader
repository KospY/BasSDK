Shader "ThunderRoad/Generated/Lit_Reveal"
{
    Properties
    {
        [NoScaleOffset] _BaseMap("Base Map", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (0, 0, 0, 0)
        [NoScaleOffset]_BumpMap("Normal Map", 2D) = "bump" {}
        [NoScaleOffset]_MetallicGlossMap("Metallic (R) Occlusion (G) Emission Mask (B) Smoothness (A)", 2D) = "white" {}
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _OcclusionStrength("Occlusion Strength", Range(0, 1)) = 1
        [HDR]_EmissionColor("Emission Color", Color) = (0, 0, 0, 0)
        [NoScaleOffset]_EmissionMap("Emission Map", 2D) = "black" {}
        [NoScaleOffset]_ColorMask("Color Mask", 2D) = "white" {}
        _Tint0("Tint0 (R)", Color) = (1, 1, 1, 0)
        _Tint1("Tint1 (G)", Color) = (1, 1, 1, 0)
        _Tint2("Tint2 (B)", Color) = (1, 1, 1, 0)
        [NoScaleOffset]_RevealMask("Reveal Mask", 2D) = "black" {}
        [NoScaleOffset]_LayerMask("Layer Mask", 2D) = "white" {}
        [NoScaleOffset]_Layer0("Layer0 (R)", 2D) = "black" {}
        [NoScaleOffset]_Layer0NormalMap("Layer0 (R) Normal", 2D) = "bump" {}
        _Layer0NormalStrength("Layer0 (R) Normal Strength", Float) = 1
        _Layer0Tiling("Layer0 (R) Tiling", Vector) = (1, 1, 0, 0)
        _Layer0Smoothness("Layer0 (R) Smoothness", Range(0, 1)) = 0.5
        _Layer0Metallic("Layer0 (R) Metallic", Range(0, 1)) = 0
        [NoScaleOffset]_Layer1("Layer1 (G)", 2D) = "black" {}
        [NoScaleOffset]_Layer1NormalMap("Layer1 (G) Normal", 2D) = "bump" {}
        _Layer1NormalStrength("Layer1 (G) Normal Strength", Float) = 1
        _Layer1Tiling("Layer1 (G) Tiling", Vector) = (1, 1, 0, 0)
        _Layer1Smoothness("Layer1 (G) Smoothness", Range(0, 1)) = 0.5
        _Layer1Metallic("Layer1 (G) Metallic", Range(0, 1)) = 0
        _Layer2Height("Layer2 (B) Height", Range(-20, 20)) = -1
        [HDR]_Layer3EmissionColor("Layer3 (A) Emission Color", Color) = (0, 0, 0, 0)
        _Bitmask("Vertex Occlusion Bitmask", Float) = 0
        [Toggle]_VERTEXOCCLUSION("Vertex Occlusion", Float) = 0
        [Toggle]_EMISSIONMAP("Use Emission Map", Float) = 0
        [Toggle]_COLORMASK("Use Color Mask", Float) = 0
    }
        SubShader
        {
            Tags
            {
                "RenderPipeline" = "UniversalPipeline"
                "RenderType" = "Opaque"
                "Queue" = "Geometry+0"
            }

            Pass
            {
                Name "Universal Forward"
                Tags
                {
                    "LightMode" = "UniversalForward"
                }

            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            #pragma shader_feature_local _ _VERTEXOCCLUSION_ON
            #pragma shader_feature_local _ _EMISSIONMAP_ON
            #pragma shader_feature_local _ _COLORMASK_ON

            #if defined(_VERTEXOCCLUSION_ON) && defined(_EMISSIONMAP_ON) && defined(_COLORMASK_ON)
                #define KEYWORD_PERMUTATION_0
            #elif defined(_VERTEXOCCLUSION_ON) && defined(_EMISSIONMAP_ON)
                #define KEYWORD_PERMUTATION_1
            #elif defined(_VERTEXOCCLUSION_ON) && defined(_COLORMASK_ON)
                #define KEYWORD_PERMUTATION_2
            #elif defined(_VERTEXOCCLUSION_ON)
                #define KEYWORD_PERMUTATION_3
            #elif defined(_EMISSIONMAP_ON) && defined(_COLORMASK_ON)
                #define KEYWORD_PERMUTATION_4
            #elif defined(_EMISSIONMAP_ON)
                #define KEYWORD_PERMUTATION_5
            #elif defined(_COLORMASK_ON)
                #define KEYWORD_PERMUTATION_6
            #else
                #define KEYWORD_PERMUTATION_7
            #endif


            // Defines


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define _NORMALMAP 1
        #endif




            #define _NORMAL_DROPOFF_TS 1
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define ATTRIBUTES_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define ATTRIBUTES_NEED_TEXCOORD2
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define ATTRIBUTES_NEED_TEXCOORD3
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define VARYINGS_NEED_POSITION_WS 
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define VARYINGS_NEED_NORMAL_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define VARYINGS_NEED_TANGENT_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define VARYINGS_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define VARYINGS_NEED_TEXCOORD1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define VARYINGS_NEED_TEXCOORD2
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define VARYINGS_NEED_TEXCOORD3
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #endif



        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #endif


            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS_FORWARD

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
            float _Smoothness;
            float _OcclusionStrength;
            float4 _EmissionColor;
            float4 _Tint0;
            float4 _Tint1;
            float4 _Tint2;
            float _Layer0NormalStrength;
            float2 _Layer0Tiling;
            float _Layer0Smoothness;
            float _Layer0Metallic;
            float _Layer1NormalStrength;
            float2 _Layer1Tiling;
            float _Layer1Smoothness;
            float _Layer1Metallic;
            float _Layer2Height;
            float4 _Layer3EmissionColor;
            float _Bitmask;
            float4 _RevealMask_TexelSize;
            CBUFFER_END
            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
            TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
            TEXTURE2D(_MetallicGlossMap); SAMPLER(sampler_MetallicGlossMap); float4 _MetallicGlossMap_TexelSize;
            TEXTURE2D(_EmissionMap); SAMPLER(sampler_EmissionMap); float4 _EmissionMap_TexelSize;
            TEXTURE2D(_ColorMask); SAMPLER(sampler_ColorMask); float4 _ColorMask_TexelSize;
            TEXTURE2D(_RevealMask); SAMPLER(sampler_RevealMask);
            TEXTURE2D(_LayerMask); SAMPLER(sampler_LayerMask); float4 _LayerMask_TexelSize;
            TEXTURE2D(_Layer0); SAMPLER(sampler_Layer0); float4 _Layer0_TexelSize;
            TEXTURE2D(_Layer0NormalMap); SAMPLER(sampler_Layer0NormalMap); float4 _Layer0NormalMap_TexelSize;
            TEXTURE2D(_Layer1); SAMPLER(sampler_Layer1); float4 _Layer1_TexelSize;
            TEXTURE2D(_Layer1NormalMap); SAMPLER(sampler_Layer1NormalMap); float4 _Layer1NormalMap_TexelSize;
            SAMPLER(_SampleTexture2D_8605BC1D_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_7B9CCAC4_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_71F12517_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_AC574377_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_6F08E612_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_A2DE4CE7_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_DF0E076D_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_51B6E38C_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_D0D2AB3D_Sampler_3_Linear_Repeat);

            // Graph Functions

            void BitwiseAND_float(float a, float b, out float Out)
            {
                Out = ((int)a & (int)b);
            }

            struct Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363
            {
            };

            void SG_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363(float Vector1_E5A799E5, float Vector1_F006FE71, Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363 IN, out float OutVector1_1)
            {
                float _Property_39DA3013_Out_0 = Vector1_E5A799E5;
                float _Property_2853174C_Out_0 = Vector1_F006FE71;
                float _CustomFunction_2F22FDC6_Out_2;
                BitwiseAND_float(_Property_39DA3013_Out_0, _Property_2853174C_Out_0, _CustomFunction_2F22FDC6_Out_2);
                OutVector1_1 = _CustomFunction_2F22FDC6_Out_2;
            }

            void Unity_Comparison_Equal_float(float A, float B, out float Out)
            {
                Out = A == B ? 1 : 0;
            }

            void Unity_Divide_float(float A, float B, out float Out)
            {
                Out = A / B;
            }

            void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
            {
                Out = Predicate ? True : False;
            }

            struct Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05
            {
            };

            void SG_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05(float3 Vector3_725ECFC1, float Vector1_A7EE8494, float Vector1_9BC3020B, Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05 IN, out float3 OutVector4_1)
            {
                float _Property_E196FA79_Out_0 = Vector1_A7EE8494;
                float _Property_FFF31F1C_Out_0 = Vector1_9BC3020B;
                Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363 _bitwiseAND_FC16028;
                float _bitwiseAND_FC16028_OutVector1_1;
                SG_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363(_Property_E196FA79_Out_0, _Property_FFF31F1C_Out_0, _bitwiseAND_FC16028, _bitwiseAND_FC16028_OutVector1_1);
                float _Comparison_84D89BD3_Out_2;
                Unity_Comparison_Equal_float(_bitwiseAND_FC16028_OutVector1_1, 0, _Comparison_84D89BD3_Out_2);
                float3 _Property_A3E178F8_Out_0 = Vector3_725ECFC1;
                float _Divide_7A3B32D4_Out_2;
                Unity_Divide_float(0, 0, _Divide_7A3B32D4_Out_2);
                float3 _Branch_602423C2_Out_3;
                Unity_Branch_float3(_Comparison_84D89BD3_Out_2, _Property_A3E178F8_Out_0, (_Divide_7A3B32D4_Out_2.xxx), _Branch_602423C2_Out_3);
                OutVector4_1 = _Branch_602423C2_Out_3;
            }

            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }

            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
            }

            struct Bindings_ColorTintMask_6636e22a321cd404c89884028dd4f823
            {
                half4 uv0;
            };

            void SG_ColorTintMask_6636e22a321cd404c89884028dd4f823(TEXTURE2D_PARAM(Texture2D_917AA17E, samplerTexture2D_917AA17E), float4 Texture2D_917AA17E_TexelSize, float4 Vector4_553970CD, float4 Vector4_C861B211, float4 Vector4_ED9FE209, Bindings_ColorTintMask_6636e22a321cd404c89884028dd4f823 IN, out float4 OutVector4_1)
            {
                float4 _Property_526A61D_Out_0 = Vector4_553970CD;
                float4 _SampleTexture2D_8605BC1D_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_917AA17E, samplerTexture2D_917AA17E, IN.uv0.xy);
                float _SampleTexture2D_8605BC1D_R_4 = _SampleTexture2D_8605BC1D_RGBA_0.r;
                float _SampleTexture2D_8605BC1D_G_5 = _SampleTexture2D_8605BC1D_RGBA_0.g;
                float _SampleTexture2D_8605BC1D_B_6 = _SampleTexture2D_8605BC1D_RGBA_0.b;
                float _SampleTexture2D_8605BC1D_A_7 = _SampleTexture2D_8605BC1D_RGBA_0.a;
                float4 _Multiply_6497A688_Out_2;
                Unity_Multiply_float(_Property_526A61D_Out_0, (_SampleTexture2D_8605BC1D_R_4.xxxx), _Multiply_6497A688_Out_2);
                float4 _Property_3A437BCB_Out_0 = Vector4_C861B211;
                float4 _Multiply_385888A6_Out_2;
                Unity_Multiply_float(_Property_3A437BCB_Out_0, (_SampleTexture2D_8605BC1D_G_5.xxxx), _Multiply_385888A6_Out_2);
                float4 _Add_A3D53A8A_Out_2;
                Unity_Add_float4(_Multiply_6497A688_Out_2, _Multiply_385888A6_Out_2, _Add_A3D53A8A_Out_2);
                float4 _Property_DA82EA32_Out_0 = Vector4_ED9FE209;
                float4 _Multiply_BB15BEFC_Out_2;
                Unity_Multiply_float(_Property_DA82EA32_Out_0, (_SampleTexture2D_8605BC1D_B_6.xxxx), _Multiply_BB15BEFC_Out_2);
                float4 _Add_E0242413_Out_2;
                Unity_Add_float4(_Add_A3D53A8A_Out_2, _Multiply_BB15BEFC_Out_2, _Add_E0242413_Out_2);
                OutVector4_1 = _Add_E0242413_Out_2;
            }

            void Unity_NormalUnpack_float(float4 In, out float3 Out)
            {
                            Out = UnpackNormal(In);
                        }

            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }

            struct Bindings_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0
            {
                half4 uv0;
            };

            void SG_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0(float Vector1_74BD58D7, float Vector1_7BD83B87, TEXTURE2D_PARAM(Texture2D_1264D9E6, samplerTexture2D_1264D9E6), float4 Texture2D_1264D9E6_TexelSize, TEXTURE2D_PARAM(Texture2D_677C4338, samplerTexture2D_677C4338), float4 Texture2D_677C4338_TexelSize, float4 Vector4_6479F3E8, Bindings_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0 IN, out float metallic_1, out float occlusion_2, out float3 emission_3, out float smoothness_4)
            {
                float4 _SampleTexture2D_AC574377_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_677C4338, samplerTexture2D_677C4338, IN.uv0.xy);
                float _SampleTexture2D_AC574377_R_4 = _SampleTexture2D_AC574377_RGBA_0.r;
                float _SampleTexture2D_AC574377_G_5 = _SampleTexture2D_AC574377_RGBA_0.g;
                float _SampleTexture2D_AC574377_B_6 = _SampleTexture2D_AC574377_RGBA_0.b;
                float _SampleTexture2D_AC574377_A_7 = _SampleTexture2D_AC574377_RGBA_0.a;
                float _Property_8B465651_Out_0 = Vector1_74BD58D7;
                float _Multiply_DE2808F8_Out_2;
                Unity_Multiply_float(_SampleTexture2D_AC574377_G_5, _Property_8B465651_Out_0, _Multiply_DE2808F8_Out_2);
                float4 _SampleTexture2D_6F08E612_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_1264D9E6, samplerTexture2D_1264D9E6, IN.uv0.xy);
                float _SampleTexture2D_6F08E612_R_4 = _SampleTexture2D_6F08E612_RGBA_0.r;
                float _SampleTexture2D_6F08E612_G_5 = _SampleTexture2D_6F08E612_RGBA_0.g;
                float _SampleTexture2D_6F08E612_B_6 = _SampleTexture2D_6F08E612_RGBA_0.b;
                float _SampleTexture2D_6F08E612_A_7 = _SampleTexture2D_6F08E612_RGBA_0.a;
                float4 _Property_892842AC_Out_0 = Vector4_6479F3E8;
                float4 _Multiply_F33D45B4_Out_2;
                Unity_Multiply_float(_SampleTexture2D_6F08E612_RGBA_0, _Property_892842AC_Out_0, _Multiply_F33D45B4_Out_2);
                float4 _Multiply_3D94F4FB_Out_2;
                Unity_Multiply_float((_SampleTexture2D_AC574377_B_6.xxxx), _Property_892842AC_Out_0, _Multiply_3D94F4FB_Out_2);
                #if defined(_EMISSIONMAP_ON)
                float4 _UseEmissionMap_160345FE_Out_0 = _Multiply_F33D45B4_Out_2;
                #else
                float4 _UseEmissionMap_160345FE_Out_0 = _Multiply_3D94F4FB_Out_2;
                #endif
                float _Property_9A418755_Out_0 = Vector1_7BD83B87;
                float _Multiply_7FDD2068_Out_2;
                Unity_Multiply_float(_SampleTexture2D_AC574377_A_7, _Property_9A418755_Out_0, _Multiply_7FDD2068_Out_2);
                metallic_1 = _SampleTexture2D_AC574377_R_4;
                occlusion_2 = _Multiply_DE2808F8_Out_2;
                emission_3 = (_UseEmissionMap_160345FE_Out_0.xyz);
                smoothness_4 = _Multiply_7FDD2068_Out_2;
            }

            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }

            void Unity_Saturate_float(float In, out float Out)
            {
                Out = saturate(In);
            }

            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
            {
                Out = lerp(A, B, T);
            }

            void Unity_Lerp_float(float A, float B, float T, out float Out)
            {
                Out = lerp(A, B, T);
            }

            void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
            {
                Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
            }

            void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
            {
                Out = lerp(A, B, T);
            }

            void Unity_Normalize_float3(float3 In, out float3 Out)
            {
                Out = normalize(In);
            }

            struct Bindings_Layer_add1b3c672351c24c8c4119908f01e02
            {
                half4 uv0;
            };

            void SG_Layer_add1b3c672351c24c8c4119908f01e02(float4 Vector4_1ED74482, float3 Vector3_1358BB6D, float Vector1_89498AA5, float Vector1_E4A5BF8E, TEXTURE2D_PARAM(Texture2D_CB4BECC7, samplerTexture2D_CB4BECC7), float4 Texture2D_CB4BECC7_TexelSize, float2 Vector2_CF2204C5, TEXTURE2D_PARAM(Texture2D_70A11B0B, samplerTexture2D_70A11B0B), float4 Texture2D_70A11B0B_TexelSize, float Vector1_71A3EDBE, float Vector1_A7A3CDEE, float Vector1_A08FC73, float Vector1_F408666D, Bindings_Layer_add1b3c672351c24c8c4119908f01e02 IN, out float4 Albedo_1, out float Smoothness_2, out float Metallic_3, out float3 Normal_4)
            {
                float4 _Property_E0A8A88E_Out_0 = Vector4_1ED74482;
                float2 _Property_F2EA6A5A_Out_0 = Vector2_CF2204C5;
                float2 _TilingAndOffset_FDF9B562_Out_3;
                Unity_TilingAndOffset_float(IN.uv0.xy, _Property_F2EA6A5A_Out_0, float2 (0, 0), _TilingAndOffset_FDF9B562_Out_3);
                float4 _SampleTexture2D_51B6E38C_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_CB4BECC7, samplerTexture2D_CB4BECC7, _TilingAndOffset_FDF9B562_Out_3);
                float _SampleTexture2D_51B6E38C_R_4 = _SampleTexture2D_51B6E38C_RGBA_0.r;
                float _SampleTexture2D_51B6E38C_G_5 = _SampleTexture2D_51B6E38C_RGBA_0.g;
                float _SampleTexture2D_51B6E38C_B_6 = _SampleTexture2D_51B6E38C_RGBA_0.b;
                float _SampleTexture2D_51B6E38C_A_7 = _SampleTexture2D_51B6E38C_RGBA_0.a;
                float _Property_EFE4636B_Out_0 = Vector1_F408666D;
                float _Saturate_783545D1_Out_1;
                Unity_Saturate_float(_Property_EFE4636B_Out_0, _Saturate_783545D1_Out_1);
                float4 _Lerp_B0BF9BA8_Out_3;
                Unity_Lerp_float4(_Property_E0A8A88E_Out_0, _SampleTexture2D_51B6E38C_RGBA_0, (_Saturate_783545D1_Out_1.xxxx), _Lerp_B0BF9BA8_Out_3);
                float _Property_A8236A77_Out_0 = Vector1_89498AA5;
                float _Property_9DEC1A03_Out_0 = Vector1_A08FC73;
                float _Lerp_CDE58F3A_Out_3;
                Unity_Lerp_float(_Property_A8236A77_Out_0, _Property_9DEC1A03_Out_0, _Saturate_783545D1_Out_1, _Lerp_CDE58F3A_Out_3);
                float _Property_B5095A31_Out_0 = Vector1_E4A5BF8E;
                float _Property_5FFE3307_Out_0 = Vector1_A7A3CDEE;
                float _Lerp_3EF7B8D1_Out_3;
                Unity_Lerp_float(_Property_B5095A31_Out_0, _Property_5FFE3307_Out_0, _Saturate_783545D1_Out_1, _Lerp_3EF7B8D1_Out_3);
                float3 _Property_2D135E56_Out_0 = Vector3_1358BB6D;
                float4 _SampleTexture2D_D0D2AB3D_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_70A11B0B, samplerTexture2D_70A11B0B, _TilingAndOffset_FDF9B562_Out_3);
                float _SampleTexture2D_D0D2AB3D_R_4 = _SampleTexture2D_D0D2AB3D_RGBA_0.r;
                float _SampleTexture2D_D0D2AB3D_G_5 = _SampleTexture2D_D0D2AB3D_RGBA_0.g;
                float _SampleTexture2D_D0D2AB3D_B_6 = _SampleTexture2D_D0D2AB3D_RGBA_0.b;
                float _SampleTexture2D_D0D2AB3D_A_7 = _SampleTexture2D_D0D2AB3D_RGBA_0.a;
                float3 _NormalUnpack_3906D491_Out_1;
                Unity_NormalUnpack_float(_SampleTexture2D_D0D2AB3D_RGBA_0, _NormalUnpack_3906D491_Out_1);
                float _Property_E1B61021_Out_0 = Vector1_71A3EDBE;
                float3 _NormalStrength_3F987ED_Out_2;
                Unity_NormalStrength_float(_NormalUnpack_3906D491_Out_1, _Property_E1B61021_Out_0, _NormalStrength_3F987ED_Out_2);
                float3 _Lerp_C7A67762_Out_3;
                Unity_Lerp_float3(_Property_2D135E56_Out_0, _NormalStrength_3F987ED_Out_2, (_Saturate_783545D1_Out_1.xxx), _Lerp_C7A67762_Out_3);
                float3 _Normalize_FE9E0F4E_Out_1;
                Unity_Normalize_float3(_Lerp_C7A67762_Out_3, _Normalize_FE9E0F4E_Out_1);
                Albedo_1 = _Lerp_B0BF9BA8_Out_3;
                Smoothness_2 = _Lerp_CDE58F3A_Out_3;
                Metallic_3 = _Lerp_3EF7B8D1_Out_3;
                Normal_4 = _Normalize_FE9E0F4E_Out_1;
            }

            // 6f775ce51a1f17b59a6a0ef37877cf5e
            #include "Assets/SDK/Shaders/SubGraphs/RevealMaskNormalCrossFilter.hlsl"

            void Unity_NormalBlend_float(float3 A, float3 B, out float3 Out)
            {
                Out = SafeNormalize(float3(A.rg + B.rg, A.b * B.b));
            }

            void Unity_OneMinus_float(float In, out float Out)
            {
                Out = 1 - In;
            }

            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }

            void Unity_Add_float3(float3 A, float3 B, out float3 Out)
            {
                Out = A + B;
            }

            // Graph Vertex
            struct VertexDescriptionInputs
            {
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 ObjectSpaceNormal;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 ObjectSpaceTangent;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 ObjectSpacePosition;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                float4 uv1;
                #endif
            };

            struct VertexDescription
            {
                float3 VertexPosition;
                float3 VertexNormal;
                float3 VertexTangent;
            };

            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                float _Property_64870DC_Out_0 = _Bitmask;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                float4 _UV_592331DC_Out_0 = IN.uv1;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05 _NaNVertexDivideByZero_13598E37;
                float3 _NaNVertexDivideByZero_13598E37_OutVector4_1;
                SG_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05(IN.ObjectSpacePosition, _Property_64870DC_Out_0, (_UV_592331DC_Out_0).x, _NaNVertexDivideByZero_13598E37, _NaNVertexDivideByZero_13598E37_OutVector4_1);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #if defined(_VERTEXOCCLUSION_ON)
                float3 _VertexOcclusion_EB96B2B1_Out_0 = _NaNVertexDivideByZero_13598E37_OutVector4_1;
                #else
                float3 _VertexOcclusion_EB96B2B1_Out_0 = IN.ObjectSpacePosition;
                #endif
                #endif
                description.VertexPosition = _VertexOcclusion_EB96B2B1_Out_0;
                description.VertexNormal = IN.ObjectSpaceNormal;
                description.VertexTangent = IN.ObjectSpaceTangent;
                return description;
            }

            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 uv0;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 uv1;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 uv2;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 uv3;
                #endif
            };

            struct SurfaceDescription
            {
                float3 Albedo;
                float3 Normal;
                float3 Emission;
                float Metallic;
                float Smoothness;
                float Occlusion;
                float Alpha;
                float AlphaClipThreshold;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                float4 _Property_AD19C48F_Out_0 = _Tint0;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                float4 _Property_93A6AEE3_Out_0 = _Tint1;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                float4 _Property_A543E979_Out_0 = _Tint2;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                Bindings_ColorTintMask_6636e22a321cd404c89884028dd4f823 _ColorTintMask_72CC80CD;
                _ColorTintMask_72CC80CD.uv0 = IN.uv0;
                float4 _ColorTintMask_72CC80CD_OutVector4_1;
                SG_ColorTintMask_6636e22a321cd404c89884028dd4f823(TEXTURE2D_ARGS(_ColorMask, sampler_ColorMask), _ColorMask_TexelSize, _Property_AD19C48F_Out_0, _Property_93A6AEE3_Out_0, _Property_A543E979_Out_0, _ColorTintMask_72CC80CD, _ColorTintMask_72CC80CD_OutVector4_1);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #if defined(_COLORMASK_ON)
                float4 _UseColorMask_E399B65A_Out_0 = _ColorTintMask_72CC80CD_OutVector4_1;
                #else
                float4 _UseColorMask_E399B65A_Out_0 = float4(1, 1, 1, 1);
                #endif
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 _SampleTexture2D_7B9CCAC4_RGBA_0 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv0.xy);
                float _SampleTexture2D_7B9CCAC4_R_4 = _SampleTexture2D_7B9CCAC4_RGBA_0.r;
                float _SampleTexture2D_7B9CCAC4_G_5 = _SampleTexture2D_7B9CCAC4_RGBA_0.g;
                float _SampleTexture2D_7B9CCAC4_B_6 = _SampleTexture2D_7B9CCAC4_RGBA_0.b;
                float _SampleTexture2D_7B9CCAC4_A_7 = _SampleTexture2D_7B9CCAC4_RGBA_0.a;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 _Property_4BBEEE49_Out_0 = _BaseColor;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 _Multiply_D2EA8F9D_Out_2;
                Unity_Multiply_float(_SampleTexture2D_7B9CCAC4_RGBA_0, _Property_4BBEEE49_Out_0, _Multiply_D2EA8F9D_Out_2);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 _Multiply_2695D335_Out_2;
                Unity_Multiply_float(_UseColorMask_E399B65A_Out_0, _Multiply_D2EA8F9D_Out_2, _Multiply_2695D335_Out_2);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 _SampleTexture2D_71F12517_RGBA_0 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv0.xy);
                float _SampleTexture2D_71F12517_R_4 = _SampleTexture2D_71F12517_RGBA_0.r;
                float _SampleTexture2D_71F12517_G_5 = _SampleTexture2D_71F12517_RGBA_0.g;
                float _SampleTexture2D_71F12517_B_6 = _SampleTexture2D_71F12517_RGBA_0.b;
                float _SampleTexture2D_71F12517_A_7 = _SampleTexture2D_71F12517_RGBA_0.a;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 _NormalUnpack_2BEA82FF_Out_1;
                Unity_NormalUnpack_float(_SampleTexture2D_71F12517_RGBA_0, _NormalUnpack_2BEA82FF_Out_1);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Property_5E8C46C8_Out_0 = _OcclusionStrength;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Property_F9DCB6F7_Out_0 = _Smoothness;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 _Property_2D04FE2D_Out_0 = _EmissionColor;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                Bindings_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0 _MetallicOcclusionEmissionSmoothness_2A14B742;
                _MetallicOcclusionEmissionSmoothness_2A14B742.uv0 = IN.uv0;
                float _MetallicOcclusionEmissionSmoothness_2A14B742_metallic_1;
                float _MetallicOcclusionEmissionSmoothness_2A14B742_occlusion_2;
                float3 _MetallicOcclusionEmissionSmoothness_2A14B742_emission_3;
                float _MetallicOcclusionEmissionSmoothness_2A14B742_smoothness_4;
                SG_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0(_Property_5E8C46C8_Out_0, _Property_F9DCB6F7_Out_0, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap), _EmissionMap_TexelSize, TEXTURE2D_ARGS(_MetallicGlossMap, sampler_MetallicGlossMap), _MetallicGlossMap_TexelSize, _Property_2D04FE2D_Out_0, _MetallicOcclusionEmissionSmoothness_2A14B742, _MetallicOcclusionEmissionSmoothness_2A14B742_metallic_1, _MetallicOcclusionEmissionSmoothness_2A14B742_occlusion_2, _MetallicOcclusionEmissionSmoothness_2A14B742_emission_3, _MetallicOcclusionEmissionSmoothness_2A14B742_smoothness_4);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float2 _Property_DAE65D7E_Out_0 = _Layer0Tiling;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Property_565C9782_Out_0 = _Layer0NormalStrength;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Property_CD37297_Out_0 = _Layer0Metallic;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Property_5FB4E89B_Out_0 = _Layer0Smoothness;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 _SampleTexture2D_A2DE4CE7_RGBA_0 = SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, IN.uv0.xy);
                float _SampleTexture2D_A2DE4CE7_R_4 = _SampleTexture2D_A2DE4CE7_RGBA_0.r;
                float _SampleTexture2D_A2DE4CE7_G_5 = _SampleTexture2D_A2DE4CE7_RGBA_0.g;
                float _SampleTexture2D_A2DE4CE7_B_6 = _SampleTexture2D_A2DE4CE7_RGBA_0.b;
                float _SampleTexture2D_A2DE4CE7_A_7 = _SampleTexture2D_A2DE4CE7_RGBA_0.a;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 _SampleTexture2D_DF0E076D_RGBA_0 = SAMPLE_TEXTURE2D(_LayerMask, sampler_LayerMask, IN.uv0.xy);
                float _SampleTexture2D_DF0E076D_R_4 = _SampleTexture2D_DF0E076D_RGBA_0.r;
                float _SampleTexture2D_DF0E076D_G_5 = _SampleTexture2D_DF0E076D_RGBA_0.g;
                float _SampleTexture2D_DF0E076D_B_6 = _SampleTexture2D_DF0E076D_RGBA_0.b;
                float _SampleTexture2D_DF0E076D_A_7 = _SampleTexture2D_DF0E076D_RGBA_0.a;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Multiply_BF4B5506_Out_2;
                Unity_Multiply_float(_SampleTexture2D_A2DE4CE7_R_4, _SampleTexture2D_DF0E076D_R_4, _Multiply_BF4B5506_Out_2);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                Bindings_Layer_add1b3c672351c24c8c4119908f01e02 _Layer_AC9D210F;
                _Layer_AC9D210F.uv0 = IN.uv0;
                float4 _Layer_AC9D210F_Albedo_1;
                float _Layer_AC9D210F_Smoothness_2;
                float _Layer_AC9D210F_Metallic_3;
                float3 _Layer_AC9D210F_Normal_4;
                SG_Layer_add1b3c672351c24c8c4119908f01e02(_Multiply_2695D335_Out_2, _NormalUnpack_2BEA82FF_Out_1, _MetallicOcclusionEmissionSmoothness_2A14B742_smoothness_4, _MetallicOcclusionEmissionSmoothness_2A14B742_metallic_1, TEXTURE2D_ARGS(_Layer0, sampler_Layer0), _Layer0_TexelSize, _Property_DAE65D7E_Out_0, TEXTURE2D_ARGS(_Layer0NormalMap, sampler_Layer0NormalMap), _Layer0NormalMap_TexelSize, _Property_565C9782_Out_0, _Property_CD37297_Out_0, _Property_5FB4E89B_Out_0, _Multiply_BF4B5506_Out_2, _Layer_AC9D210F, _Layer_AC9D210F_Albedo_1, _Layer_AC9D210F_Smoothness_2, _Layer_AC9D210F_Metallic_3, _Layer_AC9D210F_Normal_4);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float2 _Property_9097A2BA_Out_0 = _Layer1Tiling;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Property_D59CFE18_Out_0 = _Layer1NormalStrength;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Property_94544800_Out_0 = _Layer1Metallic;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Property_FEAF1E87_Out_0 = _Layer1Smoothness;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Multiply_50BAAB0D_Out_2;
                Unity_Multiply_float(_SampleTexture2D_A2DE4CE7_G_5, _SampleTexture2D_DF0E076D_G_5, _Multiply_50BAAB0D_Out_2);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                Bindings_Layer_add1b3c672351c24c8c4119908f01e02 _Layer_E43A5B0B;
                _Layer_E43A5B0B.uv0 = IN.uv0;
                float4 _Layer_E43A5B0B_Albedo_1;
                float _Layer_E43A5B0B_Smoothness_2;
                float _Layer_E43A5B0B_Metallic_3;
                float3 _Layer_E43A5B0B_Normal_4;
                SG_Layer_add1b3c672351c24c8c4119908f01e02(_Layer_AC9D210F_Albedo_1, _Layer_AC9D210F_Normal_4, _Layer_AC9D210F_Smoothness_2, _Layer_AC9D210F_Metallic_3, TEXTURE2D_ARGS(_Layer1, sampler_Layer1), _Layer1_TexelSize, _Property_9097A2BA_Out_0, TEXTURE2D_ARGS(_Layer1NormalMap, sampler_Layer1NormalMap), _Layer1NormalMap_TexelSize, _Property_D59CFE18_Out_0, _Property_94544800_Out_0, _Property_FEAF1E87_Out_0, _Multiply_50BAAB0D_Out_2, _Layer_E43A5B0B, _Layer_E43A5B0B_Albedo_1, _Layer_E43A5B0B_Smoothness_2, _Layer_E43A5B0B_Metallic_3, _Layer_E43A5B0B_Normal_4);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 _UV_90461049_Out_0 = IN.uv0;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Property_C8E3BC2E_Out_0 = _Layer2Height;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Multiply_64F3186B_Out_2;
                Unity_Multiply_float(_SampleTexture2D_DF0E076D_B_6, _Property_C8E3BC2E_Out_0, _Multiply_64F3186B_Out_2);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _TexelSize_CEAAF712_Width_0 = _RevealMask_TexelSize.z;
                float _TexelSize_CEAAF712_Height_2 = _RevealMask_TexelSize.w;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 _CustomFunction_DB257E4_Out_0;
                RevealMaskNormalCrossFilter_float((_UV_90461049_Out_0.xy), _Multiply_64F3186B_Out_2, _TexelSize_CEAAF712_Width_0, _TexelSize_CEAAF712_Height_2, _CustomFunction_DB257E4_Out_0);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 _NormalBlend_CA918F19_Out_2;
                Unity_NormalBlend_float(_Layer_E43A5B0B_Normal_4, _CustomFunction_DB257E4_Out_0, _NormalBlend_CA918F19_Out_2);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Multiply_16B99BE_Out_2;
                Unity_Multiply_float(_SampleTexture2D_A2DE4CE7_A_7, _SampleTexture2D_DF0E076D_A_7, _Multiply_16B99BE_Out_2);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _Saturate_1D60C27B_Out_1;
                Unity_Saturate_float(_Multiply_16B99BE_Out_2, _Saturate_1D60C27B_Out_1);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float _OneMinus_5C44DFC9_Out_1;
                Unity_OneMinus_float(_Saturate_1D60C27B_Out_1, _OneMinus_5C44DFC9_Out_1);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 _Multiply_8B9DEC1C_Out_2;
                Unity_Multiply_float(_MetallicOcclusionEmissionSmoothness_2A14B742_emission_3, (_OneMinus_5C44DFC9_Out_1.xxx), _Multiply_8B9DEC1C_Out_2);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 _Property_B15756AE_Out_0 = _Layer3EmissionColor;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 _Multiply_FAF8AEF2_Out_2;
                Unity_Multiply_float((_Saturate_1D60C27B_Out_1.xxxx), _Property_B15756AE_Out_0, _Multiply_FAF8AEF2_Out_2);
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 _Add_C7227C15_Out_2;
                Unity_Add_float3(_Multiply_8B9DEC1C_Out_2, (_Multiply_FAF8AEF2_Out_2.xyz), _Add_C7227C15_Out_2);
                #endif
                surface.Albedo = (_Layer_E43A5B0B_Albedo_1.xyz);
                surface.Normal = _NormalBlend_CA918F19_Out_2;
                surface.Emission = _Add_C7227C15_Out_2;
                surface.Metallic = _Layer_E43A5B0B_Metallic_3;
                surface.Smoothness = _Layer_E43A5B0B_Smoothness_2;
                surface.Occlusion = _MetallicOcclusionEmissionSmoothness_2A14B742_occlusion_2;
                surface.Alpha = 1;
                surface.AlphaClipThreshold = 0;
                return surface;
            }

            // --------------------------------------------------
            // Structs and Packing

            // Generated Type: Attributes
            struct Attributes
            {
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 positionOS : POSITION;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 normalOS : NORMAL;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 tangentOS : TANGENT;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 uv0 : TEXCOORD0;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 uv1 : TEXCOORD1;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 uv2 : TEXCOORD2;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 uv3 : TEXCOORD3;
                #endif
                #if UNITY_ANY_INSTANCING_ENABLED
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
                #endif
            };

            // Generated Type: Varyings
            struct Varyings
            {
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 positionCS : SV_POSITION;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 positionWS;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 normalWS;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 tangentWS;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 texCoord0;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 texCoord1;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 texCoord2;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 texCoord3;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 viewDirectionWS;
                #endif
                #if defined(LIGHTMAP_ON)
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float2 lightmapUV;
                #endif
                #endif
                #if !defined(LIGHTMAP_ON)
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float3 sh;
                #endif
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 fogFactorAndVertexLight;
                #endif
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                float4 shadowCoord;
                #endif
                #if UNITY_ANY_INSTANCING_ENABLED
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
                #endif
            };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if defined(LIGHTMAP_ON)
                #endif
                #if !defined(LIGHTMAP_ON)
                #endif
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                float3 interp01 : TEXCOORD1;
                float4 interp02 : TEXCOORD2;
                float4 interp03 : TEXCOORD3;
                float4 interp04 : TEXCOORD4;
                float4 interp05 : TEXCOORD5;
                float4 interp06 : TEXCOORD6;
                float3 interp07 : TEXCOORD7;
                float2 interp08 : TEXCOORD8;
                float3 interp09 : TEXCOORD9;
                float4 interp10 : TEXCOORD10;
                float4 interp11 : TEXCOORD11;
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

            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                output.interp01.xyz = input.normalWS;
                output.interp02.xyzw = input.tangentWS;
                output.interp03.xyzw = input.texCoord0;
                output.interp04.xyzw = input.texCoord1;
                output.interp05.xyzw = input.texCoord2;
                output.interp06.xyzw = input.texCoord3;
                output.interp07.xyz = input.viewDirectionWS;
                #if defined(LIGHTMAP_ON)
                output.interp08.xy = input.lightmapUV;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.interp09.xyz = input.sh;
                #endif
                output.interp10.xyzw = input.fogFactorAndVertexLight;
                output.interp11.xyzw = input.shadowCoord;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                output.normalWS = input.interp01.xyz;
                output.tangentWS = input.interp02.xyzw;
                output.texCoord0 = input.interp03.xyzw;
                output.texCoord1 = input.interp04.xyzw;
                output.texCoord2 = input.interp05.xyzw;
                output.texCoord3 = input.interp06.xyzw;
                output.viewDirectionWS = input.interp07.xyz;
                #if defined(LIGHTMAP_ON)
                output.lightmapUV = input.interp08.xy;
                #endif
                #if !defined(LIGHTMAP_ON)
                output.sh = input.interp09.xyz;
                #endif
                output.fogFactorAndVertexLight = input.interp10.xyzw;
                output.shadowCoord = input.interp11.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            #endif

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
            output.ObjectSpaceNormal = input.normalOS;
            #endif




            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
            output.ObjectSpaceTangent = input.tangentOS;
            #endif








            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
            output.ObjectSpacePosition = input.positionOS;
            #endif












            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
            output.uv1 = input.uv1;
            #endif








                return output;
            }

            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





































            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
            output.uv0 = input.texCoord0;
            #endif

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
            output.uv1 = input.texCoord1;
            #endif

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
            output.uv2 = input.texCoord2;
            #endif

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
            output.uv3 = input.texCoord3;
            #endif



            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif

            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                return output;
            }


            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

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


                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                // Debug
                // <None>

                // --------------------------------------------------
                // Pass

                // Pragmas
                #pragma prefer_hlslcc gles
                #pragma exclude_renderers d3d11_9x
                #pragma target 2.0
                #pragma multi_compile_instancing

                // Keywords
                // PassKeywords: <None>
                #pragma shader_feature_local _ _VERTEXOCCLUSION_ON
                #pragma shader_feature_local _ _EMISSIONMAP_ON
                #pragma shader_feature_local _ _COLORMASK_ON

                #if defined(_VERTEXOCCLUSION_ON) && defined(_EMISSIONMAP_ON) && defined(_COLORMASK_ON)
                    #define KEYWORD_PERMUTATION_0
                #elif defined(_VERTEXOCCLUSION_ON) && defined(_EMISSIONMAP_ON)
                    #define KEYWORD_PERMUTATION_1
                #elif defined(_VERTEXOCCLUSION_ON) && defined(_COLORMASK_ON)
                    #define KEYWORD_PERMUTATION_2
                #elif defined(_VERTEXOCCLUSION_ON)
                    #define KEYWORD_PERMUTATION_3
                #elif defined(_EMISSIONMAP_ON) && defined(_COLORMASK_ON)
                    #define KEYWORD_PERMUTATION_4
                #elif defined(_EMISSIONMAP_ON)
                    #define KEYWORD_PERMUTATION_5
                #elif defined(_COLORMASK_ON)
                    #define KEYWORD_PERMUTATION_6
                #else
                    #define KEYWORD_PERMUTATION_7
                #endif


                // Defines


            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
            #define _NORMALMAP 1
            #endif




                #define _NORMAL_DROPOFF_TS 1
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
            #define ATTRIBUTES_NEED_NORMAL
            #endif

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
            #define ATTRIBUTES_NEED_TANGENT
            #endif


            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
            #define ATTRIBUTES_NEED_TEXCOORD1
            #endif

















                #define FEATURES_GRAPH_VERTEX
                #define SHADERPASS_SHADOWCASTER

                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                // --------------------------------------------------
                // Graph

                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Smoothness;
                float _OcclusionStrength;
                float4 _EmissionColor;
                float4 _Tint0;
                float4 _Tint1;
                float4 _Tint2;
                float _Layer0NormalStrength;
                float2 _Layer0Tiling;
                float _Layer0Smoothness;
                float _Layer0Metallic;
                float _Layer1NormalStrength;
                float2 _Layer1Tiling;
                float _Layer1Smoothness;
                float _Layer1Metallic;
                float _Layer2Height;
                float4 _Layer3EmissionColor;
                float _Bitmask;
                float4 _RevealMask_TexelSize;
                CBUFFER_END
                TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
                TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
                TEXTURE2D(_MetallicGlossMap); SAMPLER(sampler_MetallicGlossMap); float4 _MetallicGlossMap_TexelSize;
                TEXTURE2D(_EmissionMap); SAMPLER(sampler_EmissionMap); float4 _EmissionMap_TexelSize;
                TEXTURE2D(_ColorMask); SAMPLER(sampler_ColorMask); float4 _ColorMask_TexelSize;
                TEXTURE2D(_RevealMask); SAMPLER(sampler_RevealMask);
                TEXTURE2D(_LayerMask); SAMPLER(sampler_LayerMask); float4 _LayerMask_TexelSize;
                TEXTURE2D(_Layer0); SAMPLER(sampler_Layer0); float4 _Layer0_TexelSize;
                TEXTURE2D(_Layer0NormalMap); SAMPLER(sampler_Layer0NormalMap); float4 _Layer0NormalMap_TexelSize;
                TEXTURE2D(_Layer1); SAMPLER(sampler_Layer1); float4 _Layer1_TexelSize;
                TEXTURE2D(_Layer1NormalMap); SAMPLER(sampler_Layer1NormalMap); float4 _Layer1NormalMap_TexelSize;

                // Graph Functions

                void BitwiseAND_float(float a, float b, out float Out)
                {
                    Out = ((int)a & (int)b);
                }

                struct Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363
                {
                };

                void SG_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363(float Vector1_E5A799E5, float Vector1_F006FE71, Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363 IN, out float OutVector1_1)
                {
                    float _Property_39DA3013_Out_0 = Vector1_E5A799E5;
                    float _Property_2853174C_Out_0 = Vector1_F006FE71;
                    float _CustomFunction_2F22FDC6_Out_2;
                    BitwiseAND_float(_Property_39DA3013_Out_0, _Property_2853174C_Out_0, _CustomFunction_2F22FDC6_Out_2);
                    OutVector1_1 = _CustomFunction_2F22FDC6_Out_2;
                }

                void Unity_Comparison_Equal_float(float A, float B, out float Out)
                {
                    Out = A == B ? 1 : 0;
                }

                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }

                void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
                {
                    Out = Predicate ? True : False;
                }

                struct Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05
                {
                };

                void SG_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05(float3 Vector3_725ECFC1, float Vector1_A7EE8494, float Vector1_9BC3020B, Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05 IN, out float3 OutVector4_1)
                {
                    float _Property_E196FA79_Out_0 = Vector1_A7EE8494;
                    float _Property_FFF31F1C_Out_0 = Vector1_9BC3020B;
                    Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363 _bitwiseAND_FC16028;
                    float _bitwiseAND_FC16028_OutVector1_1;
                    SG_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363(_Property_E196FA79_Out_0, _Property_FFF31F1C_Out_0, _bitwiseAND_FC16028, _bitwiseAND_FC16028_OutVector1_1);
                    float _Comparison_84D89BD3_Out_2;
                    Unity_Comparison_Equal_float(_bitwiseAND_FC16028_OutVector1_1, 0, _Comparison_84D89BD3_Out_2);
                    float3 _Property_A3E178F8_Out_0 = Vector3_725ECFC1;
                    float _Divide_7A3B32D4_Out_2;
                    Unity_Divide_float(0, 0, _Divide_7A3B32D4_Out_2);
                    float3 _Branch_602423C2_Out_3;
                    Unity_Branch_float3(_Comparison_84D89BD3_Out_2, _Property_A3E178F8_Out_0, (_Divide_7A3B32D4_Out_2.xxx), _Branch_602423C2_Out_3);
                    OutVector4_1 = _Branch_602423C2_Out_3;
                }

                // Graph Vertex
                struct VertexDescriptionInputs
                {
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    float3 ObjectSpaceNormal;
                    #endif
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    float3 ObjectSpaceTangent;
                    #endif
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    float3 ObjectSpacePosition;
                    #endif
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                    float4 uv1;
                    #endif
                };

                struct VertexDescription
                {
                    float3 VertexPosition;
                    float3 VertexNormal;
                    float3 VertexTangent;
                };

                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                    float _Property_64870DC_Out_0 = _Bitmask;
                    #endif
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                    float4 _UV_592331DC_Out_0 = IN.uv1;
                    #endif
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                    Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05 _NaNVertexDivideByZero_13598E37;
                    float3 _NaNVertexDivideByZero_13598E37_OutVector4_1;
                    SG_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05(IN.ObjectSpacePosition, _Property_64870DC_Out_0, (_UV_592331DC_Out_0).x, _NaNVertexDivideByZero_13598E37, _NaNVertexDivideByZero_13598E37_OutVector4_1);
                    #endif
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    #if defined(_VERTEXOCCLUSION_ON)
                    float3 _VertexOcclusion_EB96B2B1_Out_0 = _NaNVertexDivideByZero_13598E37_OutVector4_1;
                    #else
                    float3 _VertexOcclusion_EB96B2B1_Out_0 = IN.ObjectSpacePosition;
                    #endif
                    #endif
                    description.VertexPosition = _VertexOcclusion_EB96B2B1_Out_0;
                    description.VertexNormal = IN.ObjectSpaceNormal;
                    description.VertexTangent = IN.ObjectSpaceTangent;
                    return description;
                }

                // Graph Pixel
                struct SurfaceDescriptionInputs
                {
                };

                struct SurfaceDescription
                {
                    float Alpha;
                    float AlphaClipThreshold;
                };

                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    surface.Alpha = 1;
                    surface.AlphaClipThreshold = 0;
                    return surface;
                }

                // --------------------------------------------------
                // Structs and Packing

                // Generated Type: Attributes
                struct Attributes
                {
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    float3 positionOS : POSITION;
                    #endif
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    float3 normalOS : NORMAL;
                    #endif
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    float4 tangentOS : TANGENT;
                    #endif
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                    float4 uv1 : TEXCOORD1;
                    #endif
                    #if UNITY_ANY_INSTANCING_ENABLED
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                    #endif
                };

                // Generated Type: Varyings
                struct Varyings
                {
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    float4 positionCS : SV_POSITION;
                    #endif
                    #if UNITY_ANY_INSTANCING_ENABLED
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                    #endif
                };

                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                // Generated Type: PackedVaryings
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
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

                // Packed Type: Varyings
                PackedVaryings PackVaryings(Varyings input)
                {
                    PackedVaryings output = (PackedVaryings)0;
                    output.positionCS = input.positionCS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // Unpacked Type: Varyings
                Varyings UnpackVaryings(PackedVaryings input)
                {
                    Varyings output = (Varyings)0;
                    output.positionCS = input.positionCS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                #endif

                // --------------------------------------------------
                // Build Graph Inputs

                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);

                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                output.ObjectSpaceNormal = input.normalOS;
                #endif




                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                output.ObjectSpaceTangent = input.tangentOS;
                #endif








                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                output.ObjectSpacePosition = input.positionOS;
                #endif












                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                output.uv1 = input.uv1;
                #endif








                    return output;
                }

                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);











































                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif

                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
                }


                // --------------------------------------------------
                // Main

                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

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


                    HLSLPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag

                    // Debug
                    // <None>

                    // --------------------------------------------------
                    // Pass

                    // Pragmas
                    #pragma prefer_hlslcc gles
                    #pragma exclude_renderers d3d11_9x
                    #pragma target 2.0
                    #pragma multi_compile_instancing

                    // Keywords
                    // PassKeywords: <None>
                    #pragma shader_feature_local _ _VERTEXOCCLUSION_ON
                    #pragma shader_feature_local _ _EMISSIONMAP_ON
                    #pragma shader_feature_local _ _COLORMASK_ON

                    #if defined(_VERTEXOCCLUSION_ON) && defined(_EMISSIONMAP_ON) && defined(_COLORMASK_ON)
                        #define KEYWORD_PERMUTATION_0
                    #elif defined(_VERTEXOCCLUSION_ON) && defined(_EMISSIONMAP_ON)
                        #define KEYWORD_PERMUTATION_1
                    #elif defined(_VERTEXOCCLUSION_ON) && defined(_COLORMASK_ON)
                        #define KEYWORD_PERMUTATION_2
                    #elif defined(_VERTEXOCCLUSION_ON)
                        #define KEYWORD_PERMUTATION_3
                    #elif defined(_EMISSIONMAP_ON) && defined(_COLORMASK_ON)
                        #define KEYWORD_PERMUTATION_4
                    #elif defined(_EMISSIONMAP_ON)
                        #define KEYWORD_PERMUTATION_5
                    #elif defined(_COLORMASK_ON)
                        #define KEYWORD_PERMUTATION_6
                    #else
                        #define KEYWORD_PERMUTATION_7
                    #endif


                    // Defines


                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #define _NORMALMAP 1
                #endif




                    #define _NORMAL_DROPOFF_TS 1
                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #define ATTRIBUTES_NEED_NORMAL
                #endif

                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                #define ATTRIBUTES_NEED_TANGENT
                #endif


                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                #define ATTRIBUTES_NEED_TEXCOORD1
                #endif

















                    #define FEATURES_GRAPH_VERTEX
                    #define SHADERPASS_DEPTHONLY

                    // Includes
                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                    #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                    // --------------------------------------------------
                    // Graph

                    // Graph Properties
                    CBUFFER_START(UnityPerMaterial)
                    float4 _BaseColor;
                    float _Smoothness;
                    float _OcclusionStrength;
                    float4 _EmissionColor;
                    float4 _Tint0;
                    float4 _Tint1;
                    float4 _Tint2;
                    float _Layer0NormalStrength;
                    float2 _Layer0Tiling;
                    float _Layer0Smoothness;
                    float _Layer0Metallic;
                    float _Layer1NormalStrength;
                    float2 _Layer1Tiling;
                    float _Layer1Smoothness;
                    float _Layer1Metallic;
                    float _Layer2Height;
                    float4 _Layer3EmissionColor;
                    float _Bitmask;
                    float4 _RevealMask_TexelSize;
                    CBUFFER_END
                    TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
                    TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
                    TEXTURE2D(_MetallicGlossMap); SAMPLER(sampler_MetallicGlossMap); float4 _MetallicGlossMap_TexelSize;
                    TEXTURE2D(_EmissionMap); SAMPLER(sampler_EmissionMap); float4 _EmissionMap_TexelSize;
                    TEXTURE2D(_ColorMask); SAMPLER(sampler_ColorMask); float4 _ColorMask_TexelSize;
                    TEXTURE2D(_RevealMask); SAMPLER(sampler_RevealMask);
                    TEXTURE2D(_LayerMask); SAMPLER(sampler_LayerMask); float4 _LayerMask_TexelSize;
                    TEXTURE2D(_Layer0); SAMPLER(sampler_Layer0); float4 _Layer0_TexelSize;
                    TEXTURE2D(_Layer0NormalMap); SAMPLER(sampler_Layer0NormalMap); float4 _Layer0NormalMap_TexelSize;
                    TEXTURE2D(_Layer1); SAMPLER(sampler_Layer1); float4 _Layer1_TexelSize;
                    TEXTURE2D(_Layer1NormalMap); SAMPLER(sampler_Layer1NormalMap); float4 _Layer1NormalMap_TexelSize;

                    // Graph Functions

                    void BitwiseAND_float(float a, float b, out float Out)
                    {
                        Out = ((int)a & (int)b);
                    }

                    struct Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363
                    {
                    };

                    void SG_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363(float Vector1_E5A799E5, float Vector1_F006FE71, Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363 IN, out float OutVector1_1)
                    {
                        float _Property_39DA3013_Out_0 = Vector1_E5A799E5;
                        float _Property_2853174C_Out_0 = Vector1_F006FE71;
                        float _CustomFunction_2F22FDC6_Out_2;
                        BitwiseAND_float(_Property_39DA3013_Out_0, _Property_2853174C_Out_0, _CustomFunction_2F22FDC6_Out_2);
                        OutVector1_1 = _CustomFunction_2F22FDC6_Out_2;
                    }

                    void Unity_Comparison_Equal_float(float A, float B, out float Out)
                    {
                        Out = A == B ? 1 : 0;
                    }

                    void Unity_Divide_float(float A, float B, out float Out)
                    {
                        Out = A / B;
                    }

                    void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
                    {
                        Out = Predicate ? True : False;
                    }

                    struct Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05
                    {
                    };

                    void SG_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05(float3 Vector3_725ECFC1, float Vector1_A7EE8494, float Vector1_9BC3020B, Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05 IN, out float3 OutVector4_1)
                    {
                        float _Property_E196FA79_Out_0 = Vector1_A7EE8494;
                        float _Property_FFF31F1C_Out_0 = Vector1_9BC3020B;
                        Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363 _bitwiseAND_FC16028;
                        float _bitwiseAND_FC16028_OutVector1_1;
                        SG_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363(_Property_E196FA79_Out_0, _Property_FFF31F1C_Out_0, _bitwiseAND_FC16028, _bitwiseAND_FC16028_OutVector1_1);
                        float _Comparison_84D89BD3_Out_2;
                        Unity_Comparison_Equal_float(_bitwiseAND_FC16028_OutVector1_1, 0, _Comparison_84D89BD3_Out_2);
                        float3 _Property_A3E178F8_Out_0 = Vector3_725ECFC1;
                        float _Divide_7A3B32D4_Out_2;
                        Unity_Divide_float(0, 0, _Divide_7A3B32D4_Out_2);
                        float3 _Branch_602423C2_Out_3;
                        Unity_Branch_float3(_Comparison_84D89BD3_Out_2, _Property_A3E178F8_Out_0, (_Divide_7A3B32D4_Out_2.xxx), _Branch_602423C2_Out_3);
                        OutVector4_1 = _Branch_602423C2_Out_3;
                    }

                    // Graph Vertex
                    struct VertexDescriptionInputs
                    {
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        float3 ObjectSpaceNormal;
                        #endif
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        float3 ObjectSpaceTangent;
                        #endif
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        float3 ObjectSpacePosition;
                        #endif
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                        float4 uv1;
                        #endif
                    };

                    struct VertexDescription
                    {
                        float3 VertexPosition;
                        float3 VertexNormal;
                        float3 VertexTangent;
                    };

                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                        float _Property_64870DC_Out_0 = _Bitmask;
                        #endif
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                        float4 _UV_592331DC_Out_0 = IN.uv1;
                        #endif
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                        Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05 _NaNVertexDivideByZero_13598E37;
                        float3 _NaNVertexDivideByZero_13598E37_OutVector4_1;
                        SG_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05(IN.ObjectSpacePosition, _Property_64870DC_Out_0, (_UV_592331DC_Out_0).x, _NaNVertexDivideByZero_13598E37, _NaNVertexDivideByZero_13598E37_OutVector4_1);
                        #endif
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        #if defined(_VERTEXOCCLUSION_ON)
                        float3 _VertexOcclusion_EB96B2B1_Out_0 = _NaNVertexDivideByZero_13598E37_OutVector4_1;
                        #else
                        float3 _VertexOcclusion_EB96B2B1_Out_0 = IN.ObjectSpacePosition;
                        #endif
                        #endif
                        description.VertexPosition = _VertexOcclusion_EB96B2B1_Out_0;
                        description.VertexNormal = IN.ObjectSpaceNormal;
                        description.VertexTangent = IN.ObjectSpaceTangent;
                        return description;
                    }

                    // Graph Pixel
                    struct SurfaceDescriptionInputs
                    {
                    };

                    struct SurfaceDescription
                    {
                        float Alpha;
                        float AlphaClipThreshold;
                    };

                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        surface.Alpha = 1;
                        surface.AlphaClipThreshold = 0;
                        return surface;
                    }

                    // --------------------------------------------------
                    // Structs and Packing

                    // Generated Type: Attributes
                    struct Attributes
                    {
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        float3 positionOS : POSITION;
                        #endif
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        float3 normalOS : NORMAL;
                        #endif
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        float4 tangentOS : TANGENT;
                        #endif
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                        float4 uv1 : TEXCOORD1;
                        #endif
                        #if UNITY_ANY_INSTANCING_ENABLED
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        uint instanceID : INSTANCEID_SEMANTIC;
                        #endif
                        #endif
                    };

                    // Generated Type: Varyings
                    struct Varyings
                    {
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        float4 positionCS : SV_POSITION;
                        #endif
                        #if UNITY_ANY_INSTANCING_ENABLED
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                        #endif
                    };

                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    // Generated Type: PackedVaryings
                    struct PackedVaryings
                    {
                        float4 positionCS : SV_POSITION;
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

                    // Packed Type: Varyings
                    PackedVaryings PackVaryings(Varyings input)
                    {
                        PackedVaryings output = (PackedVaryings)0;
                        output.positionCS = input.positionCS;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // Unpacked Type: Varyings
                    Varyings UnpackVaryings(PackedVaryings input)
                    {
                        Varyings output = (Varyings)0;
                        output.positionCS = input.positionCS;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }
                    #endif

                    // --------------------------------------------------
                    // Build Graph Inputs

                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                    {
                        VertexDescriptionInputs output;
                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    output.ObjectSpaceNormal = input.normalOS;
                    #endif




                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    output.ObjectSpaceTangent = input.tangentOS;
                    #endif








                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    output.ObjectSpacePosition = input.positionOS;
                    #endif












                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                    output.uv1 = input.uv1;
                    #endif








                        return output;
                    }

                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                    {
                        SurfaceDescriptionInputs output;
                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);











































                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                    #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                    #endif

                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                        return output;
                    }


                    // --------------------------------------------------
                    // Main

                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

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


                        HLSLPROGRAM
                        #pragma vertex vert
                        #pragma fragment frag

                        // Debug
                        // <None>

                        // --------------------------------------------------
                        // Pass

                        // Pragmas
                        #pragma prefer_hlslcc gles
                        #pragma exclude_renderers d3d11_9x
                        #pragma target 2.0

                        // Keywords
                        #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                        #pragma shader_feature_local _ _VERTEXOCCLUSION_ON
                        #pragma shader_feature_local _ _EMISSIONMAP_ON
                        #pragma shader_feature_local _ _COLORMASK_ON

                        #if defined(_VERTEXOCCLUSION_ON) && defined(_EMISSIONMAP_ON) && defined(_COLORMASK_ON)
                            #define KEYWORD_PERMUTATION_0
                        #elif defined(_VERTEXOCCLUSION_ON) && defined(_EMISSIONMAP_ON)
                            #define KEYWORD_PERMUTATION_1
                        #elif defined(_VERTEXOCCLUSION_ON) && defined(_COLORMASK_ON)
                            #define KEYWORD_PERMUTATION_2
                        #elif defined(_VERTEXOCCLUSION_ON)
                            #define KEYWORD_PERMUTATION_3
                        #elif defined(_EMISSIONMAP_ON) && defined(_COLORMASK_ON)
                            #define KEYWORD_PERMUTATION_4
                        #elif defined(_EMISSIONMAP_ON)
                            #define KEYWORD_PERMUTATION_5
                        #elif defined(_COLORMASK_ON)
                            #define KEYWORD_PERMUTATION_6
                        #else
                            #define KEYWORD_PERMUTATION_7
                        #endif


                        // Defines


                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    #define _NORMALMAP 1
                    #endif




                        #define _NORMAL_DROPOFF_TS 1
                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    #define ATTRIBUTES_NEED_NORMAL
                    #endif

                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    #define ATTRIBUTES_NEED_TANGENT
                    #endif

                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    #define ATTRIBUTES_NEED_TEXCOORD0
                    #endif

                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    #define ATTRIBUTES_NEED_TEXCOORD1
                    #endif

                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    #define ATTRIBUTES_NEED_TEXCOORD2
                    #endif






                    #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                    #define VARYINGS_NEED_TEXCOORD0
                    #endif










                        #define FEATURES_GRAPH_VERTEX
                        #define SHADERPASS_META

                        // Includes
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
                        #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                        // --------------------------------------------------
                        // Graph

                        // Graph Properties
                        CBUFFER_START(UnityPerMaterial)
                        float4 _BaseColor;
                        float _Smoothness;
                        float _OcclusionStrength;
                        float4 _EmissionColor;
                        float4 _Tint0;
                        float4 _Tint1;
                        float4 _Tint2;
                        float _Layer0NormalStrength;
                        float2 _Layer0Tiling;
                        float _Layer0Smoothness;
                        float _Layer0Metallic;
                        float _Layer1NormalStrength;
                        float2 _Layer1Tiling;
                        float _Layer1Smoothness;
                        float _Layer1Metallic;
                        float _Layer2Height;
                        float4 _Layer3EmissionColor;
                        float _Bitmask;
                        float4 _RevealMask_TexelSize;
                        CBUFFER_END
                        TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
                        TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
                        TEXTURE2D(_MetallicGlossMap); SAMPLER(sampler_MetallicGlossMap); float4 _MetallicGlossMap_TexelSize;
                        TEXTURE2D(_EmissionMap); SAMPLER(sampler_EmissionMap); float4 _EmissionMap_TexelSize;
                        TEXTURE2D(_ColorMask); SAMPLER(sampler_ColorMask); float4 _ColorMask_TexelSize;
                        TEXTURE2D(_RevealMask); SAMPLER(sampler_RevealMask);
                        TEXTURE2D(_LayerMask); SAMPLER(sampler_LayerMask); float4 _LayerMask_TexelSize;
                        TEXTURE2D(_Layer0); SAMPLER(sampler_Layer0); float4 _Layer0_TexelSize;
                        TEXTURE2D(_Layer0NormalMap); SAMPLER(sampler_Layer0NormalMap); float4 _Layer0NormalMap_TexelSize;
                        TEXTURE2D(_Layer1); SAMPLER(sampler_Layer1); float4 _Layer1_TexelSize;
                        TEXTURE2D(_Layer1NormalMap); SAMPLER(sampler_Layer1NormalMap); float4 _Layer1NormalMap_TexelSize;
                        SAMPLER(_SampleTexture2D_8605BC1D_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_7B9CCAC4_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_71F12517_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_AC574377_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_6F08E612_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_A2DE4CE7_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_DF0E076D_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_51B6E38C_Sampler_3_Linear_Repeat);
                        SAMPLER(_SampleTexture2D_D0D2AB3D_Sampler_3_Linear_Repeat);

                        // Graph Functions

                        void BitwiseAND_float(float a, float b, out float Out)
                        {
                            Out = ((int)a & (int)b);
                        }

                        struct Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363
                        {
                        };

                        void SG_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363(float Vector1_E5A799E5, float Vector1_F006FE71, Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363 IN, out float OutVector1_1)
                        {
                            float _Property_39DA3013_Out_0 = Vector1_E5A799E5;
                            float _Property_2853174C_Out_0 = Vector1_F006FE71;
                            float _CustomFunction_2F22FDC6_Out_2;
                            BitwiseAND_float(_Property_39DA3013_Out_0, _Property_2853174C_Out_0, _CustomFunction_2F22FDC6_Out_2);
                            OutVector1_1 = _CustomFunction_2F22FDC6_Out_2;
                        }

                        void Unity_Comparison_Equal_float(float A, float B, out float Out)
                        {
                            Out = A == B ? 1 : 0;
                        }

                        void Unity_Divide_float(float A, float B, out float Out)
                        {
                            Out = A / B;
                        }

                        void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
                        {
                            Out = Predicate ? True : False;
                        }

                        struct Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05
                        {
                        };

                        void SG_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05(float3 Vector3_725ECFC1, float Vector1_A7EE8494, float Vector1_9BC3020B, Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05 IN, out float3 OutVector4_1)
                        {
                            float _Property_E196FA79_Out_0 = Vector1_A7EE8494;
                            float _Property_FFF31F1C_Out_0 = Vector1_9BC3020B;
                            Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363 _bitwiseAND_FC16028;
                            float _bitwiseAND_FC16028_OutVector1_1;
                            SG_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363(_Property_E196FA79_Out_0, _Property_FFF31F1C_Out_0, _bitwiseAND_FC16028, _bitwiseAND_FC16028_OutVector1_1);
                            float _Comparison_84D89BD3_Out_2;
                            Unity_Comparison_Equal_float(_bitwiseAND_FC16028_OutVector1_1, 0, _Comparison_84D89BD3_Out_2);
                            float3 _Property_A3E178F8_Out_0 = Vector3_725ECFC1;
                            float _Divide_7A3B32D4_Out_2;
                            Unity_Divide_float(0, 0, _Divide_7A3B32D4_Out_2);
                            float3 _Branch_602423C2_Out_3;
                            Unity_Branch_float3(_Comparison_84D89BD3_Out_2, _Property_A3E178F8_Out_0, (_Divide_7A3B32D4_Out_2.xxx), _Branch_602423C2_Out_3);
                            OutVector4_1 = _Branch_602423C2_Out_3;
                        }

                        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                        {
                            Out = A * B;
                        }

                        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
                        {
                            Out = A + B;
                        }

                        struct Bindings_ColorTintMask_6636e22a321cd404c89884028dd4f823
                        {
                            half4 uv0;
                        };

                        void SG_ColorTintMask_6636e22a321cd404c89884028dd4f823(TEXTURE2D_PARAM(Texture2D_917AA17E, samplerTexture2D_917AA17E), float4 Texture2D_917AA17E_TexelSize, float4 Vector4_553970CD, float4 Vector4_C861B211, float4 Vector4_ED9FE209, Bindings_ColorTintMask_6636e22a321cd404c89884028dd4f823 IN, out float4 OutVector4_1)
                        {
                            float4 _Property_526A61D_Out_0 = Vector4_553970CD;
                            float4 _SampleTexture2D_8605BC1D_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_917AA17E, samplerTexture2D_917AA17E, IN.uv0.xy);
                            float _SampleTexture2D_8605BC1D_R_4 = _SampleTexture2D_8605BC1D_RGBA_0.r;
                            float _SampleTexture2D_8605BC1D_G_5 = _SampleTexture2D_8605BC1D_RGBA_0.g;
                            float _SampleTexture2D_8605BC1D_B_6 = _SampleTexture2D_8605BC1D_RGBA_0.b;
                            float _SampleTexture2D_8605BC1D_A_7 = _SampleTexture2D_8605BC1D_RGBA_0.a;
                            float4 _Multiply_6497A688_Out_2;
                            Unity_Multiply_float(_Property_526A61D_Out_0, (_SampleTexture2D_8605BC1D_R_4.xxxx), _Multiply_6497A688_Out_2);
                            float4 _Property_3A437BCB_Out_0 = Vector4_C861B211;
                            float4 _Multiply_385888A6_Out_2;
                            Unity_Multiply_float(_Property_3A437BCB_Out_0, (_SampleTexture2D_8605BC1D_G_5.xxxx), _Multiply_385888A6_Out_2);
                            float4 _Add_A3D53A8A_Out_2;
                            Unity_Add_float4(_Multiply_6497A688_Out_2, _Multiply_385888A6_Out_2, _Add_A3D53A8A_Out_2);
                            float4 _Property_DA82EA32_Out_0 = Vector4_ED9FE209;
                            float4 _Multiply_BB15BEFC_Out_2;
                            Unity_Multiply_float(_Property_DA82EA32_Out_0, (_SampleTexture2D_8605BC1D_B_6.xxxx), _Multiply_BB15BEFC_Out_2);
                            float4 _Add_E0242413_Out_2;
                            Unity_Add_float4(_Add_A3D53A8A_Out_2, _Multiply_BB15BEFC_Out_2, _Add_E0242413_Out_2);
                            OutVector4_1 = _Add_E0242413_Out_2;
                        }

                        void Unity_NormalUnpack_float(float4 In, out float3 Out)
                        {
                                        Out = UnpackNormal(In);
                                    }

                        void Unity_Multiply_float(float A, float B, out float Out)
                        {
                            Out = A * B;
                        }

                        struct Bindings_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0
                        {
                            half4 uv0;
                        };

                        void SG_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0(float Vector1_74BD58D7, float Vector1_7BD83B87, TEXTURE2D_PARAM(Texture2D_1264D9E6, samplerTexture2D_1264D9E6), float4 Texture2D_1264D9E6_TexelSize, TEXTURE2D_PARAM(Texture2D_677C4338, samplerTexture2D_677C4338), float4 Texture2D_677C4338_TexelSize, float4 Vector4_6479F3E8, Bindings_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0 IN, out float metallic_1, out float occlusion_2, out float3 emission_3, out float smoothness_4)
                        {
                            float4 _SampleTexture2D_AC574377_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_677C4338, samplerTexture2D_677C4338, IN.uv0.xy);
                            float _SampleTexture2D_AC574377_R_4 = _SampleTexture2D_AC574377_RGBA_0.r;
                            float _SampleTexture2D_AC574377_G_5 = _SampleTexture2D_AC574377_RGBA_0.g;
                            float _SampleTexture2D_AC574377_B_6 = _SampleTexture2D_AC574377_RGBA_0.b;
                            float _SampleTexture2D_AC574377_A_7 = _SampleTexture2D_AC574377_RGBA_0.a;
                            float _Property_8B465651_Out_0 = Vector1_74BD58D7;
                            float _Multiply_DE2808F8_Out_2;
                            Unity_Multiply_float(_SampleTexture2D_AC574377_G_5, _Property_8B465651_Out_0, _Multiply_DE2808F8_Out_2);
                            float4 _SampleTexture2D_6F08E612_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_1264D9E6, samplerTexture2D_1264D9E6, IN.uv0.xy);
                            float _SampleTexture2D_6F08E612_R_4 = _SampleTexture2D_6F08E612_RGBA_0.r;
                            float _SampleTexture2D_6F08E612_G_5 = _SampleTexture2D_6F08E612_RGBA_0.g;
                            float _SampleTexture2D_6F08E612_B_6 = _SampleTexture2D_6F08E612_RGBA_0.b;
                            float _SampleTexture2D_6F08E612_A_7 = _SampleTexture2D_6F08E612_RGBA_0.a;
                            float4 _Property_892842AC_Out_0 = Vector4_6479F3E8;
                            float4 _Multiply_F33D45B4_Out_2;
                            Unity_Multiply_float(_SampleTexture2D_6F08E612_RGBA_0, _Property_892842AC_Out_0, _Multiply_F33D45B4_Out_2);
                            float4 _Multiply_3D94F4FB_Out_2;
                            Unity_Multiply_float((_SampleTexture2D_AC574377_B_6.xxxx), _Property_892842AC_Out_0, _Multiply_3D94F4FB_Out_2);
                            #if defined(_EMISSIONMAP_ON)
                            float4 _UseEmissionMap_160345FE_Out_0 = _Multiply_F33D45B4_Out_2;
                            #else
                            float4 _UseEmissionMap_160345FE_Out_0 = _Multiply_3D94F4FB_Out_2;
                            #endif
                            float _Property_9A418755_Out_0 = Vector1_7BD83B87;
                            float _Multiply_7FDD2068_Out_2;
                            Unity_Multiply_float(_SampleTexture2D_AC574377_A_7, _Property_9A418755_Out_0, _Multiply_7FDD2068_Out_2);
                            metallic_1 = _SampleTexture2D_AC574377_R_4;
                            occlusion_2 = _Multiply_DE2808F8_Out_2;
                            emission_3 = (_UseEmissionMap_160345FE_Out_0.xyz);
                            smoothness_4 = _Multiply_7FDD2068_Out_2;
                        }

                        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                        {
                            Out = UV * Tiling + Offset;
                        }

                        void Unity_Saturate_float(float In, out float Out)
                        {
                            Out = saturate(In);
                        }

                        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
                        {
                            Out = lerp(A, B, T);
                        }

                        void Unity_Lerp_float(float A, float B, float T, out float Out)
                        {
                            Out = lerp(A, B, T);
                        }

                        void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
                        {
                            Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
                        }

                        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
                        {
                            Out = lerp(A, B, T);
                        }

                        void Unity_Normalize_float3(float3 In, out float3 Out)
                        {
                            Out = normalize(In);
                        }

                        struct Bindings_Layer_add1b3c672351c24c8c4119908f01e02
                        {
                            half4 uv0;
                        };

                        void SG_Layer_add1b3c672351c24c8c4119908f01e02(float4 Vector4_1ED74482, float3 Vector3_1358BB6D, float Vector1_89498AA5, float Vector1_E4A5BF8E, TEXTURE2D_PARAM(Texture2D_CB4BECC7, samplerTexture2D_CB4BECC7), float4 Texture2D_CB4BECC7_TexelSize, float2 Vector2_CF2204C5, TEXTURE2D_PARAM(Texture2D_70A11B0B, samplerTexture2D_70A11B0B), float4 Texture2D_70A11B0B_TexelSize, float Vector1_71A3EDBE, float Vector1_A7A3CDEE, float Vector1_A08FC73, float Vector1_F408666D, Bindings_Layer_add1b3c672351c24c8c4119908f01e02 IN, out float4 Albedo_1, out float Smoothness_2, out float Metallic_3, out float3 Normal_4)
                        {
                            float4 _Property_E0A8A88E_Out_0 = Vector4_1ED74482;
                            float2 _Property_F2EA6A5A_Out_0 = Vector2_CF2204C5;
                            float2 _TilingAndOffset_FDF9B562_Out_3;
                            Unity_TilingAndOffset_float(IN.uv0.xy, _Property_F2EA6A5A_Out_0, float2 (0, 0), _TilingAndOffset_FDF9B562_Out_3);
                            float4 _SampleTexture2D_51B6E38C_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_CB4BECC7, samplerTexture2D_CB4BECC7, _TilingAndOffset_FDF9B562_Out_3);
                            float _SampleTexture2D_51B6E38C_R_4 = _SampleTexture2D_51B6E38C_RGBA_0.r;
                            float _SampleTexture2D_51B6E38C_G_5 = _SampleTexture2D_51B6E38C_RGBA_0.g;
                            float _SampleTexture2D_51B6E38C_B_6 = _SampleTexture2D_51B6E38C_RGBA_0.b;
                            float _SampleTexture2D_51B6E38C_A_7 = _SampleTexture2D_51B6E38C_RGBA_0.a;
                            float _Property_EFE4636B_Out_0 = Vector1_F408666D;
                            float _Saturate_783545D1_Out_1;
                            Unity_Saturate_float(_Property_EFE4636B_Out_0, _Saturate_783545D1_Out_1);
                            float4 _Lerp_B0BF9BA8_Out_3;
                            Unity_Lerp_float4(_Property_E0A8A88E_Out_0, _SampleTexture2D_51B6E38C_RGBA_0, (_Saturate_783545D1_Out_1.xxxx), _Lerp_B0BF9BA8_Out_3);
                            float _Property_A8236A77_Out_0 = Vector1_89498AA5;
                            float _Property_9DEC1A03_Out_0 = Vector1_A08FC73;
                            float _Lerp_CDE58F3A_Out_3;
                            Unity_Lerp_float(_Property_A8236A77_Out_0, _Property_9DEC1A03_Out_0, _Saturate_783545D1_Out_1, _Lerp_CDE58F3A_Out_3);
                            float _Property_B5095A31_Out_0 = Vector1_E4A5BF8E;
                            float _Property_5FFE3307_Out_0 = Vector1_A7A3CDEE;
                            float _Lerp_3EF7B8D1_Out_3;
                            Unity_Lerp_float(_Property_B5095A31_Out_0, _Property_5FFE3307_Out_0, _Saturate_783545D1_Out_1, _Lerp_3EF7B8D1_Out_3);
                            float3 _Property_2D135E56_Out_0 = Vector3_1358BB6D;
                            float4 _SampleTexture2D_D0D2AB3D_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_70A11B0B, samplerTexture2D_70A11B0B, _TilingAndOffset_FDF9B562_Out_3);
                            float _SampleTexture2D_D0D2AB3D_R_4 = _SampleTexture2D_D0D2AB3D_RGBA_0.r;
                            float _SampleTexture2D_D0D2AB3D_G_5 = _SampleTexture2D_D0D2AB3D_RGBA_0.g;
                            float _SampleTexture2D_D0D2AB3D_B_6 = _SampleTexture2D_D0D2AB3D_RGBA_0.b;
                            float _SampleTexture2D_D0D2AB3D_A_7 = _SampleTexture2D_D0D2AB3D_RGBA_0.a;
                            float3 _NormalUnpack_3906D491_Out_1;
                            Unity_NormalUnpack_float(_SampleTexture2D_D0D2AB3D_RGBA_0, _NormalUnpack_3906D491_Out_1);
                            float _Property_E1B61021_Out_0 = Vector1_71A3EDBE;
                            float3 _NormalStrength_3F987ED_Out_2;
                            Unity_NormalStrength_float(_NormalUnpack_3906D491_Out_1, _Property_E1B61021_Out_0, _NormalStrength_3F987ED_Out_2);
                            float3 _Lerp_C7A67762_Out_3;
                            Unity_Lerp_float3(_Property_2D135E56_Out_0, _NormalStrength_3F987ED_Out_2, (_Saturate_783545D1_Out_1.xxx), _Lerp_C7A67762_Out_3);
                            float3 _Normalize_FE9E0F4E_Out_1;
                            Unity_Normalize_float3(_Lerp_C7A67762_Out_3, _Normalize_FE9E0F4E_Out_1);
                            Albedo_1 = _Lerp_B0BF9BA8_Out_3;
                            Smoothness_2 = _Lerp_CDE58F3A_Out_3;
                            Metallic_3 = _Lerp_3EF7B8D1_Out_3;
                            Normal_4 = _Normalize_FE9E0F4E_Out_1;
                        }

                        void Unity_OneMinus_float(float In, out float Out)
                        {
                            Out = 1 - In;
                        }

                        void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
                        {
                            Out = A * B;
                        }

                        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
                        {
                            Out = A + B;
                        }

                        // Graph Vertex
                        struct VertexDescriptionInputs
                        {
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float3 ObjectSpaceNormal;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float3 ObjectSpaceTangent;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float3 ObjectSpacePosition;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                            float4 uv1;
                            #endif
                        };

                        struct VertexDescription
                        {
                            float3 VertexPosition;
                            float3 VertexNormal;
                            float3 VertexTangent;
                        };

                        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                        {
                            VertexDescription description = (VertexDescription)0;
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                            float _Property_64870DC_Out_0 = _Bitmask;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                            float4 _UV_592331DC_Out_0 = IN.uv1;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                            Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05 _NaNVertexDivideByZero_13598E37;
                            float3 _NaNVertexDivideByZero_13598E37_OutVector4_1;
                            SG_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05(IN.ObjectSpacePosition, _Property_64870DC_Out_0, (_UV_592331DC_Out_0).x, _NaNVertexDivideByZero_13598E37, _NaNVertexDivideByZero_13598E37_OutVector4_1);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #if defined(_VERTEXOCCLUSION_ON)
                            float3 _VertexOcclusion_EB96B2B1_Out_0 = _NaNVertexDivideByZero_13598E37_OutVector4_1;
                            #else
                            float3 _VertexOcclusion_EB96B2B1_Out_0 = IN.ObjectSpacePosition;
                            #endif
                            #endif
                            description.VertexPosition = _VertexOcclusion_EB96B2B1_Out_0;
                            description.VertexNormal = IN.ObjectSpaceNormal;
                            description.VertexTangent = IN.ObjectSpaceTangent;
                            return description;
                        }

                        // Graph Pixel
                        struct SurfaceDescriptionInputs
                        {
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 uv0;
                            #endif
                        };

                        struct SurfaceDescription
                        {
                            float3 Albedo;
                            float3 Emission;
                            float Alpha;
                            float AlphaClipThreshold;
                        };

                        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                        {
                            SurfaceDescription surface = (SurfaceDescription)0;
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                            float4 _Property_AD19C48F_Out_0 = _Tint0;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                            float4 _Property_93A6AEE3_Out_0 = _Tint1;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                            float4 _Property_A543E979_Out_0 = _Tint2;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                            Bindings_ColorTintMask_6636e22a321cd404c89884028dd4f823 _ColorTintMask_72CC80CD;
                            _ColorTintMask_72CC80CD.uv0 = IN.uv0;
                            float4 _ColorTintMask_72CC80CD_OutVector4_1;
                            SG_ColorTintMask_6636e22a321cd404c89884028dd4f823(TEXTURE2D_ARGS(_ColorMask, sampler_ColorMask), _ColorMask_TexelSize, _Property_AD19C48F_Out_0, _Property_93A6AEE3_Out_0, _Property_A543E979_Out_0, _ColorTintMask_72CC80CD, _ColorTintMask_72CC80CD_OutVector4_1);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #if defined(_COLORMASK_ON)
                            float4 _UseColorMask_E399B65A_Out_0 = _ColorTintMask_72CC80CD_OutVector4_1;
                            #else
                            float4 _UseColorMask_E399B65A_Out_0 = float4(1, 1, 1, 1);
                            #endif
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 _SampleTexture2D_7B9CCAC4_RGBA_0 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv0.xy);
                            float _SampleTexture2D_7B9CCAC4_R_4 = _SampleTexture2D_7B9CCAC4_RGBA_0.r;
                            float _SampleTexture2D_7B9CCAC4_G_5 = _SampleTexture2D_7B9CCAC4_RGBA_0.g;
                            float _SampleTexture2D_7B9CCAC4_B_6 = _SampleTexture2D_7B9CCAC4_RGBA_0.b;
                            float _SampleTexture2D_7B9CCAC4_A_7 = _SampleTexture2D_7B9CCAC4_RGBA_0.a;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 _Property_4BBEEE49_Out_0 = _BaseColor;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 _Multiply_D2EA8F9D_Out_2;
                            Unity_Multiply_float(_SampleTexture2D_7B9CCAC4_RGBA_0, _Property_4BBEEE49_Out_0, _Multiply_D2EA8F9D_Out_2);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 _Multiply_2695D335_Out_2;
                            Unity_Multiply_float(_UseColorMask_E399B65A_Out_0, _Multiply_D2EA8F9D_Out_2, _Multiply_2695D335_Out_2);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 _SampleTexture2D_71F12517_RGBA_0 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv0.xy);
                            float _SampleTexture2D_71F12517_R_4 = _SampleTexture2D_71F12517_RGBA_0.r;
                            float _SampleTexture2D_71F12517_G_5 = _SampleTexture2D_71F12517_RGBA_0.g;
                            float _SampleTexture2D_71F12517_B_6 = _SampleTexture2D_71F12517_RGBA_0.b;
                            float _SampleTexture2D_71F12517_A_7 = _SampleTexture2D_71F12517_RGBA_0.a;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float3 _NormalUnpack_2BEA82FF_Out_1;
                            Unity_NormalUnpack_float(_SampleTexture2D_71F12517_RGBA_0, _NormalUnpack_2BEA82FF_Out_1);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Property_5E8C46C8_Out_0 = _OcclusionStrength;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Property_F9DCB6F7_Out_0 = _Smoothness;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 _Property_2D04FE2D_Out_0 = _EmissionColor;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            Bindings_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0 _MetallicOcclusionEmissionSmoothness_2A14B742;
                            _MetallicOcclusionEmissionSmoothness_2A14B742.uv0 = IN.uv0;
                            float _MetallicOcclusionEmissionSmoothness_2A14B742_metallic_1;
                            float _MetallicOcclusionEmissionSmoothness_2A14B742_occlusion_2;
                            float3 _MetallicOcclusionEmissionSmoothness_2A14B742_emission_3;
                            float _MetallicOcclusionEmissionSmoothness_2A14B742_smoothness_4;
                            SG_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0(_Property_5E8C46C8_Out_0, _Property_F9DCB6F7_Out_0, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap), _EmissionMap_TexelSize, TEXTURE2D_ARGS(_MetallicGlossMap, sampler_MetallicGlossMap), _MetallicGlossMap_TexelSize, _Property_2D04FE2D_Out_0, _MetallicOcclusionEmissionSmoothness_2A14B742, _MetallicOcclusionEmissionSmoothness_2A14B742_metallic_1, _MetallicOcclusionEmissionSmoothness_2A14B742_occlusion_2, _MetallicOcclusionEmissionSmoothness_2A14B742_emission_3, _MetallicOcclusionEmissionSmoothness_2A14B742_smoothness_4);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float2 _Property_DAE65D7E_Out_0 = _Layer0Tiling;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Property_565C9782_Out_0 = _Layer0NormalStrength;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Property_CD37297_Out_0 = _Layer0Metallic;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Property_5FB4E89B_Out_0 = _Layer0Smoothness;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 _SampleTexture2D_A2DE4CE7_RGBA_0 = SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, IN.uv0.xy);
                            float _SampleTexture2D_A2DE4CE7_R_4 = _SampleTexture2D_A2DE4CE7_RGBA_0.r;
                            float _SampleTexture2D_A2DE4CE7_G_5 = _SampleTexture2D_A2DE4CE7_RGBA_0.g;
                            float _SampleTexture2D_A2DE4CE7_B_6 = _SampleTexture2D_A2DE4CE7_RGBA_0.b;
                            float _SampleTexture2D_A2DE4CE7_A_7 = _SampleTexture2D_A2DE4CE7_RGBA_0.a;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 _SampleTexture2D_DF0E076D_RGBA_0 = SAMPLE_TEXTURE2D(_LayerMask, sampler_LayerMask, IN.uv0.xy);
                            float _SampleTexture2D_DF0E076D_R_4 = _SampleTexture2D_DF0E076D_RGBA_0.r;
                            float _SampleTexture2D_DF0E076D_G_5 = _SampleTexture2D_DF0E076D_RGBA_0.g;
                            float _SampleTexture2D_DF0E076D_B_6 = _SampleTexture2D_DF0E076D_RGBA_0.b;
                            float _SampleTexture2D_DF0E076D_A_7 = _SampleTexture2D_DF0E076D_RGBA_0.a;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Multiply_BF4B5506_Out_2;
                            Unity_Multiply_float(_SampleTexture2D_A2DE4CE7_R_4, _SampleTexture2D_DF0E076D_R_4, _Multiply_BF4B5506_Out_2);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            Bindings_Layer_add1b3c672351c24c8c4119908f01e02 _Layer_AC9D210F;
                            _Layer_AC9D210F.uv0 = IN.uv0;
                            float4 _Layer_AC9D210F_Albedo_1;
                            float _Layer_AC9D210F_Smoothness_2;
                            float _Layer_AC9D210F_Metallic_3;
                            float3 _Layer_AC9D210F_Normal_4;
                            SG_Layer_add1b3c672351c24c8c4119908f01e02(_Multiply_2695D335_Out_2, _NormalUnpack_2BEA82FF_Out_1, _MetallicOcclusionEmissionSmoothness_2A14B742_smoothness_4, _MetallicOcclusionEmissionSmoothness_2A14B742_metallic_1, TEXTURE2D_ARGS(_Layer0, sampler_Layer0), _Layer0_TexelSize, _Property_DAE65D7E_Out_0, TEXTURE2D_ARGS(_Layer0NormalMap, sampler_Layer0NormalMap), _Layer0NormalMap_TexelSize, _Property_565C9782_Out_0, _Property_CD37297_Out_0, _Property_5FB4E89B_Out_0, _Multiply_BF4B5506_Out_2, _Layer_AC9D210F, _Layer_AC9D210F_Albedo_1, _Layer_AC9D210F_Smoothness_2, _Layer_AC9D210F_Metallic_3, _Layer_AC9D210F_Normal_4);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float2 _Property_9097A2BA_Out_0 = _Layer1Tiling;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Property_D59CFE18_Out_0 = _Layer1NormalStrength;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Property_94544800_Out_0 = _Layer1Metallic;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Property_FEAF1E87_Out_0 = _Layer1Smoothness;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Multiply_50BAAB0D_Out_2;
                            Unity_Multiply_float(_SampleTexture2D_A2DE4CE7_G_5, _SampleTexture2D_DF0E076D_G_5, _Multiply_50BAAB0D_Out_2);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            Bindings_Layer_add1b3c672351c24c8c4119908f01e02 _Layer_E43A5B0B;
                            _Layer_E43A5B0B.uv0 = IN.uv0;
                            float4 _Layer_E43A5B0B_Albedo_1;
                            float _Layer_E43A5B0B_Smoothness_2;
                            float _Layer_E43A5B0B_Metallic_3;
                            float3 _Layer_E43A5B0B_Normal_4;
                            SG_Layer_add1b3c672351c24c8c4119908f01e02(_Layer_AC9D210F_Albedo_1, _Layer_AC9D210F_Normal_4, _Layer_AC9D210F_Smoothness_2, _Layer_AC9D210F_Metallic_3, TEXTURE2D_ARGS(_Layer1, sampler_Layer1), _Layer1_TexelSize, _Property_9097A2BA_Out_0, TEXTURE2D_ARGS(_Layer1NormalMap, sampler_Layer1NormalMap), _Layer1NormalMap_TexelSize, _Property_D59CFE18_Out_0, _Property_94544800_Out_0, _Property_FEAF1E87_Out_0, _Multiply_50BAAB0D_Out_2, _Layer_E43A5B0B, _Layer_E43A5B0B_Albedo_1, _Layer_E43A5B0B_Smoothness_2, _Layer_E43A5B0B_Metallic_3, _Layer_E43A5B0B_Normal_4);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Multiply_16B99BE_Out_2;
                            Unity_Multiply_float(_SampleTexture2D_A2DE4CE7_A_7, _SampleTexture2D_DF0E076D_A_7, _Multiply_16B99BE_Out_2);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _Saturate_1D60C27B_Out_1;
                            Unity_Saturate_float(_Multiply_16B99BE_Out_2, _Saturate_1D60C27B_Out_1);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float _OneMinus_5C44DFC9_Out_1;
                            Unity_OneMinus_float(_Saturate_1D60C27B_Out_1, _OneMinus_5C44DFC9_Out_1);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float3 _Multiply_8B9DEC1C_Out_2;
                            Unity_Multiply_float(_MetallicOcclusionEmissionSmoothness_2A14B742_emission_3, (_OneMinus_5C44DFC9_Out_1.xxx), _Multiply_8B9DEC1C_Out_2);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 _Property_B15756AE_Out_0 = _Layer3EmissionColor;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 _Multiply_FAF8AEF2_Out_2;
                            Unity_Multiply_float((_Saturate_1D60C27B_Out_1.xxxx), _Property_B15756AE_Out_0, _Multiply_FAF8AEF2_Out_2);
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float3 _Add_C7227C15_Out_2;
                            Unity_Add_float3(_Multiply_8B9DEC1C_Out_2, (_Multiply_FAF8AEF2_Out_2.xyz), _Add_C7227C15_Out_2);
                            #endif
                            surface.Albedo = (_Layer_E43A5B0B_Albedo_1.xyz);
                            surface.Emission = _Add_C7227C15_Out_2;
                            surface.Alpha = 1;
                            surface.AlphaClipThreshold = 0;
                            return surface;
                        }

                        // --------------------------------------------------
                        // Structs and Packing

                        // Generated Type: Attributes
                        struct Attributes
                        {
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float3 positionOS : POSITION;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float3 normalOS : NORMAL;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 tangentOS : TANGENT;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 uv0 : TEXCOORD0;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 uv1 : TEXCOORD1;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 uv2 : TEXCOORD2;
                            #endif
                            #if UNITY_ANY_INSTANCING_ENABLED
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            uint instanceID : INSTANCEID_SEMANTIC;
                            #endif
                            #endif
                        };

                        // Generated Type: Varyings
                        struct Varyings
                        {
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 positionCS : SV_POSITION;
                            #endif
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            float4 texCoord0;
                            #endif
                            #if UNITY_ANY_INSTANCING_ENABLED
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                            #endif
                        };

                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        // Generated Type: PackedVaryings
                        struct PackedVaryings
                        {
                            float4 positionCS : SV_POSITION;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            float4 interp00 : TEXCOORD0;
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

                        // Packed Type: Varyings
                        PackedVaryings PackVaryings(Varyings input)
                        {
                            PackedVaryings output = (PackedVaryings)0;
                            output.positionCS = input.positionCS;
                            output.interp00.xyzw = input.texCoord0;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }

                        // Unpacked Type: Varyings
                        Varyings UnpackVaryings(PackedVaryings input)
                        {
                            Varyings output = (Varyings)0;
                            output.positionCS = input.positionCS;
                            output.texCoord0 = input.interp00.xyzw;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }
                        #endif

                        // --------------------------------------------------
                        // Build Graph Inputs

                        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                        {
                            VertexDescriptionInputs output;
                            ZERO_INITIALIZE(VertexDescriptionInputs, output);

                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        output.ObjectSpaceNormal = input.normalOS;
                        #endif




                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        output.ObjectSpaceTangent = input.tangentOS;
                        #endif








                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        output.ObjectSpacePosition = input.positionOS;
                        #endif












                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                        output.uv1 = input.uv1;
                        #endif








                            return output;
                        }

                        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                        {
                            SurfaceDescriptionInputs output;
                            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





































                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        output.uv0 = input.texCoord0;
                        #endif






                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                        #else
                        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                        #endif

                        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                            return output;
                        }


                        // --------------------------------------------------
                        // Main

                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"

                        ENDHLSL
                    }

                    Pass
                    {
                            // Name: <None>
                            Tags
                            {
                                "LightMode" = "Universal2D"
                            }

                            // Render State
                            Blend One Zero, One Zero
                            Cull Back
                            ZTest LEqual
                            ZWrite On
                            // ColorMask: <None>


                            HLSLPROGRAM
                            #pragma vertex vert
                            #pragma fragment frag

                            // Debug
                            // <None>

                            // --------------------------------------------------
                            // Pass

                            // Pragmas
                            #pragma prefer_hlslcc gles
                            #pragma exclude_renderers d3d11_9x
                            #pragma target 2.0
                            #pragma multi_compile_instancing

                            // Keywords
                            // PassKeywords: <None>
                            #pragma shader_feature_local _ _VERTEXOCCLUSION_ON
                            #pragma shader_feature_local _ _EMISSIONMAP_ON
                            #pragma shader_feature_local _ _COLORMASK_ON

                            #if defined(_VERTEXOCCLUSION_ON) && defined(_EMISSIONMAP_ON) && defined(_COLORMASK_ON)
                                #define KEYWORD_PERMUTATION_0
                            #elif defined(_VERTEXOCCLUSION_ON) && defined(_EMISSIONMAP_ON)
                                #define KEYWORD_PERMUTATION_1
                            #elif defined(_VERTEXOCCLUSION_ON) && defined(_COLORMASK_ON)
                                #define KEYWORD_PERMUTATION_2
                            #elif defined(_VERTEXOCCLUSION_ON)
                                #define KEYWORD_PERMUTATION_3
                            #elif defined(_EMISSIONMAP_ON) && defined(_COLORMASK_ON)
                                #define KEYWORD_PERMUTATION_4
                            #elif defined(_EMISSIONMAP_ON)
                                #define KEYWORD_PERMUTATION_5
                            #elif defined(_COLORMASK_ON)
                                #define KEYWORD_PERMUTATION_6
                            #else
                                #define KEYWORD_PERMUTATION_7
                            #endif


                            // Defines


                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        #define _NORMALMAP 1
                        #endif




                            #define _NORMAL_DROPOFF_TS 1
                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        #define ATTRIBUTES_NEED_NORMAL
                        #endif

                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        #define ATTRIBUTES_NEED_TANGENT
                        #endif

                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        #define ATTRIBUTES_NEED_TEXCOORD0
                        #endif

                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                        #define ATTRIBUTES_NEED_TEXCOORD1
                        #endif







                        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                        #define VARYINGS_NEED_TEXCOORD0
                        #endif










                            #define FEATURES_GRAPH_VERTEX
                            #define SHADERPASS_2D

                            // Includes
                            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                            // --------------------------------------------------
                            // Graph

                            // Graph Properties
                            CBUFFER_START(UnityPerMaterial)
                            float4 _BaseColor;
                            float _Smoothness;
                            float _OcclusionStrength;
                            float4 _EmissionColor;
                            float4 _Tint0;
                            float4 _Tint1;
                            float4 _Tint2;
                            float _Layer0NormalStrength;
                            float2 _Layer0Tiling;
                            float _Layer0Smoothness;
                            float _Layer0Metallic;
                            float _Layer1NormalStrength;
                            float2 _Layer1Tiling;
                            float _Layer1Smoothness;
                            float _Layer1Metallic;
                            float _Layer2Height;
                            float4 _Layer3EmissionColor;
                            float _Bitmask;
                            float4 _RevealMask_TexelSize;
                            CBUFFER_END
                            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap); float4 _BaseMap_TexelSize;
                            TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap); float4 _BumpMap_TexelSize;
                            TEXTURE2D(_MetallicGlossMap); SAMPLER(sampler_MetallicGlossMap); float4 _MetallicGlossMap_TexelSize;
                            TEXTURE2D(_EmissionMap); SAMPLER(sampler_EmissionMap); float4 _EmissionMap_TexelSize;
                            TEXTURE2D(_ColorMask); SAMPLER(sampler_ColorMask); float4 _ColorMask_TexelSize;
                            TEXTURE2D(_RevealMask); SAMPLER(sampler_RevealMask);
                            TEXTURE2D(_LayerMask); SAMPLER(sampler_LayerMask); float4 _LayerMask_TexelSize;
                            TEXTURE2D(_Layer0); SAMPLER(sampler_Layer0); float4 _Layer0_TexelSize;
                            TEXTURE2D(_Layer0NormalMap); SAMPLER(sampler_Layer0NormalMap); float4 _Layer0NormalMap_TexelSize;
                            TEXTURE2D(_Layer1); SAMPLER(sampler_Layer1); float4 _Layer1_TexelSize;
                            TEXTURE2D(_Layer1NormalMap); SAMPLER(sampler_Layer1NormalMap); float4 _Layer1NormalMap_TexelSize;
                            SAMPLER(_SampleTexture2D_8605BC1D_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2D_7B9CCAC4_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2D_71F12517_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2D_AC574377_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2D_6F08E612_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2D_A2DE4CE7_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2D_DF0E076D_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2D_51B6E38C_Sampler_3_Linear_Repeat);
                            SAMPLER(_SampleTexture2D_D0D2AB3D_Sampler_3_Linear_Repeat);

                            // Graph Functions

                            void BitwiseAND_float(float a, float b, out float Out)
                            {
                                Out = ((int)a & (int)b);
                            }

                            struct Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363
                            {
                            };

                            void SG_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363(float Vector1_E5A799E5, float Vector1_F006FE71, Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363 IN, out float OutVector1_1)
                            {
                                float _Property_39DA3013_Out_0 = Vector1_E5A799E5;
                                float _Property_2853174C_Out_0 = Vector1_F006FE71;
                                float _CustomFunction_2F22FDC6_Out_2;
                                BitwiseAND_float(_Property_39DA3013_Out_0, _Property_2853174C_Out_0, _CustomFunction_2F22FDC6_Out_2);
                                OutVector1_1 = _CustomFunction_2F22FDC6_Out_2;
                            }

                            void Unity_Comparison_Equal_float(float A, float B, out float Out)
                            {
                                Out = A == B ? 1 : 0;
                            }

                            void Unity_Divide_float(float A, float B, out float Out)
                            {
                                Out = A / B;
                            }

                            void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
                            {
                                Out = Predicate ? True : False;
                            }

                            struct Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05
                            {
                            };

                            void SG_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05(float3 Vector3_725ECFC1, float Vector1_A7EE8494, float Vector1_9BC3020B, Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05 IN, out float3 OutVector4_1)
                            {
                                float _Property_E196FA79_Out_0 = Vector1_A7EE8494;
                                float _Property_FFF31F1C_Out_0 = Vector1_9BC3020B;
                                Bindings_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363 _bitwiseAND_FC16028;
                                float _bitwiseAND_FC16028_OutVector1_1;
                                SG_bitwiseAND_10a32ab58b5a31d4fa11263fed4ed363(_Property_E196FA79_Out_0, _Property_FFF31F1C_Out_0, _bitwiseAND_FC16028, _bitwiseAND_FC16028_OutVector1_1);
                                float _Comparison_84D89BD3_Out_2;
                                Unity_Comparison_Equal_float(_bitwiseAND_FC16028_OutVector1_1, 0, _Comparison_84D89BD3_Out_2);
                                float3 _Property_A3E178F8_Out_0 = Vector3_725ECFC1;
                                float _Divide_7A3B32D4_Out_2;
                                Unity_Divide_float(0, 0, _Divide_7A3B32D4_Out_2);
                                float3 _Branch_602423C2_Out_3;
                                Unity_Branch_float3(_Comparison_84D89BD3_Out_2, _Property_A3E178F8_Out_0, (_Divide_7A3B32D4_Out_2.xxx), _Branch_602423C2_Out_3);
                                OutVector4_1 = _Branch_602423C2_Out_3;
                            }

                            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                            {
                                Out = A * B;
                            }

                            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
                            {
                                Out = A + B;
                            }

                            struct Bindings_ColorTintMask_6636e22a321cd404c89884028dd4f823
                            {
                                half4 uv0;
                            };

                            void SG_ColorTintMask_6636e22a321cd404c89884028dd4f823(TEXTURE2D_PARAM(Texture2D_917AA17E, samplerTexture2D_917AA17E), float4 Texture2D_917AA17E_TexelSize, float4 Vector4_553970CD, float4 Vector4_C861B211, float4 Vector4_ED9FE209, Bindings_ColorTintMask_6636e22a321cd404c89884028dd4f823 IN, out float4 OutVector4_1)
                            {
                                float4 _Property_526A61D_Out_0 = Vector4_553970CD;
                                float4 _SampleTexture2D_8605BC1D_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_917AA17E, samplerTexture2D_917AA17E, IN.uv0.xy);
                                float _SampleTexture2D_8605BC1D_R_4 = _SampleTexture2D_8605BC1D_RGBA_0.r;
                                float _SampleTexture2D_8605BC1D_G_5 = _SampleTexture2D_8605BC1D_RGBA_0.g;
                                float _SampleTexture2D_8605BC1D_B_6 = _SampleTexture2D_8605BC1D_RGBA_0.b;
                                float _SampleTexture2D_8605BC1D_A_7 = _SampleTexture2D_8605BC1D_RGBA_0.a;
                                float4 _Multiply_6497A688_Out_2;
                                Unity_Multiply_float(_Property_526A61D_Out_0, (_SampleTexture2D_8605BC1D_R_4.xxxx), _Multiply_6497A688_Out_2);
                                float4 _Property_3A437BCB_Out_0 = Vector4_C861B211;
                                float4 _Multiply_385888A6_Out_2;
                                Unity_Multiply_float(_Property_3A437BCB_Out_0, (_SampleTexture2D_8605BC1D_G_5.xxxx), _Multiply_385888A6_Out_2);
                                float4 _Add_A3D53A8A_Out_2;
                                Unity_Add_float4(_Multiply_6497A688_Out_2, _Multiply_385888A6_Out_2, _Add_A3D53A8A_Out_2);
                                float4 _Property_DA82EA32_Out_0 = Vector4_ED9FE209;
                                float4 _Multiply_BB15BEFC_Out_2;
                                Unity_Multiply_float(_Property_DA82EA32_Out_0, (_SampleTexture2D_8605BC1D_B_6.xxxx), _Multiply_BB15BEFC_Out_2);
                                float4 _Add_E0242413_Out_2;
                                Unity_Add_float4(_Add_A3D53A8A_Out_2, _Multiply_BB15BEFC_Out_2, _Add_E0242413_Out_2);
                                OutVector4_1 = _Add_E0242413_Out_2;
                            }

                            void Unity_NormalUnpack_float(float4 In, out float3 Out)
                            {
                                            Out = UnpackNormal(In);
                                        }

                            void Unity_Multiply_float(float A, float B, out float Out)
                            {
                                Out = A * B;
                            }

                            struct Bindings_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0
                            {
                                half4 uv0;
                            };

                            void SG_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0(float Vector1_74BD58D7, float Vector1_7BD83B87, TEXTURE2D_PARAM(Texture2D_1264D9E6, samplerTexture2D_1264D9E6), float4 Texture2D_1264D9E6_TexelSize, TEXTURE2D_PARAM(Texture2D_677C4338, samplerTexture2D_677C4338), float4 Texture2D_677C4338_TexelSize, float4 Vector4_6479F3E8, Bindings_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0 IN, out float metallic_1, out float occlusion_2, out float3 emission_3, out float smoothness_4)
                            {
                                float4 _SampleTexture2D_AC574377_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_677C4338, samplerTexture2D_677C4338, IN.uv0.xy);
                                float _SampleTexture2D_AC574377_R_4 = _SampleTexture2D_AC574377_RGBA_0.r;
                                float _SampleTexture2D_AC574377_G_5 = _SampleTexture2D_AC574377_RGBA_0.g;
                                float _SampleTexture2D_AC574377_B_6 = _SampleTexture2D_AC574377_RGBA_0.b;
                                float _SampleTexture2D_AC574377_A_7 = _SampleTexture2D_AC574377_RGBA_0.a;
                                float _Property_8B465651_Out_0 = Vector1_74BD58D7;
                                float _Multiply_DE2808F8_Out_2;
                                Unity_Multiply_float(_SampleTexture2D_AC574377_G_5, _Property_8B465651_Out_0, _Multiply_DE2808F8_Out_2);
                                float4 _SampleTexture2D_6F08E612_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_1264D9E6, samplerTexture2D_1264D9E6, IN.uv0.xy);
                                float _SampleTexture2D_6F08E612_R_4 = _SampleTexture2D_6F08E612_RGBA_0.r;
                                float _SampleTexture2D_6F08E612_G_5 = _SampleTexture2D_6F08E612_RGBA_0.g;
                                float _SampleTexture2D_6F08E612_B_6 = _SampleTexture2D_6F08E612_RGBA_0.b;
                                float _SampleTexture2D_6F08E612_A_7 = _SampleTexture2D_6F08E612_RGBA_0.a;
                                float4 _Property_892842AC_Out_0 = Vector4_6479F3E8;
                                float4 _Multiply_F33D45B4_Out_2;
                                Unity_Multiply_float(_SampleTexture2D_6F08E612_RGBA_0, _Property_892842AC_Out_0, _Multiply_F33D45B4_Out_2);
                                float4 _Multiply_3D94F4FB_Out_2;
                                Unity_Multiply_float((_SampleTexture2D_AC574377_B_6.xxxx), _Property_892842AC_Out_0, _Multiply_3D94F4FB_Out_2);
                                #if defined(_EMISSIONMAP_ON)
                                float4 _UseEmissionMap_160345FE_Out_0 = _Multiply_F33D45B4_Out_2;
                                #else
                                float4 _UseEmissionMap_160345FE_Out_0 = _Multiply_3D94F4FB_Out_2;
                                #endif
                                float _Property_9A418755_Out_0 = Vector1_7BD83B87;
                                float _Multiply_7FDD2068_Out_2;
                                Unity_Multiply_float(_SampleTexture2D_AC574377_A_7, _Property_9A418755_Out_0, _Multiply_7FDD2068_Out_2);
                                metallic_1 = _SampleTexture2D_AC574377_R_4;
                                occlusion_2 = _Multiply_DE2808F8_Out_2;
                                emission_3 = (_UseEmissionMap_160345FE_Out_0.xyz);
                                smoothness_4 = _Multiply_7FDD2068_Out_2;
                            }

                            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                            {
                                Out = UV * Tiling + Offset;
                            }

                            void Unity_Saturate_float(float In, out float Out)
                            {
                                Out = saturate(In);
                            }

                            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
                            {
                                Out = lerp(A, B, T);
                            }

                            void Unity_Lerp_float(float A, float B, float T, out float Out)
                            {
                                Out = lerp(A, B, T);
                            }

                            void Unity_NormalStrength_float(float3 In, float Strength, out float3 Out)
                            {
                                Out = float3(In.rg * Strength, lerp(1, In.b, saturate(Strength)));
                            }

                            void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
                            {
                                Out = lerp(A, B, T);
                            }

                            void Unity_Normalize_float3(float3 In, out float3 Out)
                            {
                                Out = normalize(In);
                            }

                            struct Bindings_Layer_add1b3c672351c24c8c4119908f01e02
                            {
                                half4 uv0;
                            };

                            void SG_Layer_add1b3c672351c24c8c4119908f01e02(float4 Vector4_1ED74482, float3 Vector3_1358BB6D, float Vector1_89498AA5, float Vector1_E4A5BF8E, TEXTURE2D_PARAM(Texture2D_CB4BECC7, samplerTexture2D_CB4BECC7), float4 Texture2D_CB4BECC7_TexelSize, float2 Vector2_CF2204C5, TEXTURE2D_PARAM(Texture2D_70A11B0B, samplerTexture2D_70A11B0B), float4 Texture2D_70A11B0B_TexelSize, float Vector1_71A3EDBE, float Vector1_A7A3CDEE, float Vector1_A08FC73, float Vector1_F408666D, Bindings_Layer_add1b3c672351c24c8c4119908f01e02 IN, out float4 Albedo_1, out float Smoothness_2, out float Metallic_3, out float3 Normal_4)
                            {
                                float4 _Property_E0A8A88E_Out_0 = Vector4_1ED74482;
                                float2 _Property_F2EA6A5A_Out_0 = Vector2_CF2204C5;
                                float2 _TilingAndOffset_FDF9B562_Out_3;
                                Unity_TilingAndOffset_float(IN.uv0.xy, _Property_F2EA6A5A_Out_0, float2 (0, 0), _TilingAndOffset_FDF9B562_Out_3);
                                float4 _SampleTexture2D_51B6E38C_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_CB4BECC7, samplerTexture2D_CB4BECC7, _TilingAndOffset_FDF9B562_Out_3);
                                float _SampleTexture2D_51B6E38C_R_4 = _SampleTexture2D_51B6E38C_RGBA_0.r;
                                float _SampleTexture2D_51B6E38C_G_5 = _SampleTexture2D_51B6E38C_RGBA_0.g;
                                float _SampleTexture2D_51B6E38C_B_6 = _SampleTexture2D_51B6E38C_RGBA_0.b;
                                float _SampleTexture2D_51B6E38C_A_7 = _SampleTexture2D_51B6E38C_RGBA_0.a;
                                float _Property_EFE4636B_Out_0 = Vector1_F408666D;
                                float _Saturate_783545D1_Out_1;
                                Unity_Saturate_float(_Property_EFE4636B_Out_0, _Saturate_783545D1_Out_1);
                                float4 _Lerp_B0BF9BA8_Out_3;
                                Unity_Lerp_float4(_Property_E0A8A88E_Out_0, _SampleTexture2D_51B6E38C_RGBA_0, (_Saturate_783545D1_Out_1.xxxx), _Lerp_B0BF9BA8_Out_3);
                                float _Property_A8236A77_Out_0 = Vector1_89498AA5;
                                float _Property_9DEC1A03_Out_0 = Vector1_A08FC73;
                                float _Lerp_CDE58F3A_Out_3;
                                Unity_Lerp_float(_Property_A8236A77_Out_0, _Property_9DEC1A03_Out_0, _Saturate_783545D1_Out_1, _Lerp_CDE58F3A_Out_3);
                                float _Property_B5095A31_Out_0 = Vector1_E4A5BF8E;
                                float _Property_5FFE3307_Out_0 = Vector1_A7A3CDEE;
                                float _Lerp_3EF7B8D1_Out_3;
                                Unity_Lerp_float(_Property_B5095A31_Out_0, _Property_5FFE3307_Out_0, _Saturate_783545D1_Out_1, _Lerp_3EF7B8D1_Out_3);
                                float3 _Property_2D135E56_Out_0 = Vector3_1358BB6D;
                                float4 _SampleTexture2D_D0D2AB3D_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_70A11B0B, samplerTexture2D_70A11B0B, _TilingAndOffset_FDF9B562_Out_3);
                                float _SampleTexture2D_D0D2AB3D_R_4 = _SampleTexture2D_D0D2AB3D_RGBA_0.r;
                                float _SampleTexture2D_D0D2AB3D_G_5 = _SampleTexture2D_D0D2AB3D_RGBA_0.g;
                                float _SampleTexture2D_D0D2AB3D_B_6 = _SampleTexture2D_D0D2AB3D_RGBA_0.b;
                                float _SampleTexture2D_D0D2AB3D_A_7 = _SampleTexture2D_D0D2AB3D_RGBA_0.a;
                                float3 _NormalUnpack_3906D491_Out_1;
                                Unity_NormalUnpack_float(_SampleTexture2D_D0D2AB3D_RGBA_0, _NormalUnpack_3906D491_Out_1);
                                float _Property_E1B61021_Out_0 = Vector1_71A3EDBE;
                                float3 _NormalStrength_3F987ED_Out_2;
                                Unity_NormalStrength_float(_NormalUnpack_3906D491_Out_1, _Property_E1B61021_Out_0, _NormalStrength_3F987ED_Out_2);
                                float3 _Lerp_C7A67762_Out_3;
                                Unity_Lerp_float3(_Property_2D135E56_Out_0, _NormalStrength_3F987ED_Out_2, (_Saturate_783545D1_Out_1.xxx), _Lerp_C7A67762_Out_3);
                                float3 _Normalize_FE9E0F4E_Out_1;
                                Unity_Normalize_float3(_Lerp_C7A67762_Out_3, _Normalize_FE9E0F4E_Out_1);
                                Albedo_1 = _Lerp_B0BF9BA8_Out_3;
                                Smoothness_2 = _Lerp_CDE58F3A_Out_3;
                                Metallic_3 = _Lerp_3EF7B8D1_Out_3;
                                Normal_4 = _Normalize_FE9E0F4E_Out_1;
                            }

                            // Graph Vertex
                            struct VertexDescriptionInputs
                            {
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float3 ObjectSpaceNormal;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float3 ObjectSpaceTangent;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float3 ObjectSpacePosition;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                                float4 uv1;
                                #endif
                            };

                            struct VertexDescription
                            {
                                float3 VertexPosition;
                                float3 VertexNormal;
                                float3 VertexTangent;
                            };

                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                            {
                                VertexDescription description = (VertexDescription)0;
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                                float _Property_64870DC_Out_0 = _Bitmask;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                                float4 _UV_592331DC_Out_0 = IN.uv1;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                                Bindings_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05 _NaNVertexDivideByZero_13598E37;
                                float3 _NaNVertexDivideByZero_13598E37_OutVector4_1;
                                SG_NaNVertexDivideByZero_653ad3dadcf3b8346afb17a5f528aa05(IN.ObjectSpacePosition, _Property_64870DC_Out_0, (_UV_592331DC_Out_0).x, _NaNVertexDivideByZero_13598E37, _NaNVertexDivideByZero_13598E37_OutVector4_1);
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #if defined(_VERTEXOCCLUSION_ON)
                                float3 _VertexOcclusion_EB96B2B1_Out_0 = _NaNVertexDivideByZero_13598E37_OutVector4_1;
                                #else
                                float3 _VertexOcclusion_EB96B2B1_Out_0 = IN.ObjectSpacePosition;
                                #endif
                                #endif
                                description.VertexPosition = _VertexOcclusion_EB96B2B1_Out_0;
                                description.VertexNormal = IN.ObjectSpaceNormal;
                                description.VertexTangent = IN.ObjectSpaceTangent;
                                return description;
                            }

                            // Graph Pixel
                            struct SurfaceDescriptionInputs
                            {
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 uv0;
                                #endif
                            };

                            struct SurfaceDescription
                            {
                                float3 Albedo;
                                float Alpha;
                                float AlphaClipThreshold;
                            };

                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                            {
                                SurfaceDescription surface = (SurfaceDescription)0;
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                                float4 _Property_AD19C48F_Out_0 = _Tint0;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                                float4 _Property_93A6AEE3_Out_0 = _Tint1;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                                float4 _Property_A543E979_Out_0 = _Tint2;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_6)
                                Bindings_ColorTintMask_6636e22a321cd404c89884028dd4f823 _ColorTintMask_72CC80CD;
                                _ColorTintMask_72CC80CD.uv0 = IN.uv0;
                                float4 _ColorTintMask_72CC80CD_OutVector4_1;
                                SG_ColorTintMask_6636e22a321cd404c89884028dd4f823(TEXTURE2D_ARGS(_ColorMask, sampler_ColorMask), _ColorMask_TexelSize, _Property_AD19C48F_Out_0, _Property_93A6AEE3_Out_0, _Property_A543E979_Out_0, _ColorTintMask_72CC80CD, _ColorTintMask_72CC80CD_OutVector4_1);
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #if defined(_COLORMASK_ON)
                                float4 _UseColorMask_E399B65A_Out_0 = _ColorTintMask_72CC80CD_OutVector4_1;
                                #else
                                float4 _UseColorMask_E399B65A_Out_0 = float4(1, 1, 1, 1);
                                #endif
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 _SampleTexture2D_7B9CCAC4_RGBA_0 = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv0.xy);
                                float _SampleTexture2D_7B9CCAC4_R_4 = _SampleTexture2D_7B9CCAC4_RGBA_0.r;
                                float _SampleTexture2D_7B9CCAC4_G_5 = _SampleTexture2D_7B9CCAC4_RGBA_0.g;
                                float _SampleTexture2D_7B9CCAC4_B_6 = _SampleTexture2D_7B9CCAC4_RGBA_0.b;
                                float _SampleTexture2D_7B9CCAC4_A_7 = _SampleTexture2D_7B9CCAC4_RGBA_0.a;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 _Property_4BBEEE49_Out_0 = _BaseColor;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 _Multiply_D2EA8F9D_Out_2;
                                Unity_Multiply_float(_SampleTexture2D_7B9CCAC4_RGBA_0, _Property_4BBEEE49_Out_0, _Multiply_D2EA8F9D_Out_2);
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 _Multiply_2695D335_Out_2;
                                Unity_Multiply_float(_UseColorMask_E399B65A_Out_0, _Multiply_D2EA8F9D_Out_2, _Multiply_2695D335_Out_2);
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 _SampleTexture2D_71F12517_RGBA_0 = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv0.xy);
                                float _SampleTexture2D_71F12517_R_4 = _SampleTexture2D_71F12517_RGBA_0.r;
                                float _SampleTexture2D_71F12517_G_5 = _SampleTexture2D_71F12517_RGBA_0.g;
                                float _SampleTexture2D_71F12517_B_6 = _SampleTexture2D_71F12517_RGBA_0.b;
                                float _SampleTexture2D_71F12517_A_7 = _SampleTexture2D_71F12517_RGBA_0.a;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float3 _NormalUnpack_2BEA82FF_Out_1;
                                Unity_NormalUnpack_float(_SampleTexture2D_71F12517_RGBA_0, _NormalUnpack_2BEA82FF_Out_1);
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float _Property_5E8C46C8_Out_0 = _OcclusionStrength;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float _Property_F9DCB6F7_Out_0 = _Smoothness;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 _Property_2D04FE2D_Out_0 = _EmissionColor;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                Bindings_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0 _MetallicOcclusionEmissionSmoothness_2A14B742;
                                _MetallicOcclusionEmissionSmoothness_2A14B742.uv0 = IN.uv0;
                                float _MetallicOcclusionEmissionSmoothness_2A14B742_metallic_1;
                                float _MetallicOcclusionEmissionSmoothness_2A14B742_occlusion_2;
                                float3 _MetallicOcclusionEmissionSmoothness_2A14B742_emission_3;
                                float _MetallicOcclusionEmissionSmoothness_2A14B742_smoothness_4;
                                SG_MetallicOcclusionEmissionSmoothness_e993591d00feb0c439bf9cbb13ec81a0(_Property_5E8C46C8_Out_0, _Property_F9DCB6F7_Out_0, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap), _EmissionMap_TexelSize, TEXTURE2D_ARGS(_MetallicGlossMap, sampler_MetallicGlossMap), _MetallicGlossMap_TexelSize, _Property_2D04FE2D_Out_0, _MetallicOcclusionEmissionSmoothness_2A14B742, _MetallicOcclusionEmissionSmoothness_2A14B742_metallic_1, _MetallicOcclusionEmissionSmoothness_2A14B742_occlusion_2, _MetallicOcclusionEmissionSmoothness_2A14B742_emission_3, _MetallicOcclusionEmissionSmoothness_2A14B742_smoothness_4);
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float2 _Property_DAE65D7E_Out_0 = _Layer0Tiling;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float _Property_565C9782_Out_0 = _Layer0NormalStrength;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float _Property_CD37297_Out_0 = _Layer0Metallic;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float _Property_5FB4E89B_Out_0 = _Layer0Smoothness;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 _SampleTexture2D_A2DE4CE7_RGBA_0 = SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, IN.uv0.xy);
                                float _SampleTexture2D_A2DE4CE7_R_4 = _SampleTexture2D_A2DE4CE7_RGBA_0.r;
                                float _SampleTexture2D_A2DE4CE7_G_5 = _SampleTexture2D_A2DE4CE7_RGBA_0.g;
                                float _SampleTexture2D_A2DE4CE7_B_6 = _SampleTexture2D_A2DE4CE7_RGBA_0.b;
                                float _SampleTexture2D_A2DE4CE7_A_7 = _SampleTexture2D_A2DE4CE7_RGBA_0.a;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 _SampleTexture2D_DF0E076D_RGBA_0 = SAMPLE_TEXTURE2D(_LayerMask, sampler_LayerMask, IN.uv0.xy);
                                float _SampleTexture2D_DF0E076D_R_4 = _SampleTexture2D_DF0E076D_RGBA_0.r;
                                float _SampleTexture2D_DF0E076D_G_5 = _SampleTexture2D_DF0E076D_RGBA_0.g;
                                float _SampleTexture2D_DF0E076D_B_6 = _SampleTexture2D_DF0E076D_RGBA_0.b;
                                float _SampleTexture2D_DF0E076D_A_7 = _SampleTexture2D_DF0E076D_RGBA_0.a;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float _Multiply_BF4B5506_Out_2;
                                Unity_Multiply_float(_SampleTexture2D_A2DE4CE7_R_4, _SampleTexture2D_DF0E076D_R_4, _Multiply_BF4B5506_Out_2);
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                Bindings_Layer_add1b3c672351c24c8c4119908f01e02 _Layer_AC9D210F;
                                _Layer_AC9D210F.uv0 = IN.uv0;
                                float4 _Layer_AC9D210F_Albedo_1;
                                float _Layer_AC9D210F_Smoothness_2;
                                float _Layer_AC9D210F_Metallic_3;
                                float3 _Layer_AC9D210F_Normal_4;
                                SG_Layer_add1b3c672351c24c8c4119908f01e02(_Multiply_2695D335_Out_2, _NormalUnpack_2BEA82FF_Out_1, _MetallicOcclusionEmissionSmoothness_2A14B742_smoothness_4, _MetallicOcclusionEmissionSmoothness_2A14B742_metallic_1, TEXTURE2D_ARGS(_Layer0, sampler_Layer0), _Layer0_TexelSize, _Property_DAE65D7E_Out_0, TEXTURE2D_ARGS(_Layer0NormalMap, sampler_Layer0NormalMap), _Layer0NormalMap_TexelSize, _Property_565C9782_Out_0, _Property_CD37297_Out_0, _Property_5FB4E89B_Out_0, _Multiply_BF4B5506_Out_2, _Layer_AC9D210F, _Layer_AC9D210F_Albedo_1, _Layer_AC9D210F_Smoothness_2, _Layer_AC9D210F_Metallic_3, _Layer_AC9D210F_Normal_4);
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float2 _Property_9097A2BA_Out_0 = _Layer1Tiling;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float _Property_D59CFE18_Out_0 = _Layer1NormalStrength;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float _Property_94544800_Out_0 = _Layer1Metallic;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float _Property_FEAF1E87_Out_0 = _Layer1Smoothness;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float _Multiply_50BAAB0D_Out_2;
                                Unity_Multiply_float(_SampleTexture2D_A2DE4CE7_G_5, _SampleTexture2D_DF0E076D_G_5, _Multiply_50BAAB0D_Out_2);
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                Bindings_Layer_add1b3c672351c24c8c4119908f01e02 _Layer_E43A5B0B;
                                _Layer_E43A5B0B.uv0 = IN.uv0;
                                float4 _Layer_E43A5B0B_Albedo_1;
                                float _Layer_E43A5B0B_Smoothness_2;
                                float _Layer_E43A5B0B_Metallic_3;
                                float3 _Layer_E43A5B0B_Normal_4;
                                SG_Layer_add1b3c672351c24c8c4119908f01e02(_Layer_AC9D210F_Albedo_1, _Layer_AC9D210F_Normal_4, _Layer_AC9D210F_Smoothness_2, _Layer_AC9D210F_Metallic_3, TEXTURE2D_ARGS(_Layer1, sampler_Layer1), _Layer1_TexelSize, _Property_9097A2BA_Out_0, TEXTURE2D_ARGS(_Layer1NormalMap, sampler_Layer1NormalMap), _Layer1NormalMap_TexelSize, _Property_D59CFE18_Out_0, _Property_94544800_Out_0, _Property_FEAF1E87_Out_0, _Multiply_50BAAB0D_Out_2, _Layer_E43A5B0B, _Layer_E43A5B0B_Albedo_1, _Layer_E43A5B0B_Smoothness_2, _Layer_E43A5B0B_Metallic_3, _Layer_E43A5B0B_Normal_4);
                                #endif
                                surface.Albedo = (_Layer_E43A5B0B_Albedo_1.xyz);
                                surface.Alpha = 1;
                                surface.AlphaClipThreshold = 0;
                                return surface;
                            }

                            // --------------------------------------------------
                            // Structs and Packing

                            // Generated Type: Attributes
                            struct Attributes
                            {
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float3 positionOS : POSITION;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float3 normalOS : NORMAL;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 tangentOS : TANGENT;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 uv0 : TEXCOORD0;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                                float4 uv1 : TEXCOORD1;
                                #endif
                                #if UNITY_ANY_INSTANCING_ENABLED
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                uint instanceID : INSTANCEID_SEMANTIC;
                                #endif
                                #endif
                            };

                            // Generated Type: Varyings
                            struct Varyings
                            {
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 positionCS : SV_POSITION;
                                #endif
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                float4 texCoord0;
                                #endif
                                #if UNITY_ANY_INSTANCING_ENABLED
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                uint instanceID : CUSTOM_INSTANCE_ID;
                                #endif
                                #endif
                                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                #endif
                                #endif
                                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                #endif
                                #endif
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                #endif
                                #endif
                            };

                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            // Generated Type: PackedVaryings
                            struct PackedVaryings
                            {
                                float4 positionCS : SV_POSITION;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                uint instanceID : CUSTOM_INSTANCE_ID;
                                #endif
                                float4 interp00 : TEXCOORD0;
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

                            // Packed Type: Varyings
                            PackedVaryings PackVaryings(Varyings input)
                            {
                                PackedVaryings output = (PackedVaryings)0;
                                output.positionCS = input.positionCS;
                                output.interp00.xyzw = input.texCoord0;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                output.instanceID = input.instanceID;
                                #endif
                                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                #endif
                                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                #endif
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                output.cullFace = input.cullFace;
                                #endif
                                return output;
                            }

                            // Unpacked Type: Varyings
                            Varyings UnpackVaryings(PackedVaryings input)
                            {
                                Varyings output = (Varyings)0;
                                output.positionCS = input.positionCS;
                                output.texCoord0 = input.interp00.xyzw;
                                #if UNITY_ANY_INSTANCING_ENABLED
                                output.instanceID = input.instanceID;
                                #endif
                                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                #endif
                                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                #endif
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                output.cullFace = input.cullFace;
                                #endif
                                return output;
                            }
                            #endif

                            // --------------------------------------------------
                            // Build Graph Inputs

                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                            {
                                VertexDescriptionInputs output;
                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            output.ObjectSpaceNormal = input.normalOS;
                            #endif




                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            output.ObjectSpaceTangent = input.tangentOS;
                            #endif








                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            output.ObjectSpacePosition = input.positionOS;
                            #endif












                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3)
                            output.uv1 = input.uv1;
                            #endif








                                return output;
                            }

                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                            {
                                SurfaceDescriptionInputs output;
                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





































                            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2) || defined(KEYWORD_PERMUTATION_3) || defined(KEYWORD_PERMUTATION_4) || defined(KEYWORD_PERMUTATION_5) || defined(KEYWORD_PERMUTATION_6) || defined(KEYWORD_PERMUTATION_7)
                            output.uv0 = input.texCoord0;
                            #endif






                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                            #else
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                            #endif

                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                return output;
                            }


                            // --------------------------------------------------
                            // Main

                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

                            ENDHLSL
                        }

        }
            CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
                                CustomEditor "BaSDecalShaderEditor"
                                FallBack "Hidden/Shader Graph/FallbackError"
}
