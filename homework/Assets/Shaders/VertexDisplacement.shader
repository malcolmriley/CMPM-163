Shader "Custom/VertexDisplacement" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct VertexInput {
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct VertexOutput {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				float4 simpleVertex = UnityObjectToClipPos(input.vertex);
				output.vertex = simpleVertex;
				simpleVertex += input.normal * 2.0F;
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				return tex2D(_MainTex, input.uv);
			}

			ENDCG
		}
	}
}
