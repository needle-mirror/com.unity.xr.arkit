using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// Use this to construct an <see cref="ARCollaborationData"/> incrementally from serialized bytes.
    /// </summary>
    /// <remarks>
    /// This struct can be useful if you receive data through a stream. If you already have all
    /// the bytes, use a <see cref="ARCollaborationData"/> constructor instead.
    /// This struct represents a native resource and must be explicitly disposed when no longer needed.
    /// While this struct is not thread safe, you can construct, Dispose, and Append from any thread.
    /// </remarks>
    public struct ARCollaborationDataBuilder : IDisposable, IEquatable<ARCollaborationDataBuilder>
    {
        internal NSMutableData m_NSMutableData;

        /// <summary>
        /// Whether the <see cref="ARCollaborationDataBuilder"/> has allocated any data. If <c>true</c>,
        /// this struct must be disposed to avoid leaking native resources. If <c>false</c>, this struct
        /// either never allocated memory
        /// (with <see cref="Append(byte[],int,int)"/> or <see cref="Append(Unity.Collections.NativeSlice{byte})"/>)
        /// or it has already been <see cref="Dispose"/>d.
        /// </summary>
        public bool hasData => m_NSMutableData.created;

        /// <summary>
        /// The number of bytes owned by this struct.
        /// </summary>
        /// <seealso cref="Append(byte[])"/>
        /// <seealso cref="Append(byte[],int,int)"/>
        /// <seealso cref="Append(Unity.Collections.NativeSlice{byte})"/>
        public int length => m_NSMutableData.created ? m_NSMutableData.length : 0;

        /// <summary>
        /// Converts the bytes accumulated through calls to <see cref="Append(byte[],int,int)"/> to an <see cref="ARCollaborationData"/>.
        /// The caller is responsible for disposing the returned <see cref="ARCollaborationData"/>.
        /// </summary>
        /// <returns>A new <see cref="ARCollaborationData"/> constructed from the bytes added to this <see cref="ARCollaborationDataBuilder"/>.</returns>
        /// <exception cref="System.NotSupportedException">Thrown if <see cref="ARCollaborationData"/> is not supported.
        /// Check for support with <see cref="ARKitSessionSubsystem.supportsCollaboration"/>.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if <see cref="hasData"/> is false.</exception>
        public ARCollaborationData ToCollaborationData()
        {
            if (!ARKitSessionSubsystem.supportsCollaboration)
                throw new NotSupportedException("ARCollaborationData is not supported by this version of iOS.");

            if (!hasData)
                throw new InvalidOperationException("No data to convert to ARCollaborationData.");

            return new ARCollaborationData(m_NSMutableData);
        }

        /// <summary>
        /// Appends <paramref name="size"/> bytes of the array <paramref name="buffer"/> to an existing array of bytes.
        /// </summary>
        /// <param name="buffer">A buffer containing bytes to append.</param>
        /// <param name="offset">The offset within <paramref name="buffer"/> to start appending bytes to the internal array.</param>
        /// <param name="size">The number of bytes from <paramref name="buffer"/> to append. Must be less than <paramref name="buffer"/>`.Length` + <paramref name="offset"/>.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="buffer"/> is <see langword="null"/>.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="size"/> is less than zero.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> is less than zero.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if <paramref name="size"/> is less than zero or greater than the length of <paramref name="buffer"/>.</exception>
        public unsafe void Append(byte[] buffer, int offset, int size)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), size, $"'{nameof(size)}' must be greater than or equal to zero.");

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), offset, $"'{nameof(offset)}' must be greater than or equal to zero.");

            if (buffer.Length < size + offset)
                throw new InvalidOperationException($"Reading {size} bytes starting at offset {offset} would write past the end of '{nameof(buffer)}' (buffer length = {buffer.Length}).");

            fixed (byte* ptr = &buffer[offset])
            {
                AppendUnchecked(ptr, size);
            }
        }

        /// <summary>
        /// Appends all bytes of the array <paramref name="buffer"/> to an existing array of bytes.
        /// </summary>
        /// <param name="buffer">A buffer containing bytes to append.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="buffer"/> is <see langword="null"/>.</exception>
        public unsafe void Append(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            fixed (byte* ptr = buffer)
            {
                AppendUnchecked(ptr, buffer.Length);
            }
        }

        /// <summary>
        /// Appends <paramref name="bytes"/> to an existing array of bytes.
        /// </summary>
        /// <param name="bytes">An array of bytes to append to the existing data.</param>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="bytes"/> does not reference valid data.</exception>
        public unsafe void Append(NativeSlice<byte> bytes)
        {
            void* ptr = bytes.GetUnsafePtr();
            if (ptr == null)
                throw new ArgumentException("Invalid NativeSlice", nameof(bytes));

            AppendUnchecked(ptr, bytes.Length);
        }

        /// <summary>
        /// Releases the native resource.
        /// </summary>
        public void Dispose() => m_NSMutableData.Dispose();

        /// <summary>
        /// Appends data to the underlying `NSMutableData` array.
        /// </summary>
        /// <param name="bytes">A pointer to an array of bytes to append to the existing data.</param>
        /// <param name="size">The number of bytes pointed to by <paramref name="bytes"/>.</param>
        unsafe void AppendUnchecked(void* bytes, int size)
        {
            if (m_NSMutableData.created)
            {
                m_NSMutableData.AppendBytes(bytes, size);
            }
            else
            {
                m_NSMutableData = new NSMutableData(bytes, size);
            }
        }

        /// <summary>
        /// Computes a hash code suitable for use in a <c>Dictionary</c> or <c>HashSet</c>.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode() => m_NSMutableData.GetHashCode();

        /// <summary>
        /// Compares for equality.
        /// </summary>
        /// <param name="obj">The object to compare for equality.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is of type `ARCollaborationDataBuilder` and
        /// compares equal to this instance with <see cref="Equals(ARCollaborationDataBuilder)"/>.
        /// Otherwise, returns <see langword="false"/>.</returns>
        public override bool Equals(object obj) => (obj is ARCollaborationDataBuilder) && Equals((ARCollaborationDataBuilder)obj);

        /// <summary>
        /// Compares for equality.
        /// </summary>
        /// <param name="other">The <see cref="ARCollaborationDataBuilder"/> to compare against.</param>
        /// <returns><see langword="true"/> if all fields of this instance compare equal to <paramref name="other"/>.
        /// Otherwise, returns <see langword="false"/>.</returns>
        public bool Equals(ARCollaborationDataBuilder other) => m_NSMutableData.Equals(other.m_NSMutableData);

        /// <summary>
        /// Compares for equality. Equivalent to <see cref="Equals(ARCollaborationDataBuilder)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><see langword="true"/> if all fields of the instances are equal. Otherwise, returns <see langword="false"/>.</returns>
        public static bool operator ==(ARCollaborationDataBuilder lhs, ARCollaborationDataBuilder rhs) => lhs.Equals(rhs);

        /// <summary>
        /// Compares for inequality. Equivalent to <c>!</c><see cref="Equals(ARCollaborationDataBuilder)"/>.
        /// </summary>
        /// <param name="lhs">The left-hand side of the comparison.</param>
        /// <param name="rhs">The right-hand side of the comparison.</param>
        /// <returns><see langword="true"/> if any of the fields of the instances are not equal. Otherwise, returns <see langword="false"/>.</returns>
        public static bool operator !=(ARCollaborationDataBuilder lhs, ARCollaborationDataBuilder rhs) => !lhs.Equals(rhs);
    }
}
