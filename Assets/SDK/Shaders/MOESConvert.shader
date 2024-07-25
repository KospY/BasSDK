Shader "Hidden/MOESConvert"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [NoScaleOffset]_MetallicGlossMap("Metallic", 2D) = "black" {}
        [NoScaleOffset]_OcclusionMap("Occlusion", 2D) = "white" {}
        [NoScaleOffset]_EmissionMap("Emissive", 2D) = "black" {}
        [NoScaleOffset]_SpecGlossMap("Smoothness", 2D) = "black" {}
        [ToggleUI]_InvertSmoothness("Invert Smoothness", Float) = 0
        [ToggleUI]_SmoothnessIsAlpha("Smoothness is Alpha", Float) = 0
        [ToggleUI]_FixColorSpace("Fix color space", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _MetallicGlossMap;
            sampler2D _OcclusionMap;
            sampler2D _EmissionMap;
            sampler2D _SpecGlossMap;
            float _InvertSmoothness;
            float _SmoothnessIsAlpha;
            float _FixColorSpace;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 col = fixed4(tex2D(_MetallicGlossMap, i.uv).r, tex2D(_OcclusionMap, i.uv).g, tex2D(_EmissionMap, i.uv).b, tex2D(_SpecGlossMap, i.uv).r);
                
                // Extract smoothness from Alpha channel (used in Unity PBR shaders)
                if (_SmoothnessIsAlpha > 0) {
                    col.a = tex2D(_SpecGlossMap, i.uv).a;
                }
                
                // Will automatically invert colors if Autodesk shader is detected
                if (_InvertSmoothness > 0)
                {
                    // Fix color space conversion from Autodesk to Unity PBR (Linear to RGB)
                    if (_FixColorSpace > 0) {
                        col.a = (col.a <= 0.0031308) ? col.a * 12.92 : (pow(max(abs(col.a), 1.192092896e-07), 1.0 / 2.4) * 1.055) - 0.055;
                    }
                    
                    // Perform inversion
                    col.a = 1.0 - col.a;
                }

                return col;
            }
            ENDCG
        }
    }
}
