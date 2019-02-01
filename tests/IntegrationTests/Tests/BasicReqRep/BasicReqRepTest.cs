using System;
using System.Diagnostics;
using System.Threading;
using IntegrationTests.Infrastructure;
using NNGNET;
using NNGNET.Protocols;

namespace IntegrationTests.Tests.BasicReqRep
{
    public class BasicReqRepTest : TestBase
    {
        private const int MessageSize = 100;

        private const string PipeName = "inproc://" + nameof(BasicReqRepTest);

        //private const string PipeName = "ipc:///tmp/" + nameof(BasicReqRepTest);

        private static long _cnt = 1;

        private bool IsDone { get; set; }

        private bool ReplyIsDone { get; set; }

        private readonly object _dialListenLock = new object();

        /// <inheritdoc />
        public override void Run()
        {
            NNG.Initialize();

            CreateReplySocket();
            CreateRequestSocket();

            while (!IsDone)
            {
                Thread.Sleep(10);
            }

            NNG.CloseAll();
            _cnt++;
        }

        private void CreateReplySocket()
        {
            var thr = new Thread(Reply) { Name = "ReplyThread" + _cnt };
            thr.Start();
        }

        private void Reply()
        {
            Debug.WriteLine($"[{_cnt}] Spawn reply thread {Thread.CurrentThread.ManagedThreadId}");

            using (var rep = new ReplySocket())
            {
                lock (_dialListenLock)
                {
                    var listener = NNG.Listen(rep.Socket, PipeName + _cnt + ".ipc");
                }

                var received = NNG.Receive(rep.Socket);
                Console.WriteLine("server received: " + received[0].ToString());
                received[0]++;
                NNG.Send(rep.Socket, received, false, true);

                //NNG.CloseListener(listener);
                ReplyIsDone = true;

                Thread.Sleep(5);
            }

            Debug.WriteLine($"[{_cnt}] Exit reply thread " + Thread.CurrentThread.ManagedThreadId);
        }

        private void CreateRequestSocket()
        {
            Thread.Sleep(10);
            var thr = new Thread(Request) { Name = "RequestThread" + _cnt};
            thr.Start();
        }

        private readonly Random _random = new Random(DateTime.Now.Millisecond);

        private unsafe void Request(object obj)
        {
            Debug.WriteLine($"[{_cnt}] Spawn request thread {Thread.CurrentThread.ManagedThreadId}");

            using (var req = new RequestSocket())
            {
                lock (_dialListenLock)
                {
                    var dialer = NNG.Dial(req.Socket, PipeName + _cnt + ".ipc");
                }

                var ptr = stackalloc byte[MessageSize];
                var sp = new Span<byte>(ptr, MessageSize);
                _random.NextBytes(sp);

                Console.WriteLine("sending: " + sp[0]);

                var res = NNG.Send(req.Socket, sp);
                Debug.Assert(res);

                var received = NNG.Receive(req.Socket);

                Console.WriteLine("received: " + received[0]);
                Debug.Assert((sp[0] + 1) % 256 == received[0], "(sp[0] + 1) % 256 == received[0]");

                NNG.Free(received);
                //NNG.CloseDialer(dialer);
                IsDone = true;
            }

            Debug.WriteLine($"[{_cnt}] Exit request thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
