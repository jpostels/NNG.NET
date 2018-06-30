using System.Reflection;

namespace NNGNET.Native
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    using InteropTypes;

    using nng_duration = System.Int32;

    /// <summary>
    ///     Provider for P/Invoke of nng library functions
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Supressed")]
    internal static unsafe class Interop
    {
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments

        /// <summary>
        ///     The librarys name
        /// </summary>
        public const string LibraryName = "nng";

        /// <summary>
        ///     Gets a value indicating whether the interop functions are initialized.
        /// </summary>
        /// <value>
        ///     <c>true</c> if the interop functions are initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        ///     The initialization lock
        /// </summary>
        private static readonly object InitLock = new object();

        /// <summary>
        ///     Initializes the interop functions.
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

            lock (InitLock)
            {
                if (IsInitialized)
                {
                    return;
                }

                // Prelink all P/Invoke functions.
                // This makes sure all methods are functioning correctly
                // NOTE: Does not actually invoke a function call
                Marshal.PrelinkAll(typeof(Interop));

                IsInitialized = true;
            }
        }

        /// <summary>
        ///     The maximum length of a socket address. This includes the terminating NUL.
        /// </summary>
        /// <remarks>
        ///     This limit is built into other implementations, so do not change it.
        /// </remarks>
        public const int NngMaxAddressLength = 128;

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
        [DllImport(LibraryName, EntryPoint = "nng_fini")]
        public static extern void Fini();

        /// <summary>
        ///     The nng_close function closes the supplied socket.
        ///     Messages that have been submitted for sending may be flushed or delivered,
        ///     depending upon the transport and the setting of the NNG_OPT_LINGER option.
        ///     Further attempts to use the socket after this call returns will result in <see cref="nng_errno.NNG_ECLOSED"/>.
        ///     Threads waiting for operations on the socket when this call is executed
        ///     may also return with an <see cref="nng_errno.NNG_ECLOSED"/> result.
        /// </summary>
        /// <param name="socketId">The socket.</param>
        /// <returns>
        ///     This function returns 0 on success, and non-zero otherwise.
        ///     <see cref="nng_errno.NNG_ECLOSED"/>: The socket is already closed or was never opened.
        /// </returns>
        [DllImport(LibraryName, EntryPoint = "nng_close")]
        public static extern nng_errno Close(NNGSocket socketId);

        [DllImport(LibraryName, EntryPoint = "nng_socket_id")]
        public static extern int GetSocketId(NNGSocket socketId);

        [DllImport(LibraryName, EntryPoint = "nng_closeall")]
        public static extern void CloseAll();

