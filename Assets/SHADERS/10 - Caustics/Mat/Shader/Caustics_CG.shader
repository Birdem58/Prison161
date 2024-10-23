Shader "Jettelly/Caustics_CG"
{
    Properties
    {
        [Header(TEXTURE SETTINGS)]
        _Texture ("Texture", 2D) = "white" {}         
        _AmbientFactor ("Ambient Factor", Range(0, 1)) = 1            

        [Header(CAUSTIC SETTINGS)]
        _CausticTex ("Texture", 2D) = "white" {} 
        _Degree ("Caustic Texture Rotation", Range(0, 180)) = 90
        _Speed ("Caustic Global Speed", Range(0, 1)) = .15
        _SpeedDis ("Caustic Distortion Speed", Range(-1, 1)) = 1
        _DistortionFactor ("Caustic Distortion Factor", Range(1, 3)) = 3    
        _Brightness ("Caustic Brightness", Range(0, 1)) = 1
        _Tile ("Caustic Tile", Range(0.1, 0.2)) = 0.15
    }
    SubShader
    {
        Tags {"Queue"="Geometry" "RenderType"="Opaque" }
        LOD 100               

        Pass
        {
            Tags {"LightMode"="ForwardBase"}
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #include "UnityCG.cginc"
            #include "Lighting.cginc"            
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;  
                SHADOW_COORDS(1) 
                float3 normal : NORMAL;
                float3 worldPos : TEXCOORD2;
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
            };

            sampler2D _CausticTex;
            sampler2D _Texture;
            float _DistortionFactor;
            float _AmbientFactor;
            float _Degree;
            float _Speed;
            float _Brightness;
            float _SpeedDis;
            float _Tile;

            inline float unity_noise_randomValue (float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
            }

            inline float unity_noise_interpolate (float a, float b, float t)
            {
                return (1.0-t)*a + (t*b);
            }

            inline float unity_valueNoise (float2 uv)
            {
                float2 i = floor(uv);
                float2 f = frac(uv);
                f = f * f * (3.0 - 2.0 * f);

                uv = abs(frac(uv) - 0.5);
                float2 c0 = i + float2(0.0, 0.0);
                float2 c1 = i + float2(1.0, 0.0);
                float2 c2 = i + float2(0.0, 1.0);
                float2 c3 = i + float2(1.0, 1.0);
                float r0 = unity_noise_randomValue(c0);
                float r1 = unity_noise_randomValue(c1);
                float r2 = unity_noise_randomValue(c2);
                float r3 = unity_noise_randomValue(c3);

                float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
                float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
                float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
                return t;
            }

            void Unity_SimpleNoise_float(float2 UV, float Scale, out float Out)
            {
                float t = 0.0;

                float freq = pow(2.0, 0);
                float amp = pow(0.5, 3);
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                freq = pow(2.0, 1);
                amp = pow(0.5, 2);
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                freq = pow(2.0, 2);
                amp = pow(0.5, 1);
                t += unity_valueNoise(float2(UV.x*Scale/freq, UV.y*Scale/freq))*amp;

                Out = t;
            }

            void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
            {
                Rotation = Rotation * (3.1415926f/180.0f);
                UV -= Center;
                float s = sin(Rotation);
                float c = cos(Rotation);
                float2x2 rMatrix = float2x2(c, -s, s, c);
                rMatrix *= 0.5;
                rMatrix += 0.5;
                rMatrix = rMatrix * 2 - 1;
                UV.xy = mul(UV.xy, rMatrix);
                UV += Center;
                Out = UV;
            }

            float4 Unity_Texture_Projection_float(sampler2D Tex, float3 worldNormal, float3 worldPos, float Rotation)
            {
                float speed_02 = _Time.y * _Speed; 
                float3 _worldPos = worldPos;
                float3 _worldNormal = worldNormal;

                float2 _worldPos_yz = 0;
                float2 _worldPos_xz = 0;
                float2 _worldPos_xy = 0;

                Unity_Rotate_Degrees_float(_worldPos.yz, 0.5, 90 + Rotation, _worldPos_yz);   
                Unity_Rotate_Degrees_float(_worldPos.xz, 0.5, Rotation, _worldPos_xz);  
                Unity_Rotate_Degrees_float(_worldPos.xy, 0.5, Rotation, _worldPos_xy);               

                float4 col_xz = tex2D(Tex, _worldPos_xz * _Tile + speed_02) * _Brightness;
                float4 col_yz = tex2D(Tex, _worldPos_yz * _Tile + speed_02) * _Brightness;
                float4 col_xy = tex2D(Tex, _worldPos_xy * _Tile + speed_02) * _Brightness; 

                float4 tp_nor = _worldNormal.y * col_xz;
                float4 sd_nor = _worldNormal.x * col_yz;
                float4 fr_nor = _worldNormal.z * col_xy;
                
                float4 render = clamp(abs(sd_nor) + tp_nor + abs(fr_nor), 0, 1);
                return render;

            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = v.normal;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                half3 _worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(_worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(_worldNormal,1)) * _AmbientFactor;

                TRANSFER_SHADOW(o)
                return o;
            }  

            fixed4 frag (v2f i) : SV_Target
            {         
                float speed_01 = _Time.y * _SpeedDis; 
                float speed_02 = _Time.y * _Speed; 

                float simpleNoise = 0;   
                Unity_SimpleNoise_float(float2(i.worldPos.xz + speed_01), 10, simpleNoise);
                i.worldPos.xyz += simpleNoise / _DistortionFactor;  

                float3 worldNormal = UnityObjectToWorldNormal(i.normal); 

                float2 i_worldPos_yz = 0;
                Unity_Rotate_Degrees_float(i.worldPos.yz, 0, _Degree, i_worldPos_yz);

                float4 tex = tex2D(_Texture, i.uv);               
                fixed shadow = SHADOW_ATTENUATION(i);

                float4 render_01 = Unity_Texture_Projection_float(_CausticTex, worldNormal, i.worldPos, 0) * float4(i.diff, 1);
                render_01 *= shadow;
                float4 render_02 = Unity_Texture_Projection_float(_CausticTex, worldNormal, i.worldPos, _Degree) * float4(i.diff, 1);   
                render_02 *= shadow;             

                return float4(i.ambient, 1) * tex + render_01 + render_02;
            }
            ENDCG            
        }   

        // shadow casting support
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"       
    }
}
