---
uid: arkit-camera
---
# Camera

## Light estimation

ARKit light estimation can only be `enabled` or `disabled`. The availability of either  `Ambient Intensity` or `Environmental HDR` data is governed by the active tracking mode. See the following table for more details.

| Tracking configuration | Ambient intensity (lumens) | Color temperature | Main light direction | Main light intensity (lumens) | Ambient spherical harmonics |
|------------------------|----------------------------|-------------------|----------------------|-------------------------------|-----------------------------|
| World Tracking         | Yes                        | Yes               | No                   | No                            | No                          |
| Face Tracking          | Yes                        | Yes               | Yes                  | Yes                           | Yes                         |

## Camera configuration

[XRCameraConfiguration](xref:UnityEngine.XR.ARSubsystems.XRCameraConfiguration) contains an `IntPtr` field `nativeConfigurationHandle` which is a platform-specific handle. For ARKit, this handle is a pointer to the native [ARVideoFormat](https://developer.apple.com/documentation/arkit/arvideoformat?language=objc) Objective-C object.

## Advanced camera hardware configuration

On supported devices with iOS 16 or newer, you can manually configure advanced camera hardware properties such as exposure. This is useful in situations where you want more control over the camera.

> [!NOTE]
> In addition to iOS 16 or newer, advanced camera hardware configuration also requires a device with an ultra-wide camera. Most iOS devices starting with iPhone 11 have an ultra-wide camera. For more device-specific information you can check Apple's [Tech Specs](https://support.apple.com/en_US/specs).

To configure the camera's advanced hardware properties, you must first lock the camera for configuration using [ARKitCameraSubsystem.TryGetLockedCamera](xref:UnityEngine.XR.ARKit.ARKitCameraSubsystem.TryGetLockedCamera(UnityEngine.XR.ARKit.ARKitLockedCamera@)). If this method returns `true`, you can configure the device's advanced camera hardware properties using the [ARKitLockedCamera](xref:UnityEngine.XR.ARKit.ARKitLockedCamera) instance passed as its out argument.

The following code samples demonstrate how to configure advanced camera hardware properties:

### Exposure

```C#
var cameraManager = GetComponent<ARCameraManager>();
var subsystem = cameraManager.subsystem as ARKitCameraSubsystem;

if (!subsystem.TryGetLockedCamera(out var lockedCamera))
    return;

using (lockedCamera)
{
    var duration = 0.1f;
    var iso = 500f;
    lockedCamera.exposure = new ARKitExposure(duration, iso);
}
```

## High resolution CPU image

You can asynchronously capture a high resolution [XRCpuImage](xref:UnityEngine.XR.ARSubsystems.XRCpuImage) (or simply, CPU Image) using [ARKitCameraSubsystem.TryAcquireHighResolutionCpuImage](xref:UnityEngine.XR.ARKit.ARKitCameraSubsystem.TryAcquireHighResolutionCpuImage) on iOS 16 and newer.

The example below demonstrates a coroutine to set up and handle the asynchronous request:

[!code-cs[HighResolutionCpuImageSample](../Tests/CodeSamples/ARKitCameraSubsystemTests.cs#HighResolutionCpuImageSample)]

Whenever you successfully acquire a high resolution CPU image, you should [Dispose](xref:UnityEngine.XR.ARSubsystems.XRCpuImage.Dispose) it as soon as possible, as CPU images require native memory resources. If you retain too many high-resolution images, ARKit can be prevented from rendering new frames.

For a complete usage example, see the [AR Foundation Samples](https://github.com/Unity-Technologies/arfoundation-samples/tree/main/Assets/Scenes/ARKit/HighResolutionCpuImage/HighResolutionCpuImageSample.cs) repository.

### Select a resolution

The exact resolution of the high resolution CPU image you receive depends on your camera manager's [currentConfiguration](xref:UnityEngine.XR.ARFoundation.ARCameraManager.currentConfiguration). For the highest resolution capture, choose a configuration with a non-binned video format such as 4K resolution (3840x2160).

For more information on binned vs non-binned video formats, see Apple's [Discover ARKit 6](https://developer.apple.com/videos/play/wwdc2022/10126/) video, which explains the ARKit camera architecture in greater detail.

[!include[](snippets/apple-arkit-trademark.md)]
