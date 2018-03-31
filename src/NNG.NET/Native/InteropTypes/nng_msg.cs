using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNG.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    public struct nng_msg
    {
        public nni_chunk m_header;

        public nni_chunk m_body;

        public nni_time m_expire; // µsec

        public nni_list m_options;

        public uint m_pipe; // set on receive
    }
}