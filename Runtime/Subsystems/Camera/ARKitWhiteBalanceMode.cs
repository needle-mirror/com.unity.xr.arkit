using System;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Defines the white balance modes for the camera device.
    /// </summary>
    [Flags]
    public enum ARKitWhiteBalanceMode
    {
        /// <summary>
        /// Update to white balance is unsupported.
        /// </summary>
        None = 0,

        /// <summary>
        /// Locks the white balance for the camera device.
        /// </summary>
        Locked = 1 << 0,

        /// <summary>
        /// Automatically adjusts the white balance one time, then locks white balance for the camera device.
        /// </summary>
        Auto = 1 << 1,

        /// <summary>
        /// Continuously monitors and adjusts the white balance for the camera device.
        /// </summary>
        ContinuousAuto = 1 << 2,
    }
}
