Shader "Custom/SmokeShader" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
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
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct VertexOutput {
				float2 uv : TEXCOORD0;
				float2 uvNoise : TEXCOORD1;
				float4 screen : TEXCOORD2;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				output.screen = ComputeScreenPos(input.vertex);
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				output.uvNoise = TRANSFORM_TEX(input.uv, _NoiseTex);
				output.color = input.color;
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				fixed4 color = tex2D(_MainTex, input.uv);
				fixed4 noise = tex2D(_NoiseTex, input.screen.xy * input.uvNoise + _Time.x);
				
				fixed alpha = (noise.r + noise.g + noise.b) / 3;
				
				color.a = clamp(color.a - alpha, 0, 1);
				
				return color * input.color;
			}

			ENDCG
		}
	}
}
