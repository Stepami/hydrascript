using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd.Instructions.WithJump;

public class Goto : Instruction
{
    protected Label jump;

    public InsideStatementJumpType? JumpType { get; }
        
    public Goto(Label jump) =>
        this.jump = jump;

    public Goto(InsideStatementJumpType jumpType) =>
        JumpType = jumpType;

    public override IAddress Execute(VirtualMachine vm) =>
        jump;
    
    public void SetJump(Label newJump) =>
        jump = newJump;

    protected override string ToStringInternal() =>
        $"Goto {jump.Name}";
}

public enum InsideStatementJumpType
{
    Break,
    Continue
}