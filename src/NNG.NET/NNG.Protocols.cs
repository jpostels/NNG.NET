using NNGNET.ErrorHandling;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;

namespace NNGNET
{
    public static partial class NNG
    {
        private delegate nng_errno SocketOpenFunction(out NNGSocket socket);

        private static NNGSocket OpenSocket(SocketOpenFunction socketOpenFunction)
        {
            var err = socketOpenFunction(out var socket);
            ThrowHelper.ThrowIfNotSuccess(err);
            return socket;
        }

        public static NNGSocket OpenReq0() => OpenSocket(Interop.OpenReq0);

        public static NNGSocket OpenReq0Raw() => OpenSocket(Interop.OpenReq0Raw);

        public static NNGSocket OpenRep0() => OpenSocket(Interop.OpenRep0);

        public static NNGSocket OpenRep0Raw() => OpenSocket(Interop.OpenRep0Raw);

        public static NNGSocket OpenSurveyor0() => OpenSocket(Interop.OpenSurveyor0);

        public static NNGSocket OpenSurveyor0Raw() => OpenSocket(Interop.OpenSurveyor0Raw);

        public static NNGSocket OpenRespondent0() => OpenSocket(Interop.OpenRespondent0);

        public static NNGSocket OpenRespondent0Raw() => OpenSocket(Interop.OpenRespondent0Raw);

        public static NNGSocket OpenPub0() => OpenSocket(Interop.OpenPub0);

        public static NNGSocket OpenPub0Raw() => OpenSocket(Interop.OpenPub0Raw);

        public static NNGSocket OpenSub0() => OpenSocket(Interop.OpenSub0);

        public static NNGSocket OpenSub0Raw() => OpenSocket(Interop.OpenSub0Raw);

        public static NNGSocket OpenPush0() => OpenSocket(Interop.OpenPush0);

        public static NNGSocket OpenPush0Raw() => OpenSocket(Interop.OpenPush0Raw);

        public static NNGSocket OpenPull0() => OpenSocket(Interop.OpenPull0);

        public static NNGSocket OpenPull0Raw() => OpenSocket(Interop.OpenPull0Raw);

        public static NNGSocket OpenPair0() => OpenSocket(Interop.OpenPair0);

        public static NNGSocket OpenPair0Raw() => OpenSocket(Interop.OpenPair0Raw);

        public static NNGSocket OpenPair1() => OpenSocket(Interop.OpenPair1);

        public static NNGSocket OpenPair1Raw() => OpenSocket(Interop.OpenPair1Raw);

        public static NNGSocket OpenBus0() => OpenSocket(Interop.OpenBus0);

        public static NNGSocket OpenBus0Raw() => OpenSocket(Interop.OpenBus0Raw);
    }
}