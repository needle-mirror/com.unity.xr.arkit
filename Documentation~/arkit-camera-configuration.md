---
uid: arkit-camera-configuration
---
# Camera configuration

[XRCameraConfiguration](xref:UnityEngine.XR.ARSubsystems.XRCameraConfiguration) contains an `IntPtr` field `nativeConfigurationHandle` which is a platform-specific handle. For ARKit, this handle is a pointer to the native [ARVideoFormat](https://developer.apple.com/documentation/arkit/arvideoformat?language=objc) Objective-C object.
