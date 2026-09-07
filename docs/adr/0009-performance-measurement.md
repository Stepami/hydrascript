# ADR 0009: Measure interpreter optimizations with BenchmarkDotNet

- Status: Accepted
- Decision date: 2025-09-25
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

Interpreter allocation and throughput work needed measurements. Follow-up changes introduced ZLogger, ZString, and ZLinq and published before/after data.

## Decision

Keep a BenchmarkDotNet harness with managed .NET and Native AOT jobs and memory diagnostics. Use the adopted logging/string/query libraries where the code already relies on them; substantiate further performance changes with a comparable workload and correctness checks.

## Consequences

Historical benchmark numbers are evidence for those runs, not universal performance guarantees. The current harness shuffles sample order, executes a subset, and reuses the provider; account for workload and state differences when comparing changes. Preserve builder lifetime/reentrancy rules when editing ZString code.

## Alternatives

Conventional logging, string-building, and LINQ implementations preceded the changes. The sources provide some measured comparisons, not a requirement to replace every use of those APIs.

## Evidence

- [PR #180](https://github.com/Stepami/hydrascript/pull/180), [PR #181](https://github.com/Stepami/hydrascript/pull/181), [PR #182](https://github.com/Stepami/hydrascript/pull/182), [PR #183](https://github.com/Stepami/hydrascript/pull/183); [#169](https://github.com/Stepami/hydrascript/issues/169), [#170](https://github.com/Stepami/hydrascript/issues/170), [#171](https://github.com/Stepami/hydrascript/issues/171). Shipped in [v2.4.0](https://github.com/Stepami/hydrascript/releases/tag/v2.4.0)/[milestone #9](https://github.com/Stepami/hydrascript/milestone/9).
- Later dump regression: [#227](https://github.com/Stepami/hydrascript/issues/227), [PR #228](https://github.com/Stepami/hydrascript/pull/228), [v2.6.1](https://github.com/Stepami/hydrascript/releases/tag/v2.6.1)/[milestone #14](https://github.com/Stepami/hydrascript/milestone/14).
- Current [benchmark](../../benchmarks/HydraScript.Benchmarks/InvokeBenchmark.cs) and [recorded results](../../benchmarks/Readme.md).