using System;

namespace IntegrationTests.Infrastructure
{
    public class TestResults
    {
        public Result Result { get; set; }

        public Exception Exception { get; set; }

        /// <inheritdoc />
        public TestResults(Result result)
        {
            Result = result;
        }

        /// <inheritdoc />
        public TestResults(Result result, Exception exception)
        {
            Result = result;
            Exception = exception;
        }
    }
}