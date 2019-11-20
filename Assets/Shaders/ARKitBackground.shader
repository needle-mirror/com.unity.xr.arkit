Shader "Unlit/ARKitBackground"
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


            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_local __ ARKIT_HUMAN_SEGMENTATION_ENABLED

            #include "UnityCG.cginc"


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
                half4 color : SV_Target;
                float depth : SV_Depth;
            };


            CBUFFER_START(UnityPerFrame)
            // Device display transform is provided by the AR Foundation camera background renderer.
            float4x4 _UnityDisplayTransform;
            CBUFFER_END

            v2f vert (appdata v)
            {
                // Transform the position from object space to clip space.
                float4 position = UnityObjectToClipPos(v.position);

                // Remap the texture coordinates based on the device rotation.
                float2 texcoord = mul(float3(v.texcoord, 1.0f), _UnityDisplayTransform).xy;

                v2f o;
                o.position = position;
                o.texcoord = texcoord;
                return o;
            }


            CBUFFER_START(ARKitColorTransformations)
            static const half4x4 s_YCbCrToSRGB = half4x4(
                half4(1.0h,  0.0000h,  1.4020h, -0.7010h),
                half4(1.0h, -0.3441h, -0.7141h,  0.5291h),
                half4(1.0h,  1.7720h,  0.0000h, -0.8860h),
                half4(0.0h,  0.0000h,  0.0000h,  1.0000h)
            );
            CBUFFER_END

            inline float ConvertDistanceToDepth(float d)
            {
                // Clip any distances smaller than the near clip plane, and compute the depth value from the distance.
                return (d < _ProjectionParams.y) ? 0.0f : ((0.5f / _ZBufferParams.z) * ((1.0f / d) - _ZBufferParams.w));
            }

            UNITY_DECLARE_TEX2D_HALF(_textureY);
            UNITY_DECLARE_TEX2D_HALF(_textureCbCr);
#if ARKIT_HUMAN_SEGMENTATION_ENABLED
            UNITY_DECLARE_TEX2D_HALF(_HumanStencil);
            UNITY_DECLARE_TEX2D_FLOAT(_HumanDepth);
#endif // ARKIT_HUMAN_SEGMENTATION_ENABLED

            fragment_output frag (v2f i)
            {
                // Sample the video textures (in YCbCr).
                half4 ycbcr = half4(UNITY_SAMPLE_TEX2D(_textureY, i.texcoord).r,
                                    UNITY_SAMPLE_TEX2D(_textureCbCr, i.texcoord).rg,
                                    1.0h);

                // Convert from YCbCr to sRGB.
                half4 videoColor = mul(s_YCbCrToSRGB, ycbcr);

#if !UNITY_COLORSPACE_GAMMA
                // If rendering in linear color space, convert from sRGB to RGB.
                videoColor.xyz = GammaToLinearSpace(videoColor.xyz);
#endif // !UNITY_COLORSPACE_GAMMA

                // Assume the background depth is the back of the depth clipping volume.
                float depthValue = 0.0f;

#if ARKIT_HUMAN_SEGMENTATION_ENABLED
                // Check the human stencil, and skip non-human pixels.
                if (UNITY_SAMPLE_TEX2D(_HumanStencil, i.texcoord).r > 0.5h)
                {
                    // Sample the human depth (in meters).
                    float humanDistance = UNITY_SAMPLE_TEX2D(_HumanDepth, i.texcoord).r;

                    // Convert the distance to depth.
                    depthValue = ConvertDistanceToDepth(humanDistance);
                }
#endif // ARKIT_HUMAN_SEGMENTATION_ENABLED

                fragment_output o;
                o.color = videoColor;
                o.depth = depthValue;
                return o;
            }
            ENDCG
        }
    }
}
