using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;

namespace NNGNET.ErrorHandling
{
    /// <summary>
    ///     Provides helper methods for throwing library exceptions. 
    /// </summary>
    internal static class ThrowHelper
    {
        /// <summary>
        ///     Gets the error description for the specified <paramref name="errorCode"/>.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <returns>
        ///     Returns the error description if there is one; Otherwise an empty string.
        /// </returns>
        public static string GetNanomsgError(nng_errno errorCode) => Interop.GetErrorString((int)errorCode);

        /// <summary>
        ///     Throws an exception with the specified <paramref name="errorCode"/>,
        ///     if it does not match <see cref="nng_errno.NNG_SUCCESS"/>.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <exception cref="NngException">
        ///     The specified <paramref name="errorCode"/> was something other than <see cref="nng_errno.NNG_SUCCESS"/>
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotSuccess(nng_errno errorCode, [CallerMemberName] string source = null)
        {
            if (errorCode == nng_errno.NNG_SUCCESS)
            {
                return;
            }

            if ((errorCode & nng_errno.NNG_ESYSERR) != 0)
            {
                var inner = GetSystemErrorException(((int) errorCode & ~((int) nng_errno.NNG_ESYSERR)));
                throw new NngException("A system error occured. See inner exception for details. ", inner);
                //throw GetExceptionForErrorCode("SysError: 0x" + ((int)errorCode & ~((int)nng_errno.NNG_ESYSERR)).ToString("X"), source);
            }

            throw GetExceptionForErrorCode(errorCode, source: source);
        }

        private static Win32Exception GetSystemErrorException(int errorCode)
        {
            return new Win32Exception(errorCode);
        }

        /// <summary>
        ///     Gets an exception with the specified <paramref name="errorCode"/>.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        public static NngException GetExceptionForErrorCode(nng_errno errorCode, string message = null, [CallerMemberName] string source = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = source + ": " + GetNanomsgError(errorCode);
            }

            return new NngException(message, errorCode);
        }

        /// <summary>
        ///     Gets an exception with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        public static NngException GetExceptionForErrorCode(string message, [CallerMemberName] string source = null)
        {
            return new NngException(source + ": " + message);
        }

        /// <summary>
        ///     Gets an exception with the specified <paramref name="message"/> and <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="source">The source.</param>
        public static NngException GetExceptionForErrorCode(string message, Exception innerException, [CallerMemberName] string source = null)
        {
            return new NngException(source + ": " + message, innerException);
        }
    }
}