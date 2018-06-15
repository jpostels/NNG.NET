using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
        public void VersionTest01()
        {
            Interop.Initialize();
            var nngVersionPtr = Interop.Version();

            Assert.NotEqual(IntPtr.Zero, nngVersionPtr);

            var nngVersionStr = Marshal.PtrToStringAnsi(nngVersionPtr);

            Assert.NotNull(nngVersionStr);
            Assert.False(string.IsNullOrWhiteSpace(nngVersionStr), "string.IsNullOrWhiteSpace(nngVersionStr)");

            Print("Version: " + nngVersionStr);
        }

        [Fact]
        public void VersionTest02()
        {
            const int majorVersion = 1;
            const int minorVersion = 0;
            const int patchVersion = 0;

            Interop.Initialize();

            var nngVersionPtr = Interop.Version();
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
            nngSocket.Id = 2;
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
        public void AllocTest01()
        {
            const int bufSize = 512;
            Interop.Initialize();
            var nngAlloc = Interop.Alloc(new UIntPtr(bufSize));
            Assert.NotEqual(IntPtr.Zero, nngAlloc);

            unsafe
            {
                var pointer = nngAlloc.ToPointer();

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
            }

            Interop.Free(nngAlloc, new UIntPtr(bufSize));
        }

        [Fact]
        public void AllocAndFreeTest01()
        {
            const int bufSize = 512;
            Interop.Initialize();
            var nngAlloc = Interop.Alloc(new UIntPtr(bufSize));
            Assert.NotEqual(IntPtr.Zero, nngAlloc);

            Print("Ptr: " + nngAlloc.ToString("X"));

            Interop.Free(nngAlloc, new UIntPtr(bufSize));
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
        public void PipeNotifyCall()
        {
            Interop.Initialize();
            var error = Interop.PipeSetNotification(new NNGSocket(), nng_pipe_ev.NNG_PIPE_EV_ADD_POST, (_, __, ___) => { }, IntPtr.Zero);

            Print("ERROR: 0x" + error.ToString("X"));
        }
    }
}
