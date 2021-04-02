Shader "Hidden/AtmosphereScattering"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	
	SubShader
	{
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

		Pass
	    {
		HLSLPROGRAM
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
		#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

        #pragma vertex vert
        #pragma fragment frag

        #include "Includes/Atmosphere.cginc"
        
         struct Attributes 
         {
            float4 positionOS   : POSITION;
            float2 texcoord : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
         };
        
         struct Varyings 
         {
            float4 positionCS : SV_POSITION;
			float4 wpos         : TEXCOORD1;
            half4  uv           : TEXCOORD0;
			float4 ScreenPosition : TEXCOORD2;
            UNITY_VERTEX_OUTPUT_STEREO
        };

	    Varyings vert(Attributes input)
	    {
		    Varyings output;
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
			output.wpos = mul(unity_ObjectToWorld, input.positionOS);
			output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(output.wpos), _ProjectionParams.x);

            float4 projPos = output.positionCS * 0.5;
            projPos.xy = projPos.xy + projPos.w;

            output.uv.xy = UnityStereoTransformScreenSpaceTex(input.texcoord);
            output.uv.zw = projPos.xy;

            return output;
	    }
	    
	    TEXTURE2D(_MainTex);
	    SAMPLER(sampler_MainTex);

		TEXTURE2D_X(_CameraDepthTexture);
		SAMPLER(sampler_CameraDepthTexture);

	    half _AtmoScatteringIntensity;

	    float _AtmoScatterFarPoint;

	    float _AtmoStartDist;
	    float _AtmoEndDist;

		float _MaxScatteringValue;

		float _HorizonPoint;
		float _HorizonTransitionSmoothness;

	    float _DEBUG_SCATTER;

		float SampleSceneDepth(float2 uv)
		{
			return SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(uv)).r;
		}

		void SceneDepthLinear01(float4 UV, out float Out)
		{
			Out = Linear01Depth(SampleSceneDepth(UV.xy), _ZBufferParams);
		}

	    half4 frag(Varyings i) : SV_Target
	    {
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

			float4 screenPosRaw = i.ScreenPosition;
			float4 screenPos = float4(i.ScreenPosition.xy / i.ScreenPosition.w, 0, 0);

			float texDepth = 0.0;
			SceneDepthLinear01(screenPos, texDepth);

			//// WPos Calc
			float3 vtx_wpos = i.wpos;

			float d1 = screenPosRaw.w;// / _ProjectionParams.z;
			float d2 = texDepth * _ProjectionParams.z;
			
			float d = SampleSceneDepth(screenPos);
			float dx = i.positionCS.z;

			float clipPlaneDelta = clamp(_ZBufferParams.w * 1.0, 0.001, 100.0);

			float maxDistanceRampUp = clipPlaneDelta * d1;
			
			float epsilon = 0.0000001;
			if(abs(d - dx) > epsilon)
			    return half4(0,0,0,0);

			//if (abs(d1 - d2) > maxDistanceRampUp)
			//	return half4(0, 0, 0, 0);

			//return half4(d1, 0, 0, 1);

			////////////

            float3 viewDirRef = i.wpos-_WorldSpaceCameraPos;
            float distFromPoint = length(viewDirRef);
            float3 viewDir = viewDirRef / distFromPoint;
            
			float3 eyeRay = viewDir;
	        
	        float lerpVal = saturate((distFromPoint - _AtmoStartDist) / (_AtmoEndDist - _AtmoStartDist) * _AtmoScatteringIntensity);

	        // Attenuate by height factor
	        lerpVal /= 1.0 + (saturate(eyeRay.y) / 1.5);

	        //UNITY_FLATTEN
	        //if (!_DEBUG_SCATTER && screenDepth >= 1)
	        //	lerpVal = _AtmoScatterFarPoint;
	        lerpVal = min(_AtmoScatterFarPoint, lerpVal);

	        // Save some ALU
	        //if (saturate(lerpVal) <= 0.0)
	    	//    discard;

	        float far = 0.0;
	        half3 cIn, cOut;

	        // Atmosphere Code
	        //float3 kSkyTintInGammaSpace = COLOR_2_GAMMA(_SkyTint); // convert tint from Linear back to Gamma
	        float3 kSkyTintInGammaSpace = _SkyTint; // convert tint from Linear back to Gamma
	        float3 kScatteringWavelength = lerp(
		    kDefaultScatteringWavelength - kVariableRangeForScatteringWavelength,
		    kDefaultScatteringWavelength + kVariableRangeForScatteringWavelength,
		    half3(1, 1, 1) - kSkyTintInGammaSpace); // using Tint in sRGB gamma allows for more visually linear interpolation and to keep (.5) at (128, gray in sRGB) point
	        float3 kInvWavelength = 1.0 / pow(kScatteringWavelength, 4);

	        float kKrESun = kRAYLEIGH * kSUN_BRIGHTNESS;
	        float kKr4PI = kRAYLEIGH * 4.0 * 3.14159265;

	        float3 cameraPos = float3(0, kInnerRadius + kCameraHeight, 0); 	// The camera's current position

	        //if (eyeRay.y >= 0.0)
	        {
		        UNITY_FLATTEN
		        if (eyeRay.y < 0.0)
			        eyeRay.y = 0.0;

		        // Sky
		        // Calculate the length of the "atmosphere"
		        far = sqrt(kOuterRadius2 + kInnerRadius2 * eyeRay.y * eyeRay.y - kInnerRadius2) - kInnerRadius * eyeRay.y;

		        float3 pos = cameraPos + far * eyeRay;

		        // Calculate the ray's starting position, then calculate its scattering offset
		        float height = kInnerRadius + kCameraHeight;
		        float depth = exp(kScaleOverScaleDepth * (-kCameraHeight));
		        float startAngle = dot(eyeRay, cameraPos) / height;
		        float startOffset = depth*scale(startAngle);

		        // Initialize the scattering loop variables
		        float sampleLength = far / kSamples;
		        float scaledLength = sampleLength * kScale/* * screenDepth*/;
		        float3 sampleRay = eyeRay * sampleLength;
		        float3 samplePoint = cameraPos + sampleRay * 0.5;

	    	    // Now loop through the sample rays
	    	    float3 frontColor = float3(0.0, 0.0, 0.0);
	    	
		        {
			        float height = length(samplePoint);
			        float depth = exp(kScaleOverScaleDepth * (kInnerRadius - height));
			        float lightAngle = dot(_MainLightPosition.xyz, samplePoint) / height;
		    	    float cameraAngle = dot(eyeRay, samplePoint) / height;
			        float scatter = (startOffset + depth*(scale(lightAngle) - scale(cameraAngle)));
		    	    float3 attenuate = exp(-clamp(scatter, 0.0, kMAX_SCATTER) * (kInvWavelength * kKr4PI + kKm4PI));

			        frontColor += attenuate * (depth * scaledLength);
			        samplePoint += sampleRay;
		        }
		        {
			        float height = length(samplePoint);
			        float depth = exp(kScaleOverScaleDepth * (kInnerRadius - height));
			        float lightAngle = dot(_MainLightPosition.xyz, samplePoint) / height;
			        float cameraAngle = dot(eyeRay, samplePoint) / height;
			        float scatter = (startOffset + depth*(scale(lightAngle) - scale(cameraAngle)));
			        float3 attenuate = exp(-clamp(scatter, 0.0, kMAX_SCATTER) * (kInvWavelength * kKr4PI + kKm4PI));

			        frontColor += attenuate * (depth * scaledLength);
			        samplePoint += sampleRay;
		        }

		        // Finally, scale the Mie and Rayleigh colors and set up the varying variables for the pixel shader
		        cIn = frontColor * (kInvWavelength * kKrESun);
		        cOut = frontColor * kKmESun;
	        }

	        float skyGroundFactor = -eyeRay.y / SKY_GROUND_THRESHOLD;

    	    // if we want to calculate color in vprog:
    	    // 1. in case of linear: multiply by _SkyExposure in here (even in case of lerp it will be common multiplier, so we can skip mul in fshader)
	        // 2. in case of gamma and SKYBOX_COLOR_IN_TARGET_COLOR_SPACE: do sqrt right away instead of doing that in fshader
    
	        half3 groundColor = _SkyExposure * (cIn + /*COLOR_2_LINEAR(*/_GroundColor/*)*/ * cOut);
	        //half3 skyColor = _SkyExposure * (cIn * getRayleighPhase(_MainLightPosition.xyz, -eyeRay));
			half3 skyColor = _SkyExposure * (cIn + _SkyTint) * cOut;

#if defined(UNITY_COLORSPACE_GAMMA) && SKYBOX_COLOR_IN_TARGET_COLOR_SPACE
	        groundColor = sqrt(groundColor);
	        skyColor = sqrt(skyColor);
#endif

	        half4 col = half4(0.0, 0.0, 0.0, 0.0);

	        // if y > 1 [eyeRay.y < -SKY_GROUND_THRESHOLD] - ground
	        // if y >= 0 and < 1 [eyeRay.y <= 0 and > -SKY_GROUND_THRESHOLD] - horizon
	        // if y < 0 [eyeRay.y > 0] - sky
			half y = viewDir.y / SKY_GROUND_THRESHOLD;//skyGroundFactor;

	        // if we did precalculate color in vprog: just do lerp between them
	        /*col = lerp(float4(skyColor, 1.0), float4(groundColor, _GroundColor.a), saturate(y));

#if defined(UNITY_COLORSPACE_GAMMA) && !SKYBOX_COLOR_IN_TARGET_COLOR_SPACE
	        col = LINEAR_2_OUTPUT(col);
#endif*/

			// Homogeneous Space Calculation
			/*float3 csToVp = TransformWViewToHClip(TransformWorldToView(i.wpos));
			csToVp.y *= -1.0;

			y = saturate((csToVp.y - _HorizonPoint) / _HorizonTransitionSmoothness);*/

			y = saturate((i.wpos.y - _HorizonPoint) / _HorizonTransitionSmoothness);

	        //col = (lerp(_GroundColor * _SkyExposure, _SkyTint * _SkyExposure, (eyeRay.y + 1.0) * 0.5));
    	    col = (lerp(_GroundColor * _SkyExposure, _SkyTint * _SkyExposure, y/*eyeRay.y*/));

	            //return lerp(colScreen, half4(col.rgb, colScreen.a), saturate(lerpVal));
    	        return half4(col.rgb, clamp(saturate(lerpVal), 0, _MaxScatteringValue));
    	        //return half4(1,1,1,1);
	        }
		    ENDHLSL
	    }
	}
}