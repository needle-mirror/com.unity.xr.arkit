using System;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Defines the focus modes for the camera device.
    /// </summary>
    [Flags]
    public enum ARKitFocusMode
    {
        /// <summary>
        /// Update to focus is unsupported.
        /// </summary>
        None = 0,

        /// <summary>
        /// Locks the focus for the camera device.
        /// </summary>
        Locked = 1 << 0,

        /// <summary>
        /// Automatically adjusts the focus one time, and then locks focus for the camera device.
        /// </summary>
        Auto = 1 << 1,

        /// <summary>
        /// Continuously monitors and adjusts the focus for the camera device.
        /// </summary>
        ContinuousAuto = 1 << 2,
    }
}
