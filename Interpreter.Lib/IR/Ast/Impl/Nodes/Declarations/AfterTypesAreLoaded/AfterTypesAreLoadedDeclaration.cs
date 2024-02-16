using Interpreter.Lib.IR.CheckSemantics.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;

public abstract class AfterTypesAreLoadedDeclaration : Declaration
{
    public abstract override Unit Accept(DeclarationVisitor visitor);
}