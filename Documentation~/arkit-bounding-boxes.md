---
uid: arkit-bounding-boxes
---
# Bounding box detection

Apple's RoomPlan framework allows users to scan environments and detect objects through room capture functionality. It also provides the bounding box properties of captured objects during and after the room capture process. Users must complete a room capture before retrieving the bounding boxes of the detected objects. To learn more about the RoomPlan framework, refer to Apple's [RoomPlan](https://developer.apple.com/documentation/roomplan) documentation.

[!include[](snippets/arf-docs-tip.md)]

## Requirements

This feature is only available on iOS devices equipped with a LiDAR scanner running iOS 17 and newer.

> [!NOTE]
> To check whether a device has a LiDAR scanner, refer to Apple's [Tech Specs](https://support.apple.com/en_US/specs).

## Bounding box classifications

This package maps Apple's native [semantic labels](https://developer.apple.com/documentation/roomplan/capturedroom/object/category-swift.enum) to AR Foundation's [BoundingBoxClassifications](xref:UnityEngine.XR.ARFoundation.ARBoundingBox.classifications). Apple supports multiple classifications per bounding box.

Refer to the following table to understand the mapping between AR Foundation's classifications and Apple's semantic labels:

| AR Foundation Label | Apple Label  |
|:--------------------|:-------------|
| Couch               | sofa         |
| Table               | table        |
| Bed                 | bed          |
| Screen              | television   |
| Storage             | storage      |
| Bathtub             | bathtub      |
| Chair               | chair        |
| Dishwasher          | dishwasher   |
| Fireplace           | fireplace    |
| Oven                | oven         |
| Refrigerator        | refrigerator |
| Sink                | sink         |
| Stairs              | stairs       |
| Stove               | stove        |
| Toilet              | toilet       |
| WasherDryer         | washerDryer  |

## Room capture instructions

Apple provides [instructions](https://developer.apple.com/documentation/roomplan/roomcapturesession/instruction) to guide the user through the room capture process. Refer to the following table for the set of instruction enumerations:

| ARKit Label      | Apple Label      | Description                                               |
|:-----------------|:-----------------|:----------------------------------------------------------|
| Normal           | normal           | Room capturing normally and the user needs no coaching.   |
| MoveCloseToWall  | moveCloseToWall  | The user should move closer to the wall.                  |
| MoveAwayFromWall | moveAwayFromWall | The user should move further from the wall.               |
| TurnOnLight      | turnOnLight      | The user should increase the amount of light in the room. |
| SlowDown         | slowDown         | The user should move slower.                              |
| LowTexture       | lowTexture       | The process doesn't detect distinguishable features.      |

## Check room capture support

The following example method checks whether the [RoomPlanBoundingBoxSubsystem](xref:UnityEngine.XR.ARKit.RoomPlanBoundingBoxSubsystem) is available. This method is used by the other code examples on this page.

[!code-cs[CheckRoomCaptureSupportSample](../Tests/Runtime/CodeSamples/RoomPlanBoundingBoxSubsystemTests.cs#CheckRoomCaptureSupportSample)]

## Take a room capture

Use the following methods to take a room capture:

| Life cycle event                                                                                                                                               | Description                                          |
|:---------------------------------------------------------------------------------------------------------------------------------------------------------------|:-----------------------------------------------------|
| [SetupRoomCapture](xref:UnityEngine.XR.ARKit.RoomPlanBoundingBoxSubsystem.SetupRoomCapture)                                                                    | Set up room capture before the process.              |
| [StartRoomCapture](xref:UnityEngine.XR.ARKit.RoomPlanBoundingBoxSubsystem.StartRoomCapture)                                                                    | Start room capture process.                          |
| [StopRoomCapture](xref:UnityEngine.XR.ARKit.RoomPlanBoundingBoxSubsystem.StopRoomCapture)                                                                      | Stop room capture process.                           |
| [IsRoomCapturing](xref:UnityEngine.XR.ARKit.RoomPlanBoundingBoxSubsystem.IsRoomCapturing)                                                                      | Check the room capture status.                       |
| [GetRoomCaptureInstruction](xref:UnityEngine.XR.ARKit.RoomPlanBoundingBoxSubsystem.GetRoomCaptureInstruction(UnityEngine.XR.ARKit.XRBoundingBoxInstructions@)) | Retrieve the instructions for accurate room capture. |
