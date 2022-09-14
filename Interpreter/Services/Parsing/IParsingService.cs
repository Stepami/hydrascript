using Interpreter.Lib.IR.Ast;

namespace Interpreter.Services.Parsing
{
    public interface IParsingService
    {
        IAbstractSyntaxTree Parse(string text);
    }
}