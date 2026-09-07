# ADR 0007: Centralize build policy and derive releases from GitHub history

- Status: Accepted
- Decision date: 2024-08-11
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

Release numbering and notes needed automation. Shared build/package settings and later platform migrations needed a single place for consistent configuration.

## Decision

Use GitVersion mainline configuration to tag master and GitReleaseManager to generate/publish releases from matching milestones on the release branch. Share build settings through Directory.Build.props and dependency versions through Directory.Packages.props with transitive pinning. The current solution is SLNX and targets .NET 10 except the two netstandard2.0 projects.

## Consequences

A version tag is not necessarily a published release. Match release claims to release objects and milestones; use commit/PR evidence for changes only on master. Preserve parent imports, centralized versions, and the distinction between push/PR/master/release workflows. Treat release branch pushes as publishing actions.

## Alternatives

Manual release versioning and the previous solution/SDK formats were replaced incrementally. The sources do not record a comparative evaluation of every versioning tool or build setting; avoid inventing rationales, including for BuildInParallel=false.

## Evidence

- [#48](https://github.com/Stepami/hydrascript/issues/48), [#74](https://github.com/Stepami/hydrascript/issues/74), [#105](https://github.com/Stepami/hydrascript/issues/105), [#172](https://github.com/Stepami/hydrascript/issues/172), [#191](https://github.com/Stepami/hydrascript/issues/191), [#229](https://github.com/Stepami/hydrascript/issues/229); [PR #86](https://github.com/Stepami/hydrascript/pull/86), [PR #87](https://github.com/Stepami/hydrascript/pull/87), [PR #106](https://github.com/Stepami/hydrascript/pull/106), [PR #113](https://github.com/Stepami/hydrascript/pull/113), [PR #173](https://github.com/Stepami/hydrascript/pull/173), [PR #206](https://github.com/Stepami/hydrascript/pull/206), [PR #230](https://github.com/Stepami/hydrascript/pull/230).
- Initial automation: [v2.0.0](https://github.com/Stepami/hydrascript/releases/tag/v2.0.0)/[milestone #1](https://github.com/Stepami/hydrascript/milestone/1); SLNX: [v2.4.0](https://github.com/Stepami/hydrascript/releases/tag/v2.4.0)/[milestone #9](https://github.com/Stepami/hydrascript/milestone/9); .NET 10: [v2.5.0](https://github.com/Stepami/hydrascript/releases/tag/v2.5.0)/[milestone #11](https://github.com/Stepami/hydrascript/milestone/11).
- Current [GitVersion.yml](../../GitVersion.yml), [build props](../../Directory.Build.props), [packages](../../Directory.Packages.props), and [release workflow](../../.github/workflows/release.yml).