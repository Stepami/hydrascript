using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using HydraScript;
using HydraScript.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

return CliParser.Parse(GetCommand(), args).Invoke();

[ExcludeFromCodeCoverage]
internal static partial class Program
{
    private static ExecuteCommand GetCommand()
    {
        ExecuteCommand command = new();
        command.SetAction(parseResult =>
        {
            var fileInfo = parseResult.GetValue(command.PathArgument)!;
            var dump = parseResult.GetValue(command.DumpOption);
            using var serviceProvider = GetServiceProvider(fileInfo, dump);
            var executor = serviceProvider.GetRequiredService<Executor>();
            return executor.Invoke();
        });
        return command;
    }

    private const string SuppressMessage = $"{nameof(ConsoleFormatterOptions)} is used";

    [UnconditionalSuppressMessage(
        category: "ReflectionAnalysis",
        checkId: "IL2026:RequiresUnreferencedCode",
        Justification = SuppressMessage)]
    [UnconditionalSuppressMessage(
        category: "AotAnalysis",
        checkId: "IL3050:RequiresDynamicCode",
        Justification = SuppressMessage)]
    private static ServiceProvider GetServiceProvider(FileInfo fileInfo, bool dump)
    {
        var services = new ServiceCollection();
        services.AddLogging(c => c.ClearProviders()
            .AddConsole(options => options.FormatterName = nameof(SimplestConsoleFormatter))
            .AddConsoleFormatter<SimplestConsoleFormatter, ConsoleFormatterOptions>());
        services
            .AddDomain()
            .AddApplication()
            .AddInfrastructure(dump, fileInfo);
        return services.BuildServiceProvider();
    }
}