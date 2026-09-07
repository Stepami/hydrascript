# ADR 0005: Preserve Native AOT and prefer measured, allocation-conscious implementations

- Status: Accepted
- Decision date: 2025-04-03
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

HydraScript should remain a maintainable C# interpreter while being distributed as a small native executable without a separately installed .NET runtime. Native compilation and binary size shaped dependency selection and service composition. Recurring interpreter work also motivated lower-allocation query, text, and logging implementations.

These goals are related but not interchangeable: AOT compatibility, executable size, startup, throughput, and allocations have different tradeoffs.

## Decision

Publish Native AOT binaries for `win-x64`, `linux-x64`, and `osx-arm64`. The .NET tool also supplies native RID packages and an `any` managed fallback built with `PublishAot=false`.

Preserve AOT/trimming compatibility and prefer lower-allocation implementations in recurring paths when they preserve semantics and their benefit can be demonstrated. The current implementation makes the following choices:

| Area | Adopted approach |
| --- | --- |
| Distribution policy | `PublishAot=true`; Release favors size, disables debug symbols/stack-trace support, and uses invariant globalization |
| Composition | Explicit service registration and keyed dump decorators instead of runtime discovery/decorator infrastructure |
| Source generation | Generated lexer/regex, Visitor.NET visitable dispatch, and a source-generated JSON context for supported runtime values |
| Queries | ZLinq's `AsValueEnumerable()` in analysis, emission, type handling, and traversal; ordinary LINQ remains where it is still used |
| Text and logging | ZString builders/join/concat and ZLogger's interpolated console logging |
| Small operations and traversal | Selected structs/record structs and a reusable traversal queue with explicit disposal |
| Scanning and lookup | Span-based newline scanning with `SearchValues<char>`, read-heavy frozen token lookup, and lazy token enumeration |

Value types are selective: token-definition DTOs, operation descriptors, operator implementations, and the AST traversal enumerator use them. They do not establish an allocation-free contract; storing an operator struct as `IOperator` can box it. Token and coordinate records are still reference types.

Spans are used, but the current source does not use `stackalloc`. Stack allocation is not a historical adoption to claim or a blanket requirement for future edits. Its suitability depends on buffer size, lifetime, safety, and measured benefit.

## Consequences

- Evaluate new runtime dependencies and reflection-dependent code against AOT/trimming constraints. Preserve generated serializer coverage when changing runtime value shapes.
- Native binaries remain platform-specific, and the managed fallback still has runtime requirements. Preserve the fallback override rather than treating every package as a native executable.
- Size-oriented Release settings reduce diagnostic information and change globalization assumptions. Compare performance under the relevant configuration rather than assuming Debug and native Release behave alike.
- Prefer existing ZLinq/ZString/ZLogger patterns in their established paths, but do not replace every LINQ query or class solely on the assumption that another API or a struct must be faster.
- Respect builder and reusable-enumerator lifetimes, disposal, nesting, and reentrancy. Lower allocation is not useful if it changes behavior; dumping deliberately materializes tokens for inspection.
- Use the BenchmarkDotNet managed/AOT jobs and memory diagnostics for comparable measurements. The current harness shuffles samples, executes a subset, and reuses a provider; control workload and state before attributing a difference. Historical numbers are not universal speedup guarantees.

## Alternatives

Framework-dependent execution preceded native distribution. Hosting integration and Scrutor decoration were removed during AOT preparation. Conventional logging, string-building, and LINQ implementations preceded the measured library changes; they are not universally prohibited.

## Evidence

- [Issue #146](https://github.com/Stepami/hydrascript/issues/146) and [PR #166](https://github.com/Stepami/hydrascript/pull/166): native compilation and binary-size work; [PR #156](https://github.com/Stepami/hydrascript/pull/156) / [PR #159](https://github.com/Stepami/hydrascript/pull/159): composition changes.
- [PR #181](https://github.com/Stepami/hydrascript/pull/181), [PR #182](https://github.com/Stepami/hydrascript/pull/182), and [PR #183](https://github.com/Stepami/hydrascript/pull/183): ZLogger, ZString, and ZLinq adoption with historical comparisons.
- [Issue #81](https://github.com/Stepami/hydrascript/issues/81) and [PR #211](https://github.com/Stepami/hydrascript/pull/211): read-heavy frozen lookup and lazy lexing.
- [PR #244](https://github.com/Stepami/hydrascript/pull/244) / [PR #246](https://github.com/Stepami/hydrascript/pull/246): hybrid native/managed tool distribution.
- Current [build policy](../../Directory.Build.props), [CLI packaging](../../src/HydraScript/HydraScript.csproj), [JSON source generation](../../src/Domain/HydraScript.Domain.BackEnd/Impl/Instructions/WithAssignment/ExplicitCast/AsString.cs), and [operator storage](../../src/Domain/HydraScript.Domain.IR/Types/Type.cs); see also [Visitor.NET](0002-visitor-net-adoption.md) and [lexer generation](0004-generated-lexer-pattern.md).
- Current [traversal enumerator](../../src/Domain/HydraScript.Domain.FrontEnd/Parser/Impl/Ast/TraverseEnumerator.cs), [span-based scanning](../../src/Domain/HydraScript.Domain.FrontEnd/Lexer/Impl/TextCoordinateSystemComputer.cs), [benchmark harness](../../benchmarks/HydraScript.Benchmarks/InvokeBenchmark.cs), and [measurement guidance](../../.agents/testing.md).