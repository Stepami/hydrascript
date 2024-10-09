using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using HydraScript.Infrastructure;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace HydraScript.IntegrationTests;

public class TestHostFixture(
    Xunit.DependencyInjection.ITestOutputHelperAccessor accessor) :
    IDisposable, ITestOutputHelperAccessor
{
    private readonly List<string> _logMessages = [];

    public readonly string[] InMemoryScript = ["file.js"];
    public IReadOnlyCollection<string> LogMessages => _logMessages;

    public ITestOutputHelper? OutputHelper
    {
        get => accessor.Output;
        set { }
    }

    public Parser GetRunner(Action<IServiceCollection>? configureTestServices = null) =>
        Program.GetRunner(configureHost: builder => builder
                .ConfigureLogging(x => x.ClearProviders()
                    .AddXUnit(this)
                    .AddFakeLogging(options =>
                    {
                        options.OutputSink = logMessage => _logMessages.Add(logMessage);
                        options.OutputFormatter = fakeLogRecord =>
                            fakeLogRecord.Level switch
                            {
                                LogLevel.Error => $"{fakeLogRecord.Message} {fakeLogRecord.Exception?.Message}",
                                _ => fakeLogRecord.ToString()
                            };
                    }))
                .ConfigureServices((context, services) =>
                {
                    services.Configure<InvocationLifetimeOptions>(options => options.SuppressStatusMessages = true);
                    var fileInfo = context.GetInvocationContext().ParseResult
                        .GetValueForArgument(Program.Command.PathArgument);
                    services
                        .AddDomain()
                        .AddApplication()
                        .AddInfrastructure(dump: false, fileInfo);
                    configureTestServices?.Invoke(services);
                })
                .UseCommandHandler<ExecuteCommand, ExecuteCommandHandler>(),
            useDefault: false);

    public void Dispose() => _logMessages.Clear();
}