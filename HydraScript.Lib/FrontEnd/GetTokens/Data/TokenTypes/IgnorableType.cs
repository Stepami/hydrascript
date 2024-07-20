namespace HydraScript.Lib.FrontEnd.GetTokens.Data.TokenTypes;

public record IgnorableType(string Tag = null, string Pattern = null, int Priority = 0)
    : TokenType(Tag, Pattern, Priority)
{
    public override bool CanIgnore() => true;
}