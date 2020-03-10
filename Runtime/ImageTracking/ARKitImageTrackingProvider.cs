using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    [Preserve]
    /// <summary>
    /// ARKit implementation of the <c>XRImageTrackingSubsystem</c>. You shouldn't
    /// need to interact directly with this unless using the <see cref="maximumNumberOfTrackedImages"/>
    /// property.
    /// </summary>
    public sealed class ARKitImageTrackingSubsystem : XRImageTrackingSubsystem
    {
        class ARKitProvider : Provider
        {
            public unsafe override RuntimeReferenceImageLibrary CreateRuntimeLibrary(
                XRReferenceImageLibrary serializedLibrary)
            {
                return new ARKitImageDatabase(serializedLibrary);
            }

            public override RuntimeReferenceImageLibrary imageLibrary
            {
                set
                {
                    if (value == null)
                    {
                        UnityARKit_ImageTracking_Stop();
                    }
                    else if (value is ARKitImageDatabase database)
                    {
                        UnityARKit_ImageTracking_SetDatabase(database.nativePtr);
                    }
                    else
                    {
                        throw new ArgumentException($"{value.GetType().Name} is not a valid ARKit image library.");
                    }
                }
            }

            public unsafe override TrackableChanges<XRTrackedImage> GetChanges(
                XRTrackedImage defaultTrackedImage,
                Allocator allocator)
            {
                void* addedPtr, updatedPtr, removedPtr;
                int addedLength, updatedLength, removedLength, stride;

                var context = UnityARKit_ImageTracking_AcquireChanges(
                    out addedPtr, out addedLength,
                    out updatedPtr, out updatedLength,
                    out removedPtr, out removedLength,
                    out stride);

                try
                {
                    return new TrackableChanges<XRTrackedImage>(
                        addedPtr, addedLength,
                        updatedPtr, updatedLength,
                        removedPtr, removedLength,
                        defaultTrackedImage, stride,
                        allocator);
                }
                finally
                {
                    UnityARKit_ImageTracking_ReleaseChanges(context);
                }
            }

            public override void Destroy() => UnityARKit_ImageTracking_Destroy();

            public override int requestedMaxNumberOfMovingImages
            {
                get => UnityARKit_ImageTracking_GetRequestedMaximumNumberOfTrackedImages();
                set => UnityARKit_ImageTracking_SetRequestedMaximumNumberOfTrackedImages(value);
            }

            public override int currentMaxNumberOfMovingImages => UnityARKit_ImageTracking_GetCurrentMaximumNumberOfTrackedImages();
        }

        [DllImport("__Internal")]
        static extern int UnityARKit_ImageTracking_GetRequestedMaximumNumberOfTrackedImages();

        [DllImport("__Internal")]
        static extern void UnityARKit_ImageTracking_SetRequestedMaximumNumberOfTrackedImages(int maxNumTrackedImages);

        [DllImport("__Internal")]
        static extern int UnityARKit_ImageTracking_GetCurrentMaximumNumberOfTrackedImages();

        [DllImport("__Internal")]
        static extern void UnityARKit_ImageTracking_SetDatabase(IntPtr database);

        [DllImport("__Internal")]
        static extern void UnityARKit_ImageTracking_Stop();

        [DllImport("__Internal")]
        static extern void UnityARKit_ImageTracking_Destroy();

        [DllImport("__Internal")]
        static extern unsafe void* UnityARKit_ImageTracking_AcquireChanges(
            out void* addedPtr, out int addedLength,
            out void* updatedPtr, out int updatedLength,
            out void* removedPtr, out int removedLength,
            out int stride);

        [DllImport("__Internal")]
        static extern unsafe void UnityARKit_ImageTracking_ReleaseChanges(void* changes);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
            // No support before iOS 11.3
            if (!Api.AtLeast11_3())
                return;

            XRImageTrackingSubsystemDescriptor.Create(new XRImageTrackingSubsystemDescriptor.Cinfo
            {
                id = "ARKit-ImageTracking",
                subsystemImplementationType = typeof(ARKitImageTrackingSubsystem),
                supportsMovingImages = Api.AtLeast12_0(),
                supportsMutableLibrary = true,
                requiresPhysicalImageDimensions = true
            });
        }

        protected override Provider CreateProvider()
        {
            return new ARKitProvider();
        }
    }
}
