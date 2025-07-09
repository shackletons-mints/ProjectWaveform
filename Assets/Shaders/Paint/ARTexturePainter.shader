Shader "ARTexturePainter"
{
    Properties
    {
        _PainterColor ("Painter Color", Color) = (0, 0, 0, 0)
        _MainTex ("Main Texture", 2D) = "white" {}
		_ProjectionScale ("Projection Scale", Float) = 1.0
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
			float _ProjectionScale;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
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
                return o;
            }


            half4 frag(v2f i) : SV_Target
            {
				float2 projectedUV = i.worldPos.xz * _ProjectionScale;
                if (_PrepareUV > 0) {
                    return float4(0, 0, 1, 1); // Debug to visualize UVs
                }

                float4 prevColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, projectedUV);

                float d = distance(i.worldPos, _PainterPosition.xyz);
                float m = 1 - smoothstep(_Radius * _Hardness, _Radius, d);

                float4 result = lerp(prevColor, _PainterColor, m * _Strength);
                return result;
            }

            ENDHLSL
        }
    }
}
