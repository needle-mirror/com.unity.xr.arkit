using System;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
#if UNITY_XR_ARKIT_LOADER_ENABLED
using System.Runtime.InteropServices;
#endif

namespace UnityEngine.XR.ARKit
{
    class ARKitCpuImageApi : XRCpuImage.Api
    {
        /// <summary>
        /// The type of image to acquire. Used by <see cref="ARKitCpuImageApi.TryAcquireLatestImage"/>.
        /// </summary>
        public enum ImageType
        {
            Camera,
            HumanDepth,
            HumanStencil,
            EnvironmentDepth,
            EnvironmentDepthConfidence,
            RawEnvironmentDepth,
            TemporallySmoothedEnvironmentDepth,
        }

        /// <summary>
        /// The shared API instance.
        /// </summary>
        public static ARKitCpuImageApi instance { get; } = new();

        /// <summary>
        /// Dispose an existing native image identified by <paramref name="nativeHandle"/>.
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for this camera image.</param>
        public override void DisposeImage(int nativeHandle) => Native.DisposeImage(nativeHandle);

        /// <summary>
        /// Get information about an image plane from a native image handle by index.
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for this camera image.</param>
        /// <param name="planeIndex">The index of the plane to get.</param>
        /// <param name="planeCinfo">The returned camera plane information if successful.</param>
        /// <returns><see langword="true"/> if the image plane was acquired. Otherwise, <see langword="false"/>.</returns>
        public override bool TryGetPlane(
            int nativeHandle,
            int planeIndex,
            out XRCpuImage.Plane.Cinfo planeCinfo)
            => Native.TryGetPlane(nativeHandle, planeIndex, out planeCinfo);

        /// <summary>
        /// Determine whether a native image handle returned by
        /// <see cref="ARKitCameraSubsystem.Provider.TryAcquireLatestCpuImage"/> is currently valid. An image can
        /// become invalid if it has been disposed.
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for the camera image in question.</param>
        /// <returns><see langword="true"/>, if it is a valid handle. Otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// If a handle is valid, <see cref="TryConvert"/> and <see cref="TryGetConvertedDataSize"/> should not fail.
        /// </remarks>
        /// <seealso cref="DisposeImage"/>
        public override bool NativeHandleValid(int nativeHandle) => Native.HandleValid(nativeHandle);

        /// <summary>
        /// Tries to acquire the latest image of type <paramref name="imageType"/>.
        /// </summary>
        /// <param name="imageType">The type of image to acquire.</param>
        /// <param name="cinfo">On success, populated with construction information for an <see cref="XRCpuImage"/>.</param>
        /// <returns><see langword="true"/> if the latest image of type <paramref name="imageType"/> was successfully acquired.
        /// Otherwise, <see langword="false"/>.</returns>
        public static bool TryAcquireLatestImage(ImageType imageType, out XRCpuImage.Cinfo cinfo)
            => Native.TryAcquireLatestImage(imageType, out cinfo);

        /// <summary>
        /// Get the status of an existing asynchronous conversion request.
        /// </summary>
        /// <param name="requestId">The unique identifier associated with a request.</param>
        /// <returns>The state of the request.</returns>
        /// <seealso cref="ConvertAsync(int, XRCpuImage.ConversionParams)"/>
        public override XRCpuImage.AsyncConversionStatus GetAsyncRequestStatus(int requestId)
            => Native.GetAsyncRequestStatus(requestId);

        /// <summary>
        /// Dispose an existing async conversion request.
        /// </summary>
        /// <param name="requestId">A unique identifier for the request.</param>
        /// <seealso cref="ConvertAsync(int, XRCpuImage.ConversionParams)"/>
        public override void DisposeAsyncRequest(int requestId) => Native.DisposeAsyncRequest(requestId);

        /// <summary>
        /// Get the number of bytes required to store an image with the given dimensions and
        /// [TextureFormat](https://docs.unity3d.com/ScriptReference/TextureFormat.html).
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
        /// <param name="dimensions">The dimensions of the output image.</param>
        /// <param name="format">The <c>TextureFormat</c> for the image.</param>
        /// <param name="size">The number of bytes required to store the converted image.</param>
        /// <returns><see langword="true"/> if the output <paramref name="size"/> was set. Otherwise, <see langword="false"/>.</returns>
        public override bool TryGetConvertedDataSize(
            int nativeHandle,
            Vector2Int dimensions,
            TextureFormat format,
            out int size)
            => Native.TryGetConvertedDataSize(nativeHandle, dimensions, format, out size);

