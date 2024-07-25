
// Stub removing all Stochastic functionality
half4 StochasticSample2DWeightsR(Texture2D tex, SamplerState samplerTex, float2 uv, out half3 cw, out float2 uv1, out float2 uv2, out float2 uv3, out float2 dx, out float2 dy, float scale, float contrast)
{
	cw = 0;
	uv1 = 0;
	uv2 = 0;
	uv3 = 0;
	dx = 0;
	dy = 0;
	
	return SAMPLE_TEXTURE2D( tex, samplerTex, uv );
}
