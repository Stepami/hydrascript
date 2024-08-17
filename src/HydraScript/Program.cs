using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using HydraScript;
using HydraScript.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

return GetRunner(ConfigureHost).Invoke(args);

internal static partial class Program
{
    internal static readonly ExecuteCommand Command = new();

    internal static Parser GetRunner(Action<IHostBuilder> configureHost, bool useDefault = true)
    {
        var builder = new CommandLineBuilder(Command)
            .UseHost(Host.CreateDefaultBuilder, configureHost);
        if (useDefault)
            builder = builder.UseDefaults();
        return builder.Build();
    }

    private static void ConfigureHost(IHostBuilder builder) => builder
        .ConfigureServices((context, services) =>
        {
            services.AddLogging(c => c.ClearProviders().AddConsole());
            services.Configure<InvocationLifetimeOptions>(options => options.SuppressStatusMessages = true);
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