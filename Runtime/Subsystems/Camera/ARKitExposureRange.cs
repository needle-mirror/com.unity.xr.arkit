using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Represents the minimum and maximum supported exposure duration and ISO values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ARKitExposureRange : IEquatable<ARKitExposureRange>
    {
        double m_MinimumDuration;
        double m_MaximumDuration;
        float m_MinimumIso;
        float m_MaximumIso;

        /// <summary>
        /// Minimum supported exposure duration in seconds with sub-millisecond precision.
        /// </summary>
        public double minimumDuration => m_MinimumDuration;

        /// <summary>
        /// Maximum supported exposure duration in seconds with sub-millisecond precision.
        /// </summary>
        public double maximumDuration => m_MaximumDuration;

        /// <summary>
        /// Minimum supported exposure ISO value.
        /// </summary>
        public float minimumIso => m_MinimumIso;

        /// <summary>
        /// Maximum supported exposure ISO value.
        /// </summary>
        public float maximumIso => m_MaximumIso;

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <remarks>
        /// Two <see cref="ARKitExposureRange"/>s are considered equal if their minimum duration, maximum duration, minimum ISO, and maximum ISO values are equal.
        /// </remarks>
        /// <param name="other">The <see cref="ARKitExposureRange"/> to compare against.</param>
        /// <returns><see langword="true"/> if the duration range and ISO range are the same. Otherwise, <see langword="false"/>.</returns>
        public bool Equals(ARKitExposureRange other)
        {
            return m_MinimumDuration.Equals(other.m_MinimumDuration)
                   && m_MaximumDuration.Equals(other.m_MaximumDuration)
                   && m_MinimumIso.Equals(other.m_MinimumIso)
                   && m_MaximumIso.Equals(other.m_MaximumIso);
        }

        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="obj">An <see cref="object"/> to compare against.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is an <see cref="ARKitExposureRange"/> and is
        ///     equal to this instance using <see cref="Equals(UnityEngine.XR.ARKit.ARKitExposureRange)"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is ARKitExposureRange other
                   && Equals(other);
        }

        /// <summary>
        /// Generates a hash code suitable for use with a `HashSet` or `Dictionary`
        /// </summary>
        /// <returns>A hash code for this <see cref="ARKitExposureRange"/>.</returns>
        public override int GetHashCode()
        {
            int hashCode = 486187739;
            unchecked
            {
                hashCode = (hashCode * 486187739) + m_MinimumDuration.GetHashCode();
                hashCode = (hashCode * 486187739) + m_MaximumDuration.GetHashCode();
                hashCode = (hashCode * 486187739) + m_MinimumIso.GetHashCode();
                hashCode = (hashCode * 486187739) + m_MaximumIso.GetHashCode();
            }
            return hashCode;
        }

        /// <summary>
        /// Tests for equality. Same as <see cref="Equals(UnityEngine.XR.ARKit.ARKitExposureRange)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitExposureRange"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitExposureRange"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(UnityEngine.XR.ARKit.ARKitExposureRange)"/>. Otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(ARKitExposureRange lhs, ARKitExposureRange rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Tests for inequality. Same as the negation of <see cref="Equals(UnityEngine.XR.ARKit.ARKitExposureRange)"/>.
        /// </summary>
        /// <param name="lhs">The <see cref="ARKitExposureRange"/> to compare with <paramref name="rhs"/>.</param>
        /// <param name="rhs">The <see cref="ARKitExposureRange"/> to compare with <paramref name="lhs"/>.</param>
        /// <returns><see langword="false"/> if <paramref name="lhs"/> is equal to <paramref name="rhs"/> using
        ///     <see cref="Equals(UnityEngine.XR.ARKit.ARKitExposureRange)"/>. Otherwise, <see langword="true"/>.</returns>
        public static bool operator !=(ARKitExposureRange lhs, ARKitExposureRange rhs) => !lhs.Equals(rhs);

        /// <summary>
        /// Generates a string representation of this <see cref="ARKitExposureRange"/> suitable for debugging purposes.
        /// </summary>
        /// <returns>A string representation of this <see cref="ARKitExposureRange"/>.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  MinimumDuration: {m_MinimumDuration:0.000}");
            sb.AppendLine($"  MaximumDuration: {m_MaximumDuration:0.000}");
            sb.AppendLine($"  MinimumISO: {m_MinimumIso:0.000}");
            sb.AppendLine($"  MaximumISO: {m_MaximumIso:0.000}");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}

