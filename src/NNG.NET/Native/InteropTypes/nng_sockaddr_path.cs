using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    /// <summary>
    ///     This is in some ways like a traditional sockets
    ///     sockaddr, but we have our own to cope with our unique families, etc.
    ///     The details of this structure are directly exposed to applications.
    ///     These structures can be obtained via property lookups, etc.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct nng_sockaddr_path
    {
        public nng_sockaddr_family sa_family;

        public fixed char sa_path[Interop.NngMaxAddressLength];
    }
}