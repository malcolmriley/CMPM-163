﻿Shader "Custom/TerrainDisplace" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_HighTex ("High Texture", 2D) = "white" {}
		_Height ("Height", Float) = 1.0
		_Color ("Color", Color) = (0.0, 0.0, 0.0, 0.0)
		_Outline ("Outline", Color) = (0.0, 0.0, 0.0, 0.0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100
		
		Pass {
			Cull Front
			
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
			fixed4 _Outline;

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				
				float4 displaced = input.vertex;
				displaced.z += tex2Dlod(_MainTex, float4(input.uv, 0, 0)).r * _Height + 0.01;
				output.vertex = UnityObjectToClipPos(displaced);
				
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				UNITY_TRANSFER_FOG(output, output.vertex);
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				return _Outline;
			}

			ENDCG
		}

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
				fixed4 displacement : AUX1;
			};

			sampler2D _MainTex;
			sampler2D _HighTex;
			float4 _MainTex_ST;
			float _Height;
			fixed4 _Color;

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				
				float4 displaced = input.vertex;
				fixed4 disp = tex2Dlod(_MainTex, float4(input.uv, 0, 0)).r * _Height;
				displaced.z += disp;
				output.vertex = UnityObjectToClipPos(displaced);
				output.displacement = disp;
				
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				UNITY_TRANSFER_FOG(output, output.vertex);
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				fixed4 texColor = tex2D(_MainTex, input.uv);
				fixed4 highColor = tex2D(_HighTex, input.uv).a * smoothstep(0, _Height * 2, input.displacement);
				fixed4 color = (texColor + highColor) * _Color;
				UNITY_APPLY_FOG(input.fogCoord, color);
				return color;
			}

			ENDCG
		}
	}
}
