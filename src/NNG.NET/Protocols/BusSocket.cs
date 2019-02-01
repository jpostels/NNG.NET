using System;
using System.Collections.Generic;
using System.Text;
using NNGNET.Native;

namespace NNGNET.Protocols
{
    public class BusSocket : NngBaseSocket
    {
        public BusSocket() : base(Interop.OpenBus0)
        {
        }
    }
}
