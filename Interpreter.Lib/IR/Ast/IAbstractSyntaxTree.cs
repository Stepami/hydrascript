using Interpreter.Lib.BackEnd;

namespace Interpreter.Lib.IR.Ast;

public interface IAbstractSyntaxTree
{
    AddressedInstructions GetInstructions();
}