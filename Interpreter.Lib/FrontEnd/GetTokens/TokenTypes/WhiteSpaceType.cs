namespace Interpreter.Lib.FrontEnd.GetTokens.TokenTypes
{
    public record WhiteSpaceType(string Tag = null, string Pattern = null, int Priority = 0)
        : TokenType(Tag, Pattern, Priority)
    {
        public override bool WhiteSpace() => true;
    }
}