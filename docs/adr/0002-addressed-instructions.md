# ADR 0002: Keep instruction identity independent of position

- Status: Accepted
- Decision date: 2022-12-23
- Recorded: 2026-09-07
- Evidence basis: Retrospective; linked GitHub records and current source. Consequences include implementation-derived guidance.
- Supersedes: None
- Superseded by: None

## Context

[#18](https://github.com/Stepami/hydrascript/issues/18) describes array-position addresses breaking jumps after instruction removal. Subsequent work also required address uniqueness and backend encapsulation.

## Decision

Execute AddressedInstructions using IAddress identity, linked successor addresses, and lookup tables. Instructions return the next address to the VM. Preserve an instruction's address when replacing it. HashAddress equality includes its identity GUID.

## Consequences

Instruction order and address identity are separate concerns; collection edits must preserve successor links and jump destinations. The instruction/frame runtime is mutable.

## Alternatives

Array indices as instruction addresses were replaced because instruction removal could invalidate control flow.

## Evidence

- [#18](https://github.com/Stepami/hydrascript/issues/18), [#29](https://github.com/Stepami/hydrascript/issues/29); [PR #21](https://github.com/Stepami/hydrascript/pull/21), [PR #65](https://github.com/Stepami/hydrascript/pull/65), [PR #215](https://github.com/Stepami/hydrascript/pull/215).
- Address redesign is included in [v2.0.0](https://github.com/Stepami/hydrascript/releases/tag/v2.0.0); [milestone #1](https://github.com/Stepami/hydrascript/milestone/1).
- Current [AddressedInstructions](../../src/Domain/HydraScript.Domain.BackEnd/AddressedInstructions.cs) and [VirtualMachine](../../src/Domain/HydraScript.Domain.BackEnd/Impl/VirtualMachine.cs).