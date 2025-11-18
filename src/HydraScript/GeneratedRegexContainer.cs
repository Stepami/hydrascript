using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Infrastructure;

namespace HydraScript;

public sealed partial class GeneratedRegexContainer : IGeneratedRegexContainer
{
    [GeneratedRegex(PatternContainer.Value, RegexOptions.Compiled)]
    public static partial Regex Regex { get; }
}