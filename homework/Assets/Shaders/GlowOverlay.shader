Shader "Custom/GlowOverlay" {
	Properties {
		_OutlineColor("Outline Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_XrayColor ("X-Ray Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Intensity ("Effect Intensity", Range(0.0, 1.0)) = 0.5
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

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
			};
			
			fixed4 _XrayColor;
			fixed4 _OutlineColor;

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.normal = input.normal;
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				return _XrayColor;
			}

			ENDCG
		}
	}
}
