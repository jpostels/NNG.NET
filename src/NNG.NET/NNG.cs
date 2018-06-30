using System.Buffers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

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
        public static unsafe void SetOption(NNGSocket socket, SocketOption option, IntPtr pointer, uint size)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.SetOption(socket, optionName, pointer.ToPointer(), new UIntPtr(size));
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
        public static unsafe void SetOptionPointer(NNGSocket socket, SocketOption option, IntPtr pointer)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.SetOptionPointer(socket, optionName, pointer.ToPointer());
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
        public static unsafe Span<byte> GetOption(NNGSocket socket, SocketOption option)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.GetOption(socket, optionName, out var pointer, out var size);

            ThrowHelper.ThrowIfNotSuccess(err);
            return new Span<byte>(pointer, (int)size.ToUInt32());
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
        public static unsafe IntPtr GetOptionPointer(NNGSocket socket, SocketOption option)
        {
            var optionName = OptionNames.GetNameByEnum(option);
            var err = Interop.GetOptionPointer(socket, optionName, out var value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return new IntPtr(value);
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
        public static unsafe void SetPipeNotification(NNGSocket socket, PipeEvent pipeEvent, PipeCallback callback, IntPtr args = default)
        {
            var err = Interop.PipeSetNotification(socket, pipeEvent, callback, args != default ? args.ToPointer() : IntPtr.Zero.ToPointer());
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
                    err = Interop.Send(socket, ptr, new UIntPtr((uint)buffer.Length), flags);
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
                ref var rp = ref targetBuffer.GetPinnableReference();
                fixed (byte* p = &rp)
                {
                    err = Interop.Receive(socket, p, ref size, NNGFlag.None);
                }
            }

            ThrowHelper.ThrowIfNotSuccess(err);
            return size.ToUInt32();
        }

        public static unsafe Span<byte> Receive(NNGSocket socket)
        {
            void* ptr = null;
            var size = new UIntPtr();

            var err = Interop.Receive(socket, ref ptr, ref size, NNGFlag.Alloc);
            ThrowHelper.ThrowIfNotSuccess(err);

            return new Span<byte>(ptr, (int)size.ToUInt32());
        }

        private sealed unsafe class NanoMem : MemoryManager<byte>
        {
            private readonly void* _handle;

            private readonly int _length;

            public NanoMem(void* pointer, UIntPtr size)
            {
                _handle = pointer;
                _length = (int)size.ToUInt32();
            }

            public NanoMem(void* pointer, int size)
            {
                _handle = pointer;
                _length = size;
            }

            /// <inheritdoc />
            protected override void Dispose(bool disposing)
            {
                NNG.Free(GetSpan());
                //Interop.Free(_handle, _length);
            }

            /// <inheritdoc />
            public override Span<byte> GetSpan()
            {
                return new Span<byte>(_handle, _length);
            }

            /// <inheritdoc />
            public override MemoryHandle Pin(int elementIndex = 0)
            {
                return new MemoryHandle(Unsafe.Add<byte>(_handle, elementIndex));
            }

            /// <inheritdoc />
            public override void Unpin()
            {

            }
        }

        public static unsafe void SendMessage(NNGSocket socket, NNGMessage message)
        {
            var err = Interop.SendMessage(socket, message.MessageHandle, NNGFlag.None);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe bool TrySendMessage(NNGSocket socket, NNGMessage message)
        {
            var err = Interop.SendMessage(socket, message.MessageHandle, NNGFlag.NonBlocking);

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

        public static unsafe NNGMessage ReceiveMessage(NNGSocket socket)
        {
            nng_msg* ptr = default;
            var err = Interop.ReceiveMessage(socket, ref ptr, NNGFlag.None);
            ThrowHelper.ThrowIfNotSuccess(err);
            return new NNGMessage(ptr);
        }

        public static unsafe bool TryReceiveMessage(NNGSocket socket, out NNGMessage message, bool nonBlocking = false)
        {
            nng_msg* ptr = default;
            var err = Interop.ReceiveMessage(socket, ref ptr, nonBlocking ? NNGFlag.NonBlocking : NNGFlag.None);

            switch (err)
            {
                case nng_errno.NNG_SUCCESS:
                    message = new NNGMessage(ptr);
                    return true;
                case nng_errno.NNG_EAGAIN:
                    message = default;
                    return false;
                default:
                    throw ThrowHelper.GetExceptionForErrorCode(err);
            }
        }

        public static void SendAio(NNGSocket socketId, ref nng_aio aio)
        {
            throw new NotImplementedException();
        }

        public static void ReceiveAio(NNGSocket socketId, ref nng_aio aio)
        {
            throw new NotImplementedException();
        }

        // TODO Context support

        public static Span<byte> Alloc(uint size)
        {
            unsafe
            {
                var ptr = Interop.Alloc(new UIntPtr(size));
                return new Span<byte>(ptr, (int)size);
            }
        }

        public static unsafe void Free(Span<byte> buffer)
        {
            fixed (byte* ptr = buffer)
            {
                Interop.Free(ptr, new UIntPtr((uint)buffer.Length));
            }
        }

        public static string DuplicateString(string s) => Interop.StringDuplicate(s);

        [Obsolete("Not supported.", true)]
        public static void FreeString(string s) => throw new NotSupportedException();

        #region AIO
        // TODO
        #endregion

        #region Message API

        public static unsafe NNGMessage AllocMessage(uint size)
        {
            var err = Interop.MessageAlloc(out var ptr, new UIntPtr(size));
            ThrowHelper.ThrowIfNotSuccess(err);
            return new NNGMessage(ptr);
        }

        public static unsafe void FreeMessage(NNGMessage message)
        {
            Interop.MessageFree(message.MessageHandle);
        }

        public static unsafe void ReallocMessage(NNGMessage message, uint newSize)
        {
            var err = Interop.MessageRealloc(message.MessageHandle, new UIntPtr(newSize));
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe Span<byte> GetMessageHeader(NNGMessage message)
        {
            var ptr = Interop.MessageHeader(message.MessageHandle);
            var len = Interop.MessageHeaderLength(message.MessageHandle);
            return new Span<byte>(ptr, (int)len);
        }

        public static unsafe uint GetMessageHeaderLength(NNGMessage message)
        {
            return Interop.MessageHeaderLength(message.MessageHandle).ToUInt32();
        }

        public static unsafe Span<byte> GetMessageBody(NNGMessage message)
        {
            var ptr = Interop.MessageBody(message.MessageHandle);
            var len = Interop.MessageLength(message.MessageHandle);
            return new Span<byte>(ptr, (int)len);
        }

        public static unsafe uint GetMessageLength(NNGMessage message)
        {
            return Interop.MessageLength(message.MessageHandle).ToUInt32();
        }

        public static unsafe void AppendToMessage(NNGMessage message, Span<byte> data)
        {
            fixed (byte* ptr = data)
            {
                var err = Interop.MessageAppend(message.MessageHandle, ptr, (UIntPtr)data.Length);
                ThrowHelper.ThrowIfNotSuccess(err);
            }
        }

        public static unsafe void AppendToMessage(NNGMessage message, uint value)
        {
            var err = Interop.MessageAppend(message.MessageHandle, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe void PrependToMessage(NNGMessage message, Span<byte> data)
        {
            fixed (byte* ptr = data)
            {
                var err = Interop.MessageInsert(message.MessageHandle, ptr, (UIntPtr)data.Length);
                ThrowHelper.ThrowIfNotSuccess(err);
            }
        }

        public static unsafe void PrependToMessage(NNGMessage message, uint value)
        {
            var err = Interop.MessageInsert(message.MessageHandle, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe void TrimMessage(NNGMessage message, uint size)
        {
            var err = Interop.MessageTrim(message.MessageHandle, (UIntPtr) size);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe uint TrimMessageUInt32(NNGMessage message)
        {
            var err = Interop.MessageTrim(message.MessageHandle, out var value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return value;
        }

        public static unsafe void ChopMessage(NNGMessage message, uint size)
        {
            var err = Interop.MessageChop(message.MessageHandle, (UIntPtr)size);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe uint ChopMessageUInt32(NNGMessage message)
        {
            var err = Interop.MessageChop(message.MessageHandle, out var value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return value;
        }
        public static unsafe void AppendToMessageHeader(NNGMessage message, Span<byte> data)
        {
            fixed (byte* ptr = data)
            {
                var err = Interop.MessageHeaderAppend(message.MessageHandle, ptr, (UIntPtr)data.Length);
                ThrowHelper.ThrowIfNotSuccess(err);
            }
        }

        public static unsafe void AppendToMessageHeader(NNGMessage message, uint value)
        {
            var err = Interop.MessageHeaderAppend(message.MessageHandle, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe void PrependToMessageHeader(NNGMessage message, Span<byte> data)
        {
            fixed (byte* ptr = data)
            {
                var err = Interop.MessageHeaderInsert(message.MessageHandle, ptr, (UIntPtr)data.Length);
                ThrowHelper.ThrowIfNotSuccess(err);
            }
        }

        public static unsafe void PrependToMessageHeader(NNGMessage message, uint value)
        {
            var err = Interop.MessageHeaderInsert(message.MessageHandle, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe void TrimMessageHeader(NNGMessage message, uint size)
        {
            var err = Interop.MessageHeaderTrim(message.MessageHandle, (UIntPtr)size);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe uint TrimMessageHeaderUInt32(NNGMessage message)
        {
            var err = Interop.MessageHeaderTrim(message.MessageHandle, out var value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return value;
        }

        public static unsafe void ChopMessageHeader(NNGMessage message, uint size)
        {
            var err = Interop.MessageHeaderChop(message.MessageHandle, (UIntPtr)size);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe uint ChopMessageHeaderUInt32(NNGMessage message)
        {
            var err = Interop.MessageHeaderChop(message.MessageHandle, out var value);
            ThrowHelper.ThrowIfNotSuccess(err);
            return value;
        }

        public static unsafe NNGMessage DuplicateMessage(NNGMessage message)
        {
            var err = Interop.MessageDuplicate(out var duplicate, message.MessageHandle);
            ThrowHelper.ThrowIfNotSuccess(err);
            return new NNGMessage(duplicate);
        }

        public static unsafe void ClearMessage(NNGMessage message) => Interop.MessageClear(message.MessageHandle);

        public static unsafe void ClearMessageHeader(NNGMessage message) => Interop.MessageHeaderClear(message.MessageHandle);

        public static unsafe void SetMessagePipe(NNGMessage message, Pipe pipe) => Interop.MessageSetPipe(message.MessageHandle, pipe);

        public static unsafe Pipe GetMessagePipe(NNGMessage message) => Interop.MessageGetPipe(message.MessageHandle);

        public static unsafe Span<byte> GetMessageOption(NNGMessage message, int option)
        {
            var err = Interop.MessageGetOption(message.MessageHandle, option, out var ptr, out var size);
            ThrowHelper.ThrowIfNotSuccess(err);
            return new Span<byte>(ptr, (int) size);
        }

        #endregion

        #region Pipe API
        // TODO
        #endregion

        #region Statistics
        // TODO
        // Note: Statistic support is currently not implemented by NNG itself.
        #endregion

        public static void Forward(NNGSocket socket1, NNGSocket socket2)
        {
            var err = Interop.Device(socket1, socket2);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        #region URL support

        public static unsafe NNGUrl ParseUrl(string address)
        {
            nng_url* ptr = default;
            try
            {
                var err = Interop.UrlParse(out ptr, address);
                ThrowHelper.ThrowIfNotSuccess(err);
                return new NNGUrl(ptr);
            }
            finally
            {
                if (ptr != default)
                {
                    NNG.FreeUrl(ptr);
                }
            }
        }

        private static unsafe void FreeUrl(nng_url* url) => Interop.UrlFree(url);

        #endregion

        public static unsafe string GetVersionString()
        {
            var ptr = Interop.GetVersionUnsafe();
            return Marshal.PtrToStringAnsi(new IntPtr(ptr));
        }

        private static Version ConstructVersion(string versionString)
        {
            return versionString == null ? null : new Version(versionString);
        }

        private static readonly Lazy<Version> LazyVersion = new Lazy<Version>(() => ConstructVersion(GetVersionString()));

        public static Version Version => LazyVersion.Value;

        #region Protocols

        private delegate nng_errno SocketOpenFunction(out NNGSocket socket);

        private static NNGSocket OpenSocket(SocketOpenFunction socketOpenFunction)
        {
            var err = socketOpenFunction(out var socket);
            ThrowHelper.ThrowIfNotSuccess(err);
            return socket;
        }

        public static NNGSocket OpenReq0() => OpenSocket(Interop.OpenReq0);

        public static NNGSocket OpenReq0Raw() => OpenSocket(Interop.OpenReq0Raw);

        public static NNGSocket OpenRep0() => OpenSocket(Interop.OpenRep0);

        public static NNGSocket OpenRep0Raw() => OpenSocket(Interop.OpenRep0Raw);

        public static NNGSocket OpenSurveyor0() => OpenSocket(Interop.OpenSurveyor0);

        public static NNGSocket OpenSurveyor0Raw() => OpenSocket(Interop.OpenSurveyor0Raw);

        public static NNGSocket OpenRespondent0() => OpenSocket(Interop.OpenRespondent0);

        public static NNGSocket OpenRespondent0Raw() => OpenSocket(Interop.OpenRespondent0Raw);

        public static NNGSocket OpenPub0() => OpenSocket(Interop.OpenPub0);

        public static NNGSocket OpenPub0Raw() => OpenSocket(Interop.OpenPub0Raw);

        public static NNGSocket OpenSub0() => OpenSocket(Interop.OpenSub0);

        public static NNGSocket OpenSub0Raw() => OpenSocket(Interop.OpenSub0Raw);

        public static NNGSocket OpenPush0() => OpenSocket(Interop.OpenPush0);

        public static NNGSocket OpenPush0Raw() => OpenSocket(Interop.OpenPush0Raw);

        public static NNGSocket OpenPull0() => OpenSocket(Interop.OpenPull0);

        public static NNGSocket OpenPull0Raw() => OpenSocket(Interop.OpenPull0Raw);

        public static NNGSocket OpenPair0() => OpenSocket(Interop.OpenPair0);

        public static NNGSocket OpenPair0Raw() => OpenSocket(Interop.OpenPair0Raw);

        public static NNGSocket OpenPair1() => OpenSocket(Interop.OpenPair1);

        public static NNGSocket OpenPair1Raw() => OpenSocket(Interop.OpenPair1Raw);

        public static NNGSocket OpenBus0() => OpenSocket(Interop.OpenBus0);

        public static NNGSocket OpenBus0Raw() => OpenSocket(Interop.OpenBus0Raw);

        #endregion
    }
}
