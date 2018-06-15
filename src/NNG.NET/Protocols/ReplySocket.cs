using System;
using System.Collections.Generic;
using System.Text;
using NNGNET.Native;

namespace NNGNET.Protocols
{
    public class ReplySocket : NngBaseSocket
    {
        public ReplySocket() : base(Interop.nng_rep0_open)
        {
        }
    }
}
