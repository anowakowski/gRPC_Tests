using GrpcClient4.PerformanceTest.ConsoleApp.Services;

Console.WriteLine("Client4");

await ConnectionsManagerTestService.RunTest();


Console.WriteLine("Client4 finished");
Console.ReadLine();
