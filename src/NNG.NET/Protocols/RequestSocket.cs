using System;
using System.Collections.Generic;
using System.Text;
using NNGNET.Native;

namespace NNGNET.Protocols
{
    public class RequestSocket : NngBaseSocket
    {
        internal const string NNG_OPT_REQ_RESENDTIME = "req:resend-time";

        public RequestSocket() : base(Interop.OpenReq0)
        {
        }
    }
}
