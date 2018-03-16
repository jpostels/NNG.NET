using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace NNG.Native.Utils.Linux
{
    /// <summary>
    ///     Provides the means to load a shared library on a unix system.
    /// </summary>
    internal static class UnixLibraryLoader
    {
        /// <summary>
        ///     The function dlerror() returns a human readable string describing the most recent error that occurred from
        ///     dlopen(), dlsym() or dlclose() since the last call to dlerror(). It returns NULL if no errors have occurred since
        ///     initialization or since it was last called.
        /// </summary>
        /// <returns>
        ///     The human readable string describing the most recent error.
        ///     Returns NULL if no errors have occurred since initialization or since it was last called.
        /// </returns>
        [DllImport("libdl.so", EntryPoint = "dlerror")]
        // ReSharper disable once StyleCop.SA1300
        private static extern IntPtr Dlerror();

        /// <summary>
        ///     The function loads the dynamic library file named by the null-terminated string <paramref name="fileName" /> and
        ///     returns an opaque "handle" for the dynamic library. If <paramref name="fileName" /> is NULL, then the returned
        ///     handle is for the main program. If <paramref name="fileName" /> contains a slash ("/"), then it is interpreted as a
        ///     (relative or absolute) pathname.
        /// </summary>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        /// <param name="flags">
        ///     The flags. See man pages for details.
        /// </param>
        /// <returns>
        ///     The opaque "handle" for the dynamic library.
        /// </returns>
        [DllImport("libdl.so", EntryPoint = "dlopen")]
        // ReSharper disable once StyleCop.SA1300
        private static extern IntPtr Dlopen(string fileName, int flags);

        /// <summary>
        ///     Loads a posix shared library.
        /// </summary>
        /// <param name="libName">
        ///     The library name.
        /// </param>
        /// <returns>
        ///     The entry <see cref="IntPtr" /> of the fully loaded library.
        /// </returns>
        /// <exception cref="LibraryLoadException">
        ///     Loading library failed.
        /// </exception>
        public static IntPtr LoadPosixLibrary(string libName)
        {
            const int rtldNowFlags = 2;

            var libFile = "lib" + libName.ToLower() + ".so";
            var rootDirectory = AppContext.BaseDirectory;
            var is64Bit = SystemInformation.IsX64();
            var paths = new[]
            {
                Path.Combine(rootDirectory, "bin", is64Bit ? "x64" : "x86", libFile),
                Path.Combine(rootDirectory, is64Bit ? "x64" : "x86", libFile),
                Path.Combine(rootDirectory, libFile), Path.Combine("/usr/local/lib", libFile),
                Path.Combine("/usr/lib", libFile)
            };

            foreach (var path in paths)
            {
                if (path == null)
                {
                    continue;
                }

                if (!File.Exists(path))
                {
                    continue;
                }

                var addr = Dlopen(path, rtldNowFlags);
                if (addr == IntPtr.Zero)
                {
                    throw new LibraryLoadException(
                       "dlopen failed: " + path + " : " + Marshal.PtrToStringAnsi(Dlerror()), new[] { path });
                }

                return addr;
            }

            throw new LibraryLoadException(
                "dlopen failed: unable to locate library " + libFile + ". Searched: "
                + paths.Aggregate((a, b) => a + "; " + b), paths.ToArray());
        }
    }
}