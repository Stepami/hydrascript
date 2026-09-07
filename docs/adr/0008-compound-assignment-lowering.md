# ADR 0008: Desugar compound assignments in the parser

- Status: Accepted
- Decision date: 2026-05-22
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

[#233](https://github.com/Stepami/hydrascript/issues/233) requested concise assignments such as `i += 1` and array concatenation assignment. The existing pipeline already understood assignment and binary expressions.

## Decision

Lower compound assignment into assignment plus the corresponding operator expression during parsing. PR #247 also separated parser source files, adjusted MemberExpression/assignment grammar, and retained expression cloning for that lowering.

## Consequences

The backend can consume existing expression forms. Changes to lowering must consider cloned member expressions, parent scopes, evaluation order, and repeated side effects; the historical PR does not establish correctness for every effectful left-hand side.

## Alternatives

The previous source spelling was explicit x = x + 1. Parser lowering reuses that existing representation for the compound syntax.

## Evidence

- [#233](https://github.com/Stepami/hydrascript/issues/233) and [PR #247](https://github.com/Stepami/hydrascript/pull/247); shipped in [v2.7.0](https://github.com/Stepami/hydrascript/releases/tag/v2.7.0)/[milestone #15](https://github.com/Stepami/hydrascript/milestone/15).
- Current [expression parser](../../src/Domain/HydraScript.Domain.FrontEnd/Parser/Impl/TopDownParser.Expression.cs), [grammar](../grammar.txt), and [compound assignment sample](../../tests/HydraScript.IntegrationTests/Samples/compound_assign.js).