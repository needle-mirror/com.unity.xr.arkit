using System;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Defines the exposure modes for the camera device.
    /// </summary>
    [Flags]
    public enum ARKitExposureMode
    {
        /// <summary>
        /// Update to exposure is unsupported.
        /// </summary>
        None = 0,

        /// <summary>
        /// Locks the exposure for the camera device.
        /// </summary>
        Locked = 1 << 0,

        /// <summary>
        /// Automatically adjusts the exposure one time, and then locks exposure for the camera device.
        /// </summary>
        Auto = 1 << 1,

        /// <summary>
        /// Continuously monitors and adjusts the exposure for the camera device.
        /// </summary>
        ContinuousAuto = 1 << 2,

        /// <summary>
        /// Allow manual adjustment of the exposure for the camera device.
        /// </summary>
        Custom = 1 << 3
    }
}
