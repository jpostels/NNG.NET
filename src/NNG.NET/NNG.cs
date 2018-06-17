using System.Reflection;

namespace NNGNET
{
    using System;

    using Native;
    using ErrorHandling;
    using Native.InteropTypes;

    /// <summary>
    ///     Safe provider for P/Invoke of nng library functions.
    ///     Also handles possible error return values and throws them as <see cref="NngException"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "API Provider. All member implicitly used.")]
    public static class NNG
    {
        /// <summary>
        ///     The maximum length of a socket address. This includes the terminating NUL.
        /// </summary>
        /// <remarks>
        ///     This limit is built into other implementations, so do not change it.
        /// </remarks>
        public const int MaxAddressLength = Interop.NngMaxAddressLength;

        /// <summary>
        ///     Gets a value indicating whether the interop functions are initialized.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the interop functions are initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized => Interop.IsInitialized;

        /// <summary>
        ///     Initializes the natvive library functions.
        ///     NOTE: This is not required to be run, but it may reduce initial latency during first calls.
        /// </summary>
        /// <remarks>
        ///     Prelinks all P/Invoke functions.
        ///     This also makes sure all methods are functioning correctly.
        /// </remarks> 
        public static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            Interop.Initialize();
        }

        /// <summary>
        ///     The nng_fini function is used to terminate the library, freeing certain global resources.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This should only be called during atexit() or just before dlclose().
        /// </para>
        /// <para>
        ///     THIS FUNCTION MUST NOT BE CALLED CONCURRENTLY WITH ANY OTHER FUNCTION
        ///     IN THIS LIBRARY; IT IS NOT REENTRANT OR THREADSAFE.
        /// </para>
        /// <para>
        ///     For most cases, this call is unnecessary, but it is provided to assist
        ///     when debugging with memory checkers (e.g. valgrind).  Calling this
        ///     function prevents global library resources from being reported incorrectly
        ///     as memory leaks.  In those cases, we recommend doing this with atexit().
        /// </para>
        /// </remarks>
        public static void Fini()
        {
            Interop.Fini();
        }

