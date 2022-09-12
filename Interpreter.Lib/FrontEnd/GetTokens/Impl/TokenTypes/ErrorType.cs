namespace Interpreter.Lib.FrontEnd.GetTokens.Impl.TokenTypes
{
    internal record ErrorType() : TokenType("ERROR", @"\S+", int.MaxValue)
    {
        public override bool Error() => true;
    }
}