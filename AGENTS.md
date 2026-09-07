# Agent entry point

HydraScript is a statically typed scripting-language interpreter in C#. Keep this file short; load the follow-ups needed for the task.

## Route the task

- Read [architecture](.agents/architecture.md) before changing language behavior, dependencies, or project boundaries.
- Read [testing](.agents/testing.md) before changing executable code or tests, or choosing validation commands.
- Read the [ADR index](docs/adr/README.md) and relevant records before making architectural decisions. Keep checked-in guidance focused on the current implementation.
- Follow [.editorconfig](.editorconfig), [CONTRIBUTING.md](CONTRIBUTING.md), and the existing local conventions. New guidance and ADRs use English.

## Required tools and skills

- **Use Rider MCP for repository development:** inspect the open solution and worktree first; use its symbol navigation, usages, diagnostics, refactoring, build, and run capabilities where applicable. Run CLI-only operations through Rider's terminal when available.
- **Read and use the applicable Rider skills:** `rider-skills:refactoring-code` for semantic refactoring, `rider-skills:debugging-code` for runtime investigation needing debugger evidence, and `rider-skills:finding-tests` before locating tests for C# changes. Equivalent unprefixed Rider skills are valid. Follow their documented `execute_tool` routing.
- **Read and use applicable .NET skills before acting:** `dotnet-test:platform-detection` and `dotnet-test:run-tests` for test commands; `code-testing-agent` and `assertion-quality` from `dotnet-test` when authoring tests; relevant `dotnet-msbuild`, `dotnet-nuget`, `dotnet-upgrade`, or `dotnet-advanced` skills for their respective tasks. Resolve names from the installed skill catalog; read referenced instructions as required.
- Skill use is mandatory when applicable, not a requirement to run unrelated workflows. Prose changes do not require a debugger, refactoring, or test generation.
- If Rider MCP or a required skill is unavailable, report the exact limitation and use an available documented fallback. Never silently skip the requirement or claim a tool ran.

## Work and handoff

1. Establish acceptance criteria, inspect existing changes, and identify affected stages and tests.
2. Use GitHub MCP to search for context relevant to the task, affected symbols, or linked issue/PR. Read matching discussions, reviews, and release/milestone context as needed. Keep only concise evidence links for implemented decisions in ADRs; do not copy GitHub inventories or planned work into Markdown.
3. Implement the scoped change, preserving unrelated work. Use semantic refactoring tools for symbol changes and patch-based edits for other content.
4. Validate using [testing](.agents/testing.md). Update affected follow-ups and add or supersede a consistently structured ADR for a material decision.
5. Report the result, checks actually completed, and remaining limitations. Keep GitHub writes and release actions within the user's authorization.