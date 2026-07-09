using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents the white balance gain values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ARKitWhiteBalanceGains : IEquatable<ARKitWhiteBalanceGains>
    {
        float m_Blue;
        float m_Green;
        float m_Red;

        /// <summary>
        /// The blue gain component of the white balance value.
        /// </summary>
        public float blue => m_Blue;

        /// <summary>
        /// The green gain component of the white balance value.
        /// </summary>
        public float green => m_Green;

        /// <summary>
        /// The red gain component of the white balance value.
        /// </summary>
        public float red => m_Red;

        /// <summary>
        /// Constructs an instance with given white balance gain values.
        /// </summary>
        /// <param name="blue">The blue gain.</param>
        /// <param name="green">The green gain.</param>
        /// <param name="red">The red gain.</param>
        public ARKitWhiteBalanceGains(float blue, float green, float red)
        {
            m_Blue = blue;
            m_Green = green;
            m_Red = red;
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <remarks>
        /// Two <see cref="ARKitWhiteBalanceGains"/>s are considered equal if their underlying blue, green,
        /// and red gain values are equal.
        /// </remarks>
        /// <param name="other">The <see cref="ARKitWhiteBalanceGains"/> to compare against.</param>
        /// <returns><see langword="true"/> if the underlying blue, green, and red gains are the same.
        ///   Otherwise, <see langword="false"/>.</returns>
        public bool Equals(ARKitWhiteBalanceGains other)
        {
            return m_Blue.Equals(other.m_Blue)
                   && m_Green.Equals(other.m_Green)
                   && m_Red.Equals(other.m_Red);
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare against.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is an <see cref="ARKitWhiteBalanceGains"/> and it compares
        ///     equal to this one using <see cref="Equals(ARKitWhiteBalanceGains)"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is ARKitWhiteBalanceGains other
                   && Equals(other);
        }

        /// <summary>
        /// Generates a hash code suitable for use with a `HashSet` or `Dictionary`.
        /// </summary>
        /// <returns>A hash code for this <see cref="ARKitWhiteBalanceGains"/>.</returns>
        public override int GetHashCode()
        {
            int hashCode = 486187739;
            unchecked
            {
                hashCode = (hashCode * 486187739) + m_Blue.GetHashCode();
                hashCode = (hashCode * 486187739) + m_Green.GetHashCode();
                hashCode = (hashCode * 486187739) + m_Red.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Tests for equality. Same as <see cref="Equals(ARKitWhiteBalanceGains)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitWhiteBalanceGains"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitWhiteBalanceGains"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(ARKitWhiteBalanceGains)"/>. Otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(ARKitWhiteBalanceGains lhs, ARKitWhiteBalanceGains rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Tests for inequality. Same as the negation of <see cref="Equals(ARKitWhiteBalanceGains)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitWhiteBalanceGains"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitWhiteBalanceGains"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="false"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(ARKitWhiteBalanceGains)"/>. Otherwise, <see langword="true"/>.</returns>
        public static bool operator !=(ARKitWhiteBalanceGains lhs, ARKitWhiteBalanceGains rhs) => !lhs.Equals(rhs);

        /// <summary>
        /// Generates a string representation of this <see cref="ARKitWhiteBalanceGains"/> suitable for debugging purposes.
        /// </summary>
        /// <returns>A string representation of this <see cref="ARKitWhiteBalanceGains"/>.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  Blue gain: {m_Blue:0.000}");
            sb.AppendLine($"  Green gain: {m_Green:0.000}");
            sb.AppendLine($"  Red gain: {m_Red:0.000}");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
