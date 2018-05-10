using System.Diagnostics.CodeAnalysis;

namespace NNG.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [SuppressMessage("ReSharper", "EnumUnderlyingTypeIsInt", Justification = "Being explicit here.")]
    internal enum nng_pipe_action : int
    {
        NNG_PIPE_ADD,

        NNG_PIPE_REM
    }
}