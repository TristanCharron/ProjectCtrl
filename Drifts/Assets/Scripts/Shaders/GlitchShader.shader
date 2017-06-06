// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/GlitchShader" {
	Properties{
	_MainTex("Base (RGB)", 2D) = "white" {}
	_Intensity("Glitch Intensity", Range(0.1, 1.0)) = 1
	}

		SubShader{
		Pass{
		ZTest Always Cull Off ZWrite Off
		Fog{ Mode off }

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 

#include "UnityCG.cginc"

	uniform sampler2D _MainTex;
	float _Intensity;
	
	struct v2f {
		float4 vertex : SV_POSITION;
		float2 uv : TEXCOORD0;
	};

	v2f vert(appdata_img v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		return o;
	}

	half4 frag(v2f i) : COLOR
	{
		half4 color = tex2D(_MainTex,  i.uv + float2(0, sin(_Time[3] + i.vertex.x / 50)/300) * _Intensity)  ;
		return color;
	}
		ENDCG
	}


	}

		Fallback off

}