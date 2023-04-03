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

    half _VertexColorIntensity;
CBUFFER_END

struct VertexToPixel
{
    float4 pos : SV_POSITION; //used
    float3 worldPos : TEXCOORD0; //used
    float3 worldNormal : TEXCOORD1; //used
    float4 worldTangent : TEXCOORD2; //used
    float4 texcoord0 : TEXCOORD3; //used
    float4 screenPos : TEXCOORD7; //used

    #if defined(LIGHTMAP_ON)
        float2 lightmapUV : TEXCOORD8; //used
    #endif
    #if defined(DYNAMICLIGHTMAP_ON)
        float2 dynamicLightmapUV : TEXCOORD9; //used but I dont think we use dynamic lightmaps? (enlighten)
    #endif
    #if !defined(LIGHTMAP_ON)
        float3 sh : TEXCOORD10; //used maybe?
    #endif

    #if defined(VARYINGS_NEED_FOG_AND_VERTEX_LIGHT)
        float4 fogFactorAndVertexLight : TEXCOORD11; //used
    #endif

    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        float4 shadowCoord : TEXCOORD12; //used
    #endif

    float4 vertexColor : TEXCOORD13; //used
    float4 vertexTexCoord : TEXCOORD16; //used

    //all used
    #if UNITY_ANY_INSTANCING_ENABLED
        uint instanceID : CUSTOM_INSTANCE_ID;
    #endif
    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
    #endif
    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
    #endif
    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
    #endif
};


struct Surface
{
    half3 Albedo; // used
    half Height; //used
    half3 Normal; //used
    half Smoothness; //used
    half3 Emission; //used
    half Metallic; //used
    half Occlusion; //used
    half Alpha; //used
    
    #if defined(_OVERRIDE_BAKEDGI)
        float3 DiffuseGI; //used
        float3 BackDiffuseGI; //not used
        float3 SpecularGI; // used but nothing assigned to it?
    #endif

    float4 ShadowMask; //used
};

struct Blackboard
{
    float2 baseuv; //used

};

struct ShaderData
{
    float4 clipPos; //used
    float3 worldSpacePosition; //used
    float3 worldSpaceNormal; //used
    float3 worldSpaceTangent; //used
    float tangentSign; //used

    float3 worldSpaceViewDir; //used
    float3 tangentSpaceViewDir; //used assigned to but never read

    float4 texcoord0; //used
    float4 texcoord1; //used
    float4 texcoord2; //used - sort of?
    float4 texcoord3; //not used

    float2 screenUV; //used assigned to but never read
    float4 screenPos; //used 

    float4 vertexColor; //used 

    float4 vertexTexCoord; //used 
    
    float3x3 TBNMatrix; //used 
    Blackboard blackboard; //used 
};

struct VertexData
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
    float4 texcoord0 : TEXCOORD0;
    #if _URP && (_USINGTEXCOORD1 || _PASSMETA || _PASSFORWARD || _PASSGBUFFER)
        float4 texcoord1 : TEXCOORD1;
    #endif
    #if _URP && (_USINGTEXCOORD2 || _PASSMETA || ((_PASSFORWARD || _PASSGBUFFER) && defined(DYNAMICLIGHTMAP_ON)))
        float4 texcoord2 : TEXCOORD2;
    #endif
    float4 vertexColor : COLOR;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};



#ifdef unity_WorldToObject
    #undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
    #undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

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

#if defined(_DETAIL) && !defined(SHADER_API_MOBILE)
    TEXTURE2D(_DetailAlbedoMap);
    SAMPLER(sampler_DetailAlbedoMap);
    TEXTURE2D(_DetailNormalMap);
    SAMPLER(sampler_DetailNormalMap);
#endif

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

    const float3 TexelOffsets = float3(0, 1, -1);
    float3 RevealMaskNormalCrossFilter(float2 uv, float height, float2 texelSize)
    {

        float4 h = float4(
            saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, mad(texelSize, TexelOffsets.xz, uv)).b),
            saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, mad(texelSize, TexelOffsets.zx, uv)).b),
            saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, mad(texelSize, TexelOffsets.yx, uv)).b),
            saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, mad(texelSize, TexelOffsets.xy, uv)).b)
        ) * height;

        float3 n = float3(h.y - h.z, h.x - h.w, 2 );

        return normalize(n);
    }

