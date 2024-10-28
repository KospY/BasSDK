// https://answers.unity.com/questions/1495552/shader-to-dilate-a-binary-image.htm

Shader "Hidden/Dilation"
{
	Properties
	{
		_MainTex ("MainTex", 2D) = "white" {}
		//_BlendMask ("BlendMask", 2D) = "white" {}
		_MaxSteps("Max Steps",Range(0,100)) = 100
		_MaskThreshold("Mask Threshold",Range(-10,200)) = 99
		_MaskThreshold2("Mask Threshold 2",Range(0,200)) = 101
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
 		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float2 _MainTex_TexelSize;
			
			//sampler2D _BlendMask;
			
			//sampler2D _MainTex2;
			//float4 _MainTex2_ST;
			
			float _PixelOffset;
			float _Mult;
			float _MaskThreshold;
			float _MaskThreshold2;
		
			float _MaxSteps;
			//int MaxSteps = 1;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{	
				/*
				//min and max Vector
				float2 _min = float2(0,0);
				float2 _max = float2(1,1);

				//get the color of 8 neighbour pixel
				fixed4 U = tex2D(_MainTex,clamp(i.uv + float2(0,_PixelOffset),_min,_max));
				fixed4 UR = tex2D(_MainTex,clamp(i.uv + float2(_PixelOffset,_PixelOffset),_min,_max));
				fixed4 R = tex2D(_MainTex,clamp(i.uv + float2(_PixelOffset,0),_min,_max));
				fixed4 DR = tex2D(_MainTex,clamp(i.uv + float2(_PixelOffset,-_PixelOffset),_min,_max));
				fixed4 D = tex2D(_MainTex,clamp(i.uv + float2(0,-_PixelOffset),_min,_max));
				fixed4 DL = tex2D(_MainTex,clamp(i.uv + float2(-_PixelOffset,-_PixelOffset),_min,_max));
				fixed4 L = tex2D(_MainTex,clamp(i.uv + float2(-_PixelOffset,0),_min,_max));
				fixed4 UL = tex2D(_MainTex,clamp(i.uv + float2(-_PixelOffset,_PixelOffset),_min,_max));
				
				//add all colors up to one final color
				fixed4 finalColor = (U + UR + R + DR + D + DL + L + UL )* _Mult;
				
				//////////////// UV Positional Dilation ///////////////////////////
				// Tex // Input Texture Object storing Volume Data
				// UV // Input float2 for UVs
				// TextureSize // Resolution of render target
				// MaxSteps // Pixel Radius to search
				*/

				float2 UV = i.uv;
				
				float texelsize = _MainTex_TexelSize.x;//1.0f / 4096.0f;
				float mindist = 10000000;

				float2 offsets[8] = {float2(-1,0), float2(1,0), float2(0,1), float2(0,-1), float2(-1,1), float2(1,1), float2(1,-1), float2(-1,-1)};

				//float3 sample = Tex.SampleLevel( TexSampler, UV, 0 );
				float4 sample = tex2D( _MainTex, float4(UV,0,0) );
				//float4 sample = tex2D( _BlendMask, float4(UV,0,0) );
				
				
				//float3 sampleMask = tex2D( _MainTex2, float4(UV,0,0) );
				//float4 curminsample = sample.rgba;
				float4 curminsample = tex2D( _MainTex, float4(UV,0,0) ).rgba;
				
			
				
 
				float sampleMask = sample.r+sample.g+sample.b;
				

				
				//Debug
				if ( sampleMask > _MaskThreshold) {
					//curminsample *= float4(1,0,0,1);
					//sample *= float4(1,0,0,1);
					//return float4(1,0,0,1);
				} else {
					//return float4(1,1,1,1);
				}
				
					
 
				//if ( sampleMask < _MaskThreshold) {
				if ( sampleMask <= 0.0) {
					int i = 0;
					
					//curminsample = float4(0,0,0,0);
					
					int MaxSteps = (int)_MaxSteps;
					//MaxSteps = 8;
							
					while( i < MaxSteps ) { 
						i++;
						int j = 0;
						
						while ( j < 8 ) {
							
							float2 curUV = UV + (offsets[j] * texelsize * i );
							
							//float3 offsetsample = Tex.SampleLevel(TexSampler, curUV, 0);
							float4 offsetsample = tex2Dlod( _MainTex, float4(curUV,0,0) );
//offsetsample *= float4(1,0,0,1);

							
							//if( offsetsample.a >= _MaskThreshold2) {
							if( offsetsample.r +  offsetsample.g +  offsetsample.b >= 0.1) {
								float curdist = length(UV - curUV);

								if ( curdist < mindist ) {
								
									//float2 projectUV = curUV + offsets[j] * texelsize * i * 0.25;
									//float3 direction = Tex.SampleLevel(TexSampler, projectUV, 0);
									//float4 direction = tex2Dlod(_MainTex, float4(projectUV,0,0));
									mindist = curdist;

									//if( direction.a >= 1 ) {
										//float3 delta = offsetsample - direction;
										//curminsample = offsetsample + delta * 4;
									//	curminsample = offsetsample;
									//} else {
										curminsample = offsetsample * 1;
									//}
								}
							}

							j++;
							
						}
 
					}

				}
				
				mindist = 10000000;
				
				float4 curminsample2 = tex2D( _MainTex, float4(UV,0,0) ).rgba;
				float sampleMask2 = sample.a;
				if ( sampleMask2 > 99.0) {
					int i = 0;
					int MaxSteps = (int)_MaxSteps;
							
					while( i < MaxSteps ) { 
						i++;
						int j = 0;
						while ( j < 8 ) {
							int k = 0;
							//while ( k < 2 ) {
								float2 curUV = UV + (offsets[j] * texelsize * i );
	
								float4 offsetsample = tex2Dlod( _MainTex, float4(curUV,0,0) );
	
								if( offsetsample.a <= 99.0) {
								
									float curdist = length(UV - curUV);
	
									if ( curdist < mindist ) {
										mindist = curdist;
										curminsample2 = offsetsample * 1;
									}
								}
								//k++;
							//}
							j++;
							
						}
 
					}

				}
				
//return float4(sample.a,0,0,0);

				//return float4(sample.rgb,0);
				
				
				//if(curminsample2.a>99) curminsample2.a = 0.0;
				//curminsample2.a = saturate(curminsample2.a*0.01);
				//return float4(curminsample2.a,curminsample2.a,curminsample2.a,1);
				
				
				return float4(curminsample.rgb,curminsample2.a);
				//return finalColor;
			}
			ENDCG
		}
	}
 }