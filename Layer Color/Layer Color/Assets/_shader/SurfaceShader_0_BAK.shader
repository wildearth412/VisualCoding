Shader "Custom/SurfaceShader_0_BAK" {
	Properties {

		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo 1 (RGB)", 2D) = "white" {}
		_MainTex2 ("Albedo 2 (RGB)", 2D) = "white" {}
		_Transparency1("Transparency 1", Range(0,1)) = 1
		_Transparency2("Transparency 2", Range(0,1)) = 1
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		//[Enum(Normal,0,Dissolve,1,Darken,2,Lighten,3,Multiply,4,Screen,5,ColorBurn,6,ColorDodge,7,LinearBurn,8,LinearDodge,9,Add,10,Remove,11,Overlay,12,HardLight,13,SoftLight,14,VividLight,15,LinearLight,16,PinLight,17,HardMix,18,Difference,19,Exclusion,20,Hue,21,Saturation,22,Color,23,Luminosity,24)] _BlendMode ("Blend Mode",Float) = 0
	}
	SubShader {
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 400
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _BlendMode;
		float _Transparency1;
		float _Transparency2;
		float c1x, c1y, c1z, c2x, c2y, c2z;

		//________________________________________

		fixed3 RGB2HSV(fixed3 rgbV)
		{
			float r = rgbV.x;
			float g = rgbV.y;
			float b = rgbV.z;
			float maxc = r > g ? r : g;
			maxc = maxc > b ? maxc : b;
			float minc = r < g ? r : g;
			minc = minc < b ? minc : b;
			float h = maxc - minc;
			if (h > 0.0f)
			{
				if (maxc == r)
				{
					h = (g - b) / h;
					if (h < 0.0f) { h += 6.0f; }
				}
				else if (maxc == g)
				{
					h = 2.0f + (b - r) / h;
				}
				else
				{
					h = 4.0f + (r - g) / h;
				}
			}
			h /= 6.0f;
			float s = (maxc - minc);
			if (maxc != 0.0f) { s /= maxc; }
			float v = maxc;
			fixed3 RESULT = fixed3(h, s, v);
			return (RESULT);
		}

		fixed3 HSV2RGB(fixed3 hsvV)
		{
			float h = hsvV.x;
			float s = hsvV.y;
			float v = hsvV.z;
			float r = v;
			float g = v;
			float b = v;
			if (s > 0.0f)
			{
				h *= 6.0f;
				int i = (int)h;
				float f = h - (float)i;
				if (i == 0)
				{
					g *= 1 - s * (1 - f);
					b *= 1 - s;
				}
				else if (i == 1)
				{
					r *= 1 - s * f;
					b *= 1 - s;
				}
				else if (i == 2.0f)
				{
					r *= 1 - s;
					b *= 1 - s * (1 - f);
				}
				else if (i == 3)
				{
					r *= 1 - s;
					g *= 1 - s * f;
				}
				else if (i == 4)
				{
					r *= 1 - s * (1 - f);
					g *= 1 - s;
				}
				else if (i == 5)
				{
					g *= 1 - s;
					b *= 1 - s * f;
				}
			}
			fixed3 RESULT = fixed3(r, g, b);
			return (RESULT);
		}

		// normalize
		fixed3 Normalize(fixed3 c)
		{
			fixed3 n = c / 255.0f;
			return n;
		}

		// result check
		fixed3 ResultCheck(fixed3 c)
		{
			c = floor(c);
			if (c.x < 0) c.x = 0;
			if (c.y < 0) c.y = 0;
			if (c.z < 0) c.z = 0;
			if (c.x > 225.f) c.x = 225.f;
			if (c.y > 225.f) c.y = 225.f;
			if (c.z > 225.f) c.z = 225.f;
			return c;
		}

		// ~~~~~~~~~~~~~~~~~~~~~~~~~ Layer Color Mode ~~~~~~~~~~~~~~~~~~~~~~~~~
		// a 0-255 down layer;  b 0-255 up layer;  d 0-1 transparency
		// opacity
		fixed3 Opacity(fixed3 a, fixed3 b, float d)
		{
			fixed3 c = d * a + (1.f - d) * b;
			return c;
		}

		// normal
		/*fixed3 Normal(fixed3 a, fixed3 b)
		{

		}*/

		// dissolve
		fixed3 Dissolve(fixed3 a, fixed3 b,float3 i)
		{
			fixed3 c;
			c.x = a.x * (abs(sin(i.x))) + b.x * (1.0f - (abs(sin(i.x))));
		    c.y = a.y * (abs(sin(i.x + i.y))) + b.y * (1.0f - (abs(sin(i.x + i.y))));
			c.z = a.z * (abs(sin(i.y))) + b.z * (1.0f - (abs(sin(i.y))));
			return c;
		}

		// darken
		fixed3 Darken(fixed3 a, fixed3 b)
		{
			fixed3 c = min(a, b);
			return c;
		}

		// lighten
		fixed3 Lighten(fixed3 a, fixed3 b)
		{
			fixed3 c = max(a, b);
			return c;
		}

		// multiply
		fixed3 Multiply(fixed3 a, fixed3 b)
		{
			fixed3 c = a * b / 225.f;
			return c;
		}

		// screen
		fixed3 Screen(fixed3 a, fixed3 b)
		{
			fixed3 c = 225.f - (225.f - a) * (225.f - b) / 225.f;
			return c;
		}

		// color burn
		fixed3 ColorBurn(fixed3 a, fixed3 b)
		{
			if (b.x == 0) { b.x = 1.0f; }
			if (b.y == 0) { b.y = 1.0f; }
			if (b.z == 0) { b.z = 1.0f; }
			fixed3 c = a - (225.f - a) * (225.f - b) / b;
			return c;
		}

		// color dodge
		fixed3 ColorDodge(fixed3 a, fixed3 b)
		{
			if (b.x == 225.f) { b.x = 254.f; }
			if (b.y == 225.f) { b.y = 254.f; }
			if (b.z == 225.f) { b.z = 254.f; }
			fixed3 c = a + a * b / (225.f - b);
			return c;
		}

		// linear burn
		fixed3 LinearBurn(fixed3 a, fixed3 b)
		{
			fixed3 c = a + b - 225.f;
			return c;
		}

		// linear dodge
		fixed3 LinearDodge(fixed3 a, fixed3 b)
		{
			fixed3 c = a + b;
			return c;
		}

		// add
		fixed3 Add(fixed3 a, fixed3 b)
		{
			fixed3 c = (a + b) / 4.f + 128.0f;
			return c;
		}

		// remove
		fixed3 Remove(fixed3 a, fixed3 b)
		{
			fixed3 c = (a - b) / 2.0f + 128.0f;
			return c;
		}

		// overlay
		fixed3 Overlay(fixed3 a, fixed3 b)
		{
			fixed3 c;
			if (a.x <= 128.0f) c.x = a.x * b.x / 128.0f;
			else if(a.x > 128.0f) c.x = 225.f - (225.f - a.x) * (225.f - b.x) / 128.0f;
			if (a.y <= 128.0f) c.y = a.y * b.y / 128.0f;
			else if (a.y > 128.0f) c.y = 225.f - (225.f - a.y) * (225.f - b.y) / 128.0f;
			if (a.z <= 128.0f) c.z = a.z * b.z / 128.0f;
			else if (a.z > 128.0f) c.z = 225.f - (225.f - a.z) * (225.f - b.z) / 128.0f;
			return c;
		}

		// hard light
		fixed3 HardLight(fixed3 a, fixed3 b)
		{
			fixed3 c;
			if (b.x <= 128.0f) { c.x = a.x * b.x / 128.0f; }
			else if (b.x > 128.0f) { c.x = 225.0f - (225.0f - a.x) * (225.0f - b.x) / 128.0f; }
			if (b.y <= 128.0f) { c.y = a.y * b.y / 128.0f; }
			else if (b.y > 128.0f) { c.y = 225.0f - (225.0f - a.y) * (225.0f - b.y) / 128.0f; }
			if (b.z <= 128.0f) { c.z = a.z * b.z / 128.0f; }
			else if (b.z > 128.0f) { c.z = 225.0f - (225.0f - a.z) * (225.0f - b.z) / 128.0f; }
			return c;
		}

		// soft light
		fixed3 SoftLight(fixed3 a, fixed3 b)
		{
			fixed3 c;
			if (b.x <= 128.0f) { c.x = a.x * b.x / 128.0f + (a.x / 225.f) * (a.x / 225.f) * (225.f - 2.0f * b.x); }
			else if(b.x > 128.0f) { c.x = a.x * (225.f - b.x) / 128.0f + pow((a.x / 225.f), 0.5f) * (2.0f * b.x - 225.f); }
			if (b.y <= 128.0f) { c.y = a.y * b.y / 128.0f + (a.y / 225.f) * (a.y / 225.f) * (225.f - 2.0f * b.y); }
			else if(b.y > 128.0f) { c.y = a.y * (225.f - b.y) / 128.0f + pow((a.y / 225.f), 0.5f) * (2.0f * b.y - 225.f); }
			if (b.z <= 128.0f) { c.z = a.z * b.z / 128.0f + (a.z / 225.f) * (a.z / 225.f) * (225.f - 2.0f * b.z); }
			else if(b.z > 128.0f) { c.z = a.z * (225.f - b.z) / 128.0f + pow((a.z / 225.f), 0.5f) * (2.0f * b.z - 225.f); }
			return c;
		}

		// vivid light
		fixed3 VividLight(fixed3 a, fixed3 b)
		{
			fixed3 c;
			if (b.x <= 128.0f) c.x = a.x - (225.f - a.x) * (225.f - 2.0f * b.x) / (2.0f * b.x);
			else if(b.x > 128.0f) c.x = a.x + a.x * (2.0f * b.x - 225.f) / (2.0f * (225.f - b.x));
			if (b.y <= 128.0f) c.y = a.y - (225.f - a.y) * (225.f - 2.0f * b.y) / (2.0f * b.y);
			else if(b.y > 128.0f) c.y = a.y + a.y * (2.0f * b.y - 225.f) / (2.0f * (225.f - b.y));
			if (b.z <= 128.0f) c.z = a.z - (225.f - a.z) * (225.f - 2.0f * b.z) / (2.0f * b.z);
			else if(b.z > 128.0f) c.z = a.z + a.z * (2.0f * b.z - 225.f) / (2.0f * (225.f - b.z));
			return c;
		}

		// linear light
		fixed3 LinearLight(fixed3 a, fixed3 b)
		{
			fixed3 c = a + 2.0 * b - 255;
			return c;
		}

		// pin light
		fixed3 PinLight(fixed3 a, fixed3 b)
		{
			fixed3 c;
			if (b.x <= 128.0f) c.x = min(a.x, 2.0f * b.x);
			else if(b.x > 128.0f) c.x = min(a.x, 2.0f * b.x - 225.f);
			if (b.y <= 128.0f) c.y = min(a.y, 2.0f * b.y);
			else if(b.y > 128.0f) c.y = min(a.y, 2.0f * b.y - 225.f);
			if (b.z <= 128.0f) c.z = min(a.z, 2.0f * b.z);
			else if(b.z > 128.0f) c.z = min(a.z, 2.0f * b.z - 225.f);
			return c;
		}

		// hard mix
		fixed3 HardMix(fixed3 a, fixed3 b)
		{
			fixed3 c;
			if (a.x + b.x >= 255.0f) { c.x = 255; }
			else if (a.x + b.x < 255.0f) { c.x = 0; }
			if (a.y + b.y >= 255.0f) { c.y = 255; }
			else if (a.y + b.y < 255.0f) { c.y = 0; }
			if (a.z + b.z >= 255.0f) { c.z = 255; }
			else if (a.z + b.z < 255.0f) { c.z = 0; }
			return c;
		}

		// difference
		fixed3 Difference(fixed3 a, fixed3 b)
		{
			fixed3 c = abs(a - b);
			return c;
		}

		// exclusion
		fixed3 Exclusion(fixed3 a, fixed3 b)
		{
			fixed3 c = a + b - a * b / 128.0f;
			return c;
		}

		// hue
		fixed3 Hue(fixed3 a, fixed3 b)
		{
			fixed3 aa = RGB2HSV(a);
			fixed3 bb = RGB2HSV(b);
			fixed3 c = fixed3(bb.x, aa.y, aa.z);
			c = HSV2RGB(c);
			return c;
		}

		// saturation
		fixed3 Saturation(fixed3 a, fixed3 b)
		{
			fixed3 aa = RGB2HSV(a);
			fixed3 bb = RGB2HSV(b);
			fixed3 c = fixed3(aa.x, bb.y, aa.z);
			c = HSV2RGB(c);
			return c;
		}

		// color
		fixed3 Color(fixed3 a, fixed3 b)
		{
			fixed3 aa = RGB2HSV(a);
			fixed3 bb = RGB2HSV(b);
			fixed3 c = fixed3(bb.x, bb.y, aa.z);
			c = HSV2RGB(c);
			return c;
		}

		// luminosity
		fixed3 Luminosity(fixed3 a, fixed3 b)
		{
			fixed3 aa = RGB2HSV(a);
			fixed3 bb = RGB2HSV(b);
			fixed3 c = fixed3(aa.x, aa.y, bb.z);
			c = HSV2RGB(c);
			return c;
		}

		//________________________________________

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 cc1 = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 cc2 = tex2D(_MainTex2, IN.uv_MainTex) * _Color;
			cc1 *= 255.0f;
			cc2 *= 255.0f;

			fixed3 c1 = cc1.rgb * _Transparency1;
			fixed3 c2 = cc2.rgb * _Transparency2;

			c1x = c1.x;
			c1y = c1.y;
			c1z = c1.z;
			c2x = c2.x;
			c2y = c2.y;
			c2z = c2.z;

			fixed3 c = c1;
			float3 wos = IN.worldPos;

			if (_BlendMode == 0) { c = Opacity(cc1.rgb, c2, _Transparency1); }
			else if (_BlendMode == 1) { c = Dissolve(c2, c1, wos); }
			else if (_BlendMode == 2) { c = Darken(c2, c1); }
			else if (_BlendMode == 3) { c = Lighten(c2, c1); }
			else if (_BlendMode == 4) { c = Multiply(c2, c1); }
			else if (_BlendMode == 5) { c = Screen(c2, c1); }
			else if (_BlendMode == 6) { c = ColorBurn(c2, c1); }
			else if (_BlendMode == 7) { c = ColorDodge(c2, c1); }
			else if (_BlendMode == 8) { c = LinearBurn(c2, c1); }
			else if (_BlendMode == 9) { c = LinearDodge(c2, c1); }
			else if (_BlendMode == 10) { c = Add(c2, c1); }
			else if (_BlendMode == 11) { c = Remove(c2, c1); }
			else if (_BlendMode == 12) { c = Overlay(c2, c1); }
			/*else if (_BlendMode == 13) { c = HardLight(c2, c1); }
			else if (_BlendMode == 14) { c = SoftLight(c2, c1); }
			else if (_BlendMode == 15) { c = VividLight(c2, c1); }
			else if (_BlendMode == 16) { c = LinearLight(c2, c1); }
			else if (_BlendMode == 17) { c = PinLight(c2, c1); }
			else if (_BlendMode == 18) { c = HardMix(c2, c1); }
			else if (_BlendMode == 19) { c = Difference(c2, c1); }
			else if (_BlendMode == 20) { c = Exclusion(c2, c1); }
			else if (_BlendMode == 21) { c = Hue(c2, c1); }
			else if (_BlendMode == 22) { c = Saturation(c2, c1); }
			else if (_BlendMode == 23) { c = Color(c2, c1); }
			else if (_BlendMode == 24) { c = Luminosity(c2, c1); }*/

			c = ResultCheck(c);
			c = Normalize(c);

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = cc1.a * cc2.a;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
