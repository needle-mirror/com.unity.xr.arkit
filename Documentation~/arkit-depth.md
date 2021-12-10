---
uid: arkit-depth
---
# Depth subsystem

Raycasts always return a `Pose` for the item the raycast hit. When raycasting against feature points, the pose is oriented to provide an estimate for the surface the feature point might represent.

The depth subsystem doesn't require additional resources, so enabling it doesn't affect performance

ARKit's depth subsystem only ever produces a single [XRPointCloud](xref:UnityEngine.XR.ARSubsystems.XRPointCloud).

For more information, see the [ARSubsystems depth subsystem documentation](xref:arsubsystems-depth-subsystem).
