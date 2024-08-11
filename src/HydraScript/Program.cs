using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using HydraScript;
using HydraScript.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

return GetRunner(ConfigureHost).Invoke(args);

public static partial class Program
{
    public static readonly ExecuteCommand Command = new();

    public static Parser GetRunner(Action<IHostBuilder> configureHost) =>
        new CommandLineBuilder(Command)
            .UseHost(Host.CreateDefaultBuilder, configureHost)
            .UseHelp()
            .UseVersionOption()
            .Build();

    private static void ConfigureHost(IHostBuilder builder) => builder
        .ConfigureServices((context, services) =>
        {
            services.AddLogging(c => c.ClearProviders());
            var parseResult = context.GetInvocationContext().ParseResult;
            var fileInfo = parseResult.GetValueForArgument(Command.PathArgument);
            var dump = parseResult.GetValueForOption(Command.DumpOption);
            services
                .AddDomain()
                .AddApplication()
                .AddInfrastructure(dump, fileInfo);
        })
        .UseDefaultServiceProvider((_, options) => options.ValidateScopes = true)
        .UseCommandHandler<ExecuteCommand, ExecuteCommandHandler>();
}