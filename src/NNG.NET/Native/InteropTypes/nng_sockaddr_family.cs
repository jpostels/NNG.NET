using System.Diagnostics.CodeAnalysis;

namespace NNG.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [SuppressMessage("ReSharper", "EnumUnderlyingTypeIsInt", Justification = "Being explicit here.")]
    public enum nng_sockaddr_family : ushort
    {
        NNG_AF_UNSPEC = 0,

        NNG_AF_INPROC = 1,

        NNG_AF_IPC = 2,

        NNG_AF_INET = 3,

        NNG_AF_INET6 = 4,

        NNG_AF_ZT = 5
    }
}