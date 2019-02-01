using System.Diagnostics.CodeAnalysis;

namespace NNGNET.Native.InteropTypes
{
    /// <summary>
    ///     Error codes.  These generally have different values from UNIX errnos,
    ///     so take care about converting them.  The one exception is that 0 is
    ///     unambigiously "success".
    /// </summary>
    /// <remarks>
    ///     NNG_SYSERR is a special code, which allows us to wrap errors from the
    ///     underlying operating system. We generally prefer to map errors to one
    ///     of the above, but if we cannot, then we just encode an error this way.
    ///     The bit is large enough to accommodate all known UNIX and Win32 error
    ///     codes. We try hard to match things semantically to one of our standard
    ///     errors. For example, a connection reset or aborted we treat as a
    ///     closed connection, because that's basically what it means. (The remote
    ///     peer closed the connection.) For certain kinds of resource exhaustion
    ///     we treat it the same as memory. But for files, etc. that's OS-specific,
    ///     and we use the generic below. Some of the above error codes we use
    ///     internally, and the application should never see (e.g. NNG_EINTR). <br />
    ///     NNG_ETRANERR is like ESYSERR, but is used to wrap transport specific
    ///     errors, from different transports. It should only be used when none
    ///     of the other options are available.
    /// </remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [SuppressMessage("ReSharper", "EnumUnderlyingTypeIsInt", Justification = "Being explicit here.")]
    internal enum nng_errno : int
    {
        NNG_SUCCESS = 0,
        NNG_EINTR = 1,
        NNG_ENOMEM = 2,
        NNG_EINVAL = 3,
        NNG_EBUSY = 4,
        NNG_ETIMEDOUT = 5,
        NNG_ECONNREFUSED = 6,
        NNG_ECLOSED = 7,
        NNG_EAGAIN = 8,
        NNG_ENOTSUP = 9,
        NNG_EADDRINUSE = 10,
        NNG_ESTATE = 11,
        NNG_ENOENT = 12,
        NNG_EPROTO = 13,
        NNG_EUNREACHABLE = 14,
        NNG_EADDRINVAL = 15,
        NNG_EPERM = 16,
        NNG_EMSGSIZE = 17,
        NNG_ECONNABORTED = 18,
        NNG_ECONNRESET = 19,
        NNG_ECANCELED = 20,
        NNG_ENOFILES = 21,
        NNG_ENOSPC = 22,
        NNG_EEXIST = 23,
        NNG_EREADONLY = 24,
        NNG_EWRITEONLY = 25,
        NNG_ECRYPTO = 26,
        NNG_EPEERAUTH = 27,
        NNG_ENOARG = 28,
        NNG_EAMBIGUOUS = 29,
        NNG_EBADTYPE = 30,
        NNG_EINTERNAL = 1000,
        NNG_ESYSERR = 0x10000000,
        NNG_ETRANERR = 0x20000000
    }
}