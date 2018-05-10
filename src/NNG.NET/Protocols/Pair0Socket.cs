using System;
using System.Collections.Generic;
using System.Text;
using NNG.Native;

namespace NNG.Protocols
{
    public class Pair0Socket : NngBaseSocket
    {
        public Pair0Socket() : base(Interop.nng_pair0_open)
        {
        }
    }
}
