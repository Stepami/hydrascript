using HydraScript.Domain.BackEnd;
using Microsoft.Extensions.Logging;

namespace HydraScript.Infrastructure;

internal partial class LoggingWriter(ILogger<LoggingWriter> logger) : IOutputWriter
{
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "`{obj}`")]
    public partial void WriteLine(object? obj);

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "`{message}`")]
    public partial void WriteError(Exception e, string message);
}