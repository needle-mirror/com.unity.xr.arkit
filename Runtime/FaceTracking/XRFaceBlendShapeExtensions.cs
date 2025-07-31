using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Extensions to the [XRFaceBlendShape](xref:UnityEngine.XR.ARSubsystems.XRFaceBlendShape) object.
    /// </summary>
    public static class XRFaceBlendShapeExtensions
    {
        /// <summary>
        /// Gets blend shape location as an ARKit <see cref="ARKitBlendShapeLocation"/>.
        /// </summary>
        /// <param name="blendshape">The [XRFaceBlendShape](xref:UnityEngine.XR.ARSubsystems.XRFaceBlendShape) being extended.</param>
        /// <returns>Returns the ARKit-specific representation for blend shape location.</returns>
        public static ARKitBlendShapeLocation AsARKitBlendShapeLocation(this XRFaceBlendShape blendshape)
        {
            return (ARKitBlendShapeLocation)blendshape.blendShapeId;
        }
    }
}
