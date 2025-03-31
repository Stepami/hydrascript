using System.Text.Json;
using System.Text.Json.Serialization;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;

public partial class AsString(IValue value) : Simple(value)
{
    private static readonly AsStringSerializationContext AsStringJsonContext = new(new JsonSerializerOptions
    {
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
    });
    
    public override IAddress Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        frame[Left!] = JsonSerializer.Serialize(
            value: Right.right!.Get(frame)!,
            AsStringJsonContext.Object);

        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"{Left} = {Right.right} as string";

    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization)]
    [JsonSerializable(typeof(List<object>))]
    [JsonSerializable(typeof(Dictionary<string, object>))]
    [JsonSerializable(typeof(bool))]
    [JsonSerializable(typeof(double))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int))]
    private partial class AsStringSerializationContext : JsonSerializerContext;
}