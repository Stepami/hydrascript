using HydraScript.Lib.BackEnd.Values;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public abstract partial class PrimaryExpression : Expression
{
    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield break;
    }

    public abstract IValue ToValue();
}