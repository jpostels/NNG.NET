using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    /// <summary>
    ///     An <see cref="NNGSocket"/> is a handle to an underlying “socket” object.
    ///     All communication between the application and remote Scalability Protocol peers is done through sockets. 
    ///     A given socket can have multiple dialers (nng_dialer) and/or listeners (nng_listener),
    ///     and multiple pipes (nng_pipe), and may be connected to multiple transports at the same time.
    ///     However, a given socket will have exactly one “protocol” associated with it,
    ///     and is responsible for any state machines or other protocol-specific logic.
    /// </summary>
    /// <remarks>
    ///     Original name: nng_socket
    /// </remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    public struct NNGSocket : IEquatable<NNGSocket>
    {
        /// <summary>
        ///     The identifier
        /// </summary>
        public uint Id;

        /// <inheritdoc />
        public bool Equals(NNGSocket other) => Id == other.Id;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is NNGSocket socket && Equals(socket);
        }

        /// <inheritdoc />
        public override int GetHashCode() => (int)Id;

        public static bool operator ==(NNGSocket left, NNGSocket right) => left.Equals(right);

        public static bool operator !=(NNGSocket left, NNGSocket right) => !left.Equals(right);
    }
}