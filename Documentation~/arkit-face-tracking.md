---
uid: arkitfacetracking-manual
---
# Face Tracking

This package implements the [face tracking subsystem](xref:arsubsystems-face-subsystem) defined in the [AR Foundation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest/index.html) package specific to ARKit.

ARKit provides a series of [blend shapes](https://developer.apple.com/documentation/arkit/arfaceanchor/2928251-blendshapes?language=objc) to describe different features of a face. Each blend shape is modulated from 0..1. For example, one blend shape defines how open the mouth is.

## Front facing camera

Face tracking requires the use of the front-facing or "selfie" camera. When the front-facing camera is active, other tracking subsystems like plane tracking or image tracking may not be available. If the rear-facing camera is active, face tracking might not be available.

Different iOS devices support different combinations of features. If you `Start` a subsystem that requires the rear-facing camera, the Apple ARKit package might decide to use the rear-facing camera instead. For more information, see [Camera and Tracking Mode Selection](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/manual/migration-guide-3.html#camera-and-tracking-mode-selection).

## Technical details

### Requirements

To use face tracking, you must have:

- An iOS device capable of performing face tracking. Such devices require either a front-facing TrueDepth camera or an A12 Bionic chip (or later). Devices include:
  - iPhone X
  - iPhone XS
  - iPhone XS Max
  - iPhone XR
  - iPhone 11
  - iPhone 12
  - iPad Pro (11-inch)
  - iPad Pro (12.9-inch, 3rd generation)
  - iPhone SE
- iOS 11.0 or later
- Xcode 11.0 or later

### Contents

**Apple ARKit XR Plug-in** includes a static library that provides an implementation of the [XRFaceSubsystem](xref:arsubsystems-face-subsystem).

[!include[](snippets/apple-arkit-trademark.md)]