        /// <summary>
        ///     The nng_close function closes the supplied socket. <br/>
        ///     Messages that have been submitted for sending may be flushed or delivered, <br/>
        ///     depending upon the transport and the setting of the NNG_OPT_LINGER option. <br/>
        ///     Further attempts to use the socket after this call returns will result in <see cref="nng_errno.NNG_ECLOSED"/>. <br/>
        ///     Threads waiting for operations on the socket when this call is executed
        ///     may also return with an <see cref="nng_errno.NNG_ECLOSED"/> result.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <exception cref="NngException">
        ///     <see cref="nng_errno.NNG_ECLOSED"/>: The socket is already closed or was never opened.
        /// </exception>
        public static void Close(NNGSocket socket)
        {
            var err = Interop.Close(socket);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        /// <summary>
        ///     Gets the socket identifier.
        /// </summary>
        /// <param name="socket">The socket identifier.</param>
        /// <returns>
        ///     Returns the positive value for the socket identifier.
        /// </returns>
        /// <exception cref="NngException">"The given socket is invalid."</exception>
        public static int GetSocketId(NNGSocket socket)
        {
            var val = Interop.GetSocketId(socket);
            return val >= 0 ? val : throw ThrowHelper.GetExceptionForErrorCode(nng_errno.NNG_EINVAL, "The given socket is invalid. ");
        }

        /// <summary>
        ///     Closes all open sockets. 
        /// </summary>
        /// <remarks>
        ///     Do not call this from a library; it will affect all sockets.
        /// </remarks>
        public static void CloseAll() => Interop.CloseAll();

        #region Set Socket options

        /// <summary>
        ///     Sets an option identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">Name of the option.</param>
        /// <param name="pointer">The pointer.</param>
        /// <param name="size">The size.</param>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_EINVAL The value being passed is invalid.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_EREADONLY The specified option is read-only.
        ///     -or-
        ///     NNG_ESTATE The <paramref name="socket"/> is in an inappropriate state for setting this option. 
        /// </exception>
        public static void SetOption(NNGSocket socket, SocketOption option, IntPtr pointer, uint size)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.SetOption(socket, optionName, pointer, new UIntPtr(size));
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        /// <summary>
        ///     Sets an option identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">Name of the option.</param>
        /// <param name="value">The value</param>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_EINVAL The <paramref name="value"/> being passed is invalid.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_EREADONLY The specified <paramref name="option"/> is read-only.
        ///     -or-
        ///     NNG_ESTATE The <paramref name="socket"/> is in an inappropriate state for setting this option. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static void SetOption(NNGSocket socket, SocketOption option, bool value)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.SetOption(socket, optionName, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        /// <summary>
        ///     Sets an option identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">Name of the option.</param>
        /// <param name="value">The value</param>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_EINVAL The <paramref name="value"/> being passed is invalid.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_EREADONLY The specified <paramref name="option"/> is read-only.
        ///     -or-
        ///     NNG_ESTATE The <paramref name="socket"/> is in an inappropriate state for setting this option. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static void SetOption(NNGSocket socket, SocketOption option, int value)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.SetOption(socket, optionName, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        /// <summary>
        ///     Sets an option identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">Name of the option.</param>
        /// <param name="value">The value</param>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_EINVAL The <paramref name="value"/> being passed is invalid.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_EREADONLY The specified <paramref name="option"/> is read-only.
        ///     -or-
        ///     NNG_ESTATE The <paramref name="socket"/> is in an inappropriate state for setting this option. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static void SetOption(NNGSocket socket, SocketOption option, UIntPtr value)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.SetOption(socket, optionName, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        /// <summary>
        ///     Sets an option identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">Name of the option.</param>
        /// <param name="value">The value</param>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_EINVAL The <paramref name="value"/> being passed is invalid.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_EREADONLY The specified <paramref name="option"/> is read-only.
        ///     -or-
        ///     NNG_ESTATE The <paramref name="socket"/> is in an inappropriate state for setting this option. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static void SetOption(NNGSocket socket, SocketOption option, TimeSpan value)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.SetOptionDuration(socket, optionName, value.Milliseconds);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        /// <summary>
        ///     Sets an option identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">Name of the option.</param>
        /// <param name="value">The value</param>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_EINVAL The <paramref name="value"/> being passed is invalid.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_EREADONLY The specified <paramref name="option"/> is read-only.
        ///     -or-
        ///     NNG_ESTATE The <paramref name="socket"/> is in an inappropriate state for setting this option. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static void SetOption(NNGSocket socket, SocketOption option, ulong value)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.SetOption(socket, optionName, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        /// <summary>
        ///     Sets an option identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">Name of the option.</param>
        /// <param name="value">The value</param>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_EINVAL The <paramref name="value"/> being passed is invalid.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_EREADONLY The specified <paramref name="option"/> is read-only.
        ///     -or-
        ///     NNG_ESTATE The <paramref name="socket"/> is in an inappropriate state for setting this option. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static void SetOption(NNGSocket socket, SocketOption option, string value)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.SetOption(socket, optionName, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        /// <summary>
        ///     Sets an option identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">Name of the option.</param>
        /// <param name="pointer">The value</param>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_EINVAL The <paramref name="pointer"/> being passed is invalid.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_EREADONLY The specified <paramref name="option"/> is read-only.
        ///     -or-
        ///     NNG_ESTATE The <paramref name="socket"/> is in an inappropriate state for setting this option. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static void SetOptionPointer(NNGSocket socket, SocketOption option, IntPtr pointer)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.SetOptionPointer(socket, optionName, pointer);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        #endregion

        #region Get option

        /// <summary>
        ///     Gets an option value identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        ///     A <see cref="Span{Byte}"/> pointing to the option value.
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_ENOMEM Insufficient memory exists.
        ///     -or-
        ///     NNG_EWRITEONLY The option is write-only. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static Span<byte> GetOption(NNGSocket socket, SocketOption option)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.GetOption(socket, optionName, out var pointer, out var size);

            ThrowHelper.ThrowIfNotSuccess(err);
            unsafe
            {
                return new Span<byte>(pointer.ToPointer(), (int)size.ToUInt32());
            }
        }

