Shader "Custom/Outline_Shader_01"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Amount("Outline Width", Range(0,2.0)) = 0.04
		_Color("Color", Color) = (0.5,1,1,1)
	}
	SubShader
	{
		//Tags { "Queue" = "Geometry+1" "RenderType" = "Opaque" }
		Tags {"Queue" = "Transparent"  }
		LOD 200

		Pass
		{
			Cull Off
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			half _Amount;
			fixed4 _Color;

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};
			
			v2f vert (appdata_base v)
			{
				v2f o;
				v.vertex.xyz += v.normal * _Amount;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _Color;
			}
		ENDCG
		}

		UsePass "Custom/Outline_Shader_00/NORMALPASS"
	}
}
