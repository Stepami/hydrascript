namespace Interpreter.Lib.FrontEnd.Lex.TokenTypes
{
    internal record ErrorType() : TokenType("ERROR", @"\S+", int.MaxValue)
    {
        public override bool Error() => true;
    }
}