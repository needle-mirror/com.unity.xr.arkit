---
uid: arkit-face-tracking
---
# Face tracking

This page is a supplement to the AR Foundation [Face tracking](xref:arfoundation-face-tracking) manual. The following sections only contain information about APIs where ARKit exhibits unique platform-specific behavior.

> [!IMPORTANT]
> To use face tracking with ARKit, you must first enable face tracking in the XR Plug-in Management settings. Refer to [Enable the Face tracking subsystem](xref:arkit-project-config#enable-face-tracking) to understand how to enable face tracking for ARKit.

## Optional feature support

ARKit implements the following optional features of AR Foundation's [XRFaceSubsystem](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystem). The availability of features on specific devices depends on device hardware and software. Refer to [Requirements](#requirements) for more information.

| Feature | Descriptor Property | Supported |
| :------ | :------------------ | :-------: |
| **Face pose** | [supportsFacePose](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.supportsFacePose) | Yes |
| **Face mesh vertices and indices** | [supportsFaceMeshVerticesAndIndices](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.supportsFaceMeshVerticesAndIndices) | Yes |
| **Face mesh UVs** | [supportsFaceMeshUVs](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.supportsFaceMeshUVs) | Yes |
| **Face mesh normals** | [supportsFaceMeshNormals](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.supportsFaceMeshNormals) | |
| **Eye tracking** |  [supportsEyeTracking](xref:UnityEngine.XR.ARSubsystems.XRFaceSubsystemDescriptor.supportsEyeTracking) | Yes |

> [!NOTE]
> Refer to AR Foundation [Face tracking platform support](xref:arfoundation-face-tracking-platform-support) for more information
> on the optional features of the face subsystem.

## Session configuration

Face tracking requires the use of the user-facing or "selfie" camera. It is the responsibility of your session's [XRSessionSubsystem.configurationChooser](xref:UnityEngine.XR.ARSubsystems.XRSessionSubsystem.configurationChooser) to choose the camera facing direction. You can override the configuration chooser to meet your app's needs. For more information, refer to [Configuration Chooser](xref:arfoundation-session-configuration-chooser).

You can access a sample that shows how to use the `ConfigurationChooser` to choose between the user-facing and world-facing camera on the [AR Foundation samples](https://github.com/Unity-Technologies/arfoundation-samples/tree/main/Assets/Scenes/ConfigurationChooser) GitHub repository.

### Configuration chooser

iOS devices support different combinations of features in different camera facing directions. If your scene contains several manager components that require the world-facing camera, AR Foundation's default configuration chooser might decide to use the world-facing camera, even if the AR Face Manager component is also enabled in your scene. You can create your own [ConfigurationChooser](xref:UnityEngine.XR.ARSubsystems.ConfigurationChooser) to prioritize face tracking functionality over other features if you desire greater control over the camera's facing direction.

You can access an example of using a custom `ConfigurationChooser` in the `Rear Camera (ARKit)` sample on the [AR Foundations samples](https://github.com/Unity-Technologies/arfoundation-samples/tree/main/Assets/Scenes/FaceTracking/WorldCameraWithUserFacingFaceTracking) GitHub. This example demonstrates how you can use the user-facing camera for face tracking, and the world-facing (rear) camera for passthrough video (iOS 13+).

## Blend shapes

ARKit provides a series of [blend shapes](https://developer.apple.com/documentation/arkit/arfaceanchor/2928251-blendshapes?language=objc) to describe different features of a face. Each blend shape is modulated from 0..1. For example, one blend shape defines how open the mouth is.

A blend shape represents action at a location on a face. Each blend shape is defined by an [ARKitBlendShapeLocation](xref:UnityEngine.XR.ARKit.ARKitBlendShapeLocation) to identify the location of the face action and a [ARKitBlendShapeCoefficient](xref:UnityEngine.XR.ARKit.ARKitBlendShapeCoefficient) to describe the amount of action at the location. The `ARKitBlendShapeCoefficient` is a value between `0.0` and `1.0`.

You can learn more about blend shapes with the `Blend shapes` sample on the [AR Foundation Samples](https://github.com/Unity-Technologies/arfoundation-samples/tree/main?tab=readme-ov-file#blend-shapes-arkit) GitHub. This sample uses blend shapes to puppet a cartoon face which is displayed over the detected face.

## Face visualizer samples

The [AR Foundation Samples](https://github.com/Unity-Technologies/arfoundation-samples) GitHub repository contains ARKit-specific prefabs that you can use to visualize faces in your scene, as outlined in the following table. Refer to the AR Foundation [AR Face](xref:arfoundation-face-tracking-arface) manual for more information on how to use these prefabs.

| Prefab | Description |
| :----- | :---------- |
| [AR Eye Pose Visualizer](https://github.com/Unity-Technologies/arfoundation-samples/blob/main/Assets/Prefabs/AR%20Eye%20Pose%20Visualizer.prefab) | Visualize the location and direction of the eyes of a detected face. |
| [Eye Laser Visualizer](https://github.com/Unity-Technologies/arfoundation-samples/blob/main/Assets/Prefabs/Eye%20Laser%20Prefab.prefab) | Use the eye pose to draw laser beams emitted from the detected face. |
| [Sloth Head](https://github.com/Unity-Technologies/arfoundation-samples/blob/main/Assets/Prefabs/SlothHead.prefab) | Use the face blend shapes provided by ARKit to animate a 3D character. |

<a id="requirements"></a>

## Requirements

Face tracking supports devices with Apple Neural Engine in iOS 14 and newer, and iPadOS 14 and newer. Devices with iOS 13 and earlier, and iPadOS 13 and earlier, require a TrueDepth camera for face tracking. Refer to Apple's [Tracking and Visualizing Faces](https://developer.apple.com/documentation/arkit/content_anchors/tracking_and_visualizing_faces?language=objc) documentation for more information.

> [!NOTE]
> To check whether a device has an Apple Neural Engine or a TrueDepth camera, refer to Apple's [Tech Specs](https://support.apple.com/en_US/specs).

[!include[](snippets/apple-arkit-trademark.md)]
