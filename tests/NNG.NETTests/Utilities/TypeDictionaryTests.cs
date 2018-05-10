using FluentAssertions;
using NNG.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace NNG.NETTests.Utilities
{
    public class TypeDictionaryTests : TestOutputBase
    {
        public TypeDictionaryTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void ExampleGetSet()
        {
            TypeDictionary<string>.Set<int>("int");
            TypeDictionary<string>.Set<uint>("uint");

            var intValue = TypeDictionary<string>.Get<int>();
            var uintValue = TypeDictionary<string>.Get<uint>();

            intValue.Should().Be("int");
            uintValue.Should().Be("uint");
        }
    }
}
