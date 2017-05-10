Shader "universe/universeShader02"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		//Tags { "RenderType"="Opaque" }
		//LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			#define iterations 17
			#define formuparam .53

			#define volsteps 20
			#define stepsize .1

			#define zoom .800
			#define tile .850
			#define speed 0.010

			#define brightness .0003
			#define distfading .690
			#define saturation .850

			uniform sampler2D _MainTex;

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			    float time = _Time.x / 16.;
			    float3 from = float3(1.,1.,1.);
			    from += float3(time*2.,time,-2.);
				//coords   direction
			    float2 uvc = i.uv;
			    float3 dir = float3(uvc*zoom,1.);
			    // rending
			    float s = .25 * abs(sin(time * 2.025)), fade = .24;
			    float3 v = float3(0.,0.,0.);
			    for(int r = 0; r < volsteps; r++){
				    float3 p = from + s * dir * .5;
				    //  tiling fold 
				    p = abs( float3(tile,tile,tile) - fmod(p,float3(tile * 2.,tile * 2.,tile * 2.)) );
				    float prep, pa;
				    pa = prep = 0.;
				    for (int i = 0; i < iterations; i++){
					    // the magic formula
					    p = abs(p) / dot(p,p) - formuparam;  //    i.e.   p = abs(p) / pow(abs(p),2.) - formuparam;
					    // absolute sum of average change
					    pa += abs(length(p) - prep);
					    prep = length(p);
				    }
				    // add contrast
				    pa *= pa * pa;
				    // coloring based on distance
				    v+=float3(s*s*s*s,s*s,s* ((abs(cos(time)))*.7+.3) )*pa*brightness*fade + fade*.1;
				    // distance fading
				    fade *= distfading;
				    s += stepsize;
			    }
			    // color adjust
				v = lerp(float3(length(v),length(v),length(v)),v,saturation); 
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				col *= .75;
				col = fixed4((fixed3)v * col.rgb,1.);
				//col = fixed4((fixed3)v,1.);
				return col;
			}
			ENDCG
		}
	}
}
