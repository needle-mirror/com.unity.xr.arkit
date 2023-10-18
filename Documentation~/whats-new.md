---
uid: arkit-whats-new
---
# What's new in version 6.0

The most significant updates in this release include:

## Added

- Added support for `XRCameraSubsystem.GetShaderKeywords` to `ARKitCameraSubsystem` and `ARKitOcclusionSubsystem`.

## Deprecated

- Several subsystems have been deprecated and renamed for consistency with other subsystems. Unity's API Updater should automatically convert any deprecated APIs references to the new APIs when the project is loaded into the Editor again.
  - `ARKitXRObjectTrackingSubsystem` has been renamed to `ARKitObjectTrackingSubsystem`.
  - `ARKitXRPlaneSubsystem` has been renamed to `ARKitPlaneSubsystem`.
  - `ARKitXRPointCloudSubsystem` has been renamed to `ARKitPointCloudSubsystem`.

## Removed

The following table contains a list of APIs that have been removed from Apple ARKit XR Plug-in 6.0.
If your code uses any of these APIs, you must upgrade to use the recommended replacement instead.


| Obsolete API                                  | Recommendation                                                                        |
|:----------------------------------------------|:--------------------------------------------------------------------------------------|
| `ARKitSettingsProvider`                       | This class is now deprecated. Its internal functionality is replaced by XR Management |
| `NSError.IsNull`                              | Compare with null instead, e.g., error == null.                                       |
| `ARKitErrorCode.CollaborationDataUnavailable` | Use InvalidCollaborationData instead.                                                 |
| `ARKitSessionSubsystem.coachingGoal`          | Use requestedCoachingGoal or currentCoachingGoal instead.                             |

[!include[](snippets/apple-arkit-trademark.md)]

- Removed the `Description` attribute from `ARMeshClassification` enum as the attribute was unused.
