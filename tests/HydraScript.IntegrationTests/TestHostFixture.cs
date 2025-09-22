using System.IO.Abstractions;
using HydraScript.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

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
        var serviceProvider = Program.GetServiceProvider(
            new FileInfo(options.FileName),
            options.Dump,
            services =>
            {
                services.AddLogging(x => x.ClearProviders()
                    .AddXUnit(new ImplicitTestOutputHelperAccessor())
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