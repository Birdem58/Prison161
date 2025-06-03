Shader "PostEffect/Dithering"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            Name "DitheringPass"
            Cull Off ZWrite Off ZTest Always

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;

            float _DitherStrength;
            float _DitherThreshold;
            float _DitherScale;
            int _PatternIndex;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            Varyings Vert(Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.positionHCS);
                return o;
            }

            float4x4 GetDitherPattern(int index)
            {
                float4x4 pattern;
                if (index == 0)
                {
                    pattern = float4x4(
                        0, 1, 0, 1,
                        1, 0, 1, 0,
                        0, 1, 0, 1,
                        1, 0, 1, 0
                    );
                }
                else
                {
                    pattern = float4x4(0.25, 0.75, 0.25, 0.75,
                                       0.75, 0.25, 0.75, 0.25,
                                       0.25, 0.75, 0.25, 0.75,
                                       0.75, 0.25, 0.75, 0.25);
                }
                return pattern;
            }

            float GetBrightness(float3 color)
            {
                return dot(color, float3(0.3, 0.59, 0.11));
            }

            float GetDitherMask(float2 uv, float brightness, float4x4 pattern)
            {
                int2 coord = int2(fmod(uv, 4));
                float threshold = pattern[coord.y][coord.x];
                return step(threshold, brightness * _DitherThreshold);
            }

            float4 Frag(Varyings i) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float2 ditherUV = floor(screenUV * _ScreenParams.xy / _DitherScale);

                float brightness = GetBrightness(color.rgb);
                float4x4 pattern = GetDitherPattern(_PatternIndex);
                float mask = GetDitherMask(ditherUV, brightness, pattern);

                return float4(color.rgb * lerp(1.0, mask, _DitherStrength), color.a);
            }

            ENDHLSL
        }
    }
}
