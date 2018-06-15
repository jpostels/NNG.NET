using System;

namespace NNG.Native.Utils.Linux
{
    /// <summary>
    ///     Provides error information about a failed library load attempt.
    /// </summary>
    /// <seealso cref="Exception" />
    public class LibraryLoadException : Exception
    {
        /// <summary>
        ///     Gets the used search paths for library loading.
        /// </summary>
        /// <value>
        ///     The search paths.
        /// </value>
        public string[] SearchPaths { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LibraryLoadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="searchPaths">The search paths.</param>
        public LibraryLoadException(string message, string[] searchPaths) : base(message)
        {
            SearchPaths = searchPaths;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LibraryLoadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="searchPaths">The search paths.</param>
        /// <param name="innerException">The inner exception.</param>
        public LibraryLoadException(string message, string[] searchPaths, Exception innerException) : base(message, innerException)
        {
            SearchPaths = searchPaths;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LibraryLoadException"/> class.
        /// </summary>
        /// <param name="searchPaths">The search paths.</param>
        public LibraryLoadException(string[] searchPaths)
        {
            SearchPaths = searchPaths;
        }

        /// <inheritdoc />
        public LibraryLoadException()
        {
        }

        /// <inheritdoc />
        public LibraryLoadException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public LibraryLoadException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}