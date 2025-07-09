Shader "ARExtendIslands"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _UVIslands ("Texture UVIslands", 2D) = "white" {}
        _OffsetUV ("UVOffset", float) = 1
		_ProjectionScale ("Projection Scale", Float) = 1.0
		_PainterCenter ("Painter Position", Vector) = (0, 0, 0, 0)
		_MaxExtendDistance ("Max Extend Distance", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            Name "ExtendIslands"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_UVIslands);
            SAMPLER(sampler_UVIslands);

            float4 _MainTex_TexelSize;
            float _OffsetUV;
            float _ProjectionScale;
			float4 _PainterCenter;
			float _MaxExtendDistance;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.worldPos = TransformObjectToWorld(v.vertex.xyz);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
				float2 projectedUV = i.worldPos.xz * _ProjectionScale;
				float distFromCenter = distance(i.worldPos, _PainterCenter.xyz);
				
				if (distFromCenter > _MaxExtendDistance)
				{
					discard;
				}

				float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, projectedUV);
				float4 island = SAMPLE_TEXTURE2D(_UVIslands, sampler_UVIslands, projectedUV);

				if (island.z < 1)
				{
					float4 extendedColor = color;

					float2 offsets[8] = {
						float2(-_OffsetUV, 0),
						float2(_OffsetUV, 0),
						float2(0, _OffsetUV),
						float2(0, -_OffsetUV),
						float2(-_OffsetUV, _OffsetUV),
						float2(_OffsetUV, _OffsetUV),
						float2(_OffsetUV, -_OffsetUV),
						float2(-_OffsetUV, -_OffsetUV)
					};

					[unroll]
					for (int j = 0; j < 8; ++j)
					{
						float2 offsetUV = projectedUV + offsets[j] * _MainTex_TexelSize.xy;
						float4 offsettedColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, offsetUV);
						extendedColor = max(offsettedColor, extendedColor);
					}

					color = extendedColor;
				}

				return color;
            }

            ENDHLSL
        }
    }
}

