using System;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Management;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents an ARKit-specific reference object for participation in an
    /// <c>XRReferenceObjectLibrary</c>.
    /// </summary>
    /// <remarks>
    /// The actual data used at runtime is packaged into the Xcode project
    /// in an asset catalog called <c>ARReferenceObjects.xcassets</c>. It should
    /// exist on disk in your project as an <c>.arobject</c> file.
    /// See <a href="https://developer.apple.com/documentation/arkit/scanning_and_detecting_3d_objects">Scanning and Detecting 3D Objects</a>
    /// for instructions on how to generate these files.
    /// </remarks>
    /// <seealso cref="XRReferenceObject"/>
    /// <seealso cref="XRReferenceObjectLibrary"/>
    public sealed class ARKitReferenceObjectEntry : XRReferenceObjectEntry, ISerializationCallbackReceiver
    {
        /// <summary>
        /// (Read Only) The reference origin of the scanned object (in session space).
        /// </summary>
        public Pose referenceOrigin => m_ReferenceOrigin;

        internal ARReferenceObject GetARKitReferenceObject(XRReferenceObject referenceObject)
        {
            m_ARKitReferenceObject.SetName(referenceObject);
            return m_ARKitReferenceObject;
        }

        /// <summary>
        /// Creates a new <see cref="ARKitReferenceObjectEntry"/> from a serialized
        /// [ARReferenceObject](https://developer.apple.com/documentation/arkit/arreferenceobject?language=objc).
        /// </summary>
        /// <param name="data">The bytes of a serialized
        /// [ARReferenceObject](https://developer.apple.com/documentation/arkit/arreferenceobject?language=objc).</param>
        /// <returns>Returns a new <see cref="ARKitReferenceObjectEntry"/> if <paramref name="data"/> is successfully
        /// deserialized into an ARReferenceObject. Returns `null` otherwise.</returns>
        public static ARKitReferenceObjectEntry Create(NativeSlice<byte> data)
        {
            var referenceObject = new ARReferenceObject(data);
            if (referenceObject == null)
                return null;

            var entry = CreateInstance<ARKitReferenceObjectEntry>();
            entry.m_ARKitReferenceObject = referenceObject;
            return entry;
        }

        /// <summary>
        /// Invoked when a new [XRReferenceObject](xref:UnityEngine.XR.ARSubsystems.XRReferenceObject) is added to an
        /// [XRReferenceObjectLibrary](xref:UnityEngine.XR.ARSubsystems.XRReferenceObjectLibrary).
        /// </summary>
        /// <param name="library">The library to which the reference object is being added.</param>
        /// <param name="xrReferenceObject">The reference object being added to the <paramref name="library"/>.</param>
        protected override void OnAddToLibrary(XRReferenceObjectLibrary library, XRReferenceObject xrReferenceObject)
        {
            base.OnAddToLibrary(library, xrReferenceObject);

            if (m_ARKitReferenceObject == null)
                return;

            var instance = XRGeneralSettings.Instance;
            if (instance == null)
                return;

            var manager = instance.Manager;
            if (manager == null)
                return;

            var loader = manager.activeLoader;
            if (loader == null)
                return;

            if (loader.GetLoadedSubsystem<XRObjectTrackingSubsystem>() is ARKitXRObjectTrackingSubsystem subsystem)
            {
                subsystem.AddReferenceObject(library, GetARKitReferenceObject(xrReferenceObject));
            }
        }

        void OnDestroy() => m_ARKitReferenceObject.Dispose();

        /// <summary>
        /// Invoked just before serialization.
        /// </summary>
        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        /// <summary>
        /// Invoked just after deserialization.
        /// </summary>
        void ISerializationCallbackReceiver.OnAfterDeserialize() =>
            m_ARKitReferenceObject = new ARReferenceObject(m_ReferenceObjectBytes);

#if UNITY_EDITOR
        internal void SetReferenceObjectBytes(byte[] data)
        {
            m_ReferenceObjectBytes = data;
        }
#endif

#pragma warning disable CS0649
        [SerializeField]
        internal Pose m_ReferenceOrigin;
#pragma warning restore CS0649

        [SerializeField]
        byte[] m_ReferenceObjectBytes;

        ARReferenceObject m_ARKitReferenceObject;
    }
}
