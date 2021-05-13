void RevealMaskNormalCrossFilter_float(float2 uv, float height, float texWidth, float texHeight, out float3 Out)
{
	float2 texelSize = float2(1.0 / texWidth, 1.0 / texHeight);
	float4 h;
	h[0] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0, -1) * texelSize).b) * height;
	h[1] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(-1, 0) * texelSize).b) * height;
	h[2] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(1, 0) * texelSize).b) * height;
	h[3] = saturate(SAMPLE_TEXTURE2D(_RevealMask, sampler_RevealMask, uv + float2(0, 1) * texelSize).b) * height;

	float3 n;
	n.z = 2;
	n.x = h[1] - h[2];
	n.y = h[0] - h[3];

	Out = normalize(n);
}