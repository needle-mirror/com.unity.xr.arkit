using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Assertions;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// ARKit implementation of the <c>XRRaycastSubsystem</c>. Do not create this directly. Use the <c>SubsystemManager</c> instead.
    /// </summary>
    [Preserve]
    public sealed class ARKitRaycastSubsystem : XRRaycastSubsystem
    {
        /// <summary>
        /// Creates the ARKit-specific implementation which will service the `XRRaycastSubsystem`.
        /// </summary>
        /// <returns>A new instance of the `Provider` specific to ARKit.</returns>
        protected override Provider CreateProvider() => new ARKitProvider();

        class ARKitProvider : XRRaycastSubsystem.Provider
        {
            IntPtr m_Self;

            public ARKitProvider() => m_Self = NativeApi.Construct();

            public override void Start()
            {
                Assert.AreNotEqual(IntPtr.Zero, m_Self);
                NativeApi.Start(m_Self);
            }

            public override void Stop()
            {
                Assert.AreNotEqual(IntPtr.Zero, m_Self);
                NativeApi.Stop(m_Self);
            }

            public override void Destroy()
            {
                Assert.AreNotEqual(IntPtr.Zero, m_Self);
                Api.CFRelease(ref m_Self);
            }

            public override unsafe TrackableChanges<XRRaycast> GetChanges(XRRaycast defaultRaycast, Allocator allocator)
            {
                Assert.AreNotEqual(IntPtr.Zero, m_Self);
                NativeApi.AcquireChanges(m_Self,
                    out XRRaycast* added, out int addedCount,
                    out XRRaycast* updated, out int updatedCount,
                    out TrackableId* removed, out int removedCount,
                    out int elementSize);

                try
                {
                    return new TrackableChanges<XRRaycast>(
                        added, addedCount,
                        updated, updatedCount,
                        removed, removedCount,
                        defaultRaycast, elementSize, allocator);
                }
                finally
                {
                    NativeApi.ReleaseChanges(added, updated, removed);
                }
            }

            public override bool TryAddRaycast(Ray ray, float estimatedDistance, out XRRaycast sessionRelativeData)
            {
                Assert.AreNotEqual(IntPtr.Zero, m_Self);
                return NativeApi.TryAddRaycast(m_Self, ray.origin, ray.direction, estimatedDistance, out sessionRelativeData);
            }

            public override void RemoveRaycast(TrackableId trackableId)
            {
                Assert.AreNotEqual(IntPtr.Zero, m_Self);
                NativeApi.RemoveRaycast(m_Self, trackableId);
            }

            public override unsafe NativeArray<XRRaycastHit> Raycast(
                XRRaycastHit defaultRaycastHit,
                Vector2 screenPoint,
                TrackableType trackableTypeMask,
                Allocator allocator)
            {
                void* hitResults;
                int count;
                NativeApi.UnityARKit_Raycast_AcquireHitResults(
                    screenPoint,
                    trackableTypeMask,
                    out hitResults,
                    out count);

                var results = new NativeArray<XRRaycastHit>(count, allocator);
                NativeApi.UnityARKit_Raycast_CopyAndReleaseHitResults(
                    UnsafeUtility.AddressOf(ref defaultRaycastHit),
                    UnsafeUtility.SizeOf<XRRaycastHit>(),
                    hitResults,
                    results.GetUnsafePtr());

                return results;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
            if (!Api.AtLeast11_0())
                return;

            XRRaycastSubsystemDescriptor.RegisterDescriptor(new XRRaycastSubsystemDescriptor.Cinfo
            {
                id = "ARKit-Raycast",
                subsystemImplementationType = typeof(ARKitRaycastSubsystem),
                supportsViewportBasedRaycast = true,
                supportsWorldBasedRaycast = false,
                supportedTrackableTypes =
                    TrackableType.Planes |
                    TrackableType.FeaturePoint,
                supportsTrackedRaycasts = true,
            });
        }

        static class NativeApi
        {
            [DllImport("__Internal", EntryPoint = "UnityARKit_Raycast_AcquireHitResults")]
            public static unsafe extern void UnityARKit_Raycast_AcquireHitResults(
                Vector2 screenPoint,
                TrackableType filter,
                out void* hitResults,
                out int hitCount);

            [DllImport("__Internal", EntryPoint = "UnityARKit_Raycast_CopyAndReleaseHitResults")]
            public static unsafe extern void UnityARKit_Raycast_CopyAndReleaseHitResults(
                void* defaultHit,
                int stride,
                void* hitResults,
                void* dstBuffer);

            [DllImport("__Internal", EntryPoint = "UnityARKit_Raycast_Construct")]
            public static extern IntPtr Construct();

            [DllImport("__Internal", EntryPoint = "UnityARKit_Raycast_Start")]
            public static extern void Start(IntPtr self);

            [DllImport("__Internal", EntryPoint = "UnityARKit_Raycast_Stop")]
            public static extern void Stop(IntPtr self);

            [DllImport("__Internal", EntryPoint = "UnityARKit_Raycast_AcquireChanges")]
            public static extern unsafe void AcquireChanges(IntPtr self,
                out XRRaycast* added, out int addedCount,
                out XRRaycast* updated, out int updatedCount,
                out TrackableId* removed, out int removedCount,
                out int elementSize);

            [DllImport("__Internal", EntryPoint = "UnityARKit_Raycast_ReleaseChanges")]
            public static extern unsafe void ReleaseChanges(XRRaycast* added, XRRaycast* updated, TrackableId* removed);

            [DllImport("__Internal", EntryPoint = "UnityARKit_Raycast_TryAddRaycast")]
            public static extern bool TryAddRaycast(IntPtr self, Vector3 origin, Vector3 direction, float estimatedDistance, out XRRaycast raycast);

            [DllImport("__Internal", EntryPoint = "UnityARKit_Raycast_RemoveRaycast")]
            public static extern void RemoveRaycast(IntPtr self, TrackableId trackableId);
        }
    }
}
