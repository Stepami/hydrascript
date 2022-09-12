using Interpreter.Lib.Semantic;

namespace Interpreter.Lib.FrontEnd.TopDownParse
{
    public interface IParser
    {
        AbstractSyntaxTree TopDownParse(string text);
    }
}