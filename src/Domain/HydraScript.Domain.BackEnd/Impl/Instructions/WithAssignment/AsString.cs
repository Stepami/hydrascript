using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;

public partial class AsString(IValue value) : Simple(value)
{
    protected override void Assign()
    {
        var value = Right.right!.Get();
        var valueAsString = value is string
            ? value
            : JsonSerializer.Serialize(
                value: Right.right!.Get()!,
                AsStringSerializationContext.Default.Object);
        Left?.Set(valueAsString);
    }

    protected override string ToStringInternal() =>
        $"{Left} = {Right.right} as string";

    [JsonSourceGenerationOptions(
        GenerationMode = JsonSourceGenerationMode.Serialization,
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        ReferenceHandler = JsonKnownReferenceHandler.IgnoreCycles,
        WriteIndented = true)]
    [JsonSerializable(typeof(List<object>))]
    [JsonSerializable(typeof(Dictionary<string, object>))]
    [JsonSerializable(typeof(bool))]
    [JsonSerializable(typeof(double))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int))]
    private sealed partial class AsStringSerializationContext : JsonSerializerContext
    {
        static AsStringSerializationContext()
        {
            Default = new AsStringSerializationContext(
                new JsonSerializerOptions(Default.GeneratedSerializerOptions!)
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
        }
    }
}