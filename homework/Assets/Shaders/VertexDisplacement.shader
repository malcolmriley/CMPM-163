Shader "Custom/VertexDisplacement" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Texture", 2D) = "white" {}
		_Frequency ("Frequency", Range(1, 10)) = 5
		_Amplitude ("Amplitude", Range(1, 10)) = 1
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
				float3 normal : NORMAL;
				float3 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};

			struct VertexOutput {
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Frequency;
			float _Amplitude;
			
			float3 wobble(float3 seed) {
				return sin(_Time.w + seed * _Frequency * 100) * _Amplitude * 0.01;
			}

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				float4 transformVertex = input.vertex;
				output.vertex = UnityObjectToClipPos(transformVertex);
				
				output.vertex.xyz += wobble(input.vertex.yzx);
				
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				output.normal = input.normal;
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				return tex2D(_MainTex, input.uv);
			}

			ENDCG
		}
	}
}
