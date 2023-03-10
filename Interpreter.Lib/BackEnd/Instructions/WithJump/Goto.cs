using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd.Instructions.WithJump;

public class Goto : Instruction
{
    protected Label jump;

    public InsideLoopStatementType? JumpType { get; }
        
    public Goto(Label jump) =>
        this.jump = jump;

    public Goto(InsideLoopStatementType jumpType) =>
        JumpType = jumpType;

    public override IAddress Execute(VirtualMachine vm) =>
        jump;
    
    public void SetJump(Label newJump) =>
        jump = newJump;

    protected override string ToStringInternal() =>
        $"Goto {jump.Name}";
}

public enum InsideLoopStatementType
{
    Break,
    Continue
}