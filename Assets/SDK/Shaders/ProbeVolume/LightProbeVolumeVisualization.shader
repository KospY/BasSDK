Shader "Unlit/LightProbeVolumeVisualization"
{
    Properties
    {
        [NoScaleOffset]_ProbeVolumeShR("Probe Volume SH Red", 3D) = "white" {}
        [NoScaleOffset]_ProbeVolumeShG("Probe Volume SH Green", 3D) = "white" {}
        [NoScaleOffset]_ProbeVolumeShB("Probe Volume SH Blue", 3D) = "white" {}
        _ProbeVolumeMin("Probe Volume Min", Vector) = (0, 0, 0, 0)
        _ProbeVolumeSizeInv("Probe Volume Size Inverse", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                half3 normal: TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                half3 normal : TEXCOORD1;
            };

            TEXTURE3D(_ProbeVolumeShR);
            SAMPLER(sampler_ProbeVolumeShR);
            TEXTURE3D(_ProbeVolumeShG);
            SAMPLER(sampler_ProbeVolumeShG);
            TEXTURE3D(_ProbeVolumeShB);
            SAMPLER(sampler_ProbeVolumeShB);

            float4x4 _ProbeWorldToTexture;
            float3 _ProbeVolumeMin;
            float3 _ProbeVolumeSizeInv;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.worldPos = TransformObjectToWorld(v.vertex.xyz);
                o.normal = v.normal;// TransformObjectToWorldNormal(v.normal);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 position = mul(_ProbeWorldToTexture, float4(i.worldPos, 1.0f)).xyz;
                float3 texCoord = (position - _ProbeVolumeMin.xyz) * _ProbeVolumeSizeInv;

                half4 SHAr = SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord);
                half4 SHAg = SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShG, texCoord);
                half4 SHAb = SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShB, texCoord);

                //SH Linear L0 L1
                float4 ambient;
                half4 normal = half4(i.normal, 1.0);
                ambient.r = dot(SHAr, normal);
                ambient.g = dot(SHAg, normal);
                ambient.b = dot(SHAb, normal);
                ambient.a = 1;

                return ambient;
            }
            ENDHLSL
        }
    }
}
