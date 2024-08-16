using Unity.Collections;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;
#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// The ARKit implementation of the <c>XRAnchorSubsystem</c>. Do not create this directly.
    /// Use the <c>SubsystemManager</c> instead.
    /// </summary>
    [Preserve]
    public sealed class ARKitAnchorSubsystem : XRAnchorSubsystem
    {
        class ARKitProvider : Provider
        {
            public override void Start() => UnityARKit_anchors_onStart();

            public override void Stop() => UnityARKit_anchors_onStop();

            public override void Destroy() => UnityARKit_anchors_onDestroy();

            public override unsafe TrackableChanges<XRAnchor> GetChanges(
                XRAnchor defaultAnchor,
                Allocator allocator)
            {
                void* addedPtr, updatedPtr, removedPtr;
                int addedCount, updatedCount, removedCount, elementSize;
                var context = UnityARKit_anchors_acquireChanges(
                    out addedPtr, out addedCount,
                    out updatedPtr, out updatedCount,
                    out removedPtr, out removedCount,
                    out elementSize);

                try
                {
                    return new TrackableChanges<XRAnchor>(
                        addedPtr, addedCount,
                        updatedPtr, updatedCount,
                        removedPtr, removedCount,
                        defaultAnchor, elementSize,
                        allocator);
                }
                finally
                {
                    UnityARKit_anchors_releaseChanges(context);
                }
            }

            public override bool TryAddAnchor(Pose pose, out XRAnchor anchor)
            {
                return UnityARKit_anchors_tryAdd(pose, out anchor);
            }

            public override bool TryAttachAnchor(
                TrackableId attachedToId,
                Pose pose,
                out XRAnchor anchor)
            {
                return UnityARKit_anchors_tryAttach(attachedToId, pose, out anchor);
            }

            public override bool TryRemoveAnchor(TrackableId anchorId)
            {
                return UnityARKit_anchors_tryRemove(anchorId);
            }

#if UNITY_IOS && !UNITY_EDITOR
            [DllImport("__Internal")]
            static extern void UnityARKit_anchors_onStart();

            [DllImport("__Internal")]
            static extern void UnityARKit_anchors_onStop();

            [DllImport("__Internal")]
            static extern unsafe void UnityARKit_anchors_onDestroy();

            [DllImport("__Internal")]
            static extern unsafe void* UnityARKit_anchors_acquireChanges(
                out void* addedPtr, out int addedCount,
                out void* updatedPtr, out int updatedCount,
                out void* removedPtr, out int removedCount,
                out int elementSize);

            [DllImport("__Internal")]
            static extern unsafe void UnityARKit_anchors_releaseChanges(void* changes);

            [DllImport("__Internal")]
            static extern bool UnityARKit_anchors_tryAdd(
                Pose pose,
                out XRAnchor anchor);

            [DllImport("__Internal")]
            static extern bool UnityARKit_anchors_tryAttach(
                TrackableId trackableToAffix,
                Pose pose,
                out XRAnchor anchor);

            [DllImport("__Internal")]
            static extern bool UnityARKit_anchors_tryRemove(TrackableId anchorId);
#else
            static readonly string k_ExceptionMsg = "Apple ARKit XR Plug-in Provider not enabled in project settings.";

            static void UnityARKit_anchors_onStart()
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }

            static void UnityARKit_anchors_onStop()
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }

            static unsafe void UnityARKit_anchors_onDestroy()
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }

            static unsafe void* UnityARKit_anchors_acquireChanges(
                out void* addedPtr, out int addedCount,
                out void* updatedPtr, out int updatedCount,
                out void* removedPtr, out int removedCount,
                out int elementSize)
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }

            static unsafe void UnityARKit_anchors_releaseChanges(void* changes)
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }

            static bool UnityARKit_anchors_tryAdd(
                Pose pose,
                out XRAnchor anchor)
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }

            static bool UnityARKit_anchors_tryAttach(
                TrackableId trackableToAffix,
                Pose pose,
                out XRAnchor anchor)
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }

            static bool UnityARKit_anchors_tryRemove(TrackableId anchorId)
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
            if (!Api.AtLeast11_0())
                return;

#if UNITY_IOS && !UNITY_EDITOR
            var cinfo = new XRAnchorSubsystemDescriptor.Cinfo
            {
                id = "ARKit-Anchor",
                providerType = typeof(ARKitAnchorSubsystem.ARKitProvider),
                subsystemTypeOverride = typeof(ARKitAnchorSubsystem),
                supportsTrackableAttachments = true,
                supportsSynchronousAdd = true,
                supportsSaveAnchor = false,
                supportsLoadAnchor = false,
                supportsEraseAnchor = false,
                supportsGetSavedAnchorIds = false,
                supportsAsyncCancellation = false,
            };

            XRAnchorSubsystemDescriptor.Register(cinfo);
#endif
        }
    }
}
