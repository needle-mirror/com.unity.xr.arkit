---
uid: arkit-camera
---
# Camera

This page is a supplement to the AR Foundation [Camera](xref:arfoundation-camera) manual. The following sections only contain information about APIs where ARKit exhibits unique platform-specific behavior.

[!include[](snippets/arf-docs-tip.md)]

## Optional feature support

ARKit implements the following optional features of AR Foundation's [XRCameraSubsystem](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystem):

| Feature | Descriptor Property | Supported |
| :------ | :--------------- | :---------: |
| **Brightness** | [supportsAverageBrightness](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsAverageBrightness) | |
| **Color temperature** | [supportsAverageColorTemperature](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsAverageColorTemperature) | Yes |
| **Color correction** | [supportsColorCorrection](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsColorCorrection) | |
| **Display matrix** | [supportsDisplayMatrix](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsDisplayMatrix) | Yes |
| **Projection matrix** | [supportsProjectionMatrix](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsProjectionMatrix) | Yes |
| **Timestamp** | [supportsTimestamp](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsTimestamp) | Yes |
| **Camera configuration** | [supportsCameraConfigurations](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsCameraConfigurations) | Yes |
| **Camera image** | [supportsCameraImage](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsCameraImage) | Yes |
| **Average intensity in lumens** | [supportsAverageIntensityInLumens](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsAverageIntensityInLumens) | Yes |
| **Focus modes** | [supportsFocusModes](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsFocusModes) | Yes |
| **Face tracking ambient intensity light estimation** | [supportsFaceTrackingAmbientIntensityLightEstimation](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsFaceTrackingAmbientIntensityLightEstimation) | Yes |
| **Face tracking HDR light estimation** | [supportsFaceTrackingHDRLightEstimation](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsFaceTrackingHDRLightEstimation) | Yes |
| **World tracking ambient intensity light estimation** | [supportsWorldTrackingAmbientIntensityLightEstimation](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsWorldTrackingAmbientIntensityLightEstimation) | Yes |
| **World tracking HDR light estimation** | [supportsWorldTrackingHDRLightEstimation](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsWorldTrackingHDRLightEstimation) | |
| **Camera grain** | [supportsCameraGrain](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsCameraGrain) | iOS 13+ |
| **Image stabilization** | [supportsImageStabilization](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsImageStabilization) | |
| **Exif data** | [supportsExifData](xref:UnityEngine.XR.ARSubsystems.XRCameraSubsystemDescriptor.supportsExifData) | iOS 16+ |

> [!NOTE]
> Refer to AR Foundation [Camera platform support](xref:arfoundation-camera-platform-support) for more information 
> on the optional features of the camera subsystem.

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

### Check Support
The following example method checks whether the ARKitCameraSubsystem is available and whether it supports the advanced camera configuration feature. This method is used by the other code examples on this page.

[!code-cs[CameraExposure](../Tests/CodeSamples/ARKitCameraSubsystemTests.cs#AdvancedConfigurationSupport)]

### Exposure
The following example method tries to lock the camera and, if successful, sets the exposure.
[!code-cs[CameraExposure](../Tests/CodeSamples/ARKitCameraSubsystemTests.cs#CameraExposure)]

### White Balance
The following example method tries to lock the camera and, if successful, sets the white balance.

[!code-cs[CameraWhiteBalance](../Tests/CodeSamples/ARKitCameraSubsystemTests.cs#CameraWhiteBalance)]

### Focus
The following example method tries to lock the camera and, if successful, sets the focus.
[!code-cs[CameraFocus](../Tests/CodeSamples/ARKitCameraSubsystemTests.cs#CameraFocus)]


## High resolution CPU image

You can asynchronously capture a high resolution [XRCpuImage](xref:UnityEngine.XR.ARSubsystems.XRCpuImage) (or simply, CPU Image) using [ARKitCameraSubsystem.TryAcquireHighResolutionCpuImage](xref:UnityEngine.XR.ARKit.ARKitCameraSubsystem.TryAcquireHighResolutionCpuImage) on iOS 16 and newer.

The example below demonstrates a coroutine to set up and handle the asynchronous request:

[!code-cs[HighResolutionCpuImageSample](../Tests/CodeSamples/ARKitCameraSubsystemTests.cs#HighResolutionCpuImageSample)]

Whenever you successfully acquire a high resolution CPU image, you should [Dispose](xref:UnityEngine.XR.ARSubsystems.XRCpuImage.Dispose) it as soon as possible, as CPU images require native memory resources. If you retain too many high-resolution images, ARKit can be prevented from rendering new frames.

For a complete usage example, see the [AR Foundation Samples](https://github.com/Unity-Technologies/arfoundation-samples/tree/main/Assets/Scenes/ARKit/HighResolutionCpuImage/HighResolutionCpuImageSample.cs) repository.

### Select a resolution

The exact resolution of the high resolution CPU image you receive depends on your camera manager's [currentConfiguration](xref:UnityEngine.XR.ARFoundation.ARCameraManager.currentConfiguration). For the highest resolution capture, choose a configuration with a non-binned video format such as 4K resolution (3840x2160).

For more information on binned vs non-binned video formats, see Apple's [Discover ARKit 6](https://developer.apple.com/videos/play/wwdc2022/10126/) video, which explains the ARKit camera architecture in greater detail.

## EXIF data

You are able to access camera frame's [EXIF data](xref:arfoundation-exif-data) on devices running iOS 16 or newer.

For more information, refer to the [EXIF specification](https://web.archive.org/web/20190624045241if_/http://www.cipa.jp:80/std/documents/e/DC-008-Translation-2019-E.pdf).

[!include[](snippets/apple-arkit-trademark.md)]
