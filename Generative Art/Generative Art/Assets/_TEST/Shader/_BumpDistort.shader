
Shader "Custom/BumpDistort" {
Properties {
	_BumpAmt  ("Distortion", range (0,128)) = 10
	//_MainTex ("Tint Color (RGB)", 2D) = "white" {}
	_Alpha("Alpha", 2D) = "black" {}
	_AnumX("Alpha Num X", Int) = 8
	_AnumY("Alpha Num Y", Int) = 4
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_ScaleX("Scale X", Float) = 1.0
	_ScaleY("Scale Y", Float) = 1.0
}

Category {

	Tags { "Queue"="Transparent" "IgnoreProjector" = "True" "RenderType"="Opaque" }
	Blend SrcAlpha OneMinusSrcAlpha

	SubShader {

		GrabPass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
		}
		
		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		struct appdata_t {
			float4 vertex : POSITION;
			float2 texcoord: TEXCOORD0;
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			float4 uvgrab : TEXCOORD0;
			float2 uvbump : TEXCOORD1;
			float2 uvmain : TEXCOORD2;
			float2 uvalpha : TEXCOORD3;
		};

		float _BumpAmt;
		float4 _BumpMap_ST;
		float4 _Alpha_ST;
		int _AnumX;
		int _AnumY;
		float _Ascale;
		
		//float4 _MainTex_ST;
		float _ScaleX;
		float _ScaleY;

		v2f vert (appdata_t v)
		{
			v2f o;
			//o.vertex = UnityObjectToClipPos(v.vertex);
			o.vertex = mul(UNITY_MATRIX_P,mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0))+ float4(v.vertex.x, v.vertex.y, 0.0, 0.0)* float4(_ScaleX, _ScaleY, 1.0, 1.0));
			o.uvgrab = ComputeGrabScreenPos(o.vertex);
			o.uvbump = TRANSFORM_TEX( v.texcoord, _BumpMap );
			//o.uvmain = TRANSFORM_TEX( v.texcoord, _MainTex );
			o.uvalpha = TRANSFORM_TEX( v.texcoord, _Alpha );
			float _AofsX = 1.0f / _AnumX;
			float _AofsY = 1.0f / _AnumY;
			//o.uvalpha = float2( (float)floor(o.uvalpha.x / _AnumX + _AofsX * fmod(_Time.y,_AnumX) ), (float)floor( o.uvalpha.y / _AnumY + _AofsY * fmod(_Time.y, _AnumX * _AnumY) / _AnumX ) );
			o.uvalpha = float2(o.uvalpha.x / _AnumX + _AofsX * floor(fmod(_Time.y, _AnumX)), o.uvalpha.y / _AnumY + _AofsY * floor(fmod(_Time.y, _AnumX * _AnumY) / _AnumX) );
			return o;
		}

		sampler2D _GrabTexture;
		float4 _GrabTexture_TexelSize;
		sampler2D _BumpMap;
		sampler2D _Alpha;
		//sampler2D _MainTex;

		half4 frag (v2f i) : SV_Target
		{
			#if UNITY_SINGLE_PASS_STEREO
			i.uvgrab.xy = TransformStereoScreenSpaceTex(i.uvgrab.xy, i.uvgrab.w);
			#endif

			half2 bump = UnpackNormal(tex2D( _BumpMap, i.uvbump )).rg; 
			float2 offset = bump * _BumpAmt * _GrabTexture_TexelSize.xy;
			#ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE
				i.uvgrab.xy = offset * UNITY_Z_0_FAR_FROM_CLIPSPACE(i.uvgrab.z) + i.uvgrab.xy;
			#else
				i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
			#endif

			half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
			//half4 tint = tex2D(_MainTex, i.uvmain);
			fixed4 alpha = tex2D(_Alpha,i.uvalpha);
			//col *= tint;
			col.a = alpha.a;

			return col;
		}
		ENDCG
				}
			}

			/*SubShader {
				Blend DstColor Zero
				Pass {
					Name "BASE"
					SetTexture [_MainTex] {	combine texture }
				}
			}*/
		}

}
