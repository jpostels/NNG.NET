using System;
using NNGNET.ErrorHandling;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;

namespace NNGNET
{
    public static partial class NNG
    {
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
            return new Span<byte>(ptr, (int) len);
        }

        public static unsafe uint GetMessageHeaderLength(NNGMessage message)
        {
            return Interop.MessageHeaderLength(message.MessageHandle).ToUInt32();
        }

        public static unsafe Span<byte> GetMessageBody(NNGMessage message)
        {
            var ptr = Interop.MessageBody(message.MessageHandle);
            var len = Interop.MessageLength(message.MessageHandle);
            return new Span<byte>(ptr, (int) len);
        }

        public static unsafe uint GetMessageLength(NNGMessage message)
        {
            return Interop.MessageLength(message.MessageHandle).ToUInt32();
        }

        public static unsafe void AppendToMessage(NNGMessage message, Span<byte> data)
        {
            fixed (byte* ptr = data)
            {
                var err = Interop.MessageAppend(message.MessageHandle, ptr, (UIntPtr) data.Length);
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
                var err = Interop.MessageInsert(message.MessageHandle, ptr, (UIntPtr) data.Length);
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
            var err = Interop.MessageChop(message.MessageHandle, (UIntPtr) size);
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
                var err = Interop.MessageHeaderAppend(message.MessageHandle, ptr, (UIntPtr) data.Length);
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
                var err = Interop.MessageHeaderInsert(message.MessageHandle, ptr, (UIntPtr) data.Length);
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
            var err = Interop.MessageHeaderTrim(message.MessageHandle, (UIntPtr) size);
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
            var err = Interop.MessageHeaderChop(message.MessageHandle, (UIntPtr) size);
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

        public static unsafe void ClearMessage(NNGMessage message)
        {
            Interop.MessageClear(message.MessageHandle);
        }

        public static unsafe void ClearMessageHeader(NNGMessage message)
        {
            Interop.MessageHeaderClear(message.MessageHandle);
        }

        public static unsafe void SetMessagePipe(NNGMessage message, Pipe pipe)
        {
            Interop.MessageSetPipe(message.MessageHandle, pipe);
        }

        public static unsafe Pipe GetMessagePipe(NNGMessage message)
        {
            return Interop.MessageGetPipe(message.MessageHandle);
        }

        public static unsafe Span<byte> GetMessageOption(NNGMessage message, int option)
        {
            var err = Interop.MessageGetOption(message.MessageHandle, option, out var ptr, out var size);
            ThrowHelper.ThrowIfNotSuccess(err);
            return new Span<byte>(ptr, (int) size);
        }
    }
}