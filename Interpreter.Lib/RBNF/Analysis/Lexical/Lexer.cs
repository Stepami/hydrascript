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

        public Structure Structure { get; }

        public Lexer(Structure structure, string source)
        {
            Structure = structure;
            this.source = source;

            var lineMatches =
                new Regex(@"(?<NEWLINE>\n)").Matches(source[^1] == '\n'
                    ? source
                    : new string(source.Append('\n').ToArray()));
            foreach (Match match in lineMatches) lines.Add(match.Groups["NEWLINE"].Index);
        }

        public IEnumerator<Token> GetEnumerator()
        {
            var matches = Structure.Regex.Matches(source);
            foreach (Match match in matches)
            {
                foreach (var type in Structure)
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

                    yield return token;
                }
            }

            yield return new Token(LexerUtils.End);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}