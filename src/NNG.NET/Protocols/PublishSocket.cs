using System;
using System.Collections.Generic;
using System.Text;
using NNGNET.Native;

namespace NNGNET.Protocols
{
    public class PublishSocket : NngBaseSocket
    {
        /// <inheritdoc />
        public PublishSocket() : base(Interop.nng_pub0_open)
        {
        }
    }
}
