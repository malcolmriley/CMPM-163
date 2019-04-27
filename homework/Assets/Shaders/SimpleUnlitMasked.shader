Shader "Custom/SimpleUnlitMasked" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_OverlayTexture ("Overlay Texture", 2D) = "white" {}
	}
	SubShader {
		Tags {
			"Queue"="Transparent"
		}
		Blend SrcAlpha OneMinusSrcAlpha

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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvOverlay : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _OverlayTexture;
			float4 _MainTex_ST;
			float4 _OverlayTexture_ST;

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				output.uvOverlay = TRANSFORM_TEX(input.uv, _OverlayTexture);
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				fixed4 alpha = tex2D(_MainTex, input.uv);
				fixed4 color = tex2D(_OverlayTexture, input.uvOverlay);
				return fixed4(color.rgb, alpha.a);
			}

			ENDCG
		}
	}
}
