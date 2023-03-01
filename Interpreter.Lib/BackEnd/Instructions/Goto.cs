using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd.Instructions;

public class Goto : Instruction
{
    protected Label jump;
        
    public Goto(Label jump) =>
        this.jump = jump;

    public override IAddress Execute(VirtualMachine vm) =>
        jump;
    
    public void SetJump(Label newJump) =>
        jump = newJump;

    protected override string ToStringInternal() =>
        $"Goto {jump.Name}";
}