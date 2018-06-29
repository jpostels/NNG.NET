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

        public uint GetLength()
        {
            return Interop.MessageLength(MessageHandle).ToUInt32();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Interop.MessageFree(MessageHandle);
        }
    }
}