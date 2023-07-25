using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcReports;
using Microsoft.Extensions.Logging;

namespace WpfApp.GrpcServices;

internal class ReportsService : Reports.ReportsBase
{
    private readonly ILogger<ReportsService> _logger;

    public ReportsService(ILogger<ReportsService> logger)
    {
        _logger = logger;
    }

    public override async Task Report1(Empty request, IServerStreamWriter<Report1Dto> responseStream, ServerCallContext context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await Task.Delay(500); // Gotta look busy

            var report = new Report1Dto
            {
                Name = "Report1",
                Description = "This is report 1",
                DateTimeStamp = Timestamp.FromDateTime(DateTime.UtcNow),
            };

            _logger.LogInformation("Sending Report1 data...");

            await responseStream.WriteAsync(report);
        }
    }
}