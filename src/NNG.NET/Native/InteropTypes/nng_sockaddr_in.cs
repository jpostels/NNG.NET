using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNG.Native.InteropTypes
{
    /// <summary>
    ///     TODO Insert doc
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [StructLayout(LayoutKind.Sequential)]
    public struct nng_sockaddr_in
    {
        public nng_sockaddr_family sa_family;

        public ushort sa_port;

        public uint sa_addr;
    }
}