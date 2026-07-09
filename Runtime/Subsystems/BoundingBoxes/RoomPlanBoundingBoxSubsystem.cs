using System;
using Unity.Collections;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;
#if UNITY_XR_ARKIT_LOADER_ENABLED
using System.Runtime.InteropServices;
#endif

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// The room capture instructions, as defined by the specification of
    /// <a href="https://developer.apple.com/documentation/roomplan/roomcapturesession/instruction">RoomCaptureSession.Instruction</a> by Apple.
    /// </summary>
    [Flags]
    public enum XRBoundingBoxInstructions : uint
    {
        /// <summary>
        /// No instruction returning.
        /// </summary>
        None = 0,

        /// <summary>
        /// The instruction that indicates scanning proceeds normally.
        /// </summary>
        Normal = 1 << 0,

        /// <summary>
        /// The instruction that requests the user move closer to the wall when scanning.
        /// </summary>
        MoveCloseToWall = 1 << 1,

        /// <summary>
        /// The instruction that requests the user move further from the wall when scanning.
        /// </summary>
        MoveAwayFromWall = 1 << 2,

        /// <summary>
        /// The instruction that requests the user increase the amount of light in the room when scanning.
        /// </summary>
        TurnOnLight = 1 << 3,

        /// <summary>
        /// The instruction that requests the user move slower when scanning.
        /// </summary>
        SlowDown = 1 << 4,

        /// <summary>
        /// The instruction that indicates the room feature is not distinguishable to detect when scanning.
        /// </summary>
        LowTexture = 1 << 5,
    }

    /// <summary>
    /// The implementation of the <c>XRBoundingBoxSubsystem</c>. Do not create this directly. Use the <c>SubsystemManager</c> instead.
    /// </summary>
    [Preserve]
    public sealed class RoomPlanBoundingBoxSubsystem : XRBoundingBoxSubsystem
    {
        class ARKitProvider : Provider
        {
            public ARKitProvider() => NativeApi.UnityARKit_BoundingBox_Construct();

            public override void Start() { }

            public override void Stop() { }

            public override void Destroy() => NativeApi.UnityARKit_BoundingBox_Destruct();

            public override unsafe TrackableChanges<XRBoundingBox> GetChanges(XRBoundingBox defaultXRBoundingBox, Allocator allocator)
            {
                var context = NativeApi.UnityARKit_BoundingBox_GetChanges(
                    out void* addedPtr, out int addedCount,
                    out void* updatedPtr, out int updatedCount,
                    out void* removedPtr, out int removedCount,
                    out int elementSize);

                try
                {
                    return new TrackableChanges<XRBoundingBox>(
                        addedPtr, addedCount,
                        updatedPtr, updatedCount,
                        removedPtr, removedCount,
                        defaultXRBoundingBox, elementSize,
                        allocator);
                }
                finally
                {
                    NativeApi.UnityARKit_BoundingBox_ReleaseChanges(context);
                }
            }
        }

        /// <summary>
        /// Set up room capture.
        /// </summary>
        /// <returns>`True` if room capture is successfully set up, otherwise false.</returns>
        /// <example>
        /// <code source="../Tests/Runtime/CodeSamples/RoomPlanBoundingBoxSubsystemTests.cs" region="SetupRoomCaptureSample"/>
        /// </example>
        public bool SetupRoomCapture() => NativeApi.UnityARKit_BoundingBox_SetupRoomCapture();

        /// <summary>
        /// Start the room capture process.
        /// </summary>
        /// <example>
        /// <code source="../Tests/Runtime/CodeSamples/RoomPlanBoundingBoxSubsystemTests.cs" region="StartRoomCaptureSample"/>
        /// </example>
        public void StartRoomCapture() => NativeApi.UnityARKit_BoundingBox_StartRoomCapture();

        /// <summary>
        /// Stop the room capture process.
        /// </summary>
        /// <example>
        /// <code source="../Tests/Runtime/CodeSamples/RoomPlanBoundingBoxSubsystemTests.cs" region="StopRoomCaptureSample"/>
        /// </example>
        public void StopRoomCapture() => NativeApi.UnityARKit_BoundingBox_StopRoomCapture();

        /// <summary>
        /// Check the status of room capture process.
        /// </summary>
        /// <returns>`True` if the process is room capturing, otherwise false.</returns>
        /// <example>
        /// <code source="../Tests/Runtime/CodeSamples/RoomPlanBoundingBoxSubsystemTests.cs" region="IsRoomCapturingSample"/>
        /// </example>
        public bool IsRoomCapturing() => NativeApi.UnityARKit_BoundingBox_IsRoomCapturing();

        /// <summary>
        /// Receive the instruction during room capture.
        /// </summary>
        /// <param name="instruction">The instruction from room capture process.</param>
        /// <example>
        /// <code source="../Tests/Runtime/CodeSamples/RoomPlanBoundingBoxSubsystemTests.cs" region="GetRoomCaptureInstructionSample"/>
        /// </example>
        public void GetRoomCaptureInstruction(out XRBoundingBoxInstructions instruction)
            => NativeApi.UnityARKit_BoundingBox_GetRoomCaptureInstruction(out instruction);

        /// <summary>
        /// Register the roomplan boundingbox subsystem if iOS and not the editor.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
            if (!Api.AtLeast17_0())
                return;

            const string subsystemId = "ARKit-RoomPlan-BoundingBox";
            var boundingboxSubsystemCinfo = new XRBoundingBoxSubsystemDescriptor.Cinfo
            {
                id = subsystemId,
                providerType = typeof(RoomPlanBoundingBoxSubsystem.ARKitProvider),
                subsystemTypeOverride = typeof(RoomPlanBoundingBoxSubsystem),
                supportsClassification = true,
            };

            XRBoundingBoxSubsystemDescriptor.Register(boundingboxSubsystemCinfo);
        }

        /// <summary>
        /// Container to wrap the native roomplan boundingbox APIs.
        /// </summary>
        static class NativeApi
        {
#if UNITY_XR_ARKIT_LOADER_ENABLED
            [DllImport("__Internal")]
            public static extern void UnityARKit_BoundingBox_Construct();

            [DllImport("__Internal")]
            public static extern void UnityARKit_BoundingBox_Destruct();

            [DllImport("__Internal")]
            public static extern void UnityARKit_BoundingBox_StartRoomCapture();

            [DllImport("__Internal")]
            public static extern void UnityARKit_BoundingBox_StopRoomCapture();

            [DllImport("__Internal")]
            public static extern unsafe void UnityARKit_BoundingBox_GetRoomCaptureInstruction(out XRBoundingBoxInstructions instruction);

            [DllImport("__Internal")]
            public static extern bool UnityARKit_BoundingBox_SetupRoomCapture();

            [DllImport("__Internal")]
            public static extern bool UnityARKit_BoundingBox_IsRoomCapturing();

            [DllImport("__Internal")]
            public static extern unsafe void* UnityARKit_BoundingBox_GetChanges(
                    out void* addedPtr, out int addedCount,
                    out void* updatedPtr, out int updatedCount,
                    out void* removedPtr, out int removedCount,
                    out int elementSize);

            [DllImport("__Internal")]
            public static extern unsafe void UnityARKit_BoundingBox_ReleaseChanges(void* changes);
#else
            public static void UnityARKit_BoundingBox_Construct()
                => throw new NotSupportedException();

            public static void UnityARKit_BoundingBox_Destruct()
                => throw new NotSupportedException();

            public static void UnityARKit_BoundingBox_StartRoomCapture()
                => throw new NotSupportedException();

            public static void UnityARKit_BoundingBox_StopRoomCapture()
                => throw new NotSupportedException();

            public static unsafe void UnityARKit_BoundingBox_GetRoomCaptureInstruction(out XRBoundingBoxInstructions instruction)
                => throw new NotSupportedException();

            public static bool UnityARKit_BoundingBox_SetupRoomCapture()
                => throw new NotSupportedException();

            public static bool UnityARKit_BoundingBox_IsRoomCapturing()
                => throw new NotSupportedException();

            public static unsafe void* UnityARKit_BoundingBox_GetChanges(
                    out void* addedPtr, out int addedCount,
                    out void* updatedPtr, out int updatedCount,
                    out void* removedPtr, out int removedCount,
                    out int elementSize)
                => throw new NotSupportedException();

            public static unsafe void UnityARKit_BoundingBox_ReleaseChanges(void* changes)
                => throw new NotSupportedException();
#endif
        }
    }
}
