using Grpc.Net.Client;
using GrpcClient;

namespace GrpcClient4.PerformanceTest.ConsoleApp.Services
{
    public class ConnectToServerService
    {
        public async Task Subscribe()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7187");
            var client = new Connections.ConnectionsClient(channel);

            string machineName = Environment.MachineName.ToString();
            string userName = Environment.UserName.ToString();

            var clientData = new ClientRequest()
            {
                ClientName = "Client4",
                ClientMachineName = machineName,
                ClientUserName = userName
            };

            var cancellationToken = new CancellationTokenSource();

            var subscribe = client.Subscribe(clientData);

            while (await subscribe.ResponseStream.MoveNext(cancellationToken.Token))
            {
                var message = subscribe.ResponseStream.Current;
                Console.WriteLine(message.Name + " " + message.Id);
            }
        }
    }
}
