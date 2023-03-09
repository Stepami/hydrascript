using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions;

public abstract class LeftHandSideExpression : Expression
{
    public abstract IdentifierReference Id { get; }
}