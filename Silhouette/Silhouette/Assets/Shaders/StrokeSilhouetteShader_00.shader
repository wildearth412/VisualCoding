
// iteration stroke silhouette algorithm

Shader "Custom/StrokeSilhouette_00" 
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Weight("Outline Weight", Range(1,4)) = 1
		_Threshold("Threshold", Range(0,0.05)) = 0.005    // i ki chi
		_Amount("Amount",Range(0,100)) = 15
		//_Color("Color", Color) = (0.5,1,1,1)
	}
	SubShader
	{
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			Cull Off
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float4 _MainTex_TexelSize;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			int _Weight;
			float _Threshold;
			float _Amount;
			//fixed4 _Color;

			fixed3 StrokeSilhouette(float2 uv)
			{
				//float4 duv = float4(0,0,_MainTex_TexelSize.xy);   // TexelSize:  x: 1.0/width  y: 1.0/height  z: width  w: height
				float dw = _MainTex_TexelSize.x;
				float dh = _MainTex_TexelSize.y;

				int wt = _Weight;
				float mind = 999.0f;

				for (int i = -wt; i <= wt; i++)
				{
					for (int j = -wt; j <= wt; j++)
					{
						if (i*j != 0)    // except itself
						{
							float2 duv = float2(dw * (float)i, dh * (float)j);
							float3 dc = tex2D(_MainTex, uv + duv).xyz;
							float3 c = tex2D(_MainTex, uv).xyz;
							c = c - dc;
							float d = length(c);      // color difference
							mind = min(mind, d);
						}
					}
				}

				mind = mind < _Threshold ? 0 : mind - _Threshold;
				float3 lc = float3(saturate(mind*_Amount), saturate(mind*_Amount), saturate(mind*_Amount));
				return lc;
			}

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = float4(StrokeSilhouette(i.uv).xyz,1.0f);
				return col;
			}
			ENDCG
		}
	}
}