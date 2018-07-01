using System;
using NNGNET.ErrorHandling;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;

namespace NNGNET
{
    public static partial class NNG
    {
        public static unsafe Span<byte> GetPipeOption(Pipe pipe, string optionName)
        {
            var err = Interop.PipeGetOption(pipe, optionName, out var ptr, out var size);
            ThrowHelper.ThrowIfNotSuccess(err);
            return new Span<byte>(ptr, (int) size.ToUInt64());
        }

        public static bool GetPipeOptionBool(Pipe pipe, string optionName)
        {
            var err = Interop.PipeGetOption(pipe, optionName, out bool val);
            ThrowHelper.ThrowIfNotSuccess(err);
            return val;
        }

        public static int GetPipeOptionInt32(Pipe pipe, string optionName)
        {
            var err = Interop.PipeGetOption(pipe, optionName, out int val);
            ThrowHelper.ThrowIfNotSuccess(err);
            return val;
        }

        public static TimeSpan GetPipeOptionTimeSpan(Pipe pipe, string optionName)
        {
            var err = Interop.PipeGetOptionDuration(pipe, optionName, out var val);
            ThrowHelper.ThrowIfNotSuccess(err);
            return TimeSpan.FromMilliseconds(val);
        }

        public static UIntPtr GetPipeOptionSize(Pipe pipe, string optionName)
        {
            var err = Interop.PipeGetOption(pipe, optionName, out UIntPtr val);
            ThrowHelper.ThrowIfNotSuccess(err);
            return val;
        }

        public static nng_sockaddr GetPipeOptionAddress(Pipe pipe, string optionName)
        {
            var err = Interop.PipeGetOption(pipe, optionName, out nng_sockaddr val);
            ThrowHelper.ThrowIfNotSuccess(err);
            return val;
        }

        public static ulong GetPipeOptionUInt64(Pipe pipe, string optionName)
        {
            var err = Interop.PipeGetOption(pipe, optionName, out ulong val);
            ThrowHelper.ThrowIfNotSuccess(err);
            return val;
        }

        public static unsafe IntPtr GetPipeOptionPointer(Pipe pipe, string optionName)
        {
            var err = Interop.PipeGetOptionPointer(pipe, optionName, out var val);
            ThrowHelper.ThrowIfNotSuccess(err);
            return new IntPtr(val);
        }

        public static string GetPipeOptionString(Pipe pipe, string optionName)
        {
            var err = Interop.PipeGetOption(pipe, optionName, out string val);
            ThrowHelper.ThrowIfNotSuccess(err);
            return val;
        }

        public static void ClosePipe(Pipe pipe)
        {
            var err = Interop.PipeClose(pipe);
            ThrowHelper.ThrowIfNotSuccess(err);
        }

        public static int GetPipeId(Pipe pipe)
        {
            return Interop.GetPipeId(pipe);
        }

        public static NNGSocket GetPipeSocket(Pipe pipe)
        {
            return Interop.GetPipeSocket(pipe);
        }

        public static Dialer GetPipeDialer(Pipe pipe)
        {
            return Interop.GetPipeDialer(pipe);
        }

        public static Listener GetPipeListener(Pipe pipe)
        {
            return Interop.GetPipeListener(pipe);
        }
    }
}