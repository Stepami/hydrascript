using System.IO.Abstractions;
using HydraScript.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Serilog;
using Serilog.Sinks.XUnit3;

namespace HydraScript.IntegrationTests;

public class TestHostFixture : IDisposable
{
    public const string ScriptFileName = "file";
    public record Options(
        string FileName = ScriptFileName + ".js",
        bool Dump = false,
        bool MockFileSystem = true,
        string InMemoryScript = "");

    public class Runner(ServiceProvider serviceProvider, Executor executor): IDisposable
    {
        public ServiceProvider ServiceProvider => serviceProvider;
        public int Invoke() => executor.Invoke();

        public void Dispose()
        {
            serviceProvider.Dispose();
        }
    }

    private readonly List<string> _logMessages = [];
    public IReadOnlyCollection<string> LogMessages => _logMessages;

    public Runner GetRunner(Options options, Action<IServiceCollection>? configureTestServices = null)
    {
        var services = new ServiceCollection()
            .AddDomain<GeneratedRegexContainer>()
            .AddApplication()
            .AddInfrastructure(options.Dump, new FileInfo(options.FileName));
        const string serilogTemplate = "[{Timestamp:HH:mm:ss} {Level:u} [{SourceContext}]]{NewLine}{Message:lj} {Exception}";
        services.AddLogging(x => x.ClearProviders()
            .AddSerilog(new LoggerConfiguration().WriteTo.XUnit3TestOutput(serilogTemplate).CreateLogger())
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

        var serviceProvider = services.BuildServiceProvider();
        var executor = serviceProvider.GetRequiredService<Executor>();
        return new Runner(serviceProvider, executor);
    }

    public void Dispose() => _logMessages.Clear();
}