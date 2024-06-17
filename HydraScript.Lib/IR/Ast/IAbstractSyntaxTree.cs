using HydraScript.Lib.BackEnd;

namespace HydraScript.Lib.IR.Ast;

public interface IAbstractSyntaxTree
{
    AddressedInstructions GetInstructions();
}