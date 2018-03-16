using System;
using System.IO;
using System.Runtime.InteropServices;
using NNG.Native.Windows;

namespace NNG.Native
{
    /// <summary>
    ///     Provider for P/Invoke of nng library functions
    /// </summary>
    internal static class Interop
    {
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
                Kernel32.SetDllDirectory(GetWindowsLibraryPath());
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // TODO set lookup path for linux environment or load directly
            }
            else
            {
                // ReSharper disable once ThrowExceptionInUnexpectedLocation
                throw new NotSupportedException("NNG.NET does not support this Operating System.");
            }

            Marshal.PrelinkAll(typeof(Interop));
        }

        /// <summary>
        ///     Gets the windows library path.
        /// </summary>
        /// <returns>
        ///     Returns a string in the form of "runtimes/win-{arch}/native/"
        /// </returns>
        private static string GetWindowsLibraryPath()
        {
            var arch = IsX64() ? "x64" : "x86";
            return Path.Combine(AppContext.BaseDirectory, $"runtimes/win-{arch}/native/");
        }

        /// <summary>
        ///     Determines whether this platforms architecture is x64.
        /// </summary>
        /// <remarks>
        ///     This is necessary due to an error in .NET Framework v4.7 where 
        ///     <see cref="System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture"/> 
        ///     may return the wrong value i.e. x86 on x64 processes. <br />
        ///     see also: https://github.com/dotnet/corefx/issues/25267
        /// </remarks>
        /// <returns>
        ///     <c>true</c> if this platforms architecture is x64; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsX64() => IntPtr.Size == sizeof(ulong);

        [DllImport("nng")]
        public static extern int nng_close(uint socket);

        [DllImport("nng")]
        public static extern IntPtr nng_version();
    }
}
