using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;

public abstract class AfterTypesAreLoadedDeclaration : Declaration
{
    public abstract override Unit Accept(DeclarationVisitor visitor);
}