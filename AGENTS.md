# Agent entry point

HydraScript is a statically typed scripting-language interpreter in C#. Keep this file short; load the follow-ups needed for the task.

## Route the task

- Read [architecture](.agents/architecture.md) before changing language behavior, dependencies, or project boundaries.
- Read [testing](.agents/testing.md) before changing executable code or tests, or choosing validation commands.
- Read the [ADR index](docs/adr/README.md) and relevant records before making architectural decisions. Keep checked-in guidance focused on the current implementation.
- Follow [.editorconfig](.editorconfig), [CONTRIBUTING.md](CONTRIBUTING.md), and the existing local conventions. New guidance and ADRs use English.

## Required tools, skills, and agents

- **Use Rider MCP for repository development:** inspect the open solution and worktree first; use its symbol navigation, usages, diagnostics, refactoring, build, and run capabilities where applicable. Run CLI-only operations through Rider's terminal when available.
- **Resolve installed capabilities:** the table uses skill short names and explicitly labels agents. Resolve equivalent prefixed names from the active catalog, read the applicable `SKILL.md` before acting, and follow its scope and tool routing (`execute_tool` or directly exposed Rider tools, as documented). An installed skill does not imply its tools or agents are available.

| Task | Skill or agent entry point |
| --- | --- |
| Symbols, usages, and refactoring | `navigating-code` for symbol navigation; `refactoring-code` for semantic IDE refactoring; `csharp-refactoring` for behavior-preserving C# restructuring. |
| Runtime investigation requiring debugger evidence | `debugging-code`; ordinary static diagnosis does not require a debugger. |
| Locate tests for C# production code | `finding-tests` before searching for existing coverage or writing related tests. |
| Run .NET tests or choose commands/flags | `run-tests` directly; `platform-detection` is for identification-only questions. Load `filter-syntax` when needed. |
| Write or extend tests | `code-testing-agent` skill: focused work stays direct; broad suites use its `code-testing-generator` agent and prescribed pipeline. |
| Audit test quality, assertions, gaps, or coverage | `test-quality-auditor` agent selects the matching specialist, such as `assertion-quality` or `test-gap-analysis`; use the combined audit only for broad reviews. |
| Make code testable | `testability-obstacle` for one behavior needing a minimal production seam and tests when seam selection is still open; `testability-migration` agent for static-dependency inventories or migrations to abstractions. Use `code-testing-agent` when a suitable seam already exists. |
| Migrate test framework or platform | `test-migration` agent and its selected migration skill. |
| Diagnose or improve MSBuild | `msbuild` agent; `build-perf` for build performance and `msbuild-code-review` for project-file reviews. Follow their selected skills. |
| Investigate .NET runtime performance | `optimizing-dotnet-performance` agent with applicable `analyzing-dotnet-performance` guidance; `microbenchmarking` for BenchmarkDotNet work. |

- Select other installed skills by the actual task, such as `convert-to-cpm` for central package management, the matching `migrate-dotnet*` skill for SDK/runtime upgrades, and `dotnet-aot-compat` for AOT compatibility. Use the `template-engine` agent for .NET template discovery, scaffolding, or authoring.
- Skill and agent use is mandatory when applicable; keep workflows proportional to scope. Prose changes do not require a debugger, refactoring, test generation, or a test audit. Delegate independent work with explicit ownership and preserve other agents' edits.
- If a required tool, skill, or agent is unavailable, report the exact limitation and use its documented fallback when available. Never silently skip the requirement, present a generic helper as a named specialist, or claim a tool ran.

## Work and handoff

1. Establish acceptance criteria, inspect existing changes, and identify affected stages and tests.
2. Use GitHub MCP to search for context relevant to the task, affected symbols, or linked issue/PR. Read matching discussions, reviews, and release/milestone context as needed. Keep only concise evidence links for implemented decisions in ADRs; do not copy GitHub inventories or planned work into Markdown.
3. Implement the scoped change, preserving unrelated work. Use semantic refactoring tools for symbol changes and patch-based edits for other content.
4. Validate using [testing](.agents/testing.md). Update affected follow-ups and add or supersede a consistently structured ADR for a material decision.
5. Report the result, checks actually completed, and remaining limitations. Keep GitHub writes and release actions within the user's authorization.