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
                Debug.LogError("High resolution CPU image capture requires ARKit and is not supported on this device.");
                yield break;
            }

            // Yield return on the promise returned by the ARKitCameraSubsystem
            var promise = subsystem.TryAcquireHighResolutionCpuImage();
            yield return promise;

            // If the promise was not successful, check your Console logs for more information about the error.
            if (!promise.result.wasSuccessful)
                yield break;

            // If the promise was successful, handle the result.
            DoSomethingWithHighResolutionCpuImage(promise.result.highResolutionCpuImage);
            promise.result.highResolutionCpuImage.Dispose();
        }
        #endregion

        static void DoSomethingWithHighResolutionCpuImage(XRCpuImage cpuImage)
        {
            // Intentionally left blank as a sample
        }
    }
}
