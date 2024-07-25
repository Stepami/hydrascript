using System.Text.Json;
using System.Text.Json.Serialization;
using HydraScript.Lib.BackEnd.Addresses;
using HydraScript.Lib.BackEnd.Values;

namespace HydraScript.Lib.BackEnd.Instructions.WithAssignment;

public class AsString(IValue value) : Simple(value)
{
    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
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