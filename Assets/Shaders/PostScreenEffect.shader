
// Name of the Shader + Folder where it is stored
Shader "Hidden/PostScreenEffect"
{
	// List of properties that will be used in our Material Interface
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Intensity("Glitch Intensity", Range(0.1, 1.0)) = 1
	}
		SubShader
		{
			// No culling or depth
			Cull Off ZWrite Off ZTest Always

			Pass
			{
				// All the work of the shader is executed between CGPROGRAM AND CGEND
				CGPROGRAM
				// Pragma telled we included Vertex & Frag Shaders (so we used Frag and Vert functions)
				#pragma vertex vert
				#pragma fragment frag
				#define VERTEX_P highp
				#define FRAGMENT_P highp

				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				float _Intensity;
				float distortion;
				float flip_up, flip_down;
				float displace, scale;
				float Red, Green, Blue;
				float isInversedColor;
				float inverseRed, inverseBlue, inverseGreen;


				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;

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
					output.vertex = mul(UNITY_MATRIX_MVP, input.vertex);
					output.uv = input.uv;
					return output;
				}

				void onSetUVFlip(v2f input)
				{
					if (input.uv.y < flip_up)
						input.uv.y = 1 - (input.uv.y + flip_up);

					if (input.uv.y > flip_down)
						input.uv.y = 1 - (input.uv.y - flip_down);
				}


			

				// Fragment function that is runned on every single pixel that it applies too (ex : 1920 x 1080 times)
				fixed4 frag(v2f input) : SV_Target
				{

				//onSetUVFlip(input);
				float magnitude = 0.009 * distortion;
				half4 normal = tex2D(_MainTex, input.uv.xy);
				
				//Glitch Offset
				input.uv.x += (_Intensity * 5 > 1) ? (normal.y - _Intensity) * displace : -(normal.y - _Intensity) * displace;
				input.uv.y += (_Intensity * 5 > 1) ? (normal.x - _Intensity) * displace : -(normal.x - _Intensity) * displace;
				
				
				//Red offset
				v2f offsetRedUV = input;
				offsetRedUV.uv.x = input.uv.x + rand(float2(_Time[1] * 0.001, input.uv.y )) * 0.003;
				offsetRedUV.uv.x += cos(rand(float2(_Time[1], input.uv.y)))*magnitude;

				//Green Offset
				v2f offsetGreenUV = input;
				offsetGreenUV.uv.x = input.uv.x + rand(float2(_Time[1] * 0.001, input.uv.y)) * 0.001;
				offsetGreenUV.uv.x += (sin(rand(float2(_Time[1], input.uv.y)))*magnitude);

				float r = tex2D(_MainTex, offsetRedUV.uv).r;
				float g = tex2D(_MainTex, offsetGreenUV.uv).g;
				fixed4 col = tex2D(_MainTex, input.uv);

				if (isInversedColor == 1)
					col = float4(inverseRed > 0 ? inverseRed - r * Red : r * Red, inverseGreen > 0 ? inverseGreen - g * Green : g * Green, inverseBlue > 0 ? inverseBlue - col.b * Blue : col.b * Blue, 1);
				else
					col = float4(r * Red, g * Green, col.b * Blue, 1);
				
				return col;
				}
				ENDCG
			}
		}
}

