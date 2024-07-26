using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.CheckSemantics.Variables;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

public abstract class AbstractLiteral(TypeValue type) : PrimaryExpression
{
    public TypeValue Type { get; } = type;

    /// <inheritdoc cref="AbstractSyntaxTreeNode.InitScope"/>
    public override void InitScope(SymbolTable? scope = null)
    {
        base.InitScope(scope);
        Type.SymbolTable = Parent.SymbolTable;
    }
}