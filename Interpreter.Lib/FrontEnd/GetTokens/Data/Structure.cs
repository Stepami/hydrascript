using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using Interpreter.Lib.FrontEnd.GetTokens.Data.TokenTypes;

namespace Interpreter.Lib.FrontEnd.GetTokens.Data;

public class Structure : IEnumerable<TokenType>
{
    public Structure(List<TokenType> types)
    {
        types.AddRange(new List<TokenType>
        {
            new EndOfProgramType(),
            new ErrorType()
        });
        types = types
            .OrderBy(t => t.Priority)
            .ToList();
            
        Types = types
            .ToDictionary(x => x.Tag, x => x);

        Regex = new Regex(
            string.Join(
                '|',
                types
                    .Where(t => !t.EndOfProgram())
                    .Select(t => t.GetNamedRegex())
                    .ToList()
            )
        );
    }

    private Dictionary<string, TokenType> Types { get; }
        
    public Regex Regex { get; }

    public TokenType FindByTag(string tag) =>
        Types[tag];

    public override string ToString() =>
        new StringBuilder()
            .AppendJoin('\n',
                Types.Select(x => $"{x.Key} {x.Value.Pattern}")
            ).ToString();

    public IEnumerator<TokenType> GetEnumerator() => 
        Types.Values.GetEnumerator();

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}