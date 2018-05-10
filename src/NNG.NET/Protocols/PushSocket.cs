using System;
using System.Collections.Generic;
using System.Text;
using NNG.Native;

namespace NNG.Protocols
{
    public class PushSocket : NngBaseSocket
    {
        public PushSocket() : base(Interop.nng_push0_open)
        {
        }
    }
}
