using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNG.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct nng_iov
    {
        public void* iov_buf;

        public UIntPtr iov_len;
    }
}