#endif

#if defined(_PROBEVOLUME_ON)
    TEXTURE3D(_ProbeVolumeShR);
    TEXTURE3D(_ProbeVolumeShG);
    TEXTURE3D(_ProbeVolumeShB);
    TEXTURE3D(_ProbeVolumeOcc);
    SAMPLER(sampler_ProbeVolumeShR);
#endif

#if defined(_PASSSHADOW)
    float3 _LightDirection;
    float3 _LightPosition;
#endif

#if defined(_COLORMASK_ON)
    TEXTURE2D(_ColorMask);
    SAMPLER(sampler_ColorMask);
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

#if _URP
    half3 UnpackScaleNormal(half4 packednormal, half scale)
    {
        #ifndef UNITY_NO_DXT5nm

        packednormal.x *= packednormal.w;
        #endif
        half3 normal;
        normal.xy = (packednormal.xy * 2 - 1) * scale;
        normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
        return normal;
    }
#endif


void LODFadeCrossFade(inout Surface o, ShaderData d)
{
    #if LOD_FADE_CROSSFADE && !defined(SHADER_API_MOBILE)
        float4 screenPosNorm = d.screenPos / d.screenPos.w;
        screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? screenPosNorm.z : screenPosNorm.z * 0.5 + 0.5;
        float2 clipScreen = screenPosNorm.xy * _ScreenParams.xy;
        ApplyDitherCrossFadeVSP(clipScreen,unity_LODFade.x);
    #endif
}

void MainSurface(inout Surface o, inout ShaderData d)
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
    o.Emission = mask.b;
    o.Smoothness = mask.a * _Smoothness;


    #if defined(_EMISSION)
        o.Emission *= SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, uv).rgb * _EmissionColor.rgb;
    #else
        o.Emission *= _EmissionColor.rgb;
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

void ColorMask(inout Surface o, inout ShaderData d)
{
    #if defined(_COLORMASK_ON)
        float4 colorMask = SAMPLE_TEXTURE2D(_ColorMask, sampler_ColorMask, d.blackboard.baseuv);
        colorMask.a = (1.0 - colorMask.a);
        float3 c = ((colorMask.r * _Tint0) + (colorMask.g * _Tint1) + (colorMask.b * _Tint2) + (colorMask.a * _Tint3)).rgb * o.Albedo;
        o.Albedo = lerp(o.Albedo, c, dot(colorMask,1));
    #endif
}

void RevealLayers(inout Surface o, inout ShaderData d)
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
        o.Normal = normalize(float3(o.Normal.rg + heightNormal.rg, o.Normal.b * heightNormal.b)); 

        float at = saturate(revealMask.a * layerMask.a);
        o.Emission = ((1.0 - at) * o.Emission.rgb) + (at * _Layer3EmissionColor.rgb);

    #endif
}

void VertexOcclusion(inout VertexData v)
{
    #if defined(_VERTEXOCCLUSION_ON)
        if(_Bitmask & (int)v.texcoord1.x)
        {
            v.vertex *= (0.0 / 0.0);
        }
    #endif
}

void VertexProbeVolume(inout VertexData v, inout float3 d)
{
    #if defined(_PROBEVOLUME_ON)
        float3 position = mul(_ProbeWorldToTexture, float4(TransformObjectToWorld(v.vertex.xyz), 1.0f)).xyz;
        float3 texCoord = (position - _ProbeVolumeMin.xyz) * _ProbeVolumeSizeInv;
        d = texCoord; 
    #endif
}

