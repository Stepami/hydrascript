using HydraScript.Domain.BackEnd;
using Microsoft.Extensions.Logging;

namespace HydraScript.Infrastructure;

internal class LoggingWriter(ILogger<LoggingWriter> logger) : IOutputWriter
{
    public void WriteLine(object? obj) =>
        logger.LogInformation("{Object}", obj);

    public void WriteError(Exception e, string message) =>
        logger.LogError(e, "{Message}", message);
}