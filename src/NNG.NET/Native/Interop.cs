namespace NNGNET.Native
{
    using System;
    using System.Runtime.InteropServices;

    using InteropTypes;

    using Utils.Linux;
    using Utils.Windows;

    using nng_duration = System.Int32;

    /// <summary>
    ///     Provider for P/Invoke of nng library functions
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle", Justification = "Using native names")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Supressed")]
    internal static class Interop
    {
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments

        /// <summary>
        ///     The librarys name
        /// </summary>
        public const string LibraryName = "nng";

        /// <summary>
        ///     Gets a value indicating whether the interop functions are initialized.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the interop functions are initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        ///     The initialization lock
        /// </summary>
        private static readonly object InitLock = new object();

        /// <summary>
        ///     Initializes the interop functions.
        /// </summary>
        /// <remarks>
        ///     Prelinks all P/Invoke functions.
        ///     This makes sure all methods are functioning correctly.
        /// </remarks> 
        /// <exception cref="NotSupportedException">
        ///     NNG.NET does not support this Operating System.
        /// </exception>
        /// <exception cref="NngException">
        ///     Failed to load posix library. See inner exception for more details.
        /// </exception>
        public static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            lock (InitLock)
            {
                if (IsInitialized)
                {
                    return;
                }

                SetLibraryPath();

                // Prelink all P/Invoke functions.
                // This makes sure all methods are functioning correctly
                // NOTE: Does not actually invoke a function call
                Marshal.PrelinkAll(typeof(Interop));
                IsInitialized = true;
            }
        }

        /// <summary>
        ///     Sets the library path.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///     NNG.NET does not support this Operating System.
        /// </exception>
        /// <exception cref="NngException">
        ///     Failed to load posix library. See inner exception for more details.
        /// </exception>
        private static void SetLibraryPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Just set the dll directory. The automatic marshalling of the DllImportAtribute will do the rest.
                Kernel32.SetDllDirectory(WindowsLibraryLoader.GetWindowsLibraryPath());
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Loading it once explicitly will avoid implicit loading via the DllImportAttribute,
                // which would likely use the wrong search path.
                try
                {
                    UnixLibraryLoader.LoadPosixLibrary(LibraryName);
                }
                catch (LibraryLoadException libraryLoadException)
                {
                    throw new NngException("Failed to load posix library. See inner exception for more details. ", libraryLoadException);
                }
            }
            else
            {
                throw new NotSupportedException("NNG.NET does not support this Operating System.");
            }
        }

        /// <summary>
        ///     The maximum length of a socket address. This includes the terminating NUL.
        /// </summary>
        /// <remarks>
        ///     This limit is built into other implementations, so do not change it.
        /// </remarks>
        public const int NngMaxAddressLength = 128;

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
        [DllImport(LibraryName, EntryPoint = "nng_fini")]
        public static extern void nng_fini();

        /// <summary>
        ///     The nng_close function closes the supplied socket.
        ///     Messages that have been submitted for sending may be flushed or delivered,
        ///     depending upon the transport and the setting of the NNG_OPT_LINGER option.
        ///     Further attempts to use the socket after this call returns will result in <see cref="nng_errno.NNG_ECLOSED"/>.
        ///     Threads waiting for operations on the socket when this call is executed
        ///     may also return with an <see cref="nng_errno.NNG_ECLOSED"/> result.
        /// </summary>
        /// <param name="socketId">The socket.</param>
        /// <returns>
        ///     This function returns 0 on success, and non-zero otherwise.
        ///     <see cref="nng_errno.NNG_ECLOSED"/>: The socket is already closed or was never opened.
        /// </returns>
        [DllImport(LibraryName, EntryPoint = "nng_close")]
        public static extern nng_errno nng_close(nng_socket socketId);

        [DllImport(LibraryName, EntryPoint = "nng_socket_id")]
        public static extern int nng_socket_id(nng_socket socketId);

        [DllImport(LibraryName, EntryPoint = "nng_closeall")]
        public static extern void nng_closeall();

