# ADR 0006: Use xUnit on MTP with unit and interpreter regression suites

- Status: Accepted
- Decision date: 2025-09-19
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

The project needed isolated component tests and full interpreter checks. xUnit's newer runner removed the need for XUnit.DependencyInjection and enabled Microsoft.Testing.Platform integration.

## Decision

Use xUnit with MTP, NSubstitute, and existing fixture/assertion helpers. Separate component, generator, and integration test projects. Discover successful script samples automatically; add targeted output/error assertions for behavioral regressions. Use MethodName_Scenario_ExpectedBehavior test names.

## Consequences

Runner settings and inherited props determine valid CLI syntax. Current PR CI enforces 80% changed-line coverage from integration tests; a sample's successful exit alone is not a semantic assertion.

## Alternatives

Moq was replaced with NSubstitute. XUnit.DependencyInjection was removed when the newer xUnit runner supplied the required test-context support.

## Evidence

- [#47](https://github.com/Stepami/hydrascript/issues/47), [#52](https://github.com/Stepami/hydrascript/issues/52), [#53](https://github.com/Stepami/hydrascript/issues/53), [#152](https://github.com/Stepami/hydrascript/issues/152), [#175](https://github.com/Stepami/hydrascript/issues/175), [#205](https://github.com/Stepami/hydrascript/issues/205); [PR #73](https://github.com/Stepami/hydrascript/pull/73), [PR #75](https://github.com/Stepami/hydrascript/pull/75), [PR #141](https://github.com/Stepami/hydrascript/pull/141), [PR #161](https://github.com/Stepami/hydrascript/pull/161), [PR #176](https://github.com/Stepami/hydrascript/pull/176), [PR #248](https://github.com/Stepami/hydrascript/pull/248), [PR #249](https://github.com/Stepami/hydrascript/pull/249), [PR #251](https://github.com/Stepami/hydrascript/pull/251).
- MTP/xUnit migration shipped in [v2.4.0](https://github.com/Stepami/hydrascript/releases/tag/v2.4.0)/[milestone #9](https://github.com/Stepami/hydrascript/milestone/9). Later CI/test-name changes exist on master through tag v2.7.4, beyond published v2.7.0.
- Current [testing guide](../../.agents/testing.md), [global.json](../../global.json), and [PR workflow](../../.github/workflows/pr.yml).