namespace Interpreter.Lib.BackEnd.Instructions;

public class BeginFunction : Instruction
{
    private readonly FunctionInfo _function;
        
    public BeginFunction(int number, FunctionInfo function) : base(number)
    {
        _function = function;
    }

    public override int Execute(VirtualMachine vm) => Number + 1;

    protected override string ToStringRepresentation() => $"BeginFunction {_function.CallId()}";
}