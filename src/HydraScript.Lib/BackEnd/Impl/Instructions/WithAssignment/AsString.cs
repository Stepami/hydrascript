using System.Text.Json;
using System.Text.Json.Serialization;

namespace HydraScript.Lib.BackEnd.Impl.Instructions.WithAssignment;

public class AsString(IValue value) : Simple(value)
{
    public override IAddress Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        frame[Left!] = JsonSerializer.Serialize(
            Right.right!.Get(frame),
            new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            }
        );

        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"{Left} = {Right.right} as string";
}