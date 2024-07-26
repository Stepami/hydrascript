using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.CheckSemantics.Variables;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class TypeDeclaration(IdentifierReference typeId, TypeValue typeValue) : Declaration
{
    public IdentifierReference TypeId { get; } = typeId;
    public TypeValue TypeValue { get; } = typeValue;

    /// <inheritdoc cref="AbstractSyntaxTreeNode.InitScope"/>
    public override void InitScope(ISymbolTable? scope = null)
    {
        base.InitScope(scope);
        TypeValue.SymbolTable = SymbolTable;
    }

    protected override string NodeRepresentation() =>
        $"type {TypeId.Name} = {TypeValue}";
}