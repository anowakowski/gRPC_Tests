using Grpc.Core;
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

        public ConnectedToElementModel SendConnectToElement()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7187");
            var client = new Connections.ConnectionsClient(channel);

            var request = new ConnectToElementRequest
            {
                ElementName = "Element 1",
                ClientMachineName = _machineName,
                ClientUserName = _userName
            };

            var result = client.SendConnectToElement(request);

            var model = new ConnectedToElementModel
            {
                HasError = result.HasError,
                ErrorMessage = result.ErrorMessage,
                IsConnectedToElementSuccessfully = result.IsConnectedToElementSuccessfully
            };

            return model;
        }

        public async Task<FinishedConnectionToElementModel> SubscribeToElement()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7187");
            var client = new Connections.ConnectionsClient(channel);

            var request = new ConnectToElementRequest
            {
                ElementName = "Element 1",
                ClientMachineName = _machineName,
                ClientUserName = _userName
            };

            var response = new SubscribeToConnectedElementResponse();

            try
            {
                var cancellationToken = new CancellationTokenSource();

                var subscribeToElement = client.SubscribeToConnectedElement(request);

                while (await subscribeToElement.ResponseStream.MoveNext(cancellationToken.Token))
                {
                    var message = subscribeToElement.ResponseStream.Current;
                    response = message;

                    if (message.ConnectionWasFinished)
                    {
                        //cancellationToken.Cancel();
                    }
                }

                if (response.ConnectionWasFinished)
                {
                    return new FinishedConnectionToElementModel
                    {
                        IsStopConnectToElement = true,
                    };
                }
            }
            catch (RpcException e)
            {

            }
            catch (Exception ex)
            {

            }


            return null;
        }
    }

    public class ClientExistModel
    {
        public bool IsExist { get; set; }
    }

    public class ConnectedToElementModel
    {
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsConnectedToElementSuccessfully { get; set; }
    }

    public class FinishedConnectionToElementModel
    {
        public bool IsStopConnectToElement { get; set; }
    }
}
