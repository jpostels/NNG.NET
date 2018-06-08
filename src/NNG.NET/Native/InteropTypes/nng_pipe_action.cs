using System.Diagnostics.CodeAnalysis;

namespace NNG.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [SuppressMessage("ReSharper", "EnumUnderlyingTypeIsInt", Justification = "Being explicit here.")]
    internal enum nng_pipe_ev : int
    {
        /// <summary>
        ///     Called just before pipe added to socket
        /// </summary>
        NNG_PIPE_EV_ADD_PRE,

        /// <summary>
        ///     Called just after pipe added to socket
        /// </summary>
        NNG_PIPE_EV_ADD_POST,

        /// <summary>
        ///     Called just after poipe removed from socket
        /// </summary>
        NNG_PIPE_EV_REM_POST,

        /// <summary>
        ///     Used internally, must be last.
        /// </summary>
        NNG_PIPE_EV_NUM
    }
}