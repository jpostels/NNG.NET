using System.Runtime.InteropServices;

namespace NNG.Native.InteropTypes
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMember.Global
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal unsafe delegate void nng_pipe_cb(nng_pipe pipe, nng_pipe_ev action, void* arg);
}
