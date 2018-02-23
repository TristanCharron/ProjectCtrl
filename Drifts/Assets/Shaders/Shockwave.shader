//Converted by ShaderMan + DommerMan
//https://github.com/smkplus/ShaderMan
Shader"ShaderMan/Shockwave"{
	Properties{
		_MainTex("_MainTex", 2D) = "white"{}
		_Size("_Size", float) = 1
		_Pos("_Pos", Vector) = (1,1,0,0)
		_Lerp("_Lerp", float) = 1
		_Color("_Color", Color) = (1,1,1,1)
		_EnergyBurst("_EnergyBurst", float) = 80
		_Decay("_Decay", float) = 40
		_ExtraParam1("_ExtraParam1", float) = 10
		_ExtraParam2("_ExtraParam2", float) = .8
		_ExtraParam3("_ExtraParam3", float) = .1

		_Intensity("_Intensity", Range(0,1)) = .5
		_LifeTime("_LifeTime", Range(0,10)) = .5
		_ScreenYRatio("_ScreenYRatio", Range(0,1)) = .5
	}
	
	SubShader
	{
		Pass
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"
			
			struct appdata{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			sampler2D _MainTex;
			float _Lerp;
			
			float _Size;
			float2 _Pos;

			float4 _Color;
			
			float _Decay;
			float _EnergyBurst; 
			
			float _ExtraParam1;
			float _ExtraParam2;
			float _ExtraParam3;

			float _Intensity;
			float _LifeTime;
			float _ScreenYRatio;
			
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed time = _Lerp;
			   
			     // Wave design params
			    fixed3 waveParams = fixed3(_ExtraParam1, _ExtraParam2, _ExtraParam3);

			    // Find coordinate, flexible to different resolutions
			    fixed2 uv = i.uv;
			    fixed2 shockuv = i.uv;
			   
			     // Find center, flexible to different resolutions
				fixed2 center = _Pos;
				
				fixed2 dir = uv - center;
				fixed2 uvRatio = shockuv.xy;
				uvRatio.y -= dir.y * _ScreenYRatio;
				
			    fixed dist = distance(uvRatio, center);
 				
				
			    // Original color
			    fixed4 c = tex2D(_MainTex, uv);

			    // Limit to waves
			    if (time > 0. && dist <= time + waveParams.z && dist >= time - waveParams.z) 
			    {
			        // The pixel offset distance based on the input parameters
			        fixed diff = (dist - time) * _Size;
			        fixed diffPow = (1.0 - pow(abs(diff * waveParams.x), waveParams.y));
			        fixed diffTime = (diff  * diffPow);

			        // The direction of the distortion
			        dir = normalize(dir) * _LifeTime;
					
			        // Perform the distortion and reduce the effect over time
			        uv += ((dir * diffTime) / (time * dist * _EnergyBurst));

					fixed4 c2 = tex2D(_MainTex, uv);
			        // Grab color for the new coord
			        c = (c * _Intensity) + (c2 * (1 - _Intensity)) + (_Color * _Color.a);	

			        // Optionally: Blow out the color for brighter-energy origin
			       // c += (c * diffPow) / (time * dist * _Decay);
			    }

			    return  c;
			}
			ENDCG
		}
	}
}