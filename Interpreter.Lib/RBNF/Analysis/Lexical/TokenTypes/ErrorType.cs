namespace Interpreter.Lib.RBNF.Analysis.Lexical.TokenTypes
{
    internal record ErrorType() : TokenType("ERROR", @"\S+", int.MaxValue)
    {
        public override bool Error() => true;
    }
}