Shader "Custom/BellShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_FracTex ("Fracture", 2D) = "white" {}
		_FracTex2 ("Fracture2", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_FracRatio("Fracture ratio", Range(0,1)) = 0
		_FracSpeed("Fracture speed", float) = 0
		_FracColor("Fracture color", Color) = (1,1,1,1)
		_FracColor2("Fracture color2", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _FracTex;
		sampler2D _FracTex2;

		struct Input {
			float2 uv_MainTex;
			float2 uv_FracTex;
			float2 uv_FracTex2;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _FracRatio;
		half _FracSpeed;
		fixed4 _FracColor;
		fixed4 _FracColor2;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 fracture = tex2D (_FracTex, IN.uv_FracTex);
			fixed4 fracture2 = tex2D (_FracTex2, IN.uv_FracTex2);
			// * abs(_SinTime.w * _FracSpeed)
			//o.Albedo = c.rgb + (_FracRatio * fracture.rgb);
			half sinAnim = abs(cos(_Time.w * _FracSpeed));
			half sinAnim2 = abs(sin(_Time.w * _FracSpeed));

			//o.Albedo = ((fracture*sinAnim+fracture2*sinAnim2) * (_FracRatio) < .3) ? (_FracColor2 * sinAnim2) + (_FracColor * sinAnim) + c: c;
			fixed4 col1 = ((1-fracture) * (_FracRatio*10) > .6) ?  (_FracColor * sinAnim) : c;
			fixed4 col2 = ((.5-fracture2) * (_FracRatio) > .1) ? (_FracColor2 * sinAnim2) : c;

			//o.Albedo = (fracture < .1 | fracture2 < .1) ? (_FracColor2 * sinAnim2) + (_FracColor * sinAnim) + c: c;
			//o.Albedo =  (col1+col2+c) / 3;
			o.Albedo =  min(col1,(col2+c) / 2);

			
			//o.Albedo = (_FracRatio * (_FracColor * sinAnim)) + ((1-_FracRatio) * c);
			
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
