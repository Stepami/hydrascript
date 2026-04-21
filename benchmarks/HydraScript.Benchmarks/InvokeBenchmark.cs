using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using HydraScript.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace HydraScript.Benchmarks;

[SimpleJob(RuntimeMoniker.Net10_0)]
[SimpleJob(RuntimeMoniker.NativeAot10_0)]
[MemoryDiagnoser]
public class InvokeBenchmark
{
    private ServiceProvider? _provider;
    private Executor? _executor;

    private readonly UpdatableFileOptions _updatableFileOptions = new(new FileInfo(nameof(FileInfo)));
    private readonly FileInfo[] _scriptPaths =
        Directory.GetFiles("Samples")
            .Select(x => new FileInfo(x))
            .ToArray();

    private readonly int _benchmarkSize;

    public InvokeBenchmark() => _benchmarkSize = _scriptPaths.Length / 3;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _provider = new ServiceCollection()
            .AddLogging(x => x.ClearProviders().AddProvider(NullLoggerProvider.Instance))
            .AddDomain()
            .AddApplication()
            .AddInfrastructure(dump: false, _updatableFileOptions.Value)
            .AddSingleton<IOptions<FileInfo>>(_updatableFileOptions)
            .BuildServiceProvider();
        _executor = _provider.GetRequiredService<Executor>();
    }

    [IterationSetup]
    public void IterationSetup() => Random.Shared.Shuffle(_scriptPaths);

    [GlobalCleanup]
    public void GlobalCleanup() => _provider?.Dispose();

    [Benchmark]
    public void Invoke()
    {
        for (var i = 0; i <_benchmarkSize; i++)
        {
            _updatableFileOptions.Update(_scriptPaths[i]);
            _executor?.Invoke();
        }
    }
}