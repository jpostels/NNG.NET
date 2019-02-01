using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct nng_iov
    {
        public void* iov_buf;

        public UIntPtr iov_len;
    }
}