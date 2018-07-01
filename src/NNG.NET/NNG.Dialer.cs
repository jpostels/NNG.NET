using NNGNET.ErrorHandling;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;

namespace NNGNET
{
    public static partial class NNG
    {
        /// <summary>
        ///     Creates a newly initialized <see cref="Dialer"/> object, associated with <paramref name="socket"/>,
        ///     and configured to listen at the <paramref name="address"/>, and starts it. <br/>
        ///     Dialers initiate a remote connection to a listener.
        ///     Upon a successful connection being established, they create a pipe, add it to the socket,
        ///     and then wait for that pipe to be closed. <br/>
        ///     When the pipe is closed, the dialer attempts to re-establish the connection.
        ///     Dialers will also periodically retry a connection automatically
        ///     if an attempt to connect asynchronously fails. <br/>
        ///     Normally, the first attempt to connect to the address is done synchronously,
        ///     including any necessary name resolution. <br/>
        ///     As a result, a failure, such as if the connection is refused, will be returned immediately,
        ///     and no further action will be taken.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="address">The address.</param>
        /// <param name="nonBlocking">if set to <c>true</c> the call is done asynchronously.</param>
        /// <returns>
        ///     A newly initialized <see cref="Dialer"/> object.
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_EADDRINVAL An invalid url was specified.
        ///     -or-
        ///     NNG_ECLOSED The <paramref name="socket"/> is not open.
        ///     -or-
        ///     NNG_ECONNREFUSED The remote peer refused the connection.
        ///     -or-
        ///     NNG_ECONNRESET The remote peer reset the connection.
        ///     -or-
        ///     NNG_EINVAL An invalid set of flags was specified.
        ///     -or-
        ///     NNG_ENOMEM Insufficient memory is available.
        ///     -or-
        ///     NNG_EPEERAUTH Authentication or authorization failure.
        ///     -or-
        ///     NNG_EPROTO A protocol error occurred.
        ///     -or-
        ///     NNG_EUNREACHABLE The remote address is not reachable. 
        /// </exception>
        public static Dialer Dial(NNGSocket socket, string address, bool nonBlocking = false)
        {
            var flags = nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None;
            var err = Interop.Dial(socket, address, out var dialer, flags);
            ThrowHelper.ThrowIfNotSuccess(err);
            return dialer;
        }

        public static Dialer CreateDialer(NNGSocket socket, string address)
        {
            var err = Interop.DialerCreate(out var dialer, socket, address);
            ThrowHelper.ThrowIfNotSuccess(err);
            return dialer;
        }

        public static Dialer StartDialer(Dialer dialer, bool nonBlocking = false)
        {
            var flags = nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None;
            var err = Interop.DialerStart(dialer, flags);
            ThrowHelper.ThrowIfNotSuccess(err);
            return dialer;
        }

        public static Dialer CloseDialer(Dialer dialer)
        {
            var err = Interop.DialerClose(dialer);
            ThrowHelper.ThrowIfNotSuccess(err);
            return dialer;
        }

        public static int GetDialerId(Dialer dialer)
        {
            return Interop.GetDialerId(dialer);
        }

        // TODO Dialer option getter/setter functions
    }
}