#region nng setopt

        [DllImport(LibraryName, EntryPoint = "nng_setopt")]
        public static extern nng_errno SetOption(NNGSocket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* value, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_bool")]
        public static extern nng_errno SetOption(NNGSocket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_int")]
        public static extern nng_errno SetOption(NNGSocket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_size")]
        public static extern nng_errno SetOption(NNGSocket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_ms")]
        public static extern nng_errno SetOptionDuration(NNGSocket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_uint64")]
        public static extern nng_errno SetOption(NNGSocket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_string")]
        public static extern nng_errno SetOption(NNGSocket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport(LibraryName, EntryPoint = "nng_setopt_ptr")]
        public static extern nng_errno SetOptionPointer(NNGSocket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* ptr);

#endregion

#region nng getopt

        [DllImport(LibraryName, EntryPoint = "nng_getopt")]
        public static extern nng_errno GetOption(NNGSocket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* value, out UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_bool")]
        public static extern nng_errno GetOption(NNGSocket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_int")]
        public static extern nng_errno GetOption(NNGSocket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_ms")]
        public static extern nng_errno GetOptionDuration(NNGSocket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_size")]
        public static extern nng_errno GetOption(NNGSocket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_uint64")]
        public static extern nng_errno GetOption(NNGSocket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_ptr")]
        public static extern nng_errno GetOptionPointer(NNGSocket socketId, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* value);

        [DllImport(LibraryName, EntryPoint = "nng_getopt_string")]
        public static extern nng_errno GetOption(NNGSocket sockedId, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string value);

#endregion

        [DllImport(LibraryName, EntryPoint = "nng_pipe_notify")]
        public static extern nng_errno PipeSetNotification(NNGSocket socketId, [MarshalAs(UnmanagedType.I4)] PipeEvent pipeEvent, [MarshalAs(UnmanagedType.FunctionPtr)] PipeCallback callback, void* args);

        [DllImport(LibraryName, EntryPoint = "nng_listen")]
        public static extern nng_errno Listen(NNGSocket sockedId, [MarshalAs(UnmanagedType.LPStr)] string addr, out Listener listener, [MarshalAs(UnmanagedType.I4)] NNGFlag flags);

        [DllImport(LibraryName, EntryPoint = "nng_dial")]
        public static extern nng_errno Dial(NNGSocket sockedId, [MarshalAs(UnmanagedType.LPStr)] string addr, out Dialer listener, [MarshalAs(UnmanagedType.I4)] NNGFlag flags);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_create")]
        public static extern nng_errno DialerCreate(out Dialer dialer, NNGSocket socketId, [MarshalAs(UnmanagedType.LPStr)] string addr);

        [DllImport(LibraryName, EntryPoint = "nng_listener_create")]
        public static extern nng_errno ListenerCreate(out Listener listener, NNGSocket socketId, [MarshalAs(UnmanagedType.LPStr)] string addr);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_start")]
        public static extern nng_errno DialerStart(Dialer dialer, [MarshalAs(UnmanagedType.I4)] NNGFlag flags);

        [DllImport(LibraryName, EntryPoint = "nng_listener_start")]
        public static extern nng_errno ListenerStart(Listener listener, [MarshalAs(UnmanagedType.I4)] NNGFlag flags);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_close")]
        public static extern nng_errno DialerClose(Dialer dialer);

        [DllImport(LibraryName, EntryPoint = "nng_listener_close")]
        public static extern nng_errno ListenerClose(Listener listener);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_id")]
        public static extern int GetDialerId(Dialer dialer);

        [DllImport(LibraryName, EntryPoint = "nng_listener_id")]
        public static extern int GetListenerId(Listener listener);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt")]
        public static extern nng_errno DialerSetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* value, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_bool")]
        public static extern nng_errno DialerSetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_int")]
        public static extern nng_errno DialerSetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_ms")]
        public static extern nng_errno DialerSetOptionDuration(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_size")]
        public static extern nng_errno DialerSetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_uint64")]
        public static extern nng_errno DialerSetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_ptr")]
        public static extern nng_errno DialerSetOptionPointer(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_setopt_string")]
        public static extern nng_errno DialerSetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt")]
        public static extern nng_errno DialerGetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* value, out UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_bool")]
        public static extern nng_errno DialerGetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_int")]
        public static extern nng_errno DialerGetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_ms")]
        public static extern nng_errno DialerGetOptionDuration(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_size")]
        public static extern nng_errno DialerGetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_sockaddr")]
        public static extern nng_errno DialerGetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_sockaddr value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_uint64")]
        public static extern nng_errno DialerGetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_ptr")]
        public static extern nng_errno DialerGetOptionPointer(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* value);

        [DllImport(LibraryName, EntryPoint = "nng_dialer_getopt_string")]
        public static extern nng_errno DialerGetOption(Dialer dialer, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt")]
        public static extern nng_errno ListenerSetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* value, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_bool")]
        public static extern nng_errno ListenerSetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_int")]
        public static extern nng_errno ListenerSetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_ms")]
        public static extern nng_errno ListenerSetOptionDuration(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_size")]
        public static extern nng_errno ListenerSetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_uint64")]
        public static extern nng_errno ListenerSetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_ptr")]
        public static extern nng_errno ListenerSetOptionPointer(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, void* value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_setopt_string")]
        public static extern nng_errno ListenerSetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt")]
        public static extern nng_errno ListenerGetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* value, out UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_bool")]
        public static extern nng_errno ListenerGetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_int")]
        public static extern nng_errno ListenerGetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_ms")]
        public static extern nng_errno ListenerGetOptionDuration(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_size")]
        public static extern nng_errno ListenerGetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_sockaddr")]
        public static extern nng_errno ListenerGetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_sockaddr value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_uint64")]
        public static extern nng_errno ListenerGetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_ptr")]
        public static extern nng_errno ListenerGetOptionPointer(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* value);

        [DllImport(LibraryName, EntryPoint = "nng_listener_getopt_string")]
        public static extern nng_errno ListenerGetOption(Listener listener, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string value);

        [DllImport(LibraryName, EntryPoint = "nng_strerror")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string GetErrorString(int errorCode);

        [DllImport(LibraryName, EntryPoint = "nng_strerror")]
        public static extern char* GetErrorStringUnsafe(int errorCode);

        [DllImport(LibraryName, EntryPoint = "nng_send")]
        public static extern nng_errno Send(NNGSocket socketId, void* ptr, UIntPtr size, [MarshalAs(UnmanagedType.I4)] NNGFlag flags);

        [DllImport(LibraryName, EntryPoint = "nng_recv")]
        public static extern nng_errno Receive(NNGSocket socketId, [Out, In] ref void* ptr, [Out, In] ref UIntPtr size, [MarshalAs(UnmanagedType.I4)] NNGFlag flags);

        [DllImport(LibraryName, EntryPoint = "nng_recv")]
        public static extern nng_errno Receive(NNGSocket socketId, void* ptr, [Out, In] ref UIntPtr size, [MarshalAs(UnmanagedType.I4)] NNGFlag flags);

        [DllImport(LibraryName, EntryPoint = "nng_sendmsg")]
        public static extern nng_errno SendMessage(NNGSocket socketId, nng_msg* message, NNGFlag flags);

        [DllImport(LibraryName, EntryPoint = "nng_recvmsg")]
        public static extern nng_errno ReceiveMessage(NNGSocket socketId, ref nng_msg* message, NNGFlag flags);

        [DllImport(LibraryName, EntryPoint = "nng_send_aio")]
        public static extern void SendAio(NNGSocket socketId, nng_aio* aio);

        [DllImport(LibraryName, EntryPoint = "nng_recv_aio")]
        public static extern void ReceiveAio(NNGSocket socketId, nng_aio* aio);

#region Context support

        [DllImport(LibraryName, EntryPoint = "nng_ctx_open")]
        public static extern nng_errno ContextOpen([Out, In] ref nng_ctx ctx, NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_close")]
        public static extern nng_errno ContextClose(nng_ctx ctx);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_id")]
        public static extern int GetContextId(nng_ctx ctx);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_recv")]
        public static extern void ContextReceive(nng_ctx ctx, nng_aio* aio);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_send")]
        public static extern void ContextSend(nng_ctx ctx, nng_aio* aio);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_getopt")]
        public static extern nng_errno ContextGetOption(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* value, out UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_getopt_bool")]
        public static extern nng_errno ContextGetOption(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool value);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_getopt_int")]
        public static extern nng_errno ContextGetOption(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int value);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_getopt_ms")]
        public static extern nng_errno ContextGetOptionDuration(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration duration);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_getopt_size")]
        public static extern nng_errno ContextGetOption(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* value);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_setopt")]
        public static extern nng_errno ContextSetOption(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, ref void* value, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_setopt_bool")]
        public static extern nng_errno ContextSetOption(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, bool value);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_setopt_int")]
        public static extern nng_errno ContextSetOption(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, int value);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_setopt_ms")]
        public static extern nng_errno ContextSetOptionDuration(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, nng_duration duration);

        [DllImport(LibraryName, EntryPoint = "nng_ctx_setopt_size")]
        public static extern nng_errno ContextSetOption(nng_ctx ctx, [MarshalAs(UnmanagedType.LPStr)] string optionName, UIntPtr value);

#endregion

        [DllImport(LibraryName, EntryPoint = "nng_alloc")]
        public static extern void* Alloc(UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_free")]
        public static extern void Free(void* ptr, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_strdup")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string StringDuplicate([MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImport(LibraryName, EntryPoint = "nng_strfree")]
        public static extern void StringFree([MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImport(LibraryName, EntryPoint = "nng_strfree")]
        public static extern void StringFreeUnsafe(void* str);

        #region AIO

        [DllImport(LibraryName, EntryPoint = "nng_aio_alloc")]
        public static extern nng_errno AioAlloc(out nng_aio* aio, [MarshalAs(UnmanagedType.FunctionPtr)] AioCompletionCallback completionCallback, void* args);

        [DllImport(LibraryName, EntryPoint = "nng_aio_free")]
        public static extern void AioFree(nng_aio* aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_stop")]
        public static extern void AioStop(nng_aio* aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_result")]
        public static extern nng_errno AioResult(nng_aio* aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_count")]
        public static extern UIntPtr AioCount(nng_aio* aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_cancel")]
        public static extern void AioCancel(nng_aio* aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_abort")]
        public static extern void AioAbort(nng_aio* aio, int err);

        [DllImport(LibraryName, EntryPoint = "nng_aio_wait")]
        public static extern void AioWait(nng_aio* aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_set_msg")]
        public static extern void AioSetMessage(nng_aio* aio, nng_msg* msg);

        [DllImport(LibraryName, EntryPoint = "nng_aio_get_msg")]
        public static extern nng_msg* AioGetMessage(nng_aio* aio);

        [DllImport(LibraryName, EntryPoint = "nng_aio_set_input")]
        public static extern nng_errno AioSetInput(nng_aio* aio, uint index, void* arg);

        [DllImport(LibraryName, EntryPoint = "nng_aio_get_input")]
        public static extern void* AioGetInput(nng_aio* aio, uint index);

        [DllImport(LibraryName, EntryPoint = "nng_aio_set_output")]
        public static extern nng_errno AioSetOutput(nng_aio* aio, uint index, void* arg);

        [DllImport(LibraryName, EntryPoint = "nng_aio_get_output")]
        public static extern void* AioGetOutput(nng_aio* aio, uint index);

        [DllImport(LibraryName, EntryPoint = "nng_aio_set_timeout")]
        public static extern void AioSetTimeout(nng_aio* aio, nng_duration timeout);

        [DllImport(LibraryName, EntryPoint = "nng_aio_set_iov")]
        public static extern nng_errno AioSetIoVector(nng_aio* aio, uint niov, nng_iov* iov);

        [DllImport(LibraryName, EntryPoint = "nng_aio_finish")]
        public static extern void AioFinish(nng_aio* aio, int rv);

        [DllImport(LibraryName, EntryPoint = "nng_sleep_aio")]
        public static extern void AioSleep(nng_duration duration, nng_aio* aio);

#endregion AIO

#region Message API

        [DllImport(LibraryName, EntryPoint = "nng_msg_alloc")]
        public static extern nng_errno MessageAlloc(out nng_msg* msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_free")]
        public static extern void MessageFree(nng_msg* msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_realloc")]
        public static extern nng_errno MessageRealloc(nng_msg* msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header")]
        public static extern void* MessageHeader(nng_msg* msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_len")]
        public static extern UIntPtr MessageHeaderLength(nng_msg* msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_body")]
        public static extern void* MessageBody(nng_msg* msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_len")]
        public static extern UIntPtr MessageLength(nng_msg* msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_append")]
        public static extern nng_errno MessageAppend(nng_msg* msg, void* data, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_insert")]
        public static extern nng_errno MessageInsert(nng_msg* msg, void* data, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_trim")]
        public static extern nng_errno MessageTrim(nng_msg* msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_chop")]
        public static extern nng_errno MessageChop(nng_msg* msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_append")]
        public static extern nng_errno MessageHeaderAppend(nng_msg* msg, void* data, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_insert")]
        public static extern nng_errno MessageHeaderInsert(nng_msg* msg, void* data, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_trim")]
        public static extern nng_errno MessageHeaderTrim(nng_msg* msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_chop")]
        public static extern nng_errno MessageHeaderChop(nng_msg* msg, UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_append_u32")]
        public static extern nng_errno MessageHeaderAppend(nng_msg* msg, uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_insert_u32")]
        public static extern nng_errno MessageHeaderInsert(nng_msg* msg, uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_chop_u32")]
        public static extern nng_errno MessageHeaderChop(nng_msg* msg, out uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_trim_u32")]
        public static extern nng_errno MessageHeaderTrim(nng_msg* msg, out uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_append_u32")]
        public static extern nng_errno MessageAppend(nng_msg* msg, uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_insert_u32")]
        public static extern nng_errno MessageInsert(nng_msg* msg, uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_chop_u32")]
        public static extern nng_errno MessageChop(nng_msg* msg, out uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_trim_u32")]
        public static extern nng_errno MessageTrim(nng_msg* msg, out uint val);

        [DllImport(LibraryName, EntryPoint = "nng_msg_dup")]
        public static extern nng_errno MessageDuplicate(out nng_msg* msgDuplicate, nng_msg* msgSource);

        [DllImport(LibraryName, EntryPoint = "nng_msg_clear")]
        public static extern void MessageClear(nng_msg* msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_header_clear")]
        public static extern void MessageHeaderClear(nng_msg* msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_set_pipe")]
        public static extern void MessageSetPipe(nng_msg* msg, Pipe pipe);

        [DllImport(LibraryName, EntryPoint = "nng_msg_get_pipe")]
        public static extern Pipe MessageGetPipe(nng_msg* msg);

        [DllImport(LibraryName, EntryPoint = "nng_msg_getopt")]
        public static extern nng_errno MessageGetOption(nng_msg* msg, int opt, out void* ptr, out UIntPtr size);

#endregion

#region Pipe API

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt")]
        public static extern nng_errno PipeGetOption(Pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* ptr, out UIntPtr size);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_bool")]
        public static extern nng_errno PipeGetOption(Pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out bool val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_int")]
        public static extern nng_errno PipeGetOption(Pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out int val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_ms")]
        public static extern nng_errno PipeGetOptionDuration(Pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_duration duration);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_size")]
        public static extern nng_errno PipeGetOption(Pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out UIntPtr val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_sockaddr")]
        public static extern nng_errno PipeGetOption(Pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out nng_sockaddr val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_uint64")]
        public static extern nng_errno PipeGetOption(Pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out ulong val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_ptr")]
        public static extern nng_errno PipeGetOptionPointer(Pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, out void* val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_getopt_string")]
        public static extern nng_errno PipeGetOption(Pipe pipe, [MarshalAs(UnmanagedType.LPStr)] string optionName, [MarshalAs(UnmanagedType.LPStr)] out string val);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_close")]
        public static extern nng_errno PipeClose(Pipe pipe);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_id")]
        public static extern int GetPipeId(Pipe pipe);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_socket")]
        public static extern NNGSocket GetPipeSocket(Pipe pipe);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_dialer")]
        public static extern Dialer GetPipeDialer(Pipe pipe);

        [DllImport(LibraryName, EntryPoint = "nng_pipe_listener")]
        public static extern Listener GetPipeListener(Pipe pipe);

#endregion

#region Statistics

        // Statistics.  These are for informational purposes only, and subject
        // to change without notice.  The API for accessing these is stable,
        // but the individual statistic names, values, and meanings are all
        // subject to change.

        //BUG the following methods are not yet implemented by nng

        //[DllImport(LibraryName)]
        //public static extern unsafe nng_errno snapshot_create(nng_socket socket, ref nng_snapshot* snapshot);

        //[DllImport(LibraryName)]
        //public static extern void snapshot_free(ref nng_snapshot snapshot);

        //[DllImport(LibraryName)]
        //public static extern nng_errno snapshot_update(ref nng_snapshot snapshot);

        //[DllImport(LibraryName)]
        //public static extern unsafe nng_errno snapshot_next(ref nng_snapshot snapshot, ref nng_stat* stat);

        //[DllImport(LibraryName)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public static extern string stat_name(ref nng_stat stat);

        //[DllImport(LibraryName)]
        //[return: MarshalAs(UnmanagedType.I4)]
        //public static extern nng_stat_type_enum stat_type(ref nng_stat stat);

        //[DllImport(LibraryName)]
        //[return: MarshalAs(UnmanagedType.I4)]
        //public static extern nng_unit_enum stat_unit(ref nng_stat stat);

        //[DllImport(LibraryName)]
        //public static extern long stat_value(ref nng_stat stat);

#endregion

        [DllImport(LibraryName, EntryPoint = "nng_device")]
        public static extern nng_errno Device(NNGSocket socket1, NNGSocket socket2);

#region URL support

        [DllImport(LibraryName, EntryPoint = "nng_url_parse")]
        public static extern nng_errno UrlParse(out nng_url* url, [MarshalAs(UnmanagedType.LPStr)] string str);

        [DllImport(LibraryName, EntryPoint = "nng_url_free")]
        public static extern void UrlFree(nng_url* url);

        [DllImport(LibraryName, EntryPoint = "nng_url_clone")]
        public static extern nng_errno UrlClone(out nng_url* urlDuplicate, nng_url* urlSource);

        #endregion

        /// <summary>
        ///     Report library version
        /// </summary>
        /// <returns></returns>
        [DllImport(LibraryName, EntryPoint = "nng_version")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string GetVersion();

        /// <summary>
        ///     Report library version
        /// </summary>
        /// <returns></returns>
        [DllImport(LibraryName, EntryPoint = "nng_version")]
        public static extern void* GetVersionUnsafe();

#region protocols

        [DllImport(LibraryName, EntryPoint = "nng_req0_open")]
        public static extern nng_errno OpenReq0(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_req0_open_raw")]
        public static extern nng_errno OpenReq0Raw(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_rep0_open")]
        public static extern nng_errno OpenRep0(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_rep0_open_raw")]
        public static extern nng_errno OpenRep0Raw(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_surveyor0_open")]
        public static extern nng_errno OpenSurveyor0(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_surveyor0_open_raw")]
        public static extern nng_errno OpenSurveyor0Raw(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_respondent0_open")]
        public static extern nng_errno OpenRespondent0(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_respondent0_open_raw")]
        public static extern nng_errno OpenRespondent0Raw(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pub0_open")]
        public static extern nng_errno OpenPub0(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pub0_open_raw")]
        public static extern nng_errno OpenPub0Raw(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_sub0_open")]
        public static extern nng_errno OpenSub0(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_sub0_open_raw")]
        public static extern nng_errno OpenSub0Raw(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_push0_open")]
        public static extern nng_errno OpenPush0(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_push0_open_raw")]
        public static extern nng_errno OpenPush0Raw(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pull0_open")]
        public static extern nng_errno OpenPull0(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pull0_open_raw")]
        public static extern nng_errno OpenPull0Raw(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pair0_open")]
        public static extern nng_errno OpenPair0(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pair0_open_raw")]
        public static extern nng_errno OpenPair0Raw(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pair1_open")]
        public static extern nng_errno OpenPair1(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_pair1_open_raw")]
        public static extern nng_errno OpenPair1Raw(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_bus0_open")]
        public static extern nng_errno OpenBus0(out NNGSocket socket);

        [DllImport(LibraryName, EntryPoint = "nng_bus0_open_raw")]
        public static extern nng_errno OpenBus0Raw(out NNGSocket socket);

#endregion

        /*#region transports

        [DllImport(LibraryName, EntryPoint = "nng_inproc_register")]
        public static extern  nng_errno inproc_register();

        [DllImport(LibraryName, EntryPoint = "nng_ipc_register")]
        public static extern  nng_errno ipc_register();

        [DllImport(LibraryName, EntryPoint = "nng_tcp_register")]
        public static extern  nng_errno tcp_register();

        [DllImport(LibraryName, EntryPoint = "nng_ws_register")]
        public static extern  nng_errno ws_register();

        [DllImport(LibraryName, EntryPoint = "nng_wss_register")]
        public static extern  nng_errno wss_register();

        #endregion*/

#pragma warning restore CA2101 // Specify marshaling for P/Invoke string arguments
    }
}