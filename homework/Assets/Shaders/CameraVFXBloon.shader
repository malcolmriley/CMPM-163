Shader "Custom/CameraVFXBloon" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_BloomTex ("Bloom Texture", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100

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
			sampler2D _BloomTex;
			float4 _MainTex_ST;

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				fixed4 mainColor = tex2D(_MainTex, input.uv);
				fixed4 bloomColor = tex2D(_BloomTex, input.uv);
				return mainColor + bloomColor;
			}

			ENDCG
		}
	}
}
