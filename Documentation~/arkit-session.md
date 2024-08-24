---
uid: arkit-session
---
# Session

ARKit implements [XRSessionSubsystem.GetAvailabilityAsync](xref:UnityEngine.XR.ARSubsystems.XRSessionSubsystem.GetAvailabilityAsync), which consists of the device checking that it's running on iOS 11.0 or above. For more information, see [Session](xref:arfoundation-session).

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
// Marhshal the native session data from the XRSessionSubsystem.nativePtr in C#
UnityXRNativeSession nativeSessionData;
ArSession* session = static_cast<ArSession*>(nativeSessionData.sessionPtr);
```

To learn more about native pointers and their usage, refer to [Extending AR Foundation](xref:arfoundation-extensions).
