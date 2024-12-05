using System.Collections;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit.CodeSamples.Tests
{
    /// <summary>
    /// Use this class to write sample code for <see cref="ARKitCameraSubsystem"/> to be rendered to the documentation manual.
    /// </summary>
    public class ARKitCameraSubsystemTests
    {
        ARCameraManager m_CameraManager;

        #region HighResolutionCpuImageSample
        IEnumerator CaptureHighResolutionCpuImage()
        {
            if (m_CameraManager.subsystem is not ARKitCameraSubsystem subsystem)
            {
                Debug.LogError("High resolution CPU image capture requires ARKit.");
                yield break;
            }

            // Yield return on the promise returned by the ARKitCameraSubsystem
            var promise = subsystem.TryAcquireHighResolutionCpuImage();
            yield return promise;

            // If the promise was not successful, check your Console logs for more
            // information about the error.
            if (!promise.result.wasSuccessful)
                yield break;

            // If the promise was successful, handle the result.
            DoSomethingWithHighResolutionCpuImage(promise.result.highResolutionCpuImage);
            promise.result.highResolutionCpuImage.Dispose();
        }
        #endregion

        #region AdvancedConfigurationSupport
        bool AdvancedConfigurationSupported(out ARKitCameraSubsystem subsystem)
        {
            // check if arkit subsystem is available
            subsystem = m_CameraManager.subsystem as ARKitCameraSubsystem;
            if (subsystem == null)
            {
                Debug.LogError("Advanced camera configuration requires ARKit.");
                return false;
            }

            // check whether the device supports advanced camera configuration
            if (!subsystem.advancedCameraConfigurationSupported)
            {
                Debug.LogError("Advanced camera configuration is not supported on this device.");
                return false;
            }

            return true;
        }
        #endregion AdvancedConfigurationSupport

        #region CameraExposure
        void UpdateCameraExposure()
        {
            if (!AdvancedConfigurationSupported(out ARKitCameraSubsystem subsystem))
                return;

            // try to get a locked camera
            if (!subsystem.TryGetLockedCamera(out var lockedCamera))
            {
                Debug.LogError("Unable to lock the camera for advanced camera configuration.");
                return;
            }

            // using statement will automatically dispose the locked camera
            using (lockedCamera)
            {
                // set the exposure
                const double duration = 0.1f;
                const float iso = 500f;

                lockedCamera.exposure = new ARKitExposure(duration, iso);
            }
        }
        #endregion CameraExposure

        #region CameraWhiteBalance
        void UpdateCameraWhiteBalance()
        {
            if (!AdvancedConfigurationSupported(out ARKitCameraSubsystem subsystem))
                return;

            // try to get a locked camera
            if (!subsystem.TryGetLockedCamera(out var lockedCamera))
            {
                Debug.LogError("Unable to lock the camera for advanced camera configuration.");
                return;
            }

            // using statement will automatically dispose the locked camera
            using (lockedCamera)
            {
                // set the white balance
                const float blueGain = 2.0f;
                const float greenGain = 1.0f;
                const float redGain = 1.5f;

                lockedCamera.whiteBalance = new ARKitWhiteBalanceGains(blueGain, greenGain, redGain);
            }
        }
        #endregion CameraWhiteBalance

        #region CameraFocus
        void UpdateCameraFocus()
        {
            if (!AdvancedConfigurationSupported(out ARKitCameraSubsystem subsystem))
                return;

            // try to get a locked camera
            if (!subsystem.TryGetLockedCamera(out var lockedCamera))
            {
                Debug.LogError("Unable to lock the camera for advanced camera configuration.");
                return;
            }

            // using statement will automatically dispose the locked camera
            using (lockedCamera)
            {
                // set the focus
                const float lensPosition = 2.0f;

                lockedCamera.focus = new ARKitFocus(lensPosition);
            }
        }
        #endregion CameraFocus

        static void DoSomethingWithHighResolutionCpuImage(XRCpuImage cpuImage)
        {
            // Intentionally left blank as a sample
        }
    }
}
