using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

namespace HydraScript.Infrastructure;

internal class TokenTypesProvider : ITokenTypesProvider
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new TokenTypesReadConverter() }
    };

    public IEnumerable<TokenType> GetTokenTypes() =>
        JsonSerializer.Deserialize<IEnumerable<TokenType>>(
            TokenTypesJson.String,
            JsonSerializerOptions)!;

    [ExcludeFromCodeCoverage]
    private class TokenTypesReadConverter : JsonConverter<IEnumerable<TokenType>>
    {
        public override IEnumerable<TokenType> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var root = JsonElement.ParseValue(ref reader);
            var tokenTypes = root.EnumerateArray()
                .Select(element =>
                {
                    var tag = element.GetProperty("tag").GetString()!;
                    var pattern = element.GetProperty("pattern").GetString()!;
                    var priority = element.GetProperty("priority").GetInt32();

                    var ignorable = element.TryGetProperty("canIgnore", out var canIgnore);

                    return ignorable && canIgnore.GetBoolean()
                        ? new IgnorableType(tag, pattern, priority)
                        : new TokenType(tag, pattern, priority);
                })
                .OrderBy(x => x.Priority);
            return tokenTypes;
        }

        public override void Write(
            Utf8JsonWriter writer,
            IEnumerable<TokenType> value,
            JsonSerializerOptions options) => throw new NotSupportedException();
    }
}