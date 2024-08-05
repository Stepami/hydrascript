using System.Text.Json.Serialization;

namespace HydraScript.Infrastructure.LexerRegexGenerator;

public partial class PatternGenerator
{
    private record TokenType(
        string Tag,
        string Pattern,
        int Priority)
    {
        public string GetNamedRegex() => $"(?<{Tag}>{Pattern})";
    }

    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    [JsonSerializable(typeof(IEnumerable<TokenType>))]
    private partial class PatternGeneratorContext : JsonSerializerContext;
}