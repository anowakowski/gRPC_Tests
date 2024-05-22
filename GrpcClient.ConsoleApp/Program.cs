using Grpc.Core;
using Grpc.Net.Client;
using GrpcClient;

/*
var input = new HelloRequest
{
    Name = "Peter"
};

var channel = GrpcChannel.ForAddress("https://localhost:7187");
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(input);

Console.WriteLine(reply.Message);
*/



/*
var channel = GrpcChannel.ForAddress("https://localhost:7187");
var client = new Customer.CustomerClient(channel);

var input = new CustomerLookupModel
{
    UserId = 1
};

var reply = await client.GetCustomerInfoAsync(input);

Console.WriteLine("New customer from GetCustomerInfoAsync");
Console.WriteLine(reply.FirstName + " " + reply.LastName);

Console.WriteLine();
Console.WriteLine("New Customer List");
Console.WriteLine();

using (var call = client.GetNewCustomers(new NewCustomerRequest()))
{
    while (await call.ResponseStream.MoveNext(CancellationToken.None))
    {
        var currentCustommer = call.ResponseStream.Current;

        Console.WriteLine(currentCustommer.FirstName + " " + currentCustommer.LastName);
    }
}
*/

//var channel = new Channel("localhost", 5184, ChannelCredentials.Insecure);
Console.WriteLine("Client1");

var channel = GrpcChannel.ForAddress("https://localhost:7187");
var client = new Connections.ConnectionsClient(channel);

string machineName = Environment.MachineName.ToString();
string userName = Environment.UserName.ToString();

var clientData = new ClientRequest()
{
    ClientName = "client1",
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

Console.ReadLine();
