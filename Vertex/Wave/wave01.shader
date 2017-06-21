Shader "universe/wave/wave01"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Amount ("Extrusion Amount", Range(-1,1)) = 0.5

		_c ("Wave Velocity", Range(0,10)) = 1
		_d ("Distance Interval", Range(0.1,10)) = 1
		_t ("Time Step", Range(0.01,10)) = 1
		_mu ("Viscosity Coefficient", Range(0,10)) = 1

		_cpu ("Collision Point U", Range(0,100)) = 50
		_cpv ("Collision Point V", Range(0,100)) = 50
	}
	SubShader
	{
	    Tags { "RenderType" = "Opaque" }
	    LOD 200
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			#define iterations 17
			#define formuparam .53


			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				uint vid : SV_VertexID;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Amount;

			float3 buf0[100];
			float3 buf1[100];
			int bufIdx;

			float _c;
			float _d;
			float _t;
			float _mu;
			float k1;
			float k2;
			float k3;

			v2f vert (appdata v)
			{
			    float f1 = _c*_c*_t*_t/(_d * _d);
			    float f2 = 1.0f / (_mu * _t + 2.0f);
			    k1 = (4.0f - 8.0f * f1) * f2;
			    k2 = (_mu * _t - 2.0f) * f2;
			    k3 = 2.0f * f1 * f2;

				v2f o;
				v.vertex.xyz += v.normal * _Amount;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv,_MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
//			    float time = _Time.x / 16.;
//			    float3 from = float3(1.,1.,1.);
//			    from += float3(time*2.,time,-2.);
//				//coords   direction
//			    float2 uvc = i.uv;
//			    float3 dir = float3(uvc*zoom,1.);



			    // color adjust
				//v = lerp(float3(length(v),length(v),length(v)),v,saturation); 
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				col *= .75;
				//col = fixed4((fixed3)v * col.rgb,1.);
				//col = fixed4((fixed3)v,1.);
				return col;
			}
			ENDCG
		}
	}
}
