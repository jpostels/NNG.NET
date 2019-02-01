using System;
using NNGNET.ErrorHandling;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;

namespace NNGNET
{
    public static partial class NNG
    {
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
            return new Span<byte>(pointer, (int) size.ToUInt32());
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
    }
}