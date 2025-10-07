---
uid: arkit-meshing
---
# Meshing

ARKit provides support for the scene reconstruction feature that became available in ARKit 3.5 and is enabled on the new iPad Pro with LiDAR scanner.

ARKit scene reconstruction provides a meshing feature that generates a mesh based on scanned real-world geometry. The mesh manager enables and configures this functionality.

Using the LiDAR sensor, ARKit scene reconstruction scans the environment to create mesh geometry representing the real world environment. Additionally, ARKit provides an optional classification of each triangle in the scanned mesh. The per-triangle classification identifies the type of surface corresponding to the triangle's location in the real world.

## Requirements

This new mesh functionality requires Xcode 11.4 or later, and it only works on iOS devices with the LiDAR scanner, such as the new iPad Pro.

The meshing feature requires iOS 13.4 or newer, and iPadOS 13.4 or newer.

## Use meshing in a scene

To get started with ARKit meshing with AR Foundation, follow the instructions in AR Foundation's [Use meshing in your scene](xref:arfoundation-meshing-use).

## Optional feature support

The following table indicates which meshing features ARKit supports:

| **Feature** | **Supported** |
| :---------- | :-----------: |
| Density | |
| Normals | Yes |
| Tangents | |
| Texture coordinates |
| Colors | |
| Queue size | Yes |
| Classification | Yes |

Refer to [AR Mesh Manager](xref:arfoundation-meshing-manager) (AR Foundation) to learn more about AR Mesh Manager features.

## Sample scenes

AR Foundation provides three meshing sample scenes to demonstrate meshing features. To learn more about these meshing samples, refer to [Meshing samples](xref:arfoundation-samples-meshing).

## Meshing behaviors

**Note:** It usually takes about 4 seconds after the Made With Unity logo disappears (or when a new AR Session starts) before the scanned meshes start to show up.

Additionally, the LiDAR scanner alone might produce a slightly uneven mesh on a real-world surface. If you add and enable an [ARPlaneManager](xref:UnityEngine.XR.ARFoundation.ARPlaneManager) to your scene, ARKit considers that plane information when constructing a mesh and smooths out the mesh where it detects a plane on that surface.

## Additional information

For more information about ARKit 3.5, refer to [Introducing ARKit 3.5](https://developer.apple.com/augmented-reality/arkit/).

For more information about scene reconstruction, see [Visualizing and Interacting with a Reconstructed Scene](https://developer.apple.com/documentation/arkit/world_tracking/visualizing_and_interacting_with_a_reconstructed_scene?language=objc).

[!include[](snippets/apple-arkit-trademark.md)]