        /// <summary>
        ///     Gets an option value identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        ///     A <see cref="bool"/> containing the current option value. 
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_ENOMEM Insufficient memory exists.
        ///     -or-
        ///     NNG_EWRITEONLY The option is write-only. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static bool GetOptionBool(NNGSocket socket, SocketOption option)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.GetOption(socket, optionName, out bool value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return value;
        }

        /// <summary>
        ///     Gets an option value identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        ///     A <see cref="int"/> containing the current option value. 
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_ENOMEM Insufficient memory exists.
        ///     -or-
        ///     NNG_EWRITEONLY The option is write-only. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static int GetOptionInt32(NNGSocket socket, SocketOption option)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.GetOption(socket, optionName, out int value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return value;
        }

        /// <summary>
        ///     Gets an option value identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        ///     A <see cref="TimeSpan"/> containing the current option value. 
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_ENOMEM Insufficient memory exists.
        ///     -or-
        ///     NNG_EWRITEONLY The option is write-only. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static TimeSpan GetOptionTimeSpan(NNGSocket socket, SocketOption option)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.GetOptionDuration(socket, optionName, out var value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return TimeSpan.FromMilliseconds(value);
        }

        /// <summary>
        ///     Gets an option value identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        ///     A <see cref="UIntPtr"/> containing the current option value. 
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_ENOMEM Insufficient memory exists.
        ///     -or-
        ///     NNG_EWRITEONLY The option is write-only. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static UIntPtr GetOptionUIntPtr(NNGSocket socket, SocketOption option)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.GetOption(socket, optionName, out UIntPtr value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return value;
        }

        /// <summary>
        ///     Gets an option value identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        ///     A <see cref="ulong"/> containing the current option value. 
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_ENOMEM Insufficient memory exists.
        ///     -or-
        ///     NNG_EWRITEONLY The option is write-only. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static ulong GetOptionUInt64(NNGSocket socket, SocketOption option)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.GetOption(socket, optionName, out ulong value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return value;
        }

        /// <summary>
        ///     Gets an option value identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        ///     An <see cref="IntPtr"/> pointing to the option value.
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_ENOMEM Insufficient memory exists.
        ///     -or-
        ///     NNG_EWRITEONLY The option is write-only. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static IntPtr GetOptionPointer(NNGSocket socket, SocketOption option)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.GetOptionPointer(socket, optionName, out var value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return value;
        }

        /// <summary>
        ///     Gets an option value identified by <paramref name="option"/> for a specific <paramref name="socket"/>.
        /// </summary>
        /// <remarks>
        ///     The actual options that may be configured in this way vary between socket types.
        /// </remarks>
        /// <param name="socket">The socket.</param>
        /// <param name="option">The option.</param>
        /// <returns>
        ///     A <see cref="string"/> containing the current option value. 
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_ECLOSED	<paramref name="socket"/> does not refer to an open socket.
        ///     -or-
        ///     NNG_ENOTSUP The <paramref name="option"/> is not supported.
        ///     -or-
        ///     NNG_ENOMEM Insufficient memory exists.
        ///     -or-
        ///     NNG_EWRITEONLY The option is write-only. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="option"/> is null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The property is retrieved and <paramref name="option"/> does not exist in the collection.
        /// </exception>
        public static string GetOptionString(NNGSocket socket, SocketOption option)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.GetOption(socket, optionName, out string value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return value;
        }

        #endregion

        /// <summary>
        ///     Registers the <paramref name="callback"/> action to be called
        ///     whenever a pipe fires the event specified by <paramref name="pipeEvent"/>
        ///     on the specified <paramref name="socket"/>.
        ///     Optionally user data may be supplied in <paramref name="args"/>,
        ///     which is then provided to the callback.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="pipeEvent">The pipe event.</param>
        /// <param name="callback">The callback action.</param>
        /// <param name="args">Optional user data</param>
        /// <exception cref="NngException">
        ///     The specified errorCode was something other than <see cref="nng_errno.NNG_SUCCESS"/>. <br/>
        ///     NNG_ECLOSED The <paramref name="socket"/> does not refer to an open socket.
        /// </exception>
        public static void SetPipeNotification(NNGSocket socket, PipeEvent pipeEvent, PipeCallback callback, IntPtr args = default)
        {
            var err = Interop.PipeSetNotification(socket, pipeEvent, callback, args != default ? args : IntPtr.Zero);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        /// <summary>
        ///     Creates a newly initialized <see cref="Listener"/> object, associated with the <paramref name="socket"/>,
        ///     and configured to listen at the <paramref name="address"/>, and starts it. <br/>
        ///     Normally, the act of “binding” to the address is done synchronously,
        ///     including any necessary name resolution.
        ///     As a result, a failure, such as if the address is already in use, will be returned immediately. <br/>
        ///     However, if <paramref name="nonBlocking"/> is set to <c>true</c>, then this is done asynchronously;
        ///     furthermore any failure to bind will be periodically reattempted in the background.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="address">The address.</param>
        /// <param name="nonBlocking">if set to <c>true</c> the call is done asynchronously.</param>
        /// <returns>
        ///     A newly initialized <see cref="Listener"/> object.
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_EADDRINUSE The <paramref name="address"/> specified by url is already in use.
        ///     -or-
        ///     NNG_EADDRINVAL An invalid url was specified.
        ///     -or- 
        ///     NNG_ECLOSED The <paramref name="socket"/> is not open.
        ///      -or- 
        ///     NNG_ENOMEM Insufficient memory is available. 
        /// </exception>
        public static Listener Listen(NNGSocket socket, string address, bool nonBlocking = false)
        {
            var flags = nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None;
            var err = Interop.Listen(socket, address, out var listener, flags);
            ThrowHelper.ThrowIfNotSuccess(err);
            return listener;
        }

        /// <summary>
        ///     Creates a newly initialized <see cref="Dialer"/> object, associated with <paramref name="socket"/>,
        ///     and configured to listen at the <paramref name="address"/>, and starts it. <br/>
        ///     Dialers initiate a remote connection to a listener.
        ///     Upon a successful connection being established, they create a pipe, add it to the socket,
        ///     and then wait for that pipe to be closed. <br/>
        ///     When the pipe is closed, the dialer attempts to re-establish the connection.
        ///     Dialers will also periodically retry a connection automatically
        ///     if an attempt to connect asynchronously fails. <br/>
        ///     Normally, the first attempt to connect to the address is done synchronously,
        ///     including any necessary name resolution. <br/>
        ///     As a result, a failure, such as if the connection is refused, will be returned immediately,
        ///     and no further action will be taken.
        /// </summary>
        /// <param name="socket">The socket.</param>
        /// <param name="address">The address.</param>
        /// <param name="nonBlocking">if set to <c>true</c> the call is done asynchronously.</param>
        /// <returns>
        ///     A newly initialized <see cref="Dialer"/> object.
        /// </returns>
        /// <exception cref="NngException">
        ///     NNG_EADDRINVAL An invalid url was specified.
        ///     -or-
        ///     NNG_ECLOSED The <paramref name="socket"/> is not open.
        ///     -or-
        ///     NNG_ECONNREFUSED The remote peer refused the connection.
        ///     -or-
        ///     NNG_ECONNRESET The remote peer reset the connection.
        ///     -or-
        ///     NNG_EINVAL An invalid set of flags was specified.
        ///     -or-
        ///     NNG_ENOMEM Insufficient memory is available.
        ///     -or-
        ///     NNG_EPEERAUTH Authentication or authorization failure.
        ///     -or-
        ///     NNG_EPROTO A protocol error occurred.
        ///     -or-
        ///     NNG_EUNREACHABLE The remote address is not reachable. 
        /// </exception>
        public static Dialer Dial(NNGSocket socket, string address, bool nonBlocking = false)
        {
            var flags = nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None;
            var err = Interop.Dial(socket, address, out var dialer, flags);
            ThrowHelper.ThrowIfNotSuccess(err);
            return dialer;
        }

        public static Dialer CreateDialer(NNGSocket socket, string address)
        {
            var err = Interop.DialerCreate(out var dialer, socket, address);
            ThrowHelper.ThrowIfNotSuccess(err);
            return dialer;
        }

        public static Listener CreateListener(NNGSocket socket, string address)
        {
            var err = Interop.ListenerCreate(out var dialer, socket, address);
            ThrowHelper.ThrowIfNotSuccess(err);
            return dialer;
        }

        public static Dialer StartDialer(Dialer dialer, bool nonBlocking = false)
        {
            var flags = nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None;
            var err = Interop.DialerStart(dialer, flags);
            ThrowHelper.ThrowIfNotSuccess(err);
            return dialer;
        }

        public static Listener StartListenerr(Listener listener, bool nonBlocking = false)
        {
            var flags = nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None;
            var err = Interop.ListenerStart(listener, flags);
            ThrowHelper.ThrowIfNotSuccess(err);
            return listener;
        }

        public static Dialer CloseDialer(Dialer dialer)
        {
            var err = Interop.DialerClose(dialer);
            ThrowHelper.ThrowIfNotSuccess(err);
            return dialer;
        }

        public static Listener CloseListener(Listener listener)
        {
            var err = Interop.ListenerClose(listener);
            ThrowHelper.ThrowIfNotSuccess(err);
            return listener;
        }

        public static int GetDialerId(Dialer dialer)
        {
            return Interop.GetDialerId(dialer);
        }

        public static int GetListenerId(Listener listener)
        {
            return Interop.GetListenerId(listener);
        }

        // TODO Dialer and Listener option getter/setter functions

        internal static string GetErrorString(nng_errno errorCode)
        {
            return Interop.GetErrorString((int)errorCode);
        }

        public static string GetErrorString(int errorCode)
        {
            return Interop.GetErrorString(errorCode);
        }

        public static bool Send(NNGSocket socket, Span<byte> buffer, bool nonBlocking = false, bool alloc = false)
        {
            var flags = nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None;
            if (alloc)
            {
                flags |= NNGFlag.Alloc;
            }

            nng_errno err;
            unsafe
            {
                fixed (byte* ptr = buffer)
                {
                    err = Interop.Send(socket, new IntPtr(ptr), new UIntPtr((uint)buffer.Length), flags);
                }
            }

            switch (err)
            {
                case nng_errno.NNG_SUCCESS:
                    return true;
                case nng_errno.NNG_EAGAIN:
                    return false;
                default:
                    throw ThrowHelper.GetExceptionForErrorCode(err);
            }
        }

        public static uint Receive(NNGSocket socket, Span<byte> targetBuffer)
        {
            var size = new UIntPtr((uint)targetBuffer.Length);

            nng_errno err;
            unsafe
            {
                fixed (byte* ptr = targetBuffer)
                {
                    err = Interop.Receive(socket, new IntPtr(ptr), ref size, NNGFlag.None);
                }
            }

            ThrowHelper.ThrowIfNotSuccess(err);
            return size.ToUInt32();
        }

        public static unsafe Span<byte> Receive(NNGSocket socket)
        {
            void* pointer;
            var x = &pointer;

            var ptr = new IntPtr(x);
            var size = new UIntPtr();

            var err = Interop.Receive(socket, ptr, ref size, NNGFlag.Alloc);
            ThrowHelper.ThrowIfNotSuccess(err);

            return new Span<byte>(pointer, (int)size.ToUInt32());
        }
    }
}
