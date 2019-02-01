using System;
using System.Collections.Generic;
using System.Text;
using NNGNET;
using NNGNET.Native.InteropTypes;
using NNGNET.NETTests;
using Xunit;
using Xunit.Abstractions;

namespace NNGNETTests
{
    public class NNGTests : TestOutputBase
    {
        /// <inheritdoc />
        public NNGTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void GetVersionString_ShouldReturnValidString()
        {
            var version = NNG.GetVersionString();
            Print("Reported version: " + version);

            Assert.NotNull(version);
            Assert.NotEmpty(version);
            Assert.NotEqual("", version);
        }

        [Fact]
        public void Version_ShouldReturnValidValue()
        {
            var version = NNG.Version;
            Assert.NotNull(version);
            Print("Major: " + version.Major);
            Print("Minor: " + version.Minor);
            Print("Build: " + version.Build);
            Print("Revision: " + version.Revision);
        }

        private void TestSocketOpenFunction(Func<NNGSocket> socketOpenFunc)
        {
            var nngSocket = socketOpenFunc();
            Print("Socket ID: " + nngSocket.Id.ToString("X"));
            Assert.NotEqual(new NNGSocket(), nngSocket);
        }

        [Fact]
        public void ParseUrl_ShouldReturnValidValue()
        {
            var url = NNG.ParseUrl("tcp://127.0.0.1");
            Assert.NotNull(url);
        }

        [Fact]
        public unsafe void AioAlloc_ShouldReturnValidValue()
        {
            void Cb(void* ptr) => Print("CB called: " + new IntPtr(ptr));
            var aio = NNG.AllocAio(Cb, IntPtr.Zero);
            Assert.NotEqual(new NNGAIO(), aio);

            var count = NNG.GetAioCount(aio);
            Print(count.ToUInt64().ToString());
        }

        [Fact]
        public unsafe void AioSetMessage_ShouldSetMessage()
        {
            void Cb(void* ptr) => Print("CB called: " + new IntPtr(ptr));
            var aio = NNG.AllocAio(Cb, IntPtr.Zero);
            Assert.NotEqual(new NNGAIO(), aio);

            const uint MessageLength = 100;

            var msg = NNG.AllocMessage(MessageLength);
            NNG.SetAioMessage(aio, msg);

            var rmsg = NNG.GetAioMessage(aio);
            var len = NNG.GetMessageLength(rmsg);

            Print("Length: " + len.ToString());
            Assert.Equal(MessageLength, len);
        }

        [Fact]
        public void OpenContext_ShouldReturnValidValue()
        {
            var socket = NNG.OpenReq0();

            var context = NNG.OpenContext(socket);
            Assert.NotEqual(new NNGContext(), context);

            NNG.CloseContext(context);
            NNG.Close(socket);
        }

        [Fact]
        public void OpenReq0_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenReq0);

        [Fact]
        public void OpenReq0Raw_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenReq0Raw);

        [Fact]
        public void OpenRep0_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenRep0);

        [Fact]
        public void OpenRep0Raw_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenRep0Raw);

        [Fact]
        public void OpenSurveyor0_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenSurveyor0);

        [Fact]
        public void OpenSurveyor0Raw_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenSurveyor0Raw);

        [Fact]
        public void OpenRespondent0_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenRespondent0);

        [Fact]
        public void OpenRespondent0Raw_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenRespondent0Raw);

        [Fact]
        public void OpenPub0_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenPub0);

        [Fact]
        public void OpenPub0Raw_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenPub0Raw);

        [Fact]
        public void OpenSub0_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenSub0);

        [Fact]
        public void OpenSub0Raw_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenSub0Raw);

        [Fact]
        public void OpenPush0_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenPush0);

        [Fact]
        public void OpenPush0Raw_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenPush0Raw);

        [Fact]
        public void OpenPull0_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenPull0);

        [Fact]
        public void OpenPull0Raw_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenPull0Raw);

        [Fact]
        public void OpenPair0_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenPair0);

        [Fact]
        public void OpenPair0Raw_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenPair0Raw);

        [Fact]
        public void OpenPair1_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenPair1);

        [Fact]
        public void OpenPair1Raw_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenPair1Raw);

        [Fact]
        public void OpenBus0_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenBus0);

        [Fact]
        public void OpenBus0Raw_ShouldReturnValidSocket() => TestSocketOpenFunction(NNG.OpenBus0Raw);
    }
}
