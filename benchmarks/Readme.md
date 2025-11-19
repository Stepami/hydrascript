```
BenchmarkDotNet v0.15.6, Windows 10 (10.0.19045.6456/22H2/2022Update)
12th Gen Intel Core i7-12650H 2.30GHz, 1 CPU, 16 logical and 10 physical cores
.NET SDK 10.0.100
  [Host]         : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  .NET 10.0      : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  NativeAOT 10.0 : .NET 10.0.0, X64 NativeAOT x86-64-v3
```

| Method | Job            | Runtime        |      Mean |     Error |    StdDev |     Gen0 |     Gen1 | Allocated |
|--------|----------------|----------------|----------:|----------:|----------:|---------:|---------:|----------:|
| Invoke | .NET 10.0      | .NET 10.0      | 11.569 ms | 0.2307 ms | 0.4966 ms | 828.1250 | 312.5000 |  10.04 MB |
| Invoke | NativeAOT 10.0 | NativeAOT 10.0 |  8.227 ms | 0.1093 ms | 0.0969 ms | 828.1250 | 218.7500 |  10.05 MB |
