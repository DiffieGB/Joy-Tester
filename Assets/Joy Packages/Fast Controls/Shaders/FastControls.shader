Shader "Fast Controls" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	Category {
	    Tags { "Queue"="Transparent+100" "IgnoreProjector"="True" "RenderType"="Transparent" }

	    Blend SrcAlpha OneMinusSrcAlpha

	    Cull Off Lighting Off ZWrite Off Fog { Mode Off }

	    BindChannels {
	        Bind "Color", color
	        Bind "Vertex", vertex
	        Bind "TexCoord", texcoord
	    }

	    SubShader {
	        Pass {
	            CGPROGRAM
	            #pragma vertex vert
	            #pragma fragment frag
	            #pragma fragmentoption ARB_precision_hint_fastest

	            #include "UnityCG.cginc"

	            struct appdata {
	                float4 vertex : POSITION;
	                float2 texcoord : TEXCOORD0;
	                fixed4 color : COLOR;
	            };

	            struct v2f {
	                float4 pos : SV_POSITION;
	                float2 texcoord : TEXCOORD0;
	                fixed4 color : COLOR;
	            };

	            sampler2D _MainTex;
	            uniform float4 _MainTex_ST;

	            v2f vert (appdata v) {
	                v2f output;
	                output.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
	                output.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	                output.color = v.color;
	                return output;
	            }
	            
	            fixed4 frag (v2f i) : COLOR0 {
	                return tex2D(_MainTex, i.texcoord) * i.color;
	            }
	            ENDCG
	        }
	    }
	}
}