// Made with Amplify Shader Editor v1.9.3.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ThunderRoad/VFX/vfx_fake_bloom_additive_flowmap"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[Header(Culling Mode)]_Culling("Culling", Range( 0 , 2)) = 2
		[Feature(_CAUSTICSENABLE)]HeaderOceanFog("# Ocean Fog Caustics", Float) = 0
		_Main("Main", 2D) = "black" {}
		_Size("Size", Range( 0 , 10)) = 1
		_Flowmap("Flowmap", 2D) = "white" {}
		_TextureIntensity("Texture Intensity", Float) = 10
		_FlowSpeed("Flow Speed", Float) = 1
		_FlowDirection("Flow Direction", Vector) = (0,0,0,0)
		_TintColor("Tint Color", Color) = (1,1,1,0)
		_WaveContrast("Wave Contrast", Range( 0 , 2)) = 1
		_FlowStrengh("Flow Strengh", Vector) = (0,0,0,0)


		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25

		[HideInInspector] _QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector] _QueueControl("_QueueControl", Float) = -1

        [HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}

		[HideInInspector][ToggleOff] _ReceiveShadows("Receive Shadows", Float) = 1.0
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" "UniversalMaterialType"="Unlit" }

		Cull [_Culling]
		AlphaToMask Off

		

		HLSLINCLUDE
		#pragma target 3.5
		#pragma prefer_hlslcc gles
		// ensure rendering platforms toggle list is visible

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}

		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
							(( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }

			Blend OneMinusDstColor One, One OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			

			HLSLPROGRAM

			#define _SURFACE_TYPE_TRANSPARENT 1
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_SRP_VERSION 120115
			#define UNDERWATERELSEWHERE
			#define TONEMAPPINGELSEWHERE
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"


			#pragma shader_feature_local _RECEIVE_SHADOWS_OFF
			#pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3

			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DYNAMICLIGHTMAP_ON
			#pragma multi_compile_fragment _ DEBUG_DISPLAY

            #pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS SHADERPASS_UNLIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging3D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature GLOBALTONEMAPPING


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 positionWS : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
					float fogFactor : TEXCOORD2;
				#endif
				float4 ase_color : COLOR;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _TintColor;
			float4 _Main_ST;
			float2 _FlowDirection;
			float2 _FlowStrengh;
			float _Culling;
			float _TextureIntensity;
			float _Size;
			float _FlowSpeed;
			float _WaveContrast;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			sampler2D _Caustics;
			sampler2D _Main;
			sampler2D _Flowmap;


			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			float4 CalculateContrast( float contrastValue, float4 colorTarget )
			{
				float t = 0.5 * ( 1.0 - contrastValue );
				return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
			}
			float3 HSVToRGB( float3 c )
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}
			inline float4 GetUnderWaterFogs240_g2875( float3 viewDir, float3 camWorldPos, float3 posWS, float4 oceanFogDensities, float oceanHeight, float4 oceanFogTop_RGB_Exponent, float4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			
			float3 ApplyTonemapper( float4 settings, float3 color )
			{
				#ifdef TONEMAPPING_GEAR
				return ApplySatExposureOnly( color, settings );
				#else
				return color;
				#endif
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_color = v.ase_color;
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.positionWS = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				#ifdef ASE_FOG
					o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif

				o.positionCS = positionCS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.positionWS;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float4 appendResult11_g2887 = (float4(_TonemappingSettings.x , _TonemappingSettings.y , _TonemappingSettings.z , _TonemappingSettings.w));
				float4 temp_output_4_0_g2886 = appendResult11_g2887;
				float4 settings7_g2886 = temp_output_4_0_g2886;
				float4 appendResult42 = (float4(IN.ase_color.r , IN.ase_color.g , IN.ase_color.b , 0.0));
				float2 uv_Main = IN.ase_texcoord3.xy * _Main_ST.xy + _Main_ST.zw;
				float2 temp_output_4_0_g1 = (( uv_Main / _Size )).xy;
				float2 temp_output_17_0_g1 = _FlowStrengh;
				float mulTime22_g1 = _TimeParameters.x * _FlowSpeed;
				float temp_output_27_0_g1 = frac( mulTime22_g1 );
				float2 temp_output_11_0_g1 = ( temp_output_4_0_g1 + ( -(_FlowDirection*2.0 + -1.0) * temp_output_17_0_g1 * temp_output_27_0_g1 ) );
				float temp_output_55_0_g1 = 1.0;
				float3 unpack48_g1 = UnpackNormalScale( tex2D( _Flowmap, temp_output_11_0_g1 ), temp_output_55_0_g1 );
				unpack48_g1.z = lerp( 1, unpack48_g1.z, saturate(temp_output_55_0_g1) );
				float2 temp_output_12_0_g1 = ( temp_output_4_0_g1 + ( -(_FlowDirection*2.0 + -1.0) * temp_output_17_0_g1 * frac( ( mulTime22_g1 + 0.5 ) ) ) );
				float3 unpack49_g1 = UnpackNormalScale( tex2D( _Flowmap, temp_output_12_0_g1 ), temp_output_55_0_g1 );
				unpack49_g1.z = lerp( 1, unpack49_g1.z, saturate(temp_output_55_0_g1) );
				float3 lerpResult9_g1 = lerp( unpack48_g1 , unpack49_g1 , ( abs( ( temp_output_27_0_g1 - 0.5 ) ) / 0.5 ));
				float4 tex2DNode5 = tex2D( _Main, lerpResult9_g1.xy );
				float4 temp_output_38_0 = ( ( appendResult42 * float4( (( ( IN.ase_color.a * _TextureIntensity ) * tex2DNode5 )).rgb , 0.0 ) ) * tex2DNode5.a );
				float4 temp_output_59_0 = ( _TintColor * temp_output_38_0 );
				float4 temp_output_51_0 = saturate( ( temp_output_59_0 + ( temp_output_59_0 * CalculateContrast(_WaveContrast,temp_output_38_0) ) ) );
				float3 temp_output_14_0_g2903 = temp_output_51_0.rgb;
				float3 desaturateInitialColor46_g2903 = temp_output_14_0_g2903;
				float desaturateDot46_g2903 = dot( desaturateInitialColor46_g2903, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar46_g2903 = lerp( desaturateInitialColor46_g2903, desaturateDot46_g2903.xxx, 0.2 );
				float4 _Vector3 = float4(1,1,1,0);
				float4 appendResult47_g2828 = (float4(_Vector3));
				float OceanUnder289_g2828 = GlobalOceanUnder;
				float3 WorldPosition256_g2828 = WorldPosition;
				float3 WorldPos252_g2875 = WorldPosition256_g2828;
				float temp_output_67_0_g2828 = ( GlobalOceanOffset + GlobalOceanHeight );
				float OceanHeight274_g2828 = temp_output_67_0_g2828;
				float temp_output_108_0_g2875 = OceanHeight274_g2828;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ViewDir264_g2828 = ase_worldViewDir;
				float3 viewDir240_g2875 = ViewDir264_g2828;
				float3 camWorldPos240_g2875 = _WorldSpaceCameraPos;
				float3 posWS240_g2875 = WorldPos252_g2875;
				float4 oceanFogDensities240_g2875 = OceanFogDensities;
				float oceanHeight240_g2875 = temp_output_108_0_g2875;
				float4 oceanFogTop_RGB_Exponent240_g2875 = OceanFogTop_RGB_Exponent;
				float4 oceanFogBottom_RGB_Intensity240_g2875 = OceanFogBottom_RGB_Intensity;
				float4 localGetUnderWaterFogs240_g2875 = GetUnderWaterFogs240_g2875( viewDir240_g2875 , camWorldPos240_g2875 , posWS240_g2875 , oceanFogDensities240_g2875 , oceanHeight240_g2875 , oceanFogTop_RGB_Exponent240_g2875 , oceanFogBottom_RGB_Intensity240_g2875 );
				float4 ifLocalVar257_g2875 = 0;
				UNITY_BRANCH 
				if( (WorldPos252_g2875).y < ( temp_output_108_0_g2875 + 0.2 ) )
				ifLocalVar257_g2875 = localGetUnderWaterFogs240_g2875;
				float4 FogRes185_g2875 = ifLocalVar257_g2875;
				float3 appendResult94_g2875 = (float3(FogRes185_g2875.xyz));
				float3 temp_output_394_103_g2828 = appendResult94_g2875;
				float temp_output_61_0_g2875 = ( 1.0 - (FogRes185_g2875).w );
				float temp_output_58_0_g2828 = ( 1.0 - temp_output_61_0_g2875 );
				float4 appendResult44_g2828 = (float4(temp_output_394_103_g2828 , temp_output_58_0_g2828));
				float4 ifLocalVar49_g2828 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g2828 >= 1.0 )
				ifLocalVar49_g2828 = appendResult44_g2828;
				else
				ifLocalVar49_g2828 = appendResult47_g2828;
				#ifdef _USEUNDERWATER
				float4 staticSwitch215_g2828 = max( ifLocalVar49_g2828 , float4( 0,0,0,0 ) );
				#else
				float4 staticSwitch215_g2828 = appendResult47_g2828;
				#endif
				float4 temp_output_1_0_g2903 = staticSwitch215_g2828;
				float3 FogRGB6_g2903 = (temp_output_1_0_g2903).xyz;
				float3 temp_cast_9 = (1.0).xxx;
				float3 appendResult100_g2875 = (float3(OceanWaterTint_RGB.xyz));
				float3 temp_cast_11 = (1.0).xxx;
				float3 ifLocalVar170_g2828 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g2828 >= 1.0 )
				ifLocalVar170_g2828 = appendResult100_g2875;
				else
				ifLocalVar170_g2828 = temp_cast_11;
				#ifdef _USEUNDERWATER
				float3 staticSwitch212_g2828 = ifLocalVar170_g2828;
				#else
				float3 staticSwitch212_g2828 = temp_cast_9;
				#endif
				float3 temp_output_2_0_g2903 = staticSwitch212_g2828;
				float3 hsvTorgb48_g2903 = RGBToHSV( ( FogRGB6_g2903 * temp_output_2_0_g2903 ) );
				float clampResult51_g2903 = clamp( hsvTorgb48_g2903.y , 0.0 , 0.8 );
				float3 hsvTorgb49_g2903 = HSVToRGB( float3(hsvTorgb48_g2903.x,clampResult51_g2903,1.0) );
				float3 temp_output_42_0_g2903 = saturate( hsvTorgb49_g2903 );
				float DepthMask5_g2903 = (temp_output_1_0_g2903).w;
				float3 lerpResult7_g2903 = lerp( ( desaturateVar46_g2903 * temp_output_42_0_g2903 ) , float3(0,0,0) , DepthMask5_g2903);
				float temp_output_31_0_g2903 = tex2DNode5.a;
				float3 lerpResult34_g2903 = lerp( float3( 0,0,0 ) , lerpResult7_g2903 , temp_output_31_0_g2903);
				float temp_output_27_0_g2903 = ( 1.0 - DepthMask5_g2903 );
				#ifdef _USEUNDERWATER
				float3 staticSwitch37_g2903 = ( lerpResult34_g2903 * temp_output_27_0_g2903 );
				#else
				float3 staticSwitch37_g2903 = temp_output_14_0_g2903;
				#endif
				float3 temp_output_3_0_g2886 = staticSwitch37_g2903;
				float3 color7_g2886 = temp_output_3_0_g2886;
				float3 localApplyTonemapper7_g2886 = ApplyTonemapper( settings7_g2886 , color7_g2886 );
				
				#ifdef _USEUNDERWATER
				float staticSwitch38_g2903 = ( temp_output_27_0_g2903 * temp_output_31_0_g2903 );
				#else
				float staticSwitch38_g2903 = temp_output_31_0_g2903;
				#endif
				float temp_output_126_25 = staticSwitch38_g2903;
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = localApplyTonemapper7_g2886;
				float Alpha = temp_output_126_25;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#if defined(_DBUFFER)
					ApplyDecalToBaseColor(IN.positionCS, Color);
				#endif

				#if defined(_ALPHAPREMULTIPLY_ON)
				Color *= Alpha;
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.positionCS.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM

            #define _SURFACE_TYPE_TRANSPARENT 1
            #pragma multi_compile_instancing
            #define ASE_SRP_VERSION 120115
            #define UNDERWATERELSEWHERE
            #define TONEMAPPINGELSEWHERE
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"


            #pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature GLOBALTONEMAPPING


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 positionWS : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _TintColor;
			float4 _Main_ST;
			float2 _FlowDirection;
			float2 _FlowStrengh;
			float _Culling;
			float _TextureIntensity;
			float _Size;
			float _FlowSpeed;
			float _WaveContrast;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			sampler2D _Caustics;
			sampler2D _Main;
			sampler2D _Flowmap;


			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			inline float4 GetUnderWaterFogs240_g2875( float3 viewDir, float3 camWorldPos, float3 posWS, float4 oceanFogDensities, float oceanHeight, float4 oceanFogTop_RGB_Exponent, float4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.positionWS = positionWS;
				#endif

				o.positionCS = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.positionWS;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 uv_Main = IN.ase_texcoord2.xy * _Main_ST.xy + _Main_ST.zw;
				float2 temp_output_4_0_g1 = (( uv_Main / _Size )).xy;
				float2 temp_output_17_0_g1 = _FlowStrengh;
				float mulTime22_g1 = _TimeParameters.x * _FlowSpeed;
				float temp_output_27_0_g1 = frac( mulTime22_g1 );
				float2 temp_output_11_0_g1 = ( temp_output_4_0_g1 + ( -(_FlowDirection*2.0 + -1.0) * temp_output_17_0_g1 * temp_output_27_0_g1 ) );
				float temp_output_55_0_g1 = 1.0;
				float3 unpack48_g1 = UnpackNormalScale( tex2D( _Flowmap, temp_output_11_0_g1 ), temp_output_55_0_g1 );
				unpack48_g1.z = lerp( 1, unpack48_g1.z, saturate(temp_output_55_0_g1) );
				float2 temp_output_12_0_g1 = ( temp_output_4_0_g1 + ( -(_FlowDirection*2.0 + -1.0) * temp_output_17_0_g1 * frac( ( mulTime22_g1 + 0.5 ) ) ) );
				float3 unpack49_g1 = UnpackNormalScale( tex2D( _Flowmap, temp_output_12_0_g1 ), temp_output_55_0_g1 );
				unpack49_g1.z = lerp( 1, unpack49_g1.z, saturate(temp_output_55_0_g1) );
				float3 lerpResult9_g1 = lerp( unpack48_g1 , unpack49_g1 , ( abs( ( temp_output_27_0_g1 - 0.5 ) ) / 0.5 ));
				float4 tex2DNode5 = tex2D( _Main, lerpResult9_g1.xy );
				float temp_output_31_0_g2903 = tex2DNode5.a;
				float4 _Vector3 = float4(1,1,1,0);
				float4 appendResult47_g2828 = (float4(_Vector3));
				float OceanUnder289_g2828 = GlobalOceanUnder;
				float3 WorldPosition256_g2828 = WorldPosition;
				float3 WorldPos252_g2875 = WorldPosition256_g2828;
				float temp_output_67_0_g2828 = ( GlobalOceanOffset + GlobalOceanHeight );
				float OceanHeight274_g2828 = temp_output_67_0_g2828;
				float temp_output_108_0_g2875 = OceanHeight274_g2828;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ViewDir264_g2828 = ase_worldViewDir;
				float3 viewDir240_g2875 = ViewDir264_g2828;
				float3 camWorldPos240_g2875 = _WorldSpaceCameraPos;
				float3 posWS240_g2875 = WorldPos252_g2875;
				float4 oceanFogDensities240_g2875 = OceanFogDensities;
				float oceanHeight240_g2875 = temp_output_108_0_g2875;
				float4 oceanFogTop_RGB_Exponent240_g2875 = OceanFogTop_RGB_Exponent;
				float4 oceanFogBottom_RGB_Intensity240_g2875 = OceanFogBottom_RGB_Intensity;
				float4 localGetUnderWaterFogs240_g2875 = GetUnderWaterFogs240_g2875( viewDir240_g2875 , camWorldPos240_g2875 , posWS240_g2875 , oceanFogDensities240_g2875 , oceanHeight240_g2875 , oceanFogTop_RGB_Exponent240_g2875 , oceanFogBottom_RGB_Intensity240_g2875 );
				float4 ifLocalVar257_g2875 = 0;
				UNITY_BRANCH 
				if( (WorldPos252_g2875).y < ( temp_output_108_0_g2875 + 0.2 ) )
				ifLocalVar257_g2875 = localGetUnderWaterFogs240_g2875;
				float4 FogRes185_g2875 = ifLocalVar257_g2875;
				float3 appendResult94_g2875 = (float3(FogRes185_g2875.xyz));
				float3 temp_output_394_103_g2828 = appendResult94_g2875;
				float temp_output_61_0_g2875 = ( 1.0 - (FogRes185_g2875).w );
				float temp_output_58_0_g2828 = ( 1.0 - temp_output_61_0_g2875 );
				float4 appendResult44_g2828 = (float4(temp_output_394_103_g2828 , temp_output_58_0_g2828));
				float4 ifLocalVar49_g2828 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g2828 >= 1.0 )
				ifLocalVar49_g2828 = appendResult44_g2828;
				else
				ifLocalVar49_g2828 = appendResult47_g2828;
				#ifdef _USEUNDERWATER
				float4 staticSwitch215_g2828 = max( ifLocalVar49_g2828 , float4( 0,0,0,0 ) );
				#else
				float4 staticSwitch215_g2828 = appendResult47_g2828;
				#endif
				float4 temp_output_1_0_g2903 = staticSwitch215_g2828;
				float DepthMask5_g2903 = (temp_output_1_0_g2903).w;
				float temp_output_27_0_g2903 = ( 1.0 - DepthMask5_g2903 );
				#ifdef _USEUNDERWATER
				float staticSwitch38_g2903 = ( temp_output_27_0_g2903 * temp_output_31_0_g2903 );
				#else
				float staticSwitch38_g2903 = temp_output_31_0_g2903;
				#endif
				float temp_output_126_25 = staticSwitch38_g2903;
				

				float Alpha = temp_output_126_25;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.positionCS.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "SceneSelectionPass"
			Tags { "LightMode"="SceneSelectionPass" }

			Cull Off
			AlphaToMask Off

			HLSLPROGRAM

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ASE_SRP_VERSION 120115
            #define UNDERWATERELSEWHERE
            #define TONEMAPPINGELSEWHERE
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"


            #pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature GLOBALTONEMAPPING


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _TintColor;
			float4 _Main_ST;
			float2 _FlowDirection;
			float2 _FlowStrengh;
			float _Culling;
			float _TextureIntensity;
			float _Size;
			float _FlowSpeed;
			float _WaveContrast;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			sampler2D _Caustics;
			sampler2D _Main;
			sampler2D _Flowmap;


			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			inline float4 GetUnderWaterFogs240_g2875( float3 viewDir, float3 camWorldPos, float3 posWS, float4 oceanFogDensities, float oceanHeight, float4 oceanFogTop_RGB_Exponent, float4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			

			int _ObjectId;
			int _PassValue;

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				o.ase_texcoord1.xyz = ase_worldPos;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				o.ase_texcoord1.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );

				o.positionCS = TransformWorldToHClip(positionWS);

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float2 uv_Main = IN.ase_texcoord.xy * _Main_ST.xy + _Main_ST.zw;
				float2 temp_output_4_0_g1 = (( uv_Main / _Size )).xy;
				float2 temp_output_17_0_g1 = _FlowStrengh;
				float mulTime22_g1 = _TimeParameters.x * _FlowSpeed;
				float temp_output_27_0_g1 = frac( mulTime22_g1 );
				float2 temp_output_11_0_g1 = ( temp_output_4_0_g1 + ( -(_FlowDirection*2.0 + -1.0) * temp_output_17_0_g1 * temp_output_27_0_g1 ) );
				float temp_output_55_0_g1 = 1.0;
				float3 unpack48_g1 = UnpackNormalScale( tex2D( _Flowmap, temp_output_11_0_g1 ), temp_output_55_0_g1 );
				unpack48_g1.z = lerp( 1, unpack48_g1.z, saturate(temp_output_55_0_g1) );
				float2 temp_output_12_0_g1 = ( temp_output_4_0_g1 + ( -(_FlowDirection*2.0 + -1.0) * temp_output_17_0_g1 * frac( ( mulTime22_g1 + 0.5 ) ) ) );
				float3 unpack49_g1 = UnpackNormalScale( tex2D( _Flowmap, temp_output_12_0_g1 ), temp_output_55_0_g1 );
				unpack49_g1.z = lerp( 1, unpack49_g1.z, saturate(temp_output_55_0_g1) );
				float3 lerpResult9_g1 = lerp( unpack48_g1 , unpack49_g1 , ( abs( ( temp_output_27_0_g1 - 0.5 ) ) / 0.5 ));
				float4 tex2DNode5 = tex2D( _Main, lerpResult9_g1.xy );
				float temp_output_31_0_g2903 = tex2DNode5.a;
				float4 _Vector3 = float4(1,1,1,0);
				float4 appendResult47_g2828 = (float4(_Vector3));
				float OceanUnder289_g2828 = GlobalOceanUnder;
				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float3 WorldPosition256_g2828 = ase_worldPos;
				float3 WorldPos252_g2875 = WorldPosition256_g2828;
				float temp_output_67_0_g2828 = ( GlobalOceanOffset + GlobalOceanHeight );
				float OceanHeight274_g2828 = temp_output_67_0_g2828;
				float temp_output_108_0_g2875 = OceanHeight274_g2828;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ViewDir264_g2828 = ase_worldViewDir;
				float3 viewDir240_g2875 = ViewDir264_g2828;
				float3 camWorldPos240_g2875 = _WorldSpaceCameraPos;
				float3 posWS240_g2875 = WorldPos252_g2875;
				float4 oceanFogDensities240_g2875 = OceanFogDensities;
				float oceanHeight240_g2875 = temp_output_108_0_g2875;
				float4 oceanFogTop_RGB_Exponent240_g2875 = OceanFogTop_RGB_Exponent;
				float4 oceanFogBottom_RGB_Intensity240_g2875 = OceanFogBottom_RGB_Intensity;
				float4 localGetUnderWaterFogs240_g2875 = GetUnderWaterFogs240_g2875( viewDir240_g2875 , camWorldPos240_g2875 , posWS240_g2875 , oceanFogDensities240_g2875 , oceanHeight240_g2875 , oceanFogTop_RGB_Exponent240_g2875 , oceanFogBottom_RGB_Intensity240_g2875 );
				float4 ifLocalVar257_g2875 = 0;
				UNITY_BRANCH 
				if( (WorldPos252_g2875).y < ( temp_output_108_0_g2875 + 0.2 ) )
				ifLocalVar257_g2875 = localGetUnderWaterFogs240_g2875;
				float4 FogRes185_g2875 = ifLocalVar257_g2875;
				float3 appendResult94_g2875 = (float3(FogRes185_g2875.xyz));
				float3 temp_output_394_103_g2828 = appendResult94_g2875;
				float temp_output_61_0_g2875 = ( 1.0 - (FogRes185_g2875).w );
				float temp_output_58_0_g2828 = ( 1.0 - temp_output_61_0_g2875 );
				float4 appendResult44_g2828 = (float4(temp_output_394_103_g2828 , temp_output_58_0_g2828));
				float4 ifLocalVar49_g2828 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g2828 >= 1.0 )
				ifLocalVar49_g2828 = appendResult44_g2828;
				else
				ifLocalVar49_g2828 = appendResult47_g2828;
				#ifdef _USEUNDERWATER
				float4 staticSwitch215_g2828 = max( ifLocalVar49_g2828 , float4( 0,0,0,0 ) );
				#else
				float4 staticSwitch215_g2828 = appendResult47_g2828;
				#endif
				float4 temp_output_1_0_g2903 = staticSwitch215_g2828;
				float DepthMask5_g2903 = (temp_output_1_0_g2903).w;
				float temp_output_27_0_g2903 = ( 1.0 - DepthMask5_g2903 );
				#ifdef _USEUNDERWATER
				float staticSwitch38_g2903 = ( temp_output_27_0_g2903 * temp_output_31_0_g2903 );
				#else
				float staticSwitch38_g2903 = temp_output_31_0_g2903;
				#endif
				float temp_output_126_25 = staticSwitch38_g2903;
				

				surfaceDescription.Alpha = temp_output_126_25;
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				return outColor;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "ScenePickingPass"
			Tags { "LightMode"="Picking" }

			AlphaToMask Off

			HLSLPROGRAM

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ASE_SRP_VERSION 120115
            #define UNDERWATERELSEWHERE
            #define TONEMAPPINGELSEWHERE
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"


            #pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT

			#define SHADERPASS SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature GLOBALTONEMAPPING


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _TintColor;
			float4 _Main_ST;
			float2 _FlowDirection;
			float2 _FlowStrengh;
			float _Culling;
			float _TextureIntensity;
			float _Size;
			float _FlowSpeed;
			float _WaveContrast;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			sampler2D _Caustics;
			sampler2D _Main;
			sampler2D _Flowmap;


			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			inline float4 GetUnderWaterFogs240_g2875( float3 viewDir, float3 camWorldPos, float3 posWS, float4 oceanFogDensities, float oceanHeight, float4 oceanFogTop_RGB_Exponent, float4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			

			float4 _SelectionID;

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				o.ase_texcoord1.xyz = ase_worldPos;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				o.ase_texcoord1.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );
				o.positionCS = TransformWorldToHClip(positionWS);
				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float2 uv_Main = IN.ase_texcoord.xy * _Main_ST.xy + _Main_ST.zw;
				float2 temp_output_4_0_g1 = (( uv_Main / _Size )).xy;
				float2 temp_output_17_0_g1 = _FlowStrengh;
				float mulTime22_g1 = _TimeParameters.x * _FlowSpeed;
				float temp_output_27_0_g1 = frac( mulTime22_g1 );
				float2 temp_output_11_0_g1 = ( temp_output_4_0_g1 + ( -(_FlowDirection*2.0 + -1.0) * temp_output_17_0_g1 * temp_output_27_0_g1 ) );
				float temp_output_55_0_g1 = 1.0;
				float3 unpack48_g1 = UnpackNormalScale( tex2D( _Flowmap, temp_output_11_0_g1 ), temp_output_55_0_g1 );
				unpack48_g1.z = lerp( 1, unpack48_g1.z, saturate(temp_output_55_0_g1) );
				float2 temp_output_12_0_g1 = ( temp_output_4_0_g1 + ( -(_FlowDirection*2.0 + -1.0) * temp_output_17_0_g1 * frac( ( mulTime22_g1 + 0.5 ) ) ) );
				float3 unpack49_g1 = UnpackNormalScale( tex2D( _Flowmap, temp_output_12_0_g1 ), temp_output_55_0_g1 );
				unpack49_g1.z = lerp( 1, unpack49_g1.z, saturate(temp_output_55_0_g1) );
				float3 lerpResult9_g1 = lerp( unpack48_g1 , unpack49_g1 , ( abs( ( temp_output_27_0_g1 - 0.5 ) ) / 0.5 ));
				float4 tex2DNode5 = tex2D( _Main, lerpResult9_g1.xy );
				float temp_output_31_0_g2903 = tex2DNode5.a;
				float4 _Vector3 = float4(1,1,1,0);
				float4 appendResult47_g2828 = (float4(_Vector3));
				float OceanUnder289_g2828 = GlobalOceanUnder;
				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float3 WorldPosition256_g2828 = ase_worldPos;
				float3 WorldPos252_g2875 = WorldPosition256_g2828;
				float temp_output_67_0_g2828 = ( GlobalOceanOffset + GlobalOceanHeight );
				float OceanHeight274_g2828 = temp_output_67_0_g2828;
				float temp_output_108_0_g2875 = OceanHeight274_g2828;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ViewDir264_g2828 = ase_worldViewDir;
				float3 viewDir240_g2875 = ViewDir264_g2828;
				float3 camWorldPos240_g2875 = _WorldSpaceCameraPos;
				float3 posWS240_g2875 = WorldPos252_g2875;
				float4 oceanFogDensities240_g2875 = OceanFogDensities;
				float oceanHeight240_g2875 = temp_output_108_0_g2875;
				float4 oceanFogTop_RGB_Exponent240_g2875 = OceanFogTop_RGB_Exponent;
				float4 oceanFogBottom_RGB_Intensity240_g2875 = OceanFogBottom_RGB_Intensity;
				float4 localGetUnderWaterFogs240_g2875 = GetUnderWaterFogs240_g2875( viewDir240_g2875 , camWorldPos240_g2875 , posWS240_g2875 , oceanFogDensities240_g2875 , oceanHeight240_g2875 , oceanFogTop_RGB_Exponent240_g2875 , oceanFogBottom_RGB_Intensity240_g2875 );
				float4 ifLocalVar257_g2875 = 0;
				UNITY_BRANCH 
				if( (WorldPos252_g2875).y < ( temp_output_108_0_g2875 + 0.2 ) )
				ifLocalVar257_g2875 = localGetUnderWaterFogs240_g2875;
				float4 FogRes185_g2875 = ifLocalVar257_g2875;
				float3 appendResult94_g2875 = (float3(FogRes185_g2875.xyz));
				float3 temp_output_394_103_g2828 = appendResult94_g2875;
				float temp_output_61_0_g2875 = ( 1.0 - (FogRes185_g2875).w );
				float temp_output_58_0_g2828 = ( 1.0 - temp_output_61_0_g2875 );
				float4 appendResult44_g2828 = (float4(temp_output_394_103_g2828 , temp_output_58_0_g2828));
				float4 ifLocalVar49_g2828 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g2828 >= 1.0 )
				ifLocalVar49_g2828 = appendResult44_g2828;
				else
				ifLocalVar49_g2828 = appendResult47_g2828;
				#ifdef _USEUNDERWATER
				float4 staticSwitch215_g2828 = max( ifLocalVar49_g2828 , float4( 0,0,0,0 ) );
				#else
				float4 staticSwitch215_g2828 = appendResult47_g2828;
				#endif
				float4 temp_output_1_0_g2903 = staticSwitch215_g2828;
				float DepthMask5_g2903 = (temp_output_1_0_g2903).w;
				float temp_output_27_0_g2903 = ( 1.0 - DepthMask5_g2903 );
				#ifdef _USEUNDERWATER
				float staticSwitch38_g2903 = ( temp_output_27_0_g2903 * temp_output_31_0_g2903 );
				#else
				float staticSwitch38_g2903 = temp_output_31_0_g2903;
				#endif
				float temp_output_126_25 = staticSwitch38_g2903;
				

				surfaceDescription.Alpha = temp_output_126_25;
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					float alphaClipThreshold = 0.01f;
					#if ALPHA_CLIP_THRESHOLD
						alphaClipThreshold = surfaceDescription.AlphaClipThreshold;
					#endif
					clip(surfaceDescription.Alpha - alphaClipThreshold);
				#endif

				half4 outColor = 0;
				outColor = _SelectionID;

				return outColor;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthNormals"
			Tags { "LightMode"="DepthNormalsOnly" }

			ZTest LEqual
			ZWrite On

			HLSLPROGRAM

            #define _SURFACE_TYPE_TRANSPARENT 1
            #pragma multi_compile_instancing
            #define ASE_SRP_VERSION 120115
            #define UNDERWATERELSEWHERE
            #define TONEMAPPINGELSEWHERE
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"


            #pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex vert
			#pragma fragment frag

			#define ATTRIBUTES_NEED_NORMAL
			#define ATTRIBUTES_NEED_TANGENT
			#define VARYINGS_NEED_NORMAL_WS

			#define SHADERPASS SHADERPASS_DEPTHNORMALSONLY

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#pragma multi_compile __ _USEUNDERWATER
			#pragma shader_feature GLOBALTONEMAPPING


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float3 normalWS : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _TintColor;
			float4 _Main_ST;
			float2 _FlowDirection;
			float2 _FlowStrengh;
			float _Culling;
			float _TextureIntensity;
			float _Size;
			float _FlowSpeed;
			float _WaveContrast;
			#ifdef ASE_TESSELLATION
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			half _CausticsScale;
			half2 _CausticsPanSpeed;
			half4 _CausticsColor;
			sampler2D _Caustics;
			sampler2D _Main;
			sampler2D _Flowmap;


			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
			}
			
			inline float4 GetUnderWaterFogs240_g2875( float3 viewDir, float3 camWorldPos, float3 posWS, float4 oceanFogDensities, float oceanHeight, float4 oceanFogTop_RGB_Exponent, float4 oceanFogBottom_RGB_Intensity )
			{
				return GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity );;
			}
			

			struct SurfaceDescription
			{
				float Alpha;
				float AlphaClipThreshold;
			};

			VertexOutput VertexFunction(VertexInput v  )
			{
				VertexOutput o;
				ZERO_INITIALIZE(VertexOutput, o);

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldPos = TransformObjectToWorld( (v.positionOS).xyz );
				o.ase_texcoord2.xyz = ase_worldPos;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				o.ase_texcoord2.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				v.normalOS = v.normalOS;

				float3 positionWS = TransformObjectToWorld( v.positionOS.xyz );
				float3 normalWS = TransformObjectToWorldNormal(v.normalOS);

				o.positionCS = TransformWorldToHClip(positionWS);
				o.normalWS.xyz =  normalWS;

				return o;
			}

			#if defined(ASE_TESSELLATION)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 normalOS : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.positionOS;
				o.normalOS = v.normalOS;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
				return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.positionOS = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.normalOS = patch[0].normalOS * bary.x + patch[1].normalOS * bary.y + patch[2].normalOS * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.positionOS.xyz - patch[i].normalOS * (dot(o.positionOS.xyz, patch[i].normalOS) - dot(patch[i].vertex.xyz, patch[i].normalOS));
				float phongStrength = _TessPhongStrength;
				o.positionOS.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.positionOS.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float2 uv_Main = IN.ase_texcoord1.xy * _Main_ST.xy + _Main_ST.zw;
				float2 temp_output_4_0_g1 = (( uv_Main / _Size )).xy;
				float2 temp_output_17_0_g1 = _FlowStrengh;
				float mulTime22_g1 = _TimeParameters.x * _FlowSpeed;
				float temp_output_27_0_g1 = frac( mulTime22_g1 );
				float2 temp_output_11_0_g1 = ( temp_output_4_0_g1 + ( -(_FlowDirection*2.0 + -1.0) * temp_output_17_0_g1 * temp_output_27_0_g1 ) );
				float temp_output_55_0_g1 = 1.0;
				float3 unpack48_g1 = UnpackNormalScale( tex2D( _Flowmap, temp_output_11_0_g1 ), temp_output_55_0_g1 );
				unpack48_g1.z = lerp( 1, unpack48_g1.z, saturate(temp_output_55_0_g1) );
				float2 temp_output_12_0_g1 = ( temp_output_4_0_g1 + ( -(_FlowDirection*2.0 + -1.0) * temp_output_17_0_g1 * frac( ( mulTime22_g1 + 0.5 ) ) ) );
				float3 unpack49_g1 = UnpackNormalScale( tex2D( _Flowmap, temp_output_12_0_g1 ), temp_output_55_0_g1 );
				unpack49_g1.z = lerp( 1, unpack49_g1.z, saturate(temp_output_55_0_g1) );
				float3 lerpResult9_g1 = lerp( unpack48_g1 , unpack49_g1 , ( abs( ( temp_output_27_0_g1 - 0.5 ) ) / 0.5 ));
				float4 tex2DNode5 = tex2D( _Main, lerpResult9_g1.xy );
				float temp_output_31_0_g2903 = tex2DNode5.a;
				float4 _Vector3 = float4(1,1,1,0);
				float4 appendResult47_g2828 = (float4(_Vector3));
				float OceanUnder289_g2828 = GlobalOceanUnder;
				float3 ase_worldPos = IN.ase_texcoord2.xyz;
				float3 WorldPosition256_g2828 = ase_worldPos;
				float3 WorldPos252_g2875 = WorldPosition256_g2828;
				float temp_output_67_0_g2828 = ( GlobalOceanOffset + GlobalOceanHeight );
				float OceanHeight274_g2828 = temp_output_67_0_g2828;
				float temp_output_108_0_g2875 = OceanHeight274_g2828;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - ase_worldPos );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ViewDir264_g2828 = ase_worldViewDir;
				float3 viewDir240_g2875 = ViewDir264_g2828;
				float3 camWorldPos240_g2875 = _WorldSpaceCameraPos;
				float3 posWS240_g2875 = WorldPos252_g2875;
				float4 oceanFogDensities240_g2875 = OceanFogDensities;
				float oceanHeight240_g2875 = temp_output_108_0_g2875;
				float4 oceanFogTop_RGB_Exponent240_g2875 = OceanFogTop_RGB_Exponent;
				float4 oceanFogBottom_RGB_Intensity240_g2875 = OceanFogBottom_RGB_Intensity;
				float4 localGetUnderWaterFogs240_g2875 = GetUnderWaterFogs240_g2875( viewDir240_g2875 , camWorldPos240_g2875 , posWS240_g2875 , oceanFogDensities240_g2875 , oceanHeight240_g2875 , oceanFogTop_RGB_Exponent240_g2875 , oceanFogBottom_RGB_Intensity240_g2875 );
				float4 ifLocalVar257_g2875 = 0;
				UNITY_BRANCH 
				if( (WorldPos252_g2875).y < ( temp_output_108_0_g2875 + 0.2 ) )
				ifLocalVar257_g2875 = localGetUnderWaterFogs240_g2875;
				float4 FogRes185_g2875 = ifLocalVar257_g2875;
				float3 appendResult94_g2875 = (float3(FogRes185_g2875.xyz));
				float3 temp_output_394_103_g2828 = appendResult94_g2875;
				float temp_output_61_0_g2875 = ( 1.0 - (FogRes185_g2875).w );
				float temp_output_58_0_g2828 = ( 1.0 - temp_output_61_0_g2875 );
				float4 appendResult44_g2828 = (float4(temp_output_394_103_g2828 , temp_output_58_0_g2828));
				float4 ifLocalVar49_g2828 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g2828 >= 1.0 )
				ifLocalVar49_g2828 = appendResult44_g2828;
				else
				ifLocalVar49_g2828 = appendResult47_g2828;
				#ifdef _USEUNDERWATER
				float4 staticSwitch215_g2828 = max( ifLocalVar49_g2828 , float4( 0,0,0,0 ) );
				#else
				float4 staticSwitch215_g2828 = appendResult47_g2828;
				#endif
				float4 temp_output_1_0_g2903 = staticSwitch215_g2828;
				float DepthMask5_g2903 = (temp_output_1_0_g2903).w;
				float temp_output_27_0_g2903 = ( 1.0 - DepthMask5_g2903 );
				#ifdef _USEUNDERWATER
				float staticSwitch38_g2903 = ( temp_output_27_0_g2903 * temp_output_31_0_g2903 );
				#else
				float staticSwitch38_g2903 = temp_output_31_0_g2903;
				#endif
				float temp_output_126_25 = staticSwitch38_g2903;
				

				surfaceDescription.Alpha = temp_output_126_25;
				surfaceDescription.AlphaClipThreshold = 0.5;

				#if _ALPHATEST_ON
					clip(surfaceDescription.Alpha - surfaceDescription.AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.positionCS.xyz, unity_LODFade.x );
				#endif

				float3 normalWS = IN.normalWS;

				return half4(NormalizeNormalPerPixel(normalWS), 0.0);
			}

			ENDHLSL
		}

	
	}
	
	CustomEditor "UnityEditor.ShaderGraphUnlitGUI"
	FallBack "Hidden/Shader Graph/FallbackError"
	
	Fallback "Hidden/InternalErrorShader"
}
/*ASEBEGIN
Version=19303
Node;AmplifyShaderEditor.TextureCoordinatesNode;60;-2764.73,-426.0533;Inherit;False;0;5;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;56;-1896.171,-503.0029;Inherit;False;Property;_FlowDirection;Flow Direction;38;0;Create;True;0;0;0;False;0;False;0,0;0.2,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;55;-1845.771,-204.4028;Inherit;False;Property;_FlowSpeed;Flow Speed;37;0;Create;True;0;0;0;False;0;False;1;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;57;-1902.953,-362.3585;Inherit;False;Property;_FlowStrengh;Flow Strengh;41;0;Create;True;0;0;0;False;0;False;0,0;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;54;-2146.928,-608.3773;Inherit;True;Property;_Flowmap;Flowmap;35;0;Create;True;0;0;0;False;0;False;None;f372519b7b406a94fb65839d124b9bec;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.FunctionNode;52;-1632.605,-478.2581;Inherit;True;Flow;32;;1;acad10cc8145e1f4eb8042bebe2d9a42;2,50,1,51,1;6;5;SAMPLER2D;;False;2;FLOAT2;0,0;False;55;FLOAT;1;False;18;FLOAT2;0,0;False;17;FLOAT2;1,1;False;24;FLOAT;0.2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1234.035,-803.4031;Inherit;True;Property;_TextureIntensity;Texture Intensity;36;0;Create;True;0;0;0;False;0;False;10;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;11;-355.682,-247.8776;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-1283.197,-521.7454;Inherit;True;Property;_Main;Main;31;0;Create;True;0;0;0;False;0;False;-1;None;adc3518309a403f4cb98809a2761e11f;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-922.8682,-783.0052;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-894.7554,-605.6954;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;42;-79.17439,-341.5371;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;20;-641.5279,-520.4891;Inherit;True;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;198.3704,-406.7085;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;64;658.753,43.63026;Inherit;False;Property;_WaveContrast;Wave Contrast;40;0;Create;True;0;0;0;False;0;False;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;162.4011,-84.93375;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;58;620.8925,-671.4954;Inherit;False;Property;_TintColor;Tint Color;39;0;Create;True;0;0;0;False;0;False;1,1,1,0;0.2066591,0,0.5377358,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;63;992.5538,-72.02232;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;886.8667,-545.061;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;1204.702,-175.5024;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;61;1449.882,-249.0436;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;51;1606.496,-347.529;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;99;-194,-2674;Inherit;False;935.606;1608.705;Comment;12;87;88;79;82;80;81;77;86;85;78;84;83;;1,1,1,1;0;0
Node;AmplifyShaderEditor.FunctionNode;98;1376,-1648;Inherit;False;CausticsAndFog;4;;2828;faa94743a70a11f47a63fa5c9cf350f2;16,175,0,230,0,246,0,236,0,216,0,162,0,370,0,237,0,387,1,384,1,388,1,15,0,100,0,171,0,168,1,13,0;10;253;FLOAT3;0,0,0;False;263;FLOAT3;0,0,0;False;245;FLOAT;0;False;67;FLOAT;0;False;195;FLOAT3;0,0,0;False;23;FLOAT;0;False;11;FLOAT3;1,1,1;False;10;FLOAT3;0,0,0;False;9;FLOAT3;0,0,1;False;57;FLOAT;0;False;6;FLOAT3;266;FLOAT3;0;FLOAT3;8;FLOAT4;25;FLOAT;243;FLOAT3;167
Node;AmplifyShaderEditor.RangedFloatNode;71;1683.612,-197.7993;Inherit;False;Property;_Culling;Culling;0;1;[Header];Create;True;1;Culling Mode;0;0;True;0;False;2;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-144,-1984;Inherit;False;Global;GlobalOceanOffset;GlobalOceanOffset;9;0;Fetch;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-144,-1904;Inherit;False;Global;GlobalOceanHeight;GlobalOceanHeight;84;0;Fetch;True;0;0;0;False;0;False;0;-5.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;78;-16,-2464;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;85;144,-1952;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;86;48,-2320;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;77;48,-2624;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector4Node;81;-32,-1616;Inherit;False;Global;OceanFogBottom_RGB_Intensity;OceanFogBottom_RGB_Intensity;1;0;Fetch;True;0;0;0;False;0;False;0,0,0,0.7;0,0.03348188,0.04790472,0.7;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;80;0,-1792;Inherit;False;Global;OceanFogTop_RGB_Exponent;OceanFogTop_RGB_Exponent;1;0;Fetch;True;0;0;0;False;0;False;0,0,0,8;0.03331205,0.1193296,0.1129476,8;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;82;32,-2176;Inherit;False;Global;OceanFogDensities;OceanFogDensities;1;0;Fetch;True;0;0;0;False;0;False;0.2,0.02,0.1,0;0,1E-05,0.2,-0.36;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CustomExpressionNode;79;352,-2160;Inherit;False;GetUnderWaterFog( viewDir, camWorldPos, posWS, oceanFogDensities, oceanHeight, oceanFogTop_RGB_Exponent, oceanFogBottom_RGB_Intensity )@;4;Create;7;True;viewDir;FLOAT3;0,0,0;In;;Inherit;False;True;camWorldPos;FLOAT3;0,0,0;In;;Float;False;True;posWS;FLOAT3;0,0,0;In;;Inherit;False;True;oceanFogDensities;FLOAT4;0,0,0,0;In;;Inherit;False;True;oceanHeight;FLOAT;0;In;;Inherit;False;True;oceanFogTop_RGB_Exponent;FLOAT4;0,0,0,0;In;;Inherit;False;True;oceanFogBottom_RGB_Intensity;FLOAT4;0,0,0,0;In;;Inherit;False;GetUnderWaterFogs;True;False;0;;False;7;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;88;336,-1360;Inherit;False;FLOAT3;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector4Node;87;64,-1360;Inherit;False;Global;OceanWaterTint_RGB;OceanWaterTint_RGB;1;0;Fetch;True;0;0;0;False;0;False;1,1,1,0;0.1221388,0.2509687,0.3139888,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;2080,-896;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;106;1936,-1616;Inherit;False;TonemappingNode;1;;2886;305ac1d3fd2773448b8bd3815ea5531e;1,6,1;2;3;FLOAT3;0,0,0;False;4;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;126;1680,-1584;Inherit;False;ApplyFogAndTint;-1;;2903;6bb817da436f9a84da1721b331a80851;5,32,1,16,0,35,1,45,1,39,1;5;14;FLOAT3;0,0,0;False;17;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT3;1,1,1;False;31;FLOAT;1;False;2;FLOAT3;0;FLOAT;25
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;30;1307.217,-167.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;31;1307.217,-167.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;28;969.3797,-318.3227;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;32;1307.217,-167.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;65;1773.591,-248.7692;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;66;1773.591,-248.7692;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;SceneSelectionPass;0;6;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;67;1773.591,-248.7692;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ScenePickingPass;0;7;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;68;1773.591,-248.7692;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormals;0;8;DepthNormals;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=DepthNormalsOnly;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;69;1773.591,-248.7692;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormalsOnly;0;9;DepthNormalsOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=DepthNormalsOnly;False;True;9;d3d11;metal;vulkan;xboxone;xboxseries;playstation;ps4;ps5;switch;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;29;2192,-1584;Float;False;True;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;ThunderRoad/VFX/vfx_fake_bloom_additive_flowmap;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;True;True;0;True;_Culling;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;UniversalMaterialType=Unlit;True;3;True;12;all;0;True;True;5;4;False;;1;False;;1;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForward;False;False;5;Define;UNDERWATERELSEWHERE;False;;Custom;False;0;0;;Define;TONEMAPPINGELSEWHERE;False;;Custom;False;0;0;;Custom;#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc";False;;Custom;False;0;0;;Custom;#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc";False;;Custom;False;0;0;;Include;;False;;Native;False;0;0;;Hidden/InternalErrorShader;0;0;Standard;21;Surface;1;0;  Blend;0;0;Two Sided;1;0;Forward Only;0;0;Cast Shadows;0;638319125846057491;  Use Shadow Threshold;0;0;GPU Instancing;1;0;LOD CrossFade;0;0;Built-in Fog;0;638531017162387671;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position,InvertActionOnDeselection;1;0;0;10;False;True;False;True;False;False;True;True;True;False;False;;False;0
WireConnection;52;5;54;0
WireConnection;52;2;60;0
WireConnection;52;18;56;0
WireConnection;52;17;57;0
WireConnection;52;24;55;0
WireConnection;5;1;52;0
WireConnection;50;0;11;4
WireConnection;50;1;24;0
WireConnection;23;0;50;0
WireConnection;23;1;5;0
WireConnection;42;0;11;1
WireConnection;42;1;11;2
WireConnection;42;2;11;3
WireConnection;20;0;23;0
WireConnection;22;0;42;0
WireConnection;22;1;20;0
WireConnection;38;0;22;0
WireConnection;38;1;5;4
WireConnection;63;1;38;0
WireConnection;63;0;64;0
WireConnection;59;0;58;0
WireConnection;59;1;38;0
WireConnection;62;0;59;0
WireConnection;62;1;63;0
WireConnection;61;0;59;0
WireConnection;61;1;62;0
WireConnection;51;0;61;0
WireConnection;85;0;83;0
WireConnection;85;1;84;0
WireConnection;79;0;77;0
WireConnection;79;1;78;0
WireConnection;79;2;86;0
WireConnection;79;3;82;0
WireConnection;79;4;85;0
WireConnection;79;5;80;0
WireConnection;79;6;81;0
WireConnection;88;0;87;0
WireConnection;93;0;126;25
WireConnection;93;1;51;0
WireConnection;106;3;126;0
WireConnection;126;14;51;0
WireConnection;126;1;98;25
WireConnection;126;2;98;167
WireConnection;126;31;5;4
WireConnection;29;2;106;0
WireConnection;29;3;126;25
ASEEND*/
//CHKSM=99777FBA1AAED8988CDBADF723EEB88492F3826A