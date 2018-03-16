using System.Threading;
using NNG.Native.InteropTypes;

namespace NNG.Native
{
    using System;
    using System.Runtime.InteropServices;
    using Utils.Linux;
    using Utils.Windows;

    using nng_socket = System.UInt32;
    using nng_dialer = System.UInt32;
    using nng_listener = System.UInt32;
    using nng_pipe = System.UInt32;
    using nng_duration = System.Int32;

    /// <summary>
    ///     Provider for P/Invoke of nng library functions
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle", Justification = "Using native names")]
    internal static class Interop
    {
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

                Interlocked.MemoryBarrier();
                IsInitialized = true;
            }
        }

        [DllImport(LibraryName, EntryPoint = "nng_fini")]
        public static extern void nng_fini();

        /// <summary>
        ///     Close socket
        /// </summary>
        /// <param name="socketId">The socket.</param>
        /// <returns></returns>
        [DllImport(LibraryName, EntryPoint = "nng_close")]
        public static extern int nng_close(nng_socket socketId);

        /// <summary>
        ///     Report library version
        /// </summary>
        /// <returns></returns>
        [DllImport(LibraryName, EntryPoint = "nng_version")]
        public static extern IntPtr nng_version();

        [DllImport(LibraryName, EntryPoint = "nng_closeall")]
        public static extern void nng_closeall();

        [DllImport(LibraryName)]
        public static extern unsafe int nng_setopt(nng_socket sockedId, string optionName, void* value, UIntPtr size);

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
        public static extern unsafe int nng_getopt(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* value, ref UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_int(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref int value);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_ms(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref nng_duration value);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_size(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref UIntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_uint64(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref ulong value);

        [DllImport(LibraryName)]
        public static extern int nng_getopt_ptr(nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref IntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_listen(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string addr, ref nng_listener listener, [MarshalAs(UnmanagedType.I4)] nng_flag_enum flags);

        [DllImport(LibraryName)]
        public static extern int nng_dial(nng_socket sockedId, [MarshalAs(UnmanagedType.LPStr)] string addr, ref nng_dialer listener, [MarshalAs(UnmanagedType.I4)] nng_flag_enum flags);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_create(ref nng_dialer dialer, nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string addr);

        [DllImport(LibraryName)]
        public static extern int nng_listener_create(ref nng_listener listener, nng_socket socketId, [MarshalAs(UnmanagedType.LPStr)] string addr);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_start(nng_dialer dialer, int flags);

        [DllImport(LibraryName)]
        public static extern int nng_listener_start(nng_listener listener, int flags);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_close(nng_dialer dialer);

        [DllImport(LibraryName)]
        public static extern int nng_listener_close(nng_listener listener);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_setopt(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, IntPtr value, UIntPtr size);

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
        public static extern int nng_dialer_getopt(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref IntPtr value, ref UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_int(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref int value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_ms(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref nng_duration value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_size(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref UIntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_uint64(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref ulong value);

        [DllImport(LibraryName)]
        public static extern int nng_dialer_getopt_ptr(nng_dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref IntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_setopt(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, IntPtr value, UIntPtr size);

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
        public static extern int nng_listener_getopt(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref IntPtr value, ref UIntPtr size);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_int(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref int value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_ms(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref nng_duration value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_size(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref UIntPtr value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_uint64(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref ulong value);

        [DllImport(LibraryName)]
        public static extern int nng_listener_getopt_ptr(nng_listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref IntPtr value);

        [DllImport(LibraryName)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string nng_strerror(int errorCode);

        [DllImport(LibraryName)]
        public static extern int nng_send(nng_socket socketId, IntPtr ptr, UIntPtr size, [MarshalAs(UnmanagedType.I4)] nng_flag_enum flags);

        [DllImport(LibraryName)]
        public static extern int nng_recv(nng_socket socketId, IntPtr ptr, ref UIntPtr size, [MarshalAs(UnmanagedType.I4)] nng_flag_enum flags);

        [DllImport(LibraryName)]
        public static extern int nng_sendmsg(nng_socket socketId, ref nng_msg message, int flags);

        [DllImport(LibraryName)]
        public static extern unsafe int nng_recvmsg(nng_socket socketId, ref nng_msg* message, int flags);

        [DllImport(LibraryName)]
        public static extern void nng_send_aio(nng_socket socketId, ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern void nng_recv_aio(nng_socket socketId, ref nng_aio aio);

        [DllImport(LibraryName)]
        public static extern IntPtr nng_alloc(UIntPtr size);

        [DllImport(LibraryName)]
        public static extern void nng_free(IntPtr ptr, UIntPtr size);

        // TODO continue at nng.h line 277
    }
}
