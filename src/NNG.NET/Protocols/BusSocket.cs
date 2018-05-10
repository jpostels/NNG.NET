using System;
using System.Collections.Generic;
using System.Text;
using NNG.Native;

namespace NNG.Protocols
{
    public class BusSocket : NngBaseSocket
    {
        public BusSocket() : base(Interop.nng_bus0_open)
        {
        }
    }
}
