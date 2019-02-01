using System;
using NNGNET.ErrorHandling;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;

namespace NNGNET
{
    public static partial class NNG
    {
        public static unsafe void SendAio(NNGSocket socketId, NNGAIO aio)
        {
            Interop.SendAio(socketId, aio.Handle);
        }

        public static unsafe void ReceiveAio(NNGSocket socketId, NNGAIO aio)
        {
            Interop.ReceiveAio(socketId, aio.Handle);
        }

        public static unsafe NNGAIO AllocAio(AioCompletionCallback completionCallback, IntPtr args)
        {
            var err = Interop.AioAlloc(out var ptr, completionCallback, args.ToPointer());
            ThrowHelper.ThrowIfNotSuccess(err);
            return new NNGAIO(ptr);
        }

        public static unsafe void FreeAio(NNGAIO aio)
        {
            Interop.AioFree(aio.Handle);
        }

        public static unsafe void StopAio(NNGAIO aio)
        {
            Interop.AioStop(aio.Handle);
        }

        public static unsafe int GetAioResult(NNGAIO aio)
        {
            return (int) Interop.AioResult(aio.Handle);
        }

        public static unsafe UIntPtr GetAioCount(NNGAIO aio)
        {
            return Interop.AioCount(aio.Handle);
        }

        public static unsafe void CancelAio(NNGAIO aio)
        {
            Interop.AioCancel(aio.Handle);
        }

        public static unsafe void AbortAio(NNGAIO aio, int err)
        {
            Interop.AioAbort(aio.Handle, err);
        }

        public static unsafe void WaitAio(NNGAIO aio)
        {
            Interop.AioWait(aio.Handle);
        }

        public static unsafe void SetAioMessage(NNGAIO aio, NNGMessage message)
        {
            Interop.AioSetMessage(aio.Handle, message.MessageHandle);
        }

        public static unsafe NNGMessage GetAioMessage(NNGAIO aio)
        {
            return new NNGMessage(Interop.AioGetMessage(aio.Handle));
        }

        public static unsafe void SetAioInput(NNGAIO aio, uint index, IntPtr arg)
        {
            var err = Interop.AioSetInput(aio.Handle, index, arg.ToPointer());
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe IntPtr GetAioInput(NNGAIO aio, uint index)
        {
            var ptr = Interop.AioGetInput(aio.Handle, index);
            return new IntPtr(ptr);
        }

        public static unsafe void SetAioOutput(NNGAIO aio, uint index, IntPtr arg)
        {
            var err = Interop.AioSetOutput(aio.Handle, index, arg.ToPointer());
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe IntPtr GetAioOutput(NNGAIO aio, uint index)
        {
            var ptr = Interop.AioGetOutput(aio.Handle, index);
            return new IntPtr(ptr);
        }

        public static void SetAioTimeout(NNGAIO aio, TimeSpan timeout)
        {
            SetAioTimeout(aio, (int) timeout.TotalMilliseconds);
        }

        public static unsafe void SetAioTimeout(NNGAIO aio, int timeoutMilliseconds)
        {
            Interop.AioSetTimeout(aio.Handle, timeoutMilliseconds);
        }

        public static unsafe void SetAioIoVector(NNGAIO aio, uint numberOfIov, nng_iov* iov)
        {
            var err = Interop.AioSetIoVector(aio.Handle, numberOfIov, iov);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static unsafe void FinishAio(NNGAIO aio, int err)
        {
            Interop.AioFinish(aio.Handle, err);
        }

        public static void SleepAio(NNGAIO aio, TimeSpan timeout)
        {
            SleepAio(aio, (int) timeout.TotalMilliseconds);
        }

        public static unsafe void SleepAio(NNGAIO aio, int timeoutMilliseconds)
        {
            Interop.AioSleep(timeoutMilliseconds, aio.Handle);
        }
    }
}