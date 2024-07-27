namespace HydraScript.Domain.FrontEnd.Parser;

public interface IParser
{
    IAbstractSyntaxTree Parse(string text);
}