using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ExplicitCast;

public partial class AsString(IValue value) : AsInstruction<string>(value)
{
    protected override string Convert(object? value) =>
        JsonSerializer.Serialize(value, AsStringSerializationContext.Default.Object);

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