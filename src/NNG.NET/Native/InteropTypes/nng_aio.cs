using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    public struct nng_aio
    {
        // TODO

        //char* u_rawurl;   // never NULL
        //char* u_scheme;   // never NULL
        //char* u_userinfo; // will be NULL if not specified
        //char* u_host;     // including colon and port
        //char* u_hostname; // name only, will be "" if not specified
        //char* u_port;     // port, will be "" if not specified
        //char* u_path;     // path, will be "" if not specified
        //char* u_query;    // without '?', will be NULL if not specified
        //char* u_fragment; // without '#', will be NULL if not specified
        //char* u_requri;   // includes query and fragment, "" if not specified
    }
}