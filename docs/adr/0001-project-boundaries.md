# ADR 0001: Separate compiler concerns into projects

- Status: Accepted
- Decision date: 2024-07-28
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

The AST also performed code generation, coupling syntax to backend responsibilities. [#31](https://github.com/Stepami/hydrascript/issues/31) and [#51](https://github.com/Stepami/hydrascript/issues/51) called for isolated FrontEnd, IR, and BackEnd domains.

## Decision

Use separate Domain, Application, and Infrastructure projects, with the CLI as the composition entry point. Keep AST traversal on Visitor.NET; implement semantic analysis and code generation through Application visitors. Infrastructure connects services and external I/O.

## Consequences

Project references make dependency direction visible. A language change may touch several stages and their tests. Current Domain projects must not acquire Infrastructure/CLI references; see the architecture map before adding dependencies.

## Alternatives

The previous CLI plus combined library and AST-owned code-generation design was replaced. The sources do not document a comparison against other architectural styles.

## Evidence

- [#31](https://github.com/Stepami/hydrascript/issues/31), [#51](https://github.com/Stepami/hydrascript/issues/51); [PR #4](https://github.com/Stepami/hydrascript/pull/4), [PR #72](https://github.com/Stepami/hydrascript/pull/72), [PR #73](https://github.com/Stepami/hydrascript/pull/73).
- Shipped in [v2.0.0](https://github.com/Stepami/hydrascript/releases/tag/v2.0.0); [milestone #1](https://github.com/Stepami/hydrascript/milestone/1).
- Current map: [architecture](../../.agents/architecture.md); [composition](../../src/Infrastructure/HydraScript.Infrastructure/ServiceCollectionExtensions.cs).