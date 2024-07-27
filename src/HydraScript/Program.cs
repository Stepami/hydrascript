using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using HydraScript;
using HydraScript.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var command = new ExecuteCommand();
var runner = new CommandLineBuilder(command)
    .UseHost(
        Host.CreateDefaultBuilder,
        configureHost: builder => builder
            .ConfigureServices((context, services) =>
            {
                services.AddLogging(c => c.ClearProviders());
                var parseResult = context.GetInvocationContext().ParseResult;
                var fileInfo = parseResult.GetValueForArgument(command.PathArgument);
                var dump = parseResult.GetValueForOption(command.DumpOption);
                services
                    .AddDomain()
                    .AddApplication()
                    .AddInfrastructure(dump, fileInfo);
            })
            .UseDefaultServiceProvider((_, options) => options.ValidateScopes = true)
            .UseCommandHandler<ExecuteCommand, ExecuteCommandHandler>())
    .UseDefaults().Build();

await runner.InvokeAsync(args);