using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace NNGNET.NETTests
{
    public abstract class TestOutputBase
    {
        protected ITestOutputHelper TestOutputHelper { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestOutputBase"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        protected TestOutputBase(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        /// <summary>
        ///     Adds a line of text to the output.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void Print(string message) => TestOutputHelper.WriteLine(message);

        /// <summary>
        ///     Formats a line of text and adds it to the output.
        /// </summary>
        /// <param name="format">The message format.</param>
        /// <param name="args">The format arguments.</param>
        protected void Print(string format, params object[] args) => TestOutputHelper.WriteLine(format, args);
    }
}
