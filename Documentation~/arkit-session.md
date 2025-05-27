---
uid: arkit-session
---
# Session

This page is a supplement to the AR Foundation [Session](xref:arfoundation-session) manual. The following sections only contain information about APIs where ARKit exhibits unique platform-specific behavior.

[!include[](snippets/arf-docs-tip.md)]

## Optional feature support

ARKit implements the following optional features of AR Foundation's [XRSessionSubsystem](xref:UnityEngine.XR.ARSubsystems.XRSessionSubsystem):

| Feature | Descriptor Property | Supported |
| :------ | :--------------- | :--------: |
| **Install** | [supportsInstall](xref:UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor.supportsInstall) | |
| **Match frame rate** | [supportsMatchFrameRate](xref:UnityEngine.XR.ARSubsystems.XRSessionSubsystemDescriptor.supportsMatchFrameRate) | Yes |

> [!NOTE]
> Refer to AR Foundation [Session platform support](xref:arfoundation-session-platform-support) for more information on the optional features of the session subsystem.

## Check if AR is supported

ARKit implements [XRSessionSubsystem.GetAvailabilityAsync](xref:UnityEngine.XR.ARSubsystems.XRSessionSubsystem.GetAvailabilityAsync), which consists of the device checking that it's running on iOS 11.0 or newer.

## Native pointer

[XRSessionSubsystem.nativePtr](xref:UnityEngine.XR.ARSubsystems.XRSessionSubsystem.nativePtr) values returned by this package contain a pointer to the following struct:

```c
typedef struct UnityXRNativeSession
{
    int version;
    void* sessionPtr;
} UnityXRNativeSession;
```

This package also provides a header file containing the definitions of various native data structs including `UnityXRNativeSession`. It can be found in the package directory under `Includes~/UnityXRNativePtrs.h`.

Cast `void* sessionPtr` to an [ArSession](https://developer.apple.com/documentation/arkit/arsession) handle in Objective C using the following example code:

```cpp
// Marshal the native session data from the XRSessionSubsystem.nativePtr in C#
UnityXRNativeSession nativeSessionData;
ArSession* session = static_cast<ArSession*>(nativeSessionData.sessionPtr);
```

To learn more about native pointers and their usage, refer to [Extending AR Foundation](xref:arfoundation-extensions).

[!include[](snippets/apple-arkit-trademark.md)]