void ProbeVolume(inout Surface o, inout ShaderData d)
{
    #if defined(_PROBEVOLUME_ON)
               float3 texCoord = d.vertexTexCoord.xyz;
               o.DiffuseGI = SHEvalLinearL0L1( d.worldSpaceNormal, SAMPLE_TEXTURE3D(_ProbeVolumeShR, sampler_ProbeVolumeShR, texCoord),
               SAMPLE_TEXTURE3D(_ProbeVolumeShG, sampler_ProbeVolumeShR, texCoord), SAMPLE_TEXTURE3D(_ProbeVolumeShB, sampler_ProbeVolumeShR, texCoord));
               unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, sampler_ProbeVolumeShR, texCoord);
               o.ShadowMask = unity_ProbesOcclusion;
    #endif
}

void VertexColorIntensity(inout Surface o, inout ShaderData d)
{
    o.Albedo = lerp(o.Albedo, o.Albedo * d.vertexColor.rgb, _VertexColorIntensity);
    o.Occlusion = lerp(o.Occlusion, o.Occlusion * d.vertexColor.a, _VertexColorIntensity);
}

void SurfaceFunction(inout Surface l, inout ShaderData d)
{
    LODFadeCrossFade(l, d);
    MainSurface(l, d);
    ColorMask(l, d);
    RevealLayers(l, d);
    ProbeVolume(l, d);
    VertexColorIntensity(l, d);
}

void VertexFunction(inout VertexData v, inout VertexToPixel v2p)
{
    #if defined(_VERTEXOCCLUSION_ON)
    if(_Bitmask & (int)v.texcoord1.x)
    {
        v.vertex *= (0.0 / 0.0);
    }
    #endif

    v2p.vertexColor = v.vertexColor;
    
    #if defined(_PROBEVOLUME_ON)
        float3 position = mul(_ProbeWorldToTexture, float4(TransformObjectToWorld(v.vertex.xyz), 1.0f)).xyz;
        float3 texCoord = (position - _ProbeVolumeMin.xyz) * _ProbeVolumeSizeInv;
        v2p.vertexTexCoord = float4(texCoord,0); 
    #endif
}


ShaderData CreateShaderData(VertexToPixel i)
{
    ShaderData d = (ShaderData)0;
    d.clipPos = i.pos;
    d.worldSpacePosition = i.worldPos;

    d.worldSpaceNormal = normalize(i.worldNormal);
    d.worldSpaceTangent = normalize(i.worldTangent.xyz);
    d.tangentSign = i.worldTangent.w;
    float3 biTangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;

    d.TBNMatrix = float3x3(d.worldSpaceTangent, biTangent, d.worldSpaceNormal);
    d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

    d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
    d.texcoord0 = i.texcoord0;


    d.screenPos = i.screenPos;
    d.screenUV = (i.screenPos.xy / i.screenPos.w);

    d.vertexColor = i.vertexColor;

    d.vertexTexCoord = i.vertexTexCoord;

    return d;
}

float3 WorldToTangentSpace(ShaderData d, float3 normal)
{
    return mul(d.TBNMatrix, normal);
}

float3 TangentToWorldSpace(ShaderData d, float3 normal)
{
    return mul(normal, d.TBNMatrix);
}

float Dither8x8Bayer(int x, int y)
{
    const float dither[64] = {
        1, 49, 13, 61, 4, 52, 16, 64,
        33, 17, 45, 29, 36, 20, 48, 32,
        9, 57, 5, 53, 12, 60, 8, 56,
        41, 25, 37, 21, 44, 28, 40, 24,
        3, 51, 15, 63, 2, 50, 14, 62,
        35, 19, 47, 31, 34, 18, 46, 30,
        11, 59, 7, 55, 10, 58, 6, 54,
        43, 27, 39, 23, 42, 26, 38, 22
    };
    int r = y * 8 + x;
    return dither[r] / 64;
}

float3 GetCameraWorldPosition()
{
    return _WorldSpaceCameraPos;
}

void ApplyDitherCrossFadeVSP(float2 vpos, float fadeValue)
{
    float dither = Dither8x8Bayer(fmod(vpos.x, 8), fmod(vpos.y, 8));
    float sgn = fadeValue > 0 ? 1.0f : -1.0f;
    clip(dither - (1 - fadeValue) * sgn);
}
