Shader "Hidden/Custom/OutlineColor"
{
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            ZTest Always ZWrite Off Cull Off
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D_X(_BlitTexture);
            SAMPLER(sampler_BlitTexture);

            float4 _OutlineColor;
            float _Threshold;
            float4 _TintColor;
            float4 _BlitTexture_TexelSize;

            struct v2f {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f Vert(uint vertexID : SV_VertexID) {
                v2f o;
                o.uv = float2((vertexID << 1) & 2, vertexID & 2);
                o.positionHCS = float4(o.uv * 2.0 - 1.0, 0.0, 1.0);
                #if UNITY_UV_STARTS_AT_TOP
                o.uv.y = 1.0 - o.uv.y;
                #endif
                return o;
            }

            float LuminanceCustom(float3 rgb) {
                return dot(rgb, float3(0.2126, 0.7152, 0.0722));
            }

            half4 Frag(v2f i) : SV_Target {
                float2 uv = i.uv;
                float2 offset = _BlitTexture_TexelSize.xy;

                // Sobel Edge Detection
                float gX = 0;
                float gY = 0;

                // Simple 3x3 kernel for luminance difference
                float s00 = LuminanceCustom(SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, uv + offset * float2(-1, -1)).rgb);
                float s01 = LuminanceCustom(SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, uv + offset * float2( 0, -1)).rgb);
                float s02 = LuminanceCustom(SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, uv + offset * float2( 1, -1)).rgb);
                float s10 = LuminanceCustom(SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, uv + offset * float2(-1,  0)).rgb);
                float s12 = LuminanceCustom(SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, uv + offset * float2( 1,  0)).rgb);
                float s20 = LuminanceCustom(SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, uv + offset * float2(-1,  1)).rgb);
                float s21 = LuminanceCustom(SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, uv + offset * float2( 0,  1)).rgb);
                float s22 = LuminanceCustom(SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, uv + offset * float2( 1,  1)).rgb);

                gX = s00 + 2 * s10 + s20 - s02 - 2 * s12 - s22;
                gY = s00 + 2 * s01 + s02 - s20 - 2 * s21 - s22;

                float edge = sqrt(gX * gX + gY * gY);
                half4 sceneColor = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, uv);

                // Apply Tint
                sceneColor *= _TintColor;

                // Blend Outline
                return lerp(sceneColor, _OutlineColor, saturate(edge * _Threshold));
            }
            ENDHLSL
        }
    }
}