using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Infrastructure
{
    /// <summary>
    ///     Provides runnable integration tests with the required infrastructure.
    /// </summary>
    public abstract class TestBase
    {
        /// <summary>
        ///     Runs this instance.
        /// </summary>
        public abstract void Run();
    }
}
