using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents the minimum and maximum supported white balance gain values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ARKitWhiteBalanceRange : IEquatable<ARKitWhiteBalanceRange>
    {
        float m_MinimumGain;
        float m_MaximumGain;

        /// <summary>
        /// Minimum supported white balance gain value.
        /// </summary>
        public float minimumGain => m_MinimumGain;

        /// <summary>
        /// Maximum supported white balance gain value.
        /// </summary>
        public float maximumGain => m_MaximumGain;

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <remarks>
        /// Two <see cref="ARKitWhiteBalanceRange"/>s are considered equal if their underlying minimum gain, and
        /// maximum gain values are equal.
        /// </remarks>
        /// <param name="other">The <see cref="ARKitWhiteBalanceRange"/> to compare against.</param>
        /// <returns><see langword="true"/> if the underlying white balance gain range are the same.
        ///   Otherwise, <see langword="false"/>.</returns>
        public bool Equals(ARKitWhiteBalanceRange other)
        {
            return m_MinimumGain.Equals(other.m_MinimumGain)
                   && m_MaximumGain.Equals(other.m_MaximumGain);
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare against.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is an <see cref="ARKitWhiteBalanceRange"/> and it compares
        ///     equal to this one using <see cref="Equals(UnityEngine.XR.ARKit.ARKitWhiteBalanceRange)"/>. Otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is ARKitWhiteBalanceRange other
                   && Equals(other);
        }

        /// <summary>
        /// Generates a hash code suitable for use with a `HashSet` or `Dictionary`
        /// </summary>
        /// <returns>A hash code for this <see cref="ARKitWhiteBalanceRange"/>.</returns>
        public override int GetHashCode()
        {
            int hashCode = 486187739;
            unchecked
            {
                hashCode = (hashCode * 486187739) + m_MinimumGain.GetHashCode();
                hashCode = (hashCode * 486187739) + m_MaximumGain.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Tests for equality. Same as <see cref="Equals(UnityEngine.XR.ARKit.ARKitWhiteBalanceRange)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitWhiteBalanceRange"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitWhiteBalanceRange"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(UnityEngine.XR.ARKit.ARKitWhiteBalanceRange)"/>. Otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(ARKitWhiteBalanceRange lhs, ARKitWhiteBalanceRange rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Tests for inequality. Equivalent to the negation of <see cref="Equals(UnityEngine.XR.ARKit.ARKitWhiteBalanceRange)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitWhiteBalanceRange"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitWhiteBalanceRange"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="false"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(UnityEngine.XR.ARKit.ARKitWhiteBalanceRange)"/>. Otherwise, <see langword="true"/>.</returns>
        public static bool operator !=(ARKitWhiteBalanceRange lhs, ARKitWhiteBalanceRange rhs) => !lhs.Equals(rhs);

        /// <summary>
        /// Generates a string representation of this <see cref="ARKitWhiteBalanceRange"/> suitable for debugging purposes.
        /// </summary>
        /// <returns>A string representation of this <see cref="ARKitWhiteBalanceRange"/>.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  MinimumGain: {m_MinimumGain:0.000}");
            sb.AppendLine($"  MaximumGain: {m_MaximumGain:0.000}");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}

