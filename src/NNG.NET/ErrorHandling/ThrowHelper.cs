﻿using System;
using System.Runtime.CompilerServices;
using NNG.Native;
using NNG.Native.InteropTypes;

namespace NNG.ErrorHandling
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
        public static string GetNanomsgError(nng_errno errorCode) => Interop.nng_strerror((int)errorCode);

        /// <summary>
        ///     Throws an exception with the specified <paramref name="errorCode"/>,
        ///     if it does not match <see cref="nng_errno.NNG_SUCCESS"/>.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <exception cref="NngException"></exception>
        public static void ThrowIfNotSuccess(nng_errno errorCode)
        {
            if (errorCode == nng_errno.NNG_SUCCESS)
            {
                return;
            }

            Throw(errorCode);
        }

        /// <summary>
        ///     Throws an exception with the specified <paramref name="errorCode"/>.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        /// <exception cref="NngException"></exception>
        public static void Throw(nng_errno errorCode, string message = null, [CallerMemberName] string source = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = source + ": " + GetNanomsgError(errorCode);
            }

            throw new NngException(message, errorCode);
        }

        /// <summary>
        ///     Throws an exception with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="source">The source.</param>
        /// <exception cref="NngException"></exception>
        public static void Throw(string message, [CallerMemberName] string source = null)
        {
            throw new NngException(source + ": " + message);
        }

        /// <summary>
        ///     Throws an exception with the specified <paramref name="message"/> and <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="source">The source.</param>
        /// <exception cref="NngException"></exception>
        public static void Throw(string message, Exception innerException, [CallerMemberName] string source = null)
        {
            throw new NngException(source + ": " + message, innerException);
        }
    }
}