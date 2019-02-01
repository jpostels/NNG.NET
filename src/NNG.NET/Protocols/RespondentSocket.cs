using System;
using System.Collections.Generic;
using System.Text;
using NNGNET.Native;

namespace NNGNET.Protocols
{
    public class RespondentSocket : NngBaseSocket
    {
        public RespondentSocket() : base(Interop.OpenRespondent0)
        {
        }
    }
}
