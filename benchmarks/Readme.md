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
| Invoke | .NET 9.0      | .NET 9.0      | 9.525 ms | 0.1615 ms | 0.1658 ms | 1562.5000 | 500.0000 |   9.37 MB |
| Invoke | NativeAOT 9.0 | NativeAOT 9.0 | 7.321 ms | 0.0544 ms | 0.0509 ms | 1562.5000 | 468.7500 |   9.36 MB |
