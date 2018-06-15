using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [StructLayout(LayoutKind.Explicit)]
    public struct nng_sockaddr
    {
        [FieldOffset(0)]
        public nng_sockaddr_family s_family;

        [FieldOffset(0)]
        public nng_sockaddr_path s_inproc;

        [FieldOffset(0)]
        public nng_sockaddr_in6 s_in6;

        [FieldOffset(0)]
        public nng_sockaddr_in s_in;

        [FieldOffset(0)]
        public nng_sockaddr_zt s_zt;
    }
}