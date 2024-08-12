using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class TypeDeclaration(IdentifierReference typeId, TypeValue typeValue) : Declaration
{
    public IdentifierReference TypeId { get; } = typeId;
    public TypeValue TypeValue { get; } = typeValue;

    /// <inheritdoc cref="AbstractSyntaxTreeNode.InitScope"/>
    public override void InitScope(Scope? scope = null)
    {
        base.InitScope(scope);
        TypeValue.Scope = Scope;
    }

    protected override string NodeRepresentation() =>
        $"type {TypeId.Name} = {TypeValue}";
}