using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public partial class DummyContainer : IGeneratedRegexContainer
{
    [GeneratedRegex(TokenInput.Pattern)]
    public static partial Regex Regex { get; }
}