using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNG.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct nni_chunk
    {
        public UIntPtr ch_cap;

        public UIntPtr ch_len;

        public byte* ch_buf;

        public byte* ch_ptr;
    }
}