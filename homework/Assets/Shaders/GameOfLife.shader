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
			
			int countVicinity(sampler2D sampledTexture, fixed4 threshold, float2 uv, float2 texelScale) {
				int count = 0;
				for (int xPos = 0; xPos < 3; xPos += 1) {
					for (int yPos = 0; yPos < 3; yPos += 1) {
						fixed4 color = tex2D(sampledTexture, uv + texelScale * float2(xPos - 1, yPos - 1));
						count += any(color > threshold) ? 1 : 0;
					}
				}
				return count;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				fixed4 liveColor = fixed4(1.0, 1.0, 1.0, 1.0);
				fixed4 threshold = fixed4(0.5, 0.5, 0.5, 1.0);
				fixed4 deadColor = fixed4(0.0, 0.0, 0.0, 1.0);
				
				float2 texelScale = float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y);
				int liveCount = countVicinity(_MainTex, threshold, input.uv, texelScale);
				fixed4 selfColor = tex2D(_MainTex, input.uv);
				
				fixed4 outputColor = deadColor;
				if (any(selfColor > threshold)) {
					outputColor = (liveCount > 5) ? deadColor : selfColor;
				}
				else {
					outputColor = (liveCount == 3) ? liveColor : selfColor;
				}
				
				return outputColor;
			}

			ENDCG
		}
	}
}
