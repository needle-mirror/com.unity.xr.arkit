using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// ARKit implementation of the <c>XRParticipantSubsystem</c>. Do not create this using <c>new</c>. Instead, use the
    /// <a href="https://docs.unity3d.com/ScriptReference/SubsystemManager.GetSubsystemDescriptors.html">SubsystemManager</a>
    /// to enumerate the available <see cref="XRParticipantSubsystemDescriptor"/>s and get or create an instance from the descriptor.
    /// </summary>
    [Preserve]
    public sealed class ARKitParticipantSubsystem : XRParticipantSubsystem
    {
        class ARKitProvider : Provider
        {
            IntPtr m_Ptr;

            bool created => m_Ptr != IntPtr.Zero;

            public ARKitProvider() => m_Ptr = UnityARKit_Participant_init();

            public override void Start()
            {
                if (!created)
                    throw new InvalidOperationException($"The {typeof(ARKitParticipantSubsystem).Name} provider has not been created.");

                Api.UnityARKit_TrackableProvider_start(m_Ptr);
            }

            public override void Stop()
            {
                if (!created)
                    throw new InvalidOperationException($"The {typeof(ARKitParticipantSubsystem).Name} provider has not been created.");

                Api.UnityARKit_TrackableProvider_stop(m_Ptr);
            }

            public override void Destroy() => NSObject.Dispose(ref m_Ptr);

            public override unsafe TrackableChanges<XRParticipant> GetChanges(
                XRParticipant defaultParticipant,
                Allocator allocator)
            {
                if (!created)
                    throw new InvalidOperationException($"The {typeof(ARKitParticipantSubsystem).Name} provider has not been created.");

                using (var nativeChanges = Api.UnityARKit_TrackableProvider_acquireChanges(m_Ptr))
                {
                    var changes = new TrackableChanges<XRParticipant>(
                        nativeChanges.addedLength,
                        nativeChanges.updatedLength,
                        nativeChanges.removedLength,
                        allocator, defaultParticipant);

                    Api.UnityARKit_TrackableProvider_copyChanges(
                        m_Ptr, nativeChanges,
                        UnsafeUtility.SizeOf<XRParticipant>(),
                        changes.added.GetUnsafePtr(),
                        changes.updated.GetUnsafePtr(),
                        changes.removed.GetUnsafePtr());

                    return changes;
                }
            }
#if UNITY_XR_ARKIT_LOADER_ENABLED
            [DllImport("__Internal")]
            static extern IntPtr UnityARKit_Participant_init();
#else
            static IntPtr UnityARKit_Participant_init()
            {
                throw new NotImplementedException("Apple ARKit XR Plug-in Provider not enabled in project settings.");
            }
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void RegisterDescriptor()
        {
            if (!Api.AtLeast13_0())
                return;

            XRParticipantSubsystemDescriptor.Register(
                new XRParticipantSubsystemDescriptor.Cinfo()
                {
                    id = "ARKit-Participant",
                    providerType = typeof(ARKitParticipantSubsystem.ARKitProvider),
                    subsystemTypeOverride = typeof(ARKitParticipantSubsystem)
                });
        }
    }
}
