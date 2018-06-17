using System;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMember.Global
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void PipeCallback(Pipe pipe, PipeEvent pipeEvent, IntPtr arg);
}
