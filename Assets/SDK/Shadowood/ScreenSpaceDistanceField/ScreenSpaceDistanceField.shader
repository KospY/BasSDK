Shader "Hidden/ScreenSpaceDistanceField" {
  Properties {
    _MainTex ("Texture", 2D) = "white" {}
  }

  CGINCLUDE
  #include "UnityCG.cginc"

  struct appdata {
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
  };

  struct v2f {
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
  };

  struct v2f_jump {
    float2 uv[9] : TEXCOORD0;
    float4 vertex : SV_POSITION;
  };

  float _Step;
  float2 _MainTex_TexelSize;
  sampler2D _MainTex;
  sampler2D _Source;

  v2f vert(appdata v) {
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
  }

  v2f_jump vert_jump(appdata v) {
    v2f_jump o;
    o.vertex = UnityObjectToClipPos(v.vertex);

    o.uv[0] = v.uv;

    float2 dx = float2(_MainTex_TexelSize.x, 0) * _Step;
    float2 dy = float2(0, _MainTex_TexelSize.y) * _Step;

    //Sample all pixels within a 3x3 block
    o.uv[1] = v.uv + dx;
    o.uv[2] = v.uv - dx;
    o.uv[3] = v.uv + dy;
    o.uv[4] = v.uv - dy;
    o.uv[5] = v.uv + dx + dy;
    o.uv[6] = v.uv + dx - dy;
    o.uv[7] = v.uv - dx + dy;
    o.uv[8] = v.uv - dx - dy;

    return o;
  }

  //Calculate screen space distance while compensating for
  //non-square texture sizes
  float ScreenDist(float2 v) {
    float ratio = _MainTex_TexelSize.x / _MainTex_TexelSize.y;
    v.x /= ratio;
    return dot(v, v);
  }

  float4 frag_init(v2f i) : SV_Target{
    float4 color = tex2D(_MainTex, i.uv);
    //color.w = color.r; // For mesh intersection
    //return color;
    
    float4 result;
    result.xy = float2(100.0, 100.0);
    result.z = ScreenDist(result.xy);
    
    
    //This line determines what is considered inside vs outside
    //For this example, alpha is the determining factor
    result.w = color.a > 0.5 ? 1.0 : 0.0; // Use image alpha
    //result.w = 1-step(color.a, 0); // Alternative to above
    
    //All pixels start out as pointing wayy too far out.
    //100 image units is very far.
    //W flags whether or not we are inside or outside
    
    //return float4(100,100,dot(result.x,result.y),1-step(color.a, 0));

    return result;
  }

  void checkBounds(inout float2 xy, float2 uv) {
    if (uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0) {
      xy = float2(1000.0, 1000.0);
    }
  }

  float4 frag_jump(v2f_jump i) : SV_Target{
    float4 curr = tex2D(_MainTex, i.uv[0]);

    [unroll] // Required for Vulkan, no idea why, I hate my life
    for (uint j = 1; j <= 8; j++) {
      //Sample a neighbor pixel
      float4 n = tex2D(_MainTex, i.uv[j]);

      //If the neighbor sample is on the other side
      //compared to us, treat it like there is zero
      //distance to the surface
      
      if (n.w != curr.w) {
      	n.xyz = float3(0, 0, 0);
      }
      
      // Alternative to above, removing the !=
      //float clipThreshold = 0.0001f;
      //if ( abs(n.w - curr.w) > clipThreshold) {
      //  n.xyz = float3(0, 0, 0);
      //}

      //Calculate the delta vector from our curr point
      //to the point the neighbor is pointing to
      n.xy += i.uv[j] - i.uv[0];

      //If the neighbor is out of bounds, make it invalid
      checkBounds(n.xy, i.uv[j]);

      //Calculate the screen space distance to the 
      //neighbors point using this delta
      float dist = ScreenDist(n.xy);

      //If the screen space distance is less than our
      //current min distance, update the current value
      //to point to the new location with the new distance
      if (dist < curr.z) {
        curr.xyz = float3(n.xy, dist);
        //curr = float4(curr.r,n.x,n.y,dist);
      }
    }
    
    return curr;
  }
  ENDCG

  SubShader {
    Cull Off ZWrite Off ZTest Always
    Blend One Zero

    //Pass 0: Init
    Pass {
      CGPROGRAM
      #pragma fragmentoption ARB_precision_hint_nicest
      //#pragma fragmentoption ARB_precision_hint_fastest
      #pragma target 3.0
      #pragma vertex vert
      #pragma fragment frag_init
      ENDCG
    }

    //Pass 1: Jump
    Pass {
      CGPROGRAM
      #pragma fragmentoption ARB_precision_hint_nicest
      //#pragma fragmentoption ARB_precision_hint_fastest
      #pragma target 3.0
      #pragma vertex vert_jump
      #pragma fragment frag_jump
      ENDCG
    }
  }
}
