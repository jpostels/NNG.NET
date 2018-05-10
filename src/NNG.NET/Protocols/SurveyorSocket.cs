﻿using System;
using System.Collections.Generic;
using System.Text;
using NNG.Native;

namespace NNG.Protocols
{
    public class SurveyorSocket : NngBaseSocket
    {
        internal const string NNG_OPT_SURVEYOR_SURVEYTIME = "surveyor:survey-time";

        public SurveyorSocket() : base(Interop.nng_surveyor0_open)
        {
        }
    }
}
