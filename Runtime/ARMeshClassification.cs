using System.ComponentModel;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// A value describing the classification of a mesh face.
    /// </summary>
    public enum ARMeshClassification : byte
    {
        /// <summary>
        /// The face type of the mesh is unknown.
        /// </summary>
        None = 0,

        /// <summary>
        /// The face type of the mesh is wall.
        /// </summary>
        Wall,

        /// <summary>
        /// The face type of the mesh is floor.
        /// </summary>
        Floor,

        /// <summary>
        /// The face type of the mesh is ceiling.
        /// </summary>
        Ceiling,

        /// <summary>
        /// The face type of the mesh is table.
        /// </summary>
        Table,

        /// <summary>
        /// The face type of the mesh is seat.
        /// </summary>
        Seat,

        /// <summary>
        /// The face type of the mesh is window.
        /// </summary>
        Window,

        /// <summary>
        /// The face type of the mesh is door.
        /// </summary>
        Door,
    }
}
