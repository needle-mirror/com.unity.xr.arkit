using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Read-only struct that stores lens position value of camera focus.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ARKitFocus : IEquatable<ARKitFocus>
    {
        float m_LensPosition;

        /// <summary>
        /// The lens position value. The value doesn't correspond to an exact physical distance of the lens. The range
        /// of possible positions is 0.0 to 1.0, with 0.0 being the shortest distance at which the lens can focus and
        /// 1.0 the furthest. Note that 1.0 doesn't represent focus at infinity.
        /// </summary>
        public float lensPosition => m_LensPosition;

        /// <summary>
        /// Constructs an instance with given lens position value.
        /// </summary>
        /// <param name="lensPosition">The lens position value, between 0 and 1. Refer to
        /// <see cref="lensPosition"/> for more information.</param>
        public ARKitFocus(float lensPosition)
        {
            m_LensPosition = lensPosition;
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <remarks>
        /// Two <see cref="ARKitFocus"/>s are considered equal if their lens position value is equal.
        /// </remarks>
        /// <param name="other">The <see cref="ARKitFocus"/> to compare against.</param>
        /// <returns><see langword="true"/> if the lens position value is equal. Otherwise, <see langword="false"/>.</returns>
        public bool Equals(ARKitFocus other)
        {
            return m_LensPosition.Equals(other.m_LensPosition);
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare against.</param>
        /// <returns>Returns <see langword="true"/> if <paramref name="obj"/> is an <see cref="ARKitFocus"/> and is
        ///     equal to this instance using <see cref="Equals(UnityEngine.XR.ARKit.ARKitFocus)"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is ARKitFocus other
                   && Equals(other);
        }

        /// <summary>
        /// Generates a hash code suitable for use with a `HashSet` or `Dictionary`
        /// </summary>
        /// <returns>Returns a hash code for this <see cref="ARKitFocus"/>.</returns>
        public override int GetHashCode()
        {
            int hashCode = 486187739;
            unchecked
            {
                hashCode = (hashCode * 486187739) + m_LensPosition.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Tests for equality. Same as <see cref="Equals(UnityEngine.XR.ARKit.ARKitFocus)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitFocus"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitFocus"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(UnityEngine.XR.ARKit.ARKitFocus)"/>. Otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(ARKitFocus lhs, ARKitFocus rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Tests for inequality. Same as the negation of <see cref="Equals(UnityEngine.XR.ARKit.ARKitFocus)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitFocus"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitFocus"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="false"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(UnityEngine.XR.ARKit.ARKitFocus)"/>. Otherwise, <see langword="true"/>.</returns>
        public static bool operator !=(ARKitFocus lhs, ARKitFocus rhs) => !lhs.Equals(rhs);

        /// <summary>
        /// Generates a string representation of this <see cref="ARKitFocus"/> suitable for debugging purposes.
        /// </summary>
        /// <returns>A string representation of this <see cref="ARKitFocus"/>.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  Lens Position: {m_LensPosition:0.000}");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
