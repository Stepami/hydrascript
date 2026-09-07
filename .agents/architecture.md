# Architecture

Read this for language, compiler, runtime, dependency, or project-boundary changes. This describes the checked-out implementation; [ADRs](../docs/adr/README.md) explain its history.

## Purpose and entry points

HydraScript is a TypeScript/Go-inspired interpreter with strong static structural typing. The JavaScript-like syntax is its own language: do not assume JavaScript coercion, class semantics, or complete ECMAScript support.

- [Readme](../Readme.md): language behavior and examples.
- [TokenTypes](../src/Domain/HydraScript.Domain.Constants/TokenTypes.cs): lexical definitions.
- [grammar.txt](../docs/grammar.txt): grammar.
- [Program](../src/HydraScript/Program.cs) and [ExecuteCommand](../src/HydraScript/ExecuteCommand.cs): CLI composition and the input path/`--dump` options.
- [Executor](../src/Infrastructure/HydraScript.Infrastructure/Executor.cs): application execution and exit codes.

## Execution pipeline

```text
Program / ExecuteCommand
  -> per-invocation ServiceProvider
  -> Executor.Invoke
     -> ISourceCodeProvider.GetText
     -> TopDownParser.Parse -> RegexLexer -> AST
     -> CodeGenerator.GetInstructions
        -> StaticAnalyzer.Analyze
           -> SymbolTableInitializer
           -> TypeSystemLoader
           -> DeclarationVisitor
           -> SemanticChecker
        -> InstructionProvider / ExpressionInstructionProvider
        -> AddressedInstructions
     -> VirtualMachine.Run -> instruction.Execute -> next address
```

The pre-analysis order is defined by registrations in [AddStaticAnalysis](../src/Application/HydraScript.Application.StaticAnalysis/ServiceCollectionExtensions.cs). Code generation calls static analysis before emitting instructions; preserve that sequencing.

## Ownership and dependency rules

[ADR 0001: OOP, DDD, and Clean Architecture](../docs/adr/0001-oop-ddd-clean-architecture.md#project-boundaries) owns the project-boundary table. Read it before adding references or moving responsibilities. Domain stays independent of Application/Infrastructure/CLI; Application connects the compiler subdomains, and Infrastructure supplies adapters and composition.

## Constraints to preserve

- AST nodes describe syntax and expose [Visitor.NET dispatch](../docs/adr/0002-visitor-net-adoption.md); each visitor controls its traversal. Put analysis/emission logic in the corresponding visitors. Review parent/scope propagation and visitors when adding nodes.
- [Structural type rules](../docs/adr/0006-structural-type-system.md) govern compatibility and operator checking. Type declarations are built before references are resolved; overload lookup uses typed symbol IDs and signatures.
- [Addressed instructions and the VM](../docs/adr/0003-addressed-instructions-and-virtual-machine.md) separate instruction identity from position; control flow follows returned addresses. Review jump targets and address identity when editing instruction collections.
- Runtime `IValue.Get()` still returns `object?`. Do not describe the backend as fully typed. Numeric length results were normalized to `double` in PR #242; type-system changes must agree with runtime values and operators.
- Compound assignments are desugared by the parser. Check evaluation order and side effects when changing member access or assignment lowering.
- `PatternGenerator` produces `PatternContainer.g.cs`; `GeneratedRegexContainer` consumes the constant. Change token definitions/generator inputs, not files under `obj/`. See [ADR 0004](../docs/adr/0004-generated-lexer-pattern.md). The generator links Constants sources and is referenced with `OutputItemType="Analyzer"`, `ReferenceOutputAssembly="false"`, and `PrivateAssets="all"`.
- DI registrations are predominantly singleton within a service provider. The CLI creates/disposes a provider for each invocation; tests and benchmarks can reuse state differently. Inspect storage/frame lifetime before introducing repeated execution or concurrency.
- Dumping wraps lexer, parser, and VM using keyed services. Preserve ordinary execution behavior and verify dump behavior for affected changes.
- `Executor` returns 0 on success, 1 for lexer/parser/semantic errors, and 2 for other .NET runtime errors. Tests should distinguish these paths.

## Build and platform

[Directory.Build.props](../Directory.Build.props) defines `net10.0`, latest C#, nullable references, implicit usings, warnings as errors, AOT compatibility, and currently `BuildInParallel=false`. Constants and the lexer generator override to `netstandard2.0`; tests/generator/constants opt out of AOT compatibility. Preserve child props' parent imports.

[Directory.Packages.props](../Directory.Packages.props) owns package versions, enables transitive pinning, and disables version overrides. Do not put independent package versions into project files.

[global.json](../global.json) selects Microsoft.Testing.Platform but does not pin the SDK. CI requests .NET 10.x; check the installed SDK before running commands.

The CLI publishes Native AOT executables for `win-x64`, `linux-x64`, and `osx-arm64`. Tool packaging also has an `any` managed fallback. Preserve trim/AOT compatibility and explicit DI; assess new reflection-dependent libraries against [ADR 0005](../docs/adr/0005-native-aot-and-performance.md).

## Change routing

| Change | Inspect together |
| --- | --- |
| Syntax or operator | TokenTypes, grammar, parser/AST, semantic operator rules, emission/runtime, language docs, regression tests |
| Type, overload, scope, return | IR types/symbol IDs, static-analysis visitors/storages, runtime representation, error-program tests |
| Instructions or calls | AddressedInstructions, code-generation visitors, frames/values/VM, recursion and nested-call tests |
| Host I/O or dumping | Infrastructure adapters, DI, Executor, TestHostFixture, input/dump tests |
| Build, dependencies, publishing | All inherited props, generator analyzer reference, CI workflows, ADRs 0004/0005 |

[Master CI](../.github/workflows/master.yml) derives version tags with [GitVersion](../GitVersion.yml). The [release-branch workflow](../.github/workflows/release.yml) uses GitReleaseManager and matching milestones to publish releases and packages. Tags are not themselves published releases; treat release-branch pushes as publishing actions requiring authorization.

Use GitHub MCP to search for relevant implementation context when working on one of these areas.