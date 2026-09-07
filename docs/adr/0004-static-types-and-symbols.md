# ADR 0004: Resolve types and overloads before instruction generation

- Status: Accepted
- Decision date: 2025-03-31
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

Strong static structural typing requires resolving declarations, references, function signatures, and legal operations before running code. Declaration ordering and mixed CLR numeric representations have caused regressions.

## Decision

Identify overloads with typed symbol IDs/signatures, centralize language type/operator rules, and analyze before emission. TypeDeclarationsResolver builds all stored declarations before resolving references (PR #240). Runtime string/list length results use double to agree with numeric arithmetic (PR #242).

## Consequences

Keep signature lookup, structural compatibility, type inference, and runtime operations consistent. Test forward and recursive references, overload/default-parameter cases, invalid programs, and numeric comparisons. IValue.Get() returns object?, so runtime operations must handle the stored CLR values consistently.

## Alternatives

Resolving each declaration immediately while building it was replaced after #231 exposed declaration-order dependence.

## Evidence

- [#61](https://github.com/Stepami/hydrascript/issues/61), [#153](https://github.com/Stepami/hydrascript/issues/153), [#231](https://github.com/Stepami/hydrascript/issues/231), [#232](https://github.com/Stepami/hydrascript/issues/232); [PR #151](https://github.com/Stepami/hydrascript/pull/151), [PR #157](https://github.com/Stepami/hydrascript/pull/157), [PR #167](https://github.com/Stepami/hydrascript/pull/167), [PR #217](https://github.com/Stepami/hydrascript/pull/217), [PR #226](https://github.com/Stepami/hydrascript/pull/226), [PR #240](https://github.com/Stepami/hydrascript/pull/240), [PR #242](https://github.com/Stepami/hydrascript/pull/242).
- Delivery across [v2.2.0](https://github.com/Stepami/hydrascript/releases/tag/v2.2.0)/[milestone #6](https://github.com/Stepami/hydrascript/milestone/6), [v2.3.0](https://github.com/Stepami/hydrascript/releases/tag/v2.3.0)/[milestone #7](https://github.com/Stepami/hydrascript/milestone/7), [v2.6.0](https://github.com/Stepami/hydrascript/releases/tag/v2.6.0)/[milestone #12](https://github.com/Stepami/hydrascript/milestone/12), and [v2.6.6](https://github.com/Stepami/hydrascript/releases/tag/v2.6.6)/[milestone #16](https://github.com/Stepami/hydrascript/milestone/16).
- Current [type resolver](../../src/Application/HydraScript.Application.StaticAnalysis/Impl/TypeDeclarationsResolver.cs) and [IValue](../../src/Domain/HydraScript.Domain.BackEnd/IValue.cs).