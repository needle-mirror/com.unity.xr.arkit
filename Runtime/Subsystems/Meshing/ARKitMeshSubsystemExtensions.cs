using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// ARKit extension methods to the [XRMeshSubsystem](https://docs.unity3d.com/ScriptReference/XR.XRMeshSubsystem.html).
    /// </summary>
    [Obsolete("ARKitMeshSubsystemExtensions is deprecated as of Apple ARKit XR Plug-in version 6.3. Use XRMeshSubsystem instead.", false)]
    public static class ARKitMeshSubsystemExtensions
    {
        static class NativeApi
        {
#if UNITY_XR_ARKIT_LOADER_ENABLED
            [DllImport("__Internal")]
            public static extern unsafe void* UnityARKit_MeshProvider_AcquireClassifications(TrackableId trackableId, out int numClassifications);

            [DllImport("__Internal")]
            public static extern unsafe void UnityARKit_MeshProvider_ReleaseClassifications(void* classifications);

            [DllImport("__Internal")]
            public static extern unsafe bool UnityARKit_MeshProvider_IsClassificationEnabled();

            [DllImport("__Internal")]
            public static extern unsafe void UnityARKit_MeshProvider_SetClassificationEnabled(bool enabled);
#else
            static readonly string k_ExceptionMsg = "Apple ARKit XR Plug-in Provider not enabled in project settings.";

            public static unsafe void* UnityARKit_MeshProvider_AcquireClassifications(TrackableId trackableId, out int numClassifications)
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }

            public static unsafe void UnityARKit_MeshProvider_ReleaseClassifications(void* classifications)
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }

            public static unsafe bool UnityARKit_MeshProvider_IsClassificationEnabled()
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }

            public static unsafe void UnityARKit_MeshProvider_SetClassificationEnabled(bool enabled)
            {
                throw new System.NotImplementedException(k_ExceptionMsg);
            }
#endif
        }

        /// <summary>
        /// Get the face classifications for the given mesh ID.
        /// </summary>
        /// <param name="subsystem">The meshing subsystem.</param>
        /// <param name="meshId">The trackable ID representing the mesh.</param>
        /// <param name="allocator">The memory allocator type to use in allocating the native array memory.</param>
        /// <returns>
        /// An array of mesh classifications, one for each face in the mesh.
        /// </returns>
        public static unsafe NativeArray<ARMeshClassification> GetFaceClassifications(this XRMeshSubsystem subsystem, TrackableId meshId, Allocator allocator)
        {
            void* classifications = NativeApi.UnityARKit_MeshProvider_AcquireClassifications(meshId, out int numClassifications);

            try
            {
                if (classifications == null)
                {
                    numClassifications = 0;
                }

                var meshClassifications = new NativeArray<ARMeshClassification>(numClassifications, allocator);
                if (classifications != null)
                {
                    NativeArray<ARMeshClassification> tmp = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ARMeshClassification>(classifications, numClassifications, Allocator.None);
                    meshClassifications.CopyFrom(tmp);
                }

                return meshClassifications;
            }
            finally
            {
                NativeApi.UnityARKit_MeshProvider_ReleaseClassifications(classifications);
            }
        }

        /// <summary>
        /// Get the face classifications for the given mesh ID, converted to <see cref="XRMeshClassification"/> in uint format.
        /// </summary>
        /// <param name="subsystem">The meshing subsystem.</param>
        /// <param name="meshId">The trackable ID representing the mesh.</param>
        /// <param name="allocator">The memory allocator type to use in allocating the native array memory.</param>
        /// <returns>
        /// An array of mesh classifications, one for each face in the mesh.
        /// </returns>
        public static unsafe NativeArray<uint> GetMeshClassifications(this XRMeshSubsystem subsystem, TrackableId meshId, Allocator allocator)
        {
            void* classifications = NativeApi.UnityARKit_MeshProvider_AcquireClassifications(meshId, out int numClassifications);

            try
            {
                if (classifications == null)
                {
                    numClassifications = 0;
                }

                var meshClassifications = new NativeArray<uint>(numClassifications, allocator);
                if (classifications != null)
                {
                    NativeArray<ARMeshClassification> tmp = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<ARMeshClassification>(classifications, numClassifications, Allocator.None);
                    for (int i = 0; i < numClassifications; i++)
                    {
                        meshClassifications[i] = (uint)(GetXRMeshClassification(tmp[i]));
                    }
                }

                return meshClassifications;
            }
            finally
            {
                NativeApi.UnityARKit_MeshProvider_ReleaseClassifications(classifications);
            }
        }

        static XRMeshClassification GetXRMeshClassification(ARMeshClassification classification)
        {
            switch (classification)
            {
                case ARMeshClassification.None:
                    return XRMeshClassification.Unknown;
                case ARMeshClassification.Wall:
                    return XRMeshClassification.Wall;
                case ARMeshClassification.Floor:
                    return XRMeshClassification.Floor;
                case ARMeshClassification.Ceiling:
                    return XRMeshClassification.Ceiling;
                case ARMeshClassification.Table:
                    return XRMeshClassification.Table;
                case ARMeshClassification.Seat:
                    return XRMeshClassification.Seat;
                case ARMeshClassification.Window:
                    return XRMeshClassification.Window;
                case ARMeshClassification.Door:
                    return XRMeshClassification.Door;
                default:
                    return XRMeshClassification.Other;
            }
        }

        /// <summary>
        /// Whether mesh classification is enabled.
        /// </summary>
        /// <param name="subsystem">The meshing subsystem.</param>
        /// <returns>
        /// <c>true</c> if the mesh classification is enabled. Otherwise, <c>false</c>.
        /// </returns>
        public static bool GetClassificationEnabled(this XRMeshSubsystem subsystem)
        {
            return NativeApi.UnityARKit_MeshProvider_IsClassificationEnabled();
        }

        /// <summary>
        /// Sets whether mesh classification should be enabled.
        /// </summary>
        /// <param name="subsystem">The meshing subsystem.</param>
        /// <param name="enabled">Whether the mesh classification should be enabled.</param>
        public static void SetClassificationEnabled(this XRMeshSubsystem subsystem, bool enabled)
        {
            NativeApi.UnityARKit_MeshProvider_SetClassificationEnabled(enabled);
        }
    }
}
