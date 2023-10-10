using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.FrontEnd.GetTokens.Data.TokenTypes;

namespace Interpreter.Services.Providers.StructureProvider.Impl;

public class StructureProvider : IStructureProvider
{
    public Structure CreateStructure() =>
        JsonSerializer.Deserialize<Structure>(
            TokenTypes.Json,
            new JsonSerializerOptions
            {
                Converters = { new StructureReadConverter() }
            });

    [ExcludeFromCodeCoverage]
    private class StructureReadConverter : JsonConverter<Structure>
    {
        public override Structure Read(ref Utf8JsonReader reader, 
            Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var tokenTypes = jsonDocument.RootElement
                .EnumerateArray().Select(element =>
                {
                    var tag = element.GetProperty("tag").GetString();
                    var pattern = element.GetProperty("pattern").GetString();
                    var priority = element.GetProperty("priority").GetInt32();

                    var ignorable = element.TryGetProperty("canIgnore", out var canIgnore);

                    return ignorable && canIgnore.GetBoolean()
                        ? new IgnorableType(tag, pattern, priority)
                        : new TokenType(tag, pattern, priority);
                }).ToList();
            return new Structure(tokenTypes);
        }

        public override void Write(Utf8JsonWriter writer, 
            Structure value, JsonSerializerOptions options) =>
            throw new NotSupportedException();
    }
}