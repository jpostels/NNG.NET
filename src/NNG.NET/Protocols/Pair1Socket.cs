using NNG.Native;

namespace NNG.Protocols
{
    public class Pair1Socket : NngBaseSocket
    {
        internal const string NNG_OPT_PAIR1_POLY = "pair1:polyamorous";

        public Pair1Socket() : base(Interop.nng_pair1_open)
        {
        }
    }
}