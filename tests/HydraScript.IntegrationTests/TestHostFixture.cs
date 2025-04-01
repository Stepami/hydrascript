using System.IO.Abstractions;
using HydraScript.Infrastructure;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit.Abstractions;

namespace HydraScript.IntegrationTests;

public class TestHostFixture(
    Xunit.DependencyInjection.ITestOutputHelperAccessor accessor) :
    IDisposable, ITestOutputHelperAccessor
{
    public record Options(
        string FileName = ScriptFileName + ".js",
        bool Dump = false,
        bool MockFileSystem = true,
        string InMemoryScript = "");

    public class Runner(IServiceProvider serviceProvider, Executor executor)
    {
        public IServiceProvider ServiceProvider => serviceProvider;
        public int Invoke() => executor.Invoke();
    }

    private readonly List<string> _logMessages = [];

    public const string ScriptFileName = "file";

    public IReadOnlyCollection<string> LogMessages => _logMessages;

    public ITestOutputHelper? OutputHelper
    {
        get => accessor.Output;
        set { }
    }

    public Runner GetRunner(Options options, Action<IServiceCollection>? configureTestServices = null)
    {
        var serviceProvider = Program.GetServiceProvider(
            new FileInfo(options.FileName),
            options.Dump,
            services =>
            {
                services.AddLogging(x => x.ClearProviders()
                    .AddXUnit(this)
                    .AddFakeLogging(fakeLogOptions =>
                    {
                        fakeLogOptions.OutputSink = logMessage => _logMessages.Add(logMessage);
                        fakeLogOptions.OutputFormatter = fakeLogRecord =>
                            fakeLogRecord.Level switch
                            {
                                LogLevel.Error => $"{fakeLogRecord.Message} {fakeLogRecord.Exception?.Message}",
                                _ => fakeLogRecord.ToString()
                            };
                    }));

                if (options.MockFileSystem)
                {
                    var fileSystem = Substitute.For<IFileSystem>();
                    services.AddSingleton(fileSystem);
                }

                if (!string.IsNullOrWhiteSpace(options.InMemoryScript))
                {
                    var sourceCodeProvider = Substitute.For<ISourceCodeProvider>();
                    sourceCodeProvider.GetText().ReturnsForAnyArgs(options.InMemoryScript);
                    services.AddSingleton(sourceCodeProvider);
                }

                configureTestServices?.Invoke(services);
            });
        var executor = serviceProvider.GetRequiredService<Executor>();
        return new Runner(serviceProvider, executor);
    }

    public void Dispose() => _logMessages.Clear();
}