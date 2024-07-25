using HydraScript.Lib.BackEnd.Addresses;

namespace HydraScript.Lib.BackEnd.Instructions.WithJump;

public class Goto : Instruction
{
    protected Label Jump = default!;

    public InsideStatementJumpType? JumpType { get; }
        
    public Goto(Label jump) =>
        this.Jump = jump;

    public Goto(InsideStatementJumpType jumpType) =>
        JumpType = jumpType;

    public override IAddress Execute(VirtualMachine vm) =>
        Jump;
    
    public void SetJump(Label newJump) =>
        Jump = newJump;

    protected override string ToStringInternal() =>
        $"Goto {Jump.Name}";
}

public enum InsideStatementJumpType
{
    Break,
    Continue
}