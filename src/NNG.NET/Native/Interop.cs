using System.Diagnostics.CodeAnalysis;

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
    [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle", Justification = "using alias for simple native interop types")]
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

            // Prelink all P/Invoke functions.
            // This makes sure all methods are functioning correctly
            // NOTE: Does not actually invoke a function call
            Marshal.PrelinkAll(typeof(Interop));
        }

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
    }
}
