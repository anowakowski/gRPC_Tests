using Grpc.Net.Client;
using GrpcClient;

Console.WriteLine("Client2");

//var channel = new Channel("localhost", 5184, ChannelCredentials.Insecure);
var channel = GrpcChannel.ForAddress("https://localhost:7187");
var client = new Connections.ConnectionsClient(channel);

//client.Subscribe(new ClientRequest { ClientName = "Client2" });

Console.WriteLine("Set some char and message to client1");
var input = Console.ReadLine();

if (!string.IsNullOrEmpty(input))
{
    var logSomeInfo = new LogSomeInfo
    {
        Name = "Client1",
        Id = 2
    };

    client.SendMessage(logSomeInfo);
}    

Console.ReadLine();
