using System;
using System.IO;
using NNG.Utilities;

namespace NNG.Native.Utils.Windows
{
    /// <summary>
    ///     Provides the means to load a dynamic library on a windows system.
    /// </summary>
    internal static class WindowsLibraryLoader
    {
        /// <summary>
        ///     Gets the windows library path.
        /// </summary>
        /// <returns>
        ///     Returns a string in the form of "runtimes/win-{arch}/native/"
        /// </returns>
        public static string GetWindowsLibraryPath()
        {
            var arch = SystemInformation.IsX64() ? "x64" : "x86";
            return Path.Combine(AppContext.BaseDirectory, $"runtimes/win-{arch}/native/");
        }
    }
}
