namespace NNGNET
{
    using System;
    using System.Buffers;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    using Native;
    using ErrorHandling;
    using Native.InteropTypes;

    /// <summary>
    ///     Safe provider for P/Invoke of nng library functions.
    ///     Also handles possible error return values and throws them as <see cref="NngException"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "API Provider. All member implicitly used.")]
    public static partial class NNG
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
            ThrowHelper.ThrowIfNotSuccess(err);
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

        /// <summary>
        ///     Registers the <paramref name="callback"/> action to be called
        ///     whenever a pipe fires the event specified by <paramref name="pipeEvent"/>
        ///     on the specified <paramref name="socket"/>.
        ///     Optionally user data may be supplied in <paramref name="args"/>,
        ///     which is then provided to the callback.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="pipeEvent">The pipe event.</param>
        /// <param name="callback">The callback action.</param>
        /// <param name="args">Optional user data</param>
        /// <exception cref="NngException">
        ///     The specified errorCode was something other than <see cref="nng_errno.NNG_SUCCESS"/>. <br/>
        ///     NNG_ECLOSED The <paramref name="socket"/> does not refer to an open socket.
        /// </exception>
        public static unsafe void SetPipeNotification(NNGSocket socket, PipeEvent pipeEvent, PipeCallback callback, IntPtr args = default)
        {
            var err = Interop.PipeSetNotification(socket, pipeEvent, callback, args != default ? args.ToPointer() : IntPtr.Zero.ToPointer());
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        internal static string GetErrorString(nng_errno errorCode)
        {
            return Interop.GetErrorString((int)errorCode);
        }

        public static string GetErrorString(int errorCode)
        {
            return Interop.GetErrorString(errorCode);
        }

        public static bool Send(NNGSocket socket, Span<byte> buffer, bool nonBlocking = false, bool alloc = false)
        {
            var flags = nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None;
            if (alloc)
            {
                flags |= NNGFlag.Alloc;
            }

            nng_errno err;
            unsafe
            {
                fixed (byte* ptr = buffer)
                {
                    err = Interop.Send(socket, ptr, new UIntPtr((uint)buffer.Length), flags);
                }
            }

            switch (err)
            {
                case nng_errno.NNG_SUCCESS:
                    return true;
                case nng_errno.NNG_EAGAIN:
                    return false;
                default:
                    throw ThrowHelper.GetExceptionForErrorCode(err);
            }
        }

        public static uint Receive(NNGSocket socket, Span<byte> targetBuffer)
        {
            var size = new UIntPtr((uint)targetBuffer.Length);

            nng_errno err;
            unsafe
            {
                ref var rp = ref targetBuffer.GetPinnableReference();
                fixed (byte* p = &rp)
                {
                    err = Interop.Receive(socket, p, ref size, NNGFlag.None);
                }
            }

            ThrowHelper.ThrowIfNotSuccess(err);
            return size.ToUInt32();
        }

        public static unsafe Span<byte> Receive(NNGSocket socket)
        {
            void* ptr = null;
            var size = new UIntPtr();

            var err = Interop.Receive(socket, ref ptr, ref size, NNGFlag.Alloc);
            ThrowHelper.ThrowIfNotSuccess(err);

            return new Span<byte>(ptr, (int)size.ToUInt32());
        }

        private sealed unsafe class NanoMem : MemoryManager<byte>
        {
            private readonly void* _handle;

            private readonly int _length;

            public NanoMem(void* pointer, UIntPtr size)
            {
                _handle = pointer;
                _length = (int)size.ToUInt32();
            }

            public NanoMem(void* pointer, int size)
            {
                _handle = pointer;
                _length = size;
            }

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
                NNG.Free(GetSpan());
                //Interop.Free(_handle, _length);
            }

            /// <inheritdoc />
            public override Span<byte> GetSpan()
            {
                return new Span<byte>(_handle, _length);
            }

            /// <inheritdoc />
            public override MemoryHandle Pin(int elementIndex = 0)
            {
                return new MemoryHandle(Unsafe.Add<byte>(_handle, elementIndex));
            }

            /// <inheritdoc />
            public override void Unpin()
            {

            }
        }

        public static unsafe void SendMessage(NNGSocket socket, NNGMessage message)
        {
            var err = Interop.SendMessage(socket, message.MessageHandle, NNGFlag.None);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe bool TrySendMessage(NNGSocket socket, NNGMessage message)
        {
            var err = Interop.SendMessage(socket, message.MessageHandle, NNGFlag.NonBlocking);

            switch (err)
            {
                case nng_errno.NNG_SUCCESS:
                    return true;
                case nng_errno.NNG_EAGAIN:
                    return false;
                default:
                    throw ThrowHelper.GetExceptionForErrorCode(err);
            }
        }

        public static unsafe NNGMessage ReceiveMessage(NNGSocket socket)
        {
            nng_msg* ptr = default;
            var err = Interop.ReceiveMessage(socket, ref ptr, NNGFlag.None);
            ThrowHelper.ThrowIfNotSuccess(err);
            return new NNGMessage(ptr);
        }

        public static unsafe bool TryReceiveMessage(NNGSocket socket, out NNGMessage message, bool nonBlocking = false)
        {
            nng_msg* ptr = default;
            var err = Interop.ReceiveMessage(socket, ref ptr, nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None);

            switch (err)
            {
                case nng_errno.NNG_SUCCESS:
                    message = new NNGMessage(ptr);
                    return true;
                case nng_errno.NNG_EAGAIN:
                    message = default;
                    return false;
                default:
                    throw ThrowHelper.GetExceptionForErrorCode(err);
            }
        }

        public static Span<byte> Alloc(uint size)
        {
            unsafe
            {
                var ptr = Interop.Alloc(new UIntPtr(size));
                return new Span<byte>(ptr, (int)size);
            }
        }

        public static unsafe void Free(Span<byte> buffer)
        {
            fixed (byte* ptr = buffer)
            {
                Interop.Free(ptr, new UIntPtr((uint)buffer.Length));
            }
        }

        public static string DuplicateString(string s) => Interop.StringDuplicate(s);

        [Obsolete("Not supported.", true)]
        public static void FreeString(string s) => throw new NotSupportedException();

        #region Statistics
        // TODO
        // Note: Statistic support is currently not implemented by NNG itself.
        #endregion

        public static void Forward(NNGSocket socket1, NNGSocket socket2)
        {
            var err = Interop.Device(socket1, socket2);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe NNGUrl ParseUrl(string address)
        {
            nng_url* ptr = default;
            try
            {
                var err = Interop.UrlParse(out ptr, address);
                ThrowHelper.ThrowIfNotSuccess(err);
                return new NNGUrl(ptr);
            }
            finally
            {
                if (ptr != default)
                {
                    NNG.FreeUrl(ptr);
                }
            }
        }

        private static unsafe void FreeUrl(nng_url* url) => Interop.UrlFree(url);

        public static unsafe string GetVersionString()
        {
            var ptr = Interop.GetVersionUnsafe();
            return Marshal.PtrToStringAnsi(new IntPtr(ptr));
        }

        private static Version ConstructVersion(string versionString)
        {
            return versionString == null ? null : new Version(versionString);
        }

        private static readonly Lazy<Version> LazyVersion = new Lazy<Version>(() => ConstructVersion(GetVersionString()));

        public static Version Version => LazyVersion.Value;
    }
}
