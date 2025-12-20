using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.BackEnd;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace HydraScript.Infrastructure;

[ExcludeFromCodeCoverage]
internal partial class HydraScriptConsole(ILogger<HydraScriptConsole> logger) : IConsole
{
    [ZLoggerMessage(Level = LogLevel.Information, Message = "{obj}")]
    private static partial void WriteLine(ILogger<HydraScriptConsole> logger, object? obj);

    public void WriteLine(object? obj) => WriteLine(logger, obj);

    [ZLoggerMessage(Level = LogLevel.Error, Message = "{message}")]
    private static partial void WriteError(ILogger<HydraScriptConsole> logger, Exception e, string message);

    public void WriteError(Exception e, string message) => WriteError(logger, e, message);

    public string ReadLine() => Console.ReadLine() ?? string.Empty;
}