Shader "Jettelly/DynamicWarpDrive"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _VortexFade ("Vortex Fade", Range(1, 2)) = 1
        _WarpSpeed ("Warp Speed", Range(0, 5)) = 1        
        _WarpDepth ("Warp Depth", Range(0.0, 0.5)) = 0.4
        _WarpFade ("Warp Fade", Range(0, 2)) = 1
        _TileX ("Warp Tile X", Range(1, 10)) = 5
        _TileY ("Warp Tile Y", Range(1, 10)) = 5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend One One
        ZWrite Off
        Cull Off
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uvMask : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 uvMask : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _WarpSpeed;
            float _VortexFade;
            float _WarpDepth;
            float _WarpFade;
            float _TileX;
            float _TileY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uvMask = v.uvMask;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float noise(float2 p)
            {
                p = frac(p * float2(323.33, 656.45));
                p += dot(p, p + 15.42);
                return frac(p.x * p.y);
            }

            float warpTex(float2 uv)
            {
                float b = step(uv.x, _WarpDepth);
                float t = step(1 - uv.x, _WarpDepth);
                t += b;

                float f = smoothstep(uv.y, uv.y - _WarpFade, 0.5);
                float d = smoothstep(1 - uv.y,1 - uv.y - _WarpFade, 0.5);
                float cut = clamp(f + d, 0, 1);

                return clamp((1 - t) * (1 - cut), 0, 1);
            }

            fixed4 frag (v2f i) : SV_Target
            {     
                i.uv.x *= _TileX;  
                i.uv.y *= _TileY;

                i.uv.y += (_Time.y * _WarpSpeed);

                float2 gv = float2(frac(i.uv.x), frac(i.uv.y));
                float2 id = floor(i.uv);

                float y = 0;
                float x = 0;
                fixed4 col = 0;
                
                for(y = -1; y <= 1; y++)
                {
                    for(x = -1; x <= 1; x++)
                    {
                        float2 offset = float2(x, y);
                        float n = noise(id + offset); 
                        float size = frac(n * 123.32);  
                        float warp = warpTex(gv - offset - float2(n, frac(n * 56.12)));                   
                        col += warp * size;
                    }
                }

                float v = abs(i.uvMask.y - .5) * _VortexFade;
                return clamp(col - v, 0, 1) * _Color;
            }
            ENDHLSL
        }
    }
}
