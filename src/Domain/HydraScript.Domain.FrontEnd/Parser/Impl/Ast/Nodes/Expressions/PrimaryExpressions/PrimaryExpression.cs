namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public abstract partial class PrimaryExpression : Expression
{
    public abstract ValueDto ToValueDto();
}