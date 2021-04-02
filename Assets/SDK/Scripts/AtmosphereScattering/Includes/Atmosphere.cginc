#ifndef _GAMESKY_ATMOSPHERE_CGINC
#define _GAMESKY_ATMOSPHERE_CGINC

uniform half _SunSize;
uniform half _SkyExposure;		// HDR exposure
uniform half4 _GroundColor;
uniform half4 _SkyTint;
uniform half _SunBrightness;
uniform half _AtmosphereThickness;

#include "Includes/Constant.cginc"

// Calculates the Rayleigh phase function
half getRayleighPhase(half eyeCos2)
{
	return 0.75 + 0.75*eyeCos2;
}

half getRayleighPhase(half3 light, half3 ray)
{
	half eyeCos = dot(light, ray);
	return getRayleighPhase(eyeCos * eyeCos);
}

float scale(float inCos)
{
	float x = 1.0 - inCos;
#if defined(SHADER_API_N3DS)
	// The polynomial expansion here generates too many swizzle instructions for the 3DS vertex assembler
	// Approximate by removing x^1 and x^2
	return 0.25 * exp(-0.00287 + x*x*x*(-6.80 + x*5.25));
#else
	return 0.25 * exp(-0.00287 + x*(0.459 + x*(3.83 + x*(-6.80 + x*5.25))));
#endif
}

// Calculates the Mie phase function
half getMiePhase(half eyeCos, half eyeCos2)
{
	half temp = 1.0 + MIE_G2 - 2.0 * MIE_G * eyeCos;
	temp = pow(temp, pow(_SunSize, 0.65) * 10);
	temp = max(temp, 1.0e-4); // prevent division by zero, esp. in half precision
	temp = 1.5 * ((1.0 - MIE_G2) / (2.0 + MIE_G2)) * (1.0 + eyeCos2) / temp;
#if defined(UNITY_COLORSPACE_GAMMA) && SKYBOX_COLOR_IN_TARGET_COLOR_SPACE
	temp = pow(temp, .454545);
#endif
	return temp;
}

half calcSunSpot(half3 vec1, half3 vec2)
{
	half3 delta = vec1 - vec2;
	half dist = length(delta);
	half spot = 1.0 - smoothstep(0.0, _SunSize, dist);
	return kSunScale * spot * spot;
}

#endif