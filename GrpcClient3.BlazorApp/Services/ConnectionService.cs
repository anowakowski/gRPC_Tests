using Grpc.Net.Client;
using GrpcClient;

namespace GrpcClient3.BlazorApp.Services
{
    public class ConnectionService
    {
        public ClientExistModel ChceckIfClientIsInstalledOnMachine()
        {
            string machineName = Environment.MachineName.ToString();
            string userName = Environment.UserName.ToString();

            var channel = GrpcChannel.ForAddress("https://localhost:7187");
            var client = new Connections.ConnectionsClient(channel);

            var request = new ClientExistRequest
            {
                ClientMachineName = machineName,
                ClientUserName = userName,
            };

            var result = client.CheckIfClientExists(request);

            return new ClientExistModel { IsExist = result.IsExisting };
        }
    }

    public class ClientExistModel
    {
        public bool IsExist { get; set; }
    }
}
