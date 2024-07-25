// Made with Amplify Shader Editor v1.9.3.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Skybox/SkySorcery-Dev"
{
	Properties
	{
		_Rotation("Rotation", Range( 0 , 360)) = 0
		[HDR][NoScaleOffset][maintexture]_Tex("Sky Cubemap (_Tex)", CUBE) = "gray" {}
		_Tint("Tint", Color) = (0.5,0.5,0.5,0.5019608)
		[Gamma]_Exposure("Exposure", Range( 0 , 8)) = 1
		[Toggle]_HorizonFogEnable("Horizon Fog Enable", Range( 0 , 1)) = 0
		_HorizonFogHeight("-Horizon Fog Height [_HorizonFogEnable]", Range( 0.0001 , 10)) = 3
		[HideInInspector]_Tex_HDR("_Tex_HDR", Vector) = (0,0,0,0)

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		
		
		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
			#define TONEMAPPINGELSEWHERE
			#define UNDERWATERELSEWHERE


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma shader_feature GLOBALTONEMAPPING
			#pragma multi_compile __ _USEUNDERWATER


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _skyGradientColor1;
			uniform float4 _skyGradientColor2;
			uniform samplerCUBE _Tex;
			uniform float GlobalSkyRotation;
			uniform float _Rotation;
			uniform float _Exposure;
			uniform float4 _Tint;
			uniform float4 _Tex_HDR;
			uniform float _HorizonFogHeight;
			uniform float _HorizonFogEnable;
			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			float4 MyCustomExpression311( float3 pos, float exposure, float3 tint, float4 TexHDR )
			{
				half4 tex = texCUBE (_Tex, pos);
				half3 c = DecodeHDR (tex, TexHDR);
				c = c * tint.rgb * unity_ColorSpaceDouble.rgb;
				c *= exposure;
				return float4(c,1);
			}
			
			inline float4 GetUnderWaterFogs240_g587( float3 viewDir, float3 camWorldPos, float3 posWS, float4 oceanFogDensities, float oceanHeight, float4 oceanFogTop_RGB_Exponent, float4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			
			inline float3 ApplyTonemapper( float4 settings, float3 color )
			{
				return ApplyTonemap( color, settings );;
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float temp_output_58_0 = radians( ( GlobalSkyRotation + _Rotation ) );
				float3 ase_worldPos = mul(unity_ObjectToWorld, float4( (v.vertex).xyz, 1 )).xyz;
				float3 rotatedValue310 = RotateAroundAxis( float3( 0,0,0 ), ase_worldPos, float3( 0,1,0 ), temp_output_58_0 );
				
				float vertexToFrag317 = ase_worldPos.y;
				o.ase_texcoord1.x = vertexToFrag317;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.yzw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = rotatedValue310;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float4 appendResult11_g593 = (float4(_TonemappingSettings.x , _TonemappingSettings.y , _TonemappingSettings.z , _TonemappingSettings.w));
				float4 settings65 = appendResult11_g593;
				float temp_output_58_0 = radians( ( GlobalSkyRotation + _Rotation ) );
				float3 rotatedValue310 = RotateAroundAxis( float3( 0,0,0 ), WorldPosition, float3( 0,1,0 ), temp_output_58_0 );
				float3 pos311 = rotatedValue310;
				float exposure311 = _Exposure;
				float3 tint311 = _Tint.rgb;
				float4 TexHDR311 = _Tex_HDR;
				float4 localMyCustomExpression311 = MyCustomExpression311( pos311 , exposure311 , tint311 , TexHDR311 );
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 viewDir240_g587 = ase_worldViewDir;
				float3 camWorldPos240_g587 = _WorldSpaceCameraPos;
				float3 WorldPos252_g587 = WorldPosition;
				float3 posWS240_g587 = WorldPos252_g587;
				float4 oceanFogDensities240_g587 = OceanFogDensities;
				float temp_output_250_0 = ( GlobalOceanOffset + GlobalOceanHeight );
				float temp_output_108_0_g587 = temp_output_250_0;
				float oceanHeight240_g587 = temp_output_108_0_g587;
				float4 oceanFogTop_RGB_Exponent240_g587 = OceanFogTop_RGB_Exponent;
				float4 oceanFogBottom_RGB_Intensity240_g587 = OceanFogBottom_RGB_Intensity;
				float4 localGetUnderWaterFogs240_g587 = GetUnderWaterFogs240_g587( viewDir240_g587 , camWorldPos240_g587 , posWS240_g587 , oceanFogDensities240_g587 , oceanHeight240_g587 , oceanFogTop_RGB_Exponent240_g587 , oceanFogBottom_RGB_Intensity240_g587 );
				float4 FogRes185_g587 = localGetUnderWaterFogs240_g587;
				float3 appendResult94_g587 = (float3(FogRes185_g587.xyz));
				float3 temp_output_292_103 = appendResult94_g587;
				float vertexToFrag317 = i.ase_texcoord1.x;
				float4 lerpResult104 = lerp( localMyCustomExpression311 , float4( temp_output_292_103 , 0.0 ) , ( GlobalOceanUnder * saturate( (1.0 + (vertexToFrag317 - 0.0) * (0.0 - 1.0) / (( _ProjectionParams.z * 0.5 ) - 0.0)) ) ));
				#ifdef _USEUNDERWATER
				float4 staticSwitch243 = lerpResult104;
				#else
				float4 staticSwitch243 = localMyCustomExpression311;
				#endif
				float4 lerpResult300 = lerp( staticSwitch243 , unity_FogColor , ( ( 1.0 - GlobalOceanUnder ) * pow( saturate( (1.0 + (vertexToFrag317 - 0.0) * (0.0 - 1.0) / (( _ProjectionParams.z * _HorizonFogHeight ) - 0.0)) ) , 4.0 ) * _HorizonFogEnable ));
				float3 color65 = lerpResult300.rgb;
				float3 localApplyTonemapper65 = ApplyTonemapper( settings65 , color65 );
				
				
				finalColor = float4( localApplyTonemapper65 , 0.0 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ShaderSorceryInspector"
	
	Fallback Off
}
