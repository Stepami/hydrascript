using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors.TypeSystemLoader;
using Visitor.NET;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;

public class TypeDeclaration : Declaration
{
    private readonly TypeValue _typeValue;
    public string TypeId { get; }

    public TypeDeclaration(string typeId, TypeValue typeValue)
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
        $"type {TypeId} = {_typeValue}";

    public override AddressedInstructions Accept(InstructionProvider visitor) => new();
    
    public override Unit Accept(TypeSystemLoader visitor) =>
        visitor.Visit(this);

    public override Unit Accept(DeclarationVisitor visitor) =>
        default;
}