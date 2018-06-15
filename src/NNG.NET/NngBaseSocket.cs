using System;
using NNGNET.ErrorHandling;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;

namespace NNGNET
{
    /// <summary>
    ///     Base class for shared functionality between protocols.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class NngBaseSocket : IDisposable
    {
        /// <summary>
        ///     The socket creation lock
        /// </summary>
        private static readonly object SocketCreationLock = new object();

        /// <summary>
        ///     Initializes a new instance of the <see cref="NngBaseSocket"/> class.
        /// </summary>
        /// <param name="openFunction">The open function.</param>
        internal NngBaseSocket(SocketOpenFunction openFunction)
        {
            Open(openFunction);
        }

        /// <summary>
        ///     Gets or sets the socket.
        /// </summary>
        /// <value>
        ///     The socket.
        /// </value>
        internal NNGSocket Socket { get; set; }

        /// <summary>
        ///     A socket opener function e.g. <see cref="Interop.nng_req0_open"/>.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <returns></returns>
        internal delegate nng_errno SocketOpenFunction(out NNGSocket socket);

        /// <summary>
        ///     Opens a socket with the specified <paramref name="openFunction"/>
        /// </summary>
        /// <param name="openFunction">The open function.</param>
        /// <exception cref="NngException">
        ///     Failed to call socket open function. See inner exception for more details.
        ///     -or-
        ///     Failed to open socket.
        /// </exception>
        internal void Open(SocketOpenFunction openFunction)
        {
            nng_errno result;
            NNGSocket socket;
            try
            {
                lock (SocketCreationLock)
                {
                    result = openFunction(out socket);
                }
            }
            catch (Exception exception)
            {
                throw new NngException("Failed to call socket open function. See inner exception for more details. ", exception);
            }

            if (result == nng_errno.NNG_SUCCESS)
            {
                Socket = socket;
            }
            else
            {
                throw ThrowHelper.GetExceptionForErrorCode(result);
            }
        }

        /// <summary>
        ///     Closes the underlying socket.
        /// </summary>
        internal nng_errno Close()
        {
            return Interop.Close(Socket);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources;
        ///     <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="NngBaseSocket"/> class.
        /// </summary>
        ~NngBaseSocket()
        {
            Dispose(false);
        }
    }
}
