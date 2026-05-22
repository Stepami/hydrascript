using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer;

namespace HydraScript.Infrastructure;

public sealed partial class GeneratedRegexContainer : IGeneratedRegexContainer
{
    [GeneratedRegex(
        PatternContainer.Value,
        options: RegexOptions.Compiled | RegexOptions.ExplicitCapture)]
    public static partial Regex Regex { get; }
}