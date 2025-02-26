#ifndef CUSTOMBAKEDGI_INCLUDED
#define CUSTOMBAKEDGI_INCLUDED

void SampleProbeVolume_float(float3 worldPos, float4x4 probeWorldToTexture, float3 probeVolumeMin, float3 probeVolumeSizeInv, SamplerState samplerIn)
{
	#if defined(_PROBEVOLUME_ON)
	
		// Transform the coordinates to sample from the probes cubic volume - could be done in Vertex for better perf
		float3 position = mul(probeWorldToTexture, float4(worldPos, 1.0f)).xyz;
		float3 texCoord = (position - probeVolumeMin.xyz) * probeVolumeSizeInv;
		
		// Used by SHEvalLinearL0L1 for the L0L1 spherical harmonics for diffuse probe color
		unity_SHAr = SAMPLE_TEXTURE3D(_ProbeVolumeShR, samplerIn, texCoord);
		unity_SHAg = SAMPLE_TEXTURE3D(_ProbeVolumeShG, samplerIn, texCoord);
		unity_SHAb = SAMPLE_TEXTURE3D(_ProbeVolumeShB, samplerIn, texCoord);
		
		// Shadowmask
		unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, samplerIn, texCoord);
		
		// Zero'd as we don't use SHEvalLinearL2
		unity_SHBr = 0;
		unity_SHBg = 0;
		unity_SHBb = 0;
		unity_SHC = 0;
	#endif
}

void SampleProbeVolume_float(float3 texCoord, SamplerState samplerIn, out float3 passThrough)
{
	#if defined(_PROBEVOLUME_ON)			
		// Used by SHEvalLinearL0L1 for the L0L1 spherical harmonics for diffuse probe color
		unity_SHAr = SAMPLE_TEXTURE3D(_ProbeVolumeShR, samplerIn, texCoord);
		unity_SHAg = SAMPLE_TEXTURE3D(_ProbeVolumeShG, samplerIn, texCoord);
		unity_SHAb = SAMPLE_TEXTURE3D(_ProbeVolumeShB, samplerIn, texCoord);
		
		// Shadowmask
		unity_ProbesOcclusion = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, samplerIn, texCoord);
		
		// Zero'd as we don't use SHEvalLinearL2
		unity_SHBr = 0;
		unity_SHBg = 0;
		unity_SHBb = 0;
		unity_SHC = 0;
	#endif
	
	passThrough = float3(0, 0, 0);
}

// \Library\PackageCache\com.unity.render-pipelines.core@12.1.15\ShaderLibrary\EntityLighting.hlsl
real3 SHEvalLinearL0L1C(real3 N, real4 shAr, real4 shAg, real4 shAb)
{
    real4 vA = real4(N, 1.0);

    real3 x1;
    // Linear (L1) + constant (L0) polynomial terms
    x1.r = dot(shAr, vA);
    x1.g = dot(shAg, vA);
    x1.b = dot(shAb, vA);

    return x1;
}

float3 SampleProbeVolumeCustom_float(float3 texCoord, float3 worldNormal, SamplerState samplerIn, Texture3D _ProbeVolumeShR, Texture3D _ProbeVolumeShG, Texture3D _ProbeVolumeShB, float mip)
{
	//float4 ProbeVolumeShR = SAMPLE_TEXTURE3D(_ProbeVolumeShR, samplerIn, texCoord);
	//float4 ProbeVolumeShG = SAMPLE_TEXTURE3D(_ProbeVolumeShG, samplerIn, texCoord);
	//float4 ProbeVolumeShB = SAMPLE_TEXTURE3D(_ProbeVolumeShB, samplerIn, texCoord);
	
	float4 ProbeVolumeShR = SAMPLE_TEXTURE3D_LOD(_ProbeVolumeShR,samplerIn,texCoord,mip);
	float4 ProbeVolumeShG = SAMPLE_TEXTURE3D_LOD(_ProbeVolumeShG,samplerIn,texCoord,mip);
	float4 ProbeVolumeShB = SAMPLE_TEXTURE3D_LOD(_ProbeVolumeShB,samplerIn,texCoord,mip);
	
	//float4 ProbeVolumeShR = tex3Dlod(_ProbeVolumeShR, float4(texCoord,0));
	//float4 ProbeVolumeShG = tex3Dlod(_ProbeVolumeShG, float4(texCoord,0));
	//float4 ProbeVolumeShB = tex3Dlod(_ProbeVolumeShB, float4(texCoord,0));
	
	//'tex2Dlod': no matching 3 parameter intrinsic function; Possible intrinsic functions are: tex2Dlod(sampler2D, float4|half4|min10float4|min16float4)

	return SHEvalLinearL0L1C(worldNormal, ProbeVolumeShR, ProbeVolumeShG,  ProbeVolumeShB);
	
	// Shadowmask
	//ShadowMask = SAMPLE_TEXTURE3D(_ProbeVolumeOcc, samplerIn, texCoord);
}

void ProbeTexCoord_float(float3 worldPos, float4x4 probeWorldToTexture, float3 probeVolumeMin, float3 probeVolumeSizeInv, out float3 texCoord){
	float3 position = mul(probeWorldToTexture, float4(worldPos, 1.0f)).xyz;
	texCoord = (position - probeVolumeMin.xyz) * probeVolumeSizeInv;
}

// For backward compat, normalWS not used
//void SampleProbeVolume_float(float3 worldPos, float3 normalWS, float4x4 probeWorldToTexture, float3 probeVolumeMin, float3 probeVolumeSizeInv, SamplerState samplerIn, out float3 passThrough){
//	SampleProbeVolume_float(worldPos, probeWorldToTexture, probeVolumeMin, probeVolumeSizeInv, samplerIn);
//	passThrough = float3(0, 0, 0);
//}

void SampleProbeVolume_float(float3 worldPos, float4x4 probeWorldToTexture, float3 probeVolumeMin, float3 probeVolumeSizeInv, SamplerState samplerIn, out float3 passThrough)
{
	SampleProbeVolume_float(worldPos, probeWorldToTexture, probeVolumeMin, probeVolumeSizeInv, samplerIn);
	passThrough = float3(0, 0, 0);
}



#endif