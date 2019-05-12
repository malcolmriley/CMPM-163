Shader "Custom/CameraVFXBloon" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_BloomTex ("Bloom Texture", 2D) = "black" {}
		_Intensity ("Bloom Intensity", Float) = 0.5
	}
	SubShader {

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
			float4 _BloomTex_TexelSize;
			float4 _MainTex_ST;
			float _Intensity;
			
			fixed4 getBlur(float2 uv) {
				float2 texelScale = float2(_BloomTex_TexelSize.x, _BloomTex_TexelSize.y);
				fixed4 average = fixed4(0, 0, 0, 0);
				for (int offset = 0; offset < 15; offset += 1) {
					for (int xPos = 0; xPos < 3; xPos += 1) {
						for (int yPos = 0; yPos < 3; yPos += 1) {
							fixed4 color = tex2D(_BloomTex, uv + texelScale * float2(xPos - 1, yPos - 1) * offset);
							average += color;
						}
					}
				}
				return average / 15;
			}

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				fixed4 mainColor = tex2D(_MainTex, input.uv);
				fixed4 bloomColor = tex2D(_BloomTex, input.uv);
				return mainColor + bloomColor + getBlur(input.uv) * _Intensity;
			}

			ENDCG
		}
	}
}
