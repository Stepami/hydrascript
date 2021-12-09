namespace Interpreter.Lib.RBNF.Analysis.Lexical.TokenTypes
{
    public record WhiteSpaceType(string Tag = null, string Pattern = null, int Priority = 0)
        : TokenType(Tag, Pattern, Priority)
    {
        public override bool WhiteSpace()
        {
            return true;
        }

        public override string ToString()
        {
            return Tag;
        }
    }
}