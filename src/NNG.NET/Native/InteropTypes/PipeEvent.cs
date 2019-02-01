using System.Diagnostics.CodeAnalysis;

namespace NNGNET.Native.InteropTypes
{
    /// <summary>
    ///     Determines the type of pipe notification event.
    /// </summary>
    /// <remarks>
    ///     Original name: nng_pipe_ev
    /// </remarks>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [SuppressMessage("ReSharper", "EnumUnderlyingTypeIsInt", Justification = "Being explicit here.")]
    public enum PipeEvent : int
    {
        /// <summary>
        ///     Called just before pipe added to socket.
        ///     This event occurs after a connection and negotiation has completed,
        ///     but before the pipe is added to the socket.
        ///     If the pipe is closed at this point, the socket will never see the pipe,
        ///     and no further events will occur for the given pipe.
        /// </summary>
        Adding,

        /// <summary>
        ///     Called just after pipe added to socket.
        ///     This event occurs after the pipe is fully added to the socket.
        ///     Prior to this time, it is not possible to communicate over the pipe with the socket.
        /// </summary>
        Added,

        /// <summary>
        ///     Called just after pipe removed from socket.
        ///     This event occurs after the pipe has been removed from the socket.
        ///     The underlying transport may be closed at this point,
        ///     and it is not possible communicate using this pipe.
        /// </summary>
        Removed,

        /// <summary>
        ///     Used internally, must be last.
        /// </summary>
        NumberOfEvents
    }
}