using Interpreter.Lib.Contracts;

namespace Interpreter.Services.Parsing
{
    public interface IParsingService
    {
        IAbstractSyntaxTree Parse(string text);
    }
}