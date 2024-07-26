using HydraScript.Lib.BackEnd;

namespace HydraScript.Lib.IR.Ast;

public interface IAbstractSyntaxTree
{
    public IAbstractSyntaxTreeNode Root { get; }
    AddressedInstructions GetInstructions();
}