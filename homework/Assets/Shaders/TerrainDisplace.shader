Shader "Custom/TerrainDisplace" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Height ("Height", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct VertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct VertexOutput {
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Height;

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				
				float4 displaced = input.vertex;
				displaced.z += tex2Dlod(_MainTex, float4(input.uv, 0, 0)).r * _Height;
				output.vertex = UnityObjectToClipPos(displaced);
				
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				UNITY_TRANSFER_FOG(output, output.vertex);
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				fixed4 color = tex2D(_MainTex, input.uv);
				UNITY_APPLY_FOG(input.fogCoord, color);
				return color;
			}

			ENDCG
		}
	}
}