        /// <summary>
        /// Convert the image with handle <paramref name="nativeHandle"/> using the provided
        /// <paramref cref="conversionParams"/>.
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
        /// <param name="conversionParams">The parameters to use during the conversion.</param>
        /// <param name="destinationBuffer">A buffer to write the converted image to.</param>
        /// <param name="bufferLength">The number of bytes available in the buffer.</param>
        /// <returns><see langword="true"/> if the image was converted and stored in <paramref name="destinationBuffer"/>.
        /// Otherwise, <see langword="false"/>.</returns>
        public override bool TryConvert(
            int nativeHandle,
            XRCpuImage.ConversionParams conversionParams,
            IntPtr destinationBuffer,
            int bufferLength)
            => Native.TryConvert(nativeHandle, conversionParams, destinationBuffer, bufferLength);

        /// <summary>
        /// Create an asynchronous request to convert a camera image, similar to <see cref="TryConvert"/> except
        /// the conversion should happen on a thread other than the calling (main) thread.
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
        /// <param name="conversionParams">The parameters to use during the conversion.</param>
        /// <returns>A unique identifier for this request.</returns>
        public override int ConvertAsync(int nativeHandle, XRCpuImage.ConversionParams conversionParams)
            => Native.CreateAsyncConversionRequest(nativeHandle, conversionParams);

        /// <summary>
        /// Get a pointer to the image data from a completed asynchronous request. This method should only succeed
        /// if <see cref="GetAsyncRequestStatus"/> returns <see cref="XRCpuImage.AsyncConversionStatus.Ready"/>.
        /// </summary>
        /// <param name="requestId">The unique identifier associated with a request.</param>
        /// <param name="dataPtr">A pointer to the native buffer containing the data.</param>
        /// <param name="dataLength">The number of bytes in <paramref name="dataPtr"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="dataPtr"/> and <paramref name="dataLength"/> were set and point
        /// to the image data.</returns>
        public override bool TryGetAsyncRequestData(int requestId, out IntPtr dataPtr, out int dataLength)
            => Native.TryGetAsyncRequestData(requestId, out dataPtr, out dataLength);

        /// <summary>
        /// Similar to <see cref="ConvertAsync(int, XRCpuImage.ConversionParams)"/> but takes a delegate to
        /// invoke when the request is complete, rather than returning a request id.
        /// </summary>
        /// <param name="nativeHandle">A unique identifier for the camera image to convert.</param>
        /// <param name="conversionParams">The parameters to use during the conversion.</param>
        /// <param name="callback">A delegate which must be invoked when the request is complete, whether the
        /// conversion was successful or not.</param>
        /// <param name="context">A native pointer which must be passed back unaltered to
        /// <paramref name="callback"/>.</param>
        /// <remarks>
        /// If the first parameter to <paramref name="callback"/> is
        /// <see cref="XRCpuImage.AsyncConversionStatus.Ready"/> then the <c>dataPtr</c> parameter must be valid
        /// for the duration of the invocation. The data can be destroyed immediately upon return. The
        /// <paramref name="context"/> parameter must be passed back to the <paramref name="callback"/>.
        /// </remarks>
        public override void ConvertAsync(
            int nativeHandle,
            XRCpuImage.ConversionParams conversionParams,
            OnImageRequestCompleteDelegate callback,
            IntPtr context)
            => Native.CreateAsyncConversionRequestWithCallback(nativeHandle, conversionParams, callback, context);

        static readonly HashSet<TextureFormat> s_SupportedVideoConversionFormats = new()
        {
            TextureFormat.Alpha8,
            TextureFormat.R8,
            TextureFormat.RGB24,
            TextureFormat.RGBA32,
            TextureFormat.ARGB32,
            TextureFormat.BGRA32,
        };

        /// <summary>
        /// Determines whether a given
        /// [TextureFormat](https://docs.unity3d.com/ScriptReference/TextureFormat.html) is supported for image
        /// conversion.
        /// </summary>
        /// <param name="image">The <see cref="XRCpuImage"/> to convert.</param>
        /// <param name="format">The [TextureFormat](https://docs.unity3d.com/ScriptReference/TextureFormat.html)
        /// to test.</param>
        /// <returns><see langword="true"/> if <paramref name="image"/> can be converted to <paramref name="format"/>.
        /// Otherwise, <see langword="false"/>.</returns>
        public override bool FormatSupported(XRCpuImage image, TextureFormat format)
        {
            return image.format switch
            {
                XRCpuImage.Format.IosYpCbCr420_8BiPlanarFullRange => s_SupportedVideoConversionFormats.Contains(format),
                XRCpuImage.Format.OneComponent8 => format is TextureFormat.R8 or TextureFormat.Alpha8,
                XRCpuImage.Format.DepthFloat32 => format == TextureFormat.RFloat,
                _ => false
            };
        }

