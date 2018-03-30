using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNG.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [StructLayout(LayoutKind.Sequential)]
    public struct nng_sockaddr_zt
    {
        public nng_sockaddr_family sa_family;

        public ulong sa_nwid;

        public ulong sa_nodeid;

        public uint sa_port;
    }
}