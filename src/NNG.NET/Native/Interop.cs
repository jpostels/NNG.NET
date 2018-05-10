namespace NNG.Native
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
        ///     Initializes the <see cref="Interop"/> class.
        /// </summary>
        /// <exception cref="NotSupportedException">
        ///     NNG.NET does not support this Operating System.
        /// </exception>
        static Interop()
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
                UnixLibraryLoader.LoadPosixLibrary(LibraryName);
            }
            else
            {
                // ReSharper disable once ThrowExceptionInUnexpectedLocation
                throw new NotSupportedException("NNG.NET does not support this Operating System.");
            }
        }

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

                // Prelink all P/Invoke functions.
                // This makes sure all methods are functioning correctly
                // NOTE: Does not actually invoke a function call
                Marshal.PrelinkAll(typeof(Interop));
                IsInitialized = true;
            }
        }

        /// <summary>
        ///     The maximum length of a socket address. This includes the terminating NUL.
        /// </summary>
        /// <remarks>
        ///     This limit is built into other implementations, so do not change it.
        /// </remarks>
        public const int NngMaxAddressLength = 128;

        [DllImport(LibraryName, EntryPoint = "nng_fini")]
        public static extern void nng_fini();

        /// <summary>
        ///     Close socket
        /// </summary>
        /// <param name="socketId">The socket.</param>
        /// <returns></returns>
        [DllImport(LibraryName, EntryPoint = "nng_close")]
        public static extern int nng_close(nng_socket socketId);

        [DllImport(LibraryName, EntryPoint = "nng_socket_id")]
        public static extern int nng_socket_id(nng_socket socketId);

        [DllImport(LibraryName, EntryPoint = "nng_closeall")]
        public static extern void nng_closeall();

        #region nng setopt

        [DllImport(LibraryName)]
        public static extern unsafe int nng_setopt(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* value, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_setopt_bool(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName)]
        public static extern int nng_setopt_int(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName)]
        public static extern int nng_setopt_size(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_setopt_ms(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration value);

        [DllImport(LibraryName)]
        public static extern int nng_setopt_uint64(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, ulong value);

        [DllImport(LibraryName)]
        public static extern int nng_setopt_string(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_setopt_ptr(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* ptr);

        #endregion

        #region nng getopt

        [DllImport(LibraryName)]
        public static extern unsafe int nng_getopt(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* value, out UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_bool(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_int(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_ms(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration value);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_size(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_uint64(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong value);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_ptr(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_string(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string value);

        #endregion

        [DllImport(LibraryName)]
        public static extern unsafe int nng_pipe_notify(nng_socket socketId, [MarshalAs(UnmanagedType.FunctionPtr)] nng_pipe_cb callback, void* ptr);

        [DllImport(LibraryName)]
        public static extern int nng_listen(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string addr, ref nng_listener listener, [MarshalAs(UnmanagedType.I4)] nng_flag_enum flags);

        [DllImport(LibraryName)]
        public static extern int nng_dial(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string addr, ref nng_dialer listener, [MarshalAs(UnmanagedType.I4)] nng_flag_enum flags);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_create([Out, In] ref nng_dialer dialer, nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string addr);

        [DllImport(LibraryName)]
        public static extern int nng_listener_create([Out, In] ref nng_listener listener, nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string addr);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_start(nng_dialer dialer, int flags);

        [DllImport(LibraryName)]
        public static extern int nng_listener_start(nng_listener listener, int flags);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_close(nng_dialer dialer);

        [DllImport(LibraryName)]
        public static extern int nng_listener_close(nng_listener listener);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_id(nng_dialer listener);

        [DllImport(LibraryName)]
        public static extern int nng_listener_id(nng_listener listener);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_setopt(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, IntPtr value, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_setopt_bool(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_setopt_int(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_setopt_ms(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_setopt_size(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_setopt_uint64(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, ulong value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_setopt_ptr(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, IntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_setopt_string(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value, out UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_bool(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_int(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_ms(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_size(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_sockaddr(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_sockaddr value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_uint64(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_ptr(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_string(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_setopt(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, IntPtr value, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_listener_setopt_bool(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_setopt_int(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_setopt_ms(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_setopt_size(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_setopt_uint64(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, ulong value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_setopt_ptr(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, IntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_setopt_string(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value, out UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_bool(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_int(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_ms(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_size(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_sockaddr(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_sockaddr value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_uint64(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_ptr(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_string(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string value);

        [DllImport(LibraryName, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        public static extern string nng_strerror(int errorCode);

        [DllImport(LibraryName)]
        public static extern int nng_send(nng_socket socketId, IntPtr ptr, UIntPtr size, [MarshalAs(UnmanagedType.I4)] nng_flag_enum flags);

        [DllImport(LibraryName)]
        public static extern int nng_recv(nng_socket socketId, IntPtr ptr, out UIntPtr size, [MarshalAs(UnmanagedType.I4)] nng_flag_enum flags);

        [DllImport(LibraryName)]
        public static extern int nng_sendmsg(nng_socket socketId, ref nng_msg message, int flags);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_recvmsg(nng_socket socketId, ref nng_msg* message, int flags);

        [DllImport(LibraryName)]
        public static extern void nng_send_aio(nng_socket socketId, ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern void nng_recv_aio(nng_socket socketId, ref nng_aio aio);

        #region Context support

        [DllImport(LibraryName)]
        public static extern int nng_ctx_open([Out, In] ref nng_ctx ctx, nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_close(nng_ctx ctx);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_id(nng_ctx ctx);

        [DllImport(LibraryName)]
        public static extern void nng_ctx_recv(nng_ctx ctx, ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern void nng_ctx_send(nng_ctx ctx, ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_getopt(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value, out UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_getopt_bool(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_getopt_int(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_getopt_ms(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration duration);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_getopt_size(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out IntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_setopt(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref IntPtr value, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_setopt_bool(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_setopt_int(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_setopt_ms(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration duration);

        [DllImport(LibraryName)]
        public static extern int nng_ctx_setopt_size(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

        #endregion

        [DllImport(LibraryName)]
        public static extern IntPtr nng_alloc(UIntPtr size);

        [DllImport(LibraryName)]
        public static extern void nng_free(IntPtr ptr, UIntPtr size);

        [DllImport(LibraryName)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string nng_strdup([MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImport(LibraryName)]
        public static extern void nng_strfree([MarshalAs(UnmanagedType.LPStr)] string str);

        #region AIO

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void AioAllocCallback(void* ptr);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_aio_alloc([Out, In] ref nng_aio* aio, [MarshalAs(UnmanagedType.FunctionPtr)] AioAllocCallback completionCallback, void* args);

        [DllImport(LibraryName)]
        public static extern void nng_aio_free(ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern void nng_aio_stop(ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern int nng_aio_result(ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern UIntPtr nng_aio_count(ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern void nng_aio_cancel(ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern void nng_aio_abort(ref nng_aio aio, int i);

        [DllImport(LibraryName)]
        public static extern void nng_aio_wait(ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern void nng_aio_set_msg(ref nng_aio aio, ref nng_msg msg);

        [DllImport(LibraryName)]
        public static extern ref nng_msg nng_aio_get_msg(ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_aio_set_input(ref nng_aio aio, uint index, void* arg);

        [DllImport(LibraryName)]
        public static extern unsafe void* nng_aio_get_input(ref nng_aio aio, uint index);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_aio_set_output(ref nng_aio aio, uint index, void* arg);

        [DllImport(LibraryName)]
        public static extern unsafe void* nng_aio_get_output(ref nng_aio aio, uint index);

        [DllImport(LibraryName)]
        public static extern void nng_aio_set_timeout(ref nng_aio aio, nng_duration timeout);

        [DllImport(LibraryName)]
        public static extern int nng_aio_set_iov(ref nng_aio aio, uint niov, in nng_iov iov);

        [DllImport(LibraryName)]
        public static extern void nng_aio_finish(ref nng_aio aio, int rv);

        [DllImport(LibraryName)]
        public static extern void nng_sleep_aio(nng_duration duration, ref nng_aio aio);

        #endregion AIO

        #region Message API

        [DllImport(LibraryName)]
        public static extern unsafe int nng_msg_alloc([Out, In] ref nng_msg* msg, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern void nng_msg_free(ref nng_msg msg);

        [DllImport(LibraryName)]
        public static extern int nng_msg_realloc(ref nng_msg msg, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern unsafe void* nng_msg_header(ref nng_msg msg);

        [DllImport(LibraryName)]
        public static extern UIntPtr nng_msg_header_len(in nng_msg msg);

        [DllImport(LibraryName)]
        public static extern unsafe void* nng_msg_body(ref nng_msg msg);

        [DllImport(LibraryName)]
        public static extern UIntPtr nng_msg_len(in nng_msg msg);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_msg_append(ref nng_msg msg, void* data, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_msg_insert(ref nng_msg msg, void* data, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_msg_trim(ref nng_msg msg, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_msg_chop(ref nng_msg msg, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_msg_header_append(ref nng_msg msg, void* data, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_msg_header_insert(ref nng_msg msg, void* data, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_msg_header_trim(ref nng_msg msg, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_msg_header_chop(ref nng_msg msg, UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_msg_header_append_u32(ref nng_msg msg, uint val);

        [DllImport(LibraryName)]
        public static extern int nng_msg_header_insert_u32(ref nng_msg msg, uint val);

        [DllImport(LibraryName)]
        public static extern int nng_msg_header_chop_u32(ref nng_msg msg, ref uint val);

        [DllImport(LibraryName)]
        public static extern int nng_msg_header_trim_u32(ref nng_msg msg, ref uint val);

        [DllImport(LibraryName)]
        public static extern int nng_msg_append_u32(ref nng_msg msg, uint val);

        [DllImport(LibraryName)]
        public static extern int nng_msg_insert_u32(ref nng_msg msg, uint val);

        [DllImport(LibraryName)]
        public static extern int nng_msg_chop_u32(ref nng_msg msg, ref uint val);

        [DllImport(LibraryName)]
        public static extern int nng_msg_trim_u32(ref nng_msg msg, ref uint val);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_msg_dup(ref nng_msg* msgDuplicate, ref nng_msg msgSource);

        [DllImport(LibraryName)]
        public static extern void nng_msg_clear(ref nng_msg msg);

        [DllImport(LibraryName)]
        public static extern void nng_msg_header_clear(ref nng_msg msg);

        [DllImport(LibraryName)]
        public static extern void nng_msg_set_pipe(ref nng_msg msg, nng_pipe pipe);

        [DllImport(LibraryName)]
        public static extern nng_pipe nng_msg_get_pipe(ref nng_msg msg);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_msg_getopt(ref nng_msg msg, int opt, void* ptr, ref UIntPtr size);

        #endregion

        #region Pipe API

        [DllImport(LibraryName)]
        public static extern unsafe int nng_pipe_getopt(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* ptr, out UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_pipe_getopt_bool(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool val);

        [DllImport(LibraryName)]
        public static extern int nng_pipe_getopt_int(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int val);

        [DllImport(LibraryName)]
        public static extern int nng_pipe_getopt_ms(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration duration);

        [DllImport(LibraryName)]
        public static extern int nng_pipe_getopt_size(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr val);

        [DllImport(LibraryName)]
        public static extern int nng_pipe_getopt_sockaddr(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_sockaddr val);

        [DllImport(LibraryName)]
        public static extern int nng_pipe_getopt_uint64(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong val);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_pipe_getopt_ptr(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* val);

        [DllImport(LibraryName)]
        public static extern int nng_pipe_getopt_string(nng_pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string val);

        [DllImport(LibraryName)]
        public static extern int nng_pipe_close(nng_pipe pipe);

        [DllImport(LibraryName)]
        public static extern int nng_pipe_id(nng_pipe pipe);

        [DllImport(LibraryName)]
        public static extern nng_socket nng_pipe_socket(nng_pipe pipe);

        [DllImport(LibraryName)]
        public static extern nng_dialer nng_pipe_dialer(nng_pipe pipe);

        [DllImport(LibraryName)]
        public static extern nng_listener nng_pipe_listener(nng_pipe pipe);

        #endregion

        #region Statistics

        // Statistics.  These are for informational purposes only, and subject
        // to change without notice.  The API for accessing these is stable,
        // but the individual statistic names, values, and meanings are all
        // subject to change.

        //BUG the following methods are not yet implemented by nng

        //[DllImport(LibraryName)]
        //public static extern unsafe int nng_snapshot_create(nng_socket socket, ref nng_snapshot* snapshot);

        //[DllImport(LibraryName)]
        //public static extern void nng_snapshot_free(ref nng_snapshot snapshot);

        //[DllImport(LibraryName)]
        //public static extern int nng_snapshot_update(ref nng_snapshot snapshot);

        //[DllImport(LibraryName)]
        //public static extern unsafe int nng_snapshot_next(ref nng_snapshot snapshot, ref nng_stat* stat);

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

        [DllImport(LibraryName)]
        public static extern int nng_device(nng_socket socket1, nng_socket socket2);

        #region URL support

        [DllImport(LibraryName)]
        public static extern unsafe int nng_url_parse([Out, In] ref nng_url* url, [MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImport(LibraryName)]
        public static extern void nng_url_free(ref nng_url url);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_url_clone([Out, In] ref nng_url* urlDuplicate, ref nng_url urlSource);

        #endregion

        /// <summary>
        ///     Report library version
        /// </summary>
        /// <returns></returns>
        [DllImport(LibraryName, EntryPoint = "nng_version")]
        public static extern IntPtr nng_version();

        #region protocols

        [DllImport(LibraryName)]
        public static extern int nng_req0_open(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_req0_open_raw(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_rep0_open(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_rep0_open_raw(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_surveyor0_open(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_surveyor0_open_raw(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_respondent0_open(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_respondent0_open_raw(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_pub0_open(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_pub0_open_raw(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_sub0_open(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_sub0_open_raw(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_push0_open(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_push0_open_raw(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_pull0_open(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_pull0_open_raw(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_pair0_open(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_pair0_open_raw(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_pair1_open(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_pair1_open_raw(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_bus0_open(out nng_socket socket);

        [DllImport(LibraryName)]
        public static extern int nng_bus0_open_raw(out nng_socket socket);

        #endregion

        /*#region transports
        
        [DllImport(LibraryName)]
        public static extern int nng_inproc_register();

        [DllImport(LibraryName)]
        public static extern int nng_ipc_register();

        [DllImport(LibraryName)]
        public static extern int nng_tcp_register();

        [DllImport(LibraryName)]
        public static extern int nng_ws_register();

        [DllImport(LibraryName)]
        public static extern int nng_wss_register();
        
        #endregion*/

#pragma warning restore CA2101 // Specify marshaling for P/Invoke string arguments
    }
}