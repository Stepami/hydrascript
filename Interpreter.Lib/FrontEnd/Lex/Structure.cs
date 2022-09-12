using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Interpreter.Lib.FrontEnd.Lex.TokenTypes;

namespace Interpreter.Lib.FrontEnd.Lex
{
    public class Structure : IEnumerable<TokenType>
    {
        public Structure(List<TokenType> types)
        {
            types.AddRange(new List<TokenType>
            {
                LexerUtils.End,
                LexerUtils.Error
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

        public TokenType FindByTag(string tag)
        {
            return Types[tag];
        }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendJoin('\n',
                    Types.Select(x => $"{x.Key} {x.Value.Pattern}")
                ).ToString();
        }
        
        public IEnumerator<TokenType> GetEnumerator() => Types.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}