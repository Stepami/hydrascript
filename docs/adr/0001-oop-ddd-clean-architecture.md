# ADR 0001: Organize domain objects with DDD and Clean Architecture

- Status: Accepted
- Decision date: 2024-07-28
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

HydraScript has distinct language concerns: recognizing syntax, understanding symbols and types, and executing instructions. The original AST also exposed instruction generation, making syntax responsible for application operations. This violated single responsibility and created dependencies between compiler stages that should be independently understandable.

The architecture discussions explicitly called for DDD subdomains and onion/Clean Architecture. OOP is the implementation model visible in the source, not a separately recorded historical vote.

## Decision

Use domain objects and interfaces for AST nodes, scopes, symbols, types, instructions, addresses, values, and frames. Put behavior behind the contract that owns it, using composition and polymorphism rather than a single interpreter class.

Apply DDD to the compiler's vocabulary and FrontEnd, IR, and BackEnd subdomains. This does not impose business-application patterns such as ORM repositories or aggregate roots on a compiler.

Enforce Clean Architecture boundaries through separate projects, not just folders. Domain owns representations and contracts; Application owns analysis and emission operations across domains; Infrastructure supplies concrete adapters, pipeline orchestration, and DI registration. The CLI selects options and composes the executable application. AST operations use [Visitor.NET](0002-visitor-net-adoption.md).

### Project boundaries

Paths below are under `src/`. The references column describes direct production project dependencies, not NuGet dependencies.

| Project | Owns | Direct production references |
| --- | --- | --- |
| `Domain/HydraScript.Domain.Constants` | Shared token definitions | None |
| `Domain/HydraScript.Domain.FrontEnd` | Lexer, coordinates, parser, AST, scopes | Constants |
| `Domain/HydraScript.Domain.IR` | Symbols, signatures, structural types, operator metadata | None |
| `Domain/HydraScript.Domain.BackEnd` | Addresses, executable instructions, values, frames, VM contracts | None |
| `Application/HydraScript.Application.StaticAnalysis` | Declaration resolution, type inference, semantic and return checks | FrontEnd, IR |
| `Application/HydraScript.Application.CodeGeneration` | Instruction visitors and value construction | FrontEnd, BackEnd |
| `Infrastructure/HydraScript.Infrastructure` | Pipeline adapters, files, console, environment, dumping, DI | Both Application projects; lexer generator as an analyzer |
| `Infrastructure/HydraScript.Infrastructure.LexerRegexGenerator` | Build-time lexer pattern generation | Linked Constants source files, not a normal project reference |
| `HydraScript` | CLI, executable composition, package metadata | Infrastructure |

## Consequences

- Domain must not depend on Application, Infrastructure, or the CLI. Domain projects can have appropriate NuGet dependencies; they are not dependency-free.
- StaticAnalysis joins FrontEnd with IR; CodeGeneration joins FrontEnd with BackEnd. BackEnd does not reference IR. Preserve this distinction when moving responsibilities.
- New language behavior may span several projects. Change the owning model, application passes, runtime implementation, and relevant tests together without moving application operations back into AST nodes.
- DI lifetimes are a separate concern from layering: most services are singleton within a provider, while the CLI creates a provider per invocation. Layer separation does not make mutable services reusable or thread-safe.

## Alternatives

The previous CLI plus combined library, with AST-owned analysis/emission and concrete visitor dependencies, was replaced. The records justify separating responsibilities and enforcing boundaries; they do not establish a comparative evaluation of every architectural style.

## Evidence

- [Issue #31](https://github.com/Stepami/hydrascript/issues/31): AST-owned instruction generation and single responsibility.
- [Issue #51](https://github.com/Stepami/hydrascript/issues/51) and its [boundary discussion](https://github.com/Stepami/hydrascript/issues/51#issuecomment-2254190545): DDD/Clean Architecture and the intended domain pairings.
- [PR #72](https://github.com/Stepami/hydrascript/pull/72) and [PR #73](https://github.com/Stepami/hydrascript/pull/73): domain separation and project-enforced layers; the latter supplies the decision date.
- Current [solution](../../ExtendedJavaScriptSubset.slnx), [service composition](../../src/Infrastructure/HydraScript.Infrastructure/ServiceCollectionExtensions.cs), and [architecture follow-up](../../.agents/architecture.md).