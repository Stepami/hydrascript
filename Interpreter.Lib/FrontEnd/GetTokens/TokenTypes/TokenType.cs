namespace Interpreter.Lib.FrontEnd.GetTokens.TokenTypes
{
    public record TokenType(string Tag, string Pattern, int Priority)
    {
        public TokenType() : this(null, null, 0)
        {
        }

        public virtual bool WhiteSpace() => false;

        public virtual bool EndOfProgram() => false;

        public virtual bool Error() => false;

        public string GetNamedRegex() => $"(?<{Tag}>{Pattern})";

        public sealed override string ToString() => Tag;
    }
}