using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.XR.CoreUtils.Collections;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// This subsystem provides implementing functionality for the <c>XROcclusionSubsystem</c> class.
    /// </summary>
    [Preserve]
    public sealed class ARKitOcclusionSubsystem : XROcclusionSubsystem
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
            if (!Api.AtLeast13_0())
                return;

            const string k_SubsystemId = "ARKit-Occlusion";

            var occlusionSubsystemCinfo = new XROcclusionSubsystemDescriptor.Cinfo()
            {
                id = k_SubsystemId,
                providerType = typeof(ARKitOcclusionSubsystem.ARKitProvider),
                subsystemTypeOverride = typeof(ARKitOcclusionSubsystem),
                humanSegmentationStencilImageSupportedDelegate = NativeApi.UnityARKit_OcclusionProvider_DoesSupportBodySegmentationStencil,
                humanSegmentationDepthImageSupportedDelegate = NativeApi.UnityARKit_OcclusionProvider_DoesSupportBodySegmentationDepth,
                environmentDepthImageSupportedDelegate = NativeApi.UnityARKit_OcclusionProvider_DoesSupportEnvironmentDepth,
                environmentDepthConfidenceImageSupportedDelegate = NativeApi.UnityARKit_OcclusionProvider_DoesSupportEnvironmentDepth,
                environmentDepthTemporalSmoothingSupportedDelegate = NativeApi.UnityARKit_OcclusionProvider_DoesSupportEnvironmentDepthTemporalSmoothing
            };

            XROcclusionSubsystemDescriptor.Register(occlusionSubsystemCinfo);
        }

        class ARKitProvider : Provider
        {
            const string k_TextureHumanStencilPropertyName = "_HumanStencil";
            const string k_TextureHumanDepthPropertyName = "_HumanDepth";
            const string k_TextureEnvironmentDepthPropertyName = "_EnvironmentDepth";
            const string k_TextureEnvironmentDepthConfidencePropertyName = "_EnvironmentDepthConfidence";
            const string k_HumanEnabledMaterialKeyword = "ARKIT_HUMAN_SEGMENTATION_ENABLED";
            const string k_EnvironmentDepthEnabledMaterialKeyword = "ARKIT_ENVIRONMENT_DEPTH_ENABLED";

            static readonly int k_TextureHumanStencilPropertyId = Shader.PropertyToID(k_TextureHumanStencilPropertyName);
            static readonly int k_TextureHumanDepthPropertyId = Shader.PropertyToID(k_TextureHumanDepthPropertyName);
            static readonly int k_TextureEnvironmentDepthPropertyId = Shader.PropertyToID(k_TextureEnvironmentDepthPropertyName);
            static readonly int k_TextureEnvironmentDepthConfidencePropertyId = Shader.PropertyToID(k_TextureEnvironmentDepthConfidencePropertyName);

            /// <summary>
            /// The shader keywords for enabling human segmentation rendering.
            /// </summary>
            static readonly List<string> k_HumanSegmentationKeywordList = new(){ k_HumanEnabledMaterialKeyword };
            static readonly ReadOnlyList<string> k_HumanSegmentationKeywordListReadOnly = new(k_HumanSegmentationKeywordList);

            /// <summary>
            /// The shader keywords for enabling environment depth rendering.
            /// </summary>
            static readonly List<string> k_EnvironmentDepthKeywordList = new(){ k_EnvironmentDepthEnabledMaterialKeyword };
            static readonly ReadOnlyList<string> k_EnvironmentDepthKeywordListReadOnly = new(k_EnvironmentDepthKeywordList);

            /// <summary>
            /// Keywords to use when no occlusion is enabled.
            /// </summary>
            static readonly List<string> k_AllDisabledShaderKeywords =
                new(){ k_HumanEnabledMaterialKeyword, k_EnvironmentDepthEnabledMaterialKeyword };

            static readonly ReadOnlyList<string> k_AllDisabledShaderKeywordsReadOnly = new(k_AllDisabledShaderKeywords);

            static readonly XRShaderKeywords k_DepthDisabledShaderKeywords =
                new(null, k_AllDisabledShaderKeywordsReadOnly) ;

            static readonly XRShaderKeywords k_EnvironmentDepthKeywords =
                new(k_EnvironmentDepthKeywordListReadOnly, k_HumanSegmentationKeywordListReadOnly);

            static readonly XRShaderKeywords k_HumanSegmentationKeywords =
                new(k_HumanSegmentationKeywordListReadOnly, k_EnvironmentDepthKeywordListReadOnly);

            OcclusionPreferenceMode m_OcclusionPreferenceMode;

            public ARKitProvider() => NativeApi.UnityARKit_OcclusionProvider_Construct(
                k_TextureHumanStencilPropertyId,
                k_TextureHumanDepthPropertyId,
                k_TextureEnvironmentDepthPropertyId,
                k_TextureEnvironmentDepthConfidencePropertyId);

            public override void Start() => NativeApi.UnityARKit_OcclusionProvider_Start();
            public override void Stop() => NativeApi.UnityARKit_OcclusionProvider_Stop();
            public override void Destroy() => NativeApi.UnityARKit_OcclusionProvider_Destruct();

            public override HumanSegmentationStencilMode requestedHumanStencilMode
            {
                get => NativeApi.UnityARKit_OcclusionProvider_GetRequestedSegmentationStencilMode();
                set
                {
                    NativeApi.UnityARKit_OcclusionProvider_SetRequestedSegmentationStencilMode(value);
                    Api.SetFeatureRequested(Feature.PeopleOcclusionStencil, value.Enabled());
                }
            }

            public override HumanSegmentationStencilMode currentHumanStencilMode
                => NativeApi.UnityARKit_OcclusionProvider_GetCurrentSegmentationStencilMode();

            public override HumanSegmentationDepthMode requestedHumanDepthMode
            {
                get => NativeApi.UnityARKit_OcclusionProvider_GetRequestedSegmentationDepthMode();
                set
                {
                    NativeApi.UnityARKit_OcclusionProvider_SetRequestedSegmentationDepthMode(value);
                    Api.SetFeatureRequested(Feature.PeopleOcclusionDepth, value.Enabled());
                }
            }

            public override HumanSegmentationDepthMode currentHumanDepthMode
                => NativeApi.UnityARKit_OcclusionProvider_GetCurrentSegmentationDepthMode();

            public override EnvironmentDepthMode requestedEnvironmentDepthMode
            {
                get => NativeApi.UnityARKit_OcclusionProvider_GetRequestedEnvironmentDepthMode();
                set
                {
                    NativeApi.UnityARKit_OcclusionProvider_SetRequestedEnvironmentDepthMode(value);
                    Api.SetFeatureRequested(Feature.EnvironmentDepth, value.Enabled());
                }
            }

            public override EnvironmentDepthMode currentEnvironmentDepthMode
                => NativeApi.UnityARKit_OcclusionProvider_GetCurrentEnvironmentDepthMode();

            public override bool environmentDepthTemporalSmoothingEnabled =>
                NativeApi.UnityARKit_OcclusionProvider_GetEnvironmentDepthTemporalSmoothingEnabled();

            public override bool environmentDepthTemporalSmoothingRequested
            {
                get => Api.GetRequestedFeatures().Any(Feature.EnvironmentDepthTemporalSmoothing);
                set => Api.SetFeatureRequested(Feature.EnvironmentDepthTemporalSmoothing, value);
            }

            public override OcclusionPreferenceMode requestedOcclusionPreferenceMode
            {
                get => m_OcclusionPreferenceMode;
                set => m_OcclusionPreferenceMode = value;
            }

            public override OcclusionPreferenceMode currentOcclusionPreferenceMode => m_OcclusionPreferenceMode;

            public override bool TryGetHumanStencil(out XRTextureDescriptor humanStencilDescriptor)
                => NativeApi.UnityARKit_OcclusionProvider_TryGetHumanStencil(out humanStencilDescriptor);

            public override XRCpuImage.Api humanStencilCpuImageApi => ARKitCpuImageApi.instance;

            public override bool TryAcquireHumanStencilCpuImage(out XRCpuImage.Cinfo cinfo)
                => ARKitCpuImageApi.TryAcquireLatestImage(ARKitCpuImageApi.ImageType.HumanStencil, out cinfo);

            public override bool TryGetHumanDepth(out XRTextureDescriptor humanDepthDescriptor)
                => NativeApi.UnityARKit_OcclusionProvider_TryGetHumanDepth(out humanDepthDescriptor);

            public override XRCpuImage.Api humanDepthCpuImageApi => ARKitCpuImageApi.instance;

            public override bool TryAcquireHumanDepthCpuImage(out XRCpuImage.Cinfo cinfo)
                => ARKitCpuImageApi.TryAcquireLatestImage(ARKitCpuImageApi.ImageType.HumanDepth, out cinfo);

            public override bool TryGetEnvironmentDepth(out XRTextureDescriptor environmentDepthDescriptor)
                => NativeApi.UnityARKit_OcclusionProvider_TryGetEnvironmentDepth(out environmentDepthDescriptor);

            public override bool TryAcquireEnvironmentDepthCpuImage(out XRCpuImage.Cinfo cinfo)
                => ARKitCpuImageApi.TryAcquireLatestImage(ARKitCpuImageApi.ImageType.EnvironmentDepth, out cinfo);

            public override bool TryAcquireRawEnvironmentDepthCpuImage(out XRCpuImage.Cinfo cinfo) =>
                ARKitCpuImageApi.TryAcquireLatestImage(ARKitCpuImageApi.ImageType.RawEnvironmentDepth, out cinfo);

            public override bool TryAcquireSmoothedEnvironmentDepthCpuImage(out XRCpuImage.Cinfo cinfo) =>
                ARKitCpuImageApi.TryAcquireLatestImage(ARKitCpuImageApi.ImageType.TemporallySmoothedEnvironmentDepth, out cinfo);

            public override XRCpuImage.Api environmentDepthCpuImageApi => ARKitCpuImageApi.instance;

            public override bool TryGetEnvironmentDepthConfidence(out XRTextureDescriptor environmentDepthConfidenceDescriptor)
                => NativeApi.UnityARKit_OcclusionProvider_TryGetEnvironmentDepthConfidence(out environmentDepthConfidenceDescriptor);

            public override bool TryAcquireEnvironmentDepthConfidenceCpuImage(out XRCpuImage.Cinfo cinfo)
                => ARKitCpuImageApi.TryAcquireLatestImage(ARKitCpuImageApi.ImageType.EnvironmentDepthConfidence,
                    out cinfo);

            public override XRCpuImage.Api environmentDepthConfidenceCpuImageApi => ARKitCpuImageApi.instance;

            public override unsafe NativeArray<XRTextureDescriptor> GetTextureDescriptors(
                XRTextureDescriptor defaultDescriptor,
                Allocator allocator)
            {
                var textureDescriptors = NativeApi.UnityARKit_OcclusionProvider_AcquireTextureDescriptors(
                    out var length,
                    out var elementSize);

                try
                {
                    return NativeCopyUtility.PtrToNativeArrayWithDefault(
                        defaultDescriptor,
                        textureDescriptors,
                        elementSize,
                        length,
                        allocator);
                }
                finally
                {
                    NativeApi.UnityARKit_OcclusionProvider_ReleaseTextureDescriptors(textureDescriptors);
                }
            }

            [Obsolete]
            public override void GetMaterialKeywords(out List<string> enabledKeywords, out List<string> disabledKeywords)
            {
                var isEnvDepthEnabled = NativeApi.UnityARKit_OcclusionProvider_IsEnvironmentEnabled();
                var isHumanDepthEnabled = NativeApi.UnityARKit_OcclusionProvider_IsHumanEnabled();

                // If no occlusion is preferred or if neither depth is enabled, then all disable occlusion.
                if (m_OcclusionPreferenceMode == OcclusionPreferenceMode.NoOcclusion || (!isEnvDepthEnabled && !isHumanDepthEnabled))
                {
                    enabledKeywords = null;
                    disabledKeywords = k_AllDisabledShaderKeywords;
                }
                // Else if environment depth is enabled and human depth is not enabled/prefered, then use environment depth.
                else if (isEnvDepthEnabled && (!isHumanDepthEnabled || m_OcclusionPreferenceMode == OcclusionPreferenceMode.PreferEnvironmentOcclusion))
                {
                    enabledKeywords = k_EnvironmentDepthKeywordList;
                    disabledKeywords = k_HumanSegmentationKeywordList;
                }
                // Otherwise, human depth is enabled and/or preferred, so use human depth.
                else
                {
                    enabledKeywords = k_HumanSegmentationKeywordList;
                    disabledKeywords = k_EnvironmentDepthKeywordList;
                }
            }

            [Obsolete]
            public override ShaderKeywords GetShaderKeywords()
            {
                var isEnvDepthEnabled = NativeApi.UnityARKit_OcclusionProvider_IsEnvironmentEnabled();
                var isHumanDepthEnabled = NativeApi.UnityARKit_OcclusionProvider_IsHumanEnabled();

                if (ShouldUseDepthDisabledKeywords(isEnvDepthEnabled, isHumanDepthEnabled))
                    return new ShaderKeywords(null, k_AllDisabledShaderKeywords.AsReadOnly());

                if (ShouldUseEnvironmentDepthEnabledKeywords(isEnvDepthEnabled, isHumanDepthEnabled))
                    return new ShaderKeywords(k_EnvironmentDepthKeywordList.AsReadOnly(), k_HumanSegmentationKeywordList.AsReadOnly());

                return new ShaderKeywords(k_HumanSegmentationKeywordList.AsReadOnly(), k_EnvironmentDepthKeywordList.AsReadOnly());
            }

            public override XRShaderKeywords GetShaderKeywords2()
            {
                var isEnvDepthEnabled = NativeApi.UnityARKit_OcclusionProvider_IsEnvironmentEnabled();
                var isHumanDepthEnabled = NativeApi.UnityARKit_OcclusionProvider_IsHumanEnabled();

                if (ShouldUseDepthDisabledKeywords(isEnvDepthEnabled, isHumanDepthEnabled))
                    return k_DepthDisabledShaderKeywords;

                if (ShouldUseEnvironmentDepthEnabledKeywords(isEnvDepthEnabled, isHumanDepthEnabled))
                    return k_EnvironmentDepthKeywords;

                return k_HumanSegmentationKeywords;
            }

            bool ShouldUseDepthDisabledKeywords(bool isEnvDepthEnabled, bool isHumanDepthEnabled)
            {
                return m_OcclusionPreferenceMode == OcclusionPreferenceMode.NoOcclusion || (!isEnvDepthEnabled && !isHumanDepthEnabled);
            }

            bool ShouldUseEnvironmentDepthEnabledKeywords(bool isEnvDepthEnabled, bool isHumanDepthEnabled)
            {
                return isEnvDepthEnabled && (!isHumanDepthEnabled || m_OcclusionPreferenceMode == OcclusionPreferenceMode.PreferEnvironmentOcclusion);
            }
        }

        static class NativeApi
        {
#if UNITY_XR_ARKIT_LOADER_ENABLED
            [DllImport("__Internal")]
            public static extern void UnityARKit_OcclusionProvider_Construct(
                int textureHumanStencilPropertyId,
                int textureHumanDepthPropertyId,
                int textureEnvDepthPropertyId,
                int textureEnvDepthConfidencePropertyId);

            [DllImport("__Internal")]
            public static extern void UnityARKit_OcclusionProvider_Start();

            [DllImport("__Internal")]
            public static extern void UnityARKit_OcclusionProvider_Stop();

            [DllImport("__Internal")]
            public static extern void UnityARKit_OcclusionProvider_Destruct();

            [DllImport("__Internal")]
            public static extern HumanSegmentationStencilMode UnityARKit_OcclusionProvider_GetRequestedSegmentationStencilMode();

            [DllImport("__Internal")]
            public static extern void UnityARKit_OcclusionProvider_SetRequestedSegmentationStencilMode(HumanSegmentationStencilMode humanSegmentationStencilMode);

            [DllImport("__Internal")]
            public static extern HumanSegmentationStencilMode UnityARKit_OcclusionProvider_GetCurrentSegmentationStencilMode();

            [DllImport("__Internal")]
            public static extern HumanSegmentationDepthMode UnityARKit_OcclusionProvider_GetRequestedSegmentationDepthMode();

            [DllImport("__Internal")]
            public static extern void UnityARKit_OcclusionProvider_SetRequestedSegmentationDepthMode(HumanSegmentationDepthMode humanSegmentationDepthMode);

            [DllImport("__Internal")]
            public static extern HumanSegmentationDepthMode UnityARKit_OcclusionProvider_GetCurrentSegmentationDepthMode();

            [DllImport("__Internal")]
            public static extern EnvironmentDepthMode UnityARKit_OcclusionProvider_GetRequestedEnvironmentDepthMode();

            [DllImport("__Internal")]
            public static extern void UnityARKit_OcclusionProvider_SetRequestedEnvironmentDepthMode(EnvironmentDepthMode environmentDepthMode);

            [DllImport("__Internal")]
            public static extern EnvironmentDepthMode UnityARKit_OcclusionProvider_GetCurrentEnvironmentDepthMode();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_OcclusionProvider_GetEnvironmentDepthTemporalSmoothingEnabled();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_OcclusionProvider_TryGetHumanStencil(out XRTextureDescriptor humanStencilDescriptor);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_OcclusionProvider_TryGetHumanDepth(out XRTextureDescriptor humanDepthDescriptor);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_OcclusionProvider_TryGetEnvironmentDepth(out XRTextureDescriptor envDepthDescriptor);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_OcclusionProvider_TryGetEnvironmentDepthConfidence(out XRTextureDescriptor envDepthConfidenceDescriptor);

            [DllImport("__Internal")]
            public static extern unsafe void* UnityARKit_OcclusionProvider_AcquireTextureDescriptors(out int length, out int elementSize);

            [DllImport("__Internal")]
            public static extern unsafe void UnityARKit_OcclusionProvider_ReleaseTextureDescriptors(void* descriptors);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_OcclusionProvider_IsHumanEnabled();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_OcclusionProvider_IsEnvironmentEnabled();

            [DllImport("__Internal")]
            public static extern Supported UnityARKit_OcclusionProvider_DoesSupportBodySegmentationStencil();

            [DllImport("__Internal")]
            public static extern Supported UnityARKit_OcclusionProvider_DoesSupportBodySegmentationDepth();

            [DllImport("__Internal")]
            public static extern Supported UnityARKit_OcclusionProvider_DoesSupportEnvironmentDepth();

            [DllImport("__Internal")]
            public static extern Supported UnityARKit_OcclusionProvider_DoesSupportEnvironmentDepthTemporalSmoothing();
#else
            static readonly string k_ExceptionMsg = "Apple ARKit XR Plug-in Provider not enabled in project settings.";

            public static void UnityARKit_OcclusionProvider_Construct(
                int textureHumanStencilPropertyId,
                int textureHumanDepthPropertyId,
                int textureEnvDepthPropertyId,
                int textureEnvDepthConfidencePropertyId)
                => throw new NotImplementedException(k_ExceptionMsg);

            public static void UnityARKit_OcclusionProvider_Start() => throw new NotImplementedException(k_ExceptionMsg);
            public static void UnityARKit_OcclusionProvider_Stop() => throw new NotImplementedException(k_ExceptionMsg);
            public static void UnityARKit_OcclusionProvider_Destruct() => throw new NotImplementedException(k_ExceptionMsg);

            public static HumanSegmentationStencilMode UnityARKit_OcclusionProvider_GetRequestedSegmentationStencilMode()
                => throw new NotImplementedException(k_ExceptionMsg);

            public static void UnityARKit_OcclusionProvider_SetRequestedSegmentationStencilMode(
                HumanSegmentationStencilMode humanSegmentationStencilMode)
                => throw new NotImplementedException(k_ExceptionMsg);

            public static HumanSegmentationStencilMode UnityARKit_OcclusionProvider_GetCurrentSegmentationStencilMode()
                => throw new NotImplementedException(k_ExceptionMsg);

            public static HumanSegmentationDepthMode UnityARKit_OcclusionProvider_GetRequestedSegmentationDepthMode()
                => throw new NotImplementedException(k_ExceptionMsg);

            public static void UnityARKit_OcclusionProvider_SetRequestedSegmentationDepthMode(
                HumanSegmentationDepthMode humanSegmentationDepthMode)
                => throw new NotImplementedException(k_ExceptionMsg);

            public static HumanSegmentationDepthMode UnityARKit_OcclusionProvider_GetCurrentSegmentationDepthMode()
                => throw new NotImplementedException(k_ExceptionMsg);

            public static EnvironmentDepthMode UnityARKit_OcclusionProvider_GetRequestedEnvironmentDepthMode()
                => throw new NotImplementedException(k_ExceptionMsg);

            public static void UnityARKit_OcclusionProvider_SetRequestedEnvironmentDepthMode(
                EnvironmentDepthMode environmentDepthMode)
                => throw new System.NotImplementedException(k_ExceptionMsg);

            public static EnvironmentDepthMode UnityARKit_OcclusionProvider_GetCurrentEnvironmentDepthMode()
                => throw new NotImplementedException(k_ExceptionMsg);

            public static bool UnityARKit_OcclusionProvider_TryGetHumanStencil(
                out XRTextureDescriptor humanStencilDescriptor)
                => throw new NotImplementedException(k_ExceptionMsg);

            public static bool UnityARKit_OcclusionProvider_TryGetHumanDepth(
                out XRTextureDescriptor humanDepthDescriptor)
                => throw new NotImplementedException(k_ExceptionMsg);

            public static bool UnityARKit_OcclusionProvider_TryGetEnvironmentDepth(
                out XRTextureDescriptor envDepthDescriptor)
                => throw new NotImplementedException(k_ExceptionMsg);

            public static bool UnityARKit_OcclusionProvider_TryGetEnvironmentDepthConfidence(
                out XRTextureDescriptor envDepthConfidenceDescriptor)
                => throw new NotImplementedException(k_ExceptionMsg);

            public static unsafe void* UnityARKit_OcclusionProvider_AcquireTextureDescriptors(
                out int length, out int elementSize)
                => throw new NotImplementedException(k_ExceptionMsg);

            public static unsafe void UnityARKit_OcclusionProvider_ReleaseTextureDescriptors(void* descriptors)
                => throw new NotImplementedException(k_ExceptionMsg);

            public static bool UnityARKit_OcclusionProvider_IsHumanEnabled() => false;

            public static bool UnityARKit_OcclusionProvider_IsEnvironmentEnabled() => false;

            public static Supported UnityARKit_OcclusionProvider_DoesSupportBodySegmentationStencil() => Supported.Unsupported;

            public static Supported UnityARKit_OcclusionProvider_DoesSupportBodySegmentationDepth() => Supported.Unsupported;

            public static Supported UnityARKit_OcclusionProvider_DoesSupportEnvironmentDepth() => Supported.Unsupported;

            public static Supported UnityARKit_OcclusionProvider_DoesSupportEnvironmentDepthTemporalSmoothing() => Supported.Unsupported;

            public static bool UnityARKit_OcclusionProvider_GetEnvironmentDepthTemporalSmoothingEnabled() => false;
#endif
        }
    }
}
