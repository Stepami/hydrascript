```

BenchmarkDotNet v0.15.4, macOS Ventura 13.7.2 (22H313) [Darwin 22.6.0]
Apple M1 Pro, 1 CPU, 10 logical and 10 physical cores
.NET SDK 9.0.305
  [Host]        : .NET 9.0.9 (9.0.9, 9.0.925.41916), Arm64 RyuJIT armv8.0-a
  .NET 9.0      : .NET 9.0.9 (9.0.9, 9.0.925.41916), Arm64 RyuJIT armv8.0-a
  NativeAOT 9.0 : .NET 9.0.9, Arm64 NativeAOT armv8.0-a


```
| Method | Job           | Runtime       |     Mean |     Error |    StdDev |      Gen0 |     Gen1 | Allocated |
|--------|---------------|---------------|---------:|----------:|----------:|----------:|---------:|----------:|
| Invoke | .NET 9.0      | .NET 9.0      | 7.427 ms | 0.1477 ms | 0.1701 ms | 1593.7500 | 406.2500 |   9.57 MB |
| Invoke | NativeAOT 9.0 | NativeAOT 9.0 | 7.215 ms | 0.0836 ms | 0.0699 ms | 1593.7500 | 453.1250 |   9.56 MB |
