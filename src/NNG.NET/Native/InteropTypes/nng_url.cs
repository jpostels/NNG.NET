using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct nng_url
    {
        public char* u_rawurl;

        public char* u_scheme;

        public char* u_userinfo;

        public char* u_host;

        public char* u_hostname;

        public char* u_port;

        public char* u_path;

        public char* u_query;

        public char* u_fragment;

        public char* u_requri;
    }
}