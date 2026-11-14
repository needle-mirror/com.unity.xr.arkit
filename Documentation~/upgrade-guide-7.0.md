---
uid: arkit-upgrade-guide-7-0
---
# Upgrade to Apple ARKit plug-in 7.0

Upgrade your project to Apple ARKit plug-in 7.0.

Apple ARKit plug-in 7.0 contains breaking changes which require action to successfully upgrade your project when you [upgrade the package](xref:um-upm-ui-update) from previous versions.

The following sections outline the changes you need to make to your project to use Apple ARKit plug-in 7.0.

## Depth subsystem is replaced by the point cloud subsystem

Apple ARKit plug-in 7.0 removes `ARKitXRDepthSubsystem`, `ARKitXRPointCloudSubsystem`, and `ARKitLoader.depthSubsystem`. These APIs were deprecated in earlier versions. If your project references them, it won't compile.

To upgrade, replace `ARKitXRDepthSubsystem` and `ARKitXRPointCloudSubsystem` with [`ARKitPointCloudSubsystem`](xref:UnityEngine.XR.ARKit.ARKitPointCloudSubsystem), and replace `ARKitLoader.depthSubsystem` with [`ARKitLoader.pointCloudSubsystem`](xref:UnityEngine.XR.ARKit.ARKitLoader.pointCloudSubsystem).

## Plane and object tracking subsystem classes are removed

Apple ARKit plug-in 7.0 removes `ARKitXRPlaneSubsystem` and `ARKitXRObjectTrackingSubsystem`. These classes were deprecated in version 6.0. If your project references them, it won't compile.

To upgrade, replace `ARKitXRPlaneSubsystem` with [`ARKitPlaneSubsystem`](xref:UnityEngine.XR.ARKit.ARKitPlaneSubsystem), and replace `ARKitXRObjectTrackingSubsystem` with [`ARKitObjectTrackingSubsystem`](xref:UnityEngine.XR.ARKit.ARKitObjectTrackingSubsystem).

## Deprecated camera and meshing APIs are obsolete

The APIs in the following table were deprecated in earlier versions and are obsolete in 7.0, so they produce compile errors instead of warnings. To upgrade, replace each API with its replacement in the table.

| Previous API | Replacement |
| :-- | :-- |
| [`ARKitCameraSubsystem.backgroundShaderName`](xref:UnityEngine.XR.ARKit.ARKitCameraSubsystem.backgroundShaderName) | [`ARKitCameraSubsystem.backgroundShaderNames`](xref:UnityEngine.XR.ARKit.ARKitCameraSubsystem.backgroundShaderNames) |
| [`ARKitMeshSubsystemExtensions`](xref:UnityEngine.XR.ARKit.ARKitMeshSubsystemExtensions) | [`XRMeshSubsystem`](xref:UnityEngine.XR.XRMeshSubsystem) |
| [`ARMeshClassification`](xref:UnityEngine.XR.ARKit.ARMeshClassification) | [`XRMeshClassification`](xref:UnityEngine.XR.ARSubsystems.XRMeshClassification) |

## Additional resources

- [What's new in version 7.0](xref:arkit-whats-new)
- [Changelog](xref:arkit-changelog)

[!include[](snippets/apple-arkit-trademark.md)]
