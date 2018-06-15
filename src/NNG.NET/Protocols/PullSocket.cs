using System;
using System.Collections.Generic;
using System.Text;
using NNGNET.Native;

namespace NNGNET.Protocols
{
    public class PullSocket : NngBaseSocket
    {
        public PullSocket() : base(Interop.nng_pull0_open)
        {
        }
    }
}
