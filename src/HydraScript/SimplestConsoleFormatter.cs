using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace HydraScript;

[ExcludeFromCodeCoverage]
internal class SimplestConsoleFormatter() : ConsoleFormatter(nameof(SimplestConsoleFormatter))
{
    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider scopeProvider,
        TextWriter textWriter)
    {
        if (logEntry.LogLevel is LogLevel.Error)
        {
            var message = logEntry.Formatter.Invoke(logEntry.State, logEntry.Exception);
            textWriter.WriteLine($"{message}: {logEntry.Exception?.Message}");
#if DEBUG
            textWriter.WriteLine($"{logEntry.Exception?.StackTrace}");
#endif
            return;
        }

        if (logEntry.LogLevel is not LogLevel.Information)
            return;
        textWriter.WriteLine(logEntry.State);
    }
}