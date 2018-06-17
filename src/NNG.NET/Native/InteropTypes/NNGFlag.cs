namespace NNGNET.Native.InteropTypes
{
    /// <summary>
    ///     Provides flags for various interop calls.
    /// </summary>
    internal enum NNGFlag
    {
        /// <summary>
        ///     No flags
        /// </summary>
        None = 0,

        /// <summary>
        ///     Recv to allocate receive buffer
        /// </summary>
        Alloc = 1,

        /// <summary>
        ///     Non-blocking operations
        /// </summary>
        NonBlocking = 2
    }
}