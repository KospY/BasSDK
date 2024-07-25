Shader "Unlit/ProbeVolumeExample-UnLit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        [Toggle(_PROBEVOLUME_ON)] _UseProbeVolume("Use Probe Volume", Float) = 0

        // You could prefix with '[HideInInspector]' to hide them from inspector to keep things tidy
        [NoScaleOffset] _ProbeVolumeShR("ProbeVolumeShR", 3D) = "black" {}
        [NoScaleOffset] _ProbeVolumeShG("ProbeVolumeShG", 3D) = "black" {}
        [NoScaleOffset] _ProbeVolumeShB("ProbeVolumeShB", 3D) = "black" {}
        [NoScaleOffset] _ProbeVolumeOcc("ProbeVolumeOcc", 3D) = "black" {}

        _ProbeVolumeMin("ProbeVolumeMin", Vector) = (0,0,0,0)
        _ProbeVolumeSizeInv("ProbeVolumeSizeInv", Vector) = (0,0,0,0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM            
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma multi_compile_local __ _PROBEVOLUME_ON

            // Includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl" // Needed for SHEvalLinearL0L1
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

            struct Attributes // appdata
            {
                float3 positionOS : POSITION; // vertex pos
                float3 normalOS : NORMAL;
 
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings // v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1; // positionWS
                float3 normalWS : TEXCOORD2; // worldNormal

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
        
            CBUFFER_START(UnityPerMaterial)
            float4x4 _ProbeWorldToTexture;
            float3 _ProbeVolumeMin;
            float3 _ProbeVolumeSizeInv;
            CBUFFER_END

            TEXTURE3D(_ProbeVolumeShR);
            TEXTURE3D(_ProbeVolumeShG);
            TEXTURE3D(_ProbeVolumeShB);
            TEXTURE3D(_ProbeVolumeOcc);

            SAMPLER(sampler_Linear_Clamp); // SamplerState set to linear clamped to avoid repeating past volume boundries
        
            #include "Assets\SDK\Shaders\ProbeVolume\CustomSampleGI.hlsl" // Include after the TEXTURE3D items are defined above
        
            sampler2D _MainTex;
            float4 _MainTex_ST;

            // vertex shader
            Varyings vert (Attributes input)
            {
                Varyings output = (Varyings)0;
    
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.vertex = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);        
                output.worldPos = TransformObjectToWorld( input.positionOS );
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);

                return output;
            }

            // fragment shader
            float4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float4 col = tex2D(_MainTex, input.uv);

                SampleProbeVolume_float(input.worldPos, _ProbeWorldToTexture, _ProbeVolumeMin, _ProbeVolumeSizeInv, sampler_Linear_Clamp);

                // Perform lighting/PBR here eg: UniversalFragmentPBR(inputData, surfaceData)
                
                // Or for UnLit, you can sample the spherical harmonics:
                col.rgb *= SHEvalLinearL0L1( input.normalWS, unity_SHAr, unity_SHAg, unity_SHAb);
                
                // If you need ShadowMask it is stored in: unity_ProbesOcclusion
                // So you would assign this to your 'inputData.shadowMask' for UniversalFragmentPBR

                return col;

            }

            ENDHLSL
        }
    }
}

/* JUNK - REMOVE

               //#ifdef _TANGENT_TO_WORLD
                //float4 tangentOS : TANGENT;
                //#endif
                
                output.worldPos =  mul (unity_ObjectToWorld, input.positionOS).xyz;
                //VertexNormalInputs normalInput = GetVertexNormalInputs( input.normalOS, input.tangentOS );
                //output.normalWS = normalInput.normalWS;
                //TransformObjectToWorld( output.vertex.xyz );
                
                
// Includes
//#include "UnityCG.cginc"
//#include "UnityShaderVariables.cginc"

//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
//

//#include "UnityCG.cginc"
//#include "UnityShaderVariables.cginc"

//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
//#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"


                //float4 shr = SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord);
                //float4 shg = SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShR, texCoord);
                //float4 shb = SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShR, texCoord);
                
                //o.DiffuseGI = SHEvalLinearL0L1( d.worldSpaceNormal, shr, shg, shb);
                //unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeShR, texCoord);
                //o.ShadowMask = unity_ProbesOcclusion;

half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.worldPos);

//input.normalWS = NormalizeNormalPerPixel(input.normalWS);

// Insert code to apply baked or probe lighting here / PBR Lighting
InputData inputData = (InputData)0;
inputData.positionWS = input.worldPos;
inputData.viewDirectionWS = viewDirWS;
inputData.normalWS = input.normalWS;
inputData.shadowCoord = float4(0, 0, 0, 0);

//SH / vertexSH
half3 SH = half3(0,0,0);
inputData.bakedGI = SAMPLE_GI(input.lightmapUVOrVertexSH.xy, SH, inputData.normalWS);
inputData.shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);

SurfaceData surfaceData;

surfaceData = (SurfaceData)0; // avoids "not completely initalized" errors
surfaceData.albedo              = float3(0.5,0.5,0.5);
//surfaceData.metallic            = saturate(0);
surfaceData.specular            = 0.8;
surfaceData.smoothness          = saturate(0.8),
//surfaceData.occlusion           = 0,
//surfaceData.emission            = float3(0,0,0),
//surfaceData.alpha               = saturate(1);
//surfaceData.normalTS            = float3(0,0,0);//Normal;
//surfaceData.clearCoatMask       = 0;
//surfaceData.clearCoatSmoothness = 0;

col = UniversalFragmentPBR( inputData, surfaceData);
//col.rgb = input.normalWS;
return col;
*/