using System;
using System.Collections.Generic;
using System.Text;
using NNG.Native;

namespace NNG.Protocols
{
    public class PullSocket : NngBaseSocket
    {
        public PullSocket() : base(Interop.nng_pull0_open)
        {
        }
    }
}
