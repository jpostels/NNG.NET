using System;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    /// <summary>
    ///     A handle to a "dialer" object, which is responsible for creating a single <see cref="Pipe"/> at a time
    ///     by establishing an outgoing connection. <br/>
    ///     If the connection is broken, or fails, the dialer object will automatically attempt to reconnect,
    ///     and will keep doing so until the dialer or <see cref="NNGSocket"/> is destroyed.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Dialer : IEquatable<Dialer>
    {
        /// <summary>
        ///     The identifier
        /// </summary>
        public uint Id;

        /// <inheritdoc />
        public bool Equals(Dialer other) => Id == other.Id;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is Dialer socket && Equals(socket);
        }

        /// <inheritdoc />
        public override int GetHashCode() => (int)Id;

        public static bool operator ==(Dialer left, Dialer right) => left.Equals(right);

        public static bool operator !=(Dialer left, Dialer right) => !left.Equals(right);
    }
}