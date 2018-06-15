namespace NNGNET
{
    using System;

    using Native;
    using ErrorHandling;
    using Native.InteropTypes;

    public static class NNG
    {
        /// <summary>
        ///     The maximum length of a socket address. This includes the terminating NUL.
        /// </summary>
        /// <remarks>
        ///     This limit is built into other implementations, so do not change it.
        /// </remarks>
        public const int MaxAddressLength = Interop.NngMaxAddressLength;

        /// <summary>
        ///     Gets a value indicating whether the interop functions are initialized.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the interop functions are initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized => Interop.IsInitialized;

        /// <summary>
        ///     Initializes the natvive library functions.
        ///     NOTE: This is not required to be run, but it may reduce initial latency during first calls.
        /// </summary>
        /// <remarks>
        ///     Prelinks all P/Invoke functions.
        ///     This also makes sure all methods are functioning correctly.
        /// </remarks> 
        public static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            Interop.Initialize();
        }

        /// <summary>
        ///     The nng_fini function is used to terminate the library, freeing certain global resources.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This should only be called during atexit() or just before dlclose().
        /// </para>
        /// <para>
        ///     THIS FUNCTION MUST NOT BE CALLED CONCURRENTLY WITH ANY OTHER FUNCTION
        ///     IN THIS LIBRARY; IT IS NOT REENTRANT OR THREADSAFE.
        /// </para>
        /// <para>
        ///     For most cases, this call is unnecessary, but it is provided to assist
        ///     when debugging with memory checkers (e.g. valgrind).  Calling this
        ///     function prevents global library resources from being reported incorrectly
        ///     as memory leaks.  In those cases, we recommend doing this with atexit().
        /// </para>
        /// </remarks>
        public static void Fini()
        {
            Interop.Fini();
        }

        /// <summary>
        ///     The nng_close function closes the supplied socket. <br/>
        ///     Messages that have been submitted for sending may be flushed or delivered, <br/>
        ///     depending upon the transport and the setting of the NNG_OPT_LINGER option. <br/>
        ///     Further attempts to use the socket after this call returns will result in <see cref="nng_errno.NNG_ECLOSED"/>. <br/>
        ///     Threads waiting for operations on the socket when this call is executed
        ///     may also return with an <see cref="nng_errno.NNG_ECLOSED"/> result.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <exception cref="NngException">
        ///     <see cref="nng_errno.NNG_ECLOSED"/>: The socket is already closed or was never opened.
        /// </exception>
        public static void Close(NNGSocket socket)
        {
            var err = Interop.Close(socket);
            if (err != nng_errno.NNG_SUCCESS)
            {
                throw ThrowHelper.GetExceptionForErrorCode(err);
            }
        }

        /// <summary>
        ///     Gets the socket identifier.
        /// </summary>
        /// <param name="socket">The socket identifier.</param>
        /// <returns>
        ///     Returns the positive value for the socket identifier.
        /// </returns>
        /// <exception cref="NngException">"The given socket is invalid."</exception>
        public static int GetSocketId(NNGSocket socket)
        {
            var val = Interop.GetSocketId(socket);
            return val >= 0 ? val : throw ThrowHelper.GetExceptionForErrorCode(nng_errno.NNG_EINVAL, "The given socket is invalid. ");
        }

        /// <summary>
        ///     Closes all open sockets. 
        /// </summary>
        /// <remarks>
        ///     Do not call this from a library; it will affect all sockets.
        /// </remarks>
        public static void CloseAll() => Interop.CloseAll();
    }
}
