using System;
using System.Runtime.InteropServices;
using System.Threading;
using IntegrationTests.Infrastructure;
using NNG.ErrorHandling;
using NNG.Native;
using NNG.Native.InteropTypes;
using NNG.Protocols;

namespace IntegrationTests.Tests.BasicReqRep
{
    public class BasicReqRepTest : TestBase
    {
        private const string PipeName = "inproc://" + nameof(BasicReqRepTest);

        private bool IsDone { get; set; }

        /// <inheritdoc />
        public override void Run()
        {
            CreateReplySocket();
            CreateRequestSocket();

            while (!IsDone)
            {
                Thread.Sleep(10);
            }

            Interop.nng_closeall();
        }

        private void CreateReplySocket()
        {
            var thr = new Thread(Reply) { Name = "ReplyThread" };
            thr.Start();
        }

        private unsafe void Reply()
        {
            using (var rep = new ReplySocket())
            {
                var res = Interop.nng_listen(rep.Socket, PipeName, out var listener, nng_flag.NONE);
                AssertResult(res);

                var buf = IntPtr.Zero;
                var size = UIntPtr.Zero;
                res = Interop.nng_recv(rep.Socket, ref buf, ref size, nng_flag.NNG_FLAG_ALLOC);
                AssertResult(res);

                Console.WriteLine("received: " + ((byte*)buf.ToPointer())[0].ToString());
                ((byte*)buf.ToPointer())[0]++;

                res = Interop.nng_send(rep.Socket, buf, size, nng_flag.NNG_FLAG_ALLOC);
                AssertResult(res);
            }
        }

        private void CreateRequestSocket()
        {
            Thread.Sleep(10);
            var thr = new Thread(Request) { Name = "RequestThread" };
            thr.Start();
        }

        private unsafe void Request(object obj)
        {
            using (var req = new RequestSocket())
            {
                var res = Interop.nng_dial(req.Socket, PipeName, out var dialer, nng_flag.NONE);
                AssertResult(res);

                var ptr = Marshal.AllocHGlobal(1);
                var size = new UIntPtr(1);

                Console.WriteLine("sending: " + ((byte*)ptr.ToPointer())[0].ToString());

                res = Interop.nng_send(req.Socket, ptr, size, nng_flag.NONE);
                AssertResult(res);

                Marshal.FreeHGlobal(ptr);

                res = Interop.nng_recv(req.Socket, ref ptr, ref size, nng_flag.NNG_FLAG_ALLOC);
                AssertResult(res);

                Console.WriteLine("received: " + ((byte*)ptr.ToPointer())[0].ToString());

                Interop.nng_free(ptr, size);

                IsDone = true;
            }
        }

        private static void AssertResult(nng_errno errorCode)
        {
            if (errorCode == nng_errno.NNG_SUCCESS)
            {
                return;
            }

            ThrowHelper.Throw(errorCode);
        }
    }
}
