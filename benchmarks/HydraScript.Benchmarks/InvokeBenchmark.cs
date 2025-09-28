using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using HydraScript.Benchmarks;
using HydraScript.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

[assembly: ExcludeFromCodeCoverage]

BenchmarkRunner.Run<InvokeBenchmark>();

[SimpleJob(RuntimeMoniker.Net90), MemoryDiagnoser]
public class InvokeBenchmark
{
    private ServiceProvider? _provider;
    private Executor? _executor;
    private readonly UpdatableFileOptions _updatableFileOptions = new(new FileInfo(nameof(FileInfo)));

    private readonly IReadOnlyList<FileInfo> _scriptPaths =
        Directory.GetFiles("Samples")
            .Select(x => new FileInfo(x))
            .ToArray();

    [GlobalSetup]
    public void GlobalSetup()
    {
        _provider = new ServiceCollection()
            .AddLogging(x => x.ClearProviders().AddProvider(NullLoggerProvider.Instance))
            .AddDomain<GeneratedRegexContainer>()
            .AddApplication()
            .AddInfrastructure(dump: false, _updatableFileOptions.Value)
            .AddSingleton<IOptions<FileInfo>>(_updatableFileOptions)
            .BuildServiceProvider();
        _executor = _provider.GetRequiredService<Executor>();
    }

    [GlobalCleanup]
    public void GlobalCleanup() => _provider?.Dispose();

    [Benchmark]
    public void Invoke()
    {
        for (var i = 0; i < _scriptPaths.Count; i++)
        {
            _updatableFileOptions.Update(_scriptPaths[i]);
            _executor?.Invoke();
        }
    }
}