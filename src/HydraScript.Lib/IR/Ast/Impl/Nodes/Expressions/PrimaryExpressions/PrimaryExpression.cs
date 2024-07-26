using HydraScript.Lib.BackEnd.Values;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public abstract partial class PrimaryExpression : Expression
{
    public abstract IValue ToValue();
}