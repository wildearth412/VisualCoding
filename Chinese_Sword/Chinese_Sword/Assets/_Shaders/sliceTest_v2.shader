Shader "Example/Slices" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _BumpMap ("Bumpmap", 2D) = "bump" {}
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      Cull Off
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
          float2 uv_MainTex;
          float2 uv_BumpMap;
          float3 worldPos;
      };
      sampler2D _MainTex;
      sampler2D _BumpMap;
      void surf (Input IN, inout SurfaceOutput o) {
          float w = sin(_Time.xyz * 0.07)  + 0.002;
          float ss = sin(_Time.y * 0.05 ) + 0.001;
          float cc = cos(_Time.y * 0.05 ) + 0.001;
//          clip (frac((IN.worldPos.x*ss+IN.worldPos.y*(1-abs(ss))+IN.worldPos.z*cc) * 20)-ss);
          clip (frac((IN.worldPos.x*(ss + ss)+IN.worldPos.y*w*w+IN.worldPos.z*(ss + cc)) * 6)-0.85);
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
          o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }