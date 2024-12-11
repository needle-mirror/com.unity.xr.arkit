using System;
using System.Runtime.InteropServices;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents a disposable locked configurable primary camera associated with the current
    /// session configuration. Use this object to configure advanced camera hardware properties.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ARKitLockedCamera : IEquatable<ARKitLockedCamera>, IDisposable
    {
        IntPtr m_Self;
        ARKitExposureMode m_ExposureMode;
        ARKitWhiteBalanceMode m_WhiteBalanceMode;
        ARKitFocusMode m_FocusMode;

        /// <summary>
        /// Get the native pointer of [AVCaptureDevice]
        /// (https://developer.apple.com/documentation/avfoundation/avcapturedevice?language=objc).
        /// </summary>
        /// <returns>Returns the native pointer of [AVCaptureDevice]
        /// (https://developer.apple.com/documentation/avfoundation/avcapturedevice?language=objc).</returns>
        public IntPtr AsIntPtr() => m_Self;

        /// <summary>
        /// Get the supported exposure modes for the camera.
        /// </summary>
        public ARKitExposureMode supportedExposureModes => NativeApi.UnityARKit_Camera_GetSupportedExposureModes(m_Self);

        /// <summary>
        /// Get the supported white balance modes for the camera.
        /// </summary>
        public ARKitWhiteBalanceMode supportedWhiteBalanceModes => NativeApi.UnityARKit_Camera_GetSupportedWhiteBalanceModes(m_Self);

        /// <summary>
        /// Get the supported focus modes for the camera.
        /// </summary>
        public ARKitFocusMode supportedFocusModes => NativeApi.UnityARKit_Camera_GetSupportedFocusModes(m_Self);

        /// <summary>
        /// Get the current exposure mode for the camera.
        /// </summary>
        /// <seealso cref="requestedExposureMode"/>
        public ARKitExposureMode currentExposureMode => NativeApi.UnityARKit_Camera_GetExposureMode(m_Self);

        /// <summary>
        /// Get the current white balance mode for the camera device.
        /// </summary>
        /// <seealso cref="requestedWhiteBalanceMode"/>
        public ARKitWhiteBalanceMode currentWhiteBalanceMode => NativeApi.UnityARKit_Camera_GetWhiteBalanceMode(m_Self);

        /// <summary>
        /// Get the current focus mode for the camera.
        /// </summary>
        /// <seealso cref="requestedFocusMode"/>
        public ARKitFocusMode currentFocusMode => NativeApi.UnityARKit_Camera_GetFocusMode(m_Self);

        /// <summary>
        /// Get or set the requested exposure mode for the camera.
        /// </summary>
        /// <seealso cref="currentExposureMode"/>
        public ARKitExposureMode requestedExposureMode
        {
            get => m_ExposureMode;
            set
            {
                m_ExposureMode = value;
                NativeApi.UnityARKit_Camera_SetExposureMode(m_Self, value);
            }
        }

        /// <summary>
        /// Get or set the requested white balance mode for the camera device.
        /// </summary>
        /// <seealso cref="currentWhiteBalanceMode"/>
        public ARKitWhiteBalanceMode requestedWhiteBalanceMode
        {
            get => m_WhiteBalanceMode;
            set
            {
                m_WhiteBalanceMode = value;
                NativeApi.UnityARKit_Camera_SetWhiteBalanceMode(m_Self, value);
            }
        }

        /// <summary>
        /// Get or set the requested focus mode for the camera.
        /// </summary>
        /// <seealso cref="currentFocusMode"/>
        public ARKitFocusMode requestedFocusMode
        {
            get => m_FocusMode;
            set
            {
                m_FocusMode = value;
                NativeApi.UnityARKit_Camera_SetFocusMode(m_Self, value);
            }
        }

        /// <summary>
        /// Get the supported exposure range of the camera.
        /// </summary>
        public ARKitExposureRange exposureRange
        {
            get
            {
                NativeApi.UnityARKit_Camera_GetExposureRange(m_Self, out var range);
                return range;
            }
        }

        /// <summary>
        /// Get the supported white balance gain values.
        /// </summary>
        public ARKitWhiteBalanceRange whiteBalanceRange
        {
            get
            {
                NativeApi.UnityARKit_Camera_GetWhiteBalanceRange(m_Self, out var range);
                return range;
            }
        }

        /// <summary>
        /// Get the supported focus range of the camera.
        /// </summary>
        public ARKitFocusRange focusRange
        {
            get
            {
                NativeApi.UnityARKit_Camera_GetFocusRange(m_Self, out var range);
                return range;
            }
        }

        /// <summary>
        /// Get or set the current exposure of the camera.
        /// </summary>
        public ARKitExposure exposure
        {
            get
            {
                NativeApi.UnityARKit_Camera_GetExposure(m_Self, out var duration, out var iso);
                return new ARKitExposure(duration, iso);
            }

            set => NativeApi.UnityARKit_Camera_TrySetExposure(m_Self, value.duration, value.iso);
        }

        /// <summary>
        /// Get or set the current white balance of the camera.
        /// </summary>
        public ARKitWhiteBalanceGains whiteBalance
        {
            get
            {
                NativeApi.UnityARKit_Camera_GetWhiteBalance(m_Self, out var blue, out var green, out var red);
                return new ARKitWhiteBalanceGains(blue, green, red);
            }

            set => NativeApi.UnityARKit_Camera_TrySetWhiteBalance(m_Self, value.blue, value.green, value.red);
        }

        /// <summary>
        /// Get or set the current focus of the camera.
        /// </summary>
        public ARKitFocus focus
        {
            get
            {
                NativeApi.UnityARKit_Camera_GetFocus(m_Self, out var lensPosition);
                return new ARKitFocus(lensPosition);
            }

            set => NativeApi.UnityARKit_Camera_TrySetFocus(m_Self, value.lensPosition);
        }

        ARKitLockedCamera(IntPtr value)
        {
            m_Self = value;
            m_ExposureMode = ARKitExposureMode.Auto;
            m_WhiteBalanceMode = ARKitWhiteBalanceMode.Auto;
            m_FocusMode = ARKitFocusMode.Auto;
        }

        /// <summary>
        /// Creates an <see cref="ARKitLockedCamera"/> from an existing native pointer. The native pointer must point to
        /// a valid [AVCaptureDevice](https://developer.apple.com/documentation/avfoundation/avcapturedevice?language=objc).
        /// </summary>
        /// <param name="value">A pointer to [AVCaptureDevice]
        /// (https://developer.apple.com/documentation/avfoundation/avcapturedevice?language=objc).</param>
        /// <returns>Returns an <see cref="ARKitLockedCamera"/> whose underlying [AVCaptureDevice]
        /// (https://developer.apple.com/documentation/avfoundation/avcapturedevice?language=objc) pointer is
        ///     <paramref name="value"/>.</returns>
        internal static ARKitLockedCamera FromIntPtr(IntPtr value) => new(value);

        /// <summary>
        /// Releases the lock on the native camera device. For more information, see ARKit's
        /// [AVCaptureDevice.unlockForConfiguration](https://developer.apple.com/documentation/avfoundation/avcapturedevice/1387917-unlockforconfiguration?language=objc).
        /// </summary>
        public void Dispose()
        {
            if (m_Self == IntPtr.Zero)
                return;

            NativeApi.UnityARKit_Camera_ReleaseCameraLock(m_Self);
            m_Self = IntPtr.Zero;
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <remarks>
        /// Two <see cref="ARKitLockedCamera"/>s are considered equal if their underlying pointers are equal.
        /// </remarks>
        /// <param name="other">The <see cref="ARKitLockedCamera"/> to compare against.</param>
        /// <returns>Returns <see langword="true"/> if the underlying native pointers are the same. Returns <see langword="false"/> otherwise.</returns>
        public bool Equals(ARKitLockedCamera other) => m_Self == other.m_Self;

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare against.</param>
        /// <returns>Returns <see langword="true"/> if <paramref name="obj"/> is an <see cref="ARKitLockedCamera"/> and it compares
        ///     equal to this one using <see cref="Equals(ARKitLockedCamera)"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is ARKitLockedCamera other
                   && Equals(other);
        }

        /// <summary>
        /// Generates a hash code suitable for use with a `HashSet` or `Dictionary`
        /// </summary>
        /// <returns>Returns a hash code for this <see cref="ARKitLockedCamera"/>.</returns>
        public override int GetHashCode() => m_Self.GetHashCode();

        /// <summary>
        /// Tests for equality. Equivalent to <see cref="Equals(ARKitLockedCamera)"/>.
        /// </summary>
        /// <param name="lhs">The instance to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The instance to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        /// <see cref="Equals(ARKitLockedCamera)"/>. Otherwise, returns <see langword="false"/>.</returns>
        public static bool operator ==(ARKitLockedCamera lhs, ARKitLockedCamera rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Tests for inequality. Equivalent to the negation of <see cref="Equals(ARKitLockedCamera)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitLockedCamera"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitLockedCamera"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="false"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        /// <see cref="Equals(ARKitLockedCamera)"/>. Otherwise, returns <see langword="true"/>.</returns>
        public static bool operator !=(ARKitLockedCamera lhs, ARKitLockedCamera rhs) => !lhs.Equals(rhs);

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <remarks>
        /// This equality operator lets you compare an instance with <see langword="null"/> to determine whether its
        /// native pointer is `null`.
        /// </remarks>
        /// <example>
        /// <code>
        /// bool TestForNull(ARKitLockedCamera obj)
        /// {
        ///     if (obj == null)
        ///     {
        ///         // obj.AsIntPtr() is IntPtr.Zero
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <param name="lhs">The nullable `ARKitLockedCamera` to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The nullable `ARKitLockedCamera` to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="true"/> if any of these conditions are met:
        /// - <paramref name="lhs"/> and <paramref name="rhs"/> are both not `null` and their native pointers are equal.
        /// - <paramref name="lhs"/> is `null` and <paramref name="rhs"/>'s native pointer is `null`.
        /// - <paramref name="rhs"/> is `null` and <paramref name="lhs"/>'s native pointer is `null`.
        /// - Both <paramref name="lhs"/> and <paramref name="rhs"/> are `null`.
        ///
        /// Otherwise, returns <see langword="false"/>.
        /// </returns>
        public static bool operator ==(ARKitLockedCamera? lhs, ARKitLockedCamera? rhs)
        {
            var lhsPtr = lhs?.m_Self;
            var rhsPtr = rhs?.m_Self;

            // Both non null; compare pointers
            if (lhsPtr.HasValue && rhsPtr.HasValue)
                return lhsPtr.Value == rhsPtr.Value;

            // rhsPtr is null
            if (lhsPtr.HasValue)
                return lhsPtr.Value == IntPtr.Zero;

            // lhsPtr is null
            if (rhsPtr.HasValue)
                return rhsPtr.Value == IntPtr.Zero;

            // both null
            return true;
        }

        /// <summary>
        /// Tests for inequality.
        /// </summary>
        /// <remarks>
        /// This inequality operator lets you compare an instance with <see langword="null"/> to determine whether its
        /// native pointer is `null`.
        /// </remarks>
        /// <example>
        /// <code>
        /// bool TestForNull(ARKitLockedCamera obj)
        /// {
        ///     if (obj != null)
        ///     {
        ///         // obj.AsIntPtr() is not IntPtr.Zero
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <param name="lhs">The nullable `ARKitLockedCamera` to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The nullable `ARKitLockedCamera` to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="false"/> if any of these conditions are met:
        /// - <paramref name="lhs"/> and <paramref name="rhs"/> are both not `null` and their native pointers are equal.
        /// - <paramref name="lhs"/> is `null` and <paramref name="rhs"/>'s native pointer is `null`.
        /// - <paramref name="rhs"/> is `null` and <paramref name="lhs"/>'s native pointer is `null`.
        /// - Both <paramref name="lhs"/> and <paramref name="rhs"/> are `null`.
        ///
        /// Otherwise, returns <see langword="true"/>.
        /// </returns>
        public static bool operator !=(ARKitLockedCamera? lhs, ARKitLockedCamera? rhs) => !(lhs == rhs);

        /// <summary>
        /// Casts an instance to its underlying native pointer.
        /// </summary>
        /// <param name="lockedCamera">The instance to cast.</param>
        /// <returns>The native pointer.</returns>
        public static explicit operator IntPtr(ARKitLockedCamera lockedCamera) => lockedCamera.AsIntPtr();

        static class NativeApi
        {
#if UNITY_XR_ARKIT_LOADER_ENABLED
            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_ReleaseCameraLock(IntPtr cameraDevice);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_SetExposureMode(IntPtr cameraDevice, ARKitExposureMode exposureMode);

            [DllImport("__Internal")]
            public static extern ARKitExposureMode UnityARKit_Camera_GetExposureMode(IntPtr cameraDevice);

            [DllImport("__Internal")]
            public static extern ARKitExposureMode UnityARKit_Camera_GetSupportedExposureModes(IntPtr cameraDevice);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_SetWhiteBalanceMode(IntPtr cameraDevice, ARKitWhiteBalanceMode whiteBalanceMode);

            [DllImport("__Internal")]
            public static extern ARKitWhiteBalanceMode UnityARKit_Camera_GetWhiteBalanceMode(IntPtr cameraDevice);

            [DllImport("__Internal")]
            public static extern ARKitWhiteBalanceMode UnityARKit_Camera_GetSupportedWhiteBalanceModes(IntPtr cameraDevice);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_SetFocusMode(IntPtr cameraDevice, ARKitFocusMode exposureMode);

            [DllImport("__Internal")]
            public static extern ARKitFocusMode UnityARKit_Camera_GetFocusMode(IntPtr cameraDevice);

            [DllImport("__Internal")]
            public static extern ARKitFocusMode UnityARKit_Camera_GetSupportedFocusModes(IntPtr cameraDevice);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_GetExposureRange(IntPtr cameraDevice, out ARKitExposureRange exposureRange);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_GetWhiteBalanceRange(IntPtr cameraDevice, out ARKitWhiteBalanceRange whiteBalanceRange);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_GetFocusRange(IntPtr cameraDevice, out ARKitFocusRange focusRange);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TrySetExposure(IntPtr cameraDevice, double duration, float iso);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_GetExposure(IntPtr cameraDevice, out double duration, out float iso);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TrySetWhiteBalance(IntPtr cameraDevice, float blue, float green, float red);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_GetWhiteBalance(IntPtr cameraDevice, out float blue, out float green, out float red);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_Camera_TrySetFocus(IntPtr cameraDevice, float lensPosition);

            [DllImport("__Internal")]
            public static extern void UnityARKit_Camera_GetFocus(IntPtr cameraDevice, out float lensPosition);
#else
            public static void UnityARKit_Camera_ReleaseCameraLock(IntPtr cameraDevice)
                => throw new System.NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void UnityARKit_Camera_SetExposureMode(IntPtr cameraDevice, ARKitExposureMode exposureMode)
                => throw new System.NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static ARKitExposureMode UnityARKit_Camera_GetExposureMode(IntPtr cameraDevice) => ARKitExposureMode.None;

            public static ARKitExposureMode UnityARKit_Camera_GetSupportedExposureModes(IntPtr cameraDevice) => ARKitExposureMode.None;

            public static void UnityARKit_Camera_SetWhiteBalanceMode(IntPtr cameraDevice, ARKitWhiteBalanceMode whiteBalanceMode)
                => throw new System.NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static ARKitWhiteBalanceMode UnityARKit_Camera_GetWhiteBalanceMode(IntPtr cameraDevice) => ARKitWhiteBalanceMode.None;

            public static ARKitWhiteBalanceMode UnityARKit_Camera_GetSupportedWhiteBalanceModes(IntPtr cameraDevice) => ARKitWhiteBalanceMode.None;

            public static void UnityARKit_Camera_SetFocusMode(IntPtr cameraDevice, ARKitFocusMode exposureMode)
                => throw new System.NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static ARKitFocusMode UnityARKit_Camera_GetFocusMode(IntPtr cameraDevice) => ARKitFocusMode.None;

            public static ARKitFocusMode UnityARKit_Camera_GetSupportedFocusModes(IntPtr cameraDevice) => ARKitFocusMode.None;

            public static void UnityARKit_Camera_GetExposureRange(IntPtr cameraDevice, out ARKitExposureRange exposureRange)
                => throw new System.NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void UnityARKit_Camera_GetWhiteBalanceRange(IntPtr cameraDevice, out ARKitWhiteBalanceRange whiteBalanceRange)
                => throw new System.NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static void UnityARKit_Camera_GetFocusRange(IntPtr cameraDevice, out ARKitFocusRange focusRange)
                => throw new System.NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool UnityARKit_Camera_TrySetExposure(IntPtr cameraDevice, double duration, float iso) => false;

            public static void UnityARKit_Camera_GetExposure(IntPtr cameraDevice, out double duration, out float iso)
                => throw new System.NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool UnityARKit_Camera_TrySetWhiteBalance(IntPtr cameraDevice, float blue, float green, float red) => false;

            public static void UnityARKit_Camera_GetWhiteBalance(IntPtr cameraDevice, out float blue, out float green, out float red)
                => throw new System.NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);

            public static bool UnityARKit_Camera_TrySetFocus(IntPtr cameraDevice, float lensPosition) => false;

            public static void UnityARKit_Camera_GetFocus(IntPtr cameraDevice, out float lensPosition)
                => throw new System.NotImplementedException(Constants.k_LoaderDisabledExceptionMsg);
#endif
        }
    }
}
