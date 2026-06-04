using System.Collections;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit.CodeSamples.Tests
{
    /// <summary>
    /// Use this class to write sample code for <see cref="RoomPlanBoundingBoxSubsystem"/> to be rendered to the documentation manual.
    /// </summary>
    public class RoomPlanBoundingBoxSubsystemTests
    {
        #region CheckRoomCaptureSupportSample
        bool CheckRoomCaptureSupport(out RoomPlanBoundingBoxSubsystem subsystem, ARBoundingBoxManager boundingBoxManager)
        {
            // check if roomplan bounding box subsystem is available
            subsystem = boundingBoxManager.subsystem as RoomPlanBoundingBoxSubsystem;
            if (subsystem == null)
            {
                Debug.LogError("RoomPlanBoundingBoxSubsystem is not available.");
                return false;
            }

            return true;
        }
        #endregion CheckRoomCaptureSupportSample

        #region SetupRoomCaptureSample
        void RoomCaptureSetup(ARBoundingBoxManager boundingBoxManager)
        {
            if (!CheckRoomCaptureSupport(out RoomPlanBoundingBoxSubsystem subsystem, boundingBoxManager))
                return;

            if (subsystem.SetupRoomCapture())
            {
                // The room capture has been successfully set up
            }
        }
        #endregion SetupRoomCaptureSample

        #region StartRoomCaptureSample
        void RoomCaptureStart(ARBoundingBoxManager boundingBoxManager)
        {
            if (!CheckRoomCaptureSupport(out RoomPlanBoundingBoxSubsystem subsystem, boundingBoxManager))
                return;

            subsystem.StartRoomCapture();
        }
        #endregion StartRoomCaptureSample

        #region StopRoomCaptureSample
        void RoomCaptureStop(ARBoundingBoxManager boundingBoxManager)
        {
            if (!CheckRoomCaptureSupport(out RoomPlanBoundingBoxSubsystem subsystem, boundingBoxManager))
                return;

            subsystem.StopRoomCapture();
        }
        #endregion StopRoomCaptureSample

        #region IsRoomCapturingSample
        void CheckRoomCapturingStatus(ARBoundingBoxManager boundingBoxManager)
        {
            if (!CheckRoomCaptureSupport(out RoomPlanBoundingBoxSubsystem subsystem, boundingBoxManager))
                return;

            if (subsystem.IsRoomCapturing())
            {
                // Room capture is in progress
            }
        }
        #endregion IsRoomCapturingSample

        #region GetRoomCaptureInstructionSample
        void UpdateRoomCaptureInstruction(ARBoundingBoxManager boundingBoxManager)
        {
            if (!CheckRoomCaptureSupport(out RoomPlanBoundingBoxSubsystem subsystem, boundingBoxManager))
                return;

            subsystem.GetRoomCaptureInstruction(out XRBoundingBoxInstructions instruction);
            // display the instruction to guide users for room capture
        }
        #endregion GetRoomCaptureInstructionSample
    }
}
