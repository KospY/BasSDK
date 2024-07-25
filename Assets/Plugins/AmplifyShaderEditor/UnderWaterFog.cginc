//#include "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"

// Requires input / globals:
//#ifdef UNDERWATER_GEAR
//	#ifndef SHADOWOOD_WATER_SHADER_VARIABLES_INCLUDED
//		#define SHADOWOOD_WATER_SHADER_VARIABLES_INCLUDED
//		float4 OceanWaterTint_RGB;
//		float4 OceanFogBottom_RGB_Intensity;
//		float4 OceanFogTop_RGB_Exponent;
//		float4 OceanFogDensities;
//		float GlobalOceanHeight;
//		float GlobalOceanUnder;
//		float GlobalOceanOffset;
//	#endif
//#endif

#ifndef UNDERWATER_GEAR
	#define UNDERWATER_GEAR
	
	// Externally some places skip all this if UNDERWATERELSEWHERE
	//#define UNDERWATERELSEWHERE
	
	#pragma shader_feature _USEUNDERWATER
	//#pragma multi_compile __ _USEUNDERWATER
	
	#ifndef SHADOWOOD_WATER_SHADER_VARIABLES_INCLUDED
		#define SHADOWOOD_WATER_SHADER_VARIABLES_INCLUDED
		float4 OceanWaterTint_RGB;
		float4 OceanFogBottom_RGB_Intensity;
		float4 OceanFogTop_RGB_Exponent;
		float4 OceanFogDensities;
		float GlobalOceanHeight;
		float GlobalOceanUnder;
		float GlobalOceanOffset;
	#endif
	
	float4 GetUnderWaterFog(
	float3 viewDir, 
	float3 camWorldPos, 
	float3 posWS, 
	float4 _OceanFogDensities, 
	float _OceanHeight,  
	float4 _OceanFogTop_RGB_Exponent, 
	float4 _OceanFogBottom_RGB_Intensity){
		
		float fogDensityDepth = _OceanFogDensities.y;
		float fogDensityDistance = _OceanFogDensities.z;
		float fogOffset = _OceanFogDensities.w;
		
		float oceanHeight = _OceanHeight;// + _GlobalOceanOffset;
		
		float exponent = _OceanFogTop_RGB_Exponent.w;
		float intensity = _OceanFogBottom_RGB_Intensity.w;
		float3 fogBottom = _OceanFogBottom_RGB_Intensity.rgb;
		float3 fogTop = _OceanFogTop_RGB_Exponent.rgb;
		
		//float clipZ_0Far = UNITY_Z_0_FAR_FROM_CLIPSPACE(posCS);
		//float fogFactor = saturate(clipZ_0Far * unity_FogParams.z + unity_FogParams.w);
		//float length = 1;
		//float distance = (clipZ_0Far - fogOffset) / length;
		float dist = distance(posWS, camWorldPos) - fogOffset;
		
		float surfaceDistance = abs(oceanHeight - camWorldPos.y);
		//beer-lambert law, Fog =1/e^(distance * density)
		float e = 2.7182818284590452353602874713527f;
		float distanceFog = 1.0 - saturate(1.0f / pow(e, (dist * fogDensityDistance)));
		float depthFog = 1.0 - saturate(1.0f / pow(e, (surfaceDistance * fogDensityDepth)));
		
		float tt = lerp(-0.976,0.4,depthFog);
		float surfaceDir = dot( viewDir,  float3(0,-1,0) );
		float sunFog = pow( max( ((surfaceDir - tt) * 0.5 ) + 0.5,0), exponent ) * intensity;
		
		float3 fogRGB = lerp(fogBottom, fogTop, sunFog);
		float4 fogCol = float4(fogRGB, distanceFog);
		
		return fogCol;
	}
	
	float4 ApplyUnderWaterFog(float4 colorIn, float4 _OceanWaterTint_RGB){
		colorIn.rgb *= _OceanWaterTint_RGB.rgb; // Apply tint
		return colorIn;
	}
	
#endif