        static class Native
        {
#if UNITY_XR_ARKIT_LOADER_ENABLED
            [DllImport("__Internal", EntryPoint = "UnityARKit_CpuImage_TryAcquireLatestImage")]
            public static extern bool TryAcquireLatestImage(ImageType imageType, out XRCpuImage.Cinfo cinfo);

            [DllImport("__Internal", EntryPoint = "UnityARKit_CpuImage_DisposeImage")]
            public static extern void DisposeImage(int nativeHandle);

            [DllImport("__Internal", EntryPoint = "UnityARKit_CpuImage_TryGetPlane")]
            public static extern bool TryGetPlane(
                int nativeHandle,
                int planeIndex,
                out XRCpuImage.Plane.Cinfo planeCinfo);

            [DllImport("__Internal", EntryPoint = "UnityARKit_CpuImage_HandleValid")]
            public static extern bool HandleValid(int nativeHandle);

            [DllImport("__Internal", EntryPoint = "UnityARKit_CpuImage_GetAsyncRequestStatus")]
            public static extern XRCpuImage.AsyncConversionStatus GetAsyncRequestStatus(int requestId);

            [DllImport("__Internal", EntryPoint = "UnityARKit_CpuImage_DisposeAsyncRequest")]
            public static extern void DisposeAsyncRequest(int requestHandle);

            [DllImport("__Internal", EntryPoint = "UnityARKit_CpuImage_TryGetConvertedDataSize")]
            public static extern bool TryGetConvertedDataSize(
                int nativeHandle, Vector2Int dimensions, TextureFormat format, out int size);

            [DllImport("__Internal", EntryPoint = "UnityARKit_CpuImage_TryConvert")]
            public static extern bool TryConvert(
                int nativeHandle,
                XRCpuImage.ConversionParams conversionParams,
                IntPtr buffer,
                int bufferLength);

            [DllImport("__Internal", EntryPoint = "UnityARKit_CpuImage_CreateAsyncConversionRequest")]
            public static extern int CreateAsyncConversionRequest(
                int nativeHandle, XRCpuImage.ConversionParams conversionParams);

            [DllImport("__Internal", EntryPoint = "UnityARKit_CpuImage_TryGetAsyncRequestData")]
            public static extern bool TryGetAsyncRequestData(
                int requestHandle, out IntPtr dataPtr, out int dataLength);

            [DllImport("__Internal", EntryPoint = "UnityARKit_CpuImage_CreateAsyncConversionRequestWithCallback")]
            public static extern void CreateAsyncConversionRequestWithCallback(
                int nativeHandle,
                XRCpuImage.ConversionParams conversionParams,
                OnImageRequestCompleteDelegate callback,
                IntPtr context);
#else
            public static bool TryAcquireLatestImage(ImageType imageType, out XRCpuImage.Cinfo cinfo)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void DisposeImage(int nativeHandle)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool TryGetPlane(
                int nativeHandle, int planeIndex, out XRCpuImage.Plane.Cinfo planeCinfo)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool HandleValid(int nativeHandle)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static XRCpuImage.AsyncConversionStatus GetAsyncRequestStatus(int requestId)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void DisposeAsyncRequest(int requestHandle)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool TryGetConvertedDataSize(
                int nativeHandle, Vector2Int dimensions, TextureFormat format, out int size)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool TryConvert(
                int nativeHandle, XRCpuImage.ConversionParams conversionParams, IntPtr buffer, int bufferLength)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static int CreateAsyncConversionRequest(
                int nativeHandle, XRCpuImage.ConversionParams conversionParams)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool TryGetAsyncRequestData(int requestHandle, out IntPtr dataPtr, out int dataLength)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void CreateAsyncConversionRequestWithCallback(
                int nativeHandle,
                XRCpuImage.ConversionParams conversionParams,
                OnImageRequestCompleteDelegate callback,
                IntPtr context)
                => throw new NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);
#endif
        }
    }
}
