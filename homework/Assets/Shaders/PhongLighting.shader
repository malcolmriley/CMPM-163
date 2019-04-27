Shader "Custom/PhongLighting" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Ambience ("Ambient Light", Range(0.0, 1.0)) = 0.0
		_Diffusion("Diffusion", Range(0.0, 1.0)) = 0.2
		_Specularity("Specularity", Range(0.0, 10.0)) = 0.2
		_Shininess ("Shininess", Range(0.01, 100.0)) = 1.0
	}
	SubShader {
		Tags {
			"RenderType"="Opaque"
			"LightMode"="ForwardBase"
		}

		Pass {
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
				float3 worldCoords : COORDS;
				float3 surfaceNormal : SURF;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _AmbientColor;
			float _Ambience;
			float _Shininess;
			float _Diffusion;
			float _Specularity;
			
			fixed4 getPhong(float3 lightCoordinates, half4 lightColor, float3 worldCoordinates, float3 surfaceNormal) {
				float3 lightDirection = normalize(lightCoordinates - worldCoordinates);
				float3 viewDirection = normalize(_WorldSpaceCameraPos - worldCoordinates);
				float3 halfway = normalize(viewDirection + lightDirection);
			
				float3 diffuse = _Diffusion * dot(surfaceNormal, lightDirection);
				float3 specular = _Specularity * lightColor * pow(max(dot(surfaceNormal, halfway), 0), _Shininess);
				
				return float4(diffuse + specular, 1);
			}

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
				output.uv = TRANSFORM_TEX(input.uv, _MainTex);
				output.worldCoords = mul(unity_ObjectToWorld, input.vertex);
				output.surfaceNormal = UnityObjectToWorldNormal(input.normal);
				return output;
			}

			fixed4 frag (VertexOutput input) : SV_Target {
				fixed4 texColor = tex2D(_MainTex, input.uv);
				fixed4 phong = fixed4(0,0,0,0);
				
				for (int index = 0; index < 4; index += 1) {
					float3 lightCoordinates = float3(unity_4LightPosX0[index], unity_4LightPosY0[index], unity_4LightPosZ0[index]);
					phong += getPhong(lightCoordinates, unity_LightColor[index], input.worldCoords, normalize(input.surfaceNormal));
				}
				
				float4 averageChroma = (unity_LightColor[0] + unity_LightColor[1] + unity_LightColor[2] + unity_LightColor[3]) / 4;
				
				return _Ambience * averageChroma + texColor + phong;
			}

			ENDCG
		}
	}
}