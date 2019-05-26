Shader "Custom/EdgeGlow" {
	Properties {
		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_AlphaTex ("Alpha Texture", 2D) = "white" {}
		_Intensity ("Effect Intensity", Range(0.0, 2.0)) = 0.5
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
				float3 normal : NORMAL;
			};

			struct VertexOutput {
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float4 screen : TEXCOORD0;
				float3 view : DIRECTION;
			};
			
			fixed4 _Color;
			fixed _Intensity;
			
			sampler2D _AlphaTex;
			fixed4 _AlphaTex_ST;
			

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.screen = ComputeScreenPos(input.vertex);
				output.normal = UnityObjectToWorldNormal(input.normal);
				output.view = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, input.vertex).xyz);
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				fixed4 alphaColor = tex2D(_AlphaTex, TRANSFORM_TEX(input.screen.xy, _AlphaTex) + _Time.x);
				fixed4 incidence = 1 - dot(input.normal, input.view);
				return _Color * incidence * _Intensity * alphaColor;
			}

			ENDCG
		}
	}
}
