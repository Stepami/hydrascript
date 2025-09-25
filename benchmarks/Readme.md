```

BenchmarkDotNet v0.15.4, Windows 10 (10.0.19045.6332/22H2/2022Update)
12th Gen Intel Core i7-12650H 2.30GHz, 1 CPU, 16 logical and 10 physical cores
.NET SDK 9.0.305
  [Host] : .NET 9.0.9 (9.0.9, 9.0.925.41916), X64 RyuJIT x86-64-v3

Job=InProcess  Toolchain=InProcessEmitToolchain  

```
| Method |     Mean |     Error |    StdDev |     Gen0 |     Gen1 | Allocated |
|--------|---------:|----------:|----------:|---------:|---------:|----------:|
| Invoke | 8.748 ms | 0.1720 ms | 0.2574 ms | 828.1250 | 343.7500 |  10.06 MB |
