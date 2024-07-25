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