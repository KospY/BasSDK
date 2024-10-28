//#include "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"

// Requires input / global floats:
//#ifdef TONEMAPPING_GEAR
//	float4 _TonemappingSettings;
//	float4 _TonemappingMasterBlend;
//#endif

// Example use:
//#ifdef TONEMAPPING_GEAR
//    color.rgb = ApplyTonemap(color.rgb,Tonemapping);
//#endif

#ifndef TONEMAPPING_GEAR
	#define TONEMAPPING_GEAR
	
	//#define TONEMAPPINGELSEWHERE
	
	#ifndef TONEMAPPING_VARIABLES_INCLUDED
		#define TONEMAPPING_VARIABLES_INCLUDED
		float4 _TonemappingSettings;
		float _TonemappingMasterBlend;
	#endif
	
	//#pragma shader_feature _TONEMAPPING_ON
	//#pragma shader_feature GLOBALTONEMAPPING
	
	#if SHADER_API_MOBILE
		#define GLOBALTONEMAPPING
	#else
		#undef GLOBALTONEMAPPING
	#endif
	
	//#pragma multi_compile __ _TONEMAPPING_ON
	
	// https://github.com/bevyengine/bevy/issues/7195
	// sRGB => XYZ => D65_2_D60 => AP1 => RRT_SAT
	static const float3x3 ACESInputMat =
	{
		{0.59719, 0.35458, 0.04823},
		{0.07600, 0.90834, 0.01566},
		{0.02840, 0.13383, 0.83777}
	};
	
	// ODT_SAT => XYZ => D60_2_D65 => sRGB
	static const float3x3 ACESOutputMat =
	{
		{ 1.60475, -0.53108, -0.07367},
		{-0.10208,  1.10813, -0.00605},
		{-0.00327, -0.07276,  1.07602}
	};
	
	float3 RRTAndODTFit(float3 v)
	{
		float3 a = v * (v + 0.0245786f) - 0.000090537f;
		float3 b = v * (0.983729f * v + 0.4329510f) + 0.238081f;
		return a / b;
	}
	
	float3 ACESFitted(float3 color)
	{
		//color = saturate(color);
		color = mul(ACESInputMat, color);
		color = RRTAndODTFit(color); // Apply RRT and ODT
		color = mul(ACESOutputMat, color);
		//color = saturate(color); // Clamp to [0, 1]
		return color;
	}
	
	#define RGB_Lift  float3(1.000, 1.000, 1.000)  //[0.000 to 2.000] Adjust shadows for Red, Green and Blue.
	#define RGB_Gamma float3(1.45, 1.40, 1.40)  //[0.000 to 2.000] Adjust midtones for Red, Green and Blue
	#define RGB_Gain  float3(1.00, 1.00, 1.00)  //[0.000 to 2.000] Adjust highlights for Red, Green and Blue
	// Note that a value of 1.000 is a neutral setting that leave the color unchanged.
	
	// https://github.com/zachsaw/RenderScripts/blob/master/RenderScripts/ImageProcessingShaders/SweetFX/LiftGammaGain.hlsl
	float3 LiftGammaGainPass( float3 color )
	{
		// -- Lift --
		//color = color + (RGB_Lift / 2.0 - 0.5) * (1.0 - color); 
		//color = color * (1.5-0.5 * RGB_Lift) + 0.5 * RGB_Lift - 0.5;
		//color = saturate(color); // isn't strictly necessary, but doesn't cost performance.
		//color *= RGB_Gain;  // Gain
		color.rgb = pow(color, 1.0 / RGB_Gamma); // Gamma
		return color; // saturate(colorInput);
	}
	
	static const float3 LuminanceWeights = float3(0.299,0.587,0.114);
	
	float3 ApplyTonemapAlways(float3 colorIn, float4 Tonemapping){
	
		float exposure = Tonemapping.x + 0.0001; // Addition of tiny value prevents weirdness, went solid black on device in some places without?!
		float contrast = Tonemapping.y;
		float saturation = Tonemapping.z;
		
		float blend = Tonemapping.w * _TonemappingMasterBlend; // Needed to turn off tonemapping to capture reflection probes in HDR
		
		//colorIn = saturate(colorIn);
		//colorIn = clamp(colorIn,0.01,0.99);
		float3 x = colorIn * exposure * 0.66;
		
		// Simplified ACES
		//x = mul(ACESInputMat, x);
		float3 tonemapped = (x*(2.51*x+0.03)) / (x*(2.43*x+0.59)+0.14);// * 0.8; // Loses bright spots going to white if color fully saturated without the SRGB conversion ( ACESInputMat/ACESOutputMat )
		//tonemapped = mul(ACESOutputMat, tonemapped);
		
		// Better 'fitted' ACES - keeps bright spots
		//float3 tonemapped = ACESFitted(x*1.44); // https://github.com/TheRealMJP/BakingLab/blob/master/BakingLab/ACES.hlsl
		//tonemapped = abs(tonemapped);
		float3 color = pow( tonemapped, 0.7142857143 * contrast ); // Gamma correction, lowers contrast so it doesnt go so dark
		//float3 color = tonemapped;
		
		//float3 LuminanceWeights = float3(0.299,0.587,0.114);
		float luminance = dot(color,LuminanceWeights);
		color.rgb = lerp(luminance,color.rgb,saturation); // Saturation
		
		//#if !SHADER_API_MOBILE
			color = lerp( colorIn, color, blend  ); // Blend - // Needed to turn off tonemapping to capture reflection probes in HDR
		//#endif
		
		//color = saturate(color); // Prevent bloom blowouts, shouldn't need as you should not have bloom/post on as well as tone mapping in shader
		
		return color;
	}
	
	float3 ApplyTonemap(float3 colorIn, float4 Tonemapping){
		#if SHADER_API_MOBILE
			#define GLOBALTONEMAPPING
		#else
			#undef GLOBALTONEMAPPING
		#endif
		#if defined(GLOBALTONEMAPPING) //&& !defined(TONEMAPPINGELSEWHERE) // _TONEMAPPING_ON
			float3 color = ApplyTonemapAlways(colorIn, Tonemapping);
			return color;// * float3(0,1,0);
		#else			
			return colorIn; 
			/* // Removed Saturation as on desktop it will double apply to grabpass/water, do as post effect instead
			float3 color = colorIn.rgb;
			
			float saturation = Tonemapping.z;
			float blend = Tonemapping.w * _TonemappingMasterBlend; // Needed to turn off tonemapping to capture reflection probes in HDR
			
			float luminance = dot(color,LuminanceWeights);
			color.rgb = lerp(luminance,color.rgb,saturation); // Saturation
			
			color = lerp( colorIn, color, blend  ); // Blend - // Needed to turn off tonemapping to capture reflection probes in HDR
			
			return colorIn; 
			*/
			//float exposure = Tonemapping.x; // Always apply exposure even if no tonemapping is used // Edit: breaks grabpass as it gets double exposed
			//return colorIn * exposure;// * float3(1,0,0);;
		#endif
	}

#endif