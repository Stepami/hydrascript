namespace HydraScript.Domain.FrontEnd.Parser;

public interface IParser
{
    public IAbstractSyntaxTree Parse(string text);
}