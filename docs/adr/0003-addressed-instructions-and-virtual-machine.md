# ADR 0003: Execute addressed instructions through a virtual machine

- Status: Accepted
- Decision date: 2022-12-23
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

HydraScript lowers source programs into executable instruction objects. When addresses were array positions, removing an instruction shifted later positions and broke control flow. Instruction identity needed to survive collection edits independently of the instruction's position.

## Decision

Keep code generation separate from execution. Application visitors emit `AddressedInstructions`; the VM follows addresses and delegates behavior to each instruction's `Execute` method.

- `AddressedInstructions` maintains instruction order with a linked list of addresses and dictionaries for address/node/instruction lookup.
- `IAddress.Next` represents fall-through. Instructions return the next `IAddress?`; jumps, calls, and returns can choose a different address, and `null` terminates execution.
- Replacing an instruction preserves its address. `HashAddress` equality includes a GUID as well as its seed; labels instead use their label names.
- `ExecuteParams` carries console access, frame context, the call stack, and the argument queue. Instructions encapsulate runtime operations, and `IValue` encapsulates value access.
- Calls enter a frame and retain the calling address and return destination. Returns read the callee result, leave its frame, write the result into the caller context, and resume at the saved address's successor.

The emitted program is an in-memory VM instruction model. The [TAC dump](../dump.md) is a diagnostic listing, not a stable serialized bytecode format.

## Consequences

- Preserve address identity for retained/replaced instructions and keep fall-through links and branch targets valid.
- Removal rewires a predecessor's successor; it does not automatically retarget every incoming jump. Deletion still requires control-flow analysis of the affected references.
- Expression emission often obtains its result from the last `Simple.Left`, and assignment emission can redirect that destination. New instructions must respect their callers' result conventions.
- Instructions, frames, and execution parameters are mutable. Provider reuse, recursion, nested calls, and repeated execution require explicit lifetime checks.
- `IValue.Get()` returns `object?`. Static language checking precedes execution, but the CLR payloads and runtime operator implementations must still agree; the backend is not a fully typed CLR instruction system.

## Alternatives

Array indices as instruction addresses were replaced after removal invalidated jumps. Seed-only address identity was later strengthened to distinguish otherwise colliding instructions. The records do not establish a formal comparison against native code emission or another bytecode VM design.

## Evidence

- [Issue #18](https://github.com/Stepami/hydrascript/issues/18) and [PR #21](https://github.com/Stepami/hydrascript/pull/21): position-independent, linked addresses.
- [Issue #29](https://github.com/Stepami/hydrascript/issues/29) and [PR #65](https://github.com/Stepami/hydrascript/pull/65): collision-safe instruction identity.
- [PR #215](https://github.com/Stepami/hydrascript/pull/215) and [PR #214](https://github.com/Stepami/hydrascript/pull/214): instruction destinations and encapsulated frame/value access.
- Current [instruction collection](../../src/Domain/HydraScript.Domain.BackEnd/AddressedInstructions.cs), [VM](../../src/Domain/HydraScript.Domain.BackEnd/Impl/VirtualMachine.cs), [call](../../src/Domain/HydraScript.Domain.BackEnd/Impl/Instructions/WithAssignment/CallFunction.cs), and [return](../../src/Domain/HydraScript.Domain.BackEnd/Impl/Instructions/Return.cs).