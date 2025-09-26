using HydraScript.Domain.BackEnd;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace HydraScript.Infrastructure;

internal partial class LoggingWriter(ILogger<LoggingWriter> logger) : IOutputWriter
{
    [ZLoggerMessage(Level = LogLevel.Information, Message = "{obj}")]
    private static partial void WriteLine(ILogger<LoggingWriter> logger, object? obj);

    public void WriteLine(object? obj) => WriteLine(logger, obj);

    [ZLoggerMessage(Level = LogLevel.Error, Message = "{message}")]
    private static partial void WriteError(ILogger<LoggingWriter> logger, Exception e, string message);

    public void WriteError(Exception e, string message) => WriteError(logger, e, message);
}