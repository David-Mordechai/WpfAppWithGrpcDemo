using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcCommands;
using GrpcReports;

var channel = GrpcChannel.ForAddress("http://localhost:5151");

// Sending Commands to Server
var commands = new Commands.CommandsClient(channel);

var command1Replay = await commands.SendCommand1Async(new Command1Dto{Name = "Command1", Description = "First command"});
Console.WriteLine($"Command1 replay success: {command1Replay.Success}");

var command2Replay = await commands.SendCommand2Async(new Command2Dto { Name = "Command2", Description = "Second command"});
Console.WriteLine($"Command2 replay success: {command2Replay.Success}");


// Receiving reports from Server
var reports = new Reports.ReportsClient(channel);

var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
using var streamingCall = reports.Report1(new Empty(), cancellationToken: cts.Token);

try
{
    await foreach (var report in streamingCall.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
    {
        Console.WriteLine($"{report.DateTimeStamp.ToDateTime():s} | {report.Name} | {report.Description}");
    }
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
{
    Console.WriteLine("Stream cancelled.");
}

Console.ReadLine();