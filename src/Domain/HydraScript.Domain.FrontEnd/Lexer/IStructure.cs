using System.Text.RegularExpressions;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

namespace HydraScript.Domain.FrontEnd.Lexer;

public interface IStructure : IEnumerable<TokenType>
{
    public Regex Regex { get; }

    public TokenType FindByTag(string tag);
}