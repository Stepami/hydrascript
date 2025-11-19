using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Running;
using HydraScript.Benchmarks;

[assembly: ExcludeFromCodeCoverage]

BenchmarkRunner.Run<InvokeBenchmark>();