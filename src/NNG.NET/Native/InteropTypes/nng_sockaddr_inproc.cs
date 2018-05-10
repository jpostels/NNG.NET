﻿using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNG.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct nng_sockaddr_inproc
    {
        public nng_sockaddr_family sa_family;

        public fixed char sa_name[Interop.NngMaxAddressLength];
    }
}