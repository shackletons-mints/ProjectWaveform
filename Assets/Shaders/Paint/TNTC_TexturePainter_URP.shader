Shader "TNTC/TexturePainter"
{
    Properties
    {
        _PainterColor ("Painter Color", Color) = (0, 0, 0, 0)
        _Color ("Color", Color) = (0, 1, 0, 0)
        _MainTex ("Main Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Opaque" }

        Cull Off ZWrite Off ZTest Off

        Pass
        {
            Name "TexturePainter"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            float3 _PainterPosition;
            float _Radius;
            float _Hardness;
            float _Strength;
            float4 _PainterColor;
            float _PrepareUV;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            float mask(float3 position, float3 center, float radius, float hardness)
            {
                float m = distance(center, position);
                return 1 - smoothstep(radius * hardness, radius, m);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.worldPos = TransformObjectToWorld(v.vertex.xyz);
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                if (_PrepareUV > 0) {
                    return float4(0, 0, 1, 1); // encode UV in blue channel?
                }
                float4 prevColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.worldPos); // previous paint
                // float2 centerUV = _PainterPosition.xy;
                // float d = distance(i.uv, centerUV);

				float d = distance(i.worldPos, _PainterPosition.xyz);
                float mask = 1 - smoothstep(_Radius * _Hardness, _Radius, d);

                // blend previous paint with new paint
                float4 result = lerp(prevColor, _PainterColor, mask * _Strength);
                return result;

                // float2 centerUV = _PainterPosition.xy; // _PainterPosition now holds UV
                // float d = distance(i.uv, centerUV);
                // float mask = 1 - smoothstep(_Radius * _Hardness, _Radius, d);

                // float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                // return lerp(col, _PainterColor, mask * _Strength);
            }

            ENDHLSL
        }
    }
}
