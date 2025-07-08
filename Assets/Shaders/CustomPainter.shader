Shader "Custom/PainterURP"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _PainterColor ("Painter Color", Color) = (1, 0, 0, 1)
        _PainterPosition ("Painter Position", Vector) = (0, 0, 0, 0)
        _Radius ("Radius", Float) = 1
        _Hardness ("Hardness", Float) = 0.5
        _Strength ("Strength", Float) = 1
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Opaque" }

        Pass
        {
            Name "PainterPass"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Textures
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // Brush settings
            float4 _MainTex_ST;
            float3 _PainterPosition;
            float4 _PainterColor;
            float _Radius;
            float _Hardness;
            float _Strength;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.worldPos = TransformObjectToWorld(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float BrushFalloff(float3 pos, float3 center, float radius, float hardness)
            {
                float dist = distance(center, pos);
                return 1.0 - smoothstep(radius * hardness, radius, dist);
            }

            half4 frag (Varyings i) : SV_Target
            {
                half4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float falloff = BrushFalloff(i.worldPos, _PainterPosition, _Radius, _Hardness);
                float blend = falloff * _Strength;
                return lerp(baseColor, _PainterColor, blend);
            }

            ENDHLSL
        }
    }
}

