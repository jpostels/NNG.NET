using System;
using System.Collections.Generic;
using System.Text;
using NNG.Native;

namespace NNG.Protocols
{
    public class RespondentSocket : NngBaseSocket
    {
        public RespondentSocket() : base(Interop.nng_respondent0_open)
        {
        }
    }
}
