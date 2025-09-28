using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using HydraScript;
using HydraScript.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZLogger;

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

    private static ServiceProvider GetServiceProvider(FileInfo fileInfo, bool dump) =>
        new ServiceCollection()
            .AddLogging(c => c.ClearProviders().AddZLoggerConsole())
            .AddDomain<GeneratedRegexContainer>()
            .AddApplication()
            .AddInfrastructure(dump, fileInfo)
            .BuildServiceProvider();
}