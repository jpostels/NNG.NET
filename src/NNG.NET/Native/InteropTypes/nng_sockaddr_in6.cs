using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace NNG.Native.InteropTypes
{
    /// <summary>
    ///     TODO Insert doc
    /// </summary>
    /// <remarks>
    ///     Known typedef aliases: nng_sockaddr_udp6, nng_sockaddr_tcp6
    /// </remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct nng_sockaddr_in6
    {
        public nng_sockaddr_family sa_family;

        public ushort sa_port;

        public fixed byte sa_addr[16];
    }
}
