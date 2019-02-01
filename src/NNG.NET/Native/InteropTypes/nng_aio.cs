using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void AioCompletionCallback(void* ptr);

    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct nng_aio
    {
#pragma warning disable RCS1169 // Mark field as read-only.
#pragma warning disable RCS1213 // Remove unused member declaration.
#pragma warning disable IDE0044 // Add readonly modifier
        // ReSharper disable FieldCanBeMadeReadOnly.Local

        public int a_result;

        public UIntPtr a_count;

        public ulong a_expire;

        public uint a_timeout;

        private byte a_stop; // bool

        private byte a_closed; // bool

        private byte a_sleep; // bool

        private int a_sleeprv;

        private nni_task* a_task;

        public nni_iov* a_iov;

        public uint a_niov;

        public nni_iov a_iovinl0;
        public nni_iov a_iovinl1;
        public nni_iov a_iovinl2;
        public nni_iov a_iovinl3;

        public nni_iov* a_iovalloc;

        public uint a_niovalloc;

        public nni_msg* a_msg;

        public void* a_user_data0;
        public void* a_user_data1;
        public void* a_user_data2;
        public void* a_user_data3;

        public void* a_inputs0;
        public void* a_inputs1;
        public void* a_inputs2;
        public void* a_inputs3;

        public void* a_outputs0;
        public void* a_outputs1;
        public void* a_outputs2;
        public void* a_outputs3;

        internal IntPtr a_prov_cancel;

        private void* a_prov_data;

        private nni_list_node a_prov_node;

        private void* a_prov_extra0;
        private void* a_prov_extra1;
        private void* a_prov_extra2;
        private void* a_prov_extra3;

        private nni_list_node a_expire_node;

        // ReSharper restore FieldCanBeMadeReadOnly.Local
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore RCS1213 // Remove unused member declaration.
#pragma warning restore RCS1169 // Mark field as read-only.
    }

    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct nni_iov
    {
        public void* iov_buf;

        public UIntPtr iov_len;
    }

    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    internal struct nni_msg
    {
        // Implementation details not required.
    }

    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    internal struct nni_task
    {
        // Implementation details not required.
    }

    internal unsafe delegate void AioCancelFunction(nng_aio* aio, int val);

    // ReSharper disable once InconsistentNaming
    public readonly unsafe struct NNGAIO : IDisposable, IEquatable<NNGAIO>
    {
        internal readonly nng_aio* Handle;

        internal NNGAIO(nng_aio* aio)
        {
            Handle = aio;
        }

        //internal ref nng_aio AsRef()
        //{
        //    return ref Unsafe.AsRef<nng_aio>(Handle);
        //}

        internal AioCancelFunction CancelFunction => Marshal.GetDelegateForFunctionPointer<AioCancelFunction>(Handle[0].a_prov_cancel);

        /// <inheritdoc />
        public void Dispose()
        {
            NNG.FreeAio(this);
        }

        public static NNGAIO Create(AioCompletionCallback completionCallback, IntPtr args) => NNG.AllocAio(completionCallback, args);

        public void Stop() => NNG.StopAio(this);

        public int GetResult() => NNG.GetAioResult(this);

        public UIntPtr GetCount() => NNG.GetAioCount(this);

        public void Cancel() => NNG.CancelAio(this);

        public void Abort(int err) => NNG.AbortAio(this, err);

        public void Wait() => NNG.WaitAio(this);

        public NNGMessage Message
        {
            get => NNG.GetAioMessage(this);
            set => NNG.SetAioMessage(this, value);
        }

        public void SetInput(uint index, IntPtr arg) => NNG.SetAioInput(this, index, arg);

        public IntPtr GetInput(uint index) => NNG.GetAioInput(this, index);

        public void SetOutput(uint index, IntPtr arg) => NNG.SetAioOutput(this, index, arg);

        public IntPtr GetOutput(uint index) => NNG.GetAioOutput(this, index);

        public void SetTimeout(TimeSpan timeout) => NNG.SetAioTimeout(this, timeout);

        public void SetTimeout(int timeout) => NNG.SetAioTimeout(this, timeout);

        public void SetIoVector(nng_iov* iov, uint num) => NNG.SetAioIoVector(this, num, iov);

        public void Finish(int err) => NNG.FinishAio(this, err);

        public void Sleep(TimeSpan duration) => NNG.SleepAio(this, duration);

        public void Sleep(int duration) => NNG.SleepAio(this, duration);

        /// <inheritdoc />
        public bool Equals(NNGAIO other)
        {
            return Handle == other.Handle;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is NNGAIO nngaio && Equals(nngaio);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return unchecked((int) (long) Handle);
        }

        public static bool operator ==(NNGAIO left, NNGAIO right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(NNGAIO left, NNGAIO right)
        {
            return !left.Equals(right);
        }
    }
}