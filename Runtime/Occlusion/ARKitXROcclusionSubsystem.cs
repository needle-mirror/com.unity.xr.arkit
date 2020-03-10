using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Registration utility to register the ARKit occlusion subsystem.
    /// </summary>
    internal static class ARKitOcclusionRegistration
    {
        /// <summary>
        /// Register the ARKit occlusion subsystem if iOS and not the editor.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
            if (!Api.AtLeast13_0())
                return;

            const string k_SubsystemId = "ARKit-Occlusion";

            bool supportsHumanSegmentationStencilImage = NativeApi.UnityARKit_OcclusionProvider_DoesSupportBodySegmentationStencil();
            bool supportsHumanSegmentationDepthImage = NativeApi.UnityARKit_OcclusionProvider_DoesSupportBodySegmentationDepth();

            if (supportsHumanSegmentationStencilImage || supportsHumanSegmentationDepthImage)
            {
                XROcclusionSubsystemCinfo occlusionSubsystemCinfo = new XROcclusionSubsystemCinfo()
                {
                    id = k_SubsystemId,
                    implementationType = typeof(ARKitOcclusionSubsystem),
                    supportsHumanSegmentationStencilImage = supportsHumanSegmentationStencilImage,
                    supportsHumanSegmentationDepthImage = supportsHumanSegmentationDepthImage,
                };

                if (!XROcclusionSubsystem.Register(occlusionSubsystemCinfo))
                {
                    Debug.Log($"Cannot register the {k_SubsystemId} subsystem");
                }
            }
        }

        /// <summary>
        /// Container to wrap the native ARKit human body APIs.
        /// </summary>
        static class NativeApi
        {
            [DllImport("__Internal")]
            public static extern bool UnityARKit_OcclusionProvider_DoesSupportBodySegmentationStencil();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_OcclusionProvider_DoesSupportBodySegmentationDepth();
        }
    }

    /// <summary>
    /// This subsystem provides implementing functionality for the <c>XROcclusionSubsystem</c> class.
    /// </summary>
    [Preserve]
    class ARKitOcclusionSubsystem : XROcclusionSubsystem
    {
        /// <summary>
        /// Create the implementation provider.
        /// </summary>
        /// <returns>
        /// The implementation provider.
        /// </returns>
        protected override Provider CreateProvider() => new ARKitProvider();

        /// <summary>
        /// The implementation provider class.
        /// </summary>
        class ARKitProvider : XROcclusionSubsystem.Provider
        {
            /// <summary>
            /// The shader property name for the human segmentation stencil texture.
            /// </summary>
            /// <value>
            /// The shader property name for the human segmentation stencil texture.
            /// </value>
            const string k_TextureHumanStencilPropertyName = "_HumanStencil";

            /// <summary>
            /// The shader property name for the human segmentation depth texture.
            /// </summary>
            /// <value>
            /// The shader property name for the human segmentation depth texture.
            /// </value>
            const string k_TextureHumanDepthPropertyName = "_HumanDepth";

            /// <summary>
            /// The shader keyword for enabling human segmentation rendering.
            /// </summary>
            /// <value>
            /// The shader keyword for enabling human segmentation rendering.
            /// </value>
            const string k_HumanEnabledMaterialKeyword = "ARKIT_HUMAN_SEGMENTATION_ENABLED";

            /// <summary>
            /// The shader property name identifier for the human segmentation stencil texture.
            /// </summary>
            /// <value>
            /// The shader property name identifier for the human segmentation stencil texture.
            /// </value>
            static readonly int k_TextureHumanStencilPropertyId = Shader.PropertyToID(k_TextureHumanStencilPropertyName);

            /// <summary>
            /// The shader property name identifier for the human segmentation depth texture.
            /// </summary>
            /// <value>
            /// The shader property name identifier for the human segmentation depth texture.
            /// </value>
            static readonly int k_TextureHumanDepthPropertyId = Shader.PropertyToID(k_TextureHumanDepthPropertyName);

            /// <summary>
            /// The shader keywords for enabling human segmentation rendering.
            /// </summary>
            /// <value>
            /// The shader keywords for enabling human segmentation rendering.
            /// </value>
            static readonly List<string> m_HumanEnabledMaterialKeywords = new List<string>() {k_HumanEnabledMaterialKeyword};

            /// <summary>
            /// Construct the implementation provider.
            /// </summary>
            public ARKitProvider() => NativeApi.UnityARKit_OcclusionProvider_Construct(k_TextureHumanStencilPropertyId,
                                                                                       k_TextureHumanDepthPropertyId);

            /// <summary>
            /// Start the provider.
            /// </summary>
            public override void Start() => NativeApi.UnityARKit_OcclusionProvider_Start();

            /// <summary>
            /// Stop the provider.
            /// </summary>
            public override void Stop() => NativeApi.UnityARKit_OcclusionProvider_Stop();

            /// <summary>
            /// Destroy the provider.
            /// </summary>
            public override void Destroy() => NativeApi.UnityARKit_OcclusionProvider_Destruct();

            /// <summary>
            /// Property to be implemented by the provider to get/set the human segmentation stencil mode.
            /// </summary>
            /// <value>
            /// The human segmentation stencil mode.
            /// </value>
            /// <exception cref="System.NotSupportedException">Thrown when setting the human segmentation stencil mode
            /// to enabled if the implementation does not support human segmentation.</exception>
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

            /// <summary>
            /// Property to be implemented by the provider to get/set the human segmentation depth mode.
            /// </summary>
            /// <value>
            /// The human segmentation depth mode.
            /// </value>
            /// <exception cref="System.NotSupportedException">Thrown when setting the human segmentation depth mode
            /// to enabled if the implementation does not support human segmentation.</exception>
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

            /// <summary>
            /// Gets the human stencil texture descriptor.
            /// </summary>
            /// <param name="humanStencilDescriptor">The human stencil texture descriptor to be populated, if
            /// available.</param>
            /// <returns>
            /// <c>true</c> if the human stencil texture descriptor is available and is returned. Otherwise,
            /// <c>false</c>.
            /// </returns>
            public override bool TryGetHumanStencil(out XRTextureDescriptor humanStencilDescriptor)
                => NativeApi.UnityARKit_OcclusionProvider_TryGetHumanStencil(out humanStencilDescriptor);

            /// <summary>
            /// Get the human depth texture descriptor.
            /// </summary>
            /// <param name="humanDepthDescriptor">The human depth texture descriptor to be populated, if available
            /// </param>
            /// <returns>
            /// <c>true</c> if the human depth texture descriptor is available and is returned. Otherwise,
            /// <c>false</c>.
            /// </returns>
            public override bool TryGetHumanDepth(out XRTextureDescriptor humanDepthDescriptor)
                => NativeApi.UnityARKit_OcclusionProvider_TryGetHumanDepth(out humanDepthDescriptor);

            /// <summary>
            /// Gets the occlusion texture descriptors associated with the current AR frame.
            /// </summary>
            /// <param name="defaultDescriptor">The default descriptor value.</param>
            /// <param name="allocator">The allocator to use when creating the returned <c>NativeArray</c>.</param>
            /// <returns>The occlusion texture descriptors.</returns>
            public unsafe override NativeArray<XRTextureDescriptor> GetTextureDescriptors(XRTextureDescriptor defaultDescriptor,
                                                                                          Allocator allocator)
            {
                var textureDescriptors = NativeApi.UnityARKit_OcclusionProvider_AcquireTextureDescriptors(out int length,
                                                                                                          out int elementSize);

                try
                {
                    return NativeCopyUtility.PtrToNativeArrayWithDefault(defaultDescriptor, textureDescriptors,
                                                                         elementSize, length, allocator);
                }
                finally
                {
                    NativeApi.UnityARKit_OcclusionProvider_ReleaseTextureDescriptors(textureDescriptors);
                }
            }

            /// <summary>
            /// Get the enabled and disabled shader keywords for the material.
            /// </summary>
            /// <param name="enabledKeywords">The keywords to enable for the material.</param>
            /// <param name="disabledKeywords">The keywords to disable for the material.</param>
            public override void GetMaterialKeywords(out List<string> enabledKeywords, out List<string> disabledKeyWords)
            {
                if (NativeApi.UnityARKit_OcclusionProvider_IsHumanEnabled())
                {
                    enabledKeywords = m_HumanEnabledMaterialKeywords;
                    disabledKeyWords = null;
                }
                else
                {
                    enabledKeywords = null;
                    disabledKeyWords = m_HumanEnabledMaterialKeywords;
                }
            }
        }

        /// <summary>
        /// Container to wrap the native ARKit human body APIs.
        /// </summary>
        static class NativeApi
        {
            [DllImport("__Internal")]
            public static extern void UnityARKit_OcclusionProvider_Construct(int textureHumanStencilPropertyId,
                                                                             int textureHumanDepthPropertyId);

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
            public static unsafe extern bool UnityARKit_OcclusionProvider_TryGetHumanStencil(out XRTextureDescriptor humanStencilDescriptor);

            [DllImport("__Internal")]
            public static unsafe extern bool UnityARKit_OcclusionProvider_TryGetHumanDepth(out XRTextureDescriptor humanDepthDescriptor);

            [DllImport("__Internal")]
            public static unsafe extern void* UnityARKit_OcclusionProvider_AcquireTextureDescriptors(out int length, out int elementSize);

            [DllImport("__Internal")]
            public static unsafe extern void UnityARKit_OcclusionProvider_ReleaseTextureDescriptors(void* descriptors);

            [DllImport("__Internal")]
            public static unsafe extern bool UnityARKit_OcclusionProvider_IsHumanEnabled();
        }
    }
}
