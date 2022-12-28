namespace Interpreter.Lib.BackEnd.Instructions;

public class BeginFunction : Instruction
{
    private readonly FunctionInfo _function;
        
    public BeginFunction(FunctionInfo function) =>
        _function = function;

    public override int Execute(VirtualMachine vm) => 0 + 1;

    protected override string ToStringInternal() =>
        $"BeginFunction {_function.CallId()}";
}