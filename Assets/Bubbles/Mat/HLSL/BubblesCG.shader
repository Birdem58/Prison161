Shader "Jettelly/BubblesCG"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _Cube ("CubeTex", Cube) = "grey" {}
        _LOD ("Cubemap LOD", Range(0,8)) = 0
        _Brightness ("Brightness", Range(0, 7)) = 7 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_particles

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;                
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				half3 worldRefl : TEXCOORD1;
            };

            sampler2D _MainTex;
            samplerCUBE _Cube;
            float4 _MainTex_ST;
            half _LOD;
            half _Brightness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float3 worldNormal = UnityObjectToWorldNormal(v.normal.xyz);
				o.worldRefl = reflect(float3(v.uv.x - 0.5, v.uv.y - 0.5, 0.5), worldNormal);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 reflection = texCUBEbias (_Cube, half4(i.worldRefl,_LOD));
                fixed4 bubble = tex2D (_MainTex, i.uv);
                fixed4 col = 0;
                col.rgb = (reflection.rgb * bubble.rgb);
                col += col * _Brightness;
				col.a = (reflection.r + reflection.g + reflection.b) * reflection.a * bubble.a;
                return col;
            }
            ENDCG
        }
    }
}
