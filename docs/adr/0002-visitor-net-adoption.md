# ADR 0002: Adopt Visitor.NET for AST operations

- Status: Accepted
- Decision date: 2024-07-20
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

The same syntax tree needs symbol initialization, type loading, semantic checking, and instruction generation. Keeping these operations on nodes, or making nodes depend on each concrete visitor and its result type, couples FrontEnd to Application and BackEnd. The project needed extensible AST operations without reversing the dependency direction.

## Decision

Use Visitor.NET's acyclic generic visitor contracts and generated visitable dispatch.

- `IAbstractSyntaxTreeNode` implements `IVisitable<IAbstractSyntaxTreeNode>` and exposes its children through `IReadOnlyList<IAbstractSyntaxTreeNode>`.
- Concrete AST nodes are partial types marked with `[AutoVisitable<IAbstractSyntaxTreeNode>]`. `Visitor.NET.AutoVisitableGen` generates their dispatch code.
- Application passes over AST nodes derive from `VisitorBase<IAbstractSyntaxTreeNode, TResult>` or `VisitorNoReturnBase<IAbstractSyntaxTreeNode>`, implementing `IVisitor<ConcreteNode, TResult>` or its no-return form for the node kinds they handle.
- Recursive processing uses `child.Accept(This)`. Nodes depend on the shared visitable contract, not on `SemanticChecker`, `InstructionProvider`, or their concrete result types.

The same approach serves a separate type-syntax hierarchy: `TypeValue : IVisitable<TypeValue>`, concrete `[AutoVisitable<TypeValue>]` records, and `TypeBuilder : VisitorBase<TypeValue, Type>`.

Generated dispatch selects the node-specific operation; it does not decide traversal. Each pass controls which children it visits and in what order. The static-analysis pre-pass order remains explicit in DI registration, and analysis runs before emission.

## Consequences

- A new analysis or emission pass can be added without adding a visitor-specific `Accept` overload to every AST node.
- A new node kind still requires checking generated visitability, child enumeration, parent/scope propagation, and the relevant visitors. A default visitor implementation can accept a node without performing the required work.
- Traversal ordering and pass ordering are semantic contracts. Do not replace them with indiscriminate traversal just because dispatch is generated.
- Keep the generator a build-time dependency. Edit node declarations and visitor implementations, not generated output.

## Alternatives

The code previously placed operations on the AST, then used hand-written `Accept` overloads tied to concrete visitors and backend result types. The generic/generated integration replaced those dependencies. There is no recorded comparison against Roslyn visitors or a universal pattern-matching dispatcher.

## Evidence

- [PR #4](https://github.com/Stepami/hydrascript/pull/4): initial visitor adoption.
- [PR #69](https://github.com/Stepami/hydrascript/pull/69): acyclic generic contracts and generated visitability replace concrete visitor dependencies; [issue #51](https://github.com/Stepami/hydrascript/issues/51) connects that change to layer isolation.
- Current [node contract](../../src/Domain/HydraScript.Domain.FrontEnd/Parser/IAbstractSyntaxTreeNode.cs), [example node](../../src/Domain/HydraScript.Domain.FrontEnd/Parser/Impl/Ast/Nodes/Expressions/BinaryExpression.cs), and [FrontEnd package references](../../src/Domain/HydraScript.Domain.FrontEnd/HydraScript.Domain.FrontEnd.csproj).
- Current [type syntax](../../src/Domain/HydraScript.Domain.FrontEnd/Parser/Impl/Ast/Nodes/Declarations/TypeValue.cs), [type visitor](../../src/Application/HydraScript.Application.StaticAnalysis/Visitors/TypeBuilder.cs), [analysis registration](../../src/Application/HydraScript.Application.StaticAnalysis/ServiceCollectionExtensions.cs), and [instruction visitor](../../src/Application/HydraScript.Application.CodeGeneration/Visitors/InstructionProvider.cs).