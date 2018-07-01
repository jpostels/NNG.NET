using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FluentAssertions;
using NNGNET.Native;
using NNGNET.Native.InteropTypes;
using Xunit;
using Xunit.Abstractions;

namespace NNGNET.NETTests.Native
{
    public class InteropTests : TestOutputBase
    {
        public InteropTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void InitializeTest01()
        {
            Interop.Initialize();

            Print("AFTER => Initialized: " + Interop.IsInitialized.ToString());
            Assert.True(Interop.IsInitialized, "Interop.IsInitialized");
        }

        [Fact]
        public void MarshalPrelinkTest()
        {
            foreach (var method in typeof(Interop).GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                Print("Prelinking " + method.Name + "... ");
                try
                {
                    Marshal.Prelink(method);
                }
                // ReSharper disable once UncatchableException
                catch (MarshalDirectiveException)
                {
                    Print(method.Name + " failed to prelink! ");
                    throw;
                }

                Print("Prelink succesful! ");
            }
        }

        [Fact]
        public unsafe void VersionTest01()
        {
            Interop.Initialize();
            var nngVersionPtr = new IntPtr(Interop.GetVersionUnsafe());

            Assert.NotEqual(IntPtr.Zero, nngVersionPtr);

            var nngVersionStr = Marshal.PtrToStringAnsi(nngVersionPtr);

            Assert.NotNull(nngVersionStr);
            Assert.False(string.IsNullOrWhiteSpace(nngVersionStr), "string.IsNullOrWhiteSpace(nngVersionStr)");

            Print("Version: " + nngVersionStr);
        }

        [Fact]
        public unsafe void VersionTest02()
        {
            const int majorVersion = 1;
            const int minorVersion = 0;
            const int patchVersion = 0;

            Interop.Initialize();

            var nngVersionPtr = new IntPtr(Interop.GetVersionUnsafe());
            var nngVersionStr = Marshal.PtrToStringAnsi(nngVersionPtr);

            Print("Version: " + nngVersionStr);

            var presplit = nngVersionStr.Split('-');
            var split = presplit[0].Split('.');

            split.Length.Should().Be(3);

            Assert.True(int.TryParse(split[0], out var major), "int.TryParse(split[0], out var major)");
            Assert.True(int.TryParse(split[1], out var minor), "int.TryParse(split[1], out var minor)");
            Assert.True(int.TryParse(split[2], out var patch), "int.TryParse(split[2], out var patch)");

            Assert.Equal(majorVersion, major);
            Assert.Equal(minorVersion, minor);
            Assert.Equal(patchVersion, patch);
        }

        [Fact]
        public void CloseTest01()
        {
            Interop.Initialize();

            NNGSocket nngSocket;
            nngSocket.Id = 2222222212;
            var res = Interop.Close(nngSocket);
            Print("HRESULT: 0x" + ((int)res).ToString("X8"));
            Print("FLAG: " + res.ToString("G"));

            Assert.Equal(nng_errno.NNG_ECLOSED, res);
        }

        [Fact]
        public void CloseAllTest01()
        {
            Interop.Initialize();
            Print("Call nng_closeall");
            Interop.CloseAll();
        }

        [Fact]
        public unsafe void AllocTest01()
        {
            const int bufSize = 512;
            Interop.Initialize();
            var nngAlloc = Interop.Alloc(new UIntPtr(bufSize));
            Assert.NotEqual(IntPtr.Zero, new IntPtr(nngAlloc));

            var pointer = nngAlloc;

            for (var i = 0; i < bufSize; i++)
            {
                *((byte*)pointer + i) = (byte)i;
            }

            for (var i = 0; i < bufSize; i++)
            {
                var bytePointer = (byte*)pointer;

                Print(i.ToString("D3") + ": " + bytePointer[i].ToString("D3"));
                bytePointer[i].Should().Be((byte)(i % 256));
            }

            Interop.Free(nngAlloc, new UIntPtr(bufSize));
        }

        [Fact]
        public unsafe void AllocAndFreeTest01()
        {
            const int bufSize = 512;
            Interop.Initialize();
            var nngAlloc = new IntPtr(Interop.Alloc(new UIntPtr(bufSize)));
            Assert.NotEqual(IntPtr.Zero, nngAlloc);

            Print("Ptr: " + nngAlloc.ToString("X"));

            Interop.Free(nngAlloc.ToPointer(), new UIntPtr(bufSize));
        }

        [Fact]
        public void ReqOpen01()
        {
            Interop.Initialize();
            var error = Interop.OpenReq0(out var socket);

            Print("ERROR: 0x" + error.ToString("X"));
            Print("SOCKET_ID: 0x" + socket.Id.ToString("X"));

            Assert.Equal(nng_errno.NNG_SUCCESS, error);
        }

