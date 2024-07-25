Shader "Hidden/RainyReignGames/ProbeVolumeVisualization"
{
    Properties
    {
        [Enum(Off,0,Front,1,Back,2)] _CullMode("Culling Mode", Float) = 2

        [NoScaleOffset] _ProbeVolumeShR("Probe Volume SH Red", 3D) = "black" {}
        [NoScaleOffset]_ProbeVolumeShG("Probe Volume SH Green", 3D) = "black" {}
        [NoScaleOffset]_ProbeVolumeShB("Probe Volume SH Blue", 3D) = "black" {}
        [NoScaleOffset]_ProbeVolumeOcc("Probe Volume Occlusion", 3D) = "black" {}

        _ProbeVolumeMin("Probe Volume Min", Vector) = (0,0,0)
        _ProbeVolumeSizeInv("Probe Volume Size Inverse", Vector) = (0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Cull[_CullMode]

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Includes
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : TEXCOORD1;
                float2 texCoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 texCoord : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            float4x4 _ProbeWorldToTexture;
            float3 _ProbeVolumeMin;
            float3 _ProbeVolumeSizeInv;

            TEXTURE3D(_ProbeVolumeShR);
            TEXTURE3D(_ProbeVolumeShG);
            TEXTURE3D(_ProbeVolumeShB);
            TEXTURE3D(_ProbeVolumeOcc);
            SAMPLER(sampler_ProbeVolumeShR);

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.worldNormal = TransformObjectToWorldNormal(v.normal);

                float3 position = mul(_ProbeWorldToTexture, float4(TransformObjectToWorld(v.vertex.xyz), 1.0f)).xyz;
                o.texCoord = (position - _ProbeVolumeMin.xyz) * _ProbeVolumeSizeInv;//position in space relative to the volume.

                return o;
            }

            real3 frag (v2f i) : SV_Target
            {
                return SHEvalLinearL0L1(i.worldNormal, SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, i.texCoord),
                                            SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShR, i.texCoord), SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShR, i.texCoord));
            }
            ENDHLSL
        }
    }
}
