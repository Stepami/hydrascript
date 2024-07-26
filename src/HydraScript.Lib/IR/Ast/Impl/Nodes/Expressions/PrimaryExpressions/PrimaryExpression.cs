using HydraScript.Lib.BackEnd.Values;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public abstract partial class PrimaryExpression : Expression
{
    public abstract IValue ToValue();
}