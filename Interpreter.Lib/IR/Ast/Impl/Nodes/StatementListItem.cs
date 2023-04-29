using Interpreter.Lib.IR.CheckSemantics.Visitors;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes;

public abstract class StatementListItem :
    AbstractSyntaxTreeNode { }

public abstract class Statement :
    StatementListItem { }

public abstract class Declaration :
    StatementListItem
{
    public abstract override Unit Accept(DeclarationVisitor visitor);
}