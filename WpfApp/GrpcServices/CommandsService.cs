using System.Threading.Tasks;
using Grpc.Core;
using GrpcCommands;
using Microsoft.Extensions.Logging;

namespace WpfApp.GrpcServices;

internal class CommandsService : Commands.CommandsBase
{
    private readonly ILogger<CommandsService> _logger;

    public CommandsService(ILogger<CommandsService> logger)
    {
        _logger = logger;
    }

    public override Task<ResultDto> SendCommand1(Command1Dto request, ServerCallContext context)
    {
        _logger.LogInformation("Command 1 invoked");
        return Task.FromResult(new ResultDto { Success = true });
    }

    public override Task<ResultDto> SendCommand2(Command2Dto request, ServerCallContext context)
    {
        _logger.LogInformation("Command 2 invoked");
        return Task.FromResult(new ResultDto { Success = true });
    }
}