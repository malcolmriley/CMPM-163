Shader "Custom/Dissolve" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_DissolveTex("Dissolve", 2D) = "white" {}
		_RampTex("Color Ramp", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM

			#pragma vertex ShaderVertex
			#pragma fragment ShaderFragment

			#include "UnityCG.cginc"
			#include "ShaderCommon.cginc"

			struct VertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct VertexOutput {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			sampler2D _MainTex;
			sampler2D _DissolveTex;
			sampler2D _RampTex;
			float4 _MainTex_ST;
			float4 _DissolveTex_ST;

			VertexOutput ShaderVertex (VertexInput input) {
				VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				return output;
			}

			fixed4 ShaderFragment (VertexOutput input) : SV_Target {
				fixed4 textureColor = tex2D(_MainTex, input.uv);
				float dissolve = tex2D(_DissolveTex, input.uv);
				fixed4 useColor = textureColor;
				float progress = _SinTime.w;
				if (progress > 0.0) {
					if (dissolve < progress) {
						return fixed4(0.0, 0.0, 0.0, 0.0);
					}
					if (dissolve - progress < 0.15) {
						float dissolveProgress = (dissolve - progress) / 0.15;
						useColor = (textureColor * dissolveProgress) + (tex2D(_RampTex, float2(dissolveProgress, 1.0)) * (1 - dissolveProgress));
						useColor.a = textureColor.a;
					}
				}
				return useColor;
			}

			ENDCG
		}
	}
}
