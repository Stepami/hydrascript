using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Infrastructure;

namespace HydraScript.Benchmarks;

internal sealed partial class GeneratedRegexContainer : IGeneratedRegexContainer
{
#if NET10_0
    [GeneratedRegex(PatternContainer.Value, RegexOptions.Compiled)]
    public static partial Regex Regex { get; }
#else
    [GeneratedRegex(PatternContainer.Value, RegexOptions.Compiled)]
    public static partial Regex GetRegex();

    public static Regex Regex { get; } = GetRegex();
#endif
}