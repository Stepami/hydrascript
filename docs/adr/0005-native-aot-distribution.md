# ADR 0005: Ship native executables and a hybrid .NET tool

- Status: Accepted
- Decision date: 2025-04-03
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

Early AOT exploration was deferred because of tooling and library constraints; a closed early issue did not mean AOT had shipped. Later work removed blockers and aimed to provide executables that require no separately installed .NET runtime.

## Decision

Publish Native AOT binaries for Windows x64, Linux x64, and macOS arm64. Use explicit/keyed DI decoration and compatible serialization/logging approaches. Also distribute HydraScript as a NuGet .NET tool, with native RID packages and an any managed fallback. NuGet publishing uses OIDC trusted publishing.

## Consequences

New runtime dependencies must preserve trimming/AOT compatibility. Validate changes to publish/pack on relevant RIDs. Keep the any package's PublishAot=false override. Release tooling and package configuration are part of the distribution contract.

## Alternatives

Framework-dependent distribution and continued AOT deferral were the earlier state. Scrutor-based decoration and the previous CLI hosting integration were removed during AOT preparation. Do not reuse the 2024 incompatibility discussion as a current prohibition.

## Evidence

- [#45](https://github.com/Stepami/hydrascript/issues/45) ([historical blocker](https://github.com/Stepami/hydrascript/issues/45#issuecomment-2269862066)), [#146](https://github.com/Stepami/hydrascript/issues/146); [PR #80](https://github.com/Stepami/hydrascript/pull/80), [PR #156](https://github.com/Stepami/hydrascript/pull/156), [PR #159](https://github.com/Stepami/hydrascript/pull/159), [PR #166](https://github.com/Stepami/hydrascript/pull/166).
- Native binaries: [v2.3.0](https://github.com/Stepami/hydrascript/releases/tag/v2.3.0)/[milestone #7](https://github.com/Stepami/hydrascript/milestone/7). Tool: [#216](https://github.com/Stepami/hydrascript/issues/216), [PR #220](https://github.com/Stepami/hydrascript/pull/220), [v2.6.0](https://github.com/Stepami/hydrascript/releases/tag/v2.6.0)/[milestone #12](https://github.com/Stepami/hydrascript/milestone/12).
- Trusted/hybrid publishing: [#243](https://github.com/Stepami/hydrascript/issues/243), [#245](https://github.com/Stepami/hydrascript/issues/245), [PR #244](https://github.com/Stepami/hydrascript/pull/244), [PR #246](https://github.com/Stepami/hydrascript/pull/246), [v2.6.7](https://github.com/Stepami/hydrascript/releases/tag/v2.6.7)/[milestone #19](https://github.com/Stepami/hydrascript/milestone/19), [v2.6.8](https://github.com/Stepami/hydrascript/releases/tag/v2.6.8)/[milestone #20](https://github.com/Stepami/hydrascript/milestone/20).
- Current [CLI project](../../src/HydraScript/HydraScript.csproj) and [release workflow](../../.github/workflows/release.yml).