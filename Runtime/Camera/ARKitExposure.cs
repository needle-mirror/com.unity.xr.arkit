using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Read-only struct that stores exposure duration and ISO values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ARKitExposure : IEquatable<ARKitExposure>
    {
        double m_Duration;
        float m_Iso;

        /// <summary>
        /// Exposure duration in seconds with sub-millisecond precision.
        /// </summary>
        public double duration => m_Duration;

        /// <summary>
        /// The exposure ISO value.
        /// </summary>
        public float iso => m_Iso;

        /// <summary>
        /// Constructs an instance with given exposure values.
        /// </summary>
        /// <param name="duration">Exposure duration in seconds.</param>
        /// <param name="iso">The ISO value.</param>
        public ARKitExposure(double duration, float iso)
        {
            m_Duration = duration;
            m_Iso = iso;
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <remarks>
        /// Two <see cref="ARKitExposure"/>s are considered equal if their duration and ISO values are equal.
        /// </remarks>
        /// <param name="other">The <see cref="ARKitExposure"/> to compare against.</param>
        /// <returns><see langword="true"/> if the duration and ISO values are equal. Otherwise, <see langword="false"/>.</returns>
        public bool Equals(ARKitExposure other)
        {
            return m_Duration.Equals(other.m_Duration)
                   && m_Iso.Equals(other.m_Iso);
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare against.</param>
        /// <returns>Returns <see langword="true"/> if <paramref name="obj"/> is an <see cref="ARKitExposure"/> and is
        ///     equal to this instance using <see cref="Equals(UnityEngine.XR.ARKit.ARKitExposure)"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is ARKitExposure other
                   && Equals(other);
        }

        /// <summary>
        /// Generates a hash code suitable for use with a `HashSet` or `Dictionary`
        /// </summary>
        /// <returns>Returns a hash code for this <see cref="ARKitExposure"/>.</returns>
        public override int GetHashCode()
        {
            int hashCode = 486187739;
            unchecked
            {
                hashCode = (hashCode * 486187739) + m_Duration.GetHashCode();
                hashCode = (hashCode * 486187739) + m_Iso.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Tests for equality. Same as <see cref="Equals(UnityEngine.XR.ARKit.ARKitExposure)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitExposure"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitExposure"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(UnityEngine.XR.ARKit.ARKitExposure)"/>. Otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(ARKitExposure lhs, ARKitExposure rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Tests for inequality. Same as the negation of <see cref="Equals(UnityEngine.XR.ARKit.ARKitExposure)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitExposure"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitExposure"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="false"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(UnityEngine.XR.ARKit.ARKitExposure)"/>. Otherwise, <see langword="true"/>.</returns>
        public static bool operator !=(ARKitExposure lhs, ARKitExposure rhs) => !lhs.Equals(rhs);

        /// <summary>
        /// Generates a string representation of this <see cref="ARKitExposure"/> suitable for debugging purposes.
        /// </summary>
        /// <returns>A string representation of this <see cref="ARKitExposure"/>.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  Duration: {m_Duration:0.000}");
            sb.AppendLine($"  ISO: {m_Iso:0.000}");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
