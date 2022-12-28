using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd.Instructions;

public class BeginFunction : Instruction
{
    private readonly FunctionInfo _function;
        
    public BeginFunction(FunctionInfo function) =>
        _function = function;

    public override IAddress Execute(VirtualMachine vm) =>
        Address.Next;

    protected override string ToStringInternal() =>
        $"BeginFunction {_function.CallId()}";
}