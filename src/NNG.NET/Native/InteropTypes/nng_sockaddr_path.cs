using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNG.Native.InteropTypes
{
    /// <summary>
    ///     This is in some ways like a traditional sockets
    ///     sockaddr, but we have our own to cope with our unique families, etc.
    ///     The details of this structure are directly exposed to applications.
    ///     These structures can be obtained via property lookups, etc.
    /// </summary>
    /// <remarks>
    ///     Known typedef aliases: nng_sockaddr_ipc, nng_sockaddr_inproc
    /// </remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct nng_sockaddr_path
    {
        public nng_sockaddr_family sa_family;

        public fixed char sa_path[128]; // 128: Maximum length of a socket address. This includes the terminating NUL. This limit is built into other implementations, so do not change it.
    }
}