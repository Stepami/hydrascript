# Architecture decision records

These records describe the six lasting design pillars that shaped HydraScript. They explain the implemented choices, their rationale, and the constraints agents should preserve. They are retrospective records, not a release log or a list of individual refactorings.

| ADR | Project-shaping decision |
| --- | --- |
| [0001](0001-oop-ddd-clean-architecture.md) | OOP, DDD, Clean Architecture, and project boundaries |
| [0002](0002-visitor-net-adoption.md) | Visitor.NET for extensible AST operations |
| [0003](0003-addressed-instructions-and-virtual-machine.md) | Addressed instructions and virtual-machine execution |
| [0004](0004-generated-lexer-pattern.md) | Generated lexer pattern from shared token definitions |
| [0005](0005-native-aot-and-performance.md) | Native AOT and measured, allocation-conscious implementation |
| [0006](0006-structural-type-system.md) | Structural static types, Go-inspired methods, and operator rules |

## What belongs here

A record should explain a lasting choice that shapes several parts of the system. Supporting implementation details belong under the relevant pillar; ordinary fixes, package updates, CI changes, and test-tool migrations do not need their own ADR.

Use [architecture](../../.agents/architecture.md) for task routing, [testing](../../.agents/testing.md) for test tools, fixtures, commands, and coverage, and the [README](../../Readme.md) / [dump guide](../dump.md) for language and CLI usage.

## Keep records consistent

1. Use [template.md](template.md), retaining its metadata keys and the five sections: Context, Decision, Consequences, Alternatives, Evidence.
2. Record only implemented choices. Use concise GitHub evidence and source links, not copied inventories or planned work. Search GitHub MCP for task-relevant discussions, reviews, commits, releases, and milestones when more context is needed.
3. Distinguish direct historical rationale from current-source observations and inferred consequences. Do not invent rejected alternatives or claim an optimization is universally faster.
4. The decision date identifies the principal documented adoption; later evolution belongs in the evidence. Use `Unknown` when the original choice cannot be dated, and keep the documentation date separate.
5. Update the relevant pillar when clarifying its current implementation. A genuinely new project-shaping decision uses the next unused ID; an actual replacement links `Supersedes` / `Superseded by` and preserves the superseded rationale.
6. Update the index and affected follow-ups together. Keep one authoritative project-boundary table in ADR 0001 and operational testing guidance in `.agents/testing.md`.