using System;
using System.Collections.Generic;
using System.Text;
using NNG.Native;

namespace NNG.Protocols
{
    public class RequestSocket : NngBaseSocket
    {
        internal const string NNG_OPT_REQ_RESENDTIME = "req:resend-time";

        public RequestSocket() : base(Interop.nng_req0_open)
        {
        }
    }
}
