---
uid: arkit-whats-new
---
# What's new in version 5.1

## ARKit 6

ARKit 6 is the latest version of ARKit, and is included with iOS 16 on all supported devices. See Apple's [ARKit 6 documentation](https://developer.apple.com/augmented-reality/arkit/) for an introduction to the changes and new features.

### Xcode 14

As of version 5.1.0-pre.1, you are now required to build iOS apps using this plug-in with Xcode 14. See Apple's [Xcode 14 documentation](https://developer.apple.com/xcode/) for download and install instructions.

### 4K video

You are able to access the new 4K camera configurations introduced by ARKit 6 using pre-existing AR Foundation API's.

To access 4K video:

* Install Xcode 14
* Install iOS 16 on a compatible device (iPhone 11 and up or iPad Pro 5th Generation and up)
* Set up a scene to request a 4K camera configuration
* Build your Unity project as normal for iOS
* Open the resulting Xcode project in Xcode 14, then build and run your app on your iOS 16 device

> [!NOTE]
> 4K video is not available on devices running iOS 15 or below.

#### Request a 4K camera configuration

The following sample code demonstrates how to request a 4K camera configuration from the [ARCameraManager](xref:UnityEngine.XR.ARFoundation.ARCameraManager):


```csharp
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
 
[RequireComponent(typeof(ARCameraManager))]
public class CameraConfigurator : MonoBehaviour
{
    IEnumerator Start()
    {
        // Wait for configurations to become available
        yield return new WaitForEndOfFrame();
 
        // Get available camera configurations
        var cameraManager = GetComponent<ARCameraManager>();
        using var configs = cameraManager.GetConfigurations(Allocator.Temp);
 
        // Switch to the first available 4k camera configuration
        var fourKResolution = new Vector2(3840, 2160);
        foreach (var c in configs)
        {
            if (c.resolution == fourKResolution)
            {
                Debug.Log($"Switching to 4k camera configuration:\n{c}");
                cameraManager.currentConfiguration = c;
                break;
            }
        }
    }
}
```

### High resolution CPU image

You are able to asynchronously request a [High resolution CPU image](xref:arkit-camera#high-resolution-cpu-image) capture on iOS 16 or newer. This API natively calls the [captureHighResolutionFrameWithCompletion](https://developer.apple.com/documentation/arkit/arsession/3975720-capturehighresolutionframewithco) API introduced in ARKit 6.

### EXIF data

You are able to access camera frame's [EXIF data](xref:arkit-camera#exif-data) on devices running iOS 16 or newer. This API natively accesses ARKit's [exifData](https://developer.apple.com/documentation/arkit/arframe/3930051-exifdata) and returns a [blittable](https://learn.microsoft.com/en-us/dotnet/framework/interop/blittable-and-non-blittable-types) subset of available data.

### New 2D skeleton joints

You are able to access the new ear joints introduced to the 2D skeleton using the pre-existing AR Foundation APIs. See the [HumanBodyTracking2D](https://github.com/Unity-Technologies/arfoundation-samples#humanbodytracking2d) sample scene for example code and scene setup.

### ARPlaneExtent

ARKit 6 introduces a new class called [ARPlaneExtent](https://developer.apple.com/documentation/arkit/arplaneextent?language=objc) that changes the way ARKit natively handles plane rotations on iOS 16 and up. To preserve backwards compatibility and a consistent experience across all devices, the Apple ARKit XR Plug-in now applies the new [rotationOnYAxis](https://developer.apple.com/documentation/arkit/arplaneextent/3950861-rotationonyaxis?language=objc) on your behalf. The end result for you is that no action is necessary, and nothing has changed.

For a more stable way to anchor AR content to a plane, you can add your own anchor to the plane's surface after the plane is initially detected.

## Advanced Camera Hardware Configuration

ARKit 6 allows access to the underlying configurable primary camera in some configurations when running iOS 16 or newer on a device with ultra-wide camera. If available, you can get a lock on this device to configure the hardware properties of the camera, such as exposure, white balance, etc. Currently, the following properties are supported for configuration:
  - Exposure mode, duration, and ISO
  - White balance mode and gains
  - Focus mode and lens position

[!include[](snippets/apple-arkit-trademark.md)]
