using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

public class TypeDeclaration : Declaration
{
    private readonly TypeValue _typeValue;
    public IdentifierReference TypeId { get; }

    public TypeDeclaration(IdentifierReference typeId, TypeValue typeValue)
    {
        TypeId = typeId;
        _typeValue = typeValue;
    }

    public Type BuildType() =>
        _typeValue.BuildType(SymbolTable);

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield break;
    }

    protected override string NodeRepresentation() =>
        $"type {TypeId.Name} = {_typeValue}";

    public override AddressedInstructions Accept(InstructionProvider visitor) => new();

    public override Unit Accept(TypeSystemLoader visitor) =>
        visitor.Visit(this);
}