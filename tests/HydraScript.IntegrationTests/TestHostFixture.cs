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

    public readonly string[] InMemoryScript = ["file.js"];

    public Parser GetRunner(
        ITestOutputHelper testOutputHelper,
        Action<IServiceCollection>? configureTestServices = null) =>
        Program.GetRunner(configureHost: builder => builder
                .ConfigureLogging(x =>
                {
                    x.ClearProviders();
                    x.AddXUnit(testOutputHelper);
                })
                .ConfigureServices((context, services) =>
                {
                    var fileInfo = context.GetInvocationContext().ParseResult
                        .GetValueForArgument(Program.Command.PathArgument);
                    services
                        .AddDomain()
                        .AddApplication()
                        .AddInfrastructure(dump: false, fileInfo);
                    services.AddSingleton(Writer);
                    configureTestServices?.Invoke(services);
                })
                .UseCommandHandler<ExecuteCommand, ExecuteCommandHandler>(),
            useDefault: false);

    public void Dispose() => Writer.Dispose();
}