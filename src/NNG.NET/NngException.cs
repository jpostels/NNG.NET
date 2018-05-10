using System;

namespace NNG
{
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
        public int ErrorCode { get; }

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
        public NngException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
            NanomsgError = ThrowHelper.GetNanomsgError(errorCode);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NngException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public NngException(string message, int errorCode, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
            NanomsgError = ThrowHelper.GetNanomsgError(errorCode);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NngException"/> class.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.
        /// </param>
        protected NngException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
