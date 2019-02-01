using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct nni_list_node
    {
        internal nni_list_node* ln_next;

        internal nni_list_node* ln_prev;
    }
}