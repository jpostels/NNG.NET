using NNGNET.Native.InteropTypes;

namespace NNGNET
{
    using System;

    using ErrorHandling;

    /// <summary>
    ///     Provides information about NNG based errors.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class NngException : Exception
    {
        /// <summary>
        ///     Gets the error code.
        /// </summary>
        /// <value>
        ///     The error code.
        /// </value>
        public int ErrorCode => (int) NngErrNo;

        /// <summary>
        ///     The NNG error number
        /// </summary>
        internal readonly nng_errno NngErrNo;

        /// <summary>
        ///     Gets the nanomsg error.
        /// </summary>
        /// <value>
        ///     The nanomsg error.
        /// </value>
        public string NanomsgError { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NngException"/> class.
        /// </summary>
        public NngException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NngException"/> class.
        /// </summary>
        /// <param name="errorCode">The code that identifies the error.</param>
        internal NngException(nng_errno errorCode) : base(ThrowHelper.GetNanomsgError(errorCode))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NngException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NngException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NngException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public NngException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NngException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="errorCode">The error code.</param>
        internal NngException(string message, nng_errno errorCode) : base(message)
        {
            NngErrNo = errorCode;
            NanomsgError = ThrowHelper.GetNanomsgError(errorCode);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NngException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        internal NngException(string message, nng_errno errorCode, Exception innerException) : base(message, innerException)
        {
            NngErrNo = errorCode;
            NanomsgError = ThrowHelper.GetNanomsgError(errorCode);
        }
    }
}
