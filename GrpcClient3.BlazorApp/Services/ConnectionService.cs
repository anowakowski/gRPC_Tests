using Grpc.Net.Client;
using GrpcClient;

namespace GrpcClient3.BlazorApp.Services
{
    public class ConnectionService
    {
        private readonly string _machineName; 
        private readonly string _userName; 
        public ConnectionService()
        {
            _machineName = Environment.MachineName.ToString();
            _userName = Environment.UserName.ToString();
        }
        public ClientExistModel ChceckIfClientIsInstalledOnMachine()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7187");
            var client = new Connections.ConnectionsClient(channel);

            var request = new ClientExistRequest
            {
                ClientMachineName = _machineName,
                ClientUserName = _userName,
            };

            var result = client.CheckIfClientExists(request);

            return new ClientExistModel { IsExist = result.IsExisting };
        }

        public void SendConnectToElement()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7187");
            var client = new Connections.ConnectionsClient(channel);

            var request = new ConnectToElementRequest
            {
                ElementName = "Element 1",
                ClientMachineName = _machineName,
                ClientUserName = _userName
            };

            client.SendConnectToElement(request);
        }
    }

    public class ClientExistModel
    {
        public bool IsExist { get; set; }
    }
}
