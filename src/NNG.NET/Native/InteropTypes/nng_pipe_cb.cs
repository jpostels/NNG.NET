using System;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMember.Global
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void nng_pipe_cb(nng_pipe pipe, nng_pipe_ev action, IntPtr arg);
}
