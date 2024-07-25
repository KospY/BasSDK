// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/PackedBlit"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Channel("Channel", Vector) = (0,0,1,0)
		[HideInInspector]_Remap("Remap", Vector) = (0,1,0,1)
		_Mode("Mode", Int) = 0

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
		ZTest Always
		
		
		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
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

			uniform int _Mode;
			uniform sampler2D _MainTex;
			uniform float4 _Channel;
			uniform float4 _Remap;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
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
				float4 ifLocalVars25 = 0;
				float2 texCoord7 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float dotResult22 = dot( tex2D( _MainTex, texCoord7 ) , _Channel );
				float4 break5_g3 = _Remap;
				float temp_output_29_0_g3 = saturate( (0.0 + (saturate( dotResult22 ) - break5_g3.x) * (1.0 - 0.0) / (break5_g3.y - break5_g3.x)) );
				float clampResult2_g3 = clamp( temp_output_29_0_g3 , break5_g3.z , break5_g3.w );
				float temp_output_27_0_g3 = saturate( clampResult2_g3 );
				float4 temp_cast_1 = (( temp_output_27_0_g3 + 0.0 )).xxxx;
				if(_Mode==0){ifLocalVars25 = temp_cast_1; };
				float4 break5_g778 = _Remap;
				float temp_output_29_0_g778 = saturate( (0.0 + (saturate( dotResult22 ) - break5_g778.x) * (1.0 - 0.0) / (break5_g778.y - break5_g778.x)) );
				float temp_output_21_0_g778 = (break5_g778.z + (temp_output_29_0_g778 - 0.0) * (break5_g778.w - break5_g778.z) / (1.0 - 0.0));
				float temp_output_14_0_g779 = temp_output_21_0_g778;
				float temp_output_6_0_g779 = saturate( temp_output_14_0_g779 );
				float temp_output_12_0_g779 = (0.0 + (temp_output_14_0_g779 - -1.0) * (1.0 - 0.0) / (0.0 - -1.0));
				float temp_output_13_0_g779 = step( temp_output_12_0_g779 , 1.0 );
				float temp_output_11_0_g779 = ( 1.0 - saturate( ( temp_output_12_0_g779 * temp_output_13_0_g779 ) ) );
				if(_Mode==1){ifLocalVars25 = ( ( ( float4( 1,0,0,0 ) * temp_output_6_0_g779 ) + ( float4( 0,0,1,0 ) * temp_output_11_0_g779 * temp_output_13_0_g779 ) ) + float4( 0,0,0,0 ) ); };
				
				
				finalColor = ifLocalVars25;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-187,-69;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;12;-431.4739,429.069;Inherit;True;5;0;FLOAT;0.5;False;1;FLOAT;0.11;False;2;FLOAT;0.1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;13;-383.474,685.0687;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;14;-383.474,845.069;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;22;-260.4353,172.9963;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;2;-556,-211;Inherit;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;0;False;0;False;1,0.3208255,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-612,-23;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;46437c5a2e7e21f4fb6a2d7ced735cfe;46437c5a2e7e21f4fb6a2d7ced735cfe;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;4;-1029.4,-25.6001;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1063.4,-159.6;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;8;-838.3998,-153.6;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.5,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;18;-732.8743,622.7687;Inherit;False;Property;_Remap;Remap;2;1;[HideInInspector];Create;True;0;0;0;False;0;False;0,1,0,1;0,1,0.5,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;21;-511.4353,221.9963;Inherit;False;Property;_Channel;Channel;1;0;Create;True;0;0;0;False;0;False;0,0,1,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;23;6.783419,203.2059;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;243.0266,568.8688;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;341.9176,326.7771;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;15;-32.47351,669.0687;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;27;12.9176,448.7771;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;17;545.7379,574.8583;Inherit;True;LinearLightDebug;-1;;2;9593a61b712c0ed4f989fd16be0a12ab;1,18,0;3;14;FLOAT;0;False;15;COLOR;0,0,1,0;False;16;COLOR;1,0,0,0;False;3;COLOR;0;FLOAT;21;FLOAT;22
Node;AmplifyShaderEditor.SwitchByInt;25;1293.932,376.1498;Inherit;False;2;33;0;INT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;9;FLOAT4;0,0,0,0;False;10;FLOAT4;0,0,0,0;False;11;FLOAT4;0,0,0,0;False;12;FLOAT4;0,0,0,0;False;13;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT4;0,0,0,0;False;16;FLOAT4;0,0,0,0;False;17;FLOAT4;0,0,0,0;False;18;FLOAT4;0,0,0,0;False;19;FLOAT4;0,0,0,0;False;20;FLOAT4;0,0,0,0;False;21;FLOAT4;0,0,0,0;False;22;FLOAT4;0,0,0,0;False;23;FLOAT4;0,0,0,0;False;24;FLOAT4;0,0,0,0;False;25;FLOAT4;0,0,0,0;False;26;FLOAT4;0,0,0,0;False;27;FLOAT4;0,0,0,0;False;28;FLOAT4;0,0,0,0;False;29;FLOAT4;0,0,0,0;False;30;FLOAT4;0,0,0,0;False;31;FLOAT4;0,0,0,0;False;32;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1485.932,456.1498;Float;False;True;-1;2;ASEMaterialInspector;100;5;Hidden/PackedBlit;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;;0;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;2;False;;True;7;False;;True;False;0;False;;0;False;;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;765.9023,183.8795;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;764.594,-35.92561;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;24;1117.932,296.1498;Inherit;False;Property;_Mode;Mode;3;0;Create;True;0;0;0;False;0;False;0;0;False;0;1;INT;0
Node;AmplifyShaderEditor.FunctionNode;32;521.6662,45.2431;Inherit;False;RemapNode;-1;;3;513e405bb9fdf6448b381310d0b5b97c;3,14,0,22,0,24,1;2;6;FLOAT4;0,0,0,0;False;8;FLOAT;0;False;2;FLOAT;12;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;33;531.7054,172.1041;Inherit;False;RemapNode;-1;;778;513e405bb9fdf6448b381310d0b5b97c;3,14,1,22,1,24,1;2;6;FLOAT4;0,0,0,0;False;8;FLOAT;0;False;2;COLOR;12;FLOAT;0
WireConnection;12;0;22;0
WireConnection;12;1;18;1
WireConnection;12;2;18;2
WireConnection;13;0;18;3
WireConnection;14;0;18;4
WireConnection;22;0;1;0
WireConnection;22;1;21;0
WireConnection;1;1;7;0
WireConnection;8;0;7;0
WireConnection;8;1;4;0
WireConnection;23;0;22;0
WireConnection;16;0;15;0
WireConnection;26;0;27;0
WireConnection;15;0;12;0
WireConnection;15;1;13;0
WireConnection;15;2;14;0
WireConnection;27;0;12;0
WireConnection;27;1;18;3
WireConnection;27;2;18;4
WireConnection;17;14;16;0
WireConnection;25;0;24;0
WireConnection;25;1;31;0
WireConnection;25;2;30;0
WireConnection;0;0;25;0
WireConnection;30;0;33;12
WireConnection;31;0;32;0
WireConnection;32;6;18;0
WireConnection;32;8;22;0
WireConnection;33;6;18;0
WireConnection;33;8;22;0
ASEEND*/
//CHKSM=DF9EBBFED3C0E7902A993375162EC084CAD09FDD