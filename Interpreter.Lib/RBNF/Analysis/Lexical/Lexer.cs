using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Interpreter.Lib.RBNF.Analysis.Exceptions;
using Interpreter.Lib.RBNF.Utils;

namespace Interpreter.Lib.RBNF.Analysis.Lexical
{
    public class Lexer : IEnumerable<Token>
    {
        private readonly List<int> lines = new();
        private readonly string source;

        public Domain Domain { get; }

        public Lexer(Domain domain, string source)
        {
            Domain = domain;
            this.source = source;

            var lineMatches =
                new Regex(@"(?<NEWLINE>\n)").Matches(source[^1] == '\n'
                    ? source
                    : new string(source.Append('\n').ToArray()));
            foreach (Match match in lineMatches) lines.Add(match.Groups["NEWLINE"].Index);
        }

        public IEnumerator<Token> GetEnumerator()
        {
            var tokens = Domain.Regex.Matches(source).Select(match =>
            {
                foreach (var type in Domain)
                {
                    var group = match.Groups[type.Tag];

                    if (!group.Success) continue;

                    var value = group.Value;
                    var segment = new Segment(
                        new Coordinates(group.Index, lines),
                        new Coordinates(group.Index + group.Length, lines)
                    );
                    var token = new Token(type, segment, value);

                    if (type == LexerUtils.Error) throw new LexerException(token);

                    return token;
                }

                return null;
            }).Where(t => t != null && !t.Type.WhiteSpace()).ToList();
            tokens.Add(new Token(LexerUtils.End));
            return tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}