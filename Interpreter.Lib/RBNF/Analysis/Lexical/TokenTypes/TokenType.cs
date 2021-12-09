namespace Interpreter.Lib.RBNF.Analysis.Lexical.TokenTypes
{
    public record TokenType(string Tag, string Pattern, int Priority)
    {
        public TokenType() : this(null, null, 0)
        {
        }

        public virtual bool WhiteSpace()
        {
            return false;
        }

        public virtual bool EndOfProgram()
        {
            return false;
        }

        public virtual bool Error()
        {
            return false;
        }

        public virtual bool NonTerminal()
        {
            return false;
        }

        public virtual bool Terminal()
        {
            return false;
        }

        public virtual bool Epsilon()
        {
            return false;
        }

        public string GetNamedRegex()
        {
            return $"(?<{Tag}>{Pattern})";
        }

        public override string ToString()
        {
            return Tag;
        }
    }
}