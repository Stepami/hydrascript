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
| Invoke | .NET 9.0      | .NET 9.0      | 9.496 ms | 0.1434 ms | 0.1341 ms | 1625.0000 | 546.8750 |   9.79 MB |
| Invoke | NativeAOT 9.0 | NativeAOT 9.0 | 7.418 ms | 0.0217 ms | 0.0203 ms | 1632.8125 | 453.1250 |   9.78 MB |
