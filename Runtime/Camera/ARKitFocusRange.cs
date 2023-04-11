using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents the minimum and maximum supported lens position. A lens position value doesn't correspond
    /// to an exact physical distance, nor does it represent a consistent focus distance from device to device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ARKitFocusRange : IEquatable<ARKitFocusRange>
    {
        float m_MinimumLensPosition;
        float m_MaximumLensPosition;

        /// <summary>
        /// Minimum supported lens position. This value represent the shortest distance at which the lens can focus.
        /// </summary>
        public float minimumLensPosition => m_MinimumLensPosition;

        /// <summary>
        /// Maximum supported lens position. This value represent the furthest distance at which the lens can focus.
        /// </summary>
        public float maximumLensPosition => m_MaximumLensPosition;

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <remarks>
        /// Two <see cref="ARKitFocusRange"/>s are considered equal if their minimum lens position and maximum lens position are equal.
        /// </remarks>
        /// <param name="other">The <see cref="ARKitFocusRange"/> to compare against.</param>
        /// <returns><see langword="true"/> if the lens position range is the same. Otherwise, <see langword="false"/>.</returns>
        public bool Equals(ARKitFocusRange other)
        {
            return m_MinimumLensPosition.Equals(other.m_MinimumLensPosition)
                   && m_MaximumLensPosition.Equals(other.m_MaximumLensPosition);
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare against.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is an <see cref="ARKitFocusRange"/> and is
        ///     equal to this instance using <see cref="Equals(UnityEngine.XR.ARKit.ARKitFocusRange)"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is ARKitFocusRange other
                   && Equals(other);
        }

        /// <summary>
        /// Generates a hash code suitable for use with a `HashSet` or `Dictionary`
        /// </summary>
        /// <returns>A hash code for this <see cref="ARKitFocusRange"/>.</returns>
        public override int GetHashCode()
        {
            int hashCode = 486187739;
            unchecked
            {
                hashCode = (hashCode * 486187739) + m_MinimumLensPosition.GetHashCode();
                hashCode = (hashCode * 486187739) + m_MaximumLensPosition.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Tests for equality. Same as <see cref="Equals(UnityEngine.XR.ARKit.ARKitFocusRange)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitFocusRange"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitFocusRange"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(UnityEngine.XR.ARKit.ARKitFocusRange)"/>. Otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(ARKitFocusRange lhs, ARKitFocusRange rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Tests for inequality. Same as the negation of <see cref="Equals(UnityEngine.XR.ARKit.ARKitFocusRange)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitFocusRange"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitFocusRange"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="false"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(UnityEngine.XR.ARKit.ARKitFocusRange)"/>. Otherwise, <see langword="true"/>.</returns>
        public static bool operator !=(ARKitFocusRange lhs, ARKitFocusRange rhs) => !lhs.Equals(rhs);

        /// <summary>
        /// Generates a string representation of this <see cref="ARKitFocusRange"/> suitable for debugging purposes.
        /// </summary>
        /// <returns>A string representation of this <see cref="ARKitFocusRange"/>.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  MinimumLensPosition: {m_MinimumLensPosition:0.000}");
            sb.AppendLine($"  MaximumLensPosition: {m_MaximumLensPosition:0.000}");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}

