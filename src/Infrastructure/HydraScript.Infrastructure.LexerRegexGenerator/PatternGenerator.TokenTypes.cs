using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HydraScript.Infrastructure.LexerRegexGenerator;

public partial class PatternGenerator
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new TokenTypesReadConverter() }
    };

    private record TokenType(
        string Tag,
        string Pattern,
        int Priority)
    {
        public string GetNamedRegex() => $"(?<{Tag}>{Pattern})";
    }

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

                    return new TokenType(tag, pattern, priority);
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