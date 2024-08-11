using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using HydraScript.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace HydraScript.IntegrationTests;

public class TestHostFixture : IDisposable
{
    public readonly TextWriter Writer = new StringWriter();

    public Parser GetRunner(ITestOutputHelper testOutputHelper) =>
        Program.GetRunner(configureHost: builder => builder
                .ConfigureLogging(x =>
                {
                    x.ClearProviders();
                    x.AddXUnit(testOutputHelper);
                })
                .ConfigureServices((context, services) =>
                {
                    var parseResult = context.GetInvocationContext().ParseResult;
                    var fileInfo = parseResult.GetValueForArgument(Program.Command.PathArgument);
                    var dump = parseResult.GetValueForOption(Program.Command.DumpOption);
                    services
                        .AddDomain()
                        .AddApplication()
                        .AddInfrastructure(dump, fileInfo);
                    services.AddSingleton(Writer);
                })
                .UseCommandHandler<ExecuteCommand, ExecuteCommandHandler>(),
            useDefault: false);

    public void Dispose() => Writer.Dispose();
}