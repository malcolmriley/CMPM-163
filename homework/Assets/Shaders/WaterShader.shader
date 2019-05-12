Shader "Custom/WaterShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Skybox ("Skybox", Cube) = "white" {}
		_Height ("Heightmap", 2D) = "white" {}
		_Level ("Level", Range(0.0, 1.0)) = 0.0
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		
		Pass {
			ZWrite Off
        	Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct VertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct VertexOutput {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL_WORLD;
				float3 worldVertex : VERTEX_WORLD;
			};
			
			fixed4 _Color;
			float _Level;
			sampler2D _Height;
			float4 _Height_ST;
			samplerCUBE _Skybox;

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = TRANSFORM_TEX(input.uv, _Height);
				output.worldNormal = UnityObjectToWorldNormal(input.normal);
				output.worldVertex = mul(unity_ObjectToWorld, input.vertex);
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				// Reflection Color
				float3 reflectVector = reflect(normalize(input.worldVertex - _WorldSpaceCameraPos), input.worldNormal);
				fixed4 reflectColor = texCUBE(_Skybox, reflectVector);
				
				// Albedo Color
				fixed4 texColor = tex2D(_Height, input.uv);
				float level = _Level + _SinTime.w * 0.007;
				if (texColor.r > level) {
					texColor = fixed4(0.8, 0.8, 0.8, 0.8);
				}
				
				// Refraction Color
				float3 refractAngle = 0.5 + 0.3 * sin(_Time.w);
				float3 refractVector = refract(input.worldVertex - _WorldSpaceCameraPos, input.worldNormal, refractAngle);
				float4 refractColor = texCUBE(_Skybox, refractVector) * 0.2;
				
				fixed4 finalColor = (texColor + reflectColor + refractColor) * _Color;
				finalColor.a = _Color.a;
				return finalColor;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
