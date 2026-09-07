# ADR 0004: Generate the lexer pattern from shared token definitions

- Status: Accepted
- Decision date: 2024-08-03
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

Token spellings and patterns are known at build time. Building their combined regex at interpreter startup adds runtime work; maintaining an independent handwritten regex risks disagreement with token metadata. The generated implementation must also remain outside the Domain layer that consumes it.

## Decision

Generate the combined lexer pattern from a single set of shared definitions, then use .NET regex source generation for the concrete matcher.

1. `TokenTypes.Stream` in Constants defines each token's tag, regex pattern, priority, and ignore flag.
2. `PatternGenerator` orders those definitions by priority, forms named alternatives, appends the fallback `ERROR` pattern, and emits the constant in `PatternContainer.g.cs`.
3. Infrastructure's `GeneratedRegexContainer` consumes that constant through `[GeneratedRegex]`.
4. Domain's `Structure<TContainer>` accesses the matcher through the static-abstract `IGeneratedRegexContainer` contract. Runtime token metadata is built from the same shared definitions.

The custom generator targets `netstandard2.0` and links Constants source files. Infrastructure references it as an analyzer with `OutputItemType="Analyzer"`, `ReferenceOutputAssembly="false"`, and `PrivateAssets="all"`; it is compiler infrastructure, not a runtime service.

## Consequences

- Add or change lexical definitions in Constants; do not hand-edit generated files or copy the resulting regex into another source file.
- Overlapping tokens depend on priority. Matching groups, runtime token tags, ignore behavior, and the terminal error fallback must remain aligned.
- Check both generated source and runtime tokenization when changing the pipeline. Successful generation alone does not establish correct token boundaries.
- Keep the Domain contract independent of Infrastructure's concrete generated class. Preserve the generator's target framework, linked sources, and analyzer-only reference semantics.
- This supports the project's [AOT and performance approach](0005-native-aot-and-performance.md) without requiring the Domain to know how the matcher was generated.

## Alternatives

Runtime pattern construction and a separately maintained handwritten regex were avoided. The first generator iteration still required copying output manually; the later fully automatic integration removed that intermediate maintenance step. That old copying workflow is not part of the current design.

## Evidence

- [Issue #57](https://github.com/Stepami/hydrascript/issues/57) and its [architecture discussion](https://github.com/Stepami/hydrascript/issues/57#issuecomment-2263958010): build-time pattern knowledge and the Domain/Infrastructure contract.
- [PR #77](https://github.com/Stepami/hydrascript/pull/77) and [PR #115](https://github.com/Stepami/hydrascript/pull/115): initial generation and removal of manual copying.
- [Issue #236](https://github.com/Stepami/hydrascript/issues/236) and [PR #237](https://github.com/Stepami/hydrascript/pull/237): linked Constants sources and generator references.
- Current [token definitions](../../src/Domain/HydraScript.Domain.Constants/TokenTypes.cs), [pattern generator](../../src/Infrastructure/HydraScript.Infrastructure.LexerRegexGenerator/PatternGenerator.cs), [generator project](../../src/Infrastructure/HydraScript.Infrastructure.LexerRegexGenerator/HydraScript.Infrastructure.LexerRegexGenerator.csproj), and [regex container](../../src/Infrastructure/HydraScript.Infrastructure/GeneratedRegexContainer.cs).