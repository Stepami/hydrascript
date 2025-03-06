using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer;

namespace HydraScript.Infrastructure;

internal partial class GeneratedRegexContainer : IGeneratedRegexContainer
{
    [GeneratedRegex(PatternContainer.Value)]
    public static partial Regex Regex { get; }
}