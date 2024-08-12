using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

namespace HydraScript.Infrastructure;

internal partial class TokenTypesProvider : ITokenTypesProvider
{
    public IEnumerable<TokenType> GetTokenTypes() =>
        JsonSerializer.Deserialize(
            TokenTypesJson.String,
            TokenTypesProviderContext.Default.IEnumerableTokenType)!;

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
                    var priority = element.GetProperty("priority").GetInt32();

                    var ignorable = element.TryGetProperty("canIgnore", out var canIgnore);

                    var tokenType = ignorable && canIgnore.GetBoolean()
                        ? new IgnorableType(tag)
                        : new TokenType(tag);
                    return new PrioritizedTokenType(tokenType, priority);
                })
                .OrderBy(x => x.Priority)
                .Select(x => x.TokenType);
            return tokenTypes;
        }

        public override void Write(
            Utf8JsonWriter writer,
            IEnumerable<TokenType> value,
            JsonSerializerOptions options) => throw new NotSupportedException();

        private record PrioritizedTokenType(TokenType TokenType, int Priority);
    }

    [JsonSourceGenerationOptions(Converters = [typeof(TokenTypesReadConverter)])]
    [JsonSerializable(typeof(IEnumerable<TokenType>))]
    private partial class TokenTypesProviderContext : JsonSerializerContext;
}