using System;
using System.Threading;
using IntegrationTests.Infrastructure;
using IntegrationTests.Tests.BasicReqRep;

namespace IntegrationTests
{
    public static class Program
    {
        public const string PipeName = "integrationTest01";

        public static void Main(string[] args)
        {
            while (true)
            {
                PrintTestList();

                var continueTests = SelectAndRunTest();

                if (!continueTests)
                {
                    break;
                }

                GC.Collect();

                Console.WriteLine("\n##########################\n");
            }

            Console.WriteLine("Exiting... ");
        }

        private static bool SelectAndRunTest()
        {
            int? input = null;

            while (!input.HasValue)
            {
                Console.WriteLine("Please select a test to run: ");
                var line = Console.ReadLine();

                if (line.Contains("exit"
#if !NET47
                    , StringComparison.InvariantCultureIgnoreCase
#endif
                    ))
                {
                    return false;
                }

#if NET47
                if (!int.TryParse(line.Trim(), out var parsed))
                {
                    Console.WriteLine("Invalid value: " + line);
                }
#else
                if (!int.TryParse((ReadOnlySpan<char>) line.Trim(), out var parsed))
                {
                    Console.WriteLine("Invalid value: " + line);
                }
#endif
                input = parsed;
            }

            TestResults results;
            switch (input.Value)
            {
                case 1:
                    results = TestRunner.Run<BasicReqRepTest>();
                    break;
                case 2:
                    results = TestRunner.Run<LoopedReqRepTest>();
                    break;
                default:
                    Console.WriteLine("Unknown test selected. ");
                    return true;
            }

            AnalyzeResults(results);
            return true;
        }

        private static void AnalyzeResults(TestResults results)
        {
            ConsoleColor old;
            switch (results.Result)
            {
                case Result.Unknown:
                    Console.WriteLine("Unknown test result... ");
                    break;
                case Result.Success:
                    old = Console.ForegroundColor; try
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Test succesful!");
                    }
                    finally
                    {
                        Console.ForegroundColor = old;
                    }
                    break;
                case Result.Failure:
                    old = Console.ForegroundColor; try
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Test failed!");
                    }
                    finally
                    {
                        Console.ForegroundColor = old;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PrintResults(results);
        }

        private static void PrintResults(TestResults results)
        {
            switch (results.Result)
            {
                case Result.Unknown:
                    break;
                case Result.Success:
                    break;
                case Result.Failure:
                    PrintFailure(results);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void PrintFailure(TestResults results)
        {
            var inner = results.Exception;

            while (inner != null)
            {
                Console.WriteLine(inner.GetType().Name + ": " + inner.Message);
                Console.WriteLine(inner.StackTrace);
                inner = inner.InnerException;
            }
        }

        private static void PrintTestList()
        {
            Console.WriteLine("Available tests: ");

            Console.WriteLine("#01: " + nameof(BasicReqRepTest));
            Console.WriteLine("#02: " + nameof(LoopedReqRepTest));

            Console.WriteLine("------------------");
        }
    }
}
