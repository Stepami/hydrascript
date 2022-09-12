using Interpreter.Lib.Contracts;

namespace Interpreter.Lib.FrontEnd.TopDownParse
{
    public interface IParser
    {
        IAbstractSyntaxTree TopDownParse(string text);
    }
}