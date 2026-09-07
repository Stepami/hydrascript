# ADR 0003: Generate the lexer pattern from shared token definitions

- Status: Accepted
- Decision date: 2024-08-03
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

[#57](https://github.com/Stepami/hydrascript/issues/57) sought a compiled lexer pattern without hand-maintaining a second lexical specification. Initial generator limitations required a partly manual process; later PRs evolved that implementation.

## Decision

Generate PatternContainer from token definitions and consume it from GeneratedRegexContainer. Keep the generator as an Infrastructure analyzer. Constants live in a separate netstandard2.0 project; the generator links those source files instead of depending on that assembly at analyzer load time.

## Consequences

Token priority, named captures, and the terminal ERROR pattern form a shared contract. Test generated source and lexer behavior together. Preserve analyzer-only reference metadata; edit generation inputs, never generated obj output.

## Alternatives

The earlier implementation required copying the generated pattern by hand. The current GeneratedRegexContainer consumes the generated constant directly.

## Evidence

- [#57](https://github.com/Stepami/hydrascript/issues/57) and its [design discussion](https://github.com/Stepami/hydrascript/issues/57#issuecomment-2263958010); [PR #77](https://github.com/Stepami/hydrascript/pull/77), [PR #115](https://github.com/Stepami/hydrascript/pull/115), [PR #140](https://github.com/Stepami/hydrascript/pull/140), [PR #237](https://github.com/Stepami/hydrascript/pull/237); [#236](https://github.com/Stepami/hydrascript/issues/236).
- Initial delivery: [v2.0.0](https://github.com/Stepami/hydrascript/releases/tag/v2.0.0)/[milestone #1](https://github.com/Stepami/hydrascript/milestone/1); constants: [v2.2.0](https://github.com/Stepami/hydrascript/releases/tag/v2.2.0)/[milestone #6](https://github.com/Stepami/hydrascript/milestone/6); reference fix: [v2.6.6](https://github.com/Stepami/hydrascript/releases/tag/v2.6.6)/[milestone #16](https://github.com/Stepami/hydrascript/milestone/16).
- Current [generator project](../../src/Infrastructure/HydraScript.Infrastructure.LexerRegexGenerator/HydraScript.Infrastructure.LexerRegexGenerator.csproj) and [regex container](../../src/Infrastructure/HydraScript.Infrastructure/GeneratedRegexContainer.cs).