Shader "ThunderRoad/Environment/CompactSplatmap"
{
    Properties
    {
        [NoScaleOffset]_L0_PackedPBR("L0 (Base) PackedPBR (R/G) Normal Derivative (B) Greyscale Albedo", 2D) = "white" {}
        _L0_Scale("L0 Scale (Tiling, Bump, Smoothness)", Vector) = (1, 1, 0.5, 0)
        _L0_HiColor("L0 HiColor Ramp", Color) = (1, 1, 1, 1)
        _L0_DarkColor("L0 DarkColor RampContrast", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_L1_PackedPBR("L1 (Red) PackedPBR (R/G) Normal Derivative (B) Greyscale Albedo", 2D) = "white" {}
        _L1_Scale("L1 Scale (Tiling, Bump, Smoothness)", Vector) = (1, 1, 0.5, 0)
        _L1_HiColor("L1 HiColor Ramp", Color) = (1, 1, 1, 1)
        _L1_DarkColor("L1 DarkColor RampContrast", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_L2_PackedPBR("L2 (Green) PackedPBR (R/G) Normal Derivative (B) Greyscale Albedo", 2D) = "white" {}
        _L2_Scale("L2 Scale (Tiling, Bump, Smoothness)", Vector) = (1, 1, 0.5, 0)
        _L2_HiColor("L2 HiColor Ramp", Color) = (1, 1, 1, 1)
        _L2_DarkColor("L2 DarkColor RampContrast", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_L3_PackedPBR("L3 (Blue) PackedPBR (R/G) Normal Derivative (B) Greyscale Albedo", 2D) = "white" {}
        _L3_Scale("L3 Scale (Tiling, Bump, Smoothness)", Vector) = (1, 1, 0.5, 0)
        _L3_HiColor("L3 HiColor Ramp", Color) = (1, 1, 1, 1)
        _L3_DarkColor("L3 DarkColor RampContrast", Color) = (1, 1, 1, 1)
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
        [KeywordEnum(TWO, THREE, FOUR)]_BLENDS("Blends", Float) = 2
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "UniversalMaterialType" = "Lit"
            "Queue"="Geometry"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // Render State
            Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

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
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_POSITION_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_NORMAL_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TANGENT_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_FORWARD
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color : COLOR;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 texCoord0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 viewDirectionWS;
            #endif
            #if defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float2 lightmapUV;
            #endif
            #endif
            #if !defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 sh;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 fogFactorAndVertexLight;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceBiTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 VertexColor;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp2 : TEXCOORD2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp3 : TEXCOORD3;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp4 : TEXCOORD4;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp5 : TEXCOORD5;
            #endif
            #if defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float2 interp6 : TEXCOORD6;
            #endif
            #endif
            #if !defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp7 : TEXCOORD7;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp8 : TEXCOORD8;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp9 : TEXCOORD9;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyzw =  input.color;
            output.interp5.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp6.xy =  input.lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp7.xyz =  input.sh;
            #endif
            output.interp8.xyzw =  input.fogFactorAndVertexLight;
            output.interp9.xyzw =  input.shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.color = input.interp4.xyzw;
            output.viewDirectionWS = input.interp5.xyz;
            #if defined(LIGHTMAP_ON)
            output.lightmapUV = input.interp6.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp7.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp8.xyzw;
            output.shadowCoord = input.interp9.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }

        void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }

        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }

        struct Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6
        {
        };

        void SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(float2 Vector2_3A634BA2, float3 Vector3_5F009C38, float3 Vector3_C20A1E88, Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 IN, out float3 Surfgrad_1)
        {
            float2 _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0 = Vector2_3A634BA2;
            float _Split_fa955e5150c59085814bb861db67e7bb_R_1 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[0];
            float _Split_fa955e5150c59085814bb861db67e7bb_G_2 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[1];
            float _Split_fa955e5150c59085814bb861db67e7bb_B_3 = 0;
            float _Split_fa955e5150c59085814bb861db67e7bb_A_4 = 0;
            float3 _Property_d45f3727ade82d83841b281672c628a9_Out_0 = Vector3_5F009C38;
            float3 _Multiply_139befd1872fc284ae04fd888147402b_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_R_1.xxx), _Property_d45f3727ade82d83841b281672c628a9_Out_0, _Multiply_139befd1872fc284ae04fd888147402b_Out_2);
            float3 _Property_05e48436b398cf81a0bbecc045476954_Out_0 = Vector3_C20A1E88;
            float3 _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_G_2.xxx), _Property_05e48436b398cf81a0bbecc045476954_Out_0, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2);
            float3 _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
            Unity_Add_float3(_Multiply_139befd1872fc284ae04fd888147402b_Out_2, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2, _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2);
            Surfgrad_1 = _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
        }

        struct Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3
        {
            float3 WorldSpaceTangent;
            float3 WorldSpaceBiTangent;
            half4 uv0;
        };

        void SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(UnityTexture2D Texture2D_837487C9, float Vector1_165383D, float Vector1_F84F2839, float Vector1_5E2B75FF, float4 Vector4_969C6B4B, float4 Vector4_EC08F10A, UnitySamplerState SamplerState_3228d7ed10833a89a68419b45957d95d, Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 IN, out float4 AlbedoRGBSmoothA_2, out float3 SurfaceGradient_4)
        {
            float4 _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0 = Vector4_969C6B4B;
            float _Split_8f40059a26aad58ea7fc15b48377ff81_R_1 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[0];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_G_2 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[1];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_B_3 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[2];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_A_4 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[3];
            float3 _Vector3_83106cbf4654388ab2e1e131995f0304_Out_0 = float3(_Split_8f40059a26aad58ea7fc15b48377ff81_R_1, _Split_8f40059a26aad58ea7fc15b48377ff81_G_2, _Split_8f40059a26aad58ea7fc15b48377ff81_B_3);
            float4 _Property_946018a8e481d286bba9b6f9f04be770_Out_0 = Vector4_EC08F10A;
            float _Split_bd15d0620edc7d81aa0b6858188998d0_R_1 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[0];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_G_2 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[1];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_B_3 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[2];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_A_4 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[3];
            float3 _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0 = float3(_Split_bd15d0620edc7d81aa0b6858188998d0_R_1, _Split_bd15d0620edc7d81aa0b6858188998d0_G_2, _Split_bd15d0620edc7d81aa0b6858188998d0_B_3);
            float _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2;
            Unity_Multiply_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Split_bd15d0620edc7d81aa0b6858188998d0_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2);
            float _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2;
            Unity_Subtract_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2);
            UnityTexture2D _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0 = Texture2D_837487C9;
            float4 _UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0 = IN.uv0;
            float _Property_bf6c674956c02182918786a76c12072d_Out_0 = Vector1_165383D;
            float4 _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2;
            Unity_Multiply_float(_UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0, (_Property_bf6c674956c02182918786a76c12072d_Out_0.xxxx), _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2);
            float4 _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0 = SAMPLE_TEXTURE2D(_Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.tex, _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.samplerstate, (_Multiply_eddf09568efe2688bcb001ed575adb65_Out_2.xy));
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.r;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.g;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.b;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_A_7 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.a;
            float _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3;
            Unity_Smoothstep_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6, _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3);
            float3 _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3;
            Unity_Lerp_float3(_Vector3_83106cbf4654388ab2e1e131995f0304_Out_0, _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0, (_Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3.xxx), _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3);
            float3 _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2;
            Unity_Multiply_float(_Lerp_e5bf62a1b39520899c9266e476b29622_Out_3, (_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6.xxx), _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2);
            float _Split_998ad15b2f43cc858515aa49288c551e_R_1 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[0];
            float _Split_998ad15b2f43cc858515aa49288c551e_G_2 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[1];
            float _Split_998ad15b2f43cc858515aa49288c551e_B_3 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[2];
            float _Split_998ad15b2f43cc858515aa49288c551e_A_4 = 0;
            float _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0 = Vector1_5E2B75FF;
            float4 _Vector4_389e7e34d055228294ed777d6eb09074_Out_0 = float4(_Split_998ad15b2f43cc858515aa49288c551e_R_1, _Split_998ad15b2f43cc858515aa49288c551e_G_2, _Split_998ad15b2f43cc858515aa49288c551e_B_3, _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0);
            float2 _Vector2_073d67308379728ba264495744b3ffd0_Out_0 = float2(_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5);
            float2 _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3;
            Unity_Remap_float2(_Vector2_073d67308379728ba264495744b3ffd0_Out_0, float2 (0, 1), float2 (-1, 1), _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3);
            Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9;
            float3 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1;
            SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(_Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3, IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1);
            float _Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0 = Vector1_F84F2839;
            float3 _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
            Unity_Multiply_float(_SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1, (_Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0.xxx), _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2);
            AlbedoRGBSmoothA_2 = _Vector4_389e7e34d055228294ed777d6eb09074_Out_0;
            SurfaceGradient_4 = _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        struct Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4
        {
        };

        void SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(float4 Vector4_15e97cd620f64317ba5a28959ec2138a, float4 Vector4_cf83c91754ee45cf8642ff61470d2b5b, float4 Vector4_98522ade6b93428c8a4f3dc80b4d0143, float4 Vector4_2e484743ab8346889462fc834ed042d6, float4 Vector4_b10432de33d7494bad6b3bdea8b9a800, Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 IN, out float4 Out_0)
        {
            float4 _Property_f82d995fc1274519b5084f295092fc9e_Out_0 = Vector4_b10432de33d7494bad6b3bdea8b9a800;
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_R_1 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[0];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[1];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[2];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_A_4 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[3];
            float4 _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0 = Vector4_cf83c91754ee45cf8642ff61470d2b5b;
            float4 _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1.xxxx), _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0, _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2);
            float4 _Property_a2230e2c15b4464894f7213c9c26912e_Out_0 = Vector4_98522ade6b93428c8a4f3dc80b4d0143;
            float4 _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_G_2.xxxx), _Property_a2230e2c15b4464894f7213c9c26912e_Out_0, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2);
            float4 _Add_c0bd96a255484d0ab09251404523f169_Out_2;
            Unity_Add_float4(_Multiply_713e3c6b7440441cb64592afea430ce1_Out_2, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2, _Add_c0bd96a255484d0ab09251404523f169_Out_2);
            float4 _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0 = Vector4_2e484743ab8346889462fc834ed042d6;
            float4 _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_B_3.xxxx), _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2);
            float4 _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2;
            Unity_Add_float4(_Add_c0bd96a255484d0ab09251404523f169_Out_2, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2, _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2);
            float _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2;
            Unity_Add_float(_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1, _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2, _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2);
            float _Add_c463d10242e0473ca6267b3b7449ad38_Out_2;
            Unity_Add_float(_Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2, _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2);
            float _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2;
            Unity_Subtract_float(1, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2, _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2);
            float4 _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0 = Vector4_15e97cd620f64317ba5a28959ec2138a;
            float4 _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2;
            Unity_Multiply_float((_Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2.xxxx), _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2);
            float4 _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
            Unity_Add_float4(_Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2, _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2);
            Out_0 = _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
        }

        void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A - B;
        }

        void Unity_Normalize_float3(float3 In, out float3 Out)
        {
            Out = normalize(In);
        }

        struct Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3
        {
            float3 WorldSpaceNormal;
        };

        void SG_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3(float3 Vector3_F0D2275A, Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3 IN, out float3 PerturbedNormal_1)
        {
            float3 _Property_1425bba3f7108d898479b20ae4f35b3e_Out_0 = Vector3_F0D2275A;
            float3 _Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2;
            Unity_Subtract_float3(IN.WorldSpaceNormal, _Property_1425bba3f7108d898479b20ae4f35b3e_Out_0, _Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2);
            float3 _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1;
            Unity_Normalize_float3(_Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2, _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1);
            PerturbedNormal_1 = _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalWS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0 = UnityBuildTexture2DStructNoScale(_L0_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_10872350cc9241f3b46806a4018bf0f0_Out_0 = _L0_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_4caaaadf2c70490fa860c5606041738b_R_1 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[0];
            float _Split_4caaaadf2c70490fa860c5606041738b_G_2 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[1];
            float _Split_4caaaadf2c70490fa860c5606041738b_B_3 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[2];
            float _Split_4caaaadf2c70490fa860c5606041738b_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_20037ac60e0044b381c9f33069964b02_Out_0 = _L0_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0 = _L0_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnitySamplerState _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0 = UnityBuildSamplerStateStruct(SamplerState_Linear_Repeat);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0, _Split_4caaaadf2c70490fa860c5606041738b_R_1, _Split_4caaaadf2c70490fa860c5606041738b_G_2, _Split_4caaaadf2c70490fa860c5606041738b_B_3, _Property_20037ac60e0044b381c9f33069964b02_Out_0, _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_0ca12103ad5649548ee937693f1d142b_Out_0 = UnityBuildTexture2DStructNoScale(_L1_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_83bbb6e7270144b2a80b93491a289205_Out_0 = _L1_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[0];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[1];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[2];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0 = _L1_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0 = _L1_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_0ca12103ad5649548ee937693f1d142b_Out_0, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3, _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0, _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_a2c7714b15e84e09a63d503ff3404e42_Out_0 = UnityBuildTexture2DStructNoScale(_L2_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e348f547036b4b7d8a2e445d89f95329_Out_0 = _L2_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_1b6e2921f9c94dca833eef8aa999721a_R_1 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[0];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_G_2 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[1];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_B_3 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[2];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_065199bb3c2742cc9ce32af94147ceda_Out_0 = _L2_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0 = _L2_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_a2c7714b15e84e09a63d503ff3404e42_Out_0, _Split_1b6e2921f9c94dca833eef8aa999721a_R_1, _Split_1b6e2921f9c94dca833eef8aa999721a_G_2, _Split_1b6e2921f9c94dca833eef8aa999721a_B_3, _Property_065199bb3c2742cc9ce32af94147ceda_Out_0, _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #else
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0 = UnityBuildTexture2DStructNoScale(_L3_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0 = _L3_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_76aee813701645668c93c265ea8adabb_R_1 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[0];
            float _Split_76aee813701645668c93c265ea8adabb_G_2 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[1];
            float _Split_76aee813701645668c93c265ea8adabb_B_3 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[2];
            float _Split_76aee813701645668c93c265ea8adabb_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_ea91a4facf7844ddbb966160d75086d5_Out_0 = _L3_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0 = _L3_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0, _Split_76aee813701645668c93c265ea8adabb_R_1, _Split_76aee813701645668c93c265ea8adabb_G_2, _Split_76aee813701645668c93c265ea8adabb_B_3, _Property_ea91a4facf7844ddbb966160d75086d5_Out_0, _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #else
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f;
            float4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _Blends_882ad28b51d341919454eef7d4e2745e_Out_0, _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0, IN.VertexColor, _SplatmapLERP_b032b15907124a54983bf332fd37c71f, _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = float3(0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            #else
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = float3(0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = float3(0, 0, 0);
            #else
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_2f8453db063148c88bd785b74f7f693c;
            float4 _SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4((float4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4, 1.0)), (float4(_CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4, 1.0)), (float4(_Blends_0ad572595ace47a29c398846917f72fc_Out_0, 1.0)), (float4(_Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0, 1.0)), IN.VertexColor, _SplatmapLERP_2f8453db063148c88bd785b74f7f693c, _SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3 _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230;
            _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230.WorldSpaceNormal = IN.WorldSpaceNormal;
            float3 _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1;
            SG_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3((_SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0.xyz), _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230, _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_R_1 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[0];
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_G_2 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[1];
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_B_3 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[2];
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_A_4 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[3];
            #endif
            surface.BaseColor = (_SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0.xyz);
            surface.NormalWS = _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1;
            surface.Emission = float3(0, 0, 0);
            surface.Metallic = 0;
            surface.Smoothness = _Split_ac805d2580534bacb0c0f8c6fd25b39c_A_4;
            surface.Occlusion = 1;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
        #endif


            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 unnormalizedNormalWS = input.normalWS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // use bitangent on the fly like in hdrp
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // IMPORTANT! If we ever support Flip on double sided materials ensure bitangent and tangent are NOT flipped.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float crossSign = (input.tangentWS.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 bitang = crossSign * cross(input.normalWS.xyz, input.tangentWS.xyz);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // to preserve mikktspace compliance we use same scale renormFactor as was used on the normal.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // This is explained in section 2.2 in "surface gradient based bump mapping framework"
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceTangent =           renormFactor*input.tangentWS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceBiTangent =         renormFactor*bitang;
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.uv0 =                         input.texCoord0;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.VertexColor =                 input.color;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "GBuffer"
            Tags
            {
                "LightMode" = "UniversalGBuffer"
            }

            // Render State
            Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
        #pragma multi_compile _ _SHADOWS_SOFT
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
        #pragma multi_compile _ _GBUFFER_NORMALS_OCT
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_POSITION_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_NORMAL_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TANGENT_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_GBUFFER
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color : COLOR;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 texCoord0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 viewDirectionWS;
            #endif
            #if defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float2 lightmapUV;
            #endif
            #endif
            #if !defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 sh;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 fogFactorAndVertexLight;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceBiTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 VertexColor;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp2 : TEXCOORD2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp3 : TEXCOORD3;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp4 : TEXCOORD4;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp5 : TEXCOORD5;
            #endif
            #if defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float2 interp6 : TEXCOORD6;
            #endif
            #endif
            #if !defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp7 : TEXCOORD7;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp8 : TEXCOORD8;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp9 : TEXCOORD9;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyzw =  input.color;
            output.interp5.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp6.xy =  input.lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp7.xyz =  input.sh;
            #endif
            output.interp8.xyzw =  input.fogFactorAndVertexLight;
            output.interp9.xyzw =  input.shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.color = input.interp4.xyzw;
            output.viewDirectionWS = input.interp5.xyz;
            #if defined(LIGHTMAP_ON)
            output.lightmapUV = input.interp6.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp7.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp8.xyzw;
            output.shadowCoord = input.interp9.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }

        void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }

        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }

        struct Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6
        {
        };

        void SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(float2 Vector2_3A634BA2, float3 Vector3_5F009C38, float3 Vector3_C20A1E88, Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 IN, out float3 Surfgrad_1)
        {
            float2 _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0 = Vector2_3A634BA2;
            float _Split_fa955e5150c59085814bb861db67e7bb_R_1 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[0];
            float _Split_fa955e5150c59085814bb861db67e7bb_G_2 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[1];
            float _Split_fa955e5150c59085814bb861db67e7bb_B_3 = 0;
            float _Split_fa955e5150c59085814bb861db67e7bb_A_4 = 0;
            float3 _Property_d45f3727ade82d83841b281672c628a9_Out_0 = Vector3_5F009C38;
            float3 _Multiply_139befd1872fc284ae04fd888147402b_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_R_1.xxx), _Property_d45f3727ade82d83841b281672c628a9_Out_0, _Multiply_139befd1872fc284ae04fd888147402b_Out_2);
            float3 _Property_05e48436b398cf81a0bbecc045476954_Out_0 = Vector3_C20A1E88;
            float3 _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_G_2.xxx), _Property_05e48436b398cf81a0bbecc045476954_Out_0, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2);
            float3 _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
            Unity_Add_float3(_Multiply_139befd1872fc284ae04fd888147402b_Out_2, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2, _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2);
            Surfgrad_1 = _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
        }

        struct Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3
        {
            float3 WorldSpaceTangent;
            float3 WorldSpaceBiTangent;
            half4 uv0;
        };

        void SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(UnityTexture2D Texture2D_837487C9, float Vector1_165383D, float Vector1_F84F2839, float Vector1_5E2B75FF, float4 Vector4_969C6B4B, float4 Vector4_EC08F10A, UnitySamplerState SamplerState_3228d7ed10833a89a68419b45957d95d, Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 IN, out float4 AlbedoRGBSmoothA_2, out float3 SurfaceGradient_4)
        {
            float4 _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0 = Vector4_969C6B4B;
            float _Split_8f40059a26aad58ea7fc15b48377ff81_R_1 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[0];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_G_2 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[1];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_B_3 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[2];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_A_4 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[3];
            float3 _Vector3_83106cbf4654388ab2e1e131995f0304_Out_0 = float3(_Split_8f40059a26aad58ea7fc15b48377ff81_R_1, _Split_8f40059a26aad58ea7fc15b48377ff81_G_2, _Split_8f40059a26aad58ea7fc15b48377ff81_B_3);
            float4 _Property_946018a8e481d286bba9b6f9f04be770_Out_0 = Vector4_EC08F10A;
            float _Split_bd15d0620edc7d81aa0b6858188998d0_R_1 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[0];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_G_2 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[1];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_B_3 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[2];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_A_4 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[3];
            float3 _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0 = float3(_Split_bd15d0620edc7d81aa0b6858188998d0_R_1, _Split_bd15d0620edc7d81aa0b6858188998d0_G_2, _Split_bd15d0620edc7d81aa0b6858188998d0_B_3);
            float _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2;
            Unity_Multiply_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Split_bd15d0620edc7d81aa0b6858188998d0_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2);
            float _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2;
            Unity_Subtract_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2);
            UnityTexture2D _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0 = Texture2D_837487C9;
            float4 _UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0 = IN.uv0;
            float _Property_bf6c674956c02182918786a76c12072d_Out_0 = Vector1_165383D;
            float4 _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2;
            Unity_Multiply_float(_UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0, (_Property_bf6c674956c02182918786a76c12072d_Out_0.xxxx), _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2);
            float4 _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0 = SAMPLE_TEXTURE2D(_Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.tex, _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.samplerstate, (_Multiply_eddf09568efe2688bcb001ed575adb65_Out_2.xy));
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.r;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.g;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.b;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_A_7 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.a;
            float _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3;
            Unity_Smoothstep_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6, _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3);
            float3 _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3;
            Unity_Lerp_float3(_Vector3_83106cbf4654388ab2e1e131995f0304_Out_0, _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0, (_Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3.xxx), _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3);
            float3 _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2;
            Unity_Multiply_float(_Lerp_e5bf62a1b39520899c9266e476b29622_Out_3, (_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6.xxx), _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2);
            float _Split_998ad15b2f43cc858515aa49288c551e_R_1 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[0];
            float _Split_998ad15b2f43cc858515aa49288c551e_G_2 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[1];
            float _Split_998ad15b2f43cc858515aa49288c551e_B_3 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[2];
            float _Split_998ad15b2f43cc858515aa49288c551e_A_4 = 0;
            float _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0 = Vector1_5E2B75FF;
            float4 _Vector4_389e7e34d055228294ed777d6eb09074_Out_0 = float4(_Split_998ad15b2f43cc858515aa49288c551e_R_1, _Split_998ad15b2f43cc858515aa49288c551e_G_2, _Split_998ad15b2f43cc858515aa49288c551e_B_3, _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0);
            float2 _Vector2_073d67308379728ba264495744b3ffd0_Out_0 = float2(_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5);
            float2 _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3;
            Unity_Remap_float2(_Vector2_073d67308379728ba264495744b3ffd0_Out_0, float2 (0, 1), float2 (-1, 1), _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3);
            Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9;
            float3 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1;
            SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(_Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3, IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1);
            float _Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0 = Vector1_F84F2839;
            float3 _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
            Unity_Multiply_float(_SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1, (_Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0.xxx), _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2);
            AlbedoRGBSmoothA_2 = _Vector4_389e7e34d055228294ed777d6eb09074_Out_0;
            SurfaceGradient_4 = _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        struct Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4
        {
        };

        void SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(float4 Vector4_15e97cd620f64317ba5a28959ec2138a, float4 Vector4_cf83c91754ee45cf8642ff61470d2b5b, float4 Vector4_98522ade6b93428c8a4f3dc80b4d0143, float4 Vector4_2e484743ab8346889462fc834ed042d6, float4 Vector4_b10432de33d7494bad6b3bdea8b9a800, Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 IN, out float4 Out_0)
        {
            float4 _Property_f82d995fc1274519b5084f295092fc9e_Out_0 = Vector4_b10432de33d7494bad6b3bdea8b9a800;
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_R_1 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[0];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[1];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[2];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_A_4 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[3];
            float4 _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0 = Vector4_cf83c91754ee45cf8642ff61470d2b5b;
            float4 _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1.xxxx), _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0, _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2);
            float4 _Property_a2230e2c15b4464894f7213c9c26912e_Out_0 = Vector4_98522ade6b93428c8a4f3dc80b4d0143;
            float4 _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_G_2.xxxx), _Property_a2230e2c15b4464894f7213c9c26912e_Out_0, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2);
            float4 _Add_c0bd96a255484d0ab09251404523f169_Out_2;
            Unity_Add_float4(_Multiply_713e3c6b7440441cb64592afea430ce1_Out_2, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2, _Add_c0bd96a255484d0ab09251404523f169_Out_2);
            float4 _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0 = Vector4_2e484743ab8346889462fc834ed042d6;
            float4 _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_B_3.xxxx), _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2);
            float4 _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2;
            Unity_Add_float4(_Add_c0bd96a255484d0ab09251404523f169_Out_2, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2, _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2);
            float _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2;
            Unity_Add_float(_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1, _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2, _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2);
            float _Add_c463d10242e0473ca6267b3b7449ad38_Out_2;
            Unity_Add_float(_Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2, _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2);
            float _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2;
            Unity_Subtract_float(1, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2, _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2);
            float4 _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0 = Vector4_15e97cd620f64317ba5a28959ec2138a;
            float4 _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2;
            Unity_Multiply_float((_Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2.xxxx), _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2);
            float4 _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
            Unity_Add_float4(_Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2, _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2);
            Out_0 = _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
        }

        void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A - B;
        }

        void Unity_Normalize_float3(float3 In, out float3 Out)
        {
            Out = normalize(In);
        }

        struct Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3
        {
            float3 WorldSpaceNormal;
        };

        void SG_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3(float3 Vector3_F0D2275A, Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3 IN, out float3 PerturbedNormal_1)
        {
            float3 _Property_1425bba3f7108d898479b20ae4f35b3e_Out_0 = Vector3_F0D2275A;
            float3 _Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2;
            Unity_Subtract_float3(IN.WorldSpaceNormal, _Property_1425bba3f7108d898479b20ae4f35b3e_Out_0, _Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2);
            float3 _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1;
            Unity_Normalize_float3(_Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2, _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1);
            PerturbedNormal_1 = _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalWS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0 = UnityBuildTexture2DStructNoScale(_L0_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_10872350cc9241f3b46806a4018bf0f0_Out_0 = _L0_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_4caaaadf2c70490fa860c5606041738b_R_1 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[0];
            float _Split_4caaaadf2c70490fa860c5606041738b_G_2 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[1];
            float _Split_4caaaadf2c70490fa860c5606041738b_B_3 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[2];
            float _Split_4caaaadf2c70490fa860c5606041738b_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_20037ac60e0044b381c9f33069964b02_Out_0 = _L0_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0 = _L0_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnitySamplerState _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0 = UnityBuildSamplerStateStruct(SamplerState_Linear_Repeat);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0, _Split_4caaaadf2c70490fa860c5606041738b_R_1, _Split_4caaaadf2c70490fa860c5606041738b_G_2, _Split_4caaaadf2c70490fa860c5606041738b_B_3, _Property_20037ac60e0044b381c9f33069964b02_Out_0, _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_0ca12103ad5649548ee937693f1d142b_Out_0 = UnityBuildTexture2DStructNoScale(_L1_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_83bbb6e7270144b2a80b93491a289205_Out_0 = _L1_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[0];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[1];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[2];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0 = _L1_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0 = _L1_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_0ca12103ad5649548ee937693f1d142b_Out_0, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3, _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0, _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_a2c7714b15e84e09a63d503ff3404e42_Out_0 = UnityBuildTexture2DStructNoScale(_L2_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e348f547036b4b7d8a2e445d89f95329_Out_0 = _L2_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_1b6e2921f9c94dca833eef8aa999721a_R_1 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[0];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_G_2 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[1];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_B_3 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[2];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_065199bb3c2742cc9ce32af94147ceda_Out_0 = _L2_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0 = _L2_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_a2c7714b15e84e09a63d503ff3404e42_Out_0, _Split_1b6e2921f9c94dca833eef8aa999721a_R_1, _Split_1b6e2921f9c94dca833eef8aa999721a_G_2, _Split_1b6e2921f9c94dca833eef8aa999721a_B_3, _Property_065199bb3c2742cc9ce32af94147ceda_Out_0, _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #else
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0 = UnityBuildTexture2DStructNoScale(_L3_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0 = _L3_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_76aee813701645668c93c265ea8adabb_R_1 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[0];
            float _Split_76aee813701645668c93c265ea8adabb_G_2 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[1];
            float _Split_76aee813701645668c93c265ea8adabb_B_3 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[2];
            float _Split_76aee813701645668c93c265ea8adabb_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_ea91a4facf7844ddbb966160d75086d5_Out_0 = _L3_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0 = _L3_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0, _Split_76aee813701645668c93c265ea8adabb_R_1, _Split_76aee813701645668c93c265ea8adabb_G_2, _Split_76aee813701645668c93c265ea8adabb_B_3, _Property_ea91a4facf7844ddbb966160d75086d5_Out_0, _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #else
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f;
            float4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _Blends_882ad28b51d341919454eef7d4e2745e_Out_0, _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0, IN.VertexColor, _SplatmapLERP_b032b15907124a54983bf332fd37c71f, _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = float3(0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            #else
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = float3(0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = float3(0, 0, 0);
            #else
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_2f8453db063148c88bd785b74f7f693c;
            float4 _SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4((float4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4, 1.0)), (float4(_CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4, 1.0)), (float4(_Blends_0ad572595ace47a29c398846917f72fc_Out_0, 1.0)), (float4(_Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0, 1.0)), IN.VertexColor, _SplatmapLERP_2f8453db063148c88bd785b74f7f693c, _SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3 _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230;
            _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230.WorldSpaceNormal = IN.WorldSpaceNormal;
            float3 _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1;
            SG_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3((_SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0.xyz), _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230, _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_R_1 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[0];
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_G_2 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[1];
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_B_3 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[2];
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_A_4 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[3];
            #endif
            surface.BaseColor = (_SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0.xyz);
            surface.NormalWS = _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1;
            surface.Emission = float3(0, 0, 0);
            surface.Metallic = 0;
            surface.Smoothness = _Split_ac805d2580534bacb0c0f8c6fd25b39c_A_4;
            surface.Occlusion = 1;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
        #endif


            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 unnormalizedNormalWS = input.normalWS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // use bitangent on the fly like in hdrp
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // IMPORTANT! If we ever support Flip on double sided materials ensure bitangent and tangent are NOT flipped.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float crossSign = (input.tangentWS.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 bitang = crossSign * cross(input.normalWS.xyz, input.tangentWS.xyz);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // to preserve mikktspace compliance we use same scale renormFactor as was used on the normal.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // This is explained in section 2.2 in "surface gradient based bump mapping framework"
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceTangent =           renormFactor*input.tangentWS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceBiTangent =         renormFactor*bitang;
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.uv0 =                         input.texCoord0;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.VertexColor =                 input.color;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRGBufferPass.hlsl"

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
            Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On
        ColorMask 0

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SHADOWCASTER
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            // GraphFunctions: <None>

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
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
            Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On
        ColorMask 0

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHONLY
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            // GraphFunctions: <None>

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

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

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_NORMAL_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TANGENT_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_COLOR
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color : COLOR;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 texCoord0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceBiTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 VertexColor;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp2 : TEXCOORD2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp3 : TEXCOORD3;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            output.texCoord0 = input.interp2.xyzw;
            output.color = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }

        void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }

        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }

        struct Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6
        {
        };

        void SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(float2 Vector2_3A634BA2, float3 Vector3_5F009C38, float3 Vector3_C20A1E88, Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 IN, out float3 Surfgrad_1)
        {
            float2 _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0 = Vector2_3A634BA2;
            float _Split_fa955e5150c59085814bb861db67e7bb_R_1 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[0];
            float _Split_fa955e5150c59085814bb861db67e7bb_G_2 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[1];
            float _Split_fa955e5150c59085814bb861db67e7bb_B_3 = 0;
            float _Split_fa955e5150c59085814bb861db67e7bb_A_4 = 0;
            float3 _Property_d45f3727ade82d83841b281672c628a9_Out_0 = Vector3_5F009C38;
            float3 _Multiply_139befd1872fc284ae04fd888147402b_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_R_1.xxx), _Property_d45f3727ade82d83841b281672c628a9_Out_0, _Multiply_139befd1872fc284ae04fd888147402b_Out_2);
            float3 _Property_05e48436b398cf81a0bbecc045476954_Out_0 = Vector3_C20A1E88;
            float3 _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_G_2.xxx), _Property_05e48436b398cf81a0bbecc045476954_Out_0, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2);
            float3 _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
            Unity_Add_float3(_Multiply_139befd1872fc284ae04fd888147402b_Out_2, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2, _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2);
            Surfgrad_1 = _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
        }

        struct Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3
        {
            float3 WorldSpaceTangent;
            float3 WorldSpaceBiTangent;
            half4 uv0;
        };

        void SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(UnityTexture2D Texture2D_837487C9, float Vector1_165383D, float Vector1_F84F2839, float Vector1_5E2B75FF, float4 Vector4_969C6B4B, float4 Vector4_EC08F10A, UnitySamplerState SamplerState_3228d7ed10833a89a68419b45957d95d, Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 IN, out float4 AlbedoRGBSmoothA_2, out float3 SurfaceGradient_4)
        {
            float4 _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0 = Vector4_969C6B4B;
            float _Split_8f40059a26aad58ea7fc15b48377ff81_R_1 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[0];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_G_2 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[1];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_B_3 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[2];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_A_4 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[3];
            float3 _Vector3_83106cbf4654388ab2e1e131995f0304_Out_0 = float3(_Split_8f40059a26aad58ea7fc15b48377ff81_R_1, _Split_8f40059a26aad58ea7fc15b48377ff81_G_2, _Split_8f40059a26aad58ea7fc15b48377ff81_B_3);
            float4 _Property_946018a8e481d286bba9b6f9f04be770_Out_0 = Vector4_EC08F10A;
            float _Split_bd15d0620edc7d81aa0b6858188998d0_R_1 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[0];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_G_2 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[1];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_B_3 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[2];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_A_4 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[3];
            float3 _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0 = float3(_Split_bd15d0620edc7d81aa0b6858188998d0_R_1, _Split_bd15d0620edc7d81aa0b6858188998d0_G_2, _Split_bd15d0620edc7d81aa0b6858188998d0_B_3);
            float _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2;
            Unity_Multiply_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Split_bd15d0620edc7d81aa0b6858188998d0_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2);
            float _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2;
            Unity_Subtract_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2);
            UnityTexture2D _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0 = Texture2D_837487C9;
            float4 _UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0 = IN.uv0;
            float _Property_bf6c674956c02182918786a76c12072d_Out_0 = Vector1_165383D;
            float4 _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2;
            Unity_Multiply_float(_UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0, (_Property_bf6c674956c02182918786a76c12072d_Out_0.xxxx), _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2);
            float4 _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0 = SAMPLE_TEXTURE2D(_Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.tex, _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.samplerstate, (_Multiply_eddf09568efe2688bcb001ed575adb65_Out_2.xy));
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.r;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.g;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.b;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_A_7 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.a;
            float _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3;
            Unity_Smoothstep_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6, _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3);
            float3 _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3;
            Unity_Lerp_float3(_Vector3_83106cbf4654388ab2e1e131995f0304_Out_0, _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0, (_Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3.xxx), _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3);
            float3 _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2;
            Unity_Multiply_float(_Lerp_e5bf62a1b39520899c9266e476b29622_Out_3, (_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6.xxx), _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2);
            float _Split_998ad15b2f43cc858515aa49288c551e_R_1 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[0];
            float _Split_998ad15b2f43cc858515aa49288c551e_G_2 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[1];
            float _Split_998ad15b2f43cc858515aa49288c551e_B_3 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[2];
            float _Split_998ad15b2f43cc858515aa49288c551e_A_4 = 0;
            float _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0 = Vector1_5E2B75FF;
            float4 _Vector4_389e7e34d055228294ed777d6eb09074_Out_0 = float4(_Split_998ad15b2f43cc858515aa49288c551e_R_1, _Split_998ad15b2f43cc858515aa49288c551e_G_2, _Split_998ad15b2f43cc858515aa49288c551e_B_3, _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0);
            float2 _Vector2_073d67308379728ba264495744b3ffd0_Out_0 = float2(_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5);
            float2 _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3;
            Unity_Remap_float2(_Vector2_073d67308379728ba264495744b3ffd0_Out_0, float2 (0, 1), float2 (-1, 1), _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3);
            Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9;
            float3 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1;
            SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(_Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3, IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1);
            float _Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0 = Vector1_F84F2839;
            float3 _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
            Unity_Multiply_float(_SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1, (_Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0.xxx), _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2);
            AlbedoRGBSmoothA_2 = _Vector4_389e7e34d055228294ed777d6eb09074_Out_0;
            SurfaceGradient_4 = _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        struct Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4
        {
        };

        void SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(float4 Vector4_15e97cd620f64317ba5a28959ec2138a, float4 Vector4_cf83c91754ee45cf8642ff61470d2b5b, float4 Vector4_98522ade6b93428c8a4f3dc80b4d0143, float4 Vector4_2e484743ab8346889462fc834ed042d6, float4 Vector4_b10432de33d7494bad6b3bdea8b9a800, Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 IN, out float4 Out_0)
        {
            float4 _Property_f82d995fc1274519b5084f295092fc9e_Out_0 = Vector4_b10432de33d7494bad6b3bdea8b9a800;
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_R_1 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[0];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[1];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[2];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_A_4 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[3];
            float4 _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0 = Vector4_cf83c91754ee45cf8642ff61470d2b5b;
            float4 _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1.xxxx), _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0, _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2);
            float4 _Property_a2230e2c15b4464894f7213c9c26912e_Out_0 = Vector4_98522ade6b93428c8a4f3dc80b4d0143;
            float4 _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_G_2.xxxx), _Property_a2230e2c15b4464894f7213c9c26912e_Out_0, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2);
            float4 _Add_c0bd96a255484d0ab09251404523f169_Out_2;
            Unity_Add_float4(_Multiply_713e3c6b7440441cb64592afea430ce1_Out_2, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2, _Add_c0bd96a255484d0ab09251404523f169_Out_2);
            float4 _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0 = Vector4_2e484743ab8346889462fc834ed042d6;
            float4 _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_B_3.xxxx), _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2);
            float4 _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2;
            Unity_Add_float4(_Add_c0bd96a255484d0ab09251404523f169_Out_2, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2, _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2);
            float _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2;
            Unity_Add_float(_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1, _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2, _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2);
            float _Add_c463d10242e0473ca6267b3b7449ad38_Out_2;
            Unity_Add_float(_Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2, _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2);
            float _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2;
            Unity_Subtract_float(1, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2, _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2);
            float4 _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0 = Vector4_15e97cd620f64317ba5a28959ec2138a;
            float4 _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2;
            Unity_Multiply_float((_Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2.xxxx), _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2);
            float4 _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
            Unity_Add_float4(_Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2, _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2);
            Out_0 = _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
        }

        void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A - B;
        }

        void Unity_Normalize_float3(float3 In, out float3 Out)
        {
            Out = normalize(In);
        }

        struct Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3
        {
            float3 WorldSpaceNormal;
        };

        void SG_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3(float3 Vector3_F0D2275A, Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3 IN, out float3 PerturbedNormal_1)
        {
            float3 _Property_1425bba3f7108d898479b20ae4f35b3e_Out_0 = Vector3_F0D2275A;
            float3 _Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2;
            Unity_Subtract_float3(IN.WorldSpaceNormal, _Property_1425bba3f7108d898479b20ae4f35b3e_Out_0, _Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2);
            float3 _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1;
            Unity_Normalize_float3(_Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2, _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1);
            PerturbedNormal_1 = _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 NormalWS;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0 = UnityBuildTexture2DStructNoScale(_L0_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_10872350cc9241f3b46806a4018bf0f0_Out_0 = _L0_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_4caaaadf2c70490fa860c5606041738b_R_1 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[0];
            float _Split_4caaaadf2c70490fa860c5606041738b_G_2 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[1];
            float _Split_4caaaadf2c70490fa860c5606041738b_B_3 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[2];
            float _Split_4caaaadf2c70490fa860c5606041738b_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_20037ac60e0044b381c9f33069964b02_Out_0 = _L0_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0 = _L0_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnitySamplerState _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0 = UnityBuildSamplerStateStruct(SamplerState_Linear_Repeat);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0, _Split_4caaaadf2c70490fa860c5606041738b_R_1, _Split_4caaaadf2c70490fa860c5606041738b_G_2, _Split_4caaaadf2c70490fa860c5606041738b_B_3, _Property_20037ac60e0044b381c9f33069964b02_Out_0, _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_0ca12103ad5649548ee937693f1d142b_Out_0 = UnityBuildTexture2DStructNoScale(_L1_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_83bbb6e7270144b2a80b93491a289205_Out_0 = _L1_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[0];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[1];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[2];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0 = _L1_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0 = _L1_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_0ca12103ad5649548ee937693f1d142b_Out_0, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3, _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0, _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_a2c7714b15e84e09a63d503ff3404e42_Out_0 = UnityBuildTexture2DStructNoScale(_L2_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e348f547036b4b7d8a2e445d89f95329_Out_0 = _L2_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_1b6e2921f9c94dca833eef8aa999721a_R_1 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[0];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_G_2 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[1];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_B_3 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[2];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_065199bb3c2742cc9ce32af94147ceda_Out_0 = _L2_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0 = _L2_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_a2c7714b15e84e09a63d503ff3404e42_Out_0, _Split_1b6e2921f9c94dca833eef8aa999721a_R_1, _Split_1b6e2921f9c94dca833eef8aa999721a_G_2, _Split_1b6e2921f9c94dca833eef8aa999721a_B_3, _Property_065199bb3c2742cc9ce32af94147ceda_Out_0, _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = float3(0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            #else
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0 = UnityBuildTexture2DStructNoScale(_L3_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0 = _L3_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_76aee813701645668c93c265ea8adabb_R_1 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[0];
            float _Split_76aee813701645668c93c265ea8adabb_G_2 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[1];
            float _Split_76aee813701645668c93c265ea8adabb_B_3 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[2];
            float _Split_76aee813701645668c93c265ea8adabb_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_ea91a4facf7844ddbb966160d75086d5_Out_0 = _L3_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0 = _L3_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0, _Split_76aee813701645668c93c265ea8adabb_R_1, _Split_76aee813701645668c93c265ea8adabb_G_2, _Split_76aee813701645668c93c265ea8adabb_B_3, _Property_ea91a4facf7844ddbb966160d75086d5_Out_0, _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = float3(0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = float3(0, 0, 0);
            #else
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_2f8453db063148c88bd785b74f7f693c;
            float4 _SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4((float4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4, 1.0)), (float4(_CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4, 1.0)), (float4(_Blends_0ad572595ace47a29c398846917f72fc_Out_0, 1.0)), (float4(_Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0, 1.0)), IN.VertexColor, _SplatmapLERP_2f8453db063148c88bd785b74f7f693c, _SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3 _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230;
            _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230.WorldSpaceNormal = IN.WorldSpaceNormal;
            float3 _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1;
            SG_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3((_SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0.xyz), _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230, _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1);
            #endif
            surface.NormalWS = _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
        #endif


            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 unnormalizedNormalWS = input.normalWS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // use bitangent on the fly like in hdrp
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // IMPORTANT! If we ever support Flip on double sided materials ensure bitangent and tangent are NOT flipped.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float crossSign = (input.tangentWS.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 bitang = crossSign * cross(input.normalWS.xyz, input.tangentWS.xyz);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // to preserve mikktspace compliance we use same scale renormFactor as was used on the normal.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // This is explained in section 2.2 in "surface gradient based bump mapping framework"
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceTangent =           renormFactor*input.tangentWS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceBiTangent =         renormFactor*bitang;
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.uv0 =                         input.texCoord0;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.VertexColor =                 input.color;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"

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
            Cull Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD2
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_NORMAL_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TANGENT_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_COLOR
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_META
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv2 : TEXCOORD2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color : COLOR;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 texCoord0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceBiTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 VertexColor;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp2 : TEXCOORD2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp3 : TEXCOORD3;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            output.texCoord0 = input.interp2.xyzw;
            output.color = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }

        void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }

        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }

        struct Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6
        {
        };

        void SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(float2 Vector2_3A634BA2, float3 Vector3_5F009C38, float3 Vector3_C20A1E88, Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 IN, out float3 Surfgrad_1)
        {
            float2 _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0 = Vector2_3A634BA2;
            float _Split_fa955e5150c59085814bb861db67e7bb_R_1 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[0];
            float _Split_fa955e5150c59085814bb861db67e7bb_G_2 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[1];
            float _Split_fa955e5150c59085814bb861db67e7bb_B_3 = 0;
            float _Split_fa955e5150c59085814bb861db67e7bb_A_4 = 0;
            float3 _Property_d45f3727ade82d83841b281672c628a9_Out_0 = Vector3_5F009C38;
            float3 _Multiply_139befd1872fc284ae04fd888147402b_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_R_1.xxx), _Property_d45f3727ade82d83841b281672c628a9_Out_0, _Multiply_139befd1872fc284ae04fd888147402b_Out_2);
            float3 _Property_05e48436b398cf81a0bbecc045476954_Out_0 = Vector3_C20A1E88;
            float3 _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_G_2.xxx), _Property_05e48436b398cf81a0bbecc045476954_Out_0, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2);
            float3 _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
            Unity_Add_float3(_Multiply_139befd1872fc284ae04fd888147402b_Out_2, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2, _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2);
            Surfgrad_1 = _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
        }

        struct Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3
        {
            float3 WorldSpaceTangent;
            float3 WorldSpaceBiTangent;
            half4 uv0;
        };

        void SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(UnityTexture2D Texture2D_837487C9, float Vector1_165383D, float Vector1_F84F2839, float Vector1_5E2B75FF, float4 Vector4_969C6B4B, float4 Vector4_EC08F10A, UnitySamplerState SamplerState_3228d7ed10833a89a68419b45957d95d, Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 IN, out float4 AlbedoRGBSmoothA_2, out float3 SurfaceGradient_4)
        {
            float4 _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0 = Vector4_969C6B4B;
            float _Split_8f40059a26aad58ea7fc15b48377ff81_R_1 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[0];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_G_2 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[1];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_B_3 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[2];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_A_4 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[3];
            float3 _Vector3_83106cbf4654388ab2e1e131995f0304_Out_0 = float3(_Split_8f40059a26aad58ea7fc15b48377ff81_R_1, _Split_8f40059a26aad58ea7fc15b48377ff81_G_2, _Split_8f40059a26aad58ea7fc15b48377ff81_B_3);
            float4 _Property_946018a8e481d286bba9b6f9f04be770_Out_0 = Vector4_EC08F10A;
            float _Split_bd15d0620edc7d81aa0b6858188998d0_R_1 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[0];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_G_2 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[1];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_B_3 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[2];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_A_4 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[3];
            float3 _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0 = float3(_Split_bd15d0620edc7d81aa0b6858188998d0_R_1, _Split_bd15d0620edc7d81aa0b6858188998d0_G_2, _Split_bd15d0620edc7d81aa0b6858188998d0_B_3);
            float _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2;
            Unity_Multiply_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Split_bd15d0620edc7d81aa0b6858188998d0_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2);
            float _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2;
            Unity_Subtract_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2);
            UnityTexture2D _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0 = Texture2D_837487C9;
            float4 _UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0 = IN.uv0;
            float _Property_bf6c674956c02182918786a76c12072d_Out_0 = Vector1_165383D;
            float4 _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2;
            Unity_Multiply_float(_UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0, (_Property_bf6c674956c02182918786a76c12072d_Out_0.xxxx), _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2);
            float4 _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0 = SAMPLE_TEXTURE2D(_Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.tex, _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.samplerstate, (_Multiply_eddf09568efe2688bcb001ed575adb65_Out_2.xy));
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.r;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.g;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.b;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_A_7 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.a;
            float _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3;
            Unity_Smoothstep_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6, _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3);
            float3 _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3;
            Unity_Lerp_float3(_Vector3_83106cbf4654388ab2e1e131995f0304_Out_0, _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0, (_Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3.xxx), _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3);
            float3 _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2;
            Unity_Multiply_float(_Lerp_e5bf62a1b39520899c9266e476b29622_Out_3, (_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6.xxx), _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2);
            float _Split_998ad15b2f43cc858515aa49288c551e_R_1 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[0];
            float _Split_998ad15b2f43cc858515aa49288c551e_G_2 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[1];
            float _Split_998ad15b2f43cc858515aa49288c551e_B_3 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[2];
            float _Split_998ad15b2f43cc858515aa49288c551e_A_4 = 0;
            float _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0 = Vector1_5E2B75FF;
            float4 _Vector4_389e7e34d055228294ed777d6eb09074_Out_0 = float4(_Split_998ad15b2f43cc858515aa49288c551e_R_1, _Split_998ad15b2f43cc858515aa49288c551e_G_2, _Split_998ad15b2f43cc858515aa49288c551e_B_3, _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0);
            float2 _Vector2_073d67308379728ba264495744b3ffd0_Out_0 = float2(_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5);
            float2 _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3;
            Unity_Remap_float2(_Vector2_073d67308379728ba264495744b3ffd0_Out_0, float2 (0, 1), float2 (-1, 1), _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3);
            Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9;
            float3 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1;
            SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(_Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3, IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1);
            float _Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0 = Vector1_F84F2839;
            float3 _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
            Unity_Multiply_float(_SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1, (_Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0.xxx), _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2);
            AlbedoRGBSmoothA_2 = _Vector4_389e7e34d055228294ed777d6eb09074_Out_0;
            SurfaceGradient_4 = _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        struct Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4
        {
        };

        void SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(float4 Vector4_15e97cd620f64317ba5a28959ec2138a, float4 Vector4_cf83c91754ee45cf8642ff61470d2b5b, float4 Vector4_98522ade6b93428c8a4f3dc80b4d0143, float4 Vector4_2e484743ab8346889462fc834ed042d6, float4 Vector4_b10432de33d7494bad6b3bdea8b9a800, Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 IN, out float4 Out_0)
        {
            float4 _Property_f82d995fc1274519b5084f295092fc9e_Out_0 = Vector4_b10432de33d7494bad6b3bdea8b9a800;
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_R_1 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[0];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[1];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[2];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_A_4 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[3];
            float4 _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0 = Vector4_cf83c91754ee45cf8642ff61470d2b5b;
            float4 _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1.xxxx), _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0, _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2);
            float4 _Property_a2230e2c15b4464894f7213c9c26912e_Out_0 = Vector4_98522ade6b93428c8a4f3dc80b4d0143;
            float4 _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_G_2.xxxx), _Property_a2230e2c15b4464894f7213c9c26912e_Out_0, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2);
            float4 _Add_c0bd96a255484d0ab09251404523f169_Out_2;
            Unity_Add_float4(_Multiply_713e3c6b7440441cb64592afea430ce1_Out_2, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2, _Add_c0bd96a255484d0ab09251404523f169_Out_2);
            float4 _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0 = Vector4_2e484743ab8346889462fc834ed042d6;
            float4 _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_B_3.xxxx), _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2);
            float4 _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2;
            Unity_Add_float4(_Add_c0bd96a255484d0ab09251404523f169_Out_2, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2, _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2);
            float _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2;
            Unity_Add_float(_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1, _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2, _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2);
            float _Add_c463d10242e0473ca6267b3b7449ad38_Out_2;
            Unity_Add_float(_Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2, _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2);
            float _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2;
            Unity_Subtract_float(1, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2, _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2);
            float4 _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0 = Vector4_15e97cd620f64317ba5a28959ec2138a;
            float4 _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2;
            Unity_Multiply_float((_Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2.xxxx), _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2);
            float4 _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
            Unity_Add_float4(_Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2, _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2);
            Out_0 = _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0 = UnityBuildTexture2DStructNoScale(_L0_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_10872350cc9241f3b46806a4018bf0f0_Out_0 = _L0_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_4caaaadf2c70490fa860c5606041738b_R_1 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[0];
            float _Split_4caaaadf2c70490fa860c5606041738b_G_2 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[1];
            float _Split_4caaaadf2c70490fa860c5606041738b_B_3 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[2];
            float _Split_4caaaadf2c70490fa860c5606041738b_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_20037ac60e0044b381c9f33069964b02_Out_0 = _L0_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0 = _L0_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnitySamplerState _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0 = UnityBuildSamplerStateStruct(SamplerState_Linear_Repeat);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0, _Split_4caaaadf2c70490fa860c5606041738b_R_1, _Split_4caaaadf2c70490fa860c5606041738b_G_2, _Split_4caaaadf2c70490fa860c5606041738b_B_3, _Property_20037ac60e0044b381c9f33069964b02_Out_0, _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_0ca12103ad5649548ee937693f1d142b_Out_0 = UnityBuildTexture2DStructNoScale(_L1_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_83bbb6e7270144b2a80b93491a289205_Out_0 = _L1_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[0];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[1];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[2];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0 = _L1_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0 = _L1_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_0ca12103ad5649548ee937693f1d142b_Out_0, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3, _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0, _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_a2c7714b15e84e09a63d503ff3404e42_Out_0 = UnityBuildTexture2DStructNoScale(_L2_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e348f547036b4b7d8a2e445d89f95329_Out_0 = _L2_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_1b6e2921f9c94dca833eef8aa999721a_R_1 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[0];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_G_2 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[1];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_B_3 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[2];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_065199bb3c2742cc9ce32af94147ceda_Out_0 = _L2_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0 = _L2_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_a2c7714b15e84e09a63d503ff3404e42_Out_0, _Split_1b6e2921f9c94dca833eef8aa999721a_R_1, _Split_1b6e2921f9c94dca833eef8aa999721a_G_2, _Split_1b6e2921f9c94dca833eef8aa999721a_B_3, _Property_065199bb3c2742cc9ce32af94147ceda_Out_0, _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #else
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0 = UnityBuildTexture2DStructNoScale(_L3_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0 = _L3_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_76aee813701645668c93c265ea8adabb_R_1 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[0];
            float _Split_76aee813701645668c93c265ea8adabb_G_2 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[1];
            float _Split_76aee813701645668c93c265ea8adabb_B_3 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[2];
            float _Split_76aee813701645668c93c265ea8adabb_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_ea91a4facf7844ddbb966160d75086d5_Out_0 = _L3_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0 = _L3_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0, _Split_76aee813701645668c93c265ea8adabb_R_1, _Split_76aee813701645668c93c265ea8adabb_G_2, _Split_76aee813701645668c93c265ea8adabb_B_3, _Property_ea91a4facf7844ddbb966160d75086d5_Out_0, _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #else
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f;
            float4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _Blends_882ad28b51d341919454eef7d4e2745e_Out_0, _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0, IN.VertexColor, _SplatmapLERP_b032b15907124a54983bf332fd37c71f, _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0);
            #endif
            surface.BaseColor = (_SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0.xyz);
            surface.Emission = float3(0, 0, 0);
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
        #endif


            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 unnormalizedNormalWS = input.normalWS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // use bitangent on the fly like in hdrp
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // IMPORTANT! If we ever support Flip on double sided materials ensure bitangent and tangent are NOT flipped.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float crossSign = (input.tangentWS.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 bitang = crossSign * cross(input.normalWS.xyz, input.tangentWS.xyz);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // to preserve mikktspace compliance we use same scale renormFactor as was used on the normal.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // This is explained in section 2.2 in "surface gradient based bump mapping framework"
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceTangent =           renormFactor*input.tangentWS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceBiTangent =         renormFactor*bitang;
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.uv0 =                         input.texCoord0;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.VertexColor =                 input.color;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
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
            Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_NORMAL_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TANGENT_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_COLOR
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_2D
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color : COLOR;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 texCoord0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceBiTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 VertexColor;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp2 : TEXCOORD2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp3 : TEXCOORD3;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            output.texCoord0 = input.interp2.xyzw;
            output.color = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }

        void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }

        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }

        struct Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6
        {
        };

        void SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(float2 Vector2_3A634BA2, float3 Vector3_5F009C38, float3 Vector3_C20A1E88, Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 IN, out float3 Surfgrad_1)
        {
            float2 _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0 = Vector2_3A634BA2;
            float _Split_fa955e5150c59085814bb861db67e7bb_R_1 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[0];
            float _Split_fa955e5150c59085814bb861db67e7bb_G_2 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[1];
            float _Split_fa955e5150c59085814bb861db67e7bb_B_3 = 0;
            float _Split_fa955e5150c59085814bb861db67e7bb_A_4 = 0;
            float3 _Property_d45f3727ade82d83841b281672c628a9_Out_0 = Vector3_5F009C38;
            float3 _Multiply_139befd1872fc284ae04fd888147402b_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_R_1.xxx), _Property_d45f3727ade82d83841b281672c628a9_Out_0, _Multiply_139befd1872fc284ae04fd888147402b_Out_2);
            float3 _Property_05e48436b398cf81a0bbecc045476954_Out_0 = Vector3_C20A1E88;
            float3 _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_G_2.xxx), _Property_05e48436b398cf81a0bbecc045476954_Out_0, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2);
            float3 _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
            Unity_Add_float3(_Multiply_139befd1872fc284ae04fd888147402b_Out_2, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2, _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2);
            Surfgrad_1 = _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
        }

        struct Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3
        {
            float3 WorldSpaceTangent;
            float3 WorldSpaceBiTangent;
            half4 uv0;
        };

        void SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(UnityTexture2D Texture2D_837487C9, float Vector1_165383D, float Vector1_F84F2839, float Vector1_5E2B75FF, float4 Vector4_969C6B4B, float4 Vector4_EC08F10A, UnitySamplerState SamplerState_3228d7ed10833a89a68419b45957d95d, Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 IN, out float4 AlbedoRGBSmoothA_2, out float3 SurfaceGradient_4)
        {
            float4 _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0 = Vector4_969C6B4B;
            float _Split_8f40059a26aad58ea7fc15b48377ff81_R_1 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[0];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_G_2 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[1];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_B_3 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[2];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_A_4 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[3];
            float3 _Vector3_83106cbf4654388ab2e1e131995f0304_Out_0 = float3(_Split_8f40059a26aad58ea7fc15b48377ff81_R_1, _Split_8f40059a26aad58ea7fc15b48377ff81_G_2, _Split_8f40059a26aad58ea7fc15b48377ff81_B_3);
            float4 _Property_946018a8e481d286bba9b6f9f04be770_Out_0 = Vector4_EC08F10A;
            float _Split_bd15d0620edc7d81aa0b6858188998d0_R_1 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[0];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_G_2 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[1];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_B_3 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[2];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_A_4 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[3];
            float3 _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0 = float3(_Split_bd15d0620edc7d81aa0b6858188998d0_R_1, _Split_bd15d0620edc7d81aa0b6858188998d0_G_2, _Split_bd15d0620edc7d81aa0b6858188998d0_B_3);
            float _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2;
            Unity_Multiply_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Split_bd15d0620edc7d81aa0b6858188998d0_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2);
            float _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2;
            Unity_Subtract_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2);
            UnityTexture2D _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0 = Texture2D_837487C9;
            float4 _UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0 = IN.uv0;
            float _Property_bf6c674956c02182918786a76c12072d_Out_0 = Vector1_165383D;
            float4 _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2;
            Unity_Multiply_float(_UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0, (_Property_bf6c674956c02182918786a76c12072d_Out_0.xxxx), _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2);
            float4 _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0 = SAMPLE_TEXTURE2D(_Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.tex, _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.samplerstate, (_Multiply_eddf09568efe2688bcb001ed575adb65_Out_2.xy));
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.r;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.g;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.b;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_A_7 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.a;
            float _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3;
            Unity_Smoothstep_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6, _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3);
            float3 _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3;
            Unity_Lerp_float3(_Vector3_83106cbf4654388ab2e1e131995f0304_Out_0, _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0, (_Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3.xxx), _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3);
            float3 _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2;
            Unity_Multiply_float(_Lerp_e5bf62a1b39520899c9266e476b29622_Out_3, (_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6.xxx), _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2);
            float _Split_998ad15b2f43cc858515aa49288c551e_R_1 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[0];
            float _Split_998ad15b2f43cc858515aa49288c551e_G_2 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[1];
            float _Split_998ad15b2f43cc858515aa49288c551e_B_3 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[2];
            float _Split_998ad15b2f43cc858515aa49288c551e_A_4 = 0;
            float _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0 = Vector1_5E2B75FF;
            float4 _Vector4_389e7e34d055228294ed777d6eb09074_Out_0 = float4(_Split_998ad15b2f43cc858515aa49288c551e_R_1, _Split_998ad15b2f43cc858515aa49288c551e_G_2, _Split_998ad15b2f43cc858515aa49288c551e_B_3, _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0);
            float2 _Vector2_073d67308379728ba264495744b3ffd0_Out_0 = float2(_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5);
            float2 _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3;
            Unity_Remap_float2(_Vector2_073d67308379728ba264495744b3ffd0_Out_0, float2 (0, 1), float2 (-1, 1), _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3);
            Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9;
            float3 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1;
            SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(_Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3, IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1);
            float _Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0 = Vector1_F84F2839;
            float3 _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
            Unity_Multiply_float(_SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1, (_Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0.xxx), _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2);
            AlbedoRGBSmoothA_2 = _Vector4_389e7e34d055228294ed777d6eb09074_Out_0;
            SurfaceGradient_4 = _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        struct Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4
        {
        };

        void SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(float4 Vector4_15e97cd620f64317ba5a28959ec2138a, float4 Vector4_cf83c91754ee45cf8642ff61470d2b5b, float4 Vector4_98522ade6b93428c8a4f3dc80b4d0143, float4 Vector4_2e484743ab8346889462fc834ed042d6, float4 Vector4_b10432de33d7494bad6b3bdea8b9a800, Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 IN, out float4 Out_0)
        {
            float4 _Property_f82d995fc1274519b5084f295092fc9e_Out_0 = Vector4_b10432de33d7494bad6b3bdea8b9a800;
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_R_1 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[0];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[1];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[2];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_A_4 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[3];
            float4 _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0 = Vector4_cf83c91754ee45cf8642ff61470d2b5b;
            float4 _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1.xxxx), _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0, _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2);
            float4 _Property_a2230e2c15b4464894f7213c9c26912e_Out_0 = Vector4_98522ade6b93428c8a4f3dc80b4d0143;
            float4 _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_G_2.xxxx), _Property_a2230e2c15b4464894f7213c9c26912e_Out_0, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2);
            float4 _Add_c0bd96a255484d0ab09251404523f169_Out_2;
            Unity_Add_float4(_Multiply_713e3c6b7440441cb64592afea430ce1_Out_2, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2, _Add_c0bd96a255484d0ab09251404523f169_Out_2);
            float4 _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0 = Vector4_2e484743ab8346889462fc834ed042d6;
            float4 _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_B_3.xxxx), _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2);
            float4 _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2;
            Unity_Add_float4(_Add_c0bd96a255484d0ab09251404523f169_Out_2, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2, _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2);
            float _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2;
            Unity_Add_float(_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1, _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2, _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2);
            float _Add_c463d10242e0473ca6267b3b7449ad38_Out_2;
            Unity_Add_float(_Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2, _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2);
            float _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2;
            Unity_Subtract_float(1, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2, _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2);
            float4 _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0 = Vector4_15e97cd620f64317ba5a28959ec2138a;
            float4 _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2;
            Unity_Multiply_float((_Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2.xxxx), _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2);
            float4 _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
            Unity_Add_float4(_Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2, _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2);
            Out_0 = _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0 = UnityBuildTexture2DStructNoScale(_L0_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_10872350cc9241f3b46806a4018bf0f0_Out_0 = _L0_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_4caaaadf2c70490fa860c5606041738b_R_1 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[0];
            float _Split_4caaaadf2c70490fa860c5606041738b_G_2 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[1];
            float _Split_4caaaadf2c70490fa860c5606041738b_B_3 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[2];
            float _Split_4caaaadf2c70490fa860c5606041738b_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_20037ac60e0044b381c9f33069964b02_Out_0 = _L0_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0 = _L0_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnitySamplerState _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0 = UnityBuildSamplerStateStruct(SamplerState_Linear_Repeat);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0, _Split_4caaaadf2c70490fa860c5606041738b_R_1, _Split_4caaaadf2c70490fa860c5606041738b_G_2, _Split_4caaaadf2c70490fa860c5606041738b_B_3, _Property_20037ac60e0044b381c9f33069964b02_Out_0, _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_0ca12103ad5649548ee937693f1d142b_Out_0 = UnityBuildTexture2DStructNoScale(_L1_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_83bbb6e7270144b2a80b93491a289205_Out_0 = _L1_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[0];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[1];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[2];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0 = _L1_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0 = _L1_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_0ca12103ad5649548ee937693f1d142b_Out_0, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3, _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0, _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_a2c7714b15e84e09a63d503ff3404e42_Out_0 = UnityBuildTexture2DStructNoScale(_L2_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e348f547036b4b7d8a2e445d89f95329_Out_0 = _L2_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_1b6e2921f9c94dca833eef8aa999721a_R_1 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[0];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_G_2 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[1];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_B_3 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[2];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_065199bb3c2742cc9ce32af94147ceda_Out_0 = _L2_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0 = _L2_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_a2c7714b15e84e09a63d503ff3404e42_Out_0, _Split_1b6e2921f9c94dca833eef8aa999721a_R_1, _Split_1b6e2921f9c94dca833eef8aa999721a_G_2, _Split_1b6e2921f9c94dca833eef8aa999721a_B_3, _Property_065199bb3c2742cc9ce32af94147ceda_Out_0, _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #else
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0 = UnityBuildTexture2DStructNoScale(_L3_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0 = _L3_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_76aee813701645668c93c265ea8adabb_R_1 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[0];
            float _Split_76aee813701645668c93c265ea8adabb_G_2 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[1];
            float _Split_76aee813701645668c93c265ea8adabb_B_3 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[2];
            float _Split_76aee813701645668c93c265ea8adabb_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_ea91a4facf7844ddbb966160d75086d5_Out_0 = _L3_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0 = _L3_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0, _Split_76aee813701645668c93c265ea8adabb_R_1, _Split_76aee813701645668c93c265ea8adabb_G_2, _Split_76aee813701645668c93c265ea8adabb_B_3, _Property_ea91a4facf7844ddbb966160d75086d5_Out_0, _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #else
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f;
            float4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _Blends_882ad28b51d341919454eef7d4e2745e_Out_0, _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0, IN.VertexColor, _SplatmapLERP_b032b15907124a54983bf332fd37c71f, _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0);
            #endif
            surface.BaseColor = (_SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0.xyz);
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
        #endif


            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 unnormalizedNormalWS = input.normalWS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // use bitangent on the fly like in hdrp
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // IMPORTANT! If we ever support Flip on double sided materials ensure bitangent and tangent are NOT flipped.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float crossSign = (input.tangentWS.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 bitang = crossSign * cross(input.normalWS.xyz, input.tangentWS.xyz);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // to preserve mikktspace compliance we use same scale renormFactor as was used on the normal.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // This is explained in section 2.2 in "surface gradient based bump mapping framework"
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceTangent =           renormFactor*input.tangentWS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceBiTangent =         renormFactor*bitang;
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.uv0 =                         input.texCoord0;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.VertexColor =                 input.color;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

            ENDHLSL
        }
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "UniversalMaterialType" = "Lit"
            "Queue"="Geometry"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // Render State
            Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

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
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_POSITION_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_NORMAL_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TANGENT_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_FORWARD
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color : COLOR;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 texCoord0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 viewDirectionWS;
            #endif
            #if defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float2 lightmapUV;
            #endif
            #endif
            #if !defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 sh;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 fogFactorAndVertexLight;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 shadowCoord;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceBiTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 VertexColor;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp2 : TEXCOORD2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp3 : TEXCOORD3;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp4 : TEXCOORD4;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp5 : TEXCOORD5;
            #endif
            #if defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float2 interp6 : TEXCOORD6;
            #endif
            #endif
            #if !defined(LIGHTMAP_ON)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp7 : TEXCOORD7;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp8 : TEXCOORD8;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp9 : TEXCOORD9;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyzw =  input.color;
            output.interp5.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp6.xy =  input.lightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp7.xyz =  input.sh;
            #endif
            output.interp8.xyzw =  input.fogFactorAndVertexLight;
            output.interp9.xyzw =  input.shadowCoord;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.color = input.interp4.xyzw;
            output.viewDirectionWS = input.interp5.xyz;
            #if defined(LIGHTMAP_ON)
            output.lightmapUV = input.interp6.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp7.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp8.xyzw;
            output.shadowCoord = input.interp9.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }

        void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }

        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }

        struct Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6
        {
        };

        void SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(float2 Vector2_3A634BA2, float3 Vector3_5F009C38, float3 Vector3_C20A1E88, Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 IN, out float3 Surfgrad_1)
        {
            float2 _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0 = Vector2_3A634BA2;
            float _Split_fa955e5150c59085814bb861db67e7bb_R_1 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[0];
            float _Split_fa955e5150c59085814bb861db67e7bb_G_2 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[1];
            float _Split_fa955e5150c59085814bb861db67e7bb_B_3 = 0;
            float _Split_fa955e5150c59085814bb861db67e7bb_A_4 = 0;
            float3 _Property_d45f3727ade82d83841b281672c628a9_Out_0 = Vector3_5F009C38;
            float3 _Multiply_139befd1872fc284ae04fd888147402b_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_R_1.xxx), _Property_d45f3727ade82d83841b281672c628a9_Out_0, _Multiply_139befd1872fc284ae04fd888147402b_Out_2);
            float3 _Property_05e48436b398cf81a0bbecc045476954_Out_0 = Vector3_C20A1E88;
            float3 _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_G_2.xxx), _Property_05e48436b398cf81a0bbecc045476954_Out_0, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2);
            float3 _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
            Unity_Add_float3(_Multiply_139befd1872fc284ae04fd888147402b_Out_2, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2, _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2);
            Surfgrad_1 = _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
        }

        struct Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3
        {
            float3 WorldSpaceTangent;
            float3 WorldSpaceBiTangent;
            half4 uv0;
        };

        void SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(UnityTexture2D Texture2D_837487C9, float Vector1_165383D, float Vector1_F84F2839, float Vector1_5E2B75FF, float4 Vector4_969C6B4B, float4 Vector4_EC08F10A, UnitySamplerState SamplerState_3228d7ed10833a89a68419b45957d95d, Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 IN, out float4 AlbedoRGBSmoothA_2, out float3 SurfaceGradient_4)
        {
            float4 _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0 = Vector4_969C6B4B;
            float _Split_8f40059a26aad58ea7fc15b48377ff81_R_1 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[0];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_G_2 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[1];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_B_3 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[2];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_A_4 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[3];
            float3 _Vector3_83106cbf4654388ab2e1e131995f0304_Out_0 = float3(_Split_8f40059a26aad58ea7fc15b48377ff81_R_1, _Split_8f40059a26aad58ea7fc15b48377ff81_G_2, _Split_8f40059a26aad58ea7fc15b48377ff81_B_3);
            float4 _Property_946018a8e481d286bba9b6f9f04be770_Out_0 = Vector4_EC08F10A;
            float _Split_bd15d0620edc7d81aa0b6858188998d0_R_1 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[0];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_G_2 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[1];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_B_3 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[2];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_A_4 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[3];
            float3 _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0 = float3(_Split_bd15d0620edc7d81aa0b6858188998d0_R_1, _Split_bd15d0620edc7d81aa0b6858188998d0_G_2, _Split_bd15d0620edc7d81aa0b6858188998d0_B_3);
            float _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2;
            Unity_Multiply_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Split_bd15d0620edc7d81aa0b6858188998d0_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2);
            float _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2;
            Unity_Subtract_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2);
            UnityTexture2D _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0 = Texture2D_837487C9;
            float4 _UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0 = IN.uv0;
            float _Property_bf6c674956c02182918786a76c12072d_Out_0 = Vector1_165383D;
            float4 _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2;
            Unity_Multiply_float(_UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0, (_Property_bf6c674956c02182918786a76c12072d_Out_0.xxxx), _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2);
            float4 _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0 = SAMPLE_TEXTURE2D(_Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.tex, _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.samplerstate, (_Multiply_eddf09568efe2688bcb001ed575adb65_Out_2.xy));
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.r;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.g;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.b;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_A_7 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.a;
            float _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3;
            Unity_Smoothstep_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6, _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3);
            float3 _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3;
            Unity_Lerp_float3(_Vector3_83106cbf4654388ab2e1e131995f0304_Out_0, _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0, (_Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3.xxx), _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3);
            float3 _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2;
            Unity_Multiply_float(_Lerp_e5bf62a1b39520899c9266e476b29622_Out_3, (_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6.xxx), _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2);
            float _Split_998ad15b2f43cc858515aa49288c551e_R_1 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[0];
            float _Split_998ad15b2f43cc858515aa49288c551e_G_2 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[1];
            float _Split_998ad15b2f43cc858515aa49288c551e_B_3 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[2];
            float _Split_998ad15b2f43cc858515aa49288c551e_A_4 = 0;
            float _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0 = Vector1_5E2B75FF;
            float4 _Vector4_389e7e34d055228294ed777d6eb09074_Out_0 = float4(_Split_998ad15b2f43cc858515aa49288c551e_R_1, _Split_998ad15b2f43cc858515aa49288c551e_G_2, _Split_998ad15b2f43cc858515aa49288c551e_B_3, _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0);
            float2 _Vector2_073d67308379728ba264495744b3ffd0_Out_0 = float2(_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5);
            float2 _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3;
            Unity_Remap_float2(_Vector2_073d67308379728ba264495744b3ffd0_Out_0, float2 (0, 1), float2 (-1, 1), _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3);
            Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9;
            float3 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1;
            SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(_Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3, IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1);
            float _Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0 = Vector1_F84F2839;
            float3 _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
            Unity_Multiply_float(_SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1, (_Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0.xxx), _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2);
            AlbedoRGBSmoothA_2 = _Vector4_389e7e34d055228294ed777d6eb09074_Out_0;
            SurfaceGradient_4 = _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        struct Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4
        {
        };

        void SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(float4 Vector4_15e97cd620f64317ba5a28959ec2138a, float4 Vector4_cf83c91754ee45cf8642ff61470d2b5b, float4 Vector4_98522ade6b93428c8a4f3dc80b4d0143, float4 Vector4_2e484743ab8346889462fc834ed042d6, float4 Vector4_b10432de33d7494bad6b3bdea8b9a800, Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 IN, out float4 Out_0)
        {
            float4 _Property_f82d995fc1274519b5084f295092fc9e_Out_0 = Vector4_b10432de33d7494bad6b3bdea8b9a800;
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_R_1 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[0];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[1];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[2];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_A_4 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[3];
            float4 _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0 = Vector4_cf83c91754ee45cf8642ff61470d2b5b;
            float4 _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1.xxxx), _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0, _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2);
            float4 _Property_a2230e2c15b4464894f7213c9c26912e_Out_0 = Vector4_98522ade6b93428c8a4f3dc80b4d0143;
            float4 _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_G_2.xxxx), _Property_a2230e2c15b4464894f7213c9c26912e_Out_0, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2);
            float4 _Add_c0bd96a255484d0ab09251404523f169_Out_2;
            Unity_Add_float4(_Multiply_713e3c6b7440441cb64592afea430ce1_Out_2, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2, _Add_c0bd96a255484d0ab09251404523f169_Out_2);
            float4 _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0 = Vector4_2e484743ab8346889462fc834ed042d6;
            float4 _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_B_3.xxxx), _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2);
            float4 _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2;
            Unity_Add_float4(_Add_c0bd96a255484d0ab09251404523f169_Out_2, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2, _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2);
            float _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2;
            Unity_Add_float(_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1, _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2, _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2);
            float _Add_c463d10242e0473ca6267b3b7449ad38_Out_2;
            Unity_Add_float(_Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2, _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2);
            float _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2;
            Unity_Subtract_float(1, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2, _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2);
            float4 _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0 = Vector4_15e97cd620f64317ba5a28959ec2138a;
            float4 _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2;
            Unity_Multiply_float((_Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2.xxxx), _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2);
            float4 _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
            Unity_Add_float4(_Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2, _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2);
            Out_0 = _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
        }

        void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A - B;
        }

        void Unity_Normalize_float3(float3 In, out float3 Out)
        {
            Out = normalize(In);
        }

        struct Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3
        {
            float3 WorldSpaceNormal;
        };

        void SG_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3(float3 Vector3_F0D2275A, Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3 IN, out float3 PerturbedNormal_1)
        {
            float3 _Property_1425bba3f7108d898479b20ae4f35b3e_Out_0 = Vector3_F0D2275A;
            float3 _Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2;
            Unity_Subtract_float3(IN.WorldSpaceNormal, _Property_1425bba3f7108d898479b20ae4f35b3e_Out_0, _Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2);
            float3 _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1;
            Unity_Normalize_float3(_Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2, _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1);
            PerturbedNormal_1 = _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 NormalWS;
            float3 Emission;
            float Metallic;
            float Smoothness;
            float Occlusion;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0 = UnityBuildTexture2DStructNoScale(_L0_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_10872350cc9241f3b46806a4018bf0f0_Out_0 = _L0_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_4caaaadf2c70490fa860c5606041738b_R_1 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[0];
            float _Split_4caaaadf2c70490fa860c5606041738b_G_2 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[1];
            float _Split_4caaaadf2c70490fa860c5606041738b_B_3 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[2];
            float _Split_4caaaadf2c70490fa860c5606041738b_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_20037ac60e0044b381c9f33069964b02_Out_0 = _L0_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0 = _L0_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnitySamplerState _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0 = UnityBuildSamplerStateStruct(SamplerState_Linear_Repeat);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0, _Split_4caaaadf2c70490fa860c5606041738b_R_1, _Split_4caaaadf2c70490fa860c5606041738b_G_2, _Split_4caaaadf2c70490fa860c5606041738b_B_3, _Property_20037ac60e0044b381c9f33069964b02_Out_0, _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_0ca12103ad5649548ee937693f1d142b_Out_0 = UnityBuildTexture2DStructNoScale(_L1_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_83bbb6e7270144b2a80b93491a289205_Out_0 = _L1_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[0];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[1];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[2];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0 = _L1_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0 = _L1_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_0ca12103ad5649548ee937693f1d142b_Out_0, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3, _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0, _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_a2c7714b15e84e09a63d503ff3404e42_Out_0 = UnityBuildTexture2DStructNoScale(_L2_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e348f547036b4b7d8a2e445d89f95329_Out_0 = _L2_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_1b6e2921f9c94dca833eef8aa999721a_R_1 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[0];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_G_2 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[1];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_B_3 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[2];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_065199bb3c2742cc9ce32af94147ceda_Out_0 = _L2_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0 = _L2_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_a2c7714b15e84e09a63d503ff3404e42_Out_0, _Split_1b6e2921f9c94dca833eef8aa999721a_R_1, _Split_1b6e2921f9c94dca833eef8aa999721a_G_2, _Split_1b6e2921f9c94dca833eef8aa999721a_B_3, _Property_065199bb3c2742cc9ce32af94147ceda_Out_0, _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #else
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0 = UnityBuildTexture2DStructNoScale(_L3_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0 = _L3_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_76aee813701645668c93c265ea8adabb_R_1 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[0];
            float _Split_76aee813701645668c93c265ea8adabb_G_2 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[1];
            float _Split_76aee813701645668c93c265ea8adabb_B_3 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[2];
            float _Split_76aee813701645668c93c265ea8adabb_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_ea91a4facf7844ddbb966160d75086d5_Out_0 = _L3_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0 = _L3_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0, _Split_76aee813701645668c93c265ea8adabb_R_1, _Split_76aee813701645668c93c265ea8adabb_G_2, _Split_76aee813701645668c93c265ea8adabb_B_3, _Property_ea91a4facf7844ddbb966160d75086d5_Out_0, _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #else
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f;
            float4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _Blends_882ad28b51d341919454eef7d4e2745e_Out_0, _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0, IN.VertexColor, _SplatmapLERP_b032b15907124a54983bf332fd37c71f, _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = float3(0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            #else
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = float3(0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = float3(0, 0, 0);
            #else
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_2f8453db063148c88bd785b74f7f693c;
            float4 _SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4((float4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4, 1.0)), (float4(_CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4, 1.0)), (float4(_Blends_0ad572595ace47a29c398846917f72fc_Out_0, 1.0)), (float4(_Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0, 1.0)), IN.VertexColor, _SplatmapLERP_2f8453db063148c88bd785b74f7f693c, _SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3 _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230;
            _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230.WorldSpaceNormal = IN.WorldSpaceNormal;
            float3 _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1;
            SG_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3((_SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0.xyz), _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230, _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_R_1 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[0];
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_G_2 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[1];
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_B_3 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[2];
            float _Split_ac805d2580534bacb0c0f8c6fd25b39c_A_4 = _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0[3];
            #endif
            surface.BaseColor = (_SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0.xyz);
            surface.NormalWS = _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1;
            surface.Emission = float3(0, 0, 0);
            surface.Metallic = 0;
            surface.Smoothness = _Split_ac805d2580534bacb0c0f8c6fd25b39c_A_4;
            surface.Occlusion = 1;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
        #endif


            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 unnormalizedNormalWS = input.normalWS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // use bitangent on the fly like in hdrp
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // IMPORTANT! If we ever support Flip on double sided materials ensure bitangent and tangent are NOT flipped.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float crossSign = (input.tangentWS.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 bitang = crossSign * cross(input.normalWS.xyz, input.tangentWS.xyz);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // to preserve mikktspace compliance we use same scale renormFactor as was used on the normal.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // This is explained in section 2.2 in "surface gradient based bump mapping framework"
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceTangent =           renormFactor*input.tangentWS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceBiTangent =         renormFactor*bitang;
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.uv0 =                         input.texCoord0;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.VertexColor =                 input.color;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
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
            Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On
        ColorMask 0

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SHADOWCASTER
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            // GraphFunctions: <None>

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
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
            Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On
        ColorMask 0

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHONLY
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            // GraphFunctions: <None>

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

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

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_NORMAL_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TANGENT_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_COLOR
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color : COLOR;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 texCoord0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceBiTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 VertexColor;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp2 : TEXCOORD2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp3 : TEXCOORD3;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            output.texCoord0 = input.interp2.xyzw;
            output.color = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }

        void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }

        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }

        struct Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6
        {
        };

        void SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(float2 Vector2_3A634BA2, float3 Vector3_5F009C38, float3 Vector3_C20A1E88, Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 IN, out float3 Surfgrad_1)
        {
            float2 _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0 = Vector2_3A634BA2;
            float _Split_fa955e5150c59085814bb861db67e7bb_R_1 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[0];
            float _Split_fa955e5150c59085814bb861db67e7bb_G_2 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[1];
            float _Split_fa955e5150c59085814bb861db67e7bb_B_3 = 0;
            float _Split_fa955e5150c59085814bb861db67e7bb_A_4 = 0;
            float3 _Property_d45f3727ade82d83841b281672c628a9_Out_0 = Vector3_5F009C38;
            float3 _Multiply_139befd1872fc284ae04fd888147402b_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_R_1.xxx), _Property_d45f3727ade82d83841b281672c628a9_Out_0, _Multiply_139befd1872fc284ae04fd888147402b_Out_2);
            float3 _Property_05e48436b398cf81a0bbecc045476954_Out_0 = Vector3_C20A1E88;
            float3 _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_G_2.xxx), _Property_05e48436b398cf81a0bbecc045476954_Out_0, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2);
            float3 _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
            Unity_Add_float3(_Multiply_139befd1872fc284ae04fd888147402b_Out_2, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2, _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2);
            Surfgrad_1 = _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
        }

        struct Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3
        {
            float3 WorldSpaceTangent;
            float3 WorldSpaceBiTangent;
            half4 uv0;
        };

        void SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(UnityTexture2D Texture2D_837487C9, float Vector1_165383D, float Vector1_F84F2839, float Vector1_5E2B75FF, float4 Vector4_969C6B4B, float4 Vector4_EC08F10A, UnitySamplerState SamplerState_3228d7ed10833a89a68419b45957d95d, Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 IN, out float4 AlbedoRGBSmoothA_2, out float3 SurfaceGradient_4)
        {
            float4 _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0 = Vector4_969C6B4B;
            float _Split_8f40059a26aad58ea7fc15b48377ff81_R_1 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[0];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_G_2 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[1];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_B_3 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[2];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_A_4 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[3];
            float3 _Vector3_83106cbf4654388ab2e1e131995f0304_Out_0 = float3(_Split_8f40059a26aad58ea7fc15b48377ff81_R_1, _Split_8f40059a26aad58ea7fc15b48377ff81_G_2, _Split_8f40059a26aad58ea7fc15b48377ff81_B_3);
            float4 _Property_946018a8e481d286bba9b6f9f04be770_Out_0 = Vector4_EC08F10A;
            float _Split_bd15d0620edc7d81aa0b6858188998d0_R_1 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[0];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_G_2 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[1];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_B_3 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[2];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_A_4 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[3];
            float3 _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0 = float3(_Split_bd15d0620edc7d81aa0b6858188998d0_R_1, _Split_bd15d0620edc7d81aa0b6858188998d0_G_2, _Split_bd15d0620edc7d81aa0b6858188998d0_B_3);
            float _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2;
            Unity_Multiply_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Split_bd15d0620edc7d81aa0b6858188998d0_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2);
            float _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2;
            Unity_Subtract_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2);
            UnityTexture2D _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0 = Texture2D_837487C9;
            float4 _UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0 = IN.uv0;
            float _Property_bf6c674956c02182918786a76c12072d_Out_0 = Vector1_165383D;
            float4 _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2;
            Unity_Multiply_float(_UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0, (_Property_bf6c674956c02182918786a76c12072d_Out_0.xxxx), _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2);
            float4 _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0 = SAMPLE_TEXTURE2D(_Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.tex, _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.samplerstate, (_Multiply_eddf09568efe2688bcb001ed575adb65_Out_2.xy));
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.r;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.g;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.b;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_A_7 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.a;
            float _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3;
            Unity_Smoothstep_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6, _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3);
            float3 _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3;
            Unity_Lerp_float3(_Vector3_83106cbf4654388ab2e1e131995f0304_Out_0, _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0, (_Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3.xxx), _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3);
            float3 _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2;
            Unity_Multiply_float(_Lerp_e5bf62a1b39520899c9266e476b29622_Out_3, (_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6.xxx), _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2);
            float _Split_998ad15b2f43cc858515aa49288c551e_R_1 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[0];
            float _Split_998ad15b2f43cc858515aa49288c551e_G_2 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[1];
            float _Split_998ad15b2f43cc858515aa49288c551e_B_3 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[2];
            float _Split_998ad15b2f43cc858515aa49288c551e_A_4 = 0;
            float _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0 = Vector1_5E2B75FF;
            float4 _Vector4_389e7e34d055228294ed777d6eb09074_Out_0 = float4(_Split_998ad15b2f43cc858515aa49288c551e_R_1, _Split_998ad15b2f43cc858515aa49288c551e_G_2, _Split_998ad15b2f43cc858515aa49288c551e_B_3, _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0);
            float2 _Vector2_073d67308379728ba264495744b3ffd0_Out_0 = float2(_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5);
            float2 _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3;
            Unity_Remap_float2(_Vector2_073d67308379728ba264495744b3ffd0_Out_0, float2 (0, 1), float2 (-1, 1), _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3);
            Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9;
            float3 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1;
            SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(_Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3, IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1);
            float _Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0 = Vector1_F84F2839;
            float3 _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
            Unity_Multiply_float(_SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1, (_Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0.xxx), _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2);
            AlbedoRGBSmoothA_2 = _Vector4_389e7e34d055228294ed777d6eb09074_Out_0;
            SurfaceGradient_4 = _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        struct Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4
        {
        };

        void SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(float4 Vector4_15e97cd620f64317ba5a28959ec2138a, float4 Vector4_cf83c91754ee45cf8642ff61470d2b5b, float4 Vector4_98522ade6b93428c8a4f3dc80b4d0143, float4 Vector4_2e484743ab8346889462fc834ed042d6, float4 Vector4_b10432de33d7494bad6b3bdea8b9a800, Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 IN, out float4 Out_0)
        {
            float4 _Property_f82d995fc1274519b5084f295092fc9e_Out_0 = Vector4_b10432de33d7494bad6b3bdea8b9a800;
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_R_1 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[0];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[1];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[2];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_A_4 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[3];
            float4 _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0 = Vector4_cf83c91754ee45cf8642ff61470d2b5b;
            float4 _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1.xxxx), _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0, _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2);
            float4 _Property_a2230e2c15b4464894f7213c9c26912e_Out_0 = Vector4_98522ade6b93428c8a4f3dc80b4d0143;
            float4 _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_G_2.xxxx), _Property_a2230e2c15b4464894f7213c9c26912e_Out_0, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2);
            float4 _Add_c0bd96a255484d0ab09251404523f169_Out_2;
            Unity_Add_float4(_Multiply_713e3c6b7440441cb64592afea430ce1_Out_2, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2, _Add_c0bd96a255484d0ab09251404523f169_Out_2);
            float4 _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0 = Vector4_2e484743ab8346889462fc834ed042d6;
            float4 _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_B_3.xxxx), _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2);
            float4 _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2;
            Unity_Add_float4(_Add_c0bd96a255484d0ab09251404523f169_Out_2, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2, _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2);
            float _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2;
            Unity_Add_float(_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1, _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2, _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2);
            float _Add_c463d10242e0473ca6267b3b7449ad38_Out_2;
            Unity_Add_float(_Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2, _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2);
            float _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2;
            Unity_Subtract_float(1, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2, _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2);
            float4 _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0 = Vector4_15e97cd620f64317ba5a28959ec2138a;
            float4 _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2;
            Unity_Multiply_float((_Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2.xxxx), _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2);
            float4 _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
            Unity_Add_float4(_Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2, _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2);
            Out_0 = _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
        }

        void Unity_Subtract_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A - B;
        }

        void Unity_Normalize_float3(float3 In, out float3 Out)
        {
            Out = normalize(In);
        }

        struct Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3
        {
            float3 WorldSpaceNormal;
        };

        void SG_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3(float3 Vector3_F0D2275A, Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3 IN, out float3 PerturbedNormal_1)
        {
            float3 _Property_1425bba3f7108d898479b20ae4f35b3e_Out_0 = Vector3_F0D2275A;
            float3 _Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2;
            Unity_Subtract_float3(IN.WorldSpaceNormal, _Property_1425bba3f7108d898479b20ae4f35b3e_Out_0, _Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2);
            float3 _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1;
            Unity_Normalize_float3(_Subtract_c3fb7e492a559089b8acf36fece83d50_Out_2, _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1);
            PerturbedNormal_1 = _Normalize_a7b150e0ad246a81a32ebad812301b2b_Out_1;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 NormalWS;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0 = UnityBuildTexture2DStructNoScale(_L0_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_10872350cc9241f3b46806a4018bf0f0_Out_0 = _L0_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_4caaaadf2c70490fa860c5606041738b_R_1 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[0];
            float _Split_4caaaadf2c70490fa860c5606041738b_G_2 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[1];
            float _Split_4caaaadf2c70490fa860c5606041738b_B_3 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[2];
            float _Split_4caaaadf2c70490fa860c5606041738b_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_20037ac60e0044b381c9f33069964b02_Out_0 = _L0_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0 = _L0_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnitySamplerState _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0 = UnityBuildSamplerStateStruct(SamplerState_Linear_Repeat);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0, _Split_4caaaadf2c70490fa860c5606041738b_R_1, _Split_4caaaadf2c70490fa860c5606041738b_G_2, _Split_4caaaadf2c70490fa860c5606041738b_B_3, _Property_20037ac60e0044b381c9f33069964b02_Out_0, _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_0ca12103ad5649548ee937693f1d142b_Out_0 = UnityBuildTexture2DStructNoScale(_L1_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_83bbb6e7270144b2a80b93491a289205_Out_0 = _L1_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[0];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[1];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[2];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0 = _L1_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0 = _L1_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_0ca12103ad5649548ee937693f1d142b_Out_0, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3, _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0, _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_a2c7714b15e84e09a63d503ff3404e42_Out_0 = UnityBuildTexture2DStructNoScale(_L2_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e348f547036b4b7d8a2e445d89f95329_Out_0 = _L2_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_1b6e2921f9c94dca833eef8aa999721a_R_1 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[0];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_G_2 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[1];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_B_3 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[2];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_065199bb3c2742cc9ce32af94147ceda_Out_0 = _L2_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0 = _L2_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_a2c7714b15e84e09a63d503ff3404e42_Out_0, _Split_1b6e2921f9c94dca833eef8aa999721a_R_1, _Split_1b6e2921f9c94dca833eef8aa999721a_G_2, _Split_1b6e2921f9c94dca833eef8aa999721a_B_3, _Property_065199bb3c2742cc9ce32af94147ceda_Out_0, _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = float3(0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            #else
            float3 _Blends_0ad572595ace47a29c398846917f72fc_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0 = UnityBuildTexture2DStructNoScale(_L3_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0 = _L3_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_76aee813701645668c93c265ea8adabb_R_1 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[0];
            float _Split_76aee813701645668c93c265ea8adabb_G_2 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[1];
            float _Split_76aee813701645668c93c265ea8adabb_B_3 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[2];
            float _Split_76aee813701645668c93c265ea8adabb_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_ea91a4facf7844ddbb966160d75086d5_Out_0 = _L3_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0 = _L3_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0, _Split_76aee813701645668c93c265ea8adabb_R_1, _Split_76aee813701645668c93c265ea8adabb_G_2, _Split_76aee813701645668c93c265ea8adabb_B_3, _Property_ea91a4facf7844ddbb966160d75086d5_Out_0, _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = float3(0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = float3(0, 0, 0);
            #else
            float3 _Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_2f8453db063148c88bd785b74f7f693c;
            float4 _SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4((float4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4, 1.0)), (float4(_CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4, 1.0)), (float4(_Blends_0ad572595ace47a29c398846917f72fc_Out_0, 1.0)), (float4(_Blends_1c25a9d1643e4b35bdd808bb844e9de2_Out_0, 1.0)), IN.VertexColor, _SplatmapLERP_2f8453db063148c88bd785b74f7f693c, _SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3 _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230;
            _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230.WorldSpaceNormal = IN.WorldSpaceNormal;
            float3 _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1;
            SG_ResolveSurfgrad_ca4500f3f8a626b49ad080e0f18265a3((_SplatmapLERP_2f8453db063148c88bd785b74f7f693c_Out_0.xyz), _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230, _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1);
            #endif
            surface.NormalWS = _ResolveSurfgrad_ac25d1c95917436fae1b0c97600d0230_PerturbedNormal_1;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
        #endif


            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 unnormalizedNormalWS = input.normalWS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // use bitangent on the fly like in hdrp
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // IMPORTANT! If we ever support Flip on double sided materials ensure bitangent and tangent are NOT flipped.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float crossSign = (input.tangentWS.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 bitang = crossSign * cross(input.normalWS.xyz, input.tangentWS.xyz);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // to preserve mikktspace compliance we use same scale renormFactor as was used on the normal.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // This is explained in section 2.2 in "surface gradient based bump mapping framework"
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceTangent =           renormFactor*input.tangentWS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceBiTangent =         renormFactor*bitang;
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.uv0 =                         input.texCoord0;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.VertexColor =                 input.color;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"

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
            Cull Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD2
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_NORMAL_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TANGENT_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_COLOR
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_META
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv2 : TEXCOORD2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color : COLOR;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 texCoord0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceBiTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 VertexColor;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp2 : TEXCOORD2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp3 : TEXCOORD3;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            output.texCoord0 = input.interp2.xyzw;
            output.color = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }

        void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }

        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }

        struct Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6
        {
        };

        void SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(float2 Vector2_3A634BA2, float3 Vector3_5F009C38, float3 Vector3_C20A1E88, Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 IN, out float3 Surfgrad_1)
        {
            float2 _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0 = Vector2_3A634BA2;
            float _Split_fa955e5150c59085814bb861db67e7bb_R_1 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[0];
            float _Split_fa955e5150c59085814bb861db67e7bb_G_2 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[1];
            float _Split_fa955e5150c59085814bb861db67e7bb_B_3 = 0;
            float _Split_fa955e5150c59085814bb861db67e7bb_A_4 = 0;
            float3 _Property_d45f3727ade82d83841b281672c628a9_Out_0 = Vector3_5F009C38;
            float3 _Multiply_139befd1872fc284ae04fd888147402b_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_R_1.xxx), _Property_d45f3727ade82d83841b281672c628a9_Out_0, _Multiply_139befd1872fc284ae04fd888147402b_Out_2);
            float3 _Property_05e48436b398cf81a0bbecc045476954_Out_0 = Vector3_C20A1E88;
            float3 _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_G_2.xxx), _Property_05e48436b398cf81a0bbecc045476954_Out_0, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2);
            float3 _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
            Unity_Add_float3(_Multiply_139befd1872fc284ae04fd888147402b_Out_2, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2, _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2);
            Surfgrad_1 = _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
        }

        struct Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3
        {
            float3 WorldSpaceTangent;
            float3 WorldSpaceBiTangent;
            half4 uv0;
        };

        void SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(UnityTexture2D Texture2D_837487C9, float Vector1_165383D, float Vector1_F84F2839, float Vector1_5E2B75FF, float4 Vector4_969C6B4B, float4 Vector4_EC08F10A, UnitySamplerState SamplerState_3228d7ed10833a89a68419b45957d95d, Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 IN, out float4 AlbedoRGBSmoothA_2, out float3 SurfaceGradient_4)
        {
            float4 _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0 = Vector4_969C6B4B;
            float _Split_8f40059a26aad58ea7fc15b48377ff81_R_1 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[0];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_G_2 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[1];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_B_3 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[2];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_A_4 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[3];
            float3 _Vector3_83106cbf4654388ab2e1e131995f0304_Out_0 = float3(_Split_8f40059a26aad58ea7fc15b48377ff81_R_1, _Split_8f40059a26aad58ea7fc15b48377ff81_G_2, _Split_8f40059a26aad58ea7fc15b48377ff81_B_3);
            float4 _Property_946018a8e481d286bba9b6f9f04be770_Out_0 = Vector4_EC08F10A;
            float _Split_bd15d0620edc7d81aa0b6858188998d0_R_1 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[0];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_G_2 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[1];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_B_3 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[2];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_A_4 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[3];
            float3 _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0 = float3(_Split_bd15d0620edc7d81aa0b6858188998d0_R_1, _Split_bd15d0620edc7d81aa0b6858188998d0_G_2, _Split_bd15d0620edc7d81aa0b6858188998d0_B_3);
            float _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2;
            Unity_Multiply_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Split_bd15d0620edc7d81aa0b6858188998d0_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2);
            float _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2;
            Unity_Subtract_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2);
            UnityTexture2D _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0 = Texture2D_837487C9;
            float4 _UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0 = IN.uv0;
            float _Property_bf6c674956c02182918786a76c12072d_Out_0 = Vector1_165383D;
            float4 _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2;
            Unity_Multiply_float(_UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0, (_Property_bf6c674956c02182918786a76c12072d_Out_0.xxxx), _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2);
            float4 _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0 = SAMPLE_TEXTURE2D(_Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.tex, _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.samplerstate, (_Multiply_eddf09568efe2688bcb001ed575adb65_Out_2.xy));
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.r;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.g;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.b;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_A_7 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.a;
            float _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3;
            Unity_Smoothstep_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6, _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3);
            float3 _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3;
            Unity_Lerp_float3(_Vector3_83106cbf4654388ab2e1e131995f0304_Out_0, _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0, (_Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3.xxx), _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3);
            float3 _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2;
            Unity_Multiply_float(_Lerp_e5bf62a1b39520899c9266e476b29622_Out_3, (_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6.xxx), _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2);
            float _Split_998ad15b2f43cc858515aa49288c551e_R_1 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[0];
            float _Split_998ad15b2f43cc858515aa49288c551e_G_2 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[1];
            float _Split_998ad15b2f43cc858515aa49288c551e_B_3 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[2];
            float _Split_998ad15b2f43cc858515aa49288c551e_A_4 = 0;
            float _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0 = Vector1_5E2B75FF;
            float4 _Vector4_389e7e34d055228294ed777d6eb09074_Out_0 = float4(_Split_998ad15b2f43cc858515aa49288c551e_R_1, _Split_998ad15b2f43cc858515aa49288c551e_G_2, _Split_998ad15b2f43cc858515aa49288c551e_B_3, _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0);
            float2 _Vector2_073d67308379728ba264495744b3ffd0_Out_0 = float2(_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5);
            float2 _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3;
            Unity_Remap_float2(_Vector2_073d67308379728ba264495744b3ffd0_Out_0, float2 (0, 1), float2 (-1, 1), _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3);
            Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9;
            float3 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1;
            SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(_Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3, IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1);
            float _Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0 = Vector1_F84F2839;
            float3 _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
            Unity_Multiply_float(_SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1, (_Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0.xxx), _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2);
            AlbedoRGBSmoothA_2 = _Vector4_389e7e34d055228294ed777d6eb09074_Out_0;
            SurfaceGradient_4 = _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        struct Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4
        {
        };

        void SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(float4 Vector4_15e97cd620f64317ba5a28959ec2138a, float4 Vector4_cf83c91754ee45cf8642ff61470d2b5b, float4 Vector4_98522ade6b93428c8a4f3dc80b4d0143, float4 Vector4_2e484743ab8346889462fc834ed042d6, float4 Vector4_b10432de33d7494bad6b3bdea8b9a800, Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 IN, out float4 Out_0)
        {
            float4 _Property_f82d995fc1274519b5084f295092fc9e_Out_0 = Vector4_b10432de33d7494bad6b3bdea8b9a800;
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_R_1 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[0];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[1];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[2];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_A_4 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[3];
            float4 _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0 = Vector4_cf83c91754ee45cf8642ff61470d2b5b;
            float4 _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1.xxxx), _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0, _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2);
            float4 _Property_a2230e2c15b4464894f7213c9c26912e_Out_0 = Vector4_98522ade6b93428c8a4f3dc80b4d0143;
            float4 _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_G_2.xxxx), _Property_a2230e2c15b4464894f7213c9c26912e_Out_0, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2);
            float4 _Add_c0bd96a255484d0ab09251404523f169_Out_2;
            Unity_Add_float4(_Multiply_713e3c6b7440441cb64592afea430ce1_Out_2, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2, _Add_c0bd96a255484d0ab09251404523f169_Out_2);
            float4 _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0 = Vector4_2e484743ab8346889462fc834ed042d6;
            float4 _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_B_3.xxxx), _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2);
            float4 _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2;
            Unity_Add_float4(_Add_c0bd96a255484d0ab09251404523f169_Out_2, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2, _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2);
            float _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2;
            Unity_Add_float(_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1, _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2, _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2);
            float _Add_c463d10242e0473ca6267b3b7449ad38_Out_2;
            Unity_Add_float(_Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2, _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2);
            float _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2;
            Unity_Subtract_float(1, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2, _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2);
            float4 _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0 = Vector4_15e97cd620f64317ba5a28959ec2138a;
            float4 _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2;
            Unity_Multiply_float((_Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2.xxxx), _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2);
            float4 _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
            Unity_Add_float4(_Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2, _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2);
            Out_0 = _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0 = UnityBuildTexture2DStructNoScale(_L0_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_10872350cc9241f3b46806a4018bf0f0_Out_0 = _L0_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_4caaaadf2c70490fa860c5606041738b_R_1 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[0];
            float _Split_4caaaadf2c70490fa860c5606041738b_G_2 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[1];
            float _Split_4caaaadf2c70490fa860c5606041738b_B_3 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[2];
            float _Split_4caaaadf2c70490fa860c5606041738b_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_20037ac60e0044b381c9f33069964b02_Out_0 = _L0_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0 = _L0_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnitySamplerState _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0 = UnityBuildSamplerStateStruct(SamplerState_Linear_Repeat);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0, _Split_4caaaadf2c70490fa860c5606041738b_R_1, _Split_4caaaadf2c70490fa860c5606041738b_G_2, _Split_4caaaadf2c70490fa860c5606041738b_B_3, _Property_20037ac60e0044b381c9f33069964b02_Out_0, _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_0ca12103ad5649548ee937693f1d142b_Out_0 = UnityBuildTexture2DStructNoScale(_L1_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_83bbb6e7270144b2a80b93491a289205_Out_0 = _L1_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[0];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[1];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[2];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0 = _L1_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0 = _L1_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_0ca12103ad5649548ee937693f1d142b_Out_0, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3, _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0, _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_a2c7714b15e84e09a63d503ff3404e42_Out_0 = UnityBuildTexture2DStructNoScale(_L2_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e348f547036b4b7d8a2e445d89f95329_Out_0 = _L2_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_1b6e2921f9c94dca833eef8aa999721a_R_1 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[0];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_G_2 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[1];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_B_3 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[2];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_065199bb3c2742cc9ce32af94147ceda_Out_0 = _L2_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0 = _L2_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_a2c7714b15e84e09a63d503ff3404e42_Out_0, _Split_1b6e2921f9c94dca833eef8aa999721a_R_1, _Split_1b6e2921f9c94dca833eef8aa999721a_G_2, _Split_1b6e2921f9c94dca833eef8aa999721a_B_3, _Property_065199bb3c2742cc9ce32af94147ceda_Out_0, _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #else
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0 = UnityBuildTexture2DStructNoScale(_L3_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0 = _L3_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_76aee813701645668c93c265ea8adabb_R_1 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[0];
            float _Split_76aee813701645668c93c265ea8adabb_G_2 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[1];
            float _Split_76aee813701645668c93c265ea8adabb_B_3 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[2];
            float _Split_76aee813701645668c93c265ea8adabb_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_ea91a4facf7844ddbb966160d75086d5_Out_0 = _L3_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0 = _L3_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0, _Split_76aee813701645668c93c265ea8adabb_R_1, _Split_76aee813701645668c93c265ea8adabb_G_2, _Split_76aee813701645668c93c265ea8adabb_B_3, _Property_ea91a4facf7844ddbb966160d75086d5_Out_0, _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #else
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f;
            float4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _Blends_882ad28b51d341919454eef7d4e2745e_Out_0, _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0, IN.VertexColor, _SplatmapLERP_b032b15907124a54983bf332fd37c71f, _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0);
            #endif
            surface.BaseColor = (_SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0.xyz);
            surface.Emission = float3(0, 0, 0);
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
        #endif


            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 unnormalizedNormalWS = input.normalWS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // use bitangent on the fly like in hdrp
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // IMPORTANT! If we ever support Flip on double sided materials ensure bitangent and tangent are NOT flipped.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float crossSign = (input.tangentWS.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 bitang = crossSign * cross(input.normalWS.xyz, input.tangentWS.xyz);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // to preserve mikktspace compliance we use same scale renormFactor as was used on the normal.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // This is explained in section 2.2 in "surface gradient based bump mapping framework"
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceTangent =           renormFactor*input.tangentWS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceBiTangent =         renormFactor*bitang;
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.uv0 =                         input.texCoord0;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.VertexColor =                 input.color;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
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
            Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            #pragma shader_feature_local _BLENDS_TWO _BLENDS_THREE _BLENDS_FOUR

        #if defined(_BLENDS_TWO)
            #define KEYWORD_PERMUTATION_0
        #elif defined(_BLENDS_THREE)
            #define KEYWORD_PERMUTATION_1
        #else
            #define KEYWORD_PERMUTATION_2
        #endif


            // Defines
        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMALMAP 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define _NORMAL_DROPOFF_WS 1
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_NORMAL
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TANGENT
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define ATTRIBUTES_NEED_COLOR
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_NORMAL_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TANGENT_WS
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_TEXCOORD0
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        #define VARYINGS_NEED_COLOR
        #endif

            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_2D
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 positionOS : POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalOS : NORMAL;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentOS : TANGENT;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color : COLOR;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
            #endif
        };
        struct Varyings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 normalWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 tangentWS;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 texCoord0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 color;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 WorldSpaceBiTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 uv0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 VertexColor;
            #endif
        };
        struct VertexDescriptionInputs
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceNormal;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpaceTangent;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 ObjectSpacePosition;
            #endif
        };
        struct PackedVaryings
        {
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 positionCS : SV_POSITION;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 interp0 : TEXCOORD0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp1 : TEXCOORD1;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp2 : TEXCOORD2;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 interp3 : TEXCOORD3;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
            #endif
        };

            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            output.texCoord0 = input.interp2.xyzw;
            output.color = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        #endif

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _L0_PackedPBR_TexelSize;
        float3 _L0_Scale;
        float4 _L0_HiColor;
        float4 _L0_DarkColor;
        float4 _L1_PackedPBR_TexelSize;
        float3 _L1_Scale;
        float4 _L1_HiColor;
        float4 _L1_DarkColor;
        float4 _L2_PackedPBR_TexelSize;
        float3 _L2_Scale;
        float4 _L2_HiColor;
        float4 _L2_DarkColor;
        float4 _L3_PackedPBR_TexelSize;
        float3 _L3_Scale;
        float4 _L3_HiColor;
        float4 _L3_DarkColor;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_L0_PackedPBR);
        SAMPLER(sampler_L0_PackedPBR);
        TEXTURE2D(_L1_PackedPBR);
        SAMPLER(sampler_L1_PackedPBR);
        TEXTURE2D(_L2_PackedPBR);
        SAMPLER(sampler_L2_PackedPBR);
        TEXTURE2D(_L3_PackedPBR);
        SAMPLER(sampler_L3_PackedPBR);

            // Graph Functions
            
        void Unity_Multiply_float(float A, float B, out float Out)
        {
            Out = A * B;
        }

        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }

        void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
        {
            Out = lerp(A, B, T);
        }

        void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
        {
            Out = A * B;
        }

        void Unity_Remap_float2(float2 In, float2 InMinMax, float2 OutMinMax, out float2 Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        void Unity_Add_float3(float3 A, float3 B, out float3 Out)
        {
            Out = A + B;
        }

        struct Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6
        {
        };

        void SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(float2 Vector2_3A634BA2, float3 Vector3_5F009C38, float3 Vector3_C20A1E88, Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 IN, out float3 Surfgrad_1)
        {
            float2 _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0 = Vector2_3A634BA2;
            float _Split_fa955e5150c59085814bb861db67e7bb_R_1 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[0];
            float _Split_fa955e5150c59085814bb861db67e7bb_G_2 = _Property_46d3f7b429b1b18494886f68f5a9d196_Out_0[1];
            float _Split_fa955e5150c59085814bb861db67e7bb_B_3 = 0;
            float _Split_fa955e5150c59085814bb861db67e7bb_A_4 = 0;
            float3 _Property_d45f3727ade82d83841b281672c628a9_Out_0 = Vector3_5F009C38;
            float3 _Multiply_139befd1872fc284ae04fd888147402b_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_R_1.xxx), _Property_d45f3727ade82d83841b281672c628a9_Out_0, _Multiply_139befd1872fc284ae04fd888147402b_Out_2);
            float3 _Property_05e48436b398cf81a0bbecc045476954_Out_0 = Vector3_C20A1E88;
            float3 _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2;
            Unity_Multiply_float((_Split_fa955e5150c59085814bb861db67e7bb_G_2.xxx), _Property_05e48436b398cf81a0bbecc045476954_Out_0, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2);
            float3 _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
            Unity_Add_float3(_Multiply_139befd1872fc284ae04fd888147402b_Out_2, _Multiply_6f25fbecca673583b98af5d8e918f144_Out_2, _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2);
            Surfgrad_1 = _Add_f07b6f1dc14246858e7dae2b64725f29_Out_2;
        }

        struct Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3
        {
            float3 WorldSpaceTangent;
            float3 WorldSpaceBiTangent;
            half4 uv0;
        };

        void SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(UnityTexture2D Texture2D_837487C9, float Vector1_165383D, float Vector1_F84F2839, float Vector1_5E2B75FF, float4 Vector4_969C6B4B, float4 Vector4_EC08F10A, UnitySamplerState SamplerState_3228d7ed10833a89a68419b45957d95d, Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 IN, out float4 AlbedoRGBSmoothA_2, out float3 SurfaceGradient_4)
        {
            float4 _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0 = Vector4_969C6B4B;
            float _Split_8f40059a26aad58ea7fc15b48377ff81_R_1 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[0];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_G_2 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[1];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_B_3 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[2];
            float _Split_8f40059a26aad58ea7fc15b48377ff81_A_4 = _Property_5fe4cfd4916bb3899b7a7ff937196ecc_Out_0[3];
            float3 _Vector3_83106cbf4654388ab2e1e131995f0304_Out_0 = float3(_Split_8f40059a26aad58ea7fc15b48377ff81_R_1, _Split_8f40059a26aad58ea7fc15b48377ff81_G_2, _Split_8f40059a26aad58ea7fc15b48377ff81_B_3);
            float4 _Property_946018a8e481d286bba9b6f9f04be770_Out_0 = Vector4_EC08F10A;
            float _Split_bd15d0620edc7d81aa0b6858188998d0_R_1 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[0];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_G_2 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[1];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_B_3 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[2];
            float _Split_bd15d0620edc7d81aa0b6858188998d0_A_4 = _Property_946018a8e481d286bba9b6f9f04be770_Out_0[3];
            float3 _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0 = float3(_Split_bd15d0620edc7d81aa0b6858188998d0_R_1, _Split_bd15d0620edc7d81aa0b6858188998d0_G_2, _Split_bd15d0620edc7d81aa0b6858188998d0_B_3);
            float _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2;
            Unity_Multiply_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Split_bd15d0620edc7d81aa0b6858188998d0_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2);
            float _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2;
            Unity_Subtract_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Multiply_8eaf2ca065b4f081969d5050a8713b17_Out_2, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2);
            UnityTexture2D _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0 = Texture2D_837487C9;
            float4 _UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0 = IN.uv0;
            float _Property_bf6c674956c02182918786a76c12072d_Out_0 = Vector1_165383D;
            float4 _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2;
            Unity_Multiply_float(_UV_c327022ac21b268a91dcc1b7aae16ccc_Out_0, (_Property_bf6c674956c02182918786a76c12072d_Out_0.xxxx), _Multiply_eddf09568efe2688bcb001ed575adb65_Out_2);
            float4 _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0 = SAMPLE_TEXTURE2D(_Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.tex, _Property_39e94bcab7a0fc8990d10bd5e9271712_Out_0.samplerstate, (_Multiply_eddf09568efe2688bcb001ed575adb65_Out_2.xy));
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.r;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.g;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.b;
            float _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_A_7 = _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_RGBA_0.a;
            float _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3;
            Unity_Smoothstep_float(_Split_8f40059a26aad58ea7fc15b48377ff81_A_4, _Subtract_d3634c488cb8b285bf2085b04dd0799e_Out_2, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6, _Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3);
            float3 _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3;
            Unity_Lerp_float3(_Vector3_83106cbf4654388ab2e1e131995f0304_Out_0, _Vector3_aa8b22d2d8fc128b93b93adfb8c56d26_Out_0, (_Smoothstep_a8b271b22f205e8dae64f471d89254b4_Out_3.xxx), _Lerp_e5bf62a1b39520899c9266e476b29622_Out_3);
            float3 _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2;
            Unity_Multiply_float(_Lerp_e5bf62a1b39520899c9266e476b29622_Out_3, (_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_B_6.xxx), _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2);
            float _Split_998ad15b2f43cc858515aa49288c551e_R_1 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[0];
            float _Split_998ad15b2f43cc858515aa49288c551e_G_2 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[1];
            float _Split_998ad15b2f43cc858515aa49288c551e_B_3 = _Multiply_05cac8c3d83aa38797d1963c6f3c8c05_Out_2[2];
            float _Split_998ad15b2f43cc858515aa49288c551e_A_4 = 0;
            float _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0 = Vector1_5E2B75FF;
            float4 _Vector4_389e7e34d055228294ed777d6eb09074_Out_0 = float4(_Split_998ad15b2f43cc858515aa49288c551e_R_1, _Split_998ad15b2f43cc858515aa49288c551e_G_2, _Split_998ad15b2f43cc858515aa49288c551e_B_3, _Property_11e7665a7b4d401381533ef423e5ddcf_Out_0);
            float2 _Vector2_073d67308379728ba264495744b3ffd0_Out_0 = float2(_SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_R_4, _SampleTexture2D_957e16af6195ab88ab267ae4d0ae4b27_G_5);
            float2 _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3;
            Unity_Remap_float2(_Vector2_073d67308379728ba264495744b3ffd0_Out_0, float2 (0, 1), float2 (-1, 1), _Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3);
            Bindings_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9;
            float3 _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1;
            SG_SurfgradTBN_6cbc6665dfce04544bab1c6e181170d6(_Remap_a53c1cbab949ba81a57129f1c1fd152a_Out_3, IN.WorldSpaceTangent, IN.WorldSpaceBiTangent, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9, _SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1);
            float _Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0 = Vector1_F84F2839;
            float3 _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
            Unity_Multiply_float(_SurfgradTBN_ae0b384910e9448ba6688bdf79c7bea9_Surfgrad_1, (_Property_6120cea2a2e0f08d8c619d59ef41e601_Out_0.xxx), _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2);
            AlbedoRGBSmoothA_2 = _Vector4_389e7e34d055228294ed777d6eb09074_Out_0;
            SurfaceGradient_4 = _Multiply_608fb2164ee0118baef1c5cd37bf91fa_Out_2;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }

        struct Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4
        {
        };

        void SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(float4 Vector4_15e97cd620f64317ba5a28959ec2138a, float4 Vector4_cf83c91754ee45cf8642ff61470d2b5b, float4 Vector4_98522ade6b93428c8a4f3dc80b4d0143, float4 Vector4_2e484743ab8346889462fc834ed042d6, float4 Vector4_b10432de33d7494bad6b3bdea8b9a800, Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 IN, out float4 Out_0)
        {
            float4 _Property_f82d995fc1274519b5084f295092fc9e_Out_0 = Vector4_b10432de33d7494bad6b3bdea8b9a800;
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_R_1 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[0];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[1];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[2];
            float _Split_f602e73ebdeb444f8f9a6704ece426e8_A_4 = _Property_f82d995fc1274519b5084f295092fc9e_Out_0[3];
            float4 _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0 = Vector4_cf83c91754ee45cf8642ff61470d2b5b;
            float4 _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1.xxxx), _Property_f59116e1c6444a9c90712585f7ca1c71_Out_0, _Multiply_713e3c6b7440441cb64592afea430ce1_Out_2);
            float4 _Property_a2230e2c15b4464894f7213c9c26912e_Out_0 = Vector4_98522ade6b93428c8a4f3dc80b4d0143;
            float4 _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_G_2.xxxx), _Property_a2230e2c15b4464894f7213c9c26912e_Out_0, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2);
            float4 _Add_c0bd96a255484d0ab09251404523f169_Out_2;
            Unity_Add_float4(_Multiply_713e3c6b7440441cb64592afea430ce1_Out_2, _Multiply_19525ef16d4d4d3fbbb50ce6db69d155_Out_2, _Add_c0bd96a255484d0ab09251404523f169_Out_2);
            float4 _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0 = Vector4_2e484743ab8346889462fc834ed042d6;
            float4 _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2;
            Unity_Multiply_float((_Split_f602e73ebdeb444f8f9a6704ece426e8_B_3.xxxx), _Property_0bf4345ad4d546b0a7829de1b8013d8a_Out_0, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2);
            float4 _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2;
            Unity_Add_float4(_Add_c0bd96a255484d0ab09251404523f169_Out_2, _Multiply_bd04a077a4884527aff60287cb00cc7c_Out_2, _Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2);
            float _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2;
            Unity_Add_float(_Split_f602e73ebdeb444f8f9a6704ece426e8_R_1, _Split_f602e73ebdeb444f8f9a6704ece426e8_G_2, _Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2);
            float _Add_c463d10242e0473ca6267b3b7449ad38_Out_2;
            Unity_Add_float(_Add_58c0533714b84dbe8b7a2df25ff405e2_Out_2, _Split_f602e73ebdeb444f8f9a6704ece426e8_B_3, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2);
            float _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2;
            Unity_Subtract_float(1, _Add_c463d10242e0473ca6267b3b7449ad38_Out_2, _Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2);
            float4 _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0 = Vector4_15e97cd620f64317ba5a28959ec2138a;
            float4 _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2;
            Unity_Multiply_float((_Subtract_b7a578a9f5c6460aba2ce75513a9ad42_Out_2.xxxx), _Property_04f02cb0b4d64f1aaabb1d31e03a4b7a_Out_0, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2);
            float4 _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
            Unity_Add_float4(_Add_4a720664c2aa4b14bb24b3074b75ff80_Out_2, _Multiply_8d9b7bc8fbf74a9ab165add2e8a44db4_Out_2, _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2);
            Out_0 = _Add_e7c7073eaec54991a43dd4e7b242dbb9_Out_2;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0 = UnityBuildTexture2DStructNoScale(_L0_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_10872350cc9241f3b46806a4018bf0f0_Out_0 = _L0_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_4caaaadf2c70490fa860c5606041738b_R_1 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[0];
            float _Split_4caaaadf2c70490fa860c5606041738b_G_2 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[1];
            float _Split_4caaaadf2c70490fa860c5606041738b_B_3 = _Property_10872350cc9241f3b46806a4018bf0f0_Out_0[2];
            float _Split_4caaaadf2c70490fa860c5606041738b_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_20037ac60e0044b381c9f33069964b02_Out_0 = _L0_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0 = _L0_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnitySamplerState _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0 = UnityBuildSamplerStateStruct(SamplerState_Linear_Repeat);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_01d9fc96c06c44cdb39ce1d02bf63946_Out_0, _Split_4caaaadf2c70490fa860c5606041738b_R_1, _Split_4caaaadf2c70490fa860c5606041738b_G_2, _Split_4caaaadf2c70490fa860c5606041738b_B_3, _Property_20037ac60e0044b381c9f33069964b02_Out_0, _Property_8bf52e2758ec415b8783d4e1297a26b5_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_0ca12103ad5649548ee937693f1d142b_Out_0 = UnityBuildTexture2DStructNoScale(_L1_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_83bbb6e7270144b2a80b93491a289205_Out_0 = _L1_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[0];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[1];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3 = _Property_83bbb6e7270144b2a80b93491a289205_Out_0[2];
            float _Split_49819b3f1b0842e2ac1ef7aa6e759a01_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0 = _L1_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0 = _L1_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_0ca12103ad5649548ee937693f1d142b_Out_0, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_R_1, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_G_2, _Split_49819b3f1b0842e2ac1ef7aa6e759a01_B_3, _Property_c6720d5ed84b40009ca0a05e9898261c_Out_0, _Property_dcc45003f7ae442cb85f5204e3aed6b3_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_a2c7714b15e84e09a63d503ff3404e42_Out_0 = UnityBuildTexture2DStructNoScale(_L2_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e348f547036b4b7d8a2e445d89f95329_Out_0 = _L2_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_1b6e2921f9c94dca833eef8aa999721a_R_1 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[0];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_G_2 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[1];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_B_3 = _Property_e348f547036b4b7d8a2e445d89f95329_Out_0[2];
            float _Split_1b6e2921f9c94dca833eef8aa999721a_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_065199bb3c2742cc9ce32af94147ceda_Out_0 = _L2_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0 = _L2_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_a2c7714b15e84e09a63d503ff3404e42_Out_0, _Split_1b6e2921f9c94dca833eef8aa999721a_R_1, _Split_1b6e2921f9c94dca833eef8aa999721a_G_2, _Split_1b6e2921f9c94dca833eef8aa999721a_B_3, _Property_065199bb3c2742cc9ce32af94147ceda_Out_0, _Property_35559b3247644a7d8d2abbb4b2439f4b_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #else
            float4 _Blends_882ad28b51d341919454eef7d4e2745e_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_42867078272f433b89ba4f270f149d7b_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            UnityTexture2D _Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0 = UnityBuildTexture2DStructNoScale(_L3_PackedPBR);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float3 _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0 = _L3_Scale;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float _Split_76aee813701645668c93c265ea8adabb_R_1 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[0];
            float _Split_76aee813701645668c93c265ea8adabb_G_2 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[1];
            float _Split_76aee813701645668c93c265ea8adabb_B_3 = _Property_e395d9310ba54ab8b6fcf5d782f8081a_Out_0[2];
            float _Split_76aee813701645668c93c265ea8adabb_A_4 = 0;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_ea91a4facf7844ddbb966160d75086d5_Out_0 = _L3_HiColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            float4 _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0 = _L3_DarkColor;
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceTangent = IN.WorldSpaceTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.WorldSpaceBiTangent = IN.WorldSpaceBiTangent;
            _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982.uv0 = IN.uv0;
            float4 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            float3 _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4;
            SG_CompactSingleMapPBRUnPackSplatmapSubGraph_0e9a784794d430845b7657abfc69f7a3(_Property_ebf937e2d3aa4125bf97eba73da026c6_Out_0, _Split_76aee813701645668c93c265ea8adabb_R_1, _Split_76aee813701645668c93c265ea8adabb_G_2, _Split_76aee813701645668c93c265ea8adabb_B_3, _Property_ea91a4facf7844ddbb966160d75086d5_Out_0, _Property_a42b07fe99d0478889ba1299d12dcae6_Out_0, _Property_f5cb0c23eb3c4c29b953752a10e7b737_Out_0, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_SurfaceGradient_4);
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            #if defined(_BLENDS_TWO)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #elif defined(_BLENDS_THREE)
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = float4(0, 0, 0, 0);
            #else
            float4 _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0 = _CompactSingleMapPBRUnPackSplatmapSubGraph_7d140dcee02d40fa89542301b5a1f982_AlbedoRGBSmoothA_2;
            #endif
            #endif
            #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
            Bindings_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f;
            float4 _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0;
            SG_SplatmapLERP_3b2505ddf7604f6408a95340456c52f4(_CompactSingleMapPBRUnPackSplatmapSubGraph_5270afa1936f401fb1741958bf95f395_AlbedoRGBSmoothA_2, _CompactSingleMapPBRUnPackSplatmapSubGraph_6fef469c08d845be9f8f0ef64501d32c_AlbedoRGBSmoothA_2, _Blends_882ad28b51d341919454eef7d4e2745e_Out_0, _Blends_ed90a0909e424e648cb63610d142fdcd_Out_0, IN.VertexColor, _SplatmapLERP_b032b15907124a54983bf332fd37c71f, _SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0);
            #endif
            surface.BaseColor = (_SplatmapLERP_b032b15907124a54983bf332fd37c71f_Out_0.xyz);
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceNormal =           input.normalOS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpaceTangent =          input.tangentOS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.ObjectSpacePosition =         input.positionOS;
        #endif


            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 unnormalizedNormalWS = input.normalWS;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        const float renormFactor = 1.0 / length(unnormalizedNormalWS);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // use bitangent on the fly like in hdrp
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // IMPORTANT! If we ever support Flip on double sided materials ensure bitangent and tangent are NOT flipped.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float crossSign = (input.tangentWS.w > 0.0 ? 1.0 : -1.0) * GetOddNegativeScale();
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        float3 bitang = crossSign * cross(input.normalWS.xyz, input.tangentWS.xyz);
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceNormal =            renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // to preserve mikktspace compliance we use same scale renormFactor as was used on the normal.
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        // This is explained in section 2.2 in "surface gradient based bump mapping framework"
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceTangent =           renormFactor*input.tangentWS.xyz;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.WorldSpaceBiTangent =         renormFactor*bitang;
        #endif


        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.uv0 =                         input.texCoord0;
        #endif

        #if defined(KEYWORD_PERMUTATION_0) || defined(KEYWORD_PERMUTATION_1) || defined(KEYWORD_PERMUTATION_2)
        output.VertexColor =                 input.color;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"

            ENDHLSL
        }
    }
    CustomEditor "ShaderGraph.PBRMasterGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}