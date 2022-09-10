namespace Interpreter.Lib.Contracts
{
    public interface IParser
    {
        IAbstractSyntaxTree TopDownParse(string text);
    }
}