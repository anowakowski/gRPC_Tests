using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcClient4.PerformanceTest.ConsoleApp.Services
{
    public static class ConnectionsManagerTestService
    {
        public static async Task RunTest()
        {
            var connectionService = new ConnectToServerService();

            await connectionService.Subscribe();

        }
    }
}
