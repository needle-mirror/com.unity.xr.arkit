Shader "Unlit/ARKitURPBackground"
{
    Properties
    {
        _textureY ("TextureY", 2D) = "white" {}
        _textureCbCr ("TextureCbCr", 2D) = "black" {}
        _HumanStencil ("HumanStencil", 2D) = "black" {}
        _HumanDepth ("HumanDepth", 2D) = "black" {}
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Background"
            "RenderType" = "Background"
            "ForceNoShadowCasting" = "True"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Cull Off
            ZTest Always
            ZWrite On
            Lighting Off
            LOD 100
            Tags
            {
                "LightMode" = "Always"
            }


            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_local __ ARKIT_HUMAN_SEGMENTATION_ENABLED

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"


            struct appdata
            {
                float3 position : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct fragment_output
            {
                real4 color : SV_Target;
                float depth : SV_Depth;
            };


            CBUFFER_START(UnityPerFrame)
            // Device display transform is provided by the AR Foundation camera background renderer.
            float4x4 _UnityDisplayTransform;
            CBUFFER_END

            v2f vert (appdata v)
            {
                // Transform the position from object space to clip space.
                float4 position = TransformObjectToHClip(v.position);

                // Remap the texture coordinates based on the device rotation.
                float2 texcoord = mul(float3(v.texcoord, 1.0f), _UnityDisplayTransform).xy;

                v2f o;
                o.position = position;
                o.texcoord = texcoord;
                return o;
            }


            CBUFFER_START(ARKitColorTransformations)
            static const real4x4 s_YCbCrToSRGB = real4x4(
                real4(1.0h,  0.0000h,  1.4020h, -0.7010h),
                real4(1.0h, -0.3441h, -0.7141h,  0.5291h),
                real4(1.0h,  1.7720h,  0.0000h, -0.8860h),
                real4(0.0h,  0.0000h,  0.0000h,  1.0000h)
            );
            CBUFFER_END

            inline float ConvertDistanceToDepth(float d)
            {
                // Clip any distances smaller than the near clip plane, and compute the depth value from the distance.
                return (d < _ProjectionParams.y) ? 0.0f : ((0.5f / _ZBufferParams.z) * ((1.0f / d) - _ZBufferParams.w));
            }

            TEXTURE2D(_textureY);
            SAMPLER(sampler_textureY);
            TEXTURE2D(_textureCbCr);
            SAMPLER(sampler_textureCbCr);
#if ARKIT_HUMAN_SEGMENTATION_ENABLED
            TEXTURE2D(_HumanStencil);
            SAMPLER(sampler_HumanStencil);
            TEXTURE2D_FLOAT(_HumanDepth);
            SAMPLER(sampler_HumanDepth);
#endif // ARKIT_HUMAN_SEGMENTATION_ENABLED

            fragment_output frag (v2f i)
            {
                // Sample the video textures (in YCbCr).
                real4 ycbcr = real4(SAMPLE_TEXTURE2D(_textureY, sampler_textureY, i.texcoord).r,
                                    SAMPLE_TEXTURE2D(_textureCbCr, sampler_textureCbCr, i.texcoord).rg,
                                    1.0h);

                // Convert from YCbCr to sRGB.
                real4 videoColor = mul(s_YCbCrToSRGB, ycbcr);

#if !UNITY_COLORSPACE_GAMMA
                // If rendering in linear color space, convert from sRGB to RGB.
                videoColor.xyz = FastSRGBToLinear(videoColor.xyz);
#endif // !UNITY_COLORSPACE_GAMMA

                // Assume the background depth is the back of the depth clipping volume.
                float depthValue = 0.0f;

#if ARKIT_HUMAN_SEGMENTATION_ENABLED
                // Check the human stencil, and skip non-human pixels.
                if (SAMPLE_TEXTURE2D(_HumanStencil, sampler_HumanStencil, i.texcoord).r > 0.5h)
                {
                    // Sample the human depth (in meters).
                    float humanDistance = SAMPLE_TEXTURE2D(_HumanDepth, sampler_HumanDepth, i.texcoord).r;

                    // Convert the distance to depth.
                    depthValue = ConvertDistanceToDepth(humanDistance);
                }
#endif // ARKIT_HUMAN_SEGMENTATION_ENABLED

                fragment_output o;
                o.color = videoColor;
                o.depth = depthValue;
                return o;
            }
            ENDHLSL
        }
    }
}
