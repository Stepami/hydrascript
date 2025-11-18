using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public partial class DummyContainer : IGeneratedRegexContainer
{
#if NET10_0
    [GeneratedRegex(TokenInput.Pattern, RegexOptions.Compiled)]
    public static partial Regex Regex { get; }
#else
    [GeneratedRegex(TokenInput.Pattern, RegexOptions.Compiled)]
    public static partial Regex GetRegex();

    public static Regex Regex { get; } = GetRegex();
#endif
}