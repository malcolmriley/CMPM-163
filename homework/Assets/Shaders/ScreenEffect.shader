Shader "Custom/ScreenEffect" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_ScanLines ("Scan Lines", 2D) = "white" {}
		_ScanIntensity ("Scan Lines Intensity", Range(0.0, 1.0)) = 0.1
		_Sharpness("Sharpness", Range(0.0, 1.0)) = 0.2
		_Color ("Color", Color) = (1, 1, 1, 1)
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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvScan : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _ScanLines;
			float4 _MainTex_ST;
			float4 _ScanLines_ST;
			float4 _MainTex_TexelSize;
			fixed4 _Color;
			float _ScanIntensity;
			float _Sharpness;

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				output.uvScan = TRANSFORM_TEX(input.uv, _ScanLines);
				return output;
			}
			
			fixed4 fetchTexel(sampler2D sampledTexture, float2 uv, float2 texelScale, float2 offset) {
				return tex2D(sampledTexture, uv + texelScale * offset);
			}
			
			float kernelSum(float3x3 kernel) {
				float sum = 0;
				for (int xPos = 0; xPos < 3; xPos += 1) {
					for (int yPos = 0; yPos < 3; yPos += 1) {
						sum += kernel[xPos][yPos];
					}
				}
				return sum;
			}
			
			fixed4 getConvolution(sampler2D sampledTexture, float3x3 kernel, float2 uv, float2 texelScale) {
				fixed4 result = fixed4(0,0,0,0);
				for (int xPos = 0; xPos < 3; xPos += 1) {
					for (int yPos = 0; yPos < 3; yPos += 1) {
						result += kernel[xPos][yPos] * fetchTexel(sampledTexture, uv, texelScale, float2(xPos - 1, yPos - 1));
					}
				}
				return result / kernelSum(kernel); // normalize
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				// Perform Convolution
				float2 texelScale = float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y);
				float3x3 kernel = float3x3(1 - _Sharpness, 1 - _Sharpness, 1 - _Sharpness, 1 - _Sharpness, _Sharpness, 1 - _Sharpness, 1 - _Sharpness, 1 - _Sharpness, 1 - _Sharpness);
				fixed4 baseColor = getConvolution(_MainTex, kernel, input.uv, texelScale);
				
				// Scan Lines
				fixed4 smallLines = _ScanIntensity * tex2D(_ScanLines, input.uvScan * _ScanLines_ST.x + _ScanLines_ST.yy * _Time.x * 0.5);
				fixed4 bigLines = _ScanIntensity * tex2D(_ScanLines, input.uv + _Time.ww);
				fixed4 centerBias = _ScanIntensity * 0.5 * tex2D(_ScanLines, input.uv);
				
				// Final Composite Color
				fixed4 final = _Color * (baseColor + smallLines + bigLines + centerBias);
				return clamp(final, fixed4(0,0,0,0), fixed4(1,1,1,1));
			}

			ENDCG
		}
	}
}
