using System;
using System.Collections.Generic;

namespace NNGNET.Native.Utils.Linux
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
        public IList<string> SearchPaths { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LibraryLoadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="searchPaths">The search paths.</param>
        public LibraryLoadException(string message, IList<string> searchPaths) : base(message)
        {
            SearchPaths = searchPaths;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LibraryLoadException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="searchPaths">The search paths.</param>
        /// <param name="innerException">The inner exception.</param>
        public LibraryLoadException(string message, IList<string> searchPaths, Exception innerException) : base(message, innerException)
        {
            SearchPaths = searchPaths;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LibraryLoadException"/> class.
        /// </summary>
        /// <param name="searchPaths">The search paths.</param>
        public LibraryLoadException(IList<string> searchPaths)
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="LibraryLoadException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info">info</paramref> parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is null or <see cref="System.Exception.HResult"></see> is zero (0).</exception>
        protected LibraryLoadException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}