using HydraScript.Lib.BackEnd.Impl.Addresses;

namespace HydraScript.Lib.BackEnd.Impl.Instructions.WithJump;

public class Goto : Instruction
{
    protected Label Jump = default!;

    public InsideStatementJumpType? JumpType { get; }
        
    public Goto(Label jump) =>
        this.Jump = jump;

    public Goto(InsideStatementJumpType jumpType) =>
        JumpType = jumpType;

    public override IAddress Execute(IExecuteParams executeParams) =>
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