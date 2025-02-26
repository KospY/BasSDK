// Made with Amplify Shader Editor v1.9.3.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ThunderRoad/VFX/vfx_fake_bloom_additive_MOBILE"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		_Main("Main", 2D) = "black" {}
		_Bloom("Bloom", 2D) = "black" {}
		_BloomIntensity("Bloom Intensity", Float) = 0
		[Feature(_CAUSTICSENABLE)]HeaderOceanFog("# Ocean Fog Caustics", Float) = 0
		_TextureIntensity("Texture Intensity", Float) = 10
		[HideInInspector] _texcoord( "", 2D ) = "white" {}


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

		Cull Back
		AlphaToMask Off

		

		HLSLINCLUDE
		#pragma target 4.5
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

			Blend One One, SrcAlpha One
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			

			HLSLPROGRAM

			#define _SURFACE_TYPE_TRANSPARENT 1
			#pragma multi_compile_instancing
			#pragma instancing_options renderinglayer
			#define ASE_SRP_VERSION 120115
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
			#define TONEMAPPINGELSEWHERE
			#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"


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
			float4 _Main_ST;
			float4 _Bloom_ST;
			float _TextureIntensity;
			float _BloomIntensity;
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
			sampler2D _Bloom;


			float calcFogFactor( float distance, float gradientFogDensity )
			{
					//beer-lambert law, Fog =1/e^(distance * density)
					float e = 2.7182818284590452353602874713527f;
					return 1.0 - saturate(1.0f / pow(e, (distance * gradientFogDensity)));
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

				float4 appendResult11_g2882 = (float4(_TonemappingSettings.x , _TonemappingSettings.y , _TonemappingSettings.z , _TonemappingSettings.w));
				float4 temp_output_4_0_g2881 = appendResult11_g2882;
				float4 settings7_g2881 = temp_output_4_0_g2881;
				float4 appendResult42 = (float4(IN.ase_color.r , IN.ase_color.g , IN.ase_color.b , 0.0));
				float2 uv_Main = IN.ase_texcoord3.xy * _Main_ST.xy + _Main_ST.zw;
				float4 tex2DNode5 = tex2D( _Main, uv_Main );
				float2 uv_Bloom = IN.ase_texcoord3.xy * _Bloom_ST.xy + _Bloom_ST.zw;
				float4 tex2DNode6 = tex2D( _Bloom, uv_Bloom );
				float4 temp_output_8_0 = ( ( ( appendResult42 * float4( (( ( IN.ase_color.a * _TextureIntensity ) * tex2DNode5 )).rgb , 0.0 ) ) * tex2DNode5.a ) + ( ( ( appendResult42 * tex2DNode6 ) * ( tex2DNode6.a * _BloomIntensity ) ) * IN.ase_color.a ) );
				float3 temp_output_14_0_g2908 = temp_output_8_0.xyz;
				float3 desaturateInitialColor46_g2908 = temp_output_14_0_g2908;
				float desaturateDot46_g2908 = dot( desaturateInitialColor46_g2908, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar46_g2908 = lerp( desaturateInitialColor46_g2908, desaturateDot46_g2908.xxx, 0.2 );
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
				float4 temp_output_1_0_g2908 = staticSwitch215_g2828;
				float3 FogRGB6_g2908 = (temp_output_1_0_g2908).xyz;
				float3 temp_cast_4 = (1.0).xxx;
				float3 appendResult100_g2875 = (float3(OceanWaterTint_RGB.xyz));
				float3 temp_cast_6 = (1.0).xxx;
				float3 ifLocalVar170_g2828 = 0;
				UNITY_BRANCH 
				if( OceanUnder289_g2828 >= 1.0 )
				ifLocalVar170_g2828 = appendResult100_g2875;
				else
				ifLocalVar170_g2828 = temp_cast_6;
				#ifdef _USEUNDERWATER
				float3 staticSwitch212_g2828 = ifLocalVar170_g2828;
				#else
				float3 staticSwitch212_g2828 = temp_cast_4;
				#endif
				float3 temp_output_2_0_g2908 = staticSwitch212_g2828;
				float3 hsvTorgb48_g2908 = RGBToHSV( ( FogRGB6_g2908 * temp_output_2_0_g2908 ) );
				float clampResult51_g2908 = clamp( hsvTorgb48_g2908.y , 0.0 , 0.8 );
				float3 hsvTorgb49_g2908 = HSVToRGB( float3(hsvTorgb48_g2908.x,clampResult51_g2908,1.0) );
				float3 temp_output_42_0_g2908 = saturate( hsvTorgb49_g2908 );
				float DepthMask5_g2908 = (temp_output_1_0_g2908).w;
				float3 lerpResult7_g2908 = lerp( ( desaturateVar46_g2908 * temp_output_42_0_g2908 ) , float3(0,0,0) , DepthMask5_g2908);
				float grayscale69 = (temp_output_8_0.xyz.r + temp_output_8_0.xyz.g + temp_output_8_0.xyz.b) / 3;
				float temp_output_31_0_g2908 = grayscale69;
				float3 lerpResult34_g2908 = lerp( float3( 0,0,0 ) , lerpResult7_g2908 , temp_output_31_0_g2908);
				float temp_output_27_0_g2908 = ( 1.0 - DepthMask5_g2908 );
				#ifdef _USEUNDERWATER
				float3 staticSwitch37_g2908 = ( lerpResult34_g2908 * temp_output_27_0_g2908 );
				#else
				float3 staticSwitch37_g2908 = temp_output_14_0_g2908;
				#endif
				float3 temp_output_3_0_g2881 = staticSwitch37_g2908;
				float3 color7_g2881 = temp_output_3_0_g2881;
				float3 localApplyTonemapper7_g2881 = ApplyTonemapper( settings7_g2881 , color7_g2881 );
				
				#ifdef _USEUNDERWATER
				float staticSwitch38_g2908 = ( temp_output_27_0_g2908 * temp_output_31_0_g2908 );
				#else
				float staticSwitch38_g2908 = temp_output_31_0_g2908;
				#endif
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = localApplyTonemapper7_g2881;
				float Alpha = staticSwitch38_g2908;
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
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
            #define TONEMAPPINGELSEWHERE
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"


            #pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

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
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Main_ST;
			float4 _Bloom_ST;
			float _TextureIntensity;
			float _BloomIntensity;
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
			sampler2D _Bloom;


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

				o.ase_color = v.ase_color;
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

				float4 appendResult42 = (float4(IN.ase_color.r , IN.ase_color.g , IN.ase_color.b , 0.0));
				float2 uv_Main = IN.ase_texcoord2.xy * _Main_ST.xy + _Main_ST.zw;
				float4 tex2DNode5 = tex2D( _Main, uv_Main );
				float2 uv_Bloom = IN.ase_texcoord2.xy * _Bloom_ST.xy + _Bloom_ST.zw;
				float4 tex2DNode6 = tex2D( _Bloom, uv_Bloom );
				float4 temp_output_8_0 = ( ( ( appendResult42 * float4( (( ( IN.ase_color.a * _TextureIntensity ) * tex2DNode5 )).rgb , 0.0 ) ) * tex2DNode5.a ) + ( ( ( appendResult42 * tex2DNode6 ) * ( tex2DNode6.a * _BloomIntensity ) ) * IN.ase_color.a ) );
				float grayscale69 = (temp_output_8_0.xyz.r + temp_output_8_0.xyz.g + temp_output_8_0.xyz.b) / 3;
				float temp_output_31_0_g2908 = grayscale69;
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
				float4 temp_output_1_0_g2908 = staticSwitch215_g2828;
				float DepthMask5_g2908 = (temp_output_1_0_g2908).w;
				float temp_output_27_0_g2908 = ( 1.0 - DepthMask5_g2908 );
				#ifdef _USEUNDERWATER
				float staticSwitch38_g2908 = ( temp_output_27_0_g2908 * temp_output_31_0_g2908 );
				#else
				float staticSwitch38_g2908 = temp_output_31_0_g2908;
				#endif
				

				float Alpha = staticSwitch38_g2908;
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
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
            #define TONEMAPPINGELSEWHERE
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"


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

			#define ASE_NEEDS_FRAG_COLOR
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
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Main_ST;
			float4 _Bloom_ST;
			float _TextureIntensity;
			float _BloomIntensity;
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
			sampler2D _Bloom;


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
				
				o.ase_color = v.ase_color;
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

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float4 appendResult42 = (float4(IN.ase_color.r , IN.ase_color.g , IN.ase_color.b , 0.0));
				float2 uv_Main = IN.ase_texcoord.xy * _Main_ST.xy + _Main_ST.zw;
				float4 tex2DNode5 = tex2D( _Main, uv_Main );
				float2 uv_Bloom = IN.ase_texcoord.xy * _Bloom_ST.xy + _Bloom_ST.zw;
				float4 tex2DNode6 = tex2D( _Bloom, uv_Bloom );
				float4 temp_output_8_0 = ( ( ( appendResult42 * float4( (( ( IN.ase_color.a * _TextureIntensity ) * tex2DNode5 )).rgb , 0.0 ) ) * tex2DNode5.a ) + ( ( ( appendResult42 * tex2DNode6 ) * ( tex2DNode6.a * _BloomIntensity ) ) * IN.ase_color.a ) );
				float grayscale69 = (temp_output_8_0.xyz.r + temp_output_8_0.xyz.g + temp_output_8_0.xyz.b) / 3;
				float temp_output_31_0_g2908 = grayscale69;
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
				float4 temp_output_1_0_g2908 = staticSwitch215_g2828;
				float DepthMask5_g2908 = (temp_output_1_0_g2908).w;
				float temp_output_27_0_g2908 = ( 1.0 - DepthMask5_g2908 );
				#ifdef _USEUNDERWATER
				float staticSwitch38_g2908 = ( temp_output_27_0_g2908 * temp_output_31_0_g2908 );
				#else
				float staticSwitch38_g2908 = temp_output_31_0_g2908;
				#endif
				

				surfaceDescription.Alpha = staticSwitch38_g2908;
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
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
            #define TONEMAPPINGELSEWHERE
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"


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

			#define ASE_NEEDS_FRAG_COLOR
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
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Main_ST;
			float4 _Bloom_ST;
			float _TextureIntensity;
			float _BloomIntensity;
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
			sampler2D _Bloom;


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
				
				o.ase_color = v.ase_color;
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

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float4 appendResult42 = (float4(IN.ase_color.r , IN.ase_color.g , IN.ase_color.b , 0.0));
				float2 uv_Main = IN.ase_texcoord.xy * _Main_ST.xy + _Main_ST.zw;
				float4 tex2DNode5 = tex2D( _Main, uv_Main );
				float2 uv_Bloom = IN.ase_texcoord.xy * _Bloom_ST.xy + _Bloom_ST.zw;
				float4 tex2DNode6 = tex2D( _Bloom, uv_Bloom );
				float4 temp_output_8_0 = ( ( ( appendResult42 * float4( (( ( IN.ase_color.a * _TextureIntensity ) * tex2DNode5 )).rgb , 0.0 ) ) * tex2DNode5.a ) + ( ( ( appendResult42 * tex2DNode6 ) * ( tex2DNode6.a * _BloomIntensity ) ) * IN.ase_color.a ) );
				float grayscale69 = (temp_output_8_0.xyz.r + temp_output_8_0.xyz.g + temp_output_8_0.xyz.b) / 3;
				float temp_output_31_0_g2908 = grayscale69;
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
				float4 temp_output_1_0_g2908 = staticSwitch215_g2828;
				float DepthMask5_g2908 = (temp_output_1_0_g2908).w;
				float temp_output_27_0_g2908 = ( 1.0 - DepthMask5_g2908 );
				#ifdef _USEUNDERWATER
				float staticSwitch38_g2908 = ( temp_output_27_0_g2908 * temp_output_31_0_g2908 );
				#else
				float staticSwitch38_g2908 = temp_output_31_0_g2908;
				#endif
				

				surfaceDescription.Alpha = staticSwitch38_g2908;
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
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc"
            #define TONEMAPPINGELSEWHERE
            #include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc"


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

			#define ASE_NEEDS_FRAG_COLOR
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
				float3 normalWS : TEXCOORD0;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Main_ST;
			float4 _Bloom_ST;
			float _TextureIntensity;
			float _BloomIntensity;
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
			sampler2D _Bloom;


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
				
				o.ase_color = v.ase_color;
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

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				SurfaceDescription surfaceDescription = (SurfaceDescription)0;

				float4 appendResult42 = (float4(IN.ase_color.r , IN.ase_color.g , IN.ase_color.b , 0.0));
				float2 uv_Main = IN.ase_texcoord1.xy * _Main_ST.xy + _Main_ST.zw;
				float4 tex2DNode5 = tex2D( _Main, uv_Main );
				float2 uv_Bloom = IN.ase_texcoord1.xy * _Bloom_ST.xy + _Bloom_ST.zw;
				float4 tex2DNode6 = tex2D( _Bloom, uv_Bloom );
				float4 temp_output_8_0 = ( ( ( appendResult42 * float4( (( ( IN.ase_color.a * _TextureIntensity ) * tex2DNode5 )).rgb , 0.0 ) ) * tex2DNode5.a ) + ( ( ( appendResult42 * tex2DNode6 ) * ( tex2DNode6.a * _BloomIntensity ) ) * IN.ase_color.a ) );
				float grayscale69 = (temp_output_8_0.xyz.r + temp_output_8_0.xyz.g + temp_output_8_0.xyz.b) / 3;
				float temp_output_31_0_g2908 = grayscale69;
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
				float4 temp_output_1_0_g2908 = staticSwitch215_g2828;
				float DepthMask5_g2908 = (temp_output_1_0_g2908).w;
				float temp_output_27_0_g2908 = ( 1.0 - DepthMask5_g2908 );
				#ifdef _USEUNDERWATER
				float staticSwitch38_g2908 = ( temp_output_27_0_g2908 * temp_output_31_0_g2908 );
				#else
				float staticSwitch38_g2908 = temp_output_31_0_g2908;
				#endif
				

				surfaceDescription.Alpha = staticSwitch38_g2908;
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
Node;AmplifyShaderEditor.VertexColorNode;11;-1371.221,-253.7932;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-1591.765,-490.2866;Inherit;False;0;5;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-1234.035,-803.4031;Inherit;True;Property;_TextureIntensity;Texture Intensity;33;0;Create;True;0;0;0;False;0;False;10;77.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-922.8682,-783.0052;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-1283.197,-521.7454;Inherit;True;Property;_Main;Main;0;0;Create;True;0;0;0;False;0;False;-1;None;8f5ee04947e003441921dffbf787fbb1;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-1065.519,493.1418;Inherit;True;Property;_BloomIntensity;Bloom Intensity;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1028.272,58.34963;Inherit;True;Property;_Bloom;Bloom;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;42;-704.2477,-230.6376;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-894.7554,-605.6954;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-535.389,274.8041;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-448.9529,5.99932;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ComponentMaskNode;20;-641.5279,-520.4891;Inherit;True;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;69.94791,129.4711;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VertexColorNode;52;423.9097,318.0675;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;198.3704,-406.7085;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;458.5055,-292.3741;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;764.6016,128.0024;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;909.049,-147.5448;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;60;1344,-576;Inherit;False;CausticsAndFog;6;;2828;faa94743a70a11f47a63fa5c9cf350f2;16,175,0,230,0,246,0,236,0,216,0,162,0,370,0,237,0,387,1,384,1,388,1,15,0,100,0,171,0,168,1,13,0;10;253;FLOAT3;0,0,0;False;263;FLOAT3;0,0,0;False;245;FLOAT;0;False;67;FLOAT;0;False;195;FLOAT3;0,0,0;False;23;FLOAT;0;False;11;FLOAT3;1,1,1;False;10;FLOAT3;0,0,0;False;9;FLOAT3;0,0,1;False;57;FLOAT;0;False;6;FLOAT3;266;FLOAT3;0;FLOAT3;8;FLOAT4;25;FLOAT;243;FLOAT3;167
Node;AmplifyShaderEditor.TFHCGrayscale;69;1232,-112;Inherit;False;2;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-1375.758,84.95694;Inherit;False;0;6;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;59;1984,-528;Inherit;False;TonemappingNode;1;;2881;305ac1d3fd2773448b8bd3815ea5531e;1,6,1;2;3;FLOAT3;0,0,0;False;4;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;98;1696,-496;Inherit;False;ApplyFogAndTint;-1;;2908;6bb817da436f9a84da1721b331a80851;5,32,1,16,0,35,1,45,1,39,1;5;14;FLOAT3;0,0,0;False;17;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT3;1,1,1;False;31;FLOAT;1;False;2;FLOAT3;0;FLOAT;25
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;32;1307.217,-167.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;31;1307.217,-167.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;28;1307.217,-167.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;30;1307.217,-167.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;54;1616.917,-117.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;55;1616.917,-117.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;SceneSelectionPass;0;6;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;56;1616.917,-117.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ScenePickingPass;0;7;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;57;1616.917,-117.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormals;0;8;DepthNormals;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=DepthNormalsOnly;False;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;58;1616.917,-117.8989;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthNormalsOnly;0;9;DepthNormalsOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=DepthNormalsOnly;False;True;9;d3d11;metal;vulkan;xboxone;xboxseries;playstation;ps4;ps5;switch;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;29;2240,-496;Float;False;True;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;13;ThunderRoad/VFX/vfx_fake_bloom_additive_MOBILE;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;UniversalMaterialType=Unlit;True;5;True;12;all;0;True;True;4;1;False;;1;False;;8;5;False;;1;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForward;False;False;4;Custom;#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/TonemappingGear.cginc";False;;Custom;False;0;0;;Define;TONEMAPPINGELSEWHERE;False;;Custom;False;0;0;;Custom;#include_with_pragmas "Assets/Plugins/AmplifyShaderEditor/UnderWaterFog.cginc";False;;Custom;False;0;0;;Include;;False;;Native;False;0;0;;Hidden/InternalErrorShader;0;0;Standard;21;Surface;1;0;  Blend;0;638417669479169489;Two Sided;1;0;Forward Only;0;0;Cast Shadows;0;637788597880831726;  Use Shadow Threshold;0;0;GPU Instancing;1;0;LOD CrossFade;0;0;Built-in Fog;0;0;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position,InvertActionOnDeselection;1;0;0;10;False;True;False;True;False;False;True;True;True;False;False;;False;0
WireConnection;50;0;11;4
WireConnection;50;1;24;0
WireConnection;5;1;25;0
WireConnection;42;0;11;1
WireConnection;42;1;11;2
WireConnection;42;2;11;3
WireConnection;23;0;50;0
WireConnection;23;1;5;0
WireConnection;13;0;6;4
WireConnection;13;1;14;0
WireConnection;10;0;42;0
WireConnection;10;1;6;0
WireConnection;20;0;23;0
WireConnection;40;0;10;0
WireConnection;40;1;13;0
WireConnection;22;0;42;0
WireConnection;22;1;20;0
WireConnection;38;0;22;0
WireConnection;38;1;5;4
WireConnection;53;0;40;0
WireConnection;53;1;52;4
WireConnection;8;0;38;0
WireConnection;8;1;53;0
WireConnection;69;0;8;0
WireConnection;59;3;98;0
WireConnection;98;14;8;0
WireConnection;98;1;60;25
WireConnection;98;2;60;167
WireConnection;98;31;69;0
WireConnection;29;2;59;0
WireConnection;29;3;98;25
ASEEND*/
//CHKSM=431576B862C382A46893874F35565EA5AC553542