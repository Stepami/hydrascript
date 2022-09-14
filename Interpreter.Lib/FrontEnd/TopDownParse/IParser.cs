using Interpreter.Lib.IR.Ast;

namespace Interpreter.Lib.FrontEnd.TopDownParse
{
    public interface IParser
    {
        IAbstractSyntaxTree TopDownParse(string text);
    }
}