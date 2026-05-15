using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.BackEnd;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace HydraScript.Infrastructure;

[ExcludeFromCodeCoverage]
internal sealed class HydraScriptConsole(ILogger<HydraScriptConsole> logger) : IConsole
{
    public void WriteLine(object? obj) => logger.ZLogInformation($"{obj}");

    public void WriteError(Exception e, string message) => logger.ZLogError(e, $"{message}");

    public string ReadLine() => Console.ReadLine() ?? string.Empty;
}