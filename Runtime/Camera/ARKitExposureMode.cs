namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Defines the exposure modes for the camera device.
    /// </summary>
    public enum ARKitExposureMode
    {
        /// <summary>
        /// Allow the platform provider to choose a mode.
        /// </summary>
        None = 0,

        /// <summary>
        /// Locks the exposure for the camera device.
        /// </summary>
        Locked = 1,

        /// <summary>
        /// Automatically adjusts the exposure one time, and then locks exposure for the camera device.
        /// </summary>
        Auto = 2,

        /// <summary>
        /// Continuously monitors and adjusts the exposure for the camera device.
        /// </summary>
        ContinuousAuto = 3,

        /// <summary>
        /// Allow manual adjustment of the exposure for the camera device.
        /// </summary>
        Custom = 4
    }
}
