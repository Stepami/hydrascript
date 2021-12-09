namespace Interpreter.Lib.RBNF.Analysis.Lexical.TokenTypes
{
    internal record ErrorType : TokenType
    {
        public ErrorType() : base("ERROR", @"\S+", int.MaxValue)
        {
        }

        public override bool Error()
        {
            return true;
        }

        public override string ToString()
        {
            return Tag;
        }
    }
}