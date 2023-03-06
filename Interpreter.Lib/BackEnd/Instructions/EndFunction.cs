using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.BackEnd.Instructions;

public class EndFunction : Instruction
{
    private readonly FunctionInfo _function;
    
    public EndFunction(FunctionInfo function) =>
        _function = function;

    public override IAddress Execute(VirtualMachine vm) =>
        Address.Next;

    protected override string ToStringInternal() =>
        $"EndFunction {_function}";
}