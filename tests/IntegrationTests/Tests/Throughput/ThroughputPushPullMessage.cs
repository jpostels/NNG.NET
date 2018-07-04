using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using IntegrationTests.Infrastructure;
using NNGNET;
using NNGNET.Protocols;

namespace IntegrationTests.Tests.Throughput
{
    public class ThroughputPushPullMessage : TestBase
    {
        public const int MsgSize = 64 * 1024;

        public const long NumberOfMessages = 50_000;

        //private const string PipeName = "inproc://" + nameof(ThroughputPushPullMessage);
        private const string PipeName = "ipc:///tmp/" + nameof(ThroughputPushPullMessage);

        private bool IsDone { get; set; }

        private bool ReplyIsDone { get; set; }

        /// <inheritdoc />
        public override void Run()
        {
            NNG.Initialize();

            CreatePullSocket();
            CreatePushSocket();

            while (!IsDone || !ReplyIsDone)
            {
                Thread.Sleep(10);
            }

            //NNG.CloseAll();
        }

        private void CreatePullSocket()
        {
            var thr = new Thread(Pull) { Name = "PullThread" };
            thr.Start();
        }

        private void Pull()
        {
            using (var rep = new PullSocket())
            {
                var listener = NNG.Listen(rep.Socket, PipeName);

                var sw = Stopwatch.StartNew();
                for (var i = 0; i < NumberOfMessages; i++)
                {
                    var received = NNG.ReceiveMessage(rep.Socket);
                    NNG.FreeMessage(received);
                }

                sw.Stop();

                //NNG.CloseListener(listener);
                Console.WriteLine($"Rep: {sw.ElapsedMilliseconds:N0}ms");
                Console.WriteLine($"Rate: {NumberOfMessages * MsgSize / TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).TotalSeconds:N0} b/s");
                ReplyIsDone = true;

                Thread.Sleep(1);
            }
        }

        private void CreatePushSocket()
        {
            Thread.Sleep(10);
            var thr = new Thread(Push) { Name = "PushThread" };
            thr.Start();
        }

        private static readonly Random _random = new Random(DateTime.Now.Millisecond);

        private unsafe void Push(object obj)
        {
            var ptr = stackalloc byte[MsgSize];
            var sp = new Span<byte>(ptr, MsgSize);
            _random.NextBytes(sp);

            using (var req = new PushSocket())
            {
                var dialer = NNG.Dial(req.Socket, PipeName);

                var sw = Stopwatch.StartNew();
                for (var i = 0; i < NumberOfMessages; i++)
                {
                    var msg = NNG.AllocMessage(MsgSize);
                    sp.CopyTo(msg.GetSpan());
                    NNG.SendMessage(req.Socket, msg);
                }

                sw.Stop();

                //NNG.CloseDialer(dialer);
                Console.WriteLine($"Req: {sw.ElapsedMilliseconds:N0}ms");
                Console.WriteLine($"Rate: {NumberOfMessages * MsgSize / TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).TotalSeconds:N0} b/s");
                IsDone = true;

                Thread.Sleep(1);
            }
        }
    }
}
