Shader "Custom/WaterShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Skybox ("Skybox", CUBE) = "white" {}
		_Height ("Heightmap", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
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
				float3 worldNormal : NORMAL_WORLD;
				float3 worldVertex : VERTEX_WORLD;
			};
			
			fixed4 _Color;
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
				float3 reflectVector = reflect(normalize(input.worldVertex - _WorldSpaceCameraPos), input.worldNormal);
				
				fixed4 texColor = tex2D(_Height, input.uv);
				fixed4 reflectColor = texCUBE(_Skybox, reflectVector);
				
				return (texColor + reflectColor) * _Color;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
