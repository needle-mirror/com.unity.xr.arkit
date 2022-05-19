---
uid: arkit-manual
---
# About Apple ARKit XR Plug-in

Use the Apple ARKit XR Plug-in package to enable ARKit support via Unity's multi-platform XR API. This package implements the following XR Subsystems:

* [Session](xref:arsubsystems-session-subsystem)
* [Camera](xref:arsubsystems-camera-subsystem)
* [Point cloud](xref:arsubsystems-point-cloud-subsystem)
* [Input](xref:UnityEngine.XR.XRInputSubsystem)
* [Planes](xref:arsubsystems-plane-subsystem)
* [Raycast](xref:arsubsystems-raycast-subsystem)
* [Anchors](xref:arsubsystems-anchor-subsystem)
* [Image tracking](xref:arsubsystems-image-tracking-subsystem)
* [Environment probes](xref:arsubsystems-environment-probe-subsystem)
* [Body tracking](xref:UnityEngine.XR.ARSubsystems.XRHumanBodySubsystem)
* [Occlusion](xref:arsubsystems-occlusion-subsystem)
* [Participant](xref:arsubsystems-participant-subsystem)
* [Meshes](xref:arsubsystems-mesh-subsystem)
* [Face tracking](xref:arsubsystems-face-subsystem)

This version of Apple ARKit XR Plug-in supports the following features:

* Device localization
* Horizontal plane detection
* Vertical plane detection
* Point clouds
* Pass-through camera view
* Light estimation
* Anchors
* Hit testing
* Session management
* Image tracking
* Object tracking
* Environment probes
* Participant tracking
* Meshes (also known as Scene Reconstruction)
* Occlusion

> [!IMPORTANT]
> Apple's App Store rejects any app that contains certain face tracking-related symbols in its binary if the app developer doesn't intend to use face tracking. To avoid ambiguity, [face tracking](xref:arsubsystems-face-subsystem) support is available only when face tracking is enabled. See [Enable the Face tracking subsytem](xref:arkit-project-config#enable-face-tracking) for instructions for changing this setting. 

# Installing Apple ARKit XR Plug-in

When you enable the ARKit plug-in in the **XR Plug-in Management** settings, Unity automatically installs the ARKit package (if necessary). See [Enable the ARKit plug-in](xref:arkit-project-config#enable-the-apple-arkit-plug-in) for instructions.

In addition, you can install the AR Foundation package, which uses Apple ARKit XR Plug-in and provides many useful scripts and Prefabs. For more information about this package, see the [AR Foundation documentation](https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@latest).

> [!TIP]
> You can also install and uninstall this package using the Unity Package Manager. Installing through the Package Manager does not automatically enable the plug-in. You must still enable it in the **XR Plug-in Management settings**. See [Installing from a registry](xref:upm-ui-install) for more information about installing packages with the Unity Package Manager.


# Project configuration

See [Project configuration](xref:arkit-project-config) for information about the project settings that affect ARKit applications. 


# Using Apple ARKit XR Plug-in

The Apple ARKit XR Plug-in implements the native iOS endpoints required for building Handheld AR apps using Unity's multi-platform XR API. However, this package doesn't expose any public scripting interface of its own. In most cases, you should use the scripts, Prefabs, and assets provided by AR Foundation as the basis for your Handheld AR apps.

Including the Apple ARKit XR Plug-in also includes source files, static libraries, shader files, and plug-in metadata.

ARKit requires iOS 11.0. Some specific features require later versions (see below).


## Requiring AR

You can flag ARKit as either required or optional. By default, ARKit is required, which means your app can only be installed on AR-supported devices and operating systems (iOS 11.0 and above). If you specify that AR is optional, your app can also be installed on iOS devices that don't support ARKit.

See [Set the ARKit support Requirement](xref:arkit-project-config#arkit-required) for instructions on how to change this setting.


## Project Validation

Apple ARKit XR Plug-in package supports project validation. Project validation is a set of rules that the Unity Editor checks to detect possible problems with your project's configuration. See [Project Validation](xref:arkit-project-config#project-validation) section for more information about the rules checked for Apple ARKit XR Plug-in.

# Technical details

## Requirements

This version of Apple ARKit XR Plug-in is compatible with the following versions of the Unity Editor:

* 2021.2
* 2022.1

You must use Xcode 12 or later when compiling an iOS Player that includes this package.

## Known limitations

* Color correction is not available as an RGB Value (only as color temperature).

## Package contents

This version of Apple ARKit XR Plug-in includes:

* A static library which provides implementation of the XR Subsystems listed above
* An Objective-C source file
* A shader used for rendering the camera image
* A plug-in metadata file

For more code examples, see the [AR Foundation Samples repo](https://github.com/Unity-Technologies/arfoundation-samples).

[!include[](snippets/apple-arkit-trademark.md)]
