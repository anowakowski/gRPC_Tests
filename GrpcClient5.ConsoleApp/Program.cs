using Grpc.Net.Client;
using GrpcClient;

Console.WriteLine("Client5");

var channel = GrpcChannel.ForAddress("https://localhost:7187");

var client = new Connections.ConnectionsClient(channel);

string machineName = Environment.MachineName.ToString();
string userName = Environment.UserName.ToString();

var clientData = new ClientRequest()
{
    ClientName = "Client5",
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

Console.WriteLine("Client5 finished");
Console.ReadLine();

