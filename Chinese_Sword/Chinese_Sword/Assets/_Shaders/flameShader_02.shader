Shader "FlameShader/flameShader_02" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_SolidTex ("Albedo (RGB)", 2D) = "white" {}
		_Distort ("Distort (A)", 2D) = "white" {}
		_DynaTex ("Albedo sub (RGB)", 2D) = "white" {}
		_Emission ("Emission", Range(0,10)) = 0.5
		_DistortX ("Distort in X", Range(0,1)) = 0.5
		_DistortY ("Distort in Y", Range(0,1)) = 0.5
		_Glossiness ("Glossiness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _SolidTex;
		sampler2D _Distort;
		sampler2D _DynaTex;
		fixed _DistortX;
		fixed _DistortY;
		fixed _Emission;
		fixed4 _Color;

		struct Input {
			float2 uv_SolidTex;
			float2 uv2_DynaTex;
		};

		half _Glossiness;
		half _Metallic;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_SolidTex, IN.uv_SolidTex) * _Color;
			fixed distort = tex2D(_Distort, IN.uv_SolidTex).a;

			fixed2 uv_scroll;
			uv_scroll = fixed2(IN.uv2_DynaTex.x -  distort * _DistortX,IN.uv2_DynaTex.y - distort * _DistortY);

			fixed4 tex2 = tex2D(_DynaTex,uv_scroll);
			c.rgb *= 1 + _Emission;
			c.rgb = lerp(tex2.rgb, c.rgb, c.a);

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
