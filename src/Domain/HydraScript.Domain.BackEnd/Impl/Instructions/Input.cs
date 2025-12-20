using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class Input(Name name) : Instruction
{
    public override IAddress? Execute(IExecuteParams executeParams)
    {
        var input = executeParams.Console.ReadLine();
        name.Set(input);
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"Input {name}";
}