using System;
using System.Collections.Generic;
using System.Text;
using NNGNET.Native;

namespace NNGNET.Protocols
{
    public class SurveyorSocket : NngBaseSocket
    {
        internal const string NNG_OPT_SURVEYOR_SURVEYTIME = "surveyor:survey-time";

        public SurveyorSocket() : base(Interop.OpenSurveyor0)
        {
        }
    }
}
