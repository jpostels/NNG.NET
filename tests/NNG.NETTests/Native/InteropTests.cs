using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using FluentAssertions;
using NNG.Native;
using NNG.Native.InteropTypes;
using Xunit;
using Xunit.Abstractions;

namespace NNG.NETTests.Native
{
    public class InteropTests : TestOutputBase
    {
        public InteropTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void InitializeTest01()
        {
            Print("BEFORE => Initialized: " + Interop.IsInitialized.ToString());
            Assert.False(Interop.IsInitialized, "Interop.IsInitialized");
            Interop.Initialize();

            Print("AFTER  => Initialized: " + Interop.IsInitialized.ToString());
            Assert.True(Interop.IsInitialized, "Interop.IsInitialized");
        }

        [Fact]
        public void VersionTest01()
        {
            var nngVersionPtr = Interop.nng_version();

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

            var nngVersionPtr = Interop.nng_version();
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
            nng_socket nngSocket;
            nngSocket.id = 2;
            var res = Interop.nng_close(nngSocket);
            Print("HRESULT: 0x" + res.ToString("X8"));
        }

        [Fact]
        public void CloseAllTest01()
        {
            Print("Call nng_closeall");
            Interop.nng_closeall();
        }

        [Fact]
        public void AllocTest01()
        {
            const int bufSize = 512;
            var nngAlloc = Interop.nng_alloc(new UIntPtr(bufSize));
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

            Interop.nng_free(nngAlloc, new UIntPtr(bufSize));
        }

        [Fact]
        public void AllocAndFreeTest01()
        {
            const int bufSize = 512;
            var nngAlloc = Interop.nng_alloc(new UIntPtr(bufSize));
            Assert.NotEqual(IntPtr.Zero, nngAlloc);

            Print("Ptr: " + nngAlloc.ToString("X"));

            Interop.nng_free(nngAlloc, new UIntPtr(bufSize));
        }

        [Fact]
        public void ReqOpen01()
        {
            var error = Interop.nng_req0_open(out var socket);

            Print("ERROR: 0x" + error.ToString("X"));
            Print("SOCKET_ID: 0x" + socket.id.ToString("X"));

            Assert.Equal(0, error);
        }

        [Fact]
        public void OpenAndClose01()
        {
            var error = Interop.nng_req0_open(out var socket);

            Print("ERROR: 0x" + error.ToString("X"));
            Print("SOCKET_ID: 0x" + socket.id.ToString("X"));

            Assert.Equal(0, error);

            var error2 = Interop.nng_close(socket);

            Print("ERROR: 0x" + error2.ToString("X"));
            Assert.Equal(0, error2);
        }

        [Fact]
        public unsafe void PipeNotifyCall()
        {
            var error = Interop.nng_pipe_notify(new nng_socket(), nng_pipe_ev.NNG_PIPE_EV_ADD_POST, (pipe, action, arg) => { },
                IntPtr.Zero.ToPointer());

            Print("ERROR: 0x" + error.ToString("X"));
        }
    }
}