        [Fact]
        public void OpenAndClose01()
        {
            Interop.Initialize();
            var error = Interop.OpenReq0(out var socket);

            Print("ERROR: 0x" + error.ToString("X"));
            Print("SOCKET_ID: 0x" + socket.Id.ToString("X"));

            Assert.Equal(nng_errno.NNG_SUCCESS, error);

            var error2 = Interop.Close(socket);

            Print("ERROR: 0x" + error2.ToString("X"));
            Assert.Equal(nng_errno.NNG_SUCCESS, error2);
        }

        [Fact]
        public unsafe void PipeNotifyCall()
        {
            Interop.Initialize();
            var error = Interop.PipeSetNotification(new NNGSocket(), PipeEvent.Added, (_, __, ___) => { }, IntPtr.Zero.ToPointer());

            Print("ERROR: 0x" + error.ToString("X"));
        }

        [Fact]
        public void DialerTest()
        {
            Interop.Initialize();

            var res = Interop.OpenRep0(out var socket);
            Assert.Equal(nng_errno.NNG_SUCCESS, res);
            res = Interop.DialerCreate(out var dialer, socket, "ipc://test");
            Assert.Equal(nng_errno.NNG_SUCCESS, res);

            res = Interop.DialerStart(dialer, NNGFlag.None);
            Assert.Equal(nng_errno.NNG_SUCCESS, res);

            Print("DIALER ID: " + dialer.Id.ToString("X"));

            res = Interop.DialerClose(dialer);
            Assert.Equal(nng_errno.NNG_SUCCESS, res);
        }

        [Fact]
        public unsafe void MessageTest()
        {
            const int size = 10;
            var res = Interop.MessageAlloc(out var ptr, new UIntPtr(size));
            Assert.Equal(nng_errno.NNG_SUCCESS, res);

            var len = Interop.MessageLength(ptr);
            Print("Length: " + len.ToUInt32());

            res = Interop.MessageAppend(ptr, 0xAABBCCDDU);
            Assert.Equal(nng_errno.NNG_SUCCESS, res);

            len = Interop.MessageLength(ptr);
            Print("Length: " + len.ToUInt32());

            res = Interop.MessageInsert(ptr, 0x11223344);
            Assert.Equal(nng_errno.NNG_SUCCESS, res);

            len = Interop.MessageLength(ptr);
            Print("Length: " + len.ToUInt32());

            var msgPtr = Interop.MessageBody(ptr);

            var sp = new Span<byte>(msgPtr, (int) len);

            Assert.Equal(0x11, sp[0]);
            Assert.Equal(0x22, sp[1]);
            Assert.Equal(0x33, sp[2]);
            Assert.Equal(0x44, sp[3]);

            Assert.Equal(0xAA, sp[14]);
            Assert.Equal(0xBB, sp[15]);
            Assert.Equal(0xCC, sp[16]);
            Assert.Equal(0xDD, sp[17]);

            for (var i = 0; i < sp.Length; i++)
            {
                Print("{0}: {1:X2}", i, sp[i]);
            }

            Interop.MessageFree(ptr);
        }

        [Fact]
        public unsafe void MessageSendTest()
        {
            const int size = 10;
            var res = Interop.MessageAlloc(out var ptr, new UIntPtr(size));
            Assert.Equal(nng_errno.NNG_SUCCESS, res);

            var len = Interop.MessageLength(ptr);
            Print("Length: " + len.ToUInt32());
            Assert.Equal(size, (int) len);

            const string pipe = "inproc://" + nameof(MessageSendTest);

            Interop.OpenPush0(out var pushSocket);
            Interop.OpenPull0(out var pullSocket);

            var t1 = Task.Run(() => Interop.Listen(pullSocket, pipe, out _, NNGFlag.None));
            var t2 = Task.Run(() => Interop.Dial(pushSocket, pipe, out _, NNGFlag.None));

            Task.WaitAll(t1, t2);

            var t3 = Task.Run(() => Interop.SendMessage(pushSocket, ptr, NNGFlag.None));

            nng_msg* rPtr = default;
            var t4 = Task.Run(() => Interop.ReceiveMessage(pushSocket, ref rPtr, NNGFlag.None));

            Task.WaitAll(t3, t4);

            var rlen = Interop.MessageLength(ptr);
            Print("Length: " + rlen.ToUInt32());
            Assert.Equal(size, (int)rlen);

            Interop.CloseAll();
        }
    }
}
