using System;
using NNGNET.ErrorHandling;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;

namespace NNGNET
{
    public static partial class NNG
    {
        public static NNGContext OpenContext(NNGSocket socket)
        {
            var err = Interop.ContextOpen(out var ctx, socket);
            ThrowHelper.ThrowIfNotSuccess(err);
            return ctx;
        }

        public static void CloseContext(NNGContext context)
        {
            var err = Interop.ContextClose(context);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static int GetContextId(NNGContext context)
        {
            return Interop.GetContextId(context);
        }

        public static unsafe void ReceiveContext(NNGContext context, NNGAIO aio)
        {
            Interop.ContextReceive(context, aio.Handle);
        }

        public static unsafe void SendContext(NNGContext context, NNGAIO aio)
        {
            Interop.ContextSend(context, aio.Handle);
        }

        public static unsafe Span<byte> GetContextOption(NNGContext context, string optionName)
        {
            var err = Interop.ContextGetOption(context, optionName, out var ptr, out var size);
            ThrowHelper.ThrowIfNotSuccess(err);
            return new Span<byte>(ptr, (int)size.ToUInt64());
        }

        public static bool GetContextOptionBool(NNGContext context, string optionName)
        {
            var err = Interop.ContextGetOption(context, optionName, out bool val);
            ThrowHelper.ThrowIfNotSuccess(err);
            return val;
        }

        public static int GetContextOptionInt32(NNGContext context, string optionName)
        {
            var err = Interop.ContextGetOption(context, optionName, out int val);
            ThrowHelper.ThrowIfNotSuccess(err);
            return val;
        }

        public static TimeSpan GetContextOptionTimeSpan(NNGContext context, string optionName)
        {
            var err = Interop.ContextGetOptionDuration(context, optionName, out var val);
            ThrowHelper.ThrowIfNotSuccess(err);
            return TimeSpan.FromMilliseconds(val);
        }

        public static unsafe IntPtr GetContextOptionPointer(NNGContext context, string optionName)
        {
            var err = Interop.ContextGetOption(context, optionName, out void* ptr);
            ThrowHelper.ThrowIfNotSuccess(err);
            return new IntPtr(ptr);
        }

        public static unsafe void SetContextOption(NNGContext context, string optionName, Span<byte> value)
        {
            fixed (byte* ptr = value)
            {
                var err = Interop.ContextSetOption(context, optionName, ptr, new UIntPtr((uint)value.Length));
                ThrowHelper.ThrowIfNotSuccess(err);
            }
        }

        public static void SetContextOption(NNGContext context, string optionName, bool value)
        {
            var err = Interop.ContextSetOption(context, optionName, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static void SetContextOption(NNGContext context, string optionName, int value)
        {
            var err = Interop.ContextSetOption(context, optionName, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static void SetContextOption(NNGContext context, string optionName, TimeSpan value)
        {
            var err = Interop.ContextSetOption(context, optionName, (int) value.TotalMilliseconds);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static void SetContextOption(NNGContext context, string optionName, UIntPtr value)
        {
            var err = Interop.ContextSetOption(context, optionName, value);
            ThrowHelper.ThrowIfNotSuccess(err);
        }
    }
}