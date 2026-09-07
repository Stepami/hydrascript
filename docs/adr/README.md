# Architecture decision records

These short records reconstruct decisions from [Stepami/hydrascript](https://github.com/Stepami/hydrascript) as of **2026-09-07**. They were written retrospectively, not at the historical decision dates. The date identifies the principal implementing PR's merge date; later evolution is linked in each record.

| ADR | Decision |
| --- | --- |
| [0001](0001-project-boundaries.md) | Separate compiler concerns into projects |
| [0002](0002-addressed-instructions.md) | Keep instruction identity independent of position |
| [0003](0003-generated-lexer-pattern.md) | Generate the lexer pattern from shared token definitions |
| [0004](0004-static-types-and-symbols.md) | Resolve types and overloads before instruction generation |
| [0005](0005-native-aot-distribution.md) | Ship native executables and a hybrid .NET tool |
| [0006](0006-testing-platform-and-regressions.md) | Use xUnit on MTP with unit and interpreter regression suites |
| [0007](0007-build-and-release-management.md) | Centralize build policy and derive releases from GitHub history |
| [0008](0008-compound-assignment-lowering.md) | Desugar compound assignments in the parser |
| [0009](0009-performance-measurement.md) | Measure interpreter optimizations with BenchmarkDotNet |

These records describe implemented decisions. Use GitHub MCP to find additional context relevant to the current task.

## Keeping records consistent

1. Copy [template.md](template.md) for every new ADR. Use the next unused four-digit ID and a descriptive kebab-case filename.
2. Keep the metadata keys and five section headings in the same order in every record. Prefer a short record; link evidence instead of copying discussions.
3. Record only implemented decisions. Use `Accepted` for implemented decisions and `Superseded` for replaced decisions; confirm the implementation in source and relevant merged PRs.
4. Record the actual decision date when known and the documentation date separately. State when a record is retrospective; distinguish direct rationale from inferred consequences. Do not invent rejected alternatives.
5. For a material change to an accepted decision, create a new ADR, link `Supersedes`/`Superseded by` both ways, and mark the old record `Superseded`. Keep the earlier context intact.
6. Update this index and affected architecture/testing guidance with the implementation. Ordinary bug fixes or package bumps need no new ADR unless they change a lasting contract.
7. Search GitHub MCP by task, affected symbols, or issue/PR references. Read relevant discussions, reviews, commits, releases, and milestones as needed; retain only concise supporting links in the ADR.
8. Keep GitHub inventories and planned work out of these files. For implemented changes awaiting release, cite the merged PR/commit and state that they are not yet released. A tag or milestone is not a published release.