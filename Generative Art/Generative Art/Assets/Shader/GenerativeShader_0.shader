Shader "Custom/GenerativeShader_0"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,1)	
		_NumX("Num X",Range(1,100)) = 5
		_NumY("Num Y",Range(1,100)) = 5
		_Threshold("Threshold",Range(0,0.1)) = 0.05
		_Scale("Scale",Range(0,5)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			//uniform float4 _MainTex_TexelSize;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _TintColor;
			int _NumX;
			int _NumY;
			float _Threshold;
			float _Scale;
			
			float2 UVOffset(float2 uv)
			{
				float dw = 1.0f/_NumY;
				float dh = 1.0f/_NumY;

				float2 idx = float2((float)floor(uv.x / dw), (float)floor(uv.y / dh));               // cell idx     _NumX - _NumY
				float2 ccuv = float2((idx.x * dw + dw / 2.0f), (idx.y * dh + dh / 2.0f));                                  // cell center uv
				float2 outuv = abs( distance(uv, ccuv) - max(dw, dh) * _Scale ) > _Threshold  ? ccuv : float2(0.5f, 0.5f);
				return outuv;

			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//v.uv = UVOffset(v.uv);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, UVOffset(i.uv));
				col *= _TintColor;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
