

using Grpc.Core;

namespace GrpcServer.Services
{
    public class ConnectionsTestService : Connections.ConnectionsBase 
    {
        private static readonly Dictionary<ClientRequest, IServerStreamWriter<LogSomeInfo>> _messageSubscriptions = 
            new Dictionary<ClientRequest, IServerStreamWriter<LogSomeInfo>>();

        private static readonly Dictionary<ConnectToElementRequest, IServerStreamWriter<SubscribeToConnectedElementResponse>> _connectedToElementSubscriptions =
            new Dictionary<ConnectToElementRequest, IServerStreamWriter<SubscribeToConnectedElementResponse>>();

        private readonly NewEmptyRequest _newEmptyRequest = new NewEmptyRequest();
        public ConnectionsTestService() { }

        public override async Task Subscribe(ClientRequest request, IServerStreamWriter<LogSomeInfo> responseStream, ServerCallContext context)
        {
            try
            {
                if (_messageSubscriptions.ContainsKey(request))
                {
                    return;
                }

                await responseStream.WriteAsync(
                    new LogSomeInfo
                    {
                        Name = $"{request.ClientName} is listening for messages!",
                    }).ConfigureAwait(false);

                _messageSubscriptions.TryAdd(request, responseStream);

                await Task.Delay(-1, context.CancellationToken);
            }
            catch (Exception ex)
            {
                _messageSubscriptions.Remove(request);
            }
        }

        public override async Task SubscribeToConnectedElement(ConnectToElementRequest request, IServerStreamWriter<SubscribeToConnectedElementResponse> responseStream, ServerCallContext context)
        {
            try
            {
                if (_connectedToElementSubscriptions.ContainsKey(request))
                {
                    return;
                }

                await responseStream.WriteAsync
                    (
                        new SubscribeToConnectedElementResponse
                        {

                        }
                    ).ConfigureAwait(false);

                _connectedToElementSubscriptions.TryAdd(request, responseStream);

                await Task.Delay(-1, context.CancellationToken);
            }
            catch (Exception ex) 
            {
                _connectedToElementSubscriptions.Remove(request);
            }
        }

        public override async Task<NewEmptyRequest> SendMessage(LogSomeInfo request, ServerCallContext context)
        {
            foreach(var messageStream in _messageSubscriptions.Values)
            {
                try
                {
                    await messageStream.WriteAsync(request);
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }

            return _newEmptyRequest;
        }

        public override async Task<NewEmptyRequest> SendFinishedConnectToElement(SubscribeToConnectedElementResponse request, ServerCallContext context)
        {
            foreach (var connectedToElementSubscription in _connectedToElementSubscriptions)
            {
                try
                {
                    if (connectedToElementSubscription.Key.ClientMachineName.Equals(request.ClientMachineName) && 
                        connectedToElementSubscription.Key.ClientUserName.Equals(request.ClientUserName) &&
                        connectedToElementSubscription.Key.ElementName.Equals(request.ElementName))
                    {
                        await connectedToElementSubscription.Value.WriteAsync(request);
                    }
                    
                }
                catch(Exception ex)
                {
                    var message = ex.Message;
                }
            }

            return _newEmptyRequest;
        }

        public override async Task<ConnectToElementResponse> SendConnectToElement(ConnectToElementRequest request, ServerCallContext context)
        {
            var response = new ConnectToElementResponse();
            foreach (var subscriber in _messageSubscriptions)
            {
                try
                {
                    if (subscriber.Key.ClientMachineName.Equals(request.ClientMachineName) && subscriber.Key.ClientUserName.Equals(request.ClientUserName))
                    {
                        var logSomeInfo = new LogSomeInfo
                        {
                            Name = request.ElementName,
                            Id = 1
                        };
                        await subscriber.Value.WriteAsync(logSomeInfo);

                        response.ErrorMessage = string.Empty;
                        response.IsConnectedToElementSuccessfully = true;
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Cleint not exist";
                    }
                }
                catch (Exception ex)
                {
                    response.HasError = true;
                    response.IsConnectedToElementSuccessfully = false;
                    response.ErrorMessage = ex.Message;
                }

            }

            return response;
        }

        public override Task<ClientExistResponse> CheckIfClientExists(ClientExistRequest request, ServerCallContext context)
        {
            var messageSubscription = _messageSubscriptions.Keys.FirstOrDefault(x => 
                                            x.ClientMachineName == request.ClientMachineName && 
                                            x.ClientUserName == request.ClientUserName);

            var clientExistResponse = new ClientExistResponse();

            if (messageSubscription != null)
            {
                clientExistResponse.IsExisting = true;
            }

            return Task.FromResult(clientExistResponse);
        }

    }
}
