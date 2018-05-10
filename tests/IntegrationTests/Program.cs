using System;
using System.Transactions;
using NNG.Native;

namespace IntegrationTests
{
    public static class Program
    {
        public const string PipeName = "integrationTest01";

        public static void Main(string[] args)
        {
            Console.WriteLine("Running integration test #1");

            Console.Write("Initialize NNG... ");
            Interop.Initialize();
            Console.Write("DONE! \n");

            CreateReplySocket();
            CreateRequestSocket();
        }

        private static void CreateReplySocket()
        {
            throw new NotImplementedException();
        }

        private static void CreateRequestSocket()
        {
            throw new NotImplementedException();
        }
    }
}
