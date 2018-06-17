using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    /// <summary>
    ///     An NNGPipe is a handle to a "pipe", which can be thought of as a single connection. 
    ///     (In most cases this is actually the case — the pipe is an abstraction for a single TCP or IPC connection.) 
    ///     Pipes are associated with either the listener or dialer that created them, 
    ///     and therefore are also automatically associated with a single socket. 
    ///     Pipe objects are created by dialers (<see cref="Dialer"/> objects) and listeners (<see cref="Listener"/> objects), 
    ///     which can be thought of as "owning" the pipe. 
    ///     Pipe objects may be destroyed by the <see cref="Interop.PipeClose"/> function. 
    ///     They are also closed when their "owning" dialer or listener is closed, 
    ///     or when the remote peer closes the underlying connection. 
    /// </summary>
    /// <remarks>
    ///     Original name: nng_pipe
    /// </remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    public struct Pipe : IEquatable<Pipe>
    {
        /// <summary>
        ///     The identifier
        /// </summary>
        public uint Id;

        /// <inheritdoc />
        public bool Equals(Pipe other) => Id == other.Id;

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is Pipe socket && Equals(socket);
        }

        /// <inheritdoc />
        public override int GetHashCode() => (int)Id;

        public static bool operator ==(Pipe left, Pipe right) => left.Equals(right);

        public static bool operator !=(Pipe left, Pipe right) => !left.Equals(right);
    }
}