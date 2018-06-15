using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    /// <summary>
    ///     TODO Insert doc
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct nng_sockaddr_in6
    {
        public nng_sockaddr_family sa_family;

        public ushort sa_port;

        public fixed byte sa_addr[16];
    }
}
