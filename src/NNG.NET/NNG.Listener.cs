using NNGNET.ErrorHandling;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;

namespace NNGNET
{
    public static partial class NNG
    {
        /// <summary>
        ///     Creates a newly initialized <see cref="Listener"/> object, associated with the <paramref name="socket"/>,
        ///     and configured to listen at the <paramref name="address"/>, and starts it. <br/>
        ///     Normally, the act of “binding” to the address is done synchronously,
        ///     including any necessary name resolution.
        ///     As a result, a failure, such as if the address is already in use, will be returned immediately. <br/>
        ///     However, if <paramref name="nonBlocking"/> is set to <c>true</c>, then this is done asynchronously;
        ///     furthermore any failure to bind will be periodically reattempted in the background.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="address">The address.</param>
        /// <param name="nonBlocking">if set to <c>true</c> the call is done asynchronously.</param>
        /// <returns>
        ///     A newly initialized <see cref="Listener"/> object.
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_EADDRINUSE The <paramref name="address"/> specified by url is already in use.
        ///     -or-
        ///     NNG_EADDRINVAL An invalid url was specified.
        ///     -or- 
        ///     NNG_ECLOSED The <paramref name="socket"/> is not open.
        ///      -or- 
        ///     NNG_ENOMEM Insufficient memory is available. 
        /// </exception>
        public static Listener Listen(NNGSocket socket, string address, bool nonBlocking = false)
        {
            var flags = nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None;
            var err = Interop.Listen(socket, address, out var listener, flags);
            ThrowHelper.ThrowIfNotSuccess(err);
            return listener;
        }

        public static Listener CreateListener(NNGSocket socket, string address)
        {
            var err = Interop.ListenerCreate(out var dialer, socket, address);
            ThrowHelper.ThrowIfNotSuccess(err);
            return dialer;
        }

        public static Listener StartListenerr(Listener listener, bool nonBlocking = false)
        {
            var flags = nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None;
            var err = Interop.ListenerStart(listener, flags);
            ThrowHelper.ThrowIfNotSuccess(err);
            return listener;
        }

        public static Listener CloseListener(Listener listener)
        {
            var err = Interop.ListenerClose(listener);
            ThrowHelper.ThrowIfNotSuccess(err);
            return listener;
        }

        public static int GetListenerId(Listener listener)
        {
            return Interop.GetListenerId(listener);
        }

        // TODO Listener option getter/setter functions
    }
}