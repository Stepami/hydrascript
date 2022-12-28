namespace Interpreter.Lib.BackEnd.Instructions;

public class Goto : Instruction
{
    protected int jump;
        
    public Goto(int jump) =>
        this.jump = jump;

    public override int Execute(VirtualMachine vm) => 0;

    public void SetJump(int newJump) => jump = newJump;

    protected override string ToStringInternal() =>
        $"Goto {0}";
}