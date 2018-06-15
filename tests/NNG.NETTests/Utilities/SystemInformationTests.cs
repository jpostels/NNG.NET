using System;
using FluentAssertions;
using NNGNET.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace NNGNET.NETTests.Utilities
{
    public class SystemInformationTests : TestOutputBase
    {
        public SystemInformationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void IsX64Test01()
        {
            var isX64 = SystemInformation.IsX64();

            if (IntPtr.Size == sizeof(ulong))
            {
                isX64.Should().BeTrue();
            }
            else
            {
                isX64.Should().BeFalse();
            }
        }
    }
}
