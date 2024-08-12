using System.Text.RegularExpressions;

namespace HydraScript.Domain.FrontEnd.Lexer;

public interface IGeneratedRegexContainer
{
    public static abstract Regex GetRegex();
}