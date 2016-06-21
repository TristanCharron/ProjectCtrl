Shader "Unlit/VHS"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPosition : TEXCOORD1;
			};

			float rand(float2 co)
			{
				float a = 12.9898;
				float b = 78.233;
				float c = 43758.5453;
				float dt = dot(co.xy, float2(a, b));
				float sn = fmod(dt, 3.14);
				return frac(sin(sn) * c);
			}


			v2f vert(appdata input)
			{
				v2f output;

				float4 offset = float4(sin(_Time[2]) / 10, cos(_Time[1]) / 10,0,0);
				output.worldPosition = input.vertex;

				output.vertex = mul(UNITY_MATRIX_MVP, output.worldPosition);
				output.uv = input.uv;
				return output;
			}

			sampler2D _MainTex;


			// Fragment function that is runned on every single pixel that it applies too (ex : 1920 x 1080 times)
			fixed4 frag(v2f input) : SV_Target
			{
			float magnitude = 0.015;
			input.uv = input.vertex.xy / _ScreenParams.xy;
			//input.uv = -input.uv;
			//Red
			v2f offsetRedUV = input;
			offsetRedUV.uv.x = input.uv.x + rand(float2(_Time[1] * 0.03, input.uv.y*0.42)) * 0.001;
			offsetRedUV.uv.x += cos(rand(float2(_Time[1] * 0.2, input.uv.y)))*magnitude;

			//Green
			v2f offsetGreenUV = input;
			offsetGreenUV.uv.x = input.uv.x + rand(float2(_Time[1] * 0.04, input.uv.y*0.02)) * 0.004;
			offsetGreenUV.uv.x += (cos(rand(float2(_Time[1] * 0.2, input.uv.y)))*magnitude);

			float r = tex2D(_MainTex, offsetRedUV.uv).r;
			float g = tex2D(_MainTex, offsetGreenUV.uv).g;
			fixed4 col = tex2D(_MainTex, input.uv);
			return float4(r, g, col.b,0);
			}
			ENDCG
		}
	}
}
