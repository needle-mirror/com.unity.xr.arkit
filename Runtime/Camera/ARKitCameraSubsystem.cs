using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Collections;
using Unity.XR.CoreUtils.Collections;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;
#if UNITY_XR_ARKIT_LOADER_ENABLED
using System.Runtime.InteropServices;
#endif
#if URP_7_OR_NEWER
using UnityEngine.Rendering.Universal;
#endif

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// The camera subsystem implementation for ARKit.
    /// </summary>
    [Preserve]
    public sealed class ARKitCameraSubsystem : XRCameraSubsystem
    {
        const string k_SubsystemId = "ARKit-Camera";
        const string k_BeforeOpaquesBackgroundShaderName = "Unlit/ARKitBackground";
        const string k_AfterOpaquesBackgroundShaderName = "Unlit/ARKitBackground/AfterOpaques";
        const string k_BackgroundShaderKeywordURP = "ARKIT_BACKGROUND_URP";

        static readonly List<string> k_BackgroundShaderKeywordsToNotCompile = new()
        {
#if !URP_7_OR_NEWER
            k_BackgroundShaderKeywordURP,
#endif // !URP_7_OR_NEWER
        };

        /// <summary>
        /// The names for the background shaders based on the current render pipeline.
        /// </summary>
        /// <value>The names for the background shaders based on the current render pipeline.</value>
        /// <remarks>
        /// <para>There are two shaders in the Apple ARKit Provider Package. One is used for rendering
        /// before opaques and one is used for rendering after opaques.</para>
        /// <para>In order:
        /// <list type="number">
        /// <item>Before Opaques Shader Name</item>
        /// <item>After Opaques Shader Name</item>
        /// </list>
        /// </para>
        /// </remarks>
        public static readonly string[] backgroundShaderNames = {
            k_BeforeOpaquesBackgroundShaderName,
            k_AfterOpaquesBackgroundShaderName
        };

        /// <summary>
        /// The name for the background shader.
        /// </summary>
        /// <value>The name for the background shader.</value>
        [Obsolete("'backgroundShaderName' is obsolete, use backgroundShaderNames instead. (2022/04/04)")]
        public static string backgroundShaderName => k_BeforeOpaquesBackgroundShaderName;

        internal static List<string> backgroundShaderKeywordsToNotCompile => k_BackgroundShaderKeywordsToNotCompile;

        /// <summary>
        /// Resulting values from setting the camera configuration.
        /// </summary>
        enum CameraConfigurationResult
        {
            /// <summary>
            /// Setting the camera configuration was successful.
            /// </summary>
            Success = 0,

            /// <summary>
            /// Setting camera configuration was not supported by the provider.
            /// </summary>
            Unsupported = 1,

            /// <summary>
            /// The given camera configuration was not valid to be set by the provider.
            /// </summary>
            InvalidCameraConfiguration = 2,

            /// <summary>
            /// The provider session was invalid.
            /// </summary>
            InvalidSession = 3,
        }

        /// <summary>
        /// <para>Creates and returns a promise of a high resolution CPU image. You can <c>yield return</c> on the promise
        /// in a coroutine to await the asynchronous result.</para>
        /// <para>Requires iOS 16 or newer.</para>
        /// </summary>
        /// <returns>The promise instance.</returns>
        /// <example>
        /// <code>
        /// IEnumerator MyCoroutine()
        /// {
        ///     var promise = TryAcquireHighResolutionCpuImage();
        ///     yield return promise;
        ///     if (promise.result.wasSuccessful)
        ///         Debug.Log(promise.result.highResolutionCpuImage.ToString());
        /// }
        /// </code>
        /// </example>
        /// <remarks>
        /// <para>Refer to ARKit's <a href="https://developer.apple.com/documentation/arkit/arsession/3975720-capturehighresolutionframewithco?language=objc">
        /// captureHighResolutionFrameWithCompletion</a> documentation for more information.</para>
        /// <para>On iOS 15 or below, promises will immediately resolve with an unsuccessful result.</para>
        /// <para>Only one instance of <see cref="HighResolutionCpuImagePromise"/> at a time can await a result.
        /// Promises created while another promise is in progress will immediately resolve with an unsuccessful result.</para>
        /// </remarks>
        // ReSharper disable once MemberCanBeMadeStatic.Global -- This method is dependent on a valid provider instance
        public HighResolutionCpuImagePromise TryAcquireHighResolutionCpuImage()
            => ((ARKitProvider)provider).TryAcquireHighResolutionCpuImage();

        /// <summary>
        /// Allows <see cref="HighResolutionCpuImagePromise"/> to access native API for the ARKit Camera provider
        /// so the asynchronous promise can execute successfully without allocating a delegate object.
        /// </summary>
        /// <param name="callback">The callback for ARKit to call when the image is ready.</param>
        /// <param name="cpuImageCinfo">The <see cref="XRCpuImage.Cinfo"/> object to be assigned.</param>
        internal static void TryAcquireHighResolutionCpuImageNative(IntPtr callback, out XRCpuImage.Cinfo cpuImageCinfo)
            => NativeApi.TryAcquireHighResolutionCpuImage(callback, out cpuImageCinfo);

        /// <summary>
        /// Get whether advanced camera hardware configuration is supported. Advanced camera
        /// hardware configuration requires iOS 16 or newer and a device with an ultra-wide camera.
        /// </summary>
        /// <value><see langword="true"/> if setting advance camera hardware configurations is
        /// supported. Otherwise, <see langword="false"/>.</value>
        /// <seealso cref="TryGetLockedCamera"/>
        public bool advancedCameraConfigurationSupported =>
            Api.AtLeast16_0() && ((ARKitProvider)provider).nativePtr != IntPtr.Zero;

        /// <summary>
        /// Locks the primary camera associated with the current session configuration and returns an
        /// object that represents it. Use this object to configure advanced camera hardware properties.
        /// For more information, see ARKit's [AVCaptureDevice.lockForConfiguration]
        /// (https://developer.apple.com/documentation/avfoundation/avcapturedevice/1387810-lockforconfiguration?language=objc).
        /// </summary>
        /// <param name="lockedCamera">A disposable object that represents the locked configurable primary camera.</param>
        /// <returns><see langword="true"/> if a configurable camera is available in the current session
        /// configuration and can be locked. Otherwise, <see langword="false"/>.</returns>
        /// <remarks>Requires iOS 16 or newer and a device with an ultra-wide camera.</remarks>
        /// <seealso cref="advancedCameraConfigurationSupported"/>
        public bool TryGetLockedCamera(out ARKitLockedCamera lockedCamera) =>
            ((ARKitProvider)provider).TryGetLockedCamera(out lockedCamera);

        class ARKitProvider : Provider
        {
            /// <summary>
            /// The shader property name for the luminance component of the camera video frame.
            /// </summary>
            const string k_TextureYPropertyName = "_textureY";

            /// <summary>
            /// The shader property name for the chrominance components of the camera video frame.
            /// </summary>
            const string k_TextureCbCrPropertyName = "_textureCbCr";

            /// <summary>
            /// The shader property name identifier for the luminance component of the camera video frame.
            /// </summary>
            static readonly int k_TextureYPropertyNameId = Shader.PropertyToID(k_TextureYPropertyName);

            /// <summary>
            /// The shader property name identifier for the chrominance components of the camera video frame.
            /// </summary>
            static readonly int k_TextureCbCrPropertyNameId = Shader.PropertyToID(k_TextureCbCrPropertyName);

            /// <summary>
            /// The shader keywords to disable when the Built-in Render Pipeline is enabled.
            /// </summary>
            static readonly List<string> k_BuiltInRPKeywordsToDisable = new(){ k_BackgroundShaderKeywordURP };
            static readonly ReadOnlyList<string> k_BuiltInRPKeywordsToDisableReadOnly = new(k_BuiltInRPKeywordsToDisable);
            static readonly XRShaderKeywords k_BuiltInRPShaderKeywords = new(null, k_BuiltInRPKeywordsToDisableReadOnly);

            /// <summary>
            /// The current <see cref="RenderingThreadingMode"/> use by Unity's rendering pipeline.
            /// </summary>
            static readonly RenderingThreadingMode k_RenderingThreadingMode = SystemInfo.renderingThreadingMode;

            /// <summary>
            /// Indicates whether multithreaded rendering is enabled.
            /// </summary>
            static readonly bool k_MultithreadedRenderingEnabled =
                k_RenderingThreadingMode is RenderingThreadingMode.MultiThreaded or RenderingThreadingMode.NativeGraphicsJobs;

#if URP_7_OR_NEWER
            /// <summary>
            /// The shader keywords to enable when URP is enabled.
            /// </summary>
            static readonly List<string> k_URPEnabledKeywordList = new(){ k_BackgroundShaderKeywordURP };
            static readonly ReadOnlyList<string> k_URPEnabledKeywordListReadOnly = new(k_URPEnabledKeywordList);

            static readonly XRShaderKeywords k_URPShaderKeywords = new(k_URPEnabledKeywordListReadOnly, null);
#endif // URP_7_OR_NEWER

            Material m_BeforeOpaqueCameraMaterial;
            Material m_AfterOpaqueCameraMaterial;

            /// <remarks>
            /// This subsystem will lazily create the camera materials depending on the <see cref="currentBackgroundRenderingMode"/>.
            /// Once created, the materials exist for the lifespan of the subsystem.
            /// </remarks>
            public override Material cameraMaterial
            {
                get
                {
                    switch (currentBackgroundRenderingMode)
                    {
                        case XRCameraBackgroundRenderingMode.BeforeOpaques:
                            return m_BeforeOpaqueCameraMaterial ??= CreateCameraMaterial(k_BeforeOpaquesBackgroundShaderName);

                        case XRCameraBackgroundRenderingMode.AfterOpaques:
                            return m_AfterOpaqueCameraMaterial ??= CreateCameraMaterial(k_AfterOpaquesBackgroundShaderName);

                        default:
                            Debug.LogError($"Unable to create material for unknown background rendering mode {currentBackgroundRenderingMode}.");
                            return null;
                    }
                }
            }

            public override bool permissionGranted => NativeApi.UnityARKit_Camera_IsCameraPermissionGranted();

            public ARKitProvider()
            {
                NativeApi.UnityARKit_Camera_Construct(k_TextureYPropertyNameId, k_TextureCbCrPropertyNameId,
                    k_MultithreadedRenderingEnabled);
            }

            // ReSharper disable once MemberCanBeMadeStatic.Local -- This method is dependent on a valid provider instance
            public HighResolutionCpuImagePromise TryAcquireHighResolutionCpuImage() => new();

            public override Feature currentCamera => NativeApi.UnityARKit_Camera_GetCurrentCamera();

            public override Feature requestedCamera
            {
                get => Api.GetRequestedFeatures();
                set
                {
                    Api.SetFeatureRequested(Feature.AnyCamera, false);
                    Api.SetFeatureRequested(value, true);
                }
            }

            public override XRCameraBackgroundRenderingMode currentBackgroundRenderingMode
            {
                get
                {
                    switch (m_RequestedCameraRenderingMode)
                    {
                        case XRSupportedCameraBackgroundRenderingMode.Any:
                        case XRSupportedCameraBackgroundRenderingMode.BeforeOpaques:
                            return XRCameraBackgroundRenderingMode.BeforeOpaques;

                        case XRSupportedCameraBackgroundRenderingMode.AfterOpaques:
                            return XRCameraBackgroundRenderingMode.AfterOpaques;

                        case XRSupportedCameraBackgroundRenderingMode.None:
                        default:
                            return XRCameraBackgroundRenderingMode.None;
                    }
                }
            }

            public override XRSupportedCameraBackgroundRenderingMode requestedBackgroundRenderingMode
            {
                get => m_RequestedCameraRenderingMode;
                set => m_RequestedCameraRenderingMode = value;
            }

            XRSupportedCameraBackgroundRenderingMode m_RequestedCameraRenderingMode = XRSupportedCameraBackgroundRenderingMode.Any;

            public override XRSupportedCameraBackgroundRenderingMode supportedBackgroundRenderingMode
                => XRSupportedCameraBackgroundRenderingMode.Any;

            public override void Start() => NativeApi.UnityARKit_Camera_Start();
            public override void Stop() => NativeApi.UnityARKit_Camera_Stop();
            public override void Destroy() => NativeApi.UnityARKit_Camera_Destruct();

            public override bool TryGetFrame(XRCameraParams cameraParams, out XRCameraFrame cameraFrame)
                => NativeApi.UnityARKit_Camera_TryGetFrame(cameraParams, out cameraFrame);

            public override bool autoFocusEnabled => NativeApi.UnityARKit_Camera_GetAutoFocusEnabled();

            public override bool autoFocusRequested
            {
                get => Api.GetRequestedFeatures().All(Feature.AutoFocus);
                set => Api.SetFeatureRequested(Feature.AutoFocus, value);
            }

            /// <summary>
            /// Gets the current light estimation mode as reported by the
            /// [ARSession's configuration](https://developer.apple.com/documentation/arkit/arconfiguration/2923546-lightestimationenabled).
            /// </summary>
            public override Feature currentLightEstimation => NativeApi.GetCurrentLightEstimation();

            public override Feature requestedLightEstimation
            {
                get => Api.GetRequestedFeatures();
                set
                {
                    Api.SetFeatureRequested(Feature.AnyLightEstimation, false);
                    Api.SetFeatureRequested(value.Intersection(Feature.AnyLightEstimation), true);
                }
            }

            /// <summary>
            /// Get a pointer to a configurable primary camera associated with the current session
            /// configuration. You can configure advanced camera hardware properties in your own
            /// Objective-C code using this pointer.
            /// </summary>
            /// <returns>A pointer to a configurable [AVCaptureDevice]
            /// (https://developer.apple.com/documentation/avfoundation/avcapturedevice?language=objc) on supported devices.
            /// Otherwise, <c>IntPtr.Zero</c>.</returns>
            /// <remarks>Requires iOS 16 or newer and a device with an ultra-wide camera.</remarks>
            internal IntPtr nativePtr => NativeApi.UnityARKit_Camera_GetConfigurableCamera();

            public bool TryGetLockedCamera(out ARKitLockedCamera lockedCamera)
            {
                lockedCamera = default;

                var ptr = nativePtr;
                if (ptr != IntPtr.Zero && NativeApi.UnityARKit_Camera_TryLockCamera(ptr))
                {
                    lockedCamera = ARKitLockedCamera.FromIntPtr(ptr);
                    return true;
                }

                return false;
            }

            public override bool TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics)
                => NativeApi.UnityARKit_Camera_TryGetIntrinsics(out cameraIntrinsics);

            public override NativeArray<XRCameraConfiguration> GetConfigurations(
                XRCameraConfiguration defaultCameraConfiguration,
                Allocator allocator)
            {
                IntPtr configurations = NativeApi.UnityARKit_Camera_AcquireConfigurations(
                    out int configurationsCount,
                    out int configurationSize);

                try
                {
                    unsafe
                    {
                        return NativeCopyUtility.PtrToNativeArrayWithDefault(
                            defaultCameraConfiguration,
                            (void*)configurations,
                            configurationSize, configurationsCount,
                            allocator);
                    }
                }
                finally
                {
                    NativeApi.UnityARKit_Camera_ReleaseConfigurations(configurations);
                }
            }

            /// <summary>
            /// The current camera configuration.
            /// </summary>
            /// <value>The current camera configuration if it exists. Otherwise, <see langword="null"/>.</value>
            /// <exception cref="System.ArgumentException">Thrown when setting the current configuration if the given
            /// configuration is not a valid, supported camera configuration.</exception>
            /// <exception cref="System.InvalidOperationException">Thrown when setting the current configuration if the
            /// implementation is unable to set the current camera configuration for various reasons such as:
            /// <list type="bullet">
            /// <item><description>Version of iOS does not support camera configurations.</description></item>
            /// <item><description>ARKit session is invalid.</description></item>
            /// </list>
            /// </exception>
            public override XRCameraConfiguration? currentConfiguration
            {
                get
                {
                    if (NativeApi.UnityARKit_Camera_TryGetCurrentConfiguration(out XRCameraConfiguration cameraConfiguration))
                    {
                        return cameraConfiguration;
                    }

                    return null;
                }
                set
                {
                    // Assert that the camera configuration is not null.
                    // The XRCameraSubsystem should have already checked this.
                    Debug.Assert(value != null, "Cannot set the current camera configuration to null");

                    switch (NativeApi.UnityARKit_Camera_TrySetCurrentConfiguration((XRCameraConfiguration)value))
                    {
                        case CameraConfigurationResult.Success:
                            break;
                        case CameraConfigurationResult.Unsupported:
                            throw new InvalidOperationException(
                                "cannot set camera configuration because ARKit version does not support camera configurations");
                        case CameraConfigurationResult.InvalidCameraConfiguration:
                            throw new ArgumentException(
                                "camera configuration does not exist in the available configurations", nameof(value));
                        case CameraConfigurationResult.InvalidSession:
                            throw new InvalidOperationException(
                                "cannot set camera configuration because the ARKit session is not valid");
                        default:
                            throw new InvalidOperationException("cannot set camera configuration for ARKit");
                    }
                }
            }

            public override unsafe NativeArray<XRTextureDescriptor> GetTextureDescriptors(
                XRTextureDescriptor defaultDescriptor,
                Allocator allocator)
            {
                var textureDescriptors = NativeApi.UnityARKit_Camera_AcquireTextureDescriptors(
                    out int length, out int elementSize);

                try
                {
                    return NativeCopyUtility.PtrToNativeArrayWithDefault(
                        defaultDescriptor, textureDescriptors, elementSize, length, allocator);
                }
                finally
                {
                    NativeApi.UnityARKit_Camera_ReleaseTextureDescriptors(textureDescriptors);
                }
            }

            [Obsolete]
            public override void GetMaterialKeywords(out List<string> enabledKeywords, out List<string> disabledKeywords)
            {
                if (GraphicsSettings.currentRenderPipeline == null)
                {
                    enabledKeywords = null;
                    disabledKeywords = k_BuiltInRPKeywordsToDisable;
                }
#if URP_7_OR_NEWER
                else if (GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset)
                {
                    enabledKeywords = k_URPEnabledKeywordList;
                    disabledKeywords = null;
                }
#endif // URP_7_OR_NEWER
                else
                {
                    enabledKeywords = null;
                    disabledKeywords = null;
                }
            }

            [Obsolete]
            public override ShaderKeywords GetShaderKeywords()
            {
                if (GraphicsSettings.currentRenderPipeline == null)
                    return new ShaderKeywords(null, k_BuiltInRPKeywordsToDisable.AsReadOnly());
#if URP_7_OR_NEWER
                if (GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset)
                    return new ShaderKeywords(k_URPEnabledKeywordList.AsReadOnly(), null);
#endif // URP_7_OR_NEWER
                return default;
            }

            public override XRShaderKeywords GetShaderKeywords2()
            {
                if (GraphicsSettings.currentRenderPipeline == null)
                    return k_BuiltInRPShaderKeywords;
#if URP_7_OR_NEWER
                if (GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset)
                    return k_URPShaderKeywords;
#endif // URP_7_OR_NEWER
                return default;
            }

            public override XRCpuImage.Api cpuImageApi => ARKitCpuImageApi.instance;

            public override bool TryAcquireLatestCpuImage(out XRCpuImage.Cinfo cameraImageCinfo)
                => ARKitCpuImageApi.TryAcquireLatestImage(ARKitCpuImageApi.ImageType.Camera, out cameraImageCinfo);

            /// <summary>
            /// Called on the render thread by background rendering code immediately before the background
            /// is rendered.
            /// For ARKit, this is required in order to free the metal textures retained on the main thread.
            /// </summary>
            /// <param name="id">Platform-specific data.</param>
            public override void OnBeforeBackgroundRender(int id)
            {
                // callback to schedule the release of the metal texture buffers after rendering is complete
                NativeApi.UnityARKit_Camera_ScheduleReleaseTextureBuffers();
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
            if (!Api.AtLeast11_0())
                return;

            var cameraSubsystemCinfo = new XRCameraSubsystemDescriptor.Cinfo
            {
                id = k_SubsystemId,
                providerType = typeof(ARKitProvider),
                subsystemTypeOverride = typeof(ARKitCameraSubsystem),
                supportsAverageBrightness = false,
                supportsAverageColorTemperature = true,
                supportsColorCorrection = false,
                supportsDisplayMatrix = true,
                supportsProjectionMatrix = true,
                supportsTimestamp = true,
                supportsCameraConfigurations = true,
                supportsCameraImage = true,
                supportsAverageIntensityInLumens = true,
                supportsFocusModes = true,
                supportsFaceTrackingAmbientIntensityLightEstimation = true,
                supportsFaceTrackingHDRLightEstimation = true,
                supportsWorldTrackingAmbientIntensityLightEstimation = true,
                supportsWorldTrackingHDRLightEstimation = false,
                supportsCameraGrain = Api.AtLeast13_0(),
                supportsExifData = Api.AtLeast16_0(),
            };

            XRCameraSubsystemDescriptor.Register(cameraSubsystemCinfo);
        }

        static class NativeApi
        {
#if UNITY_XR_ARKIT_LOADER_ENABLED
            [DllImport("__Internal", EntryPoint="UnityARKit_Camera_GetCurrentLightEstimation")]
            public static extern Feature GetCurrentLightEstimation();

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_Construct(
                int textureYPropertyNameId,
                int textureCbCrPropertyNameId,
                bool mtRenderingEnabled);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_Destruct();

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_Start();

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_Stop();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryGetFrame(
                XRCameraParams cameraParams,
                out XRCameraFrame cameraFrame);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_IsCameraPermissionGranted();

            [DllImport("__Internal")]
            public static extern IntPtr UnityARKit_Camera_AcquireConfigurations(
                out int configurationsCount,
                out int configurationSize);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_ReleaseConfigurations(IntPtr configurations);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryGetCurrentConfiguration(
                out XRCameraConfiguration cameraConfiguration);

            [DllImport("__Internal")]
            public static extern CameraConfigurationResult UnityARKit_Camera_TrySetCurrentConfiguration(
                XRCameraConfiguration cameraConfiguration);

            [DllImport("__Internal")]
            public static extern unsafe void* UnityARKit_Camera_AcquireTextureDescriptors(
                out int length, out int elementSize);

            [DllImport("__Internal")]
            public static extern unsafe void UnityARKit_Camera_ReleaseTextureDescriptors(
                void* descriptors);

            [DllImport("__Internal")]
            public static extern Feature UnityARKit_Camera_GetCurrentCamera();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_GetAutoFocusEnabled();

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_ScheduleReleaseTextureBuffers();

            [DllImport("__Internal", EntryPoint = "UnityARKit_Camera_TryAcquireHighResolutionCpuImage")]
            public static extern void TryAcquireHighResolutionCpuImage(IntPtr callback, out XRCpuImage.Cinfo cpuImageCinfo);

            [DllImport("__Internal")]
            public static extern IntPtr UnityARKit_Camera_GetConfigurableCamera();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TryLockCamera(IntPtr cameraDevicePtr);
#else
            public static Feature GetCurrentLightEstimation() => Feature.None;

            public static void UnityARKit_Camera_Construct(
                int textureYPropertyNameId,
                int textureCbCrPropertyNameId,
                bool mtRenderingEnabled)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void UnityARKit_Camera_Destruct()
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void UnityARKit_Camera_Start()
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void UnityARKit_Camera_Stop()
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool UnityARKit_Camera_TryGetFrame(XRCameraParams cameraParams, out XRCameraFrame cameraFrame)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool UnityARKit_Camera_TryGetIntrinsics(out XRCameraIntrinsics cameraIntrinsics)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool UnityARKit_Camera_IsCameraPermissionGranted() => false;

            public static IntPtr UnityARKit_Camera_AcquireConfigurations(
                out int configurationsCount,
                out int configurationSize)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void UnityARKit_Camera_ReleaseConfigurations(IntPtr configurations)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool UnityARKit_Camera_TryGetCurrentConfiguration(out XRCameraConfiguration cameraConfiguration)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static CameraConfigurationResult UnityARKit_Camera_TrySetCurrentConfiguration(XRCameraConfiguration cameraConfiguration)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static unsafe void* UnityARKit_Camera_AcquireTextureDescriptors(out int length, out int elementSize)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static unsafe void UnityARKit_Camera_ReleaseTextureDescriptors(void* descriptors)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void UnityARKit_Camera_ScheduleReleaseTextureBuffers()
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static Feature UnityARKit_Camera_GetCurrentCamera() => Feature.None;

            public static bool UnityARKit_Camera_GetAutoFocusEnabled() => false;

            public static IntPtr UnityARKit_Camera_GetConfigurableCamera() => IntPtr.Zero;

            public static bool UnityARKit_Camera_TryLockCamera(IntPtr cameraDevicePtr)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void TryAcquireHighResolutionCpuImage(IntPtr callback, out XRCpuImage.Cinfo cpuImageCinfo)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);
#endif
        }
    }
}
