Shader "Custom/GameOfLife" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct VertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct VertexOutput {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _MainTex_TexelSize;

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				fixed4 liveColor = fixed4(1, 1, 1, 1);
				fixed4 deadColor = fixed4(0, 0, 0, 0);
				float2 texelScale = float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y);
				
				fixed4 outputColor = liveColor;
				
				return outputColor;
			}

			ENDCG
		}
	}
}
