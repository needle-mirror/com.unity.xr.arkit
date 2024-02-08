---
uid: arkit-whats-new
---
# What's new in version 6.0

This release includes the following significant changes:

## Added

- Added support for the `XRCameraSubsystem.GetShaderKeywords` API to `ARKitCameraSubsystem` and `ARKitOcclusionSubsystem`.

## Changed

- Upgraded minimum Unity version from 2021.2 to 2023.3.

## Deprecated

- The following subsystems were deprecated and renamed for consistency with other subsystems. Unity's API Updater should automatically convert any deprecated APIs references to the new APIs when the project is loaded into the Editor again.
  - `ARKitXRObjectTrackingSubsystem` renamed to `ARKitObjectTrackingSubsystem`.
  - `ARKitXRPlaneSubsystem` renamed to `ARKitPlaneSubsystem`.
  - `ARKitXRPointCloudSubsystem` renamed to `ARKitPointCloudSubsystem`.

## Removed

- The following previously deprecated APIs were removed. If your code uses any of these APIs, you must upgrade to the recommended replacement instead.

| Obsolete API | Recommendation |
| :----------- | :------------- |
| `ARKitSettingsProvider` | This class is now deprecated. Its internal functionality is replaced by XR Management |
| `NSError.IsNull` | Compare with null instead, e.g., `error == null`. |
| `ARKitErrorCode.CollaborationDataUnavailable` | Use `InvalidCollaborationData` instead. |
| `ARKitSessionSubsystem.coachingGoal` | Use `requestedCoachingGoal` or `currentCoachingGoal` instead. |

- Removed the `Description` attribute from `ARMeshClassification` enum as the attribute was unused.

For a full list of changes in this version including backwards-compatible bugfixes, refer to the package [changelog](xref:arkit-changelog).

[!include[](snippets/apple-arkit-trademark.md)]
