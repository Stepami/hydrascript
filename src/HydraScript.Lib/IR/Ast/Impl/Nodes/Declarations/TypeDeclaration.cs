using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class TypeDeclaration(IdentifierReference typeId, TypeValue typeValue) : Declaration
{
    public IdentifierReference TypeId { get; } = typeId;

    public Type BuildType() =>
        typeValue.BuildType(SymbolTable);

    protected override string NodeRepresentation() =>
        $"type {TypeId.Name} = {typeValue}";
}