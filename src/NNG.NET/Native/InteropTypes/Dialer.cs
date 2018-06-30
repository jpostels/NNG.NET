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
    public struct Dialer
    {
        /// <summary>
        ///     The identifier
        /// </summary>
        public uint Id;
    }
}