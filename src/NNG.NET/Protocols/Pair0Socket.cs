using System;
using System.Collections.Generic;
using System.Text;
using NNGNET.Native;

namespace NNGNET.Protocols
{
    public class Pair0Socket : NngBaseSocket
    {
        public Pair0Socket() : base(Interop.OpenPair0)
        {
        }
    }
}
