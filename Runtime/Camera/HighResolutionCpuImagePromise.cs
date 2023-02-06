using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents an asynchronous promise of a high-resolution CPU image.
    /// </summary>
    public class HighResolutionCpuImagePromise : Promise<HighResolutionCpuImagePromise.Result>
    {
        /// <summary>
        /// The instance currently awaiting a result, or <see langword="null"/> if no instance currently awaits a result.
        /// </summary>
        /// <remarks>
        /// <para>IL2CPP cannot currently marshal instance methods, therefore the callback function passed to the
        /// native API must be static. In order to call the instance method <see cref="Promise{T}.Resolve"/> from within
        /// the callback, this class must keep a static reference to the instance that will be resolved by the callback.</para>
        /// <para>This design has a limitation that only one promise instance at a time can await a result; however, the
        /// native ARKit API for high resolution frame capture has the same limitation, so there is no net negative impact.</para>
        /// <para>See ARKit's <see href="https://developer.apple.com/documentation/arkit/arsession/3975720-capturehighresolutionframewithco?language=objc">
        /// captureHighResolutionFrameWithCompletion</see> documentation for more information.</para> 
        /// </remarks>
        static HighResolutionCpuImagePromise s_Instance;

        /// <summary>
        /// The <see cref="XRCpuImage.Cinfo"/> object whose fields will be assigned by the native API.
        /// </summary>
        static XRCpuImage.Cinfo s_Cinfo;

        /// <summary>
        /// The function pointer for <see cref="OnPromiseComplete"/> to be marshalled to native API.
        /// </summary>
        static IntPtr s_OnPromiseCompleteFuncPtr = Marshal.GetFunctionPointerForDelegate((HighResolutionCpuImageCallback)OnPromiseComplete);

        /// <summary>
        /// Callback passed to the native API to call when the high resolution frame capture is complete.
        /// </summary>
        delegate void HighResolutionCpuImageCallback(bool wasSuccessful);

        internal HighResolutionCpuImagePromise()
        {
            if (s_Instance != null)
            {
                Debug.LogError("A previous request for a high resolution capture hasn't completed yet. Subsequent requests will fail until the request in progress is completed.");
                Resolve(default);
                return;
            }

            s_Instance = this;
            ARKitCameraSubsystem.TryAcquireHighResolutionCpuImageNative(s_OnPromiseCompleteFuncPtr, out s_Cinfo);
        }

        /// <summary>
        /// Unused but required by parent class.
        /// </summary>
        protected override void OnKeepWaiting()
        { }

        [MonoPInvokeCallback(typeof(HighResolutionCpuImageCallback))]
        static void OnPromiseComplete(bool wasSuccessful)
        {
            var result = new Result
            {
                wasSuccessful = wasSuccessful,
                highResolutionCpuImage = wasSuccessful ? new XRCpuImage(ARKitCpuImageApi.instance, s_Cinfo) : default
            };

            s_Instance.Resolve(result);
            s_Instance = null;
        }

        /// <summary>
        /// Represents the result of an asynchronous request for a high resolution CPU image.
        /// </summary>
        public struct Result
        {
            /// <summary>
            /// If <see langword="true"/>, <see cref="highResolutionCpuImage"/> is initialized with a valid CPU image.
            /// If <see langword="false"/>, <see cref="highResolutionCpuImage"/> has a default value.
            /// </summary>
            public bool wasSuccessful;

            /// <summary>
            /// The high resolution CPU image. You are responsible to <see cref="XRCpuImage.Dispose"/> this image if
            /// <see cref="wasSuccessful"/> is <see langword="true"/>.
            /// </summary>
            /// <seealso cref="wasSuccessful"/>
            public XRCpuImage highResolutionCpuImage;
        }
    }
}
