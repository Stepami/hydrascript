namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;

public abstract class AfterTypesAreLoadedDeclaration : Declaration
{
    public abstract override TReturn Accept<TReturn>(
        IVisitor<IAbstractSyntaxTreeNode, TReturn> visitor);
}