---
uid: arkit-anchors
---
# Anchors

This page is a supplement to the AR Foundation [Anchors](xref:arfoundation-anchors) manual. The following sections describe the optional features of AR Foundation's [XRAnchorSubsystem](xref:UnityEngine.XR.ARSubsystems.XRAnchorSubsystem) supported by ARKit.

[!include[](snippets/arf-docs-tip.md)]

## Optional feature support

ARKit implements the following optional features of AR Foundation's [XRAnchorSubsystem](xref:UnityEngine.XR.ARSubsystems.XRAnchorSubsystem):

| Feature | Descriptor Property | Supported |
| :------ | :------------------ | :-------: |
| **Trackable attachments** | [supportsTrackableAttachments](xref:UnityEngine.XR.ARSubsystems.XRAnchorSubsystemDescriptor.supportsTrackableAttachments) | Yes |
| **Synchronous add** | [supportsSynchronousAdd](xref:UnityEngine.XR.ARSubsystems.XRAnchorSubsystemDescriptor.supportsSynchronousAdd) | Yes |
| **Save anchor** | [supportsSaveAnchor](xref:UnityEngine.XR.ARSubsystems.XRAnchorSubsystemDescriptor.supportsSaveAnchor) |  |
| **Load anchor** | [supportsLoadAnchor](xref:UnityEngine.XR.ARSubsystems.XRAnchorSubsystemDescriptor.supportsLoadAnchor) |  |
| **Erase anchor** | [supportsEraseAnchor](xref:UnityEngine.XR.ARSubsystems.XRAnchorSubsystemDescriptor.supportsEraseAnchor) |  |
| **Get saved anchor IDs** | [supportsGetSavedAnchorIds](xref:UnityEngine.XR.ARSubsystems.XRAnchorSubsystemDescriptor.supportsGetSavedAnchorIds) |  |
| **Async cancellation** | [supportsAsyncCancellation](xref:UnityEngine.XR.ARSubsystems.XRAnchorSubsystemDescriptor.supportsAsyncCancellation) |  |

> [!NOTE]
> Refer to AR Foundation [Anchors platform support](xref:arfoundation-anchors-platform-support) for more information on the optional features of the anchor subsystem.
