using System;
using System.Runtime.InteropServices;
using NNG.Native;
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
            Assert.False(Interop.IsInitialized, "Interop.IsInitialized");
            Interop.Initialize();
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
            const int majorVersion = 0;
            const int minorVersion = 9;
            const int patchVersion = 0;

            var nngVersionPtr = Interop.nng_version();
            var nngVersionStr = Marshal.PtrToStringAnsi(nngVersionPtr);

            Print("Version: " + nngVersionStr);

            var split = nngVersionStr.Split('.');

            Assert.Equal(3, split.Length);

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
            var res = Interop.nng_close(2);
            Print("HRESULT: 0x" + res.ToString("X8"));
        }

        [Fact]
        public void CloseAllTest01()
        {
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
                    Assert.Equal((byte)(i % 256), bytePointer[i]);
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

            Interop.nng_free(nngAlloc, new UIntPtr(bufSize));
        }
    }
}
