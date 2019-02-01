using System.Threading;
using IntegrationTests.Infrastructure;

namespace IntegrationTests.Tests.BasicReqRep
{
    public class LoopedReqRepTest : TestBase
    {
        /// <inheritdoc />
        public override void Run()
        {
            for (var i = 0; i < 1000; i++)
            {
                var t = new BasicReqRepTest();
                t.Run();
                Thread.Sleep(5);
            }
        }
    }
}
