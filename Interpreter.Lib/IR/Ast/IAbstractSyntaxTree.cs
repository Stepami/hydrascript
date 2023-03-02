using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Addresses;

namespace Interpreter.Lib.IR.Ast;

public interface IAbstractSyntaxTree
{
    AddressedInstructions GetInstructions();
}