Shader "Jettelly/Bubble"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        [Header(REFLECTION)]
        _ReflectionFactor ("Reflection Factor", Range(0, 1)) = 1        
        _Cube ("Reflection Map", Cube) = "" {}
        _Detail ("Reflection Detail", Range(1, 9)) = 1.0
        _ReflectionExposure ("Reflection Exposure", Range(1, 2)) = 1.0

        [Header(FRESNEL)]
        _FresnelFactor ("Fresnel Factor", Range(0, 1)) = 1
        _FresnelPower ("Fresnel Power", Range(0, 10)) = 3 

        [Header(RGB)]
        _Iter ("Iterations", Range(10, 30)) = 10
        _R ("R", Range(0, 1)) = 0.5
        _G ("G", Range(0, 1)) = 0.5
        _B ("B", Range(0, 1)) = 0.5

        [Header(DISTORTION)]
        _DisSpeed ("Distortion Speed", Range(0.0, 0.1)) = 0.1
        _DisValue ("Distortion Value", Range(2, 10)) = 3

        [Header(SPECULAR)]
        _SpecularFactor ("Specular Factor", Range(0, 1)) = 1
        _SpecularPower ("Specular Power", Range(1, 200)) = 1  
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"            

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD2;
                float3 worldPos : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _DisValue;
            float _DisSpeed;

            float _FresnelFactor;
            float _FresnelPower;

            samplerCUBE _Cube;
            float _Detail;
            float _ReflectionExposure;
            float _ReflectionFactor;
            float _EmissionPower;

            float _R;
            float _G;
            float _B;
            float _Iter;

            float _SpecularFactor;
            float _SpecularPower;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            void FresnelEffect_float(float3 WorldNormal, float3 ViewDir, float Power, out float Out)
            {
                Out = pow((1.0 - saturate(dot(normalize(WorldNormal), normalize(ViewDir)))), Power);
            }

            void Reflection_float(samplerCUBE CubeMap, float Detail, float3 WorldReflection, float Exposure, float ReflectionFactor, out float3 Out)
            {
                float4 cubeMapColor = texCUBElod(CubeMap, float4(WorldReflection, Detail)).rgba;
                Out = ReflectionFactor * cubeMapColor.rgb * (cubeMapColor.a * Exposure);
            }

            void Specular_float(float3 NormalDir, float3 LightDir, float3 ViewDir, float3 SpecularColor, float SpecularFactor, float Attenuation, float SpecularPower, out float3 Out)
            {
                float3 halfwayDir = normalize(LightDir + ViewDir);
                Out = SpecularColor * SpecularFactor * Attenuation * pow(max(0, dot(normalize(NormalDir), normalize(halfwayDir))), SpecularPower);
            } 

            fixed4 frag (v2f i) : SV_Target
            {
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                float3 lightDir = _WorldSpaceLightPos0.xyz;

                float fresnel = 0;
                FresnelEffect_float(i.worldNormal, viewDir, _FresnelPower, fresnel);
                fresnel *= _FresnelFactor;

                float3 reflectionColor = 0;
                float3 worldReflection = reflect(-viewDir, i.worldNormal);
                Reflection_float(_Cube, _Detail, worldReflection, _ReflectionExposure, _ReflectionFactor, reflectionColor);

                float3 specularColor = 0;
                Specular_float(i.worldNormal, lightDir, viewDir, float3(5, 5, 5), _SpecularFactor, 1, _SpecularPower, specularColor);

                float3 c = normalize(_WorldSpaceCameraPos);
                float sinCol = (sin(_Time.y) * 0.5 + 0.5) * c;
                float3 gradCol = sin(float3(_R * sinCol, _G, _B * sinCol) * fresnel * _Iter) * 0.5 + 0.5;
                gradCol -= (1 - fresnel);
                
                fixed distortion = tex2D(_MainTex, i.uv + (_Time * _DisSpeed)).r;
                i.uv += distortion / _DisValue;

                
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= gradCol + (col.a * 0.5);
                col.rgb += reflectionColor;
                col.rgb += specularColor;

                fixed alpha = saturate(fresnel + (col.a * 0.2));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return float4(col.rgb, alpha);
            }
            ENDCG
        }
    }
}
