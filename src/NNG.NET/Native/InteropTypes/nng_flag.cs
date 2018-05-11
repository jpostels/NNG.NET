using System.Diagnostics.CodeAnalysis;

namespace NNG.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    public enum nng_flag : int
    {
        /// <summary>
        ///     No flags
        /// </summary>
        NONE = 0,

        /// <summary>
        ///     Recv to allocate receive buffer
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
        NNG_FLAG_ALLOC = 1,

        /// <summary>
        ///     Non-blocking operations
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
        NNG_FLAG_NONBLOCK = 2
    }
}