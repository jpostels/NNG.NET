using System;
using System.Diagnostics;
using System.Threading;
using IntegrationTests.Infrastructure;
using NNGNET;
using NNGNET.Protocols;

namespace IntegrationTests.Tests.Throughput
{
    public class ThroughputReqRep : TestBase
    {
        public const int MsgSize = 64 * 1024;

        public const long NumberOfMessages = 50_000;

        private const string PipeName = "inproc://" + nameof(ThroughputReqRep);
        //private const string PipeName = "tcp://localhost";

        private bool IsDone { get; set; }

        private bool ReplyIsDone { get; set; }

        /// <inheritdoc />
        public override void Run()
        {
            NNG.Initialize();

            CreateReplySocket();
            CreateRequestSocket();

            while (!IsDone || !ReplyIsDone)
            {
                Thread.Sleep(10);
            }

            //NNG.CloseAll();
        }

        private void CreateReplySocket()
        {
            var thr = new Thread(Reply) { Name = "ReplyThread" };
            thr.Start();
        }

        private void Reply()
        {
            using (var rep = new ReplySocket())
            {
                var listener = NNG.Listen(rep.Socket, PipeName);

                var sw = Stopwatch.StartNew();
                for (var i = 0; i < NumberOfMessages; i++)
                {
                    var received = NNG.Receive(rep.Socket);
                    NNG.Send(rep.Socket, received, false, true);
                }

                sw.Stop();

                NNG.CloseListener(listener);
                Console.WriteLine($"Rep: {sw.ElapsedMilliseconds:N0}ms");
                Console.WriteLine($"Rate: {NumberOfMessages * MsgSize / TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).TotalSeconds:N0} b/s");
                ReplyIsDone = true;
            }
        }

        private void CreateRequestSocket()
        {
            Thread.Sleep(10);
            var thr = new Thread(Request) { Name = "RequestThread" };
            thr.Start();
        }

        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        private unsafe void Request(object obj)
        {
            var ptr = stackalloc byte[MsgSize];
            var sp = new Span<byte>(ptr, MsgSize);
            _random.NextBytes(sp);

            using (var req = new RequestSocket())
            {
                var dialer = NNG.Dial(req.Socket, PipeName);

                var sw = Stopwatch.StartNew();
                for (var i = 0; i < NumberOfMessages; i++)
                {
                    NNG.Send(req.Socket, sp);
                    var received = NNG.Receive(req.Socket);
                    NNG.Free(received);
                }

                sw.Stop();

                NNG.CloseDialer(dialer);
                Console.WriteLine($"Req: {sw.ElapsedMilliseconds:N0}ms");
                Console.WriteLine($"Rate: {NumberOfMessages * MsgSize / TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).TotalSeconds:N0} b/s");
                IsDone = true;
            }
        }
    }
}
