using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

public abstract class AbstractLiteral(TypeValue type) : PrimaryExpression
{
    public TypeValue Type { get; } = type;

    /// <inheritdoc cref="AbstractSyntaxTreeNode.InitScope"/>
    public override void InitScope(Scope? scope = null)
    {
        base.InitScope(scope);
        Type.Scope = Parent.Scope;
    }
}