using System;
using System.Runtime.InteropServices;
using System.Threading;
using IntegrationTests.Infrastructure;
using NNGNET;
using NNGNET.ErrorHandling;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;
using NNGNET.Protocols;

namespace IntegrationTests.Tests.BasicReqRep
{
    public class BasicReqRepTest : TestBase
    {
        private const string PipeName = "inproc://" + nameof(BasicReqRepTest);

        private bool IsDone { get; set; }

        private bool ReplyIsDone { get; set; }

        /// <inheritdoc />
        public override void Run()
        {
            Interop.Initialize();

            CreateReplySocket();
            CreateRequestSocket();

            while (!IsDone)
            {
                Thread.Sleep(10);
            }

            Interop.CloseAll();
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
                var res = Interop.Listen(rep.Socket, PipeName, out var listener, NNGFlag.None);
                AssertResult(res);

                var buf = IntPtr.Zero;
                var size = UIntPtr.Zero;
                res = Interop.Receive(rep.Socket, ref buf, ref size, NNGFlag.Alloc);
                AssertResult(res);

                Console.WriteLine("received: " + ((byte*)buf.ToPointer())[0].ToString());
                ((byte*)buf.ToPointer())[0]++;

                res = Interop.Send(rep.Socket, buf, size, NNGFlag.Alloc);
                AssertResult(res);
                ReplyIsDone = true;
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
                var res = Interop.Dial(req.Socket, PipeName, out var dialer, NNGFlag.None);
                AssertResult(res);

                var ptr = Marshal.AllocHGlobal(1);
                var size = new UIntPtr(1);

                Console.WriteLine("sending: " + ((byte*)ptr.ToPointer())[0].ToString());

                res = Interop.Send(req.Socket, ptr, size, NNGFlag.None);
                AssertResult(res);

                Marshal.FreeHGlobal(ptr);

                res = Interop.Receive(req.Socket, ref ptr, ref size, NNGFlag.Alloc);
                AssertResult(res);

                Console.WriteLine("received: " + ((byte*)ptr.ToPointer())[0].ToString());

                Interop.Free(ptr, size);

                IsDone = true;
            }
        }

        private static void AssertResult(nng_errno errorCode)
        {
            if (errorCode == nng_errno.NNG_SUCCESS)
            {
                return;
            }

            throw ThrowHelper.GetExceptionForErrorCode(errorCode);
        }
    }
}
