using NNGNET.Native;

namespace NNGNET.Protocols
{
    public class Pair1Socket : NngBaseSocket
    {
        internal const string NNG_OPT_PAIR1_POLY = "pair1:polyamorous";

        public Pair1Socket() : base(Interop.OpenPair1)
        {
        }
    }
}