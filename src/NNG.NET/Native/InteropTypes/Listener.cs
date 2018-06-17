using System;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    /// <summary>
    ///     A handle to a “listener” object,
    ///     which is responsible for creating <see cref="Pipe"/> objects by accepting incoming connections.
    ///     A given listener object may create many pipes at the same time,
    ///     much like an HTTP server can have many connections to multiple clients simultaneously.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Listener : IEquatable<Listener>
    {
        /// <summary>
        ///     The identifier
        /// </summary>
        public uint Id;

        /// <inheritdoc />
        public bool Equals(Listener other) => Id == other.Id;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is Listener socket && Equals(socket);
        }

        /// <inheritdoc />
        public override int GetHashCode() => (int)Id;

        public static bool operator ==(Listener left, Listener right) => left.Equals(right);

        public static bool operator !=(Listener left, Listener right) => !left.Equals(right);
    }
}