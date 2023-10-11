namespace Interpreter.Lib.IR.Ast.Impl.Nodes;

public abstract class StatementListItem :
    AbstractSyntaxTreeNode { }

public abstract class Statement :
    StatementListItem { }

public abstract class Declaration :
    StatementListItem { }