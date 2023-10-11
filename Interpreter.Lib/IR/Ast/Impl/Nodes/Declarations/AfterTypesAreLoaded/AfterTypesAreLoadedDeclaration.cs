using Interpreter.Lib.IR.CheckSemantics.Visitors;
using Visitor.NET;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;

public abstract class AfterTypesAreLoadedDeclaration : Declaration
{
    public abstract override Unit Accept(DeclarationVisitor visitor);
}