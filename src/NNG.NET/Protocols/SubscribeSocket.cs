using System;
using System.Collections.Generic;
using System.Text;
using NNGNET.Native;

namespace NNGNET.Protocols
{
    public class SubscribeSocket : NngBaseSocket
    {
        internal const string NNG_OPT_SUB_SUBSCRIBE = "sub:subscribe";

        internal const string NNG_OPT_SUB_UNSUBSCRIBE = "sub:unsubscribe";

        public SubscribeSocket() : base(Interop.nng_sub0_open)
        {
        }
    }
}
