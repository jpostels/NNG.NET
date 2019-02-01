using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace NNGNET.Native.InteropTypes
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native name")]
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct nng_url
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

    public sealed class NNGUrl
    {
        /// <inheritdoc />
        internal unsafe NNGUrl(nng_url* url)
        {
            RawUrl = Marshal.PtrToStringAnsi((IntPtr)url[0].u_rawurl);
            Scheme = Marshal.PtrToStringAnsi((IntPtr)url[0].u_scheme);
            UserInfo = Marshal.PtrToStringAnsi((IntPtr)url[0].u_userinfo);
            Host = Marshal.PtrToStringAnsi((IntPtr)url[0].u_host);
            HostName = Marshal.PtrToStringAnsi((IntPtr)url[0].u_hostname);
            Port = Marshal.PtrToStringAnsi((IntPtr)url[0].u_port);
            Path = Marshal.PtrToStringAnsi((IntPtr)url[0].u_path);
            Query = Marshal.PtrToStringAnsi((IntPtr)url[0].u_query);
            Fragment = Marshal.PtrToStringAnsi((IntPtr)url[0].u_fragment);
            ReqUri = Marshal.PtrToStringAnsi((IntPtr)url[0].u_requri);
        }

        public string RawUrl { get; }

        public string Scheme { get; }

        public string UserInfo { get; }

        public string Host { get; }

        public string HostName { get; }

        public string Port { get; }

        public string Path { get; }

        public string Query { get; }

        public string Fragment { get; }

        public string ReqUri { get; }
    }
}