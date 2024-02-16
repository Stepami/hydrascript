using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;

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