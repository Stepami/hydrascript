# ADR 0006: Use structural static types with type-owned operator rules

- Status: Accepted
- Decision date: Unknown
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

HydraScript combines JavaScript-like syntax with strong static structural typing. The language needs to reject incompatible operations before execution without introducing classes, inheritance, or nominal identities for every user-defined type.

Structural typing predates the later overload, cast, and operator refactorings linked below; those PR dates are not treated as the original adoption date.

## Decision

Resolve declarations, signatures, and expression types before emitting instructions. Keep type descriptions and operator metadata in Domain.IR and their construction/checking in Application.StaticAnalysis. `IHydraScriptTypesService` centralizes built-in types, defaults, and explicit conversion rules. Type declarations are built before their references are resolved.

### Structural equality, not nominal identity

A type alias names a type description; it does not create a distinct nominal type. Resolved object types compare their complete property-name/type shape, and arrays compare their element types. Declared and inferred object properties are canonicalized before comparison; differing declaration order does not define a different shape. Methods are not included in object type equality.

```text
type Point = { x: number; y: number; }
type Vector = { x: number; y: number; }

let p: Point = { x: 3; y: 4; }
let v: Vector = p
```

The assignment is allowed because the property shapes match, not because the aliases have the same name. Object matching is exact-shape equality, not a general rule that any object with extra properties is assignable.

Compatibility is broader than a single symmetric equality function: nullable/object/null rules can make `Type.Equals` directional. Assignment, equality-operator validation, and explicit-cast checks use `CommutativeTypeEqualityComparer` to accept either comparison direction; other checks still call `Equals` directly. Preserve the intended context instead of globally substituting CLR identity, alias names, or one comparer.

### Go-inspired methods

A method is an ordinary function whose first parameter is an explicit receiver. Registration requires that parameter to resolve to an object type and be written using a named type alias:

```text
function lengthSquared(self: Point): number {
    return self.x * self.x + self.y * self.y
}

>>> p.lengthSquared()
```

The receiver is inserted as the first call argument. An explicitly annotated `p: Point` retains the object-type instance and method metadata associated with that alias. An inferred lookalike object does not automatically acquire that method list. This binding role for aliases does not make structural type equality nominal. Overload lookup uses function names and parameter-type signatures.

### Operators and scripting syntax

`IOperator` describes static operator capabilities, not execution:

- `Values` lists the recognized spellings.
- `TryGetResultType(OperationDescriptor, out Type)` checks the operand types and determines the result type.
- `OperationDescriptor` carries the spelling and operand types; `Type.TryGetOperator` selects the `IOperator` rule used by semantic checking.
- Backend instructions implement the actual operation separately. A new operator must align lexical recognition, parsing, type metadata, emission, and execution.

| Operand family | Examples |
| --- | --- |
| Numbers | Arithmetic, unary `-`, ordering |
| Booleans | `!`, `&&`, `\|\|` |
| Strings | `+` / `++` concatenation, `~` length, `[]` indexing returning a string |
| Arrays | `++` concatenation, `~` length, `[]` element access, `::` removal returning `void` |
| Compatible types | `==` and `!=`, registered through the base type's operator metadata |

Assignments, `as`, and `with` have dedicated syntax and checks rather than being `IOperator` implementations. Compound assignments lower into assignment plus an existing binary operator; their user-facing behavior is documented in the [README](../../Readme.md#compound-assignments).

Shell-like facilities are also statically checked: `$NAME` accesses a string-valued environment variable, `<<< destination` reads a line into a string destination, and `>>> expression` produces console output. These are references/statements with their own visitors and runtime instructions, not `IOperator` descriptors or an interactive shell.

## Consequences

- Preserve structural compatibility across aliases, canonical property construction, nullable handling, and signature lookup. Do not infer nominal typing from the use of named aliases for method registration.
- Structural **type** equality does not imply deep runtime **value** equality. The VM's `==` uses CLR `object.Equals`; separate list/dictionary payloads do not become deeply equal because their static types match.
- Keep static result types consistent with CLR payloads returned through `IValue.Get(): object?`. Length results use `double` to participate consistently in numeric operations.
- Adding a node/operator/type rule requires positive and negative semantic cases plus observable runtime regressions. A metadata-only change is not a complete language feature.
- The language exposes built-in operator metadata, not a syntax for users to define arbitrary operator overloads.

## Alternatives

Nominal alias identity is not the implemented model; Go-inspired receiver syntax does not import Go's entire type system. The available history does not establish a formal comparison of typing paradigms. One-phase declaration resolution and dispersed operation checks were replaced by the current staged resolution and centralized type/operator rules.

## Evidence

- [PR #151](https://github.com/Stepami/hydrascript/pull/151): typed symbol IDs and overload signatures; [PR #217](https://github.com/Stepami/hydrascript/pull/217): explicit casts and type equality.
- [PR #226](https://github.com/Stepami/hydrascript/pull/226): centralized types, symmetric assignment checking, and operators as type metadata.
- [PR #240](https://github.com/Stepami/hydrascript/pull/240) / [issue #231](https://github.com/Stepami/hydrascript/issues/231): two-phase type resolution; [PR #242](https://github.com/Stepami/hydrascript/pull/242): numeric length representation.
- [Issue #201](https://github.com/Stepami/hydrascript/issues/201) / [PR #214](https://github.com/Stepami/hydrascript/pull/214) and [issue #200](https://github.com/Stepami/hydrascript/issues/200) / [PR #218](https://github.com/Stepami/hydrascript/pull/218): environment and input syntax.
- Current [object equality](../../src/Domain/HydraScript.Domain.IR/Types/ObjectType.cs), [type construction](../../src/Application/HydraScript.Application.StaticAnalysis/Visitors/TypeBuilder.cs), [method registration](../../src/Application/HydraScript.Application.StaticAnalysis/Visitors/DeclarationVisitor.cs), and [compatibility comparer](../../src/Domain/HydraScript.Domain.IR/Types/CommutativeTypeEqualityComparer.cs).
- Current [IOperator](../../src/Domain/HydraScript.Domain.IR/Types/IOperator.cs), [semantic checking](../../src/Application/HydraScript.Application.StaticAnalysis/Visitors/SemanticChecker.cs), and [runtime operators](../../src/Domain/HydraScript.Domain.BackEnd/Impl/Instructions/WithAssignment/Simple.cs).