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
            const int minorVersion = 5;
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
            Print("HRESULT: " + res);
        }
    }
}
