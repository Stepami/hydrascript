using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using HydraScript.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

BenchmarkRunner.Run<InvokeBenchmark>();

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.NativeAot90)]
public class InvokeBenchmark
{
    private ServiceProvider? _provider;
    private Executor? _executor;

    private readonly string _samplesPath = Path.Combine(
        paths: Enumerable.Repeat("..", 6).ToArray()
            .Concat(["hydrascript", "tests", "HydraScript.IntegrationTests", "Samples"])
            .ToArray());
    public IEnumerable<string> ScriptPaths => Directory.GetFiles(_samplesPath);
    [ParamsSource(nameof(ScriptPaths))]
    public required string ScriptPath;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var services = new ServiceCollection();
        services.AddLogging(c => c.ClearProviders().AddConsole())
            .AddDomain()
            .AddApplication()
            .AddInfrastructure(dump: false, new FileInfo(ScriptPath));
        _provider = services.BuildServiceProvider();
        _executor = _provider.GetRequiredService<Executor>();
    }

    [GlobalCleanup]
    public void GlobalCleanup() => _provider?.Dispose();

    [Benchmark]
    public void Invoke() => _executor?.Invoke();
}