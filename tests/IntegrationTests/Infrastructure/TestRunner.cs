﻿using System;

namespace IntegrationTests.Infrastructure
{
    /// <summary>
    ///     Starts and manager test runs
    /// </summary>
    public static class TestRunner
    {
        /// <summary>
        ///     Runs the test specified by <typeparamref name="TIntegrationTest"/>.
        /// </summary>
        /// <typeparam name="TIntegrationTest">The type of the integration test.</typeparam>
        /// <returns>
        ///     The test results.
        /// </returns>
        public static TestResults Run<TIntegrationTest>() where TIntegrationTest : TestBase, new()
        {
            var test = TestBase.CreateInstance<TIntegrationTest>();
            try
            {
                test.Run();
            }
            // ReSharper disable once CatchAllClause
            catch (Exception exception)
            {
                return new TestResults(Result.Failure, exception);
            }

            return new TestResults(Result.Success);
        }

        /// <summary>
        ///     Runs the test specified by <paramref name="testType"/>.
        /// </summary>
        /// <returns>
        ///     The test results.
        /// </returns>
        public static TestResults Run(Type testType)
        {
            var test = TestBase.CreateInstance(testType);
            try
            {
                test.Run();
            }
            // ReSharper disable once CatchAllClause
            catch (Exception exception)
            {
                return new TestResults(Result.Failure, exception);
            }

            return new TestResults(Result.Success);
        }
    }
}