#region nng setopt

        [DllImport(LibraryName, EntryPoint = "nng_setopt")]
        public static extern unsafe nng_errno nng_setopt(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* value, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_bool")]
        public static extern nng_errno nng_setopt_bool(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_int")]
        public static extern nng_errno nng_setopt_int(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_size")]
        public static extern nng_errno nng_setopt_size(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_ms")]
        public static extern nng_errno nng_setopt_ms(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_uint64")]
        public static extern nng_errno nng_setopt_uint64(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_string")]
        public static extern nng_errno nng_setopt_string(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_ptr")]
        public static extern unsafe nng_errno nng_setopt_ptr(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* ptr);

#endregion

#region nng getopt

        [DllImport(LibraryName, EntryPoint = "nng_getopt")]
        public static extern unsafe nng_errno nng_getopt(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* value, out UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_bool")]
        public static extern nng_errno nng_getopt_bool(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_int")]
        public static extern nng_errno nng_getopt_int(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_ms")]
        public static extern nng_errno nng_getopt_ms(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_size")]
        public static extern nng_errno nng_getopt_size(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_uint64")]
        public static extern nng_errno nng_getopt_uint64(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_ptr")]
        public static extern nng_errno nng_getopt_ptr(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_string")]
        public static extern nng_errno nng_getopt_string(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string value);

#endregion

        [DllImport(LibraryName, EntryPoint = "nng_pipe_notify")]
        public static extern unsafe nng_errno nng_pipe_notify(nng_socket socketId, [MarshalAs(UnmanagedType.I4)] nng_pipe_ev ev, [MarshalAs(UnmanagedType.FunctionPtr)] nng_pipe_cb callback, void* args);

        [DllImport(LibraryName, EntryPoint = "nng_listen")]
        public static extern nng_errno nng_listen(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string addr, out nng_listener listener, [MarshalAs(UnmanagedType.I4)] nng_flag flags);

        [DllImport(LibraryName, EntryPoint = "nng_dial")]
        public static extern nng_errno nng_dial(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string addr, out nng_dialer listener, [MarshalAs(UnmanagedType.I4)] nng_flag flags);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_create")]
        public static extern nng_errno nng_dialer_create([Out, In] ref nng_dialer dialer, nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string addr);

        [DllImport(LibraryName, EntryPoint = "nng_listener_create")]
        public static extern nng_errno nng_listener_create([Out, In] ref nng_listener listener, nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string addr);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_start")]
        public static extern nng_errno nng_dialer_start(nng_dialer dialer, int flags);

        [DllImport(LibraryName, EntryPoint = "nng_listener_start")]
        public static extern nng_errno nng_listener_start(nng_listener listener, int flags);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_close")]
        public static extern nng_errno nng_dialer_close(nng_dialer dialer);

        [DllImport(LibraryName, EntryPoint = "nng_listener_close")]
        public static extern nng_errno nng_listener_close(nng_listener listener);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_id")]
        public static extern int nng_dialer_id(nng_dialer listener);

        [DllImport(LibraryName, EntryPoint = "nng_listener_id")]
        public static extern int nng_listener_id(nng_listener listener);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt")]
        public static extern nng_errno nng_dialer_setopt(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, IntPtr value, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_bool")]
        public static extern nng_errno nng_dialer_setopt_bool(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_int")]
        public static extern nng_errno nng_dialer_setopt_int(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_ms")]
        public static extern nng_errno nng_dialer_setopt_ms(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_size")]
        public static extern nng_errno nng_dialer_setopt_size(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_uint64")]
        public static extern nng_errno nng_dialer_setopt_uint64(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_ptr")]
        public static extern nng_errno nng_dialer_setopt_ptr(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, IntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_string")]
        public static extern nng_errno nng_dialer_setopt_string(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt")]
        public static extern nng_errno nng_dialer_getopt(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value, out UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_bool")]
        public static extern nng_errno nng_dialer_getopt_bool(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_int")]
        public static extern nng_errno nng_dialer_getopt_int(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_ms")]
        public static extern nng_errno nng_dialer_getopt_ms(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_size")]
        public static extern nng_errno nng_dialer_getopt_size(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_sockaddr")]
        public static extern nng_errno nng_dialer_getopt_sockaddr(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_sockaddr value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_uint64")]
        public static extern nng_errno nng_dialer_getopt_uint64(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_ptr")]
        public static extern nng_errno nng_dialer_getopt_ptr(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_string")]
        public static extern nng_errno nng_dialer_getopt_string(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt")]
        public static extern nng_errno nng_listener_setopt(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, IntPtr value, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_bool")]
        public static extern nng_errno nng_listener_setopt_bool(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_int")]
        public static extern nng_errno nng_listener_setopt_int(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_ms")]
        public static extern nng_errno nng_listener_setopt_ms(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_size")]
        public static extern nng_errno nng_listener_setopt_size(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_uint64")]
        public static extern nng_errno nng_listener_setopt_uint64(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_ptr")]
        public static extern nng_errno nng_listener_setopt_ptr(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, IntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_string")]
        public static extern nng_errno nng_listener_setopt_string(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt")]
        public static extern nng_errno nng_listener_getopt(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value, out UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_bool")]
        public static extern nng_errno nng_listener_getopt_bool(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_int")]
        public static extern nng_errno nng_listener_getopt_int(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_ms")]
        public static extern nng_errno nng_listener_getopt_ms(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_size")]
        public static extern nng_errno nng_listener_getopt_size(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_sockaddr")]
        public static extern nng_errno nng_listener_getopt_sockaddr(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_sockaddr value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_uint64")]
        public static extern nng_errno nng_listener_getopt_uint64(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_ptr")]
        public static extern nng_errno nng_listener_getopt_ptr(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_string")]
        public static extern nng_errno nng_listener_getopt_string(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string value);

        [DllImport(LibraryName, EntryPoint = "nng_strerror", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static extern string nng_strerror(int errorCode);

        [DllImport(LibraryName, EntryPoint = "nng_send")]
        public static extern nng_errno nng_send(nng_socket socketId, IntPtr ptr, UIntPtr size, [MarshalAs(UnmanagedType.I4)] nng_flag flags);

        [DllImport(LibraryName, EntryPoint = "nng_recv")]
        public static extern nng_errno nng_recv(nng_socket socketId, [Out, In] ref IntPtr ptr, [Out, In] ref UIntPtr size, [MarshalAs(UnmanagedType.I4)] nng_flag flags);

        [DllImport(LibraryName, EntryPoint = "nng_sendmsg")]
        public static extern nng_errno nng_sendmsg(nng_socket socketId, ref nng_msg message, int flags);

        [DllImport(LibraryName, EntryPoint = "nng_recvmsg")]
        public static extern unsafe nng_errno nng_recvmsg(nng_socket socketId, ref nng_msg* message, int flags);

        [DllImport(LibraryName, EntryPoint = "nng_send_aio")]
        public static extern void nng_send_aio(nng_socket socketId, ref nng_aio aio);

        [DllImport(LibraryName, EntryPoint = "nng_recv_aio")]
        public static extern void nng_recv_aio(nng_socket socketId, ref nng_aio aio);

#region Context support

        [DllImport(LibraryName, EntryPoint = "nng_ctx_open")]
        public static extern nng_errno nng_ctx_open([Out, In] ref nng_ctx ctx, nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_close")]
        public static extern nng_errno nng_ctx_close(nng_ctx ctx);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_id")]
        public static extern int nng_ctx_id(nng_ctx ctx);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_recv")]
        public static extern void nng_ctx_recv(nng_ctx ctx, ref nng_aio aio);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_send")]
        public static extern void nng_ctx_send(nng_ctx ctx, ref nng_aio aio);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_getopt")]
        public static extern nng_errno nng_ctx_getopt(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value, out UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_getopt_bool")]
        public static extern nng_errno nng_ctx_getopt_bool(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_getopt_int")]
        public static extern nng_errno nng_ctx_getopt_int(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_getopt_ms")]
        public static extern nng_errno nng_ctx_getopt_ms(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration duration);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_getopt_size")]
        public static extern nng_errno nng_ctx_getopt_size(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_setopt")]
        public static extern nng_errno nng_ctx_setopt(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref IntPtr value, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_setopt_bool")]
        public static extern nng_errno nng_ctx_setopt_bool(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_setopt_int")]
        public static extern nng_errno nng_ctx_setopt_int(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_setopt_ms")]
        public static extern nng_errno nng_ctx_setopt_ms(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration duration);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_setopt_size")]
        public static extern nng_errno nng_ctx_setopt_size(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

#endregion

        [DllImport(LibraryName, EntryPoint = "nng_alloc")]
        public static extern IntPtr nng_alloc(UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_free")]
        public static extern void nng_free(IntPtr ptr, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_strdup")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string nng_strdup([MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImport(LibraryName, EntryPoint = "nng_strfree")]
        public static extern void nng_strfree([MarshalAs(UnmanagedType.LPStr)] string str);

#region AIO

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void AioAllocCallback(void* ptr);

        [DllImport(LibraryName, EntryPoint = "nng_aio_alloc")]
        public static extern unsafe nng_errno nng_aio_alloc([Out, In] ref nng_aio* aio, [MarshalAs(UnmanagedType.FunctionPtr)] AioAllocCallback completionCallback, void* args);

        [DllImport(LibraryName, EntryPoint = "nng_aio_free")]
        public static extern void nng_aio_free(ref nng_aio aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_stop")]
        public static extern void nng_aio_stop(ref nng_aio aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_result")]
        public static extern nng_errno nng_aio_result(ref nng_aio aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_count")]
        public static extern UIntPtr nng_aio_count(ref nng_aio aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_cancel")]
        public static extern void nng_aio_cancel(ref nng_aio aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_abort")]
        public static extern void nng_aio_abort(ref nng_aio aio, int i);

        [DllImport(LibraryName, EntryPoint = "nng_aio_wait")]
        public static extern void nng_aio_wait(ref nng_aio aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_set_msg")]
        public static extern void nng_aio_set_msg(ref nng_aio aio, ref nng_msg msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg")]
        public static extern ref nng_msg nng_aio_get_msg(ref nng_aio aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_set_input")]
        public static extern unsafe nng_errno nng_aio_set_input(ref nng_aio aio, uint index, void* arg);

        [DllImport(LibraryName, EntryPoint = "nng_aio_get_input")]
        public static extern unsafe void* nng_aio_get_input(ref nng_aio aio, uint index);

        [DllImport(LibraryName, EntryPoint = "nng_aio_set_output")]
        public static extern unsafe nng_errno nng_aio_set_output(ref nng_aio aio, uint index, void* arg);

        [DllImport(LibraryName, EntryPoint = "nng_aio_get_output")]
        public static extern unsafe void* nng_aio_get_output(ref nng_aio aio, uint index);

        [DllImport(LibraryName, EntryPoint = "nng_aio_set_timeout")]
        public static extern void nng_aio_set_timeout(ref nng_aio aio, nng_duration timeout);

        [DllImport(LibraryName, EntryPoint = "nng_aio_set_iov")]
        public static extern nng_errno nng_aio_set_iov(ref nng_aio aio, uint niov, in nng_iov iov);

        [DllImport(LibraryName, EntryPoint = "nng_aio_finish")]
        public static extern void nng_aio_finish(ref nng_aio aio, int rv);

        [DllImport(LibraryName, EntryPoint = "nng_sleep_aio")]
        public static extern void nng_sleep_aio(nng_duration duration, ref nng_aio aio);

#endregion AIO

#region Message API

        [DllImport(LibraryName, EntryPoint = "nng_msg_alloc")]
        public static extern unsafe nng_errno nng_msg_alloc([Out, In] ref nng_msg* msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_free")]
        public static extern void nng_msg_free(ref nng_msg msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_realloc")]
        public static extern nng_errno nng_msg_realloc(ref nng_msg msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header")]
        public static extern unsafe void* nng_msg_header(ref nng_msg msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_len")]
        public static extern UIntPtr nng_msg_header_len(in nng_msg msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_body")]
        public static extern unsafe void* nng_msg_body(ref nng_msg msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_len")]
        public static extern UIntPtr nng_msg_len(in nng_msg msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_append")]
        public static extern unsafe nng_errno nng_msg_append(ref nng_msg msg, void* data, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_insert")]
        public static extern unsafe nng_errno nng_msg_insert(ref nng_msg msg, void* data, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_trim")]
        public static extern nng_errno nng_msg_trim(ref nng_msg msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_chop")]
        public static extern nng_errno nng_msg_chop(ref nng_msg msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_append")]
        public static extern unsafe nng_errno nng_msg_header_append(ref nng_msg msg, void* data, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_insert")]
        public static extern unsafe nng_errno nng_msg_header_insert(ref nng_msg msg, void* data, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_trim")]
        public static extern nng_errno nng_msg_header_trim(ref nng_msg msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_chop")]
        public static extern nng_errno nng_msg_header_chop(ref nng_msg msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_append_u32")]
        public static extern nng_errno nng_msg_header_append_u32(ref nng_msg msg, uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_insert_u32")]
        public static extern nng_errno nng_msg_header_insert_u32(ref nng_msg msg, uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_chop_u32")]
        public static extern nng_errno nng_msg_header_chop_u32(ref nng_msg msg, ref uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_trim_u32")]
        public static extern nng_errno nng_msg_header_trim_u32(ref nng_msg msg, ref uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_append_u32")]
        public static extern nng_errno nng_msg_append_u32(ref nng_msg msg, uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_insert_u32")]
        public static extern nng_errno nng_msg_insert_u32(ref nng_msg msg, uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_chop_u32")]
        public static extern nng_errno nng_msg_chop_u32(ref nng_msg msg, ref uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_trim_u32")]
        public static extern nng_errno nng_msg_trim_u32(ref nng_msg msg, ref uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_dup")]
        public static extern unsafe nng_errno nng_msg_dup(ref nng_msg* msgDuplicate, ref nng_msg msgSource);

        [DllImport(LibraryName, EntryPoint = "nng_msg_clear")]
        public static extern void nng_msg_clear(ref nng_msg msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_clear")]
        public static extern void nng_msg_header_clear(ref nng_msg msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_set_pipe")]
        public static extern void nng_msg_set_pipe(ref nng_msg msg, nng_pipe pipe);

        [DllImport(LibraryName, EntryPoint = "nng_msg_get_pipe")]
        public static extern nng_pipe nng_msg_get_pipe(ref nng_msg msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_getopt")]
        public static extern unsafe nng_errno nng_msg_getopt(ref nng_msg msg, int opt, void* ptr, ref UIntPtr size);

#endregion

#region Pipe API

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt")]
        public static extern unsafe nng_errno nng_pipe_getopt(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* ptr, out UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_bool")]
        public static extern nng_errno nng_pipe_getopt_bool(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_int")]
        public static extern nng_errno nng_pipe_getopt_int(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_ms")]
        public static extern nng_errno nng_pipe_getopt_ms(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration duration);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_size")]
        public static extern nng_errno nng_pipe_getopt_size(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_sockaddr")]
        public static extern nng_errno nng_pipe_getopt_sockaddr(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_sockaddr val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_uint64")]
        public static extern nng_errno nng_pipe_getopt_uint64(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_ptr")]
        public static extern unsafe nng_errno nng_pipe_getopt_ptr(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_string")]
        public static extern nng_errno nng_pipe_getopt_string(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_close")]
        public static extern nng_errno nng_pipe_close(nng_pipe pipe);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_id")]
        public static extern int nng_pipe_id(nng_pipe pipe);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_socket")]
        public static extern nng_socket nng_pipe_socket(nng_pipe pipe);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_dialer")]
        public static extern nng_dialer nng_pipe_dialer(nng_pipe pipe);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_listener")]
        public static extern nng_listener nng_pipe_listener(nng_pipe pipe);

#endregion

#region Statistics

        // Statistics.  These are for informational purposes only, and subject
        // to change without notice.  The API for accessing these is stable,
        // but the individual statistic names, values, and meanings are all
        // subject to change.

        //BUG the following methods are not yet implemented by nng

        //[DllImport(LibraryName)]
        //public static extern unsafe nng_errno nng_snapshot_create(nng_socket socket, ref nng_snapshot* snapshot);

        //[DllImport(LibraryName)]
        //public static extern void nng_snapshot_free(ref nng_snapshot snapshot);

        //[DllImport(LibraryName)]
        //public static extern nng_errno nng_snapshot_update(ref nng_snapshot snapshot);

        //[DllImport(LibraryName)]
        //public static extern unsafe nng_errno nng_snapshot_next(ref nng_snapshot snapshot, ref nng_stat* stat);

        //[DllImport(LibraryName)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public static extern string nng_stat_name(ref nng_stat stat);

        //[DllImport(LibraryName)]
        //[return: MarshalAs(UnmanagedType.I4)]
        //public static extern nng_stat_type_enum nng_stat_type(ref nng_stat stat);

        //[DllImport(LibraryName)]
        //[return: MarshalAs(UnmanagedType.I4)]
        //public static extern nng_unit_enum nng_stat_unit(ref nng_stat stat);

        //[DllImport(LibraryName)]
        //public static extern long nng_stat_value(ref nng_stat stat);

#endregion

        [DllImport(LibraryName, EntryPoint = "nng_device")]
        public static extern nng_errno nng_device(nng_socket socket1, nng_socket socket2);

#region URL support

        [DllImport(LibraryName, EntryPoint = "nng_url_parse")]
        public static extern unsafe nng_errno nng_url_parse(out nng_url* url, [MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImport(LibraryName, EntryPoint = "nng_url_free")]
        public static extern void nng_url_free(ref nng_url url);

        [DllImport(LibraryName, EntryPoint = "nng_url_clone")]
        public static extern unsafe nng_errno nng_url_clone(out nng_url* urlDuplicate, ref nng_url urlSource);

#endregion

        /// <summary>
        ///     Report library version
        /// </summary>
        /// <returns></returns>
        [DllImport(LibraryName, EntryPoint = "nng_version")]
        public static extern IntPtr nng_version();

#region protocols

        [DllImport(LibraryName, EntryPoint = "nng_req0_open")]
        public static extern nng_errno nng_req0_open(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_req0_open_raw")]
        public static extern nng_errno nng_req0_open_raw(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_rep0_open")]
        public static extern nng_errno nng_rep0_open(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_rep0_open_raw")]
        public static extern nng_errno nng_rep0_open_raw(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_surveyor0_open")]
        public static extern nng_errno nng_surveyor0_open(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_surveyor0_open_raw")]
        public static extern nng_errno nng_surveyor0_open_raw(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_respondent0_open")]
        public static extern nng_errno nng_respondent0_open(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_respondent0_open_raw")]
        public static extern nng_errno nng_respondent0_open_raw(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pub0_open")]
        public static extern nng_errno nng_pub0_open(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pub0_open_raw")]
        public static extern nng_errno nng_pub0_open_raw(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_sub0_open")]
        public static extern nng_errno nng_sub0_open(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_sub0_open_raw")]
        public static extern nng_errno nng_sub0_open_raw(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_push0_open")]
        public static extern nng_errno nng_push0_open(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_push0_open_raw")]
        public static extern nng_errno nng_push0_open_raw(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pull0_open")]
        public static extern nng_errno nng_pull0_open(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pull0_open_raw")]
        public static extern nng_errno nng_pull0_open_raw(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pair0_open")]
        public static extern nng_errno nng_pair0_open(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pair0_open_raw")]
        public static extern nng_errno nng_pair0_open_raw(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pair1_open")]
        public static extern nng_errno nng_pair1_open(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pair1_open_raw")]
        public static extern nng_errno nng_pair1_open_raw(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_bus0_open")]
        public static extern nng_errno nng_bus0_open(out nng_socket socket);

        [DllImport(LibraryName, EntryPoint = "nng_bus0_open_raw")]
        public static extern nng_errno nng_bus0_open_raw(out nng_socket socket);

#endregion

        /*#region transports

        [DllImport(LibraryName, EntryPoint = "nng_inproc_register")]
        public static extern  nng_errno nng_inproc_register();

        [DllImport(LibraryName, EntryPoint = "nng_ipc_register")]
        public static extern  nng_errno nng_ipc_register();

        [DllImport(LibraryName, EntryPoint = "nng_tcp_register")]
        public static extern  nng_errno nng_tcp_register();

        [DllImport(LibraryName, EntryPoint = "nng_ws_register")]
        public static extern  nng_errno nng_ws_register();

        [DllImport(LibraryName, EntryPoint = "nng_wss_register")]
        public static extern  nng_errno nng_wss_register();

        #endregion*/

#pragma warning restore CA2101 // Specify marshaling for P/Invoke string arguments
    }
}