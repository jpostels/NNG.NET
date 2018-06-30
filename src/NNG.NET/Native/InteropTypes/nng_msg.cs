using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using nni_time = System.UInt64;

namespace NNGNET.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    internal struct nng_msg
    {
        internal nni_chunk m_header;

        internal nni_chunk m_body;

        internal nni_time m_expire; // µsec

        internal nni_list m_options;

        internal uint m_pipe; // set on receive
    }

    public readonly unsafe struct NNGMessage : IDisposable
    {
        internal readonly nng_msg* MessageHandle;

        internal NNGMessage(nng_msg* messageHandle)
        {
            MessageHandle = messageHandle;
        }

        public static NNGMessage Create(uint size) => NNG.AllocMessage(size);

        public uint GetLength() => NNG.GetMessageLength(this);

        public void Realloc(uint newSize) => NNG.ReallocMessage(this, newSize);

        public Span<byte> GetSpan() => NNG.GetMessageBody(this);

        public Span<byte> GetHeader() => NNG.GetMessageHeader(this);

        public void Append(Span<byte> data) => NNG.AppendToMessage(this, data);

        public void Append(uint value) => NNG.AppendToMessage(this, value);

        public void Prepend(Span<byte> data) => NNG.PrependToMessage(this, data);

        public void Prepend(uint value) => NNG.PrependToMessage(this, value);

        public void Trim(uint size) => NNG.TrimMessage(this, size);

        public uint TrimUInt32() => NNG.TrimMessageUInt32(this);

        public void Chop(uint size) => NNG.ChopMessage(this, size);

        public uint ChopUInt32() => NNG.ChopMessageUInt32(this);

        public void AppendHeader(Span<byte> data) => NNG.AppendToMessageHeader(this, data);

        public void AppendHeader(uint value) => NNG.AppendToMessageHeader(this, value);

        public void PrependHeader(Span<byte> data) => NNG.PrependToMessageHeader(this, data);

        public void PrependHeader(uint value) => NNG.PrependToMessageHeader(this, value);

        public void TrimHeader(uint size) => NNG.TrimMessageHeader(this, size);

        public uint TrimHeaderUInt32() => NNG.TrimMessageHeaderUInt32(this);

        public void ChopHeader(uint size) => NNG.ChopMessageHeader(this, size);

        public uint ChopHeaderUInt32() => NNG.ChopMessageHeaderUInt32(this);

        public NNGMessage Duplicate() => NNG.DuplicateMessage(this);

        public void Clear() => NNG.ClearMessage(this);

        public void ClearHeader() => NNG.ClearMessageHeader(this);

        public void SetPipe(Pipe pipe) => NNG.SetMessagePipe(this, pipe);

        public Pipe GetPipe() => NNG.GetMessagePipe(this);

        public Span<byte> GetOption(int option) => NNG.GetMessageOption(this, option);

        /// <inheritdoc />
        public void Dispose()
        {
            NNG.FreeMessage(this);
        }
    }
}